using System;
using System.Collections.Generic;
using System.Windows.Forms;
using JuegodeMemoria.Modelos;
using JuegodeMemoria.Data;

namespace JuegodeMemoria
{
    public partial class Estadisticas : Form
    {
        private int jugadorId;
        private string connectionString = @"Data Source=C:\Users\fgfed\Desktop\JuegoDeMemoria\JuegodeMemoria\JuegodeMemoria\DBMemoryGame.db;Version=3;";

        public Estadisticas(int jugadorId)
        {
            InitializeComponent();
            this.jugadorId = jugadorId;
        }

        private void Estadisticas_Load(object sender, EventArgs e)
        {
            CargarResultados();
        }

        private void CargarResultados()
        {
            try
            {
                // Crear una instancia de NivelDAL
                NivelDAL nivelDAL = new NivelDAL(connectionString);

                // Obtener resultados por jugador
                List<Nivel> resultados = nivelDAL.ObtenerResultadosPorJugador(jugadorId);

                // Limpiar cualquier fila existente en el DataGridView
                dataGridView1.Rows.Clear();

                // Agregar las filas al DataGridView
                foreach (var nivel in resultados)
                {
                    dataGridView1.Rows.Add(nivel.Numero, nivel.Intentos, nivel.Puntaje);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar los resultados: " + ex.Message);
            }
        }

        private void btnFin_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
