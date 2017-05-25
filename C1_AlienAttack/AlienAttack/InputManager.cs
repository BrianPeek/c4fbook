using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace AlienAttack
{
	public struct ControlState
	{
		public bool Left;
		public bool Right;
		public bool Start;
		public bool Quit;
		public bool Fire;
	}

	public static class InputManager
	{
#if !ZUNE
		private static KeyboardState keyboardState, lastKeyboard;
#endif
		private static GamePadState gamePadState, lastGamePad;
		private static ControlState controlState;

		public static void Update()
		{
#if !ZUNE
			keyboardState = Keyboard.GetState();
#endif

			gamePadState = GamePad.GetState(PlayerIndex.One);

			controlState.Quit	= (gamePadState.Buttons.Back== ButtonState.Pressed);
			controlState.Start	= (gamePadState.Buttons.B	== ButtonState.Pressed); 
			controlState.Left	= (gamePadState.DPad.Left	== ButtonState.Pressed);
			controlState.Right	= (gamePadState.DPad.Right	== ButtonState.Pressed);
			controlState.Fire	= (gamePadState.Buttons.B	== ButtonState.Pressed && lastGamePad.Buttons.B == ButtonState.Released);

#if !ZUNE
			controlState.Quit	= (controlState.Quit	|| keyboardState.IsKeyDown(Keys.Escape));
			controlState.Start	= (controlState.Start	|| keyboardState.IsKeyDown(Keys.Enter));
			controlState.Left	= (controlState.Left	|| gamePadState.ThumbSticks.Left.X < -0.1f);
			controlState.Right	= (controlState.Right	|| gamePadState.ThumbSticks.Left.X > 0.1f);
			controlState.Left	= (controlState.Left	|| keyboardState.IsKeyDown(Keys.Left));
			controlState.Right	= (controlState.Right	|| keyboardState.IsKeyDown(Keys.Right));
			controlState.Fire	= (controlState.Fire	|| keyboardState.IsKeyDown(Keys.Space) && !lastKeyboard.IsKeyDown(Keys.Space));
#endif

			lastGamePad = gamePadState;

#if !ZUNE
			lastKeyboard = keyboardState;
#endif
		}

		public static ControlState ControlState
		{
			get { return controlState; }
		}
	}
}
