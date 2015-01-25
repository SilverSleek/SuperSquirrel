using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using SuperSquirrel.Common;
using SuperSquirrel.Entities.Controllers;
using SuperSquirrel.Entities.Events;
using SuperSquirrel.Entities.Planets;
using SuperSquirrel.Helpers;
using SuperSquirrel.Interfaces;
using SuperSquirrel.Wrappers;

namespace SuperSquirrel.Entities
{
	class Player : LivingEntity, ISimpleUpdateable, ISimpleDrawable, ISimpleEventListener
	{
		private const int STARTING_HEALTH = 20;
		private const int CIRCLE_RADIUS = 10;

		private Camera camera;
		private Sprite sprite;
		private Grapple grapple;
		private Mass tetherMass;
		private Planet landedPlanet;
		private Planet mostRecentPlanet;
		private PlanetHelper planetHelper;
		private PlanetRunningController runningController;

		private List<ProximityData> dataList;

		public Player(Planet startingPlanet, PlanetHelper planetHelper) :
			base(Vector2.Zero, CIRCLE_RADIUS, STARTING_HEALTH)
		{
			const float ACCELERATION = 2000;
			const float DECELERATION = 1000;
			const float MAX_SPEED = 300;

			this.planetHelper = planetHelper;

			landedPlanet = startingPlanet;
			mostRecentPlanet = landedPlanet;

			camera = Camera.Instance;
			camera.Position = -startingPlanet.Center;
			
			sprite = new Sprite(ContentLoader.LoadTexture("Player"), Vector2.Zero, OriginLocations.CENTER);
			grapple = new Grapple();
			runningController = new PlanetRunningController(ACCELERATION, DECELERATION, MAX_SPEED, startingPlanet);

			SimpleEvent.Queue.Enqueue(new SimpleEvent(EventTypes.LISTENER, new ListenerEventData(EventTypes.KEYBOARD, this)));
			SimpleEvent.Queue.Enqueue(new SimpleEvent(EventTypes.LISTENER, new ListenerEventData(EventTypes.MOUSE, this)));
			SimpleEvent.Queue.Enqueue(new SimpleEvent(EventTypes.HEALTH, STARTING_HEALTH));
		}

		public void EventResponse(SimpleEvent simpleEvent)
		{
			switch (simpleEvent.Type)
			{
				case EventTypes.KEYBOARD:
					HandleKeyboardData((KeyboardEventData)simpleEvent.Data);
					break;

				case EventTypes.MOUSE:
					HandleMouseData((MouseEventData)simpleEvent.Data);
					break;
			}
		}

		private void HandleKeyboardData(KeyboardEventData data)
		{
			if (landedPlanet != null)
			{
				HandlePlanetJumping(data);
				UpdateAngularAcceleration(data);
			}
		}

		private void HandlePlanetJumping(KeyboardEventData data)
		{
			const float JUMP_SPEED = 300;

			foreach (Keys key in data.KeysPressedThisFrame)
			{
				if (key == Keys.W)
				{
					Vector2 realVelocityVector = runningController.ComputeRealVelocity();

					Vector2 jumpVector = Functions.ComputeDirection(runningController.Angle) * JUMP_SPEED;
					jumpVector += runningController.ComputeRealVelocity();

					Velocity = Vector2.Normalize(jumpVector) * JUMP_SPEED;
					mostRecentPlanet = landedPlanet;
					landedPlanet = null;

					camera.SetLerpPositions(camera.Position, Position);

					break;
				}
			}
		}

		private void UpdateAngularAcceleration(KeyboardEventData data)
		{
			bool aDown = false;
			bool dDown = false;

			KeyboardState keyboardState = Keyboard.GetState();

			foreach (Keys key in data.KeysDown)
			{
				switch (key)
				{
					case Keys.A:
						aDown = true;
						break;

					case Keys.D:
						dDown = true;
						break;
				}

				if (aDown && dDown)
				{
					break;
				}
			}

			if (aDown && !dDown)
			{
				runningController.AccelerationValue = AccelerationValues.NEGATIVE;
			}
			else if (dDown && !aDown)
			{
				runningController.AccelerationValue = AccelerationValues.POSITIVE;
			}
			else
			{
				runningController.AccelerationValue = AccelerationValues.NONE;
			}
		}

		private void HandleMouseData(MouseEventData data)
		{
			const int LASER_SPEED = 600;
			const int GRAPPLE_SPEED = 100;

			bool leftButtonPressedThisFrame = data.LeftButtonState == ButtonStates.PRESSED_THIS_FRAME;
			bool rightButtonPressedThisFrame = data.RightButtonState == ButtonStates.PRESSED_THIS_FRAME;

			if (leftButtonPressedThisFrame || rightButtonPressedThisFrame)
			{
				float angle = Functions.ComputeAngle(Position, data.NewWorldPosition);
				float x = (float)Math.Cos(angle);
				float y = (float)Math.Sin(angle);

				Vector2 direction = new Vector2(x, y);

				if (leftButtonPressedThisFrame)
				{
					SimpleEvent.Queue.Enqueue(new SimpleEvent(EventTypes.LASER, new LaserEventData(Position, direction * LASER_SPEED,
						angle, this)));
				}

				if (rightButtonPressedThisFrame)
				{
					tetherMass = new Mass(1, Position, Vector2.Zero);
					tetherMass.Fixed = true;

					grapple.Launch(Position, direction * GRAPPLE_SPEED, angle, tetherMass);
				}
			}
		}

		protected override void OnDamage(int damage)
		{
			base.OnDamage(damage);

			SimpleEvent.Queue.Enqueue(new SimpleEvent(EventTypes.HEALTH, Health));
		}

		protected override void OnDeath()
		{
		}

		public override void Update(float dt)
		{
			const int MASS = 1;
			const float CAMERA_DRIFT_FACTOR = 0.5f;

			if (landedPlanet != null)
			{
				runningController.Update(dt);

				Position = runningController.Position;
				sprite.Rotation = runningController.Angle;

				Vector2 cameraTarget = CalculateLandedCameraTarget();

				if (camera.Lerping)
				{
					camera.LerpTargetPosition = cameraTarget;
				}
				else
				{
					camera.Position = cameraTarget;
				}
			}
			else
			{
				dataList = planetHelper.GetProximityData(Position);

				CheckPlanetLanding(dataList);

				// if the player lands on a planet in the previous function, this variable will be non-null here
				if (landedPlanet == null)
				{
					Velocity += planetHelper.CalculateGravity(Position, MASS) * dt;
					Position += Velocity * dt;

					Vector2 cameraTarget = -(Position - Velocity * CAMERA_DRIFT_FACTOR);

					if (camera.Lerping)
					{
						camera.LerpTargetPosition = cameraTarget;
					}
					else
					{
						camera.Position = cameraTarget;
					}
				}
			}

			if (grapple.Active)
			{
				tetherMass.Position = Position;

				grapple.Update(dt);
			}

			BoundingCircle.Center = Position;
			sprite.Position = Position;
		}

		private void CheckPlanetLanding(List<ProximityData> dataList)
		{
			Planet planet = planetHelper.GetClosestPlanet(dataList);

			// this prevents an immediate collision with the planet you just jumped off
			if (planet == mostRecentPlanet)
			{
				if (!BoundingCircle.Intersects(mostRecentPlanet.BoundingCircle))
				{
					mostRecentPlanet = null;
				}
			}
			else if (BoundingCircle.Intersects(planet.BoundingCircle))
			{
				landedPlanet = planet;

				runningController.SetLanding(landedPlanet, Position, Velocity);
				camera.SetLerpPositions(camera.Position, CalculateLandedCameraTarget());
			}
		}

		private Vector2 CalculateLandedCameraTarget()
		{
			Vector2 direction = Vector2.Normalize(landedPlanet.Center - Position);

			return -(landedPlanet.Center - direction * landedPlanet.Radius + direction * Planet.CAMERA_SURFACE_DEPTH);
		}

		public override void Draw(SpriteBatch sb)
		{
			const int INDICATOR_LENGTH = 50;

			sprite.Draw(sb);

			if (grapple.Active)
			{
				grapple.Draw(sb);
			}

			if (Constants.DEBUG && landedPlanet == null)
			{
				for (int i = 0; i < dataList.Count; i++)
				{
					DrawingFunctions.DrawLine(sb, Position, Position + dataList[i].Direction * INDICATOR_LENGTH, Color.LightGray);
				}
			}
		}
	}
}
