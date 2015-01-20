using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using SuperSquirrel.Interfaces;

namespace SuperSquirrel.Entities.Display
{
	class Hud : ISimpleUpdateable, ISimpleDrawable
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

		public void Update(float dt)
		{
			heartDisplay.Update(dt);
			pointDisplay.Update(dt);
			timeDisplay.Update(dt);
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
