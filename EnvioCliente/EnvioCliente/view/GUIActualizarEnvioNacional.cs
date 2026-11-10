using System;
using System.Globalization;
using System.Windows.Forms;
using EnvioCliente.controller;

namespace EnvioCliente.view
{
    public partial class GUIActualizarEnvioNacional : Form
    {
// Codigo del envio que se busco y cuya informacion se esta mostrando.
        // Actualizar siempre opera sobre este codigo, no sobre lo que haya en el cuadro de texto.
        private string codigoBuscado = null;

        public GUIActualizarEnvioNacional()
        {
            InitializeComponent();

            // Si el usuario cambia el codigo despues de buscar, debe buscar de nuevo.
            txtCodigo.TextChanged += (s, e) =>
            {
                if (codigoBuscado != null && !string.Equals(txtCodigo.Text.Trim(), codigoBuscado, StringComparison.OrdinalIgnoreCase))
                {
                    codigoBuscado = null;
                    habilitarCampos(false);
                }
            };
        }

        private async void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                var envio = await EnvioNacionalController.Instance.BuscarPorCodigoAsync(txtCodigo.Text);

                if (envio == null)
                {
                    MessageBox.Show("No existe un envio con ese codigo.");
                    codigoBuscado = null;
                    habilitarCampos(false);
                    return;
                }

                codigoBuscado = envio.codigo;
                txtOrigen.Text = envio.origen;
                txtDestino.Text = envio.destino;
                txtPrecio.Text = envio.precio.ToString();
                txtFechaSalida.Value = DateTime.Parse(envio.fechaSalida, CultureInfo.InvariantCulture);
                txtDepartamentoDestino.Text = envio.departamentoDestino;
                txtCodigoPostalNacional.Text = envio.codigoPostalNacional.ToString();

                habilitarCampos(true);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void btnActualizar_Click(object sender, EventArgs e)
        {
            if (codigoBuscado == null)
            {
                MessageBox.Show("Primero busque el envio que desea actualizar.");
                return;
            }

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
                await EnvioNacionalController.Instance.ActualizarAsync(
                    codigoBuscado, txtOrigen.Text, txtDestino.Text, precio, fechaSalida,
                    txtDepartamentoDestino.Text, codigoPostal);

                MessageBox.Show("Envio actualizado correctamente.");
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void habilitarCampos(bool habilitar)
        {
            txtOrigen.Enabled = habilitar;
            txtDestino.Enabled = habilitar;
            txtPrecio.Enabled = habilitar;
            txtFechaSalida.Enabled = habilitar;
            txtDepartamentoDestino.Enabled = habilitar;
            txtCodigoPostalNacional.Enabled = habilitar;
            btnActualizar.Enabled = habilitar;
        }
    }
}
