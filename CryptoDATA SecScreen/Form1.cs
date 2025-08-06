using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CryptoDATA_SecScreen
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public int tabIndex = 0;

        private void button1_Click(object sender, EventArgs e)
        {
            // Überprüfen, ob mindestens zwei Monitore angeschlossen sind
            if (Screen.AllScreens.Length < 2)
            {
                MessageBox.Show("Es muss mindestens ein zweiter Monitor angeschlossen sein.");
                return;
            }

            // Den zweiten Bildschirm (Index 1) ermitteln.
            // Beachte: Der erste Bildschirm ist an Index 0.
            Screen secondScreen = Screen.AllScreens[1];

            // Eine neue Instanz von Form2 erstellen
            Form2 secondForm = new Form2();

            // Die Position des neuen Fensters manuell setzen
            secondForm.StartPosition = FormStartPosition.Manual;

            // Die linke obere Ecke des Fensters auf die linke obere Ecke
            // des Arbeitsbereichs des zweiten Monitors setzen.
            // Der Arbeitsbereich ('WorkingArea') ignoriert die Taskleiste.
            secondForm.Location = secondScreen.WorkingArea.Location;

            // Die Größe des Fensters auf die Größe des Arbeitsbereichs des zweiten
            // Monitors setzen, um den gesamten verfügbaren Platz auszufüllen.
            secondForm.Size = secondScreen.WorkingArea.Size;

            // Den Fensterstil auf 'None' setzen, um die Titelleiste und Ränder zu entfernen.
            secondForm.FormBorderStyle = FormBorderStyle.None;

            // Das Fenster maximieren, um sicherzustellen, dass es den gesamten Bereich ausfüllt.
            secondForm.WindowState = FormWindowState.Maximized;

            // Das neue Fenster anzeigen
            secondForm.Show();
        }

    }
}
