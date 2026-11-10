// VISTA: solo construye y lee el DOM. Cada sección es un caso de uso.
// Tipos de campo: text, number (decimal), int (entero), datetime-local, checkbox.
const CAMPOS = [
  ['codigo', 'Código', 'text'], ['origen', 'Origen', 'text'], ['destino', 'Destino', 'text'],
  ['precio', 'Precio', 'number'], ['fechaSalida', 'Fecha de salida', 'datetime-local'],
  ['departamentoDestino', 'Departamento destino', 'text'], ['codigoPostalNacional', 'Código postal (opcional)', 'int', true]
];
const CAMPOS_PAQUETE = [
  ['codigo', 'Código', 'text'], ['envioCodigo', 'Código del envío', 'text'], ['descripcion', 'Descripción', 'text'],
  ['pesoKg', 'Peso (kg)', 'number'], ['unidades', 'Unidades', 'int'],
  ['fechaRegistro', 'Fecha de registro', 'datetime-local'], ['fragil', 'Frágil', 'checkbox']
];
// Listado de paquetes: todos los atributos del paquete (detalle) + origen y destino de su envío (maestro).
const CAMPOS_PAQUETE_LISTA = [...CAMPOS_PAQUETE, ['envioOrigen', 'Origen del envío', 'text'], ['envioDestino', 'Destino del envío', 'text']];
// Consulta maestro-detalle: todos los datos del envío + código, descripción y peso de cada paquete.
const CAMPOS_ENVIO_PAQUETES = [...CAMPOS, ['paqueteCodigo', 'Paquete', 'text'], ['paqueteDescripcion', 'Descripción del paquete', 'text'], ['paquetePesoKg', 'Peso (kg)', 'number']];
const $ = (id) => document.getElementById(id);

const campo = (p, [k, l, t], ro) => t === 'checkbox'
  ? `<label class="chk"><input id="${p}-${k}" type="checkbox" ${ro ? 'disabled' : ''}> ${l}</label>`
  : `<label>${l}<input id="${p}-${k}" type="${t === 'int' ? 'number' : t}" ${ro ? 'readonly' : ''}
      ${t === 'number' ? 'step="any"' : t === 'int' ? 'step="1"' : ''}></label>`;
// roClaves: campos que siempre quedan de solo lectura (por ejemplo, el envío al que pertenece un paquete).
const campos = (p, lista, ro = false, roClaves = []) => lista.map(c => campo(p, c, ro || roClaves.includes(c[0]))).join('');
const buscar = (p, etiqueta, btn) => `<form id="${p}-buscar"><label>${etiqueta}<input id="${p}-q" required></label><button>${btn}</button></form>`;

const SECCIONES = {
  // ----- Maestro: envío nacional -----
  adicionar: ['Adicionar envío', `<form id="add-form">${campos('add', CAMPOS)}<button>Adicionar</button></form>`, 'Adicionar'],
  consultar: ['Consultar envío', `
    <form id="con-form"><label>Código<input id="con-q-codigo"></label><label>Origen<input id="con-q-origen"></label>
      <label>Destino<input id="con-q-destino"></label><button>Consultar</button><button type="button" id="con-limpiar" class="sec">Limpiar</button></form>
    <p class="ayuda">Diligencie uno o dos criterios.</p><div class="resultado">${campos('con-r', CAMPOS, true)}</div>`, 'Consultar'],
  listar: ['Listar envíos', `
    <form id="lis-form"><label>Origen<input id="lis-origen"></label><label>Departamento destino<input id="lis-dep"></label>
      <button>Filtrar</button><button type="button" id="lis-todos" class="sec">Ver todos</button></form>
    <div class="tabla"><table><thead><tr>${CAMPOS.map(c => `<th>${c[1]}</th>`).join('')}</tr></thead><tbody id="lis-cuerpo"></tbody></table></div>`, 'Listar'],
  actualizar: ['Actualizar envío', `${buscar('act', 'Código del envío', 'Buscar')}
    <form id="act-form"><fieldset id="act-set" disabled style="display:contents">${campos('act', CAMPOS)}<button>Actualizar</button></fieldset></form>`, 'Actualizar'],
  'envios-con-paquetes': ['Envíos con sus paquetes', `
    <form id="ecp-form"><label>Código del envío<input id="ecp-envio"></label>
      <button>Consultar</button><button type="button" id="ecp-todos" class="sec">Ver todos</button></form>
    <div class="tabla"><table><thead><tr>${CAMPOS_ENVIO_PAQUETES.map(c => `<th>${c[1]}</th>`).join('')}</tr></thead><tbody id="ecp-cuerpo"></tbody></table></div>`, 'Con paquetes'],
  eliminar: ['Eliminar envío', `${buscar('eli', 'Código del envío', 'Buscar')}
    <form id="eli-form"><fieldset id="eli-set" disabled style="display:contents">${campos('eli', CAMPOS, true)}<button class="peligro">Eliminar</button></fieldset></form>`, 'Eliminar'],

  // ----- Detalle: paquete (pertenece a un envío) -----
  'p-adicionar': ['Adicionar paquete', `<form id="padd-form">${campos('padd', CAMPOS_PAQUETE)}<button>Adicionar</button></form>`, 'Adicionar'],
  'p-consultar': ['Consultar paquete', `
    <form id="pcon-form"><label>Código<input id="pcon-q-codigo"></label><label>Código del envío<input id="pcon-q-envioCodigo"></label>
      <label>Descripción<input id="pcon-q-descripcion"></label><button>Consultar</button><button type="button" id="pcon-limpiar" class="sec">Limpiar</button></form>
    <p class="ayuda">Diligencie uno o dos criterios.</p><div class="resultado">${campos('pcon-r', CAMPOS_PAQUETE, true)}</div>`, 'Consultar'],
  'p-listar': ['Listar paquetes', `
    <form id="plis-form"><label>Código del envío<input id="plis-envio"></label><label>Descripción<input id="plis-desc"></label>
      <button>Filtrar</button><button type="button" id="plis-todos" class="sec">Ver todos</button></form>
    <div class="tabla"><table><thead><tr>${CAMPOS_PAQUETE_LISTA.map(c => `<th>${c[1]}</th>`).join('')}</tr></thead><tbody id="plis-cuerpo"></tbody></table></div>`, 'Listar'],
  'p-actualizar': ['Actualizar paquete', `${buscar('pact', 'Código del paquete', 'Buscar')}
    <form id="pact-form"><fieldset id="pact-set" disabled style="display:contents">${campos('pact', CAMPOS_PAQUETE, false, ['codigo', 'envioCodigo'])}<button>Actualizar</button></fieldset></form>`, 'Actualizar'],
  'p-eliminar': ['Eliminar paquete', `${buscar('peli', 'Código del paquete', 'Buscar')}
    <form id="peli-form"><fieldset id="peli-set" disabled style="display:contents">${campos('peli', CAMPOS_PAQUETE, true)}<button class="peligro">Eliminar</button></fieldset></form>`, 'Eliminar']
};

const GRUPOS = [
  { titulo: 'Envío', claves: ['adicionar', 'consultar', 'listar', 'envios-con-paquetes', 'actualizar', 'eliminar'] },
  { titulo: 'Paquete', claves: ['p-adicionar', 'p-consultar', 'p-listar', 'p-actualizar', 'p-eliminar'] }
];

const celda = (e, [k, , t]) => {
  const v = e[k];
  if (v == null) return '';
  if (t === 'checkbox') return v ? 'Sí' : 'No';
  return t === 'datetime-local' ? String(v).replace('T', ' ').slice(0, 16) : String(v);
};

const Vista = {
  construirMenu(alElegir, alAcerca) {
    $('menu').innerHTML = GRUPOS.map(g => `<span class="grupo"><b>${g.titulo}:</b>`
      + g.claves.map(k => `<button data-s="${k}">${SECCIONES[k][2]}</button>`).join('') + '</span>').join('')
      + '<button id="btn-acerca">Ayuda: Acerca de…</button>';
    $('menu').onclick = (e) => {
      if (e.target.id === 'btn-acerca') alAcerca();
      else if (e.target.dataset.s) alElegir(e.target.dataset.s);
    };
  },
  // Cada caso de uso es una ventana (sección) propia; se pueden tener varias abiertas a la vez.
  // Devuelve false si la sección ya estaba abierta (solo la lleva a la vista).
  mostrar(nombre) {
    const existente = $(`sec-${nombre}`);
    if (existente) { existente.scrollIntoView({ behavior: 'smooth', block: 'start' }); return false; }
    const sec = document.createElement('section');
    sec.id = `sec-${nombre}`;
    sec.innerHTML = `<div class="cab"><h2>${SECCIONES[nombre][0]}</h2>`
      + `<button type="button" class="cerrar" data-cerrar="${nombre}" aria-label="Cerrar ventana" title="Cerrar">✕</button></div>${SECCIONES[nombre][1]}`;
    $('contenido').prepend(sec);
    Vista.marcarAbiertas();
    return true;
  },
  cerrar(nombre) {
    const sec = $(`sec-${nombre}`);
    if (sec) sec.remove();
    Vista.marcarAbiertas();
  },
  marcarAbiertas() {
    document.querySelectorAll('nav button[data-s]').forEach(b => b.setAttribute('aria-current', !!$(`sec-${b.dataset.s}`)));
  },
  leer(p, lista = CAMPOS) {
    const o = {};
    lista.forEach(([k, , t, opcional]) => {
      const el = $(`${p}-${k}`);
      if (t === 'checkbox') o[k] = el.checked;
      else if (t === 'number') o[k] = parseFloat(el.value.trim());
      else if (t === 'int') o[k] = opcional && el.value.trim() === '' ? null : parseInt(el.value.trim(), 10);
      else if (t === 'datetime-local') { const f = el.value.trim(); o[k] = f.length === 16 ? f + ':00' : f; }
      else o[k] = el.value.trim();
    });
    return o;
  },
  llenar(p, e, lista = CAMPOS) {
    lista.forEach(([k, , t]) => {
      const el = $(`${p}-${k}`);
      if (t === 'checkbox') el.checked = !!(e && e[k]);
      else el.value = e && e[k] != null ? (t === 'datetime-local' ? String(e[k]).slice(0, 16) : e[k]) : '';
    });
  },
  tabla(envios, idCuerpo = 'lis-cuerpo', lista = CAMPOS, vacio = 'No hay envíos para mostrar.') {
    $(idCuerpo).innerHTML = envios.length
      ? envios.map(e => `<tr>${lista.map(c => `<td>${celda(e, c)}</td>`).join('')}</tr>`).join('')
      : `<tr><td colspan="${lista.length}">${vacio}</td></tr>`;
  },
  aviso(texto, error = false) {
    const a = $('aviso'); a.textContent = texto; a.className = error ? 'error' : ''; a.hidden = false;
    clearTimeout(Vista.t); Vista.t = setTimeout(() => a.hidden = true, 3500);
  }
};
