using QRCoder;
using System;
using System.Drawing;
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
            this.FormClosing += Form1_FormClosing; // Handler hinzufügen
        }

        // Neue Methode in Form1.cs
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit(); // Beendet die gesamte Anwendung
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
            secondForm.ShowInTaskbar = false;
            // Inhalt an Form2 übergeben, basierend auf der ausgewählten Registerkarte
            switch (tabControl1.SelectedIndex)
            {
                case 0:
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
                    break;
                case 1:
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
                    
                    break;
                case 2:

                    secondForm.LoadQrCode(pictureBox2.Image);
                    secondForm.Show();
                    break;
                default:
                    break;
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

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            // QR-Code generieren
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(textBox2.Text, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            Bitmap qrCodeImage = qrCode.GetGraphic(20); // Die Zahl (20) ist die Pixelgröße pro Modul
            pictureBox2.Image = qrCodeImage;
        }
    }
}
