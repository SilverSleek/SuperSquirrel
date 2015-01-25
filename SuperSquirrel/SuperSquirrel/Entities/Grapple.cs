using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using SuperSquirrel.Common;

namespace SuperSquirrel.Entities
{
	class Grapple : Mass
	{
		private const int MASS = 25;

		private Sprite sprite;

		public Grapple() :
			base(MASS, Vector2.Zero, Vector2.Zero)
		{
			sprite = new Sprite(ContentLoader.LoadTexture("Grapple"), Vector2.Zero, OriginLocations.BOTTOM_CENTER);
		}

		public Rope Rope { get; private set; }

		public bool Active { get; private set; }

		public void Launch(Vector2 position, Vector2 velocity, float angle, Mass tailMass)
		{
			const int ROPE_LENGTH = 300;

			sprite.Position = position;
			sprite.Rotation = angle + MathHelper.PiOver2;

			Rope = new Rope(ROPE_LENGTH, position, tailMass, this);
			Position = position;
			Velocity = velocity;
			Active = true;
		}

		public void Update(float dt)
		{
			Rope.Update(dt);

			sprite.Position = Position;
			sprite.Rotation = Rope.CalculateEndRotation() - MathHelper.PiOver2;
		}

		public void Draw(SpriteBatch sb)
		{
			Rope.Draw(sb);
			sprite.Draw(sb);
		}
	}
}
