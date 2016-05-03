using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MavenSynchronizationTool.Entities.Collection
{
    public interface IKeyValueCollection<IKey, IValue> : ICollection<IValue> where IValue : IKeyObject<IKey>
    {
        IValue this[IKey key] { get; }
        IValue this[int index] { get; }

        IValue GetValueByKey(IKey key);
        bool Contains(IKey key);
    }
}