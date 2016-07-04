using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace MathDictionary
{
    public partial class frmNotepad : Form
    {
        public frmNotepad()
        {
            InitializeComponent();
        }
        private void frmNotepad_Load(object sender, EventArgs e)
        {
            this.Icon = Form.ActiveForm.Icon;
            this.txtEdit.Text = frmNotepad.szContent;
            this.txtEdit.SelectionStart = this.txtEdit.TextLength;
            this.txtEdit.Font = frmNotepad.fFont;
        }
        private void frmNotepad_FormClosing(object sender, FormClosingEventArgs e)
        {
            frmNotepad.szContent = this.txtEdit.Text;
        }
        private void mnView_WordWrap_Click(object sender, EventArgs e)
        {
            this.mnView_WordWrap.Checked = !this.mnView_WordWrap.Checked;
            this.txtEdit.WordWrap = this.mnView_WordWrap.Checked;
        }
        private void mnView_Font_Click(object sender, EventArgs e)
        {
            FontDialog fontDialog = new FontDialog();
            fontDialog.Font = frmNotepad.fFont;
            DialogResult dialogResult = fontDialog.ShowDialog();
            if (dialogResult == DialogResult.OK)
            {
                frmNotepad.fFont = fontDialog.Font;
                this.txtEdit.Font = frmNotepad.fFont;
            }
        }
        private void mnFile_Close_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}