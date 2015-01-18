using Microsoft.Xna.Framework;

using SuperSquirrel.Common;

namespace SuperSquirrel.Entities
{
	class Camera
	{
		private Vector2 screenCenter;
		private Vector2 lerpBasePosition;

		private Timer lerpTimer;

		public Camera()
		{
			screenCenter = new Vector2(Constants.SCREEN_WIDTH, Constants.SCREEN_HEIGHT) / 2;
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

			Transform = Matrix.CreateTranslation(new Vector3(Position, 0)) * Matrix.CreateRotationZ(Rotation) * Matrix.CreateScale(Zoom) *
				Matrix.CreateTranslation(new Vector3(screenCenter, 0));
			InverseTransform = Matrix.Invert(Transform);
		}
	}
}
