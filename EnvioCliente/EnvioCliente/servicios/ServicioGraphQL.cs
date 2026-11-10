using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EnvioCliente.servicios
{
    /// <summary>
    /// Singleton: unico punto de acceso al servicio web GraphQL del servidor.
    /// Mantiene un solo HttpClient y la URL del endpoint en un unico lugar.
    /// Cada operacion (query o mutation) es una peticion POST con {query, variables}.
    /// </summary>
    public sealed class ServicioGraphQL
    {
        private const string UrlGraphQL = "http://localhost:8081/graphql";

        private static readonly ServicioGraphQL instancia = new ServicioGraphQL();
        private readonly HttpClient cliente = new HttpClient();
        private readonly JsonSerializerOptions opciones = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        private ServicioGraphQL() { }

        public static ServicioGraphQL Instance
        {
            get { return instancia; }
        }

        /// <summary>
        /// Ejecuta una query o mutation y devuelve el campo indicado de "data" como T.
        /// Devuelve null cuando el servidor responde null (por ejemplo, objeto no encontrado).
        /// Lanza Exception con un mensaje apto para el usuario si hay un error.
        /// </summary>
        public async Task<T> EjecutarAsync<T>(string consulta, object variables, string campo) where T : class
        {
            string cuerpo = JsonSerializer.Serialize(new
            {
                query = consulta,
                variables = variables ?? new object()
            });

            HttpResponseMessage respuesta;
            try
            {
                respuesta = await cliente.PostAsync(UrlGraphQL,
                    new StringContent(cuerpo, Encoding.UTF8, "application/json"));
            }
            catch (Exception ex)
            {
                throw new Exception("No se pudo conectar con el servidor: " + ex.Message, ex);
            }

            string texto = await respuesta.Content.ReadAsStringAsync();

            JsonDocument documento;
            try
            {
                documento = JsonDocument.Parse(texto);
            }
            catch (JsonException)
            {
                throw new Exception("Error " + (int)respuesta.StatusCode + " del servidor.");
            }

            using (documento)
            {
                JsonElement raiz = documento.RootElement;

                // GraphQL responde 200 aunque haya errores: se reportan en "errors".
                JsonElement errores;
                if (raiz.TryGetProperty("errors", out errores) &&
                    errores.ValueKind == JsonValueKind.Array && errores.GetArrayLength() > 0)
                {
                    JsonElement mensaje;
                    string texto0 = errores[0].TryGetProperty("message", out mensaje)
                        ? mensaje.GetString()
                        : "Error del servidor.";
                    throw new Exception(texto0);
                }

                if (!respuesta.IsSuccessStatusCode)
                {
                    throw new Exception("Error " + (int)respuesta.StatusCode + " del servidor.");
                }

                JsonElement datos;
                JsonElement valor;
                if (!raiz.TryGetProperty("data", out datos) || datos.ValueKind != JsonValueKind.Object ||
                    !datos.TryGetProperty(campo, out valor) || valor.ValueKind == JsonValueKind.Null)
                {
                    return null;
                }

                return JsonSerializer.Deserialize<T>(valor.GetRawText(), opciones);
            }
        }
    }
}
