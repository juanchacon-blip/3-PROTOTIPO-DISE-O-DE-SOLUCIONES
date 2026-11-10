// CONTROLADOR: conecta vista y modelo. Cada caso de uso tiene su propio manejador.
const modelo = EnvioModelo.instancia;          // maestro: envíos
const paquetes = PaqueteModelo.instancia;      // detalle: paquetes
let filtroListar = null;                       // recuerda el último filtro de envíos para el Observer
let filtroEnvioPaquetes = '';                  // ídem para la consulta envíos con paquetes
let filtroListarPaquetes = null;               // ídem para paquetes
const abiertas = new Set();                    // casos de uso (ventanas) abiertos a la vez

const seguro = (fn) => async (e) => {
  e?.preventDefault();
  try { await fn(e); } catch (err) { Vista.aviso(err.message, true); }
};

function validar(env) {
  if (!env.codigo || !env.origen || !env.destino || !env.departamentoDestino || !env.fechaSalida) throw new Error('Complete todos los campos.');
  if (Number.isNaN(env.precio)) throw new Error('El precio debe ser numérico.');
  if (env.precio <= 0) throw new Error('El precio debe ser mayor que cero.');
  // El código postal es opcional: si se escribe, debe ser un entero positivo de hasta 6 dígitos.
  const cp = env.codigoPostalNacional;
  if (cp !== null && (!Number.isInteger(cp) || cp < 1 || cp > 999999)) throw new Error('El código postal (opcional) debe ser un entero positivo de hasta 6 dígitos.');
}

function validarPaquete(p) {
  if (!p.codigo || !p.envioCodigo || !p.descripcion || !p.fechaRegistro) throw new Error('Complete todos los campos.');
  if (Number.isNaN(p.pesoKg) || p.pesoKg <= 0) throw new Error('El peso debe ser un número mayor que cero.');
  if (Number.isNaN(p.unidades) || p.unidades < 1) throw new Error('Las unidades deben ser un número entero mayor o igual a 1.');
}

const manejadores = {
  // ---------------- Maestro: envíos ----------------
  adicionar() {
    $('add-form').onsubmit = seguro(async () => {
      const env = Vista.leer('add'); validar(env);
      await modelo.adicionar(env);
      Vista.aviso('Envío adicionado correctamente.'); $('add-form').reset();
    });
  },
  consultar() {
    $('con-form').onsubmit = seguro(async () => {
      const q = ['codigo', 'origen', 'destino'].map(k => $(`con-q-${k}`).value.trim());
      const n = q.filter(Boolean).length;
      if (n === 0) throw new Error('Ingrese al menos un criterio de búsqueda.');
      if (n > 2) throw new Error('Ingrese máximo dos criterios de búsqueda.');
      const e = await modelo.porCriterios(...q);
      Vista.llenar('con-r', e);
      if (!e) throw new Error('No se encontró ningún envío con esos criterios.');
    });
    $('con-limpiar').onclick = () => { $('con-form').reset(); Vista.llenar('con-r', null); };
  },
  listar() {
    // Observer: la vista de listar se refresca cuando el modelo notifica un cambio.
    $('lis-form').onsubmit = seguro(async () => {
      filtroListar = [$('lis-origen').value.trim(), $('lis-dep').value.trim()];
      if (!filtroListar[0] && !filtroListar[1]) throw new Error('Ingrese un origen o un departamento para filtrar.');
      await refrescarListar();
    });
    $('lis-todos').onclick = seguro(async () => { filtroListar = null; $('lis-form').reset(); await refrescarListar(); });
    seguro(refrescarListar)();
  },
  'envios-con-paquetes'() {
    // Consulta personalizada maestro-detalle (se refresca con el Observer).
    $('ecp-form').onsubmit = seguro(async () => { filtroEnvioPaquetes = $('ecp-envio').value.trim(); await refrescarEnvioPaquetes(); });
    $('ecp-todos').onclick = seguro(async () => { filtroEnvioPaquetes = ''; $('ecp-form').reset(); await refrescarEnvioPaquetes(); });
    seguro(refrescarEnvioPaquetes)();
  },
  actualizar() {
    $('act-buscar').onsubmit = seguro(async () => {
      const e = await modelo.porCodigo($('act-q').value.trim());
      Vista.llenar('act', e); $('act-set').disabled = !e; if (e) $('act-codigo').readOnly = true;
      if (!e) throw new Error('No existe un envío con ese código.');
    });
    $('act-form').onsubmit = seguro(async () => {
      const env = Vista.leer('act'); validar(env);
      await modelo.actualizar(env);
      Vista.aviso('Envío actualizado correctamente.'); Vista.llenar('act', null); $('act-set').disabled = true;
    });
  },
  eliminar() {
    $('eli-buscar').onsubmit = seguro(async () => {
      const e = await modelo.porCodigo($('eli-q').value.trim());
      Vista.llenar('eli', e); $('eli-set').disabled = !e;
      if (!e) throw new Error('No existe un envío con ese código.');
    });
    $('eli-form').onsubmit = seguro(async () => {
      // Maestro-detalle: al eliminar el envío se eliminan también sus paquetes.
      const codigo = $('eli-codigo').value;
      const n = (await paquetes.filtrar(codigo, '')).length;
      if (!confirm(n ? `¿Eliminar este envío? También se eliminarán sus ${n} paquete(s).` : '¿Eliminar este envío?')) return;
      await modelo.eliminar(codigo);
      Vista.aviso('Envío eliminado correctamente.'); Vista.llenar('eli', null); $('eli-set').disabled = true;
    });
  },

  // ---------------- Detalle: paquetes ----------------
  'p-adicionar'() {
    $('padd-form').onsubmit = seguro(async () => {
      const p = Vista.leer('padd', CAMPOS_PAQUETE); validarPaquete(p);
      await paquetes.adicionar(p);
      Vista.aviso('Paquete adicionado correctamente.'); $('padd-form').reset();
    });
  },
  'p-consultar'() {
    $('pcon-form').onsubmit = seguro(async () => {
      const q = ['codigo', 'envioCodigo', 'descripcion'].map(k => $(`pcon-q-${k}`).value.trim());
      const n = q.filter(Boolean).length;
      if (n === 0) throw new Error('Ingrese al menos un criterio de búsqueda.');
      if (n > 2) throw new Error('Ingrese máximo dos criterios de búsqueda.');
      const p = await paquetes.porCriterios(...q);
      Vista.llenar('pcon-r', p, CAMPOS_PAQUETE);
      if (!p) throw new Error('No se encontró ningún paquete con esos criterios.');
    });
    $('pcon-limpiar').onclick = () => { $('pcon-form').reset(); Vista.llenar('pcon-r', null, CAMPOS_PAQUETE); };
  },
  'p-listar'() {
    // Observer: se refresca cuando el modelo notifica un cambio.
    $('plis-form').onsubmit = seguro(async () => {
      filtroListarPaquetes = [$('plis-envio').value.trim(), $('plis-desc').value.trim()];
      if (!filtroListarPaquetes[0] && !filtroListarPaquetes[1]) throw new Error('Ingrese un código de envío o una descripción para filtrar.');
      await refrescarListarPaquetes();
    });
    $('plis-todos').onclick = seguro(async () => { filtroListarPaquetes = null; $('plis-form').reset(); await refrescarListarPaquetes(); });
    seguro(refrescarListarPaquetes)();
  },
  'p-actualizar'() {
    $('pact-buscar').onsubmit = seguro(async () => {
      const p = await paquetes.porCodigo($('pact-q').value.trim());
      Vista.llenar('pact', p, CAMPOS_PAQUETE); $('pact-set').disabled = !p;
      if (!p) throw new Error('No existe un paquete con ese código.');
    });
    $('pact-form').onsubmit = seguro(async () => {
      const p = Vista.leer('pact', CAMPOS_PAQUETE); validarPaquete(p);
      await paquetes.actualizar(p);
      Vista.aviso('Paquete actualizado correctamente.'); Vista.llenar('pact', null, CAMPOS_PAQUETE); $('pact-set').disabled = true;
    });
  },
  'p-eliminar'() {
    $('peli-buscar').onsubmit = seguro(async () => {
      const p = await paquetes.porCodigo($('peli-q').value.trim());
      Vista.llenar('peli', p, CAMPOS_PAQUETE); $('peli-set').disabled = !p;
      if (!p) throw new Error('No existe un paquete con ese código.');
    });
    $('peli-form').onsubmit = seguro(async () => {
      if (!confirm('¿Eliminar este paquete?')) return;
      await paquetes.eliminar($('peli-codigo').value);
      Vista.aviso('Paquete eliminado correctamente.'); Vista.llenar('peli', null, CAMPOS_PAQUETE); $('peli-set').disabled = true;
    });
  }
};

async function refrescarListar() {
  Vista.tabla(filtroListar ? await modelo.filtrar(...filtroListar) : await modelo.listar());
}

async function refrescarEnvioPaquetes() {
  Vista.tabla(await modelo.conPaquetes(filtroEnvioPaquetes), 'ecp-cuerpo', CAMPOS_ENVIO_PAQUETES, 'No hay envíos para mostrar.');
}

async function refrescarListarPaquetes() {
  const datos = filtroListarPaquetes ? await paquetes.filtrar(...filtroListarPaquetes) : await paquetes.listar();
  Vista.tabla(datos, 'plis-cuerpo', CAMPOS_PAQUETE_LISTA, 'No hay paquetes para mostrar.');
}

function ir(nombre) {
  if (!Vista.mostrar(nombre)) return;   // ya estaba abierta: solo se lleva a la vista
  abiertas.add(nombre);
  manejadores[nombre]();
}

function cerrar(nombre) { abiertas.delete(nombre); Vista.cerrar(nombre); }
$('contenido').onclick = (e) => { const n = e.target.dataset?.cerrar; if (n) cerrar(n); };

// Observer: si hay cambios en el modelo y se está viendo un listado, se actualiza.
modelo.suscribir(() => {
  if (abiertas.has('listar')) seguro(refrescarListar)();
  if (abiertas.has('p-listar')) seguro(refrescarListarPaquetes)();
  if (abiertas.has('envios-con-paquetes')) seguro(refrescarEnvioPaquetes)();
});

// Selector de servicio web: GraphQL o REST (el modelo oculta la diferencia).
$('protocolo').onchange = (e) => {
  modelo.usar(e.target.value);
  Vista.aviso(`Usando servicio web ${modelo.protocolo}.`);
  modelo.notificar(); // Observer: refresca el listado si está abierto
};

Vista.construirMenu(ir, () => $('acerca').showModal());
$('cerrar-acerca').onclick = () => $('acerca').close();
ir('adicionar');
