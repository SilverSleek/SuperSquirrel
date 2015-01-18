using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using SuperSquirrel.Common;

namespace SuperSquirrel.Entities.Planets
{
	public enum PlanetSizes
	{
		SMALL = 0,
		MEDIUM = 1,
		LARGE = 2
	}

	class Planet
	{
		public const int CAMERA_LERP_RANGE = 300;
		public const int CAMERA_SURFACE_DEPTH = 48;

		private static Texture2D[] textures;

		private static float[] masses;

		static Planet()
		{
			textures = new Texture2D[3];
			textures[0] = ContentLoader.LoadTexture("Planets/Small");
			textures[1] = ContentLoader.LoadTexture("Planets/Medium");
			textures[2] = ContentLoader.LoadTexture("Planets/Large");

			masses = new float[3];

			for (int i = 0; i < masses.Length; i++)
			{
				int radius = textures[i].Width;

				// A = pi * r^2
				masses[i] = MathHelper.Pi * radius * radius;
			}
		}

		private Sprite sprite;

		public Planet(Vector2 center, PlanetSizes size)
		{
			int index = (int)size;

			Texture2D texture = textures[index];

			sprite = new Sprite(texture, center, OriginLocations.CENTER);
			Center = center;
			Radius = texture.Width / 2;
			BoundingCircle = new BoundingCircle(center, Radius);
			Mass = masses[index];
		}

		// both of these are kept because in many places, you only need the center
		public Vector2 Center { get; private set; }
		public BoundingCircle BoundingCircle { get; private set; }

		// these variables could be shared among all planets of the same size, but a bunch of things are easier if each planet just stores
		// them independently
		public int Radius { get; private set; }
		public float Mass { get; private set; }

		public void Update(float dt)
		{
		}

		public void Draw(SpriteBatch sb)
		{
			sprite.Draw(sb);
		}
	}
}
