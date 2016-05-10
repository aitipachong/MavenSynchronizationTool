using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MavenSynchronizationTool.Entities
{
    /// <summary>
    /// SQLite库中“DownloadSuffix”表实体定义
    /// </summary>
    public class DownloadSuffix
    {
        public Int64 ID { get; set; }
        public string Suffix { get; set; }
        public bool Ignore { get; set; }
    }

    /// <summary>
    /// SQLite库中“CentralRepository”表实体定义
    /// </summary>
    public class CentralRepository
    {
        public CentralRepository()
        {
            this.IndexList = new List<CentralRepositoryIndex>();
        }

        public Int64 ID { get; set; }
        public string CentralName { get; set; }
        public string CentralUrl { get; set; }
        public string LocalFolder { get; set; }
        public int SynchState { get; set; }

        public List<CentralRepositoryIndex> IndexList { get; set; }
    }

    /// <summary>
    /// SQLite库中“CentralRepositoryIndex”表实体定义
    /// </summary>
    public class CentralRepositoryIndex
    {
        public CentralRepositoryIndex()
        {
            this.MetadataList = new List<CentralRepositoryMetadata>();
        }

        public Int64 ID { get; set; }
        public Int64 CentralID { get; set; }
        public string IndexName { get; set; }
        public int SynchState { get; set; }

        public List<CentralRepositoryMetadata> MetadataList { get; set; }
    }

    /// <summary>
    /// SQLite库中“CentralRepositoryMetadata”表实体定义
    /// </summary>
    public class CentralRepositoryMetadata
    {
        public Int64 ID { get; set; }
        public Int64 CentralID { get; set; }
        public Int64 IndexID { get; set; }
        public string GroupId { get; set; }
        public string ArtifactId { get; set; }
        public string Versions { get; set; }

        public List<string> VersionCollections
        {
            get
            {
                if (string.IsNullOrEmpty(this.Versions)) return null;
                return Versions.Split('|').ToList<string>();
            }
        }
    }
}