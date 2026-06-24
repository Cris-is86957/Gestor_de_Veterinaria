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
        private string rutGuardado;

        public Usuarioscs(string rolDelUsuario, string rutDelUsuario)
        {
            InitializeComponent();
            rolGuardado = rolDelUsuario;
            rutGuardado = rutDelUsuario;
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
                                miUsuario.rol_usuario


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
            textBox5.Text = string.Empty;
            comboBox1.SelectedIndex = -1;

            textBox1.ReadOnly = false;


        }
        private void Ingresar_Estado()
        {
            // Devolvemos esta función a su propósito original: Llenar el ComboBox
            var opcionesEstado = new[] {
                new { Id = "Activo", Nombre = "Activo" },
                new { Id = "Inactivo", Nombre = "Inactivo" },
            };

            comboBox1.DataSource = opcionesEstado.ToList();
            comboBox1.DisplayMember = "Nombre";
            comboBox1.ValueMember = "Id";
            comboBox1.SelectedIndex = -1;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            eliminar();
        }
        private void eliminar()
        {
            // Se verifica que el campo del identificador (RUT del usuario) no esté vacío antes de intentar proceder con la eliminación.
            if (textBox1.Text != string.Empty)
            {
                // Se inicializa el contexto de la base de datos mediante Entity Framework, garantizando el cierre automático de la conexión al finalizar.
                using (VeterinariaEntities bd = new VeterinariaEntities())
                {
                    // Se realiza la búsqueda del registro exacto del usuario en la base de datos utilizando el texto ingresado como clave primaria.
                    var nuevoUsuario = bd.Usuario.Find(textBox1.Text);

                    // Se comprueba que la búsqueda haya devuelto un resultado válido (que el usuario exista en el sistema).
                    if (nuevoUsuario != null)
                    {
                        try
                        {
                            // Se marca la entidad del usuario para ser removida del conjunto de datos.
                            bd.Usuario.Remove(nuevoUsuario);
                            // Se intentan ejecutar y aplicar los cambios físicamente en el servidor de base de datos (instrucción DELETE).
                            bd.SaveChanges();

                            // Se recarga la tabla visual de la interfaz para reflejar que el usuario ya no está.
                            Actualizar_Datos();
                            // Se restablecen los cuadros de texto a su estado original (vacíos).
                            Limpiar_Datos();
                            // Se notifica que la operación se realizó de manera exitosa.
                            MessageBox.Show("Usuario eliminado del sistema.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        catch (Exception)
                        {
                            // Captura la excepción que arroja SQL Server cuando se viola la Integridad Referencial (Clave Foránea).
                            // Esto evita que el programa se cierre de golpe si el usuario tiene transacciones enlazadas a su RUT.
                            MessageBox.Show("No se puede eliminar a este usuario porque ya tiene ventas o citas agendadas en el sistema. Cambie su estado a 'Inactivo'.", "Acción bloqueada", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        }
                    }
                }
            }
            else
            {
                // Muestra un mensaje de advertencia si el operador intenta presionar el botón de eliminar sin haber seleccionado a un usuario de la tabla.
                MessageBox.Show("Se debe seleccionar un usuario de la tabla para eliminar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            sobreescribir();
        }
        private void sobreescribir()
        {
            // Se verifica que el campo correspondiente al identificador (RUT) del usuario no esté vacío antes de iniciar el proceso de modificación.
            if (textBox1.Text != string.Empty)
            {
                // Se inicializa el contexto de Entity Framework para manejar la conexión a la base de datos de forma segura, liberando recursos al terminar.
                using (VeterinariaEntities bd = new VeterinariaEntities())
                {
                    // Se realiza la búsqueda del registro exacto del usuario en la base de datos utilizando el texto ingresado (RUT) como clave primaria.
                    var nuevoUsuario = bd.Usuario.Find(textBox1.Text);

                    // Se comprueba que la consulta haya encontrado exitosamente el registro en el sistema.
                    if (nuevoUsuario != null)
                    {
                        // Se sobrescriben las propiedades del objeto recuperado con los nuevos datos que el administrador ingresó en los campos del formulario.
                        nuevoUsuario.Rut_Usuario = textBox1.Text;
                        nuevoUsuario.Estado_Usuario = comboBox1.SelectedValue.ToString();
                        nuevoUsuario.nombre = textBox2.Text;
                        nuevoUsuario.apellido = textBox3.Text;
                        nuevoUsuario.password = textBox4.Text; // Se actualiza la contraseña con el valor del cuadro de texto.
                        nuevoUsuario.rol_usuario = textBox5.Text; // Se asigna el nivel de permisos o cargo (por ejemplo, admin, cajero, veterinario).

                        // Se ejecutan y aplican los cambios físicamente en el servidor de base de datos (instrucción UPDATE).
                        bd.SaveChanges();

                        // Se recarga la tabla visual de usuarios para reflejar inmediatamente las modificaciones en pantalla.
                        Actualizar_Datos();
                        // Se restablecen y vacían los controles de la interfaz para dejar el formulario limpio.
                        Limpiar_Datos();
                    }
                }
            }
            else
            {
                // Se emite un mensaje de error si el administrador intenta presionar el botón de modificar sin haber seleccionado previamente un registro.
                MessageBox.Show("Debe seleccionar una fila para modificar.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Form1 menuPrincipal = new Form1(rolGuardado, rutGuardado);
            menuPrincipal.Show();
            this.Close();
        }

        private void dataGridView1_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow Fila = dataGridView1.Rows[e.RowIndex];

                // Extracción segura contra valores nulos
                textBox1.Text = Fila.Cells["Rut_Usuario"].Value?.ToString() ?? "";
                comboBox1.Text = Fila.Cells["Estado_Usuario"].Value?.ToString() ?? "";
                textBox2.Text = Fila.Cells["nombre"].Value?.ToString() ?? "";
                textBox3.Text = Fila.Cells["apellido"].Value?.ToString() ?? "";
                textBox4.Text = Fila.Cells["password"].Value?.ToString() ?? "";
                textBox5.Text = Fila.Cells["rol_usuario"].Value?.ToString() ?? "";

                // Bloquear el RUT para no romper la llave de la BD
                textBox1.ReadOnly = true;
            }
        }
        private void txtRut_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Solo números, la letra K, guion y borrar
            if (!char.IsDigit(e.KeyChar) &&
                e.KeyChar != 'k' && e.KeyChar != 'K' &&
                e.KeyChar != '-' && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txtSoloLetras_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Solo letras y tecla de borrar
            if (!char.IsLetter(e.KeyChar) && !char.IsControl(e.KeyChar) && e.KeyChar != ' ')
            {
                e.Handled = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ingresar();

        }
        private void ingresar()
        {
            // Se valida que la totalidad de los cuadros de texto y el menú desplegable contengan información antes de intentar guardar.
            if (textBox1.Text != string.Empty && textBox2.Text != string.Empty &&
                textBox3.Text != string.Empty && textBox4.Text != string.Empty &&
                textBox5.Text != string.Empty && comboBox1.SelectedIndex != -1)
            {
                // Se inicializa el contexto de Entity Framework para establecer la conexión con la base de datos de manera segura.
                using (VeterinariaEntities bd = new VeterinariaEntities())
                {
                    // Se extrae el RUT ingresado por el administrador, eliminando posibles espacios en blanco en los extremos.
                    string rutBuscado = textBox1.Text.Trim();

                    // Se consulta la base de datos para verificar si el RUT ya existe en los registros de usuarios.
                    // Esto protege la integridad del sistema al evitar colisiones y errores de clave primaria duplicada (Primary Key).
                    if (bd.Usuario.Any(u => u.Rut_Usuario == rutBuscado))
                    {
                        MessageBox.Show("Este RUT ya está registrado como usuario en el sistema.", "RUT Duplicado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    try
                    {
                        // Se instancia un nuevo objeto de la entidad Usuario.
                        Usuario nuevoUsuario = new Usuario();

                        // Se mapean y asignan los valores capturados en la interfaz gráfica hacia las propiedades del nuevo objeto.
                        nuevoUsuario.Rut_Usuario = textBox1.Text;
                        nuevoUsuario.Estado_Usuario = comboBox1.SelectedValue.ToString();
                        nuevoUsuario.nombre = textBox2.Text;
                        nuevoUsuario.apellido = textBox3.Text;
                        nuevoUsuario.password = textBox4.Text; // Se asigna la clave de acceso.
                        nuevoUsuario.rol_usuario = textBox5.Text; // Se define el nivel de acceso al sistema (ej. Administrador, Vendedor).

                        // Se añade el nuevo registro de usuario al contexto de datos.
                        bd.Usuario.Add(nuevoUsuario);
                        // Se ejecutan y confirman los cambios físicamente en el servidor de base de datos (instrucción INSERT).
                        bd.SaveChanges();

                        // Se recarga la grilla visual de usuarios y se restablecen los controles del formulario a su estado original.
                        Actualizar_Datos();
                        Limpiar_Datos();
                        // Se notifica que la operación de registro se completó con éxito.
                        MessageBox.Show("Usuario registrado con éxito.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        // Captura cualquier falla inesperada en la base de datos o en la red y muestra un mensaje con el detalle técnico.
                        MessageBox.Show("Error al guardar en BD: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                // Se emite una alerta si el administrador intentó crear la cuenta dejando campos obligatorios en blanco.
                MessageBox.Show("Debe completar todos los campos.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
    }
}
