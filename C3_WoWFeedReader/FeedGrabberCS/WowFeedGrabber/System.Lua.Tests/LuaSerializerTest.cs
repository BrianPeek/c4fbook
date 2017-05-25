using NUnit.Framework;
using System.Text;
using System.Lua.Serialization;
using System.Diagnostics;

namespace System.Lua.Tests
{
    [TestFixture]
    public class LuaSerializerTest
    {
        private StringBuilder stringBuilder;
        private LuaWriter luaWriter;
        private LuaSerializer luaSerializer;

        [SetUp]
        public void SetUp()
        {
            stringBuilder = new StringBuilder();
            luaWriter = LuaWriter.Create(stringBuilder);

            luaSerializer = new LuaSerializer();
        }

        [TearDown]
        public void TearDown()
        {
            Trace.WriteLine(stringBuilder);

            if (luaWriter != null)
                luaWriter.Close();
        }

        [Test]
        public void SerializePrimitive()
        {
            luaSerializer.Serialize(luaWriter, "Hello world from Lua!");
        }

        [Test]
        public void SerializeClass()
        {
            var audiR8 = new Car { Manufacturer = "Audi", Model = "R8", Horsepower = 420 };

            luaSerializer.Serialize(luaWriter, audiR8);
        }

        [Test]
        public void SerializeArray()
        {
            var numbers = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

            luaSerializer.Serialize(luaWriter, numbers);
        }
    }
}
