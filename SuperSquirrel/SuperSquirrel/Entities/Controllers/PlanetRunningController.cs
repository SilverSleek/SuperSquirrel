using System;

using Microsoft.Xna.Framework;

using SuperSquirrel.Common;
using SuperSquirrel.Entities.Planets;

namespace SuperSquirrel.Entities.Controllers
{
	public enum AccelerationValues
	{
		POSITIVE,
		NEGATIVE,
		NONE
	}

	class PlanetRunningController
	{
		private float acceleration;
		private float deceleration;
		private float maxSpeed;
		private float angularAcceleration;
		private float angularDeceleration;
		private float angularMaxSpeed;

		private Planet planet;

		public PlanetRunningController(float acceleration, float deceleration, float maxSpeed, Planet planet)
		{
			this.acceleration = acceleration;
			this.deceleration = deceleration;
			this.maxSpeed = maxSpeed;
			this.planet = planet;

			SetAngularValues(Vector2.Zero, Vector2.Zero);
		}

		public AccelerationValues AccelerationValue { get; set; }

		public float Angle { get; set; }
		public float AngularVelocity { get; set; }

		public Vector2 Position { get; private set; }

		private void SetAngularValues(Vector2 position, Vector2 velocity)
		{
			int radius = planet.Radius;

			// arc length = theta * r, so theta = arc length / r (see http://www.coolmath.com/reference/circles-trigonometry.html)
			angularAcceleration = acceleration / radius;
			angularDeceleration = deceleration / radius;
			angularMaxSpeed = maxSpeed / radius;

			Angle = Functions.ComputeAngle(planet.Center, position);
			AngularVelocity = ComputeAngularVelocity(velocity);
		}

		private float ComputeAngularVelocity(Vector2 velocity)
		{
			if (velocity == Vector2.Zero)
			{
				return 0;
			}

			// this is vector projection (see http://en.wikipedia.org/wiki/Vector_projection)
			float orthogonalAngle = Angle + MathHelper.PiOver2;
			float magnitude = Vector2.Dot(velocity, Functions.ComputeDirection(orthogonalAngle));

			return magnitude / planet.Radius;
		}

		public void SetLanding(Planet planet, Vector2 position, Vector2 velocity)
		{
			this.planet = planet;

			SetAngularValues(position, velocity);
		}

		public Vector2 ComputeRealVelocity()
		{
			float speed = Math.Abs(AngularVelocity * planet.Radius);
			float orthogonalAngle = Angle;

			if (AngularVelocity > 0)
			{
				orthogonalAngle += MathHelper.PiOver2;
			}
			else if (AngularVelocity < 0)
			{
				orthogonalAngle -= MathHelper.PiOver2;
			}

			return Functions.ComputeDirection(orthogonalAngle) * speed;
		}

		public void Update(float dt)
		{
			UpdateAngularVelocity(dt);

			Angle += AngularVelocity * dt;

			float x = (float)Math.Cos(Angle) * planet.Radius;
			float y = (float)Math.Sin(Angle) * planet.Radius;

			Position = planet.Center + new Vector2(x, y);
		}

		private void UpdateAngularVelocity(float dt)
		{
			if (AccelerationValue == AccelerationValues.POSITIVE)
			{
				AngularVelocity += angularAcceleration * dt;

				if (AngularVelocity > angularMaxSpeed)
				{
					AngularVelocity = angularMaxSpeed;
				}
			}
			else if (AccelerationValue == AccelerationValues.NEGATIVE)
			{
				AngularVelocity -= angularAcceleration * dt;

				if (AngularVelocity < -angularMaxSpeed)
				{
					AngularVelocity = -angularMaxSpeed;
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
