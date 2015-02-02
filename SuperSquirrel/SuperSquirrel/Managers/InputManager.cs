using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using SuperSquirrel.Entities;
using SuperSquirrel.Entities.Events;

namespace SuperSquirrel.Managers
{
	class InputManager
	{
		private KeyboardState oldKS;
		private KeyboardState newKS;
		private MouseState oldMS;
		private MouseState newMS;

		public void Update()
		{
			AddKeyboardEvent();
			AddMouseEvent();
		}

		private void AddKeyboardEvent()
		{
			oldKS = newKS;
			newKS = Keyboard.GetState();

			Keys[] oldKeysDown = oldKS.GetPressedKeys();
			Keys[] newKeysDown = newKS.GetPressedKeys();

			List<Keys> keysDown = new List<Keys>(newKeysDown);
			List<Keys> keysPressedThisFrame = new List<Keys>(newKeysDown.Except<Keys>(oldKeysDown).ToArray<Keys>());
			List<Keys> keysReleasedThisFrame = new List<Keys>(oldKeysDown.Except<Keys>(newKeysDown).ToArray<Keys>());

			SimpleEvent.Queue.Enqueue(new SimpleEvent(EventTypes.KEYBOARD, new KeyboardEventData(keysDown, keysPressedThisFrame,
				keysReleasedThisFrame)));
		}

		private void AddMouseEvent()
		{
			oldMS = newMS;
			newMS = Mouse.GetState();

			Vector2 oldScreenPosition = new Vector2(oldMS.X, oldMS.Y);
			Vector2 newScreenPosition = new Vector2(newMS.X, newMS.Y);
			Vector2 oldWorldPosition = Vector2.Transform(oldScreenPosition, Camera.Instance.InverseTransform);
			Vector2 newWorldPosition = Vector2.Transform(newScreenPosition, Camera.Instance.InverseTransform);

			ButtonStates leftButtonState = GetButtonState(oldMS.LeftButton, newMS.LeftButton);
			ButtonStates rightButtonState = GetButtonState(oldMS.RightButton, newMS.RightButton);

			SimpleEvent.Queue.Enqueue(new SimpleEvent(EventTypes.MOUSE, new MouseEventData(oldScreenPosition, newScreenPosition,
				oldWorldPosition, newWorldPosition, leftButtonState, rightButtonState)));
		}

		private ButtonStates GetButtonState(ButtonState oldButton, ButtonState newButton)
		{
			if (oldButton != newButton)
			{
				return newButton == ButtonState.Pressed ? ButtonStates.PRESSED_THIS_FRAME : ButtonStates.RELEASED_THIS_FRAME;
			}

			return newButton == ButtonState.Pressed ? ButtonStates.HELD : ButtonStates.RELEASED;
		}
	}
}
