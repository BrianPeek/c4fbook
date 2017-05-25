using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Amib.Threading;
using System.IO;


namespace SharedUtilities
{
    public enum WorkType
    {
        UpdateFeeds, //just Update YouTube API feeds
        Download, //just download FLV files
        DownloadAndConvert, //download and convert FLV files
        Convert, //just convert FLV files
        All //do all of the above
    }


    public class InnerTubeFeedWorker : BackgroundWorker 
    {
        #region Members
        private static InnerTubeFeedWorker worker;

        //Video Collections
        ObservableCollection<InnerTubeFeed> feedList;
        List<InnerTubeVideo> allVideos;
        
        //File Path
        Setting settings;
        
        //ThreadPools
        private SmartThreadPool feedPool;
        private SmartThreadPool downloadPool;
        private SmartThreadPool convertPool;
        
        //ThreadPool results
        List<IWorkItemResult> results = new List<IWorkItemResult>();
        #endregion

        public static bool IsRunningAsync()
        {
            if (worker == null)
            {
                return false;
            }
            else
            {
                return worker.IsBusy;
            }
        }

        public static InnerTubeFeedWorker GetInstance(ObservableCollection<InnerTubeFeed> feedList, Setting settings)
        {
            //first time creating it
            if (worker == null)
            {
                worker = new InnerTubeFeedWorker(feedList, settings);
                return worker;
            }
            else
            {
                //async operation happenning return async worker
                if (worker.IsBusy)
                {
                    return worker;
                }
                else
                {
                    //no async operation, reset worker
                    worker = new InnerTubeFeedWorker(feedList, settings);
                    return worker;
                }
            }          
        }
 
        private static void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            worker.Dispose();
        }

        private InnerTubeFeedWorker(ObservableCollection<InnerTubeFeed> feedList, Setting settings) 
        {
            WorkerReportsProgress = true;
            WorkerSupportsCancellation = true;

            this.feedList = feedList.ToObservableCollection();
            this.settings = settings;
            this.RunWorkerCompleted += new RunWorkerCompletedEventHandler(worker_RunWorkerCompleted);
        }

        protected override void OnDoWork(DoWorkEventArgs e)
        {
            WorkType work = (WorkType)e.Argument;

            //Print that work starting
            WriteWorkerStartedMessages(work);

            //Check if we should just update feeds
            if (work == WorkType.UpdateFeeds)
            {
                //Start Feed Update Work
                RunUpdateFeedPool();                
            }
            else
            {
                //otherwise only All should be updating feeds
                if (work == WorkType.All)
                {
                    RunUpdateFeedPool();
                }
                
                allVideos = Utilities.Flatten(feedList);

                //Start Downloading Work
                RunDownloadPool(work);

                //Once downloads are complete, check if we need to do conversion work
                if (work == WorkType.Convert || work == WorkType.DownloadAndConvert || work == WorkType.All)
                {
                    RunConversionPool();
                }
                
                //Once conversions are complete, check for sync work
                if (work == WorkType.All)
                {
                    WriteStatus("Starting sync");
                    SyncToZuneAndiTunes();
                    WriteStatus("Sync complete");
                }

            }

                    WriteStatus("All Items Updated");

                    //Update Items to ensure valid links
                    e.Result = ValidateFeed.UpdateLinks(feedList, settings); ;
                }

        private void SyncToZuneAndiTunes()
        {
            if (settings.iTunesInstalled)
            {
                iTunesSync iTunes = new iTunesSync();
                iTunes.Sync(settings.VideoPath);
            }
            if (settings.ZuneInstalled)
            {
                ZuneSync zune = new ZuneSync();
                zune.Sync(settings.VideoPath);
            }
        }

        #region Pools

        private void RunUpdateFeedPool()
        {
            results.Clear();
            feedPool = SetupSmartThreadPool(settings.UpdateFeedPoolThreads);
            
            //Add Items
            addFeedsToQueue();
            feedPool.Start();

            SmartThreadPool.WaitAll(results.ToArray());

            feedList.Clear();
            foreach (var r in results)
            {
                feedList.Add((InnerTubeFeed)r.Result);
            }
            feedPool.Shutdown();

            WriteStatus("Update Feeds complete");
        }
        
        private void RunDownloadPool(WorkType work)
        {
            //Setup downloadPool
            downloadPool = SetupSmartThreadPool(settings.DownloadPoolThreads);

            //Select the items to add to the queue
            addDownloadTasksToQueue(work);
            downloadPool.Start();

            bool success = SmartThreadPool.WaitAll(results.ToArray());
            downloadPool.Shutdown();

            WriteStatus("Download Pool Completed, succeeded = " + success.ToString());
        }

        private void RunConversionPool()
        {
            results.Clear();
            convertPool = SetupSmartThreadPool(settings.ConversionPoolThreads);
            addConversionsToQueue();
            convertPool.Start();
            bool converted = SmartThreadPool.WaitAll(results.ToArray());
            
            convertPool.Shutdown();

            WriteStatus("conversion Pool completed, success = " + converted.ToString());

        }
       
        #endregion

        #region Queue Work Items

        private void addFeedsToQueue()
        {
            foreach (var feed in this.feedList)
            {                
                IWorkItemResult item = feedPool.QueueWorkItem(UpdateFeeds, feed);
                results.Add(item);
            }
        }

        private void addDownloadTasksToQueue(WorkType work)
        {
            switch (work)
            {
                case WorkType.UpdateFeeds:
                    addImagesToQueue();
                    break;
                case WorkType.Download:
                    addVideosToQueue();
                    break;
                case WorkType.DownloadAndConvert:
                    addVideosToQueue();
                    break;
                case WorkType.All:
                    addImagesToQueue();
                    addVideosToQueue();
                    break;
                default:
                    break;
            }
        }

        private void addImagesToQueue()
        {
            
            //Queue image tasks
            foreach (var vid in this.allVideos)
            {
                if (!File.Exists(vid.DownloadedImage))
                {
                    IWorkItemResult item = downloadPool.QueueWorkItem(downloadImages, vid);
                    results.Add(item);
                }

            }
        }

        private void addVideosToQueue()
        {            
            //////////Queue Downloading tasks
            ////////foreach (var feed in this.feedList)
            ////////{
                foreach (var vid in this.allVideos)
                {
                    //queue if converted files don't exist
                    if (!FileHelper.ConvertedFilesExist(vid, this.settings))
                    {
                        IWorkItemResult item;
                        item = downloadPool.QueueWorkItem(downloadVideo, vid);
                        results.Add(item);
                    }
                }
            
        }

        private void addConversionsToQueue()
        {
            //loop through each video and add the downloadedFlv fullPath to the convertvideo task
            foreach (var feed in this.feedList)
            {
                foreach (var vid in feed.FeedVideos)
                {
                    //only add if video(s) don't already exist
                    if (!FileHelper.ConvertedFilesExist(vid,this.settings))                        
                    {
                        IWorkItemResult item = convertPool.QueueWorkItem(convertVideo, vid);
                        results.Add(item);                        
                    }
                }
            }
        }

        #endregion

        #region Async Tasks
        private object UpdateFeeds(object state)
        {
            checkForCancellation();

            InnerTubeFeed feed = ((InnerTubeFeed)state).Clone();
            ObservableCollection<InnerTubeVideo> newVideos = InnerTubeService.ConvertYouTubeXmlToObjects(new Uri(feed.FeedUrl),this.settings);

            //pull just videos we don't have yet
            var newVids = from a in newVideos
                          from b in
                              (from n in newVideos
                               select n.Id).Except(from o in feed.FeedVideos
                                                   select o.Id)
                          where a.Id == b
                          select a;

            foreach (var n in newVids)
            {
                feed.FeedVideos.Add(n);
            }

            string status = "updated feed: " + feed.FeedName;
            WriteStatus(status);
            return feed;
        }

        private object downloadImages(object state)
        {
            checkForCancellation();

            //Convert to Feed Type
            InnerTubeVideo vid = (InnerTubeVideo)state;

            Download.DownloadImage(vid);

            
            string status; 
            if (File.Exists(vid.DownloadedImage))
	        {
                status = String.Format("Image for Video: {0} SUCCESSFULLY downloaded to {1}",vid.Title, vid.DownloadedImage);
	        }
            else
            {
                status = String.Format("Image for Video: {0} FAILED to download to {1}", vid.Title, vid.DownloadedImage);
            }
            
            
            WriteStatus(status);
            return status;
        }

        private object downloadVideo(object state)
        {
            //parse values    
            InnerTubeVideo vid = (InnerTubeVideo)state;

            WriteStatus("Starting download for: " + vid.Title);

            //Call download task, may take a few minutes to execute            
            Download.DownloadVideo(vid);

            WriteStatus("video downloaded to fullPath: " + vid.DownloadedFlv);
            return vid.DownloadedFlv;            
        }

        private object convertVideo(object state)
        {
            checkForCancellation();
            InnerTubeVideo video = (InnerTubeVideo)state;

            WriteStatus("Starting conversion for file: " + video.Title);

            VideoConverter.ConvertFlv(video, ConversionType.Wmv);
            
            string status; 

            //only convert to MP4 if they've said to add to iTunes
            if (settings.iTunesInstalled)
            {
                VideoConverter.ConvertFlv(video, ConversionType.Mp4);            
                
                if (File.Exists(video.DownloadedMp4) && (FileHelper.FileSize(video.DownloadedMp4) > 0) 
                    && File.Exists(video.DownloadedWmv)  && (FileHelper.FileSize(video.DownloadedWmv) > 0))
                {
                    //Delete Flv

                    if (video.DownloadedFlv != null)
                    {
                        File.Delete(video.DownloadedFlv);
                    }
                    
                    status = String.Format("Conversion COMPLETE for: {0} at: {1} and {2}", video.Title, video.DownloadedMp4, video.DownloadedWmv);
                }
                else
                {
                    status = String.Format("Conversion FAILED for: {0} at: {1} and {2}", video.Title, video.DownloadedMp4, video.DownloadedWmv);
                }

            }
            else
            {
                if (File.Exists(video.DownloadedWmv) && (FileHelper.FileSize(video.DownloadedWmv) > 0))
                {
                    //WMV created, delete the FLV
                    File.Delete(video.DownloadedFlv);
                    status = String.Format("Conversion COMPLETE for: {0} at: {1}", video.Title, video.DownloadedWmv);
                }
                else
                {
                    //WMV not created
                    status = String.Format("Conversion FAILED for: {0} at: {1}", video.Title, video.DownloadedWmv);
                }
            }

            WriteStatus(status);
            return status;
            

        }

        #endregion        

        #region Helper Methods

        private SmartThreadPool SetupSmartThreadPool(int threads)
        {
            STPStartInfo startInfo = new STPStartInfo();
            startInfo.StartSuspended = true;
            startInfo.MaxWorkerThreads = threads;
            startInfo.ThreadPriority = System.Threading.ThreadPriority.Normal;
            return new SmartThreadPool(startInfo);

        }

        private void checkForCancellation()
        {
            if (this.CancellationPending  || !Utilities.IsNetworkAvailable())
            {
                downloadPool.Cancel();
                downloadPool.Shutdown(true, new TimeSpan(0, 5, 0));
            }
        }
        
        private void WriteStatus(string status)
        {
            Debug.WriteLine(status + Environment.NewLine);
            ReportProgress(50, status + Environment.NewLine);
        }

        private void WriteWorkerStartedMessages(WorkType t)
        {
            WriteStatus("BackgroundWorker Started with worktype: " + t.ToString());

            WriteStatus("Total Feeds: " + this.feedList.Count);
            foreach (var feed in this.feedList)
            {
                if (feed.FeedVideos == null)
                {
                    WriteStatus("Feed: " + feed.FeedName + " has zero videos");
                }
                else
                {
                    WriteStatus("Feed: " + feed.FeedName + " has " + feed.FeedVideos.Count + " video(s)");
                }
                
            }
        }
        #endregion
    }
}
