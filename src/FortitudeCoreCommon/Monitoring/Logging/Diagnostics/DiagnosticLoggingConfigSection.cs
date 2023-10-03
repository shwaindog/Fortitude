using System.Configuration;

namespace FortitudeCommon.Monitoring.Logging.Diagnostics
{
    public class DiagnosticLoggingConfigSection : ConfigurationSection
    {
        [ConfigurationProperty("", IsRequired = true, IsDefaultCollection = true)]
        public MyConfigInstanceCollection Settings
        {
            get { return (MyConfigInstanceCollection)this[""]; }
            set { this[""] = value; }
        }
    }

    public class MyConfigInstanceCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new PropertyFile();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((PropertyFile)element).Path;
        }
    }

    public class PropertyFile : ConfigurationElement
    {
        [ConfigurationProperty("path", DefaultValue = @".\diagnostics.logging.cfg", IsRequired = true)]
        [StringValidator(InvalidCharacters = "<>*?\"|", MinLength = 1, MaxLength = 120)]
        public string Path
        {
            get { return (string)this["path"]; }
            set { this["name"] = value; }
        }

        [ConfigurationProperty("liveUpdate", DefaultValue = "true", IsRequired = false)]
        public bool LiveUpdate
        {
            get { return (bool)this["liveUpdate"]; }
            set { this["liveUpdate"] = value; }
        }
    }
}