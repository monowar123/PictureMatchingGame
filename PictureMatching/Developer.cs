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
    public partial class Developer : Form
    {
        public Developer()
        {
            InitializeComponent();
            label1.Text = "Md.Monowarul Islam";
            label1.Text += "\nCE 09016";
            label1.Text += "\nDept. of CSE";
            label1.Text += "\nMawlana Bhashani Science && Technology University";
            label1.Text += "\nEmail: monowar.mbstu@gmail.com";
            label1.Text += "\nCell: 01558 65 12 05";
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
