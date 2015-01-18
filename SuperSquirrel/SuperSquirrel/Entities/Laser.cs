using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using SuperSquirrel.Common;

namespace SuperSquirrel.Entities
{
	class Laser
	{
		private static Texture2D texture;

		static Laser()
		{
			texture = ContentLoader.LoadTexture("Laser");
		}

		private Sprite sprite;
		private Vector2 velocity;

		public Laser(Vector2 position, Vector2 velocity, float rotation)
		{
			this.velocity = velocity;

			sprite = new Sprite(texture, position, OriginLocations.CENTER);
			sprite.Rotation = rotation;
		}

		public bool Destroy { get; set; }

		public Vector2 Tip { get; private set; }

		public void Update(float dt)
		{
			const int TIP_OFFSET_LENGTH = 14;

			sprite.Position += velocity * dt;

			float tipOffsetX = (float)Math.Cos(sprite.Rotation) * TIP_OFFSET_LENGTH;
			float tipOffsetY = (float)Math.Sin(sprite.Rotation) * TIP_OFFSET_LENGTH;

			Tip = sprite.Position + new Vector2(tipOffsetX, tipOffsetY);
		}

		public void Draw(SpriteBatch sb)
		{
			sprite.Draw(sb);
		}
	}
}
