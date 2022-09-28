﻿// Decompiled with JetBrains decompiler
// Type: StardewValley.Minigames.AbigailGame
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewValley.BellsAndWhistles;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StardewValley.Minigames
{
  internal class AbigailGame : IMinigame
  {
    public static List<AbigailGame.CowboyMonster> monsters = new List<AbigailGame.CowboyMonster>();
    public static Vector2 player2Position = new Vector2();
    public static List<int> playerMovementDirections = new List<int>();
    public static List<int> playerShootingDirections = new List<int>();
    public static List<AbigailGame.CowboyBullet> enemyBullets = new List<AbigailGame.CowboyBullet>();
    public static int[,] map = new int[16, 16];
    public static int[,] nextMap = new int[16, 16];
    public static List<AbigailGame.CowboyPowerup> powerups = new List<AbigailGame.CowboyPowerup>();
    public static List<TemporaryAnimatedSprite> temporarySprites = new List<TemporaryAnimatedSprite>();
    public static int world = 0;
    public static int waveTimer = 80000;
    public static int betweenWaveTimer = 5000;
    public int lootDuration = 7500;
    public int powerupDuration = 10000;
    public List<int> player2MovementDirections = new List<int>();
    public List<int> player2ShootingDirections = new List<int>();
    public int shootingDelay = 300;
    public int lives = 3;
    public List<AbigailGame.CowboyBullet> bullets = new List<AbigailGame.CowboyBullet>();
    public List<Point>[] spawnQueue = new List<Point>[4];
    public float playerFootstepSoundTimer = 200f;
    public List<Vector2> monsterChances = new List<Vector2>()
    {
      new Vector2(0.014f, 0.4f),
      Vector2.Zero,
      Vector2.Zero,
      Vector2.Zero,
      Vector2.Zero,
      Vector2.Zero,
      Vector2.Zero
    };
    public Dictionary<int, int> activePowerups = new Dictionary<int, int>();
    public string AbigailDialogue = "";
    private Dictionary<Rectangle, int> storeItems = new Dictionary<Rectangle, int>();
    public const int mapWidth = 16;
    public const int mapHeight = 16;
    public const int pixelZoom = 3;
    public const int bulletSpeed = 8;
    public const double lootChance = 0.05;
    public const double coinChance = 0.05;
    private const int abigailPortraitDuration = 6000;
    public const float playerSpeed = 3f;
    public const int baseTileSize = 16;
    public const int orcSpeed = 2;
    public const int ogreSpeed = 1;
    public const int ghostSpeed = 3;
    public const int spikeySpeed = 3;
    public const int orcHealth = 1;
    public const int ghostHealth = 1;
    public const int ogreHealth = 3;
    public const int spikeyHealth = 2;
    public const int cactusDanceDelay = 800;
    public const int playerMotionDelay = 100;
    public const int playerFootStepDelay = 200;
    public const int deathDelay = 3000;
    public const int MAP_BARRIER1 = 0;
    public const int MAP_BARRIER2 = 1;
    public const int MAP_ROCKY1 = 2;
    public const int MAP_DESERT = 3;
    public const int MAP_GRASSY = 4;
    public const int MAP_CACTUS = 5;
    public const int MAP_FENCE = 7;
    public const int MAP_TRENCH1 = 8;
    public const int MAP_TRENCH2 = 9;
    public const int MAP_BRIDGE = 10;
    public const int orc = 0;
    public const int ghost = 1;
    public const int ogre = 2;
    public const int mummy = 3;
    public const int devil = 4;
    public const int mushroom = 5;
    public const int spikey = 6;
    public const int dracula = 7;
    public const int desert = 0;
    public const int woods = 2;
    public const int graveyard = 1;
    public const int POWERUP_LOG = -1;
    public const int POWERUP_SKULL = -2;
    public const int coin1 = 0;
    public const int coin5 = 1;
    public const int POWERUP_SPREAD = 2;
    public const int POWERUP_RAPIDFIRE = 3;
    public const int POWERUP_NUKE = 4;
    public const int POWERUP_ZOMBIE = 5;
    public const int POWERUP_SPEED = 6;
    public const int POWERUP_SHOTGUN = 7;
    public const int POWERUP_LIFE = 8;
    public const int POWERUP_TELEPORT = 9;
    public const int POWERUP_SHERRIFF = 10;
    public const int POWERUP_HEART = -3;
    public const int ITEM_FIRESPEED1 = 0;
    public const int ITEM_FIRESPEED2 = 1;
    public const int ITEM_FIRESPEED3 = 2;
    public const int ITEM_RUNSPEED1 = 3;
    public const int ITEM_RUNSPEED2 = 4;
    public const int ITEM_LIFE = 5;
    public const int ITEM_AMMO1 = 6;
    public const int ITEM_AMMO2 = 7;
    public const int ITEM_AMMO3 = 8;
    public const int ITEM_SPREADPISTOL = 9;
    public const int ITEM_STAR = 10;
    public const int ITEM_SKULL = 11;
    public const int ITEM_LOG = 12;
    public const int option_retry = 0;
    public const int option_quit = 1;
    public int runSpeedLevel;
    public int fireSpeedLevel;
    public int ammoLevel;
    public int whichRound;
    public bool spreadPistol;
    public const int waveDuration = 80000;
    public const int betweenWaveDuration = 5000;
    public Vector2 playerPosition;
    public Rectangle playerBoundingBox;
    public Rectangle merchantBox;
    public Rectangle player2BoundingBox;
    public Rectangle noPickUpBox;
    public int shotTimer;
    public int motionPause;
    public int bulletDamage;
    public int speedBonus;
    public int fireRateBonus;
    public int coins;
    public int score;
    public int player2deathtimer;
    public int player2invincibletimer;
    public static Vector2 topLeftScreenCoordinate;
    public float cactusDanceTimer;
    public float playerMotionAnimationTimer;
    public AbigailGame.behaviorAfterMotionPause behaviorAfterPause;
    public Rectangle shoppingCarpetNoPickup;
    public AbigailGame.CowboyPowerup heldItem;
    public int gameOverOption;
    public int gamerestartTimer;
    public int player2TargetUpdateTimer;
    public int player2shotTimer;
    public int player2AnimationTimer;
    public int fadethenQuitTimer;
    public int abigailPortraitYposition;
    public int abigailPortraitTimer;
    public int abigailPortraitExpression;
    public static int whichWave;
    public static int monsterConfusionTimer;
    public static int zombieModeTimer;
    public static int shoppingTimer;
    public static int holdItemTimer;
    public static int itemToHold;
    public static int newMapPosition;
    public static int playerInvincibleTimer;
    public static int screenFlash;
    public static int gopherTrainPosition;
    public static int endCutsceneTimer;
    public static int endCutscenePhase;
    public static int startTimer;
    public static float deathTimer;
    public static bool onStartMenu;
    public static bool shopping;
    public static bool gopherRunning;
    public static bool store;
    public static bool merchantLeaving;
    public static bool merchantArriving;
    public static bool merchantShopOpen;
    public static bool waitingForPlayerToMoveDownAMap;
    public static bool scrollingMap;
    public static bool hasGopherAppeared;
    public static bool shootoutLevel;
    public static bool gopherTrain;
    public static bool playerJumped;
    public static bool endCutscene;
    public static bool gameOver;
    public static bool playingWithAbigail;
    public static bool beatLevelWithAbigail;
    private bool quit;
    private bool died;
    public static Rectangle gopherBox;
    public Point gopherMotion;
    private static Cue overworldSong;
    private static Cue outlawSong;
    private int player2FootstepSoundTimer;
    private AbigailGame.CowboyMonster targetMonster;

    public static int TileSize
    {
      get
      {
        return 48;
      }
    }

    public AbigailGame(bool playingWithAbby = false)
    {
      this.reset(playingWithAbby);
    }

    public AbigailGame(int coins, int ammoLevel, int bulletDamage, int fireSpeedLevel, int runSpeedLevel, int lives, bool spreadPistol, int whichRound)
    {
      this.reset(false);
      this.coins = coins;
      this.ammoLevel = ammoLevel;
      this.bulletDamage = bulletDamage;
      this.fireSpeedLevel = fireSpeedLevel;
      this.runSpeedLevel = runSpeedLevel;
      this.lives = lives;
      this.spreadPistol = spreadPistol;
      this.whichRound = whichRound;
      this.monsterChances[0] = new Vector2((float) (0.0140000004321337 + (double) whichRound * 0.00499999988824129), (float) (0.409999996423721 + (double) whichRound * 0.0500000007450581));
      this.monsterChances[4] = new Vector2(1f / 500f, 0.1f);
      AbigailGame.onStartMenu = false;
    }

    public void reset(bool playingWithAbby)
    {
      this.died = false;
      AbigailGame.topLeftScreenCoordinate = new Vector2((float) (Game1.viewport.Width / 2 - 384), (float) (Game1.viewport.Height / 2 - 384));
      AbigailGame.enemyBullets.Clear();
      AbigailGame.holdItemTimer = 0;
      AbigailGame.itemToHold = -1;
      AbigailGame.merchantArriving = false;
      AbigailGame.merchantLeaving = false;
      AbigailGame.merchantShopOpen = false;
      AbigailGame.monsterConfusionTimer = 0;
      AbigailGame.monsters.Clear();
      AbigailGame.newMapPosition = 16 * AbigailGame.TileSize;
      AbigailGame.scrollingMap = false;
      AbigailGame.shopping = false;
      AbigailGame.store = false;
      AbigailGame.temporarySprites.Clear();
      AbigailGame.waitingForPlayerToMoveDownAMap = false;
      AbigailGame.waveTimer = 80000;
      AbigailGame.whichWave = 0;
      AbigailGame.zombieModeTimer = 0;
      this.bulletDamage = 1;
      AbigailGame.deathTimer = 0.0f;
      AbigailGame.shootoutLevel = false;
      AbigailGame.betweenWaveTimer = 5000;
      AbigailGame.gopherRunning = false;
      AbigailGame.hasGopherAppeared = false;
      AbigailGame.playerMovementDirections.Clear();
      AbigailGame.outlawSong = (Cue) null;
      AbigailGame.overworldSong = (Cue) null;
      AbigailGame.endCutscene = false;
      AbigailGame.endCutscenePhase = 0;
      AbigailGame.endCutsceneTimer = 0;
      AbigailGame.gameOver = false;
      AbigailGame.deathTimer = 0.0f;
      AbigailGame.playerInvincibleTimer = 0;
      AbigailGame.playingWithAbigail = playingWithAbby;
      AbigailGame.beatLevelWithAbigail = false;
      AbigailGame.onStartMenu = true;
      AbigailGame.startTimer = 0;
      AbigailGame.powerups.Clear();
      AbigailGame.world = 0;
      Game1.changeMusicTrack("none");
      for (int index1 = 0; index1 < 16; ++index1)
      {
        for (int index2 = 0; index2 < 16; ++index2)
          AbigailGame.map[index1, index2] = index1 != 0 && index1 != 15 && (index2 != 0 && index2 != 15) || (index1 > 6 && index1 < 10 || index2 > 6 && index2 < 10) ? (index1 == 0 || index1 == 15 || (index2 == 0 || index2 == 15) ? (Game1.random.NextDouble() < 0.15 ? 1 : 0) : (index1 == 1 || index1 == 14 || (index2 == 1 || index2 == 14) ? 2 : (Game1.random.NextDouble() < 0.1 ? 4 : 3))) : 5;
      }
      this.playerPosition = new Vector2(384f, 384f);
      this.playerBoundingBox.X = (int) this.playerPosition.X + AbigailGame.TileSize / 4;
      this.playerBoundingBox.Y = (int) this.playerPosition.Y + AbigailGame.TileSize / 4;
      this.playerBoundingBox.Width = AbigailGame.TileSize / 2;
      this.playerBoundingBox.Height = AbigailGame.TileSize / 2;
      if (AbigailGame.playingWithAbigail)
      {
        AbigailGame.onStartMenu = false;
        AbigailGame.player2Position = new Vector2(432f, 384f);
        this.player2BoundingBox = new Rectangle(9 * AbigailGame.TileSize, 8 * AbigailGame.TileSize, AbigailGame.TileSize, AbigailGame.TileSize);
        AbigailGame.betweenWaveTimer += 1500;
      }
      for (int index = 0; index < 4; ++index)
        this.spawnQueue[index] = new List<Point>();
      this.noPickUpBox = new Rectangle(0, 0, AbigailGame.TileSize, AbigailGame.TileSize);
      this.merchantBox = new Rectangle(8 * AbigailGame.TileSize, 0, AbigailGame.TileSize, AbigailGame.TileSize);
      AbigailGame.newMapPosition = 16 * AbigailGame.TileSize;
    }

    public float getMovementSpeed(float speed, int directions)
    {
      float num = speed;
      if (directions > 1)
        num = (float) Math.Max(1, (int) Math.Sqrt(2.0 * ((double) num * (double) num)) / 2);
      return num;
    }

    public bool getPowerUp(AbigailGame.CowboyPowerup c)
    {
      switch (c.which)
      {
        case -3:
          this.usePowerup(-3);
          break;
        case -2:
          this.usePowerup(-2);
          break;
        case -1:
          this.usePowerup(-1);
          break;
        case 0:
          this.coins = this.coins + 1;
          Game1.playSound("Pickup_Coin15");
          break;
        case 1:
          this.coins = this.coins + 5;
          Game1.playSound("Pickup_Coin15");
          break;
        case 8:
          this.lives = this.lives + 1;
          Game1.playSound("cowboy_powerup");
          break;
        default:
          if (this.heldItem == null)
          {
            this.heldItem = c;
            Game1.playSound("cowboy_powerup");
            break;
          }
          AbigailGame.CowboyPowerup heldItem = this.heldItem;
          this.heldItem = c;
          this.noPickUpBox.Location = c.position;
          heldItem.position = c.position;
          AbigailGame.powerups.Add(heldItem);
          Game1.playSound("cowboy_powerup");
          return true;
      }
      return true;
    }

    public bool overrideFreeMouseMovement()
    {
      return false;
    }

    public void usePowerup(int which)
    {
      if (this.activePowerups.ContainsKey(which))
      {
        this.activePowerups[which] = this.powerupDuration + 2000;
      }
      else
      {
        switch (which)
        {
          case -3:
            AbigailGame.itemToHold = 13;
            AbigailGame.holdItemTimer = 4000;
            Game1.playSound("Cowboy_Secret");
            AbigailGame.endCutscene = true;
            AbigailGame.endCutsceneTimer = 4000;
            AbigailGame.world = 0;
            if (!Game1.player.hasOrWillReceiveMail("Beat_PK"))
            {
              Game1.addMailForTomorrow("Beat_PK", false, false);
              break;
            }
            break;
          case -2:
          case -1:
            AbigailGame.itemToHold = which == -1 ? 12 : 11;
            AbigailGame.holdItemTimer = 2000;
            Game1.playSound("Cowboy_Secret");
            AbigailGame.gopherTrain = true;
            AbigailGame.gopherTrainPosition = -AbigailGame.TileSize * 2;
            break;
          case 0:
            this.coins = this.coins + 1;
            Game1.playSound("Pickup_Coin15");
            break;
          case 1:
            this.coins = this.coins + 5;
            Game1.playSound("Pickup_Coin15");
            Game1.playSound("Pickup_Coin15");
            break;
          case 2:
          case 3:
          case 7:
            this.shotTimer = 0;
            Game1.playSound("cowboy_gunload");
            this.activePowerups.Add(which, this.powerupDuration + 2000);
            break;
          case 4:
            Game1.playSound("cowboy_explosion");
            if (!AbigailGame.shootoutLevel)
            {
              foreach (AbigailGame.CowboyMonster monster in AbigailGame.monsters)
                AbigailGame.addGuts(monster.position.Location, monster.type);
              AbigailGame.monsters.Clear();
            }
            else
            {
              foreach (AbigailGame.CowboyMonster monster in AbigailGame.monsters)
              {
                monster.takeDamage(30);
                this.bullets.Add(new AbigailGame.CowboyBullet(monster.position.Center, 2, 1));
              }
            }
            for (int index = 0; index < 30; ++index)
              AbigailGame.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(464, 1792, 16, 16), 80f, 5, 0, new Vector2((float) Game1.random.Next(1, 16), (float) Game1.random.Next(1, 16)) * (float) AbigailGame.TileSize + AbigailGame.topLeftScreenCoordinate + new Vector2((float) (AbigailGame.TileSize / 2), (float) (AbigailGame.TileSize / 2)), false, false, 1f, 0.0f, Color.White, 3f, 0.0f, 0.0f, 0.0f, true)
              {
                delayBeforeAnimationStart = Game1.random.Next(800)
              });
            break;
          case 5:
            if (AbigailGame.overworldSong != null && AbigailGame.overworldSong.IsPlaying)
              AbigailGame.overworldSong.Stop(AudioStopOptions.Immediate);
            Game1.playSound("Cowboy_undead");
            this.motionPause = 1800;
            AbigailGame.zombieModeTimer = 10000;
            break;
          case 8:
            this.lives = this.lives + 1;
            Game1.playSound("cowboy_powerup");
            break;
          case 9:
            Point position = Point.Zero;
            while ((double) Math.Abs((float) position.X - this.playerPosition.X) < 8.0 || (double) Math.Abs((float) position.Y - this.playerPosition.Y) < 8.0 || (AbigailGame.isCollidingWithMap(position) || AbigailGame.isCollidingWithMonster(new Rectangle(position.X, position.Y, AbigailGame.TileSize, AbigailGame.TileSize), (AbigailGame.CowboyMonster) null)))
              position = new Point(Game1.random.Next(AbigailGame.TileSize, 16 * AbigailGame.TileSize - AbigailGame.TileSize), Game1.random.Next(AbigailGame.TileSize, 16 * AbigailGame.TileSize - AbigailGame.TileSize));
            AbigailGame.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(464, 1792, 16, 16), 120f, 5, 0, this.playerPosition + AbigailGame.topLeftScreenCoordinate + new Vector2((float) (AbigailGame.TileSize / 2), (float) (AbigailGame.TileSize / 2)), false, false, 1f, 0.0f, Color.White, 3f, 0.0f, 0.0f, 0.0f, true));
            AbigailGame.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(464, 1792, 16, 16), 120f, 5, 0, new Vector2((float) position.X, (float) position.Y) + AbigailGame.topLeftScreenCoordinate + new Vector2((float) (AbigailGame.TileSize / 2), (float) (AbigailGame.TileSize / 2)), false, false, 1f, 0.0f, Color.White, 3f, 0.0f, 0.0f, 0.0f, true));
            AbigailGame.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(464, 1792, 16, 16), 120f, 5, 0, new Vector2((float) (position.X - AbigailGame.TileSize / 2), (float) position.Y) + AbigailGame.topLeftScreenCoordinate + new Vector2((float) (AbigailGame.TileSize / 2), (float) (AbigailGame.TileSize / 2)), false, false, 1f, 0.0f, Color.White, 3f, 0.0f, 0.0f, 0.0f, true)
            {
              delayBeforeAnimationStart = 200
            });
            AbigailGame.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(464, 1792, 16, 16), 120f, 5, 0, new Vector2((float) (position.X + AbigailGame.TileSize / 2), (float) position.Y) + AbigailGame.topLeftScreenCoordinate + new Vector2((float) (AbigailGame.TileSize / 2), (float) (AbigailGame.TileSize / 2)), false, false, 1f, 0.0f, Color.White, 3f, 0.0f, 0.0f, 0.0f, true)
            {
              delayBeforeAnimationStart = 400
            });
            AbigailGame.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(464, 1792, 16, 16), 120f, 5, 0, new Vector2((float) position.X, (float) (position.Y - AbigailGame.TileSize / 2)) + AbigailGame.topLeftScreenCoordinate + new Vector2((float) (AbigailGame.TileSize / 2), (float) (AbigailGame.TileSize / 2)), false, false, 1f, 0.0f, Color.White, 3f, 0.0f, 0.0f, 0.0f, true)
            {
              delayBeforeAnimationStart = 600
            });
            AbigailGame.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(464, 1792, 16, 16), 120f, 5, 0, new Vector2((float) position.X, (float) (position.Y + AbigailGame.TileSize / 2)) + AbigailGame.topLeftScreenCoordinate + new Vector2((float) (AbigailGame.TileSize / 2), (float) (AbigailGame.TileSize / 2)), false, false, 1f, 0.0f, Color.White, 3f, 0.0f, 0.0f, 0.0f, true)
            {
              delayBeforeAnimationStart = 800
            });
            this.playerPosition = new Vector2((float) position.X, (float) position.Y);
            AbigailGame.monsterConfusionTimer = 4000;
            AbigailGame.playerInvincibleTimer = 4000;
            Game1.playSound("cowboy_powerup");
            break;
          case 10:
            this.usePowerup(7);
            this.usePowerup(3);
            this.usePowerup(6);
            for (int index = 0; index < this.activePowerups.Count; ++index)
            {
              Dictionary<int, int> activePowerups = this.activePowerups;
              int key = this.activePowerups.ElementAt<KeyValuePair<int, int>>(index).Key;
              activePowerups[key] = activePowerups[key] * 2;
            }
            break;
          default:
            this.activePowerups.Add(which, this.powerupDuration);
            Game1.playSound("cowboy_powerup");
            break;
        }
        if (this.whichRound <= 0 || !this.activePowerups.ContainsKey(which))
          return;
        Dictionary<int, int> activePowerups1 = this.activePowerups;
        int index1 = which;
        activePowerups1[index1] = activePowerups1[index1] / 2;
      }
    }

    public static void addGuts(Point position, int whichGuts)
    {
      switch (whichGuts)
      {
        case 0:
        case 2:
        case 5:
        case 6:
        case 7:
          AbigailGame.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(512, 1696, 16, 16), 80f, 6, 0, AbigailGame.topLeftScreenCoordinate + new Vector2((float) position.X, (float) position.Y), false, Game1.random.NextDouble() < 0.5, 1f / 1000f, 0.0f, Color.White, 3f, 0.0f, 0.0f, 0.0f, true));
          AbigailGame.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(592, 1696, 16, 16), 10000f, 1, 0, AbigailGame.topLeftScreenCoordinate + new Vector2((float) position.X, (float) position.Y), false, Game1.random.NextDouble() < 0.5, 1f / 1000f, 0.0f, Color.White, 3f, 0.0f, 0.0f, 0.0f, true)
          {
            delayBeforeAnimationStart = 480
          });
          break;
        case 1:
        case 4:
          AbigailGame.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(544, 1728, 16, 16), 80f, 4, 0, AbigailGame.topLeftScreenCoordinate + new Vector2((float) position.X, (float) position.Y), false, Game1.random.NextDouble() < 0.5, 1f / 1000f, 0.0f, Color.White, 3f, 0.0f, 0.0f, 0.0f, true));
          break;
        case 3:
          AbigailGame.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(464, 1792, 16, 16), 80f, 5, 0, AbigailGame.topLeftScreenCoordinate + new Vector2((float) position.X, (float) position.Y), false, Game1.random.NextDouble() < 0.5, 1f / 1000f, 0.0f, Color.White, 3f, 0.0f, 0.0f, 0.0f, true));
          break;
      }
    }

    public void endOfGopherAnimationBehavior2(int extraInfo)
    {
      Game1.playSound("cowboy_gopher");
      this.gopherMotion = Math.Abs(AbigailGame.gopherBox.X - 8 * AbigailGame.TileSize) <= Math.Abs(AbigailGame.gopherBox.Y - 8 * AbigailGame.TileSize) ? (AbigailGame.gopherBox.Y <= 8 * AbigailGame.TileSize ? new Point(0, 2) : new Point(0, -2)) : (AbigailGame.gopherBox.X <= 8 * AbigailGame.TileSize ? new Point(2, 0) : new Point(-2, 0));
      AbigailGame.gopherRunning = true;
    }

    public void endOfGopherAnimationBehavior(int extrainfo)
    {
      AbigailGame.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(384, 1792, 16, 16), 120f, 4, 2, AbigailGame.topLeftScreenCoordinate + new Vector2((float) (AbigailGame.gopherBox.X + AbigailGame.TileSize / 2), (float) (AbigailGame.gopherBox.Y + AbigailGame.TileSize / 2)), false, false, (float) AbigailGame.gopherBox.Y / 10000f, 0.0f, Color.White, 3f, 0.0f, 0.0f, 0.0f, true)
      {
        endFunction = new TemporaryAnimatedSprite.endBehavior(this.endOfGopherAnimationBehavior2)
      });
      Game1.playSound("cowboy_gopher");
    }

    public static void killOutlaw()
    {
      AbigailGame.powerups.Add(new AbigailGame.CowboyPowerup(AbigailGame.world == 0 ? -1 : -2, new Point(8 * AbigailGame.TileSize, 10 * AbigailGame.TileSize), 9999999));
      if (AbigailGame.outlawSong != null && AbigailGame.outlawSong.IsPlaying)
        AbigailGame.outlawSong.Stop(AudioStopOptions.Immediate);
      AbigailGame.map[8, 8] = 10;
      AbigailGame.screenFlash = 200;
      Game1.playSound("Cowboy_monsterDie");
      for (int index = 0; index < 15; ++index)
        AbigailGame.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(464, 1792, 16, 16), 80f, 5, 0, new Vector2((float) (AbigailGame.monsters[0].position.X + Game1.random.Next(-AbigailGame.TileSize, AbigailGame.TileSize)), (float) (AbigailGame.monsters[0].position.Y + Game1.random.Next(-AbigailGame.TileSize, AbigailGame.TileSize))) + AbigailGame.topLeftScreenCoordinate + new Vector2((float) (AbigailGame.TileSize / 2), (float) (AbigailGame.TileSize / 2)), false, false, 1f, 0.0f, Color.White, 3f, 0.0f, 0.0f, 0.0f, true)
        {
          delayBeforeAnimationStart = index * 75
        });
      AbigailGame.monsters.Clear();
    }

    public void updateBullets(GameTime time)
    {
      for (int index1 = this.bullets.Count - 1; index1 >= 0; --index1)
      {
        this.bullets[index1].position.X += this.bullets[index1].motion.X;
        this.bullets[index1].position.Y += this.bullets[index1].motion.Y;
        if (this.bullets[index1].position.X <= 0 || this.bullets[index1].position.Y <= 0 || (this.bullets[index1].position.X >= 768 || this.bullets[index1].position.Y >= 768))
          this.bullets.RemoveAt(index1);
        else if (AbigailGame.map[this.bullets[index1].position.X / 16 / 3, this.bullets[index1].position.Y / 16 / 3] == 7)
        {
          this.bullets.RemoveAt(index1);
        }
        else
        {
          for (int index2 = AbigailGame.monsters.Count - 1; index2 >= 0; --index2)
          {
            if (AbigailGame.monsters[index2].position.Intersects(new Rectangle(this.bullets[index1].position.X, this.bullets[index1].position.Y, 12, 12)))
            {
              int health1 = AbigailGame.monsters[index2].health;
              int health2;
              if (AbigailGame.monsters[index2].takeDamage(this.bullets[index1].damage))
              {
                health2 = AbigailGame.monsters[index2].health;
                AbigailGame.addGuts(AbigailGame.monsters[index2].position.Location, AbigailGame.monsters[index2].type);
                int which = AbigailGame.monsters[index2].getLootDrop();
                if (this.whichRound == 1 && Game1.random.NextDouble() < 0.5)
                  which = -1;
                if (this.whichRound > 0 && (which == 5 || which == 8) && Game1.random.NextDouble() < 0.4)
                  which = -1;
                if (which != -1 && AbigailGame.whichWave != 12)
                  AbigailGame.powerups.Add(new AbigailGame.CowboyPowerup(which, AbigailGame.monsters[index2].position.Location, this.lootDuration));
                if (AbigailGame.shootoutLevel)
                {
                  if (AbigailGame.whichWave == 12 && AbigailGame.monsters[index2].type == -2)
                  {
                    Game1.playSound("cowboy_explosion");
                    AbigailGame.powerups.Add(new AbigailGame.CowboyPowerup(-3, new Point(8 * AbigailGame.TileSize, 10 * AbigailGame.TileSize), 9999999));
                    this.noPickUpBox = new Rectangle(8 * AbigailGame.TileSize, 10 * AbigailGame.TileSize, AbigailGame.TileSize, AbigailGame.TileSize);
                    if (AbigailGame.outlawSong != null && AbigailGame.outlawSong.IsPlaying)
                      AbigailGame.outlawSong.Stop(AudioStopOptions.Immediate);
                    AbigailGame.screenFlash = 200;
                    for (int index3 = 0; index3 < 30; ++index3)
                    {
                      AbigailGame.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(512, 1696, 16, 16), 70f, 6, 0, new Vector2((float) (AbigailGame.monsters[index2].position.X + Game1.random.Next(-AbigailGame.TileSize, AbigailGame.TileSize)), (float) (AbigailGame.monsters[index2].position.Y + Game1.random.Next(-AbigailGame.TileSize, AbigailGame.TileSize))) + AbigailGame.topLeftScreenCoordinate + new Vector2((float) (AbigailGame.TileSize / 2), (float) (AbigailGame.TileSize / 2)), false, false, 1f, 0.0f, Color.White, 3f, 0.0f, 0.0f, 0.0f, true)
                      {
                        delayBeforeAnimationStart = index3 * 75
                      });
                      if (index3 % 4 == 0)
                        AbigailGame.addGuts(new Point(AbigailGame.monsters[index2].position.X + Game1.random.Next(-AbigailGame.TileSize, AbigailGame.TileSize), AbigailGame.monsters[index2].position.Y + Game1.random.Next(-AbigailGame.TileSize, AbigailGame.TileSize)), 7);
                      if (index3 % 4 == 0)
                        AbigailGame.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(464, 1792, 16, 16), 80f, 5, 0, new Vector2((float) (AbigailGame.monsters[index2].position.X + Game1.random.Next(-AbigailGame.TileSize, AbigailGame.TileSize)), (float) (AbigailGame.monsters[index2].position.Y + Game1.random.Next(-AbigailGame.TileSize, AbigailGame.TileSize))) + AbigailGame.topLeftScreenCoordinate + new Vector2((float) (AbigailGame.TileSize / 2), (float) (AbigailGame.TileSize / 2)), false, false, 1f, 0.0f, Color.White, 3f, 0.0f, 0.0f, 0.0f, true)
                        {
                          delayBeforeAnimationStart = index3 * 75
                        });
                      if (index3 % 3 == 0)
                        AbigailGame.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(544, 1728, 16, 16), 100f, 4, 0, new Vector2((float) (AbigailGame.monsters[index2].position.X + Game1.random.Next(-AbigailGame.TileSize, AbigailGame.TileSize)), (float) (AbigailGame.monsters[index2].position.Y + Game1.random.Next(-AbigailGame.TileSize, AbigailGame.TileSize))) + AbigailGame.topLeftScreenCoordinate + new Vector2((float) (AbigailGame.TileSize / 2), (float) (AbigailGame.TileSize / 2)), false, false, 1f, 0.0f, Color.White, 3f, 0.0f, 0.0f, 0.0f, true)
                        {
                          delayBeforeAnimationStart = index3 * 75
                        });
                    }
                  }
                  else if (AbigailGame.whichWave != 12)
                  {
                    AbigailGame.powerups.Add(new AbigailGame.CowboyPowerup(AbigailGame.world == 0 ? -1 : -2, new Point(8 * AbigailGame.TileSize, 10 * AbigailGame.TileSize), 9999999));
                    if (AbigailGame.outlawSong != null && AbigailGame.outlawSong.IsPlaying)
                      AbigailGame.outlawSong.Stop(AudioStopOptions.Immediate);
                    AbigailGame.map[8, 8] = 10;
                    AbigailGame.screenFlash = 200;
                    for (int index3 = 0; index3 < 15; ++index3)
                      AbigailGame.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(464, 1792, 16, 16), 80f, 5, 0, new Vector2((float) (AbigailGame.monsters[index2].position.X + Game1.random.Next(-AbigailGame.TileSize, AbigailGame.TileSize)), (float) (AbigailGame.monsters[index2].position.Y + Game1.random.Next(-AbigailGame.TileSize, AbigailGame.TileSize))) + AbigailGame.topLeftScreenCoordinate + new Vector2((float) (AbigailGame.TileSize / 2), (float) (AbigailGame.TileSize / 2)), false, false, 1f, 0.0f, Color.White, 3f, 0.0f, 0.0f, 0.0f, true)
                      {
                        delayBeforeAnimationStart = index3 * 75
                      });
                  }
                }
                AbigailGame.monsters.RemoveAt(index2);
                Game1.playSound("Cowboy_monsterDie");
              }
              else
                health2 = AbigailGame.monsters[index2].health;
              this.bullets[index1].damage -= health1 - health2;
              if (this.bullets[index1].damage <= 0)
              {
                this.bullets.RemoveAt(index1);
                break;
              }
              break;
            }
          }
        }
      }
      for (int index = AbigailGame.enemyBullets.Count - 1; index >= 0; --index)
      {
        AbigailGame.enemyBullets[index].position.X += AbigailGame.enemyBullets[index].motion.X;
        AbigailGame.enemyBullets[index].position.Y += AbigailGame.enemyBullets[index].motion.Y;
        if (AbigailGame.enemyBullets[index].position.X <= 0 || AbigailGame.enemyBullets[index].position.Y <= 0 || (AbigailGame.enemyBullets[index].position.X >= 762 || AbigailGame.enemyBullets[index].position.Y >= 762))
          AbigailGame.enemyBullets.RemoveAt(index);
        else if (AbigailGame.map[(AbigailGame.enemyBullets[index].position.X + 6) / 16 / 3, (AbigailGame.enemyBullets[index].position.Y + 6) / 16 / 3] == 7)
          AbigailGame.enemyBullets.RemoveAt(index);
        else if (AbigailGame.playerInvincibleTimer <= 0 && (double) AbigailGame.deathTimer <= 0.0 && this.playerBoundingBox.Intersects(new Rectangle(AbigailGame.enemyBullets[index].position.X, AbigailGame.enemyBullets[index].position.Y, 15, 15)))
        {
          this.playerDie();
          break;
        }
      }
    }

    public void playerDie()
    {
      AbigailGame.gopherRunning = false;
      AbigailGame.hasGopherAppeared = false;
      this.spawnQueue = new List<Point>[4];
      for (int index = 0; index < 4; ++index)
        this.spawnQueue[index] = new List<Point>();
      AbigailGame.enemyBullets.Clear();
      if (!AbigailGame.shootoutLevel)
      {
        AbigailGame.powerups.Clear();
        AbigailGame.monsters.Clear();
      }
      this.died = true;
      this.activePowerups.Clear();
      AbigailGame.deathTimer = 3000f;
      if (AbigailGame.overworldSong != null && AbigailGame.overworldSong.IsPlaying)
        AbigailGame.overworldSong.Stop(AudioStopOptions.Immediate);
      AbigailGame.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(464, 1808, 16, 16), 120f, 5, 0, this.playerPosition + AbigailGame.topLeftScreenCoordinate, false, false, 1f, 0.0f, Color.White, 3f, 0.0f, 0.0f, 0.0f, true));
      AbigailGame.waveTimer = Math.Min(80000, AbigailGame.waveTimer + 10000);
      AbigailGame.betweenWaveTimer = 4000;
      this.lives = this.lives - 1;
      AbigailGame.playerInvincibleTimer = 5000;
      if (AbigailGame.shootoutLevel)
      {
        this.playerPosition = new Vector2((float) (8 * AbigailGame.TileSize), (float) (3 * AbigailGame.TileSize));
        Game1.playSound("Cowboy_monsterDie");
      }
      else
      {
        this.playerPosition = new Vector2((float) (8 * AbigailGame.TileSize - AbigailGame.TileSize), (float) (8 * AbigailGame.TileSize));
        this.playerBoundingBox.X = (int) this.playerPosition.X;
        this.playerBoundingBox.Y = (int) this.playerPosition.Y;
        if (this.playerBoundingBox.Intersects(this.player2BoundingBox))
        {
          this.playerPosition.X -= (float) (AbigailGame.TileSize * 3 / 2);
          this.player2deathtimer = (int) AbigailGame.deathTimer;
        }
        Game1.playSound("cowboy_dead");
      }
      if (this.lives >= 0)
        return;
      List<TemporaryAnimatedSprite> temporarySprites = AbigailGame.temporarySprites;
      TemporaryAnimatedSprite temporaryAnimatedSprite = new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(464, 1808, 16, 16), 550f, 5, 0, this.playerPosition + AbigailGame.topLeftScreenCoordinate, false, false, 1f, 0.0f, Color.White, 3f, 0.0f, 0.0f, 0.0f, true);
      temporaryAnimatedSprite.alpha = 1f / 1000f;
      TemporaryAnimatedSprite.endBehavior endBehavior = new TemporaryAnimatedSprite.endBehavior(this.afterPlayerDeathFunction);
      temporaryAnimatedSprite.endFunction = endBehavior;
      temporarySprites.Add(temporaryAnimatedSprite);
      AbigailGame.deathTimer *= 3f;
    }

    public void afterPlayerDeathFunction(int extra)
    {
      if (this.lives >= 0)
        return;
      AbigailGame.gameOver = true;
      if (AbigailGame.overworldSong != null && !AbigailGame.overworldSong.IsPlaying)
        AbigailGame.overworldSong.Stop(AudioStopOptions.Immediate);
      if (AbigailGame.outlawSong != null && !AbigailGame.outlawSong.IsPlaying)
        AbigailGame.overworldSong.Stop(AudioStopOptions.Immediate);
      AbigailGame.monsters.Clear();
      AbigailGame.powerups.Clear();
      this.died = false;
      Game1.playSound("Cowboy_monsterDie");
      if (!AbigailGame.playingWithAbigail || Game1.currentLocation.currentEvent == null)
        return;
      this.unload();
      Game1.currentMinigame = (IMinigame) null;
      ++Game1.currentLocation.currentEvent.CurrentCommand;
    }

    public void startAbigailPortrait(int whichExpression, string sayWhat)
    {
      if (this.abigailPortraitTimer > 0)
        return;
      this.abigailPortraitTimer = 6000;
      this.AbigailDialogue = sayWhat;
      this.abigailPortraitExpression = whichExpression;
      this.abigailPortraitYposition = Game1.graphics.GraphicsDevice.Viewport.Height;
      Game1.playSound("dwop");
    }

    public void startNewRound()
    {
      this.gamerestartTimer = 2000;
      Game1.playSound("Cowboy_monsterDie");
      this.whichRound = this.whichRound + 1;
    }

    public bool tick(GameTime time)
    {
      if (this.quit)
      {
        if (Game1.currentLocation != null && Game1.currentLocation.name.Equals("Saloon") && Game1.timeOfDay >= 1700)
          Game1.changeMusicTrack("Saloon1");
        return true;
      }
      if (AbigailGame.gameOver || AbigailGame.onStartMenu)
      {
        if (AbigailGame.startTimer > 0)
        {
          AbigailGame.startTimer -= time.ElapsedGameTime.Milliseconds;
          if (AbigailGame.startTimer <= 0)
            AbigailGame.onStartMenu = false;
        }
        else
          AbigailGame.startTimer = 1500;
        return false;
      }
      if (this.gamerestartTimer > 0)
      {
        this.gamerestartTimer = this.gamerestartTimer - time.ElapsedGameTime.Milliseconds;
        if (this.gamerestartTimer <= 0)
        {
          this.unload();
          Game1.currentMinigame = this.whichRound == 0 || !AbigailGame.endCutscene ? (IMinigame) new AbigailGame(false) : (IMinigame) new AbigailGame(this.coins, this.ammoLevel, this.bulletDamage, this.fireSpeedLevel, this.runSpeedLevel, this.lives, this.spreadPistol, this.whichRound);
        }
      }
      if (this.fadethenQuitTimer > 0 && (double) this.abigailPortraitTimer <= 0.0)
      {
        this.fadethenQuitTimer = this.fadethenQuitTimer - time.ElapsedGameTime.Milliseconds;
        if (this.fadethenQuitTimer <= 0)
        {
          if (Game1.currentLocation.currentEvent != null)
          {
            ++Game1.currentLocation.currentEvent.CurrentCommand;
            if (AbigailGame.beatLevelWithAbigail)
              Game1.currentLocation.currentEvent.specialEventVariable1 = true;
          }
          return true;
        }
      }
      if (this.abigailPortraitTimer > 0)
      {
        this.abigailPortraitTimer = this.abigailPortraitTimer - time.ElapsedGameTime.Milliseconds;
        if (this.abigailPortraitTimer > 1000 && this.abigailPortraitYposition > Game1.graphics.GraphicsDevice.Viewport.Height - Game1.tileSize * 4)
          this.abigailPortraitYposition = this.abigailPortraitYposition - 16;
        else if (this.abigailPortraitTimer <= 1000)
          this.abigailPortraitYposition = this.abigailPortraitYposition + 16;
      }
      if (AbigailGame.endCutscene)
      {
        int endCutsceneTimer = AbigailGame.endCutsceneTimer;
        TimeSpan elapsedGameTime = time.ElapsedGameTime;
        int milliseconds1 = elapsedGameTime.Milliseconds;
        AbigailGame.endCutsceneTimer = endCutsceneTimer - milliseconds1;
        if (AbigailGame.endCutsceneTimer < 0)
        {
          ++AbigailGame.endCutscenePhase;
          if (AbigailGame.endCutscenePhase > 5)
            AbigailGame.endCutscenePhase = 5;
          switch (AbigailGame.endCutscenePhase)
          {
            case 1:
              Game1.getSteamAchievement("Achievement_PrairieKing");
              if (!this.died)
                Game1.getSteamAchievement("Achievement_FectorsChallenge");
              AbigailGame.endCutsceneTimer = 15500;
              Game1.playSound("Cowboy_singing");
              AbigailGame.map = this.getMap(-1);
              break;
            case 2:
              this.playerPosition = new Vector2(0.0f, (float) (8 * AbigailGame.TileSize));
              AbigailGame.endCutsceneTimer = 12000;
              break;
            case 3:
              AbigailGame.endCutsceneTimer = 5000;
              break;
            case 4:
              AbigailGame.endCutsceneTimer = 1000;
              break;
            case 5:
              if (Game1.oldKBState.GetPressedKeys().Length == 0)
              {
                GamePadState oldPadState = Game1.oldPadState;
                GamePadButtons buttons = Game1.oldPadState.Buttons;
                if (buttons.X != ButtonState.Pressed)
                {
                  buttons = Game1.oldPadState.Buttons;
                  if (buttons.Start != ButtonState.Pressed)
                  {
                    buttons = Game1.oldPadState.Buttons;
                    if (buttons.A != ButtonState.Pressed)
                      break;
                  }
                }
              }
              if (this.gamerestartTimer <= 0)
              {
                this.startNewRound();
                break;
              }
              break;
          }
        }
        if (AbigailGame.endCutscenePhase == 2 && (double) this.playerPosition.X < (double) (9 * AbigailGame.TileSize))
        {
          ++this.playerPosition.X;
          double motionAnimationTimer = (double) this.playerMotionAnimationTimer;
          elapsedGameTime = time.ElapsedGameTime;
          double milliseconds2 = (double) elapsedGameTime.Milliseconds;
          this.playerMotionAnimationTimer = (float) (motionAnimationTimer + milliseconds2);
          this.playerMotionAnimationTimer = this.playerMotionAnimationTimer % 400f;
        }
        return false;
      }
      TimeSpan elapsedGameTime1;
      if (this.motionPause > 0)
      {
        this.motionPause = this.motionPause - time.ElapsedGameTime.Milliseconds;
        if (this.motionPause <= 0 && this.behaviorAfterPause != null)
        {
          this.behaviorAfterPause();
          this.behaviorAfterPause = (AbigailGame.behaviorAfterMotionPause) null;
        }
      }
      else if (AbigailGame.monsterConfusionTimer > 0)
      {
        int monsterConfusionTimer = AbigailGame.monsterConfusionTimer;
        elapsedGameTime1 = time.ElapsedGameTime;
        int milliseconds = elapsedGameTime1.Milliseconds;
        AbigailGame.monsterConfusionTimer = monsterConfusionTimer - milliseconds;
      }
      if (AbigailGame.zombieModeTimer > 0)
      {
        int zombieModeTimer = AbigailGame.zombieModeTimer;
        elapsedGameTime1 = time.ElapsedGameTime;
        int milliseconds = elapsedGameTime1.Milliseconds;
        AbigailGame.zombieModeTimer = zombieModeTimer - milliseconds;
      }
      if (AbigailGame.holdItemTimer > 0)
      {
        int holdItemTimer = AbigailGame.holdItemTimer;
        elapsedGameTime1 = time.ElapsedGameTime;
        int milliseconds = elapsedGameTime1.Milliseconds;
        AbigailGame.holdItemTimer = holdItemTimer - milliseconds;
        return false;
      }
      if (AbigailGame.screenFlash > 0)
      {
        int screenFlash = AbigailGame.screenFlash;
        elapsedGameTime1 = time.ElapsedGameTime;
        int milliseconds = elapsedGameTime1.Milliseconds;
        AbigailGame.screenFlash = screenFlash - milliseconds;
      }
      if (AbigailGame.gopherTrain)
      {
        AbigailGame.gopherTrainPosition += 3;
        if (AbigailGame.gopherTrainPosition % 30 == 0)
          Game1.playSound("Cowboy_Footstep");
        if (AbigailGame.playerJumped)
          this.playerPosition.Y += 3f;
        if ((double) Math.Abs(this.playerPosition.Y - (float) (AbigailGame.gopherTrainPosition - AbigailGame.TileSize)) <= 16.0)
        {
          AbigailGame.playerJumped = true;
          this.playerPosition.Y = (float) (AbigailGame.gopherTrainPosition - AbigailGame.TileSize);
        }
        if (AbigailGame.gopherTrainPosition > 16 * AbigailGame.TileSize + AbigailGame.TileSize)
        {
          AbigailGame.gopherTrain = false;
          AbigailGame.playerJumped = false;
          ++AbigailGame.whichWave;
          AbigailGame.map = this.getMap(AbigailGame.whichWave);
          this.playerPosition = new Vector2((float) (8 * AbigailGame.TileSize), (float) (8 * AbigailGame.TileSize));
          AbigailGame.world = AbigailGame.world == 0 ? 2 : 1;
          AbigailGame.waveTimer = 80000;
          AbigailGame.betweenWaveTimer = 5000;
          AbigailGame.waitingForPlayerToMoveDownAMap = false;
          AbigailGame.shootoutLevel = false;
        }
      }
      if ((AbigailGame.shopping || AbigailGame.merchantArriving || (AbigailGame.merchantLeaving || AbigailGame.waitingForPlayerToMoveDownAMap)) && AbigailGame.holdItemTimer <= 0)
      {
        int shoppingTimer1 = AbigailGame.shoppingTimer;
        int shoppingTimer2 = AbigailGame.shoppingTimer;
        elapsedGameTime1 = time.ElapsedGameTime;
        int milliseconds = elapsedGameTime1.Milliseconds;
        AbigailGame.shoppingTimer = shoppingTimer2 + milliseconds;
        AbigailGame.shoppingTimer %= 500;
        if (!AbigailGame.merchantShopOpen && AbigailGame.shopping && (shoppingTimer1 < 250 && AbigailGame.shoppingTimer >= 250 || shoppingTimer1 > AbigailGame.shoppingTimer))
          Game1.playSound("Cowboy_Footstep");
      }
      if (AbigailGame.playerInvincibleTimer > 0)
      {
        int playerInvincibleTimer = AbigailGame.playerInvincibleTimer;
        elapsedGameTime1 = time.ElapsedGameTime;
        int milliseconds = elapsedGameTime1.Milliseconds;
        AbigailGame.playerInvincibleTimer = playerInvincibleTimer - milliseconds;
      }
      if (AbigailGame.scrollingMap)
      {
        AbigailGame.newMapPosition -= AbigailGame.TileSize / 8;
        this.playerPosition.Y -= (float) (AbigailGame.TileSize / 8);
        this.playerPosition.Y += 3f;
        this.playerBoundingBox.X = (int) this.playerPosition.X + AbigailGame.TileSize / 4;
        this.playerBoundingBox.Y = (int) this.playerPosition.Y + AbigailGame.TileSize / 4;
        this.playerBoundingBox.Width = AbigailGame.TileSize / 2;
        this.playerBoundingBox.Height = AbigailGame.TileSize / 2;
        AbigailGame.playerMovementDirections = new List<int>()
        {
          2
        };
        double motionAnimationTimer = (double) this.playerMotionAnimationTimer;
        elapsedGameTime1 = time.ElapsedGameTime;
        double milliseconds = (double) elapsedGameTime1.Milliseconds;
        this.playerMotionAnimationTimer = (float) (motionAnimationTimer + milliseconds);
        this.playerMotionAnimationTimer = this.playerMotionAnimationTimer % 400f;
        if (AbigailGame.newMapPosition <= 0)
        {
          AbigailGame.scrollingMap = false;
          AbigailGame.map = AbigailGame.nextMap;
          AbigailGame.newMapPosition = 16 * AbigailGame.TileSize;
          AbigailGame.shopping = false;
          AbigailGame.betweenWaveTimer = 5000;
          AbigailGame.waitingForPlayerToMoveDownAMap = false;
          AbigailGame.playerMovementDirections.Clear();
          if (AbigailGame.whichWave == 12)
          {
            AbigailGame.shootoutLevel = true;
            AbigailGame.monsters.Add((AbigailGame.CowboyMonster) new AbigailGame.Dracula());
            if (this.whichRound > 0)
              AbigailGame.monsters.Last<AbigailGame.CowboyMonster>().health *= 2;
          }
          else if (AbigailGame.whichWave % 4 == 0)
          {
            AbigailGame.shootoutLevel = true;
            AbigailGame.monsters.Add((AbigailGame.CowboyMonster) new AbigailGame.Outlaw(new Point(8 * AbigailGame.TileSize, 13 * AbigailGame.TileSize), AbigailGame.world == 0 ? 50 : 100));
            if (Game1.soundBank != null)
            {
              AbigailGame.outlawSong = Game1.soundBank.GetCue("cowboy_outlawsong");
              AbigailGame.outlawSong.Play();
            }
          }
        }
      }
      if (AbigailGame.gopherRunning)
      {
        AbigailGame.gopherBox.X += this.gopherMotion.X;
        AbigailGame.gopherBox.Y += this.gopherMotion.Y;
        for (int index = AbigailGame.monsters.Count - 1; index >= 0; --index)
        {
          if (AbigailGame.gopherBox.Intersects(AbigailGame.monsters[index].position))
          {
            AbigailGame.addGuts(AbigailGame.monsters[index].position.Location, AbigailGame.monsters[index].type);
            AbigailGame.monsters.RemoveAt(index);
            Game1.playSound("Cowboy_monsterDie");
          }
        }
        if (AbigailGame.gopherBox.X < 0 || AbigailGame.gopherBox.Y < 0 || (AbigailGame.gopherBox.X > 16 * AbigailGame.TileSize || AbigailGame.gopherBox.Y > 16 * AbigailGame.TileSize))
          AbigailGame.gopherRunning = false;
      }
      for (int index = AbigailGame.temporarySprites.Count - 1; index >= 0; --index)
      {
        if (AbigailGame.temporarySprites[index].update(time))
          AbigailGame.temporarySprites.RemoveAt(index);
      }
      if (this.motionPause <= 0)
      {
        for (int index = AbigailGame.powerups.Count - 1; index >= 0; --index)
        {
          if ((double) Utility.distance((float) this.playerBoundingBox.Center.X, (float) (AbigailGame.powerups[index].position.X + AbigailGame.TileSize / 2), (float) this.playerBoundingBox.Center.Y, (float) (AbigailGame.powerups[index].position.Y + AbigailGame.TileSize / 2)) <= (double) (AbigailGame.TileSize + 3) && (AbigailGame.powerups[index].position.X < AbigailGame.TileSize || AbigailGame.powerups[index].position.X >= 16 * AbigailGame.TileSize - AbigailGame.TileSize || (AbigailGame.powerups[index].position.Y < AbigailGame.TileSize || AbigailGame.powerups[index].position.Y >= 16 * AbigailGame.TileSize - AbigailGame.TileSize)))
          {
            if (AbigailGame.powerups[index].position.X + AbigailGame.TileSize / 2 < this.playerBoundingBox.Center.X)
              ++AbigailGame.powerups[index].position.X;
            if (AbigailGame.powerups[index].position.X + AbigailGame.TileSize / 2 > this.playerBoundingBox.Center.X)
              --AbigailGame.powerups[index].position.X;
            if (AbigailGame.powerups[index].position.Y + AbigailGame.TileSize / 2 < this.playerBoundingBox.Center.Y)
              ++AbigailGame.powerups[index].position.Y;
            if (AbigailGame.powerups[index].position.Y + AbigailGame.TileSize / 2 > this.playerBoundingBox.Center.Y)
              --AbigailGame.powerups[index].position.Y;
          }
          AbigailGame.CowboyPowerup powerup = AbigailGame.powerups[index];
          int duration = powerup.duration;
          elapsedGameTime1 = time.ElapsedGameTime;
          int milliseconds = elapsedGameTime1.Milliseconds;
          int num = duration - milliseconds;
          powerup.duration = num;
          if (AbigailGame.powerups[index].duration <= 0)
            AbigailGame.powerups.RemoveAt(index);
        }
        for (int index1 = this.activePowerups.Count - 1; index1 >= 0; --index1)
        {
          Dictionary<int, int> activePowerups = this.activePowerups;
          int key = this.activePowerups.ElementAt<KeyValuePair<int, int>>(index1).Key;
          Dictionary<int, int> dictionary = activePowerups;
          int index2 = key;
          int num1 = activePowerups[key];
          elapsedGameTime1 = time.ElapsedGameTime;
          int milliseconds = elapsedGameTime1.Milliseconds;
          int num2 = num1 - milliseconds;
          dictionary[index2] = num2;
          if (this.activePowerups[this.activePowerups.ElementAt<KeyValuePair<int, int>>(index1).Key] <= 0)
            this.activePowerups.Remove(this.activePowerups.ElementAt<KeyValuePair<int, int>>(index1).Key);
        }
        if ((double) AbigailGame.deathTimer <= 0.0 && AbigailGame.playerMovementDirections.Count > 0 && !AbigailGame.scrollingMap)
        {
          int directions = AbigailGame.playerMovementDirections.Count;
          if (directions >= 2 && AbigailGame.playerMovementDirections.Last<int>() == (AbigailGame.playerMovementDirections.ElementAt<int>(AbigailGame.playerMovementDirections.Count - 2) + 2) % 4)
            directions = 1;
          float movementSpeed = this.getMovementSpeed(3f, directions);
          if (this.activePowerups.Keys.Contains<int>(6))
            movementSpeed *= 1.5f;
          if (AbigailGame.zombieModeTimer > 0)
            movementSpeed *= 1.5f;
          for (int index = 0; index < this.runSpeedLevel; ++index)
            movementSpeed *= 1.25f;
          for (int index = Math.Max(0, AbigailGame.playerMovementDirections.Count - 2); index < AbigailGame.playerMovementDirections.Count; ++index)
          {
            if (index != 0 || AbigailGame.playerMovementDirections.Count < 2 || AbigailGame.playerMovementDirections.Last<int>() != (AbigailGame.playerMovementDirections.ElementAt<int>(AbigailGame.playerMovementDirections.Count - 2) + 2) % 4)
            {
              Vector2 playerPosition = this.playerPosition;
              switch (AbigailGame.playerMovementDirections.ElementAt<int>(index))
              {
                case 0:
                  playerPosition.Y -= movementSpeed;
                  break;
                case 1:
                  playerPosition.X += movementSpeed;
                  break;
                case 2:
                  playerPosition.Y += movementSpeed;
                  break;
                case 3:
                  playerPosition.X -= movementSpeed;
                  break;
              }
              Rectangle positionToCheck = new Rectangle((int) playerPosition.X + AbigailGame.TileSize / 4, (int) playerPosition.Y + AbigailGame.TileSize / 4, AbigailGame.TileSize / 2, AbigailGame.TileSize / 2);
              if (!AbigailGame.isCollidingWithMap(positionToCheck) && (!this.merchantBox.Intersects(positionToCheck) || this.merchantBox.Intersects(this.playerBoundingBox)) && (!AbigailGame.playingWithAbigail || !positionToCheck.Intersects(this.player2BoundingBox)))
                this.playerPosition = playerPosition;
            }
          }
          this.playerBoundingBox.X = (int) this.playerPosition.X + AbigailGame.TileSize / 4;
          this.playerBoundingBox.Y = (int) this.playerPosition.Y + AbigailGame.TileSize / 4;
          this.playerBoundingBox.Width = AbigailGame.TileSize / 2;
          this.playerBoundingBox.Height = AbigailGame.TileSize / 2;
          double motionAnimationTimer = (double) this.playerMotionAnimationTimer;
          elapsedGameTime1 = time.ElapsedGameTime;
          double milliseconds1 = (double) elapsedGameTime1.Milliseconds;
          this.playerMotionAnimationTimer = (float) (motionAnimationTimer + milliseconds1);
          this.playerMotionAnimationTimer = this.playerMotionAnimationTimer % 400f;
          double footstepSoundTimer = (double) this.playerFootstepSoundTimer;
          elapsedGameTime1 = time.ElapsedGameTime;
          double milliseconds2 = (double) elapsedGameTime1.Milliseconds;
          this.playerFootstepSoundTimer = (float) (footstepSoundTimer - milliseconds2);
          if ((double) this.playerFootstepSoundTimer <= 0.0)
          {
            Game1.playSound("Cowboy_Footstep");
            this.playerFootstepSoundTimer = 200f;
          }
          for (int index = AbigailGame.powerups.Count - 1; index >= 0; --index)
          {
            if (this.playerBoundingBox.Intersects(new Rectangle(AbigailGame.powerups[index].position.X, AbigailGame.powerups[index].position.Y, AbigailGame.TileSize, AbigailGame.TileSize)) && !this.playerBoundingBox.Intersects(this.noPickUpBox))
            {
              if (this.heldItem != null)
              {
                this.usePowerup(AbigailGame.powerups[index].which);
                AbigailGame.powerups.RemoveAt(index);
              }
              else if (this.getPowerUp(AbigailGame.powerups[index]))
                AbigailGame.powerups.RemoveAt(index);
            }
          }
          if (!this.playerBoundingBox.Intersects(this.noPickUpBox))
            this.noPickUpBox.Location = new Point(0, 0);
          if (AbigailGame.waitingForPlayerToMoveDownAMap && this.playerBoundingBox.Bottom >= 16 * AbigailGame.TileSize - AbigailGame.TileSize / 2)
          {
            AbigailGame.shopping = false;
            AbigailGame.merchantArriving = false;
            AbigailGame.merchantLeaving = false;
            AbigailGame.merchantShopOpen = false;
            this.merchantBox.Y = -AbigailGame.TileSize;
            AbigailGame.scrollingMap = true;
            AbigailGame.nextMap = this.getMap(AbigailGame.whichWave);
            AbigailGame.newMapPosition = 16 * AbigailGame.TileSize;
            AbigailGame.temporarySprites.Clear();
            AbigailGame.powerups.Clear();
          }
          if (!this.shoppingCarpetNoPickup.Intersects(this.playerBoundingBox))
            this.shoppingCarpetNoPickup.X = -1000;
        }
        if (AbigailGame.shopping)
        {
          if (this.merchantBox.Y < 8 * AbigailGame.TileSize - AbigailGame.TileSize * 3 && AbigailGame.merchantArriving)
          {
            this.merchantBox.Y += 2;
            if (this.merchantBox.Y >= 8 * AbigailGame.TileSize - AbigailGame.TileSize * 3)
            {
              AbigailGame.merchantShopOpen = true;
              Game1.playSound("cowboy_monsterhit");
              AbigailGame.map[8, 15] = 3;
              AbigailGame.map[7, 15] = 3;
              AbigailGame.map[7, 15] = 3;
              AbigailGame.map[8, 14] = 3;
              AbigailGame.map[7, 14] = 3;
              AbigailGame.map[7, 14] = 3;
              this.shoppingCarpetNoPickup = new Rectangle(this.merchantBox.X - AbigailGame.TileSize, this.merchantBox.Y + AbigailGame.TileSize, AbigailGame.TileSize * 3, AbigailGame.TileSize * 2);
            }
          }
          else if (AbigailGame.merchantLeaving)
          {
            this.merchantBox.Y -= 2;
            if (this.merchantBox.Y <= -AbigailGame.TileSize)
            {
              AbigailGame.shopping = false;
              AbigailGame.merchantLeaving = false;
              AbigailGame.merchantArriving = true;
            }
          }
          else if (AbigailGame.merchantShopOpen)
          {
            for (int index = this.storeItems.Count - 1; index >= 0; --index)
            {
              if (!this.playerBoundingBox.Intersects(this.shoppingCarpetNoPickup))
              {
                KeyValuePair<Rectangle, int> keyValuePair = this.storeItems.ElementAt<KeyValuePair<Rectangle, int>>(index);
                Rectangle key1 = keyValuePair.Key;
                if ((this.playerBoundingBox).Intersects(key1))
                {
                  int coins = this.coins;
                  keyValuePair = this.storeItems.ElementAt<KeyValuePair<Rectangle, int>>(index);
                  int priceForItem = this.getPriceForItem(keyValuePair.Value);
                  if (coins >= priceForItem)
                  {
                    Game1.playSound("Cowboy_Secret");
                    AbigailGame.holdItemTimer = 2500;
                    this.motionPause = 2500;
                    keyValuePair = this.storeItems.ElementAt<KeyValuePair<Rectangle, int>>(index);
                    AbigailGame.itemToHold = keyValuePair.Value;
                    Dictionary<Rectangle, int> storeItems = this.storeItems;
                    keyValuePair = this.storeItems.ElementAt<KeyValuePair<Rectangle, int>>(index);
                    Rectangle key2 = keyValuePair.Key;
                    storeItems.Remove(key2);
                    AbigailGame.merchantLeaving = true;
                    AbigailGame.merchantArriving = false;
                    AbigailGame.merchantShopOpen = false;
                    this.coins = this.coins - this.getPriceForItem(AbigailGame.itemToHold);
                    switch (AbigailGame.itemToHold)
                    {
                      case 0:
                      case 1:
                      case 2:
                        this.fireSpeedLevel = this.fireSpeedLevel + 1;
                        continue;
                      case 3:
                      case 4:
                        this.runSpeedLevel = this.runSpeedLevel + 1;
                        continue;
                      case 5:
                        this.lives = this.lives + 1;
                        continue;
                      case 6:
                      case 7:
                      case 8:
                        this.ammoLevel = this.ammoLevel + 1;
                        this.bulletDamage = this.bulletDamage + 1;
                        continue;
                      case 9:
                        this.spreadPistol = true;
                        continue;
                      case 10:
                        this.heldItem = new AbigailGame.CowboyPowerup(10, Point.Zero, 9999);
                        continue;
                      default:
                        continue;
                    }
                  }
                }
              }
            }
          }
        }
        double cactusDanceTimer = (double) this.cactusDanceTimer;
        elapsedGameTime1 = time.ElapsedGameTime;
        double milliseconds3 = (double) elapsedGameTime1.Milliseconds;
        this.cactusDanceTimer = (float) (cactusDanceTimer + milliseconds3);
        this.cactusDanceTimer = this.cactusDanceTimer % 1600f;
        if (this.shotTimer > 0)
        {
          int shotTimer = this.shotTimer;
          elapsedGameTime1 = time.ElapsedGameTime;
          int milliseconds1 = elapsedGameTime1.Milliseconds;
          this.shotTimer = shotTimer - milliseconds1;
        }
        if ((double) AbigailGame.deathTimer <= 0.0 && AbigailGame.playerShootingDirections.Count > 0 && this.shotTimer <= 0)
        {
          if (this.activePowerups.ContainsKey(2))
          {
            this.spawnBullets(new int[1], this.playerPosition);
            this.spawnBullets(new int[1]{ 1 }, this.playerPosition);
            this.spawnBullets(new int[1]{ 2 }, this.playerPosition);
            this.spawnBullets(new int[1]{ 3 }, this.playerPosition);
            this.spawnBullets(new int[2]{ 0, 1 }, this.playerPosition);
            this.spawnBullets(new int[2]{ 1, 2 }, this.playerPosition);
            this.spawnBullets(new int[2]{ 2, 3 }, this.playerPosition);
            this.spawnBullets(new int[2]{ 3, 0 }, this.playerPosition);
          }
          else if (AbigailGame.playerShootingDirections.Count == 1 || AbigailGame.playerShootingDirections.Last<int>() == (AbigailGame.playerShootingDirections.ElementAt<int>(AbigailGame.playerShootingDirections.Count - 2) + 2) % 4)
            this.spawnBullets(new int[1]
            {
              AbigailGame.playerShootingDirections.Count != 2 || AbigailGame.playerShootingDirections.Last<int>() != (AbigailGame.playerShootingDirections.ElementAt<int>(AbigailGame.playerShootingDirections.Count - 2) + 2) % 4 ? AbigailGame.playerShootingDirections.ElementAt<int>(0) : AbigailGame.playerShootingDirections.ElementAt<int>(1)
            }, this.playerPosition);
          else
            this.spawnBullets(AbigailGame.playerShootingDirections.ToArray(), this.playerPosition);
          Game1.playSound("Cowboy_gunshot");
          this.shotTimer = this.shootingDelay;
          if (this.activePowerups.ContainsKey(3))
            this.shotTimer = this.shotTimer / 4;
          for (int index = 0; index < this.fireSpeedLevel; ++index)
            this.shotTimer = this.shotTimer * 3 / 4;
          if (this.activePowerups.ContainsKey(7))
            this.shotTimer = this.shotTimer * 3 / 2;
          this.shotTimer = Math.Max(this.shotTimer, 20);
        }
        this.updateBullets(time);
        if (AbigailGame.waveTimer > 0 && AbigailGame.betweenWaveTimer <= 0 && (AbigailGame.zombieModeTimer <= 0 && !AbigailGame.shootoutLevel) && ((AbigailGame.overworldSong == null || !AbigailGame.overworldSong.IsPlaying) && Game1.soundBank != null))
        {
          AbigailGame.overworldSong = Game1.soundBank.GetCue("Cowboy_OVERWORLD");
          AbigailGame.overworldSong.Play();
          Game1.musicPlayerVolume = Game1.options.musicVolumeLevel;
          Game1.musicCategory.SetVolume(Game1.musicPlayerVolume);
        }
        if ((double) AbigailGame.deathTimer > 0.0)
        {
          double deathTimer = (double) AbigailGame.deathTimer;
          elapsedGameTime1 = time.ElapsedGameTime;
          double milliseconds1 = (double) elapsedGameTime1.Milliseconds;
          AbigailGame.deathTimer = (float) (deathTimer - milliseconds1);
        }
        if (AbigailGame.betweenWaveTimer > 0 && AbigailGame.monsters.Count == 0 && (this.isSpawnQueueEmpty() && !AbigailGame.shopping) && !AbigailGame.waitingForPlayerToMoveDownAMap)
        {
          int betweenWaveTimer = AbigailGame.betweenWaveTimer;
          elapsedGameTime1 = time.ElapsedGameTime;
          int milliseconds1 = elapsedGameTime1.Milliseconds;
          AbigailGame.betweenWaveTimer = betweenWaveTimer - milliseconds1;
          if (AbigailGame.betweenWaveTimer <= 0 && AbigailGame.playingWithAbigail)
            this.startAbigailPortrait(7, Game1.content.LoadString("Strings\\StringsFromCSFiles:AbigailGame.cs.11896"));
        }
        else if ((double) AbigailGame.deathTimer <= 0.0 && !AbigailGame.waitingForPlayerToMoveDownAMap && (!AbigailGame.shopping && !AbigailGame.shootoutLevel))
        {
          if (AbigailGame.waveTimer > 0)
          {
            int waveTimer1 = AbigailGame.waveTimer;
            int waveTimer2 = AbigailGame.waveTimer;
            elapsedGameTime1 = time.ElapsedGameTime;
            int milliseconds1 = elapsedGameTime1.Milliseconds;
            AbigailGame.waveTimer = waveTimer2 - milliseconds1;
            if (AbigailGame.playingWithAbigail && waveTimer1 > 40000 && AbigailGame.waveTimer <= 40000)
              this.startAbigailPortrait(0, Game1.content.LoadString("Strings\\StringsFromCSFiles:AbigailGame.cs.11897"));
            int x = 0;
            foreach (Vector2 monsterChance in this.monsterChances)
            {
              if (Game1.random.NextDouble() < (double) monsterChance.X * (AbigailGame.monsters.Count == 0 ? 2.0 : 1.0))
              {
                int y = 1;
                while (Game1.random.NextDouble() < (double) monsterChance.Y && y < 15)
                  ++y;
                this.spawnQueue[AbigailGame.whichWave == 11 ? Game1.random.Next(1, 3) * 2 - 1 : Game1.random.Next(4)].Add(new Point(x, y));
              }
              ++x;
            }
            if (!AbigailGame.hasGopherAppeared && AbigailGame.monsters.Count > 6 && (Game1.random.NextDouble() < 0.0004 && AbigailGame.waveTimer > 7000) && AbigailGame.waveTimer < 50000)
            {
              AbigailGame.hasGopherAppeared = true;
              for (AbigailGame.gopherBox = new Rectangle(Game1.random.Next(16 * AbigailGame.TileSize), Game1.random.Next(16 * AbigailGame.TileSize), AbigailGame.TileSize, AbigailGame.TileSize); AbigailGame.isCollidingWithMap(AbigailGame.gopherBox) || AbigailGame.isCollidingWithMonster(AbigailGame.gopherBox, (AbigailGame.CowboyMonster) null) || ((double) Math.Abs((float) AbigailGame.gopherBox.X - this.playerPosition.X) < (double) (AbigailGame.TileSize * 6) || (double) Math.Abs((float) AbigailGame.gopherBox.Y - this.playerPosition.Y) < (double) (AbigailGame.TileSize * 6)) || (Math.Abs(AbigailGame.gopherBox.X - 8 * AbigailGame.TileSize) < AbigailGame.TileSize * 4 || Math.Abs(AbigailGame.gopherBox.Y - 8 * AbigailGame.TileSize) < AbigailGame.TileSize * 4); AbigailGame.gopherBox.Y = Game1.random.Next(16 * AbigailGame.TileSize))
                AbigailGame.gopherBox.X = Game1.random.Next(16 * AbigailGame.TileSize);
              AbigailGame.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(256, 1664, 16, 32), 80f, 5, 0, AbigailGame.topLeftScreenCoordinate + new Vector2((float) (AbigailGame.gopherBox.X + AbigailGame.TileSize / 2), (float) (AbigailGame.gopherBox.Y - AbigailGame.TileSize + AbigailGame.TileSize / 2)), false, false, (float) AbigailGame.gopherBox.Y / 10000f, 0.0f, Color.White, 3f, 0.0f, 0.0f, 0.0f, true)
              {
                endFunction = new TemporaryAnimatedSprite.endBehavior(this.endOfGopherAnimationBehavior)
              });
            }
          }
          for (int index1 = 0; index1 < 4; ++index1)
          {
            if (this.spawnQueue[index1].Count > 0)
            {
              if (this.spawnQueue[index1][0].X == 1 || this.spawnQueue[index1][0].X == 4)
              {
                List<Vector2> borderOfThisRectangle = Utility.getBorderOfThisRectangle(new Rectangle(0, 0, 16, 16));
                Vector2 vector2 = borderOfThisRectangle.ElementAt<Vector2>(Game1.random.Next(borderOfThisRectangle.Count));
                while (AbigailGame.isCollidingWithMonster(new Rectangle((int) vector2.X * AbigailGame.TileSize, (int) vector2.Y * AbigailGame.TileSize, AbigailGame.TileSize, AbigailGame.TileSize), (AbigailGame.CowboyMonster) null))
                  vector2 = borderOfThisRectangle.ElementAt<Vector2>(Game1.random.Next(borderOfThisRectangle.Count));
                AbigailGame.monsters.Add(new AbigailGame.CowboyMonster(this.spawnQueue[index1][0].X, new Point((int) vector2.X * AbigailGame.TileSize, (int) vector2.Y * AbigailGame.TileSize)));
                if (this.whichRound > 0)
                  AbigailGame.monsters.Last<AbigailGame.CowboyMonster>().health += this.whichRound * 2;
                this.spawnQueue[index1][0] = new Point(this.spawnQueue[index1][0].X, this.spawnQueue[index1][0].Y - 1);
                if (this.spawnQueue[index1][0].Y <= 0)
                  this.spawnQueue[index1].RemoveAt(0);
              }
              else
              {
                switch (index1)
                {
                  case 0:
                    for (int index2 = 7; index2 < 10; ++index2)
                    {
                      if (Game1.random.NextDouble() < 0.5 && !AbigailGame.isCollidingWithMonster(new Rectangle(index2 * 16 * 3, 0, 48, 48), (AbigailGame.CowboyMonster) null))
                      {
                        AbigailGame.monsters.Add(new AbigailGame.CowboyMonster(this.spawnQueue[index1].First<Point>().X, new Point(index2 * AbigailGame.TileSize, 0)));
                        if (this.whichRound > 0)
                          AbigailGame.monsters.Last<AbigailGame.CowboyMonster>().health += this.whichRound * 2;
                        this.spawnQueue[index1][0] = new Point(this.spawnQueue[index1][0].X, this.spawnQueue[index1][0].Y - 1);
                        if (this.spawnQueue[index1][0].Y <= 0)
                        {
                          this.spawnQueue[index1].RemoveAt(0);
                          break;
                        }
                        break;
                      }
                    }
                    continue;
                  case 1:
                    for (int index2 = 7; index2 < 10; ++index2)
                    {
                      if (Game1.random.NextDouble() < 0.5 && !AbigailGame.isCollidingWithMonster(new Rectangle(720, index2 * AbigailGame.TileSize, 48, 48), (AbigailGame.CowboyMonster) null))
                      {
                        AbigailGame.monsters.Add(new AbigailGame.CowboyMonster(this.spawnQueue[index1].First<Point>().X, new Point(15 * AbigailGame.TileSize, index2 * AbigailGame.TileSize)));
                        if (this.whichRound > 0)
                          AbigailGame.monsters.Last<AbigailGame.CowboyMonster>().health += this.whichRound * 2;
                        this.spawnQueue[index1][0] = new Point(this.spawnQueue[index1][0].X, this.spawnQueue[index1][0].Y - 1);
                        if (this.spawnQueue[index1][0].Y <= 0)
                        {
                          this.spawnQueue[index1].RemoveAt(0);
                          break;
                        }
                        break;
                      }
                    }
                    continue;
                  case 2:
                    for (int index2 = 7; index2 < 10; ++index2)
                    {
                      if (Game1.random.NextDouble() < 0.5 && !AbigailGame.isCollidingWithMonster(new Rectangle(index2 * 16 * 3, 15 * AbigailGame.TileSize, 48, 48), (AbigailGame.CowboyMonster) null))
                      {
                        AbigailGame.monsters.Add(new AbigailGame.CowboyMonster(this.spawnQueue[index1].First<Point>().X, new Point(index2 * AbigailGame.TileSize, 15 * AbigailGame.TileSize)));
                        if (this.whichRound > 0)
                          AbigailGame.monsters.Last<AbigailGame.CowboyMonster>().health += this.whichRound * 2;
                        this.spawnQueue[index1][0] = new Point(this.spawnQueue[index1][0].X, this.spawnQueue[index1][0].Y - 1);
                        if (this.spawnQueue[index1][0].Y <= 0)
                        {
                          this.spawnQueue[index1].RemoveAt(0);
                          break;
                        }
                        break;
                      }
                    }
                    continue;
                  case 3:
                    for (int index2 = 7; index2 < 10; ++index2)
                    {
                      if (Game1.random.NextDouble() < 0.5 && !AbigailGame.isCollidingWithMonster(new Rectangle(0, index2 * AbigailGame.TileSize, 48, 48), (AbigailGame.CowboyMonster) null))
                      {
                        AbigailGame.monsters.Add(new AbigailGame.CowboyMonster(this.spawnQueue[index1].First<Point>().X, new Point(0, index2 * AbigailGame.TileSize)));
                        if (this.whichRound > 0)
                          AbigailGame.monsters.Last<AbigailGame.CowboyMonster>().health += this.whichRound * 2;
                        this.spawnQueue[index1][0] = new Point(this.spawnQueue[index1][0].X, this.spawnQueue[index1][0].Y - 1);
                        if (this.spawnQueue[index1][0].Y <= 0)
                        {
                          this.spawnQueue[index1].RemoveAt(0);
                          break;
                        }
                        break;
                      }
                    }
                    continue;
                  default:
                    continue;
                }
              }
            }
          }
          if (AbigailGame.waveTimer <= 0 && AbigailGame.monsters.Count > 0 && this.isSpawnQueueEmpty())
          {
            bool flag = true;
            foreach (AbigailGame.CowboyMonster monster in AbigailGame.monsters)
            {
              if (monster.type != 6)
              {
                flag = false;
                break;
              }
            }
            if (flag)
            {
              foreach (AbigailGame.CowboyMonster monster in AbigailGame.monsters)
                monster.health = 1;
            }
          }
          if (AbigailGame.waveTimer <= 0 && AbigailGame.monsters.Count == 0 && this.isSpawnQueueEmpty())
          {
            AbigailGame.hasGopherAppeared = false;
            if (AbigailGame.playingWithAbigail)
              this.startAbigailPortrait(1, Game1.content.LoadString("Strings\\StringsFromCSFiles:AbigailGame.cs.11898"));
            AbigailGame.waveTimer = 80000;
            AbigailGame.betweenWaveTimer = 3333;
            ++AbigailGame.whichWave;
            if (AbigailGame.playingWithAbigail)
            {
              AbigailGame.beatLevelWithAbigail = true;
              this.fadethenQuitTimer = 2000;
            }
            switch (AbigailGame.whichWave)
            {
              case 1:
              case 2:
              case 3:
                this.monsterChances[0] = new Vector2(this.monsterChances[0].X + 1f / 1000f, this.monsterChances[0].Y + 0.02f);
                if (AbigailGame.whichWave > 1)
                  this.monsterChances[2] = new Vector2(this.monsterChances[2].X + 1f / 1000f, this.monsterChances[2].Y + 0.01f);
                this.monsterChances[6] = new Vector2(this.monsterChances[6].X + 1f / 1000f, this.monsterChances[6].Y + 0.01f);
                if (this.whichRound > 0)
                {
                  this.monsterChances[4] = new Vector2(1f / 500f, 0.1f);
                  break;
                }
                break;
              case 4:
              case 5:
              case 6:
              case 7:
                if (this.monsterChances[5].Equals(Vector2.Zero))
                {
                  this.monsterChances[5] = new Vector2(0.01f, 0.15f);
                  if (this.whichRound > 0)
                    this.monsterChances[5] = new Vector2((float) (0.00999999977648258 + (double) this.whichRound * 0.00400000018998981), (float) (0.150000005960464 + (double) this.whichRound * 0.0399999991059303));
                }
                this.monsterChances[0] = Vector2.Zero;
                this.monsterChances[6] = Vector2.Zero;
                this.monsterChances[2] = new Vector2(this.monsterChances[2].X + 1f / 500f, this.monsterChances[2].Y + 0.02f);
                this.monsterChances[5] = new Vector2(this.monsterChances[5].X + 1f / 1000f, this.monsterChances[5].Y + 0.02f);
                this.monsterChances[1] = new Vector2(this.monsterChances[1].X + 0.0018f, this.monsterChances[1].Y + 0.08f);
                if (this.whichRound > 0)
                {
                  this.monsterChances[4] = new Vector2(1f / 1000f, 0.1f);
                  break;
                }
                break;
              case 8:
              case 9:
              case 10:
              case 11:
                this.monsterChances[5] = Vector2.Zero;
                this.monsterChances[1] = Vector2.Zero;
                this.monsterChances[2] = Vector2.Zero;
                Vector2 monsterChance1 = this.monsterChances[3];
                if (monsterChance1.Equals(Vector2.Zero))
                {
                  this.monsterChances[3] = new Vector2(0.012f, 0.4f);
                  if (this.whichRound > 0)
                    this.monsterChances[3] = new Vector2((float) (0.0120000001043081 + (double) this.whichRound * 0.00499999988824129), (float) (0.400000005960464 + (double) this.whichRound * 0.0750000029802322));
                }
                monsterChance1 = this.monsterChances[4];
                if (monsterChance1.Equals(Vector2.Zero))
                  this.monsterChances[4] = new Vector2(3f / 1000f, 0.1f);
                this.monsterChances[3] = new Vector2(this.monsterChances[3].X + 1f / 500f, this.monsterChances[3].Y + 0.05f);
                this.monsterChances[4] = new Vector2(this.monsterChances[4].X + 0.0015f, this.monsterChances[4].Y + 0.04f);
                if (AbigailGame.whichWave == 11)
                {
                  this.monsterChances[4] = new Vector2(this.monsterChances[4].X + 0.01f, this.monsterChances[4].Y + 0.04f);
                  this.monsterChances[3] = new Vector2(this.monsterChances[3].X - 0.01f, this.monsterChances[3].Y + 0.04f);
                  break;
                }
                break;
            }
            if (this.whichRound > 0)
            {
              for (int index1 = 0; index1 < this.monsterChances.Count<Vector2>(); ++index1)
              {
                Vector2 monsterChance2 = this.monsterChances[index1];
                List<Vector2> monsterChances = this.monsterChances;
                int index2 = index1;
                monsterChances[index2] = monsterChances[index2] * 1.1f;
              }
            }
            if (AbigailGame.whichWave > 0 && AbigailGame.whichWave % 2 == 0)
              this.startShoppingLevel();
            else if (AbigailGame.whichWave > 0)
            {
              AbigailGame.waitingForPlayerToMoveDownAMap = true;
              if (!AbigailGame.playingWithAbigail)
              {
                AbigailGame.map[8, 15] = 3;
                AbigailGame.map[7, 15] = 3;
                AbigailGame.map[9, 15] = 3;
              }
            }
          }
        }
        if (AbigailGame.playingWithAbigail)
          this.updateAbigail(time);
        for (int index = AbigailGame.monsters.Count - 1; index >= 0; --index)
        {
          AbigailGame.monsters[index].move(this.playerPosition, time);
          if (index < AbigailGame.monsters.Count && AbigailGame.monsters[index].position.Intersects(this.playerBoundingBox) && AbigailGame.playerInvincibleTimer <= 0)
          {
            if (AbigailGame.zombieModeTimer > 0)
            {
              AbigailGame.addGuts(AbigailGame.monsters[index].position.Location, AbigailGame.monsters[index].type);
              AbigailGame.monsters.RemoveAt(index);
              Game1.playSound("Cowboy_monsterDie");
            }
            else
            {
              this.playerDie();
              break;
            }
          }
          if (AbigailGame.playingWithAbigail && index < AbigailGame.monsters.Count && (AbigailGame.monsters[index].position.Intersects(this.player2BoundingBox) && this.player2invincibletimer <= 0))
          {
            Game1.playSound("Cowboy_monsterDie");
            this.player2deathtimer = 3000;
            AbigailGame.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(464, 1808, 16, 16), 120f, 5, 0, AbigailGame.player2Position + AbigailGame.topLeftScreenCoordinate + new Vector2((float) (AbigailGame.TileSize / 2), (float) (AbigailGame.TileSize / 2)), false, false, 1f, 0.0f, Color.White, 3f, 0.0f, 0.0f, 0.0f, true));
            this.player2invincibletimer = 4000;
            AbigailGame.player2Position = new Vector2(8f, 8f) * (float) AbigailGame.TileSize;
            this.startAbigailPortrait(5, Game1.random.NextDouble() < 0.5 ? Game1.content.LoadString("Strings\\StringsFromCSFiles:AbigailGame.cs.11901") : Game1.content.LoadString("Strings\\StringsFromCSFiles:AbigailGame.cs.11902"));
          }
        }
      }
      return false;
    }

    public void updateAbigail(GameTime time)
    {
      this.player2TargetUpdateTimer = this.player2TargetUpdateTimer - time.ElapsedGameTime.Milliseconds;
      if (this.player2deathtimer > 0)
        this.player2deathtimer = this.player2deathtimer - time.ElapsedGameTime.Milliseconds;
      if (this.player2invincibletimer > 0)
        this.player2invincibletimer = this.player2invincibletimer - time.ElapsedGameTime.Milliseconds;
      if (this.player2deathtimer > 0)
        return;
      if (this.player2TargetUpdateTimer < 0)
      {
        this.player2TargetUpdateTimer = 500;
        AbigailGame.CowboyMonster cowboyMonster = (AbigailGame.CowboyMonster) null;
        double num1 = 99999.0;
        foreach (AbigailGame.CowboyMonster monster in AbigailGame.monsters)
        {
          double num2 = Math.Sqrt(Math.Pow((double) monster.position.X - (double) AbigailGame.player2Position.X, 2.0) - Math.Pow((double) monster.position.Y - (double) AbigailGame.player2Position.Y, 2.0));
          if (cowboyMonster == null || num2 < num1)
          {
            cowboyMonster = monster;
            num1 = Math.Sqrt(Math.Pow((double) cowboyMonster.position.X - (double) AbigailGame.player2Position.X, 2.0) - Math.Pow((double) cowboyMonster.position.Y - (double) AbigailGame.player2Position.Y, 2.0));
          }
        }
        this.targetMonster = cowboyMonster;
      }
      this.player2ShootingDirections.Clear();
      this.player2MovementDirections.Clear();
      if (this.targetMonster != null)
      {
        if (Math.Sqrt(Math.Pow((double) this.targetMonster.position.X - (double) AbigailGame.player2Position.X, 2.0) - Math.Pow((double) this.targetMonster.position.Y - (double) AbigailGame.player2Position.Y, 2.0)) < (double) (AbigailGame.TileSize * 3))
        {
          if ((double) this.targetMonster.position.X > (double) AbigailGame.player2Position.X)
            this.addPlayer2MovementDirection(3);
          else if ((double) this.targetMonster.position.X < (double) AbigailGame.player2Position.X)
            this.addPlayer2MovementDirection(1);
          if ((double) this.targetMonster.position.Y > (double) AbigailGame.player2Position.Y)
            this.addPlayer2MovementDirection(0);
          else if ((double) this.targetMonster.position.Y < (double) AbigailGame.player2Position.Y)
            this.addPlayer2MovementDirection(2);
          foreach (int movementDirection in this.player2MovementDirections)
            this.player2ShootingDirections.Add((movementDirection + 2) % 4);
        }
        else
        {
          if ((double) Math.Abs((float) this.targetMonster.position.X - AbigailGame.player2Position.X) > (double) Math.Abs((float) this.targetMonster.position.Y - AbigailGame.player2Position.Y) && (double) Math.Abs((float) this.targetMonster.position.Y - AbigailGame.player2Position.Y) > 4.0)
          {
            if ((double) this.targetMonster.position.Y > (double) AbigailGame.player2Position.Y + 3.0)
              this.addPlayer2MovementDirection(2);
            else if ((double) this.targetMonster.position.Y < (double) AbigailGame.player2Position.Y - 3.0)
              this.addPlayer2MovementDirection(0);
          }
          else if ((double) Math.Abs((float) this.targetMonster.position.X - AbigailGame.player2Position.X) > 4.0)
          {
            if ((double) this.targetMonster.position.X > (double) AbigailGame.player2Position.X + 3.0)
              this.addPlayer2MovementDirection(1);
            else if ((double) this.targetMonster.position.X < (double) AbigailGame.player2Position.X - 3.0)
              this.addPlayer2MovementDirection(3);
          }
          if ((double) this.targetMonster.position.X > (double) AbigailGame.player2Position.X + 3.0)
            this.addPlayer2ShootingDirection(1);
          else if ((double) this.targetMonster.position.X < (double) AbigailGame.player2Position.X - 3.0)
            this.addPlayer2ShootingDirection(3);
          if ((double) this.targetMonster.position.Y > (double) AbigailGame.player2Position.Y + 3.0)
            this.addPlayer2ShootingDirection(2);
          else if ((double) this.targetMonster.position.Y < (double) AbigailGame.player2Position.Y - 3.0)
            this.addPlayer2ShootingDirection(0);
        }
      }
      if (this.player2MovementDirections.Count > 0)
      {
        float movementSpeed = this.getMovementSpeed(3f, this.player2MovementDirections.Count);
        for (int index = 0; index < this.player2MovementDirections.Count; ++index)
        {
          Vector2 player2Position = AbigailGame.player2Position;
          switch (this.player2MovementDirections[index])
          {
            case 0:
              player2Position.Y -= movementSpeed;
              break;
            case 1:
              player2Position.X += movementSpeed;
              break;
            case 2:
              player2Position.Y += movementSpeed;
              break;
            case 3:
              player2Position.X -= movementSpeed;
              break;
          }
          Rectangle positionToCheck = new Rectangle((int) player2Position.X + AbigailGame.TileSize / 4, (int) player2Position.Y + AbigailGame.TileSize / 4, AbigailGame.TileSize / 2, AbigailGame.TileSize / 2);
          if (!AbigailGame.isCollidingWithMap(positionToCheck) && (!this.merchantBox.Intersects(positionToCheck) || this.merchantBox.Intersects(this.player2BoundingBox)) && !positionToCheck.Intersects(this.playerBoundingBox))
            AbigailGame.player2Position = player2Position;
        }
        this.player2BoundingBox.X = (int) AbigailGame.player2Position.X + AbigailGame.TileSize / 4;
        this.player2BoundingBox.Y = (int) AbigailGame.player2Position.Y + AbigailGame.TileSize / 4;
        this.player2BoundingBox.Width = AbigailGame.TileSize / 2;
        this.player2BoundingBox.Height = AbigailGame.TileSize / 2;
        this.player2AnimationTimer = this.player2AnimationTimer + time.ElapsedGameTime.Milliseconds;
        this.player2AnimationTimer = this.player2AnimationTimer % 400;
        this.player2FootstepSoundTimer = this.player2FootstepSoundTimer - time.ElapsedGameTime.Milliseconds;
        if (this.player2FootstepSoundTimer <= 0)
        {
          Game1.playSound("Cowboy_Footstep");
          this.player2FootstepSoundTimer = 200;
        }
        for (int index = AbigailGame.powerups.Count - 1; index >= 0; --index)
        {
          if (this.player2BoundingBox.Intersects(new Rectangle(AbigailGame.powerups[index].position.X, AbigailGame.powerups[index].position.Y, AbigailGame.TileSize, AbigailGame.TileSize)) && !this.player2BoundingBox.Intersects(this.noPickUpBox))
            AbigailGame.powerups.RemoveAt(index);
        }
      }
      this.player2shotTimer = this.player2shotTimer - time.ElapsedGameTime.Milliseconds;
      if (this.player2ShootingDirections.Count <= 0 || this.player2shotTimer > 0)
        return;
      if (this.player2ShootingDirections.Count == 1)
        this.spawnBullets(new int[1]
        {
          this.player2ShootingDirections[0]
        }, AbigailGame.player2Position);
      else
        this.spawnBullets(this.player2ShootingDirections.ToArray(), AbigailGame.player2Position);
      Game1.playSound("Cowboy_gunshot");
      this.player2shotTimer = this.shootingDelay;
    }

    public int[,] getMap(int wave)
    {
      int[,] numArray = new int[16, 16];
      for (int index1 = 0; index1 < 16; ++index1)
      {
        for (int index2 = 0; index2 < 16; ++index2)
          numArray[index1, index2] = index1 != 0 && index1 != 15 && (index2 != 0 && index2 != 15) || (index1 > 6 && index1 < 10 || index2 > 6 && index2 < 10) ? (index1 == 0 || index1 == 15 || (index2 == 0 || index2 == 15) ? (Game1.random.NextDouble() < 0.15 ? 1 : 0) : (index1 == 1 || index1 == 14 || (index2 == 1 || index2 == 14) ? 2 : (Game1.random.NextDouble() < 0.1 ? 4 : 3))) : 5;
      }
      switch (wave)
      {
        case -1:
          for (int index1 = 0; index1 < 16; ++index1)
          {
            for (int index2 = 0; index2 < 16; ++index2)
            {
              if (numArray[index1, index2] == 0 || numArray[index1, index2] == 1 || (numArray[index1, index2] == 2 || numArray[index1, index2] == 5))
                numArray[index1, index2] = 3;
            }
          }
          numArray[3, 1] = 5;
          numArray[8, 2] = 5;
          numArray[13, 1] = 5;
          numArray[5, 0] = 0;
          numArray[10, 2] = 2;
          numArray[15, 2] = 1;
          numArray[14, 12] = 5;
          numArray[10, 6] = 7;
          numArray[11, 6] = 7;
          numArray[12, 6] = 7;
          numArray[13, 6] = 7;
          numArray[14, 6] = 7;
          numArray[14, 7] = 7;
          numArray[14, 8] = 7;
          numArray[14, 9] = 7;
          numArray[14, 10] = 7;
          numArray[14, 11] = 7;
          numArray[14, 12] = 7;
          numArray[14, 13] = 7;
          for (int index = 0; index < 16; ++index)
            numArray[index, 3] = index % 2 == 0 ? 9 : 8;
          numArray[3, 3] = 10;
          numArray[7, 8] = 2;
          numArray[8, 8] = 2;
          numArray[4, 11] = 2;
          numArray[11, 12] = 2;
          numArray[9, 11] = 2;
          numArray[3, 9] = 2;
          numArray[2, 12] = 5;
          numArray[8, 13] = 5;
          numArray[12, 11] = 5;
          numArray[7, 14] = 0;
          numArray[6, 14] = 2;
          numArray[8, 14] = 2;
          numArray[7, 13] = 2;
          numArray[7, 15] = 2;
          break;
        case 1:
          numArray[4, 4] = 7;
          numArray[4, 5] = 7;
          numArray[5, 4] = 7;
          numArray[12, 4] = 7;
          numArray[11, 4] = 7;
          numArray[12, 5] = 7;
          numArray[4, 12] = 7;
          numArray[5, 12] = 7;
          numArray[4, 11] = 7;
          numArray[12, 12] = 7;
          numArray[11, 12] = 7;
          numArray[12, 11] = 7;
          break;
        case 2:
          numArray[8, 4] = 7;
          numArray[12, 8] = 7;
          numArray[8, 12] = 7;
          numArray[4, 8] = 7;
          numArray[1, 1] = 5;
          numArray[14, 1] = 5;
          numArray[14, 14] = 5;
          numArray[1, 14] = 5;
          numArray[2, 1] = 5;
          numArray[13, 1] = 5;
          numArray[13, 14] = 5;
          numArray[2, 14] = 5;
          numArray[1, 2] = 5;
          numArray[14, 2] = 5;
          numArray[14, 13] = 5;
          numArray[1, 13] = 5;
          break;
        case 3:
          numArray[5, 5] = 7;
          numArray[6, 5] = 7;
          numArray[7, 5] = 7;
          numArray[9, 5] = 7;
          numArray[10, 5] = 7;
          numArray[11, 5] = 7;
          numArray[5, 11] = 7;
          numArray[6, 11] = 7;
          numArray[7, 11] = 7;
          numArray[9, 11] = 7;
          numArray[10, 11] = 7;
          numArray[11, 11] = 7;
          numArray[5, 6] = 7;
          numArray[5, 7] = 7;
          numArray[5, 9] = 7;
          numArray[5, 10] = 7;
          numArray[11, 6] = 7;
          numArray[11, 7] = 7;
          numArray[11, 9] = 7;
          numArray[11, 10] = 7;
          break;
        case 4:
        case 8:
          for (int index1 = 0; index1 < 16; ++index1)
          {
            for (int index2 = 0; index2 < 16; ++index2)
            {
              if (numArray[index1, index2] == 5)
                numArray[index1, index2] = Game1.random.NextDouble() < 0.5 ? 0 : 1;
            }
          }
          for (int index = 0; index < 16; ++index)
            numArray[index, 8] = Game1.random.NextDouble() < 0.5 ? 8 : 9;
          numArray[8, 4] = 7;
          numArray[8, 12] = 7;
          numArray[9, 12] = 7;
          numArray[7, 12] = 7;
          numArray[5, 6] = 5;
          numArray[10, 6] = 5;
          break;
        case 5:
          numArray[1, 1] = 5;
          numArray[14, 1] = 5;
          numArray[14, 14] = 5;
          numArray[1, 14] = 5;
          numArray[2, 1] = 5;
          numArray[13, 1] = 5;
          numArray[13, 14] = 5;
          numArray[2, 14] = 5;
          numArray[1, 2] = 5;
          numArray[14, 2] = 5;
          numArray[14, 13] = 5;
          numArray[1, 13] = 5;
          numArray[3, 1] = 5;
          numArray[13, 1] = 5;
          numArray[13, 13] = 5;
          numArray[1, 13] = 5;
          numArray[1, 3] = 5;
          numArray[13, 3] = 5;
          numArray[12, 13] = 5;
          numArray[3, 14] = 5;
          numArray[3, 3] = 5;
          numArray[13, 12] = 5;
          numArray[13, 12] = 5;
          numArray[3, 12] = 5;
          break;
        case 6:
          numArray[4, 5] = 2;
          numArray[12, 10] = 5;
          numArray[10, 9] = 5;
          numArray[5, 12] = 2;
          numArray[5, 9] = 5;
          numArray[12, 12] = 5;
          numArray[3, 4] = 5;
          numArray[2, 3] = 5;
          numArray[11, 3] = 5;
          numArray[10, 6] = 5;
          numArray[5, 9] = 7;
          numArray[10, 12] = 7;
          numArray[3, 12] = 7;
          numArray[10, 8] = 7;
          break;
        case 7:
          for (int index = 0; index < 16; ++index)
          {
            numArray[index, 5] = index % 2 == 0 ? 9 : 8;
            numArray[index, 10] = index % 2 == 0 ? 9 : 8;
          }
          numArray[4, 5] = 10;
          numArray[8, 5] = 10;
          numArray[12, 5] = 10;
          numArray[4, 10] = 10;
          numArray[8, 10] = 10;
          numArray[12, 10] = 10;
          break;
        case 9:
          numArray[4, 4] = 5;
          numArray[5, 4] = 5;
          numArray[10, 4] = 5;
          numArray[12, 4] = 5;
          numArray[4, 5] = 5;
          numArray[5, 5] = 5;
          numArray[10, 5] = 5;
          numArray[12, 5] = 5;
          numArray[4, 10] = 5;
          numArray[5, 10] = 5;
          numArray[10, 10] = 5;
          numArray[12, 10] = 5;
          numArray[4, 12] = 5;
          numArray[5, 12] = 5;
          numArray[10, 12] = 5;
          numArray[12, 12] = 5;
          break;
        case 10:
          for (int index = 0; index < 16; ++index)
          {
            numArray[index, 1] = index % 2 == 0 ? 9 : 8;
            numArray[index, 14] = index % 2 == 0 ? 9 : 8;
          }
          numArray[8, 1] = 10;
          numArray[7, 1] = 10;
          numArray[9, 1] = 10;
          numArray[8, 14] = 10;
          numArray[7, 14] = 10;
          numArray[9, 14] = 10;
          numArray[6, 8] = 5;
          numArray[10, 8] = 5;
          numArray[8, 6] = 5;
          numArray[8, 9] = 5;
          break;
        case 11:
          for (int index = 0; index < 16; ++index)
          {
            numArray[index, 0] = 7;
            numArray[index, 15] = 7;
            if (index % 2 == 0)
            {
              numArray[index, 1] = 5;
              numArray[index, 14] = 5;
            }
          }
          break;
        case 12:
          for (int index1 = 0; index1 < 16; ++index1)
          {
            for (int index2 = 0; index2 < 16; ++index2)
            {
              if (numArray[index1, index2] == 0 || numArray[index1, index2] == 1)
                numArray[index1, index2] = 5;
            }
          }
          for (int index = 0; index < 16; ++index)
          {
            numArray[index, 0] = index % 2 == 0 ? 9 : 8;
            numArray[index, 15] = index % 2 == 0 ? 9 : 8;
          }
          Rectangle r = new Rectangle(1, 1, 14, 14);
          foreach (Vector2 vector2 in Utility.getBorderOfThisRectangle(r))
            numArray[(int) vector2.X, (int) vector2.Y] = 10;
          r.Inflate(-1, -1);
          using (List<Vector2>.Enumerator enumerator = Utility.getBorderOfThisRectangle(r).GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              Vector2 current = enumerator.Current;
              numArray[(int) current.X, (int) current.Y] = 2;
            }
            break;
          }
        default:
          numArray[4, 4] = 5;
          numArray[12, 4] = 5;
          numArray[4, 12] = 5;
          numArray[12, 12] = 5;
          break;
      }
      return numArray;
    }

    public void receiveLeftClick(int x, int y, bool playSound = true)
    {
    }

    public void leftClickHeld(int x, int y)
    {
    }

    public void receiveRightClick(int x, int y, bool playSound = true)
    {
    }

    public void releaseLeftClick(int x, int y)
    {
    }

    public void releaseRightClick(int x, int y)
    {
    }

    public void spawnBullets(int[] directions, Vector2 spawn)
    {
      Point position = new Point((int) spawn.X + 24, (int) spawn.Y + 24 - 6);
      int movementSpeed = (int) this.getMovementSpeed(8f, 2);
      if (directions.Length == 1)
      {
        int direction = directions[0];
        switch (direction)
        {
          case 0:
            position.Y -= 22;
            break;
          case 1:
            position.X += 16;
            position.Y -= 6;
            break;
          case 2:
            position.Y += 10;
            break;
          case 3:
            position.X -= 16;
            position.Y -= 6;
            break;
        }
        this.bullets.Add(new AbigailGame.CowboyBullet(position, direction, this.bulletDamage));
        if (!this.activePowerups.ContainsKey(7) && !this.spreadPistol)
          return;
        switch (direction)
        {
          case 0:
            this.bullets.Add(new AbigailGame.CowboyBullet(new Point(position.X, position.Y), new Point(-2, -8), this.bulletDamage));
            this.bullets.Add(new AbigailGame.CowboyBullet(new Point(position.X, position.Y), new Point(2, -8), this.bulletDamage));
            break;
          case 1:
            this.bullets.Add(new AbigailGame.CowboyBullet(new Point(position.X, position.Y), new Point(8, -2), this.bulletDamage));
            this.bullets.Add(new AbigailGame.CowboyBullet(new Point(position.X, position.Y), new Point(8, 2), this.bulletDamage));
            break;
          case 2:
            this.bullets.Add(new AbigailGame.CowboyBullet(new Point(position.X, position.Y), new Point(-2, 8), this.bulletDamage));
            this.bullets.Add(new AbigailGame.CowboyBullet(new Point(position.X, position.Y), new Point(2, 8), this.bulletDamage));
            break;
          case 3:
            this.bullets.Add(new AbigailGame.CowboyBullet(new Point(position.X, position.Y), new Point(-8, -2), this.bulletDamage));
            this.bullets.Add(new AbigailGame.CowboyBullet(new Point(position.X, position.Y), new Point(-8, 2), this.bulletDamage));
            break;
        }
      }
      else if (((IEnumerable<int>) directions).Contains<int>(0) && ((IEnumerable<int>) directions).Contains<int>(1))
      {
        position.X += AbigailGame.TileSize / 2;
        position.Y -= AbigailGame.TileSize / 2;
        this.bullets.Add(new AbigailGame.CowboyBullet(position, new Point(movementSpeed, -movementSpeed), this.bulletDamage));
        if (!this.activePowerups.ContainsKey(7) && !this.spreadPistol)
          return;
        int num1 = -2;
        this.bullets.Add(new AbigailGame.CowboyBullet(position, new Point(movementSpeed + num1, -movementSpeed + num1), this.bulletDamage));
        int num2 = 2;
        this.bullets.Add(new AbigailGame.CowboyBullet(position, new Point(movementSpeed + num2, -movementSpeed + num2), this.bulletDamage));
      }
      else if (((IEnumerable<int>) directions).Contains<int>(0) && ((IEnumerable<int>) directions).Contains<int>(3))
      {
        position.X -= AbigailGame.TileSize / 2;
        position.Y -= AbigailGame.TileSize / 2;
        this.bullets.Add(new AbigailGame.CowboyBullet(position, new Point(-movementSpeed, -movementSpeed), this.bulletDamage));
        if (!this.activePowerups.ContainsKey(7) && !this.spreadPistol)
          return;
        int num1 = -2;
        this.bullets.Add(new AbigailGame.CowboyBullet(position, new Point(-movementSpeed - num1, -movementSpeed + num1), this.bulletDamage));
        int num2 = 2;
        this.bullets.Add(new AbigailGame.CowboyBullet(position, new Point(-movementSpeed - num2, -movementSpeed + num2), this.bulletDamage));
      }
      else if (((IEnumerable<int>) directions).Contains<int>(2) && ((IEnumerable<int>) directions).Contains<int>(1))
      {
        position.X += AbigailGame.TileSize / 2;
        position.Y += AbigailGame.TileSize / 4;
        this.bullets.Add(new AbigailGame.CowboyBullet(position, new Point(movementSpeed, movementSpeed), this.bulletDamage));
        if (!this.activePowerups.ContainsKey(7) && !this.spreadPistol)
          return;
        int num1 = -2;
        this.bullets.Add(new AbigailGame.CowboyBullet(position, new Point(movementSpeed - num1, movementSpeed + num1), this.bulletDamage));
        int num2 = 2;
        this.bullets.Add(new AbigailGame.CowboyBullet(position, new Point(movementSpeed - num2, movementSpeed + num2), this.bulletDamage));
      }
      else
      {
        if (!((IEnumerable<int>) directions).Contains<int>(2) || !((IEnumerable<int>) directions).Contains<int>(3))
          return;
        position.X -= AbigailGame.TileSize / 2;
        position.Y += AbigailGame.TileSize / 4;
        this.bullets.Add(new AbigailGame.CowboyBullet(position, new Point(-movementSpeed, movementSpeed), this.bulletDamage));
        if (!this.activePowerups.ContainsKey(7) && !this.spreadPistol)
          return;
        int num1 = -2;
        this.bullets.Add(new AbigailGame.CowboyBullet(position, new Point(-movementSpeed + num1, movementSpeed + num1), this.bulletDamage));
        int num2 = 2;
        this.bullets.Add(new AbigailGame.CowboyBullet(position, new Point(-movementSpeed + num2, movementSpeed + num2), this.bulletDamage));
      }
    }

    public bool isSpawnQueueEmpty()
    {
      for (int index = 0; index < 4; ++index)
      {
        if (this.spawnQueue[index].Count > 0)
          return false;
      }
      return true;
    }

    public static bool isMapTilePassable(int tileType)
    {
      switch (tileType)
      {
        case 0:
        case 1:
        case 5:
        case 6:
        case 7:
        case 8:
        case 9:
          return false;
        default:
          return true;
      }
    }

    public static bool isMapTilePassableForMonsters(int tileType)
    {
      switch (tileType)
      {
        case 5:
        case 7:
        case 8:
        case 9:
          return false;
        default:
          return true;
      }
    }

    public static bool isCollidingWithMonster(Rectangle r, AbigailGame.CowboyMonster subject)
    {
      foreach (AbigailGame.CowboyMonster monster in AbigailGame.monsters)
      {
        if ((subject == null || !subject.Equals((object) monster)) && (Math.Abs(monster.position.X - r.X) < 48 && Math.Abs(monster.position.Y - r.Y) < 48) && r.Intersects(new Rectangle(monster.position.X, monster.position.Y, 48, 48)))
          return true;
      }
      return false;
    }

    public static bool isCollidingWithMapForMonsters(Rectangle positionToCheck)
    {
      for (int corner = 0; corner < 4; ++corner)
      {
        Vector2 cornersOfThisRectangle = Utility.getCornersOfThisRectangle(ref positionToCheck, corner);
        if ((double) cornersOfThisRectangle.X < 0.0 || (double) cornersOfThisRectangle.Y < 0.0 || ((double) cornersOfThisRectangle.X >= 768.0 || (double) cornersOfThisRectangle.Y >= 768.0) || !AbigailGame.isMapTilePassableForMonsters(AbigailGame.map[(int) cornersOfThisRectangle.X / 16 / 3, (int) cornersOfThisRectangle.Y / 16 / 3]))
          return true;
      }
      return false;
    }

    public static bool isCollidingWithMap(Rectangle positionToCheck)
    {
      for (int corner = 0; corner < 4; ++corner)
      {
        Vector2 cornersOfThisRectangle = Utility.getCornersOfThisRectangle(ref positionToCheck, corner);
        if ((double) cornersOfThisRectangle.X < 0.0 || (double) cornersOfThisRectangle.Y < 0.0 || ((double) cornersOfThisRectangle.X >= 768.0 || (double) cornersOfThisRectangle.Y >= 768.0) || !AbigailGame.isMapTilePassable(AbigailGame.map[(int) cornersOfThisRectangle.X / 16 / 3, (int) cornersOfThisRectangle.Y / 16 / 3]))
          return true;
      }
      return false;
    }

    public static bool isCollidingWithMap(Point position)
    {
      Rectangle r = new Rectangle(position.X, position.Y, 48, 48);
      for (int corner = 0; corner < 4; ++corner)
      {
        Vector2 cornersOfThisRectangle = Utility.getCornersOfThisRectangle(ref r, corner);
        if ((double) cornersOfThisRectangle.X < 0.0 || (double) cornersOfThisRectangle.Y < 0.0 || ((double) cornersOfThisRectangle.X >= 768.0 || (double) cornersOfThisRectangle.Y >= 768.0) || !AbigailGame.isMapTilePassable(AbigailGame.map[(int) cornersOfThisRectangle.X / 16 / 3, (int) cornersOfThisRectangle.Y / 16 / 3]))
          return true;
      }
      return false;
    }

    public static bool isCollidingWithMap(Vector2 position)
    {
      Rectangle r = new Rectangle((int) position.X, (int) position.Y, 48, 48);
      for (int corner = 0; corner < 4; ++corner)
      {
        Vector2 cornersOfThisRectangle = Utility.getCornersOfThisRectangle(ref r, corner);
        if ((double) cornersOfThisRectangle.X < 0.0 || (double) cornersOfThisRectangle.Y < 0.0 || ((double) cornersOfThisRectangle.X >= 768.0 || (double) cornersOfThisRectangle.Y >= 768.0) || !AbigailGame.isMapTilePassable(AbigailGame.map[(int) cornersOfThisRectangle.X / 16 / 3, (int) cornersOfThisRectangle.Y / 16 / 3]))
          return true;
      }
      return false;
    }

    private void addPlayer2MovementDirection(int direction)
    {
      if (this.player2MovementDirections.Contains(direction))
        return;
      if (this.player2MovementDirections.Count == 1 && direction == (this.player2MovementDirections[0] + 2) % 4)
        this.player2MovementDirections.Clear();
      this.player2MovementDirections.Add(direction);
      if (this.player2MovementDirections.Count <= 2)
        return;
      this.player2MovementDirections.RemoveAt(0);
    }

    private void addPlayerMovementDirection(int direction)
    {
      if (AbigailGame.playerMovementDirections.Contains(direction))
        return;
      if (AbigailGame.playerMovementDirections.Count == 1)
      {
        int num = (AbigailGame.playerMovementDirections.ElementAt<int>(0) + 2) % 4;
      }
      AbigailGame.playerMovementDirections.Add(direction);
    }

    private void addPlayer2ShootingDirection(int direction)
    {
      if (this.player2ShootingDirections.Contains(direction))
        return;
      if (this.player2ShootingDirections.Count == 1 && direction == (this.player2ShootingDirections[0] + 2) % 4)
        this.player2ShootingDirections.Clear();
      this.player2ShootingDirections.Add(direction);
      if (this.player2ShootingDirections.Count <= 2)
        return;
      this.player2ShootingDirections.RemoveAt(0);
    }

    private void addPlayerShootingDirection(int direction)
    {
      if (AbigailGame.playerShootingDirections.Contains(direction))
        return;
      AbigailGame.playerShootingDirections.Add(direction);
    }

    public void startShoppingLevel()
    {
      this.merchantBox.Y = -AbigailGame.TileSize;
      AbigailGame.shopping = true;
      AbigailGame.merchantArriving = true;
      AbigailGame.merchantLeaving = false;
      AbigailGame.merchantShopOpen = false;
      if (AbigailGame.overworldSong != null)
        AbigailGame.overworldSong.Stop(AudioStopOptions.Immediate);
      AbigailGame.monsters.Clear();
      AbigailGame.waitingForPlayerToMoveDownAMap = true;
      this.storeItems.Clear();
      if (AbigailGame.whichWave == 2)
      {
        this.storeItems.Add(new Rectangle(7 * AbigailGame.TileSize + 12, 8 * AbigailGame.TileSize - AbigailGame.TileSize * 2, AbigailGame.TileSize, AbigailGame.TileSize), 3);
        this.storeItems.Add(new Rectangle(8 * AbigailGame.TileSize + 24, 8 * AbigailGame.TileSize - AbigailGame.TileSize * 2, AbigailGame.TileSize, AbigailGame.TileSize), 0);
        this.storeItems.Add(new Rectangle(9 * AbigailGame.TileSize + 36, 8 * AbigailGame.TileSize - AbigailGame.TileSize * 2, AbigailGame.TileSize, AbigailGame.TileSize), 6);
      }
      else
      {
        this.storeItems.Add(new Rectangle(7 * AbigailGame.TileSize + 12, 8 * AbigailGame.TileSize - AbigailGame.TileSize * 2, AbigailGame.TileSize, AbigailGame.TileSize), this.runSpeedLevel >= 2 ? 5 : 3 + this.runSpeedLevel);
        this.storeItems.Add(new Rectangle(8 * AbigailGame.TileSize + 24, 8 * AbigailGame.TileSize - AbigailGame.TileSize * 2, AbigailGame.TileSize, AbigailGame.TileSize), this.fireSpeedLevel >= 3 ? (this.ammoLevel < 3 || this.spreadPistol ? 10 : 9) : this.fireSpeedLevel);
        this.storeItems.Add(new Rectangle(9 * AbigailGame.TileSize + 36, 8 * AbigailGame.TileSize - AbigailGame.TileSize * 2, AbigailGame.TileSize, AbigailGame.TileSize), this.ammoLevel < 3 ? 6 + this.ammoLevel : 10);
      }
      if (this.whichRound <= 0)
        return;
      this.storeItems.Clear();
      this.storeItems.Add(new Rectangle(7 * AbigailGame.TileSize + 12, 8 * AbigailGame.TileSize - AbigailGame.TileSize * 2, AbigailGame.TileSize, AbigailGame.TileSize), this.runSpeedLevel >= 2 ? 5 : 3 + this.runSpeedLevel);
      this.storeItems.Add(new Rectangle(8 * AbigailGame.TileSize + 24, 8 * AbigailGame.TileSize - AbigailGame.TileSize * 2, AbigailGame.TileSize, AbigailGame.TileSize), this.fireSpeedLevel >= 3 ? (this.ammoLevel < 3 || this.spreadPistol ? 10 : 9) : this.fireSpeedLevel);
      this.storeItems.Add(new Rectangle(9 * AbigailGame.TileSize + 36, 8 * AbigailGame.TileSize - AbigailGame.TileSize * 2, AbigailGame.TileSize, AbigailGame.TileSize), this.ammoLevel < 3 ? 6 + this.ammoLevel : 10);
    }

    public void receiveKeyPress(Keys k)
    {
      Vector2 playerPosition = this.playerPosition;
      if (Game1.options.gamepadControls && (Game1.isAnyGamePadButtonBeingPressed() || Game1.isGamePadThumbstickInMotion(0.2)))
      {
        Game1.thumbstickMotionMargin = 0;
        if (Game1.isGamePadThumbstickInMotion(0.2) && (Game1.options.doesInputListContain(Game1.options.moveUpButton, Keys.Up) || Game1.options.doesInputListContain(Game1.options.moveRightButton, Keys.Right) || (Game1.options.doesInputListContain(Game1.options.moveDownButton, Keys.Down) || Game1.options.doesInputListContain(Game1.options.moveLeftButton, Keys.Left))))
          return;
        if (Game1.options.doesInputListContain(Game1.options.moveUpButton, k))
          k = Keys.W;
        else if (Game1.options.doesInputListContain(Game1.options.moveRightButton, k))
          k = Keys.D;
        else if (Game1.options.doesInputListContain(Game1.options.moveDownButton, k))
          k = Keys.S;
        else if (Game1.options.doesInputListContain(Game1.options.moveLeftButton, k))
          k = Keys.A;
        else if (k != Keys.None && Game1.options.doesInputListContain(Game1.options.actionButton, k))
          k = AbigailGame.gameOver ? Keys.X : Keys.Down;
        else if (k != Keys.None && Game1.options.doesInputListContain(Game1.options.useToolButton, k))
          k = Keys.Left;
        else if (Game1.options.doesInputListContain(Game1.options.toolSwapButton, k) || GamePad.GetState(Game1.playerOneIndex).IsButtonDown(Buttons.Start) && !Game1.oldPadState.IsButtonDown(Buttons.Start) || GamePad.GetState(Game1.playerOneIndex).IsButtonDown(Buttons.RightTrigger) && !Game1.oldPadState.IsButtonDown(Buttons.RightTrigger))
          k = Keys.Space;
        else if (Game1.options.doesInputListContain(Game1.options.journalButton, k))
          k = Keys.Escape;
        else if (Game1.options.doesInputListContain(Game1.options.menuButton, k))
          k = !GamePad.GetState(Game1.playerOneIndex).IsButtonDown(Buttons.Y) || Game1.oldPadState.IsButtonDown(Buttons.Y) ? (!GamePad.GetState(Game1.playerOneIndex).IsButtonDown(Buttons.B) || Game1.oldPadState.IsButtonDown(Buttons.B) ? k : Keys.Right) : Keys.Up;
      }
      else
      {
        bool flag = false;
        if (Game1.options.doesInputListContain(Game1.options.moveUpButton, k) && !Game1.options.doesInputListContain(Game1.options.moveUpButton, Keys.Up))
        {
          this.addPlayerMovementDirection(0);
          if (AbigailGame.gameOver)
          {
            this.gameOverOption = Math.Max(0, this.gameOverOption - 1);
            Game1.playSound("Cowboy_gunshot");
          }
          flag = true;
        }
        if (Game1.options.doesInputListContain(Game1.options.moveLeftButton, k) && !Game1.options.doesInputListContain(Game1.options.moveLeftButton, Keys.Left))
        {
          this.addPlayerMovementDirection(3);
          flag = true;
        }
        if (Game1.options.doesInputListContain(Game1.options.moveDownButton, k) && !Game1.options.doesInputListContain(Game1.options.moveDownButton, Keys.Down))
        {
          this.addPlayerMovementDirection(2);
          if (AbigailGame.gameOver)
          {
            this.gameOverOption = Math.Min(1, this.gameOverOption + 1);
            Game1.playSound("Cowboy_gunshot");
          }
          flag = true;
        }
        if (Game1.options.doesInputListContain(Game1.options.moveRightButton, k) && !Game1.options.doesInputListContain(Game1.options.moveRightButton, Keys.Right))
        {
          this.addPlayerMovementDirection(1);
          flag = true;
        }
        if (flag)
          return;
      }
      if (AbigailGame.onStartMenu)
      {
        AbigailGame.startTimer = 1;
        Game1.playSound("Pickup_Coin15");
      }
      else
      {
        switch (k)
        {
          case Keys.W:
            this.addPlayerMovementDirection(0);
            if (!AbigailGame.gameOver)
              break;
            this.gameOverOption = Math.Max(0, this.gameOverOption - 1);
            Game1.playSound("Cowboy_gunshot");
            break;
          case Keys.X:
          case Keys.Space:
          case Keys.Enter:
            if (AbigailGame.gameOver)
            {
              if (this.gameOverOption == 1)
              {
                this.quit = true;
                break;
              }
              this.gamerestartTimer = 1500;
              AbigailGame.gameOver = false;
              this.gameOverOption = 0;
              Game1.playSound("Pickup_Coin15");
              break;
            }
            if (this.heldItem == null || (double) AbigailGame.deathTimer > 0.0 || AbigailGame.zombieModeTimer > 0)
              break;
            this.usePowerup(this.heldItem.which);
            this.heldItem = (AbigailGame.CowboyPowerup) null;
            break;
          case Keys.D:
            this.addPlayerMovementDirection(1);
            break;
          case Keys.S:
            this.addPlayerMovementDirection(2);
            if (!AbigailGame.gameOver)
              break;
            this.gameOverOption = Math.Min(1, this.gameOverOption + 1);
            Game1.playSound("Cowboy_gunshot");
            break;
          case Keys.Left:
            this.addPlayerShootingDirection(3);
            break;
          case Keys.Up:
            this.addPlayerShootingDirection(0);
            if (!AbigailGame.gameOver)
              break;
            this.gameOverOption = Math.Max(0, this.gameOverOption - 1);
            Game1.playSound("Cowboy_gunshot");
            break;
          case Keys.Right:
            this.addPlayerShootingDirection(1);
            break;
          case Keys.Down:
            this.addPlayerShootingDirection(2);
            if (!AbigailGame.gameOver)
              break;
            this.gameOverOption = Math.Min(1, this.gameOverOption + 1);
            Game1.playSound("Cowboy_gunshot");
            break;
          case Keys.A:
            this.addPlayerMovementDirection(3);
            break;
          case Keys.Escape:
            if (AbigailGame.playingWithAbigail)
              break;
            Game1.currentMinigame = (IMinigame) null;
            if (AbigailGame.overworldSong != null && AbigailGame.overworldSong.IsPlaying)
              AbigailGame.overworldSong.Stop(AudioStopOptions.Immediate);
            if (AbigailGame.outlawSong != null && AbigailGame.outlawSong.IsPlaying)
              AbigailGame.outlawSong.Stop(AudioStopOptions.Immediate);
            if (Game1.currentLocation == null || !Game1.currentLocation.name.Equals("Saloon") || Game1.timeOfDay < 1700)
              break;
            Game1.changeMusicTrack("Saloon1");
            break;
        }
      }
    }

    public void receiveKeyRelease(Keys k)
    {
      if (Game1.options.gamepadControls && Utility.getPressedButtons(Game1.oldPadState, GamePad.GetState(Game1.playerOneIndex)).Count > 0)
      {
        if (k != Keys.None)
        {
          if (Game1.isGamePadThumbstickInMotion(0.2) && (Game1.options.doesInputListContain(Game1.options.moveUpButton, Keys.Up) || Game1.options.doesInputListContain(Game1.options.moveRightButton, Keys.Right) || (Game1.options.doesInputListContain(Game1.options.moveDownButton, Keys.Down) || Game1.options.doesInputListContain(Game1.options.moveLeftButton, Keys.Left))))
            return;
          if (Game1.options.doesInputListContain(Game1.options.moveUpButton, k))
            k = Keys.W;
          else if (Game1.options.doesInputListContain(Game1.options.moveRightButton, k))
            k = Keys.D;
          else if (Game1.options.doesInputListContain(Game1.options.moveDownButton, k))
            k = Keys.S;
          else if (Game1.options.doesInputListContain(Game1.options.moveLeftButton, k))
            k = Keys.A;
          else if (Game1.options.doesInputListContain(Game1.options.actionButton, k))
            k = Keys.Down;
          else if (Game1.options.doesInputListContain(Game1.options.useToolButton, k))
            k = Keys.Left;
          else if (Game1.options.doesInputListContain(Game1.options.toolSwapButton, k))
            k = Keys.Space;
          else if (Game1.options.doesInputListContain(Game1.options.menuButton, k))
          {
            GamePadState state;
            int num;
            if (Game1.oldPadState.IsButtonDown(Buttons.Y))
            {
              state = GamePad.GetState(Game1.playerOneIndex);
              if (!state.IsButtonDown(Buttons.Y))
              {
                num = 38;
                goto label_26;
              }
            }
            if (Game1.oldPadState.IsButtonDown(Buttons.B))
            {
              state = GamePad.GetState(Game1.playerOneIndex);
              if (!state.IsButtonDown(Buttons.B))
              {
                num = 39;
                goto label_26;
              }
            }
            num = (int) k;
label_26:
            k = (Keys) num;
          }
        }
      }
      else
      {
        bool flag = false;
        if (Game1.options.doesInputListContain(Game1.options.moveUpButton, k) && !Game1.options.doesInputListContain(Game1.options.moveUpButton, Keys.Up))
        {
          if (AbigailGame.playerMovementDirections.Contains(0))
            AbigailGame.playerMovementDirections.Remove(0);
          flag = true;
        }
        if (Game1.options.doesInputListContain(Game1.options.moveLeftButton, k) && !Game1.options.doesInputListContain(Game1.options.moveLeftButton, Keys.Left))
        {
          if (AbigailGame.playerMovementDirections.Contains(3))
            AbigailGame.playerMovementDirections.Remove(3);
          flag = true;
        }
        if (Game1.options.doesInputListContain(Game1.options.moveDownButton, k) && !Game1.options.doesInputListContain(Game1.options.moveDownButton, Keys.Down))
        {
          if (AbigailGame.playerMovementDirections.Contains(2))
            AbigailGame.playerMovementDirections.Remove(2);
          flag = true;
        }
        if (Game1.options.doesInputListContain(Game1.options.moveRightButton, k) && !Game1.options.doesInputListContain(Game1.options.moveRightButton, Keys.Right))
        {
          if (AbigailGame.playerMovementDirections.Contains(1))
            AbigailGame.playerMovementDirections.Remove(1);
          flag = true;
        }
        if (flag)
          return;
      }
      if (k <= Keys.A)
      {
        switch (k - 37)
        {
          case Keys.None:
            if (!AbigailGame.playerShootingDirections.Contains(3))
              break;
            AbigailGame.playerShootingDirections.Remove(3);
            break;
          case (Keys) 1:
            if (!AbigailGame.playerShootingDirections.Contains(0))
              break;
            AbigailGame.playerShootingDirections.Remove(0);
            break;
          case (Keys) 2:
            if (!AbigailGame.playerShootingDirections.Contains(1))
              break;
            AbigailGame.playerShootingDirections.Remove(1);
            break;
          case (Keys) 3:
            if (!AbigailGame.playerShootingDirections.Contains(2))
              break;
            AbigailGame.playerShootingDirections.Remove(2);
            break;
          default:
            if (k != Keys.A || !AbigailGame.playerMovementDirections.Contains(3))
              break;
            AbigailGame.playerMovementDirections.Remove(3);
            break;
        }
      }
      else if (k != Keys.D)
      {
        if (k != Keys.S)
        {
          if (k != Keys.W || !AbigailGame.playerMovementDirections.Contains(0))
            return;
          AbigailGame.playerMovementDirections.Remove(0);
        }
        else
        {
          if (!AbigailGame.playerMovementDirections.Contains(2))
            return;
          AbigailGame.playerMovementDirections.Remove(2);
        }
      }
      else
      {
        if (!AbigailGame.playerMovementDirections.Contains(1))
          return;
        AbigailGame.playerMovementDirections.Remove(1);
      }
    }

    public int getPriceForItem(int whichItem)
    {
      switch (whichItem)
      {
        case 0:
          return 10;
        case 1:
          return 20;
        case 2:
          return 30;
        case 3:
          return 8;
        case 4:
          return 20;
        case 5:
          return 10;
        case 6:
          return 15;
        case 7:
          return 30;
        case 8:
          return 45;
        case 9:
          return 99;
        case 10:
          return 10;
        default:
          return 5;
      }
    }

    public void draw(SpriteBatch b)
    {
      b.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.PointClamp, (DepthStencilState) null, (RasterizerState) null);
      if (AbigailGame.onStartMenu)
      {
        b.Draw(Game1.staminaRect, new Rectangle((int) AbigailGame.topLeftScreenCoordinate.X, (int) AbigailGame.topLeftScreenCoordinate.Y, 16 * AbigailGame.TileSize, 16 * AbigailGame.TileSize), new Rectangle?(Game1.staminaRect.Bounds), Color.Black, 0.0f, Vector2.Zero, SpriteEffects.None, 0.97f);
        b.Draw(Game1.mouseCursors, new Vector2((float) (Game1.graphics.GraphicsDevice.Viewport.TitleSafeArea.Width / 2 - 3 * AbigailGame.TileSize), AbigailGame.topLeftScreenCoordinate.Y + (float) (5 * AbigailGame.TileSize)), new Rectangle?(new Rectangle(128, 1744, 96, 72 - (AbigailGame.startTimer > 0 ? 16 : 0))), Color.White, 0.0f, Vector2.Zero, 3f, SpriteEffects.None, 1f);
      }
      else if ((AbigailGame.gameOver || this.gamerestartTimer > 0) && !AbigailGame.endCutscene)
      {
        b.Draw(Game1.staminaRect, new Rectangle((int) AbigailGame.topLeftScreenCoordinate.X, (int) AbigailGame.topLeftScreenCoordinate.Y, 16 * AbigailGame.TileSize, 16 * AbigailGame.TileSize), new Rectangle?(Game1.staminaRect.Bounds), Color.Black, 0.0f, Vector2.Zero, SpriteEffects.None, 0.0001f);
        b.DrawString(Game1.dialogueFont, Game1.content.LoadString("Strings\\StringsFromCSFiles:AbigailGame.cs.11914"), AbigailGame.topLeftScreenCoordinate + new Vector2(6f, 7f) * (float) AbigailGame.TileSize, Color.White, 0.0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
        b.DrawString(Game1.dialogueFont, Game1.content.LoadString("Strings\\StringsFromCSFiles:AbigailGame.cs.11914"), AbigailGame.topLeftScreenCoordinate + new Vector2(6f, 7f) * (float) AbigailGame.TileSize + new Vector2(-1f, 0.0f), Color.White, 0.0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
        b.DrawString(Game1.dialogueFont, Game1.content.LoadString("Strings\\StringsFromCSFiles:AbigailGame.cs.11914"), AbigailGame.topLeftScreenCoordinate + new Vector2(6f, 7f) * (float) AbigailGame.TileSize + new Vector2(1f, 0.0f), Color.White, 0.0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
        string text1 = Game1.content.LoadString("Strings\\StringsFromCSFiles:AbigailGame.cs.11917");
        if (this.gameOverOption == 0)
          text1 = "> " + text1;
        string text2 = Game1.content.LoadString("Strings\\StringsFromCSFiles:AbigailGame.cs.11919");
        if (this.gameOverOption == 1)
          text2 = "> " + text2;
        if (this.gamerestartTimer <= 0 || this.gamerestartTimer / 500 % 2 == 0)
          b.DrawString(Game1.smallFont, text1, AbigailGame.topLeftScreenCoordinate + new Vector2(6f, 9f) * (float) AbigailGame.TileSize, Color.White, 0.0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
        b.DrawString(Game1.smallFont, text2, AbigailGame.topLeftScreenCoordinate + new Vector2(6f, 9f) * (float) AbigailGame.TileSize + new Vector2(0.0f, (float) (AbigailGame.TileSize * 2 / 3)), Color.White, 0.0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
      }
      else if (AbigailGame.endCutscene)
      {
        switch (AbigailGame.endCutscenePhase)
        {
          case 0:
            b.Draw(Game1.staminaRect, new Rectangle((int) AbigailGame.topLeftScreenCoordinate.X, (int) AbigailGame.topLeftScreenCoordinate.Y, 16 * AbigailGame.TileSize, 16 * AbigailGame.TileSize), new Rectangle?(Game1.staminaRect.Bounds), Color.Black, 0.0f, Vector2.Zero, SpriteEffects.None, 0.0001f);
            b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + this.playerPosition + new Vector2(0.0f, (float) (-AbigailGame.TileSize / 4)), new Rectangle?(new Rectangle(384, 1760, 16, 16)), Color.White * (AbigailGame.endCutsceneTimer < 2000 ? (float) (1.0 * ((double) AbigailGame.endCutsceneTimer / 2000.0)) : 1f), 0.0f, Vector2.Zero, 3f, SpriteEffects.None, (float) ((double) this.playerPosition.Y / 10000.0 + 1.0 / 1000.0));
            b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + this.playerPosition + new Vector2(0.0f, (float) (-AbigailGame.TileSize * 2 / 3)) + new Vector2(0.0f, (float) (-AbigailGame.TileSize / 4)), new Rectangle?(new Rectangle(320 + AbigailGame.itemToHold * 16, 1776, 16, 16)), Color.White * (AbigailGame.endCutsceneTimer < 2000 ? (float) (1.0 * ((double) AbigailGame.endCutsceneTimer / 2000.0)) : 1f), 0.0f, Vector2.Zero, 3f, SpriteEffects.None, (float) ((double) this.playerPosition.Y / 10000.0 + 1.0 / 500.0));
            break;
          case 1:
          case 2:
          case 3:
            for (int index1 = 0; index1 < 16; ++index1)
            {
              for (int index2 = 0; index2 < 16; ++index2)
                b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + new Vector2((float) index1, (float) index2) * 16f * 3f + new Vector2(0.0f, (float) (AbigailGame.newMapPosition - 16 * AbigailGame.TileSize)), new Rectangle?(new Rectangle(464 + 16 * AbigailGame.map[index1, index2] + (AbigailGame.map[index1, index2] != 5 || (double) this.cactusDanceTimer <= 800.0 ? 0 : 16), 1680 - AbigailGame.world * 16, 16, 16)), Color.White, 0.0f, Vector2.Zero, 3f, SpriteEffects.None, 0.0f);
            }
            b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + new Vector2((float) (6 * AbigailGame.TileSize), (float) (3 * AbigailGame.TileSize)), new Rectangle?(new Rectangle(288, 1697, 64, 80)), Color.White, 0.0f, Vector2.Zero, 3f, SpriteEffects.None, 0.01f);
            if (AbigailGame.endCutscenePhase == 3)
            {
              b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + new Vector2((float) (9 * AbigailGame.TileSize), (float) (7 * AbigailGame.TileSize)), new Rectangle?(new Rectangle(544, 1792, 32, 32)), Color.White, 0.0f, Vector2.Zero, 3f, SpriteEffects.None, 0.05f);
              if (AbigailGame.endCutsceneTimer < 3000)
              {
                b.Draw(Game1.staminaRect, new Rectangle((int) AbigailGame.topLeftScreenCoordinate.X, (int) AbigailGame.topLeftScreenCoordinate.Y, 16 * AbigailGame.TileSize, 16 * AbigailGame.TileSize), new Rectangle?(Game1.staminaRect.Bounds), Color.Black * (float) (1.0 - (double) AbigailGame.endCutsceneTimer / 3000.0), 0.0f, Vector2.Zero, SpriteEffects.None, 1f);
                break;
              }
              break;
            }
            b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + new Vector2((float) (10 * AbigailGame.TileSize), (float) (8 * AbigailGame.TileSize)), new Rectangle?(new Rectangle(272 - AbigailGame.endCutsceneTimer / 300 % 4 * 16, 1792, 16, 16)), Color.White, 0.0f, Vector2.Zero, 3f, SpriteEffects.None, 0.02f);
            if (AbigailGame.endCutscenePhase == 2)
            {
              b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + this.playerPosition + new Vector2(4f, 13f) * 3f, new Rectangle?(new Rectangle(484, 1760 + (int) ((double) this.playerMotionAnimationTimer / 100.0) * 3, 8, 3)), Color.White, 0.0f, Vector2.Zero, 3f, SpriteEffects.None, (float) ((double) this.playerPosition.Y / 10000.0 + 1.0 / 1000.0 + 1.0 / 1000.0));
              b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + this.playerPosition, new Rectangle?(new Rectangle(384, 1760, 16, 13)), Color.White, 0.0f, Vector2.Zero, 3f, SpriteEffects.None, (float) ((double) this.playerPosition.Y / 10000.0 + 1.0 / 500.0 + 1.0 / 1000.0));
              b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + this.playerPosition + new Vector2(0.0f, (float) (-AbigailGame.TileSize * 2 / 3 - AbigailGame.TileSize / 4)), new Rectangle?(new Rectangle(320 + AbigailGame.itemToHold * 16, 1776, 16, 16)), Color.White, 0.0f, Vector2.Zero, 3f, SpriteEffects.None, (float) ((double) this.playerPosition.Y / 10000.0 + 0.00499999988824129));
            }
            b.Draw(Game1.staminaRect, new Rectangle((int) AbigailGame.topLeftScreenCoordinate.X, (int) AbigailGame.topLeftScreenCoordinate.Y, 16 * AbigailGame.TileSize, 16 * AbigailGame.TileSize), new Rectangle?(Game1.staminaRect.Bounds), Color.Black * (AbigailGame.endCutscenePhase != 1 || AbigailGame.endCutsceneTimer <= 12500 ? 0.0f : (float) ((AbigailGame.endCutsceneTimer - 12500) / 3000)), 0.0f, Vector2.Zero, SpriteEffects.None, 1f);
            break;
          case 4:
          case 5:
            b.Draw(Game1.staminaRect, new Rectangle((int) AbigailGame.topLeftScreenCoordinate.X, (int) AbigailGame.topLeftScreenCoordinate.Y, 16 * AbigailGame.TileSize, 16 * AbigailGame.TileSize), new Rectangle?(Game1.staminaRect.Bounds), Color.Black, 0.0f, Vector2.Zero, SpriteEffects.None, 0.97f);
            b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + new Vector2((float) (6 * AbigailGame.TileSize), (float) (3 * AbigailGame.TileSize)), new Rectangle?(new Rectangle(224, 1744, 64, 48)), Color.White * (AbigailGame.endCutsceneTimer > 0 ? (float) (1.0 - ((double) AbigailGame.endCutsceneTimer - 2000.0) / 2000.0) : 1f), 0.0f, Vector2.Zero, 3f, SpriteEffects.None, 1f);
            if (AbigailGame.endCutscenePhase == 5 && this.gamerestartTimer <= 0)
            {
              b.DrawString(Game1.smallFont, Game1.content.LoadString("Strings\\Locations:Saloon_Arcade_PK_NewGame+"), AbigailGame.topLeftScreenCoordinate + new Vector2(3f, 10f) * (float) AbigailGame.TileSize, Color.White, 0.0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
              break;
            }
            break;
        }
      }
      else
      {
        if (AbigailGame.zombieModeTimer > 8200)
        {
          b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + this.playerPosition, new Rectangle?(new Rectangle(384 + (AbigailGame.zombieModeTimer / 200 % 2 == 0 ? 16 : 0), 1760, 16, 16)), Color.White, 0.0f, Vector2.Zero, 3f, SpriteEffects.None, 1f);
          int num = (int) ((double) this.playerPosition.Y - (double) AbigailGame.TileSize);
          while (num > -AbigailGame.TileSize)
          {
            b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + new Vector2(this.playerPosition.X, (float) num), new Rectangle?(new Rectangle(368 + (num / AbigailGame.TileSize % 3 == 0 ? 16 : 0), 1744, 16, 16)), Color.White, 0.0f, Vector2.Zero, 3f, SpriteEffects.None, 1f);
            num -= AbigailGame.TileSize;
          }
          b.End();
          return;
        }
        for (int index1 = 0; index1 < 16; ++index1)
        {
          for (int index2 = 0; index2 < 16; ++index2)
            b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + new Vector2((float) index1, (float) index2) * 16f * 3f + new Vector2(0.0f, (float) (AbigailGame.newMapPosition - 16 * AbigailGame.TileSize)), new Rectangle?(new Rectangle(464 + 16 * AbigailGame.map[index1, index2] + (AbigailGame.map[index1, index2] != 5 || (double) this.cactusDanceTimer <= 800.0 ? 0 : 16), 1680 - AbigailGame.world * 16, 16, 16)), Color.White, 0.0f, Vector2.Zero, 3f, SpriteEffects.None, 0.0f);
        }
        if (AbigailGame.scrollingMap)
        {
          for (int index1 = 0; index1 < 16; ++index1)
          {
            for (int index2 = 0; index2 < 16; ++index2)
              b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + new Vector2((float) index1, (float) index2) * 16f * 3f + new Vector2(0.0f, (float) AbigailGame.newMapPosition), new Rectangle?(new Rectangle(464 + 16 * AbigailGame.nextMap[index1, index2] + (AbigailGame.nextMap[index1, index2] != 5 || (double) this.cactusDanceTimer <= 800.0 ? 0 : 16), 1680 - AbigailGame.world * 16, 16, 16)), Color.White, 0.0f, Vector2.Zero, 3f, SpriteEffects.None, 0.0f);
          }
          b.Draw(Game1.staminaRect, new Rectangle((int) AbigailGame.topLeftScreenCoordinate.X, -1, 16 * AbigailGame.TileSize, (int) AbigailGame.topLeftScreenCoordinate.Y), new Rectangle?(Game1.staminaRect.Bounds), Color.Black, 0.0f, Vector2.Zero, SpriteEffects.None, 1f);
          b.Draw(Game1.staminaRect, new Rectangle((int) AbigailGame.topLeftScreenCoordinate.X, (int) AbigailGame.topLeftScreenCoordinate.Y + 16 * AbigailGame.TileSize, 16 * AbigailGame.TileSize, (int) AbigailGame.topLeftScreenCoordinate.Y + 2), new Rectangle?(Game1.staminaRect.Bounds), Color.Black, 0.0f, Vector2.Zero, SpriteEffects.None, 1f);
        }
        if ((double) AbigailGame.deathTimer <= 0.0 && (AbigailGame.playerInvincibleTimer <= 0 || AbigailGame.playerInvincibleTimer / 100 % 2 == 0))
        {
          if (AbigailGame.holdItemTimer > 0)
          {
            b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + this.playerPosition + new Vector2(0.0f, (float) (-AbigailGame.TileSize / 4)), new Rectangle?(new Rectangle(384, 1760, 16, 16)), Color.White, 0.0f, Vector2.Zero, 3f, SpriteEffects.None, (float) ((double) this.playerPosition.Y / 10000.0 + 1.0 / 1000.0));
            b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + this.playerPosition + new Vector2(0.0f, (float) (-AbigailGame.TileSize * 2 / 3)) + new Vector2(0.0f, (float) (-AbigailGame.TileSize / 4)), new Rectangle?(new Rectangle(320 + AbigailGame.itemToHold * 16, 1776, 16, 16)), Color.White, 0.0f, Vector2.Zero, 3f, SpriteEffects.None, (float) ((double) this.playerPosition.Y / 10000.0 + 1.0 / 500.0));
          }
          else if (AbigailGame.zombieModeTimer > 0)
            b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + this.playerPosition + new Vector2(0.0f, (float) (-AbigailGame.TileSize / 4)), new Rectangle?(new Rectangle(352 + (AbigailGame.zombieModeTimer / 50 % 2 == 0 ? 16 : 0), 1760, 16, 16)), Color.White, 0.0f, Vector2.Zero, 3f, SpriteEffects.None, (float) ((double) this.playerPosition.Y / 10000.0 + 1.0 / 1000.0));
          else if (AbigailGame.playerMovementDirections.Count == 0 && AbigailGame.playerShootingDirections.Count == 0)
          {
            b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + this.playerPosition + new Vector2(0.0f, (float) (-AbigailGame.TileSize / 4)), new Rectangle?(new Rectangle(496, 1760, 16, 16)), Color.White, 0.0f, Vector2.Zero, 3f, SpriteEffects.None, (float) ((double) this.playerPosition.Y / 10000.0 + 1.0 / 1000.0));
          }
          else
          {
            int num = AbigailGame.playerShootingDirections.Count == 0 ? AbigailGame.playerMovementDirections.ElementAt<int>(0) : AbigailGame.playerShootingDirections.Last<int>();
            b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + this.playerPosition + new Vector2(0.0f, (float) (-AbigailGame.TileSize / 4)) + new Vector2(4f, 13f) * 3f, new Rectangle?(new Rectangle(483, 1760 + (int) ((double) this.playerMotionAnimationTimer / 100.0) * 3, 10, 3)), Color.White, 0.0f, Vector2.Zero, 3f, SpriteEffects.None, (float) ((double) this.playerPosition.Y / 10000.0 + 1.0 / 1000.0 + 1.0 / 1000.0));
            b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + this.playerPosition + new Vector2(3f, (float) (-AbigailGame.TileSize / 4)), new Rectangle?(new Rectangle(464 + num * 16, 1744, 16, 16)), Color.White, 0.0f, Vector2.Zero, 3f, SpriteEffects.None, (float) ((double) this.playerPosition.Y / 10000.0 + 1.0 / 500.0 + 1.0 / 1000.0));
          }
        }
        if (AbigailGame.playingWithAbigail && this.player2deathtimer <= 0 && (this.player2invincibletimer <= 0 || this.player2invincibletimer / 100 % 2 == 0))
        {
          if (this.player2MovementDirections.Count == 0 && this.player2ShootingDirections.Count == 0)
          {
            b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + AbigailGame.player2Position + new Vector2(0.0f, (float) (-AbigailGame.TileSize / 4)), new Rectangle?(new Rectangle(256, 1728, 16, 16)), Color.White, 0.0f, Vector2.Zero, 3f, SpriteEffects.None, (float) ((double) this.playerPosition.Y / 10000.0 + 1.0 / 1000.0));
          }
          else
          {
            int num = this.player2ShootingDirections.Count == 0 ? this.player2MovementDirections[0] : this.player2ShootingDirections[0];
            b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + AbigailGame.player2Position + new Vector2(0.0f, (float) (-AbigailGame.TileSize / 4)) + new Vector2(4f, 13f) * 3f, new Rectangle?(new Rectangle(243, 1728 + this.player2AnimationTimer / 100 * 3, 10, 3)), Color.White, 0.0f, Vector2.Zero, 3f, SpriteEffects.None, (float) ((double) AbigailGame.player2Position.Y / 10000.0 + 1.0 / 1000.0 + 1.0 / 1000.0));
            b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + AbigailGame.player2Position + new Vector2(0.0f, (float) (-AbigailGame.TileSize / 4)), new Rectangle?(new Rectangle(224 + num * 16, 1712, 16, 16)), Color.White, 0.0f, Vector2.Zero, 3f, SpriteEffects.None, (float) ((double) AbigailGame.player2Position.Y / 10000.0 + 1.0 / 500.0 + 1.0 / 1000.0));
          }
        }
        foreach (TemporaryAnimatedSprite temporarySprite in AbigailGame.temporarySprites)
          temporarySprite.draw(b, true, 0, 0);
        foreach (AbigailGame.CowboyPowerup powerup in AbigailGame.powerups)
          powerup.draw(b);
        foreach (AbigailGame.CowboyBullet bullet in this.bullets)
          b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + new Vector2((float) bullet.position.X, (float) bullet.position.Y), new Rectangle?(new Rectangle(518, 1760 + (this.bulletDamage - 1) * 4, 4, 4)), Color.White, 0.0f, Vector2.Zero, 3f, SpriteEffects.None, 0.9f);
        foreach (AbigailGame.CowboyBullet enemyBullet in AbigailGame.enemyBullets)
          b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + new Vector2((float) enemyBullet.position.X, (float) enemyBullet.position.Y), new Rectangle?(new Rectangle(523, 1760, 5, 5)), Color.White, 0.0f, Vector2.Zero, 3f, SpriteEffects.None, 0.9f);
        if (AbigailGame.shopping)
        {
          if ((AbigailGame.merchantArriving || AbigailGame.merchantLeaving) && !AbigailGame.merchantShopOpen)
          {
            b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + new Vector2((float) this.merchantBox.Location.X, (float) this.merchantBox.Location.Y), new Rectangle?(new Rectangle(464 + (AbigailGame.shoppingTimer / 100 % 2 == 0 ? 16 : 0), 1728, 16, 16)), Color.White, 0.0f, Vector2.Zero, 3f, SpriteEffects.None, (float) ((double) this.merchantBox.Y / 10000.0 + 1.0 / 1000.0));
          }
          else
          {
            int num1 = this.playerBoundingBox.X - this.merchantBox.X > AbigailGame.TileSize ? 2 : (this.merchantBox.X - this.playerBoundingBox.X > AbigailGame.TileSize ? 1 : 0);
            b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + new Vector2((float) this.merchantBox.Location.X, (float) this.merchantBox.Location.Y), new Rectangle?(new Rectangle(496 + num1 * 16, 1728, 16, 16)), Color.White, 0.0f, Vector2.Zero, 3f, SpriteEffects.None, (float) ((double) this.merchantBox.Y / 10000.0 + 1.0 / 1000.0));
            b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + new Vector2((float) (this.merchantBox.Location.X - AbigailGame.TileSize), (float) (this.merchantBox.Location.Y + AbigailGame.TileSize)), new Rectangle?(new Rectangle(529, 1744, 63, 32)), Color.White, 0.0f, Vector2.Zero, 3f, SpriteEffects.None, (float) ((double) this.merchantBox.Y / 10000.0 + 1.0 / 1000.0));
            foreach (KeyValuePair<Rectangle, int> storeItem in this.storeItems)
            {
              SpriteBatch spriteBatch1 = b;
              Texture2D mouseCursors = Game1.mouseCursors;
              Vector2 screenCoordinate1 = AbigailGame.topLeftScreenCoordinate;
              Rectangle key = storeItem.Key;
              double x = (double) key.Location.X;
              key = storeItem.Key;
              double y = (double) key.Location.Y;
              Vector2 vector2_1 = new Vector2((float) x, (float) y);
              Vector2 position1 = screenCoordinate1 + vector2_1;
              Rectangle? sourceRectangle = new Rectangle?(new Rectangle(320 + storeItem.Value * 16, 1776, 16, 16));
              Color white = Color.White;
              double num2 = 0.0;
              Vector2 zero1 = Vector2.Zero;
              double num3 = 3.0;
              int num4 = 0;
              key = storeItem.Key;
              double num5 = (double) key.Location.Y / 10000.0;
              spriteBatch1.Draw(mouseCursors, position1, sourceRectangle, white, (float) num2, zero1, (float) num3, (SpriteEffects) num4, (float) num5);
              SpriteBatch spriteBatch2 = b;
              SpriteFont smallFont1 = Game1.smallFont;
              string text1 = string.Concat((object) this.getPriceForItem(storeItem.Value));
              Vector2 screenCoordinate2 = AbigailGame.topLeftScreenCoordinate;
              key = storeItem.Key;
              double num6 = (double) (key.Location.X + AbigailGame.TileSize / 2) - (double) Game1.smallFont.MeasureString(string.Concat((object) this.getPriceForItem(storeItem.Value))).X / 2.0;
              key = storeItem.Key;
              double num7 = (double) (key.Location.Y + AbigailGame.TileSize + 3);
              Vector2 vector2_2 = new Vector2((float) num6, (float) num7);
              Vector2 position2 = screenCoordinate2 + vector2_2;
              Color color1 = new Color(88, 29, 43);
              double num8 = 0.0;
              Vector2 zero2 = Vector2.Zero;
              double num9 = 1.0;
              int num10 = 0;
              key = storeItem.Key;
              double num11 = (double) key.Location.Y / 10000.0 + 1.0 / 500.0;
              spriteBatch2.DrawString(smallFont1, text1, position2, color1, (float) num8, zero2, (float) num9, (SpriteEffects) num10, (float) num11);
              SpriteBatch spriteBatch3 = b;
              SpriteFont smallFont2 = Game1.smallFont;
              string text2 = string.Concat((object) this.getPriceForItem(storeItem.Value));
              Vector2 screenCoordinate3 = AbigailGame.topLeftScreenCoordinate;
              key = storeItem.Key;
              double num12 = (double) (key.Location.X + AbigailGame.TileSize / 2) - (double) Game1.smallFont.MeasureString(string.Concat((object) this.getPriceForItem(storeItem.Value))).X / 2.0 - 1.0;
              key = storeItem.Key;
              double num13 = (double) (key.Location.Y + AbigailGame.TileSize + 3);
              Vector2 vector2_3 = new Vector2((float) num12, (float) num13);
              Vector2 position3 = screenCoordinate3 + vector2_3;
              Color color2 = new Color(88, 29, 43);
              double num14 = 0.0;
              Vector2 zero3 = Vector2.Zero;
              double num15 = 1.0;
              int num16 = 0;
              key = storeItem.Key;
              double num17 = (double) key.Location.Y / 10000.0 + 1.0 / 500.0;
              spriteBatch3.DrawString(smallFont2, text2, position3, color2, (float) num14, zero3, (float) num15, (SpriteEffects) num16, (float) num17);
              SpriteBatch spriteBatch4 = b;
              SpriteFont smallFont3 = Game1.smallFont;
              string text3 = string.Concat((object) this.getPriceForItem(storeItem.Value));
              Vector2 screenCoordinate4 = AbigailGame.topLeftScreenCoordinate;
              key = storeItem.Key;
              double num18 = (double) (key.Location.X + AbigailGame.TileSize / 2) - (double) Game1.smallFont.MeasureString(string.Concat((object) this.getPriceForItem(storeItem.Value))).X / 2.0 + 1.0;
              key = storeItem.Key;
              double num19 = (double) (key.Location.Y + AbigailGame.TileSize + 3);
              Vector2 vector2_4 = new Vector2((float) num18, (float) num19);
              Vector2 position4 = screenCoordinate4 + vector2_4;
              Color color3 = new Color(88, 29, 43);
              double num20 = 0.0;
              Vector2 zero4 = Vector2.Zero;
              double num21 = 1.0;
              int num22 = 0;
              key = storeItem.Key;
              double num23 = (double) key.Location.Y / 10000.0 + 1.0 / 500.0;
              spriteBatch4.DrawString(smallFont3, text3, position4, color3, (float) num20, zero4, (float) num21, (SpriteEffects) num22, (float) num23);
            }
          }
        }
        if (AbigailGame.waitingForPlayerToMoveDownAMap && (AbigailGame.merchantShopOpen || AbigailGame.merchantLeaving || !AbigailGame.shopping) && AbigailGame.shoppingTimer < 250)
          b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + new Vector2(7f, 15f) * (float) AbigailGame.TileSize, new Rectangle?(new Rectangle(355, 1750, 8, 8)), Color.White, 0.0f, Vector2.Zero, 3f, SpriteEffects.None, 1f / 1000f);
        foreach (AbigailGame.CowboyMonster monster in AbigailGame.monsters)
          monster.draw(b);
        if (AbigailGame.gopherRunning)
          b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + new Vector2((float) AbigailGame.gopherBox.X, (float) AbigailGame.gopherBox.Y), new Rectangle?(new Rectangle(320 + AbigailGame.waveTimer / 100 % 4 * 16, 1792, 16, 16)), Color.White, 0.0f, Vector2.Zero, 3f, SpriteEffects.None, (float) ((double) AbigailGame.gopherBox.Y / 10000.0 + 1.0 / 1000.0));
        if (AbigailGame.gopherTrain && AbigailGame.gopherTrainPosition > -AbigailGame.TileSize)
        {
          b.Draw(Game1.staminaRect, new Rectangle((int) AbigailGame.topLeftScreenCoordinate.X, (int) AbigailGame.topLeftScreenCoordinate.Y, 16 * AbigailGame.TileSize, 16 * AbigailGame.TileSize), new Rectangle?(Game1.staminaRect.Bounds), Color.Black, 0.0f, Vector2.Zero, SpriteEffects.None, 0.95f);
          b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + new Vector2(this.playerPosition.X - (float) (AbigailGame.TileSize / 2), (float) AbigailGame.gopherTrainPosition), new Rectangle?(new Rectangle(384 + AbigailGame.gopherTrainPosition / 30 % 4 * 16, 1792, 16, 16)), Color.White, 0.0f, Vector2.Zero, 3f, SpriteEffects.None, 0.96f);
          b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + new Vector2(this.playerPosition.X + (float) (AbigailGame.TileSize / 2), (float) AbigailGame.gopherTrainPosition), new Rectangle?(new Rectangle(384 + AbigailGame.gopherTrainPosition / 30 % 4 * 16, 1792, 16, 16)), Color.White, 0.0f, Vector2.Zero, 3f, SpriteEffects.None, 0.96f);
          b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + new Vector2(this.playerPosition.X, (float) (AbigailGame.gopherTrainPosition - AbigailGame.TileSize * 3)), new Rectangle?(new Rectangle(320 + AbigailGame.gopherTrainPosition / 30 % 4 * 16, 1792, 16, 16)), Color.White, 0.0f, Vector2.Zero, 3f, SpriteEffects.None, 0.96f);
          b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + new Vector2(this.playerPosition.X - (float) (AbigailGame.TileSize / 2), (float) (AbigailGame.gopherTrainPosition - AbigailGame.TileSize)), new Rectangle?(new Rectangle(400, 1728, 32, 32)), Color.White, 0.0f, Vector2.Zero, 3f, SpriteEffects.None, 0.97f);
          if (AbigailGame.holdItemTimer > 0)
          {
            b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + this.playerPosition + new Vector2(0.0f, (float) (-AbigailGame.TileSize / 4)), new Rectangle?(new Rectangle(384, 1760, 16, 16)), Color.White, 0.0f, Vector2.Zero, 3f, SpriteEffects.None, 0.98f);
            b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + this.playerPosition + new Vector2(0.0f, (float) (-AbigailGame.TileSize * 2 / 3)) + new Vector2(0.0f, (float) (-AbigailGame.TileSize / 4)), new Rectangle?(new Rectangle(320 + AbigailGame.itemToHold * 16, 1776, 16, 16)), Color.White, 0.0f, Vector2.Zero, 3f, SpriteEffects.None, 0.99f);
          }
          else
            b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + this.playerPosition + new Vector2(0.0f, (float) (-AbigailGame.TileSize / 4)), new Rectangle?(new Rectangle(464, 1760, 16, 16)), Color.White, 0.0f, Vector2.Zero, 3f, SpriteEffects.None, 0.98f);
        }
        else
        {
          b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate - new Vector2((float) (AbigailGame.TileSize + 27), 0.0f), new Rectangle?(new Rectangle(294, 1782, 22, 22)), Color.White, 0.0f, Vector2.Zero, 3f, SpriteEffects.None, 0.25f);
          if (this.heldItem != null)
            b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate - new Vector2((float) (AbigailGame.TileSize + 18), -9f), new Rectangle?(new Rectangle(272 + this.heldItem.which * 16, 1808, 16, 16)), Color.White, 0.0f, Vector2.Zero, 3f, SpriteEffects.None, 0.5f);
          b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate - new Vector2((float) (AbigailGame.TileSize * 2), (float) (-AbigailGame.TileSize - 18)), new Rectangle?(new Rectangle(400, 1776, 16, 16)), Color.White, 0.0f, Vector2.Zero, 3f, SpriteEffects.None, 0.5f);
          b.DrawString(Game1.smallFont, "x" + (object) this.lives, AbigailGame.topLeftScreenCoordinate - new Vector2((float) AbigailGame.TileSize, (float) (-AbigailGame.TileSize - AbigailGame.TileSize / 4 - 18)), Color.White);
          b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate - new Vector2((float) (AbigailGame.TileSize * 2), (float) (-AbigailGame.TileSize * 2 - 18)), new Rectangle?(new Rectangle(272, 1808, 16, 16)), Color.White, 0.0f, Vector2.Zero, 3f, SpriteEffects.None, 0.5f);
          b.DrawString(Game1.smallFont, "x" + (object) this.coins, AbigailGame.topLeftScreenCoordinate - new Vector2((float) AbigailGame.TileSize, (float) (-AbigailGame.TileSize * 2 - AbigailGame.TileSize / 4 - 18)), Color.White);
          for (int index = 0; index < AbigailGame.whichWave + this.whichRound * 12; ++index)
            b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + new Vector2((float) (AbigailGame.TileSize * 16 + 3), (float) (index * 3 * 6)), new Rectangle?(new Rectangle(512, 1760, 5, 5)), Color.White, 0.0f, Vector2.Zero, 3f, SpriteEffects.None, 0.5f);
          b.Draw(Game1.mouseCursors, new Vector2((float) (int) AbigailGame.topLeftScreenCoordinate.X, (float) ((int) AbigailGame.topLeftScreenCoordinate.Y - AbigailGame.TileSize / 2 - 12)), new Rectangle?(new Rectangle(595, 1748, 9, 11)), Color.White, 0.0f, Vector2.Zero, 3f, SpriteEffects.None, 0.5f);
          if (!AbigailGame.shootoutLevel)
            b.Draw(Game1.staminaRect, new Rectangle((int) AbigailGame.topLeftScreenCoordinate.X + 30, (int) AbigailGame.topLeftScreenCoordinate.Y - AbigailGame.TileSize / 2 + 3, (int) ((double) (16 * AbigailGame.TileSize - 30) * ((double) AbigailGame.waveTimer / 80000.0)), AbigailGame.TileSize / 4), AbigailGame.waveTimer < 8000 ? new Color(188, 51, 74) : new Color(147, 177, 38));
          if (AbigailGame.betweenWaveTimer > 0 && AbigailGame.whichWave == 0 && !AbigailGame.scrollingMap)
          {
            Vector2 position = new Vector2((float) (Game1.graphics.GraphicsDevice.Viewport.Width / 2 - 120), (float) (Game1.graphics.GraphicsDevice.Viewport.Height - 144 - 3));
            if (!Game1.options.gamepadControls)
              b.Draw(Game1.mouseCursors, position, new Rectangle?(new Rectangle(352, 1648, 80, 48)), Color.White, 0.0f, Vector2.Zero, 3f, SpriteEffects.None, 0.99f);
            else
              b.Draw(Game1.controllerMaps, position, new Rectangle?(Utility.controllerMapSourceRect(new Rectangle(681, 157, 160, 96))), Color.White, 0.0f, Vector2.Zero, 1.5f, SpriteEffects.None, 0.99f);
          }
          if (this.bulletDamage > 1)
            b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + new Vector2((float) (-AbigailGame.TileSize - 3), (float) (16 * AbigailGame.TileSize - AbigailGame.TileSize)), new Rectangle?(new Rectangle(416 + (this.ammoLevel - 1) * 16, 1776, 16, 16)), Color.White, 0.0f, Vector2.Zero, 3f, SpriteEffects.None, 0.5f);
          if (this.fireSpeedLevel > 0)
            b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + new Vector2((float) (-AbigailGame.TileSize - 3), (float) (16 * AbigailGame.TileSize - AbigailGame.TileSize * 2)), new Rectangle?(new Rectangle(320 + (this.fireSpeedLevel - 1) * 16, 1776, 16, 16)), Color.White, 0.0f, Vector2.Zero, 3f, SpriteEffects.None, 0.5f);
          if (this.runSpeedLevel > 0)
            b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + new Vector2((float) (-AbigailGame.TileSize - 3), (float) (16 * AbigailGame.TileSize - AbigailGame.TileSize * 3)), new Rectangle?(new Rectangle(368 + (this.runSpeedLevel - 1) * 16, 1776, 16, 16)), Color.White, 0.0f, Vector2.Zero, 3f, SpriteEffects.None, 0.5f);
          if (this.spreadPistol)
            b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + new Vector2((float) (-AbigailGame.TileSize - 3), (float) (16 * AbigailGame.TileSize - AbigailGame.TileSize * 4)), new Rectangle?(new Rectangle(464, 1776, 16, 16)), Color.White, 0.0f, Vector2.Zero, 3f, SpriteEffects.None, 0.5f);
        }
        if (AbigailGame.screenFlash > 0)
          b.Draw(Game1.staminaRect, new Rectangle((int) AbigailGame.topLeftScreenCoordinate.X, (int) AbigailGame.topLeftScreenCoordinate.Y, 16 * AbigailGame.TileSize, 16 * AbigailGame.TileSize), new Rectangle?(Game1.staminaRect.Bounds), new Color((int) byte.MaxValue, 214, 168), 0.0f, Vector2.Zero, SpriteEffects.None, 1f);
      }
      if (this.fadethenQuitTimer > 0)
      {
        SpriteBatch spriteBatch = b;
        Texture2D staminaRect = Game1.staminaRect;
        int x = 0;
        int y = 0;
        Viewport viewport = Game1.graphics.GraphicsDevice.Viewport;
        int width = viewport.Width;
        viewport = Game1.graphics.GraphicsDevice.Viewport;
        int height = viewport.Height;
        Rectangle destinationRectangle = new Rectangle(x, y, width, height);
        Rectangle? sourceRectangle = new Rectangle?(Game1.staminaRect.Bounds);
        Color color = Color.Black * (float) (1.0 - (double) this.fadethenQuitTimer / 2000.0);
        double num1 = 0.0;
        Vector2 zero = Vector2.Zero;
        int num2 = 0;
        double num3 = 1.0;
        spriteBatch.Draw(staminaRect, destinationRectangle, sourceRectangle, color, (float) num1, zero, (SpriteEffects) num2, (float) num3);
      }
      if (this.abigailPortraitTimer > 0)
      {
        b.Draw(Game1.getCharacterFromName("Abigail", false).Portrait, new Vector2(AbigailGame.topLeftScreenCoordinate.X + (float) (16 * AbigailGame.TileSize), (float) this.abigailPortraitYposition), new Rectangle?(new Rectangle(64 * (this.abigailPortraitExpression % 2), 64 * (this.abigailPortraitExpression / 2), 64, 64)), Color.White, 0.0f, Vector2.Zero, (float) Game1.pixelZoom, SpriteEffects.None, 1f);
        if (this.abigailPortraitTimer < 5500 && this.abigailPortraitTimer > 500)
        {
          int widthOfString = SpriteText.getWidthOfString("0" + this.AbigailDialogue + "0");
          int num = (int) ((double) AbigailGame.topLeftScreenCoordinate.X + (double) (16 * AbigailGame.TileSize) + (double) (64 * Game1.pixelZoom / 2) - (double) (widthOfString / 2));
          int x = LocalizedContentManager.CurrentLanguageCode == LocalizedContentManager.LanguageCode.zh || LocalizedContentManager.CurrentLanguageCode == LocalizedContentManager.LanguageCode.ru ? (int) ((double) AbigailGame.topLeftScreenCoordinate.X + (double) (16 * AbigailGame.TileSize)) + widthOfString / 4 : (int) ((double) AbigailGame.topLeftScreenCoordinate.X + (double) (16 * AbigailGame.TileSize));
          SpriteText.drawString(b, this.AbigailDialogue, x, (int) ((double) this.abigailPortraitYposition - (double) Game1.tileSize * 1.25), 999999, widthOfString, 999999, 1f, 0.88f, false, -1, "", 3);
        }
      }
      b.End();
    }

    public void changeScreenSize()
    {
      Viewport viewport = Game1.graphics.GraphicsDevice.Viewport;
      double num1 = (double) (viewport.Width / 2 - 384);
      viewport = Game1.graphics.GraphicsDevice.Viewport;
      double num2 = (double) (viewport.Height / 2 - 384);
      AbigailGame.topLeftScreenCoordinate = new Vector2((float) num1, (float) num2);
    }

    public void unload()
    {
      if (AbigailGame.overworldSong != null && AbigailGame.overworldSong.IsPlaying)
        AbigailGame.overworldSong.Stop(AudioStopOptions.Immediate);
      if (AbigailGame.outlawSong != null && AbigailGame.outlawSong.IsPlaying)
        AbigailGame.outlawSong.Stop(AudioStopOptions.Immediate);
      this.lives = 3;
    }

    public void receiveEventPoke(int data)
    {
    }

    public string minigameId()
    {
      return "PrairieKing";
    }

    public delegate void behaviorAfterMotionPause();

    public class CowboyPowerup
    {
      public int which;
      public Point position;
      public int duration;
      public float yOffset;

      public CowboyPowerup(int which, Point position, int duration)
      {
        this.which = which;
        this.position = position;
        this.duration = duration;
      }

      public void draw(SpriteBatch b)
      {
        if (this.duration <= 2000 && this.duration / 200 % 2 != 0)
          return;
        b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + new Vector2((float) this.position.X, (float) this.position.Y + this.yOffset), new Rectangle?(new Rectangle(272 + this.which * 16, 1808, 16, 16)), Color.White, 0.0f, Vector2.Zero, 3f, SpriteEffects.None, (float) ((double) this.position.Y / 10000.0 + 1.0 / 1000.0));
      }
    }

    public class CowboyBullet
    {
      public Point position;
      public Point motion;
      public int damage;

      public CowboyBullet(Point position, Point motion, int damage)
      {
        this.position = position;
        this.motion = motion;
        this.damage = damage;
      }

      public CowboyBullet(Point position, int direction, int damage)
      {
        this.position = position;
        switch (direction)
        {
          case 0:
            this.motion = new Point(0, -8);
            break;
          case 1:
            this.motion = new Point(8, 0);
            break;
          case 2:
            this.motion = new Point(0, 8);
            break;
          case 3:
            this.motion = new Point(-8, 0);
            break;
        }
        this.damage = damage;
      }
    }

    public class CowboyMonster
    {
      private Color tint = Color.White;
      private Color flashColor = Color.Red;
      public const int MonsterAnimationDelay = 500;
      public int health;
      public int type;
      public int speed;
      public float movementAnimationTimer;
      public Rectangle position;
      private int movementDirection;
      private bool movedLastTurn;
      private bool oppositeMotionGuy;
      private bool invisible;
      private bool special;
      private bool uninterested;
      private bool flyer;
      public float flashColorTimer;
      public int ticksSinceLastMovement;
      public Vector2 acceleration;
      private Point targetPosition;

      public CowboyMonster(int which, int health, int speed, Point position)
      {
        this.health = health;
        this.type = which;
        this.speed = speed;
        this.position = new Rectangle(position.X, position.Y, AbigailGame.TileSize, AbigailGame.TileSize);
        this.uninterested = Game1.random.NextDouble() < 0.25;
      }

      public CowboyMonster(int which, Point position)
      {
        this.type = which;
        this.position = new Rectangle(position.X, position.Y, AbigailGame.TileSize, AbigailGame.TileSize);
        switch (this.type)
        {
          case 0:
            this.speed = 2;
            this.health = 1;
            this.uninterested = Game1.random.NextDouble() < 0.25;
            if (this.uninterested)
            {
              this.targetPosition = new Point(Game1.random.Next(2, 14) * AbigailGame.TileSize, Game1.random.Next(2, 14) * AbigailGame.TileSize);
              break;
            }
            break;
          case 1:
            this.speed = 2;
            this.health = 1;
            this.flyer = true;
            break;
          case 2:
            this.speed = 1;
            this.health = 3;
            break;
          case 3:
            this.health = 6;
            this.speed = 1;
            this.uninterested = Game1.random.NextDouble() < 0.25;
            if (this.uninterested)
            {
              this.targetPosition = new Point(Game1.random.Next(2, 14) * AbigailGame.TileSize, Game1.random.Next(2, 14) * AbigailGame.TileSize);
              break;
            }
            break;
          case 4:
            this.health = 3;
            this.speed = 3;
            this.flyer = true;
            break;
          case 5:
            this.speed = 3;
            this.health = 2;
            break;
          case 6:
            this.speed = 3;
            this.health = 2;
            do
            {
              this.targetPosition = new Point(Game1.random.Next(2, 14) * AbigailGame.TileSize, Game1.random.Next(2, 14) * AbigailGame.TileSize);
            }
            while (AbigailGame.isCollidingWithMap(this.targetPosition));
            break;
        }
        this.oppositeMotionGuy = Game1.random.NextDouble() < 0.5;
      }

      public virtual void draw(SpriteBatch b)
      {
        if (this.type == 6 && this.special)
        {
          if ((double) this.flashColorTimer > 0.0)
            b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + new Vector2((float) this.position.X, (float) this.position.Y), new Rectangle?(new Rectangle(480, 1696, 16, 16)), Color.White, 0.0f, Vector2.Zero, 3f, SpriteEffects.None, (float) ((double) this.position.Y / 10000.0 + 1.0 / 1000.0));
          else
            b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + new Vector2((float) this.position.X, (float) this.position.Y), new Rectangle?(new Rectangle(576, 1712, 16, 16)), Color.White, 0.0f, Vector2.Zero, 3f, SpriteEffects.None, (float) ((double) this.position.Y / 10000.0 + 1.0 / 1000.0));
        }
        else
        {
          if (this.invisible)
            return;
          if ((double) this.flashColorTimer > 0.0)
            b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + new Vector2((float) this.position.X, (float) this.position.Y), new Rectangle?(new Rectangle(352 + this.type * 16, 1696, 16, 16)), Color.White, 0.0f, Vector2.Zero, 3f, SpriteEffects.None, (float) ((double) this.position.Y / 10000.0 + 1.0 / 1000.0));
          else
            b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + new Vector2((float) this.position.X, (float) this.position.Y), new Rectangle?(new Rectangle(352 + (this.type * 2 + ((double) this.movementAnimationTimer < 250.0 ? 1 : 0)) * 16, 1712, 16, 16)), Color.White, 0.0f, Vector2.Zero, 3f, SpriteEffects.None, (float) ((double) this.position.Y / 10000.0 + 1.0 / 1000.0));
          if (AbigailGame.monsterConfusionTimer <= 0)
            return;
          b.DrawString(Game1.smallFont, "?", AbigailGame.topLeftScreenCoordinate + new Vector2((float) (this.position.X + AbigailGame.TileSize / 2) - Game1.smallFont.MeasureString("?").X / 2f, (float) (this.position.Y - AbigailGame.TileSize / 2)), new Color(88, 29, 43), 0.0f, Vector2.Zero, 1f, SpriteEffects.None, (float) this.position.Y / 10000f);
          b.DrawString(Game1.smallFont, "?", AbigailGame.topLeftScreenCoordinate + new Vector2((float) ((double) (this.position.X + AbigailGame.TileSize / 2) - (double) Game1.smallFont.MeasureString("?").X / 2.0 + 1.0), (float) (this.position.Y - AbigailGame.TileSize / 2)), new Color(88, 29, 43), 0.0f, Vector2.Zero, 1f, SpriteEffects.None, (float) this.position.Y / 10000f);
          b.DrawString(Game1.smallFont, "?", AbigailGame.topLeftScreenCoordinate + new Vector2((float) ((double) (this.position.X + AbigailGame.TileSize / 2) - (double) Game1.smallFont.MeasureString("?").X / 2.0 - 1.0), (float) (this.position.Y - AbigailGame.TileSize / 2)), new Color(88, 29, 43), 0.0f, Vector2.Zero, 1f, SpriteEffects.None, (float) this.position.Y / 10000f);
        }
      }

      public virtual bool takeDamage(int damage)
      {
        this.health = this.health - damage;
        this.health = Math.Max(0, this.health);
        if (this.health <= 0)
          return true;
        Game1.playSound("cowboy_monsterhit");
        this.flashColor = Color.Red;
        this.flashColorTimer = 100f;
        return false;
      }

      public virtual int getLootDrop()
      {
        if (this.type == 6 && this.special)
          return -1;
        if (Game1.random.NextDouble() < 0.05)
          return this.type != 0 && Game1.random.NextDouble() < 0.1 || Game1.random.NextDouble() < 0.01 ? 1 : 0;
        if (Game1.random.NextDouble() >= 0.05)
          return -1;
        if (Game1.random.NextDouble() < 0.15)
          return Game1.random.Next(6, 8);
        if (Game1.random.NextDouble() < 0.07)
          return 10;
        int num = Game1.random.Next(2, 10);
        if (num == 5 && Game1.random.NextDouble() < 0.4)
          num = Game1.random.Next(2, 10);
        return num;
      }

      public virtual bool move(Vector2 playerPosition, GameTime time)
      {
        this.movementAnimationTimer = this.movementAnimationTimer - (float) time.ElapsedGameTime.Milliseconds;
        if ((double) this.movementAnimationTimer <= 0.0)
          this.movementAnimationTimer = (float) Math.Max(100, 500 - this.speed * 50);
        if ((double) this.flashColorTimer > 0.0)
        {
          this.flashColorTimer = this.flashColorTimer - (float) time.ElapsedGameTime.Milliseconds;
          return false;
        }
        if (AbigailGame.monsterConfusionTimer > 0)
          return false;
        if (AbigailGame.shopping)
        {
          AbigailGame.shoppingTimer -= time.ElapsedGameTime.Milliseconds;
          if (AbigailGame.shoppingTimer <= 0)
            AbigailGame.shoppingTimer = 100;
        }
        this.ticksSinceLastMovement = this.ticksSinceLastMovement + 1;
        switch (this.type)
        {
          case 0:
          case 2:
          case 3:
          case 5:
          case 6:
            if (this.type == 6)
            {
              if (!this.special && !this.invisible)
              {
                if (this.ticksSinceLastMovement > 20)
                {
                  int num = 0;
                  do
                  {
                    ++num;
                    this.targetPosition = new Point(Game1.random.Next(2, 14) * AbigailGame.TileSize, Game1.random.Next(2, 14) * AbigailGame.TileSize);
                  }
                  while (AbigailGame.isCollidingWithMap(this.targetPosition) && num < 5);
                }
              }
              else
                break;
            }
            else if (this.ticksSinceLastMovement > 20)
            {
              int num = 0;
              do
              {
                this.oppositeMotionGuy = !this.oppositeMotionGuy;
                ++num;
                this.targetPosition = new Point(Game1.random.Next(this.position.X - AbigailGame.TileSize * 2, this.position.X + AbigailGame.TileSize * 2), Game1.random.Next(this.position.Y - AbigailGame.TileSize * 2, this.position.Y + AbigailGame.TileSize * 2));
              }
              while (AbigailGame.isCollidingWithMap(this.targetPosition) && num < 5);
            }
            Point targetPosition1 = this.targetPosition;
            Vector2 vector2_1 = !this.targetPosition.Equals(Point.Zero) ? new Vector2((float) this.targetPosition.X, (float) this.targetPosition.Y) : playerPosition;
            if (AbigailGame.playingWithAbigail && vector2_1.Equals(playerPosition))
            {
              double num = Math.Sqrt(Math.Pow((double) this.position.X - (double) vector2_1.X, 2.0) - Math.Pow((double) this.position.Y - (double) vector2_1.Y, 2.0));
              if (Math.Sqrt(Math.Pow((double) this.position.X - (double) AbigailGame.player2Position.X, 2.0) - Math.Pow((double) this.position.Y - (double) AbigailGame.player2Position.Y, 2.0)) < num)
                vector2_1 = AbigailGame.player2Position;
            }
            if (AbigailGame.gopherRunning)
              vector2_1 = new Vector2((float) AbigailGame.gopherBox.X, (float) AbigailGame.gopherBox.Y);
            if (Game1.random.NextDouble() < 0.001)
              this.oppositeMotionGuy = !this.oppositeMotionGuy;
            if (this.type == 6 && !this.oppositeMotionGuy || (double) Math.Abs(vector2_1.X - (float) this.position.X) > (double) Math.Abs(vector2_1.Y - (float) this.position.Y))
            {
              if ((double) vector2_1.X + (double) this.speed < (double) this.position.X && (this.movedLastTurn || this.movementDirection != 3))
                this.movementDirection = 3;
              else if ((double) vector2_1.X > (double) (this.position.X + this.speed) && (this.movedLastTurn || this.movementDirection != 1))
                this.movementDirection = 1;
              else if ((double) vector2_1.Y > (double) (this.position.Y + this.speed) && (this.movedLastTurn || this.movementDirection != 2))
                this.movementDirection = 2;
              else if ((double) vector2_1.Y + (double) this.speed < (double) this.position.Y && (this.movedLastTurn || this.movementDirection != 0))
                this.movementDirection = 0;
            }
            else if ((double) vector2_1.Y > (double) (this.position.Y + this.speed) && (this.movedLastTurn || this.movementDirection != 2))
              this.movementDirection = 2;
            else if ((double) vector2_1.Y + (double) this.speed < (double) this.position.Y && (this.movedLastTurn || this.movementDirection != 0))
              this.movementDirection = 0;
            else if ((double) vector2_1.X + (double) this.speed < (double) this.position.X && (this.movedLastTurn || this.movementDirection != 3))
              this.movementDirection = 3;
            else if ((double) vector2_1.X > (double) (this.position.X + this.speed) && (this.movedLastTurn || this.movementDirection != 1))
              this.movementDirection = 1;
            this.movedLastTurn = false;
            Rectangle position = this.position;
            switch (this.movementDirection)
            {
              case 0:
                position.Y -= this.speed;
                break;
              case 1:
                position.X += this.speed;
                break;
              case 2:
                position.Y += this.speed;
                break;
              case 3:
                position.X -= this.speed;
                break;
            }
            if (AbigailGame.zombieModeTimer > 0)
            {
              position.X = this.position.X - (position.X - this.position.X);
              position.Y = this.position.Y - (position.Y - this.position.Y);
            }
            if (this.type == 2)
            {
              for (int index = AbigailGame.monsters.Count - 1; index >= 0; --index)
              {
                if (AbigailGame.monsters[index].type == 6 && AbigailGame.monsters[index].special && AbigailGame.monsters[index].position.Intersects(position))
                {
                  AbigailGame.addGuts(AbigailGame.monsters[index].position.Location, AbigailGame.monsters[index].type);
                  Game1.playSound("Cowboy_monsterDie");
                  AbigailGame.monsters.RemoveAt(index);
                }
              }
            }
            if (!AbigailGame.isCollidingWithMapForMonsters(position) && !AbigailGame.isCollidingWithMonster(position, this) && (double) AbigailGame.deathTimer <= 0.0)
            {
              this.ticksSinceLastMovement = 0;
              this.position = position;
              this.movedLastTurn = true;
              if (this.position.Contains((int) vector2_1.X + AbigailGame.TileSize / 2, (int) vector2_1.Y + AbigailGame.TileSize / 2))
              {
                this.targetPosition = Point.Zero;
                if ((this.type == 0 || this.type == 3) && this.uninterested)
                {
                  this.targetPosition = new Point(Game1.random.Next(2, 14) * AbigailGame.TileSize, Game1.random.Next(2, 14) * AbigailGame.TileSize);
                  if (Game1.random.NextDouble() < 0.5)
                  {
                    this.uninterested = false;
                    this.targetPosition = Point.Zero;
                  }
                }
                if (this.type == 6 && !this.invisible)
                {
                  AbigailGame.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(352, 1728, 16, 16), 60f, 3, 0, new Vector2((float) this.position.X, (float) this.position.Y) + AbigailGame.topLeftScreenCoordinate, false, false, (float) this.position.Y / 10000f, 0.0f, Color.White, 3f, 0.0f, 0.0f, 0.0f, true)
                  {
                    endFunction = new TemporaryAnimatedSprite.endBehavior(this.spikeyEndBehavior)
                  });
                  this.invisible = true;
                  break;
                }
                break;
              }
              break;
            }
            break;
          case 1:
          case 4:
            if (this.ticksSinceLastMovement > 20)
            {
              int num = 0;
              do
              {
                this.oppositeMotionGuy = !this.oppositeMotionGuy;
                ++num;
                this.targetPosition = new Point(Game1.random.Next(this.position.X - AbigailGame.TileSize * 2, this.position.X + AbigailGame.TileSize * 2), Game1.random.Next(this.position.Y - AbigailGame.TileSize * 2, this.position.Y + AbigailGame.TileSize * 2));
              }
              while (AbigailGame.isCollidingWithMap(this.targetPosition) && num < 5);
            }
            Point targetPosition2 = this.targetPosition;
            Vector2 vector2_2 = !this.targetPosition.Equals(Point.Zero) ? new Vector2((float) this.targetPosition.X, (float) this.targetPosition.Y) : playerPosition;
            Vector2 velocityTowardPoint = Utility.getVelocityTowardPoint(this.position.Location, vector2_2 + new Vector2((float) (AbigailGame.TileSize / 2), (float) (AbigailGame.TileSize / 2)), (float) this.speed);
            float num1 = (double) velocityTowardPoint.X == 0.0 || (double) velocityTowardPoint.Y == 0.0 ? 1f : 1.5f;
            if ((double) velocityTowardPoint.X > (double) this.acceleration.X)
              this.acceleration.X += 0.1f * num1;
            if ((double) velocityTowardPoint.X < (double) this.acceleration.X)
              this.acceleration.X -= 0.1f * num1;
            if ((double) velocityTowardPoint.Y > (double) this.acceleration.Y)
              this.acceleration.Y += 0.1f * num1;
            if ((double) velocityTowardPoint.Y < (double) this.acceleration.Y)
              this.acceleration.Y -= 0.1f * num1;
            if (!AbigailGame.isCollidingWithMonster(new Rectangle(this.position.X + (int) Math.Ceiling((double) this.acceleration.X), this.position.Y + (int) Math.Ceiling((double) this.acceleration.Y), AbigailGame.TileSize, AbigailGame.TileSize), this) && (double) AbigailGame.deathTimer <= 0.0)
            {
              this.ticksSinceLastMovement = 0;
              this.position.X += (int) Math.Ceiling((double) this.acceleration.X);
              this.position.Y += (int) Math.Ceiling((double) this.acceleration.Y);
              if (this.position.Contains((int) vector2_2.X + AbigailGame.TileSize / 2, (int) vector2_2.Y + AbigailGame.TileSize / 2))
              {
                this.targetPosition = Point.Zero;
                break;
              }
              break;
            }
            break;
        }
        return false;
      }

      public void spikeyEndBehavior(int extraInfo)
      {
        this.invisible = false;
        this.health = this.health + 5;
        this.special = true;
      }
    }

    public class Dracula : AbigailGame.CowboyMonster
    {
      private int phase = -1;
      private const int gloatingPhase = -1;
      private const int walkRandomlyAndShootPhase = 0;
      private const int spreadShotPhase = 1;
      private const int summonDemonPhase = 2;
      private const int summonMummyPhase = 3;
      private int phaseInternalTimer;
      private int phaseInternalCounter;
      private int shootTimer;
      private int fullHealth;
      private Point homePosition;

      public Dracula()
        : base(-2, new Point(8 * AbigailGame.TileSize, 8 * AbigailGame.TileSize))
      {
        this.homePosition = this.position.Location;
        this.position.Y += AbigailGame.TileSize * 4;
        this.health = 350;
        this.fullHealth = this.health;
        this.phase = -1;
        this.phaseInternalTimer = 4000;
        this.speed = 2;
      }

      public override void draw(SpriteBatch b)
      {
        if (this.phase != -1)
          b.Draw(Game1.staminaRect, new Rectangle((int) AbigailGame.topLeftScreenCoordinate.X, (int) AbigailGame.topLeftScreenCoordinate.Y + 16 * AbigailGame.TileSize + 3, (int) ((double) (16 * AbigailGame.TileSize) * ((double) this.health / (double) this.fullHealth)), AbigailGame.TileSize / 3), new Color(188, 51, 74));
        if ((double) this.flashColorTimer > 0.0)
        {
          b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + new Vector2((float) this.position.X, (float) this.position.Y), new Rectangle?(new Rectangle(464, 1696, 16, 16)), Color.White, 0.0f, Vector2.Zero, 3f, SpriteEffects.None, (float) this.position.Y / 10000f);
        }
        else
        {
          switch (this.phase)
          {
            case -1:
            case 1:
            case 2:
            case 3:
              b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + new Vector2((float) this.position.X, (float) this.position.Y), new Rectangle?(new Rectangle(592 + this.phaseInternalTimer / 100 % 3 * 16, 1760, 16, 16)), Color.White, 0.0f, Vector2.Zero, 3f, SpriteEffects.None, (float) this.position.Y / 10000f);
              if (this.phase != -1)
                break;
              b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + new Vector2((float) this.position.X, (float) (this.position.Y + AbigailGame.TileSize) + (float) Math.Sin((double) this.phaseInternalTimer / 1000.0) * 3f), new Rectangle?(new Rectangle(528, 1776, 16, 16)), Color.White, 0.0f, Vector2.Zero, 3f, SpriteEffects.None, (float) this.position.Y / 10000f);
              b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + new Vector2((float) (this.position.X - AbigailGame.TileSize / 2), (float) (this.position.Y - AbigailGame.TileSize * 2)), new Rectangle?(new Rectangle(608, 1728, 32, 32)), Color.White, 0.0f, Vector2.Zero, 3f, SpriteEffects.None, (float) this.position.Y / 10000f);
              break;
            default:
              b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + new Vector2((float) this.position.X, (float) this.position.Y), new Rectangle?(new Rectangle(592 + this.phaseInternalTimer / 100 % 2 * 16, 1712, 16, 16)), Color.White, 0.0f, Vector2.Zero, 3f, SpriteEffects.None, (float) this.position.Y / 10000f);
              break;
          }
        }
      }

      public override int getLootDrop()
      {
        return -1;
      }

      public override bool takeDamage(int damage)
      {
        if (this.phase == -1)
          return false;
        this.health = this.health - damage;
        if (this.health < 0)
          return true;
        this.flashColorTimer = 100f;
        Game1.playSound("cowboy_monsterhit");
        return false;
      }

      public override bool move(Vector2 playerPosition, GameTime time)
      {
        TimeSpan elapsedGameTime;
        if ((double) this.flashColorTimer > 0.0)
        {
          double flashColorTimer = (double) this.flashColorTimer;
          elapsedGameTime = time.ElapsedGameTime;
          double milliseconds = (double) elapsedGameTime.Milliseconds;
          this.flashColorTimer = (float) (flashColorTimer - milliseconds);
        }
        int phaseInternalTimer = this.phaseInternalTimer;
        elapsedGameTime = time.ElapsedGameTime;
        int milliseconds1 = elapsedGameTime.Milliseconds;
        this.phaseInternalTimer = phaseInternalTimer - milliseconds1;
        switch (this.phase)
        {
          case -1:
            if (this.phaseInternalTimer <= 0)
            {
              this.phaseInternalCounter = 0;
              if (Game1.soundBank != null)
              {
                AbigailGame.outlawSong = Game1.soundBank.GetCue("cowboy_boss");
                AbigailGame.outlawSong.Play();
              }
              this.phase = 0;
              break;
            }
            break;
          case 0:
            if (this.phaseInternalCounter == 0)
            {
              this.phaseInternalCounter = this.phaseInternalCounter + 1;
              this.phaseInternalTimer = Game1.random.Next(3000, 7000);
            }
            if (this.phaseInternalTimer < 0)
            {
              this.phaseInternalCounter = 0;
              this.phase = Game1.random.Next(1, 4);
              this.phaseInternalTimer = 9999;
            }
            Vector2 vector2 = playerPosition;
            if ((double) AbigailGame.deathTimer <= 0.0)
            {
              int num = -1;
              if ((double) Math.Abs(vector2.X - (float) this.position.X) > (double) Math.Abs(vector2.Y - (float) this.position.Y))
              {
                if ((double) vector2.X + (double) this.speed < (double) this.position.X)
                  num = 3;
                else if ((double) vector2.X > (double) (this.position.X + this.speed))
                  num = 1;
                else if ((double) vector2.Y > (double) (this.position.Y + this.speed))
                  num = 2;
                else if ((double) vector2.Y + (double) this.speed < (double) this.position.Y)
                  num = 0;
              }
              else if ((double) vector2.Y > (double) (this.position.Y + this.speed))
                num = 2;
              else if ((double) vector2.Y + (double) this.speed < (double) this.position.Y)
                num = 0;
              else if ((double) vector2.X + (double) this.speed < (double) this.position.X)
                num = 3;
              else if ((double) vector2.X > (double) (this.position.X + this.speed))
                num = 1;
              Rectangle position = this.position;
              switch (num)
              {
                case 0:
                  position.Y -= this.speed;
                  break;
                case 1:
                  position.X += this.speed;
                  break;
                case 2:
                  position.Y += this.speed;
                  break;
                case 3:
                  position.X -= this.speed;
                  break;
              }
              position.X = this.position.X - (position.X - this.position.X);
              position.Y = this.position.Y - (position.Y - this.position.Y);
              if (!AbigailGame.isCollidingWithMapForMonsters(position) && !AbigailGame.isCollidingWithMonster(position, (AbigailGame.CowboyMonster) this))
                this.position = position;
              int shootTimer = this.shootTimer;
              elapsedGameTime = time.ElapsedGameTime;
              int milliseconds2 = elapsedGameTime.Milliseconds;
              this.shootTimer = shootTimer - milliseconds2;
              if (this.shootTimer < 0)
              {
                Vector2 p = Utility.getVelocityTowardPoint(new Point(this.position.X + AbigailGame.TileSize / 2, this.position.Y), playerPosition + new Vector2((float) (AbigailGame.TileSize / 2), (float) (AbigailGame.TileSize / 2)), 8f);
                if (AbigailGame.playerMovementDirections.Count > 0)
                  p = Utility.getTranslatedVector2(p, AbigailGame.playerMovementDirections.Last<int>(), 3f);
                AbigailGame.enemyBullets.Add(new AbigailGame.CowboyBullet(new Point(this.position.X + AbigailGame.TileSize / 2, this.position.Y + AbigailGame.TileSize / 2), new Point((int) p.X, (int) p.Y), 1));
                this.shootTimer = 250;
                Game1.playSound("Cowboy_gunshot");
                break;
              }
              break;
            }
            break;
          case 1:
            if (this.phaseInternalCounter == 0)
            {
              Point location = this.position.Location;
              if (this.position.X > this.homePosition.X + 6)
                this.position.X -= 6;
              else if (this.position.X < this.homePosition.X - 6)
                this.position.X += 6;
              if (this.position.Y > this.homePosition.Y + 6)
                this.position.Y -= 6;
              else if (this.position.Y < this.homePosition.Y - 6)
                this.position.Y += 6;
              if (this.position.Location.Equals(location))
              {
                this.phaseInternalCounter = this.phaseInternalCounter + 1;
                this.phaseInternalTimer = 1500;
                break;
              }
              break;
            }
            if (this.phaseInternalCounter == 1)
            {
              if (this.phaseInternalTimer < 0)
              {
                this.phaseInternalCounter = this.phaseInternalCounter + 1;
                this.phaseInternalTimer = 2000;
                this.shootTimer = 200;
                this.fireSpread(new Point(this.position.X + AbigailGame.TileSize / 2, this.position.Y + AbigailGame.TileSize / 2), 0.0);
                break;
              }
              break;
            }
            if (this.phaseInternalCounter == 2)
            {
              int shootTimer = this.shootTimer;
              elapsedGameTime = time.ElapsedGameTime;
              int milliseconds2 = elapsedGameTime.Milliseconds;
              this.shootTimer = shootTimer - milliseconds2;
              if (this.shootTimer < 0)
              {
                this.fireSpread(new Point(this.position.X + AbigailGame.TileSize / 2, this.position.Y + AbigailGame.TileSize / 2), 0.0);
                this.shootTimer = 200;
              }
              if (this.phaseInternalTimer < 0)
              {
                this.phaseInternalCounter = this.phaseInternalCounter + 1;
                this.phaseInternalTimer = 500;
                break;
              }
              break;
            }
            if (this.phaseInternalCounter == 3)
            {
              if (this.phaseInternalTimer < 0)
              {
                this.phaseInternalTimer = 2000;
                this.shootTimer = 200;
                this.phaseInternalCounter = this.phaseInternalCounter + 1;
                Vector2 velocityTowardPoint = Utility.getVelocityTowardPoint(new Point(this.position.X + AbigailGame.TileSize / 2, this.position.Y), playerPosition + new Vector2((float) (AbigailGame.TileSize / 2), (float) (AbigailGame.TileSize / 2)), 8f);
                AbigailGame.enemyBullets.Add(new AbigailGame.CowboyBullet(new Point(this.position.X + AbigailGame.TileSize / 2, this.position.Y + AbigailGame.TileSize / 2), new Point((int) velocityTowardPoint.X, (int) velocityTowardPoint.Y), 1));
                Game1.playSound("Cowboy_gunshot");
                break;
              }
              break;
            }
            if (this.phaseInternalCounter == 4)
            {
              int shootTimer = this.shootTimer;
              elapsedGameTime = time.ElapsedGameTime;
              int milliseconds2 = elapsedGameTime.Milliseconds;
              this.shootTimer = shootTimer - milliseconds2;
              if (this.shootTimer < 0)
              {
                Vector2 velocityTowardPoint = Utility.getVelocityTowardPoint(new Point(this.position.X + AbigailGame.TileSize / 2, this.position.Y), playerPosition + new Vector2((float) (AbigailGame.TileSize / 2), (float) (AbigailGame.TileSize / 2)), 8f);
                velocityTowardPoint.X += (float) Game1.random.Next(-1, 2);
                velocityTowardPoint.Y += (float) Game1.random.Next(-1, 2);
                AbigailGame.enemyBullets.Add(new AbigailGame.CowboyBullet(new Point(this.position.X + AbigailGame.TileSize / 2, this.position.Y + AbigailGame.TileSize / 2), new Point((int) velocityTowardPoint.X, (int) velocityTowardPoint.Y), 1));
                Game1.playSound("Cowboy_gunshot");
                this.shootTimer = 200;
              }
              if (this.phaseInternalTimer < 0)
              {
                if (Game1.random.NextDouble() < 0.4)
                {
                  this.phase = 0;
                  this.phaseInternalCounter = 0;
                  break;
                }
                this.phaseInternalTimer = 500;
                this.phaseInternalCounter = 1;
                break;
              }
              break;
            }
            break;
          case 2:
          case 3:
            if (this.phaseInternalCounter == 0)
            {
              Point location = this.position.Location;
              if (this.position.X > this.homePosition.X + 6)
                this.position.X -= 6;
              else if (this.position.X < this.homePosition.X - 6)
                this.position.X += 6;
              if (this.position.Y > this.homePosition.Y + 6)
                this.position.Y -= 6;
              else if (this.position.Y < this.homePosition.Y - 6)
                this.position.Y += 6;
              if (this.position.Location.Equals(location))
              {
                this.phaseInternalCounter = this.phaseInternalCounter + 1;
                this.phaseInternalTimer = 1500;
                break;
              }
              break;
            }
            if (this.phaseInternalCounter == 1 && this.phaseInternalTimer < 0)
            {
              this.summonEnemies(new Point(this.position.X + AbigailGame.TileSize / 2, this.position.Y + AbigailGame.TileSize / 2), Game1.random.Next(0, 5));
              if (Game1.random.NextDouble() < 0.4)
              {
                this.phase = 0;
                this.phaseInternalCounter = 0;
                break;
              }
              this.phaseInternalTimer = 2000;
              break;
            }
            break;
        }
        return false;
      }

      public void fireSpread(Point origin, double offsetAngle)
      {
        foreach (Vector2 surroundingTileLocations in Utility.getSurroundingTileLocationsArray(new Vector2((float) origin.X, (float) origin.Y)))
        {
          Vector2 velocityTowardPoint = Utility.getVelocityTowardPoint(origin, surroundingTileLocations, 6f);
          if (offsetAngle > 0.0)
          {
            offsetAngle /= 2.0;
            velocityTowardPoint.X = (float) (Math.Cos(offsetAngle) * ((double) surroundingTileLocations.X - (double) origin.X) - Math.Sin(offsetAngle) * ((double) surroundingTileLocations.Y - (double) origin.Y)) + (float) origin.X;
            velocityTowardPoint.Y = (float) (Math.Sin(offsetAngle) * ((double) surroundingTileLocations.X - (double) origin.X) + Math.Cos(offsetAngle) * ((double) surroundingTileLocations.Y - (double) origin.Y)) + (float) origin.Y;
            velocityTowardPoint = Utility.getVelocityTowardPoint(origin, velocityTowardPoint, 8f);
          }
          AbigailGame.enemyBullets.Add(new AbigailGame.CowboyBullet(origin, new Point((int) velocityTowardPoint.X, (int) velocityTowardPoint.Y), 1));
        }
        Game1.playSound("Cowboy_gunshot");
      }

      public void summonEnemies(Point origin, int which)
      {
        if (!AbigailGame.isCollidingWithMonster(new Rectangle(origin.X - AbigailGame.TileSize - AbigailGame.TileSize / 2, origin.Y, AbigailGame.TileSize, AbigailGame.TileSize), (AbigailGame.CowboyMonster) null))
          AbigailGame.monsters.Add(new AbigailGame.CowboyMonster(which, new Point(origin.X - AbigailGame.TileSize - AbigailGame.TileSize / 2, origin.Y)));
        if (!AbigailGame.isCollidingWithMonster(new Rectangle(origin.X + AbigailGame.TileSize + AbigailGame.TileSize / 2, origin.Y, AbigailGame.TileSize, AbigailGame.TileSize), (AbigailGame.CowboyMonster) null))
          AbigailGame.monsters.Add(new AbigailGame.CowboyMonster(which, new Point(origin.X + AbigailGame.TileSize + AbigailGame.TileSize / 2, origin.Y)));
        if (!AbigailGame.isCollidingWithMonster(new Rectangle(origin.X, origin.Y + AbigailGame.TileSize + AbigailGame.TileSize / 2, AbigailGame.TileSize, AbigailGame.TileSize), (AbigailGame.CowboyMonster) null))
          AbigailGame.monsters.Add(new AbigailGame.CowboyMonster(which, new Point(origin.X, origin.Y + AbigailGame.TileSize + AbigailGame.TileSize / 2)));
        if (!AbigailGame.isCollidingWithMonster(new Rectangle(origin.X, origin.Y - AbigailGame.TileSize - AbigailGame.TileSize * 3 / 4, AbigailGame.TileSize, AbigailGame.TileSize), (AbigailGame.CowboyMonster) null))
          AbigailGame.monsters.Add(new AbigailGame.CowboyMonster(which, new Point(origin.X, origin.Y - AbigailGame.TileSize - AbigailGame.TileSize * 3 / 4)));
        AbigailGame.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(464, 1792, 16, 16), 80f, 5, 0, AbigailGame.topLeftScreenCoordinate + new Vector2((float) (origin.X - AbigailGame.TileSize - AbigailGame.TileSize / 2), (float) origin.Y), false, false, 1f, 0.0f, Color.White, 3f, 0.0f, 0.0f, 0.0f, true)
        {
          delayBeforeAnimationStart = Game1.random.Next(800)
        });
        AbigailGame.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(464, 1792, 16, 16), 80f, 5, 0, AbigailGame.topLeftScreenCoordinate + new Vector2((float) (origin.X + AbigailGame.TileSize + AbigailGame.TileSize / 2), (float) origin.Y), false, false, 1f, 0.0f, Color.White, 3f, 0.0f, 0.0f, 0.0f, true)
        {
          delayBeforeAnimationStart = Game1.random.Next(800)
        });
        AbigailGame.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(464, 1792, 16, 16), 80f, 5, 0, AbigailGame.topLeftScreenCoordinate + new Vector2((float) origin.X, (float) (origin.Y - AbigailGame.TileSize - AbigailGame.TileSize * 3 / 4)), false, false, 1f, 0.0f, Color.White, 3f, 0.0f, 0.0f, 0.0f, true)
        {
          delayBeforeAnimationStart = Game1.random.Next(800)
        });
        AbigailGame.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(464, 1792, 16, 16), 80f, 5, 0, AbigailGame.topLeftScreenCoordinate + new Vector2((float) origin.X, (float) (origin.Y + AbigailGame.TileSize + AbigailGame.TileSize / 2)), false, false, 1f, 0.0f, Color.White, 3f, 0.0f, 0.0f, 0.0f, true)
        {
          delayBeforeAnimationStart = Game1.random.Next(800)
        });
        Game1.playSound("Cowboy_monsterDie");
      }
    }

    public class Outlaw : AbigailGame.CowboyMonster
    {
      private const int talkingPhase = -1;
      private const int hidingPhase = 0;
      private const int dartOutAndShootPhase = 1;
      private const int runAndGunPhase = 2;
      private const int runGunAndPantPhase = 3;
      private const int shootAtPlayerPhase = 4;
      private int phase;
      private int phaseCountdown;
      private int shootTimer;
      private int phaseInternalTimer;
      private int phaseInternalCounter;
      public bool dartLeft;
      private int fullHealth;
      private Point homePosition;

      public Outlaw(Point position, int health)
        : base(-1, position)
      {
        this.homePosition = position;
        this.health = health;
        this.fullHealth = health;
        this.phaseCountdown = 4000;
        this.phase = -1;
      }

      public override void draw(SpriteBatch b)
      {
        b.Draw(Game1.staminaRect, new Rectangle((int) AbigailGame.topLeftScreenCoordinate.X, (int) AbigailGame.topLeftScreenCoordinate.Y + 16 * AbigailGame.TileSize + 3, (int) ((double) (16 * AbigailGame.TileSize) * ((double) this.health / (double) this.fullHealth)), AbigailGame.TileSize / 3), new Color(188, 51, 74));
        if ((double) this.flashColorTimer > 0.0)
        {
          b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + new Vector2((float) this.position.X, (float) this.position.Y), new Rectangle?(new Rectangle(496, 1696, 16, 16)), Color.White, 0.0f, Vector2.Zero, 3f, SpriteEffects.None, (float) ((double) this.position.Y / 10000.0 + 1.0 / 1000.0));
        }
        else
        {
          switch (this.phase)
          {
            case -1:
            case 0:
              b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + new Vector2((float) this.position.X, (float) this.position.Y), new Rectangle?(new Rectangle(560 + (this.phaseCountdown / 250 % 2 == 0 ? 16 : 0), 1776, 16, 16)), Color.White, 0.0f, Vector2.Zero, 3f, SpriteEffects.None, (float) ((double) this.position.Y / 10000.0 + 1.0 / 1000.0));
              if (this.phase != -1 || this.phaseCountdown <= 1000)
                break;
              b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + new Vector2((float) (this.position.X - AbigailGame.TileSize / 2), (float) (this.position.Y - AbigailGame.TileSize * 2)), new Rectangle?(new Rectangle(576 + (AbigailGame.whichWave > 5 ? 32 : 0), 1792, 32, 32)), Color.White, 0.0f, Vector2.Zero, 3f, SpriteEffects.None, (float) ((double) this.position.Y / 10000.0 + 1.0 / 1000.0));
              break;
            default:
              if (this.phase == 3 && this.phaseInternalCounter == 2)
              {
                b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + new Vector2((float) this.position.X, (float) this.position.Y), new Rectangle?(new Rectangle(560 + (this.phaseCountdown / 250 % 2 == 0 ? 16 : 0), 1776, 16, 16)), Color.White, 0.0f, Vector2.Zero, 3f, SpriteEffects.None, (float) ((double) this.position.Y / 10000.0 + 1.0 / 1000.0));
                break;
              }
              b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + new Vector2((float) this.position.X, (float) this.position.Y), new Rectangle?(new Rectangle(592 + (this.phaseCountdown / 80 % 2 == 0 ? 16 : 0), 1776, 16, 16)), Color.White, 0.0f, Vector2.Zero, 3f, SpriteEffects.None, (float) ((double) this.position.Y / 10000.0 + 1.0 / 1000.0));
              break;
          }
        }
      }

      public override bool move(Vector2 playerPosition, GameTime time)
      {
        TimeSpan elapsedGameTime;
        if ((double) this.flashColorTimer > 0.0)
        {
          double flashColorTimer = (double) this.flashColorTimer;
          elapsedGameTime = time.ElapsedGameTime;
          double milliseconds = (double) elapsedGameTime.Milliseconds;
          this.flashColorTimer = (float) (flashColorTimer - milliseconds);
        }
        int phaseCountdown = this.phaseCountdown;
        elapsedGameTime = time.ElapsedGameTime;
        int milliseconds1 = elapsedGameTime.Milliseconds;
        this.phaseCountdown = phaseCountdown - milliseconds1;
        if (this.position.X > 17 * AbigailGame.TileSize || this.position.X < -AbigailGame.TileSize)
          this.position.X = 16 * AbigailGame.TileSize / 2;
        switch (this.phase)
        {
          case -1:
          case 0:
            if (this.phaseCountdown < 0)
            {
              this.phase = Game1.random.Next(1, 5);
              this.dartLeft = (double) playerPosition.X < (double) this.position.X;
              if ((double) playerPosition.X > (double) (7 * AbigailGame.TileSize) && (double) playerPosition.X < (double) (9 * AbigailGame.TileSize))
              {
                if (Game1.random.NextDouble() < 0.66 || this.phase == 2)
                  this.phase = 4;
              }
              else if (this.phase == 4)
                this.phase = 3;
              this.phaseInternalCounter = 0;
              this.phaseInternalTimer = 0;
              break;
            }
            break;
          case 1:
            int num1 = this.dartLeft ? -3 : 3;
            if (Math.Abs(this.position.Location.X - this.homePosition.X + AbigailGame.TileSize / 2) < AbigailGame.TileSize * 2 + 12 && this.phaseInternalCounter == 0)
            {
              this.position.X += num1;
              if (this.position.X > 256)
              {
                this.phaseInternalCounter = 2;
                break;
              }
              break;
            }
            if (this.phaseInternalCounter == 2)
            {
              this.position.X -= num1;
              if (Math.Abs(this.position.X - this.homePosition.X) < 4)
              {
                this.position.X = this.homePosition.X;
                this.phase = 0;
                this.phaseCountdown = Game1.random.Next(1000, 2000);
                break;
              }
              break;
            }
            if (this.phaseInternalCounter == 0)
            {
              this.phaseInternalCounter = this.phaseInternalCounter + 1;
              this.phaseInternalTimer = Game1.random.Next(1000, 2000);
            }
            int phaseInternalTimer1 = this.phaseInternalTimer;
            elapsedGameTime = time.ElapsedGameTime;
            int milliseconds2 = elapsedGameTime.Milliseconds;
            this.phaseInternalTimer = phaseInternalTimer1 - milliseconds2;
            int shootTimer1 = this.shootTimer;
            elapsedGameTime = time.ElapsedGameTime;
            int milliseconds3 = elapsedGameTime.Milliseconds;
            this.shootTimer = shootTimer1 - milliseconds3;
            if (this.shootTimer < 0)
            {
              AbigailGame.enemyBullets.Add(new AbigailGame.CowboyBullet(new Point(this.position.X + AbigailGame.TileSize / 2, this.position.Y - AbigailGame.TileSize / 2), new Point(Game1.random.Next(-2, 3), -8), 1));
              this.shootTimer = 150;
              Game1.playSound("Cowboy_gunshot");
            }
            if (this.phaseInternalTimer <= 0)
            {
              this.phaseInternalCounter = this.phaseInternalCounter + 1;
              break;
            }
            break;
          case 2:
            if (this.phaseInternalCounter == 2)
            {
              if (this.position.X < this.homePosition.X)
                this.position.X += 4;
              else
                this.position.X -= 4;
              if (Math.Abs(this.position.X - this.homePosition.X) < 5)
              {
                this.position.X = this.homePosition.X;
                this.phase = 0;
                this.phaseCountdown = Game1.random.Next(1000, 2000);
              }
              return false;
            }
            if (this.phaseInternalCounter == 0)
            {
              this.phaseInternalCounter = this.phaseInternalCounter + 1;
              this.phaseInternalTimer = Game1.random.Next(4000, 7000);
            }
            int phaseInternalTimer2 = this.phaseInternalTimer;
            elapsedGameTime = time.ElapsedGameTime;
            int milliseconds4 = elapsedGameTime.Milliseconds;
            this.phaseInternalTimer = phaseInternalTimer2 - milliseconds4;
            if ((double) this.position.X > (double) playerPosition.X && (double) this.position.X - (double) playerPosition.X > 3.0)
              this.position.X -= 2;
            else if ((double) this.position.X < (double) playerPosition.X && (double) playerPosition.X - (double) this.position.X > 3.0)
              this.position.X += 2;
            int shootTimer2 = this.shootTimer;
            elapsedGameTime = time.ElapsedGameTime;
            int milliseconds5 = elapsedGameTime.Milliseconds;
            this.shootTimer = shootTimer2 - milliseconds5;
            if (this.shootTimer < 0)
            {
              AbigailGame.enemyBullets.Add(new AbigailGame.CowboyBullet(new Point(this.position.X + AbigailGame.TileSize / 2, this.position.Y - AbigailGame.TileSize / 2), new Point(Game1.random.Next(-1, 2), -8), 1));
              this.shootTimer = 250;
              if (this.fullHealth > 50)
                this.shootTimer = this.shootTimer - 50;
              if (Game1.random.NextDouble() < 0.2)
                this.shootTimer = 150;
              Game1.playSound("Cowboy_gunshot");
            }
            if (this.phaseInternalTimer <= 0)
            {
              this.phaseInternalCounter = this.phaseInternalCounter + 1;
              break;
            }
            break;
          case 3:
            if (this.phaseInternalCounter == 0)
            {
              this.phaseInternalCounter = this.phaseInternalCounter + 1;
              this.phaseInternalTimer = Game1.random.Next(3000, 6500);
              break;
            }
            if (this.phaseInternalCounter == 2)
            {
              int phaseInternalTimer3 = this.phaseInternalTimer;
              elapsedGameTime = time.ElapsedGameTime;
              int milliseconds6 = elapsedGameTime.Milliseconds;
              this.phaseInternalTimer = phaseInternalTimer3 - milliseconds6;
              if (this.phaseInternalTimer <= 0)
              {
                this.phaseInternalCounter = this.phaseInternalCounter + 1;
                break;
              }
              break;
            }
            if (this.phaseInternalCounter == 3)
            {
              if (this.position.X < this.homePosition.X)
                this.position.X += 4;
              else
                this.position.X -= 4;
              if (Math.Abs(this.position.X - this.homePosition.X) < 5)
              {
                this.position.X = this.homePosition.X;
                this.phase = 0;
                this.phaseCountdown = Game1.random.Next(1000, 2000);
                break;
              }
              break;
            }
            this.position.X += this.dartLeft ? -3 : 3;
            if (this.position.X < AbigailGame.TileSize || this.position.X > 15 * AbigailGame.TileSize)
              this.dartLeft = !this.dartLeft;
            int shootTimer3 = this.shootTimer;
            elapsedGameTime = time.ElapsedGameTime;
            int milliseconds7 = elapsedGameTime.Milliseconds;
            this.shootTimer = shootTimer3 - milliseconds7;
            if (this.shootTimer < 0)
            {
              AbigailGame.enemyBullets.Add(new AbigailGame.CowboyBullet(new Point(this.position.X + AbigailGame.TileSize / 2, this.position.Y - AbigailGame.TileSize / 2), new Point(Game1.random.Next(-1, 2), -8), 1));
              this.shootTimer = 250;
              if (this.fullHealth > 50)
                this.shootTimer = this.shootTimer - 50;
              if (Game1.random.NextDouble() < 0.2)
                this.shootTimer = 150;
              Game1.playSound("Cowboy_gunshot");
            }
            int phaseInternalTimer4 = this.phaseInternalTimer;
            elapsedGameTime = time.ElapsedGameTime;
            int milliseconds8 = elapsedGameTime.Milliseconds;
            this.phaseInternalTimer = phaseInternalTimer4 - milliseconds8;
            if (this.phaseInternalTimer <= 0)
            {
              if (this.phase == 2)
              {
                this.phaseInternalCounter = 3;
                break;
              }
              this.phaseInternalTimer = 3000;
              this.phaseInternalCounter = this.phaseInternalCounter + 1;
              break;
            }
            break;
          case 4:
            int num2 = this.dartLeft ? -3 : 3;
            if (this.phaseInternalCounter == 0 && ((double) playerPosition.X <= (double) (7 * AbigailGame.TileSize) || (double) playerPosition.X >= (double) (9 * AbigailGame.TileSize)))
            {
              this.phaseInternalCounter = 1;
              this.phaseInternalTimer = Game1.random.Next(500, 1500);
              break;
            }
            if (Math.Abs(this.position.Location.X - this.homePosition.X + AbigailGame.TileSize / 2) < AbigailGame.TileSize * 7 + 12 && this.phaseInternalCounter == 0)
            {
              this.position.X += num2;
              break;
            }
            if (this.phaseInternalCounter == 2)
            {
              this.position.X -= this.dartLeft ? -4 : 4;
              if (Math.Abs(this.position.X - this.homePosition.X) < 4)
              {
                this.position.X = this.homePosition.X;
                this.phase = 0;
                this.phaseCountdown = Game1.random.Next(1000, 2000);
                break;
              }
              break;
            }
            if (this.phaseInternalCounter == 0)
            {
              this.phaseInternalCounter = this.phaseInternalCounter + 1;
              this.phaseInternalTimer = Game1.random.Next(1000, 2000);
            }
            int phaseInternalTimer5 = this.phaseInternalTimer;
            elapsedGameTime = time.ElapsedGameTime;
            int milliseconds9 = elapsedGameTime.Milliseconds;
            this.phaseInternalTimer = phaseInternalTimer5 - milliseconds9;
            int shootTimer4 = this.shootTimer;
            elapsedGameTime = time.ElapsedGameTime;
            int milliseconds10 = elapsedGameTime.Milliseconds;
            this.shootTimer = shootTimer4 - milliseconds10;
            if (this.shootTimer < 0)
            {
              Vector2 velocityTowardPoint = Utility.getVelocityTowardPoint(new Point(this.position.X + AbigailGame.TileSize / 2, this.position.Y), playerPosition + new Vector2((float) (AbigailGame.TileSize / 2), (float) (AbigailGame.TileSize / 2)), 8f);
              AbigailGame.enemyBullets.Add(new AbigailGame.CowboyBullet(new Point(this.position.X + AbigailGame.TileSize / 2, this.position.Y - AbigailGame.TileSize / 2), new Point((int) velocityTowardPoint.X, (int) velocityTowardPoint.Y), 1));
              this.shootTimer = 120;
              Game1.playSound("Cowboy_gunshot");
            }
            if (this.phaseInternalTimer <= 0)
            {
              this.phaseInternalCounter = this.phaseInternalCounter + 1;
              break;
            }
            break;
        }
        if (this.position.X <= 16 * AbigailGame.TileSize)
        {
          int x = this.position.X;
        }
        return false;
      }

      public override int getLootDrop()
      {
        return 8;
      }

      public override bool takeDamage(int damage)
      {
        if (Math.Abs(this.position.X - this.homePosition.X) < 5)
          return false;
        this.health = this.health - damage;
        if (this.health < 0)
          return true;
        this.flashColorTimer = 150f;
        Game1.playSound("cowboy_monsterhit");
        return false;
      }
    }
  }
}
