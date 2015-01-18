using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SuperSquirrel.Entities.Display
{
	class Hud
	{
		private HeartDisplay heartDisplay;
		private PointDisplay pointDisplay;
		private TimeDisplay timeDisplay;

		public Hud()
		{
			const int EDGE_OFFSET = 15;

			heartDisplay = new HeartDisplay(EDGE_OFFSET);
			pointDisplay = new PointDisplay(EDGE_OFFSET);
			timeDisplay = new TimeDisplay(EDGE_OFFSET);
		}

		public void Update(GameTime gameTime, float dt)
		{
			heartDisplay.Update(dt);
			pointDisplay.Update(dt);
			timeDisplay.Update(gameTime, dt);
		}

		public void Draw(SpriteBatch sb)
		{
			// this should be called last to avoid having to reapply the camera
			sb.End();
			sb.Begin();

			heartDisplay.Draw(sb);
			pointDisplay.Draw(sb);
			timeDisplay.Draw(sb);
		}
	}
}
