using System;
using System.Globalization;
using System.Windows.Forms;
using EnvioCliente.controller;

namespace EnvioCliente.view
{
    public partial class GUIEliminarPaquete : Form
    {
        // Codigo del paquete que se busco y cuya informacion se esta mostrando.
        // Eliminar siempre opera sobre este codigo, no sobre lo que haya en el cuadro de texto.
        private string codigoBuscado = null;

        public GUIEliminarPaquete()
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
                var paquete = await PaqueteController.Instance.BuscarPorCodigoAsync(txtCodigo.Text);

                if (paquete == null)
                {
                    MessageBox.Show("No existe un paquete con ese codigo.");
                    codigoBuscado = null;
                    btnEliminar.Enabled = false;
                    return;
                }

                codigoBuscado = paquete.codigo;
                txtEnvioCodigo.Text = paquete.envioCodigo;
                txtDescripcion.Text = paquete.descripcion;
                txtPesoKg.Text = paquete.pesoKg.ToString(CultureInfo.CurrentCulture);
                txtUnidades.Text = paquete.unidades.ToString();
                txtFechaRegistro.Text = DateTime.Parse(paquete.fechaRegistro, CultureInfo.InvariantCulture)
                                                .ToString("dd/MM/yyyy HH:mm");
                txtFragil.Text = paquete.fragil ? "Si" : "No";

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
                MessageBox.Show("Primero busque el paquete que desea eliminar.");
                return;
            }

            try
            {
                await PaqueteController.Instance.EliminarAsync(codigoBuscado);

                MessageBox.Show("Paquete eliminado correctamente.");
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
