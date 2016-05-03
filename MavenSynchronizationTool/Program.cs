using MavenSynchronizationTool.Core.DBUtility;
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
        private const string IS_HTTP_PROXY_KEY = "IsHttpProxy";
        private const string HTTP_PROXY_HOST_KEY = "HttpProxyHost";
        private const string HTTP_PROXY_PORT_KEY = "HttpProxyPort";
        private const string HTTP_PROXY_USER_KEY = "HttpProxyUser";
        private const string HTTP_PROXY_PWD_KEY = "HttpProxyPwd";
        private const string DOWNLOAD_TYPE_KEY = "DownloadType";
        private const string MAPPING_CONFIG_KEY = "MappingConfig";

        public static bool IsHttpProxy { get; set; }
        public static string HttpProxyHost { get; set; }
        public static int HttpProxyPort { get; set; }
        public static string HttpProxyUser { get; set; }
        public static string HttpProxyPwd { get; set; }
        public static List<string> DownloadTypes { get; set; }
        public static string ConfigFilePath { get; set; }

        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            UT_ExecuteDataSet();

            GetSystemHttpProxyConfiguration();

            Application.Run(new Form1());
        }


        public static void UT_ExecuteDataSet()
        {
            string sqliteConnectionString = "Data Source={0};Version=3;";
            string sqlitePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "MavenSynchDb.db");
            if (!File.Exists(sqlitePath))
            {
                throw new ArgumentException("SQLite file is not exists.");
            }
            string connectionStr = string.Format(sqliteConnectionString, sqlitePath);
            string commandText = "SELECT id, suffix, ignore FROM DownloadSuffix WHERE ignore = 0";

            DataSet ds = SQLiteHelper.ExecuteDataSet(connectionStr, commandText, null);
            if (ds == null || ds.Tables == null || ds.Tables.Count == 0 || ds.Tables[0].Rows == null || ds.Tables[0].Rows.Count == 0)
            {
                throw new ArgumentException("ExecuteDataSet fail.");
            }

        }

        private static void GetSystemHttpProxyConfiguration()
        {
            try
            {
                IsHttpProxy = Convert.ToBoolean(ConfigurationManager.AppSettings[IS_HTTP_PROXY_KEY].ToString());
                HttpProxyHost = ConfigurationManager.AppSettings[HTTP_PROXY_HOST_KEY].ToString();
                HttpProxyPort = Convert.ToInt32(ConfigurationManager.AppSettings[HTTP_PROXY_PORT_KEY].ToString());
                HttpProxyUser = ConfigurationManager.AppSettings[HTTP_PROXY_USER_KEY].ToString();
                HttpProxyPwd = ConfigurationManager.AppSettings[HTTP_PROXY_PWD_KEY].ToString();

                string strDownloadType = ConfigurationManager.AppSettings[DOWNLOAD_TYPE_KEY].ToString();
                if (!string.IsNullOrEmpty(strDownloadType))
                {
                    DownloadTypes = new List<string>();
                    DownloadTypes = strDownloadType.Split(';').ToList<string>();
                }

                string relativePath = ConfigurationManager.AppSettings[MAPPING_CONFIG_KEY].ToString();
                if(!string.IsNullOrEmpty(relativePath))
                {
                    ConfigFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, relativePath);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "异常", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
        }
    }
}
