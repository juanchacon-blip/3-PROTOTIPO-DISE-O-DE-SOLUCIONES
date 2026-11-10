# Gestión de Envíos Nacionales

## Modelo maestro-detalle

```
EnvioNacional  1..1 ───────▶ 0..*  Paquete
   (maestro)                        (detalle)
```

- **Maestro `EnvioNacional`** (extiende `Envio`): `codigo` (String), `origen`, `destino`, `precio` (double), `fechaSalida` (LocalDateTime), `departamentoDestino`, `codigoPostalNacional` (int, **opcional**). Se guarda en la colección `envios` de MongoDB (sin listas).
- **Detalle `Paquete`**: `codigo` (String), `envioCodigo` (String, referencia al maestro), `descripcion` (String), `pesoKg` (double), `unidades` (int), `fechaRegistro` (LocalDateTime), `fragil` (boolean, **opcional**: por defecto `false`). Se guarda en la colección `paquetes`, que referencia al maestro con `envioCodigo` (Figura 2). Al consultarlo, el servidor completa `envioOrigen` y `envioDestino` (datos del maestro, no se guardan).
- Un paquete solo se puede adicionar a un envío existente. Al eliminar un envío se eliminan también sus paquetes (cascada); los clientes lo avisan antes de confirmar. Actualizar un envío no afecta a sus paquetes, y el envío de un paquete no cambia al actualizarlo.
- Ambos objetos tienen CRUD completo, listado con filtro y consulta por uno o dos criterios, en el servidor (GraphQL y REST) y en los dos clientes (una ventana/sección por caso de uso).

Servidor Java (Spring Boot) con base de datos **NO relacional MongoDB** (`envios_db`, colecciones `envios` y `paquetes`) que expone **GraphQL y REST** sobre la misma lógica, y dos clientes en tecnologías distintas. Cada cliente puede consumir **cualquiera de los dos** servicios (por defecto GraphQL):

| Carpeta | Tecnología | Rol |
|---|---|---|
| `EnvioServidor` | Java 17 + Spring Boot + MongoDB | Servidor (puerto 8081) |
| `EnvioCliente` | C# WinForms (.NET Framework 4.8) | Cliente 1 (escritorio). Menús *Envío nacional* y *Paquete* |
| `EnvioClienteWeb` | HTML + CSS + JavaScript | Cliente 2 (web) |

## Servicio GraphQL (`http://localhost:8081/graphql`, GraphiQL en `/graphiql`)

| Tipo | Operación | Descripción |
|---|---|---|
| Query | `enviosNacionales` | Lista todos |
| Query | `enviosNacionalesFiltrados(origen, departamentoDestino)` | Lista filtrando por uno o dos parámetros |
| Query | `envioNacionalPorCodigo(codigo)` | Consulta por código (usada antes de eliminar/actualizar) |
| Query | `envioNacionalPorCriterios(codigo, origen, destino)` | Consulta un envío por uno o dos criterios |
| Query | `enviosConPaquetes(envioCodigo)` | **Consulta personalizada maestro-detalle:** todos los datos del envío + código, descripción y peso de cada paquete (filtro opcional por envío) |
| Mutation | `addEnvioNacional(input)` | Adiciona (devuelve `null` si el código ya existe) |
| Mutation | `updateEnvioNacional(input)` | Actualiza (devuelve `null` si no existe) |
| Mutation | `deleteEnvioNacional(codigo)` | Elimina el envío y sus paquetes (devuelve `null` si no existe) |
| Query | `paquetes` | Lista todos los paquetes |
| Query | `paquetesFiltrados(envioCodigo, descripcion)` | Lista filtrando por uno o dos parámetros |
| Query | `paquetePorCodigo(codigo)` | Consulta por código (usada antes de eliminar/actualizar) |
| Query | `paquetePorCriterios(codigo, envioCodigo, descripcion)` | Consulta un paquete por uno o dos criterios |
| Mutation | `addPaquete(input)` | Adiciona (devuelve `null` si el código existe o el envío no existe) |
| Mutation | `updatePaquete(input)` | Actualiza (devuelve `null` si no existe) |
| Mutation | `deletePaquete(codigo)` | Elimina (devuelve `null` si no existe) |

`Paquete` incluye `envioOrigen` y `envioDestino`, de modo que *Listar paquetes* muestra todos los atributos del paquete más dos del envío.

Los errores de negocio (validaciones, códigos repetidos, envío inexistente) se devuelven en `errors[0].message` y los clientes los muestran al usuario.

## Servicios REST (`http://localhost:8081/api/envios-nacionales`)

| Método | Ruta | Descripción | Respuestas |
|---|---|---|---|
| GET | `/` | Lista todos. Filtros opcionales `?origen=&departamentoDestino=` | 200 |
| GET | `/con-paquetes?envioCodigo=` | Envío + descripción y peso de sus paquetes | 200 |
| GET | `/buscar?codigo=&origen=&destino=` | Consulta por 1 o 2 criterios | 200, 400, 404 |
| GET | `/{codigo}` | Consulta por código | 200, 404 |
| POST | `/` | Adiciona un envío (JSON) | 201, 400, 409 (código repetido) |
| PUT | `/{codigo}` | Actualiza un envío (JSON) | 200, 400, 404 |
| DELETE | `/{codigo}` | Elimina un envío | 200, 404 |

### Paquetes (`http://localhost:8081/api/paquetes`)

| Método | Ruta | Descripción | Respuestas |
|---|---|---|---|
| GET | `/` | Lista todos. Filtros opcionales `?envioCodigo=&descripcion=` | 200 |
| GET | `/buscar?codigo=&envioCodigo=&descripcion=` | Consulta por 1 o 2 criterios | 200, 400, 404 |
| GET | `/{codigo}` | Consulta por código | 200, 404 |
| POST | `/` | Adiciona un paquete (JSON) | 201, 400, 404 (envío inexistente), 409 (código repetido) |
| PUT | `/{codigo}` | Actualiza un paquete (JSON) | 200, 400, 404 |
| DELETE | `/{codigo}` | Elimina un paquete | 200, 404 |

Cuerpo JSON de POST/PUT de paquetes:
`{"codigo":"P1","envioCodigo":"E1","descripcion":"Cajas de libros","pesoKg":12.5,"unidades":3,"fechaRegistro":"2026-10-21T09:00:00","fragil":false}`

Cuerpo JSON de POST/PUT de envíos:
`{"codigo":"E1","origen":"Ibagué","destino":"Cali","precio":25000,"fechaSalida":"2026-10-21T10:00:00","departamentoDestino":"Valle","codigoPostalNacional":760001}`

Los errores se devuelven como `{"mensaje": "..."}`.

## Patrones de diseño

- **MVC:** modelo, vistas y controlador en servidor y clientes.
- **Cambio de protocolo:** cada cliente tiene una interfaz común (`IEnvioGateway` en C#, pasarelas en JS) con dos implementaciones, GraphQL y REST. En C# se elige en el menú *Servicio web*; en el cliente web, en el selector del encabezado.
- **SOLID:** los controladores del servidor dependen de las interfaces `IServicioEnvioNacional` e `IServicioPaquete` (inyectadas por Spring), no de las clases concretas; `ServicioPaquete` obtiene el maestro a través de `IServicioEnvioNacional`.
- **Singleton:** `ServicioEnvioNacional` y `ServicioPaquete` (beans singleton de Spring, servidor), `ServicioGraphQL`, `ServicioRest`, `EnvioNacionalController`, `PaqueteController` y `EnviosSubject` (C#), `ServicioGraphQL`, `ServicioRest`, `EnvioModelo` y `PaqueteModelo` (web).
- **Observer:** las ventanas Listar (envíos y paquetes) se refrescan cuando se adiciona, actualiza o elimina un envío o un paquete (incluida la cascada al eliminar un envío).

## Cómo ejecutar

0. MongoDB en `localhost:27017` (por ejemplo `docker run -d --name mongo -p 27017:27017 mongo`). Otra URI: variable de entorno `MONGODB_URI`.
1. Servidor: `cd EnvioServidor && mvn spring-boot:run`
2. Cliente C#: abrir `EnvioCliente/EnvioCliente.slnx` en Visual Studio y ejecutar.
3. Cliente web: `cd EnvioClienteWeb && python -m http.server 3000` y abrir http://localhost:3000

## Validaciones (en el servidor)

- Envío: `codigo`, `origen`, `destino`, `departamentoDestino` obligatorios; `precio` > 0; `fechaSalida` con formato `yyyy-MM-ddTHH:mm:ss`; `codigoPostalNacional` opcional, entero entre 1 y 999999.
- Paquete: `codigo`, `envioCodigo`, `descripcion` obligatorios; `pesoKg` > 0; `unidades` ≥ 1; `fechaRegistro` con formato de fecha; `fragil` opcional; el envío debe existir.
