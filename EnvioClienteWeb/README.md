# Cliente web (HTML + CSS + JavaScript)

Consume el servidor por GraphQL (`http://localhost:8081/graphql`) o por REST (`http://localhost:8081/api/envios-nacionales`); se elige en el selector "Servicio web" del encabezado (por defecto GraphQL).

1. Inicie el servidor Spring Boot (puerto 8081).
2. En esta carpeta ejecute: `python -m http.server 3000`  (o `npx serve -l 3000`)
3. Abra http://localhost:3000

Debe abrirse desde el puerto 3000 (no con doble clic en index.html) porque el servidor solo permite CORS desde http://localhost:3000.
