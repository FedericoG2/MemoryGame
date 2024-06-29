using System;
using System.Data.SQLite;
using System.Windows.Forms;
using JuegodeMemoria.Data;
using JuegodeMemoria.Modelos;

namespace JuegodeMemoria
{
    public partial class Registro : Form
    {
        // Cadena de conexión a la base de datos SQLite
        private string connectionString = @"Data Source=C:\Users\fgfed\Desktop\JuegoDeMemoria\JuegodeMemoria\JuegodeMemoria\DBMemoryGame.db;Version=3;";

        public Registro()
        {
            InitializeComponent();
        }

        private void Registro_Load(object sender, EventArgs e)
        {
            // Crear la conexión a la base de datos SQLite
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                try
                {
                    // Abrir la conexión
                    connection.Open();

                    // Verificar la conexión exitosa
                    if (connection.State == System.Data.ConnectionState.Open)
                    {
                        Console.WriteLine("Conexión exitosa a la base de datos SQLite.");
                    }
                    else
                    {
                        Console.WriteLine("No se pudo conectar a la base de datos SQLite.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error al conectar a la base de datos SQLite: " + ex.Message);
                }
            }
        }

        private void btnJugar_Click(object sender, EventArgs e)
        {
            // Obtener el nombre del jugador desde el TextBox
            string nombreJugador = txtNombre.Text.Trim();

            // Validar que se haya ingresado un nombre
            if (string.IsNullOrEmpty(nombreJugador))
            {
                MessageBox.Show("Ingrese un nombre para el jugador.");
                return;
            }

            // Crear una instancia de JugadorDAL
            JugadorDAL jugadorDAL = new JugadorDAL(connectionString);

            // Crear un objeto Jugador con el nombre ingresado
            Jugador nuevoJugador = new Jugador
            {
                Nombre = nombreJugador
            };

            try
            {
                // Insertar el jugador en la base de datos
                jugadorDAL.InsertarJugador(nuevoJugador);
                MessageBox.Show("Jugador creado correctamente.");

                // Obtener el Id del jugador recién insertado
                int jugadorId = jugadorDAL.ObtenerIdJugadorPorNombre(nombreJugador);

                // Crear una instancia del formulario Form1
                Form1 form1 = new Form1(jugadorId); // Pasa el Id del jugador al constructor de Form1

                // Mostrar el formulario Form1
                form1.Show();

                // Ocultar el formulario actual
                this.Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al crear el jugador: " + ex.Message);
            }
        }
    }
}
