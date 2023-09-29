using System.Collections;

namespace FortitudeCommon.Configuration.KeyValueProperties.PropertyFiles
{
    public class IniSection
    {
        #region Private variables

        private readonly OrderedList configList = new OrderedList();
        private readonly string name = "";
        private int commentCount;

        #endregion

        #region Constructors

        public IniSection(string name, string comment)
        {
            this.name = name;
            Comment = comment;
        }

        public IniSection(string name)
            : this(name, null)
        {
        }

        #endregion

        #region Public properties

        public string Name
        {
            get { return name; }
        }

        public string Comment { get; private set; }

        public int ItemCount
        {
            get { return configList.Count; }
        }

        #endregion

        #region Public methods

        public string GetValue(string key)
        {
            string result = null;

            if (Contains(key))
            {
                var item = (IniItem) configList[key];
                result = item.Value;
            }

            return result;
        }

        public IniItem GetItem(int index)
        {
            return (IniItem) configList[index];
        }

        public string[] GetKeys()
        {
            var list = new ArrayList();

            foreach (var t in configList)
            {
                var item = (DictionaryEntry) t;
                var iniItem = item.Value as IniItem;
                if (iniItem.Type == IniType.Key)
                {
                    list.Add(item.Key);
                }
            }
            var result = new string[list.Count];
            list.CopyTo(result, 0);

            return result;
        }

        public bool Contains(string key)
        {
            return (configList[key] != null);
        }

        public void Set(string key, string value, string comment)
        {
            IniItem item;

            if (Contains(key))
            {
                item = (IniItem) configList[key];
                item.Value = value;
                item.Comment = comment;
            }
            else
            {
                item = new IniItem(key, value, IniType.Key, comment);
                configList.Add(key, item);
            }
        }

        public void Set(string key, string value)
        {
            Set(key, value, null);
        }

        public void Set(string comment)
        {
            var commentName = "#comment" + commentCount;
            var item = new IniItem(commentName, null,
                IniType.Empty, comment);
            configList.Add(commentName, item);

            commentCount++;
        }

        public void Set()
        {
            Set(null);
        }

        public void Remove(string key)
        {
            if (Contains(key))
            {
                configList.Remove(key);
            }
        }

        #endregion
    }
}