-- Author      : Gabor_Ratky
-- Create Date : 8/18/2008 3:36:03 PM

FEED_READER_FEEDS = {}
CURRENT_FEED = {}
CURRENT_ITEM = {}

function FeedReaderFrame_OnLoad()
	this:RegisterEvent("ADDON_LOADED");
end

function FeedReaderFrame_OnEvent()
  if ((event == "ADDON_LOADED") and (arg1 == 'FeedReader')) then
	-- Select the first feed from the list of feeds
    SelectFeed(1);
  end
end

function SelectFeed(feedIndex)
	if (FEED_READER_FEEDS[feedIndex] ~= CURRENT_FEED) then
		CURRENT_FEED = FEED_READER_FEEDS[feedIndex];
		UpdateFeeds();

		SelectFeedItem(1);
	end
end

function SelectFeedItem(feedItemIndex)
	if (CURRENT_FEED.Items[feedItemIndex] ~= CURRENT_ITEM) then
		CURRENT_ITEM = CURRENT_FEED.Items[feedItemIndex];
		UpdateFeedItems();
		UpdateSummary();
	end
end

function UpdateFeeds()
   
   -- Update the scroll frame with the number of feeds
   FauxScrollFrame_Update(FeedsScrollFrame, #FEED_READER_FEEDS, 5, 16);

   local i;
   local feedIndex;

   for i = 1, 5 do
     feedIndex = i + FauxScrollFrame_GetOffset(FeedsScrollFrame);

     if (feedIndex <= #FEED_READER_FEEDS) then
       -- Shows the first 40 characters from text.
       getglobal("FeedButton" .. i):SetText( strsub(FEED_READER_FEEDS[feedIndex].Title, 0, 40) );
       getglobal("FeedButton" .. i):Show();
     else
       getglobal("FeedButton" .. i):Hide();
	 end

	 if (FEED_READER_FEEDS[feedIndex] == CURRENT_FEED) then
        getglobal("FeedButton" .. i):LockHighlight();
     else
        getglobal("FeedButton" .. i):UnlockHighlight();
     end
   end
end

function UpdateFeedItems()

   -- Update the scroll frame with the number of feed items.
   FauxScrollFrame_Update(FeedItemsScrollFrame, #(CURRENT_FEED.Items), 5, 16);

   local i;
   local feedItemIndex;

   for i = 1, 5 do
     feedItemIndex = i + FauxScrollFrame_GetOffset(FeedItemsScrollFrame);

     if (feedItemIndex <= #(CURRENT_FEED.Items)) then
       -- Shows the first 40 characters from text.
       getglobal("FeedItemButton" .. i):SetText( strsub(CURRENT_FEED.Items[feedItemIndex].Title, 0, 40) );
       getglobal("FeedItemButton" .. i):Show();
     else
       getglobal("FeedItemButton" .. i):Hide();
	 end
	
	 if (CURRENT_FEED.Items[feedItemIndex] == CURRENT_ITEM) then
        getglobal("FeedItemButton" .. i):LockHighlight();
     else
        getglobal("FeedItemButton" .. i):UnlockHighlight();
     end
   end
end

function UpdateSummary()
	SummaryFontString:SetText(CURRENT_ITEM.Content);
end

function FeedButton_Clicked(i)
	local feedIndex = i + FauxScrollFrame_GetOffset(FeedsScrollFrame);
	
	SelectFeed(feedIndex);
	
	FeedItemsScrollFrameScrollBar:SetValue(0);
end

function FeedItemButton_Clicked(i)
	local feedItemIndex = i + FauxScrollFrame_GetOffset(FeedItemsScrollFrame);
	
	SelectFeedItem(feedItemIndex);
end