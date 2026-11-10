using System;
using System.Globalization;
using System.Windows.Forms;
using EnvioCliente.controller;

namespace EnvioCliente.view
{
    public partial class GUIActualizarPaquete : Form
    {
// Codigo del paquete que se busco y cuya informacion se esta mostrando.
        // Actualizar siempre opera sobre este codigo, no sobre lo que haya en el cuadro de texto.
        private string codigoBuscado = null;

        public GUIActualizarPaquete()
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
                var paquete = await PaqueteController.Instance.BuscarPorCodigoAsync(txtCodigo.Text);

                if (paquete == null)
                {
                    MessageBox.Show("No existe un paquete con ese codigo.");
                    codigoBuscado = null;
                    habilitarCampos(false);
                    return;
                }

                codigoBuscado = paquete.codigo;
                txtEnvioCodigo.Text = paquete.envioCodigo;
                txtDescripcion.Text = paquete.descripcion;
                txtPesoKg.Text = paquete.pesoKg.ToString(CultureInfo.CurrentCulture);
                txtUnidades.Text = paquete.unidades.ToString();
                txtFechaRegistro.Value = DateTime.Parse(paquete.fechaRegistro, CultureInfo.InvariantCulture);
                chkFragil.Checked = paquete.fragil;

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
                MessageBox.Show("Primero busque el paquete que desea actualizar.");
                return;
            }

            double peso;
            int unidades;
            DateTime fecha;

            if (!double.TryParse(txtPesoKg.Text, out peso) || peso <= 0)
            {
                MessageBox.Show("El peso debe ser un numero mayor que cero.");
                return;
            }

            if (!int.TryParse(txtUnidades.Text, out unidades) || unidades < 1)
            {
                MessageBox.Show("Las unidades deben ser un numero entero mayor o igual a 1.");
                return;
            }

            fecha = txtFechaRegistro.Value;

            if (string.IsNullOrWhiteSpace(txtDescripcion.Text))
            {
                MessageBox.Show("La descripcion no puede estar vacia.");
                return;
            }

            try
            {
                await PaqueteController.Instance.ActualizarAsync(
                    codigoBuscado, txtEnvioCodigo.Text, txtDescripcion.Text, peso, unidades, fecha,
                    chkFragil.Checked);

                MessageBox.Show("Paquete actualizado correctamente.");
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void habilitarCampos(bool habilitar)
        {
            // El envio al que pertenece el paquete no se modifica (solo lectura siempre).
            txtDescripcion.Enabled = habilitar;
            txtPesoKg.Enabled = habilitar;
            txtUnidades.Enabled = habilitar;
            txtFechaRegistro.Enabled = habilitar;
            chkFragil.Enabled = habilitar;
            btnActualizar.Enabled = habilitar;
        }
    }
}
