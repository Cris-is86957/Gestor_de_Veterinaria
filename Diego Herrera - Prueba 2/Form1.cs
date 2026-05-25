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
    public partial class Form1 : Form
    {
        // CORRECCIÓN 1: Dejamos el constructor vacío para que Login.cs pueda abrirlo sin problemas
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            /* Actualizar_Datos();
            Llenar_Categoria();
            Llenar_Marca();
            Llenar_Proveedor();
            */
        }

        private void Actualizar_Datos()
        {
            // Código comentado por tu compañero
        }

        private void Llenar_Categoria()
        {
            // Código comentado por tu compañero
        }

        private void Llenar_Marca()
        {
            // Código comentado por tu compañero
        }

        private void Llenar_Proveedor()
        {
            // Código comentado por tu compañero
        }

        private void Limpiar_Datos()
        {
            // Código comentado por tu compañero
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Código comentado por tu compañero
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Código comentado por tu compañero
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // Código comentado por tu compañero
        }

        private void dataGridView1_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            // Código comentado por tu compañero
        }

        private void añadirNuevaMascotaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IngresoMascota ventana = new IngresoMascota();
            ventana.ShowDialog();
        }

        private void añadirDueñoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // CORRECCIÓN 2: Cambiamos "Dueños" por tu nombre real "FrmDueños"
            FrmDueños ventana = new FrmDueños();
            ventana.ShowDialog();
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void gestionarMascotaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IngresoMascota ventana = new IngresoMascota();
            ventana.ShowDialog();
        }

        private void gestionarDueñosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmDueños ventana = new FrmDueños();
            ventana.ShowDialog();
        }

        private void gestionarProductosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmProductos ventana = new FrmProductos();
            ventana.ShowDialog();
        }
    }
}