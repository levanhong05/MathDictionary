using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace MathDictionary
{
    public partial class frmAbout : Form
    {
        public frmAbout()
        {
            InitializeComponent();
        }

        private void cmdClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void frmAbout_Load(object sender, EventArgs e)
        {
            this.lblVersion.Text = Application.ProductVersion.ToString();
            this.lblDate.Text = QueryDB.szDBDate;
        }
        private void llbWebsite_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(this.llbWebsite.Text);
        }
    }
}
