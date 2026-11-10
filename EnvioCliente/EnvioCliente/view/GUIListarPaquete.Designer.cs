namespace EnvioCliente.view
{
    partial class GUIListarPaquete
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
            this.lblEnvioCodigo = new System.Windows.Forms.Label();
            this.txtEnvioCodigo = new System.Windows.Forms.TextBox();
            this.lblDescripcion = new System.Windows.Forms.Label();
            this.txtDescripcion = new System.Windows.Forms.TextBox();
            this.btnFiltrar = new System.Windows.Forms.Button();
            this.btnListarTodos = new System.Windows.Forms.Button();
            this.dgvPaquetes = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPaquetes)).BeginInit();
            this.SuspendLayout();
            this.lblEnvioCodigo.Location = new System.Drawing.Point(20, 20);
            this.lblEnvioCodigo.Name = "lblEnvioCodigo";
            this.lblEnvioCodigo.Size = new System.Drawing.Size(70, 20);
            this.lblEnvioCodigo.Text = "Cod. envio:";
            this.txtEnvioCodigo.Location = new System.Drawing.Point(95, 17);
            this.txtEnvioCodigo.Name = "txtEnvioCodigo";
            this.txtEnvioCodigo.Size = new System.Drawing.Size(130, 23);
            this.lblDescripcion.Location = new System.Drawing.Point(240, 20);
            this.lblDescripcion.Name = "lblDescripcion";
            this.lblDescripcion.Size = new System.Drawing.Size(80, 20);
            this.lblDescripcion.Text = "Descripcion:";
            this.txtDescripcion.Location = new System.Drawing.Point(325, 17);
            this.txtDescripcion.Name = "txtDescripcion";
            this.txtDescripcion.Size = new System.Drawing.Size(190, 23);
            this.btnFiltrar.Location = new System.Drawing.Point(530, 15);
            this.btnFiltrar.Name = "btnFiltrar";
            this.btnFiltrar.Size = new System.Drawing.Size(90, 25);
            this.btnFiltrar.Text = "Filtrar";
            this.btnFiltrar.UseVisualStyleBackColor = true;
            this.btnFiltrar.Click += new System.EventHandler(this.btnFiltrar_Click);
            this.btnListarTodos.Location = new System.Drawing.Point(630, 15);
            this.btnListarTodos.Name = "btnListarTodos";
            this.btnListarTodos.Size = new System.Drawing.Size(110, 25);
            this.btnListarTodos.Text = "Listar todos";
            this.btnListarTodos.UseVisualStyleBackColor = true;
            this.btnListarTodos.Click += new System.EventHandler(this.btnListarTodos_Click);
            this.dgvPaquetes.Location = new System.Drawing.Point(20, 55);
            this.dgvPaquetes.Name = "dgvPaquetes";
            this.dgvPaquetes.Size = new System.Drawing.Size(720, 350);
            this.dgvPaquetes.AllowUserToAddRows = false;
            this.dgvPaquetes.AllowUserToDeleteRows = false;
            this.dgvPaquetes.ReadOnly = true;
            this.dgvPaquetes.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(760, 430);
            this.Controls.Add(this.lblEnvioCodigo);
            this.Controls.Add(this.txtEnvioCodigo);
            this.Controls.Add(this.lblDescripcion);
            this.Controls.Add(this.txtDescripcion);
            this.Controls.Add(this.btnFiltrar);
            this.Controls.Add(this.btnListarTodos);
            this.Controls.Add(this.dgvPaquetes);
            this.Name = "GUIListarPaquete";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Listar Paquetes";
            this.Load += new System.EventHandler(this.GUIListarPaquete_Load);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.GUIListarPaquete_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPaquetes)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.Label lblEnvioCodigo;
        private System.Windows.Forms.TextBox txtEnvioCodigo;
        private System.Windows.Forms.Label lblDescripcion;
        private System.Windows.Forms.TextBox txtDescripcion;
        private System.Windows.Forms.Button btnFiltrar;
        private System.Windows.Forms.Button btnListarTodos;
        private System.Windows.Forms.DataGridView dgvPaquetes;
    }
}
