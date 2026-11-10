using System;
using System.Threading.Tasks;
using EnvioCliente.model;

namespace EnvioCliente.servicios
{
    /// <summary>Acceso a los paquetes mediante el servicio web GraphQL.</summary>
    public sealed class PaqueteGatewayGraphQL : IPaqueteGateway
    {
        private const string Campos =
            "codigo envioCodigo descripcion pesoKg unidades fechaRegistro fragil envioOrigen envioDestino";

        private readonly ServicioGraphQL servicio = ServicioGraphQL.Instance;

        public async Task<PaqueteData[]> ListarAsync()
        {
            return await servicio.EjecutarAsync<PaqueteData[]>(
                       "query { paquetes { " + Campos + " } }", null, "paquetes")
                   ?? new PaqueteData[0];
        }

        public async Task<PaqueteData[]> FiltrarAsync(string envioCodigo, string descripcion)
        {
            return await servicio.EjecutarAsync<PaqueteData[]>(
                       "query($envioCodigo: String, $descripcion: String) { " +
                       "paquetesFiltrados(envioCodigo: $envioCodigo, descripcion: $descripcion) { " +
                       Campos + " } }",
                       new { envioCodigo = Limpiar(envioCodigo), descripcion = Limpiar(descripcion) },
                       "paquetesFiltrados")
                   ?? new PaqueteData[0];
        }

        public Task<PaqueteData> BuscarPorCodigoAsync(string codigo)
        {
            return servicio.EjecutarAsync<PaqueteData>(
                "query($codigo: String!) { paquetePorCodigo(codigo: $codigo) { " + Campos + " } }",
                new { codigo = Limpiar(codigo) }, "paquetePorCodigo");
        }

        public Task<PaqueteData> BuscarPorCriteriosAsync(string codigo, string envioCodigo, string descripcion)
        {
            return servicio.EjecutarAsync<PaqueteData>(
                "query($codigo: String, $envioCodigo: String, $descripcion: String) { " +
                "paquetePorCriterios(codigo: $codigo, envioCodigo: $envioCodigo, descripcion: $descripcion) { " +
                Campos + " } }",
                new { codigo = Limpiar(codigo), envioCodigo = Limpiar(envioCodigo), descripcion = Limpiar(descripcion) },
                "paquetePorCriterios");
        }

        public async Task AdicionarAsync(PaqueteInput input)
        {
            var creado = await servicio.EjecutarAsync<PaqueteData>(
                "mutation($input: PaqueteInput!) { addPaquete(input: $input) { " + Campos + " } }",
                new { input = input }, "addPaquete");

            if (creado == null)
            {
                // El servidor responde null si el envio no existe o el codigo esta repetido: se distingue aqui.
                var envio = await servicio.EjecutarAsync<EnvioNacionalData>(
                    "query($codigo: String!) { envioNacionalPorCodigo(codigo: $codigo) { codigo } }",
                    new { codigo = Limpiar(input.envioCodigo) }, "envioNacionalPorCodigo");

                if (envio == null)
                {
                    throw new Exception("El envio " + Limpiar(input.envioCodigo) + " no existe.");
                }
                throw new Exception("Ya existe un paquete con ese codigo.");
            }
        }

        public async Task ActualizarAsync(PaqueteInput input)
        {
            var actualizado = await servicio.EjecutarAsync<PaqueteData>(
                "mutation($input: PaqueteInput!) { updatePaquete(input: $input) { " + Campos + " } }",
                new { input = input }, "updatePaquete");

            if (actualizado == null)
            {
                throw new Exception("El paquete ya no existe.");
            }
        }

        public async Task EliminarAsync(string codigo)
        {
            var eliminado = await servicio.EjecutarAsync<PaqueteData>(
                "mutation($codigo: String!) { deletePaquete(codigo: $codigo) { " + Campos + " } }",
                new { codigo = Limpiar(codigo) }, "deletePaquete");

            if (eliminado == null)
            {
                throw new Exception("El paquete ya no existe.");
            }
        }

        private static string Limpiar(string valor)
        {
            return (valor ?? "").Trim();
        }
    }
}
