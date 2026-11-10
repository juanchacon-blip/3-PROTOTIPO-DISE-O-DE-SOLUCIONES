using System;
using System.Globalization;
using System.Windows.Forms;
using EnvioCliente.controller;

namespace EnvioCliente.view
{
    public partial class GUIEliminarEnvioNacional : Form
    {
        // Codigo del envio que se busco y cuya informacion se esta mostrando.
        // Eliminar siempre opera sobre este codigo, no sobre lo que haya en el cuadro de texto.
        private string codigoBuscado = null;

        public GUIEliminarEnvioNacional()
        {
            InitializeComponent();

            // Si el usuario cambia el codigo despues de buscar, debe buscar de nuevo.
            txtCodigo.TextChanged += (s, e) =>
            {
                if (codigoBuscado != null && !string.Equals(txtCodigo.Text.Trim(), codigoBuscado, StringComparison.OrdinalIgnoreCase))
                {
                    codigoBuscado = null;
                    btnEliminar.Enabled = false;
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
                    btnEliminar.Enabled = false;
                    return;
                }

                codigoBuscado = envio.codigo;
                txtOrigen.Text = envio.origen;
                txtDestino.Text = envio.destino;
                txtPrecio.Text = envio.precio.ToString();
                txtFechaSalida.Text = DateTime.Parse(envio.fechaSalida, CultureInfo.InvariantCulture)
                                              .ToString("dd/MM/yyyy HH:mm");
                txtDepartamentoDestino.Text = envio.departamentoDestino;
                txtCodigoPostalNacional.Text = envio.codigoPostalNacional.ToString();

                btnEliminar.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void btnEliminar_Click(object sender, EventArgs e)
        {
            if (codigoBuscado == null)
            {
                MessageBox.Show("Primero busque el envio que desea eliminar.");
                return;
            }

            try
            {
                // Maestro-detalle: al eliminar el envio se eliminan tambien sus paquetes.
                var paquetes = await PaqueteController.Instance.FiltrarAsync(codigoBuscado, "");
                string pregunta = "¿Eliminar el envio " + codigoBuscado + "?";
                if (paquetes.Length > 0)
                {
                    pregunta += "\n\nTambien se eliminaran sus " + paquetes.Length + " paquete(s).";
                }
                if (MessageBox.Show(pregunta, "Confirmar", MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question) != DialogResult.Yes)
                {
                    return;
                }

                await EnvioNacionalController.Instance.EliminarAsync(codigoBuscado);

                MessageBox.Show("Envio eliminado correctamente.");
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
