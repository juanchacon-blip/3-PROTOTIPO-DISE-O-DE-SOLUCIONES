using System.Text.Json.Serialization;

namespace EnvioCliente.model
{
    // Paquete (DETALLE de un envio) tal como lo devuelve el servidor (GraphQL o REST).
    public class PaqueteData
    {
        [JsonPropertyName("codigo")]
        public string codigo { get; set; }

        [JsonPropertyName("envioCodigo")]
        public string envioCodigo { get; set; }

        [JsonPropertyName("descripcion")]
        public string descripcion { get; set; }

        [JsonPropertyName("pesoKg")]
        public double pesoKg { get; set; }

        [JsonPropertyName("unidades")]
        public int unidades { get; set; }

        [JsonPropertyName("fechaRegistro")]
        public string fechaRegistro { get; set; }

        [JsonPropertyName("fragil")]
        public bool fragil { get; set; }

        // Datos del envio (maestro) que completa el servidor: origen y destino.
        [JsonPropertyName("envioOrigen")]
        public string envioOrigen { get; set; }

        [JsonPropertyName("envioDestino")]
        public string envioDestino { get; set; }
    }

    // Datos que se envian al servidor: variable "input" (GraphQL) o cuerpo de POST/PUT (REST).
    public class PaqueteInput
    {
        [JsonPropertyName("codigo")]
        public string codigo { get; set; }

        [JsonPropertyName("envioCodigo")]
        public string envioCodigo { get; set; }

        [JsonPropertyName("descripcion")]
        public string descripcion { get; set; }

        [JsonPropertyName("pesoKg")]
        public double pesoKg { get; set; }

        [JsonPropertyName("unidades")]
        public int unidades { get; set; }

        [JsonPropertyName("fechaRegistro")]
        public string fechaRegistro { get; set; }

        [JsonPropertyName("fragil")]
        public bool fragil { get; set; }
    }
}
