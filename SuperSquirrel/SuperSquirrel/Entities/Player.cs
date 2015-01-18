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
	class Player : ISimpleEventListener
	{
		private Camera camera;
		private Sprite sprite;
		private Planet landedPlanet;
		private Planet mostRecentPlanet;
		private PlanetHelper planetHelper;
		private LaserWrapper laserWrapper;
		private BoundingCircle boundingCircle;

		private Vector2 position;
		private Vector2 velocity;

		private PlanetRunningController runningController;

		private List<ProximityData> dataList;

		public Player(Planet startingPlanet, PlanetHelper planetHelper, LaserWrapper laserWrapper, Camera camera)
		{
			const int BOUNDING_CIRCLE_RADIUS = 1;

			const float ANGULAR_DECELERATION = MathHelper.Pi * 3;
			const float ANGULAR_MAX_SPEED = MathHelper.Pi;

			this.planetHelper = planetHelper;
			this.laserWrapper = laserWrapper;
			this.camera = camera;

			landedPlanet = startingPlanet;
			mostRecentPlanet = landedPlanet;
			camera.Position = -startingPlanet.Center;
			
			sprite = new Sprite(ContentLoader.LoadTexture("Player"), Vector2.Zero, OriginLocations.CENTER);
			boundingCircle = new BoundingCircle(Vector2.Zero, BOUNDING_CIRCLE_RADIUS);
			runningController = new PlanetRunningController(startingPlanet, ANGULAR_DECELERATION, ANGULAR_MAX_SPEED);

			SimpleEvent.Queue.Enqueue(new SimpleEvent(EventTypes.LISTENER, new ListenerEventData(EventTypes.KEYBOARD, this)));
			SimpleEvent.Queue.Enqueue(new SimpleEvent(EventTypes.LISTENER, new ListenerEventData(EventTypes.MOUSE, this)));
		}

		public Vector2 Position
		{
			get { return position; }
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
					float angle = runningController.Angle;
					float speedX = (float)Math.Cos(angle) * JUMP_SPEED;
					float speedY = (float)Math.Sin(angle) * JUMP_SPEED;

					velocity = new Vector2(speedX, speedY);
					mostRecentPlanet = landedPlanet;
					landedPlanet = null;
					runningController.Planet = null;

					camera.SetLerpPositions(camera.Position, position);

					break;
				}
			}
		}

		private void UpdateAngularAcceleration(KeyboardEventData data)
		{
			const float ANGULAR_ACCELERATION = MathHelper.Pi * 8;

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
				runningController.AngularAcceleration = -ANGULAR_ACCELERATION;
			}
			else if (dDown && !aDown)
			{
				runningController.AngularAcceleration = ANGULAR_ACCELERATION;
			}
			else
			{
				runningController.AngularAcceleration = 0;
			}
		}

		private void HandleMouseData(MouseEventData data)
		{
			const int LASER_SPEED = 600;

			if (data.LeftButtonState == ButtonStates.PRESSED_THIS_FRAME)
			{
				float angle = Functions.ComputeAngle(position, data.NewWorldPosition);
				float speedX = (float)Math.Cos(angle) * LASER_SPEED;
				float speedY = (float)Math.Sin(angle) * LASER_SPEED;

				laserWrapper.Lasers.Add(new Laser(position, new Vector2(speedX, speedY), angle));
			}
		}

		public void Update(float dt)
		{
			const int MASS = 1;
			const float CAMERA_DRIFT_FACTOR = 0.5f;

			if (landedPlanet != null)
			{
				runningController.Update(dt);

				position = runningController.Position;
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
				dataList = planetHelper.GetProximityData(position);

				CheckPlanetLanding(dataList);

				// if the player lands on a planet in the previous function, this variable will be non-null here
				if (landedPlanet == null)
				{
					velocity += planetHelper.CalculateGravity(position, MASS, dataList) * dt;
					position += velocity * dt;

					Vector2 cameraTarget = -(position - velocity * CAMERA_DRIFT_FACTOR);

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

			sprite.Position = position;
			boundingCircle.Center = position;
		}

		private void CheckPlanetLanding(List<ProximityData> dataList)
		{
			Planet planet = planetHelper.GetNearestPlanet(dataList);

			// this prevents an immediate collision with the planet you just jumped off
			if (planet == mostRecentPlanet)
			{
				if (!boundingCircle.Intersects(mostRecentPlanet.BoundingCircle))
				{
					mostRecentPlanet = null;
				}
			}
			else if (boundingCircle.Intersects(planet.BoundingCircle))
			{
				landedPlanet = planet;

				runningController.Planet = planet;
				runningController.Angle = Functions.ComputeAngle(landedPlanet.Center, position);
				runningController.AngularVelocity = 0;
				runningController.AngularAcceleration = 0;

				camera.SetLerpPositions(camera.Position, CalculateLandedCameraTarget());
			}
		}

		private Vector2 CalculateLandedCameraTarget()
		{
			Vector2 direction = Vector2.Normalize(landedPlanet.Center - position);

			return -(landedPlanet.Center - direction * landedPlanet.Radius + direction * Planet.CAMERA_SURFACE_DEPTH);
		}

		public void Draw(SpriteBatch sb)
		{
			const int INDICATOR_LENGTH = 50;

			sprite.Draw(sb);

			if (Constants.DEBUG && landedPlanet == null)
			{
				for (int i = 0; i < dataList.Count; i++)
				{
					DrawingFunctions.DrawLine(sb, position, position + dataList[i].Direction * INDICATOR_LENGTH, Color.LightGray);
				}
			}
		}
	}
}
