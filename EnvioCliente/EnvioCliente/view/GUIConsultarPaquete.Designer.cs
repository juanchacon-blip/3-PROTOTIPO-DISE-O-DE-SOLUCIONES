namespace EnvioCliente.view
{
    partial class GUIConsultarPaquete
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
            this.lblAyuda = new System.Windows.Forms.Label();
            this.btnConsultar = new System.Windows.Forms.Button();
            this.btnLimpiar = new System.Windows.Forms.Button();
            this.lblResCodigo = new System.Windows.Forms.Label();
            this.txtResCodigo = new System.Windows.Forms.TextBox();
            this.lblResEnvioCodigo = new System.Windows.Forms.Label();
            this.txtResEnvioCodigo = new System.Windows.Forms.TextBox();
            this.lblResDescripcion = new System.Windows.Forms.Label();
            this.txtResDescripcion = new System.Windows.Forms.TextBox();
            this.lblResPesoKg = new System.Windows.Forms.Label();
            this.txtResPesoKg = new System.Windows.Forms.TextBox();
            this.lblResUnidades = new System.Windows.Forms.Label();
            this.txtResUnidades = new System.Windows.Forms.TextBox();
            this.lblResFecha = new System.Windows.Forms.Label();
            this.txtResFecha = new System.Windows.Forms.TextBox();
            this.lblResFragil = new System.Windows.Forms.Label();
            this.txtResFragil = new System.Windows.Forms.TextBox();
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
            this.lblAyuda.Location = new System.Drawing.Point(20, 120);
            this.lblAyuda.Name = "lblAyuda";
            this.lblAyuda.Size = new System.Drawing.Size(330, 20);
            this.lblAyuda.Text = "Diligencie uno o dos criterios.";
            this.btnConsultar.Location = new System.Drawing.Point(170, 145);
            this.btnConsultar.Name = "btnConsultar";
            this.btnConsultar.Size = new System.Drawing.Size(100, 30);
            this.btnConsultar.Text = "Consultar";
            this.btnConsultar.UseVisualStyleBackColor = true;
            this.btnConsultar.Click += new System.EventHandler(this.btnConsultar_Click);
            this.btnLimpiar.Location = new System.Drawing.Point(280, 145);
            this.btnLimpiar.Name = "btnLimpiar";
            this.btnLimpiar.Size = new System.Drawing.Size(100, 30);
            this.btnLimpiar.Text = "Limpiar";
            this.btnLimpiar.UseVisualStyleBackColor = true;
            this.btnLimpiar.Click += new System.EventHandler(this.btnLimpiar_Click);
            this.lblResCodigo.Location = new System.Drawing.Point(20, 195);
            this.lblResCodigo.Name = "lblResCodigo";
            this.lblResCodigo.Size = new System.Drawing.Size(140, 20);
            this.lblResCodigo.Text = "Codigo:";
            this.txtResCodigo.Location = new System.Drawing.Point(170, 192);
            this.txtResCodigo.Name = "txtResCodigo";
            this.txtResCodigo.Size = new System.Drawing.Size(180, 23);
            this.txtResCodigo.ReadOnly = true;
            this.lblResEnvioCodigo.Location = new System.Drawing.Point(20, 230);
            this.lblResEnvioCodigo.Name = "lblResEnvioCodigo";
            this.lblResEnvioCodigo.Size = new System.Drawing.Size(140, 20);
            this.lblResEnvioCodigo.Text = "Codigo del envio:";
            this.txtResEnvioCodigo.Location = new System.Drawing.Point(170, 227);
            this.txtResEnvioCodigo.Name = "txtResEnvioCodigo";
            this.txtResEnvioCodigo.Size = new System.Drawing.Size(180, 23);
            this.txtResEnvioCodigo.ReadOnly = true;
            this.lblResDescripcion.Location = new System.Drawing.Point(20, 265);
            this.lblResDescripcion.Name = "lblResDescripcion";
            this.lblResDescripcion.Size = new System.Drawing.Size(140, 20);
            this.lblResDescripcion.Text = "Descripcion:";
            this.txtResDescripcion.Location = new System.Drawing.Point(170, 262);
            this.txtResDescripcion.Name = "txtResDescripcion";
            this.txtResDescripcion.Size = new System.Drawing.Size(180, 23);
            this.txtResDescripcion.ReadOnly = true;
            this.lblResPesoKg.Location = new System.Drawing.Point(20, 300);
            this.lblResPesoKg.Name = "lblResPesoKg";
            this.lblResPesoKg.Size = new System.Drawing.Size(140, 20);
            this.lblResPesoKg.Text = "Peso (kg):";
            this.txtResPesoKg.Location = new System.Drawing.Point(170, 297);
            this.txtResPesoKg.Name = "txtResPesoKg";
            this.txtResPesoKg.Size = new System.Drawing.Size(180, 23);
            this.txtResPesoKg.ReadOnly = true;
            this.lblResUnidades.Location = new System.Drawing.Point(20, 335);
            this.lblResUnidades.Name = "lblResUnidades";
            this.lblResUnidades.Size = new System.Drawing.Size(140, 20);
            this.lblResUnidades.Text = "Unidades:";
            this.txtResUnidades.Location = new System.Drawing.Point(170, 332);
            this.txtResUnidades.Name = "txtResUnidades";
            this.txtResUnidades.Size = new System.Drawing.Size(180, 23);
            this.txtResUnidades.ReadOnly = true;
            this.lblResFecha.Location = new System.Drawing.Point(20, 370);
            this.lblResFecha.Name = "lblResFecha";
            this.lblResFecha.Size = new System.Drawing.Size(140, 20);
            this.lblResFecha.Text = "Fecha de registro:";
            this.txtResFecha.Location = new System.Drawing.Point(170, 367);
            this.txtResFecha.Name = "txtResFecha";
            this.txtResFecha.Size = new System.Drawing.Size(180, 23);
            this.txtResFecha.ReadOnly = true;
            this.lblResFragil.Location = new System.Drawing.Point(20, 405);
            this.lblResFragil.Name = "lblResFragil";
            this.lblResFragil.Size = new System.Drawing.Size(140, 20);
            this.lblResFragil.Text = "Fragil:";
            this.txtResFragil.Location = new System.Drawing.Point(170, 402);
            this.txtResFragil.Name = "txtResFragil";
            this.txtResFragil.Size = new System.Drawing.Size(180, 23);
            this.txtResFragil.ReadOnly = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(390, 460);
            this.Controls.Add(this.lblCodigo);
            this.Controls.Add(this.txtCodigo);
            this.Controls.Add(this.lblEnvioCodigo);
            this.Controls.Add(this.txtEnvioCodigo);
            this.Controls.Add(this.lblDescripcion);
            this.Controls.Add(this.txtDescripcion);
            this.Controls.Add(this.lblAyuda);
            this.Controls.Add(this.btnConsultar);
            this.Controls.Add(this.btnLimpiar);
            this.Controls.Add(this.lblResCodigo);
            this.Controls.Add(this.txtResCodigo);
            this.Controls.Add(this.lblResEnvioCodigo);
            this.Controls.Add(this.txtResEnvioCodigo);
            this.Controls.Add(this.lblResDescripcion);
            this.Controls.Add(this.txtResDescripcion);
            this.Controls.Add(this.lblResPesoKg);
            this.Controls.Add(this.txtResPesoKg);
            this.Controls.Add(this.lblResUnidades);
            this.Controls.Add(this.txtResUnidades);
            this.Controls.Add(this.lblResFecha);
            this.Controls.Add(this.txtResFecha);
            this.Controls.Add(this.lblResFragil);
            this.Controls.Add(this.txtResFragil);
            this.Name = "GUIConsultarPaquete";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Consultar Paquete";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.Label lblCodigo;
        private System.Windows.Forms.TextBox txtCodigo;
        private System.Windows.Forms.Label lblEnvioCodigo;
        private System.Windows.Forms.TextBox txtEnvioCodigo;
        private System.Windows.Forms.Label lblDescripcion;
        private System.Windows.Forms.TextBox txtDescripcion;
        private System.Windows.Forms.Label lblAyuda;
        private System.Windows.Forms.Button btnConsultar;
        private System.Windows.Forms.Button btnLimpiar;
        private System.Windows.Forms.Label lblResCodigo;
        private System.Windows.Forms.TextBox txtResCodigo;
        private System.Windows.Forms.Label lblResEnvioCodigo;
        private System.Windows.Forms.TextBox txtResEnvioCodigo;
        private System.Windows.Forms.Label lblResDescripcion;
        private System.Windows.Forms.TextBox txtResDescripcion;
        private System.Windows.Forms.Label lblResPesoKg;
        private System.Windows.Forms.TextBox txtResPesoKg;
        private System.Windows.Forms.Label lblResUnidades;
        private System.Windows.Forms.TextBox txtResUnidades;
        private System.Windows.Forms.Label lblResFecha;
        private System.Windows.Forms.TextBox txtResFecha;
        private System.Windows.Forms.Label lblResFragil;
        private System.Windows.Forms.TextBox txtResFragil;
    }
}
