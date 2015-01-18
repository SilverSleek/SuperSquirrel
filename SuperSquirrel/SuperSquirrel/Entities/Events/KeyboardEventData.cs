using System.Collections.Generic;

using Microsoft.Xna.Framework.Input;

namespace SuperSquirrel.Entities.Events
{
	class KeyboardEventData
	{
		public KeyboardEventData(List<Keys> keysDown, List<Keys> keysPressedThisFrame, List<Keys> keysReleasedThisFrame)
		{
			KeysDown = keysDown;
			KeysPressedThisFrame = keysPressedThisFrame;
			KeysReleasedThisFrame = keysReleasedThisFrame;
		}

		public List<Keys> KeysDown { get; private set; }
		public List<Keys> KeysPressedThisFrame { get; private set; }
		public List<Keys> KeysReleasedThisFrame { get; private set; }
	}
}
