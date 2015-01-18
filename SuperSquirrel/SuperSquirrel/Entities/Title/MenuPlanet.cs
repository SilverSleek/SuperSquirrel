using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using SuperSquirrel.Common;
using SuperSquirrel.Entities.Planets;

namespace SuperSquirrel.Entities.Title
{
	class MenuPlanet
	{
		private static Texture2D[] textures;

		static MenuPlanet()
		{
			textures = new Texture2D[3];
			textures[0] = ContentLoader.LoadTexture("MenuPlanets/Small");
			textures[1] = ContentLoader.LoadTexture("MenuPlanets/Medium");
			textures[2] = ContentLoader.LoadTexture("MenuPlanets/Large");
		}

		private Sprite sprite;
		private CircularText circularText;

		public MenuPlanet(string text, Vector2 position, PlanetSizes size)
		{
			Texture2D texture = textures[(int)size];

			sprite = new Sprite(texture, position, OriginLocations.CENTER);
			circularText = new CircularText(text, position, texture.Width / 2, size);
		}

		public void Update(float dt)
		{
			circularText.Update(dt);
		}

		public void Draw(SpriteBatch sb)
		{
			sprite.Draw(sb);
			circularText.Draw(sb);
		}
	}
}
