using System.Collections;
namespace System.Lua.Serialization
{
    public static class TypeExtensions
    {
        public static bool SerializeAsEnumerable(this Type type)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            return !type.SerializeAsLiteral() && type.GetInterface("IEnumerable") != null;
        }

        public static bool SerializeAsArray(this Type type)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            return type.IsArray;
        }

        public static bool SerializeAsDictionary(this Type type)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            return !type.IsArray && (type.IsClass && type != typeof(string));
        }

        public static bool SerializeAsLiteral(this Type type)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            return type.IsPrimitive || type == typeof(string);
        }
    }
}
