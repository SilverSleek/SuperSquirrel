using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SuperSquirrel.Entities
{
	class Sprite
	{
		private Texture2D texture;
		private Rectangle? sourceRect;
		private Vector2 origin;

		public Sprite(Texture2D texture, Vector2 position, OriginLocations originLocation) :
			this(texture, null, position, originLocation, Color.White)
		{
		}

		public Sprite(Texture2D texture, Vector2 position, OriginLocations originLocation, Color color) :
			this(texture, null, position, originLocation, color)
		{
		}

		public Sprite(Texture2D texture, Rectangle? sourceRect, Vector2 position, OriginLocations originLocation) :
			this(texture, sourceRect, position, originLocation, Color.White)
		{
		}

		public Sprite(Texture2D texture, Rectangle? sourceRect, Vector2 position, OriginLocations originLocation, Color color)
		{
			this.texture = texture;
			this.sourceRect = sourceRect;

			Position = position;
			Color = color;

			// TOP_LEFT just sets the origin to Vector.Zero
			switch (originLocation)
			{
				case OriginLocations.CENTER:
					origin = new Vector2(texture.Width, texture.Height) / 2;
					break;

				case OriginLocations.BOTTOM_CENTER:
					origin = new Vector2(texture.Width / 2, texture.Height);
					break;
			}
		}

		public Vector2 Position { get; set; }
		public Color Color { get; set; }

		public float Rotation { get; set; }

		public void Draw(SpriteBatch sb)
		{
			sb.Draw(texture, Position, sourceRect, Color, Rotation, origin, 1, SpriteEffects.None, 0);
		}
	}
}
