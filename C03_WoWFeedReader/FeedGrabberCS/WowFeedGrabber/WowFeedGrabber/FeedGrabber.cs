using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading;
using System.Lua;
using System.Lua.Serialization;
using Microsoft.Feeds.Interop;

namespace WowFeedGrabber
{
    public class FeedGrabber
    {
        private const string feedsVariableName = "FEED_READER_FEEDS";

        private readonly string savedVariablesPath;
        private FileSystemWatcher fileSystemWatcher;
        private IFeedsManager feedsManager;

        public FeedGrabber(string savedVariablesPath)
        {
            if (savedVariablesPath == null)
                throw new ArgumentNullException("savedVariablesPath");

            this.savedVariablesPath = savedVariablesPath;

            InitializeFileSystemWatcher();
            InitializeFeedsManager();
        }

        private void InitializeFileSystemWatcher()
        {
            fileSystemWatcher = new FileSystemWatcher(Path.GetDirectoryName(savedVariablesPath),
                                                      Path.GetFileName(savedVariablesPath));
            fileSystemWatcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.FileName;
            fileSystemWatcher.Changed += OnFileChangedOrDeleted;
            fileSystemWatcher.Deleted += OnFileChangedOrDeleted;
            fileSystemWatcher.EnableRaisingEvents = true;
        }

        private void InitializeFeedsManager()
        {
            feedsManager = new FeedsManagerClass();
        }

        private void OnFileChangedOrDeleted(object sender, FileSystemEventArgs e)
        {
            // Suspend change events
            fileSystemWatcher.EnableRaisingEvents = false;

            bool success = false;

            do
            {
                try
                {
                    this.SaveFeeds();
                    success = true;
                }
                catch (IOException)
                {
                    Thread.Sleep(5);
                }
            } while (!success);

            // Resume change events
            fileSystemWatcher.EnableRaisingEvents = true;
        }

        public void EnableBackgroundUpdates()
        {
            feedsManager.BackgroundSync(FEEDS_BACKGROUNDSYNC_ACTION.FBSA_ENABLE);
        }

        public void SaveFeeds()
        {
            Feed[] feeds = this.GetTopLevelFeeds();
            if (feeds != null)
            {
                using (var luaWriter = LuaWriter.Create(savedVariablesPath))
                {
                    var luaSerializer = new LuaSerializer();

                    luaWriter.WriteStartAssignment(feedsVariableName);
                    luaSerializer.Serialize(luaWriter, feeds);
                    luaWriter.WriteEndAssignment();
                }
            }
        }

        private Feed[] GetTopLevelFeeds()
        {
            var rootFolder = (IFeedFolder) feedsManager.RootFolder;
            var topLevelFeeds = (IFeedsEnum) rootFolder.Feeds;

            var feeds = new List<Feed>(topLevelFeeds.Count);

            foreach (IFeed feed in topLevelFeeds)
            {
                // If a feed is not yet downloaded, download now
                if (feed.DownloadStatus != FEEDS_DOWNLOAD_STATUS.FDS_DOWNLOADED)
                    feed.Download();

                // Skip feed if it could not be downloaded
                if (feed.DownloadStatus != FEEDS_DOWNLOAD_STATUS.FDS_DOWNLOADED)
                    continue;

                feeds.Add(CreateFeed(feed));
            }

            return feeds.ToArray();
        }

        private static Feed CreateFeed(IFeed feed)
        {
            var feedItems = (IFeedsEnum) feed.Items;
            return new Feed
                       {
                           Title = feed.Title,
                           Items = feedItems.OfType<IFeedItem>().Select(feedItem => CreateFeedItem(feedItem)).ToArray()
                       };
        }

        private static FeedItem CreateFeedItem(IFeedItem feedItem)
        {
            return new FeedItem
                       {
                           Title = HtmlConvert.ToPlainText(feedItem.Title),
                           Content = HtmlConvert.ToPlainText(feedItem.Description)
                       };
        }
    }
}