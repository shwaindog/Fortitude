using System;
using System.IO;
using System.Threading;
using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.Chronometry;

namespace FortitudeCommon.Configuration.KeyValueProperties.PropertyFiles
{
    public class IniConfigSource : ConfigSourceBase
    {
        public event EventHandler<EventArgs> FileChanged;

        private void OnFileChanged()
        {
            var fileChangedEvent = FileChanged;
            if (fileChangedEvent != null && lastFireEventTime + TimeSpan.FromMilliseconds(500) < TimeContext.UtcNow)
            {
                lastFireEventTime = TimeContext.UtcNow;
                ThreadPool.RegisterWaitForSingleObject(new AutoResetEvent(false), (state, timeouted) =>
                {
                    try
                    {
                        fileChangedEvent(this, new EventArgs());
                    }
                    catch (Exception e)
                    {
                        Logger.Info("IniConfigSource.OnFileChanged.ThreadPool.RegisterWaitForSingleObject had {0}", e);
                    }
                }, null, 450, true);
            }
        }

        #region Private variables

        private bool caseSensitive = true;
        private IniDocument iniDocument;
        private readonly FileSystemWatcher fileSystemWatcher;
        private DateTime lastFireEventTime;
        private static readonly IFLogger Logger = FLoggerFactory.Instance.GetLogger("DiagnosticSettings");

        #endregion

        #region Constructors

        public IniConfigSource()
        {
            iniDocument = new IniDocument();
        }

        public IniConfigSource(string filePath)
        {
            var fileInfo = new FileInfo(filePath);
            fileSystemWatcher = new FileSystemWatcher(fileInfo.DirectoryName, fileInfo.Name);
            fileSystemWatcher.Changed += (sender, args) =>
            {
                try
                {
                    OnFileChanged();
                }
                catch (Exception e)
                {
                    Logger.Info("IniConfigSource.ctor had {0}", e);
                }
            };
            fileSystemWatcher.NotifyFilter = NotifyFilters.Attributes | NotifyFilters.CreationTime |
                                             NotifyFilters.FileName |
                                             NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.Size |
                                             NotifyFilters.Security;
            fileSystemWatcher.EnableRaisingEvents = true;
            Load(filePath);
        }

        public IniConfigSource(TextReader reader)
        {
            Load(reader);
        }

        public IniConfigSource(IniDocument document)
        {
            Load(document);
        }

        public IniConfigSource(Stream stream)
        {
            Load(stream);
        }

        #endregion

        #region Public properties

        public bool CaseSensitive
        {
            get { return caseSensitive; }
            set { caseSensitive = value; }
        }

        public string SavePath { get; private set; }

        #endregion

        #region Public methods

        public void Load(string filePath)
        {
            try
            {
                Logger.Info("About to load configuration file at {0}", new FileInfo(filePath).FullName);
                using (
                    var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.None, 2048,
                        FileOptions.RandomAccess | FileOptions.Asynchronous))
                {
                    using (var streamReader = new StreamReader(fileStream))
                    {
                        Load(streamReader);
                    }
                }
            }
            catch (Exception e)
            {
                Logger.Error("Error loading configuration file at {0} got {1}", new FileInfo(filePath).FullName, e);
            }
            GC.Collect();
            SavePath = filePath;
        }

        public void Load(TextReader reader)
        {
            Load(new IniDocument(reader));
        }

        public void Load(IniDocument document)
        {
            Configs.Clear();

            Merge(this);
            iniDocument = document;
            Load();
        }

        public void Load(Stream stream)
        {
            Load(new StreamReader(stream));
        }

        public override void Save()
        {
            if (!IsSavable())
            {
                throw new ArgumentException("Source cannot be saved in this state");
            }

            MergeConfigsIntoDocument();

            iniDocument.Save(SavePath);
            base.Save();
        }

        public void Save(string path)
        {
            SavePath = path;
            Save();
        }

        public void Save(TextWriter writer)
        {
            MergeConfigsIntoDocument();
            iniDocument.Save(writer);
            SavePath = null;
            OnSaved(new EventArgs());
        }

        public void Save(Stream stream)
        {
            MergeConfigsIntoDocument();
            iniDocument.Save(stream);
            SavePath = null;
            OnSaved(new EventArgs());
        }

        public override void Reload()
        {
            if (SavePath == null)
            {
                throw new ArgumentException("Error reloading: You must have "
                                            + "the loaded the source from a file");
            }

            iniDocument = new IniDocument(SavePath);
            MergeDocumentIntoConfigs();
            base.Reload();
        }

        public override string ToString()
        {
            MergeConfigsIntoDocument();
            var writer = new StringWriter();
            iniDocument.Save(writer);

            return writer.ToString();
        }

        #endregion

        #region Private methods

        private void MergeConfigsIntoDocument()
        {
            RemoveSections();
            foreach (IConfig config in Configs)
            {
                var keys = config.GetKeys();

                if (iniDocument.Sections[config.Name] == null)
                {
                    var section = new IniSection(config.Name);
                    iniDocument.Sections.Add(section);
                }
                RemoveKeys(config.Name);

                foreach (var t in keys)
                {
                    iniDocument.Sections[config.Name].Set(t, config.Get(t));
                }
            }
        }

        private void RemoveSections()
        {
            for (var i = 0; i < iniDocument.Sections.Count; i++)
            {
                var section = iniDocument.Sections[i];
                if (Configs[section.Name] == null)
                {
                    iniDocument.Sections.Remove(section.Name);
                }
            }
        }

        private void RemoveKeys(string sectionName)
        {
            var section = iniDocument.Sections[sectionName];

            if (section != null)
            {
                foreach (var key in section.GetKeys())
                {
                    if (Configs[sectionName].Get(key) == null)
                    {
                        section.Remove(key);
                    }
                }
            }
        }

        private void Load()
        {
            for (var j = 0; j < iniDocument.Sections.Count; j++)
            {
                var section = iniDocument.Sections[j];
                var config = new IniConfig(section.Name, this);

                for (var i = 0; i < section.ItemCount; i++)
                {
                    var item = section.GetItem(i);

                    if (item.Type == IniType.Key)
                    {
                        config.Add(item.Name, item.Value);
                    }
                }

                Configs.Add(config);
            }
        }

        private void MergeDocumentIntoConfigs()
        {
            RemoveConfigs();

            for (var i = 0; i < iniDocument.Sections.Count; i++)
            {
                var section = iniDocument.Sections[i];

                var config = Configs[section.Name];
                if (config == null)
                {
                    config = new ConfigBase(section.Name, this);
                    Configs.Add(config);
                }
                RemoveConfigKeys(config);
            }
        }

        private void RemoveConfigs()
        {
            for (var i = Configs.Count - 1; i > -1; i--)
            {
                var config = Configs[i];
                if (iniDocument.Sections[config.Name] == null)
                {
                    Configs.Remove(config);
                }
            }
        }

        private void RemoveConfigKeys(IConfig config)
        {
            var section = iniDocument.Sections[config.Name];

            var configKeys = config.GetKeys();
            foreach (var configKey in configKeys)
            {
                if (!section.Contains(configKey))
                {

                    config.Remove(configKey);
                }
            }

            var keys = section.GetKeys();
            for (var i = 0; i < keys.Length; i++)
            {
                var key = keys[i];
                string oldValue = null;
                if (config.Contains(key))
                {
                    oldValue = config.Get(key);
                }
                var newValue = section.GetItem(i).Value;
                if (oldValue == null || oldValue != newValue)
                {
                    config.Set(key, newValue);
                }
            }
        }

        private bool IsSavable()
        {
            return (SavePath != null);
        }

        #endregion
    }
}