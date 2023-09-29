using System;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;

namespace FortitudeCommon.Configuration.KeyValueProperties.PropertyFiles
{
    public class DotNetConfigSource : ConfigSourceBase
    {
        #region Public properties

        public string SavePath { get; private set; }

        #endregion

        #region Private variables

        private readonly string[] sections;
        private XmlDocument configDoc;

        #endregion

        #region Constructors

        public DotNetConfigSource(string[] sections)
        {
            this.sections = sections;
            Load();
        }

        public DotNetConfigSource()
        {
            configDoc = new XmlDocument();
            configDoc.LoadXml("<configuration><configSections/></configuration>");
            PerformLoad(configDoc);
        }

        public DotNetConfigSource(string path)
        {
            Load(path);
        }

        public DotNetConfigSource(XmlReader reader)
        {
            Load(reader);
        }

        #endregion

        #region Public methods

        public void Load(string path)
        {
            SavePath = path;
            configDoc = new XmlDocument();
            configDoc.Load(SavePath);
            PerformLoad(configDoc);
        }

        public void Load(XmlReader reader)
        {
            configDoc = new XmlDocument();
            configDoc.Load(reader);
            PerformLoad(configDoc);
        }

        public override void Save()
        {
            if (!IsSavable())
            {
                throw new ArgumentException("Source cannot be saved in this state");
            }
            MergeConfigsIntoDocument();

            configDoc.Save(SavePath);
            base.Save();
        }

        public void Save(string path)
        {
            if (!IsSavable())
            {
                throw new ArgumentException("Source cannot be saved in this state");
            }

            SavePath = path;
            Save();
        }

        public void Save(TextWriter writer)
        {
            if (!IsSavable())
            {
                throw new ArgumentException("Source cannot be saved in this state");
            }

            MergeConfigsIntoDocument();
            configDoc.Save(writer);
            SavePath = null;
            OnSaved(new EventArgs());
        }

        public void Save(Stream stream)
        {
            if (!IsSavable())
            {
                throw new ArgumentException("Source cannot be saved in this state");
            }

            MergeConfigsIntoDocument();
            configDoc.Save(stream);
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

            configDoc = new XmlDocument();
            configDoc.Load(SavePath);
            MergeDocumentIntoConfigs();
            base.Reload();
        }

        public override string ToString()
        {
            MergeConfigsIntoDocument();
            var writer = new StringWriter();
            configDoc.Save(writer);

            return writer.ToString();
        }

        public static string GetFullConfigPath()
        {
            return (Assembly.GetCallingAssembly().Location + ".config");
        }

        #endregion

        #region Private methods
        
        private void MergeConfigsIntoDocument()
        {
            RemoveSections();
            foreach (IConfig config in Configs)
            {
                var keys = config.GetKeys();

                RemoveKeys(config.Name);
                var node = GetChildElement(config.Name) ?? SectionNode(config.Name);

                foreach (var t in keys)
                {
                    SetKey(node, t, config.Get(t));
                }
            }
        }
        
        private void Load()
        {
            Merge(this); 
            foreach (var t in sections)
            {
                LoadCollection(t, (NameValueCollection) ConfigurationManager.GetSection(t));
            }
        }
        
        private void PerformLoad(XmlDocument document)
        {
            Configs.Clear();

            Merge(this);

            if (document.DocumentElement.Name != "configuration")
            {
                throw new ArgumentException("Did not find configuration node");
            }

            LoadSections(document.DocumentElement);
        }
        
        private void LoadSections(XmlNode rootNode)
        {
            LoadOtherSection(rootNode, "appSettings");

            var configSections = GetChildElement(rootNode, "configSections");

            if (configSections == null)
            {
                return;
            }

            foreach (XmlNode node in configSections.ChildNodes)
            {
                if (node.NodeType == XmlNodeType.Element
                    && node.Name == "section")
                {
                    var config = new ConfigBase
                        (node.Attributes["name"].Value, this);

                    Configs.Add(config);
                    LoadKeys(rootNode, config);
                }
            }
        }
        
        private void LoadOtherSection(XmlNode rootNode, string nodeName)
        {
            var section = GetChildElement(rootNode, nodeName);

            if (section != null)
            {
                var config = new ConfigBase(section.Name, this);

                Configs.Add(config);
                LoadKeys(rootNode, config);
            }
        }
        
        private void LoadKeys(XmlNode rootNode, ConfigBase config)
        {
            var section = GetChildElement(rootNode, config.Name);

            foreach (XmlNode node in section.ChildNodes)
            {
                if (node.NodeType == XmlNodeType.Element
                    && node.Name == "add")
                {
                    config.Add(node.Attributes["key"].Value,
                        node.Attributes["value"].Value);
                }
            }
        }
        
        private void RemoveSections()
        {
            var configSections = GetChildElement("configSections");

            if (configSections == null)
            {
                return;
            }

            foreach (XmlNode node in configSections.ChildNodes)
            {
                if (node.NodeType == XmlNodeType.Element
                    && node.Name == "section")
                {
                    var attr = node.Attributes["name"];
                    if (attr != null)
                    {
                        if (Configs[attr.Value] == null)
                        {
                            node.ParentNode.RemoveChild(node);
                            
                            var dataNode = GetChildElement(attr.Value);
                            if (dataNode != null)
                            {
                                configDoc.DocumentElement.RemoveChild(dataNode);
                            }
                        }
                    }
                    else
                    {
                        throw new ArgumentException("Section name attribute not found");
                    }
                }
            }
        }
        
        private void RemoveKeys(string sectionName)
        {
            var node = GetChildElement(sectionName);

            if (node != null)
            {
                foreach (XmlNode key in node.ChildNodes)
                {
                    if (key.NodeType == XmlNodeType.Element
                        && key.Name == "add")
                    {
                        var keyName = key.Attributes["key"];
                        if (keyName != null)
                        {
                            if (Configs[sectionName].Get(keyName.Value) == null)
                            {
                                node.RemoveChild(key);
                            }
                        }
                        else
                        {
                            throw new ArgumentException("Key attribute not found in node");
                        }
                    }
                }
            }
        }

        private void SetKey(XmlNode sectionNode, string key, string value)
        {
            var keyNode = GetKey(sectionNode, key);

            if (keyNode == null)
            {
                CreateKey(sectionNode, key, value);
            }
            else
            {
                keyNode.Attributes["value"].Value = value;
            }
        }

        private XmlNode GetKey(XmlNode sectionNode, string keyName)
        {
            return
                sectionNode.ChildNodes.Cast<XmlNode>()
                    .FirstOrDefault(
                        node =>
                            node.NodeType == XmlNodeType.Element && node.Name == "add" &&
                            node.Attributes["key"].Value == keyName);
        }

        private void CreateKey(XmlNode sectionNode, string key, string value)
        {
            XmlNode node = configDoc.CreateElement("add");
            var keyAttr = configDoc.CreateAttribute("key");
            var valueAttr = configDoc.CreateAttribute("value");
            keyAttr.Value = key;
            valueAttr.Value = value;

            node.Attributes.Append(keyAttr);
            node.Attributes.Append(valueAttr);

            sectionNode.AppendChild(node);
        }

        private void LoadCollection(string name, NameValueCollection collection)
        {
            var config = new ConfigBase(name, this);

            if (collection == null)
            {
                throw new ArgumentException("Section was not found");
            }

            for (var i = 0; i < collection.Count; i++)
            {
                config.Add(collection.Keys[i], collection[i]);
            }

            Configs.Add(config);
        }

        private XmlNode SectionNode(string name)
        {
            XmlNode node = configDoc.CreateElement("section");
            var attr = configDoc.CreateAttribute("name");
            attr.Value = name;
            node.Attributes.Append(attr);

            attr = configDoc.CreateAttribute("type");
            attr.Value = "System.Configuration.NameValueSectionHandler";
            node.Attributes.Append(attr);

            var section = GetChildElement("configSections");
            section.AppendChild(node);
            
            XmlNode result = configDoc.CreateElement(name);
            configDoc.DocumentElement.AppendChild(result);

            return result;
        }

        private bool IsSavable()
        {
            return (SavePath != null
                    || configDoc != null);
        }

        private XmlNode GetChildElement(XmlNode parentNode, string name)
        {
            return
                parentNode.ChildNodes.Cast<XmlNode>()
                    .FirstOrDefault(node => node.NodeType == XmlNodeType.Element && node.Name == name);
        }

        private XmlNode GetChildElement(string name)
        {
            return GetChildElement(configDoc.DocumentElement, name);
        }

        private void MergeDocumentIntoConfigs()
        {
            RemoveConfigs();

            var configSections = GetChildElement("configSections");

            if (configSections == null)
            {
                return;
            }

            foreach (XmlNode node in configSections.ChildNodes)
            {
                if (node.NodeType == XmlNodeType.Element
                    && node.Name == "section")
                {
                    var sectionName = node.Attributes["name"].Value;
                    var config = Configs[sectionName];
                    if (config == null)
                    {
                        config = new ConfigBase(sectionName, this);
                        Configs.Add(config);
                    }
                    RemoveConfigKeys(config);
                }
            }
        }

        private void RemoveConfigs()
        {
            for (var i = Configs.Count - 1; i > -1; i--)
            {
                var config = Configs[i];
                if (GetChildElement(config.Name) == null)
                {
                    Configs.Remove(config);
                }
            }
        }

        private void RemoveConfigKeys(IConfig config)
        {
            var section = GetChildElement(config.Name);

            var configKeys = config.GetKeys();
            foreach (var configKey in configKeys)
            {
                if (GetKey(section, configKey) == null)
                {

                    config.Remove(configKey);
                }
            }

            foreach (XmlNode node in section.ChildNodes)
            {
                if (node.NodeType == XmlNodeType.Element
                    && node.Name == "add")
                {
                    config.Set(node.Attributes["key"].Value,
                        node.Attributes["value"].Value);
                }
            }
        }

        #endregion
    }
}