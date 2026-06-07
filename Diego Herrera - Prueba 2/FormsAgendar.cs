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

    public partial class FormsAgendar : Form
    {
        private string rolGuardado;
        private string rutGuardado;
        public FormsAgendar(string rolDelUsuario, string rutDelUsuario)
        {
            InitializeComponent();
            rolGuardado = rolDelUsuario;
            rutGuardado = rutDelUsuario;
        }


        private void FormsAgendar_Load(object sender, EventArgs e)
        {
            Actualizar_Datos();
        }
        private void Actualizar_Datos()
        {
            
}
