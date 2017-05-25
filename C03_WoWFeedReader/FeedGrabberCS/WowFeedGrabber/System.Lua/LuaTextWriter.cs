using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace System.Lua
{
    public class LuaTextWriter : LuaWriter
    {
        private readonly LuaWriterSettings settings = new LuaWriterSettings();
        private readonly TextWriter textWriter;

        private int indent;

        /// <summary>
        /// Creates an instance of the <see cref="LuaTextWriter"/> class using the specified <see cref="TextWriter"/>.
        /// </summary>
        /// <param name="textWriter">The <see cref="TextWriter"/> to write to.</param>
        public LuaTextWriter(TextWriter textWriter)
        {
            if (textWriter == null)
                throw new ArgumentNullException("textWriter");

            this.textWriter = textWriter;
        }

        /// <summary>
        /// Creates an instance of the <see cref="LuaTextWriter"/> class using the specified stream and encoding.
        /// </summary>
        /// <param name="stream">The stream to write.</param>
        /// <param name="encoding">The encoding to generate. If <c>null</c>, UTF-8 encoding is used.</param>
        public LuaTextWriter(Stream stream, Encoding encoding)
        {
            if (stream == null)
                throw new ArgumentNullException("stream");

            if (encoding != null)
            {
                textWriter = new StreamWriter(stream, encoding);
            }
            else
            {
                textWriter = new StreamWriter(stream);
            }
        }

        /// <summary>
        /// Creates an instance of the <see cref="LuaTextWriter"/> class using the specified file.
        /// </summary>
        /// <param name="filename">The filename to write to. If the file exists, it truncates it and overwrites it with the new content. </param>
        /// <param name="encoding">The encoding to generate. If <c>null</c>, UTF-8 encoding is used.</param>
        public LuaTextWriter(string filename, Encoding encoding)
            : this(filename, encoding, new LuaWriterSettings())
        {
        }

        public LuaTextWriter(string filename, Encoding encoding, LuaWriterSettings settings)
            : this(new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.Read), encoding)
        {
            if (settings == null)
                throw new ArgumentNullException("settings");

            this.settings = settings;
        }

        public override void WriteStartAssignment(bool local, params string[] variableNames)
        {
            if (variableNames == null)
                throw new ArgumentNullException("variableNames");

            if (local)
                textWriter.Write("local ");

            for (int i = 0; i < variableNames.Length; i++)
            {
                if (i != 0)
                    textWriter.Write(", ");

                textWriter.Write(variableNames[i]);
            }

            textWriter.Write(" = ");
        }

        public override void WriteLiteralExpression(object value)
        {
            if (value == null)
            {
                value = "nil";
            }
            else if (value is string)
            {
                string text = (String)value;

                // Depending on whether the text value contains new line, use long bracket format
                if (text.Contains("\n") || text.Contains("\r") || text.Contains("\r\n") || text.Contains("\""))
                {
                    value = String.Format("[[{0}]]", text);
                }
                else
                {
                    value = String.Format("\"{0}\"", text);
                }
            }
            else if (value is bool)
            {
                value = value.ToString().ToLower();
            }

            textWriter.Write(value);
        }

        public override void WriteExpressionListDelimiter()
        {
            textWriter.Write(", ");
        }

        public override void WriteLiteralExpressionList(params object[] values)
        {
            if (values == null)
                throw new ArgumentNullException("values");

            for (int i = 0; i < values.Length; i++)
            {
                if (i != 0)
                    this.WriteExpressionListDelimiter();

                this.WriteLiteralExpression(values[i]);
            }
        }

        public override void WriteEndAssignment()
        {
            textWriter.WriteLine();
        }

        public override void WriteStartTable()
        {
            if (settings.TableOnNewLine)
            {
                this.Indent();
                textWriter.Write(Environment.NewLine);
                this.WriteIndent();
            }

            textWriter.Write("{ ");
        }


        public override void WriteStartTableField(string name, bool evaluateAsExpression)
        {
            if (name != null && name.Length == 0)
                throw new ArgumentException("Zero length field names are not allowed. Use null for unnamed fields.");
            if (evaluateAsExpression && String.IsNullOrEmpty(name))
                throw new ArgumentException("Cannot evaluate null or empty name as expression.");

            if (settings.TableFieldOnNewLine)
            {
                this.Indent();
                textWriter.Write(Environment.NewLine);
                this.WriteIndent();
            }

            if (name != null)
            {
                if (evaluateAsExpression)
                    textWriter.Write("[");

                textWriter.Write(name);

                if (evaluateAsExpression)
                    textWriter.Write("]");

                textWriter.Write(" = ");
            }
        }

        public override void WriteEndTableField()
        {
            textWriter.Write(", ");

            if (settings.TableFieldOnNewLine)
                this.Unindent();
        }

        public override void WriteEndTable()
        {
            if (settings.EndTableOnNewLine)
            {
                textWriter.Write(Environment.NewLine);
                this.WriteIndent();
                textWriter.Write("}");
            }

            if (settings.TableOnNewLine)
                this.Unindent();
        }

        /// <summary>
        /// Flushes whatever is in the buffer to the underlying streams and also flushes the underlying stream.
        /// </summary>
        public override void Flush()
        {
            textWriter.Flush();
        }

        /// <summary>
        /// Closes the stream and the underlying stream used by the <see cref="LuaWriter"/>.
        /// </summary>
        public override void Close()
        {
            textWriter.Close();
        }

        private void Indent()
        {
            indent++;
        }

        private void Unindent()
        {
            indent--;
        }

        private void WriteIndent()
        {
            if (settings.Indent)
            {
                for (int i = 0; i < indent; i++)
                    textWriter.Write(settings.IndentChars);
            }
        }
    }
}