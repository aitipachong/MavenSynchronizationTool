using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using log4net;
using MavenSynchronizationTool.Core;
using System.IO;
using MavenSynchronizationTool.Core.DBUtility;
using MavenSynchronizationTool.Entities;
using System.Threading;

namespace MavenSynchronizationTool
{
    public partial class FrmSysInit : Form
    {
        /// <summary>
        /// SQLite数据库文件名称
        /// </summary>
        private const string SQLITE_NAME = "MavenSynchDb.db";
        /* App.config代理相关常量 */
        private const string IS_HTTP_PROXY_KEY = "IsHttpProxy";
        private const string HTTP_PROXY_HOST_KEY = "HttpProxyHost";
        private const string HTTP_PROXY_PORT_KEY = "HttpProxyPort";
        private const string HTTP_PROXY_USER_KEY = "HttpProxyUser";
        private const string HTTP_PROXY_PWD_KEY = "HttpProxyPwd";
        /* App.config检索文件相关常量 */
        private const string RETRIEVAL_FILE_NAMES = "RetrievalFileNames";

        private System.Timers.Timer initTimer = new System.Timers.Timer(1000);
        private int frmState = 0;

        public FrmSysInit()
        {
            InitializeComponent();
        }

        private void FrmSysInit_Load(object sender, EventArgs e)
        {
            //初始化progressBar和label内容
            this.label1.Text = "系统初始化...";
            this.progressBar1.Maximum = 100;
            this.progressBar1.Minimum = 0;
            this.progressBar1.Step = 1;
            this.progressBar1.Value = 0;

            this.initTimer.Elapsed += new System.Timers.ElapsedEventHandler(InitializationDataLoading);
            this.initTimer.AutoReset = false;
            this.initTimer.Enabled = true;
        }

        #region 委托
        private delegate void DoLabelDelegate(string text);
        private delegate void DoProgressDelegate(int number);
        private delegate void DoShowFrmMainDelegate();

        private void DoLabel(string text)
        {
            if(this.label1.InvokeRequired)
            {
                DoLabelDelegate d = DoLabel;
                this.label1.Invoke(d, text);
            }
            else
            {
                this.label1.Text = text;
                Application.DoEvents();
            }
        }

        private void DoProgress(int number)
        {
            if(this.progressBar1.InvokeRequired)
            {
                DoProgressDelegate d = DoProgress;
                this.progressBar1.Invoke(d, number);
            }
            else
            {
                this.progressBar1.Value = number;
                Application.DoEvents();
            }
        }

        private void DoShowFrmMain()
        {
            if(this.InvokeRequired)
            {
                DoShowFrmMainDelegate d = DoShowFrmMain;
                this.Invoke(d);
            }
            else
            {
                this.Hide();
                FrmMain frmMain = new FrmMain();
                frmMain.Show();
            }
        }

        #endregion

        private void InitializationDataLoading(object source, System.Timers.ElapsedEventArgs e)
        {
            if (this.frmState != 0) return;
            this.frmState = 1;
            try
            {
                //1.获取HTTP代理信息
                //DoLabel("获取HTTP代理信息...");
                this.Invoke(new Action(() => label1.Text = "获取HTTP代理信息..."));
                if (!GetHttpProxyInformation())
                {
                    LogHelper.WriteLogAndExitApplication("获取HTTP代理信息失败.");
                    return;
                }
                DoProgress(20);
                //2.获取检索文件名
                this.Invoke(new Action(() => label1.Text = "获取检索文件名..."));
                if (!GetRetrievalFileNames())
                {
                    LogHelper.WriteLogAndExitApplication("获取检索文件名失败.");
                    return;
                }
                DoProgress(30);
                //3.获取“下载文件后缀列表”
                this.Invoke(new Action(() => label1.Text = "获取下载文件后缀名..."));
                if (!GetDownloadSuffix())
                {
                    LogHelper.WriteLogAndExitApplication("获取下载文件后缀名失败.");
                    return;
                }
                DoProgress(50);
                //4.获取Maven中央库信息及索引信息
                this.Invoke(new Action(() => label1.Text = "正在获取Mave中央库及索引信息..."));
                if (!GetCentralRepositoryAndCentralRepositoryIndex())
                {
                    LogHelper.WriteLogAndExitApplication("获取Mave中央库及索引信息失败.");
                    return;
                }
                DoProgress(100);
                this.frmState = 2;
                DoShowFrmMain();
            }
            catch (Exception ex)
            {
                //写异常日志
                LogHelper.WriteLog("系统初始化异常", ex);
                Application.Exit();
            }
        }

        /// <summary>
        /// 获取HTTP代理相关信息
        /// </summary>
        /// <returns>获取成功与否</returns>
        private bool GetHttpProxyInformation()
        {
            bool result = false;
            try
            {
                Program.IsHttpProxy = Convert.ToBoolean(ConfigurationManager.AppSettings[IS_HTTP_PROXY_KEY].ToString());
                Program.HttpProxyHost = ConfigurationManager.AppSettings[HTTP_PROXY_HOST_KEY].ToString();
                Program.HttpProxyPort = Convert.ToInt32(ConfigurationManager.AppSettings[HTTP_PROXY_PORT_KEY].ToString());
                Program.HttpProxyUser = ConfigurationManager.AppSettings[HTTP_PROXY_USER_KEY].ToString();
                Program.HttpProxyPwd = ConfigurationManager.AppSettings[HTTP_PROXY_PWD_KEY].ToString();
                result = true;
            }
            catch(Exception ex)
            {
                throw ex;
            }

            return result;
        }


        /// <summary>
        /// 获取检索文件名列表
        /// </summary>
        /// <returns></returns>
        private bool GetRetrievalFileNames()
        {
            bool result = false;
            try
            {
                string tempStr = ConfigurationManager.AppSettings[RETRIEVAL_FILE_NAMES].ToString();
                if (Program.RetrievalFileNames == null) Program.RetrievalFileNames = new List<string>();
                Program.RetrievalFileNames = tempStr.Split('|').ToList<string>();
                result = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }

        /// <summary>
        /// 获取Maven中央库同步内容后缀
        /// </summary>
        /// <returns></returns>
        private bool GetDownloadSuffix()
        {
            bool result = false;
            string tempConnectionStr = "Data Source={0};Version=3;";
            string sqlitePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, SQLITE_NAME);
            if (!File.Exists(sqlitePath))
            {
                FileNotFoundException ex = new FileNotFoundException(string.Format("SQLite数据库文件不存在，路径：{ 0 }", sqlitePath));
                throw ex;
            }
            Program.SQLiteConnectionString = string.Format(tempConnectionStr, sqlitePath);
            string commandText = "SELECT * FROM DownloadSuffix WHERE ignore = 0";
            try
            {
                if (Program.SuffixList == null) Program.SuffixList = new List<string>();
                DataSet ds = SQLiteHelper.ExecuteDataSet(Program.SQLiteConnectionString, commandText, null);
                ReflectHandler<DownloadSuffix> reflect = new ReflectHandler<DownloadSuffix>();
                List<DownloadSuffix> downloadSuffixList = reflect.FillModel(ds);
                if (downloadSuffixList == null)
                {
                    Program.SuffixList.Add("jar");
                }
                else
                {
                    Program.SuffixList = downloadSuffixList.ConvertAll(delegate (DownloadSuffix suffix)
                    {
                        return suffix.Suffix;
                    });
                }
                result = true;
            }
            catch(Exception ex)
            {
                throw ex;
            }

            return result;
        }

        /// <summary>
        /// 获取Maven中央库及索引信息
        /// </summary>
        /// <returns></returns>
        private bool GetCentralRepositoryAndCentralRepositoryIndex()
        {
            bool result = false;
            string commandText = string.Empty;
            //获取中央库信息
            commandText = "SELECT * FROM CentralRepository";
            try
            {
                if (Program.CentralRepositoryList == null) Program.CentralRepositoryList = new List<CentralRepository>();
                DataSet ds = SQLiteHelper.ExecuteDataSet(Program.SQLiteConnectionString, commandText, null);
                ReflectHandler<CentralRepository> crReflect = new ReflectHandler<CentralRepository>();
                Program.CentralRepositoryList = crReflect.FillModel(ds);
                if(Program.CentralRepositoryList != null && Program.CentralRepositoryList.Count > 0)
                {
                    //获取指定中央库的索引信息
                    for(int loopi = 0; loopi < Program.CentralRepositoryList.Count;loopi++)
                    {
                        Int64 centralID = Program.CentralRepositoryList[loopi].ID;
                        commandText = "SELECT * FROM CentralRepositoryIndex WHERE CentralID = " + centralID.ToString();
                        DataSet ds1 = SQLiteHelper.ExecuteDataSet(Program.SQLiteConnectionString, commandText, null);
                        ReflectHandler<CentralRepositoryIndex> criReflect = new ReflectHandler<CentralRepositoryIndex>();
                        List<CentralRepositoryIndex> criList = criReflect.FillModel(ds1);
                        if(criList != null && criList.Count > 0)
                        {
                            Program.CentralRepositoryList[loopi].IndexList = criList;
                        }
                    }
                }

                result = true;
            }
            catch(Exception ex)
            {
                throw ex;
            }

            return result;
        }
    }
}