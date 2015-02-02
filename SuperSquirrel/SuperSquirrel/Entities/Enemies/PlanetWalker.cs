using Microsoft.Xna.Framework;

using SuperSquirrel.Entities.Controllers;
using SuperSquirrel.Entities.Planets;

namespace SuperSquirrel.Entities.Enemies
{
	class PlanetWalker : Enemy
	{
		private const int HEALTH = 5;
		private const int POINT_VALUE = 10;
		private const int TIME_VALUE = 5000;
		private const int CIRCLE_RADIUS = 16;

		private const float ACCELERATION = 1000;
		private const float DECELERATION = 750;
		private const float MAX_SPEED = 250;

		private PlanetRunningController runningController;

		public PlanetWalker(Planet planet) :
			base(EnemyTypes.PLANET_WALKER, Vector2.Zero, HEALTH, POINT_VALUE, TIME_VALUE, CIRCLE_RADIUS)
		{
			runningController = new PlanetRunningController(ACCELERATION, DECELERATION, MAX_SPEED, planet);
		}

		protected override void AI(float dt)
		{
			runningController.Update(dt);
			Position = runningController.Position;
			Sprite.Rotation = runningController.Angle + MathHelper.PiOver2;
		}
	}
}
