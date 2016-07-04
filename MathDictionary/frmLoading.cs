using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace MathDictionary
{
    public partial class frmLoading : Form
    {
        public bool bCanUnload;
        
        public frmLoading()
        {
            InitializeComponent();
        }
        private void frmLoading_Click(object sender, EventArgs e)
        {
            if (this.bCanUnload)
            {
                this.Close();
            }
        }
        private void frmLoading_Load(object sender, EventArgs e)
        {
        }
    }
}