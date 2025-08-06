using System;
using System.IO;
using System.Windows.Forms;

namespace CryptoDATA_SecScreen
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            tabControl1.SelectTab(0);
        }

        // Event-Handler für den Haupt-Button "button1"
        private void button1_Click(object sender, EventArgs e)
        {
            // Überprüfen, ob mindestens zwei Monitore angeschlossen sind
            if (Screen.AllScreens.Length < 2)
            {
                MessageBox.Show("Es muss mindestens ein zweiter Monitor angeschlossen sein, um das Fenster anzuzeigen.");
                return;
            }

            // Den zweiten Bildschirm (Index 1) ermitteln.
            Screen secondScreen = Screen.AllScreens[1];

            // Eine neue Instanz von Form2 erstellen
            Form2 secondForm = new Form2();

            // Die Position des neuen Fensters manuell setzen
            secondForm.StartPosition = FormStartPosition.Manual;
            secondForm.Location = secondScreen.WorkingArea.Location;
            secondForm.Size = secondScreen.WorkingArea.Size;
            secondForm.FormBorderStyle = FormBorderStyle.None;
            secondForm.WindowState = FormWindowState.Maximized;

            // Inhalt an Form2 übergeben, basierend auf der ausgewählten Registerkarte
            if (tabControl1.TabIndex == 0)
            {
                // Browser-Tab ist ausgewählt
                string url = textBox1.Text;
                if (!string.IsNullOrWhiteSpace(url))
                {
                    secondForm.LoadWebBrowser(url);
                    secondForm.Show();
                }
                else
                {
                    MessageBox.Show("Bitte geben Sie eine URL ein.");
                }
            }
            else if (tabControl1.TabIndex == 1)
            {
                // Bild-Tab ist ausgewählt
                if (!string.IsNullOrWhiteSpace(openFileDialog1.FileName))
                {
                    secondForm.LoadImage(openFileDialog1.FileName);
                    secondForm.Show();
                }
                else
                {
                    MessageBox.Show("Bitte wählen Sie ein Bild aus.");
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK) { 
            if(File.Exists(openFileDialog1.FileName))
                {
                    pictureBox1.ImageLocation = openFileDialog1.FileName;
                }
            }
        }
    }
}
