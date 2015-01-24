using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using SuperSquirrel.Common;

namespace SuperSquirrel.Entities
{
	class Rope
	{
		private Mass[] masses;
		private Spring[] springs;

		public Rope(Vector2 start, Vector2 end)
		{
			const int DEFAULT_SEGMENT_LENGTH = 25;
			const int DEFAULT_MASS = 1;

			float distance = Vector2.Distance(start, end);
			int numMasses = (int)(distance / DEFAULT_SEGMENT_LENGTH) + 1;
			float segmentBaseLength = distance / numMasses;

			Vector2 increment = Vector2.Normalize(end - start) * segmentBaseLength;

			masses = new Mass[numMasses];
			springs = new Spring[numMasses - 1];
			
			for (int i = 0; i < masses.Length; i++)
			{
				masses[i] = new Mass(DEFAULT_MASS, start + increment * i, Vector2.Zero);
			}

			for (int i = 0; i < springs.Length; i++)
			{
				springs[i] = new Spring(masses[i], masses[i + 1]);
			}

			Spring.SegmentBaseLength = segmentBaseLength;

			masses[0].Fixed = true;
		}

		public void SetFirstPoint(Vector2 position)
		{
			masses[0].Position = position;
		}

		public void Update(float dt)
		{
			const int ITERATIONS = 8;
			const float MASS_VELOCITY_FACTOR = 0.01f;

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
						mass.ApplyForce(-mass.Velocity * MASS_VELOCITY_FACTOR);
						mass.Position += mass.Velocity;
					}
				}
			}
		}

		public void Draw(SpriteBatch sb)
		{
			for (int i = 0; i < masses.Length - 1; i++)
			{
				DrawingFunctions.DrawLine(sb, masses[i].Position, masses[i + 1].Position, Color.Black);
			}
		}

		private class Spring
		{
			public static float SegmentBaseLength { get; set; }

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
				const float K = 0.025f;
				const float FRICTION = 0.075f;

				Vector2 force = Vector2.Zero;
				Vector2 difference = mass1.Position - mass2.Position;

				float magnitude = difference.Length();

				if (magnitude != 0)
				{
					force -= (difference / magnitude) * (magnitude - SegmentBaseLength) * K;
					force -= (mass1.Velocity - mass2.Velocity) * FRICTION;

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
