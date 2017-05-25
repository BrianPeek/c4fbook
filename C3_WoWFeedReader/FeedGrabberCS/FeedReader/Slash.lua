function FeedReader_OnCommand(arg)
	if arg == "show" then
		FeedReaderFrame:Show();
	end
	
	if arg == "hide" then
		FeedReaderFrame:Hide();
	end
end

SLASH_FEEDREADER1 = "/feeds";
SlashCmdList["FEEDREADER"] = FeedReader_OnCommand;
