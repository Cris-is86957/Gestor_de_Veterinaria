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
            registrarHora();
        }
        private void registrarHora()
        {
            // Se verifica que el campo del ID contenga información antes de procesar.
            if (textBox1.Text != string.Empty)
            {
                // Se intenta convertir el texto a número entero; si falla, aborta para evitar excepciones de formato.
                if (!int.TryParse(textBox1.Text, out int idMascota))
                {
                    MessageBox.Show("El ID de la mascota debe ser un número.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Se extrae únicamente la parte de la fecha del control correspondiente.
                DateTime fechaCita = dateTimePicker1.Value.Date;

                // --- NUEVA VALIDACIÓN: BLOQUEAR DOMINGOS ---
                if (fechaCita.DayOfWeek == DayOfWeek.Sunday)
                {
                    MessageBox.Show("La clínica no atiende los días domingo. Por favor, seleccione un día de lunes a sábado.", "Día no válido", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                // -------------------------------------------

                // Se extrae la hora seleccionada, omitiendo los segundos para asegurar comparaciones exactas en la base de datos.
                TimeSpan horaExacta = new TimeSpan(dateTimePicker2.Value.TimeOfDay.Hours, dateTimePicker2.Value.TimeOfDay.Minutes, 0);

                // Se definen los límites del horario operativo del negocio.
                TimeSpan horaApertura = new TimeSpan(9, 0, 0);  // 09:00 AM
                TimeSpan horaCierre = new TimeSpan(18, 0, 0);   // 06:00 PM

                // Se valida que la hora exacta se encuentre dentro del rango permitido por las reglas del negocio.
                if (horaExacta < horaApertura || horaExacta > horaCierre)
                {
                    MessageBox.Show("La clínica solo atiende entre las 09:00 y las 18:00 hrs.", "Fuera de horario", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                try
                {
                    // Se inicializa el contexto de la base de datos.
                    using (VeterinariaEntities bd = new VeterinariaEntities())
                    {
                        // Se verifica en la base de datos si ya existe una cita registrada para esa misma fecha y hora.
                        bool horaOcupada = bd.Agenda.Any(c => c.fecha == fechaCita && c.hora == horaExacta);
                        if (horaOcupada)
                        {
                            // Detiene la ejecución si el bloque de tiempo ya se encuentra asignado.
                            MessageBox.Show("Ese horario ya está reservado. Por favor, seleccione otra hora o fecha.", "Hora no disponible", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        // Se instancia el nuevo objeto de cita y se mapean las propiedades con los datos validados.
                        Agenda nuevaCita = new Agenda();
                        nuevaCita.ID_Mascota = idMascota;
                        nuevaCita.Rut_Usuario = rutGuardado; // Asigna la cita al usuario actualmente autenticado en el sistema.
                        nuevaCita.fecha = fechaCita;
                        nuevaCita.hora = horaExacta;

                        // Se añade el registro al contexto y se impacta físicamente la base de datos.
                        bd.Agenda.Add(nuevaCita);
                        bd.SaveChanges();

                        // Se refrescan las tablas visuales y se restablecen los valores de los controles.
                        Actualizar_Datos();
                        Limpiar_Datos();
                        // Confirma la operación exitosa.
                        MessageBox.Show("Cita agendada correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    // Intercepta errores de conexión o escritura y expone el detalle del fallo.
                    MessageBox.Show("Ocurrió un error al guardar: " + ex.Message, "Error Crítico", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                // Muestra advertencia indicando que es obligatorio seleccionar un registro base para proceder.
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
            // Se valida que exista un identificador de cita previamente seleccionado en la interfaz.
            if (idCitaSeleccionada != 0)
            {
                // Se extrae la nueva fecha a asignar desde el control correspondiente.
                DateTime fechaCita = dateTimePicker1.Value.Date;

                // --- NUEVA VALIDACIÓN: BLOQUEAR DOMINGOS AL MODIFICAR ---
                if (fechaCita.DayOfWeek == DayOfWeek.Sunday)
                {
                    MessageBox.Show("La clínica no atiende los días domingo. Por favor, seleccione un día de lunes a sábado.", "Día no válido", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                // --------------------------------------------------------

                // Se extrae la nueva hora seleccionada, omitiendo los segundos para asegurar comparaciones exactas en la base de datos.
                TimeSpan horaExacta = new TimeSpan(dateTimePicker2.Value.TimeOfDay.Hours, dateTimePicker2.Value.TimeOfDay.Minutes, 0);

                // Se definen los límites del horario operativo permitido por las reglas del negocio.
                TimeSpan horaApertura = new TimeSpan(9, 0, 0);
                TimeSpan horaCierre = new TimeSpan(18, 0, 0);

                // Se verifica que la hora solicitada se encuentre dentro del rango de atención establecido.
                if (horaExacta < horaApertura || horaExacta > horaCierre)
                {
                    // Se interrumpe la ejecución si el horario se encuentra fuera de los límites.
                    MessageBox.Show("La clínica solo atiende entre las 09:00 y las 18:00 hrs.", "Fuera de horario", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                // Se inicializa el contexto de la base de datos para la transacción.
                using (VeterinariaEntities bd = new VeterinariaEntities())
                {
                    // Se consulta la disponibilidad del horario, excluyendo el registro de la cita actual para evitar que colisione consigo misma si solo se modifica la fecha.
                    bool horaOcupada = bd.Agenda.Any(c => c.fecha == fechaCita && c.hora == horaExacta && c.ID_Cita != idCitaSeleccionada);
                    if (horaOcupada)
                    {
                        // Se interrumpe la operación si el bloque horario ya está asignado a otro registro.
                        MessageBox.Show("Ese horario ya está reservado por otra mascota. Seleccione otro.", "Hora no disponible", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // Se realiza la búsqueda del registro a modificar mediante su clave primaria.
                    var citaAModificar = bd.Agenda.Find(idCitaSeleccionada);

                    // Se comprueba que el registro exista en la base de datos antes de proceder.
                    if (citaAModificar != null)
                    {
                        try
                        {
                            // Se sobrescriben las propiedades del registro con los nuevos valores validados.
                            citaAModificar.fecha = fechaCita;
                            citaAModificar.hora = horaExacta;

                            // Se ejecutan y confirman los cambios físicos en la tabla correspondiente.
                            bd.SaveChanges();

                            // Se refresca la grilla visual para mostrar los datos actualizados.
                            Actualizar_Datos();
                            // Se restablecen los controles de la interfaz a su estado predeterminado.
                            Limpiar_Datos();
                            // Notifica la finalización exitosa del proceso de modificación.
                            MessageBox.Show("Cita modificada correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        catch (Exception ex)
                        {
                            // Intercepta excepciones durante el proceso de guardado y expone el mensaje técnico del error.
                            MessageBox.Show("Error al modificar: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            else
            {
                // Muestra advertencia indicando que es obligatorio seleccionar un registro de la tabla para proceder con la modificación.
                MessageBox.Show("Selecciona una cita de la tabla de Agenda para modificar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
    }
}