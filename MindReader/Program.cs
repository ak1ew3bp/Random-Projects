using System;
using System.Windows.Forms;
using System.Threading.Tasks;

namespace HelloWorldGUI
{
    public class Program : Form
    {
        private Label lblTitle;
        private Button btnStart;
        private Label lblPrompt;
        private Button btnReveal;
        private Label lblResult;

        private Random random = new Random(); // randomizer

        [STAThread]
        public static void Main()
        {
            Application.EnableVisualStyles();
            Application.Run(new Program());
        }

        public Program()
        {
            // Form setup
            this.Text = "AI Mind Reader";
            this.Size = new System.Drawing.Size(400, 300);

            // Title label
            lblTitle = new Label()
            {
                Text = "I will read your mind!",
                Top = 20,
                Left = 120,
                AutoSize = true,
                Font = new System.Drawing.Font("Arial", 12, System.Drawing.FontStyle.Bold)
            };
            this.Controls.Add(lblTitle);

            // Start button
            btnStart = new Button() { Text = "Start", Top = 60, Left = 150, Width = 100 };
            btnStart.Click += BtnStart_Click;
            this.Controls.Add(btnStart);

            // Prompt label
            lblPrompt = new Label()
            {
                Text = "Think of something...",
                Top = 110,
                Left = 20,
                AutoSize = true,
                Visible = false
            };
            this.Controls.Add(lblPrompt);

            // Reveal button
            btnReveal = new Button()
            {
                Text = "Guess",
                Top = 140,
                Left = 150,
                Width = 100,
                Visible = false
            };
            btnReveal.Click += BtnReveal_Click;
            this.Controls.Add(btnReveal);

            // Result label (for spinner)
            lblResult = new Label()
            {
                Text = "",
                Top = 180,
                Left = 20,
                AutoSize = true
            };
            this.Controls.Add(lblResult);
        }

        private void BtnStart_Click(object sender, EventArgs e)
        {
            lblPrompt.Visible = true;
            btnReveal.Visible = true;
            lblResult.Text = "";
        }

        private async void BtnReveal_Click(object sender, EventArgs e)
        {
            string[] spinner = { "/", "-", "\\", "|" };

            lblResult.Text = "Thinking ";
            for (int i = 0; i < 8; i++) // spin a few times
            {
                lblResult.Text = "Thinking " + spinner[i % spinner.Length];
                await Task.Delay(250); // 0.25 sec per frame
            }

            await Task.Delay(500); // short pause before reveal

            // List of random guesses
            string[] guesses = {
                "Miss mo?",
            };

            string randomGuess = guesses[random.Next(guesses.Length)];

            // Show custom popup on top of form
            Form popup = new Form()
            {
                Text = "AI Mind Reader",
                Size = new System.Drawing.Size(350, 180),
                StartPosition = FormStartPosition.CenterParent, // center on top of parent
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false
            };

            Label message = new Label()
            {
                Text = randomGuess,
                AutoSize = false,
                TextAlign = System.Drawing.ContentAlignment.MiddleCenter,
                Dock = DockStyle.Fill,
                Font = new System.Drawing.Font("Arial", 12, System.Drawing.FontStyle.Bold),
                ForeColor = System.Drawing.Color.DarkRed
            };
            popup.Controls.Add(message);

            Button okButton = new Button()
            {
                Text = "OK",
                Dock = DockStyle.Bottom,
                Height = 40
            };
            okButton.Click += (s, args) => popup.Close();
            popup.Controls.Add(okButton);

            popup.ShowDialog(this); // block main form until closed
        }
    }
}
