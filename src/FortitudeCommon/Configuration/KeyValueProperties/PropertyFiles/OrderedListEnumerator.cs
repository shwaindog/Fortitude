using System;
using System.Collections;

namespace FortitudeCommon.Configuration.KeyValueProperties.PropertyFiles
{
    public class OrderedListEnumerator : IDictionaryEnumerator
    {
        #region Constructors
        
        internal OrderedListEnumerator(ArrayList arrayList)
        {
            list = arrayList;
        }

        #endregion

        #region Private variables

        private int index = -1;
        private readonly ArrayList list;

        #endregion

        #region Public properties

        object IEnumerator.Current
        {
            get
            {
                if (index < 0 || index >= list.Count)
                    throw new InvalidOperationException();

                return list[index];
            }
        }

        public DictionaryEntry Current
        {
            get
            {
                if (index < 0 || index >= list.Count)
                    throw new InvalidOperationException();

                return (DictionaryEntry) list[index];
            }
        }

        public DictionaryEntry Entry
        {
            get { return Current; }
        }

        public object Key
        {
            get { return Entry.Key; }
        }

        public object Value
        {
            get { return Entry.Value; }
        }

        #endregion

        #region Public methods

        public bool MoveNext()
        {
            index++;
            if (index >= list.Count)
                return false;

            return true;
        }

        public void Reset()
        {
            index = -1;
        }

        #endregion
    }
}