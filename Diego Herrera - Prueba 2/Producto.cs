using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Diego_Herrera___Prueba_2
{
    public partial class Producto : Form
    {
        private string rolGuardado;
        public Producto(string rolDelUsuario)
        {
            InitializeComponent();
            rolGuardado = rolDelUsuario;
        }

        private void Producto_Load(object sender, EventArgs e)
        {
            Actualizar_Datos();
            Limpiar_Datos();
            Ingresar_Estado();
        }
        private void Actualizar_Datos()
        {
            using (VeterinariaEntities bd = new VeterinariaEntities())
            {
                var query = from miProducto in bd.Productos
                            select new
                            {
                                miProducto.ID_Producto,
                                miProducto.Estado_Producto,
                                miProducto.Nombre,
                                miProducto.Stock,
                                miProducto.Precio_Unidad,


                            };
                dataGridView1.DataSource = query.ToList();
                dataGridView1.Refresh();
            }

        }
        private void Ingresar_Estado()
        {

            var opcionesEstado = new[] {
                new { Id = "Disponible", Nombre = "Disponible" },
                new { Id = "Sin Stock", Nombre = "Sin stock" },

    };


            comboBox1.DataSource = opcionesEstado.ToList();
            comboBox1.DisplayMember = "Nombre";
            comboBox1.ValueMember = "Id";
            comboBox1.SelectedIndex = -1;
        }
        private void Limpiar_Datos()
        {
            textBox1.Text = string.Empty;
            textBox2.Text = string.Empty;
            textBox3.Text = string.Empty;
            textBox4.Text = string.Empty;
            comboBox1.SelectedIndex = -1;


        }

        private void button1_Click(object sender, EventArgs e)
        {
            registrarProducto();
            
        }
        private void registrarProducto()
        {
            if (textBox1.Text != string.Empty &&
                textBox2.Text != string.Empty &&
                textBox3.Text != string.Empty &&
                textBox4.Text != string.Empty &&
                comboBox1.Text != string.Empty
                )
            {
                Productos nuevoProducto = new Productos();

                nuevoProducto.ID_Producto = int.Parse(textBox1.Text);
                nuevoProducto.Estado_Producto = comboBox1.SelectedValue.ToString();
                nuevoProducto.Nombre = textBox2.Text;
                nuevoProducto.Stock = int.Parse(textBox3.Text);
                nuevoProducto.Precio_Unidad = int.Parse(textBox4.Text);



                using (VeterinariaEntities bd = new VeterinariaEntities())
                {
                    bd.Productos.Add(nuevoProducto);
                    bd.SaveChanges();

                    Actualizar_Datos();
                    Limpiar_Datos();
                }
            }
            else
            {
                MessageBox.Show("Debe ingresar los datos a guardar.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            borrar();
        }
        private void borrar()
        {
            if (textBox1.Text != string.Empty)
            {
                using (VeterinariaEntities bd = new VeterinariaEntities())
                {
                    var nuevoProducto = bd.Productos.Find(textBox1.Text);
                    if (nuevoProducto != null)
                    {
                        bd.Productos.Remove(nuevoProducto);
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
        }


        private void button3_Click(object sender, EventArgs e)
        {
            sobreescribir();
            
        }
        private void sobreescribir()
        {
            if (textBox1.Text != string.Empty)
            {
                using (VeterinariaEntities bd = new VeterinariaEntities())
                {
                    var nuevoProducto = bd.Productos.Find(int.Parse(textBox1.Text));
                    if (nuevoProducto != null)
                    {
                        nuevoProducto.ID_Producto = int.Parse(textBox1.Text);
                        nuevoProducto.Estado_Producto = comboBox1.Text;
                        nuevoProducto.Nombre = textBox2.Text;
                        nuevoProducto.Stock = int.Parse(textBox3.Text);
                        nuevoProducto.Precio_Unidad = int.Parse(textBox4.Text);
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
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView1_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            DataGridViewRow Fila = dataGridView1.Rows[e.RowIndex];
            textBox1.Text = Fila.Cells["ID_Producto"].Value.ToString();
            comboBox1.Text = Fila.Cells["Estado_Producto"].Value.ToString();
            textBox2.Text = Fila.Cells["Nombre"].Value.ToString();
            textBox3.Text = Fila.Cells["Stock"].Value.ToString();
            textBox4.Text = Fila.Cells["Precio_Unidad"].Value.ToString();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Form1 menuPrincipal = new Form1(rolGuardado);
            menuPrincipal.Show();
            this.Close();
        }
    }
}
