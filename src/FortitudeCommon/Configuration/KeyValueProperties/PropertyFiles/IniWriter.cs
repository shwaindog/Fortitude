using System;
using System.IO;
using System.Text;

namespace FortitudeCommon.Configuration.KeyValueProperties.PropertyFiles
{

    #region IniWriteState enumeration

    public enum IniWriteState
    {
        Start,
        BeforeFirstSection,
        Section,
        Closed
    };

    #endregion

    public class IniWriter : IDisposable
    {
        #region Protected methods

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (textWriter != null)
                {
                    textWriter.Close();
                }
                if (BaseStream != null)
                {
                    BaseStream.Close();
                }
                disposed = true;

                if (disposing)
                {
                    GC.SuppressFinalize(this);
                }
            }
        }

        #endregion

        #region Private variables

        private int indentation;
        private IniWriteState writeState = IniWriteState.Start;
        private char commentDelimiter = ';';
        private char assignDelimiter = '=';
        private readonly TextWriter textWriter;
        private const string Eol = "\r\n";
        private readonly StringBuilder indentationBuffer = new StringBuilder();
        private bool disposed;

        #endregion

        #region Public properties

        public int Indentation
        {
            get { return indentation; }
            set
            {
                if (value < 0)
                    throw new ArgumentException("Negative values are illegal");

                indentation = value;
                indentationBuffer.Remove(0, indentationBuffer.Length);
                for (var i = 0; i < value; i++)
                    indentationBuffer.Append(' ');
            }
        }

        public bool UseValueQuotes { get; set; }

        public IniWriteState WriteState
        {
            get { return writeState; }
        }

        public char CommentDelimiter
        {
            get { return commentDelimiter; }
            set { commentDelimiter = value; }
        }

        public char AssignDelimiter
        {
            get { return assignDelimiter; }
            set { assignDelimiter = value; }
        }

        public Stream BaseStream { get; private set; }

        #endregion

        #region Constructors

        public IniWriter(string filePath)
            : this(new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
        {
        }

        public IniWriter(TextWriter writer)
        {
            textWriter = writer;
            var streamWriter = writer as StreamWriter;
            if (streamWriter != null)
            {
                BaseStream = streamWriter.BaseStream;
            }
        }

        public IniWriter(Stream stream)
            : this(new StreamWriter(stream))
        {
        }

        #endregion

        #region Public methods

        public void Close()
        {
            textWriter.Close();
            writeState = IniWriteState.Closed;
        }

        public void Flush()
        {
            textWriter.Flush();
        }

        public override string ToString()
        {
            return textWriter.ToString();
        }

        public void WriteSection(string section)
        {
            ValidateState();
            writeState = IniWriteState.Section;
            WriteLine("[" + section + "]");
        }

        public void WriteSection(string section, string comment)
        {
            ValidateState();
            writeState = IniWriteState.Section;
            WriteLine("[" + section + "]" + Comment(comment));
        }

        public void WriteKey(string key, string value)
        {
            ValidateStateKey();
            WriteLine(key + " " + assignDelimiter + " " + GetKeyValue(value));
        }

        public void WriteKey(string key, string value, string comment)
        {
            ValidateStateKey();
            WriteLine(key + " " + assignDelimiter + " " + GetKeyValue(value) + Comment(comment));
        }

        public void WriteEmpty()
        {
            ValidateState();
            if (writeState == IniWriteState.Start)
            {
                writeState = IniWriteState.BeforeFirstSection;
            }
            WriteLine("");
        }

        public void WriteEmpty(string comment)
        {
            ValidateState();
            if (writeState == IniWriteState.Start)
            {
                writeState = IniWriteState.BeforeFirstSection;
            }
            if (comment == null)
            {
                WriteLine("");
            }
            else
            {
                WriteLine(commentDelimiter + " " + comment);
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }

        #endregion

        #region Private methods
        ~IniWriter()
        {
            Dispose(false);
        }

        private string GetKeyValue(string text)
        {
            var result = UseValueQuotes ? MassageValue('"' + text + '"') : MassageValue(text);

            return result;
        }

        private void ValidateStateKey()
        {
            ValidateState();

            switch (writeState)
            {
                case IniWriteState.BeforeFirstSection:
                case IniWriteState.Start:
                    throw new InvalidOperationException("The WriteState is not Section");
                case IniWriteState.Closed:
                    throw new InvalidOperationException("The writer is closed");
            }
        }

        private void ValidateState()
        {
            if (writeState == IniWriteState.Closed)
            {
                throw new InvalidOperationException("The writer is closed");
            }
        }

        private string Comment(string text)
        {
            return (text == null) ? "" : (" " + commentDelimiter + " " + text);
        }

        private void Write(string value)
        {
            textWriter.Write(indentationBuffer + value);
        }

        private void WriteLine(string value)
        {
            Write(value + Eol);
        }

        private string MassageValue(string text)
        {
            return text.Replace("\n", "");
        }

        #endregion
    }
}