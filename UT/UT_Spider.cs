using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MavenSynchronizationTool.Core;

namespace UT
{
    [TestClass]
    public class UT_Spider
    {
        private const string REPO1_MAVEN_URL = "https://repo1.maven.org/maven2/";
        private const string HTTP_PROXY_HOST = "10.100.66.10";
        private const int HTTP_PROXY_PORT = 808;

        [TestMethod]
        public void UT_GetPageHtml_V1()
        {
            try
            {
                string html = Spider.GetPageHtml(REPO1_MAVEN_URL, HTTP_PROXY_HOST, HTTP_PROXY_PORT);
                if(!string.IsNullOrEmpty(html))
                {
                    Assert.IsTrue(true);
                }
                else
                {
                    Assert.Fail("未获取HTML内容");
                }
            }
            catch(Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }


        [TestMethod]
        public void UT_GetHrefContent_V1()
        {
            try
            {
                string html = Spider.GetPageHtml(REPO1_MAVEN_URL, HTTP_PROXY_HOST, HTTP_PROXY_PORT);
                string content = Spider.GetHrefContent(ref html);
                string content2 = Spider.GetHrefContent(ref html);
                string content3 = Spider.GetHrefContent(ref html);
                string content4 = Spider.GetHrefContent(ref html);

                if (!string.IsNullOrEmpty(content))
                {
                    Assert.IsTrue(true);
                }
                else
                {
                    Assert.Fail("未获取<a>XXX</a>内容");
                }
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }
    }
}
