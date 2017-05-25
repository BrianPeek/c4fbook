//////////////////////////////////////////////////////////////////////////////////
//	MCIPlayback.cs
//	Light Sequencer
//	Written by Brian Peek (http://www.brianpeek.com/)
//	for the Animated Holiday Lights article
//		at Coding4Fun (http://msdn.microsoft.com/coding4fun/)
//////////////////////////////////////////////////////////////////////////////////

using System;
using System.Runtime.InteropServices;
using System.Text;

namespace LightSequencer
{
	public class MCIPlayback : IPlayback
	{
		// dll export to MCI for playing music
		[DllImport("winmm.dll")]
		static extern Int32 mciSendString(String command, StringBuilder buffer, Int32 bufferSize, IntPtr hwndCallback);

		public void Load(Sequence seq)
		{
			string cmd = "open \"" + seq.MusicFile + "\" type mpegvideo alias MediaFile";
			mciSendString(cmd, null, 0, IntPtr.Zero);
		}

		public void Unload()
		{
			string cmd = "close MediaFile";
			mciSendString(cmd, null, 0, IntPtr.Zero);
		}

		public void Start()
		{
			string cmd = "play MediaFile from 0";
			mciSendString(cmd, null, 0, IntPtr.Zero);
		}

		public void Stop()
		{
			string cmd = "stop MediaFile";
			mciSendString(cmd, null, 0, IntPtr.Zero);
		}
	}
}
