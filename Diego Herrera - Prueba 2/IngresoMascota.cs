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
    public partial class IngresoMascota : Form
    {
        public IngresoMascota()
        {
            InitializeComponent();
        }

        private void IngresoMascota_Load(object sender, EventArgs e)
        {
            Actualizar_Datos();
            Llenar_EstadoMascota();
            Llenar_Tipo();
            Limpiar_Datos();
        }

        private void Actualizar_Datos()
        {
            using (VeterinariaEntities bd = new VeterinariaEntities())
            {
                var query = from miMascota in bd.MASCOTA
                            select new
                            {
                                miMascota.ID_mascota,
                                miMascota.Rut_dueño,
                                miMascota.Estado_mascota,
                                miMascota.nombre,
                                miMascota.tipo,
                                miMascota.raza,
                                miMascota.edad,
                            };
                dataGridView1.DataSource = query.ToList();
                dataGridView1.Refresh();
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

        private void Llenar_Tipo()
        {
            var opcionesSexo = new[] {
                new { Id = 1, Nombre = "Macho" },
                new { Id = 2, Nombre = "Hembra" }
            };

            comboBox2.DataSource = opcionesSexo.ToList();
            comboBox2.DisplayMember = "Nombre";
            comboBox2.ValueMember = "Id";
            comboBox2.SelectedIndex = 0;
        }

        private void Limpiar_Datos()
        {
            textBox1.Text = string.Empty;
            textBox2.Text = string.Empty;
            textBox3.Text = string.Empty;
            textBox4.Text = string.Empty;
            textBox5.Text = string.Empty;
            comboBox1.SelectedIndex = -1;
            comboBox2.SelectedIndex = -1;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != string.Empty &&
                textBox2.Text != string.Empty &&
                textBox3.Text != string.Empty &&
                textBox4.Text != string.Empty &&
                textBox5.Text != string.Empty &&
                comboBox1.Text != string.Empty &&
                comboBox2.Text != string.Empty)
            {
                MASCOTA nuevoMascota = new MASCOTA();
                nuevoMascota.ID_mascota = int.Parse(textBox1.Text);
                nuevoMascota.Rut_dueño = textBox2.Text;
                nuevoMascota.nombre = textBox3.Text;
                nuevoMascota.raza = textBox4.Text;
                nuevoMascota.edad = int.Parse(textBox5.Text);

                // CORRECCIÓN: Estado_mascota es un bool (bit). Le pasamos true por defecto.
                nuevoMascota.Estado_mascota = true;
                // CORRECCIÓN: tipo es un texto. Guardamos el texto "Macho" o "Hembra" directo.
                nuevoMascota.tipo = comboBox2.Text;

                using (VeterinariaEntities bd = new VeterinariaEntities())
                {
                    bd.MASCOTA.Add(nuevoMascota);
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
            if (textBox1.Text != string.Empty)
            {
                using (VeterinariaEntities bd = new VeterinariaEntities())
                {
                    var nuevoMascota = bd.MASCOTA.Find(int.Parse(textBox1.Text));
                    if (nuevoMascota != null)
                    {
                        bd.MASCOTA.Remove(nuevoMascota);
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
            if (comboBox1.Text != string.Empty)
            {
                using (VeterinariaEntities bd = new VeterinariaEntities())
                {
                    var nuevoMascota = bd.MASCOTA.Find(int.Parse(textBox1.Text));
                    if (nuevoMascota != null)
                    {
                        nuevoMascota.ID_mascota = int.Parse(textBox1.Text);
                        nuevoMascota.Rut_dueño = textBox2.Text;
                        nuevoMascota.nombre = textBox3.Text;
                        nuevoMascota.raza = textBox4.Text;
                        nuevoMascota.edad = int.Parse(textBox5.Text);

                        // CORRECCIÓN: Igual que arriba
                        nuevoMascota.Estado_mascota = true;
                        nuevoMascota.tipo = comboBox2.Text;

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
            textBox1.Text = Fila.Cells["ID_mascota"].Value?.ToString();
            textBox2.Text = Fila.Cells["Rut_dueño"].Value?.ToString();
            textBox3.Text = Fila.Cells["nombre"].Value?.ToString();
            textBox4.Text = Fila.Cells["raza"].Value?.ToString();
            textBox5.Text = Fila.Cells["edad"].Value?.ToString();
            comboBox2.Text = Fila.Cells["tipo"].Value?.ToString();
        }

        private void btnVolver_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}