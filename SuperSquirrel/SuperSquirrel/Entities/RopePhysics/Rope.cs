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
		private const int ITERATIONS = 8;

		public const int MAX_SEGMENT_LENGTH = 25;

		private static PlanetHelper planetHelper;

		public static void Initialize(PlanetHelper planetHelper)
		{
			Rope.planetHelper = planetHelper;
		}

		private List<Mass> masses;
		private List<Spring> springs;

		public Rope(Vector2 position, Mass headMass, Mass tailMass, int length)
		{
			int numMasses = (int)(length / MAX_SEGMENT_LENGTH) + 1;
			int numSprings = numMasses - 1;

			springs = new List<Spring>(numSprings);
			SleepingMasses = new List<Mass>();
			masses = new List<Mass>(numMasses);
			masses.Add(headMass);

			for (int i = 1; i < numMasses - 1; i++)
			{
				Mass mass = new Mass(Mass.DEFAULT_MASS, position, Vector2.Zero);
				mass.Asleep = true;

				masses.Add(mass);
				SleepingMasses.Add(mass);
			}

			masses.Add(tailMass);

			for (int i = 0; i < numSprings; i++)
			{
				springs.Add(new Spring(masses[i], masses[i + 1], this));
			}
		}

		public List<Mass> SleepingMasses { get; private set; }

		/*
		public Rope(Vector2 start, Vector2 end, Mass fixedMass)
		{
			float length = Vector2.Distance(start, end);
			int numMasses = (int)(length / MAX_SEGMENT_LENGTH) + 1;
			int numSprings = numMasses - 1;

			Vector2 increment = (end - start) / numSprings;

			masses = new List<Mass>(numMasses);
			masses.Add(fixedMass);
			springs = new List<Spring>(numSprings);

			for (int i = 1; i < numMasses; i++)
			{
				masses.Add(new Mass(Mass.DEFAULT_MASS, start + increment * i, Vector2.Zero));
			}

			for (int i = 0; i < numSprings; i++)
			{
				springs.Add(new Spring(masses[i], masses[i + 1]));
			}
		}
		*/

		public void Update(float dt)
		{
			foreach (Mass mass in masses)
			{
				if (!(mass.Fixed || mass.Asleep))
				{
					mass.Position += mass.Velocity * dt;
				}
			}

			for (int j = 0; j < ITERATIONS; j++)
			{
				for (int i = 0; i < springs.Count; i++)
				{
					springs[i].Update(dt);
				}
			}

			CheckPlanets();
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

		private class Spring
		{
			private Mass mass1;
			private Mass mass2;
			private Rope rope;

			public Spring(Mass mass1, Mass mass2, Rope rope)
			{
				this.mass1 = mass1;
				this.mass2 = mass2;
				this.rope = rope;

				SegmentLength = MAX_SEGMENT_LENGTH;
			}

			public float SegmentLength { get; set; }

			public void Update(float dt)
			{
				if ((mass1.Fixed && mass2.Fixed) || (mass1.Asleep && mass2.Asleep))
				{
					return;
				}

				float distance = Vector2.Distance(mass1.Position, mass2.Position);
				float difference = distance - SegmentLength;

				if (difference > 0)
				{
					Vector2 direction = Vector2.Normalize(mass1.Position - mass2.Position);

					if (mass1.Fixed)
					{
						ApplyOffset(mass2, direction * difference);
					}
					else if (mass2.Fixed)
					{
						ApplyOffset(mass1, -direction * difference);
					}
					else
					{
						float totalMass = mass1.MassValue + mass2.MassValue;
						float amount1 = mass2.MassValue / totalMass;
						float amount2 = mass1.MassValue / totalMass;

						Vector2 offset1 = -direction * difference * amount1;
						Vector2 offset2 = direction * difference * amount2;

						ApplyOffset(mass1, offset1);
						ApplyOffset(mass2, offset2);
					}
				}
			}

			private void ApplyOffset(Mass mass, Vector2 offset)
			{
				mass.Position += offset;
				mass.Velocity += offset;
			}
		}
	}
}
