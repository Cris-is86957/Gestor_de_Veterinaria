using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Diego_Herrera___Prueba_2
{
    public partial class Dueños : Form
    {
        private string rolGuardado = "";
        private string rutGuardado = "";
        public Dueños(string rolDelUsuario, string rutDelUsuario)
        {
            InitializeComponent();
            rolGuardado = rolDelUsuario;
            rutGuardado = rutDelUsuario;
        }

        private void Dueños_Load(object sender, EventArgs e)
        {
            Actualizar_Datos();
            Limpiar_Datos();
            Ingresar_Estado();
        }
        private void Actualizar_Datos()
        {
            using (VeterinariaEntities bd = new VeterinariaEntities())
            {
                var query = from miDueño in bd.Dueño
                            select new
                            {
                                miDueño.Rut_dueño,
                                miDueño.Estado_Dueño,
                                miDueño.nombre,
                                miDueño.apell_pat,
                                miDueño.apell_mat,
                                

                            };
                dataGridView1.DataSource = query.ToList();
                dataGridView1.Refresh();
            }

        }
        private void Ingresar_Estado()
        {

            var opcionesEstado = new[] {
                new { Id = "Activo", Nombre = "Activo" },
                new { Id = "Inactivo", Nombre = "Inactivo" },
                
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
            registrarDueño();

        }
        private void registrarDueño()
        {
            // Se valida que todos los controles de entrada de texto y selección contengan información antes de procesar.
            if (textBox1.Text != string.Empty &&
                textBox2.Text != string.Empty &&
                textBox3.Text != string.Empty &&
                textBox4.Text != string.Empty &&
                comboBox1.Text != string.Empty
                )
            {
                // Se instancia un nuevo objeto de la entidad Dueño.
                Dueño nuevoDueño = new Dueño();

                // Se mapean y asignan los valores de la interfaz gráfica a las propiedades del objeto.
                nuevoDueño.Rut_dueño = textBox1.Text;
                nuevoDueño.Estado_Dueño = comboBox1.SelectedValue.ToString();
                nuevoDueño.nombre = textBox2.Text;
                nuevoDueño.apell_pat = textBox3.Text;
                nuevoDueño.apell_mat = textBox4.Text;

                // Se inicializa el contexto de la base de datos liberando los recursos al finalizar el bloque.
                using (VeterinariaEntities bd = new VeterinariaEntities())
                {
                    // Se realiza una consulta a la base de datos para verificar si el RUT ingresado ya existe.
                    if (bd.Dueño.Any(d => d.Rut_dueño == textBox1.Text.Trim()))
                    {
                        // Muestra advertencia de duplicidad y aborta la operación de guardado.
                        MessageBox.Show("El RUT ingresado ya está registrado en el sistema.", "RUT Duplicado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    try
                    {
                        // Se prepara el objeto para ser insertado en la tabla Dueño.
                        bd.Dueño.Add(nuevoDueño);
                        // Se ejecutan y confirman los cambios físicos en la base de datos.
                        bd.SaveChanges();

                        // Se recarga la información de la tabla visual y se vacían los cuadros de texto.
                        Actualizar_Datos();
                        Limpiar_Datos();
                        // Notifica que la inserción se realizó correctamente.
                        MessageBox.Show("Dueño registrado con éxito.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        // Intercepta errores de Entity Framework o de SQL Server y expone el mensaje técnico.
                        MessageBox.Show("Error al guardar en la base de datos: " + ex.Message, "Error Crítico", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            borrar();
        }
        private void borrar()
        {
            // Se valida que el campo del identificador (RUT) no esté vacío antes de proceder.
            if (textBox1.Text != string.Empty)
            {
                // Se inicializa el contexto de Entity Framework para la conexión con la base de datos.
                using (VeterinariaEntities bd = new VeterinariaEntities())
                {
                    // Se busca el registro del dueño utilizando el valor ingresado como clave primaria.
                    var nuevoDueño = bd.Dueño.Find(textBox1.Text);

                    // Se verifica si el registro existe en la base de datos.
                    if (nuevoDueño != null)
                    {
                        try
                        {
                            // Se remueve el objeto del conjunto de datos de la entidad.
                            bd.Dueño.Remove(nuevoDueño);
                            // Se confirman y aplican los cambios de forma física en la base de datos.
                            bd.SaveChanges();

                            // Se actualiza la grilla visual con los datos vigentes.
                            Actualizar_Datos();
                            // Se limpian los campos de la interfaz y se restablecen los controles.
                            Limpiar_Datos();
                            // Informa al usuario que el registro fue removido exitosamente.
                            MessageBox.Show("Dueño eliminado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        catch (Exception)
                        {
                            // Captura la excepción si el registro está enlazado a otra tabla (Clave Foránea) e impide la caída del sistema.
                            MessageBox.Show("No se puede eliminar este dueño porque tiene mascotas asociadas en el sistema. Considere cambiar su estado a 'Inactivo' en su lugar.", "Error de integridad", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        }
                    }
                }
            }
            // ...
        }

        private void button3_Click(object sender, EventArgs e)
        {
            sobreescribir();
        }
        private void sobreescribir()
        {
            // Se verifica que el campo del identificador (RUT) contenga información antes de intentar realizar la modificación.
            if (textBox1.Text != string.Empty)
            {
                // Se inicializa el contexto de la base de datos para manejar la conexión y liberar los recursos automáticamente.
                using (VeterinariaEntities bd = new VeterinariaEntities())
                {
                    // Se realiza la búsqueda del registro correspondiente en la base de datos utilizando la clave primaria.
                    var nuevoDueño = bd.Dueño.Find(textBox1.Text);

                    // Se comprueba que el registro a modificar haya sido encontrado exitosamente en la base de datos.
                    if (nuevoDueño != null)
                    {
                        // Se sobrescriben las propiedades del objeto recuperado con los nuevos valores ingresados en la interfaz gráfica.
                        nuevoDueño.Rut_dueño = textBox1.Text;
                        nuevoDueño.Estado_Dueño = comboBox1.SelectedValue.ToString();
                        nuevoDueño.nombre = textBox2.Text;
                        nuevoDueño.apell_pat = textBox3.Text;
                        nuevoDueño.apell_mat = textBox4.Text;

                        // Se ejecutan y aplican las actualizaciones de los datos físicamente en la base de datos.
                        bd.SaveChanges();

                        // Se refresca la grilla visual para mostrar los datos actualizados.
                        Actualizar_Datos();
                        // Se restablecen y vacían los controles de la interfaz.
                        Limpiar_Datos();
                    }
                }
            }
            else
            {
                // Muestra un mensaje de advertencia al usuario indicando que es obligatorio seleccionar un registro para modificarlo.
                MessageBox.Show("Debe seleccionar una fila para modificar.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void dataGridView1_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            
            if (e.RowIndex >= 0)
            {
                DataGridViewRow Fila = dataGridView1.Rows[e.RowIndex];

                textBox1.Text = Fila.Cells["Rut_dueño"].Value?.ToString() ?? "";
                textBox2.Text = Fila.Cells["nombre"].Value?.ToString() ?? "";
                textBox3.Text = Fila.Cells["apell_pat"].Value?.ToString() ?? "";
                textBox4.Text = Fila.Cells["apell_mat"].Value?.ToString() ?? "";
                comboBox1.Text = Fila.Cells["Estado_Dueño"].Value?.ToString() ?? "";

                
                textBox1.ReadOnly = true;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Form1 menuPrincipal = new Form1(rolGuardado, rutGuardado);
            menuPrincipal.Show();
            this.Close();
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {

        }
        private void txtSoloLetras_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Permitir letras, espacios y teclas de control (borrar, flechas)
            if (!char.IsLetter(e.KeyChar) && !char.IsControl(e.KeyChar) && e.KeyChar != ' ')
            {
                e.Handled = true; // Ignora la tecla si es un número o símbolo
            }
        }
    }
}
        
