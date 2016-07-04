using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace MathDictionary
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }
        private void frmMain_Load(object sender, EventArgs e)
        {
            int tickCount = Environment.TickCount;
            frmLoading frmLoading = new frmLoading();
            frmLoading.Show();
            Application.DoEvents();
            this.Main_Initialize();
            Application.DoEvents();
            int num = 2000;
            int num2 = Environment.TickCount - tickCount;
            if (num - num2 > 0)
            {
                Thread.Sleep(num - num2);
            }
            frmLoading.Close();
            base.BringToFront();
            if (Environment.CommandLine.Trim().EndsWith("-updated"))
            {
                MessageBox.Show(this.Text, "Updated", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
        }
        private void Main_Initialize()
        {
            this.config = Registry.CurrentUser.CreateSubKey("Software\\Math Dictionary");
            this.LoadConfig();
            this.cmdStartPage_Click(null, null);
            this.SetTextBoxIdie(true);
            this.mnSeachType(this.mnFind_Vocabulary_Name, null);
            this.cmbGroup.SelectedIndex = 0;
            this.Text += Application.ProductVersion.ToString();
            if (Environment.OSVersion.Version.Major < 6)
            {
                this.tspTask1.RenderMode = ToolStripRenderMode.System;
                this.tspTask1.BackColor = Color.Transparent;
                this.lblFillLeft.Visible = true;
                this.lblFillRight.Visible = true;
            }
        }
        private void LoadConfig()
        {
            base.Height = (int)this.config.GetValue("Height", base.Height);
            base.Width = (int)this.config.GetValue("Width", base.Width);
            base.Top = (int)this.config.GetValue("Top", base.Top);
            base.Left = (int)this.config.GetValue("Left", base.Left);
            if (this.config.GetValue("Maximize", "False").ToString() == "True")
            {
                base.WindowState = FormWindowState.Maximized;
            }
            if (this.config.GetValue("IsLanguage", "True").ToString() == "False")
            {
                this.Tab_Click(this.TabVocabulary, null);
            }
        }
        private void SaveConfig()
        {
            bool flag = base.WindowState == FormWindowState.Maximized;
            if (!flag)
            {
                this.config.SetValue("Height", base.Height);
                this.config.SetValue("Width", base.Width);
                this.config.SetValue("Top", base.Top);
                this.config.SetValue("Left", base.Left);
            }
            this.config.SetValue("Maximize", flag);
            this.config.SetValue("IsLanguage", this.TabFormula.Checked);
        }
        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.DB.DbClose();
            this.SaveConfig();
        }
        internal void cmbGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.lstKey.Items.Clear();
            this.txtLookup.Enabled = false;
            this.cmbGroup.Enabled = false;
            this.CmdABC.Enabled = false;
            this.CmdGroup.Enabled = false;
            this.lblStatus.Text = "Loading ...";
            this.lstKey.SuspendLayout();
            Application.DoEvents();
            this.DB.LoadKeys_byAlphabet(this.lstKey, this.cmbGroup.Text);
            this.lblStatus.Text = "Total " + this.cmbGroup.Text + ": " + this.lstKey.Items.Count.ToString();
            this.txtLookup.Enabled = true;
            this.cmbGroup.Enabled = true;
            this.CmdABC.Enabled = true;
            this.CmdGroup.Enabled = true;
            this.lstKey.ResumeLayout();
        }
        private void Tab_Click(object sender, EventArgs e)
        {
            this.TabFormula.Checked = false;
            this.TabVocabulary.Checked = false;
            ((ToolStripButton)sender).Checked = true;
            frmMain.bIsLanguage = this.TabFormula.Checked;
        }
        private void Cmd_Click(object sender, EventArgs e)
        {
            this.CmdABC.Checked = false;
            this.CmdGroup.Checked = false;
            ((ToolStripButton)sender).Checked = true;
            this.panelABC.Visible = this.CmdABC.Checked;
            this.lblStatus.Text = "Loading ...";
            Application.DoEvents();
            if (this.CmdGroup.Checked)
            {
                if (this.TView.Nodes.Count == 0)
                {
                    this.DB.LoadKeys_byGroup(this.TView);
                }
                this.lblStatus.Text = "Total Groups: " + this.TView.Nodes.Count.ToString();
                return;
            }
            this.lblStatus.Text = "Total " + this.cmbGroup.Text + ": " + this.lstKey.Items.Count.ToString();
        }
        private void lstKey_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.bDisableDisplay)
            {
                return;
            }
            if (this.cmbGroup.SelectedIndex == 0)
            {
                this.WB.DocumentText = this.cDisplay.DisplayDeclareInfo(this.lstKey.Text);
            }
            else
            {
                if (this.cmbGroup.SelectedIndex == 1)
                {
                    this.WB.DocumentText = this.cDisplay.DisplayUserDefineInfo(this.lstKey.Text);
                }
                else
                {
                    this.WB.DocumentText = this.cDisplay.DisplayConstInfo(this.lstKey.Text);
                }
            }
            if (this.txtLookup.ForeColor == Color.Black)
            {
                this.bDisableDisplay = true;
                this.txtLookup.Text = this.lstKey.Text;
                this.bDisableDisplay = false;
            }
            this.lblStatus.Text = "Ready !";
        }
        private void TView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (this.cmbGroup.SelectedIndex != 0)
            {
                this.cmbGroup.SelectedIndex = 0;
            }
            if (e.Node.Parent != null)
            {
                this.lstKey.SelectedItem = e.Node.Text;
            }
        }
        private void txtFind_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
            {
                this.cmdFind_ButtonClick(null, null);
            }
        }
        private void txtLookup_TextChanged(object sender, EventArgs e)
        {
            if (this.bDisableDisplay)
            {
                return;
            }
            string text = this.txtLookup.Text;
            int num = this.lstKey.FindString(this.txtLookup.Text);
            if (num >= 0)
            {
                this.bDisableDisplay = true;
                this.lstKey.SelectedIndex = num;
                this.bDisableDisplay = false;
                if (this.txtLookup.Text != "")
                {
                    this.bDisableDisplay = true;
                    this.txtLookup.Text = this.lstKey.Items[num].ToString();
                    this.lblStatus.Text = "=> " + this.txtLookup.Text;
                    this.txtLookup.Select(text.Length, 50);
                    this.bDisableDisplay = false;
                    return;
                }
            }
            else
            {
                this.lblStatus.Text = "Not found !";
            }
        }
        private void txtLookup_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData != Keys.Return)
            {
                if (e.KeyData == Keys.Back)
                {
                    if (this.txtLookup.SelectionStart > 0)
                    {
                        this.txtLookup.Select(this.txtLookup.SelectionStart - 1, 50);
                        return;
                    }
                }
                else
                {
                    if (e.KeyData == Keys.Down)
                    {
                        this.lstKey.Focus();
                        SendKeys.Send("{DOWN}");
                    }
                }
                return;
            }
            if (this.lblStatus.Text != "Not found !")
            {
                this.lstKey_SelectedIndexChanged(null, null);
                return;
            }
            string szCategory = this.cmbGroup.Text.Substring(0, this.cmbGroup.Text.Length - 1);
            this.WB.DocumentText = this.cSearch.DisplaySearchResultsFailure(this.txtLookup.Text, this.lstKey.Text, szCategory);
        }
        private void SetTextBoxIdie(bool bValue)
        {
            if (bValue)
            {
                this.txtLookup.ForeColor = Color.Gray;
                this.txtLookup.Text = "Search ...";
                return;
            }
            this.txtLookup.ForeColor = Color.Black;
            this.txtLookup.Text = "";
        }
        private void txtLookup_Leave(object sender, EventArgs e)
        {
            if (this.txtLookup.Text == "")
            {
                this.SetTextBoxIdie(true);
            }
        }
        private void txtLookup_Enter(object sender, EventArgs e)
        {
            if (this.txtLookup.ForeColor == Color.Gray)
            {
                this.SetTextBoxIdie(false);
            }
        }
        private void mnSeachType(object sender, EventArgs e)
        {
            this.mnFind_Const.Checked = false;
            this.mnFind_Const_Name.Checked = false;
            this.mnFind_Const_Value.Checked = false;
            this.mnFind_Vocabulary.Checked = false;
            this.mnFind_Vocabulary_Example.Checked = false;
            this.mnFind_Vocabulary_Description.Checked = false;
            this.mnFind_Vocabulary_Name.Checked = false;
            this.mnFind_Vocabulary_Owner.Checked = false;
            this.mnFind_Vocabulary_Mean.Checked = false;
            this.mnFind_Vocabulary_Solution.Checked = false;
            this.mnFind_Google.Checked = false;
            this.mnFind_Formula.Checked = false;
            this.mnFind_Formula_Name.Checked = false;
            this.mnFind_Formula_Owner.Checked = false;
            this.mnFind_Formula_Param.Checked = false;
            ToolStripMenuItem toolStripMenuItem = (ToolStripMenuItem)sender;
            toolStripMenuItem.Checked = true;
            this.cmdFind.Tag = toolStripMenuItem.Tag;
        }
        private void cmdFind_ButtonClick(object sender, EventArgs e)
        {
            if (this.txtFind.Text.Length < 2)
            {
                MessageBox.Show("Keyword must be at least 2 characters", "Search", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return;
            }
            
            if (this.cmdFind.Tag.ToString() == "Google")
            {
                CSearch.SearchWithGoogle(this.WB, this.txtFind.Text, false);
                Application.DoEvents();
                this.lblCopyright.Text = "Press F11 to switch to Max View";
                return;
            }
            this.lblCopyright.Text = "Searching. Please wait...";
            this.WB.DocumentText = this.cDisplay.DisplayText(this.lblCopyright.Text);
            Application.DoEvents();
            this.cSearch.Search(this.txtFind.Text, this.cmdFind.Tag.ToString());
            this.WB.DocumentText = this.cSearch.DisplaySearchResults(1);
            this.lblCopyright.Text = this.lblCopyright.Tag.ToString();
        }
        private void WB_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            this.szLastText = this.WB.DocumentText;
            string fragment = e.Url.Fragment;
            if (fragment.StartsWith("#view_Vocabulary="))
            {
                this.WB.DocumentText = this.cDisplay.DisplayDeclareInfo(fragment.Substring(15));
                return;
            }
            if (fragment.StartsWith("#view_Formula="))
            {
                this.WB.DocumentText = this.cDisplay.DisplayUserDefineInfo(fragment.Substring(16));
                return;
            }
            if (fragment.StartsWith("#view_constant="))
            {
                this.WB.DocumentText = this.cDisplay.DisplayConstInfo(fragment.Substring(15));
                return;
            }
            if (fragment.StartsWith("#view_page="))
            {
                this.lblCopyright.Text = "Loading ...";
                Application.DoEvents();
                int nPageId = int.Parse(fragment.Substring(11));
                this.WB.DocumentText = this.cSearch.DisplaySearchResults(nPageId);
                this.lblCopyright.Text = this.lblCopyright.Tag.ToString();
                return;
            }
            if (fragment.StartsWith("#view_topic="))
            {
                this.WB.DocumentText = this.cDisplay.ViewTopic(fragment.Substring(12));
            }
        }
        private void btnGoBack_Click(object sender, EventArgs e)
        {
            if (this.WB.CanGoBack)
            {
                this.WB.GoBack();
                return;
            }
            this.WB.DocumentText = this.szLastText;
        }
        private void btnGoForward_Click(object sender, EventArgs e)
        {
            this.WB.GoForward();
        }
        private void cmdStartPage_Click(object sender, EventArgs e)
        {
            this.WB.DocumentText = this.cDisplay.StartPage();
        }
        private void mnuMenu_Exit_Click(object sender, EventArgs e)
        {
            base.Close();
        }
        private void mnView_MaxView_Click(object sender, EventArgs e)
        {
            if (this.mnView_MaxView.Text == "Max View")
            {
                base.WindowState = FormWindowState.Maximized;
                this.tspTask2.Hide();
                this.SplitPanel.Height += this.tspTask2.Height - 5;
                this.SplitPanel.Top -= this.tspTask2.Height - 5;
                this.SplitPanel.Panel1Collapsed = true;
                this.mnView_MaxView.Text = "Normal View";
                this.lblCopyright.Text = "Press F11 to switch to Normal View";
                return;
            }
            base.WindowState = FormWindowState.Normal;
            this.SplitPanel.Height -= this.tspTask2.Height - 5;
            this.SplitPanel.Top += this.tspTask2.Height - 5;
            this.SplitPanel.Panel1Collapsed = false;
            this.tspTask2.Show();
            this.mnView_MaxView.Text = "Max View";
            this.lblCopyright.Text = this.lblCopyright.Tag.ToString();
        }
        private string WB_SaveToTemp()
        {
            string text = Environment.GetEnvironmentVariable("temp") + "\\apiview.htm";
            StreamWriter streamWriter = new StreamWriter(text);
            streamWriter.Write(this.WB.DocumentText);
            streamWriter.Close();
            return text;
        }
        private void mnMenu_Print_Click(object sender, EventArgs e)
        {
            this.WB.ShowPrintDialog();
        }
        private void mnMenu_Save_Click(object sender, EventArgs e)
        {
            this.WBX.Navigate(this.WB_SaveToTemp());
            Application.DoEvents();
            this.WBX.ShowSaveAsDialog();
        }
        private void mnMenu_Import_Click(object sender, EventArgs e)
        {
            CImportExport cImportExport = new CImportExport();
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "XML File (*.xml)|*.xml|All files (*.*)|*.*";
            openFileDialog.Multiselect = true;
            DialogResult dialogResult = openFileDialog.ShowDialog();
            if (dialogResult == DialogResult.OK)
            {
                int num = 0;
                string[] fileNames = openFileDialog.FileNames;
                for (int i = 0; i < fileNames.Length; i++)
                {
                    string szFilePath = fileNames[i];
                    this.lblStatus.Text = "Importing ...";
                    Application.DoEvents();
                    int num2 = cImportExport.ImportFromXmlToDB(szFilePath);
                    if (num2 == 0)
                    {
                        return;
                    }
                    num += num2;
                }
                this.lblStatus.Text = "Done !";
                MessageBox.Show(num.ToString() + " Items Imported !", "Done", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
        }
        private void mnMenu_Export_ThisItem_Click(object sender, EventArgs e)
        {
            if (this.lstKey.Text.Trim() == "")
            {
                MessageBox.Show("Please select a item on list box", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            CImportExport cImportExport = new CImportExport();
            this.dlgExport.FileName = this.lstKey.Text + ".xml";
            DialogResult dialogResult = this.dlgExport.ShowDialog();
            if (dialogResult == DialogResult.OK)
            {
                int num = cImportExport.ExportToXml(this.lstKey.Text, this.cmbGroup.Text, this.dlgExport.FileName);
                if (num > 0)
                {
                    MessageBox.Show("1 Items exported !\n\nPlease send it to us via email levanhong05@gmail.com\n\nThank you !", "Done", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
            }
        }
        private void mnMenu_Export_ModifiedByOwner_Click(object sender, EventArgs e)
        {
            CImportExport cImportExport = new CImportExport();
            this.dlgExport.FileName = "Modified_by_Owner.xml";
            DialogResult dialogResult = this.dlgExport.ShowDialog();
            if (dialogResult == DialogResult.OK)
            {
                this.lblStatus.Text = "Exporting ...";
                Application.DoEvents();
                int num = cImportExport.ExportOwnerToXml(this.dlgExport.FileName);
                if (num >= 0)
                {
                    MessageBox.Show(num.ToString() + " Items exported !\n\nPlease send to us via email levanhong05@gmail.com\n\nThank you !", "Done", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
                this.lblStatus.Text = "Done !";
            }
        }
        private void mnHelp_Website_Click(object sender, EventArgs e)
        {
            Process.Start("http://chuaCo/");
        }
        private void mnHelp_Forums_ToanTin_Click(object sender, EventArgs e)
        {
            Process.Start("http://www.toantin.org/");
        }
        private void mnHelp_Forums_ToanHocVietNam_Click(object sender, EventArgs e)
        {
            Process.Start("http://www.diendantoanhoc.net/");
        }
        private void mnHelp_About_Click(object sender, EventArgs e)
        {
            frmAbout frmAbout = new frmAbout();
            frmAbout.ShowDialog();
        }
        private void mnHelp_CheckUpdate_Database_Click(object sender, EventArgs e)
        {
            CUpdate cUpdate = new CUpdate(this);
            cUpdate.DownloadFile("http://chuaCo", Application.StartupPath + "\\Version.info", "Check_Database", 0);
        }
        private void mnHelp_CheckUpdate_Version_Click(object sender, EventArgs e)
        {
            CUpdate cUpdate = new CUpdate(this);
            cUpdate.DownloadFile("http://chuaCo" + Application.ProductVersion.ToString(), Application.StartupPath + "\\Version.info", "Check_Version", 0);
        }
        private void mnHelp_WatNew_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start(Application.StartupPath + "\\Readme.txt");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }
        
        private void mnContext_SelectAll_Click(object sender, EventArgs e)
        {
            CUtils.SendHotkey_Ctrl('A');
        }
        private void mnContext_Find_Click(object sender, EventArgs e)
        {
            CUtils.SendHotkey_Ctrl('F');
        }
        private void mnContext_Copy_Click(object sender, EventArgs e)
        {
            CUtils.SendHotkey_Ctrl('C');
        }
        private void mnContext_Google_Click(object sender, EventArgs e)
        {
            string searchTag = CUtils.GetSearchTag(this.WB.DocumentText);
            if (searchTag != "" && searchTag != "")
            {
                CSearch.SearchWithGoogle(this.WB, searchTag, false);
                Application.DoEvents();
                this.lblCopyright.Text = "Press F11 to switch to Max View";
            }
        }
       
        private void mnContext_CopySyntax_Click(object sender, EventArgs e)
        {
            CUtils.CopyItToClipBoard(this.WB.DocumentText, "Syntax");
        }
        private void mnContext_CopyStruct_Click(object sender, EventArgs e)
        {
            CUtils.CopyItToClipBoard(this.WB.DocumentText, "Struct");
        }
        private void mnContext_CopySample_Click(object sender, EventArgs e)
        {
            CUtils.CopyItToClipBoard(this.WB.DocumentText, "Sample");
        }
        private void mnContext_CopyConst_Click(object sender, EventArgs e)
        {
            CUtils.CopyItToClipBoard(this.WB.DocumentText, "Const");
        }
        private void cmbGroup_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }
        private void Edition_HideAll()
        {
            this.WB.Visible = false;
            this.pnlEditVocabulary.Visible = false;
            this.pnlEditFormula.Visible = false;
            this.pnlEditConst.Visible = false;
        }
        private void cmdEdit_Click(object sender, EventArgs e)
        {
            if (this.lstKey.SelectedIndex < 0)
            {
                return;
            }
            this.SplitPanel.Panel1Collapsed = true;
            this.Edition_HideAll();
            if (this.cmbGroup.SelectedIndex == 0)
            {
                this.pnlEditVocabulary.Visible = true;
                this.lblEditVocabulary_Title.Text = this.lstKey.Text;
                string vocabularyStr = CModifyItem.GetVocabularyStr(this.lstKey.Text);
                if (vocabularyStr != "")
                {
                    string[] array = vocabularyStr.Split(new char[]
					{
						'¦'
					});
                    this.txtEditVocabulary_Word.Text = array[0];
                    this.txtEditVocabulary_Meaning.Text = array[1];
                    this.txtEditVocabulary_Description.Text = array[2];
                    this.txtEditVocabulary_Example.Text = array[3];
                    this.txtEditVocabulary_Solution.Text = array[4];
                    this.txtEditVocabulary_Group.Text = array[5];
                    this.txtEditVocabulary_Owner.Text = ((array[6] == "") ? "Lê Văn Hồng" : array[6]);
                    return;
                }
            }
            else
            {
                if (this.cmbGroup.SelectedIndex == 1)
                {
                    this.lblEditFormula_Title.Text = this.lstKey.Text;
                    this.pnlEditFormula.Visible = true;
                    string formulaStr = CModifyItem.GetFormulaStr(this.lstKey.Text);
                    if (formulaStr != "")
                    {
                        string[] array2 = formulaStr.Split(new char[]
						{
							'¦'
						});
                        this.txtEditFormula_Name.Text = array2[0];
                        this.txtEditFormula_Value.Text = array2[1];
                        this.txtEditFormula_Parameter.Text = array2[2];
                        this.txtEditFormula_Group.Text = array2[3];
                        this.txtEditFormula_Owner.Text = ((array2[4] == "") ? "Lê Văn Hồng" : array2[4]);
                        return;
                    }
                }
                else
                {
                    this.lblEditConst_Title.Text = this.lstKey.Text;
                    this.pnlEditConst.Visible = true;
                    string constantStr = CModifyItem.GetConstantStr(this.lstKey.Text);
                    if (constantStr != "")
                    {
                        string[] array3 = constantStr.Split(new char[]
						{
							'¦'
						});
                        this.txtEditConst_Name.Text = this.lblEditConst_Title.Text;
                        this.txtEditConst_Value.Text = array3[1];
                        this.txtEditConst_Owner.Text = ((array3[2] == "") ? "Lê Văn Hồng" : array3[2]);
                    }
                }
            }
        }
        private void cmdAdd_Click(object sender, EventArgs e)
        {
            this.SplitPanel.Panel1Collapsed = true;
            this.Edition_HideAll();
            if (this.cmbGroup.SelectedIndex == 0)
            {
                this.lblEditVocabulary_Title.Text = "Add New Vocabulary";
                this.pnlEditVocabulary.Visible = true;
                this.txtEditVocabulary_Word.Text = "";
                this.txtEditVocabulary_Meaning.Text = "";
                this.txtEditVocabulary_Description.Text = "";
                this.txtEditVocabulary_Example.Text = "";
                this.txtEditVocabulary_Solution.Text = "";
                this.txtEditVocabulary_Group.Text = "";
                this.txtEditVocabulary_Owner.Text = "Lê Văn Hồng";
                return;
            }
            if (this.cmbGroup.SelectedIndex == 1)
            {
                this.lblEditFormula_Title.Text = "Add New Formula";
                this.pnlEditFormula.Visible = true;
                this.txtEditFormula_Name.Text = "";
                this.txtEditFormula_Value.Text = "";
                this.txtEditFormula_Parameter.Text = "";
                this.txtEditFormula_Group.Text = "";
                this.txtEditFormula_Owner.Text = "Lê Văn Hồng";
                return;
            }
            this.lblEditConst_Title.Text = "Add New Constant";
            this.pnlEditConst.Visible = true;
            this.txtEditConst_Name.Text = "";
            this.txtEditConst_Value.Text = "";
            this.txtEditConst_Owner.Text = "Lê Văn Hồng";
        }
        private void cmdDel_Click(object sender, EventArgs e)
        {
            if (this.lstKey.SelectedIndex < 0)
            {
                return;
            }
            if (MessageBox.Show("Are you sure you want to permanently delete this item ?", "Confirm : " + this.lstKey.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
            {
                return;
            }
            int num = CModifyItem.DeleteFromDB(this.lstKey.Text, this.cmbGroup.Text);
            if (num > 0)
            {
                this.lstKey.Items.Remove(this.lstKey.Text);
                this.cmdStartPage_Click(null, null);
                MessageBox.Show("Success !", "Result", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
        }
        private void btnEdit_OK_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure ?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            {
                return;
            }
            string text2;
            if (this.cmbGroup.SelectedIndex == 0)
            {
                string text = this.txtEditVocabulary_Group.Text.Trim();
                if (text == "")
                {
                    text = "Others";
                }
                string szData = string.Concat(new string[]
				{
					this.txtEditVocabulary_Word.Text.Trim(),
					"¦",                    
					this.txtEditVocabulary_Meaning.Text.Trim(),
					"¦",
					this.txtEditVocabulary_Description.Text.Trim(),
					"¦",
					this.txtEditVocabulary_Example.Text.Trim(),
					"¦",
					this.txtEditVocabulary_Solution.Text.Trim(),
					"¦",
					text,
					"¦",
					this.txtEditVocabulary_Owner.Text.Trim()
				});
                text2 = CModifyItem.UpdateVocabulary(this.lblEditVocabulary_Title.Text, szData);
            }
            else
            {
                if (this.cmbGroup.SelectedIndex == 1)
                {
                    string text = this.txtEditFormula_Group.Text.Trim();
                    if (text == "")
                    {
                        text = "Others";
                    }
                    string szData2 = string.Concat(new string[]
				    {
					    this.txtEditFormula_Name.Text.Trim(),
					    "¦",                    
					    this.txtEditFormula_Value.Text.Trim(),
					    "¦",
					    this.txtEditFormula_Parameter.Text.Trim(),
					    "¦",
					    text,
					    "¦",
					    this.txtEditFormula_Owner.Text.Trim()
				    });
                    text2 = CModifyItem.UpdateFormula(this.lblEditFormula_Title.Text, szData2);
                }
                else
                {
                    string szData3 = string.Concat(new string[]
					{
						this.txtEditConst_Name.Text.Trim(),
						"¦",
						this.txtEditConst_Value.Text.Trim(),
						"¦",
						this.txtEditConst_Owner.Text.Trim()
					});
                    text2 = CModifyItem.UpdateConstant(this.lblEditConst_Title.Text, szData3);
                }
            }
            if (text2 == "")
            {
                return;
            }
            if (text2.StartsWith("+"))
            {
                int selectedIndex = this.lstKey.Items.Add(text2.Substring(1));
                this.lstKey.SelectedIndex = selectedIndex;
            }
            else
            {
                if (text2 != this.lstKey.Text)
                {
                    int selectedIndex2 = this.lstKey.SelectedIndex;
                    this.lstKey.Items[selectedIndex2] = text2;
                    this.lstKey.SelectedIndex = selectedIndex2;
                }
                else
                {
                    this.lstKey_SelectedIndexChanged(sender, e);
                }
            }
            this.btnEdit_Cancel_Click(sender, e);
        }
        private void btnEdit_Cancel_Click(object sender, EventArgs e)
        {
            this.Edition_HideAll();
            this.WB.Visible = true;
            this.SplitPanel.Panel1Collapsed = false;
        }
        private void mnEdition_Editor_Click(object sender, EventArgs e)
        {
            //frmNotepad.szContent = this.mnEdition.SourceControl.Text;
            frmNotepad frmNotepad = new frmNotepad();
            frmNotepad.ShowDialog();
            this.mnEdition.SourceControl.Text = frmNotepad.szContent;
        }
        private void txtEditVocabulary_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            //frmNotepad.szContent = textBox.Text;
            frmNotepad frmNotepad = new frmNotepad();
            frmNotepad.ShowDialog();
            textBox.Text = frmNotepad.szContent;
        }
        private void mnEdition_Cut_Click(object sender, EventArgs e)
        {
            CUtils.SendHotkey_Ctrl('X');
        }
        private void mnEdition_Copy_Click(object sender, EventArgs e)
        {
            CUtils.SendHotkey_Ctrl('C');
        }
        private void mnEdition_Paste_Click(object sender, EventArgs e)
        {
            CUtils.SendHotkey_Ctrl('V');
        }
        private void mnEdition_SelectAll_Click(object sender, EventArgs e)
        {
            if (this.mnEdition.SourceControl is TextBox)
            {
                TextBox textBox = (TextBox)this.mnEdition.SourceControl;
                textBox.SelectAll();
            }
        }
    }
}