using MavenSynchronizationTool.Core;
using MavenSynchronizationTool.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

// ********************************************************************
// * 项目名称：		MavenSynchronizationTool
// * 程序集名称：	MavenSynchronizationTool.BLL
// * 文件名称：		AnalysisMavenCentralHtmlProcess.cs
// * 编写者：		Lai.Qiang
// * 编写日期：		2016-03-08
// * 程序功能描述：
// *		解析Maven中央库HTML页面，形成基于<a href="xxxx">xxxx</a>的链表对象
// *
// * 程序变更日期：
// * 程序变更者：
// * 变更说明：
// * 
// ********************************************************************

namespace MavenSynchronizationTool.BLL
{
    /// <summary>
    /// 解析Maven中央库HTML页面，形成基于<a href="xxxx">xxxx</a>的链表对象
    /// </summary>
    public class AnalysisMavenCentralHtmlProcess
    {
        /// <summary>
        /// 分析结果
        /// </summary>
        public SyncDataContract resultData { get; set; }

        /// <summary>
        /// 分析
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public bool Analysis(SyncDataContract node, string url, string name)
        {
            bool result = false;
            string html = string.Empty;

            //创建根对象
            if(node == null) node = new SyncDataContract(url, name);
            try
            {
                //获取rootURL的HTML
                html = Spider.GetPageHtml(url, Program.HttpProxyHost, Program.HttpProxyPort, Program.HttpProxyUser, Program.HttpProxyPwd);
                if (string.IsNullOrEmpty(html)) return false;
                //解析给定url的html<a href="xxx">xxx</a>节点
                this.AnalysisHrefTags(node, html);

                //递归子节点，直到遍历完maven中央库所有内容
                this.RecursiveScanChildNode(node);

                result = true;
                this.resultData = node;
            }
            catch(Exception ex)
            {
                throw ex;
            }

            return result;
        }

        private void AnalysisHrefTags(SyncDataContract parent, string html)
        {
            try
            {
                while (true)
                {
                    string content = Spider.GetHrefContent(ref html);
                    if (string.IsNullOrEmpty(content))
                    {
                        break;
                    }
                    //分析内容类型
                    if (content.LastIndexOf("..") >= 0) continue;
                    int type = this.AnalysisContentType(content);
                    //创建节点对象
                    string nodeUrl = SetNodeUrl(parent.Url, content);
                    SyncDataContract node = new SyncDataContract(parent, nodeUrl, content, type);
                    if (parent.ChildNodes == null) parent.ChildNodes = new List<SyncDataContract>();
                    parent.ChildNodes.Add(node);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private int AnalysisContentType(string content)
        {
            int iLast = content.LastIndexOf(".");
            if (iLast == -1)
            {
                return 1;
            }
            else
            {
                string strExtension = content.Substring(iLast + 1).Trim().ToLower();
                if (strExtension.EndsWith("/"))
                {
                    return 1;
                }
                else
                {
                    //if(Program.DownloadTypes.FindIndex(s => s == strExtension) >= 0)
                    //{
                    //    return 3;
                    //}
                    //else
                    //{
                    //    return 2;
                    //}
                    return 3;
                }
            }
        }

        private static bool IsNumber(string content)
        {
            return Regex.IsMatch(content, @"^/ d + (/.) ?/ d *$");
        }

        private string SetNodeUrl(string parentUrl, string currentName)
        {
            if(parentUrl.EndsWith("/"))
            {
                return parentUrl + currentName;
            }
            else
            {
                return parentUrl + "/" + currentName;
            }
        }

        private void RecursiveScanChildNode(SyncDataContract node)
        {
            try
            {
                if(node.ChildNodes != null && node.ChildNodes.Count > 0)
                {
                    for(int loopi = 0;loopi < node.ChildNodes.Count;loopi++)
                    {
                        if(node.ChildNodes[loopi].Type == 1)
                        {
                            this.Analysis(node.ChildNodes[loopi], node.ChildNodes[loopi].Url, node.ChildNodes[loopi].Name);
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}
