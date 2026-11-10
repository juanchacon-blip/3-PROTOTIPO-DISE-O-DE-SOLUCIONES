using System;
using System.Globalization;
using System.Threading.Tasks;
using EnvioCliente.model;
using EnvioCliente.observer;
using EnvioCliente.servicios;

namespace EnvioCliente.controller
{
    /// <summary>
    /// Controlador (MVC) de los paquetes (detalle del envio). Singleton.
    /// Usa el mismo protocolo (GraphQL o REST) que el controlador de envios, que se elige desde el menu.
    /// Tras cada cambio avisa a los observadores (la ventana Listar se refresca sola).
    /// </summary>
    public sealed class PaqueteController
    {
        private static readonly PaqueteController instancia = new PaqueteController();

        private readonly IPaqueteGateway graphQl = new PaqueteGatewayGraphQL();
        private readonly IPaqueteGateway rest = new PaqueteGatewayRest();

        private PaqueteController() { }

        public static PaqueteController Instance
        {
            get { return instancia; }
        }

        private IPaqueteGateway Gateway
        {
            get { return EnvioNacionalController.Instance.Protocolo == "GraphQL" ? graphQl : rest; }
        }

        // ---------- Consultas ----------

        public Task<PaqueteData[]> ListarAsync()
        {
            return Gateway.ListarAsync();
        }

        public Task<PaqueteData[]> FiltrarAsync(string envioCodigo, string descripcion)
        {
            return Gateway.FiltrarAsync(envioCodigo, descripcion);
        }

        public Task<PaqueteData> BuscarPorCodigoAsync(string codigo)
        {
            return Gateway.BuscarPorCodigoAsync(codigo);
        }

        public Task<PaqueteData> BuscarPorCriteriosAsync(string codigo, string envioCodigo, string descripcion)
        {
            return Gateway.BuscarPorCriteriosAsync(codigo, envioCodigo, descripcion);
        }

        // ---------- Operaciones que modifican datos ----------

        public async Task AdicionarAsync(string codigo, string envioCodigo, string descripcion, double pesoKg,
                                         int unidades, DateTime fechaRegistro, bool fragil)
        {
            await Gateway.AdicionarAsync(
                Construir(codigo, envioCodigo, descripcion, pesoKg, unidades, fechaRegistro, fragil));

            EnviosSubject.Instance.Notificar();
        }

        public async Task ActualizarAsync(string codigo, string envioCodigo, string descripcion, double pesoKg,
                                          int unidades, DateTime fechaRegistro, bool fragil)
        {
            await Gateway.ActualizarAsync(
                Construir(codigo, envioCodigo, descripcion, pesoKg, unidades, fechaRegistro, fragil));

            EnviosSubject.Instance.Notificar();
        }

        public async Task EliminarAsync(string codigo)
        {
            await Gateway.EliminarAsync(codigo);

            EnviosSubject.Instance.Notificar();
        }

        private static PaqueteInput Construir(string codigo, string envioCodigo, string descripcion, double pesoKg,
                                              int unidades, DateTime fechaRegistro, bool fragil)
        {
            return new PaqueteInput
            {
                codigo = (codigo ?? "").Trim(),
                envioCodigo = (envioCodigo ?? "").Trim(),
                descripcion = descripcion,
                pesoKg = pesoKg,
                unidades = unidades,
                fechaRegistro = fechaRegistro.ToString("yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture),
                fragil = fragil
            };
        }
    }
}
