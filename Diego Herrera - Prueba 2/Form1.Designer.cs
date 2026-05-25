namespace Diego_Herrera___Prueba_2
{
    partial class Form1
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.gestionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gestionarDueñosToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gestionarMascotaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gestionarProductosToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.gestionToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(683, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // gestionToolStripMenuItem
            // 
            this.gestionToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.gestionarDueñosToolStripMenuItem,
            this.gestionarMascotaToolStripMenuItem,
            this.gestionarProductosToolStripMenuItem});
            this.gestionToolStripMenuItem.Name = "gestionToolStripMenuItem";
            this.gestionToolStripMenuItem.Size = new System.Drawing.Size(59, 20);
            this.gestionToolStripMenuItem.Text = "Gestion";
            // 
            // gestionarDueñosToolStripMenuItem
            // 
            this.gestionarDueñosToolStripMenuItem.Name = "gestionarDueñosToolStripMenuItem";
            this.gestionarDueñosToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.gestionarDueñosToolStripMenuItem.Text = "Gestionar Dueños";
            this.gestionarDueñosToolStripMenuItem.Click += new System.EventHandler(this.gestionarDueñosToolStripMenuItem_Click);
            // 
            // gestionarMascotaToolStripMenuItem
            // 
            this.gestionarMascotaToolStripMenuItem.Name = "gestionarMascotaToolStripMenuItem";
            this.gestionarMascotaToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.gestionarMascotaToolStripMenuItem.Text = "Gestionar Mascota";
            this.gestionarMascotaToolStripMenuItem.Click += new System.EventHandler(this.gestionarMascotaToolStripMenuItem_Click);
            // 
            // gestionarProductosToolStripMenuItem
            // 
            this.gestionarProductosToolStripMenuItem.Name = "gestionarProductosToolStripMenuItem";
            this.gestionarProductosToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.gestionarProductosToolStripMenuItem.Text = "Gestionar Productos";
            this.gestionarProductosToolStripMenuItem.Click += new System.EventHandler(this.gestionarProductosToolStripMenuItem_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(683, 450);
            this.Controls.Add(this.menuStrip1);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem gestionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem gestionarDueñosToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem gestionarMascotaToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem gestionarProductosToolStripMenuItem;
    }
}

