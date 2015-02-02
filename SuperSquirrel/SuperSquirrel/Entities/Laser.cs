using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using SuperSquirrel.Common;

namespace SuperSquirrel.Entities
{
	class Laser
	{
		private const int TIP_OFFSET = 7;

		private static Texture2D texture;

		static Laser()
		{
			texture = ContentLoader.LoadTexture("Laser");
		}

		private Sprite sprite;
		private Vector2 velocity;
		private Vector2 tipOffset;

		public Laser(Vector2 position, Vector2 velocity, float rotation, LivingEntity owner)
		{
			this.velocity = velocity;

			float tipOffsetX = (float)Math.Cos(rotation) * TIP_OFFSET;
			float tipOffsetY = (float)Math.Sin(rotation) * TIP_OFFSET;

			Owner = owner;
			tipOffset = new Vector2(tipOffsetX, tipOffsetY);
			sprite = new Sprite(texture, position, OriginLocations.CENTER);
			sprite.Rotation = rotation;
		}

		public bool Destroy { get; set; }

		public Vector2 Tip { get; private set; }

		public LivingEntity Owner { get; private set; }

		public void Update(float dt)
		{
			sprite.Position += velocity * dt;
			Tip = sprite.Position + tipOffset;
		}

		public void Draw(SpriteBatch sb)
		{
			sprite.Draw(sb);
		}
	}
}
