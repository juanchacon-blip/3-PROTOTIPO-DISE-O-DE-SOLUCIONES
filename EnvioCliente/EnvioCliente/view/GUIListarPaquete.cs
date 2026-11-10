using System;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows.Forms;
using EnvioCliente.controller;
using EnvioCliente.observer;

namespace EnvioCliente.view
{
    // Observador: se suscribe al subject y se refresca cuando otra ventana
    // adiciona, actualiza o elimina un paquete (o elimina un envio, que arrastra sus paquetes).
    public partial class GUIListarPaquete : Form, IObservador
    {
        // Recuerda la ultima consulta (todos o filtrado) para repetirla al ser notificado.
        private bool ultimoFueFiltro = false;
        private string filtroEnvio = "";
        private string filtroDescripcion = "";

        public GUIListarPaquete()
        {
            InitializeComponent();
            dgvPaquetes.CellFormatting += dgvPaquetes_CellFormatting;
            EnviosSubject.Instance.Suscribir(this);
        }

        // Muestra la fecha ISO del servidor (2026-10-21T10:00:00) como dd/MM/yyyy HH:mm, igual que los formularios.
        private void dgvPaquetes_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0 || !(e.Value is string))
            {
                return;
            }

            if (dgvPaquetes.Columns[e.ColumnIndex].DataPropertyName != "fechaRegistro")
            {
                return;
            }

            DateTime fecha;
            if (DateTime.TryParse((string)e.Value, CultureInfo.InvariantCulture, DateTimeStyles.None, out fecha))
            {
                e.Value = fecha.ToString("dd/MM/yyyy HH:mm");
                e.FormattingApplied = true;
            }
        }

        private async void GUIListarPaquete_Load(object sender, EventArgs e)
        {
            await listarTodos();
        }

        private void GUIListarPaquete_FormClosed(object sender, FormClosedEventArgs e)
        {
            EnviosSubject.Instance.Desuscribir(this);
        }

        // Llamado por el Subject cuando los datos cambiaron.
        public async void Actualizar()
        {
            if (IsDisposed || !IsHandleCreated)
            {
                return;
            }

            if (ultimoFueFiltro)
            {
                await ejecutarFiltro(filtroEnvio, filtroDescripcion);
            }
            else
            {
                await listarTodos();
            }
        }

        private async void btnListarTodos_Click(object sender, EventArgs e)
        {
            txtEnvioCodigo.Text = "";
            txtDescripcion.Text = "";
            await listarTodos();
        }

        private async void btnFiltrar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtEnvioCodigo.Text) &&
                string.IsNullOrWhiteSpace(txtDescripcion.Text))
            {
                MessageBox.Show("Ingrese un codigo de envio o una descripcion para filtrar.");
                return;
            }

            await ejecutarFiltro(txtEnvioCodigo.Text.Trim(), txtDescripcion.Text.Trim());
        }

        private async Task listarTodos()
        {
            ultimoFueFiltro = false;

            try
            {
                dgvPaquetes.DataSource = await PaqueteController.Instance.ListarAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async Task ejecutarFiltro(string envioCodigo, string descripcion)
        {
            ultimoFueFiltro = true;
            filtroEnvio = envioCodigo;
            filtroDescripcion = descripcion;

            try
            {
                dgvPaquetes.DataSource = await PaqueteController.Instance.FiltrarAsync(envioCodigo, descripcion);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
