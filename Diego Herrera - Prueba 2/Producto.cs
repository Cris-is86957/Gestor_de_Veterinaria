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
    public partial class Producto : Form
    {
        private string rolGuardado;
        private string rutGuardado;
        public Producto(string rolDelUsuario, string rutDelUsuario)
        {
            InitializeComponent();
            rolGuardado = rolDelUsuario;
            rutGuardado = rutDelUsuario;
        }

        private void Producto_Load(object sender, EventArgs e)
        {
            Actualizar_Datos();
            Limpiar_Datos();
            Ingresar_Estado();
        }
        private void Actualizar_Datos()
        {
            using (VeterinariaEntities bd = new VeterinariaEntities())
            {
                var query = from miProducto in bd.Productos
                            select new
                            {
                                miProducto.ID_Producto,
                                miProducto.Estado_Producto,
                                miProducto.Nombre,
                                miProducto.Stock,
                                miProducto.Precio_Unidad,


                            };
                dataGridView1.DataSource = query.ToList();
                dataGridView1.Refresh();
            }

        }
        private void Ingresar_Estado()
        {

            var opcionesEstado = new[] {
                new { Id = "Disponible", Nombre = "Disponible" },
                new { Id = "Sin Stock", Nombre = "Sin stock" },

    };


            comboBox1.DataSource = opcionesEstado.ToList();
            comboBox1.DisplayMember = "Nombre";
            comboBox1.ValueMember = "Id";
            comboBox1.SelectedIndex = -1;
        }
        private void Limpiar_Datos()
        {
            textBox1.Text = string.Empty;
            textBox2.Text = string.Empty;
            textBox3.Text = string.Empty;
            textBox4.Text = string.Empty;
            comboBox1.SelectedIndex = -1;

            textBox1.ReadOnly = false;


        }

        private void button1_Click(object sender, EventArgs e)
        {
            registrarProducto();
            
        }
        private void registrarProducto()
        {
            // Se valida que todos los cuadros de texto y el menú desplegable de estado contengan información antes de iniciar el proceso.
            if (textBox1.Text != string.Empty && textBox2.Text != string.Empty &&
                textBox3.Text != string.Empty && textBox4.Text != string.Empty &&
                comboBox1.SelectedIndex != -1)
            {
                // Se intenta convertir de forma segura los textos correspondientes al ID, Stock y Precio hacia valores numéricos enteros.
                // Si el usuario ingresa caracteres no numéricos, el programa aborta la ejecución y muestra una alerta, evitando que la aplicación falle.
                if (!int.TryParse(textBox1.Text, out int id) ||
                    !int.TryParse(textBox3.Text, out int stock) ||
                    !int.TryParse(textBox4.Text, out int precio))
                {
                    MessageBox.Show("El ID, el Stock y el Precio deben ser números válidos sin letras.", "Error de formato", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Se inicializa el contexto de Entity Framework para establecer la conexión con la base de datos y liberar los recursos al terminar.
                using (VeterinariaEntities bd = new VeterinariaEntities())
                {
                    // Se consulta la base de datos para verificar si el ID del producto ingresado ya existe.
                    // Esto protege la integridad de la base de datos previniendo la duplicidad de claves primarias.
                    if (bd.Productos.Any(p => p.ID_Producto == id))
                    {
                        MessageBox.Show("Ya existe un producto con ese ID en el sistema.", "ID Duplicado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    try
                    {
                        // Se instancia un nuevo objeto de la entidad Productos.
                        Productos nuevoProducto = new Productos();

                        // Se asignan a las propiedades del objeto los valores extraídos y sanitizados desde la interfaz gráfica.
                        nuevoProducto.ID_Producto = id;
                        // Se utiliza SelectedValue para capturar el dato estructurado del desplegable en lugar del texto libre.
                        nuevoProducto.Estado_Producto = comboBox1.SelectedValue.ToString();
                        nuevoProducto.Nombre = textBox2.Text;
                        nuevoProducto.Stock = stock;
                        nuevoProducto.Precio_Unidad = precio;

                        // Se añade el nuevo registro al contexto de datos.
                        bd.Productos.Add(nuevoProducto);
                        // Se ejecutan y confirman los cambios físicamente en el servidor SQL (instrucción INSERT).
                        bd.SaveChanges();

                        // Se recarga la tabla visual del inventario y se vacían los cuadros de texto.
                        Actualizar_Datos();
                        Limpiar_Datos();
                        // Se notifica al operador que el registro se completó de manera exitosa.
                        MessageBox.Show("Producto registrado con éxito.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        // Captura cualquier falla en la conexión o restricción inesperada en la base de datos y expone el mensaje técnico.
                        MessageBox.Show("Error al guardar: " + ex.Message, "Error Crítico", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                // Se emite una alerta preventiva si el usuario intentó guardar dejando campos obligatorios en blanco.
                MessageBox.Show("Debe completar todos los campos.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            borrar();
        }
        private void borrar()
        {
            // Se verifica que el campo correspondiente al identificador (ID) del producto no esté vacío antes de intentar la eliminación.
            if (textBox1.Text != string.Empty)
            {
                // Se inicializa el contexto de la base de datos, garantizando la apertura y posterior cierre automático de la conexión.
                using (VeterinariaEntities bd = new VeterinariaEntities())
                {
                    // Se convierte el texto del ID a un número entero y se busca el registro exacto del producto utilizando su clave primaria.
                    var nuevoProducto = bd.Productos.Find(int.Parse(textBox1.Text));

                    // Se comprueba que la búsqueda haya devuelto un resultado válido (que el producto realmente exista en la base de datos).
                    if (nuevoProducto != null)
                    {
                        // Se marca el objeto recuperado para ser eliminado de forma lógica en el contexto de Entity Framework.
                        bd.Productos.Remove(nuevoProducto);
                        // Se ejecutan y aplican los cambios físicamente en el servidor SQL (instrucción DELETE).
                        bd.SaveChanges();

                        // Se recarga la tabla visual del catálogo para reflejar la eliminación del artículo.
                        Actualizar_Datos();
                        // Se restablecen los controles de texto de la interfaz a su estado original (vacíos).
                        Limpiar_Datos();
                    }
                }
            }
            else
            {
                // Muestra un mensaje de error advirtiendo al operador que es obligatorio seleccionar un registro del catálogo antes de presionar eliminar.
                MessageBox.Show("Se debe seleccionar una fila para eliminar.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }


        private void button3_Click(object sender, EventArgs e)
        {
            sobreescribir();
            
        }
        private void sobreescribir()
        {
            // Se verifica que el campo correspondiente al identificador (ID) del producto no esté vacío para asegurar que un elemento ha sido seleccionado.
            if (textBox1.Text != string.Empty)
            {
                // Se inicializa el contexto de Entity Framework para manejar la conexión a la base de datos y liberar los recursos automáticamente al terminar.
                using (VeterinariaEntities bd = new VeterinariaEntities())
                {
                    // Se convierte el texto del ID a un número entero y se busca el registro exacto del producto en la base de datos mediante su clave primaria.
                    var nuevoProducto = bd.Productos.Find(int.Parse(textBox1.Text));

                    // Se comprueba que la consulta haya encontrado exitosamente el registro en el catálogo.
                    if (nuevoProducto != null)
                    {
                        // Se sobrescriben las propiedades del objeto recuperado con los nuevos valores ingresados en la interfaz gráfica.
                        nuevoProducto.ID_Producto = int.Parse(textBox1.Text);
                        nuevoProducto.Estado_Producto = comboBox1.Text;
                        nuevoProducto.Nombre = textBox2.Text;
                        nuevoProducto.Stock = int.Parse(textBox3.Text); // Convierte el texto del stock a un valor numérico.
                        nuevoProducto.Precio_Unidad = int.Parse(textBox4.Text); // Convierte el texto del precio a un valor numérico.

                        // Se ejecutan y consolidan los cambios físicamente en el servidor de base de datos (instrucción UPDATE).
                        bd.SaveChanges();

                        // Se recarga la tabla visual del inventario para reflejar las modificaciones.
                        Actualizar_Datos();
                        // Se vacían los cuadros de texto para dejar el formulario en su estado inicial.
                        Limpiar_Datos();
                    }
                }
            }
            else
            {
                // Se emite un mensaje de error advirtiendo al operador que es obligatorio seleccionar un registro antes de presionar el botón de modificar.
                MessageBox.Show("Debe seleccionar una fila para modificar.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView1_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow Fila = dataGridView1.Rows[e.RowIndex];

                textBox1.Text = Fila.Cells["ID_Producto"].Value?.ToString() ?? "";
                comboBox1.Text = Fila.Cells["Estado_Producto"].Value?.ToString() ?? "";
                textBox2.Text = Fila.Cells["Nombre"].Value?.ToString() ?? "";
                textBox3.Text = Fila.Cells["Stock"].Value?.ToString() ?? "";
                textBox4.Text = Fila.Cells["Precio_Unidad"].Value?.ToString() ?? "";

                // Bloqueamos el cuadro del ID para que no rompan la búsqueda
                textBox1.ReadOnly = true;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Form1 menuPrincipal = new Form1(rolGuardado, rutGuardado);
            menuPrincipal.Show();
            this.Close();
        }
        private void txtSoloNumeros_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Permitir solo dígitos y la tecla de borrar
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }
    }
}
