using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using SuperSquirrel.Common;
using SuperSquirrel.Interfaces;

namespace SuperSquirrel.Entities.Display
{
	class TutorialIcons : ISimpleUpdateable, ISimpleDrawable
	{
		private List<ISimpleUpdateable> updateables;
		private List<ISimpleDrawable> drawables;
		private List<Sprite> sprites;

		public TutorialIcons(List<ISimpleUpdateable> updateables, List<ISimpleDrawable> drawables)
		{
			this.updateables = updateables;
			this.drawables = drawables;

			Texture2D spritesheet = ContentLoader.LoadTexture("TutorialIcons");
			Vector2 basePosition = new Vector2(-78, -250);

			List<SpriteData> dataList = new List<SpriteData>();
			dataList.Add(new SpriteData(new Rectangle(24, 0, 24, 24), new Vector2(28, 0)));     // W
			dataList.Add(new SpriteData(new Rectangle(24, 24, 24, 24), new Vector2(28, 28)));   // S
			dataList.Add(new SpriteData(new Rectangle(0, 24, 24, 24), new Vector2(0, 28)));     // A
			dataList.Add(new SpriteData(new Rectangle(48, 24, 24, 24), new Vector2(56, 28)));   // D
			dataList.Add(new SpriteData(new Rectangle(72, 0, 24, 32), new Vector2(100, 12)));   // Left click
			dataList.Add(new SpriteData(new Rectangle(96, 0, 24, 32), new Vector2(132, 12)));   // Right click
			dataList.Add(new SpriteData(new Rectangle(72, 32, 24, 24), new Vector2(100, -20))); // Laser
			dataList.Add(new SpriteData(new Rectangle(96, 32, 24, 24), new Vector2(132, -20))); // Grapple
			
			sprites = new List<Sprite>();

			foreach (SpriteData data in dataList)
			{
				sprites.Add(new Sprite(spritesheet, data.SourceRect, basePosition + data.Position, OriginLocations.TOP_LEFT));
			}
		}

		public void Update(float dt)
		{
		}

		public void Draw(SpriteBatch sb)
		{
			foreach (Sprite sprite in sprites)
			{
				sprite.Draw(sb);
			}
		}

		private class SpriteData
		{
			public SpriteData(Rectangle sourceRect, Vector2 position)
			{
				SourceRect = sourceRect;
				Position = position;
			}

			public Rectangle SourceRect { get; private set; }
			public Vector2 Position { get; private set; }
		}
	}
}
