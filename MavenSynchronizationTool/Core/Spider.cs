using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

// ********************************************************************
// * 项目名称：		MavenSynchronizationTool
// * 程序集名称：	MavenSynchronizationTool.Core
// * 文件名称：		Spider.cs
// * 编写者：		Lai.Qiang
// * 编写日期：		2016-03-08
// * 程序功能描述：
// *		网络蜘蛛核心处理类
// *        功能：
// *            1.获取指定URL的HTML字符串
// *            2.解析<a href="xxx">xxx</a>HTML标签内容
// *            3.下载指定URL内容
// *
// * 程序变更日期：
// * 程序变更者：
// * 变更说明：
// * 
// ********************************************************************

namespace MavenSynchronizationTool.Core
{
    /// <summary>
    /// 网络蜘蛛核心处理类
    /// </summary>
    public class Spider
    {
        /// <summary>
        /// 获取指定URL页面HTML
        /// </summary>
        /// <param name="url">URL</param>
        /// <param name="proxyHost">HTTP代理IP（可为空，为空为不需要代理）</param>
        /// <param name="proxyPort">HTTP代理端口</param>
        /// <param name="proxyUser">HTTP代理用户名</param>
        /// <param name="prxoyPwd">HTTP代理密码</param>
        /// <returns></returns>
        public static string GetPageHtml(string url, string proxyHost = "", int proxyPort = 0, string proxyUser = "", string prxoyPwd = "")
        {
            if (string.IsNullOrEmpty(url)) return null;
            //设置WebRequest参数
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            request.UserAgent = "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.0; Trident/4.0)";
            if(!string.IsNullOrEmpty(proxyHost) && IsIP(proxyHost))
            {
                //设置HTTP代理
                request.Proxy = CreateHttpProxy(proxyHost, proxyPort, proxyUser, prxoyPwd);
            }

            //获取请求响应
            HttpWebResponse response = request.GetResponse() as HttpWebResponse;
            //获取请求发挥的流
            Stream responseStream = response.GetResponseStream();
            StreamReader sr = new StreamReader(responseStream, Encoding.UTF8);      //UTF8编码
            string html = sr.ReadToEnd();
            return html;
        }

        private const string START_STRING = "<a href=";
        private const string END_STRING = "</a>";

        /// <summary>
        /// 获取给定HTML字符串中第一个"<a href=\"xxxx\">xxxx</a>"内容
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static string GetHrefContent(ref string html)
        {
            if (string.IsNullOrEmpty(html)) return null;
            int iBodyStart = html.IndexOf(START_STRING, 0);
            if (iBodyStart == -1) return null;

            iBodyStart += START_STRING.Length;
            int iBodyEnd = html.IndexOf(END_STRING, iBodyStart);
            if (iBodyEnd == -1) return null;

            string tempContent = html.Substring(iBodyStart, iBodyEnd - iBodyStart);
            html = html.Substring(iBodyEnd + END_STRING.Length);
            //截取<a></a>之间的内容
            int iContentStart = tempContent.IndexOf(">", 0);
            string content = tempContent.Substring(iContentStart + 1);
            return content;
        }

        /// <summary>
        /// 验证IP地址
        /// </summary>
        /// <param name="ip">待验证的字符串</param>
        /// <returns>true:ip地址；false:非ip地址</returns>
        public static bool IsIP(string ip)
        {
            return Regex.IsMatch(ip, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$");
        }

        /// <summary>
        /// 创建HTTP代理
        /// </summary>
        /// <param name="proxyHost"></param>
        /// <param name="proxyPort"></param>
        /// <param name="user"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        public static WebProxy CreateHttpProxy(string proxyHost, int proxyPort, string user,string pwd)
        {
            if (string.IsNullOrEmpty(proxyHost) || proxyPort <= 0) return null;
            WebProxy proxy = new WebProxy(proxyHost, proxyPort);
            if(!string.IsNullOrEmpty(user) && !string.IsNullOrEmpty(pwd))
            {
                proxy.Credentials = new NetworkCredential(user, pwd);
            }
            return proxy;
        }
    }
}