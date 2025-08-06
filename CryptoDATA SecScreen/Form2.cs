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
            this.BackColor = Color.Black;

            // Beispiel-Inhalt: Ein Label
           
            if(Form1.tabControll1 != null)
            // Hinweis zum Schließen des Fensters
            WebBrowser browser = new WebBrowser();
            browser.Dock = DockStyle.Fill;
            browser.Url = new Uri( "https://google.de");
            this.Controls.Add(browser);

            // Event-Handler für Tastendrücke, um das Fenster zu schließen
            this.KeyDown += Form2_KeyDown;
        }

        // Event-Handler zum Schließen des Fensters bei ESC-Taste
        private void Form2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }

        // Passt die Position des Labels an, wenn die Formgröße geändert wird
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            if (this.Controls.Count > 0 && this.Controls[0] is Label contentLabel)
            {
                contentLabel.Location = new Point(
                    (this.ClientSize.Width - contentLabel.Width) / 2,
                    (this.ClientSize.Height - contentLabel.Height) / 2
                );
            }
            if (this.Controls.Count > 1 && this.Controls[1] is Label closeHintLabel)
            {
                closeHintLabel.Location = new Point(
                    (this.ClientSize.Width - closeHintLabel.Width) / 2,
                    this.ClientSize.Height - closeHintLabel.Height - 50
                );
            }

        }
    }
}