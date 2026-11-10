using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using EnvioCliente.model;

namespace EnvioCliente.servicios
{
    /// <summary>
    /// Singleton: unico punto de acceso a los servicios web REST del servidor.
    /// Mantiene un solo HttpClient y la URL base (/api) en un unico lugar; cada pasarela agrega su recurso.
    /// </summary>
    public sealed class ServicioRest
    {
        private const string UrlBase = "http://localhost:8081/api";

        private static readonly ServicioRest instancia = new ServicioRest();
        private readonly HttpClient cliente = new HttpClient();
        private readonly JsonSerializerOptions opciones = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        private ServicioRest() { }

        public static ServicioRest Instance
        {
            get { return instancia; }
        }

        /// <summary>GET. Devuelve null si el servidor responde 404 (no encontrado).</summary>
        public async Task<T> ObtenerAsync<T>(string ruta) where T : class
        {
            string json = await EnviarAsync(HttpMethod.Get, ruta, null, true);
            return json == null ? null : JsonSerializer.Deserialize<T>(json, opciones);
        }

        public async Task<T> CrearAsync<T>(string ruta, object cuerpo) where T : class
        {
            string json = await EnviarAsync(HttpMethod.Post, ruta, cuerpo, false);
            return JsonSerializer.Deserialize<T>(json, opciones);
        }

        public async Task<T> ActualizarAsync<T>(string ruta, object cuerpo) where T : class
        {
            string json = await EnviarAsync(HttpMethod.Put, ruta, cuerpo, false);
            return JsonSerializer.Deserialize<T>(json, opciones);
        }

        public async Task EliminarAsync(string ruta)
        {
            await EnviarAsync(HttpMethod.Delete, ruta, null, false);
        }

        private async Task<string> EnviarAsync(HttpMethod metodo, string ruta, object cuerpo, bool nullSi404)
        {
            var peticion = new HttpRequestMessage(metodo, UrlBase + ruta);

            if (cuerpo != null)
            {
                peticion.Content = new StringContent(
                    JsonSerializer.Serialize(cuerpo), Encoding.UTF8, "application/json");
            }

            HttpResponseMessage respuesta;
            try
            {
                respuesta = await cliente.SendAsync(peticion);
            }
            catch (Exception ex)
            {
                throw new Exception("No se pudo conectar con el servidor: " + ex.Message, ex);
            }

            string texto = await respuesta.Content.ReadAsStringAsync();

            if (respuesta.StatusCode == HttpStatusCode.NotFound && nullSi404)
            {
                return null;
            }

            if (!respuesta.IsSuccessStatusCode)
            {
                throw new Exception(ExtraerMensaje(texto, (int)respuesta.StatusCode));
            }

            return texto;
        }

        // El servidor responde los errores como {"mensaje": "..."}.
        private string ExtraerMensaje(string texto, int codigo)
        {
            try
            {
                var error = JsonSerializer.Deserialize<MensajeError>(texto, opciones);
                if (error != null && !string.IsNullOrEmpty(error.mensaje))
                {
                    return error.mensaje;
                }
            }
            catch (Exception)
            {
                // Respuesta que no es JSON: se usa el mensaje generico.
            }

            return "Error " + codigo + " del servidor.";
        }
    }
}
