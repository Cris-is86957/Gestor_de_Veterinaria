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
        private string rutVeterinarioSeleccionado = "";
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
            // Se inicializa el contexto de la base de datos mediante Entity Framework.
            using (VeterinariaEntities bd = new VeterinariaEntities())
            {
                // 1. CARGA DE MASCOTAS
                // Se proyectan los datos del inventario de pacientes (mascotas).
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

                // 2. CARGA DE VETERINARIOS
                // Se filtra la tabla de usuarios para aislar únicamente a los que poseen el rol operativo de "Veterinario".
                var queryVeterinarios = from v in bd.Usuario
                                        where v.rol_usuario.ToLower() == "veterinario" && v.Estado_Usuario == "Activo"
                                        select new
                                        {
                                            Rut_Veterinario = v.Rut_Usuario,
                                            Nombre = v.nombre,
                                            Apellido = v.apellido
                                        };
                dataGridView3.DataSource = queryVeterinarios.ToList();
                dataGridView3.Refresh();

                // 3. CARGA DE AGENDA
                // Se proyectan las citas agendadas, concatenando nombre y apellido del doctor mediante llaves foráneas.
                var queryAgenda = from miCita in bd.Agenda
                                  select new
                                  {
                                      miCita.ID_Cita,
                                      Fecha = miCita.fecha,
                                      Hora = miCita.hora,
                                      Veterinario = miCita.Usuario.nombre + " " + miCita.Usuario.apellido,
                                      Nombre_Mascota = miCita.Mascota.Nombre
                                  };
                dataGridView2.DataSource = queryAgenda.ToList();
                dataGridView2.Refresh();
            }
        }
        private void Limpiar_Datos()
        {
            // Se restablecen los controles de texto y variables internas a su estado predeterminado.
            textBox1.Text = string.Empty;
            textBox2.Text = string.Empty;
            dateTimePicker1.Value = DateTime.Now;
            dateTimePicker2.Value = DateTime.Now;
            idCitaSeleccionada = 0;
            rutVeterinarioSeleccionado = "";
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Form1 menuPrincipal = new Form1(rolGuardado, rutGuardado);
            menuPrincipal.Show();
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            registrarHora();
        }
        private void registrarHora()
        {
            // Se valida la existencia de información en los campos obligatorios de selección.
            if (textBox1.Text != string.Empty)
            {
                // Se valida que el operador haya seleccionado a un especialista de la tabla correspondiente.
                if (rutVeterinarioSeleccionado == "")
                {
                    MessageBox.Show("Debe seleccionar un Veterinario de la tabla correspondiente.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                // Se intenta parsear el identificador de la mascota para prevenir excepciones de tipo de dato.
                if (!int.TryParse(textBox1.Text, out int idMascota))
                {
                    MessageBox.Show("El ID de la mascota debe ser un número.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Se extrae la fecha desde la interfaz, fijando el componente temporal a las 00:00:00.
                DateTime fechaCita = dateTimePicker1.Value.Date;

                // Validación de restricción de días operativos (bloqueo dominical).
                if (fechaCita.DayOfWeek == DayOfWeek.Sunday)
                {
                    MessageBox.Show("La clínica no atiende los días domingo. Por favor, seleccione un día de lunes a sábado.", "Día no válido", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                // Se extrae la hora descartando los segundos para realizar comparaciones precisas en SQL.
                TimeSpan horaExacta = new TimeSpan(dateTimePicker2.Value.TimeOfDay.Hours, dateTimePicker2.Value.TimeOfDay.Minutes, 0);

                // Se definen las constantes de la jornada laboral establecida por la clínica.
                TimeSpan horaApertura = new TimeSpan(9, 0, 0);  // 09:00 AM
                TimeSpan horaCierre = new TimeSpan(18, 0, 0);   // 06:00 PM

                // Se verifica la concordancia de la hora solicitada con la jornada laboral.
                if (horaExacta < horaApertura || horaExacta > horaCierre)
                {
                    MessageBox.Show("La clínica solo atiende entre las 09:00 y las 18:00 hrs.", "Fuera de horario", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                try
                {
                    using (VeterinariaEntities bd = new VeterinariaEntities())
                    {
                        // Se delimita el rango temporal de protección (1 hora) calculando 59 minutos en ambas direcciones.
                        TimeSpan rangoInicio = horaExacta.Subtract(TimeSpan.FromMinutes(59));
                        TimeSpan rangoFin = horaExacta.Add(TimeSpan.FromMinutes(59));

                        // Se consulta la existencia de colisiones temporales en la base de datos para el mismo veterinario.
                        bool horaOcupada = bd.Agenda.Any(c => c.Rut_Usuario == rutVeterinarioSeleccionado && c.fecha == fechaCita && c.hora >= rangoInicio && c.hora <= rangoFin);

                        if (horaOcupada)
                        {
                            MessageBox.Show("El veterinario seleccionado ya tiene una cita agendada en ese rango horario. Cada cita dura 1 hora. Por favor, seleccione otro horario u otro doctor.", "Hora no disponible", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        // Se instancia y se mapea la nueva entidad correspondiente a la cita.
                        Agenda nuevaCita = new Agenda();
                        nuevaCita.ID_Mascota = idMascota;
                        nuevaCita.Rut_Usuario = rutVeterinarioSeleccionado; // Se vincula la cita directamente al identificador del doctor.
                        nuevaCita.fecha = fechaCita;
                        nuevaCita.hora = horaExacta;

                        // Se persiste la nueva entidad en el servidor relacional.
                        bd.Agenda.Add(nuevaCita);
                        bd.SaveChanges();

                        // Se sincroniza la vista de datos y se depuran los campos de entrada.
                        Actualizar_Datos();
                        Limpiar_Datos();
                        MessageBox.Show("Cita agendada correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    // Se interceptan fallas a nivel de proveedor de datos y se informa el origen.
                    MessageBox.Show("Ocurrió un error al guardar: " + ex.Message, "Error Crítico", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Debe seleccionar una mascota de la tabla superior para agendar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void dataGridView1_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            // Verificación de clic válido
            if (e.RowIndex >= 0)
            {
                DataGridViewRow fila = dataGridView1.Rows[e.RowIndex];
                textBox1.Text = fila.Cells["ID_Mascota"].Value?.ToString() ?? "";
            }
        }

        private void dataGridView2_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow fila = dataGridView2.Rows[e.RowIndex];

                idCitaSeleccionada = Convert.ToInt32(fila.Cells["ID_Cita"].Value);

                // Controlamos que la fecha no venga nula desde la base de datos
                if (fila.Cells["Fecha"].Value != null)
                {
                    dateTimePicker1.Value = Convert.ToDateTime(fila.Cells["Fecha"].Value);
                }

                // Controlamos el tiempo de forma segura
                if (fila.Cells["Hora"].Value != null)
                {
                    TimeSpan horaSql = (TimeSpan)fila.Cells["Hora"].Value;
                    dateTimePicker2.Value = DateTime.Today.Add(horaSql);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            borrarHora();
        }

        private void borrarHora()
        {
            {
                // Se verifica que exista una cita previamente seleccionada mediante su identificador.
                if (idCitaSeleccionada != 0)
                {
                    // Se inicializa el contexto de conexión con la base de datos para manejar la transacción.
                    using (VeterinariaEntities bd = new VeterinariaEntities())
                    {
                        // Se realiza la búsqueda del registro correspondiente en la tabla Agenda utilizando la clave primaria.
                        var citaAEliminar = bd.Agenda.Find(idCitaSeleccionada);

                        // Se comprueba que el registro a eliminar haya sido encontrado exitosamente en la base de datos.
                        if (citaAEliminar != null)
                        {
                            // Se marca el objeto para ser removido del conjunto de datos de la entidad.
                            bd.Agenda.Remove(citaAEliminar);
                            // Se ejecutan y aplican los cambios de eliminación físicamente en la base de datos.
                            bd.SaveChanges();

                            // Se refresca la grilla visual para reflejar la eliminación del registro.
                            Actualizar_Datos();
                            // Se restablecen los controles de la interfaz gráfica a su estado original.
                            Limpiar_Datos();
                            // Se notifica la finalización exitosa del proceso de eliminación.
                            MessageBox.Show("Cita eliminada correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
                else
                {
                    // Muestra una advertencia indicando que es obligatorio seleccionar un registro de la tabla para proceder con la eliminación.
                    MessageBox.Show("Selecciona una cita de la tabla de Agenda para eliminar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            modificarHora();
        }
        private void modificarHora()
        {
            if (idCitaSeleccionada != 0)
            {
                // Se fuerza la selección de un veterinario antes de reprogramar la hora para mantener la integridad de la asignación.
                if (rutVeterinarioSeleccionado == "")
                {
                    MessageBox.Show("Debe seleccionar un Veterinario de la tabla correspondiente para modificar la cita.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                DateTime fechaCita = dateTimePicker1.Value.Date;

                // Validación dominical aplicada al proceso de reprogramación.
                if (fechaCita.DayOfWeek == DayOfWeek.Sunday)
                {
                    MessageBox.Show("La clínica no atiende los días domingo. Por favor, seleccione un día de lunes a sábado.", "Día no válido", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                TimeSpan horaExacta = new TimeSpan(dateTimePicker2.Value.TimeOfDay.Hours, dateTimePicker2.Value.TimeOfDay.Minutes, 0);

                TimeSpan horaApertura = new TimeSpan(9, 0, 0);
                TimeSpan horaCierre = new TimeSpan(18, 0, 0);

                if (horaExacta < horaApertura || horaExacta > horaCierre)
                {
                    MessageBox.Show("La clínica solo atiende entre las 09:00 y las 18:00 hrs.", "Fuera de horario", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                using (VeterinariaEntities bd = new VeterinariaEntities())
                {
                    TimeSpan rangoInicio = horaExacta.Subtract(TimeSpan.FromMinutes(59));
                    TimeSpan rangoFin = horaExacta.Add(TimeSpan.FromMinutes(59));

                    // Se verifica el conflicto de horario excluyendo la propia instancia de la cita para permitir su modificación.
                    bool horaOcupada = bd.Agenda.Any(c => c.Rut_Usuario == rutVeterinarioSeleccionado && c.fecha == fechaCita && c.hora >= rangoInicio && c.hora <= rangoFin && c.ID_Cita != idCitaSeleccionada);

                    if (horaOcupada)
                    {
                        MessageBox.Show("El veterinario ya tiene asignado ese bloque horario. Seleccione otro.", "Hora no disponible", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    var citaAModificar = bd.Agenda.Find(idCitaSeleccionada);

                    if (citaAModificar != null)
                    {
                        try
                        {
                            // Se aplican las mutaciones a las propiedades de la entidad seleccionada.
                            citaAModificar.fecha = fechaCita;
                            citaAModificar.hora = horaExacta;
                            citaAModificar.Rut_Usuario = rutVeterinarioSeleccionado; // Permite transferir la cita a un médico diferente.

                            bd.SaveChanges();

                            Actualizar_Datos();
                            Limpiar_Datos();
                            MessageBox.Show("Cita modificada correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error al modificar: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Selecciona una cita de la tabla de Agenda para modificar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void dataGridView3_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            // Se comprueba que la interacción haya ocurrido sobre una fila de datos y no sobre la cabecera de las columnas.
            if (e.RowIndex >= 0)
            {
                DataGridViewRow fila = dataGridView3.Rows[e.RowIndex];

                // Se extrae la clave primaria del veterinario de forma segura, previendo posibles retornos nulos.
                rutVeterinarioSeleccionado = fila.Cells["Rut_Veterinario"].Value?.ToString() ?? "";

                // Se extrae y concatena la información textual para brindar retroalimentación en la interfaz gráfica.
                string nombreDoc = fila.Cells["Nombre"].Value?.ToString() ?? "";
                string apellidoDoc = fila.Cells["Apellido"].Value?.ToString() ?? "";

                // Se inyecta la cadena resultante en el control visual de sólo lectura.
                textBox2.Text = nombreDoc + " " + apellidoDoc;
            }
        }
    }
}