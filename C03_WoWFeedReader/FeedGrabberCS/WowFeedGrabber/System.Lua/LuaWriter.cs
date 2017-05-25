using System.IO;
using System.Text;
using System.Globalization;

namespace System.Lua
{
    public abstract class LuaWriter : IDisposable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LuaWriter"/> class.
        /// </summary>
        protected LuaWriter()
        {
        }

        public abstract void WriteStartAssignment(bool local, params string[] variableNames);

        public void WriteStartAssignment(params string[] variableNames)
        {
            this.WriteStartAssignment(false, variableNames);
        }

        public abstract void WriteLiteralExpression(object value);

        public abstract void WriteExpressionListDelimiter();

        public abstract void WriteLiteralExpressionList(params object[] values);

        public abstract void WriteEndAssignment();

        public abstract void WriteStartTable();

        public abstract void WriteStartTableField(string name, bool evaluateAsExpression);

        public void WriteStartTableField(string name)
        {
            this.WriteStartTableField(name, false);
        }

        public void WriteStartTableField()
        {
            this.WriteStartTableField(null, false);
        }

        public void WriteTableFieldLiteralExpression(string name, bool evaluateAsExpression, object value)
        {
            this.WriteStartTableField(name, evaluateAsExpression);
            this.WriteLiteralExpression(value);
            this.WriteEndTableField();
        }

        public void WriteTableFieldLiteralExpression(string name, object value)
        {
            this.WriteStartTableField(name);
            this.WriteLiteralExpression(value);
            this.WriteEndTableField();
        }

        public void WriteTableFieldLiteralExpression(object value)
        {
            this.WriteStartTableField();
            this.WriteLiteralExpression(value);
            this.WriteEndTableField();
        }

        public abstract void WriteEndTableField();

        public abstract void WriteEndTable();

        /// <summary>
        /// When overridden in a derived class, flushes whatever is in the buffer to the underlying streams and also flushes the underlying stream.
        /// </summary>
        public abstract void Flush();

        /// <summary>
        /// When overridden in a derived class, closes the stream and the underlying stream used by the <see cref="LuaWriter"/>.
        /// </summary>
        public abstract void Close();

        /// <summary>
        /// Releases the unmanaged resources used by the <see cref="LuaWriter"/> and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            try
            {
                this.Close();
            }
            catch
            {
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        void IDisposable.Dispose()
        {
            this.Dispose(true);
        }

        /// <summary>
        /// Creates a new <see cref="LuaWriter"/> instance using the specified stream.
        /// </summary>
        /// <param name="output">The stream to write to.</param>
        /// <returns>A <see cref="LuaWriter"/> object.</returns>
        public static LuaWriter Create(Stream output)
        {
            if (output == null)
                throw new ArgumentNullException("output");

            return new LuaTextWriter(output, null);
        }

        /// <summary>
        /// Creates a new <see cref="LuaWriter"/> instance using the specified <see cref="TextWriter"/>.
        /// </summary>
        /// <param name="output">The <see cref="TextWriter"/> to write to.</param>
        /// <returns>A <see cref="LuaWriter"/> object.</returns>        
        public static LuaWriter Create(TextWriter output)
        {
            if (output == null)
                throw new ArgumentNullException("output");

            return new LuaTextWriter(output);
        }

        /// <summary>
        /// Creates a new <see cref="LuaWriter"/> instance using the specified <see cref="TextWriter"/>.
        /// </summary>
        /// <param name="output">The <see cref="StringBuilder"/> to write to. Content written by the <see cref="LuaWriter"/> is appended to the <see cref="StringBuilder"/>.</param>
        /// <returns>A <see cref="LuaWriter"/> object.</returns>      
        public static LuaWriter Create(StringBuilder output)
        {
            if (output == null)
                throw new ArgumentNullException("output");

            return new LuaTextWriter(new StringWriter(output, CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// Creates a new <see cref="LuaWriter"/> instance using the specified filename.
        /// </summary>
        /// <param name="outputFilename">The <see cref="TextWriter"/> to write to.</param>
        /// <returns>A <see cref="LuaWriter"/> object.</returns>      
        public static LuaWriter Create(string outputFilename)
        {
            if (outputFilename == null)
                throw new ArgumentNullException("outputFilename");

            return new LuaTextWriter(outputFilename, null);
        }

        public static LuaWriter Create(string outputFilename, LuaWriterSettings settings)
        {
            if (outputFilename == null)
                throw new ArgumentNullException("outputFilename");
            if (settings == null)
                throw new ArgumentNullException("settings");

            return new LuaTextWriter(outputFilename, null, settings);
        }
    }
}
