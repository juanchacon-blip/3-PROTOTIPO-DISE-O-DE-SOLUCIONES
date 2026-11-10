using System.Threading.Tasks;
using EnvioCliente.model;

namespace EnvioCliente.servicios
{
    /// <summary>
    /// Contrato de acceso al servidor. Tiene dos implementaciones intercambiables
    /// (GraphQL y REST); el controlador depende solo de esta interfaz (SOLID: DIP / OCP).
    /// Las consultas devuelven null si no hay resultado; las operaciones que modifican
    /// datos lanzan Exception con un mensaje apto para el usuario si fallan.
    /// </summary>
    public interface IEnvioGateway
    {
        string Nombre { get; }

        Task<EnvioNacionalData[]> ListarAsync();
        Task<EnvioNacionalData[]> FiltrarAsync(string origen, string departamentoDestino);
        Task<EnvioNacionalData> BuscarPorCodigoAsync(string codigo);
        Task<EnvioNacionalData> BuscarPorCriteriosAsync(string codigo, string origen, string destino);
        Task<EnvioConPaqueteData[]> ConPaquetesAsync(string envioCodigo);
        Task AdicionarAsync(EnvioNacionalInput input);
        Task ActualizarAsync(EnvioNacionalInput input);
        Task EliminarAsync(string codigo);
    }
}
