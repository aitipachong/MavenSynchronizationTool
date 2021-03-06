﻿using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MavenSynchronizationTool;
using MavenSynchronizationTool.BLL;

namespace UT
{
    /// <summary>
    /// UT_AnalysisMavenCentralHtmlProcess 的摘要说明
    /// </summary>
    [TestClass]
    public class UT_AnalysisMavenCentralHtmlProcess
    {
        public UT_AnalysisMavenCentralHtmlProcess()
        {
        }

        private TestContext testContextInstance;

        /// <summary>
        ///获取或设置测试上下文，该上下文提供
        ///有关当前测试运行及其功能的信息。
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region 附加测试特性
        //
        // 编写测试时，可以使用以下附加特性: 
        //
        // 在运行类中的第一个测试之前使用 ClassInitialize 运行代码
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // 在类中的所有测试都已运行之后使用 ClassCleanup 运行代码
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // 在运行每个测试之前，使用 TestInitialize 来运行代码
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // 在每个测试运行完之后，使用 TestCleanup 来运行代码
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void UT_Analysis_V1()
        {
            try
            {
                Program.HttpProxyHost = "10.100.66.10";
                Program.HttpProxyPort = 808;
                Program.HttpProxyUser = "";
                Program.HttpProxyPwd = "";
                Program.DownloadTypes = new List<string>();
                Program.DownloadTypes.Add("jar");
                Program.DownloadTypes.Add("xml");

                AnalysisMavenCentralHtmlProcess analysis = new AnalysisMavenCentralHtmlProcess();
                bool result = analysis.Analysis(null, "https://repo1.maven.org/maven2/", "maven2");
                Assert.AreEqual(true, result);
            }
            catch(Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }
    }
}
