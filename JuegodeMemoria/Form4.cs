using JuegodeMemoria.Data;
using JuegodeMemoria.Modelos;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace JuegodeMemoria
{
    public partial class Form4 : Form
    {
        // Cadena de conexión a la base de datos SQLite
        private string connectionString = @"Data Source=C:\Users\fgfed\Desktop\JuegoDeMemoria\JuegodeMemoria\JuegodeMemoria\DBMemoryGame.db;Version=3;";

        Random random = new Random();
        List<string> icons4 = new List<string>() { "h", "h", ",", ",", "g", "g",
                                                   "/", "/", "f", "f", "c", "c",
                                                   "#", "#", "$", "$",};

        Label firstClickedForm4 = null, secondClickedForm4 = null;
        int score4 = 0; // Variable para los puntos en Form4
        int attempts4 = 0; // Variable para los intentos en Form4
        int jugadorId; // Id del jugador

        public Form4(int jugadorId)
        {
            InitializeComponent();
            this.jugadorId = jugadorId; // Asignar el Id del jugador recibido
            AssignIconsToSquares();
        }

        private void Form4_Load(object sender, EventArgs e)
        {

        }

        private void AssignIconsToSquares()
        {
            // Asignar íconos aleatoriamente a los label del panel
            foreach (Control control in tableLayoutPanel1.Controls)
            {
                Label label = control as Label;
                if (label != null)
                {
                    int randomNumber = random.Next(icons4.Count);
                    label.Text = icons4[randomNumber];
                    icons4.RemoveAt(randomNumber);
                }
            }
        }

        private void label_Click(object sender, EventArgs e)
        {
            if (firstClickedForm4 != null && secondClickedForm4 != null)
                return;

            Label clickedLabel = sender as Label;

            if (clickedLabel == null || clickedLabel.ForeColor == Color.Black)
                return;

            if (firstClickedForm4 == null)
            {
                firstClickedForm4 = clickedLabel;
                firstClickedForm4.ForeColor = Color.Black;
                return;
            }

            secondClickedForm4 = clickedLabel;
            secondClickedForm4.ForeColor = Color.Black;

            CheckForWinner();

            if (firstClickedForm4.Text == secondClickedForm4.Text)
            {
                attempts4++;
                score4++; // Incrementar el puntaje
                firstClickedForm4 = null;
                secondClickedForm4 = null;
            }
            else
            {
                attempts4++; // Incrementar los intentos
                timer4.Start();
            }

            UpdateScoreAndAttempts(); // Actualizar el puntaje y los intentos
        }

        private void timer4_Tick(object sender, EventArgs e)
        {
            // Después del intervalo del temporizador, ocultar las etiquetas que no coinciden
            timer4.Stop();

            firstClickedForm4.ForeColor = firstClickedForm4.BackColor;
            secondClickedForm4.ForeColor = secondClickedForm4.BackColor;

            firstClickedForm4 = null;
            secondClickedForm4 = null;
        }

        private void UpdateScoreAndAttempts()
        {
            lblPuntos4.Text = score4.ToString();
            lblIntentos4.Text = attempts4.ToString();
        }

        private void CheckForWinner()
        {
            foreach (Control control in tableLayoutPanel1.Controls)
            {
                Label label = control as Label;

                if (label != null && label.ForeColor == label.BackColor)
                    return;
            }

            MessageBox.Show("¡Encontraste todas las parejas!");

            // Registrar el nivel en la base de datos
            RegistrarNivel();

            // Cerrar el formulario actual del nivel 4
            this.Hide();

            // Mostrar el formulario de estadísticas y pasar el jugadorId
            Estadisticas statsForm = new Estadisticas(jugadorId);
            statsForm.Show();
        }


        private void RegistrarNivel()
        {
            try
            {
                // Crear una instancia de NivelDAL
                NivelDAL nivelDAL = new NivelDAL(connectionString);

                // Crear un objeto Nivel con los datos correspondientes
                Nivel nivel = new Nivel
                {
                    Numero = 4, // Número del nivel (en este caso nivel 4)
                    Puntaje = score4,
                    Intentos = attempts4,
                    JugadorId = jugadorId // Id del jugador
                };

                // Insertar el nivel en la base de datos
                nivelDAL.InsertarNivel(nivel);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al registrar el nivel: " + ex.Message);
            }
        }
    }
}
