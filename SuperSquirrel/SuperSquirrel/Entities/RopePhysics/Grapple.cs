using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using SuperSquirrel.Common;
using SuperSquirrel.Entities.Planets;
using SuperSquirrel.Helpers;

namespace SuperSquirrel.Entities.RopePhysics
{
	class Grapple : Mass
	{
		private const int MASS = 25;

		private Sprite sprite;
		private PlanetHelper planetHelper;
		private BoundingCircle boundingCircle;

		private int headOffset;

		public Grapple(PlanetHelper planetHelper) :
			base(MASS, Vector2.Zero, Vector2.Zero)
		{
			const int CIRCLE_RADIUS = 4;

			this.planetHelper = planetHelper;

			Texture2D texture = ContentLoader.LoadTexture("Grapple");

			sprite = new Sprite(texture, Vector2.Zero, OriginLocations.BOTTOM_CENTER);
			boundingCircle = new BoundingCircle(Vector2.Zero, CIRCLE_RADIUS);
			headOffset = texture.Height;
			Ready = true;
		}

		public Rope Rope { get; private set; }

		public bool Ready { get; private set; }
		public bool Active { get; private set; }

		public void Launch(Vector2 position, Vector2 velocity, float angle, Mass tailMass)
		{
			const int ROPE_LENGTH = 250;

			sprite.Position = position;
			sprite.Rotation = angle + MathHelper.PiOver2;

			Rope = new Rope(ROPE_LENGTH, position, tailMass, this);
			Position = position;
			Velocity = velocity;
			Active = true;
			Fixed = false;
		}

		public void Retract()
		{
			Active = false;
		}

		public void Update(float dt)
		{
			Rope.Update(dt);

			if (!Fixed)
			{
				sprite.Position = Position;
				sprite.Rotation = Rope.CalculateEndRotation() + MathHelper.PiOver2;
				boundingCircle.Center = Position + Functions.ComputeDirection(sprite.Rotation) * headOffset;

				Planet planet = planetHelper.CheckPlanetCollision(boundingCircle);

				if (planet != null)
				{
					Vector2 difference = Position - planet.Center;

					Position = planet.Center + Vector2.Normalize(difference) * (planet.Radius + headOffset);
					sprite.Position = Position;
					sprite.Rotation = Functions.ComputeAngle(difference) - MathHelper.PiOver2;
					Fixed = true;
				}
			}
		}

		public void Draw(SpriteBatch sb)
		{
			Rope.Draw(sb);
			sprite.Draw(sb);
		}
	}
}
