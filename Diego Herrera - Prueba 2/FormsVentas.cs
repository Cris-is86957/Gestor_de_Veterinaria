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

                // Evitamos nulos en textos
                textBox1.Text = fila.Cells["ID_Producto"].Value?.ToString() ?? "";
                nombreProductoActual = fila.Cells["Nombre"].Value?.ToString() ?? "";

                // Conversión segura de números para evitar caídas
                int.TryParse(fila.Cells["Precio_Unidad"].Value?.ToString(), out precioProductoActual);
                int.TryParse(fila.Cells["Stock"].Value?.ToString(), out stockDisponible);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            agregarCarrito();
        }
        private void agregarCarrito()
        {
            // Se verifica que el campo del identificador del producto contenga información antes de procesar.
            if (textBox1.Text != string.Empty)
            {
                // Se intenta convertir el texto ingresado a un valor numérico entero; si falla, se interrumpe la ejecución para evitar errores de formato.
                if (!int.TryParse(textBox1.Text, out int idProducto))
                {
                    MessageBox.Show("El ID del producto no es válido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Se obtiene la cantidad solicitada para la venta desde el control numérico.
                int cantidadAVender = (int)numericUpDown1.Value;

                // Se valida que la cantidad solicitada no supere el inventario disponible del producto seleccionado.
                if (cantidadAVender > stockDisponible)
                {
                    MessageBox.Show($"Solo hay {stockDisponible} unidades disponibles en stock.", "Stock insuficiente", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Se comprueba que la cantidad a vender sea un valor válido y mayor a cero.
                if (cantidadAVender <= 0)
                {
                    MessageBox.Show("La cantidad a vender debe ser mayor a 0.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Se calcula el subtotal de la línea multiplicando la cantidad por el precio unitario del producto.
                int subtotal = cantidadAVender * precioProductoActual;

                // Se añade una nueva fila con los datos consolidados del producto a la estructura de datos en memoria (carrito virtual).
                carritoTemporal.Rows.Add(idProducto, nombreProductoActual, cantidadAVender, precioProductoActual, subtotal);

                // Se invoca el método encargado de recalcular y actualizar el monto total a pagar en la interfaz.
                CalcularTotal();
            }
            else
            {
                // Muestra una advertencia indicando que es obligatorio seleccionar un registro del catálogo antes de añadirlo al carrito.
                MessageBox.Show("Primero selecciona un producto del catálogo haciendo clic en la fila.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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
            procesamientoCompra();
        }
        private void procesamientoCompra()
        {
            // Se comprueba que la estructura de datos en memoria (carrito virtual) contenga al menos un registro antes de procesar la transacción.
            if (carritoTemporal.Rows.Count > 0)
            {
                try
                {
                    // Se inicializa el contexto de la base de datos para abrir la conexión y gestionar la persistencia.
                    using (VeterinariaEntities bd = new VeterinariaEntities())
                    {
                        // Se instancia un nuevo encabezado de comprobante (Boleta).
                        Boleta nuevaBoleta = new Boleta();
                        nuevaBoleta.Rut_Usuario = rutGuardado; // Registra el RUT del usuario autenticado que realiza la venta.
                        nuevaBoleta.fecha = DateTime.Now;     // Captura el registro de fecha y hora exacta del sistema.

                        int totalVenta = 0;

                        // Se recorre cada una de las filas almacenadas de forma temporal en el carrito de compras.
                        foreach (DataRow filaCarrito in carritoTemporal.Rows)
                        {
                            // Se instancia un nuevo objeto para el detalle del documento físico.
                            Ventas nuevoDetalle = new Ventas();
                            // Se mapean los tipos de datos desde la memoria temporal hacia las propiedades de la entidad.
                            nuevoDetalle.ID_Producto = Convert.ToInt32(filaCarrito["ID_Producto"]);
                            nuevoDetalle.Cantidad = Convert.ToInt32(filaCarrito["Cantidad"]);
                            nuevoDetalle.Precio_Unidad = Convert.ToInt32(filaCarrito["Precio"]);

                            // Se acumula incrementalmente el costo total multiplicando cantidad por precio unitario.
                            totalVenta += (nuevoDetalle.Cantidad * nuevoDetalle.Precio_Unidad);

                            // Se asocia y añade de manera lógica la línea de detalle al encabezado de la boleta actual.
                            nuevaBoleta.Ventas.Add(nuevoDetalle);

                            // Se recupera la clave primaria del producto para realizar la actualización física de existencias.
                            int idProd = nuevoDetalle.ID_Producto;
                            var productoBD = bd.Productos.Find(idProd);

                            // Si el producto existe dentro del catálogo maestro, se reduce el inventario disponible.
                            if (productoBD != null)
                            {
                                productoBD.Stock -= nuevoDetalle.Cantidad;
                            }
                        }

                        // Se asigna el monto acumulado global a la cabecera del documento.
                        nuevaBoleta.Total_Venta = totalVenta;

                        // Se prepara el objeto consolidado con todos sus detalles vinculados para persistencia.
                        bd.Boleta.Add(nuevaBoleta);
                        // Se ejecutan y consolidan físicamente las operaciones SQL en la base de datos distribuida.
                        bd.SaveChanges();

                        // Se reestablece el estado inicial de las estructuras de memoria y controles visuales tras un éxito de escritura.
                        carritoTemporal.Clear();
                        CalcularTotal();
                        Actualizar_Catalogo();
                        Actualizar_Historial();
                        textBox1.Text = string.Empty;

                        // Notifica al operador que la inserción integral y rebaja de stock se concretaron de forma correcta.
                        MessageBox.Show("¡Venta concretada con éxito!", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    string errorDetalle = ex.Message;

                    // Bloque de depuración avanzado: Extrae de forma recursiva los mensajes anidados internos de SQL Server si la operación SaveChanges fue rechazada.
                    if (ex.InnerException != null)
                    {
                        errorDetalle += "\n\nMotivo real: " + ex.InnerException.Message;
                        if (ex.InnerException.InnerException != null)
                        {
                            errorDetalle += "\nDetalle exacto de SQL: " + ex.InnerException.InnerException.Message;
                        }
                    }

                    // Expone de forma explícita los errores lógicos, restricciones de clave o fallas críticas en el servidor relacional.
                    MessageBox.Show("Error al guardar la venta:\n" + errorDetalle, "Error Crítico", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                // Muestra un mensaje preventivo si no existen elementos cargados en la grilla temporal al intentar presionar pagar.
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
            comprobante();
        }
        private void comprobante()
        {
            // Se verifica que exista al menos una fila completamente seleccionada en la grilla del historial de boletas.
            if (dataGridView3.SelectedRows.Count > 0)
            {
                // Se extrae de forma segura el valor de la celda correspondiente al identificador (Folio) de la boleta.
                var celdaFolio = dataGridView3.SelectedRows[0].Cells["Folio"].Value;

                // Se valida que la celda no contenga un valor nulo y que pueda ser convertida correctamente a un número entero.
                if (celdaFolio == null || !int.TryParse(celdaFolio.ToString(), out int idBoleta))
                {
                    // Muestra un mensaje de advertencia si el formato del identificador no es válido y aborta la operación.
                    MessageBox.Show("No se pudo leer el número de boleta seleccionado.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                try
                {
                    // Se inicializa el contexto de Entity Framework para abrir la conexión con la base de datos.
                    using (VeterinariaEntities bd = new VeterinariaEntities())
                    {
                        // Se realiza una consulta LINQ para extraer las líneas de detalle de venta asociadas al número de boleta seleccionado.
                        var detalles = from v in bd.Ventas
                                       where v.ID_Venta == idBoleta
                                       select new
                                       {
                                           Producto = v.Productos.Nombre, // Obtiene el nombre del producto mediante la relación de navegación de claves.
                                           v.Cantidad,
                                           v.Precio_Unidad,
                                           Subtotal = v.Cantidad * v.Precio_Unidad // Calcula el costo acumulado por cada línea de artículo.
                                       };

                        // Se inicializa la estructura de texto con el encabezado del reporte visual.
                        string mensaje = $"--- DETALLE DE LA BOLETA N° {idBoleta} ---\n\n";
                        int totalCalculado = 0;

                        // Se recorre cada uno de los elementos devueltos por la consulta para construir el cuerpo del desglose.
                        foreach (var item in detalles)
                        {
                            // Se concatena la información de cantidad, nombre, precio unitario y subtotal formateada.
                            mensaje += $"{item.Cantidad}x {item.Producto} a ${item.Precio_Unidad} c/u  --->  ${item.Subtotal}\n";
                            // Se acumula incrementalmente el monto total de la venta.
                            totalCalculado += item.Subtotal;
                        }

                        // Se añaden líneas divisorias y el cierre del reporte con el monto total final consolidado.
                        mensaje += $"\n----------------------------------\n";
                        mensaje += $"TOTAL PAGADO: ${totalCalculado}";

                        // Muestra la ventana emergente informativa con el desglose completo de la venta procesada.
                        MessageBox.Show(mensaje, "Detalle de Venta", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    // Captura cualquier excepción generada durante la consulta o la conexión y expone el mensaje técnico del fallo.
                    MessageBox.Show("Error al cargar el detalle: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                // Muestra un mensaje preventivo si el operador no ha seleccionado ninguna fila válida en el control visual.
                MessageBox.Show("Por favor, seleccione una boleta del historial haciendo clic en la fila.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            // Se extrae únicamente el componente de la fecha (estableciendo la hora internamente a las 00:00:00) desde el control visual.
            DateTime fechaInicio = dateTimePicker1.Value.Date;

            // Se calcula el límite temporal superior añadiendo exactamente un día (24 horas) a la fecha inicial.
            DateTime fechaFin = fechaInicio.AddDays(1);

            // Se inicializa el contexto de la base de datos para gestionar la conexión y asegurar la liberación de recursos al terminar.
            using (VeterinariaEntities bd = new VeterinariaEntities())
            {
                // Se ejecuta una consulta LINQ para buscar y estructurar el historial de ventas.
                var historialFiltrado = from b in bd.Boleta
                                            // Se filtra la búsqueda para incluir estrictamente los registros que ocurrieron dentro de la ventana temporal de 24 horas.
                                        where b.fecha >= fechaInicio && b.fecha < fechaFin
                                        // Se ordena el listado resultante de manera cronológica descendente (las transacciones más recientes primero).
                                        orderby b.fecha descending
                                        // Se proyectan los datos hacia un objeto anónimo definiendo alias amigables para las columnas de la interfaz.
                                        select new
                                        {
                                            Folio = b.ID_Venta,
                                            Fecha = b.fecha,
                                            Atendido_Por = b.Usuario.nombre, // Extrae el nombre del trabajador navegando a través de la relación de clave foránea.
                                            Total = b.Total_Venta
                                        };

                // Se ejecuta físicamente la consulta en SQL Server, se convierte a una colección de tipo lista y se enlaza a la grilla visual.
                dataGridView3.DataSource = historialFiltrado.ToList();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            filtroFecha();
        }
        private void filtroFecha()
        {
            // Se captura la fecha seleccionada en el control visual, descartando la fracción de tiempo (se fija a las 00:00:00).
            DateTime fechaInicio = dateTimePicker1.Value.Date;

            // Se establece el límite superior del filtro sumando un día completo (24 horas) a la fecha de inicio.
            DateTime fechaFin = fechaInicio.AddDays(1);

            // Se abre una conexión con la base de datos utilizando Entity Framework, asegurando la liberación de memoria al finalizar.
            using (VeterinariaEntities bd = new VeterinariaEntities())
            {
                // Se construye una consulta LINQ para buscar las boletas que cumplan con el criterio de fecha.
                var historialFiltrado = from b in bd.Boleta
                                        // Condición: La fecha de la boleta debe ser mayor o igual al inicio del día y menor al inicio del día siguiente.
                                        where b.fecha >= fechaInicio && b.fecha < fechaFin
                                        // Se ordenan los resultados por fecha de forma descendente (las transacciones más recientes aparecen primero).
                                        orderby b.fecha descending
                                        // Se genera un objeto anónimo con las columnas específicas que se mostrarán en la tabla visual.
                                        select new
                                        {
                                            Folio = b.ID_Venta,
                                            Fecha = b.fecha,
                                            Atendido_Por = b.Usuario.nombre, // Relación de navegación para obtener el nombre del vendedor en lugar de su RUT.
                                            Total = b.Total_Venta
                                        };

                // Se ejecuta la consulta contra la base de datos, se transforma el resultado en una lista y se asigna como origen de datos de la grilla.
                dataGridView3.DataSource = historialFiltrado.ToList();
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            limpiarCarro();
        }
        private void limpiarCarro()
        {
            // Se verifica que la estructura de datos temporal (carrito) contenga al menos un elemento antes de ejecutar la acción.
            if (carritoTemporal.Rows.Count > 0)
            {
                // Se eliminan todas las filas almacenadas en la tabla de memoria, vaciando por completo los registros del carrito.
                carritoTemporal.Clear();

                // Se invoca el método encargado de recalcular el monto a pagar, lo que actualizará la interfaz visual a $0.
                CalcularTotal();

                // Se muestra un mensaje emergente informativo para confirmar al usuario que la operación se realizó con éxito.
                MessageBox.Show("El carrito ha sido vaciado.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        }
    }

