namespace EnvioCliente.view
{
    partial class GUIListarEnvioNacional
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
            this.lblOrigen = new System.Windows.Forms.Label();
            this.txtOrigen = new System.Windows.Forms.TextBox();
            this.lblDepartamentoDestino = new System.Windows.Forms.Label();
            this.txtDepartamentoDestino = new System.Windows.Forms.TextBox();
            this.btnFiltrar = new System.Windows.Forms.Button();
            this.btnListarTodos = new System.Windows.Forms.Button();
            this.dgvEnvios = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dgvEnvios)).BeginInit();
            this.SuspendLayout();
            this.lblOrigen.Location = new System.Drawing.Point(20, 20);
            this.lblOrigen.Name = "lblOrigen";
            this.lblOrigen.Size = new System.Drawing.Size(60, 20);
            this.lblOrigen.Text = "Origen:";
            this.txtOrigen.Location = new System.Drawing.Point(85, 17);
            this.txtOrigen.Name = "txtOrigen";
            this.txtOrigen.Size = new System.Drawing.Size(140, 23);
            this.lblDepartamentoDestino.Location = new System.Drawing.Point(240, 20);
            this.lblDepartamentoDestino.Name = "lblDepartamentoDestino";
            this.lblDepartamentoDestino.Size = new System.Drawing.Size(130, 20);
            this.lblDepartamentoDestino.Text = "Depto. destino:";
            this.txtDepartamentoDestino.Location = new System.Drawing.Point(375, 17);
            this.txtDepartamentoDestino.Name = "txtDepartamentoDestino";
            this.txtDepartamentoDestino.Size = new System.Drawing.Size(140, 23);
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
            this.dgvEnvios.Location = new System.Drawing.Point(20, 55);
            this.dgvEnvios.Name = "dgvEnvios";
            this.dgvEnvios.Size = new System.Drawing.Size(720, 350);
            this.dgvEnvios.AllowUserToAddRows = false;
            this.dgvEnvios.AllowUserToDeleteRows = false;
            this.dgvEnvios.ReadOnly = true;
            this.dgvEnvios.AutoSizeColumnsMode =
                System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(760, 430);
            this.Controls.Add(this.lblOrigen);
            this.Controls.Add(this.txtOrigen);
            this.Controls.Add(this.lblDepartamentoDestino);
            this.Controls.Add(this.txtDepartamentoDestino);
            this.Controls.Add(this.btnFiltrar);
            this.Controls.Add(this.btnListarTodos);
            this.Controls.Add(this.dgvEnvios);
            this.Name = "GUIListarEnvioNacional";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Listar Envios Nacionales";
            this.Load += new System.EventHandler(this.GUIListarEnvioNacional_Load);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.GUIListarEnvioNacional_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.dgvEnvios)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.Label lblOrigen;
        private System.Windows.Forms.TextBox txtOrigen;
        private System.Windows.Forms.Label lblDepartamentoDestino;
        private System.Windows.Forms.TextBox txtDepartamentoDestino;
        private System.Windows.Forms.Button btnFiltrar;
        private System.Windows.Forms.Button btnListarTodos;
        private System.Windows.Forms.DataGridView dgvEnvios;
    }
}
