using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;


namespace Diego_Herrera___Prueba_2
{
    public partial class Form1 : Form
    {
        private string rolGuardado = "";
        private string rutGuardado = "";
        public Form1(string rolDelUsuario, string rutDelUsuario)
        {
            InitializeComponent();
            rolGuardado = rolDelUsuario;
            rutGuardado = rutDelUsuario;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            /* 
            Actualizar_Datos();
            Llenar_Categoria();
            Llenar_Marca();
            Llenar_Proveedor();
            */
        }



        

        private void button1_Click(object sender, EventArgs e)
        {
            Dueños formDueño = new Dueños(rolGuardado, rutGuardado);
            formDueño.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            IngresoMascota formMascota = new IngresoMascota(rolGuardado, rutGuardado);
            formMascota.Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Producto formProducto = new Producto(rolGuardado, rutGuardado);
            formProducto.Show();
            this.Hide();
        }

        private void dataGridView1_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {

        }



        private void button4_Click(object sender, EventArgs e)
        {
            if (rolGuardado == "Administrador")
            {
                Usuarioscs formUsuario = new Usuarioscs(rolGuardado, rutGuardado);
                formUsuario.Show();
                this.Hide();
            }
            else
            {

                MessageBox.Show("No tienes permisos de Administrador para acceder a esta seccion.", "Acceso Denegado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            FormsAgendar formAgenda = new FormsAgendar(rolGuardado, rutGuardado);
            formAgenda.Show();
            this.Hide();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            FormsVentas formVentas = new FormsVentas(rolGuardado, rutGuardado);
            formVentas.Show();
            this.Hide();
        }
    }
}
