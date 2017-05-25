namespace System.Lua 
{
    public class LuaWriterSettings 
    {
        private const string defaultIndentChars = "    ";

        public LuaWriterSettings()
        {
            this.TableOnNewLine = false;
            this.TableFieldOnNewLine = true;
            this.EndTableOnNewLine = true;

            this.Indent = true;
            this.IndentChars = defaultIndentChars;
            this.NewLineChars = Environment.NewLine;

            this.ValidateContent = true;
        }

        public bool Indent { get; set; }

        public string IndentChars { get; set; }

        public string NewLineChars { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to start tables on a new line.
        /// </summary>
        public bool TableOnNewLine { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether a table field should be on a new line.
        /// </summary>
        public bool TableFieldOnNewLine { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to close tables on a new line.
        /// </summary>
        public bool EndTableOnNewLine { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether content should be validated and only valid Lua
        /// code emitted.
        /// </summary>
        public bool ValidateContent { get; set; }
    }
}
