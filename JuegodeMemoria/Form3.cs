using JuegodeMemoria.Data;
using JuegodeMemoria.Modelos;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace JuegodeMemoria
{
    public partial class Form3 : Form
    {
        // Cadena de conexión a la base de datos SQLite
        private string connectionString = @"Data Source=C:\Users\fgfed\Desktop\JuegoDeMemoria\JuegodeMemoria\JuegodeMemoria\DBMemoryGame.db;Version=3;";

        Random random = new Random();
        List<string> icons3 = new List<string>() { "v", "v", "g", "g", "j", "j", "k", "k", "t", "t", "!", "!" };

        Label firstClickedForm3 = null, secondClickedForm3 = null;
        int score3 = 0; // Variable para los puntos en Form3
        int attempts3 = 0; // Variable para los intentos en Form3
        int jugadorId; // Id del jugador

        public Form3(int jugadorId)
        {
            InitializeComponent();
            this.jugadorId = jugadorId; // Asignar el Id del jugador recibido
            AssignIconsToSquares();
        }

        private void Form3_Load(object sender, EventArgs e)
        {

        }

        private void AssignIconsToSquares()
        {
            // Asignar íconos aleatoriamente a los label del panel
            foreach (Control control in tableLayoutPanel3.Controls)
            {
                Label label = control as Label;
                if (label != null)
                {
                    int randomNumber = random.Next(icons3.Count);
                    label.Text = icons3[randomNumber];
                    icons3.RemoveAt(randomNumber);
                }
            }
        }

        private void label_Cl(object sender, EventArgs e)
        {
            if (firstClickedForm3 != null && secondClickedForm3 != null)
                return;

            Label clickedLabel = sender as Label;

            if (clickedLabel == null || clickedLabel.ForeColor == Color.Black)
                return;

            if (firstClickedForm3 == null)
            {
                firstClickedForm3 = clickedLabel;
                firstClickedForm3.ForeColor = Color.Black;
                return;
            }

            secondClickedForm3 = clickedLabel;
            secondClickedForm3.ForeColor = Color.Black;

            CheckForWinner();

            if (firstClickedForm3.Text == secondClickedForm3.Text)
            {
                attempts3++;
                score3++; // Incrementar el puntaje
                firstClickedForm3 = null;
                secondClickedForm3 = null;
            }
            else
            {
                attempts3++; // Incrementar los intentos
                timer3.Start();
            }

            UpdateScoreAndAttempts(); // Actualizar el puntaje y los intentos
        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            // Después del intervalo del temporizador, ocultar las etiquetas que no coinciden
            timer3.Stop();

            firstClickedForm3.ForeColor = firstClickedForm3.BackColor;
            secondClickedForm3.ForeColor = secondClickedForm3.BackColor;

            firstClickedForm3 = null;
            secondClickedForm3 = null;
        }

        private void UpdateScoreAndAttempts()
        {
            lblPuntos3.Text = score3.ToString();
            lblIntentos3.Text = attempts3.ToString();
        }

        private void CheckForWinner()
        {
            foreach (Control control in tableLayoutPanel3.Controls)
            {
                Label label = control as Label;

                if (label != null && label.ForeColor == label.BackColor)
                    return;
            }
            MessageBox.Show("¡Encontraste todas las parejas!");

            // Registrar el nivel en la base de datos
            RegistrarNivel();

            // Cerrar el formulario actual del nivel 3
            this.Hide();

            // Mostrar el formulario del nivel 4
            Form4 level4Form = new Form4(jugadorId); // Asegúrate de que Form4 también reciba el jugadorId
            level4Form.Show();
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

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
                    Numero = 3, // Número del nivel (en este caso nivel 3)
                    Puntaje = score3,
                    Intentos = attempts3,
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
