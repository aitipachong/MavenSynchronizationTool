using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MavenSynchronizationTool.Core;
using MavenSynchronizationTool.Entities;
using System.IO;

namespace UT
{
    [TestClass]
    public class UT_XML
    {
        [TestMethod]
        public void UT_DeserializeMavenCentralConfigXML_V1()
        {
            try
            {
                string configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources/MavenCentralConfiguration.xml");
                if(!File.Exists(configPath))
                {
                    Assert.Fail(string.Format("Maven中央库与本地目录映射XML文件不存在，路径:{0}", configPath));
                    return;
                }
                Repositories repositories = XMLHelper.DeserializerXmlToString<Repositories>(configPath);
                Assert.AreEqual("https://repo1.maven.org/maven2/com/android/tools/build/gradle/", repositories.RepositoryCollection[0].CentralUrl);
            }
            catch(Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }
    }
}
