using System.Reflection;
using System.Collections;
namespace System.Lua.Serialization
{
    public class LuaSerializer
    {
        public void Serialize(LuaWriter luaWriter, object o)
        {
            if (luaWriter == null)
                throw new ArgumentNullException("luaWriter");

            this.SerializeImpl(luaWriter, o);
        }

        private void SerializeImpl(LuaWriter luaWriter, object o)
        {
            // If object is null, serialize as literal (to 'nil')
            if (o == null)
                this.SerializeLiteral(luaWriter, o);
            else
            {
                // Get the type of the object
                Type type = o.GetType();

                if (type.SerializeAsEnumerable())
                    this.SerializeEnumerable(luaWriter, (IEnumerable)o);
                if (type.SerializeAsDictionary())
                    this.SerializeDictionary(luaWriter, o);
                if (type.SerializeAsLiteral())
                    this.SerializeLiteral(luaWriter, o);
            }
        }

        private void SerializeEnumerable(LuaWriter luaWriter, IEnumerable collection)
        {
            luaWriter.WriteStartTable();

            foreach (var item in collection)
            {
                luaWriter.WriteStartTableField();
                this.SerializeImpl(luaWriter, item);
                luaWriter.WriteEndTableField();
            }

            luaWriter.WriteEndTable();
        }

        private void SerializeDictionary(LuaWriter luaWriter, object o)
        {
            Type type = o.GetType();

            luaWriter.WriteStartTable();

            foreach (var propertyInfo in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                luaWriter.WriteStartTableField(propertyInfo.Name, false);
                this.SerializeImpl(luaWriter, propertyInfo.GetValue(o, null));
                luaWriter.WriteEndTableField();
            }

            luaWriter.WriteEndTable();
        }

        private void SerializeLiteral(LuaWriter luaWriter, object value)
        {
            luaWriter.WriteLiteralExpression(value);
        }
    }
}
