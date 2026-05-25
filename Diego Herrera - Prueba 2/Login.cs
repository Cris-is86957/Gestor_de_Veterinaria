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
                    var usuarioValido = db.USUARIO.FirstOrDefault(u => u.Rut_usuario == rut && u.password == contra);

                    if (usuarioValido != null)
                    {
                        Form1 pantallaPrincipal = new Form1();
                        pantallaPrincipal.Show();
                        this.Hide();
                    }
                    else
                    {
                        MessageBox.Show("RUT o contraseña incorrectos.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            catch (Exception ex)
            {
                
                string errorReal = "Error principal: " + ex.Message;
                if (ex.InnerException != null)
                {
                    errorReal += "\n\nDetalle técnico: " + ex.InnerException.Message;
                }

                MessageBox.Show(errorReal, "Error Detallado", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}