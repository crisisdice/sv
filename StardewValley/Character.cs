using System;
using System.Linq;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Netcode;
using StardewValley.Network;
using xTile.Dimensions;

namespace StardewValley
{
	[InstanceStatics]
	public class Character : INetObject<NetFields>
	{
		public const float emoteBeginInterval = 20f;

		public const float emoteNormalInterval = 250f;

		public const int emptyCanEmote = 4;

		public const int questionMarkEmote = 8;

		public const int angryEmote = 12;

		public const int exclamationEmote = 16;

		public const int heartEmote = 20;

		public const int sleepEmote = 24;

		public const int sadEmote = 28;

		public const int happyEmote = 32;

		public const int xEmote = 36;

		public const int pauseEmote = 40;

		public const int videoGameEmote = 52;

		public const int musicNoteEmote = 56;

		public const int blushEmote = 60;

		public const int blockedIntervalBeforeEmote = 3000;

		public const int blockedIntervalBeforeSprint = 5000;

		public const double chanceForSound = 0.001;

		[XmlIgnore]
		public readonly NetRef<AnimatedSprite> sprite = new NetRef<AnimatedSprite>();

		[XmlIgnore]
		public readonly NetPosition position = new NetPosition();

		[XmlIgnore]
		private readonly NetInt netSpeed = new NetInt();

		[XmlIgnore]
		private readonly NetInt netAddedSpeed = new NetInt();

		[XmlIgnore]
		public readonly NetDirection facingDirection = new NetDirection(2);

		[XmlIgnore]
		public int blockedInterval;

		[XmlIgnore]
		public int faceTowardFarmerTimer;

		[XmlIgnore]
		public int forceUpdateTimer;

		[XmlIgnore]
		public int movementPause;

		[XmlIgnore]
		public NetEvent1Field<int, NetInt> faceTowardFarmerEvent = new NetEvent1Field<int, NetInt>();

		[XmlIgnore]
		public readonly NetInt faceTowardFarmerRadius = new NetInt();

		[XmlElement("name")]
		public readonly NetString name = new NetString();

		[XmlElement("forceOneTileWide")]
		public readonly NetBool forceOneTileWide = new NetBool(value: false);

		protected bool moveUp;

		protected bool moveRight;

		protected bool moveDown;

		protected bool moveLeft;

		protected bool freezeMotion;

		[XmlIgnore]
		private string _displayName;

		public bool isEmoting;

		public bool isCharging;

		public bool isGlowing;

		public bool coloredBorder;

		public bool flip;

		public bool drawOnTop;

		public bool faceTowardFarmer;

		public bool ignoreMovementAnimation;

		[XmlIgnore]
		public bool hasJustStartedFacingPlayer;

		[XmlElement("faceAwayFromFarmer")]
		public readonly NetBool faceAwayFromFarmer = new NetBool();

		protected int currentEmote;

		protected int currentEmoteFrame;

		protected readonly NetInt facingDirectionBeforeSpeakingToPlayer = new NetInt(-1);

		[XmlIgnore]
		public float emoteInterval;

		[XmlIgnore]
		public float xVelocity;

		[XmlIgnore]
		public float yVelocity;

		[XmlIgnore]
		public Vector2 lastClick = Vector2.Zero;

		public readonly NetFloat scale = new NetFloat(1f);

		public float timeBeforeAIMovementAgain;

		public float glowingTransparency;

		public float glowRate;

		private bool glowUp;

		[XmlIgnore]
		public readonly NetBool swimming = new NetBool();

		[XmlIgnore]
		public bool nextEventcommandAfterEmote;

		[XmlIgnore]
		public bool eventActor;

		[XmlIgnore]
		public bool farmerPassesThrough;

		[XmlIgnore]
		public readonly NetBool collidesWithOtherCharacters = new NetBool();

		protected bool ignoreMovementAnimations;

		[XmlIgnore]
		public int yJumpOffset;

		[XmlIgnore]
		public int ySourceRectOffset;

		[XmlIgnore]
		public float yJumpVelocity;

		[XmlIgnore]
		public float yJumpGravity = -0.5f;

		[XmlIgnore]
		public bool wasJumpWithSound;

		[XmlIgnore]
		private readonly NetFarmerRef whoToFace = new NetFarmerRef();

		[XmlIgnore]
		public Color glowingColor;

		[XmlIgnore]
		public PathFindController controller;

		private bool emoteFading;

		[XmlIgnore]
		private readonly NetBool _willDestroyObjectsUnderfoot = new NetBool(value: true);

		[XmlIgnore]
		protected readonly NetLocationRef currentLocationRef = new NetLocationRef();

		/// <summary>
		/// Used for modders to store metadata to this object. This data is synchronized in multiplayer and saved to the save data.
		/// </summary>
		[XmlIgnore]
		public ModDataDictionary modData = new ModDataDictionary();

		private Microsoft.Xna.Framework.Rectangle originalSourceRect;

		public static readonly Vector2[] AdjacentTilesOffsets = new Vector2[4]
		{
			new Vector2(1f, 0f),
			new Vector2(-1f, 0f),
			new Vector2(0f, -1f),
			new Vector2(0f, 1f)
		};

		[XmlIgnore]
		public readonly NetVector2 drawOffset = new NetVector2(Vector2.Zero);

		[XmlIgnore]
		public bool shouldShadowBeOffset;

		[XmlIgnore]
		public int speed
		{
			get
			{
				return netSpeed;
			}
			set
			{
				netSpeed.Value = value;
			}
		}

		[XmlIgnore]
		public int addedSpeed
		{
			get
			{
				return netAddedSpeed;
			}
			set
			{
				netAddedSpeed.Value = value;
			}
		}

		[XmlIgnore]
		public virtual string displayName
		{
			get
			{
				return _displayName ?? (_displayName = translateName(name));
			}
			set
			{
				_displayName = value;
			}
		}

		public bool willDestroyObjectsUnderfoot
		{
			get
			{
				return _willDestroyObjectsUnderfoot;
			}
			set
			{
				_willDestroyObjectsUnderfoot.Value = value;
			}
		}

		public Vector2 Position
		{
			get
			{
				return position;
			}
			set
			{
				if (position.Value != value)
				{
					position.Set(value);
				}
			}
		}

		public int Speed
		{
			get
			{
				return speed;
			}
			set
			{
				speed = value;
			}
		}

		public virtual int FacingDirection
		{
			get
			{
				return facingDirection;
			}
			set
			{
				facingDirection.Set(value);
			}
		}

		[XmlIgnore]
		public string Name
		{
			get
			{
				return name;
			}
			set
			{
				name.Set(value);
			}
		}

		[XmlIgnore]
		public virtual AnimatedSprite Sprite
		{
			get
			{
				return sprite.Value;
			}
			set
			{
				sprite.Value = value;
			}
		}

		public bool IsEmoting
		{
			get
			{
				return isEmoting;
			}
			set
			{
				isEmoting = value;
			}
		}

		public int CurrentEmote
		{
			get
			{
				return currentEmote;
			}
			set
			{
				currentEmote = value;
			}
		}

		public int CurrentEmoteIndex => currentEmoteFrame;

		public virtual bool IsMonster => false;

		public float Scale
		{
			get
			{
				return scale;
			}
			set
			{
				scale.Value = value;
			}
		}

		[XmlIgnore]
		public GameLocation currentLocation
		{
			get
			{
				return currentLocationRef.Value;
			}
			set
			{
				currentLocationRef.Value = value;
			}
		}

		/// <summary>Get the mod populated metadata as it will be serialized for game saving. Identical to <see cref="F:StardewValley.Character.modData" /> except returns null during save if it is empty. It is strongly recommended to use <see cref="F:StardewValley.Character.modData" /> instead.</summary>
		[XmlElement("modData")]
		public ModDataDictionary modDataForSerialization
		{
			get
			{
				return modData.GetForSerialization();
			}
			set
			{
				modData.SetFromSerialization(value);
			}
		}

		public NetFields NetFields { get; } = new NetFields();


		public Character()
		{
			initNetFields();
		}

		protected virtual void initNetFields()
		{
			NetFields.AddFields(sprite, position.NetFields, facingDirection, netSpeed, netAddedSpeed, name, scale, currentLocationRef.NetFields, swimming, collidesWithOtherCharacters, facingDirectionBeforeSpeakingToPlayer, faceTowardFarmerRadius, faceAwayFromFarmer, whoToFace.NetFields, faceTowardFarmerEvent, _willDestroyObjectsUnderfoot, forceOneTileWide);
			facingDirection.Position = position;
			faceTowardFarmerEvent.onEvent += performFaceTowardFarmerEvent;
			NetFields.AddField(modData);
		}

		public Character(AnimatedSprite sprite, Vector2 position, int speed, string name)
			: this()
		{
			Sprite = sprite;
			Position = position;
			this.speed = speed;
			Name = name;
			if (sprite != null)
			{
				originalSourceRect = sprite.SourceRect;
			}
		}

		protected virtual string translateName(string name)
		{
			return name;
		}

		public virtual void SetMovingUp(bool b)
		{
			moveUp = b;
			if (!b)
			{
				Halt();
			}
		}

		public virtual void SetMovingRight(bool b)
		{
			moveRight = b;
			if (!b)
			{
				Halt();
			}
		}

		public virtual void SetMovingDown(bool b)
		{
			moveDown = b;
			if (!b)
			{
				Halt();
			}
		}

		public virtual void SetMovingLeft(bool b)
		{
			moveLeft = b;
			if (!b)
			{
				Halt();
			}
		}

		public void setMovingInFacingDirection()
		{
			switch (FacingDirection)
			{
			case 0:
				SetMovingUp(b: true);
				break;
			case 1:
				SetMovingRight(b: true);
				break;
			case 2:
				SetMovingDown(b: true);
				break;
			case 3:
				SetMovingLeft(b: true);
				break;
			}
		}

		public int getFacingDirection()
		{
			if (Sprite.currentFrame < 4)
			{
				return 2;
			}
			if (Sprite.currentFrame < 8)
			{
				return 1;
			}
			if (Sprite.currentFrame < 12)
			{
				return 0;
			}
			return 3;
		}

		public void setTrajectory(int xVelocity, int yVelocity)
		{
			setTrajectory(new Vector2(xVelocity, yVelocity));
		}

		public virtual void setTrajectory(Vector2 trajectory)
		{
			xVelocity = trajectory.X;
			yVelocity = trajectory.Y;
		}

		public virtual void Halt()
		{
			moveUp = false;
			moveDown = false;
			moveRight = false;
			moveLeft = false;
			Sprite.StopAnimation();
		}

		public void extendSourceRect(int horizontal, int vertical, bool ignoreSourceRectUpdates = true)
		{
			Sprite.sourceRect.Inflate(Math.Abs(horizontal) / 2, Math.Abs(vertical) / 2);
			Sprite.sourceRect.Offset(horizontal / 2, vertical / 2);
			_ = originalSourceRect;
			if (Sprite.SourceRect.Equals(originalSourceRect))
			{
				Sprite.ignoreSourceRectUpdates = false;
			}
			else
			{
				Sprite.ignoreSourceRectUpdates = ignoreSourceRectUpdates;
			}
		}

		public virtual bool collideWith(Object o)
		{
			return true;
		}

		public virtual void faceDirection(int direction)
		{
			if (direction != -3)
			{
				FacingDirection = direction;
				if (Sprite != null)
				{
					Sprite.faceDirection(direction);
				}
				faceTowardFarmer = false;
			}
			else
			{
				faceTowardFarmer = true;
			}
		}

		public int getDirection()
		{
			if (moveUp)
			{
				return 0;
			}
			if (moveRight)
			{
				return 1;
			}
			if (moveDown)
			{
				return 2;
			}
			if (moveLeft)
			{
				return 3;
			}
			if (IsRemoteMoving())
			{
				return facingDirection;
			}
			return -1;
		}

		public bool IsRemoteMoving()
		{
			if (LocalMultiplayer.IsLocalMultiplayer(is_local_only: true))
			{
				if (!position.moving.Value)
				{
					return position.Field.IsInterpolating();
				}
				return true;
			}
			return position.Field.IsInterpolating();
		}

		public void tryToMoveInDirection(int direction, bool isFarmer, int damagesFarmer, bool glider)
		{
			if (!currentLocation.isCollidingPosition(nextPosition(direction), Game1.viewport, isFarmer, damagesFarmer, glider, this))
			{
				switch (direction)
				{
				case 0:
					position.Y -= speed + addedSpeed;
					break;
				case 1:
					position.X += speed + addedSpeed;
					break;
				case 2:
					position.Y += speed + addedSpeed;
					break;
				case 3:
					position.X -= speed + addedSpeed;
					break;
				}
			}
		}

		public virtual Vector2 GetShadowOffset()
		{
			if (shouldShadowBeOffset)
			{
				return drawOffset;
			}
			return Vector2.Zero;
		}

		public virtual bool shouldCollideWithBuildingLayer(GameLocation location)
		{
			if (controller == null)
			{
				return !IsMonster;
			}
			return false;
		}

		protected void applyVelocity(GameLocation currentLocation)
		{
			Microsoft.Xna.Framework.Rectangle nextPosition = GetBoundingBox();
			nextPosition.X += (int)xVelocity;
			nextPosition.Y -= (int)yVelocity;
			if (currentLocation == null || !currentLocation.isCollidingPosition(nextPosition, Game1.viewport, isFarmer: false, 0, glider: false, this))
			{
				position.X += xVelocity;
				position.Y -= yVelocity;
			}
			xVelocity = (int)(xVelocity - xVelocity / 2f);
			yVelocity = (int)(yVelocity - yVelocity / 2f);
		}

		public virtual void MovePosition(GameTime time, xTile.Dimensions.Rectangle viewport, GameLocation currentLocation)
		{
			if (this is FarmAnimal)
			{
				willDestroyObjectsUnderfoot = false;
			}
			bool should_destroy_underfoot_objects = willDestroyObjectsUnderfoot;
			if (controller != null && controller.nonDestructivePathing)
			{
				should_destroy_underfoot_objects = false;
			}
			if (xVelocity != 0f || yVelocity != 0f)
			{
				applyVelocity(currentLocation);
			}
			else if (moveUp)
			{
				if (currentLocation == null || !currentLocation.isCollidingPosition(nextPosition(0), viewport, isFarmer: false, 0, glider: false, this) || isCharging)
				{
					position.Y -= speed + addedSpeed;
					if (!ignoreMovementAnimation)
					{
						Sprite.AnimateUp(time, (speed - 2 + addedSpeed) * -25, Utility.isOnScreen(getTileLocationPoint(), 1, currentLocation) ? "Cowboy_Footstep" : "");
						faceDirection(0);
					}
				}
				else if (!currentLocation.isTilePassable(nextPosition(0), viewport) || !should_destroy_underfoot_objects)
				{
					Halt();
				}
				else if (should_destroy_underfoot_objects)
				{
					new Vector2(getStandingX() / 64, getStandingY() / 64 - 1);
					if (currentLocation.characterDestroyObjectWithinRectangle(nextPosition(0), showDestroyedObject: true))
					{
						doEmote(12);
						position.Y -= speed + addedSpeed;
					}
					else
					{
						blockedInterval += time.ElapsedGameTime.Milliseconds;
					}
				}
			}
			else if (moveRight)
			{
				if (currentLocation == null || !currentLocation.isCollidingPosition(nextPosition(1), viewport, isFarmer: false, 0, glider: false, this) || isCharging)
				{
					position.X += speed + addedSpeed;
					if (!ignoreMovementAnimation)
					{
						Sprite.AnimateRight(time, (speed - 2 + addedSpeed) * -25, Utility.isOnScreen(getTileLocationPoint(), 1, currentLocation) ? "Cowboy_Footstep" : "");
						faceDirection(1);
					}
				}
				else if (!currentLocation.isTilePassable(nextPosition(1), viewport) || !should_destroy_underfoot_objects)
				{
					Halt();
				}
				else if (should_destroy_underfoot_objects)
				{
					new Vector2(getStandingX() / 64 + 1, getStandingY() / 64);
					if (currentLocation.characterDestroyObjectWithinRectangle(nextPosition(1), showDestroyedObject: true))
					{
						doEmote(12);
						position.X += speed + addedSpeed;
					}
					else
					{
						blockedInterval += time.ElapsedGameTime.Milliseconds;
					}
				}
			}
			else if (moveDown)
			{
				if (currentLocation == null || !currentLocation.isCollidingPosition(nextPosition(2), viewport, isFarmer: false, 0, glider: false, this) || isCharging)
				{
					position.Y += speed + addedSpeed;
					if (!ignoreMovementAnimation)
					{
						Sprite.AnimateDown(time, (speed - 2 + addedSpeed) * -25, Utility.isOnScreen(getTileLocationPoint(), 1, currentLocation) ? "Cowboy_Footstep" : "");
						faceDirection(2);
					}
				}
				else if (!currentLocation.isTilePassable(nextPosition(2), viewport) || !should_destroy_underfoot_objects)
				{
					Halt();
				}
				else if (should_destroy_underfoot_objects)
				{
					new Vector2(getStandingX() / 64, getStandingY() / 64 + 1);
					if (currentLocation.characterDestroyObjectWithinRectangle(nextPosition(2), showDestroyedObject: true))
					{
						doEmote(12);
						position.Y += speed + addedSpeed;
					}
					else
					{
						blockedInterval += time.ElapsedGameTime.Milliseconds;
					}
				}
			}
			else if (moveLeft)
			{
				if (currentLocation == null || !currentLocation.isCollidingPosition(nextPosition(3), viewport, isFarmer: false, 0, glider: false, this) || isCharging)
				{
					position.X -= speed + addedSpeed;
					if (!ignoreMovementAnimation)
					{
						Sprite.AnimateLeft(time, (speed - 2 + addedSpeed) * -25, Utility.isOnScreen(getTileLocationPoint(), 1, currentLocation) ? "Cowboy_Footstep" : "");
						faceDirection(3);
					}
				}
				else if (!currentLocation.isTilePassable(nextPosition(3), viewport) || !should_destroy_underfoot_objects)
				{
					Halt();
				}
				else if (should_destroy_underfoot_objects)
				{
					new Vector2(getStandingX() / 64 - 1, getStandingY() / 64);
					if (currentLocation.characterDestroyObjectWithinRectangle(nextPosition(3), showDestroyedObject: true))
					{
						doEmote(12);
						position.X -= speed + addedSpeed;
					}
					else
					{
						blockedInterval += time.ElapsedGameTime.Milliseconds;
					}
				}
			}
			else
			{
				Sprite.animateOnce(time);
			}
			if (should_destroy_underfoot_objects && currentLocation != null && isMoving())
			{
				Point standing = getStandingXY();
				Vector2 point = new Vector2(standing.X / 64, standing.Y / 64);
				currentLocation.characterTrampleTile(point);
			}
			if (blockedInterval >= 3000 && (float)blockedInterval <= 3750f && !Game1.eventUp)
			{
				doEmote((Game1.random.NextDouble() < 0.5) ? 8 : 40);
				blockedInterval = 3750;
			}
			else if (blockedInterval >= 5000)
			{
				speed = 4;
				isCharging = true;
				blockedInterval = 0;
			}
		}

		public virtual bool canPassThroughActionTiles()
		{
			return false;
		}

		public virtual Microsoft.Xna.Framework.Rectangle nextPosition(int direction)
		{
			Microsoft.Xna.Framework.Rectangle nextPosition = GetBoundingBox();
			switch (direction)
			{
			case 0:
				nextPosition.Y -= speed + addedSpeed;
				break;
			case 1:
				nextPosition.X += speed + addedSpeed;
				break;
			case 2:
				nextPosition.Y += speed + addedSpeed;
				break;
			case 3:
				nextPosition.X -= speed + addedSpeed;
				break;
			}
			return nextPosition;
		}

		public Location nextPositionPoint()
		{
			Location nextPositionTile = default(Location);
			switch (getDirection())
			{
			case 0:
				nextPositionTile = new Location(getStandingX(), getStandingY() - 64);
				break;
			case 1:
				nextPositionTile = new Location(getStandingX() + 64, getStandingY());
				break;
			case 2:
				nextPositionTile = new Location(getStandingX(), getStandingY() + 64);
				break;
			case 3:
				nextPositionTile = new Location(getStandingX() - 64, getStandingY());
				break;
			}
			return nextPositionTile;
		}

		public int getHorizontalMovement()
		{
			if (!moveRight)
			{
				if (!moveLeft)
				{
					return 0;
				}
				return -speed - addedSpeed;
			}
			return speed + addedSpeed;
		}

		public int getVerticalMovement()
		{
			if (!moveDown)
			{
				if (!moveUp)
				{
					return 0;
				}
				return -speed - addedSpeed;
			}
			return speed + addedSpeed;
		}

		public Vector2 nextPositionVector2()
		{
			return new Vector2(getStandingX() + getHorizontalMovement(), getStandingY() + getVerticalMovement());
		}

		public Location nextPositionTile()
		{
			Location nextPositionTile = nextPositionPoint();
			nextPositionTile.X /= 64;
			nextPositionTile.Y /= 64;
			return nextPositionTile;
		}

		public virtual void doEmote(int whichEmote, bool playSound, bool nextEventCommand = true)
		{
			if (!isEmoting && (!Game1.eventUp || this is Farmer || (Game1.currentLocation.currentEvent != null && Game1.currentLocation.currentEvent.actors.Contains(this))))
			{
				isEmoting = true;
				currentEmote = whichEmote;
				currentEmoteFrame = 0;
				emoteInterval = 0f;
				nextEventcommandAfterEmote = nextEventCommand;
			}
		}

		public void doEmote(int whichEmote, bool nextEventCommand = true)
		{
			doEmote(whichEmote, playSound: true, nextEventCommand);
		}

		public void updateEmote(GameTime time)
		{
			if (!isEmoting)
			{
				return;
			}
			emoteInterval += time.ElapsedGameTime.Milliseconds;
			if (emoteFading && emoteInterval > 20f)
			{
				emoteInterval = 0f;
				currentEmoteFrame--;
				if (currentEmoteFrame < 0)
				{
					emoteFading = false;
					isEmoting = false;
					if (nextEventcommandAfterEmote && Game1.currentLocation.currentEvent != null && (Game1.currentLocation.currentEvent.actors.Contains(this) || Game1.currentLocation.currentEvent.farmerActors.Contains(this) || Name.Equals(Game1.player.Name)))
					{
						Game1.currentLocation.currentEvent.CurrentCommand++;
					}
				}
			}
			else if (!emoteFading && emoteInterval > 20f && currentEmoteFrame <= 3)
			{
				emoteInterval = 0f;
				currentEmoteFrame++;
				if (currentEmoteFrame == 4)
				{
					currentEmoteFrame = currentEmote;
				}
			}
			else if (!emoteFading && emoteInterval > 250f)
			{
				emoteInterval = 0f;
				currentEmoteFrame++;
				if (currentEmoteFrame >= currentEmote + 4)
				{
					emoteFading = true;
					currentEmoteFrame = 3;
				}
			}
		}

		public Vector2 GetGrabTile()
		{
			Microsoft.Xna.Framework.Rectangle boundingBox = GetBoundingBox();
			return FacingDirection switch
			{
				0 => new Vector2((boundingBox.X + boundingBox.Width / 2) / 64, (boundingBox.Y - 5) / 64), 
				1 => new Vector2((boundingBox.X + boundingBox.Width + 5) / 64, (boundingBox.Y + boundingBox.Height / 2) / 64), 
				2 => new Vector2((boundingBox.X + boundingBox.Width / 2) / 64, (boundingBox.Y + boundingBox.Height + 5) / 64), 
				3 => new Vector2((boundingBox.X - 5) / 64, (boundingBox.Y + boundingBox.Height / 2) / 64), 
				_ => getStandingPosition(), 
			};
		}

		public Vector2 GetDropLocation()
		{
			Microsoft.Xna.Framework.Rectangle boundingBox = GetBoundingBox();
			return FacingDirection switch
			{
				0 => new Vector2(boundingBox.X + 16, boundingBox.Y - 64), 
				1 => new Vector2(boundingBox.X + boundingBox.Width + 64, boundingBox.Y + 16), 
				2 => new Vector2(boundingBox.X + 16, boundingBox.Y + boundingBox.Height + 64), 
				3 => new Vector2(boundingBox.X - 64, boundingBox.Y + 16), 
				_ => getStandingPosition(), 
			};
		}

		public virtual Vector2 GetToolLocation(Vector2 target_position, bool ignoreClick = false)
		{
			int direction = FacingDirection;
			if ((Game1.player.CurrentTool == null || !Game1.player.CurrentTool.CanUseOnStandingTile()) && (int)(target_position.X / 64f) == Game1.player.getTileX() && (int)(target_position.Y / 64f) == Game1.player.getTileY())
			{
				Microsoft.Xna.Framework.Rectangle bb = GetBoundingBox();
				switch (FacingDirection)
				{
				case 0:
					return new Vector2(bb.X + bb.Width / 2, bb.Y - 64);
				case 1:
					return new Vector2(bb.X + bb.Width + 64, bb.Y + bb.Height / 2);
				case 2:
					return new Vector2(bb.X + bb.Width / 2, bb.Y + bb.Height + 64);
				case 3:
					return new Vector2(bb.X - 64, bb.Y + bb.Height / 2);
				}
			}
			if (!ignoreClick && !target_position.Equals(Vector2.Zero) && Name.Equals(Game1.player.Name))
			{
				bool allow_clicking_on_same_tile = false;
				if (Game1.player.CurrentTool != null && Game1.player.CurrentTool.CanUseOnStandingTile())
				{
					allow_clicking_on_same_tile = true;
				}
				if (Utility.withinRadiusOfPlayer((int)target_position.X, (int)target_position.Y, 1, Game1.player))
				{
					direction = Game1.player.getGeneralDirectionTowards(new Vector2((int)target_position.X, (int)target_position.Y));
					if (allow_clicking_on_same_tile || Math.Abs(target_position.X - (float)Game1.player.getStandingX()) >= 32f || Math.Abs(target_position.Y - (float)Game1.player.getStandingY()) >= 32f)
					{
						return target_position;
					}
				}
			}
			Microsoft.Xna.Framework.Rectangle boundingBox = GetBoundingBox();
			if (Game1.player.CurrentTool != null && Game1.player.CurrentTool.Name.Equals("Fishing Rod"))
			{
				switch (direction)
				{
				case 0:
					return new Vector2(boundingBox.X - 16, boundingBox.Y - 102);
				case 1:
					return new Vector2(boundingBox.X + boundingBox.Width + 64, boundingBox.Y);
				case 2:
					return new Vector2(boundingBox.X - 16, boundingBox.Y + boundingBox.Height + 64);
				case 3:
					return new Vector2(boundingBox.X - 112, boundingBox.Y);
				}
			}
			else
			{
				switch (direction)
				{
				case 0:
					return new Vector2(boundingBox.X + boundingBox.Width / 2, boundingBox.Y - 48);
				case 1:
					return new Vector2(boundingBox.X + boundingBox.Width + 48, boundingBox.Y + boundingBox.Height / 2);
				case 2:
					return new Vector2(boundingBox.X + boundingBox.Width / 2, boundingBox.Y + boundingBox.Height + 48);
				case 3:
					return new Vector2(boundingBox.X - 48, boundingBox.Y + boundingBox.Height / 2);
				}
			}
			return new Vector2(getStandingX(), getStandingY());
		}

		public virtual Vector2 GetToolLocation(bool ignoreClick = false)
		{
			if (!Game1.wasMouseVisibleThisFrame || Game1.isAnyGamePadButtonBeingHeld())
			{
				ignoreClick = true;
			}
			return GetToolLocation(lastClick, ignoreClick);
		}

		public int getGeneralDirectionTowards(Vector2 target, int yBias = 0, bool opposite = false, bool useTileCalculations = true)
		{
			int multiplier = ((!opposite) ? 1 : (-1));
			int xDif;
			int yDif;
			if (useTileCalculations)
			{
				int playerX = getTileX();
				int playerY = getTileY();
				xDif = ((int)(target.X / 64f) - playerX) * multiplier;
				yDif = ((int)(target.Y / 64f) - playerY) * multiplier;
				if (xDif == 0 && yDif == 0)
				{
					Vector2 vector = new Vector2(((float)(int)(target.X / 64f) + 0.5f) * 64f, ((float)(int)(target.Y / 64f) + 0.5f) * 64f);
					xDif = (int)(vector.X - (float)getStandingX()) * multiplier;
					yDif = (int)(vector.Y - (float)getStandingY()) * multiplier;
					yBias *= 64;
				}
			}
			else
			{
				int playerX = getStandingX();
				int playerY = getStandingY();
				xDif = (int)(target.X - (float)playerX) * multiplier;
				yDif = (int)(target.Y - (float)playerY) * multiplier;
			}
			if (xDif > Math.Abs(yDif) + yBias)
			{
				return 1;
			}
			if (Math.Abs(xDif) > Math.Abs(yDif) + yBias)
			{
				return 3;
			}
			if (yDif > 0 || ((float)getStandingY() - target.Y) * (float)multiplier < 0f)
			{
				return 2;
			}
			return 0;
		}

		public void faceGeneralDirection(Vector2 target, int yBias, bool opposite, bool useTileCalculations)
		{
			faceDirection(getGeneralDirectionTowards(target, yBias, opposite, useTileCalculations));
		}

		public void faceGeneralDirection(Vector2 target, int yBias = 0, bool opposite = false)
		{
			faceGeneralDirection(target, yBias, opposite, useTileCalculations: true);
		}

		public virtual void draw(SpriteBatch b)
		{
			draw(b, 1f);
		}

		public virtual void drawAboveAlwaysFrontLayer(SpriteBatch b)
		{
		}

		public virtual void draw(SpriteBatch b, float alpha = 1f)
		{
			Vector2 draw_position = Position;
			Sprite.draw(b, Game1.GlobalToLocal(Game1.viewport, draw_position), (float)GetBoundingBox().Center.Y / 10000f);
			if (IsEmoting)
			{
				Vector2 emotePosition = getLocalPosition(Game1.viewport);
				emotePosition.Y -= 96f;
				b.Draw(Game1.emoteSpriteSheet, emotePosition, new Microsoft.Xna.Framework.Rectangle(CurrentEmoteIndex * 16 % Game1.emoteSpriteSheet.Width, CurrentEmoteIndex * 16 / Game1.emoteSpriteSheet.Width * 16, 16, 16), Color.White * alpha, 0f, Vector2.Zero, 4f, SpriteEffects.None, (float)getStandingY() / 10000f);
			}
		}

		public virtual void draw(SpriteBatch b, int ySourceRectOffset, float alpha = 1f)
		{
			Microsoft.Xna.Framework.Rectangle box = GetBoundingBox();
			Sprite.draw(b, Game1.GlobalToLocal(Game1.viewport, position) + new Vector2(GetSpriteWidthForPositioning() * 4 / 2, box.Height / 2), (float)box.Center.Y / 10000f, 0, ySourceRectOffset, Color.White, flip: false, 4f, 0f, characterSourceRectOffset: true);
			if (IsEmoting)
			{
				Vector2 emotePosition = getLocalPosition(Game1.viewport);
				emotePosition.Y -= 96f;
				b.Draw(Game1.emoteSpriteSheet, emotePosition, new Microsoft.Xna.Framework.Rectangle(CurrentEmoteIndex * 16 % Game1.emoteSpriteSheet.Width, CurrentEmoteIndex * 16 / Game1.emoteSpriteSheet.Width * 16, 16, 16), Color.White * alpha, 0f, Vector2.Zero, 4f, SpriteEffects.None, (float)getStandingY() / 10000f);
			}
		}

		public virtual int GetSpriteWidthForPositioning()
		{
			if (forceOneTileWide.Value)
			{
				return 16;
			}
			return Sprite.SpriteWidth;
		}

		public virtual Microsoft.Xna.Framework.Rectangle GetBoundingBox()
		{
			if (Sprite == null)
			{
				return Microsoft.Xna.Framework.Rectangle.Empty;
			}
			Vector2 position = Position;
			int width = GetSpriteWidthForPositioning() * 4 * 3 / 4;
			return new Microsoft.Xna.Framework.Rectangle((int)position.X + 8, (int)position.Y + 16, width, 32);
		}

		public void stopWithoutChangingFrame()
		{
			moveDown = false;
			moveLeft = false;
			moveRight = false;
			moveUp = false;
		}

		public virtual void collisionWithFarmerBehavior()
		{
		}

		public int getStandingX()
		{
			Microsoft.Xna.Framework.Rectangle box = GetBoundingBox();
			return box.X + box.Width / 2;
		}

		public int getStandingY()
		{
			Microsoft.Xna.Framework.Rectangle box = GetBoundingBox();
			return box.Y + box.Height / 2;
		}

		public Vector2 getStandingPosition()
		{
			Point center = GetBoundingBox().Center;
			return new Vector2(center.X, center.Y);
		}

		public Point getStandingXY()
		{
			return GetBoundingBox().Center;
		}

		public Vector2 getLocalPosition(xTile.Dimensions.Rectangle viewport)
		{
			Vector2 position = Position;
			return new Vector2(position.X - (float)viewport.X, position.Y - (float)viewport.Y + (float)yJumpOffset) + drawOffset;
		}

		public virtual bool isMoving()
		{
			if (!moveUp && !moveDown && !moveRight && !moveLeft)
			{
				return position.Field.IsInterpolating();
			}
			return true;
		}

		public Point getTileLocationPoint()
		{
			Point standing = getStandingXY();
			return new Point(standing.X / 64, standing.Y / 64);
		}

		public int getTileX()
		{
			return getStandingX() / 64;
		}

		public int getTileY()
		{
			return getStandingY() / 64;
		}

		public Vector2 getTileLocation()
		{
			Point position = getStandingXY();
			return new Vector2(position.X / 64, position.Y / 64);
		}

		public void setTileLocation(Vector2 tileLocation)
		{
			float standingX = (tileLocation.X + 0.5f) * 64f;
			float standingY = (tileLocation.Y + 0.5f) * 64f;
			Vector2 pos = Position;
			Microsoft.Xna.Framework.Rectangle box = GetBoundingBox();
			pos.X += standingX - (float)box.Center.X;
			pos.Y += standingY - (float)box.Center.Y;
			Position = pos;
		}

		public void startGlowing(Color glowingColor, bool border, float glowRate)
		{
			if (!this.glowingColor.Equals(glowingColor))
			{
				isGlowing = true;
				coloredBorder = border;
				this.glowingColor = glowingColor;
				glowUp = true;
				this.glowRate = glowRate;
				glowingTransparency = 0f;
			}
		}

		public void stopGlowing()
		{
			isGlowing = false;
			glowingColor = Color.White;
		}

		public virtual void jumpWithoutSound(float velocity = 8f)
		{
			yJumpVelocity = velocity;
			yJumpOffset = -1;
			yJumpGravity = -0.5f;
		}

		public virtual void jump()
		{
			yJumpVelocity = 8f;
			yJumpOffset = -1;
			yJumpGravity = -0.5f;
			wasJumpWithSound = true;
			currentLocation.localSound("dwop");
		}

		public virtual void jump(float jumpVelocity)
		{
			yJumpVelocity = jumpVelocity;
			yJumpOffset = -1;
			yJumpGravity = -0.5f;
			wasJumpWithSound = true;
			currentLocation.localSound("dwop");
		}

		public void faceTowardFarmerForPeriod(int milliseconds, int radius, bool faceAway, Farmer who)
		{
			if ((Sprite != null && Sprite.CurrentAnimation == null) || isMoving())
			{
				if (isMoving())
				{
					milliseconds /= 2;
				}
				faceTowardFarmerEvent.Fire(milliseconds);
				faceTowardFarmerEvent.Poll();
				if ((int)facingDirectionBeforeSpeakingToPlayer == -1)
				{
					facingDirectionBeforeSpeakingToPlayer.Value = FacingDirection;
				}
				faceTowardFarmerRadius.Value = radius;
				faceAwayFromFarmer.Value = faceAway;
				whoToFace.Value = who;
				hasJustStartedFacingPlayer = true;
			}
		}

		private void performFaceTowardFarmerEvent(int milliseconds)
		{
			if ((Sprite != null && Sprite.CurrentAnimation == null) || isMoving())
			{
				Halt();
				faceTowardFarmerTimer = milliseconds;
				movementPause = milliseconds;
			}
		}

		public virtual void update(GameTime time, GameLocation location)
		{
			position.UpdateExtrapolation(speed + addedSpeed);
			update(time, location, 0L, move: true);
		}

		public virtual void performBehavior(byte which)
		{
		}

		public virtual void checkForFootstep()
		{
			Game1.currentLocation.playTerrainSound(getTileLocation(), this);
		}

		public virtual void update(GameTime time, GameLocation location, long id, bool move)
		{
			position.UpdateExtrapolation(speed + addedSpeed);
			currentLocation = location;
			faceTowardFarmerEvent.Poll();
			if (yJumpOffset != 0)
			{
				yJumpVelocity += yJumpGravity;
				yJumpOffset -= (int)yJumpVelocity;
				if (yJumpOffset >= 0)
				{
					yJumpOffset = 0;
					yJumpVelocity = 0f;
					if (!IsMonster && (location == null || location.Equals(Game1.currentLocation)) && wasJumpWithSound)
					{
						checkForFootstep();
					}
				}
			}
			if (forceUpdateTimer > 0)
			{
				forceUpdateTimer -= time.ElapsedGameTime.Milliseconds;
			}
			updateGlow();
			updateEmote(time);
			updateFaceTowardsFarmer(time, location);
			bool is_event_controlled_character = false;
			if (location.currentEvent != null)
			{
				if (location.isTemp())
				{
					is_event_controlled_character = true;
				}
				else if (location.currentEvent.actors.Contains(this))
				{
					is_event_controlled_character = true;
				}
			}
			if (Game1.IsMasterGame || is_event_controlled_character)
			{
				if (controller == null && move && !freezeMotion)
				{
					updateMovement(location, time);
				}
				if (controller != null && !freezeMotion && controller.update(time))
				{
					controller = null;
				}
			}
			else
			{
				updateSlaveAnimation(time);
			}
			hasJustStartedFacingPlayer = false;
		}

		public virtual void updateFaceTowardsFarmer(GameTime time, GameLocation location)
		{
			if (faceTowardFarmerTimer > 0)
			{
				faceTowardFarmerTimer -= time.ElapsedGameTime.Milliseconds;
				if (whoToFace.Value != null)
				{
					if (!faceTowardFarmer && faceTowardFarmerTimer > 0 && Utility.tileWithinRadiusOfPlayer((int)getTileLocation().X, (int)getTileLocation().Y, faceTowardFarmerRadius, whoToFace))
					{
						faceTowardFarmer = true;
					}
					else if (!Utility.tileWithinRadiusOfPlayer((int)getTileLocation().X, (int)getTileLocation().Y, faceTowardFarmerRadius, whoToFace) || faceTowardFarmerTimer <= 0)
					{
						faceDirection(facingDirectionBeforeSpeakingToPlayer.Value);
						if (faceTowardFarmerTimer <= 0)
						{
							facingDirectionBeforeSpeakingToPlayer.Value = -1;
							faceTowardFarmer = false;
							faceAwayFromFarmer.Value = false;
							faceTowardFarmerTimer = 0;
						}
					}
				}
			}
			if ((Game1.IsMasterGame || location.currentEvent != null) && faceTowardFarmer && whoToFace.Value != null)
			{
				faceGeneralDirection(whoToFace.Value.getStandingPosition(), 0, opposite: false, useTileCalculations: true);
				if ((bool)faceAwayFromFarmer)
				{
					faceDirection((FacingDirection + 2) % 4);
				}
			}
			hasJustStartedFacingPlayer = false;
		}

		public virtual bool hasSpecialCollisionRules()
		{
			return false;
		}

		/// <summary>
		///
		/// make sure that you also override hasSpecialCollisionRules() in any class that overrides isColliding().
		/// Otherwise isColliding() will never be called.
		/// dumb I kno
		/// </summary>
		/// <param name="l"></param>
		/// <param name="tile"></param>
		/// <returns></returns>
		public virtual bool isColliding(GameLocation l, Vector2 tile)
		{
			return false;
		}

		public virtual void animateInFacingDirection(GameTime time)
		{
			switch (FacingDirection)
			{
			case 0:
				Sprite.AnimateUp(time);
				break;
			case 1:
				Sprite.AnimateRight(time);
				break;
			case 2:
				Sprite.AnimateDown(time);
				break;
			case 3:
				Sprite.AnimateLeft(time);
				break;
			}
		}

		public virtual void updateMovement(GameLocation location, GameTime time)
		{
		}

		protected virtual void updateSlaveAnimation(GameTime time)
		{
			if (Sprite.CurrentAnimation != null)
			{
				Sprite.animateOnce(time);
				return;
			}
			faceDirection(FacingDirection);
			if (isMoving())
			{
				animateInFacingDirection(time);
			}
			else
			{
				Sprite.StopAnimation();
			}
		}

		public void updateGlow()
		{
			if (!isGlowing)
			{
				return;
			}
			if (glowUp)
			{
				glowingTransparency += glowRate;
				if (glowingTransparency >= 1f)
				{
					glowingTransparency = 1f;
					glowUp = false;
				}
			}
			else
			{
				glowingTransparency -= glowRate;
				if (glowingTransparency <= 0f)
				{
					glowingTransparency = 0f;
					glowUp = true;
				}
			}
		}

		public void convertEventMotionCommandToMovement(Vector2 command)
		{
			if (command.X < 0f)
			{
				SetMovingLeft(b: true);
			}
			else if (command.X > 0f)
			{
				SetMovingRight(b: true);
			}
			else if (command.Y < 0f)
			{
				SetMovingUp(b: true);
			}
			else if (command.Y > 0f)
			{
				SetMovingDown(b: true);
			}
		}
	}
}
