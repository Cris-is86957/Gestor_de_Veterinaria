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
            /*if (textID.Text != string.Empty &&
                textCodigo.Text != string.Empty &&
                textNombre.Text != string.Empty &&
                textPrecio.Text != string.Empty &&
                textStock.Text != string.Empty &&
                CbCategoria.Text != string.Empty &&
                CbMarca.Text != string.Empty &&
                CbProveedor.Text != string.Empty)
            {
                Producto nuevoProducto = new Producto();
                nuevoProducto.id_producto = int.Parse(textID.Text);
                nuevoProducto.codigo = textCodigo.Text;
                nuevoProducto.nombre = textNombre.Text;
                nuevoProducto.precio = int.Parse(textPrecio.Text);
                nuevoProducto.stock = int.Parse(textStock.Text);
                nuevoProducto.id_categoria = int.Parse(CbCategoria.SelectedValue.ToString());
                nuevoProducto.id_marca = int.Parse(CbMarca.SelectedValue.ToString());
                nuevoProducto.id_proveedor = int.Parse(CbProveedor.SelectedValue.ToString());

                using (PruebaEntities bd = new PruebaEntities())
                {
                    bd.Producto.Add(nuevoProducto);
                    bd.SaveChanges();

                    Actualizar_Datos();
                    Limpiar_Datos();
                }
            }
            else
            {
                MessageBox.Show("Debe ingresar los datos a guardar.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            */
        }

        private void button2_Click(object sender, EventArgs e)
        {
            /*if (textID.Text != string.Empty)
            {
                using (PruebaEntities bd = new PruebaEntities())
                {
                    var nuevoProducto = bd.Producto.Find(int.Parse(textID.Text));
                    if (nuevoProducto != null)
                    {
                        nuevoProducto.codigo = textCodigo.Text;
                        nuevoProducto.nombre = textNombre.Text;
                        nuevoProducto.precio = int.Parse(textPrecio.Text);
                        nuevoProducto.stock = int.Parse(textStock.Text);
                        nuevoProducto.id_categoria = int.Parse(CbCategoria.SelectedValue.ToString());
                        nuevoProducto.id_marca = int.Parse(CbMarca.SelectedValue.ToString());
                        nuevoProducto.id_proveedor = int.Parse(CbProveedor.SelectedValue.ToString());                       
                        bd.SaveChanges();

                        Actualizar_Datos();
                        Limpiar_Datos();
                    }
                    
                }
            }
            else
            {
                MessageBox.Show("Debe seleccionar una fila para modificar.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            */
        }

        private void button3_Click(object sender, EventArgs e)
        {
            /*if (textID.Text != string.Empty)
            {
                using (PruebaEntities bd = new PruebaEntities())
                {
                    var nuevoProducto = bd.Producto.Find(int.Parse(textID.Text));
                    if (nuevoProducto != null)
                    {
                        bd.Producto.Remove(nuevoProducto);
                        bd.SaveChanges();

                        Actualizar_Datos();
                        Limpiar_Datos();
                    }

                }
            }
            else
            {
                MessageBox.Show("Se debe seleccionar una fila para eliminar.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            */
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
            IngresoMascota ventana = new IngresoMascota();
            ventana.ShowDialog();
        }
    }
}
