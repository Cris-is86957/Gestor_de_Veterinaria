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
    public partial class FormsVentas : Form
    {
        private string rolGuardado;
        private string rutGuardado;
        private string nombreProductoActual = "";
        private int precioProductoActual = 0;
        private int stockDisponible = 0;

        private DataTable carritoTemporal = new DataTable();
        public FormsVentas(string rolDelUsuario, string rutDelUsuario)
        {
            InitializeComponent();
            rolGuardado = rolDelUsuario;
            rutGuardado = rutDelUsuario;

            carritoTemporal.Columns.Add("ID_Producto", typeof(int));
            carritoTemporal.Columns.Add("Nombre", typeof(string));
            carritoTemporal.Columns.Add("Cantidad", typeof(int));
            carritoTemporal.Columns.Add("Precio", typeof(int));
            carritoTemporal.Columns.Add("Subtotal", typeof(int));
        }

        private void FormsVentas_Load(object sender, EventArgs e)
        {
            dataGridView2.DataSource = carritoTemporal;
            Actualizar_Catalogo();
            Actualizar_Historial();

        }
        private void Actualizar_Catalogo()
        {
            using (VeterinariaEntities bd = new VeterinariaEntities())
            {
                var queryProductos = from prod in bd.Productos
                                     where prod.Stock > 0
                                     select new
                                     {
                                         prod.ID_Producto,
                                         prod.Nombre,
                                         prod.Precio_Unidad,
                                         prod.Stock,
                                         prod.Estado_Producto
                                     };

                dataGridView1.DataSource = queryProductos.ToList();
                dataGridView1.Refresh();
            }
        }
        private void Actualizar_Historial()
        {
            using (VeterinariaEntities bd = new VeterinariaEntities())
            {
                var historial = from b in bd.Boleta
                                orderby b.fecha descending
                                select new
                                {
                                    Folio = b.ID_Venta,
                                    Fecha = b.fecha,
                                    Atendido_Por = b.Usuario.nombre,
                                    Total = b.Total_Venta
                                };
                dataGridView3.DataSource = historial.ToList();
            }
        }

        private void dataGridView1_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow fila = dataGridView1.Rows[e.RowIndex];
                textBox1.Text = fila.Cells["ID_Producto"].Value.ToString();
                nombreProductoActual = fila.Cells["Nombre"].Value.ToString();
                precioProductoActual = Convert.ToInt32(fila.Cells["Precio_Unidad"].Value);
                stockDisponible = Convert.ToInt32(fila.Cells["Stock"].Value);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != string.Empty)
            {
                int cantidadAVender = (int)numericUpDown1.Value;

                if (cantidadAVender > stockDisponible)
                {
                    MessageBox.Show($"Solo hay {stockDisponible} unidades disponibles en stock.", "Stock insuficiente", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (cantidadAVender <= 0)
                {
                    MessageBox.Show("La cantidad debe ser mayor a 0.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int idProducto = int.Parse(textBox1.Text);
                int subtotal = cantidadAVender * precioProductoActual;

                carritoTemporal.Rows.Add(idProducto, nombreProductoActual, cantidadAVender, precioProductoActual, subtotal);
                CalcularTotal();
            }
            else
            {
                MessageBox.Show("Primero selecciona un producto del catálogo.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
        private void CalcularTotal()
        {
            int totalPagar = 0;
            foreach (DataRow fila in carritoTemporal.Rows)
            {
                totalPagar += Convert.ToInt32(fila["Subtotal"]);
            }

            label2.Text = "Total a Pagar: $" + totalPagar.ToString("N0");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (carritoTemporal.Rows.Count > 0)
            {
                try
                {
                    using (VeterinariaEntities bd = new VeterinariaEntities())
                    {
                        Boleta nuevaBoleta = new Boleta();
                        nuevaBoleta.Rut_Usuario = rutGuardado;
                        nuevaBoleta.fecha = DateTime.Now;

                        int totalVenta = 0;

                        foreach (DataRow filaCarrito in carritoTemporal.Rows)
                        {

                            Ventas nuevoDetalle = new Ventas();
                            nuevoDetalle.ID_Producto = Convert.ToInt32(filaCarrito["ID_Producto"]);
                            nuevoDetalle.Cantidad = Convert.ToInt32(filaCarrito["Cantidad"]);
                            nuevoDetalle.Precio_Unidad = Convert.ToInt32(filaCarrito["Precio"]);

                            totalVenta += (nuevoDetalle.Cantidad * nuevoDetalle.Precio_Unidad);

                            nuevaBoleta.Ventas.Add(nuevoDetalle);

                            int idProd = nuevoDetalle.ID_Producto;
                            var productoBD = bd.Productos.Find(idProd);
                            if (productoBD != null)
                            {
                                productoBD.Stock -= nuevoDetalle.Cantidad;
                            }
                        }

                        nuevaBoleta.Total_Venta = totalVenta;

                        bd.Boleta.Add(nuevaBoleta);
                        bd.SaveChanges();

                        carritoTemporal.Clear();
                        CalcularTotal();
                        Actualizar_Catalogo();
                        Actualizar_Historial();
                        textBox1.Text = string.Empty;

                        MessageBox.Show("¡Venta concretada con éxito!", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al guardar: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("El carrito está vacío.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form1 menuPrincipal = new Form1(rolGuardado, rutGuardado);
            menuPrincipal.Show();
            this.Close();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (dataGridView3.SelectedRows.Count > 0)
            {
                int idBoleta = Convert.ToInt32(dataGridView3.SelectedRows[0].Cells["Folio"].Value);

                using (VeterinariaEntities bd = new VeterinariaEntities())
                {

                    var detalles = from v in bd.Ventas
                                   where v.ID_Venta == idBoleta
                                   select new
                                   {
                                       Producto = v.Productos.Nombre,
                                       v.Cantidad,
                                       v.Precio_Unidad,
                                       Subtotal = v.Cantidad * v.Precio_Unidad
                                   };

                    string mensaje = $"--- DETALLE DE LA BOLETA N° {idBoleta} ---\n\n";
                    int totalCalculado = 0;

                    foreach (var item in detalles)
                    {
                        mensaje += $"{item.Cantidad}x {item.Producto} a ${item.Precio_Unidad} c/u  --->  ${item.Subtotal}\n";
                        totalCalculado += item.Subtotal;
                    }

                    mensaje += $"TOTAL PAGADO: ${totalCalculado}";

                    MessageBox.Show(mensaje, "Detalle de Venta", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("Por favor, seleccione una boleta del historial haciendo clic en la fila.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

            DateTime fechaInicio = dateTimePicker1.Value.Date;


            DateTime fechaFin = fechaInicio.AddDays(1);

            using (VeterinariaEntities bd = new VeterinariaEntities())
            {

                var historialFiltrado = from b in bd.Boleta
                                        where b.fecha >= fechaInicio && b.fecha < fechaFin
                                        orderby b.fecha descending
                                        select new
                                        {
                                            Folio = b.ID_Venta,
                                            Fecha = b.fecha,
                                            Atendido_Por = b.Usuario.nombre,
                                            Total = b.Total_Venta
                                        };

                dataGridView3.DataSource = historialFiltrado.ToList();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            DateTime fechaInicio = dateTimePicker1.Value.Date;
            DateTime fechaFin = fechaInicio.AddDays(1);

            using (VeterinariaEntities bd = new VeterinariaEntities())
            {
                var historialFiltrado = from b in bd.Boleta
                                        where b.fecha >= fechaInicio && b.fecha < fechaFin
                                        orderby b.fecha descending
                                        select new
                                        {
                                            Folio = b.ID_Venta,
                                            Fecha = b.fecha,
                                            Atendido_Por = b.Usuario.nombre,
                                            Total = b.Total_Venta
                                        };

                dataGridView3.DataSource = historialFiltrado.ToList();
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (carritoTemporal.Rows.Count > 0)
            {
                carritoTemporal.Clear();
                CalcularTotal();
                MessageBox.Show("El carrito ha sido vaciado.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        }
    }

