using System;
using System.Globalization;
using System.Windows.Forms;
using EnvioCliente.controller;
using EnvioCliente.model;

namespace EnvioCliente.view
{
    public partial class GUIConsultarEnvioNacional : Form
    {
        public GUIConsultarEnvioNacional()
        {
            InitializeComponent();
        }

        private async void btnConsultar_Click(object sender, EventArgs e)
        {
            string codigo = txtCodigo.Text.Trim();
            string origen = txtOrigen.Text.Trim();
            string destino = txtDestino.Text.Trim();

            int criterios = (codigo != "" ? 1 : 0) + (origen != "" ? 1 : 0) + (destino != "" ? 1 : 0);

            if (criterios == 0)
            {
                MessageBox.Show("Ingrese al menos un criterio de busqueda (codigo, origen o destino).");
                return;
            }

            if (criterios > 2)
            {
                MessageBox.Show("Ingrese maximo dos criterios de busqueda.");
                return;
            }

            try
            {
                var envio = await EnvioNacionalController.Instance
                    .BuscarPorCriteriosAsync(codigo, origen, destino);

                if (envio == null)
                {
                    MessageBox.Show("No se encontro ningun envio con los criterios ingresados.");
                    limpiarResultado();
                    return;
                }

                mostrarEnvio(envio);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            txtCodigo.Text = "";
            txtOrigen.Text = "";
            txtDestino.Text = "";
            limpiarResultado();
            txtCodigo.Focus();
        }

        private void mostrarEnvio(EnvioNacionalData envio)
        {
            txtResCodigo.Text = envio.codigo;
            txtResOrigen.Text = envio.origen;
            txtResDestino.Text = envio.destino;
            txtResPrecio.Text = envio.precio.ToString(CultureInfo.CurrentCulture);
            txtResFecha.Text = DateTime.Parse(envio.fechaSalida, CultureInfo.InvariantCulture)
                                       .ToString("dd/MM/yyyy HH:mm");
            txtResDepartamento.Text = envio.departamentoDestino;
            txtResCodigoPostal.Text = envio.codigoPostalNacional.ToString();
        }

        private void limpiarResultado()
        {
            txtResCodigo.Text = "";
            txtResOrigen.Text = "";
            txtResDestino.Text = "";
            txtResPrecio.Text = "";
            txtResFecha.Text = "";
            txtResDepartamento.Text = "";
            txtResCodigoPostal.Text = "";
        }
    }
}
