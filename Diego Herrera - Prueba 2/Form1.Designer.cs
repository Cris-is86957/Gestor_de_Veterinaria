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
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.textID = new System.Windows.Forms.TextBox();
            this.textCodigo = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textNombre = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textPrecio = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textStock = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.CbCategoria = new System.Windows.Forms.ComboBox();
            this.CbMarca = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.CbProveedor = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(12, 12);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(657, 150);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.RowHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridView1_RowHeaderMouseClick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(55, 180);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(69, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "id_producto: ";
            // 
            // textID
            // 
            this.textID.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.textID.Location = new System.Drawing.Point(130, 177);
            this.textID.Name = "textID";
            this.textID.Size = new System.Drawing.Size(165, 20);
            this.textID.TabIndex = 2;
            // 
            // textCodigo
            // 
            this.textCodigo.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.textCodigo.Location = new System.Drawing.Point(130, 203);
            this.textCodigo.Name = "textCodigo";
            this.textCodigo.Size = new System.Drawing.Size(165, 20);
            this.textCodigo.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(79, 206);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(45, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "código: ";
            // 
            // textNombre
            // 
            this.textNombre.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.textNombre.Location = new System.Drawing.Point(130, 229);
            this.textNombre.Name = "textNombre";
            this.textNombre.Size = new System.Drawing.Size(165, 20);
            this.textNombre.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(76, 232);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(48, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "nombre: ";
            // 
            // textPrecio
            // 
            this.textPrecio.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.textPrecio.Location = new System.Drawing.Point(130, 255);
            this.textPrecio.Name = "textPrecio";
            this.textPrecio.Size = new System.Drawing.Size(165, 20);
            this.textPrecio.TabIndex = 8;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(82, 258);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(42, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "precio: ";
            // 
            // textStock
            // 
            this.textStock.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.textStock.Location = new System.Drawing.Point(130, 281);
            this.textStock.Name = "textStock";
            this.textStock.Size = new System.Drawing.Size(165, 20);
            this.textStock.TabIndex = 10;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(85, 284);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(39, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "stock: ";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(65, 310);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(59, 13);
            this.label6.TabIndex = 11;
            this.label6.Text = "categoría: ";
            // 
            // CbCategoria
            // 
            this.CbCategoria.FormattingEnabled = true;
            this.CbCategoria.Location = new System.Drawing.Point(130, 307);
            this.CbCategoria.Name = "CbCategoria";
            this.CbCategoria.Size = new System.Drawing.Size(165, 21);
            this.CbCategoria.TabIndex = 12;
            // 
            // CbMarca
            // 
            this.CbMarca.FormattingEnabled = true;
            this.CbMarca.Location = new System.Drawing.Point(130, 334);
            this.CbMarca.Name = "CbMarca";
            this.CbMarca.Size = new System.Drawing.Size(165, 21);
            this.CbMarca.TabIndex = 14;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(82, 337);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(42, 13);
            this.label7.TabIndex = 13;
            this.label7.Text = "marca: ";
            // 
            // CbProveedor
            // 
            this.CbProveedor.FormattingEnabled = true;
            this.CbProveedor.Location = new System.Drawing.Point(130, 361);
            this.CbProveedor.Name = "CbProveedor";
            this.CbProveedor.Size = new System.Drawing.Size(165, 21);
            this.CbProveedor.TabIndex = 16;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(63, 364);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(61, 13);
            this.label8.TabIndex = 15;
            this.label8.Text = "proveedor: ";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(58, 388);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 17;
            this.button1.Text = "Ingresar";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(139, 388);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 18;
            this.button2.Text = "Modificar";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(220, 388);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 19;
            this.button3.Text = "Eliminar";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(683, 450);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.CbProveedor);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.CbMarca);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.CbCategoria);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.textStock);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.textPrecio);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.textNombre);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textCodigo);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textID);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dataGridView1);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textID;
        private System.Windows.Forms.TextBox textCodigo;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textNombre;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textPrecio;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textStock;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox CbCategoria;
        private System.Windows.Forms.ComboBox CbMarca;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox CbProveedor;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
    }
}

