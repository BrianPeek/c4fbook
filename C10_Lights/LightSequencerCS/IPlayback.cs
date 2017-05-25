//////////////////////////////////////////////////////////////////////////////////
//	IPlayback.cs
//	Light Sequencer
//	Written by Brian Peek (http://www.brianpeek.com/)
//	for the Animated Holiday Lights article
//		at Coding4Fun (http://msdn.microsoft.com/coding4fun/)
//////////////////////////////////////////////////////////////////////////////////

namespace LightSequencer
{
	public interface IPlayback
	{
		void Load(Sequence seq);
		void Unload();
		void Start();
		void Stop();
	}
}
