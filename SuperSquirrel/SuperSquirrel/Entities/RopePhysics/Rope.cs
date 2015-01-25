using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using SuperSquirrel.Common;
using SuperSquirrel.Helpers;

namespace SuperSquirrel.Entities.RopePhysics
{
	class Rope
	{
		private const int DEFAULT_MASS = 1;

		public const int DEFAULT_SEGMENT_LENGTH = 25;

		private static PlanetHelper planetHelper;

		public static void Initialize(PlanetHelper planetHelper)
		{
			Rope.planetHelper = planetHelper;
		}

		private List<Mass> masses;
		private List<Spring> springs;

		public Rope(int length, Vector2 position, Mass headMass, Mass tailMass)
		{
			int numMasses = (int)(length / DEFAULT_SEGMENT_LENGTH) + 1;
			int numSprings = numMasses - 1;

			masses = new List<Mass>();
			springs = new List<Spring>();

			masses.Add(headMass);

			for (int i = 1; i < numMasses - 1; i++)
			{
				masses.Add(new Mass(DEFAULT_MASS, position, Vector2.Zero));
			}

			masses.Add(tailMass);

			for (int i = 0; i < numSprings; i++)
			{
				springs.Add(new Spring(masses[i], masses[i + 1]));
			}

			Spring.SegmentLength = length / numSprings;
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
			const int ITERATIONS = 8;
			const float DAMPING = 0.01f;

			for (int i = 0; i < ITERATIONS; i++)
			{
				foreach (Spring spring in springs)
				{
					spring.Update(dt);
				}

				foreach (Mass mass in masses)
				{
					if (!mass.Fixed)
					{
						mass.ApplyForce(-mass.Velocity * DAMPING);
						mass.Position += mass.Velocity * dt;
					}
				}
			}
		}

		public void Draw(SpriteBatch sb)
		{
			for (int i = 0; i < masses.Count - 1; i++)
			{
				DrawingFunctions.DrawLine(sb, masses[i].Position, masses[i + 1].Position, Color.Blue);
			}
		}

		private class Spring
		{
			public static float SegmentLength { get; set; }

			private Mass mass1;
			private Mass mass2;

			// taken from http://nehe.gamedev.net/tutorial/rope_physics/17006/
			public Spring(Mass mass1, Mass mass2)
			{
				this.mass1 = mass1;
				this.mass2 = mass2;
			}

			public void Update(float dt)
			{
				const float K = 0.5f;
				const float FRICTION = 0.075f;

				Vector2 force = Vector2.Zero;
				Vector2 difference = mass1.Position - mass2.Position;

				float magnitude = difference.Length();

				if (magnitude != 0)
				{
					force -= (difference / magnitude) * (magnitude - SegmentLength) * K;
					force -= (mass1.Velocity - mass2.Velocity) * FRICTION * dt;

					if (!mass1.Fixed)
					{
						mass1.ApplyForce(force);
					}

					if (!mass2.Fixed)
					{
						mass2.ApplyForce(-force);
					}
				}
			}
		}
	}
}
