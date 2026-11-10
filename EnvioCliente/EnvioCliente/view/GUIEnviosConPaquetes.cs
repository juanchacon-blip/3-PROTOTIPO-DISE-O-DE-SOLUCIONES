using System;
using System.Drawing;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows.Forms;
using EnvioCliente.controller;
using EnvioCliente.observer;

namespace EnvioCliente.view
{
    // Caso de uso: consulta personalizada maestro-detalle. Muestra todos los datos del envio (maestro)
    // y, por cada paquete (detalle), su codigo, descripcion y peso. Se puede filtrar por codigo de envio.
    // Es un observador: se refresca cuando se adiciona, actualiza o elimina un envio o un paquete.
    public class GUIEnviosConPaquetes : Form, IObservador
    {
        private readonly Label lblEnvioCodigo = new Label();
        private readonly TextBox txtEnvioCodigo = new TextBox();
        private readonly Button btnConsultar = new Button();
        private readonly Button btnVerTodos = new Button();
        private readonly DataGridView dgvEnvios = new DataGridView();

        private string filtroEnvio = "";

        public GUIEnviosConPaquetes()
        {
            Text = "Envios con sus paquetes";
            ClientSize = new Size(900, 430);
            FormBorderStyle = FormBorderStyle.Sizable;

            lblEnvioCodigo.Text = "Codigo del envio:";
            lblEnvioCodigo.Location = new Point(20, 22);
            lblEnvioCodigo.AutoSize = true;

            txtEnvioCodigo.Location = new Point(130, 19);
            txtEnvioCodigo.Size = new Size(140, 20);

            btnConsultar.Text = "Consultar";
            btnConsultar.Location = new Point(285, 17);
            btnConsultar.Size = new Size(90, 25);
            btnConsultar.Click += btnConsultar_Click;

            btnVerTodos.Text = "Ver todos";
            btnVerTodos.Location = new Point(385, 17);
            btnVerTodos.Size = new Size(90, 25);
            btnVerTodos.Click += btnVerTodos_Click;

            dgvEnvios.Location = new Point(20, 55);
            dgvEnvios.Size = new Size(860, 360);
            dgvEnvios.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvEnvios.AllowUserToAddRows = false;
            dgvEnvios.AllowUserToDeleteRows = false;
            dgvEnvios.ReadOnly = true;
            dgvEnvios.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvEnvios.CellFormatting += dgvEnvios_CellFormatting;

            Controls.Add(lblEnvioCodigo);
            Controls.Add(txtEnvioCodigo);
            Controls.Add(btnConsultar);
            Controls.Add(btnVerTodos);
            Controls.Add(dgvEnvios);

            Load += async (s, e) => await consultar();
            FormClosed += (s, e) => EnviosSubject.Instance.Desuscribir(this);
            EnviosSubject.Instance.Suscribir(this);
        }

        // Muestra la fecha ISO del servidor como dd/MM/yyyy HH:mm.
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

        // Llamado por el Subject cuando los datos cambiaron.
        public async void Actualizar()
        {
            if (IsDisposed || !IsHandleCreated)
            {
                return;
            }

            await consultar();
        }

        private async void btnConsultar_Click(object sender, EventArgs e)
        {
            filtroEnvio = txtEnvioCodigo.Text.Trim();
            await consultar();
        }

        private async void btnVerTodos_Click(object sender, EventArgs e)
        {
            txtEnvioCodigo.Text = "";
            filtroEnvio = "";
            await consultar();
        }

        private async Task consultar()
        {
            try
            {
                dgvEnvios.DataSource = await EnvioNacionalController.Instance.ConPaquetesAsync(filtroEnvio);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
