using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace AlienAttack
{
	public enum GameState
	{
		TitleScreen,
		GameScreen
	};

	/// <summary>
	/// This is the main type for your game
	/// </summary>
	public class AlienAttackGame : Microsoft.Xna.Framework.Game
	{
		private GraphicsDeviceManager graphics;
		private SpriteBatch spriteBatch;
		private GameState gameState;
		private IScreen screen;

		public static AudioManager AudioManager;

#if ZUNE
		public static int ScreenWidth = 240;
		public static int ScreenHeight = 320;
#else
		public static int ScreenWidth = 1024;
		public static int ScreenHeight = 768;
#endif

		public AlienAttackGame()
		{
			graphics = new GraphicsDeviceManager(this);

			// set our screen size based on the device
			graphics.PreferredBackBufferWidth = ScreenWidth;
			graphics.PreferredBackBufferHeight = ScreenHeight;

			Content.RootDirectory = "Content";
		}

		/// <summary>
		/// Allows the game to perform any initialization it needs to before starting to run.
		/// This is where it can query for any required services and load any non-graphic
		/// related content.  Calling base.Initialize will enumerate through any components
		/// and initialize them as well.
		/// </summary>
		protected override void Initialize()
		{
			// create the title screen
			screen = new TitleScreen(this.Content);
			gameState = GameState.TitleScreen;

			// create the audio helper
			AudioManager = new AudioManager(Content);

			base.Initialize();
		}

		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		protected override void LoadContent()
		{
			// Create a new SpriteBatch, which can be used to draw textures.
			spriteBatch = new SpriteBatch(GraphicsDevice);
		}

		/// <summary>
		/// UnloadContent will be called once per game and is the place to unload
		/// all content.
		/// </summary>
		protected override void UnloadContent()
		{
		}

		/// <summary>
		/// Allows the game to run logic such as updating the world,
		/// checking for collisions, gathering input, and playing audio.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Update(GameTime gameTime)
		{
			// update the user input
			InputManager.Update();

			// Allows the game to exit
			if(InputManager.ControlState.Quit)
				this.Exit();

			// update the current screen
			GameState newState = screen.Update(gameTime);

			// if the screen returns a new state, change it here
			if(gameState != newState)
			{
				switch(newState)
				{
					case GameState.TitleScreen:
						screen = new TitleScreen(this.Content);
						break;
					case GameState.GameScreen:
						screen = new GameScreen(this.Content);
						break;
				}
				gameState = newState;
			}

			base.Update(gameTime);
		}

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw(GameTime gameTime)
		{
			// open the spritebatch, draw the screen, close it up
			this.spriteBatch.Begin();
				screen.Draw(gameTime, this.spriteBatch);
			this.spriteBatch.End();
		}
	}
}
