using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using SuperSquirrel.Common;
using SuperSquirrel.Entities.Planets;

namespace SuperSquirrel.Entities.Title
{
	class CircularText
	{
		private static CircularFontAttributes[] fontAttributeList;
		private static Random random;

		static CircularText()
		{
			fontAttributeList = new CircularFontAttributes[3];
			fontAttributeList[0] = new CircularFontAttributes(ContentLoader.LoadFont("Small"), 7);
			fontAttributeList[1] = new CircularFontAttributes(ContentLoader.LoadFont("Medium"), 8);
			fontAttributeList[2] = new CircularFontAttributes(ContentLoader.LoadFont("Large"), 9);

			random = new Random();
		}

		private CircularCharacter[] characters;

		private int characterRadius;
		private float currentBaseAngle;
		private float angleIncrement;
		private float angleSpeed;

		private Vector2 center;

		public CircularText(string text, Vector2 center, int radius, PlanetSizes size)
		{
			const int SURFACE_OFFSET = 10;
			const int ARC_SPEED = 10;

			this.center = center;

			characters = new CircularCharacter[text.Length];

			CircularFontAttributes fontAttributes = fontAttributeList[(int)size];

			float startingAngle = (float)random.NextDouble() * MathHelper.TwoPi;
			float currentAngle = startingAngle;

			// arc length = theta * r (see http://www.coolmath.com/reference/circles-trigonometry.html)
			angleIncrement = (float)fontAttributes.ArcSpacing / radius;
			angleSpeed = (float)ARC_SPEED / radius;
			currentBaseAngle = startingAngle;
			characterRadius = radius + SURFACE_OFFSET;

			for (int i = 0; i < characters.Length; i++)
			{
				Vector2 position = CalculatePosition(center, currentAngle, characterRadius);

				characters[i] = new CircularCharacter(fontAttributes.Font, text[i], position, currentAngle + MathHelper.PiOver2);
				currentAngle += angleIncrement;
			}
		}

		private Vector2 CalculatePosition(Vector2 center, float angle, int radius)
		{
			float x = (float)Math.Cos(angle) * radius;
			float y = (float)Math.Sin(angle) * radius;

			return center + new Vector2(x, y);
		}

		public void Update(float dt)
		{
			currentBaseAngle += angleSpeed * dt;
			
			float currentAngle = currentBaseAngle;

			foreach (CircularCharacter character in characters)
			{
				Vector2 position = CalculatePosition(center, currentAngle, characterRadius);

				character.UpdateText(position, currentAngle + MathHelper.PiOver2);
				character.Update(dt);

				currentAngle += angleIncrement;
			}
		}

		public void Draw(SpriteBatch sb)
		{
			foreach (CircularCharacter character in characters)
			{
				character.Draw(sb);
			}
		}

		private class CircularCharacter
		{
			private Text text;

			public CircularCharacter(SpriteFont font, char character, Vector2 position, float rotation)
			{
				text = new Text(font, character.ToString(), position, OriginLocations.CENTER, rotation);
			}

			public void UpdateText(Vector2 position, float rotation)
			{
				text.Position = position;
				text.Rotation = rotation;
			}

			public void Update(float dt)
			{
			}

			public void Draw(SpriteBatch sb)
			{
				text.Draw(sb);
			}
		}

		private class CircularFontAttributes
		{
			public CircularFontAttributes(SpriteFont font, int arcSpacing)
			{
				Font = font;
				ArcSpacing = arcSpacing;
			}

			public SpriteFont Font { get; private set; }

			public int ArcSpacing { get; private set; }
		}
	}
}
