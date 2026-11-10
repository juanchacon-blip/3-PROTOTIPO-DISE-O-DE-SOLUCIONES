using System;
using System.Globalization;
using System.Windows.Forms;
using EnvioCliente.controller;

namespace EnvioCliente.view
{
    public partial class GUIAdicionarEnvioNacional : Form
    {
        public GUIAdicionarEnvioNacional()
        {
            InitializeComponent();
        }

        private async void btnAdicionar_Click(object sender, EventArgs e)
        {
            double precio;
            int? codigoPostal = null;
            DateTime fechaSalida;

            if (!double.TryParse(txtPrecio.Text, out precio))
            {
                MessageBox.Show("El precio debe ser numerico.");
                return;
            }

            // El codigo postal es opcional: si se escribe, debe ser un entero positivo de hasta 6 digitos.
            if (!string.IsNullOrWhiteSpace(txtCodigoPostalNacional.Text))
            {
                int postal;
                if (!int.TryParse(txtCodigoPostalNacional.Text, out postal) || postal < 1 || postal > 999999)
                {
                    MessageBox.Show("El codigo postal (opcional) debe ser un entero positivo de hasta 6 digitos.");
                    return;
                }
                codigoPostal = postal;
            }

            fechaSalida = txtFechaSalida.Value;

            try
            {
                await EnvioNacionalController.Instance.AdicionarAsync(
                    txtCodigo.Text, txtOrigen.Text, txtDestino.Text, precio, fechaSalida,
                    txtDepartamentoDestino.Text, codigoPostal);

                MessageBox.Show("Envio adicionado correctamente.");
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
