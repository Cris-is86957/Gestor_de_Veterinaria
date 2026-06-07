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
    public partial class Agendas : Form
    {
        private string rolGuardado = "";
        private string rutGuardado = "";
        public Agendas(string rut, string rol)
        {
            InitializeComponent();
            rutGuardado = rut;
            rolGuardado = rol;
        }

        private void Agendas_Load(object sender, EventArgs e)
        {
            Actualizar_Datos();
        }
        private void Actualizar_Datos()
        {
            using (VeterinariaEntities bd = new VeterinariaEntities())
            {
                var queryDueños = from dueno in bd.Dueño
                                  select new
                                  {
                                      dueno.Rut_dueño, // El nombre exacto que tenga en tu SQL
                                      dueno.nombre,
                                      dueno.apell_pat
                                  };
                dataGridView1.DataSource = queryDueños.ToList();

                var queryMascotas = from mascota in bd.Mascota
                                    select new
                                    {
                                        mascota.ID_Mascota,
                                        mascota.Nombre,
                                        mascota.Rut_Dueño
                                    };
                dataGridView2.DataSource = queryMascotas.ToList();

                var queryAgenda = from cita in bd.Agenda
                                  select new
                                  {
                                      cita.ID_Cita,
                                      cita.Rut_Usuario,
                                      cita.ID_Mascota,
                                      cita.fecha,
                                      cita.hora
                                  };
                dataGridView3.DataSource = queryAgenda.ToList();
            }
        }

        private void dataGridView2_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Solo exigimos que seleccionen la Mascota
            if (dataGridView2.CurrentRow != null)
            {
                if (!maskedTextBox1.MaskFull)
                {
                    MessageBox.Show("Por favor, ingrese la hora completa de la cita.", "Falta la hora", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                using (VeterinariaEntities bd = new VeterinariaEntities())
                {
                    Agenda nuevaCita = new Agenda();

                    // ASIGNACIÓN DINÁMICA: Toma el RUT del usuario que inició sesión
                    nuevaCita.Rut_Usuario = rutGuardado;

                    // Asignación de Mascota y Fecha
                    nuevaCita.ID_Mascota = Convert.ToInt32(dataGridView2.CurrentRow.Cells["ID_Mascota"].Value);
                    nuevaCita.fecha = dateTimePicker1.Value.Date;

                    // Validación de la Hora
                    maskedTextBox1.TextMaskFormat = MaskFormat.IncludeLiterals;
                    string textoHora = maskedTextBox1.Text.Trim();
                    TimeSpan horaValidada;

                    if (TimeSpan.TryParse(textoHora, out horaValidada))
                    {
                        if (horaValidada.Days > 0)
                        {
                            MessageBox.Show("La hora no puede superar las 24 hrs.", "Hora inválida", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        nuevaCita.hora = horaValidada;
                    }
                    else
                    {
                        MessageBox.Show("La hora ingresada no es válida. Por favor, asegúrese de usar el formato HH:MM.", "Error de formato", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // Guardar en la Base de Datos
                    bd.Agenda.Add(nuevaCita);
                    bd.SaveChanges();
                    Actualizar_Datos();
                    
                }
            }
            else
            {
                MessageBox.Show("Debe seleccionar una Mascota en la tabla central para agendar.", "Faltan datos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            
            maskedTextBox1.Clear();

            
            dateTimePicker1.Value = DateTime.Now;

            MessageBox.Show("Formulario limpiado.", "Limpiar", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            
            Form1 menuPrincipal = new Form1(rutGuardado, rolGuardado);
            menuPrincipal.Show();
            this.Close(); 
        }
    }
}