namespace EnvioCliente.observer
{
    /// <summary>
    /// Observer: quien quiera enterarse de que la lista de envios cambio implementa esta interfaz.
    /// </summary>
    public interface IObservador
    {
        void Actualizar();
    }
}
