using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace MathDictionary
{
    internal class CUpdate
    {
        public const string szServerURL = "http://chuaCo/";
        private frmMain thisform;
        public CUpdate(frmMain thisform)
        {
            this.thisform = thisform;
        }
        public void DownloadFile(string szURL, string szSaveTo, string szTag, int nStatus)
        {
            frmUpdate frmUpdate = new frmUpdate();
            frmUpdate.szURL = szURL;
            frmUpdate.szSaveTo = szSaveTo;
            frmUpdate.szTag = szTag;
            frmUpdate.ChangeStatus(nStatus);
            frmUpdate.Show();
            Thread thread = new Thread(new ParameterizedThreadStart(this.ThreadCheckUpdate));
            thread.Start(frmUpdate);
        }
        private void ThreadCheckUpdate(object objParam)
        {
            frmUpdate frm = (frmUpdate)objParam;
            int ret = CUtils.URLDownloadToFile(IntPtr.Zero, frm.szURL, frm.szSaveTo, 0, IntPtr.Zero);
            if (frm.szTag.StartsWith("Download_"))
            {
                Thread.Sleep(2000);
            }
            //this.thisform.Invoke(
            //{
            //    frm.Close();
            //    if (ret != 0)
            //    {
            //        MessageBox.Show("Can not connect to server or download file !", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            //        return;
            //    }
            //    this.DoFunction(frm.szTag, frm.szSaveTo);
            //});
        }
        private void DoFunction(string szStr, string szPath)
        {
            if (szStr == "Check_Version")
            {
                string szInput = CUtils.ReadTextFile(szPath);
                string tagText = CUtils.GetTagText(szInput, "Version");
                if (File.Exists(szPath))
                {
                    File.Delete(szPath);
                }
                string tagText2 = CUtils.GetTagText(szInput, "Error");
                if (tagText2 != "")
                {
                    MessageBox.Show(tagText2, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    return;
                }
                if (Application.ProductVersion.ToString() == tagText)
                {
                    MessageBox.Show("You are using the lastest version. Please check back again for updates at a later time. ", "Quick Update", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    return;
                }
                string tagText3 = CUtils.GetTagText(szInput, "More");
                if (tagText3 != "")
                {
                    MessageBox.Show("New version {" + tagText + "} available. " + tagText3, "Quick Update", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    return;
                }
                if (MessageBox.Show("New version {" + tagText + "} available. Do you want to download it now. ", "Quick Update", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                {
                    return;
                }
                this.DownloadFile("http://chuaCo/MathDic/MathDictionary.exe", Application.StartupPath + "\\update.pak", "Download_Version", 1);
                return;
            }
            else
            {
                if (szStr == "Download_Version")
                {
                    MessageBox.Show("The changes to take effect after restart the application \n\nClick the OK button and wait a few seconds. ", "Download Complete", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    FileInfo fileInfo = new FileInfo(Application.ExecutablePath);
                    new Process
                    {
                        StartInfo =
                        {
                            WindowStyle = ProcessWindowStyle.Hidden,
                            FileName = "cmd.exe",
                            Arguments = string.Concat(new string[]
							{
								"/k cd \"",
								Application.StartupPath,
								"\" & ping 127.0.0.1 -n 3 & del \"",
								fileInfo.Name,
								"\" & ren update.pak \"",
								fileInfo.Name,
								"\" & call \"",
								fileInfo.Name,
								"\" -updated & exit "
							})
                        }
                    }.Start();
                    Application.Exit();
                    return;
                }
                if (!(szStr == "Check_Database"))
                {
                    if (szStr == "Download_Database")
                    {
                        this.thisform.lblStatus.Text = "Updating... Please wait a moment.";
                        Application.DoEvents();
                        CImportExport cImportExport = new CImportExport();
                        int num = cImportExport.ImportFromXmlToDB(szPath);
                        if (num >= 0)
                        {
                            MessageBox.Show(num.ToString() + " Items Updated ! \n\nApplication will be reload the database. ", "Update Complete", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                            if (File.Exists(szPath))
                            {
                                File.Delete(szPath);
                            }
                            if (QueryDB.DBExecuteNonQuery("UPDATE Generals SET Generals.Value = '" + QueryDB.szTemp_DBDate + "' WHERE Name = 'LastUpdate'") > 0)
                            {
                                QueryDB.szDBDate = QueryDB.szTemp_DBDate;
                            }
                            this.thisform.cmbGroup_SelectedIndexChanged(null, null);
                        }
                    }
                    return;
                }
                string szInput2 = CUtils.ReadTextFile(szPath);
                string tagText4 = CUtils.GetTagText(szInput2, "Database");
                if (File.Exists(szPath))
                {
                    File.Delete(szPath);
                }
                string tagText5 = CUtils.GetTagText(szInput2, "Error");
                if (tagText5 != "")
                {
                    MessageBox.Show(tagText5, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    return;
                }
                QueryDB.szTemp_DBDate = tagText4;
                if (tagText4 == QueryDB.szDBDate)
                {
                    MessageBox.Show("You are using the lastest database. Please check back again for updates at a later time. ", "Quick Update", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    return;
                }
                if (MessageBox.Show("New database {" + tagText4 + "} available. Do you want to download it now. ", "Quick Update", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                {
                    return;
                }
                this.DownloadFile("http://chuaCo/MathDic/server.php?date=" + QueryDB.szDBDate, Application.StartupPath + "\\NewDatabase.xml", "Download_Database", 2);
                return;
            }
        }
    }
}