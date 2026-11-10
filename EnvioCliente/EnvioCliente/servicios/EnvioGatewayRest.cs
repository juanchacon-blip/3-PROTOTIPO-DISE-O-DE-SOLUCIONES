using System;
using System.Threading.Tasks;
using EnvioCliente.model;

namespace EnvioCliente.servicios
{
    /// <summary>Acceso al servidor mediante servicios web REST.</summary>
    public sealed class EnvioGatewayRest : IEnvioGateway
    {
        private const string Recurso = "/envios-nacionales";

        private readonly ServicioRest servicio = ServicioRest.Instance;

        public string Nombre { get { return "REST"; } }

        public async Task<EnvioNacionalData[]> ListarAsync()
        {
            return await servicio.ObtenerAsync<EnvioNacionalData[]>(Recurso)
                   ?? new EnvioNacionalData[0];
        }

        public async Task<EnvioNacionalData[]> FiltrarAsync(string origen, string departamentoDestino)
        {
            return await servicio.ObtenerAsync<EnvioNacionalData[]>(
                       Recurso + "?origen=" + Q(origen) + "&departamentoDestino=" + Q(departamentoDestino))
                   ?? new EnvioNacionalData[0];
        }

        public async Task<EnvioConPaqueteData[]> ConPaquetesAsync(string envioCodigo)
        {
            return await servicio.ObtenerAsync<EnvioConPaqueteData[]>(
                       Recurso + "/con-paquetes?envioCodigo=" + Q(envioCodigo))
                   ?? new EnvioConPaqueteData[0];
        }

        public Task<EnvioNacionalData> BuscarPorCodigoAsync(string codigo)
        {
            return servicio.ObtenerAsync<EnvioNacionalData>(Recurso + "/" + Q(codigo));
        }

        public Task<EnvioNacionalData> BuscarPorCriteriosAsync(string codigo, string origen, string destino)
        {
            return servicio.ObtenerAsync<EnvioNacionalData>(
                Recurso + "/buscar?codigo=" + Q(codigo) + "&origen=" + Q(origen) + "&destino=" + Q(destino));
        }

        public async Task AdicionarAsync(EnvioNacionalInput input)
        {
            await servicio.CrearAsync<EnvioNacionalData>(Recurso, input);
        }

        public async Task ActualizarAsync(EnvioNacionalInput input)
        {
            await servicio.ActualizarAsync<EnvioNacionalData>(Recurso + "/" + Q(input.codigo), input);
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
