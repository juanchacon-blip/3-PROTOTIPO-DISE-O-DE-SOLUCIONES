using System;
using System.Threading.Tasks;
using EnvioCliente.model;
using EnvioCliente.observer;
using EnvioCliente.servicios;

namespace EnvioCliente.controller
{
    /// <summary>
    /// Controlador (MVC) del cliente: traduce las operaciones de la aplicacion a llamadas al
    /// servidor a traves de un IEnvioGateway (GraphQL o REST, intercambiables desde el menu).
    /// Las ventanas solo se encargan de la interfaz grafica.
    /// Tras cada cambio (adicionar, actualizar, eliminar) avisa a los observadores.
    /// Los metodos lanzan Exception con un mensaje apto para mostrar al usuario.
    /// </summary>
    public sealed class EnvioNacionalController
    {
        private static readonly EnvioNacionalController instancia = new EnvioNacionalController();

        private IEnvioGateway gateway = new EnvioGatewayGraphQL();

        private EnvioNacionalController() { }

        public static EnvioNacionalController Instance
        {
            get { return instancia; }
        }

        /// <summary>Protocolo en uso: "GraphQL" o "REST".</summary>
        public string Protocolo
        {
            get { return gateway.Nombre; }
        }

        public void UsarGraphQL()
        {
            gateway = new EnvioGatewayGraphQL();
        }

        public void UsarRest()
        {
            gateway = new EnvioGatewayRest();
        }

        // ---------- Consultas ----------

        public Task<EnvioNacionalData[]> ListarAsync()
        {
            return gateway.ListarAsync();
        }

        public Task<EnvioNacionalData[]> FiltrarAsync(string origen, string departamentoDestino)
        {
            return gateway.FiltrarAsync(origen, departamentoDestino);
        }

        public Task<EnvioConPaqueteData[]> ConPaquetesAsync(string envioCodigo)
        {
            return gateway.ConPaquetesAsync(envioCodigo);
        }

        public Task<EnvioNacionalData> BuscarPorCodigoAsync(string codigo)
        {
            return gateway.BuscarPorCodigoAsync(codigo);
        }

        public Task<EnvioNacionalData> BuscarPorCriteriosAsync(string codigo, string origen, string destino)
        {
            return gateway.BuscarPorCriteriosAsync(codigo, origen, destino);
        }

        // ---------- Operaciones que modifican datos ----------

        public async Task AdicionarAsync(string codigo, string origen, string destino, double precio,
                                         DateTime fechaSalida, string departamentoDestino, int? codigoPostal)
        {
            await gateway.AdicionarAsync(
                Construir(codigo, origen, destino, precio, fechaSalida, departamentoDestino, codigoPostal));

            EnviosSubject.Instance.Notificar();
        }

        public async Task ActualizarAsync(string codigo, string origen, string destino, double precio,
                                          DateTime fechaSalida, string departamentoDestino, int? codigoPostal)
        {
            await gateway.ActualizarAsync(
                Construir(codigo, origen, destino, precio, fechaSalida, departamentoDestino, codigoPostal));

            EnviosSubject.Instance.Notificar();
        }

        public async Task EliminarAsync(string codigo)
        {
            await gateway.EliminarAsync(codigo);

            EnviosSubject.Instance.Notificar();
        }

        private static EnvioNacionalInput Construir(string codigo, string origen, string destino, double precio,
                                                    DateTime fechaSalida, string departamentoDestino,
                                                    int? codigoPostal)
        {
            return new EnvioNacionalInput
            {
                codigo = (codigo ?? "").Trim(),
                origen = origen,
                destino = destino,
                precio = precio,
                fechaSalida = fechaSalida.ToString("yyyy-MM-ddTHH:mm:ss"),
                departamentoDestino = departamentoDestino,
                codigoPostalNacional = codigoPostal
            };
        }
    }
}
