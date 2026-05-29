using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;


namespace Diego_Herrera___Prueba_2
{
    public partial class Form1 : Form
    {
        string rolActual = "";
        public Form1(string rolDelLogin)
        {
            InitializeComponent();
            rolActual = rolDelLogin;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            /* 
            Actualizar_Datos();
            Llenar_Categoria();
            Llenar_Marca();
            Llenar_Proveedor();
            */
        }

        private void Actualizar_Datos()
        {
            /*
            using (PruebaEntities bd = new PruebaEntities())
            {
                var query = from miProducto in bd.Producto
                            select new
                            {
                                miProducto.id_producto,
                                miProducto.codigo,
                                miProducto.nombre,
                                miProducto.precio,
                                miProducto.stock,
                                categoria= miProducto.Categoria.descripcion,
                                marca= miProducto.Marca.descripcion,                     
                                proveedor = miProducto.Proveedor.razon_social,
                            };
                dataGridView1.DataSource = query.ToList();
                dataGridView1.Refresh();
            }
            */
        }

        private void Llenar_Categoria()
        {
            /*
            using (PruebaEntities bd = new PruebaEntities())
            {
                CbCategoria.DataSource = bd.Categoria.ToList();
                CbCategoria.DisplayMember = "descripcion";
                CbCategoria.ValueMember = "id_categoria"; 
                CbCategoria.SelectedIndex = -1; 
            }
            */
        }

        private void Llenar_Marca()
        {
            /*
            using (PruebaEntities bd = new PruebaEntities())
            {
                CbMarca.DataSource = bd.Marca.ToList();
                CbMarca.DisplayMember = "descripcion";
                CbMarca.ValueMember = "id_marca"; 
                CbMarca.SelectedIndex = -1; 
            }
            */
        }

        private void Llenar_Proveedor()
        {
            /*
            using (PruebaEntities bd = new PruebaEntities())
            {
                CbProveedor.DataSource = bd.Proveedor.ToList();
                CbProveedor.DisplayMember = "razon_social";
                CbProveedor.ValueMember = "id_proveedor"; 
                CbProveedor.SelectedIndex = -1; 
            }
            */
        }

        private void Limpiar_Datos()
        {
            /*
            textID.Text = string.Empty;
            textCodigo.Text = string.Empty;
            textNombre.Text = string.Empty;
            textPrecio.Text = string.Empty;
            textStock.Text = string.Empty;
            CbCategoria.SelectedIndex = -1;
            CbMarca.SelectedIndex = -1;
            CbProveedor.SelectedIndex = -1;

            textID.Enabled = true;
            */
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Dueños formDueño = new Dueños(rolActual);
            formDueño.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            IngresoMascota formMascota = new IngresoMascota(rolActual);
            formMascota.Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Producto formProducto = new Producto(rolActual);
            formProducto.Show();
            this.Hide();
        }

        private void dataGridView1_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            /*
            DataGridViewRow Fila = dataGridView1.Rows[e.RowIndex];
            textID.Text = Fila.Cells["id_producto"].Value.ToString();
            // ... resto del código
            */
        }

        private void añadirNuevaMascotaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IngresoMascota ventana = new IngresoMascota(rolActual);
            ventana.ShowDialog();
        }

        private void añadirDueñoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Dueños ventana = new Dueños(rolActual);
            ventana.ShowDialog();
        }

        private void mascotasToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void añadirProductoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Producto ventana = new Producto(rolActual);
            ventana.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (rolActual == "Administrador")
            {
                Usuarioscs formUsuario = new Usuarioscs(rolActual);
                formUsuario.Show();
                this.Hide();
            }
            else
            {

                MessageBox.Show("No tienes permisos de Administrador para acceder a esta seccion.", "Acceso Denegado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
