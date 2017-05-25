//////////////////////////////////////////////////////////////////////////////////
//	SequencePlayer.cs
//	Light Sequencer
//	Written by Brian Peek (http://www.brianpeek.com/)
//	for the Animated Holiday Lights article
//		at Coding4Fun (http://msdn.microsoft.com/coding4fun/)
//////////////////////////////////////////////////////////////////////////////////

using System;

namespace LightSequencer
{
	public abstract class SequencePlayer
	{
		public abstract void Start();
		public abstract void Stop();
		public abstract void Load();
		public abstract void Unload();

		public event EventHandler SequenceStopped;

		protected virtual void OnSequenceStopped(EventArgs e)
		{
			if(SequenceStopped != null)
				SequenceStopped(this, e);
		}
	}
}
