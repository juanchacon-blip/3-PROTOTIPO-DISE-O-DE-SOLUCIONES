// MODELO: acceso al servidor mediante GraphQL o REST (intercambiables) + notificación de cambios (Observer).
const URL_GRAPHQL = 'http://localhost:8081/graphql';
const URL_REST = 'http://localhost:8081/api/envios-nacionales';
const URL_REST_PAQUETES = 'http://localhost:8081/api/paquetes';
const CAMPOS_GQL = 'codigo origen destino precio fechaSalida departamentoDestino codigoPostalNacional';
const CAMPOS_GQL_PAQUETE = 'codigo envioCodigo descripcion pesoKg unidades fechaRegistro fragil envioOrigen envioDestino';
const CAMPOS_GQL_ENVIO_PAQUETES = `${CAMPOS_GQL} paqueteCodigo paqueteDescripcion paquetePesoKg`;

// Observer: el sujeto notifica a quienes se suscribieron.
class Observable {
  #observadores = [];
  suscribir(fn) { this.#observadores.push(fn); }
  notificar() { this.#observadores.forEach(fn => fn()); }
}

// Singleton: una sola instancia que habla con el servidor GraphQL.
class ServicioGraphQL {
  static #instancia = null;
  static get instancia() { return ServicioGraphQL.#instancia ??= new ServicioGraphQL(); }

  // Ejecuta una query o mutation y devuelve el campo indicado de "data".
  // Devuelve null cuando el servidor responde null (por ejemplo, envío no encontrado).
  async ejecutar(consulta, variables, campo) {
    let respuesta;
    try {
      respuesta = await fetch(URL_GRAPHQL, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ query: consulta, variables: variables ?? {} })
      });
    } catch {
      throw new Error('No se pudo conectar con el servidor (http://localhost:8081).');
    }
    const datos = await respuesta.json().catch(() => null);
    // GraphQL responde 200 aunque haya errores: se reportan en "errors".
    if (datos?.errors?.length) throw new Error(datos.errors[0].message ?? 'Error del servidor.');
    if (!respuesta.ok) throw new Error(`Error ${respuesta.status} del servidor.`);
    return datos?.data?.[campo] ?? null;
  }
}

// Singleton: una sola instancia que habla con el servidor REST.
class ServicioRest {
  static #instancia = null;
  static get instancia() { return ServicioRest.#instancia ??= new ServicioRest(); }

  // Devuelve el JSON de la respuesta. En un GET con 404 devuelve null (no encontrado).
  async solicitar(ruta = '', { metodo = 'GET', cuerpo, parametros, base = URL_REST } = {}) {
    const consulta = parametros
      ? '?' + new URLSearchParams(Object.entries(parametros).filter(([, v]) => v)).toString() : '';
    let respuesta;
    try {
      respuesta = await fetch(base + ruta + consulta, {
        method: metodo,
        headers: cuerpo ? { 'Content-Type': 'application/json' } : {},
        body: cuerpo ? JSON.stringify(cuerpo) : undefined
      });
    } catch {
      throw new Error('No se pudo conectar con el servidor (http://localhost:8081).');
    }
    const datos = await respuesta.json().catch(() => null);
    if (respuesta.status === 404 && metodo === 'GET') return null;
    if (!respuesta.ok) throw new Error(datos?.mensaje ?? `Error ${respuesta.status} del servidor.`);
    return datos;
  }
}

// Las dos "pasarelas" ofrecen la misma interfaz: el modelo no sabe qué protocolo usa.
// Las consultas devuelven null si no hay resultado; las modificaciones lanzan Error si fallan.
const PasarelaGraphQL = {
  nombre: 'GraphQL',
  get gql() { return ServicioGraphQL.instancia; },
  async listar() {
    return (await this.gql.ejecutar(`query { enviosNacionales { ${CAMPOS_GQL} } }`, {}, 'enviosNacionales')) ?? [];
  },
  async filtrar(origen, departamentoDestino) {
    return (await this.gql.ejecutar(
      `query($origen: String, $departamentoDestino: String) {
         enviosNacionalesFiltrados(origen: $origen, departamentoDestino: $departamentoDestino) { ${CAMPOS_GQL} } }`,
      { origen: origen ?? '', departamentoDestino: departamentoDestino ?? '' },
      'enviosNacionalesFiltrados')) ?? [];
  },
  // Consulta personalizada maestro-detalle: datos del envío + descripción y peso de cada paquete.
  async conPaquetes(envioCodigo) {
    return (await this.gql.ejecutar(
      `query($envioCodigo: String) { enviosConPaquetes(envioCodigo: $envioCodigo) { ${CAMPOS_GQL_ENVIO_PAQUETES} } }`,
      { envioCodigo: envioCodigo ?? '' }, 'enviosConPaquetes')) ?? [];
  },
  porCodigo(codigo) {
    return this.gql.ejecutar(
      `query($codigo: String!) { envioNacionalPorCodigo(codigo: $codigo) { ${CAMPOS_GQL} } }`,
      { codigo }, 'envioNacionalPorCodigo');
  },
  porCriterios(codigo, origen, destino) {
    return this.gql.ejecutar(
      `query($codigo: String, $origen: String, $destino: String) {
         envioNacionalPorCriterios(codigo: $codigo, origen: $origen, destino: $destino) { ${CAMPOS_GQL} } }`,
      { codigo: codigo ?? '', origen: origen ?? '', destino: destino ?? '' },
      'envioNacionalPorCriterios');
  },
  async adicionar(envio) {
    const creado = await this.gql.ejecutar(
      `mutation($input: EnvioNacionalInput!) { addEnvioNacional(input: $input) { ${CAMPOS_GQL} } }`,
      { input: envio }, 'addEnvioNacional');
    if (!creado) throw new Error('Ya existe un envío con ese código.');
  },
  async actualizar(envio) {
    const actualizado = await this.gql.ejecutar(
      `mutation($input: EnvioNacionalInput!) { updateEnvioNacional(input: $input) { ${CAMPOS_GQL} } }`,
      { input: envio }, 'updateEnvioNacional');
    if (!actualizado) throw new Error('El envío ya no existe.');
  },
  async eliminar(codigo) {
    const eliminado = await this.gql.ejecutar(
      `mutation($codigo: String!) { deleteEnvioNacional(codigo: $codigo) { ${CAMPOS_GQL} } }`,
      { codigo }, 'deleteEnvioNacional');
    if (!eliminado) throw new Error('El envío ya no existe.');
  }
};

const PasarelaRest = {
  nombre: 'REST',
  get rest() { return ServicioRest.instancia; },
  async listar() { return (await this.rest.solicitar()) ?? []; },
  async filtrar(origen, departamentoDestino) {
    return (await this.rest.solicitar('', { parametros: { origen, departamentoDestino } })) ?? [];
  },
  async conPaquetes(envioCodigo) {
    return (await this.rest.solicitar('/con-paquetes', { parametros: { envioCodigo } })) ?? [];
  },
  porCodigo(codigo) { return this.rest.solicitar('/' + encodeURIComponent(codigo)); },
  porCriterios(codigo, origen, destino) {
    return this.rest.solicitar('/buscar', { parametros: { codigo, origen, destino } });
  },
  async adicionar(envio) { await this.rest.solicitar('', { metodo: 'POST', cuerpo: envio }); },
  async actualizar(envio) {
    await this.rest.solicitar('/' + encodeURIComponent(envio.codigo), { metodo: 'PUT', cuerpo: envio });
  },
  async eliminar(codigo) {
    await this.rest.solicitar('/' + encodeURIComponent(codigo), { metodo: 'DELETE' });
  }
};

// Singleton + Observable: el modelo de envíos.
class EnvioModelo extends Observable {
  static #instancia = null;
  static get instancia() { return EnvioModelo.#instancia ??= new EnvioModelo(); }
  #pasarela = PasarelaGraphQL;

  get protocolo() { return this.#pasarela.nombre; }
  usar(nombre) { this.#pasarela = nombre === 'REST' ? PasarelaRest : PasarelaGraphQL; }

  listar() { return this.#pasarela.listar(); }
  filtrar(origen, departamentoDestino) { return this.#pasarela.filtrar(origen, departamentoDestino); }
  conPaquetes(envioCodigo) { return this.#pasarela.conPaquetes(envioCodigo); }
  porCodigo(codigo) { return this.#pasarela.porCodigo(codigo); }
  porCriterios(codigo, origen, destino) { return this.#pasarela.porCriterios(codigo, origen, destino); }
  async adicionar(envio) { await this.#pasarela.adicionar(envio); this.notificar(); }
  async actualizar(envio) { await this.#pasarela.actualizar(envio); this.notificar(); }
  async eliminar(codigo) { await this.#pasarela.eliminar(codigo); this.notificar(); }
}

// ---------- DETALLE: paquetes (cada envio tiene 0..* paquetes) ----------

const PaquetePasarelaGraphQL = {
  get gql() { return ServicioGraphQL.instancia; },
  async listar() {
    return (await this.gql.ejecutar(`query { paquetes { ${CAMPOS_GQL_PAQUETE} } }`, {}, 'paquetes')) ?? [];
  },
  async filtrar(envioCodigo, descripcion) {
    return (await this.gql.ejecutar(
      `query($envioCodigo: String, $descripcion: String) {
         paquetesFiltrados(envioCodigo: $envioCodigo, descripcion: $descripcion) { ${CAMPOS_GQL_PAQUETE} } }`,
      { envioCodigo: envioCodigo ?? '', descripcion: descripcion ?? '' }, 'paquetesFiltrados')) ?? [];
  },
  porCodigo(codigo) {
    return this.gql.ejecutar(
      `query($codigo: String!) { paquetePorCodigo(codigo: $codigo) { ${CAMPOS_GQL_PAQUETE} } }`,
      { codigo }, 'paquetePorCodigo');
  },
  porCriterios(codigo, envioCodigo, descripcion) {
    return this.gql.ejecutar(
      `query($codigo: String, $envioCodigo: String, $descripcion: String) {
         paquetePorCriterios(codigo: $codigo, envioCodigo: $envioCodigo, descripcion: $descripcion) { ${CAMPOS_GQL_PAQUETE} } }`,
      { codigo: codigo ?? '', envioCodigo: envioCodigo ?? '', descripcion: descripcion ?? '' }, 'paquetePorCriterios');
  },
  async adicionar(paquete) {
    const creado = await this.gql.ejecutar(
      `mutation($input: PaqueteInput!) { addPaquete(input: $input) { ${CAMPOS_GQL_PAQUETE} } }`,
      { input: paquete }, 'addPaquete');
    if (!creado) {
      // El servidor responde null si el envío no existe o el código está repetido: se distingue aquí.
      const envio = await PasarelaGraphQL.porCodigo(paquete.envioCodigo);
      throw new Error(envio ? 'Ya existe un paquete con ese código.' : `El envío ${paquete.envioCodigo} no existe.`);
    }
  },
  async actualizar(paquete) {
    const actualizado = await this.gql.ejecutar(
      `mutation($input: PaqueteInput!) { updatePaquete(input: $input) { ${CAMPOS_GQL_PAQUETE} } }`,
      { input: paquete }, 'updatePaquete');
    if (!actualizado) throw new Error('El paquete ya no existe.');
  },
  async eliminar(codigo) {
    const eliminado = await this.gql.ejecutar(
      `mutation($codigo: String!) { deletePaquete(codigo: $codigo) { ${CAMPOS_GQL_PAQUETE} } }`,
      { codigo }, 'deletePaquete');
    if (!eliminado) throw new Error('El paquete ya no existe.');
  }
};

const PaquetePasarelaRest = {
  get rest() { return ServicioRest.instancia; },
  async listar() { return (await this.rest.solicitar('', { base: URL_REST_PAQUETES })) ?? []; },
  async filtrar(envioCodigo, descripcion) {
    return (await this.rest.solicitar('', { base: URL_REST_PAQUETES, parametros: { envioCodigo, descripcion } })) ?? [];
  },
  porCodigo(codigo) { return this.rest.solicitar('/' + encodeURIComponent(codigo), { base: URL_REST_PAQUETES }); },
  porCriterios(codigo, envioCodigo, descripcion) {
    return this.rest.solicitar('/buscar', { base: URL_REST_PAQUETES, parametros: { codigo, envioCodigo, descripcion } });
  },
  async adicionar(paquete) { await this.rest.solicitar('', { base: URL_REST_PAQUETES, metodo: 'POST', cuerpo: paquete }); },
  async actualizar(paquete) {
    await this.rest.solicitar('/' + encodeURIComponent(paquete.codigo),
      { base: URL_REST_PAQUETES, metodo: 'PUT', cuerpo: paquete });
  },
  async eliminar(codigo) {
    await this.rest.solicitar('/' + encodeURIComponent(codigo), { base: URL_REST_PAQUETES, metodo: 'DELETE' });
  }
};

// Singleton: el modelo de paquetes usa el mismo protocolo que el de envíos y le avisa
// (Observer) cuando algo cambia, para que el listado abierto se refresque.
class PaqueteModelo {
  static #instancia = null;
  static get instancia() { return PaqueteModelo.#instancia ??= new PaqueteModelo(); }

  #pasarela() { return EnvioModelo.instancia.protocolo === 'REST' ? PaquetePasarelaRest : PaquetePasarelaGraphQL; }

  listar() { return this.#pasarela().listar(); }
  filtrar(envioCodigo, descripcion) { return this.#pasarela().filtrar(envioCodigo, descripcion); }
  porCodigo(codigo) { return this.#pasarela().porCodigo(codigo); }
  porCriterios(codigo, envioCodigo, descripcion) { return this.#pasarela().porCriterios(codigo, envioCodigo, descripcion); }
  async adicionar(paquete) { await this.#pasarela().adicionar(paquete); EnvioModelo.instancia.notificar(); }
  async actualizar(paquete) { await this.#pasarela().actualizar(paquete); EnvioModelo.instancia.notificar(); }
  async eliminar(codigo) { await this.#pasarela().eliminar(codigo); EnvioModelo.instancia.notificar(); }
}
