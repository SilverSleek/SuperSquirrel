using Microsoft.Xna.Framework;

using SuperSquirrel.Entities.Controllers;
using SuperSquirrel.Entities.Planets;

namespace SuperSquirrel.Entities.Enemies
{
	class PlanetWalker : Enemy
	{
		private const int HEALTH = 3;
		private const int POINT_VALUE = 10;
		private const int TIME_VALUE = 5000;
		private const int CIRCLE_RADIUS = 18;

		private PlanetRunningController controller;

		public PlanetWalker(Planet planet) :
			base(EnemyTypes.PLANET_WALKER, Vector2.Zero, HEALTH, POINT_VALUE, TIME_VALUE, CIRCLE_RADIUS)
		{
			//const float ANGULAR_ACCELERATION = 0.01f;
			//const float ANGULAR_MAX_SPEED = MathHelper.Pi / 8;

			//controller = new PlanetRunningController(planet, 0, ANGULAR_MAX_SPEED);
			//controller.AngularAcceleration = ANGULAR_ACCELERATION;
		}

		protected override void AI(float dt)
		{
			controller.Update(dt);
			Position = controller.Position;
			Sprite.Rotation = controller.Angle + MathHelper.PiOver2;
		}
	}
}
