using MavenSynchronizationTool.Core.DBUtility;
using MavenSynchronizationTool.Entities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MavenSynchronizationTool
{
    public static class Program
    {

        /// <summary>
        /// SQLite数据库链接字符串
        /// </summary>
        public static string SQLiteConnectionString { get; set; }

        /// <summary>
        /// 从SQLite数据库DownloadSuffix表提取的同步文件后缀列表
        /// </summary>
        public static List<string> SuffixList { get; set; } = new List<string>();

        public static bool IsHttpProxy { get; set; }            //是否使用HTTP代理
        public static string HttpProxyHost { get; set; }        //HTTP代理的Host
        public static int HttpProxyPort { get; set; }           //HTTP代理的Port
        public static string HttpProxyUser { get; set; }        //HTTP代理的用户名
        public static string HttpProxyPwd { get; set; }         //HTTP代理的密码

        /// <summary>
        /// 检索Maven中央库的文件名列表
        /// </summary>
        public static List<string> RetrievalFileNames { get; set; } = new List<string>();

        /// <summary>
        /// Maven中央库信息列表
        /// </summary>
        public static List<CentralRepository> CentralRepositoryList { get; set; } = new List<CentralRepository>();

        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            //DevExpress控件界面汉化
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("zh-CN");

            Application.Run(new FrmSysInit());
        }
    }
}
