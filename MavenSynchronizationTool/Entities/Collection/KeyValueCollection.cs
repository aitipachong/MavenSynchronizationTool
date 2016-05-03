using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MavenSynchronizationTool.Entities.Collection
{
    public class KeyValueCollection<Key, Value> : IKeyValueCollection<Key, Value> where Value : IKeyObject<Key>
    {
        private Dictionary<Key, Value> m_KeyValueObj;
        private List<Value> m_TempList;

        public KeyValueCollection()
        {
            m_KeyValueObj = new Dictionary<Key, Value>();
            m_TempList = new List<Value>();
        }

        #region IKeyValueCollection<Key, Value> Members

        public Value this[Key key]
        {
            get { return GetValueByKey(key); }
        }

        public Value this[int index]
        {
            get { return m_TempList[index]; }
        }

        public Value GetValueByKey(Key key)
        {
            Value value;
            this.m_KeyValueObj.TryGetValue(key, out value);
            return value;
        }

        public void UpExchangedElement(int index)
        {
            if (index == 0) return;
            Value tempValue = this.m_TempList[index];
            this.m_TempList[index] = this.m_TempList[index - 1];
            this.m_TempList[index - 1] = tempValue;
            this.m_KeyValueObj.Clear();
            for(int loopi = 0;loopi < m_TempList.Count;loopi++)
            {
                this.m_KeyValueObj.Add(m_TempList[loopi].Key, m_TempList[loopi]);
            }
        }

        public void DownExchangeElement(int index)
        {
            if (index == (this.m_TempList.Count - 1)) return;
            Value tempValue = this.m_TempList[index];
            this.m_TempList[index] = this.m_TempList[index + 1];
            this.m_TempList[index + 1] = tempValue;
            this.m_KeyValueObj.Clear();
            for(int loopi = 0;loopi < m_TempList.Count;loopi++)
            {
                this.m_KeyValueObj.Add(m_TempList[loopi].Key, m_TempList[loopi]);
            }
        }

        public bool Contains(Key key)
        {
            return this.m_KeyValueObj.ContainsKey(key);
        }
        #endregion

        #region ICollection<Value> Members

        public void Add(Value item)
        {
            this.m_KeyValueObj.Add(item.Key, item);
            this.m_TempList.Add(item);
        }

        public void Insert(int index, Value item)
        {
            this.m_TempList.Insert(index, item);
            this.m_KeyValueObj.Clear();
            for (int loopi = 0; loopi < m_TempList.Count; loopi++)
            {
                this.m_KeyValueObj.Add(m_TempList[loopi].Key, m_TempList[loopi]);
            }
        }

        public void Clear()
        {
            this.m_TempList.Clear();
            this.m_KeyValueObj.Clear();
        }

        public bool Contains(Value item)
        {
            return this.m_KeyValueObj.ContainsValue(item);
        }

        public void CopyTo(Value[] array, int arrayIndex)
        {
            this.m_KeyValueObj.Values.CopyTo(array, arrayIndex);
            this.m_TempList.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return this.m_KeyValueObj.Count; }
        }

        public bool IsReadOnly
        {
            get { return true; }
        }

        public bool Remove(Value item)
        {
            this.m_TempList.Remove(item);
            return this.m_KeyValueObj.Remove(item.Key);
        }

        #endregion

        #region IEnumerable<Value> Members

        public IEnumerator<Value> GetEnumerator()
        {
            return m_TempList.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return m_TempList.GetEnumerator();
        }

        #endregion
    }
}