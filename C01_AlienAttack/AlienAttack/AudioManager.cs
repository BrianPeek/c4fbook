using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

namespace AlienAttack
{
	public class AudioManager
	{
		// the different fx that can be played
		public enum Cue
		{
			Theme,
			EnemyShot,
			PlayerShot,
			Explosion
		};

		// instances of the effects
		private SoundEffect theme;
		private SoundEffect enemyShot;
		private	SoundEffect playerShot;
		private SoundEffect explosion;

		public AudioManager(ContentManager contentManager)
		{
			// load 'em up
			theme = contentManager.Load<SoundEffect>("sfx\\theme");
			enemyShot = contentManager.Load<SoundEffect>("sfx\\enemyShot");
			playerShot = contentManager.Load<SoundEffect>("sfx\\playerShot");
			explosion = contentManager.Load<SoundEffect>("sfx\\explosion");
		}

		public void PlayCue(Cue cue)
		{
			// play the effect requested
			switch(cue)
			{
				case Cue.Theme:
					theme.Play();
					break;
				case Cue.EnemyShot:
					enemyShot.Play();
					break;
				case Cue.PlayerShot:
					playerShot.Play();
					break;
				case Cue.Explosion:
					explosion.Play();
					break;
			}
		}
	}
}
