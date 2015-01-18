using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using SuperSquirrel.Common;
using SuperSquirrel.Entities.Events;
using SuperSquirrel.Interfaces;

namespace SuperSquirrel.Entities.Display
{
	class PointDisplay : ISimpleEventListener
	{
		private Text text;

		private int points;

		public PointDisplay(int edgeOffset)
		{
			SpriteFont font = ContentLoader.LoadFont("Hud");
			Vector2 position = new Vector2(Constants.SCREEN_WIDTH - edgeOffset - font.MeasureString("A").X, edgeOffset);

			text = new Text(font, "0", position, OriginLocations.TOP_LEFT, 0);

			SimpleEvent.Queue.Enqueue(new SimpleEvent(EventTypes.LISTENER, new ListenerEventData(EventTypes.POINTS, this)));
		}

		public void EventResponse(SimpleEvent simpleEvent)
		{
			points += (int)simpleEvent.Data;
		}

		public void Update(float dt)
		{
			text.Value = points.ToString();
		}

		public void Draw(SpriteBatch sb)
		{
			text.Draw(sb);
		}
	}
}
