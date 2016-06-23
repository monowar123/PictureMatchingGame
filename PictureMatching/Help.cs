using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PictureMatching
{
    public partial class Help : Form
    {
        public Help()
        {
            InitializeComponent();
            label1.Text = "This is a simple Picture Matching Game.";
            label1.Text +="\n\n=> Press Play button to start the game.";
            label1.Text +="\n=> Press Next or Prev button to select the desired picture";
            label1.Text += "\n=> Select the size to devide how many parts.";
            label1.Text += "\n=> Match the picture that is given in the preview.";
            label1.Text += "\n=> If you can match you will be the winner.";
            label1.Text += "\n   Thank You.";
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
