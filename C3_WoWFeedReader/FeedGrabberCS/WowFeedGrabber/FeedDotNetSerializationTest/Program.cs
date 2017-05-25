using System.Linq;
using System.Lua;
using System.Lua.Serialization;

using FeedDotNet;

namespace FeedDotNetSerializationTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var rawFeeds = new[]
                           {
                               FeedReader.Read("http://newsrss.bbc.co.uk/rss/newsonline_uk_edition/front_page/rss.xml")
                           };

            var feeds = rawFeeds.Select(feed => new
                                                    {
                                                        feed.Title,
                                                        Items = feed.Items.Select(item => new
                                                                                              {
                                                                                                  item.Title,
                                                                                                  item.Content
                                                                                              }).ToArray()
                                                    }).ToArray();

            using (var luaWriter = LuaWriter.Create("SavedVariables.lua", new LuaWriterSettings{ Indent = true }))
            {
                var luaSerializer = new LuaSerializer();

                luaWriter.WriteStartAssignment("FEED_READER_FEEDS");
                luaSerializer.Serialize(luaWriter, feeds);
                luaWriter.WriteEndAssignment();
            }
        }
    }
}
