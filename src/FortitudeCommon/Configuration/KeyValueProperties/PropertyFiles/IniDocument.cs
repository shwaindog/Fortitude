using System;
using System.Collections;
using System.IO;
using System.Threading;
using FortitudeCommon.Monitoring.Logging;

namespace FortitudeCommon.Configuration.KeyValueProperties.PropertyFiles
{

    #region IniFileType enumeration

    public enum IniFileType
    {
        Standard,
        PythonStyle,
        SambaStyle,
        MysqlStyle,
        WindowsStyle
    }

    #endregion

    public class IniDocument
    {
        #region Public properties

        public IniFileType FileType
        {
            get { return fileType; }
            set { fileType = value; }
        }

        #endregion

        #region Private variables

        private static readonly IFLogger Logger = FLoggerFactory.Instance.GetLogger("DiagnosticSettings");
        private readonly ArrayList initialComment = new ArrayList();
        private readonly IniSectionCollection sections = new IniSectionCollection();
        private IniFileType fileType = IniFileType.Standard;

        #endregion

        #region Constructors

        public IniDocument(string filePath)
        {
            fileType = IniFileType.Standard;
            Load(filePath);
        }

        public IniDocument(string filePath, IniFileType type)
        {
            fileType = type;
            Load(filePath);
        }

        public IniDocument(TextReader reader)
        {
            fileType = IniFileType.Standard;
            Load(reader);
        }

        public IniDocument(TextReader reader, IniFileType type)
        {
            fileType = type;
            Load(reader);
        }

        public IniDocument(Stream stream)
        {
            fileType = IniFileType.Standard;
            Load(stream);
        }

        public IniDocument(Stream stream, IniFileType type)
        {
            fileType = type;
            Load(stream);
        }

        public IniDocument(IniReader reader)
        {
            fileType = IniFileType.Standard;
            Load(reader);
        }

        public IniDocument()
        {
        }

        #endregion

        #region Public methods

        public IniSectionCollection Sections
        {
            get { return sections; }
        }

        public void Load(string filePath)
        {
            for (var i = 0; i < 10; i++)
            {
                try
                {
                    using (
                        var fileStream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Read, FileShare.None,
                            2048,
                            FileOptions.RandomAccess | FileOptions.Asynchronous))
                    {
                        using (var streamReader = new StreamReader(fileStream))
                        {
                            Load(streamReader);
                            break;
                        }
                    }
                }
                catch (IOException e)
                {
                    Logger.Info("IniDocument.Load(string filePath) had {0} will retry", e);
                    Thread.Sleep(100*i);
                }
                catch (Exception e)
                {
                    Logger.Info("IniDocument.Load(string filePath) had {0}", e);
                    break;
                }
                finally
                {
                    GC.Collect();
                }
            }
        }

        public void Load(TextReader reader)
        {
            Load(GetIniReader(reader, fileType));
        }

        public void Load(Stream stream)
        {
            Load(new StreamReader(stream));
        }

        public void Load(IniReader reader)
        {
            LoadReader(reader);
        }

        public void Save(TextWriter textWriter)
        {
            var writer = GetIniWriter(textWriter, fileType);

            foreach (string comment in initialComment)
            {
                writer.WriteEmpty(comment);
            }

            for (var j = 0; j < sections.Count; j++)
            {
                var section = sections[j];
                writer.WriteSection(section.Name, section.Comment);
                for (var i = 0; i < section.ItemCount; i++)
                {
                    var item = section.GetItem(i);
                    switch (item.Type)
                    {
                        case IniType.Key:
                            writer.WriteKey(item.Name, item.Value, item.Comment);
                            break;
                        case IniType.Empty:
                            writer.WriteEmpty(item.Comment);
                            break;
                    }
                }
            }

            writer.Close();
        }

        public void Save(string filePath)
        {
            var writer = new StreamWriter(filePath);
            Save(writer);
            writer.Close();
        }

        public void Save(Stream stream)
        {
            Save(new StreamWriter(stream));
        }

        #endregion

        #region Private methods

        private void LoadReader(IniReader reader)
        {
            reader.IgnoreComments = false;
            var sectionFound = false;
            IniSection section = null;

            try
            {
                while (reader.Read())
                {
                    switch (reader.Type)
                    {
                        case IniType.Empty:
                            if (!sectionFound)
                            {
                                initialComment.Add(reader.Comment);
                            }
                            else
                            {
                                section.Set(reader.Comment);
                            }

                            break;
                        case IniType.Section:
                            sectionFound = true;

                            if (sections[reader.Name] != null)
                            {
                                sections.Remove(reader.Name);
                            }
                            section = new IniSection(reader.Name, reader.Comment);
                            sections.Add(section);

                            break;
                        case IniType.Key:
                            if (section.GetValue(reader.Name) == null)
                            {
                                section.Set(reader.Name, reader.Value, reader.Comment);
                            }
                            break;
                    }
                }
            }
            finally
            {

                reader.Close();
            }
        }

        private IniReader GetIniReader(TextReader reader, IniFileType type)
        {
            var result = new IniReader(reader);

            switch (type)
            {
                case IniFileType.Standard:

                    break;
                case IniFileType.PythonStyle:
                    result.AcceptCommentAfterKey = false;
                    result.SetCommentDelimiters(new[] {';', '#'});
                    result.SetAssignDelimiters(new[] {':'});
                    break;
                case IniFileType.SambaStyle:
                    result.AcceptCommentAfterKey = false;
                    result.SetCommentDelimiters(new[] {';', '#'});
                    result.LineContinuation = true;
                    break;
                case IniFileType.MysqlStyle:
                    result.AcceptCommentAfterKey = false;
                    result.AcceptNoAssignmentOperator = true;
                    result.SetCommentDelimiters(new[] {'#'});
                    result.SetAssignDelimiters(new[] {':', '='});
                    break;
                case IniFileType.WindowsStyle:
                    result.ConsumeAllKeyText = true;
                    break;
            }

            return result;
        }

        private IniWriter GetIniWriter(TextWriter reader, IniFileType type)
        {
            var result = new IniWriter(reader);

            switch (type)
            {
                case IniFileType.Standard:
                case IniFileType.WindowsStyle:
                    // do nothing
                    break;
                case IniFileType.PythonStyle:
                    result.AssignDelimiter = ':';
                    result.CommentDelimiter = '#';
                    break;
                case IniFileType.SambaStyle:
                case IniFileType.MysqlStyle:
                    result.AssignDelimiter = '=';
                    result.CommentDelimiter = '#';
                    break;
            }

            return result;
        }

        #endregion
    }
}