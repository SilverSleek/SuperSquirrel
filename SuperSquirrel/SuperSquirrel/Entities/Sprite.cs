using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SuperSquirrel.Entities
{
	class Sprite
	{
		private Texture2D texture;
		private Vector2 origin;

		public Sprite(Texture2D texture, Vector2 position, OriginLocations originLocation)
		{
			this.texture = texture;

			Position = position;

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

		public float Rotation { get; set; }

		public void Draw(SpriteBatch sb)
		{
			sb.Draw(texture, Position, null, Color.White, Rotation, origin, 1, SpriteEffects.None, 0);
		}
	}
}
