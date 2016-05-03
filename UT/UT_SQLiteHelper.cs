using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Data;
using MavenSynchronizationTool.Core.DBUtility;

namespace UT
{
    [TestClass]
    public class UT_SQLiteHelper
    {
        private string sqliteConnectionString = "Data Source={0};Version=3;";

        [TestMethod]
        public void UT_ExecuteDataSet_V1()
        {
            string sqlitePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "MavenSynchDb.db");
            if(!File.Exists(sqlitePath))
            {
                Assert.Fail("SQLite file is not exists.");
                return;
            }
            string connectionStr = string.Format(sqliteConnectionString, sqlitePath);
            string commandText = "SELECT id, suffix, ignore FROM DownloadSuffix";

            DataSet ds = SQLiteHelper.ExecuteDataSet(connectionStr, commandText, null);
            if(ds == null || ds.Tables == null || ds.Tables.Count == 0 || ds.Tables[0].Rows == null || ds.Tables[0].Rows.Count == 0)
            {
                Assert.Fail("ExecuteDataSet fail.");
                return;
            }

            Assert.AreEqual(4, ds.Tables[0].Rows.Count);
        }
    }
}
