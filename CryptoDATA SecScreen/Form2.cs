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
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            this.BackColor = Color.Black; // Standardhintergrund
            this.KeyDown += Form2_KeyDown; // Event-Handler zum Schließen bei ESC
        }

        // Event-Handler zum Schließen des Fensters bei ESC-Taste
        private void Form2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }

        public void LoadWebBrowser(string url)
        {
            // Vorhandene Steuerelemente entfernen, um Konflikte zu vermeiden
            this.Controls.Clear();

            WebBrowser webBrowser = new WebBrowser();
            webBrowser.Dock = DockStyle.Fill; // Füllt die gesamte Form aus
            webBrowser.ScriptErrorsSuppressed = true; // Unterdrückt Skriptfehler-Popups
            this.Controls.Add(webBrowser);

            try
            {
                webBrowser.Navigate(url);
            }
            catch (UriFormatException ex)
            {
                MessageBox.Show($"Ungültiges URL-Format: {ex.Message}", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
                // Optional: Fenster schließen oder Fehlermeldung im Fenster anzeigen
                this.Close();
            }
        }

        // Methode zum Laden eines Bildes in einer PictureBox
        public void LoadImage(string imagePath)
        {
            // Vorhandene Steuerelemente entfernen
            this.Controls.Clear();

            PictureBox pictureBox = new PictureBox();
            pictureBox.Dock = DockStyle.Fill; // Füllt die gesamte Form aus
            pictureBox.SizeMode = PictureBoxSizeMode.StretchImage; // Bild skalieren, um in die Box zu passen
            this.Controls.Add(pictureBox);

            try
            {
                // Bild laden
                pictureBox.Image = Image.FromFile(imagePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler beim Laden des Bildes: {ex.Message}", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
                // Optional: Fenster schließen oder Fehlermeldung im Fenster anzeigen
                this.Close();
            }
        }

        public void LoadQrCode(Image qrCodeImage)
        {
            // Vorhandene Steuerelemente entfernen
            this.Controls.Clear();

            PictureBox pictureBox = new PictureBox();
            pictureBox.Dock = DockStyle.Fill;
            pictureBox.SizeMode = PictureBoxSizeMode.Zoom; // Zoom ist hier besser als Stretch, damit der QR-Code nicht verzerrt wird
            pictureBox.BackColor = Color.White; // Wichtig für gute Lesbarkeit des QR-Codes
            this.Controls.Add(pictureBox);

            try
            {
                // Das übergebene Bitmap-Objekt als Bildquelle setzen
                pictureBox.Image = qrCodeImage;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler beim Anzeigen des QR-Codes: {ex.Message}", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }
    }
}