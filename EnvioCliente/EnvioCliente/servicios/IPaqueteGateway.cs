using System.Threading.Tasks;
using EnvioCliente.model;

namespace EnvioCliente.servicios
{
    /// <summary>
    /// Contrato de acceso al servidor para los paquetes (detalle). Dos implementaciones
    /// intercambiables (GraphQL y REST); el controlador depende solo de esta interfaz.
    /// Las consultas devuelven null si no hay resultado; las operaciones que modifican
    /// datos lanzan Exception con un mensaje apto para el usuario si fallan.
    /// </summary>
    public interface IPaqueteGateway
    {
        Task<PaqueteData[]> ListarAsync();
        Task<PaqueteData[]> FiltrarAsync(string envioCodigo, string descripcion);
        Task<PaqueteData> BuscarPorCodigoAsync(string codigo);
        Task<PaqueteData> BuscarPorCriteriosAsync(string codigo, string envioCodigo, string descripcion);
        Task AdicionarAsync(PaqueteInput input);
        Task ActualizarAsync(PaqueteInput input);
        Task EliminarAsync(string codigo);
    }
}
