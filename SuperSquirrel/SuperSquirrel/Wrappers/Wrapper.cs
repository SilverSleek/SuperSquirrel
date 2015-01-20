using Microsoft.Xna.Framework.Graphics;

using SuperSquirrel.Interfaces;

namespace SuperSquirrel.Wrappers
{
	abstract class Wrapper : ISimpleUpdateable, ISimpleDrawable
	{
		public abstract void Update(float dt);
		public abstract void Draw(SpriteBatch sb);
	}
}
