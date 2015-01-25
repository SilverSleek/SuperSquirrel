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
		private const int EDGE_OFFSET = 3;

		private Random random;
		private Rectangle generationRect;
		private Rectangle destructionRect;

		private List<Star> stars;

		public Stars()
		{
			random = new Random();
			destructionRect = new Rectangle(0, 0, Constants.SCREEN_WIDTH + EDGE_OFFSET * 2, Constants.SCREEN_HEIGHT + EDGE_OFFSET * 2);
			stars = new List<Star>();
		}

		public void Update(float dt)
		{
			Vector2 cameraPosition = -Camera.Instance.Position;

			destructionRect.X = (int)cameraPosition.X - Constants.SCREEN_WIDTH / 2 - EDGE_OFFSET;
			destructionRect.Y = (int)cameraPosition.Y - Constants.SCREEN_HEIGHT / 2 - EDGE_OFFSET;

			for (int i = 0; i < stars.Count; i++)
			{
				Vector2 position = stars[i].Sprite.Position;

				if (!destructionRect.Contains((int)position.X, (int)position.Y))
				{
					stars.RemoveAt(i);
				}
			}

			float x = (float)random.NextDouble() * Constants.SCREEN_WIDTH;
			float y = (float)random.NextDouble() * Constants.SCREEN_HEIGHT;
			
			StarSizes size = (StarSizes)random.Next(0, 3);

			stars.Add(new Star(new Vector2(x, y), Color.White, size));
		}

		public void Draw(SpriteBatch sb)
		{
			foreach (Star star in stars)
			{
				star.Sprite.Draw(sb);
			}
		}

		private enum StarSizes
		{
			SMALL = 0,
			MEDIUM = 1,
			LARGE = 2
		}

		private class Star
		{
			private static Texture2D[] textures;

			static Star()
			{
				textures = new Texture2D[3];
				textures[0] = ContentLoader.LoadTexture("Stars/Small");
				textures[1] = ContentLoader.LoadTexture("Stars/Medium");
				textures[2] = ContentLoader.LoadTexture("Stars/Large");
			}

			public Star(Vector2 position, Color color, StarSizes size)
			{
				Sprite = new Sprite(textures[(int)size], position, OriginLocations.CENTER, color);
			}

			public Sprite Sprite { get; private set; }
		}
	}
}
