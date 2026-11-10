using System;
using System.Threading.Tasks;
using EnvioCliente.model;

namespace EnvioCliente.servicios
{
    /// <summary>Acceso a los paquetes mediante servicios web REST.</summary>
    public sealed class PaqueteGatewayRest : IPaqueteGateway
    {
        private const string Recurso = "/paquetes";

        private readonly ServicioRest servicio = ServicioRest.Instance;

        public async Task<PaqueteData[]> ListarAsync()
        {
            return await servicio.ObtenerAsync<PaqueteData[]>(Recurso)
                   ?? new PaqueteData[0];
        }

        public async Task<PaqueteData[]> FiltrarAsync(string envioCodigo, string descripcion)
        {
            return await servicio.ObtenerAsync<PaqueteData[]>(
                       Recurso + "?envioCodigo=" + Q(envioCodigo) + "&descripcion=" + Q(descripcion))
                   ?? new PaqueteData[0];
        }

        public Task<PaqueteData> BuscarPorCodigoAsync(string codigo)
        {
            return servicio.ObtenerAsync<PaqueteData>(Recurso + "/" + Q(codigo));
        }

        public Task<PaqueteData> BuscarPorCriteriosAsync(string codigo, string envioCodigo, string descripcion)
        {
            return servicio.ObtenerAsync<PaqueteData>(
                Recurso + "/buscar?codigo=" + Q(codigo) + "&envioCodigo=" + Q(envioCodigo) +
                "&descripcion=" + Q(descripcion));
        }

        public async Task AdicionarAsync(PaqueteInput input)
        {
            await servicio.CrearAsync<PaqueteData>(Recurso, input);
        }

        public async Task ActualizarAsync(PaqueteInput input)
        {
            await servicio.ActualizarAsync<PaqueteData>(Recurso + "/" + Q(input.codigo), input);
        }

        public Task EliminarAsync(string codigo)
        {
            return servicio.EliminarAsync(Recurso + "/" + Q(codigo));
        }

        private static string Q(string valor)
        {
            return Uri.EscapeDataString((valor ?? "").Trim());
        }
    }
}
