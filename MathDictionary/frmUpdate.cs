using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace MathDictionary
{
    public partial class frmUpdate : Form
    {
        public frmUpdate()
        {
            InitializeComponent();
            this.Icon = Form.ActiveForm.Icon;
        }
        private void frmUpdate_Load(object sender, EventArgs e)
        {
        }
        public void ChangeStatus(int ID)
        {
            if (ID == 0)
            {
                this.lblDisplay.Text = "Connecting to server. Please wait a moment";
                this.Text = " Checking ...";
                return;
            }
            if (ID == 1)
            {
                this.lblDisplay.Text = "Downloading file. Please wait a moment";
                this.Text = " Downloading ...";
                return;
            }
            this.lblDisplay.Text = "Downloading new database. Please wait a moment";
            this.Text = " Downloading ...";
        }
    }
}