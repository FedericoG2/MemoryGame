using JuegodeMemoria.Modelos;
using System;
using System.Data.SQLite;

namespace JuegodeMemoria.Data
{
    public class JugadorDAL
    {
        private string connectionString;

        public JugadorDAL(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public void VerificarYCrearTablaJugadores()
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                // Verificar si la tabla Jugadores existe
                string sqlVerificarTablaJugadores = "SELECT name FROM sqlite_master WHERE type='table' AND name='Jugadores'";
                using (var command = new SQLiteCommand(sqlVerificarTablaJugadores, connection))
                {
                    var result = command.ExecuteScalar();
                    if (result == null || result.ToString() != "Jugadores")
                    {
                        // Si la tabla Jugadores no existe, crearla
                        string sqlCrearTablaJugadores = "CREATE TABLE Jugadores (Id INTEGER PRIMARY KEY AUTOINCREMENT, Nombre TEXT NOT NULL)";
                        using (var createCommand = new SQLiteCommand(sqlCrearTablaJugadores, connection))
                        {
                            createCommand.ExecuteNonQuery();
                        }
                    }
                }
            }
        }

        public void InsertarJugador(Jugador jugador)
        {
            // Primero verificar y crear la tabla Jugadores si no existe
            VerificarYCrearTablaJugadores();

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string sql = "INSERT INTO Jugadores (Nombre) VALUES (@Nombre)";

                using (var command = new SQLiteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Nombre", jugador.Nombre);

                    command.ExecuteNonQuery();
                }
            }
        }

        public int ObtenerIdJugadorPorNombre(string nombreJugador)
        {
            int jugadorId = 0;

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string sql = "SELECT Id FROM Jugadores WHERE Nombre = @Nombre";

                using (var command = new SQLiteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Nombre", nombreJugador);

                    var result = command.ExecuteScalar();
                    if (result != null && int.TryParse(result.ToString(), out jugadorId))
                    {
                        return jugadorId;
                    }
                }
            }

            throw new Exception("No se pudo obtener el Id del jugador.");
        }

    }
}
