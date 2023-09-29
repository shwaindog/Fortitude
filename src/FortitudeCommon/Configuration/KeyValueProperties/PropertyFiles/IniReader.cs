using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace FortitudeCommon.Configuration.KeyValueProperties.PropertyFiles
{

    #region IniReadState enumeration

    public enum IniReadState
    {
        Closed,
        EndOfFile,
        Error,
        Initial,
        Interactive
    };

    #endregion

    #region IniType enumeration

    public enum IniType
    {
        Section,
        Key,
        Empty
    }

    #endregion

    public class IniReader : IDisposable
    {
        #region Protected methods

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                textReader.Close();
                disposed = true;

                if (disposing)
                {
                    GC.SuppressFinalize(this);
                }
            }
        }

        #endregion

        #region Private variables

        private readonly StringBuilder comment = new StringBuilder();
        private readonly StringBuilder name = new StringBuilder();
        private readonly TextReader textReader;
        private readonly StringBuilder value = new StringBuilder();
        private bool acceptCommentAfterKey = true;
        private char[] assignDelimiters = {'='};
        private int column = 1;
        private char[] commentDelimiters = {';'};
        private bool disposed;
        private bool hasComment;
        private IniType iniType = IniType.Empty;
        private int lineNumber = 1;
        private IniReadState readState = IniReadState.Initial;

        #endregion

        #region Public properties

        public string Name
        {
            get { return name.ToString(); }
        }

        public string Value
        {
            get { return value.ToString(); }
        }

        public IniType Type
        {
            get { return iniType; }
        }

        public string Comment
        {
            get { return (hasComment) ? comment.ToString() : null; }
        }

        public int LineNumber
        {
            get { return lineNumber; }
        }

        public int LinePosition
        {
            get { return column; }
        }

        public bool IgnoreComments { get; set; }

        public IniReadState ReadState
        {
            get { return readState; }
        }

        public bool LineContinuation { get; set; }

        public bool AcceptCommentAfterKey
        {
            get { return acceptCommentAfterKey; }
            set { acceptCommentAfterKey = value; }
        }

        public bool AcceptNoAssignmentOperator { get; set; }

        public bool ConsumeAllKeyText { get; set; }

        #endregion

        #region Constructors

        public IniReader(string filePath)
        {
            textReader = new StreamReader(filePath);
        }

        public IniReader(TextReader reader)
        {
            textReader = reader;
        }

        public IniReader(Stream stream)
            : this(new StreamReader(stream))
        {
        }

        #endregion

        #region Public methods

        public void Dispose()
        {
            Dispose(true);
        }

        public bool Read()
        {
            var result = false;

            if (readState != IniReadState.EndOfFile
                || readState != IniReadState.Closed)
            {
                readState = IniReadState.Interactive;
                result = ReadNext();
            }

            return result;
        }

        public bool MoveToNextSection()
        {
            bool result;

            while (true)
            {
                result = Read();

                if (iniType == IniType.Section || !result)
                {
                    break;
                }
            }

            return result;
        }

        public bool MoveToNextKey()
        {
            bool result;

            while (true)
            {
                result = Read();

                if (iniType == IniType.Section)
                {
                    result = false;
                    break;
                }
                if (iniType == IniType.Key || !result)
                {
                    break;
                }
            }

            return result;
        }

        public void Close()
        {
            Reset();
            readState = IniReadState.Closed;
        }

        public char[] GetCommentDelimiters()
        {
            var result = new char[commentDelimiters.Length];
            Array.Copy(commentDelimiters, 0, result, 0, commentDelimiters.Length);

            return result;
        }

        public void SetCommentDelimiters(char[] delimiters)
        {
            if (delimiters.Length < 1)
            {
                throw new ArgumentException("Must supply at least one delimiter");
            }

            commentDelimiters = delimiters;
        }

        public char[] GetAssignDelimiters()
        {
            var result = new char[assignDelimiters.Length];
            Array.Copy(assignDelimiters, 0, result, 0, assignDelimiters.Length);

            return result;
        }

        public void SetAssignDelimiters(char[] delimiters)
        {
            if (delimiters.Length < 1)
            {
                throw new ArgumentException("Must supply at least one delimiter");
            }

            assignDelimiters = delimiters;
        }

        #endregion

        #region Private methods

        ~IniReader()
        {
            Dispose(false);
        }

        private void Reset()
        {
            name.Remove(0, name.Length);
            value.Remove(0, value.Length);
            comment.Remove(0, comment.Length);
            iniType = IniType.Empty;
            hasComment = false;
        }

        private bool ReadNext()
        {
            var result = true;
            var ch = PeekChar();
            Reset();

            if (IsComment(ch))
            {
                iniType = IniType.Empty;
                ReadChar(); 
                ReadComment();

                return true;
            }

            switch (ch)
            {
                case ' ':
                case '\t':
                case '\r':
                    SkipWhitespace();
                    ReadNext();
                    break;
                case '\n':
                    ReadChar();
                    break;
                case '[':
                    ReadSection();
                    break;
                case -1:
                    readState = IniReadState.EndOfFile;
                    result = false;
                    break;
                default:
                    ReadKey();
                    break;
            }

            return result;
        }

        private void ReadComment()
        {
            int ch;
            SkipWhitespace();
            hasComment = true;

            do
            {
                ch = ReadChar();
                comment.Append((char) ch);
            } while (!EndOfLine(ch));

            RemoveTrailingWhitespace(comment);
        }

        private void RemoveTrailingWhitespace(StringBuilder builder)
        {
            var temp = builder.ToString();

            builder.Remove(0, builder.Length);
            builder.Append(temp.TrimEnd(null));
        }

        private void ReadKey()
        {
            iniType = IniType.Key;

            while (true)
            {
                var ch = PeekChar();

                if (IsAssign(ch))
                {
                    ReadChar();
                    break;
                }

                if (EndOfLine(ch))
                {
                    if (AcceptNoAssignmentOperator)
                    {
                        break;
                    }
                    throw new IniException(this,
                        string.Format("Expected assignment operator ({0})",
                            assignDelimiters[0]));
                }

                name.Append((char) ReadChar());
            }

            ReadKeyValue();
            SearchForComment();
            RemoveTrailingWhitespace(name);
        }

        private void ReadKeyValue()
        {
            var foundQuote = false;
            var characters = 0;
            SkipWhitespace();

            while (true)
            {
                var ch = PeekChar();

                if (!IsWhitespace(ch))
                {
                    characters++;
                }

                if (!ConsumeAllKeyText && ch == '"')
                {
                    ReadChar();

                    if (!foundQuote && characters == 1)
                    {
                        foundQuote = true;
                        continue;
                    }
                    break;
                }

                if (foundQuote && EndOfLine(ch))
                {
                    throw new IniException(this, "Expected closing quote (\")");
                }
                
                if (LineContinuation && ch == '\\')
                {
                    var buffer = new StringBuilder();
                    buffer.Append((char) ReadChar()); 

                    while (PeekChar() != '\n' && IsWhitespace(PeekChar()))
                    {
                        if (PeekChar() != '\r')
                        {
                            buffer.Append((char) ReadChar());
                        }
                        else
                        {
                            ReadChar(); 
                        }
                    }

                    if (PeekChar() == '\n')
                    {
                        ReadChar();
                        continue;
                    }
                    value.Append(buffer);
                }

                if (!ConsumeAllKeyText)
                {

                    if (acceptCommentAfterKey && IsComment(ch) && !foundQuote)
                    {
                        break;
                    }
                }

                if (EndOfLine(ch))
                {
                    break;
                }

                value.Append((char) ReadChar());
            }

            if (!foundQuote)
            {
                RemoveTrailingWhitespace(value);
            }
        }

        private void ReadSection()
        {
            iniType = IniType.Section;
            ReadChar();

            while (true)
            {
                var ch = PeekChar();
                if (ch == ']')
                {
                    break;
                }
                if (EndOfLine(ch))
                {
                    throw new IniException(this, "Expected section end (])");
                }

                name.Append((char) ReadChar());
            }

            ConsumeToEnd(); 	
            RemoveTrailingWhitespace(name);
        }

        private void SearchForComment()
        {
            var ch = ReadChar();

            while (!EndOfLine(ch))
            {
                if (IsComment(ch))
                {
                    if (IgnoreComments)
                    {
                        ConsumeToEnd();
                    }
                    else
                    {
                        ReadComment();
                    }
                    break;
                }
                ch = ReadChar();
            }
        }

        private void ConsumeToEnd()
        {
            int ch;

            do
            {
                ch = ReadChar();
            } while (!EndOfLine(ch));
        }

        private int ReadChar()
        {
            var result = textReader.Read();

            if (result == '\n')
            {
                lineNumber++;
                column = 1;
            }
            else
            {
                column++;
            }

            return result;
        }

        private int PeekChar()
        {
            return textReader.Peek();
        }

        private bool IsComment(int ch)
        {
            return HasCharacter(commentDelimiters, ch);
        }

        private bool IsAssign(int ch)
        {
            return HasCharacter(assignDelimiters, ch);
        }

        private bool HasCharacter(IEnumerable<char> characters, int ch)
        {
            return characters.Any(t => ch == t);
        }

        private bool IsWhitespace(int ch)
        {
            return ch == 0x20 || ch == 0x9 || ch == 0xD || ch == 0xA;
        }

        private void SkipWhitespace()
        {
            while (IsWhitespace(PeekChar()))
            {
                if (EndOfLine(PeekChar()))
                {
                    break;
                }

                ReadChar();
            }
        }

        private bool EndOfLine(int ch)
        {
            return (ch == '\n' || ch == -1);
        }

        #endregion
    }
}