namespace EnvioCliente.view
{
    partial class GUIEliminarEnvioNacional
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblCodigo = new System.Windows.Forms.Label();
            this.txtCodigo = new System.Windows.Forms.TextBox();
            this.btnBuscar = new System.Windows.Forms.Button();
            this.lblOrigen = new System.Windows.Forms.Label();
            this.txtOrigen = new System.Windows.Forms.TextBox();
            this.lblDestino = new System.Windows.Forms.Label();
            this.txtDestino = new System.Windows.Forms.TextBox();
            this.lblPrecio = new System.Windows.Forms.Label();
            this.txtPrecio = new System.Windows.Forms.TextBox();
            this.lblFechaSalida = new System.Windows.Forms.Label();
            this.txtFechaSalida = new System.Windows.Forms.TextBox();
            this.lblDepartamentoDestino = new System.Windows.Forms.Label();
            this.txtDepartamentoDestino = new System.Windows.Forms.TextBox();
            this.lblCodigoPostalNacional = new System.Windows.Forms.Label();
            this.txtCodigoPostalNacional = new System.Windows.Forms.TextBox();
            this.btnEliminar = new System.Windows.Forms.Button();
            this.SuspendLayout();
            this.lblCodigo.Location = new System.Drawing.Point(20, 20);
            this.lblCodigo.Name = "lblCodigo";
            this.lblCodigo.Size = new System.Drawing.Size(100, 20);
            this.lblCodigo.Text = "Codigo:";
            this.txtCodigo.Location = new System.Drawing.Point(130, 17);
            this.txtCodigo.Name = "txtCodigo";
            this.txtCodigo.Size = new System.Drawing.Size(140, 23);
            this.btnBuscar.Location = new System.Drawing.Point(280, 16);
            this.btnBuscar.Name = "btnBuscar";
            this.btnBuscar.Size = new System.Drawing.Size(90, 25);
            this.btnBuscar.Text = "Buscar";
            this.btnBuscar.UseVisualStyleBackColor = true;
            this.btnBuscar.Click += new System.EventHandler(this.btnBuscar_Click);
            this.lblOrigen.Location = new System.Drawing.Point(20, 60);
            this.lblOrigen.Name = "lblOrigen";
            this.lblOrigen.Size = new System.Drawing.Size(140, 20);
            this.lblOrigen.Text = "Origen:";
            this.txtOrigen.Location = new System.Drawing.Point(170, 57);
            this.txtOrigen.Name = "txtOrigen";
            this.txtOrigen.Size = new System.Drawing.Size(180, 23);
            this.txtOrigen.ReadOnly = true;
            this.lblDestino.Location = new System.Drawing.Point(20, 95);
            this.lblDestino.Name = "lblDestino";
            this.lblDestino.Size = new System.Drawing.Size(140, 20);
            this.lblDestino.Text = "Destino:";
            this.txtDestino.Location = new System.Drawing.Point(170, 92);
            this.txtDestino.Name = "txtDestino";
            this.txtDestino.Size = new System.Drawing.Size(180, 23);
            this.txtDestino.ReadOnly = true;
            this.lblPrecio.Location = new System.Drawing.Point(20, 130);
            this.lblPrecio.Name = "lblPrecio";
            this.lblPrecio.Size = new System.Drawing.Size(140, 20);
            this.lblPrecio.Text = "Precio:";
            this.txtPrecio.Location = new System.Drawing.Point(170, 127);
            this.txtPrecio.Name = "txtPrecio";
            this.txtPrecio.Size = new System.Drawing.Size(180, 23);
            this.txtPrecio.ReadOnly = true;
            this.lblFechaSalida.Location = new System.Drawing.Point(20, 165);
            this.lblFechaSalida.Name = "lblFechaSalida";
            this.lblFechaSalida.Size = new System.Drawing.Size(140, 32);
            this.lblFechaSalida.Text = "Fecha (ejemplo 04/05/2023):";
            this.txtFechaSalida.Location = new System.Drawing.Point(170, 162);
            this.txtFechaSalida.Name = "txtFechaSalida";
            this.txtFechaSalida.Size = new System.Drawing.Size(180, 23);
            this.txtFechaSalida.ReadOnly = true;
            this.lblDepartamentoDestino.Location = new System.Drawing.Point(20, 200);
            this.lblDepartamentoDestino.Name = "lblDepartamentoDestino";
            this.lblDepartamentoDestino.Size = new System.Drawing.Size(140, 20);
            this.lblDepartamentoDestino.Text = "Departamento destino:";
            this.txtDepartamentoDestino.Location = new System.Drawing.Point(170, 197);
            this.txtDepartamentoDestino.Name = "txtDepartamentoDestino";
            this.txtDepartamentoDestino.Size = new System.Drawing.Size(180, 23);
            this.txtDepartamentoDestino.ReadOnly = true;
            this.lblCodigoPostalNacional.Location = new System.Drawing.Point(20, 235);
            this.lblCodigoPostalNacional.Name = "lblCodigoPostalNacional";
            this.lblCodigoPostalNacional.Size = new System.Drawing.Size(140, 20);
            this.lblCodigoPostalNacional.Text = "Codigo postal:";
            this.txtCodigoPostalNacional.Location = new System.Drawing.Point(170, 232);
            this.txtCodigoPostalNacional.Name = "txtCodigoPostalNacional";
            this.txtCodigoPostalNacional.Size = new System.Drawing.Size(180, 23);
            this.txtCodigoPostalNacional.ReadOnly = true;
            this.btnEliminar.Location = new System.Drawing.Point(170, 275);
            this.btnEliminar.Name = "btnEliminar";
            this.btnEliminar.Size = new System.Drawing.Size(100, 30);
            this.btnEliminar.Text = "Eliminar";
            this.btnEliminar.UseVisualStyleBackColor = true;
            this.btnEliminar.Enabled = false;
            this.btnEliminar.Click += new System.EventHandler(this.btnEliminar_Click);
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(390, 335);
            this.Controls.Add(this.lblCodigo);
            this.Controls.Add(this.txtCodigo);
            this.Controls.Add(this.btnBuscar);
            this.Controls.Add(this.lblOrigen);
            this.Controls.Add(this.txtOrigen);
            this.Controls.Add(this.lblDestino);
            this.Controls.Add(this.txtDestino);
            this.Controls.Add(this.lblPrecio);
            this.Controls.Add(this.txtPrecio);
            this.Controls.Add(this.lblFechaSalida);
            this.Controls.Add(this.txtFechaSalida);
            this.Controls.Add(this.lblDepartamentoDestino);
            this.Controls.Add(this.txtDepartamentoDestino);
            this.Controls.Add(this.lblCodigoPostalNacional);
            this.Controls.Add(this.txtCodigoPostalNacional);
            this.Controls.Add(this.btnEliminar);
            this.Name = "GUIEliminarEnvioNacional";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Eliminar Envio Nacional";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.Label lblCodigo;
        private System.Windows.Forms.TextBox txtCodigo;
        private System.Windows.Forms.Button btnBuscar;
        private System.Windows.Forms.Label lblOrigen;
        private System.Windows.Forms.TextBox txtOrigen;
        private System.Windows.Forms.Label lblDestino;
        private System.Windows.Forms.TextBox txtDestino;
        private System.Windows.Forms.Label lblPrecio;
        private System.Windows.Forms.TextBox txtPrecio;
        private System.Windows.Forms.Label lblFechaSalida;
        private System.Windows.Forms.TextBox txtFechaSalida;
        private System.Windows.Forms.Label lblDepartamentoDestino;
        private System.Windows.Forms.TextBox txtDepartamentoDestino;
        private System.Windows.Forms.Label lblCodigoPostalNacional;
        private System.Windows.Forms.TextBox txtCodigoPostalNacional;
        private System.Windows.Forms.Button btnEliminar;
    }
}
