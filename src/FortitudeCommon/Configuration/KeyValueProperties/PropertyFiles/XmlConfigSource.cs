using System;
using System.IO;
using System.Linq;
using System.Xml;

namespace FortitudeCommon.Configuration.KeyValueProperties.PropertyFiles
{
    public class XmlConfigSource : ConfigSourceBase
    {
        #region Private variables

        private XmlDocument configDoc;

        #endregion

        #region Public properties

        public string SavePath { get; private set; }

        #endregion

        #region Constructors

        public XmlConfigSource()
        {
            configDoc = new XmlDocument();
            configDoc.LoadXml("<Nini/>");
            PerformLoad(configDoc);
        }

        public XmlConfigSource(string path)
        {
            Load(path);
        }

        public XmlConfigSource(XmlReader reader)
        {
            Load(reader);
        }

        #endregion

        #region Public methods

        public void Load(string path)
        {
            SavePath = path;
            configDoc = new XmlDocument();
            configDoc.Load(path);
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
            SavePath = path;
            Save();
        }

        public void Save(TextWriter writer)
        {
            MergeConfigsIntoDocument();
            configDoc.Save(writer);
            SavePath = null;
            OnSaved(new EventArgs());
        }

        public void Save(Stream stream)
        {
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

        #endregion

        #region Private methods
        private void MergeConfigsIntoDocument()
        {
            RemoveSections();
            foreach (IConfig config in Configs)
            {
                var keys = config.GetKeys();

                var node = GetSectionByName(config.Name);
                if (node == null)
                {
                    node = SectionNode(config.Name);
                    configDoc.DocumentElement.AppendChild(node);
                }
                RemoveKeys(config.Name);

                foreach (var t in keys)
                {
                    SetKey(node, t, config.Get(t));
                }
            }
        }
        private void RemoveSections()
        {
            foreach (XmlNode node in configDoc.DocumentElement.ChildNodes)
            {
                if (node.NodeType == XmlNodeType.Element
                    && node.Name == "Section")
                {
                    var attr = node.Attributes["Name"];
                    if (attr != null)
                    {
                        if (Configs[attr.Value] == null)
                        {
                            configDoc.DocumentElement.RemoveChild(node);
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
            var sectionNode = GetSectionByName(sectionName);

            if (sectionNode != null)
            {
                foreach (XmlNode node in sectionNode.ChildNodes)
                {
                    if (node.NodeType == XmlNodeType.Element
                        && node.Name == "Key")
                    {
                        var keyName = node.Attributes["Name"];
                        if (keyName != null)
                        {
                            if (Configs[sectionName].Get(keyName.Value) == null)
                            {
                                sectionNode.RemoveChild(node);
                            }
                        }
                        else
                        {
                            throw new ArgumentException("Name attribute not found in key");
                        }
                    }
                }
            }
        }
        
        private void PerformLoad(XmlDocument document)
        {
            Configs.Clear();

            Merge(this); // required for SaveAll

            if (document.DocumentElement.Name != "Nini")
            {
                throw new ArgumentException("Did not find Nini XML root node");
            }

            LoadSections(document.DocumentElement);
        }
        
        private void LoadSections(XmlNode rootNode)
        {
            foreach (XmlNode child in rootNode.ChildNodes)
            {
                if (child.NodeType == XmlNodeType.Element
                    && child.Name == "Section")
                {
                    var config = new ConfigBase(child.Attributes["Name"].Value, this);
                    Configs.Add(config);
                    LoadKeys(child, config);
                }
            }
        }
        
        private void LoadKeys(XmlNode node, ConfigBase config)
        {
            foreach (XmlNode child in node.ChildNodes)
            {
                if (child.NodeType == XmlNodeType.Element
                    && child.Name == "Key")
                {
                    config.Add(child.Attributes["Name"].Value,
                        child.Attributes["Value"].Value);
                }
            }
        }
        
        private void SetKey(XmlNode sectionNode, string key, string value)
        {
            var node = GetKeyByName(sectionNode, key);

            if (node == null)
            {
                CreateKey(sectionNode, key, value);
            }
            else
            {
                node.Attributes["Value"].Value = value;
            }
        }
        
        private void CreateKey(XmlNode sectionNode, string key, string value)
        {
            XmlNode node = configDoc.CreateElement("Key");
            var keyAttr = configDoc.CreateAttribute("Name");
            var valueAttr = configDoc.CreateAttribute("Value");
            keyAttr.Value = key;
            valueAttr.Value = value;

            node.Attributes.Append(keyAttr);
            node.Attributes.Append(valueAttr);

            sectionNode.AppendChild(node);
        }
        
        private XmlNode SectionNode(string name)
        {
            XmlNode result = configDoc.CreateElement("Section");
            var nameAttr = configDoc.CreateAttribute("Name");
            nameAttr.Value = name;
            result.Attributes.Append(nameAttr);

            return result;
        }

        private XmlNode GetSectionByName(string name)
        {
            return
                configDoc.DocumentElement.ChildNodes.Cast<XmlNode>()
                    .FirstOrDefault(
                        node =>
                            node.NodeType == XmlNodeType.Element && node.Name == "Section" &&
                            node.Attributes["Name"].Value == name);
        }

        private XmlNode GetKeyByName(XmlNode sectionNode, string name)
        {
            return
                sectionNode.ChildNodes.Cast<XmlNode>()
                    .FirstOrDefault(
                        node =>
                            node.NodeType == XmlNodeType.Element && node.Name == "Key" &&
                            node.Attributes["Name"].Value == name);
        }

        private bool IsSavable()
        {
            return (SavePath != null
                    && configDoc != null);
        }

        private void MergeDocumentIntoConfigs()
        {
            RemoveConfigs();

            foreach (XmlNode node in configDoc.DocumentElement.ChildNodes)
            {
                if (node.NodeType == XmlNodeType.Element
                    && node.Name == "Section")
                {
                    var sectionName = node.Attributes["Name"].Value;
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
                if (GetSectionByName(config.Name) == null)
                {
                    Configs.Remove(config);
                }
            }
        }

        private void RemoveConfigKeys(IConfig config)
        {
            var section = GetSectionByName(config.Name);

            var configKeys = config.GetKeys();
            foreach (var configKey in configKeys)
            {
                if (GetKeyByName(section, configKey) == null)
                {
                    config.Remove(configKey);
                }
            }

            foreach (XmlNode node in section.ChildNodes)
            {
                if (node.NodeType == XmlNodeType.Element
                    && node.Name == "Key")
                {
                    config.Set(node.Attributes["Name"].Value,
                        node.Attributes["Value"].Value);
                }
            }
        }

        #endregion
    }
}