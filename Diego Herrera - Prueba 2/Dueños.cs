using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Diego_Herrera___Prueba_2
{
    public partial class Dueños : Form
    {
        private string rolGuardado = "";
        public Dueños(string rolDelUsuario)
        {
            InitializeComponent();
            rolGuardado = rolDelUsuario;
        }

        private void Dueños_Load(object sender, EventArgs e)
        {
            Actualizar_Datos();
            Limpiar_Datos();
            Ingresar_Estado();
        }
        private void Actualizar_Datos()
        {
            using (VeterinariaEntities bd = new VeterinariaEntities())
            {
                var query = from miDueño in bd.Dueño
                            select new
                            {
                                miDueño.Rut_dueño,
                                miDueño.Estado_Dueño,
                                miDueño.nombre,
                                miDueño.apell_pat,
                                miDueño.apell_mat,
                                

                            };
                dataGridView1.DataSource = query.ToList();
                dataGridView1.Refresh();
            }

        }
        private void Ingresar_Estado()
        {

            var opcionesEstado = new[] {
                new { Id = "Activo", Nombre = "Activo" },
                new { Id = "Inactivo", Nombre = "Inactivo" },
                
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
            registrarDueño();

        }
        private void registrarDueño()
        {
            if (textBox1.Text != string.Empty &&
                textBox2.Text != string.Empty &&
                textBox3.Text != string.Empty &&
                textBox4.Text != string.Empty &&
                comboBox1.Text != string.Empty
                )
            {
                Dueño nuevoDueño = new Dueño();

                nuevoDueño.Rut_dueño = textBox1.Text;
                nuevoDueño.Estado_Dueño = comboBox1.SelectedValue.ToString();
                nuevoDueño.nombre = textBox2.Text;
                nuevoDueño.apell_pat = textBox3.Text;
                nuevoDueño.apell_mat = textBox4.Text;



                using (VeterinariaEntities bd = new VeterinariaEntities())
                {
                    bd.Dueño.Add(nuevoDueño);
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
                    var nuevoDueño = bd.Dueño.Find(textBox1.Text);
                    if (nuevoDueño != null)
                    {
                        bd.Dueño.Remove(nuevoDueño);
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
                    var nuevoDueño = bd.Dueño.Find(textBox1.Text);
                    if (nuevoDueño != null)
                    {
                        nuevoDueño.Rut_dueño = textBox1.Text;
                        nuevoDueño.Estado_Dueño = comboBox1.SelectedValue.ToString();
                        nuevoDueño.nombre = textBox2.Text;
                        nuevoDueño.apell_pat = textBox3.Text;
                        nuevoDueño.apell_mat = textBox4.Text;
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

        private void dataGridView1_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            DataGridViewRow Fila = dataGridView1.Rows[e.RowIndex];
            textBox1.Text = Fila.Cells["Rut_Dueño"].Value.ToString();
            textBox2.Text = Fila.Cells["nombre"].Value.ToString();
            textBox3.Text = Fila.Cells["apell_pat"].Value.ToString();
            textBox4.Text = Fila.Cells["apell_mat"].Value.ToString();
            comboBox1.Text = Fila.Cells["Estado_Dueño"].Value.ToString();
            



        }

        private void button4_Click(object sender, EventArgs e)
        {
            Form1 menuPrincipal = new Form1(rolGuardado);
            menuPrincipal.Show();
            this.Close();
        }
    }
}
