using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GLASGOW_SIMULATOR
{
    public partial class DeathScreen : UserControl
    {
        public static string result;

        public DeathScreen()
        {
            InitializeComponent();
            InitializeScreen();
        }

        public void InitializeScreen()
        {
            Cursor.Show();
            if(result == "LOSS")
            {
                resultLabel.Text = "YOU LOSE";
                scoreLabel.Text = $"YOU LOST IN {GameScreen.score} SECONDS";
            }
            else if(result == "WIN")
            {
                resultLabel.Text = "YOU WIN";
                scoreLabel.Text = $"YOU WON IN {GameScreen.score} SECONDS";
            }
        }

        private void playAgainButton_Click(object sender, EventArgs e)
        {
            Form1.ChangeScreen(this, new MenuScreen());
        }

        private void exitButton_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
