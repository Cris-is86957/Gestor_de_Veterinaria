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
    public partial class FrmDueños : Form
    {
        public FrmDueños()
        {
            InitializeComponent();

            // FORZAMOS LA VINCULACIÓN DE EVENTOS AQUÍ
            // Esto asegura que aunque el diseñador falle, los botones funcionen.
            this.Load += FrmDueños_Load;
            btnVolver.Click += btnVolver_Click;
            btnGuardar.Click += btnGuardar_Click;
            btnEditar.Click += btnEditar_Click;
            btnEliminar.Click += btnEliminar_Click;
        }

        private void Actualizar_Datos()
        {
            try
            {
                using (VeterinariaEntities bd = new VeterinariaEntities())
                {
                    // Consultamos los datos
                    var query = from d in bd.DUEÑOS
                                select new
                                {
                                    d.Rut_dueño,
                                    d.nombre,
                                    d.apell_pat,
                                    d.apell_mat,
                                    d.Estado_dueño
                                };

                    // Asignamos el DataSource
                    dgvDueños.DataSource = query.ToList();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar la tabla: " + ex.Message);
            }
        }

        private void FrmDueños_Load(object sender, EventArgs e)
        {
            Actualizar_Datos();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtRut.Text) || string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                MessageBox.Show("Por favor, ingresa al menos el RUT y el Nombre.");
                return;
            }

            try
            {
                using (VeterinariaEntities db = new VeterinariaEntities())
                {
                    DUEÑOS nuevoDueño = new DUEÑOS
                    {
                        Rut_dueño = txtRut.Text.Trim(),
                        nombre = txtNombre.Text.Trim(),
                        apell_pat = txtApellPat.Text.Trim(),
                        apell_mat = txtApellMat.Text.Trim(),
                        Estado_dueño = true
                    };

                    db.DUEÑOS.Add(nuevoDueño);
                    db.SaveChanges();
                    Actualizar_Datos();
                    MessageBox.Show("Dueño registrado exitosamente.");

                    // Limpiar
                    txtRut.Clear(); txtNombre.Clear(); txtApellPat.Clear(); txtApellMat.Clear();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar: " + ex.Message);
            }
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (dgvDueños.SelectedRows.Count > 0)
            {
                string rut = dgvDueños.SelectedRows[0].Cells["Rut_dueño"].Value.ToString();
                using (VeterinariaEntities db = new VeterinariaEntities())
                {
                    var dueño = db.DUEÑOS.Find(rut);
                    if (dueño != null)
                    {
                        dueño.nombre = txtNombre.Text;
                        dueño.apell_pat = txtApellPat.Text;
                        dueño.apell_mat = txtApellMat.Text;
                        db.SaveChanges();
                        Actualizar_Datos();
                    }
                }
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvDueños.SelectedRows.Count > 0)
            {
                string rut = dgvDueños.SelectedRows[0].Cells["Rut_dueño"].Value.ToString();
                using (VeterinariaEntities db = new VeterinariaEntities())
                {
                    var dueño = db.DUEÑOS.Find(rut);
                    if (dueño != null)
                    {
                        db.DUEÑOS.Remove(dueño);
                        db.SaveChanges();
                        Actualizar_Datos();
                    }
                }
            }
        }

        private void btnVolver_Click(object sender, EventArgs e)
        {
            this.Close(); // Cierra el formulario y vuelve al anterior
        }
    }
}