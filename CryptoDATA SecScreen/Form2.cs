using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Vlc.DotNet.Forms; // Die stabile Bibliothek für den VLC-Player

namespace CryptoDATA_SecScreen
{
    public partial class Form2 : Form
    {
        // Private Felder, um die zu ladenden Inhalte zu speichern
        private readonly object _contentToLoad;
        private readonly ContentType _contentType;

        // Ein privates Feld für den VLC Player, damit wir ihn später stoppen können
        private VlcControl _vlcControl;

        // Dein Enum, um die verschiedenen Inhalts-Typen zu definieren
        public enum ContentType
        {
            Website,
            Image,
            QrCode,
            Video
        }

        /// <summary>
        /// Konstruktor für Inhalte, die über einen Dateipfad geladen werden (Bild, Video, Webseite).
        /// </summary>
        public Form2(string path, ContentType type)
        {
            InitializeComponent();
            _contentToLoad = path;
            _contentType = type;
            SetupForm();
        }

        /// <summary>
        /// Konstruktor für Inhalte, die direkt als Bild-Objekt übergeben werden (QR-Code).
        /// </summary>
        public Form2(Image image, ContentType type)
        {
            InitializeComponent();
            _contentToLoad = image;
            _contentType = type;
            SetupForm();
        }

        /// <summary>
        /// Grundlegende Einrichtung des Fensters.
        /// </summary>
        private void SetupForm()
        {
            this.BackColor = Color.Black;
            this.KeyDown += Form2_KeyDown;
            this.Load += Form2_Load; // Der entscheidende Event-Handler
        }

        /// <summary>
        /// Diese Methode wird aufgerufen, sobald das Fenster vollständig geladen ist.
        /// Hier wird der eigentliche Inhalt initialisiert.
        /// </summary>
        private void Form2_Load(object sender, EventArgs e)
        {
            // Je nach Inhaltstyp die passende Methode aufrufen
            switch (_contentType)
            {
                case ContentType.Video:
                    InitializeVideo();
                    break;
                case ContentType.Image:
                    InitializeImage();
                    break;
                case ContentType.QrCode:
                    InitializeQrCode();
                    break;
                case ContentType.Website:
                    InitializeWebBrowser();
                    break;
            }
        }

        // --- Initialisierungs-Methoden für jeden Inhaltstyp ---

        // ##################################################################
        // ## HIER IST DIE FINALE, KORRIGIERTE METHODE FÜR DEN VLC-PLAYER ##
        // ##################################################################
        private void InitializeVideo()
        {
            try
            {
                string videoPath = _contentToLoad as string;
                if (string.IsNullOrEmpty(videoPath) || !File.Exists(videoPath))
                {
                    throw new FileNotFoundException("Die Videodatei wurde nicht gefunden.", videoPath);
                }

                string programFilesPath = Environment.GetEnvironmentVariable("ProgramW6432");

                if (string.IsNullOrEmpty(programFilesPath))
                {
                    // Fallback für 32-Bit-Systeme
                    programFilesPath = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
                }
                var vlcPath = Path.Combine(programFilesPath, "VideoLAN", "VLC");
                DirectoryInfo vlcLibDirectory = null;

                if (Directory.Exists(vlcPath))
                {
                    vlcLibDirectory = new DirectoryInfo(vlcPath);
                }

                // Wenn kein Pfad gefunden wurde, gib eine klare, hilfreiche Fehlermeldung aus
                if (vlcLibDirectory == null)
                {
                    string errorMessage = "Der VLC Media Player konnte nicht gefunden werden.\n\n" +
                                          "Das Programm hat in den folgenden Verzeichnissen gesucht:\n" +
                                          $"- {vlcPath}\n" +
                                          "Lösung: Bitte stellen Sie sicher, dass VLC (64-bit empfohlen) im Standardverzeichnis installiert ist.";
                    throw new DirectoryNotFoundException(errorMessage);
                }

                // --- SCHRITT 2: Erstelle und konfiguriere den Player ---
                _vlcControl = new VlcControl();
                _vlcControl.BeginInit();

                // Dem Player sagen, wo er die DLLs findet (der entscheidende Schritt!)
                _vlcControl.VlcLibDirectory = vlcLibDirectory;

                _vlcControl.Dock = DockStyle.Fill;
                this.Controls.Add(_vlcControl);
                _vlcControl.EndInit();

                // --- SCHRITT 3: Video abspielen ---
                _vlcControl.Play(new FileInfo(videoPath));
            }
            catch (Exception ex)
            {
                ShowError("Fehler beim Abspielen des Videos", ex);
            }
        }

        private void InitializeImage()
        {
            try
            {
                string imagePath = _contentToLoad as string;
                PictureBox picBox = new PictureBox
                {
                    Dock = DockStyle.Fill,
                    SizeMode = PictureBoxSizeMode.Zoom,
                    Image = Image.FromFile(imagePath)
                };
                this.Controls.Add(picBox);
            }
            catch (Exception ex)
            {
                ShowError("Fehler beim Laden des Bildes", ex);
            }
        }

        private void InitializeQrCode()
        {
            try
            {
                Image qrImage = _contentToLoad as Image;
                PictureBox picBox = new PictureBox
                {
                    Dock = DockStyle.Fill,
                    SizeMode = PictureBoxSizeMode.Zoom,
                    BackColor = Color.White,
                    Image = qrImage
                };
                this.Controls.Add(picBox);
            }
            catch (Exception ex)
            {
                ShowError("Fehler beim Anzeigen des QR-Codes", ex);
            }
        }

        private void InitializeWebBrowser()
        {
            try
            {
                string url = _contentToLoad as string;
                WebBrowser webBrowser = new WebBrowser
                {
                    Dock = DockStyle.Fill,
                    ScriptErrorsSuppressed = true
                };
                webBrowser.Navigate(url);
                this.Controls.Add(webBrowser);
            }
            catch (Exception ex)
            {
                ShowError("Fehler beim Laden der Webseite", ex);
            }
        }


        // --- Hilfsmethoden ---

        /// <summary>
        /// Schließt das Fenster bei Drücken der ESC-Taste und gibt Ressourcen frei.
        /// </summary>
        private void Form2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close(); // Das Dispose-Event wird automatisch aufgerufen
            }
        }

        /// <summary>
        /// Gibt Ressourcen sauber frei, wenn das Fenster geschlossen wird.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _vlcControl?.Dispose();
                components?.Dispose();
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Zeigt eine einheitliche Fehlermeldung an und schließt das Fenster.
        /// </summary>
        private void ShowError(string title, Exception ex)
        {
            MessageBox.Show(ex.Message, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
            // Wir stellen sicher, dass auch bei einem Fehler die Ressourcen freigegeben werden, bevor das Fenster schließt
            if (!this.IsDisposed)
            {
                this.Close();
            }
        }
    }
}