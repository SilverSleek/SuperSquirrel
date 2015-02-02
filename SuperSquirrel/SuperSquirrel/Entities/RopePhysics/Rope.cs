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
		private const int RETRACT_RATE = 150;

		private static PlanetHelper planetHelper;

		public static void Initialize(PlanetHelper planetHelper)
		{
			Rope.planetHelper = planetHelper;
		}

		private List<Mass> masses;
		private List<Spring> springs;

		public Rope(Vector2 position, Mass headMass, Mass tailMass, int length)
		{
			int numMasses = (int)(length / Spring.MAX_SEGMENT_LENGTH) + 1;
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

		public bool FullyRetracted { get; private set; }

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

		public void PurgeSleepingMasses()
		{
			// this list can be null if a grapple was abandoned and then it anchors to a planet
			if (SleepingMasses != null)
			{
				if (SleepingMasses.Count > 0)
				{
					int firstSleepingMassIndex = masses.IndexOf(SleepingMasses[0]);

					springs[firstSleepingMassIndex - 1].Mass2 = masses[masses.Count - 1];
					masses.RemoveRange(masses.IndexOf(SleepingMasses[0]), SleepingMasses.Count);
					springs.RemoveRange(firstSleepingMassIndex, SleepingMasses.Count - 1);
				}

				SleepingMasses = null;
			}
		}

		public void Retract(float dt)
		{
			Spring spring = springs[springs.Count - 1];
			spring.SegmentLength -= RETRACT_RATE * dt;

			if (spring.SegmentLength <= 0)
			{
				float leftoverRetractLength = -spring.SegmentLength;

				springs.RemoveAt(springs.Count - 1);

				if (springs.Count == 0)
				{
					FullyRetracted = true;
				}
				else
				{
					Spring nextSpring = springs[springs.Count - 1];

					masses.RemoveAt(masses.Count - 2);
					nextSpring.Mass2 = masses[masses.Count - 1];
					nextSpring.SegmentLength -= leftoverRetractLength;
				}
			}
		}

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
	}
}
