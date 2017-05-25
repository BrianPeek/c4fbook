using NUnit.Framework;
using System.Text;
using System.Diagnostics;

namespace System.Lua.Tests
{
    [TestFixture]
    public class LuaWriterTest
    {
        private StringBuilder stringBuilder;
        private LuaWriter luaWriter;

        [SetUp]
        public void SetUp()
        {
            stringBuilder = new StringBuilder();
            luaWriter = LuaWriter.Create(stringBuilder);
        }

        [TearDown]
        public void TearDown()
        {
            Trace.WriteLine(stringBuilder);

            if (luaWriter != null)
                luaWriter.Close();
        }

        [Test]
        public void WriteGlobalVariableAssignmentTest()
        {
            luaWriter.WriteStartAssignment("i");
            luaWriter.WriteLiteralExpression(0);
            luaWriter.WriteEndAssignment();
        }

        [Test]
        public void WriteGlobalVariableListAssignmentTest()
        {
            luaWriter.WriteStartAssignment("i", "j");
            luaWriter.WriteLiteralExpressionList(0, 10);
            luaWriter.WriteEndAssignment();
        }

        [Test]
        public void WriteLocalVariableAssignmentTest()
        {
            luaWriter.WriteStartAssignment(true, "i");
            luaWriter.WriteLiteralExpression(0);
            luaWriter.WriteEndAssignment();
        }

        [Test]
        public void WriteTableAsArrayTest()
        {
            luaWriter.WriteStartAssignment("numbers");
            luaWriter.WriteStartTable();

            for (int i = 0; i < 10; i++)
            {
                luaWriter.WriteStartTableField();
                luaWriter.WriteLiteralExpression(i);
                luaWriter.WriteEndTableField();
            }

            luaWriter.WriteEndTable();
            luaWriter.WriteEndAssignment();
        }

        [Test]
        public void WriteTableAsDictionaryTest()
        {
            luaWriter.WriteStartAssignment("profile");
            luaWriter.WriteStartTable();

            luaWriter.WriteTableFieldLiteralExpression("Name", "Jessica Marie Alba");
            luaWriter.WriteTableFieldLiteralExpression("Age", 27);
            luaWriter.WriteTableFieldLiteralExpression("Hot", true);

            luaWriter.WriteEndTable();
            luaWriter.WriteEndAssignment();
        }

        [Test]
        public void WriteTableWithNestedTableTest()
        {
            luaWriter.WriteStartAssignment("movie");
            luaWriter.WriteStartTable();

            luaWriter.WriteTableFieldLiteralExpression("Name", "Lost in Translation");

            luaWriter.WriteStartTableField("Starring");
            
            luaWriter.WriteStartTable();
            luaWriter.WriteTableFieldLiteralExpression("Bill Murray");
            luaWriter.WriteTableFieldLiteralExpression("Scarlett Johansson");
            luaWriter.WriteEndTable();

            luaWriter.WriteEndTableField();

            luaWriter.WriteEndTable();
        }
    }
}
