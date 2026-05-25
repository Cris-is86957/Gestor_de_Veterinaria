using System;
using System.Linq;
using System.Windows.Forms;

namespace Diego_Herrera___Prueba_2
{
    public partial class FrmProductos : Form
    {
        public FrmProductos()
        {
            InitializeComponent();
            // Vinculamos eventos
            this.Load += FrmProductos_Load;
            btnGuardar.Click += btnGuardar_Click;
            btnEditar.Click += btnEditar_Click;
            btnEliminar.Click += btnEliminar_Click;
            btnVolver.Click += btnVolver_Click;
            // Evento para que al hacer clic en la tabla se llenen los campos
            dgvProductos.CellClick += dgvProductos_CellClick;
        }

        private void Actualizar_Datos()
        {
            using (VeterinariaEntities bd = new VeterinariaEntities())
            {
                var query = from p in bd.PRODUCTOS
                            select new { p.ID_Producto, p.Nombre, p.Stock, p.Precio_unidad };
                dgvProductos.DataSource = query.ToList();
            }
        }

        private void FrmProductos_Load(object sender, EventArgs e)
        {
            Actualizar_Datos();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            // Validamos que sean números antes de guardar
            if (!int.TryParse(txtStock.Text, out int stock) || !double.TryParse(txtPrecio.Text, out double precio))
            {
                MessageBox.Show("Por favor, ingresa números válidos en Stock y Precio.");
                return;
            }

            using (VeterinariaEntities db = new VeterinariaEntities())
            {
                PRODUCTOS nuevoProd = new PRODUCTOS
                {
                    ID_Producto = int.Parse(txtId.Text),
                    Nombre = txtNombre.Text,
                    Stock = stock,
                    Precio_unidad = (decimal)precio,
                    Estado_producto = true
                };

                db.PRODUCTOS.Add(nuevoProd);
                db.SaveChanges();
                Actualizar_Datos();
                MessageBox.Show("Producto registrado.");
            }
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (dgvProductos.SelectedRows.Count > 0)
            {
                int id = int.Parse(dgvProductos.SelectedRows[0].Cells["ID_Producto"].Value.ToString());
                using (VeterinariaEntities db = new VeterinariaEntities())
                {
                    var prod = db.PRODUCTOS.Find(id);
                    if (prod != null)
                    {
                        prod.Nombre = txtNombre.Text;
                        prod.Stock = int.Parse(txtStock.Text);
                        prod.Precio_unidad = (decimal)double.Parse(txtPrecio.Text);
                        db.SaveChanges();
                        Actualizar_Datos();
                    }
                }
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvProductos.SelectedRows.Count > 0)
            {
                int id = int.Parse(dgvProductos.SelectedRows[0].Cells["ID_Producto"].Value.ToString());
                using (VeterinariaEntities db = new VeterinariaEntities())
                {
                    var prod = db.PRODUCTOS.Find(id);
                    if (prod != null)
                    {
                        db.PRODUCTOS.Remove(prod);
                        db.SaveChanges();
                        Actualizar_Datos();
                    }
                }
            }
        }

        private void btnVolver_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // Esta función llena los campos automáticamente al hacer clic en la tabla
        private void dgvProductos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvProductos.Rows[e.RowIndex];
                txtId.Text = row.Cells["ID_Producto"].Value.ToString();
                txtNombre.Text = row.Cells["Nombre"].Value.ToString();
                txtStock.Text = row.Cells["Stock"].Value.ToString();
                txtPrecio.Text = row.Cells["Precio_unidad"].Value.ToString();
            }
        }
    }
}