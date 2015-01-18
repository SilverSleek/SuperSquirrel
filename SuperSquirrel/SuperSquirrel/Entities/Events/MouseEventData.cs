using Microsoft.Xna.Framework;

namespace SuperSquirrel.Entities.Events
{
	class MouseEventData
	{
		public MouseEventData(Vector2 oldScreenPosition, Vector2 newScreenPosition, Vector2 oldWorldPosition, Vector2 newWorldPosition,
			ButtonStates leftButtonState, ButtonStates rightButtonState)
		{
			OldScreenPosition = oldScreenPosition;
			NewScreenPosition = newScreenPosition;
			OldWorldPosition = oldWorldPosition;
			NewWorldPosition = newWorldPosition;
			LeftButtonState = leftButtonState;
			RightButtonState = rightButtonState;
		}

		public Vector2 OldScreenPosition { get; private set; }
		public Vector2 NewScreenPosition { get; private set; }
		public Vector2 OldWorldPosition { get; private set; }
		public Vector2 NewWorldPosition { get; private set; }

		public ButtonStates LeftButtonState { get; private set; }
		public ButtonStates RightButtonState { get; private set; }
	}
}
