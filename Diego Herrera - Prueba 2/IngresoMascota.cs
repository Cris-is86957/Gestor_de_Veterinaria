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
        private string rutGuardado = "";
        public IngresoMascota(string rolDelUsuario, string rutDelUsuario)
        {
            InitializeComponent();
            rolGuardado = rolDelUsuario;
            rutGuardado = rutDelUsuario;
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
                                  where miDueño.Estado_Dueño == "Activo"
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

            textBox1.ReadOnly = false;




        }

        private void button1_Click(object sender, EventArgs e)
        {
            registrarMascota();
            
        }
        private void registrarMascota()
        {
            // Se valida que la totalidad de los cuadros de texto y el menú desplegable contengan información antes de iniciar el proceso.
            if (textBox1.Text != string.Empty && textBox2.Text != string.Empty &&
                textBox3.Text != string.Empty && textBox4.Text != string.Empty &&
                textBox5.Text != string.Empty && comboBox1.SelectedIndex != -1 &&
                textBox6.Text != string.Empty)
            {
                // Se intenta convertir de manera segura los textos correspondientes al ID y la edad hacia valores numéricos enteros.
                // Si el usuario ingresa caracteres no numéricos, el programa aborta la ejecución en lugar de cerrarse de forma abrupta.
                if (!int.TryParse(textBox1.Text, out int idMascota) || !int.TryParse(textBox5.Text, out int edadMascota))
                {
                    MessageBox.Show("El ID de la mascota y la edad deben ser números enteros válidos.", "Error de formato", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                try
                {
                    // Se inicializa el contexto de Entity Framework para establecer la conexión con la base de datos.
                    using (VeterinariaEntities bd = new VeterinariaEntities())
                    {
                        // Se extrae el RUT del dueño ingresado y se eliminan posibles espacios en blanco en los extremos.
                        string rutBuscar = textBox2.Text.Trim();

                        // Se consulta la base de datos para verificar la existencia del dueño mediante su RUT.
                        // Esta es una validación lógica para proteger la integridad referencial (Clave Foránea) antes de intentar guardar.
                        if (!bd.Dueño.Any(d => d.Rut_dueño == rutBuscar))
                        {
                            MessageBox.Show("El RUT ingresado no corresponde a ningún dueño registrado. Registre al dueño primero.", "Dueño no encontrado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        // Se instancia un nuevo objeto de la entidad Mascota.
                        Mascota nuevoMascota = new Mascota();

                        // Se asignan a las propiedades del objeto los valores extraídos y validados desde la interfaz gráfica.
                        nuevoMascota.ID_Mascota = idMascota;
                        nuevoMascota.Rut_Dueño = textBox2.Text;
                        nuevoMascota.Nombre = textBox3.Text;
                        nuevoMascota.raza = textBox4.Text;
                        nuevoMascota.edad = edadMascota; // Se utiliza la variable numérica previamente sanitizada.
                        nuevoMascota.Estado_Mascota = comboBox1.SelectedValue.ToString();
                        nuevoMascota.tipo = textBox6.Text;

                        // Se añade el nuevo registro al contexto de datos.
                        bd.Mascota.Add(nuevoMascota);
                        // Se ejecutan y aplican los cambios físicamente en el servidor de base de datos.
                        bd.SaveChanges();

                        // Se recarga la grilla visual de datos y se restablecen los controles del formulario.
                        Actualizar_Datos();
                        Limpiar_Datos();
                        // Se notifica al operador que el registro se completó exitosamente.
                        MessageBox.Show("Mascota registrada con éxito.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    // Captura cualquier falla en la conexión o restricción no contemplada en la base de datos y muestra el detalle técnico.
                    MessageBox.Show("Error de base de datos: " + ex.Message, "Error Crítico", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                // Se emite una alerta si el usuario intentó guardar dejando campos obligatorios en blanco.
                MessageBox.Show("Debe completar todos los campos.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            eliminar();
        }
        private void eliminar()
        {
            // Se verifica que el campo correspondiente al identificador (ID) de la mascota no esté vacío antes de intentar la eliminación.
            if (textBox1.Text != string.Empty)
            {
                // Se inicializa el contexto de la base de datos, garantizando la apertura y posterior cierre automático de la conexión.
                using (VeterinariaEntities bd = new VeterinariaEntities())
                {
                    // Se convierte el texto del ID a un número entero y se busca el registro exacto de la mascota utilizando su clave primaria.
                    var nuevoMascota = bd.Mascota.Find(int.Parse(textBox1.Text));

                    // Se comprueba que la búsqueda haya devuelto un resultado válido (que la mascota realmente exista en la base de datos).
                    if (nuevoMascota != null)
                    {
                        // Se marca el objeto recuperado para ser eliminado del conjunto de datos de la entidad.
                        bd.Mascota.Remove(nuevoMascota);
                        // Se ejecutan y aplican los cambios físicamente en la base de datos (instrucción DELETE).
                        bd.SaveChanges();

                        // Se recarga la tabla visual de la interfaz para reflejar la eliminación.
                        Actualizar_Datos();
                        // Se restablecen los controles de texto a su estado original (vacíos).
                        Limpiar_Datos();
                    }
                }
            }
            else
            {
                // Muestra un mensaje de error si el usuario presiona el botón de eliminar sin haber seleccionado previamente un registro.
                MessageBox.Show("Se debe seleccionar una fila para eliminar.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            sobreescribir();
        }
        private void sobreescribir()
        {
            // Se verifica que el cuadro desplegable (estado de la mascota) no esté vacío, usándolo como condición para confirmar que el usuario seleccionó un registro previamente.
            if (comboBox1.Text != string.Empty)
            {
                // Se inicializa el contexto de Entity Framework para manejar la conexión a la base de datos y liberar recursos automáticamente.
                using (VeterinariaEntities bd = new VeterinariaEntities())
                {
                    // Se convierte el texto del cuadro del ID a número entero y se busca el registro exacto de la mascota en la base de datos mediante su clave primaria.
                    var nuevoMascota = bd.Mascota.Find(int.Parse(textBox1.Text));

                    // Se comprueba que la consulta haya encontrado el registro solicitado.
                    if (nuevoMascota != null)
                    {
                        // Se sobrescriben las propiedades del objeto recuperado de la base de datos con los nuevos valores escritos en la interfaz gráfica.
                        nuevoMascota.ID_Mascota = int.Parse(textBox1.Text);
                        nuevoMascota.Rut_Dueño = textBox2.Text;
                        nuevoMascota.Estado_Mascota = comboBox1.SelectedValue.ToString();
                        nuevoMascota.Nombre = textBox3.Text;
                        nuevoMascota.tipo = textBox6.Text;
                        nuevoMascota.raza = textBox4.Text;
                        nuevoMascota.edad = int.Parse(textBox5.Text); // Convierte la edad escrita a un valor numérico.

                        // Se ejecutan y consolidan los cambios físicamente en la base de datos (se realiza una instrucción UPDATE).
                        bd.SaveChanges();

                        // Se recarga la tabla visual para mostrar los datos modificados.
                        Actualizar_Datos();
                        // Se vacían los cuadros de texto para dejar el formulario limpio.
                        Limpiar_Datos();
                    }
                }
            }
            else
            {
                // Se emite un mensaje de error advirtiendo al operador que es obligatorio seleccionar un registro antes de presionar el botón de modificar.
                MessageBox.Show("Debe seleccionar una fila para modificar.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void dataGridView1_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow Fila = dataGridView1.Rows[e.RowIndex];

                // Uso de operadores seguros por si algún dato es NULL en la BD
                textBox1.Text = Fila.Cells["ID_Mascota"].Value?.ToString() ?? "";
                textBox2.Text = Fila.Cells["Rut_Dueño"].Value?.ToString() ?? "";
                textBox3.Text = Fila.Cells["Nombre"].Value?.ToString() ?? "";
                textBox4.Text = Fila.Cells["raza"].Value?.ToString() ?? "";
                textBox5.Text = Fila.Cells["edad"].Value?.ToString() ?? "";
                comboBox1.Text = Fila.Cells["Estado_Mascota"].Value?.ToString() ?? "";
                textBox6.Text = Fila.Cells["tipo"].Value?.ToString() ?? "";

                // Bloqueamos el ID para que no puedan corromper la Clave Primaria al editar
                textBox1.ReadOnly = true;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Form1 menuPrincipal = new Form1(rolGuardado, rutGuardado);
            menuPrincipal.Show();
            this.Close();
        }
        private void txtSoloNumeros_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Solo permite dígitos y la tecla de borrar
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }
    }
    }

