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
			origin = originLocation == OriginLocations.TOP_LEFT ? Vector2.Zero : new Vector2(texture.Width, texture.Height) / 2;
		}

		public Vector2 Position { get; set; }

		public float Rotation { get; set; }

		public void Draw(SpriteBatch sb)
		{
			sb.Draw(texture, Position, null, Color.White, Rotation, origin, 1, SpriteEffects.None, 0);
		}
	}
}
