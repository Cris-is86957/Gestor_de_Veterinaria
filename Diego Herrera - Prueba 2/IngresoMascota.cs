using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace Diego_Herrera___Prueba_2
{
    public partial class IngresoMascota : Form
    {
        private string rolGuardado = "";
        public IngresoMascota(string rolDelUsuario)
        {
            InitializeComponent();
            rolGuardado = rolDelUsuario;
        }

        private void IngresoMascota_Load(object sender, EventArgs e)
        {
            Actualizar_Datos();
            Llenar_EstadoMascota();

            Limpiar_Datos();
        }
        private void Actualizar_Datos()
        {
            using (VeterinariaEntities bd = new VeterinariaEntities())
            {
                var query = from miMascota in bd.Mascota
                            select new
                            {
                                miMascota.ID_Mascota,
                                miMascota.Rut_Dueño,
                                miMascota.Estado_Mascota,
                                miMascota.Nombre,
                                miMascota.tipo,
                                miMascota.raza,
                                miMascota.edad,
                               
                            };
                dataGridView1.DataSource = query.ToList();
                dataGridView1.Refresh();
                var queryDueños = from miDueño in bd.Dueño
                                  select new
                                  {
                                      miDueño.Rut_dueño,
                                      miDueño.nombre
                                  };

                dataGridView2.DataSource = queryDueños.ToList();
                dataGridView2.Refresh();
            }
        }

        
        private void Llenar_EstadoMascota()
        {

            var opcionesEstado = new[] {
                new { Id = "Sano", Nombre = "Sano" },
                new { Id = "En Tratamiento", Nombre = "En Tratamiento" },
                new { Id = "Hospitalizado", Nombre = "Hospitalizado" },
                new { Id = "Crítico", Nombre = "Crítico" }
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
            textBox5.Text = string.Empty;
            textBox6.Text = string.Empty;
            comboBox1.SelectedIndex = -1;
            
            

            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            registrarMascota();
            
        }
        private void registrarMascota()
        {
            if (textBox1.Text != string.Empty &&
                textBox2.Text != string.Empty &&
                textBox3.Text != string.Empty &&
                textBox4.Text != string.Empty &&
                textBox5.Text != string.Empty &&
                comboBox1.Text != string.Empty &&
                textBox6.Text != string.Empty
               )
            {
                Mascota nuevoMascota = new Mascota();
                nuevoMascota.ID_Mascota = int.Parse(textBox1.Text);
                nuevoMascota.Rut_Dueño = textBox2.Text;
                nuevoMascota.Nombre = textBox3.Text;
                nuevoMascota.raza = textBox4.Text;
                nuevoMascota.edad = int.Parse(textBox5.Text);
                nuevoMascota.Estado_Mascota = comboBox1.SelectedValue.ToString();
                nuevoMascota.tipo = textBox6.Text;


                using (VeterinariaEntities bd = new VeterinariaEntities())
                {
                    bd.Mascota.Add(nuevoMascota);
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
                    var nuevoMascota = bd.Mascota.Find(int.Parse(textBox1.Text));
                    if (nuevoMascota != null)
                    {
                        bd.Mascota.Remove(nuevoMascota);
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
            if (comboBox1.Text != string.Empty)
            {
                using (VeterinariaEntities bd = new VeterinariaEntities())
                {
                    var nuevoMascota = bd.Mascota.Find(int.Parse(textBox1.Text));
                    if (nuevoMascota != null)
                    {
                        nuevoMascota.ID_Mascota = int.Parse(textBox1.Text);
                        nuevoMascota.Rut_Dueño = textBox2.Text;
                        nuevoMascota.Estado_Mascota = comboBox1.SelectedValue.ToString();
                        nuevoMascota.Nombre = textBox3.Text;
                        nuevoMascota.tipo = textBox6.Text;
                        nuevoMascota.raza = textBox4.Text;
                        nuevoMascota.edad = int.Parse(textBox5.Text);
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
            textBox1.Text = Fila.Cells["ID_Mascota"].Value.ToString();
            textBox2.Text = Fila.Cells["Rut_Dueño"].Value.ToString();
            textBox3.Text = Fila.Cells["Nombre"].Value.ToString();
            textBox4.Text = Fila.Cells["raza"].Value.ToString();
            textBox5.Text = Fila.Cells["edad"].Value.ToString();
            comboBox1.Text = Fila.Cells["Estado_Mascota"].Value.ToString();
            textBox6.Text = Fila.Cells["tipo"].Value.ToString();
            

            
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Form1 menuPrincipal = new Form1(rolGuardado);
            menuPrincipal.Show();
            this.Close();
        }
    }
    }

