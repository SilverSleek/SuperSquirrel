using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SuperSquirrel.Common
{
	class DrawingFunctions
	{
		private static Texture2D whitePixel;

		static DrawingFunctions()
		{
			whitePixel = ContentLoader.LoadTexture("WhitePixel");
		}

		public static void DrawLine(SpriteBatch sb, Vector2 start, Vector2 end, Color color)
		{
			float length = (end - start).Length();
			float rotation = Functions.ComputeAngle(start, end);

			Rectangle sourceRect = new Rectangle(0, 0, (int)length, 1);

			sb.Draw(whitePixel, start, sourceRect, color, rotation, Vector2.Zero, 1, SpriteEffects.None, 0);
		}
	}
}
