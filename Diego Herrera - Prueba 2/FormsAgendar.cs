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

    public partial class FormsAgendar : Form
    {
        private string rolGuardado;
        private string rutGuardado;
        private int idCitaSeleccionada = 0;
        public FormsAgendar(string rolDelUsuario, string rutDelUsuario)
        {
            InitializeComponent();
            rolGuardado = rolDelUsuario;
            rutGuardado = rutDelUsuario;
        }


        private void FormsAgendar_Load(object sender, EventArgs e)
        {
            Actualizar_Datos();
            Limpiar_Datos();
        }
        private void Actualizar_Datos()
        {
            using (VeterinariaEntities bd = new VeterinariaEntities())
            {

                var queryMascotas = from miMascota in bd.Mascota
                                    select new
                                    {
                                        miMascota.ID_Mascota,
                                        miMascota.Rut_Dueño,
                                        miMascota.Estado_Mascota,
                                        miMascota.Nombre,
                                        miMascota.tipo
                                    };
                dataGridView1.DataSource = queryMascotas.ToList();
                dataGridView1.Refresh();


                var queryAgenda = from miCita in bd.Agenda
                                  select new
                                  {
                                      miCita.ID_Cita,
                                      Fecha = miCita.fecha,
                                      Hora = miCita.hora,
                                      miCita.Rut_Usuario,


                                      Nombre_Mascota = miCita.Mascota.Nombre
                                  };
                dataGridView2.DataSource = queryAgenda.ToList();
                dataGridView2.Refresh();
            }
        }
        private void Limpiar_Datos()
        {
            textBox1.Text = string.Empty;
            dateTimePicker1.Value = DateTime.Now;
            dateTimePicker2.Value = DateTime.Now;
            idCitaSeleccionada = 0;


        }

        private void button4_Click(object sender, EventArgs e)
        {
            Form1 menuPrincipal = new Form1(rolGuardado, rutGuardado);
            menuPrincipal.Show();
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != string.Empty)
            {
                Agenda nuevaCita = new Agenda();
                nuevaCita.ID_Mascota = int.Parse(textBox1.Text);
                nuevaCita.Rut_Usuario = rutGuardado;
                nuevaCita.fecha = dateTimePicker1.Value.Date;
                nuevaCita.hora = dateTimePicker2.Value.TimeOfDay;

                try
                {
                    using (VeterinariaEntities bd = new VeterinariaEntities())
                    {
                        bd.Agenda.Add(nuevaCita);
                        bd.SaveChanges();

                        Actualizar_Datos();
                        Limpiar_Datos();
                        MessageBox.Show("Cita agendada correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ocurrió un error al guardar: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Debe seleccionar una mascota de la tabla superior para agendar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void dataGridView1_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            DataGridViewRow fila = dataGridView1.Rows[e.RowIndex];
            textBox1.Text = fila.Cells["ID_Mascota"].Value.ToString();
        }

        private void dataGridView2_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            DataGridViewRow fila = dataGridView2.Rows[e.RowIndex];
            idCitaSeleccionada = Convert.ToInt32(fila.Cells["ID_Cita"].Value);
            dateTimePicker1.Value = Convert.ToDateTime(fila.Cells["fecha"].Value);

            TimeSpan horaSql = (TimeSpan)fila.Cells["hora"].Value;
            dateTimePicker2.Value = DateTime.Today.Add(horaSql);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            {
                if (idCitaSeleccionada != 0)
                {
                    using (VeterinariaEntities bd = new VeterinariaEntities())
                    {
                        var citaAEliminar = bd.Agenda.Find(idCitaSeleccionada);
                        if (citaAEliminar != null)
                        {
                            bd.Agenda.Remove(citaAEliminar);
                            bd.SaveChanges();

                            Actualizar_Datos();
                            Limpiar_Datos();
                            MessageBox.Show("Cita eliminada correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Selecciona una cita de la tabla de Agenda para eliminar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (idCitaSeleccionada != 0)
            {
                using (VeterinariaEntities bd = new VeterinariaEntities())
                {
                    var citaAModificar = bd.Agenda.Find(idCitaSeleccionada);
                    if (citaAModificar != null)
                    {
                        citaAModificar.fecha = dateTimePicker1.Value.Date;
                        citaAModificar.hora = dateTimePicker2.Value.TimeOfDay;

                        bd.SaveChanges();
                        Actualizar_Datos();
                        Limpiar_Datos();
                        MessageBox.Show("Cita modificada correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            else
            {
                MessageBox.Show("Selecciona una cita de la tabla de Agenda para modificar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
    }
}
