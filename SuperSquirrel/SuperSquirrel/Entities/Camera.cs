using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using SuperSquirrel.Common;
using SuperSquirrel.Interfaces;

namespace SuperSquirrel.Entities
{
	class Camera : ISimpleUpdateable, ISimpleDrawable
	{
		public static Camera Instance { get; private set; }

		static Camera()
		{
			Instance = new Camera();
		}

		private Vector2 screenCenter;
		private Vector2 debugVerticalStart;
		private Vector2 debugVerticalEnd;
		private Vector2 debugHorizontalStart;
		private Vector2 debugHorizontalEnd;
		private Vector2 lerpBasePosition;

		private Timer lerpTimer;

		private Camera()
		{
			screenCenter = new Vector2(Constants.SCREEN_WIDTH, Constants.SCREEN_HEIGHT) / 2;
			debugVerticalStart = new Vector2(Constants.SCREEN_WIDTH / 2, 0);
			debugVerticalEnd = new Vector2(Constants.SCREEN_WIDTH / 2, Constants.SCREEN_HEIGHT);
			debugHorizontalStart = new Vector2(0, Constants.SCREEN_HEIGHT / 2);
			debugHorizontalEnd = new Vector2(Constants.SCREEN_WIDTH, Constants.SCREEN_HEIGHT / 2);

			Zoom = 1;
			Transform = Matrix.Identity;
			InverseTransform = Matrix.Identity;
		}

		public Vector2 Position { get; set; }
		public Vector2 LerpTargetPosition { get; set; }

		public float Rotation { get; set; }
		public float Zoom { get; set; }

		public Matrix Transform { get; private set; }
		public Matrix InverseTransform { get; private set; }

		public bool Lerping { get; private set; }

		public void SetLerpPositions(Vector2 basePosition, Vector2 targetPosition)
		{
			const int LERP_DURATION = 250;

			lerpBasePosition = basePosition;
			LerpTargetPosition = targetPosition;
			Lerping = true;

			if (lerpTimer != null)
			{
				lerpTimer.Destroy = true;
			}

			lerpTimer = new Timer(LERP_DURATION, EndLerp, false);
		}

		private void EndLerp()
		{
			Position = LerpTargetPosition;
			lerpTimer = null;
			Lerping = false;
		}

		public void Update(float dt)
		{
			if (lerpTimer != null)
			{
				Position = Vector2.Lerp(lerpBasePosition, LerpTargetPosition, lerpTimer.Progress);
			}

			Vector2 correctedPosition = new Vector2((int)Position.X, (int)Position.Y);

			Transform = Matrix.CreateTranslation(new Vector3(correctedPosition, 0)) * Matrix.CreateRotationZ(Rotation) * Matrix.CreateScale(Zoom) *
				Matrix.CreateTranslation(new Vector3(screenCenter, 0));
			InverseTransform = Matrix.Invert(Transform);
		}

		public void Draw(SpriteBatch sb)
		{
			if (Constants.DEBUG)
			{
				sb.End();
				sb.Begin();

				DrawingFunctions.DrawLine(sb, debugVerticalStart, debugVerticalEnd, Color.LightGray);
				DrawingFunctions.DrawLine(sb, debugHorizontalStart, debugHorizontalEnd, Color.LightGray);
			}
		}
	}
}
