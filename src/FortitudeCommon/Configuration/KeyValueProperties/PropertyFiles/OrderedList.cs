using System;
using System.Collections;

namespace FortitudeCommon.Configuration.KeyValueProperties.PropertyFiles
{
    public class OrderedList : IDictionary
    {
        #region Private variables

        private int IndexOf(object key)
        {
            for (var i = 0; i < list.Count; i++)
            {
                if (((DictionaryEntry) list[i]).Key.Equals(key))
                {
                    return i;
                }
            }
            return -1;
        }

        #endregion

        #region Private variables

        private readonly Hashtable table = new Hashtable();
        private readonly ArrayList list = new ArrayList();

        #endregion

        #region Public properties

        public int Count
        {
            get { return list.Count; }
        }

        public bool IsFixedSize
        {
            get { return false; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool IsSynchronized
        {
            get { return false; }
        }

        public object this[int index]
        {
            get { return ((DictionaryEntry) list[index]).Value; }
            set
            {
                if (index < 0 || index >= Count)
                    throw new ArgumentOutOfRangeException("index");

                var key = ((DictionaryEntry) list[index]).Key;
                list[index] = new DictionaryEntry(key, value);
                table[key] = value;
            }
        }

        public object this[object key]
        {
            get { return table[key]; }
            set
            {
                if (table.Contains(key))
                {
                    table[key] = value;
                    table[IndexOf(key)] = new DictionaryEntry(key, value);
                    return;
                }
                Add(key, value);
            }
        }

        public ICollection Keys
        {
            get
            {
                var retList = new ArrayList();
                foreach (var t in list)
                {
                    retList.Add(((DictionaryEntry) t).Key);
                }
                return retList;
            }
        }

        public ICollection Values
        {
            get
            {
                var retList = new ArrayList();
                foreach (var t in list)
                {
                    retList.Add(((DictionaryEntry) t).Value);
                }
                return retList;
            }
        }

        public object SyncRoot
        {
            get { return this; }
        }

        #endregion

        #region Public methods

        public void Add(object key, object value)
        {
            table.Add(key, value);
            list.Add(new DictionaryEntry(key, value));
        }

        public void Clear()
        {
            table.Clear();
            list.Clear();
        }

        public bool Contains(object key)
        {
            return table.Contains(key);
        }

        public void CopyTo(Array array, int index)
        {
            table.CopyTo(array, index);
        }

        public void CopyTo(DictionaryEntry[] array, int index)
        {
            table.CopyTo(array, index);
        }

        public void Insert(int index, object key, object value)
        {
            if (index > Count)
                throw new ArgumentOutOfRangeException("index");

            table.Add(key, value);
            list.Insert(index, new DictionaryEntry(key, value));
        }

        public void Remove(object key)
        {
            table.Remove(key);
            list.RemoveAt(IndexOf(key));
        }

        public void RemoveAt(int index)
        {
            if (index >= Count)
                throw new ArgumentOutOfRangeException("index");

            table.Remove(((DictionaryEntry) list[index]).Key);
            list.RemoveAt(index);
        }

        public IEnumerator GetEnumerator()
        {
            return new OrderedListEnumerator(list);
        }

        IDictionaryEnumerator IDictionary.GetEnumerator()
        {
            return new OrderedListEnumerator(list);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new OrderedListEnumerator(list);
        }

        #endregion
    }
}