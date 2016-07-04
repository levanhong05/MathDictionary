namespace MathDictionary
{
    partial class frmNotepad
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.txtEdit = new System.Windows.Forms.TextBox();
            this.mnuMain = new System.Windows.Forms.MenuStrip();

            this.mnFile = new System.Windows.Forms.ToolStripMenuItem();
            this.mnFile_Close = new System.Windows.Forms.ToolStripMenuItem();

            this.mnView = new System.Windows.Forms.ToolStripMenuItem();
            this.mnView_WordWrap = new System.Windows.Forms.ToolStripMenuItem();
            this.mnView_Font = new System.Windows.Forms.ToolStripMenuItem();

            this.mnuMain.SuspendLayout();
            this.SuspendLayout();

            this.txtEdit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtEdit.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            this.txtEdit.Location = new System.Drawing.Point(0, 24);
            this.txtEdit.Margin = new System.Windows.Forms.Padding(4);
            this.txtEdit.Multiline = true;
            this.txtEdit.Name = "txtEdit";
            this.txtEdit.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtEdit.Size = new System.Drawing.Size(719, 413);
            this.txtEdit.TabIndex = 0;

            this.mnuMain.BackColor = System.Drawing.Color.WhiteSmoke;
            this.mnuMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[]
			{
				this.mnFile,
				this.mnView
			});
            this.mnuMain.Location = new System.Drawing.Point(0, 0);
            this.mnuMain.Name = "mnuMain";
            this.mnuMain.Size = new System.Drawing.Size(719, 24);
            this.mnuMain.TabIndex = 1;

            this.mnFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[]
			{
				this.mnFile_Close
			});
            this.mnFile.Name = "mnFile";
            this.mnFile.Size = new System.Drawing.Size(35, 20);
            this.mnFile.Text = "File";
            this.mnFile_Close.Name = "mnFile_Close";
            this.mnFile_Close.Size = new System.Drawing.Size(148, 22);
            this.mnFile_Close.Text = "Save && Close";
            this.mnFile_Close.Click += new System.EventHandler(this.mnFile_Close_Click);

            this.mnView.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[]
			{
				this.mnView_WordWrap,
				this.mnView_Font
			});
            this.mnView.Name = "mnView";
            this.mnView.Size = new System.Drawing.Size(41, 20);
            this.mnView.Text = "View";

            this.mnView_WordWrap.Checked = true;
            this.mnView_WordWrap.CheckState = System.Windows.Forms.CheckState.Checked;
            this.mnView_WordWrap.Name = "mnView_WordWrap";
            this.mnView_WordWrap.Size = new System.Drawing.Size(140, 22);
            this.mnView_WordWrap.Text = "Word Wrap";
            this.mnView_WordWrap.Click += new System.EventHandler(this.mnView_WordWrap_Click);

            this.mnView_Font.Name = "mnView_Font";
            this.mnView_Font.Size = new System.Drawing.Size(140, 22);
            this.mnView_Font.Text = "Font...";
            this.mnView_Font.Click += new System.EventHandler(this.mnView_Font_Click);

            this.AutoScaleDimensions = new System.Drawing.SizeF(9f, 19f);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;

            this.ClientSize = new System.Drawing.Size(719, 437);
            this.Controls.Add(this.txtEdit);
            this.Controls.Add(this.mnuMain);
            this.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            this.MainMenuStrip = this.mnuMain;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "frmNotepad";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Editor";
            this.Load += new System.EventHandler(this.frmNotepad_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmNotepad_FormClosing);
            this.mnuMain.ResumeLayout(false);
            this.mnuMain.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.TextBox txtEdit;
        private System.Windows.Forms.MenuStrip mnuMain;
        private System.Windows.Forms.ToolStripMenuItem mnFile;
        private System.Windows.Forms.ToolStripMenuItem mnFile_Close;
        private System.Windows.Forms.ToolStripMenuItem mnView;
        private System.Windows.Forms.ToolStripMenuItem mnView_WordWrap;
        private System.Windows.Forms.ToolStripMenuItem mnView_Font;
        public static string szContent = "";
        private static System.Drawing.Font fFont = new System.Drawing.Font("Tahoma", 10f);
    }
}