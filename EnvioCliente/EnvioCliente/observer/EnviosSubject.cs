using System.Collections.Generic;

namespace EnvioCliente.observer
{
    /// <summary>
    /// Subject del patron Observer (ademas es un Singleton).
    /// Las ventanas Adicionar, Actualizar y Eliminar llaman a Notificar() cuando
    /// modifican datos en el servidor, y la ventana Listar (observador) se refresca sola.
    /// </summary>
    public sealed class EnviosSubject
    {
        private static readonly EnviosSubject instancia = new EnviosSubject();
        private readonly List<IObservador> observadores = new List<IObservador>();

        private EnviosSubject() { }

        public static EnviosSubject Instance
        {
            get { return instancia; }
        }

        public void Suscribir(IObservador observador)
        {
            if (!observadores.Contains(observador))
            {
                observadores.Add(observador);
            }
        }

        public void Desuscribir(IObservador observador)
        {
            observadores.Remove(observador);
        }

        public void Notificar()
        {
            // Copia para permitir que un observador se desuscriba mientras se notifica.
            foreach (var observador in new List<IObservador>(observadores))
            {
                observador.Actualizar();
            }
        }
    }
}
