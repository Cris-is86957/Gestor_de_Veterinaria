using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;

namespace Diego_Herrera___Prueba_2
{

    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void Login_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Se capturan los valores ingresados y se eliminan los espacios en blanco de los extremos.
            string rut = textBox1.Text.Trim();
            string contra = textBox2.Text.Trim();

            // Se valida que los campos no estén nulos ni vacíos.
            if (string.IsNullOrEmpty(rut) || string.IsNullOrEmpty(contra))
            {
                // Muestra advertencia si faltan datos.
                MessageBox.Show("Por favor, ingrese su RUT y contraseña.", "Campos incompletos", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                // Finaliza la ejecución de la función para evitar consultas inválidas a la base de datos.
                return;
            }

            try
            {
                // Se abre la conexión con la base de datos.
                using (VeterinariaEntities db = new VeterinariaEntities())
                {
                    // Busca el primer registro que coincida con el RUT y la contraseña proporcionados.
                    var usuarioValido = db.Usuario.FirstOrDefault(u => u.Rut_Usuario == rut && u.password == contra);

                    // Verifica si se encontró un usuario coincidente.
                    if (usuarioValido != null)
                    {
                        // Comprueba si el estado del usuario le permite operar en el sistema.
                        if (usuarioValido.Estado_Usuario == "Activo")
                        {
                            // Se instancia el menú principal pasando el rol y el RUT como parámetros de sesión.
                            Form1 pantallaPrincipal = new Form1(usuarioValido.rol_usuario, usuarioValido.Rut_Usuario);
                            // Muestra la pantalla principal.
                            pantallaPrincipal.Show();
                            // Oculta el formulario de inicio de sesión.
                            this.Hide();
                        }
                        else
                        {
                            // Muestra alerta indicando que el acceso está bloqueado por el estado de la cuenta.
                            MessageBox.Show("Tu cuenta está inactiva. Contacta al administrador.", "Acceso denegado", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        }
                    }
                    else
                    {
                        // Limpia el texto de la contraseña por seguridad.
                        textBox2.Text = string.Empty;
                        // Devuelve el cursor al cuadro de texto de la contraseña.
                        textBox2.Focus();
                        // Muestra alerta de credenciales incorrectas.
                        MessageBox.Show("RUT o contraseña incorrectos.", "Credenciales inválidas", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            catch (Exception ex)
            {
                // Captura excepciones de la base de datos y muestra el detalle del error en pantalla.
                MessageBox.Show("No se pudo conectar con la base de datos.\nDetalle: " + ex.Message, "Error de Conexión", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Se evalúa si la tecla presionada no corresponde a un dígito numérico, la letra 'k' o 'K', un guion, ni una tecla de control (como la tecla de retroceso).
            if (!char.IsDigit(e.KeyChar) &&
                e.KeyChar != 'k' && e.KeyChar != 'K' &&
                e.KeyChar != '-' && !char.IsControl(e.KeyChar))
            {
                // Se marca el evento como manejado, bloqueando la entrada del carácter y evitando que se escriba en el cuadro de texto.
                e.Handled = true;
            }
        }
    }
}