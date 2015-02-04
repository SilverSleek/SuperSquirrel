using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using SuperSquirrel.Common;
using SuperSquirrel.Interfaces;

namespace SuperSquirrel.Entities
{
	class Stars : ISimpleUpdateable, ISimpleDrawable
	{
		private const int DISTRIBUTION_RADIUS = 20;
		private const int PROXIMITY_POINT_LIMIT = 8;

		private Random random;
		private List<Star> stars;

		public Stars()
		{
			random = new Random();
			stars = new List<Star>();

			GenerateStars();
		}

		private void GenerateStars()
		{
			List<Vector2> points = new List<Vector2>();
			Vector2 startingPoint = new Vector2(Constants.SCREEN_WIDTH, Constants.SCREEN_HEIGHT) / 2;
			points.Add(startingPoint);

			while (points.Count > 0)
			{
				int pointsTested = 0;
				int sourcePointIndex = random.Next(points.Count);

				Vector2 sourcePoint = points[sourcePointIndex];

				while (pointsTested < PROXIMITY_POINT_LIMIT)
				{
					Vector2 point = ComputePointWithinDistributionRange(sourcePoint);

					if (CheckValidPoint(point))
					{
						points.Add(point);
						stars.Add(new Star(point));

						break;
					}
					else
					{
						pointsTested++;
					}
				}

				if (pointsTested == PROXIMITY_POINT_LIMIT)
				{
					points.RemoveAt(sourcePointIndex);
				}
			}
		}

		private Vector2 ComputePointWithinDistributionRange(Vector2 sourcePoint)
		{
			float radius = random.Next(DISTRIBUTION_RADIUS, DISTRIBUTION_RADIUS * 2 + 1);
			float angle = (float)random.NextDouble() * MathHelper.TwoPi;

			return sourcePoint + Functions.ComputePosition(angle, radius);
		}

		private bool CheckValidPoint(Vector2 point)
		{
			if (point.X < 0 || point.X > Constants.SCREEN_WIDTH || point.Y < 0 || point.Y > Constants.SCREEN_HEIGHT)
			{
				return false;
			}

			for (int i = 0; i < stars.Count; i++)
			{
				if (Vector2.DistanceSquared(point, stars[i].Position) < DISTRIBUTION_RADIUS * DISTRIBUTION_RADIUS)
				{
					return false;
				}
			}

			return true;
		}

		public void Update(float dt)
		{
		}

		public void Draw(SpriteBatch sb)
		{
			foreach (Star star in stars)
			{
				DrawingFunctions.DrawPoint(sb, star.Position, Color.White);
			}
		}

		private class Star
		{
			public Star(Vector2 position)
			{
				Position = position;
			}

			public Vector2 Position { get; private set; }
		}
	}
}
