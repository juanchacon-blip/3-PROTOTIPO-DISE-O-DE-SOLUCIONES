namespace EnvioCliente.view
{
    partial class GUIActualizarPaquete
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
            this.btnActualizar = new System.Windows.Forms.Button();
            this.SuspendLayout();
            this.lblCodigo.Location = new System.Drawing.Point(20, 20);
            this.lblCodigo.Name = "lblCodigo";
            this.lblCodigo.Size = new System.Drawing.Size(140, 20);
            this.lblCodigo.Text = "Codigo del paquete:";
            this.txtCodigo.Location = new System.Drawing.Point(170, 17);
            this.txtCodigo.Name = "txtCodigo";
            this.txtCodigo.Size = new System.Drawing.Size(180, 23);
            this.btnBuscar.Location = new System.Drawing.Point(360, 14);
            this.btnBuscar.Name = "btnBuscar";
            this.btnBuscar.Size = new System.Drawing.Size(80, 28);
            this.btnBuscar.Text = "Buscar";
            this.btnBuscar.UseVisualStyleBackColor = true;
            this.btnBuscar.Click += new System.EventHandler(this.btnBuscar_Click);
            this.lblEnvioCodigo.Location = new System.Drawing.Point(20, 65);
            this.lblEnvioCodigo.Name = "lblEnvioCodigo";
            this.lblEnvioCodigo.Size = new System.Drawing.Size(140, 20);
            this.lblEnvioCodigo.Text = "Codigo del envio:";
            this.txtEnvioCodigo.Location = new System.Drawing.Point(170, 62);
            this.txtEnvioCodigo.Name = "txtEnvioCodigo";
            this.txtEnvioCodigo.Size = new System.Drawing.Size(180, 23);
            this.txtEnvioCodigo.ReadOnly = true;
            this.txtEnvioCodigo.Enabled = false;
            this.lblDescripcion.Location = new System.Drawing.Point(20, 100);
            this.lblDescripcion.Name = "lblDescripcion";
            this.lblDescripcion.Size = new System.Drawing.Size(140, 20);
            this.lblDescripcion.Text = "Descripcion:";
            this.txtDescripcion.Location = new System.Drawing.Point(170, 97);
            this.txtDescripcion.Name = "txtDescripcion";
            this.txtDescripcion.Size = new System.Drawing.Size(180, 23);
            this.txtDescripcion.Enabled = false;
            this.lblPesoKg.Location = new System.Drawing.Point(20, 135);
            this.lblPesoKg.Name = "lblPesoKg";
            this.lblPesoKg.Size = new System.Drawing.Size(140, 20);
            this.lblPesoKg.Text = "Peso (kg):";
            this.txtPesoKg.Location = new System.Drawing.Point(170, 132);
            this.txtPesoKg.Name = "txtPesoKg";
            this.txtPesoKg.Size = new System.Drawing.Size(180, 23);
            this.txtPesoKg.Enabled = false;
            this.lblUnidades.Location = new System.Drawing.Point(20, 170);
            this.lblUnidades.Name = "lblUnidades";
            this.lblUnidades.Size = new System.Drawing.Size(140, 20);
            this.lblUnidades.Text = "Unidades:";
            this.txtUnidades.Location = new System.Drawing.Point(170, 167);
            this.txtUnidades.Name = "txtUnidades";
            this.txtUnidades.Size = new System.Drawing.Size(180, 23);
            this.txtUnidades.Enabled = false;
            this.lblFechaRegistro.Location = new System.Drawing.Point(20, 205);
            this.lblFechaRegistro.Name = "lblFechaRegistro";
            this.lblFechaRegistro.Size = new System.Drawing.Size(140, 32);
            this.lblFechaRegistro.Text = "Fecha y hora:";
            this.txtFechaRegistro.Location = new System.Drawing.Point(170, 202);
            this.txtFechaRegistro.Name = "txtFechaRegistro";
            this.txtFechaRegistro.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.txtFechaRegistro.CustomFormat = "dd/MM/yyyy HH:mm";
            this.txtFechaRegistro.ShowUpDown = false;
            this.txtFechaRegistro.Size = new System.Drawing.Size(180, 23);
            this.txtFechaRegistro.Enabled = false;
            this.lblFragil.Location = new System.Drawing.Point(20, 240);
            this.lblFragil.Name = "lblFragil";
            this.lblFragil.Size = new System.Drawing.Size(140, 20);
            this.lblFragil.Text = "Fragil:";
            this.chkFragil.Location = new System.Drawing.Point(170, 237);
            this.chkFragil.Name = "chkFragil";
            this.chkFragil.Size = new System.Drawing.Size(180, 23);
            this.chkFragil.Text = "Es fragil";
            this.chkFragil.Enabled = false;
            this.chkFragil.UseVisualStyleBackColor = true;
            this.btnActualizar.Location = new System.Drawing.Point(170, 280);
            this.btnActualizar.Name = "btnActualizar";
            this.btnActualizar.Size = new System.Drawing.Size(100, 30);
            this.btnActualizar.Text = "Actualizar";
            this.btnActualizar.Enabled = false;
            this.btnActualizar.UseVisualStyleBackColor = true;
            this.btnActualizar.Click += new System.EventHandler(this.btnActualizar_Click);
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(460, 340);
            this.Controls.Add(this.lblCodigo);
            this.Controls.Add(this.txtCodigo);
            this.Controls.Add(this.btnBuscar);
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
            this.Controls.Add(this.btnActualizar);
            this.Name = "GUIActualizarPaquete";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Actualizar Paquete";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.Label lblCodigo;
        private System.Windows.Forms.TextBox txtCodigo;
        private System.Windows.Forms.Button btnBuscar;
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
        private System.Windows.Forms.Button btnActualizar;
    }
}
