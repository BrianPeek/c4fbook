//////////////////////////////////////////////////////////////////////////////////
//	MCIHelper.cs
//	Light Sequencer
//  Some quick methods to play music files via the MCI command set
//	Written by Brian Peek (http://www.brianpeek.com/)
//	for the Animated Holiday Lights article
//		at Coding4Fun (http://msdn.microsoft.com/coding4fun/)
//////////////////////////////////////////////////////////////////////////////////

using System;
using System.Text;
using System.Runtime.InteropServices;

namespace LightSequencer
{
	public static class MCIHelper
	{
		// dll export to MCI for playing music
		[DllImport("winmm.dll")]
		static extern Int32 mciSendString(String command, StringBuilder buffer, Int32 bufferSize, IntPtr hwndCallback);

		public static void OpenSong(string file)
		{
			string cmd = "open \"" + file + "\" type mpegvideo alias MediaFile";
			mciSendString(cmd, null, 0, IntPtr.Zero);
		}

		public static void PlaySong()
		{
			string cmd = "play MediaFile from 0";
			mciSendString(cmd, null, 0, IntPtr.Zero);
		}

		public static void StopSong()
		{
			string cmd = "stop MediaFile";
			mciSendString(cmd, null, 0, IntPtr.Zero);
		}

		public static void CloseSong()
		{
			string cmd = "close MediaFile";
			mciSendString(cmd, null, 0, IntPtr.Zero);
		}
	}
}
