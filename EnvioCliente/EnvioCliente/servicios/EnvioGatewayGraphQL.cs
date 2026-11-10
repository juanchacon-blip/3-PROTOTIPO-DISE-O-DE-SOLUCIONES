using System;
using System.Threading.Tasks;
using EnvioCliente.model;

namespace EnvioCliente.servicios
{
    /// <summary>Acceso al servidor mediante el servicio web GraphQL (queries y mutations).</summary>
    public sealed class EnvioGatewayGraphQL : IEnvioGateway
    {
        private const string Campos =
            "codigo origen destino precio fechaSalida departamentoDestino codigoPostalNacional";

        private readonly ServicioGraphQL servicio = ServicioGraphQL.Instance;

        public string Nombre { get { return "GraphQL"; } }

        public async Task<EnvioNacionalData[]> ListarAsync()
        {
            return await servicio.EjecutarAsync<EnvioNacionalData[]>(
                       "query { enviosNacionales { " + Campos + " } }", null, "enviosNacionales")
                   ?? new EnvioNacionalData[0];
        }

        public async Task<EnvioNacionalData[]> FiltrarAsync(string origen, string departamentoDestino)
        {
            return await servicio.EjecutarAsync<EnvioNacionalData[]>(
                       "query($origen: String, $departamentoDestino: String) { " +
                       "enviosNacionalesFiltrados(origen: $origen, departamentoDestino: $departamentoDestino) { " +
                       Campos + " } }",
                       new { origen = Limpiar(origen), departamentoDestino = Limpiar(departamentoDestino) },
                       "enviosNacionalesFiltrados")
                   ?? new EnvioNacionalData[0];
        }

        public async Task<EnvioConPaqueteData[]> ConPaquetesAsync(string envioCodigo)
        {
            return await servicio.EjecutarAsync<EnvioConPaqueteData[]>(
                       "query($envioCodigo: String) { enviosConPaquetes(envioCodigo: $envioCodigo) { " +
                       "codigo origen destino precio fechaSalida departamentoDestino codigoPostalNacional " +
                       "paqueteCodigo paqueteDescripcion paquetePesoKg } }",
                       new { envioCodigo = Limpiar(envioCodigo) }, "enviosConPaquetes")
                   ?? new EnvioConPaqueteData[0];
        }

        public Task<EnvioNacionalData> BuscarPorCodigoAsync(string codigo)
        {
            return servicio.EjecutarAsync<EnvioNacionalData>(
                "query($codigo: String!) { envioNacionalPorCodigo(codigo: $codigo) { " + Campos + " } }",
                new { codigo = Limpiar(codigo) }, "envioNacionalPorCodigo");
        }

        public Task<EnvioNacionalData> BuscarPorCriteriosAsync(string codigo, string origen, string destino)
        {
            return servicio.EjecutarAsync<EnvioNacionalData>(
                "query($codigo: String, $origen: String, $destino: String) { " +
                "envioNacionalPorCriterios(codigo: $codigo, origen: $origen, destino: $destino) { " +
                Campos + " } }",
                new { codigo = Limpiar(codigo), origen = Limpiar(origen), destino = Limpiar(destino) },
                "envioNacionalPorCriterios");
        }

        public async Task AdicionarAsync(EnvioNacionalInput input)
        {
            var creado = await servicio.EjecutarAsync<EnvioNacionalData>(
                "mutation($input: EnvioNacionalInput!) { addEnvioNacional(input: $input) { " + Campos + " } }",
                new { input = input }, "addEnvioNacional");

            if (creado == null)
            {
                throw new Exception("Ya existe un envio con ese codigo.");
            }
        }

        public async Task ActualizarAsync(EnvioNacionalInput input)
        {
            var actualizado = await servicio.EjecutarAsync<EnvioNacionalData>(
                "mutation($input: EnvioNacionalInput!) { updateEnvioNacional(input: $input) { " + Campos + " } }",
                new { input = input }, "updateEnvioNacional");

            if (actualizado == null)
            {
                throw new Exception("El envio ya no existe.");
            }
        }

        public async Task EliminarAsync(string codigo)
        {
            var eliminado = await servicio.EjecutarAsync<EnvioNacionalData>(
                "mutation($codigo: String!) { deleteEnvioNacional(codigo: $codigo) { " + Campos + " } }",
                new { codigo = Limpiar(codigo) }, "deleteEnvioNacional");

            if (eliminado == null)
            {
                throw new Exception("El envio ya no existe.");
            }
        }

        private static string Limpiar(string valor)
        {
            return (valor ?? "").Trim();
        }
    }
}
