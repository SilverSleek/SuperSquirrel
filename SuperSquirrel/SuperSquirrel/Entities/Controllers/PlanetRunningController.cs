using System;

using Microsoft.Xna.Framework;

using SuperSquirrel.Entities.Planets;

namespace SuperSquirrel.Entities.Controllers
{
	class PlanetRunningController
	{
		private float angularDeceleration;
		private float angularMaxSpeed;

		public PlanetRunningController(Planet planet, float angularDeceleration, float angularMaxSpeed)
		{
			this.angularDeceleration = angularDeceleration;
			this.angularMaxSpeed = angularMaxSpeed;

			Planet = planet;
		}

		public Planet Planet { get; set; }

		public float Angle { get; set; }
		public float AngularVelocity { get; set; }
		public float AngularAcceleration { get; set; }

		public Vector2 Position { get; private set; }

		public void Update(float dt)
		{
			UpdateAngularVelocity(dt);

			Angle += AngularVelocity * dt;

			float x = (float)Math.Cos(Angle) * Planet.Radius;
			float y = (float)Math.Sin(Angle) * Planet.Radius;

			Position = Planet.Center + new Vector2(x, y);
		}

		private void UpdateAngularVelocity(float dt)
		{
			if (AngularAcceleration != 0)
			{
				AngularVelocity += AngularAcceleration * dt;

				if (AngularAcceleration < 0)
				{
					if (AngularVelocity < -angularMaxSpeed)
					{
						AngularVelocity = -angularMaxSpeed;
					}
				}
				else if (AngularAcceleration > 0)
				{
					if (AngularVelocity > angularMaxSpeed)
					{
						AngularVelocity = angularMaxSpeed;
					}
				}
			}
			else
			{
				if (AngularVelocity < 0)
				{
					AngularVelocity += angularDeceleration * dt;

					if (AngularVelocity > 0)
					{
						AngularVelocity = 0;
					}
				}
				else if (AngularVelocity > 0)
				{
					AngularVelocity -= angularDeceleration * dt;

					if (AngularVelocity < 0)
					{
						AngularVelocity = 0;
					}
				}
			}
		}
	}
}
