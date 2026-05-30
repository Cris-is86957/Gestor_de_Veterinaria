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
    public partial class Usuarioscs : Form
    {
        private string rolGuardado;

        public Usuarioscs(string rolDelUsuario)
        {
            InitializeComponent();
            rolGuardado = rolDelUsuario;
        }

        private void Usuarioscs_Load(object sender, EventArgs e)
        {
            Actualizar_Datos();
            Limpiar_Datos();
            Ingresar_Estado();
        }
        private void Actualizar_Datos()
        {
            using (VeterinariaEntities bd = new VeterinariaEntities())
            {
                var query = from miUsuario in bd.Usuario
                            select new
                            {
                                miUsuario.Rut_Usuario,
                                miUsuario.nombre,
                                miUsuario.apellido,
                                miUsuario.password,
                                miUsuario.Estado_Usuario,


                            };
                dataGridView1.DataSource = query.ToList();
                dataGridView1.Refresh();
            }

        }
        private void Limpiar_Datos()
        {
            textBox1.Text = string.Empty;
            textBox2.Text = string.Empty;
            textBox3.Text = string.Empty;
            textBox4.Text = string.Empty;
            comboBox1.SelectedIndex = -1;


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

        private void button1_Click(object sender, EventArgs e)
        {
            ingresar();

        }
        private void ingresar()
        {
            if (textBox1.Text != string.Empty &&
                textBox2.Text != string.Empty &&
                textBox3.Text != string.Empty &&
                textBox4.Text != string.Empty &&
                comboBox1.Text != string.Empty
                )
            {
                Usuario nuevoUsuario = new Usuario();

                nuevoUsuario.Rut_Usuario = textBox1.Text;
                nuevoUsuario.Estado_Usuario = comboBox1.SelectedValue.ToString();
                nuevoUsuario.nombre = textBox2.Text;
                nuevoUsuario.apellido = textBox3.Text;
                nuevoUsuario.password = textBox4.Text;



                using (VeterinariaEntities bd = new VeterinariaEntities())
                {
                    bd.Usuario.Add(nuevoUsuario);
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
            eliminar();
        }
        private void eliminar()
        {
            if (textBox1.Text != string.Empty)
            {
                using (VeterinariaEntities bd = new VeterinariaEntities())
                {
                    var nuevoUsuario = bd.Usuario.Find(textBox1.Text);
                    if (nuevoUsuario != null)
                    {
                        bd.Usuario.Remove(nuevoUsuario);
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
                    var nuevoUsuario = bd.Usuario.Find(textBox1.Text);
                    if (nuevoUsuario != null)
                    {
                        nuevoUsuario.Rut_Usuario = textBox1.Text;
                        nuevoUsuario.Estado_Usuario = comboBox1.SelectedValue.ToString();
                        nuevoUsuario.nombre = textBox2.Text;
                        nuevoUsuario.apellido = textBox3.Text;
                        nuevoUsuario.password = textBox4.Text;
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

        private void button4_Click(object sender, EventArgs e)
        {
            Form1 menuPrincipal = new Form1(rolGuardado);
            menuPrincipal.Show();
            this.Close();
        }
    }
}
