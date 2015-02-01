using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using SuperSquirrel.Common;
using SuperSquirrel.Entities.Controllers;
using SuperSquirrel.Entities.Events;
using SuperSquirrel.Entities.Planets;
using SuperSquirrel.Entities.RopePhysics;
using SuperSquirrel.Helpers;
using SuperSquirrel.Interfaces;
using SuperSquirrel.Wrappers;

namespace SuperSquirrel.Entities
{
	class Player : LivingEntity, ISimpleUpdateable, ISimpleDrawable, ISimpleEventListener
	{
		private const int STARTING_HEALTH = 20;
		private const int CIRCLE_RADIUS = 10;
		private const int MASS = 25;
		private const int LASER_SPEED = 600;
		private const int GRAPPLE_SPEED = 500;

		private enum MovementStates
		{
			PLANET,
			DRIFT,
			GRAPPLE
		}

		private Sprite sprite;
		private Grapple grapple;
		private Mass tetherMass;
		private Mass nextAwakeMass;
		private Planet landedPlanet;
		private Planet lastPlanet;
		private PlanetHelper planetHelper;
		private MovementStates movementState;
		private PlanetRunningController runningController;

		public Player(Planet startingPlanet, PlanetHelper planetHelper) :
			base(Vector2.Zero, CIRCLE_RADIUS, STARTING_HEALTH)
		{
			const float ACCELERATION = 4000;
			const float DECELERATION = 2000;
			const float MAX_SPEED = 600;

			this.planetHelper = planetHelper;

			landedPlanet = startingPlanet;
			lastPlanet = landedPlanet;

			Camera.Instance.Position = -startingPlanet.Center;
			
			sprite = new Sprite(ContentLoader.LoadTexture("Player"), Vector2.Zero, OriginLocations.CENTER);
			grapple = new Grapple(this, planetHelper);
			movementState = MovementStates.PLANET;
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
			if (movementState == MovementStates.PLANET)
			{
				HandleRunning(data);
				HandleJumping(data);
			}
		}

		private void HandleRunning(KeyboardEventData data)
		{
			bool aDown = false;
			bool dDown = false;

			KeyboardState keyboardState = Keyboard.GetState();

			foreach (Keys key in data.KeysDown)
			{
				switch (key)
				{
					case Keys.A:
					case Keys.Left:
						aDown = true;
						break;

					case Keys.D:
					case Keys.Right:
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

		private void HandleJumping(KeyboardEventData data)
		{
			const float JUMP_SPEED = 600;

			foreach (Keys key in data.KeysPressedThisFrame)
			{
				if (key == Keys.W || key == Keys.Up || key == Keys.Space)
				{
					Vector2 jumpVector = Functions.ComputeDirection(runningController.Angle) * JUMP_SPEED;
					jumpVector += runningController.ComputeRealVelocity();

					Velocity = Vector2.Normalize(jumpVector) * JUMP_SPEED;
					lastPlanet = landedPlanet;
					landedPlanet = null;
					movementState = MovementStates.DRIFT;

					Camera.Instance.SetLerpPositions(Camera.Instance.Position, Position);

					break;
				}
			}
		}

		private void HandleMouseData(MouseEventData data)
		{
			bool leftButtonPressedThisFrame = data.LeftButtonState == ButtonStates.PRESSED_THIS_FRAME;
			bool rightButtonPressedThisFrame = data.RightButtonState == ButtonStates.PRESSED_THIS_FRAME;

			if (leftButtonPressedThisFrame || rightButtonPressedThisFrame)
			{
				float angle = Functions.ComputeAngle(Position, data.NewWorldPosition);

				Vector2 direction = Functions.ComputeDirection(angle);

				if (leftButtonPressedThisFrame)
				{
					SimpleEvent.Queue.Enqueue(new SimpleEvent(EventTypes.LASER, new LaserEventData(Position, direction * LASER_SPEED,
						angle, this)));
				}

				if (rightButtonPressedThisFrame && grapple.Ready)
				{
					CreateTetherMass();

					if (movementState == MovementStates.DRIFT)
					{
						movementState = MovementStates.GRAPPLE;
						lastPlanet = null;
						Velocity = Vector2.Zero;
					}

					grapple.Launch(Position, direction * GRAPPLE_SPEED, angle, tetherMass);
					nextAwakeMass = grapple;
				}
			}
		}
		
		private void CreateTetherMass()
		{
			Vector2 tetherMassVelocity = Vector2.Zero;

			if (movementState == MovementStates.DRIFT)
			{
				tetherMassVelocity = Velocity;
			}
			else if (movementState == MovementStates.GRAPPLE)
			{
				tetherMassVelocity = tetherMass.Velocity;
			}

			tetherMass = new Mass(MASS, Position, tetherMassVelocity);
			tetherMass.Fixed = movementState == MovementStates.PLANET;
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
			if (grapple.Active)
			{
				if (nextAwakeMass != null && Vector2.Distance(Position, nextAwakeMass.Position) >= Rope.MAX_SEGMENT_LENGTH)
				{
					List<Mass> sleepingMasses = grapple.Rope.SleepingMasses;

					sleepingMasses[0].Asleep = false;
					nextAwakeMass = sleepingMasses[0];
					sleepingMasses.RemoveAt(0);

					if (sleepingMasses.Count == 0)
					{
						nextAwakeMass = null;
					}
				}

				grapple.Update(dt);
			}

			switch (movementState)
			{
				case MovementStates.PLANET:
					UpdatePlanetMovement(dt);
					break;

				case MovementStates.DRIFT:
					UpdateDriftMovement(dt);
					break;

				case MovementStates.GRAPPLE:
					UpdateGrappleMovement(dt);
					break;
			}

			if (grapple.Active && !grapple.Fixed)
			{
				foreach (Mass mass in grapple.Rope.SleepingMasses)
				{
					mass.Position = Position;
				}
			}

			BoundingCircle.Center = Position;
			sprite.Position = Position;
		}

		private void UpdatePlanetMovement(float dt)
		{
			runningController.Update(dt);
			Position = runningController.Position;
			sprite.Rotation = runningController.Angle;

			if (grapple.Active)
			{
				tetherMass.Position = Position;
			}

			Camera camera = Camera.Instance;
			Vector2 cameraTarget = CalculateCameraTarget();

			if (camera.Lerping)
			{
				camera.LerpTargetPosition = cameraTarget;
			}
			else
			{
				camera.Position = cameraTarget;
			}
		}

		private void UpdateDriftMovement(float dt)
		{
			CheckLanding();

			// the movement state can be changed in the previous function
			if (movementState == MovementStates.DRIFT)
			{
				Velocity += planetHelper.CalculateGravity(Position, MASS) * dt;
				Position += Velocity * dt;

				UpdateCamera();
			}
		}

		private void UpdateGrappleMovement(float dt)
		{
			Position = tetherMass.Position;

			CheckLanding();
			UpdateCamera();
		}

		private void CheckLanding()
		{
			Planet planet = planetHelper.CheckCollision(BoundingCircle);

			if (planet != null)
			{
				// This prevents an immediate collision with the planet you just jumped off.
				if (planet != lastPlanet && BoundingCircle.Intersects(planet.BoundingCircle))
				{
					landedPlanet = planet;
					movementState = MovementStates.PLANET;
					grapple.Retract();

					runningController.SetLanding(landedPlanet, Position, Velocity);
					Camera.Instance.SetLerpPositions(Camera.Instance.Position, CalculateCameraTarget());
				}
			}
			else
			{
				lastPlanet = null;
			}
		}

		private void UpdateCamera()
		{
			Camera camera = Camera.Instance;

			if (camera.Lerping)
			{
				camera.LerpTargetPosition = -Position;
			}
			else
			{
				camera.Position = -Position;
			}
		}

		private Vector2 CalculateCameraTarget()
		{
			Vector2 direction = Vector2.Normalize(landedPlanet.Center - Position);

			return -(landedPlanet.Center - direction * landedPlanet.Radius + direction * Planet.CAMERA_SURFACE_DEPTH);
		}

		public void RegisterGrappleFixed()
		{
			nextAwakeMass = null;
		}

		public override void Draw(SpriteBatch sb)
		{
			sprite.Draw(sb);

			if (grapple.Active)
			{
				grapple.Draw(sb);
			}
		}
	}
}
