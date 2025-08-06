using QRCoder;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace CryptoDATA_SecScreen
{
    public partial class Form1 : Form
    {
        Form2 secondForm = null;
        public Form1()
        {
            InitializeComponent();
            tabControl1.SelectTab(0);
            this.FormClosing += Form1_FormClosing; // Handler für sauberes Beenden
        }

        /// <summary>
        /// Stellt sicher, dass die gesamte Anwendung beendet wird, wenn das Hauptfenster geschlossen wird.
        /// </summary>
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        /// <summary>
        /// Der zentrale Button-Click-Handler, der das zweite Fenster öffnet.
        /// </summary>
        private void button1_Click(object sender, EventArgs e)
        {
            // Prüfen, ob ein zweiter Monitor vorhanden ist
            if (Screen.AllScreens.Length < 2)
            {
                MessageBox.Show("Es muss mindestens ein zweiter Monitor angeschlossen sein.", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Den zweiten Bildschirm ermitteln
            Screen secondScreen = Screen.AllScreens[1];
            

            // Logik basierend auf dem ausgewählten Tab
            switch (tabControl1.SelectedIndex)
            {
                // --- Case 0: Website ---
                case 0:
                    string url = textBox1.Text;
                    if (!string.IsNullOrWhiteSpace(url) && Uri.IsWellFormedUriString(url, UriKind.Absolute))
                    {
                        secondForm = new Form2(url, Form2.ContentType.Website);
                    }
                    else
                    {
                        MessageBox.Show("Bitte geben Sie eine gültige URL ein (z.B. https://google.com).", "Hinweis", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    break;

                // --- Case 1: Bild ---
                case 1:
                    string imagePath = openFileDialog1.FileName;
                    if (!string.IsNullOrWhiteSpace(imagePath) && File.Exists(imagePath))
                    {
                        secondForm = new Form2(imagePath, Form2.ContentType.Image);
                    }
                    else
                    {
                        MessageBox.Show("Bitte wählen Sie zuerst eine gültige Bilddatei aus.", "Hinweis", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    break;

                // --- Case 2: QR-Code ---
                case 2:
                    if (pictureBox2.Image != null)
                    {
                        secondForm = new Form2(pictureBox2.Image, Form2.ContentType.QrCode);
                    }
                    else
                    {
                        MessageBox.Show("Es wurde kein QR-Code generiert. Bitte geben Sie Text ein.", "Hinweis", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    break;

                // --- Case 3: Video ---
                case 3:
                    // Annahme: Du hast eine TextBox namens 'textBox3' für den Videopfad
                    string videoPath = textBox3.Text;
                    if (!string.IsNullOrWhiteSpace(videoPath) && File.Exists(videoPath))
                    {
                        axWindowsMediaPlayer1.Ctlcontrols.stop();
                        secondForm = new Form2(videoPath, Form2.ContentType.Video);
                    }
                    else
                    {
                        MessageBox.Show("Bitte wählen Sie zuerst eine gültige Videodatei aus.", "Hinweis", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    break;
            }

            // Wenn ein secondForm-Objekt erfolgreich erstellt wurde, zeige es an
            if (secondForm != null)
            {
                secondForm.StartPosition = FormStartPosition.Manual;
                secondForm.Location = secondScreen.WorkingArea.Location;
                secondForm.Size = secondScreen.WorkingArea.Size;
                secondForm.FormBorderStyle = FormBorderStyle.None;
                secondForm.WindowState = FormWindowState.Maximized;
                secondForm.ShowInTaskbar = false;
                secondForm.Show();
            }
        }

        /// <summary>
        /// Öffnet den Dialog zur Auswahl einer Bilddatei.
        /// </summary>
        private void button2_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "Bilddateien|*.jpg;*.jpeg;*.png;*.gif;*.bmp|Alle Dateien|*.*";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.ImageLocation = openFileDialog1.FileName;
            }
        }

        /// <summary>
        /// Generiert bei jeder Textänderung einen neuen QR-Code.
        /// </summary>
        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox2.Text))
            {
                pictureBox2.Image = null;
                return;
            }

            try
            {
                QRCodeGenerator qrGenerator = new QRCodeGenerator();
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(textBox2.Text, QRCodeGenerator.ECCLevel.Q);
                QRCode qrCode = new QRCode(qrCodeData);
                Bitmap qrCodeImage = qrCode.GetGraphic(20);
                pictureBox2.Image = qrCodeImage;
            }
            catch (Exception)
            {
                // Fängt Fehler bei zu viel Text für QR-Code ab
                pictureBox2.Image = null;
            }
        }

        /// <summary>
        /// Öffnet den Dialog zur Auswahl einer Videodatei.
        /// Annahme: Du hast einen Button namens 'button3' im Video-Tab.
        /// </summary>
        private void button3_Click(object sender, EventArgs e)
        {
            // Annahme: Du verwendest einen zweiten openFileDialog namens 'openFileDialog2' für Videos
            openFileDialog2.Filter = "Videodateien|*.mp4;*.mkv;*.avi;*.wmv|Alle Dateien|*.*";
            if (openFileDialog2.ShowDialog() == DialogResult.OK)
            {
                textBox3.Text = openFileDialog2.FileName; // Zeigt den Pfad in einer TextBox an
                                                          // Optional: Video-Vorschau im Hauptfenster
                axWindowsMediaPlayer1.URL = openFileDialog2.FileName;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            bool isForm2Open = Application.OpenForms.OfType<Form2>().Any();
            if (isForm2Open) { 
            button1.Enabled = false;
                button4.Enabled = true;
            } else
            {
                
                
                button1.Enabled = true;
                button4.Enabled = false;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            secondForm.Close();
        }
    }
}