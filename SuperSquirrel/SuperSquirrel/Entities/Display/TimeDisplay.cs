using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using SuperSquirrel.Common;

namespace SuperSquirrel.Entities.Display
{
	class TimeDisplay
	{
		private Text text;

		public TimeDisplay(int edgeOffset)
		{
			Vector2 position = new Vector2(edgeOffset, edgeOffset * 2 + HeartDisplay.CONTAINER_HEIGHT);

			text = new Text(ContentLoader.LoadFont("Hud"), "00:00:00", position, OriginLocations.TOP_LEFT, 0);
		}

		public void Update(GameTime gameTime, float dt)
		{
			TimeSpan totalGameTime = gameTime.TotalGameTime;
			text.Value = totalGameTime.Hours + ":" + totalGameTime.Minutes + ":" + totalGameTime.Seconds;
		}

		public void Draw(SpriteBatch sb)
		{
			text.Draw(sb);
		}
	}
}
