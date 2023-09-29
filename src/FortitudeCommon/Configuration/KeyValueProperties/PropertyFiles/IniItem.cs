namespace FortitudeCommon.Configuration.KeyValueProperties.PropertyFiles
{
    public class IniItem
    {
        protected internal IniItem(string name, string value, IniType type, string comment)
        {
            iniName = name;
            iniValue = value;
            iniType = type;
            Comment = comment;
        }

        #region Private variables

        private IniType iniType = IniType.Empty;
        private readonly string iniName = "";
        private string iniValue = "";

        #endregion

        #region Public properties

        public IniType Type
        {
            get { return iniType; }
            set { iniType = value; }
        }

        public string Value
        {
            get { return iniValue; }
            set { iniValue = value; }
        }

        public string Name
        {
            get { return iniName; }
        }

        public string Comment { get; set; }

        #endregion
    }
}