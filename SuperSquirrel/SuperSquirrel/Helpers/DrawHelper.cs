using System.Collections.Generic;

using Microsoft.Xna.Framework.Graphics;

using SuperSquirrel.Entities;
using SuperSquirrel.Interfaces;

namespace SuperSquirrel.Helpers
{
	class DrawHelper
	{
		public DrawHelper()
		{
			Drawables = new List<ISimpleDrawable>();
		}

		public List<ISimpleDrawable> Drawables { get; set; }

		public void Draw(SpriteBatch sb)
		{
			sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default,
				RasterizerState.CullCounterClockwise, null, Camera.Instance.Transform);

			foreach (ISimpleDrawable drawable in Drawables)
			{
				drawable.Draw(sb);
			}

			sb.End();
		}
	}
}
