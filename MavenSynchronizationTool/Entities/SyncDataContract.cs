using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// ********************************************************************
// * 项目名称：		MavenSynchronizationTool
// * 程序集名称：	MavenSynchronizationTool.Entities
// * 文件名称：		SyncDataContract.cs
// * 编写者：		Lai.Qiang
// * 编写日期：		2016-03-08
// * 程序功能描述：
// *		同步数据契约结构类
// *        功能：
// *            1.记录MAVEN中央库各个路径、待下载内容等
// *            2.是否已下载
// *            3.状态等
// *
// * 程序变更日期：
// * 程序变更者：
// * 变更说明：
// * 
// ********************************************************************

namespace MavenSynchronizationTool.Entities
{
    /// <summary>
    /// 同步数据契约结构类
    /// </summary>
    public class SyncDataContract
    {
        public SyncDataContract(string url, string name)
        {
            this.Parent = null;
            this.Url = url;
            this.Name = name;
            this.Type = 1;
            this.DownloadState = 0;
        }

        public SyncDataContract(SyncDataContract parent, string url, string name, int type)
        {
            this.Parent = parent;
            this.Url = url;
            this.Name = name;
            this.Type = type;
            this.DownloadState = 0;
        }

        /// <summary>
        /// 父对象
        /// </summary>
        public SyncDataContract Parent { get; set; }

        /// <summary>
        /// URL地址
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 当前层/文件夹/文件名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 类型：1，目录；2，非jar文件；3，jar文件
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// 下载状态（"Type = 3"才成立）：0，未下载；1，下载中；2，已下载；
        /// </summary>
        public int DownloadState { get; set; }

        /// <summary>
        /// 子对象集合
        /// </summary>
        public List<SyncDataContract> ChildNodes { get; set; }
    }
}
