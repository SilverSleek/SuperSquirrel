using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using SuperSquirrel.Common;
using SuperSquirrel.Entities.Planets;
using SuperSquirrel.Helpers;

namespace SuperSquirrel.Entities.RopePhysics
{
	class Grapple : Mass
	{
		private const int MASS = 150;
		private const int HEAD_OFFSET = 24;
		private const int MAX_ROPE_LENGTH = 250;
		private const int READY_DELAY = 500;
		private const int ABANDONED_DURATION = 10000;

		private Player player;
		private Sprite sprite;
		private Mass secondMass;
		private PlanetHelper planetHelper;

		public Grapple(Player player, PlanetHelper planetHelper) :
			base(MASS, Vector2.Zero, Vector2.Zero)
		{
			this.player = player;
			this.planetHelper = planetHelper;

			sprite = new Sprite(ContentLoader.LoadTexture("Grapple"), Vector2.Zero, OriginLocations.BOTTOM_CENTER);
			Ready = true;
		}

		public Rope Rope { get; private set; }

		public bool Ready { get; private set; }
		public bool Active { get; private set; }
		public bool Retracting { get; set; }
		public bool Destroy { get; private set; }

		public void Launch(Vector2 position, Vector2 velocity, float angle, Mass playerMass)
		{
			sprite.Position = position;
			sprite.Rotation = angle + MathHelper.PiOver2;

			Rope = new Rope(position, this, playerMass, MAX_ROPE_LENGTH);
			secondMass = playerMass;
			Position = position;
			Velocity = velocity;
			Active = true;
			Fixed = false;
			Ready = false;
		}

		public void Abandon()
		{
			Retracting = false;

			Timer readyTimer = new Timer(READY_DELAY, () => { Ready = true; }, false);
			Timer destroyTimer = new Timer(ABANDONED_DURATION, () => { Destroy = true; }, false);
		}

		public void Update(float dt)
		{
			Rope.Update(dt);

			if (!Fixed)
			{
				CheckCollision();
			}

			if (Retracting)
			{
				Rope.Retract(dt);

				if (Rope.FullyRetracted)
				{
					Active = false;
					Ready = true;
					Retracting = false;
				}
			}
		}

		private void CheckCollision()
		{
			float endRotation = Functions.ComputeAngle(secondMass.Position, Position);

			sprite.Position = Position;
			sprite.Rotation = endRotation + MathHelper.PiOver2;

			Vector2 headOffset = Functions.ComputeDirection(endRotation) * HEAD_OFFSET;
			Vector2 headPosition = Position + headOffset;

			ProximityData data = planetHelper.CheckCollision(headPosition);

			if (data != null)
			{
				Planet planet = data.Planet;

				Position = planet.Center - data.Direction * planet.Radius - headOffset;
				sprite.Position = Position;
				sprite.Rotation = data.Angle + MathHelper.PiOver2;

				Rope.PurgeSleepingMasses();
				player.RegisterGrappleFixed();

				Fixed = true;
			}
		}

		public void Draw(SpriteBatch sb)
		{
			Rope.Draw(sb);
			sprite.Draw(sb);
		}
	}
}
