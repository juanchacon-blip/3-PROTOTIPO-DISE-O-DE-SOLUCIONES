using System;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows.Forms;
using EnvioCliente.controller;
using EnvioCliente.observer;

namespace EnvioCliente.view
{
    // Observador: se suscribe al subject y se refresca cuando otra ventana
    // adiciona, actualiza o elimina un envio.
    public partial class GUIListarEnvioNacional : Form, IObservador
    {
        // Recuerda la ultima consulta (todos o filtrado) para repetirla al ser notificado.
        private bool ultimoFueFiltro = false;
        private string filtroOrigen = "";
        private string filtroDepartamento = "";

        public GUIListarEnvioNacional()
        {
            InitializeComponent();
            dgvEnvios.CellFormatting += dgvEnvios_CellFormatting;
            EnviosSubject.Instance.Suscribir(this);
        }

        // Muestra la fecha ISO del servidor (2026-10-21T10:00:00) como dd/MM/yyyy HH:mm, igual que los formularios.
        private void dgvEnvios_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0 || !(e.Value is string))
            {
                return;
            }

            if (dgvEnvios.Columns[e.ColumnIndex].DataPropertyName != "fechaSalida")
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

        private async void GUIListarEnvioNacional_Load(object sender, EventArgs e)
        {
            await listarTodos();
        }

        private void GUIListarEnvioNacional_FormClosed(object sender, FormClosedEventArgs e)
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
                await ejecutarFiltro(filtroOrigen, filtroDepartamento);
            }
            else
            {
                await listarTodos();
            }
        }

        private async void btnListarTodos_Click(object sender, EventArgs e)
        {
            txtOrigen.Text = "";
            txtDepartamentoDestino.Text = "";
            await listarTodos();
        }

        private async void btnFiltrar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtOrigen.Text) &&
                string.IsNullOrWhiteSpace(txtDepartamentoDestino.Text))
            {
                MessageBox.Show("Ingrese un origen o un departamento destino para filtrar.");
                return;
            }

            await ejecutarFiltro(txtOrigen.Text.Trim(), txtDepartamentoDestino.Text.Trim());
        }

        private async Task listarTodos()
        {
            ultimoFueFiltro = false;

            try
            {
                dgvEnvios.DataSource = await EnvioNacionalController.Instance.ListarAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async Task ejecutarFiltro(string origen, string departamentoDestino)
        {
            ultimoFueFiltro = true;
            filtroOrigen = origen;
            filtroDepartamento = departamentoDestino;

            try
            {
                dgvEnvios.DataSource = await EnvioNacionalController.Instance
                    .FiltrarAsync(origen, departamentoDestino);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
