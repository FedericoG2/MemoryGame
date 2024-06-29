using JuegodeMemoria.Data;
using JuegodeMemoria.Modelos;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace JuegodeMemoria
{
    public partial class Form2 : Form
    {
        // Cadena de conexión a la base de datos SQLite
        private string connectionString = @"Data Source=C:\Users\fgfed\Desktop\JuegoDeMemoria\JuegodeMemoria\JuegodeMemoria\DBMemoryGame.db;Version=3;";

        Random random = new Random();
        List<string> icons2 = new List<string>() { "q", "q", "w", "w", "r", "r" };

        Label firstClickedForm2 = null, secondClickedForm2 = null;

        int score2 = 0; // Variable para los puntos en Form2
        int attempts2 = 0; // Variable para los intentos en Form2
        int jugadorId; // Id del jugador

        public Form2(int jugadorId)
        {
            InitializeComponent();
            this.jugadorId = jugadorId; // Asignar el Id del jugador recibido
            AssignIconsToSquares();
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void AssignIconsToSquares()
        {
            // Asignar íconos aleatoriamente a los label del panel
            foreach (Control control in tableLayoutPanel2.Controls)
            {
                Label label = control as Label;
                if (label != null)
                {
                    int randomNumber = random.Next(icons2.Count);
                    label.Text = icons2[randomNumber];
                    icons2.RemoveAt(randomNumber);
                }
            }
        }

        private void label_Click(object sender, EventArgs e)
        {
            if (firstClickedForm2 != null && secondClickedForm2 != null)
                return;

            Label clickedLabel = sender as Label;

            if (clickedLabel == null || clickedLabel.ForeColor == Color.Black)
                return;

            if (firstClickedForm2 == null)
            {
                firstClickedForm2 = clickedLabel;
                firstClickedForm2.ForeColor = Color.Black;
                return;
            }

            secondClickedForm2 = clickedLabel;
            secondClickedForm2.ForeColor = Color.Black;

            CheckForWinner();

            if (firstClickedForm2.Text == secondClickedForm2.Text)
            {
                attempts2++;
                score2++; // Incrementar el puntaje
                firstClickedForm2 = null;
                secondClickedForm2 = null;
            }
            else
            {
                attempts2++; // Incrementar los intentos
                timer2.Start();
            }

            UpdateScoreAndAttempts(); // Actualizar el puntaje y los intentos
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            // Después del intervalo del temporizador, ocultar las etiquetas que no coinciden
            timer2.Stop();

            firstClickedForm2.ForeColor = firstClickedForm2.BackColor;
            secondClickedForm2.ForeColor = secondClickedForm2.BackColor;

            firstClickedForm2 = null;
            secondClickedForm2 = null;
        }

        private void UpdateScoreAndAttempts()
        {
            lblPuntos.Text = score2.ToString();
            lblIntentos.Text = attempts2.ToString();
        }

        private void CheckForWinner()
        {
            foreach (Control control in tableLayoutPanel2.Controls)
            {
                Label label = control as Label;

                if (label != null && label.ForeColor == label.BackColor)
                    return;
            }

            MessageBox.Show("¡Encontraste todas las parejas!");

            // Registrar el nivel en la base de datos
            RegistrarNivel();

            // Cerrar el formulario actual del nivel 2
            this.Hide();

            // Mostrar el formulario del nivel 3
            Form3 level3Form = new Form3(jugadorId); // Form3 también recibe el jugadorId
            level3Form.Show();
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
                    Numero = 2, // Número del nivel (en este caso nivel 2)
                    Puntaje = score2,
                    Intentos = attempts2,
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
