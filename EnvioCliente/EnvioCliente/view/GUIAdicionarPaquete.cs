using System;
using System.Globalization;
using System.Windows.Forms;
using EnvioCliente.controller;

namespace EnvioCliente.view
{
    public partial class GUIAdicionarPaquete : Form
    {
        public GUIAdicionarPaquete()
        {
            InitializeComponent();
        }

        private async void btnAdicionar_Click(object sender, EventArgs e)
        {
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

            if (string.IsNullOrWhiteSpace(txtCodigo.Text) || string.IsNullOrWhiteSpace(txtEnvioCodigo.Text) ||
                string.IsNullOrWhiteSpace(txtDescripcion.Text))
            {
                MessageBox.Show("Complete el codigo, el codigo del envio y la descripcion.");
                return;
            }

            try
            {
                await PaqueteController.Instance.AdicionarAsync(
                    txtCodigo.Text, txtEnvioCodigo.Text, txtDescripcion.Text, peso, unidades, fecha,
                    chkFragil.Checked);

                MessageBox.Show("Paquete adicionado correctamente.");
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
