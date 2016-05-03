using MavenSynchronizationTool.Entities.Collection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MavenSynchronizationTool.Entities
{
    [XmlRoot("repositories")]
    public class Repositories : IDisposable
    {
        [XmlElement("repository")]
        public KeyValueCollection<string, Repository> RepositoryCollection { get; set; }

        public void Dispose()
        {
            for(int loopi = 0;loopi < RepositoryCollection.Count;loopi++)
            {
                RepositoryCollection[loopi].Dispose();
            }

            this.RepositoryCollection.Clear();
            this.RepositoryCollection = null;
        }
    }

    public class Repository : IKeyObject<string>, IDisposable, ICloneable
    {
        [XmlAttribute("CentralUrl")]
        public string CentralUrl { get; set; }

        [XmlAttribute("LocalFolder")]
        public string LocalFolder { get; set; }

        [XmlText]
        public string Description { get; set; }

        public string Key
        {
            get
            {
                return this.CentralUrl;
            }
        }

        public void Dispose()
        {
            this.CentralUrl = null;
            this.LocalFolder = null;
            this.Description = null;
        }

        public object Clone()
        {
            Repository rep = new Repository();
            rep.CentralUrl = this.CentralUrl;
            rep.LocalFolder = this.LocalFolder;
            rep.Description = this.Description;
            return rep;
        }
    }
}
