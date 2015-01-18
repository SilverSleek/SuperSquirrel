using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using SuperSquirrel.Common;
using SuperSquirrel.Entities.Events;
using SuperSquirrel.Interfaces;

namespace SuperSquirrel.Entities.Display
{
	class HeartDisplay : ISimpleEventListener
	{
		public const int CONTAINER_HEIGHT = 24;

		private Texture2D spritesheet;

		private int health;
		private int numContainers;

		private Vector2 startPosition;

		public HeartDisplay(int edgeOffset)
		{
			const int CONTAINERS = 5;

			spritesheet = ContentLoader.LoadTexture("Hearts");
			startPosition = new Vector2(edgeOffset, edgeOffset);
			numContainers = CONTAINERS;

			SimpleEvent.Queue.Enqueue(new SimpleEvent(EventTypes.LISTENER, new ListenerEventData(EventTypes.HEALTH, this)));
		}

		public void EventResponse(SimpleEvent simpleEvent)
		{
			health = (int)simpleEvent.Data;
		}

		public void Update(float dt)
		{
		}

		public void Draw(SpriteBatch sb)
		{
			const int SPACING = 30;
			const int CONTAINER_WIDTH = 26;

			Vector2 position = startPosition;
			Rectangle sourceRect = new Rectangle(0, 0, CONTAINER_WIDTH, CONTAINER_HEIGHT);

			int partialIndex = health / 4;

			for (int i = 0; i < numContainers; i++)
			{
				int heartValue = 0;

				if (i < partialIndex)
				{
					heartValue = 4;
				}
				else if (i > partialIndex)
				{
					heartValue = 0;
				}
				else
				{
					int moduloValue = i == 0 ? 4 : i * 4;

					heartValue = health % moduloValue;
				}

				sourceRect.X = heartValue * CONTAINER_WIDTH;
				sb.Draw(spritesheet, position, sourceRect, Color.White);
				position.X += SPACING;
			}
		}
	}
}
