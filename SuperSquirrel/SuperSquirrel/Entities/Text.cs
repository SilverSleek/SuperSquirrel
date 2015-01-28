using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SuperSquirrel.Entities
{
	class Text
	{
		private SpriteFont font;
		private Vector2 origin;

		public Text(SpriteFont font, string text, Vector2 position, OriginLocations originLocation, float rotation)
		{
			this.font = font;

			Position = position;
			Rotation = rotation;
			Value = text;
			origin = originLocation == OriginLocations.TOP_LEFT ? Vector2.Zero : font.MeasureString(text) / 2;
		}

		public Vector2 Position { get; set; }

		public string Value { get; set; }
		public float Rotation { get; set; }

		public void Draw(SpriteBatch sb)
		{
			sb.DrawString(font, Value, Position, Color.White, Rotation, origin, 1, SpriteEffects.None, 0);
		}
	}
}
