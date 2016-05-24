using MavenSynchronizationTool.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MavenSynchronizationTool
{
    public partial class FrmMavenCentralRepositorySetting : Form
    {
        public CentralRepository Repository { get; set; } = null;
        private bool isNotClosed = true;
        private const string URL = @"^http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?$";

        public FrmMavenCentralRepositorySetting()
        {
            InitializeComponent();
        }

        private void FrmMavenCentralRepositorySetting_Load(object sender, EventArgs e)
        {
            Repository = new CentralRepository();
            Repository.SynchState = 0;
            this.textBox1.Text = "";
            this.textBox2.Text = "https://";
            this.textBox3.Text = "";
            this.textBox3.ReadOnly = true;
            this.textBox1.Focus();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.textBox1.Text.Trim()) ||
                string.IsNullOrEmpty(this.textBox2.Text.Trim()) ||
                string.IsNullOrEmpty(this.textBox3.Text.Trim()))
            {
                this.isNotClosed = false;
                return;
            }

            this.Repository.CentralName = this.textBox1.Text.Trim();
            bool isUrl = Regex.IsMatch(this.textBox2.Text.Trim(), URL);
            if(!isUrl)
            {
                MessageBox.Show("Maven中央仓库地址不是有效的URL字符串.", "异常", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.isNotClosed = false;
                this.textBox2.Focus();
                return;
            }
            //验证Maven中央库是否已经添加
            string tempStr = this.textBox2.Text.Trim();
            string tempUrl = tempStr.LastIndexOf('/') == (tempStr.Length - 1) ? tempStr.Substring(0, tempStr.Length - 1) : tempStr;
            List<CentralRepository> existRepositories = Program.CentralRepositoryList.FindAll(delegate (CentralRepository cr) {
                return tempUrl.Trim().ToUpper() == 
                (cr.CentralUrl.LastIndexOf('/') == (cr.CentralUrl.Length - 1) ? cr.CentralUrl.Substring(0, cr.CentralUrl.Length - 1) : cr.CentralUrl).Trim().ToUpper();
            });
            if (existRepositories != null && existRepositories.Count > 0)
            {
                MessageBox.Show("Maven中央仓库地址已存在，请输入其他Maven中央库地址.", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.isNotClosed = false;
                this.textBox2.Focus();
                return;
            }
            this.Repository.CentralUrl = this.textBox2.Text.Trim();
            this.Repository.LocalFolder = this.textBox3.Text.Trim();
        }


        private void button3_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderDlg = new FolderBrowserDialog();
            folderDlg.Description = "Select the directory that Maven local folder.";
            if(folderDlg.ShowDialog() == DialogResult.OK)
            {
                Repository.LocalFolder = folderDlg.SelectedPath;
                this.textBox3.Text = folderDlg.SelectedPath;
            }
        }

        private void FrmMavenCentralRepositorySetting_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(!this.isNotClosed)
            {
                e.Cancel = true;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}