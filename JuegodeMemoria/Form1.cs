using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using JuegodeMemoria.Modelos;
using JuegodeMemoria.Data;

namespace JuegodeMemoria
{
    public partial class Form1 : Form
    {
        // Cadena de conexión a la base de datos SQLite
        private string connectionString = @"Data Source=C:\Users\fgfed\Desktop\JuegoDeMemoria\JuegodeMemoria\JuegodeMemoria\DBMemoryGame.db;Version=3;";
        private Random random = new Random();
        private List<string> icons = new List<string>() { "a", "a", "b", "b", };
        private Label firstClickedForm1 = null, secondClickedForm1 = null;

        private int score = 0; // Variable para los puntos
        private int attempts = 0; // Variable para los intentos
        private int jugadorId; // Id del jugador

        public Form1(int jugadorId)
        {
            InitializeComponent();
            this.jugadorId = jugadorId; // Asignar el Id del jugador recibido
            AssignIconsToSquares();
        }

        private void label_Click(object sender, EventArgs e)
        {
            if (firstClickedForm1 != null && secondClickedForm1 != null)
                return;

            Label clickedLabel = sender as Label;

            if (clickedLabel == null || clickedLabel.ForeColor == Color.Black)
                return;

            if (firstClickedForm1 == null)
            {
                firstClickedForm1 = clickedLabel;
                firstClickedForm1.ForeColor = Color.Black;
                return;
            }

            secondClickedForm1 = clickedLabel;
            secondClickedForm1.ForeColor = Color.Black;

            if (firstClickedForm1.Text == secondClickedForm1.Text)
            {
                score++; // Incrementar el puntaje
                attempts++; // Incrementar los intentos
                firstClickedForm1 = null;
                secondClickedForm1 = null;
            }
            else
            {
                attempts++; // Incrementar los intentos
                timer1.Start();
            }

            UpdateScoreAndAttempts(); // Actualizar el puntaje y los intentos
            CheckForWinner();
        }

        private void AssignIconsToSquares()
        {
            // Asignar íconos aleatoriamente a los label del panel
            foreach (Control control in tableLayoutPanel1.Controls)
            {
                Label label = control as Label;
                if (label != null)
                {
                    int randomNumber = random.Next(icons.Count);
                    label.Text = icons[randomNumber];
                    icons.RemoveAt(randomNumber);
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Stop();

            firstClickedForm1.ForeColor = firstClickedForm1.BackColor;
            secondClickedForm1.ForeColor = secondClickedForm1.BackColor;

            firstClickedForm1 = null;
            secondClickedForm1 = null;
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

            // Pasar el ID del jugador y mostrar el nivel 2
            Form2 level2Form = new Form2(jugadorId);
            level2Form.Show();

            // Cerrar el formulario actual del nivel 1
            this.Hide();
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
                    Numero = 1, // Número del nivel (en este caso nivel 1)
                    Puntaje = score,
                    Intentos = attempts,
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

        private void UpdateScoreAndAttempts()
        {
            labelPuntos.Text = score.ToString();
            labelntentos.Text = attempts.ToString();
        }
    }
}
