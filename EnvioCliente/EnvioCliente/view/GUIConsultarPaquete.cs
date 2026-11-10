using System;
using System.Globalization;
using System.Windows.Forms;
using EnvioCliente.controller;
using EnvioCliente.model;

namespace EnvioCliente.view
{
    public partial class GUIConsultarPaquete : Form
    {
        public GUIConsultarPaquete()
        {
            InitializeComponent();
        }

        private async void btnConsultar_Click(object sender, EventArgs e)
        {
            string codigo = txtCodigo.Text.Trim();
            string envioCodigo = txtEnvioCodigo.Text.Trim();
            string descripcion = txtDescripcion.Text.Trim();

            int criterios = (codigo != "" ? 1 : 0) + (envioCodigo != "" ? 1 : 0) + (descripcion != "" ? 1 : 0);

            if (criterios == 0)
            {
                MessageBox.Show("Ingrese al menos un criterio de busqueda (codigo, codigo del envio o descripcion).");
                return;
            }

            if (criterios > 2)
            {
                MessageBox.Show("Ingrese maximo dos criterios de busqueda.");
                return;
            }

            try
            {
                var paquete = await PaqueteController.Instance
                    .BuscarPorCriteriosAsync(codigo, envioCodigo, descripcion);

                if (paquete == null)
                {
                    MessageBox.Show("No se encontro ningun paquete con los criterios ingresados.");
                    limpiarResultado();
                    return;
                }

                mostrarPaquete(paquete);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            txtCodigo.Text = "";
            txtEnvioCodigo.Text = "";
            txtDescripcion.Text = "";
            limpiarResultado();
            txtCodigo.Focus();
        }

        private void mostrarPaquete(PaqueteData paquete)
        {
            txtResCodigo.Text = paquete.codigo;
            txtResEnvioCodigo.Text = paquete.envioCodigo;
            txtResDescripcion.Text = paquete.descripcion;
            txtResPesoKg.Text = paquete.pesoKg.ToString(CultureInfo.CurrentCulture);
            txtResUnidades.Text = paquete.unidades.ToString();
            txtResFecha.Text = DateTime.Parse(paquete.fechaRegistro, CultureInfo.InvariantCulture)
                                       .ToString("dd/MM/yyyy HH:mm");
            txtResFragil.Text = paquete.fragil ? "Si" : "No";
        }

        private void limpiarResultado()
        {
            txtResCodigo.Text = "";
            txtResEnvioCodigo.Text = "";
            txtResDescripcion.Text = "";
            txtResPesoKg.Text = "";
            txtResUnidades.Text = "";
            txtResFecha.Text = "";
            txtResFragil.Text = "";
        }
    }
}
