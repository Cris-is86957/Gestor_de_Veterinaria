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
            string rut = textBox1.Text;
            string contra = textBox2.Text;

            try
            {
                using (VeterinariaEntities db = new VeterinariaEntities())
                {

                    var usuarioValido = db.Usuario.FirstOrDefault(u => u.Rut_Usuario == rut && u.password == contra);

                    if (usuarioValido != null)
                    {

                        if (usuarioValido.Estado_Usuario == "Activo")
                        {

                            Form1 pantallaPrincipal = new Form1(usuarioValido.rol_usuario, usuarioValido.Rut_Usuario);
                            pantallaPrincipal.Show();

                            this.Hide();
                        }
                        else
                        {
                            MessageBox.Show("Tu cuenta está inactiva. Contacta al administrador.", "Acceso denegado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                    else
                    {
                        MessageBox.Show("RUT o contraseña incorrectos.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error de base de datos: " + ex.Message);
            }
        }
    }
}