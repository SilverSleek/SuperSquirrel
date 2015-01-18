using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SuperSquirrel.Common
{
	class ContentLoader
	{
		private static ContentManager content;

		public static void Initialize(ContentManager content)
		{
			ContentLoader.content = content;
		}

		public static Texture2D LoadTexture(string filename)
		{
			return content.Load<Texture2D>("Textures/" + filename);
		}

		public static SpriteFont LoadFont(string filename)
		{
			return content.Load<SpriteFont>("Fonts/" + filename);
		}
	}
}
