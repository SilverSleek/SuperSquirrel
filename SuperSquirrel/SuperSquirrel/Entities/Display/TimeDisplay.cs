using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using SuperSquirrel.Common;
using SuperSquirrel.Entities.Events;
using SuperSquirrel.Interfaces;

namespace SuperSquirrel.Entities.Display
{
	class TimeDisplay : ISimpleEventListener
	{
		private const int STARTING_TIME = 60000;

		private Text text;
		private Timer countdownTimer;

		public TimeDisplay(int edgeOffset)
		{
			Vector2 position = new Vector2(edgeOffset, edgeOffset * 2 + HeartDisplay.CONTAINER_HEIGHT);

			text = new Text(ContentLoader.LoadFont("Hud"), "00:00:00", position, OriginLocations.TOP_LEFT, 0);
			countdownTimer = new Timer(STARTING_TIME, TimeExpired, false);

			SimpleEvent.Queue.Enqueue(new SimpleEvent(EventTypes.LISTENER, new ListenerEventData(EventTypes.TIME, this)));
		}

		private void TimeExpired()
		{
		}

		public void EventResponse(SimpleEvent simpleEvent)
		{
			countdownTimer.Delay -= (int)simpleEvent.Data;
		}

		public void Update(float dt)
		{
			TimeSpan timeSpan = TimeSpan.FromMilliseconds(STARTING_TIME - countdownTimer.Delay);

			text.Value = timeSpan.Hours + ":" + timeSpan.Minutes + ":" + timeSpan.Seconds;
		}

		public void Draw(SpriteBatch sb)
		{
			text.Draw(sb);
		}
	}
}
