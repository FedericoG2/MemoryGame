using JuegodeMemoria.Modelos;
using System.Collections.Generic;
using System.Data.SQLite;

namespace JuegodeMemoria.Data
{
    public class NivelDAL
    {
        private string connectionString;

        public NivelDAL(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public void VerificarYCrearTabla()
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                // Verificar si la tabla Niveles existe
                string sqlVerificarTabla = "SELECT name FROM sqlite_master WHERE type='table' AND name='Niveles'";
                using (var command = new SQLiteCommand(sqlVerificarTabla, connection))
                {
                    var result = command.ExecuteScalar();
                    if (result == null || result.ToString() != "Niveles")
                    {
                        // Si la tabla Niveles no existe, crearla
                        string sqlCrearTabla = "CREATE TABLE Niveles (Id INTEGER PRIMARY KEY AUTOINCREMENT, Numero INTEGER NOT NULL, Puntaje INTEGER NOT NULL, Intentos INTEGER NOT NULL, JugadorId INTEGER NOT NULL, FOREIGN KEY(JugadorId) REFERENCES Jugadores(Id))";
                        using (var createCommand = new SQLiteCommand(sqlCrearTabla, connection))
                        {
                            createCommand.ExecuteNonQuery();
                        }
                    }
                }
            }
        }

        public void InsertarNivel(Nivel nivel)
        {
            // Primero verificar y crear la tabla si no existe
            VerificarYCrearTabla();

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string sql = "INSERT INTO Niveles (Numero, Puntaje, Intentos, JugadorId) VALUES (@Numero, @Puntaje, @Intentos, @JugadorId)";

                using (var command = new SQLiteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Numero", nivel.Numero);
                    command.Parameters.AddWithValue("@Puntaje", nivel.Puntaje);
                    command.Parameters.AddWithValue("@Intentos", nivel.Intentos);
                    command.Parameters.AddWithValue("@JugadorId", nivel.JugadorId);

                    command.ExecuteNonQuery();
                }
            }
        }



        public List<Nivel> ObtenerResultadosPorJugador(int jugadorId)
        {
            List<Nivel> resultados = new List<Nivel>();

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string sql = "SELECT Numero, Puntaje, Intentos FROM Niveles WHERE JugadorId = @JugadorId";

                using (var command = new SQLiteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@JugadorId", jugadorId);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Nivel nivel = new Nivel
                            {
                                Numero = reader.GetInt32(0),
                                Puntaje = reader.GetInt32(1),
                                Intentos = reader.GetInt32(2)
                            };
                            resultados.Add(nivel);
                        }
                    }
                }
            }

            return resultados;
        }




    }
}
