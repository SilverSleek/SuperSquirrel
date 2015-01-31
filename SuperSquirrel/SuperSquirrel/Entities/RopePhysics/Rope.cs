using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using SuperSquirrel.Common;
using SuperSquirrel.Entities.Planets;
using SuperSquirrel.Helpers;

namespace SuperSquirrel.Entities.RopePhysics
{
	class Rope
	{
		private const int DEFAULT_MASS = 1;
		private const int ITERATIONS = 1;
		private const int MASS_MAX_SPEED = 40;
		private const float DAMPING = 0.0075f;

		public const int DEFAULT_SEGMENT_LENGTH = 25;

		private static PlanetHelper planetHelper;

		public static void Initialize(PlanetHelper planetHelper)
		{
			Rope.planetHelper = planetHelper;
		}

		private List<Mass> masses;

		private int numSegments;
		private int segmentLength;

		public Rope(int length, Vector2 position, Mass headMass, Mass tailMass)
		{
			int numMasses = (int)(length / DEFAULT_SEGMENT_LENGTH) + 1;

			numSegments = numMasses - 1;
			segmentLength = length / numSegments;

			masses = new List<Mass>();
			masses.Add(headMass);

			for (int i = 1; i < numMasses - 1; i++)
			{
				masses.Add(new Mass(DEFAULT_MASS, position, Vector2.Zero));
			}

			masses.Add(tailMass);
		}

		public float CalculateEndRotation()
		{
			// this assumes the rope has at least two points, but it wouldn't really be a rope without at least two points anyway
			Mass mass1 = masses[masses.Count - 1];
			Mass mass2 = masses[masses.Count - 2];

			return Functions.ComputeAngle(mass2.Position, mass1.Position);
		}

		public void Update(float dt)
		{
			for (int i = 0; i < masses.Count - 1; i++)
			{
				for (int j = i; j < masses.Count - 1; j++)
				{
					UpdateMasses(masses[j], masses[j + 1], dt);
				}
			}

			CheckPlanets();
		}

		private void UpdateMasses(Mass mass1, Mass mass2, float dt)
		{
			if (mass1.Fixed && mass2.Fixed)
			{
				return;
			}

			if (!mass1.Fixed)
			{
				mass1.Position += mass1.Velocity * dt;
			}

			if (!mass2.Fixed)
			{
				mass2.Position += mass2.Velocity * dt;
			}

			float distance = Vector2.Distance(mass1.Position, mass2.Position);
			float difference = distance - segmentLength;

			if (difference > 0)
			{
				Vector2 direction = Vector2.Normalize(mass1.Position - mass2.Position);

				if (mass1.Fixed)
				{
					UpdateMass(mass2, direction * difference);
				}
				else if (mass2.Fixed)
				{
					UpdateMass(mass1, -direction * difference);
				}
				else
				{
					float totalMass = mass1.MassValue + mass2.MassValue;
					float amount1 = mass2.MassValue / totalMass;
					float amount2 = mass1.MassValue / totalMass;

					Vector2 offset1 = -direction * difference * amount1;
					Vector2 offset2 = direction * difference * amount2;

					UpdateMass(mass1, offset1);
					UpdateMass(mass2, offset2);
				}
			}
		}

		private void UpdateMass(Mass mass, Vector2 offset)
		{
			mass.Position += offset;
			mass.Velocity += offset;
		}

		private void CheckPlanets()
		{
			foreach (Mass mass in masses)
			{
				ProximityData data = planetHelper.CheckCollision(mass.Position);

				if (data != null)
				{
					Planet planet = data.Planet;

					mass.Position = planet.Center - data.Direction * planet.Radius;
					mass.Velocity -= Functions.ProjectVector(mass.Velocity, data.Direction);
					mass.ApplyForce(data.Direction * (planet.Radius - data.Distance));
				}
			}
		}

		public void Draw(SpriteBatch sb)
		{
			for (int i = 0; i < masses.Count - 1; i++)
			{
				DrawingFunctions.DrawLine(sb, masses[i].Position, masses[i + 1].Position, Color.Black);
			}
		}
	}
}
