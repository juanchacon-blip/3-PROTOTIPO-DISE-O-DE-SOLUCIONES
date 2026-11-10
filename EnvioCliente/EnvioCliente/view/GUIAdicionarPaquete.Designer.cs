namespace EnvioCliente.view
{
    partial class GUIAdicionarPaquete
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
            this.lblEnvioCodigo = new System.Windows.Forms.Label();
            this.txtEnvioCodigo = new System.Windows.Forms.TextBox();
            this.lblDescripcion = new System.Windows.Forms.Label();
            this.txtDescripcion = new System.Windows.Forms.TextBox();
            this.lblPesoKg = new System.Windows.Forms.Label();
            this.txtPesoKg = new System.Windows.Forms.TextBox();
            this.lblUnidades = new System.Windows.Forms.Label();
            this.txtUnidades = new System.Windows.Forms.TextBox();
            this.lblFechaRegistro = new System.Windows.Forms.Label();
            this.txtFechaRegistro = new System.Windows.Forms.DateTimePicker();
            this.lblFragil = new System.Windows.Forms.Label();
            this.chkFragil = new System.Windows.Forms.CheckBox();
            this.btnAdicionar = new System.Windows.Forms.Button();
            this.SuspendLayout();
            this.lblCodigo.Location = new System.Drawing.Point(20, 20);
            this.lblCodigo.Name = "lblCodigo";
            this.lblCodigo.Size = new System.Drawing.Size(140, 20);
            this.lblCodigo.Text = "Codigo:";
            this.txtCodigo.Location = new System.Drawing.Point(170, 17);
            this.txtCodigo.Name = "txtCodigo";
            this.txtCodigo.Size = new System.Drawing.Size(180, 23);
            this.lblEnvioCodigo.Location = new System.Drawing.Point(20, 55);
            this.lblEnvioCodigo.Name = "lblEnvioCodigo";
            this.lblEnvioCodigo.Size = new System.Drawing.Size(140, 20);
            this.lblEnvioCodigo.Text = "Codigo del envio:";
            this.txtEnvioCodigo.Location = new System.Drawing.Point(170, 52);
            this.txtEnvioCodigo.Name = "txtEnvioCodigo";
            this.txtEnvioCodigo.Size = new System.Drawing.Size(180, 23);
            this.lblDescripcion.Location = new System.Drawing.Point(20, 90);
            this.lblDescripcion.Name = "lblDescripcion";
            this.lblDescripcion.Size = new System.Drawing.Size(140, 20);
            this.lblDescripcion.Text = "Descripcion:";
            this.txtDescripcion.Location = new System.Drawing.Point(170, 87);
            this.txtDescripcion.Name = "txtDescripcion";
            this.txtDescripcion.Size = new System.Drawing.Size(180, 23);
            this.lblPesoKg.Location = new System.Drawing.Point(20, 125);
            this.lblPesoKg.Name = "lblPesoKg";
            this.lblPesoKg.Size = new System.Drawing.Size(140, 20);
            this.lblPesoKg.Text = "Peso (kg):";
            this.txtPesoKg.Location = new System.Drawing.Point(170, 122);
            this.txtPesoKg.Name = "txtPesoKg";
            this.txtPesoKg.Size = new System.Drawing.Size(180, 23);
            this.lblUnidades.Location = new System.Drawing.Point(20, 160);
            this.lblUnidades.Name = "lblUnidades";
            this.lblUnidades.Size = new System.Drawing.Size(140, 20);
            this.lblUnidades.Text = "Unidades:";
            this.txtUnidades.Location = new System.Drawing.Point(170, 157);
            this.txtUnidades.Name = "txtUnidades";
            this.txtUnidades.Size = new System.Drawing.Size(180, 23);
            this.lblFechaRegistro.Location = new System.Drawing.Point(20, 195);
            this.lblFechaRegistro.Name = "lblFechaRegistro";
            this.lblFechaRegistro.Size = new System.Drawing.Size(140, 32);
            this.lblFechaRegistro.Text = "Fecha y hora:";
            this.txtFechaRegistro.Location = new System.Drawing.Point(170, 192);
            this.txtFechaRegistro.Name = "txtFechaRegistro";
            this.txtFechaRegistro.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.txtFechaRegistro.CustomFormat = "dd/MM/yyyy HH:mm";
            this.txtFechaRegistro.ShowUpDown = false;
            this.txtFechaRegistro.Size = new System.Drawing.Size(180, 23);
            this.lblFragil.Location = new System.Drawing.Point(20, 230);
            this.lblFragil.Name = "lblFragil";
            this.lblFragil.Size = new System.Drawing.Size(140, 20);
            this.lblFragil.Text = "Fragil:";
            this.chkFragil.Location = new System.Drawing.Point(170, 227);
            this.chkFragil.Name = "chkFragil";
            this.chkFragil.Size = new System.Drawing.Size(180, 23);
            this.chkFragil.Text = "Es fragil";
            this.chkFragil.UseVisualStyleBackColor = true;
            this.btnAdicionar.Location = new System.Drawing.Point(170, 270);
            this.btnAdicionar.Name = "btnAdicionar";
            this.btnAdicionar.Size = new System.Drawing.Size(100, 30);
            this.btnAdicionar.Text = "Adicionar";
            this.btnAdicionar.UseVisualStyleBackColor = true;
            this.btnAdicionar.Click += new System.EventHandler(this.btnAdicionar_Click);
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(390, 330);
            this.Controls.Add(this.lblCodigo);
            this.Controls.Add(this.txtCodigo);
            this.Controls.Add(this.lblEnvioCodigo);
            this.Controls.Add(this.txtEnvioCodigo);
            this.Controls.Add(this.lblDescripcion);
            this.Controls.Add(this.txtDescripcion);
            this.Controls.Add(this.lblPesoKg);
            this.Controls.Add(this.txtPesoKg);
            this.Controls.Add(this.lblUnidades);
            this.Controls.Add(this.txtUnidades);
            this.Controls.Add(this.lblFechaRegistro);
            this.Controls.Add(this.txtFechaRegistro);
            this.Controls.Add(this.lblFragil);
            this.Controls.Add(this.chkFragil);
            this.Controls.Add(this.btnAdicionar);
            this.Name = "GUIAdicionarPaquete";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Adicionar Paquete";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.Label lblCodigo;
        private System.Windows.Forms.TextBox txtCodigo;
        private System.Windows.Forms.Label lblEnvioCodigo;
        private System.Windows.Forms.TextBox txtEnvioCodigo;
        private System.Windows.Forms.Label lblDescripcion;
        private System.Windows.Forms.TextBox txtDescripcion;
        private System.Windows.Forms.Label lblPesoKg;
        private System.Windows.Forms.TextBox txtPesoKg;
        private System.Windows.Forms.Label lblUnidades;
        private System.Windows.Forms.TextBox txtUnidades;
        private System.Windows.Forms.Label lblFechaRegistro;
        private System.Windows.Forms.DateTimePicker txtFechaRegistro;
        private System.Windows.Forms.Label lblFragil;
        private System.Windows.Forms.CheckBox chkFragil;
        private System.Windows.Forms.Button btnAdicionar;
    }
}
