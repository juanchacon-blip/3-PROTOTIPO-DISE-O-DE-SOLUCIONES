using System.Text.Json.Serialization;

namespace EnvioCliente.model
{
    // Envio tal como lo devuelve el servidor (GraphQL o REST).
    public class EnvioNacionalData
    {
        [JsonPropertyName("codigo")]
        public string codigo { get; set; }

        [JsonPropertyName("origen")]
        public string origen { get; set; }

        [JsonPropertyName("destino")]
        public string destino { get; set; }

        [JsonPropertyName("precio")]
        public double precio { get; set; }

        [JsonPropertyName("fechaSalida")]
        public string fechaSalida { get; set; }

        [JsonPropertyName("departamentoDestino")]
        public string departamentoDestino { get; set; }

        [JsonPropertyName("codigoPostalNacional")]
        public int? codigoPostalNacional { get; set; }
    }

    // Datos que se envian al servidor: variable "input" (GraphQL) o cuerpo de POST/PUT (REST).
    public class EnvioNacionalInput
    {
        [JsonPropertyName("codigo")]
        public string codigo { get; set; }

        [JsonPropertyName("origen")]
        public string origen { get; set; }

        [JsonPropertyName("destino")]
        public string destino { get; set; }

        [JsonPropertyName("precio")]
        public double precio { get; set; }

        [JsonPropertyName("fechaSalida")]
        public string fechaSalida { get; set; }

        [JsonPropertyName("departamentoDestino")]
        public string departamentoDestino { get; set; }

        [JsonPropertyName("codigoPostalNacional")]
        public int? codigoPostalNacional { get; set; }
    }

    // Fila de la consulta maestro-detalle: datos del envio + descripcion y peso de cada paquete.
    public class EnvioConPaqueteData
    {
        [JsonPropertyName("codigo")]
        public string codigo { get; set; }

        [JsonPropertyName("origen")]
        public string origen { get; set; }

        [JsonPropertyName("destino")]
        public string destino { get; set; }

        [JsonPropertyName("precio")]
        public double precio { get; set; }

        [JsonPropertyName("fechaSalida")]
        public string fechaSalida { get; set; }

        [JsonPropertyName("departamentoDestino")]
        public string departamentoDestino { get; set; }

        [JsonPropertyName("codigoPostalNacional")]
        public int? codigoPostalNacional { get; set; }

        [JsonPropertyName("paqueteCodigo")]
        public string paqueteCodigo { get; set; }

        [JsonPropertyName("paqueteDescripcion")]
        public string paqueteDescripcion { get; set; }

        [JsonPropertyName("paquetePesoKg")]
        public double? paquetePesoKg { get; set; }
    }

    // Error que devuelve el servidor REST: {"mensaje": "..."}
    public class MensajeError
    {
        [JsonPropertyName("mensaje")]
        public string mensaje { get; set; }
    }
}
