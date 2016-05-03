using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MavenSynchronizationTool.Entities.Collection
{
    public interface IKeyObject<T>
    {
        T Key { get; }
    }
}