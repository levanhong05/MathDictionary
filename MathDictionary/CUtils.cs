using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
namespace MathDictionary
{
    internal class CUtils
    {
        private static WebBrowser WBX = new WebBrowser();
        [DllImport("user32.dll", SetLastError = true)]
        private static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, UIntPtr dwExtraInfo);
        [DllImport("urlmon.dll")]
        public static extern int URLDownloadToFile(IntPtr pCaller, string szURL, string szFileName, int dwReserved, IntPtr lpfnCB);
        private int BinarySeach(ListBox lst, int min, int max, string szValue)
        {
            int num = min + (max - min) / 2;
            if (max - min > 1)
            {
                string strB = lst.Items[num].ToString();
                if (string.Compare(szValue, strB, true) <= 0)
                {
                    return this.BinarySeach(lst, min, num, szValue);
                }
                return this.BinarySeach(lst, num, max, szValue);
            }
            else
            {
                int num2 = string.Compare(szValue, lst.Items[min].ToString(), true);
                if (num2 >= 1)
                {
                    return max;
                }
                return min;
            }
        }
        public static void SendHotkey_Ctrl(char cKey)
        {
            Application.DoEvents();
            CUtils.keybd_event(17, 0, 0u, (UIntPtr)0u);
            Application.DoEvents();
            CUtils.keybd_event((byte)cKey, 1, 0u, (UIntPtr)0u);
            Application.DoEvents();
            CUtils.keybd_event(17, 0, 2u, (UIntPtr)0u);
            Application.DoEvents();
        }
        public static string HtmlToText(string szData)
        {
            CUtils.WBX.DocumentText = szData.Trim();
            Application.DoEvents();
            return CUtils.WBX.Document.Body.OuterText;
        }
        public static string GetTextBetweenTag(string szText, string szTag)
        {
            int num = szText.IndexOf(szTag);
            if (num >= 0)
            {
                num += szTag.Length;
                int num2 = szText.IndexOf(szTag, num);
                string szData = szText.Substring(num, num2 - num);
                return CUtils.HtmlToText(szData);
            }
            return null;
        }
        public static void CopyItToClipBoard(string szText, string szTag)
        {
            szText = CUtils.GetTextBetweenTag(szText, "<!---" + szTag + "--->");
            if (szText != null)
            {
                Clipboard.Clear();
                Clipboard.SetText(szText);
                MessageBox.Show(szTag + " copied to clipboard !", "Copy", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return;
            }
            MessageBox.Show("Can not find " + szTag + " tag", "Find", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
        public static string GetSearchTag(string szText)
        {
            string result = "";
            int num = szText.LastIndexOf("<!---SearchTag(");
            if (num >= 0)
            {
                num += 15;
                int num2 = szText.IndexOf(')', num);
                result = szText.Substring(num, num2 - num);
            }
            else
            {
                MessageBox.Show("Can not find search tag", "Search", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            return result;
        }
        public static string GetTagText(string szInput, string szTag)
        {
            int num = szInput.IndexOf("<" + szTag + ">");
            if (num < 0)
            {
                return "";
            }
            int num2 = szInput.IndexOf("</" + szTag + ">");
            if (num2 < 0)
            {
                return "";
            }
            num += szTag.Length + 2;
            return szInput.Substring(num, num2 - num).Trim();
        }
        public static string ReadTextFile(string szPath)
        {
            string result = "";
            try
            {
                StreamReader streamReader = new StreamReader(szPath);
                result = streamReader.ReadToEnd();
                streamReader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            return result;
        }
    }
}