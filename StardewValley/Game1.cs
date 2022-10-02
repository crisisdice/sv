using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Netcode;
using SkiaSharp;
using StardewValley.BellsAndWhistles;
using StardewValley.Buildings;
using StardewValley.Characters;
using StardewValley.Events;
using StardewValley.GameData;
using StardewValley.Locations;
using StardewValley.Menus;
using StardewValley.Minigames;
using StardewValley.Monsters;
using StardewValley.Network;
using StardewValley.Objects;
using StardewValley.Projectiles;
using StardewValley.Quests;
using StardewValley.SDKs;
using StardewValley.TerrainFeatures;
using StardewValley.Tools;
using StardewValley.Util;
using xTile;
using xTile.Dimensions;
using xTile.Display;
using xTile.Layers;
using xTile.ObjectModel;
using xTile.Tiles;

namespace StardewValley
{
	/// <summary>
	/// This is the main type for your game
	/// </summary>
	[InstanceStatics]
	public class Game1 : InstanceGame
	{
		public enum BundleType
		{
			Default,
			Remixed
		}

		public enum MineChestType
		{
			Default,
			Remixed
		}

		public enum MusicContext
		{
			Default,
			SubLocation,
			Event,
			MiniGame,
			ImportantSplitScreenMusic,
			MAX
		}

		public delegate void afterFadeFunction();

		public bool ScreenshotBusy;

		public bool takingMapScreenshot;

		public const int defaultResolutionX = 1280;

		public const int defaultResolutionY = 720;

		public const int pixelZoom = 4;

		public const int tileSize = 64;

		public const int smallestTileSize = 16;

		public const int up = 0;

		public const int right = 1;

		public const int down = 2;

		public const int left = 3;

		public const int spriteIndexForOveralls = 3854;

		public const int colorToleranceForOveralls = 60;

		public const int spriteIndexForOverallsBorder = 3846;

		public const int colorToloranceForOverallsBorder = 20;

		public const int dialogueBoxTileHeight = 5;

		public const int realMilliSecondsPerGameTenMinutes = 7000;

		public const int rainDensity = 70;

		public const int millisecondsPerDialogueLetterType = 30;

		public const float pickToolDelay = 500f;

		public const int defaultMinFishingBiteTime = 600;

		public const int defaultMaxFishingBiteTime = 30000;

		public const int defaultMinFishingNibbleTime = 340;

		public const int defaultMaxFishingNibbleTime = 800;

		public const int minWallpaperPrice = 75;

		public const int maxWallpaperPrice = 500;

		public const int rainLoopLength = 70;

		public const int weather_sunny = 0;

		public const int weather_rain = 1;

		public const int weather_debris = 2;

		public const int weather_lightning = 3;

		public const int weather_festival = 4;

		public const int weather_snow = 5;

		public const int weather_wedding = 6;

		public const byte singlePlayer = 0;

		public const byte multiplayerClient = 1;

		public const byte multiplayerServer = 2;

		public const byte logoScreenGameMode = 4;

		public const byte titleScreenGameMode = 0;

		public const byte loadScreenGameMode = 1;

		public const byte newGameMode = 2;

		public const byte playingGameMode = 3;

		public const byte loadingMode = 6;

		public const byte saveMode = 7;

		public const byte saveCompleteMode = 8;

		public const byte selectGameScreen = 9;

		public const byte creditsMode = 10;

		public const byte errorLogMode = 11;

		public static readonly string version = "1.5.6";

		public static readonly string versionLabel = "Hotfix #3";

		public const float keyPollingThreshold = 650f;

		public const float toolHoldPerPowerupLevel = 600f;

		public const float startingMusicVolume = 1f;

		/// <summary>
		/// ContentManager specifically for loading xTile.Map(s).
		/// Will be unloaded when returning to title.
		/// </summary>
		public LocalizedContentManager xTileContent;

		public static DelayedAction morningSongPlayAction;

		private static LocalizedContentManager _temporaryContent;

		[NonInstancedStatic]
		public static GraphicsDeviceManager graphics;

		[NonInstancedStatic]
		public static LocalizedContentManager content;

		public static SpriteBatch spriteBatch;

		public static GamePadState oldPadState;

		public static float thumbStickSensitivity = 0.1f;

		public static float runThreshold = 0.5f;

		public static int rightStickHoldTime = 0;

		public static int emoteMenuShowTime = 250;

		public static int nextFarmerWarpOffsetX = 0;

		public static int nextFarmerWarpOffsetY = 0;

		public static KeyboardState oldKBState;

		public static MouseState oldMouseState;

		[NonInstancedStatic]
		public static Game1 keyboardFocusInstance = null;

		private static Farmer _player;

		public static NetFarmerRoot serverHost;

		protected static bool _isWarping = false;

		[NonInstancedStatic]
		public static bool hasLocalClientsOnly = false;

		public static bool isUsingBackToFrontSorting = false;

		protected static StringBuilder _debugStringBuilder = new StringBuilder();

		public static Dictionary<string, GameLocation> _locationLookup = new Dictionary<string, GameLocation>(StringComparer.OrdinalIgnoreCase);

		public IList<GameLocation> _locations = new List<GameLocation>();

		public static Viewport defaultDeviceViewport;

		public static LocationRequest locationRequest;

		public static bool warpingForForcedRemoteEvent = false;

		public GameLocation instanceGameLocation;

		public static IDisplayDevice mapDisplayDevice;

		[NonInstancedStatic]
		public static Microsoft.Xna.Framework.Rectangle safeAreaBounds = default(Microsoft.Xna.Framework.Rectangle);

		[NonInstancedStatic]
		public static bool shouldDrawSafeAreaBounds = false;

		public static xTile.Dimensions.Rectangle viewport;

		public static xTile.Dimensions.Rectangle uiViewport;

		public static Texture2D objectSpriteSheet;

		public static Texture2D cropSpriteSheet;

		public static Texture2D mailboxTexture;

		public static Texture2D emoteSpriteSheet;

		public static Texture2D debrisSpriteSheet;

		public static Texture2D toolIconBox;

		public static Texture2D rainTexture;

		public static Texture2D bigCraftableSpriteSheet;

		public static Texture2D swordSwipe;

		public static Texture2D swordSwipeDark;

		public static Texture2D buffsIcons;

		public static Texture2D daybg;

		public static Texture2D nightbg;

		public static Texture2D logoScreenTexture;

		public static Texture2D tvStationTexture;

		public static Texture2D cloud;

		public static Texture2D menuTexture;

		public static Texture2D uncoloredMenuTexture;

		public static Texture2D lantern;

		public static Texture2D windowLight;

		public static Texture2D sconceLight;

		public static Texture2D cauldronLight;

		public static Texture2D shadowTexture;

		public static Texture2D mouseCursors;

		public static Texture2D mouseCursors2;

		public static Texture2D giftboxTexture;

		public static Texture2D controllerMaps;

		public static Texture2D indoorWindowLight;

		public static Texture2D animations;

		public static Texture2D titleScreenBG;

		public static Texture2D logo;

		public static Texture2D concessionsSpriteSheet;

		public static Texture2D birdsSpriteSheet;

		public static Dictionary<string, Stack<Dialogue>> npcDialogues = new Dictionary<string, Stack<Dialogue>>();

		protected readonly List<Farmer> _farmerShadows = new List<Farmer>();

		public static Queue<DelayedAction.delayedBehavior> morningQueue = new Queue<DelayedAction.delayedBehavior>();

		[NonInstancedStatic]
		protected internal static ModHooks hooks = new ModHooks();

		public static InputState input = new InputState();

		protected static IInputSimulator inputSimulator = null;

		public const string objectSpriteSheetName = "Maps\\springobjects";

		public const string animationsName = "TileSheets\\animations";

		public const string mouseCursorsName = "LooseSprites\\Cursors";

		public const string mouseCursors2Name = "LooseSprites\\Cursors2";

		public const string giftboxName = "LooseSprites\\Giftbox";

		public const string toolSpriteSheetName = "TileSheets\\tools";

		public const string bigCraftableSpriteSheetName = "TileSheets\\Craftables";

		public const string debrisSpriteSheetName = "TileSheets\\debris";

		public const string parrotSheetName = "LooseSprites\\parrots";

		public const string hatsSheetName = "Characters\\Farmer\\hats";

		private static Texture2D _toolSpriteSheet = null;

		public static Dictionary<Vector2, int> crabPotOverlayTiles = new Dictionary<Vector2, int>();

		protected static bool _setSaveName = false;

		protected static string _currentSaveName = "";

		public static string savePathOverride = "";

		public static List<string> mailDeliveredFromMailForTomorrow = new List<string>();

		private static RenderTarget2D _lightmap;

		public static Texture2D fadeToBlackRect;

		public static Texture2D staminaRect;

		public static Texture2D currentCoopTexture;

		public static Texture2D currentBarnTexture;

		public static Texture2D currentHouseTexture;

		public static Texture2D greenhouseTexture;

		public static Texture2D littleEffect;

		public static SpriteFont dialogueFont;

		public static SpriteFont smallFont;

		public static SpriteFont tinyFont;

		public static SpriteFont tinyFontBorder;

		public static float pickToolInterval;

		public static float screenGlowAlpha = 0f;

		public static float flashAlpha = 0f;

		public static float starCropShimmerPause;

		public static float noteBlockTimer;

		public static int currentGemBirdIndex = 0;

		public Dictionary<string, object> newGameSetupOptions = new Dictionary<string, object>();

		public static bool dialogueUp = false;

		public static bool dialogueTyping = false;

		public static bool pickingTool = false;

		public static bool isQuestion = false;

		public static bool particleRaining = false;

		public static bool newDay = false;

		public static bool inMine = false;

		public static bool menuUp = false;

		public static bool eventUp = false;

		public static bool viewportFreeze = false;

		public static bool eventOver = false;

		public static bool nameSelectUp = false;

		public static bool screenGlow = false;

		public static bool screenGlowHold = false;

		public static bool screenGlowUp;

		public static bool progressBar = false;

		public static bool killScreen = false;

		public static bool coopDwellerBorn;

		public static bool messagePause;

		public static bool boardingBus;

		public static bool listeningForKeyControlDefinitions;

		public static bool weddingToday;

		public static bool exitToTitle;

		public static bool debugMode;

		public static bool displayHUD = true;

		public static bool displayFarmer = true;

		public static bool showKeyHelp;

		public static bool shippingTax;

		public static bool dialogueButtonShrinking;

		public static bool jukeboxPlaying;

		public static bool drawLighting;

		public static bool quit;

		public static bool startedJukeboxMusic;

		public static bool drawGrid;

		public static bool freezeControls;

		public static bool saveOnNewDay;

		public static bool panMode;

		public static bool showingEndOfNightStuff;

		public static bool wasRainingYesterday;

		public static bool hasLoadedGame;

		public static bool isActionAtCurrentCursorTile;

		public static bool isInspectionAtCurrentCursorTile;

		public static bool isSpeechAtCurrentCursorTile;

		public static bool paused;

		public static bool isTimePaused;

		public static bool frameByFrame;

		public static bool lastCursorMotionWasMouse;

		public static bool showingHealth = false;

		public static bool cabinsSeparate = false;

		public static bool hasApplied1_3_UpdateChanges = false;

		public static bool hasApplied1_4_UpdateChanges = false;

		public static bool showingHealthBar = false;

		private static Action postExitToTitleCallback = null;

		protected int _lastUsedDisplay = -1;

		public bool wasAskedLeoMemory;

		public float controllerSlingshotSafeTime;

		public static BundleType bundleType = BundleType.Default;

		public static bool isRaining = false;

		public static bool isSnowing = false;

		public static bool isLightning = false;

		public static bool isDebrisWeather = false;

		public static int weatherForTomorrow;

		public float zoomModifier = 1f;

		private static ScreenFade screenFade;

		public static string currentSeason = "spring";

		public static SerializableDictionary<string, string> bannedUsers = new SerializableDictionary<string, string>();

		private static object _debugOutputLock = new object();

		private static string _debugOutput;

		public static string requestedMusicTrack = "";

		public static string selectedItemsType;

		public static string nameSelectType;

		public static string messageAfterPause = "";

		public static string fertilizer = "";

		public static string samBandName = "The Alfalfas";

		public static string slotResult;

		public static string keyHelpString = "";

		public static string lastDebugInput = "";

		public static string loadingMessage = "";

		public static string errorMessage = "";

		protected Dictionary<MusicContext, KeyValuePair<string, bool>> _instanceRequestedMusicTracks = new Dictionary<MusicContext, KeyValuePair<string, bool>>();

		protected MusicContext _instanceActiveMusicContext;

		public static bool requestedMusicTrackOverrideable;

		public static bool currentTrackOverrideable;

		public static bool requestedMusicDirty = false;

		protected bool _useUnscaledLighting;

		protected bool _didInitiateItemStow;

		public bool instanceIsOverridingTrack;

		private static string[] _shortDayDisplayName = new string[7];

		public static Queue<string> currentObjectDialogue = new Queue<string>();

		public static List<string> worldStateIDs = new List<string>();

		public static List<Response> questionChoices = new List<Response>();

		public static int xLocationAfterWarp;

		public static int yLocationAfterWarp;

		public static int gameTimeInterval;

		public static int currentQuestionChoice;

		public static int currentDialogueCharacterIndex;

		public static int dialogueTypingInterval;

		public static int dayOfMonth = 0;

		public static int year = 1;

		public static int timeOfDay = 600;

		public static int timeOfDayAfterFade = -1;

		public static int numberOfSelectedItems = -1;

		public static int priceOfSelectedItem;

		public static int currentWallpaper;

		public static int farmerWallpaper = 22;

		public static int wallpaperPrice = 75;

		public static int currentFloor = 3;

		public static int FarmerFloor = 29;

		public static int floorPrice = 75;

		public static int dialogueWidth;

		public static int menuChoice;

		public static int tvStation = -1;

		public static int currentBillboard;

		public static int facingDirectionAfterWarp;

		public static int tmpTimeOfDay;

		public static int percentageToWinStardewHero = 70;

		public static int mouseClickPolling;

		public static int gamePadXButtonPolling;

		public static int gamePadAButtonPolling;

		public static int weatherIcon;

		public static int hitShakeTimer;

		public static int staminaShakeTimer;

		public static int pauseThenDoFunctionTimer;

		public static int currentSongIndex = 3;

		public static int cursorTileHintCheckTimer;

		public static int timerUntilMouseFade;

		public static int minecartHighScore;

		public static int whichFarm;

		public static int startingCabins;

		public static ModFarmType whichModFarm = null;

		public static ulong? startingGameSeed = null;

		public static int elliottPiano = 0;

		public static SaveGame.SaveFixes lastAppliedSaveFix;

		public static List<int> dealerCalicoJackTotal;

		public static Color morningColor = Color.LightBlue;

		public static Color eveningColor = new Color(255, 255, 0);

		public static Color unselectedOptionColor = new Color(100, 100, 100);

		public static Color screenGlowColor;

		public static NPC currentSpeaker;

		public static Random random = new Random(DateTime.Now.Millisecond);

		public static Random recentMultiplayerRandom = new Random();

		public static IDictionary<int, string> objectInformation;

		public static IDictionary<int, string> bigCraftablesInformation;

		public static IDictionary<int, string> clothingInformation;

		public static IDictionary<string, string> objectContextTags;

		public static List<HUDMessage> hudMessages = new List<HUDMessage>();

		public static IDictionary<string, string> NPCGiftTastes;

		public static float musicPlayerVolume;

		public static float ambientPlayerVolume;

		public static float pauseAccumulator;

		public static float pauseTime;

		public static float upPolling;

		public static float downPolling;

		public static float rightPolling;

		public static float leftPolling;

		public static float debrisSoundInterval;

		public static float toolHold;

		public static float windGust;

		public static float dialogueButtonScale = 1f;

		public static float creditsTimer;

		public static float globalOutdoorLighting;

		public ICue instanceCurrentSong;

		public static IAudioCategory musicCategory;

		public static IAudioCategory soundCategory;

		public static IAudioCategory ambientCategory;

		public static IAudioCategory footstepCategory;

		public PlayerIndex instancePlayerOneIndex;

		[NonInstancedStatic]
		public static IAudioEngine audioEngine;

		[NonInstancedStatic]
		public static WaveBank waveBank;

		[NonInstancedStatic]
		public static WaveBank waveBank1_4;

		[NonInstancedStatic]
		public static ISoundBank soundBank;

		public static Vector2 shiny = Vector2.Zero;

		public static Vector2 previousViewportPosition;

		public static Vector2 currentCursorTile;

		public static Vector2 lastCursorTile = Vector2.Zero;

		public static Vector2 snowPos;

		public Microsoft.Xna.Framework.Rectangle localMultiplayerWindow;

		public static RainDrop[] rainDrops = new RainDrop[70];

		public static double chanceToRainTomorrow = 0.0;

		public static ICue chargeUpSound;

		public static ICue wind;

		public static NetAudioCueManager locationCues = new NetAudioCueManager();

		public static List<WeatherDebris> debrisWeather = new List<WeatherDebris>();

		public static List<TemporaryAnimatedSprite> screenOverlayTempSprites = new List<TemporaryAnimatedSprite>();

		public static List<TemporaryAnimatedSprite> uiOverlayTempSprites = new List<TemporaryAnimatedSprite>();

		private static byte _gameMode;

		private bool _isSaving;

		protected internal static Multiplayer multiplayer = new Multiplayer();

		public static byte multiplayerMode;

		public static IEnumerator<int> currentLoader;

		public static ulong uniqueIDForThisGame = Utility.NewUniqueIdForThisGame();

		public static int[] cropsOfTheWeek;

		public static int[] directionKeyPolling = new int[4];

		public static Quest questOfTheDay;

		public static MoneyMadeScreen moneyMadeScreen;

		public static HashSet<LightSource> currentLightSources = new HashSet<LightSource>();

		public static Color ambientLight;

		public static Color outdoorLight = new Color(255, 255, 0);

		public static Color textColor = new Color(34, 17, 34);

		public static Color textShadowColor = new Color(206, 156, 95);

		public static IClickableMenu overlayMenu;

		private static IClickableMenu _activeClickableMenu;

		public static bool isCheckingNonMousePlacement = false;

		private static IMinigame _currentMinigame = null;

		public static IList<IClickableMenu> onScreenMenus = new List<IClickableMenu>();

		private const int _fpsHistory = 120;

		protected static List<float> _fpsList = new List<float>(120);

		protected static Stopwatch _fpsStopwatch = new Stopwatch();

		protected static float _fps = 0f;

		public static Dictionary<int, string> achievements;

		public static BuffsDisplay buffsDisplay;

		public static DayTimeMoneyBox dayTimeMoneyBox;

		public static NetRootDictionary<long, Farmer> otherFarmers;

		private static readonly FarmerCollection _onlineFarmers = new FarmerCollection();

		public static IGameServer server;

		public static Client client;

		public KeyboardDispatcher instanceKeyboardDispatcher;

		public static Background background;

		public static FarmEvent farmEvent;

		public static afterFadeFunction afterFade;

		public static afterFadeFunction afterDialogues;

		public static afterFadeFunction afterViewport;

		public static afterFadeFunction viewportReachedTarget;

		public static afterFadeFunction afterPause;

		public static GameTime currentGameTime;

		public static IList<DelayedAction> delayedActions = new List<DelayedAction>();

		public static Stack<IClickableMenu> endOfNightMenus = new Stack<IClickableMenu>();

		public Options instanceOptions;

		[NonInstancedStatic]
		public static SerializableDictionary<long, Options> splitscreenOptions = new SerializableDictionary<long, Options>();

		public static Game1 game1;

		public static Point lastMousePositionBeforeFade;

		public static int ticks;

		public static EmoteMenu emoteMenu;

		[NonInstancedStatic]
		public static SerializableDictionary<string, string> CustomData = new SerializableDictionary<string, string>();

		public static NetRoot<IWorldState> netWorldState;

		public static ChatBox chatBox;

		public TextEntryMenu instanceTextEntry;

		public static SpecialCurrencyDisplay specialCurrencyDisplay = null;

		public LocalCoopJoinMenu localCoopJoinMenu;

		public static bool drawbounds;

		private static string debugPresenceString;

		public static List<Action> remoteEventQueue = new List<Action>();

		public static List<long> weddingsToday = new List<long>();

		public int instanceIndex;

		public int instanceId;

		public static bool overrideGameMenuReset;

		protected bool _windowResizing;

		protected Point _oldMousePosition;

		protected bool _oldGamepadConnectedState;

		protected int _oldScrollWheelValue;

		public static Point viewportCenter;

		public static Vector2 viewportTarget = new Vector2(-2.1474836E+09f, -2.1474836E+09f);

		public static float viewportSpeed = 2f;

		public static int viewportHold;

		private static bool _cursorDragEnabled = false;

		private static bool _cursorDragPrevEnabled = false;

		private static bool _cursorSpeedDirty = true;

		private const float CursorBaseSpeed = 16f;

		private static float _cursorSpeed = 16f;

		private static float _cursorSpeedScale = 1f;

		private static float _cursorUpdateElapsedSec = 0f;

		private static int thumbstickPollingTimer;

		public static bool toggleFullScreen;

		public static string whereIsTodaysFest;

		public const string NO_LETTER_MAIL = "%&NL&%";

		public const string BROADCAST_MAIL_FOR_TOMORROW_PREFIX = "%&MFT&%";

		public const string BROADCAST_SEEN_MAIL_PREFIX = "%&SM&%";

		public const string BROADCAST_MAILBOX_PREFIX = "%&MB&%";

		public bool isLocalMultiplayerNewDayActive;

		protected static Task _newDayTask;

		private static Action _afterNewDayAction;

		public static NewDaySynchronizer newDaySync;

		public static bool forceSnapOnNextViewportUpdate = false;

		public static Vector2 currentViewportTarget;

		public static Vector2 viewportPositionLerp;

		public static float screenGlowRate = 0.005f;

		public static float screenGlowMax;

		public static bool haltAfterCheck = false;

		public static bool uiMode = false;

		public static RenderTarget2D nonUIRenderTarget = null;

		public static int uiModeCount = 0;

		protected static int _oldUIModeCount = 0;

		private string panModeString;

		public static bool conventionMode = false;

		private EventTest eventTest;

		private bool panFacingDirectionWait;

		public static bool isRunningMacro = false;

		public static int thumbstickMotionMargin;

		public static float thumbstickMotionAccell = 1f;

		public static int triggerPolling;

		public static int rightClickPolling;

		private RenderTarget2D _screen;

		private RenderTarget2D _uiScreen;

		public static Color bgColor = new Color(5, 3, 4);

		protected readonly BlendState lightingBlend = new BlendState
		{
			ColorBlendFunction = BlendFunction.ReverseSubtract,
			ColorDestinationBlend = Blend.One,
			ColorSourceBlend = Blend.SourceColor
		};

		public bool isDrawing;

		[NonInstancedStatic]
		public static bool isRenderingScreenBuffer = false;

		protected bool _lastDrewMouseCursor;

		protected static int _activatedTick = 0;

		public static int mouseCursor = 0;

		private static float _mouseCursorTransparency = 1f;

		public static bool wasMouseVisibleThisFrame = true;

		public static NPC objectDialoguePortraitPerson;

		public bool IsActiveNoOverlay
		{
			get
			{
				if (!base.IsActive)
				{
					return false;
				}
				if (Program.sdk.HasOverlay)
				{
					return false;
				}
				return true;
			}
		}

		public static LocalizedContentManager temporaryContent
		{
			get
			{
				if (_temporaryContent == null)
				{
					_temporaryContent = content.CreateTemporary();
				}
				return _temporaryContent;
			}
		}

		public static Farmer player
		{
			get
			{
				return _player;
			}
			set
			{
				if (_player != null)
				{
					_player.unload();
					_player = null;
				}
				_player = value;
			}
		}

		public static bool isWarping => _isWarping;

		public static IList<GameLocation> locations => game1._locations;

		public static GameLocation currentLocation
		{
			get
			{
				return game1.instanceGameLocation;
			}
			set
			{
				game1.instanceGameLocation = value;
			}
		}

		public static Texture2D toolSpriteSheet
		{
			get
			{
				if (_toolSpriteSheet == null)
				{
					ResetToolSpriteSheet();
				}
				return _toolSpriteSheet;
			}
		}

		public static RenderTarget2D lightmap => _lightmap;

		public static bool spawnMonstersAtNight
		{
			get
			{
				return player.team.spawnMonstersAtNight;
			}
			set
			{
				player.team.spawnMonstersAtNight.Value = value;
			}
		}

		public static bool fadeToBlack
		{
			get
			{
				return screenFade.fadeToBlack;
			}
			set
			{
				screenFade.fadeToBlack = value;
			}
		}

		public static bool fadeIn
		{
			get
			{
				return screenFade.fadeIn;
			}
			set
			{
				screenFade.fadeIn = value;
			}
		}

		public static bool globalFade
		{
			get
			{
				return screenFade.globalFade;
			}
			set
			{
				screenFade.globalFade = value;
			}
		}

		public static bool nonWarpFade
		{
			get
			{
				return screenFade.nonWarpFade;
			}
			set
			{
				screenFade.nonWarpFade = value;
			}
		}

		public static float fadeToBlackAlpha
		{
			get
			{
				return screenFade.fadeToBlackAlpha;
			}
			set
			{
				screenFade.fadeToBlackAlpha = value;
			}
		}

		public static float globalFadeSpeed
		{
			get
			{
				return screenFade.globalFadeSpeed;
			}
			set
			{
				screenFade.globalFadeSpeed = value;
			}
		}

		public static string CurrentSeasonDisplayName => content.LoadString("Strings\\StringsFromCSFiles:" + currentSeason);

		public static string debugOutput
		{
			get
			{
				return _debugOutput;
			}
			set
			{
				lock (_debugOutputLock)
				{
					if (_debugOutput != value)
					{
						_debugOutput = value;
						if (!string.IsNullOrEmpty(_debugOutput))
						{
							Console.WriteLine("DebugOutput: {0}", _debugOutput);
						}
					}
				}
			}
		}

		public static string elliottBookName
		{
			get
			{
				if (player != null && player.DialogueQuestionsAnswered.Contains(958699))
				{
					return content.LoadString("Strings\\Events:ElliottBook_mystery");
				}
				if (player != null && player.DialogueQuestionsAnswered.Contains(958700))
				{
					return content.LoadString("Strings\\Events:ElliottBook_romance");
				}
				return content.LoadString("Strings\\Events:ElliottBook_default");
			}
			set
			{
			}
		}

		protected static Dictionary<MusicContext, KeyValuePair<string, bool>> _requestedMusicTracks
		{
			get
			{
				return game1._instanceRequestedMusicTracks;
			}
			set
			{
				game1._instanceRequestedMusicTracks = value;
			}
		}

		protected static MusicContext _activeMusicContext
		{
			get
			{
				return game1._instanceActiveMusicContext;
			}
			set
			{
				game1._instanceActiveMusicContext = value;
			}
		}

		public static bool isOverridingTrack
		{
			get
			{
				return game1.instanceIsOverridingTrack;
			}
			set
			{
				game1.instanceIsOverridingTrack = value;
			}
		}

		public bool useUnscaledLighting
		{
			get
			{
				return _useUnscaledLighting;
			}
			set
			{
				if (_useUnscaledLighting != value)
				{
					_useUnscaledLighting = value;
					allocateLightmap(localMultiplayerWindow.Width, localMultiplayerWindow.Height);
				}
			}
		}

		public static IList<string> mailbox => player.mailbox;

		public static ICue currentSong
		{
			get
			{
				return game1.instanceCurrentSong;
			}
			set
			{
				game1.instanceCurrentSong = value;
			}
		}

		public static PlayerIndex playerOneIndex
		{
			get
			{
				return game1.instancePlayerOneIndex;
			}
			set
			{
				game1.instancePlayerOneIndex = value;
			}
		}

		public static byte gameMode
		{
			get
			{
				return _gameMode;
			}
			set
			{
				if (_gameMode != value)
				{
					Console.WriteLine("gameMode was '{0}', set to '{1}'.", GameModeToString(_gameMode), GameModeToString(value));
					_gameMode = value;
				}
			}
		}

		public bool IsSaving
		{
			get
			{
				return _isSaving;
			}
			set
			{
				_isSaving = value;
			}
		}

		public static Stats stats => player.stats;

		public static IClickableMenu activeClickableMenu
		{
			get
			{
				return _activeClickableMenu;
			}
			set
			{
				if (_activeClickableMenu is IDisposable && !_activeClickableMenu.HasDependencies())
				{
					(_activeClickableMenu as IDisposable).Dispose();
				}
				if (_activeClickableMenu != null && value == null)
				{
					timerUntilMouseFade = 0;
				}
				if (textEntry != null && _activeClickableMenu != value)
				{
					closeTextEntry();
				}
				_activeClickableMenu = value;
				if (_activeClickableMenu != null && (!eventUp || (CurrentEvent != null && CurrentEvent.playerControlSequence && !player.UsingTool)))
				{
					player.Halt();
				}
			}
		}

		public static IMinigame currentMinigame
		{
			get
			{
				return _currentMinigame;
			}
			set
			{
				_currentMinigame = value;
				if (value == null)
				{
					if (currentLocation != null)
					{
						setRichPresence("location", currentLocation.Name);
					}
					randomizeDebrisWeatherPositions(debrisWeather);
					randomizeRainPositions();
				}
				else if (value.minigameId() != null)
				{
					setRichPresence("minigame", value.minigameId());
				}
			}
		}

		public static Object dishOfTheDay
		{
			get
			{
				return netWorldState.Value.DishOfTheDay.Value;
			}
			set
			{
				netWorldState.Value.DishOfTheDay.Value = value;
			}
		}

		public static KeyboardDispatcher keyboardDispatcher
		{
			get
			{
				return game1.instanceKeyboardDispatcher;
			}
			set
			{
				game1.instanceKeyboardDispatcher = value;
			}
		}

		public static Options options
		{
			get
			{
				return game1.instanceOptions;
			}
			set
			{
				game1.instanceOptions = value;
			}
		}

		public static TextEntryMenu textEntry
		{
			get
			{
				return game1.instanceTextEntry;
			}
			set
			{
				game1.instanceTextEntry = value;
			}
		}

		public static WorldDate Date => netWorldState.Value.Date;

		public static bool NetTimePaused => netWorldState.Get().IsTimePaused;

		public static bool HostPaused => netWorldState.Get().IsPaused;

		public static bool IsMultiplayer => otherFarmers.Count > 0;

		public static bool IsClient => multiplayerMode == 1;

		public static bool IsServer => multiplayerMode == 2;

		public static bool IsMasterGame
		{
			get
			{
				if (multiplayerMode != 0)
				{
					return multiplayerMode == 2;
				}
				return true;
			}
		}

		public static Farmer MasterPlayer
		{
			get
			{
				if (!IsMasterGame)
				{
					return serverHost.Value;
				}
				return player;
			}
		}

		public static bool IsChatting
		{
			get
			{
				if (chatBox != null)
				{
					return chatBox.isActive();
				}
				return false;
			}
			set
			{
				if (value != chatBox.isActive())
				{
					if (value)
					{
						chatBox.activate();
					}
					else
					{
						chatBox.clickAway();
					}
				}
			}
		}

		public static Event CurrentEvent
		{
			get
			{
				if (currentLocation == null)
				{
					return null;
				}
				return currentLocation.currentEvent;
			}
		}

		public static MineShaft mine
		{
			get
			{
				if (locationRequest != null && locationRequest.Location is MineShaft)
				{
					return locationRequest.Location as MineShaft;
				}
				if (currentLocation is MineShaft)
				{
					return currentLocation as MineShaft;
				}
				return null;
			}
		}

		public static int CurrentMineLevel
		{
			get
			{
				if (currentLocation is MineShaft)
				{
					return (currentLocation as MineShaft).mineLevel;
				}
				return 0;
			}
		}

		public static int CurrentPlayerLimit
		{
			get
			{
				if (netWorldState == null || netWorldState.Value == null || netWorldState.Value.CurrentPlayerLimit == null)
				{
					return multiplayer.playerLimit;
				}
				return netWorldState.Value.CurrentPlayerLimit.Value;
			}
		}

		private static float thumbstickToMouseModifier
		{
			get
			{
				if (_cursorSpeedDirty)
				{
					ComputeCursorSpeed();
				}
				return _cursorSpeed / 720f * (float)viewport.Height * (float)currentGameTime.ElapsedGameTime.TotalSeconds;
			}
		}

		public static bool isFullscreen => graphics.IsFullScreen;

		public static bool IsSummer => currentSeason.Equals("summer");

		public static bool IsSpring => currentSeason.Equals("spring");

		public static bool IsFall => currentSeason.Equals("fall");

		public static bool IsWinter => currentSeason.Equals("winter");

		public RenderTarget2D screen
		{
			get
			{
				return _screen;
			}
			set
			{
				if (_screen != null)
				{
					_screen.Dispose();
					_screen = null;
				}
				_screen = value;
			}
		}

		public RenderTarget2D uiScreen
		{
			get
			{
				return _uiScreen;
			}
			set
			{
				if (_uiScreen != null)
				{
					_uiScreen.Dispose();
					_uiScreen = null;
				}
				_uiScreen = value;
			}
		}

		public static float mouseCursorTransparency
		{
			get
			{
				return _mouseCursorTransparency;
			}
			set
			{
				_mouseCursorTransparency = value;
			}
		}

		public static bool GetHasRoomAnotherFarm()
		{
			return true;
		}

		public bool CanTakeScreenshots()
		{
			_ = Environment.Is64BitProcess;
			return true;
		}

		public bool CanBrowseScreenshots()
		{
			string screenshot_folder = "Screenshots";
			if (Directory.Exists(Path.Combine(Path.Combine(Environment.GetFolderPath((Environment.OSVersion.Platform != PlatformID.Unix) ? Environment.SpecialFolder.ApplicationData : Environment.SpecialFolder.LocalApplicationData), "StardewValley"), screenshot_folder)))
			{
				return true;
			}
			return false;
		}

		public bool CanZoomScreenshots()
		{
			return true;
		}

		public void BrowseScreenshots()
		{
			string screenshot_folder = "Screenshots";
			string main_folder = Path.Combine(Path.Combine(Environment.GetFolderPath((Environment.OSVersion.Platform != PlatformID.Unix) ? Environment.SpecialFolder.ApplicationData : Environment.SpecialFolder.LocalApplicationData), "StardewValley"), screenshot_folder);
			if (Directory.Exists(main_folder))
			{
				try
				{
					Process.Start(new ProcessStartInfo
					{
						FileName = main_folder,
						UseShellExecute = true,
						Verb = "open"
					});
				}
				catch (Exception)
				{
				}
			}
		}

		public unsafe string takeMapScreenshot(float? in_scale, string screenshot_name, Action onDone)
		{
			float scale = in_scale.Value;
			if (screenshot_name == null || screenshot_name.Trim() == "")
			{
				screenshot_name = SaveGame.FilterFileName(player.name) + "_" + DateTime.UtcNow.Month + "-" + DateTime.UtcNow.Day + "-" + DateTime.UtcNow.Year + "_" + (int)DateTime.UtcNow.TimeOfDay.TotalMilliseconds;
			}
			if (currentLocation == null)
			{
				return null;
			}
			string filename = screenshot_name + ".png";
			int start_x = 0;
			int start_y = 0;
			int width = currentLocation.map.DisplayWidth;
			int height = currentLocation.map.DisplayHeight;
			try
			{
				PropertyValue screenshot_region_value = null;
				if (currentLocation.map.Properties.TryGetValue("ScreenshotRegion", out screenshot_region_value))
				{
					string[] array = screenshot_region_value.ToString().Split(' ');
					start_x = int.Parse(array[0]) * 64;
					start_y = int.Parse(array[1]) * 64;
					width = (int.Parse(array[2]) + 1) * 64 - start_x;
					height = (int.Parse(array[3]) + 1) * 64 - start_y;
				}
			}
			catch (Exception)
			{
				start_x = 0;
				start_y = 0;
				width = currentLocation.map.DisplayWidth;
				height = currentLocation.map.DisplayHeight;
			}
			int scaled_width = (int)((float)width * scale);
			int scaled_height = (int)((float)height * scale);
			bool failed = false;
			SKSurface map_bitmap = null;
			do
			{
				failed = false;
				scaled_width = (int)((float)width * scale);
				scaled_height = (int)((float)height * scale);
				try
				{
					map_bitmap = SKSurface.Create(scaled_width, scaled_height, SKColorType.Rgb888x, SKAlphaType.Opaque);
				}
				catch (Exception e2)
				{
					Console.WriteLine("Map Screenshot: Error trying to create Bitmap: " + e2.ToString());
					failed = true;
				}
				if (failed)
				{
					scale -= 0.25f;
				}
				if (scale <= 0f)
				{
					return null;
				}
			}
			while (failed);
			int chunk_size = 2048;
			int scaled_chunk_size = (int)((float)chunk_size * scale);
			xTile.Dimensions.Rectangle old_viewport = viewport;
			bool old_display_hud = displayHUD;
			takingMapScreenshot = true;
			float old_zoom_level = options.baseZoomLevel;
			options.baseZoomLevel = 1f;
			RenderTarget2D cached_lightmap = _lightmap;
			_lightmap = null;
			bool fail = false;
			try
			{
				allocateLightmap(chunk_size, chunk_size);
				int chunks_wide = (int)Math.Ceiling((float)scaled_width / (float)scaled_chunk_size);
				int chunks_high = (int)Math.Ceiling((float)scaled_height / (float)scaled_chunk_size);
				for (int y_offset = 0; y_offset < chunks_high; y_offset++)
				{
					for (int x_offset = 0; x_offset < chunks_wide; x_offset++)
					{
						int current_width = scaled_chunk_size;
						int current_height = scaled_chunk_size;
						int current_x = x_offset * scaled_chunk_size;
						int current_y = y_offset * scaled_chunk_size;
						if (current_x + scaled_chunk_size > scaled_width)
						{
							current_width += scaled_width - (current_x + scaled_chunk_size);
						}
						if (current_y + scaled_chunk_size > scaled_height)
						{
							current_height += scaled_height - (current_y + scaled_chunk_size);
						}
						if (current_height <= 0 || current_width <= 0)
						{
							continue;
						}
						Microsoft.Xna.Framework.Rectangle rect = new Microsoft.Xna.Framework.Rectangle(current_x, current_y, current_width, current_height);
						RenderTarget2D render_target = new RenderTarget2D(graphics.GraphicsDevice, chunk_size, chunk_size, mipMap: false, SurfaceFormat.Color, DepthFormat.None, 0, RenderTargetUsage.DiscardContents);
						viewport = new xTile.Dimensions.Rectangle(x_offset * chunk_size + start_x, y_offset * chunk_size + start_y, chunk_size, chunk_size);
						_draw(currentGameTime, render_target);
						RenderTarget2D scaled_render_target = new RenderTarget2D(graphics.GraphicsDevice, current_width, current_height, mipMap: false, SurfaceFormat.Color, DepthFormat.None, 0, RenderTargetUsage.DiscardContents);
						base.GraphicsDevice.SetRenderTarget(scaled_render_target);
						spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Opaque, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone);
						Color color = Color.White;
						spriteBatch.Draw(render_target, Vector2.Zero, render_target.Bounds, color, 0f, Vector2.Zero, scale, SpriteEffects.None, 1f);
						spriteBatch.End();
						render_target.Dispose();
						base.GraphicsDevice.SetRenderTarget(null);
						Color[] colors = new Color[current_width * current_height];
						scaled_render_target.GetData(colors);
						new SKImageInfo(current_width, current_height, SKColorType.Rgb888x);
						SKBitmap portion_bitmap = new SKBitmap(rect.Width, rect.Height, SKColorType.Rgb888x, SKAlphaType.Opaque);
						byte* ptr = (byte*)portion_bitmap.GetPixels().ToPointer();
						for (int row = 0; row < current_height; row++)
						{
							for (int col = 0; col < current_width; col++)
							{
								*(ptr++) = colors[col + row * current_width].R;
								*(ptr++) = colors[col + row * current_width].G;
								*(ptr++) = colors[col + row * current_width].B;
								*(ptr++) = byte.MaxValue;
							}
						}
						SKPaint paint = new SKPaint();
						map_bitmap.Canvas.DrawBitmap(portion_bitmap, SKRect.Create(rect.X, rect.Y, current_width, current_height), paint);
						portion_bitmap.Dispose();
						scaled_render_target.Dispose();
					}
				}
				string logDirectory = "Screenshots";
				int folder = ((Environment.OSVersion.Platform != PlatformID.Unix) ? 26 : 28);
				string fullFilePath = Path.Combine(Path.Combine(Path.Combine(Environment.GetFolderPath((Environment.SpecialFolder)folder), "StardewValley"), logDirectory), filename);
				FileInfo info = new FileInfo(Path.Combine(Path.Combine(Path.Combine(Environment.GetFolderPath((Environment.SpecialFolder)folder), "StardewValley"), logDirectory), "asdfasdf"));
				if (!info.Directory.Exists)
				{
					info.Directory.Create();
				}
				info = null;
				map_bitmap.Snapshot().Encode(SKEncodedImageFormat.Png, 100).SaveTo(new FileStream(fullFilePath, FileMode.OpenOrCreate));
				map_bitmap.Dispose();
			}
			catch (Exception e)
			{
				Console.WriteLine("Map Screenshot: Error taking screenshot: " + e.ToString());
				base.GraphicsDevice.SetRenderTarget(null);
				fail = true;
			}
			if (_lightmap != null)
			{
				_lightmap.Dispose();
				_lightmap = null;
			}
			_lightmap = cached_lightmap;
			options.baseZoomLevel = old_zoom_level;
			takingMapScreenshot = false;
			displayHUD = old_display_hud;
			viewport = old_viewport;
			if (fail)
			{
				return null;
			}
			return filename;
		}

		public void CleanupReturningToTitle()
		{
			if (!game1.IsMainInstance)
			{
				GameRunner.instance.RemoveGameInstance(this);
			}
			else
			{
				foreach (Game1 instance in GameRunner.instance.gameInstances)
				{
					if (instance != this)
					{
						GameRunner.instance.RemoveGameInstance(instance);
					}
				}
			}
			Console.WriteLine("CleanupReturningToTitle()");
			LocalizedContentManager.localizedAssetNames.Clear();
			Event.invalidFestivals.Clear();
			NPC.invalidDialogueFiles.Clear();
			SaveGame.CancelToTitle = false;
			overlayMenu = null;
			multiplayer.cachedMultiplayerMaps.Clear();
			keyboardFocusInstance = null;
			multiplayer.Disconnect(Multiplayer.DisconnectType.ExitedToMainMenu);
			BuildingPaintMenu.savedColors = null;
			startingGameSeed = null;
			_afterNewDayAction = null;
			_currentMinigame = null;
			gameMode = 0;
			_isSaving = false;
			_mouseCursorTransparency = 1f;
			_newDayTask = null;
			newDaySync = null;
			resetPlayer();
			serverHost = null;
			afterDialogues = null;
			afterFade = null;
			afterPause = null;
			afterViewport = null;
			ambientLight = new Color(0, 0, 0, 0);
			background = null;
			startedJukeboxMusic = false;
			boardingBus = false;
			chanceToRainTomorrow = 0.0;
			chatBox = null;
			if (specialCurrencyDisplay != null)
			{
				specialCurrencyDisplay.Cleanup();
			}
			specialCurrencyDisplay = null;
			client = null;
			cloud = null;
			conventionMode = false;
			coopDwellerBorn = false;
			creditsTimer = 0f;
			cropsOfTheWeek = null;
			currentBarnTexture = null;
			currentBillboard = 0;
			currentCoopTexture = null;
			currentCursorTile = Vector2.Zero;
			currentDialogueCharacterIndex = 0;
			currentFloor = 3;
			currentHouseTexture = null;
			currentLightSources.Clear();
			currentLoader = null;
			currentLocation = null;
			currentObjectDialogue.Clear();
			currentQuestionChoice = 0;
			currentSeason = "spring";
			currentSongIndex = 3;
			currentSpeaker = null;
			currentViewportTarget = Vector2.Zero;
			currentWallpaper = 0;
			cursorTileHintCheckTimer = 0;
			CustomData = new SerializableDictionary<string, string>();
			player.team.sharedDailyLuck.Value = 0.001;
			dayOfMonth = 0;
			dealerCalicoJackTotal = null;
			debrisSoundInterval = 0f;
			debrisWeather.Clear();
			debugMode = false;
			debugOutput = null;
			debugPresenceString = "In menus";
			delayedActions.Clear();
			morningSongPlayAction = null;
			dialogueButtonScale = 1f;
			dialogueButtonShrinking = false;
			dialogueTyping = false;
			dialogueTypingInterval = 0;
			dialogueUp = false;
			dialogueWidth = 1024;
			displayFarmer = true;
			displayHUD = true;
			downPolling = 0f;
			drawGrid = false;
			drawLighting = false;
			elliottBookName = "Blue Tower";
			endOfNightMenus.Clear();
			errorMessage = "";
			eveningColor = new Color(255, 255, 0, 255);
			eventOver = false;
			eventUp = false;
			exitToTitle = false;
			facingDirectionAfterWarp = 0;
			fadeIn = true;
			fadeToBlack = false;
			fadeToBlackAlpha = 1.02f;
			FarmerFloor = 29;
			farmerWallpaper = 22;
			farmEvent = null;
			fertilizer = "";
			flashAlpha = 0f;
			floorPrice = 75;
			freezeControls = false;
			gamePadAButtonPolling = 0;
			gameTimeInterval = 0;
			globalFade = false;
			globalFadeSpeed = 0f;
			globalOutdoorLighting = 0f;
			greenhouseTexture = null;
			haltAfterCheck = false;
			hasLoadedGame = false;
			hitShakeTimer = 0;
			hudMessages.Clear();
			inMine = false;
			isActionAtCurrentCursorTile = false;
			isDebrisWeather = false;
			isInspectionAtCurrentCursorTile = false;
			isLightning = false;
			isQuestion = false;
			isRaining = false;
			isSnowing = false;
			jukeboxPlaying = false;
			keyHelpString = "";
			killScreen = false;
			lastCursorMotionWasMouse = true;
			lastCursorTile = Vector2.Zero;
			lastDebugInput = "";
			lastMousePositionBeforeFade = Point.Zero;
			leftPolling = 0f;
			listeningForKeyControlDefinitions = false;
			loadingMessage = "";
			locationRequest = null;
			warpingForForcedRemoteEvent = false;
			locations.Clear();
			logo = null;
			logoScreenTexture = null;
			mailbox.Clear();
			mailboxTexture = null;
			mapDisplayDevice = new XnaDisplayDevice(content, base.GraphicsDevice);
			menuChoice = 0;
			menuUp = false;
			messageAfterPause = "";
			messagePause = false;
			minecartHighScore = 0;
			moneyMadeScreen = null;
			mouseClickPolling = 0;
			mouseCursor = 0;
			multiplayerMode = 0;
			nameSelectType = null;
			nameSelectUp = false;
			netWorldState = new NetRoot<IWorldState>(new NetWorldState());
			newDay = false;
			nonWarpFade = false;
			noteBlockTimer = 0f;
			npcDialogues = null;
			numberOfSelectedItems = -1;
			objectDialoguePortraitPerson = null;
			hasApplied1_3_UpdateChanges = false;
			hasApplied1_4_UpdateChanges = false;
			remoteEventQueue.Clear();
			if (bannedUsers != null)
			{
				bannedUsers.Clear();
			}
			onScreenMenus.Clear();
			onScreenMenus.Add(new Toolbar());
			dayTimeMoneyBox = new DayTimeMoneyBox();
			onScreenMenus.Add(dayTimeMoneyBox);
			buffsDisplay = new BuffsDisplay();
			onScreenMenus.Add(buffsDisplay);
			bool gamepad_controls = options.gamepadControls;
			bool snappy_menus = options.snappyMenus;
			options = new Options();
			options.gamepadControls = gamepad_controls;
			options.snappyMenus = snappy_menus;
			foreach (KeyValuePair<long, Farmer> otherFarmer in otherFarmers)
			{
				otherFarmer.Value.unload();
			}
			otherFarmers.Clear();
			outdoorLight = new Color(255, 255, 0, 255);
			overlayMenu = null;
			overrideGameMenuReset = false;
			panFacingDirectionWait = false;
			panMode = false;
			panModeString = null;
			particleRaining = false;
			pauseAccumulator = 0f;
			paused = false;
			pauseThenDoFunctionTimer = 0;
			pauseTime = 0f;
			percentageToWinStardewHero = 70;
			pickingTool = false;
			pickToolInterval = 0f;
			previousViewportPosition = Vector2.Zero;
			priceOfSelectedItem = 0;
			progressBar = false;
			questionChoices.Clear();
			questOfTheDay = null;
			quit = false;
			rightClickPolling = 0;
			rightPolling = 0f;
			runThreshold = 0.5f;
			samBandName = "The Alfalfas";
			saveOnNewDay = true;
			startingCabins = 0;
			cabinsSeparate = false;
			screenGlow = false;
			screenGlowAlpha = 0f;
			screenGlowColor = new Color(0, 0, 0, 0);
			screenGlowHold = false;
			screenGlowMax = 0f;
			screenGlowRate = 0.005f;
			screenGlowUp = false;
			screenOverlayTempSprites.Clear();
			uiOverlayTempSprites.Clear();
			selectedItemsType = null;
			server = null;
			shiny = Vector2.Zero;
			newGameSetupOptions.Clear();
			shippingTax = false;
			showingEndOfNightStuff = false;
			showKeyHelp = false;
			slotResult = null;
			spawnMonstersAtNight = false;
			staminaShakeTimer = 0;
			starCropShimmerPause = 0f;
			swordSwipe = null;
			swordSwipeDark = null;
			textColor = new Color(34, 17, 34, 255);
			textShadowColor = new Color(206, 156, 95, 255);
			thumbstickMotionAccell = 1f;
			thumbstickMotionMargin = 0;
			thumbstickPollingTimer = 0;
			thumbStickSensitivity = 0.1f;
			timeOfDay = 600;
			timeOfDayAfterFade = -1;
			timerUntilMouseFade = 0;
			titleScreenBG = null;
			tmpTimeOfDay = 0;
			toggleFullScreen = false;
			toolHold = 0f;
			toolIconBox = null;
			ResetToolSpriteSheet();
			triggerPolling = 0;
			tvStation = -1;
			tvStationTexture = null;
			uniqueIDForThisGame = (ulong)(DateTime.UtcNow - new DateTime(2012, 6, 22)).TotalSeconds;
			upPolling = 0f;
			viewportFreeze = false;
			viewportHold = 0;
			viewportPositionLerp = Vector2.Zero;
			viewportReachedTarget = null;
			viewportSpeed = 2f;
			viewportTarget = new Vector2(-2.1474836E+09f, -2.1474836E+09f);
			wallpaperPrice = 75;
			wasMouseVisibleThisFrame = true;
			wasRainingYesterday = false;
			weatherForTomorrow = 0;
			elliottPiano = 0;
			weatherIcon = 0;
			weddingToday = false;
			whereIsTodaysFest = null;
			worldStateIDs.Clear();
			whichFarm = 0;
			windGust = 0f;
			xLocationAfterWarp = 0;
			game1.xTileContent.Dispose();
			game1.xTileContent = CreateContentManager(content.ServiceProvider, content.RootDirectory);
			year = 1;
			yLocationAfterWarp = 0;
			mailDeliveredFromMailForTomorrow.Clear();
			bundleType = BundleType.Default;
			JojaMart.Morris = null;
			AmbientLocationSounds.onLocationLeave();
			WeatherDebris.globalWind = -0.25f;
			Utility.killAllStaticLoopingSoundCues();
			TitleMenu.subMenu = null;
			OptionsDropDown.selected = null;
			JunimoNoteMenu.tempSprites.Clear();
			JunimoNoteMenu.screenSwipe = null;
			JunimoNoteMenu.canClick = true;
			GameMenu.forcePreventClose = false;
			Club.timesPlayedCalicoJack = 0;
			MineShaft.activeMines.Clear();
			MineShaft.permanentMineChanges.Clear();
			MineShaft.numberOfCraftedStairsUsedThisRun = 0;
			MineShaft.mushroomLevelsGeneratedToday.Clear();
			Desert.boughtMagicRockCandy = false;
			VolcanoDungeon.activeLevels.Clear();
			Rumble.stopRumbling();
			game1.refreshWindowSettings();
			if (activeClickableMenu != null && activeClickableMenu is TitleMenu)
			{
				(activeClickableMenu as TitleMenu).applyPreferences();
				activeClickableMenu.gameWindowSizeChanged(graphics.GraphicsDevice.Viewport.Bounds, graphics.GraphicsDevice.Viewport.Bounds);
			}
		}

		public static void GetHasRoomAnotherFarmAsync(ReportHasRoomAnotherFarm callback)
		{
			if (LocalMultiplayer.IsLocalMultiplayer())
			{
				bool yes = GetHasRoomAnotherFarm();
				callback(yes);
				return;
			}
			Task task = new Task(delegate
			{
				Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
				bool hasRoomAnotherFarm = GetHasRoomAnotherFarm();
				callback(hasRoomAnotherFarm);
			});
			hooks.StartTask(task, "Farm_SpaceCheck");
		}

		private static string GameModeToString(byte mode)
		{
			return mode switch
			{
				4 => $"logoScreenGameMode ({mode})", 
				0 => $"titleScreenGameMode ({mode})", 
				1 => $"loadScreenGameMode ({mode})", 
				2 => $"newGameMode ({mode})", 
				3 => $"playingGameMode ({mode})", 
				6 => $"loadingMode ({mode})", 
				7 => $"saveMode ({mode})", 
				8 => $"saveCompleteMode ({mode})", 
				9 => $"selectGameScreen ({mode})", 
				10 => $"creditsMode ({mode})", 
				11 => $"errorLogMode ({mode})", 
				_ => $"unknown ({mode})", 
			};
		}

		public static string GetVersionString()
		{
			if (string.IsNullOrEmpty(versionLabel))
			{
				return version;
			}
			return version + " " + versionLabel;
		}

		public static void ResetToolSpriteSheet()
		{
			if (_toolSpriteSheet != null)
			{
				_toolSpriteSheet.Dispose();
				_toolSpriteSheet = null;
			}
			Texture2D texture = content.Load<Texture2D>("TileSheets\\tools");
			int w = texture.Width;
			int h = texture.Height;
			_ = texture.LevelCount;
			Texture2D texture2D = new Texture2D(game1.GraphicsDevice, w, h, mipmap: false, SurfaceFormat.Color);
			Color[] data = new Color[w * h];
			texture.GetData(data);
			texture2D.SetData(data);
			_toolSpriteSheet = texture2D;
		}

		public static void SetSaveName(string new_save_name)
		{
			if (new_save_name == null)
			{
				new_save_name = "";
			}
			_currentSaveName = new_save_name;
			_setSaveName = true;
		}

		public static string GetSaveGameName(bool set_value = true)
		{
			if (set_value)
			{
				string base_name = MasterPlayer.farmName.Value;
				string save_name = base_name + "_" + DateTime.Now.ToString("yyyyMMddHHMMss");
				int collision_index = 2;
				while (SaveGame.IsNewGameSaveNameCollision(save_name))
				{
					save_name = base_name + collision_index;
					collision_index++;
				}
				SetSaveName(save_name);
			}
			return _currentSaveName;
		}

		private static void allocateLightmap(int width, int height)
		{
			int quality = 32;
			float zoom = 1f;
			if (options != null)
			{
				quality = options.lightingQuality;
				zoom = ((!game1.useUnscaledLighting) ? options.zoomLevel : 1f);
			}
			int w = (int)((float)width * (1f / zoom) + 64f) / (quality / 2);
			int h = (int)((float)height * (1f / zoom) + 64f) / (quality / 2);
			if (lightmap == null || lightmap.Width != w || lightmap.Height != h)
			{
				if (_lightmap != null)
				{
					_lightmap.Dispose();
				}
				_lightmap = new RenderTarget2D(graphics.GraphicsDevice, w, h, mipMap: false, SurfaceFormat.Color, DepthFormat.None, 0, RenderTargetUsage.PreserveContents);
			}
		}

		public static bool canHaveWeddingOnDay(int day, string season)
		{
			if (Utility.isFestivalDay(day, season))
			{
				return false;
			}
			return true;
		}

		public static void RefreshQuestOfTheDay()
		{
			questOfTheDay = Utility.getQuestOfTheDay();
			if (Utility.isFestivalDay(dayOfMonth, currentSeason) || Utility.isFestivalDay(dayOfMonth + 1, currentSeason))
			{
				questOfTheDay = null;
			}
		}

		public static void ExitToTitle(Action postExitCallback = null)
		{
			_requestedMusicTracks.Clear();
			UpdateRequestedMusicTrack();
			changeMusicTrack("none");
			setGameMode(0);
			exitToTitle = true;
			postExitToTitleCallback = postExitCallback;
		}

		public Game1(PlayerIndex player_index, int index)
			: this()
		{
			instancePlayerOneIndex = player_index;
			instanceIndex = index;
		}

		public Game1()
		{
			instanceId = GameRunner.instance.GetNewInstanceID();
			if (Program.gamePtr == null)
			{
				Program.gamePtr = this;
			}
			_temporaryContent = CreateContentManager(base.Content.ServiceProvider, base.Content.RootDirectory);
		}

		public void TranslateFields()
		{
			LocalizedContentManager.localizedAssetNames.Clear();
			samBandName = content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.2156");
			elliottBookName = content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.2157");
			objectSpriteSheet = content.Load<Texture2D>("Maps\\springobjects");
			dialogueFont = content.Load<SpriteFont>("Fonts\\SpriteFont1");
			smallFont = content.Load<SpriteFont>("Fonts\\SmallFont");
			smallFont.LineSpacing = 26;
			if (LocalizedContentManager.CurrentLanguageCode == LocalizedContentManager.LanguageCode.ko)
			{
				smallFont.LineSpacing += 16;
			}
			else if (LocalizedContentManager.CurrentLanguageCode == LocalizedContentManager.LanguageCode.tr)
			{
				smallFont.LineSpacing += 4;
			}
			else if (LocalizedContentManager.CurrentLanguageCode == LocalizedContentManager.LanguageCode.mod)
			{
				smallFont.LineSpacing = LocalizedContentManager.CurrentModLanguage.SmallFontLineSpacing;
			}
			tinyFont = content.Load<SpriteFont>("Fonts\\tinyFont");
			tinyFontBorder = content.Load<SpriteFont>("Fonts\\tinyFontBorder");
			objectInformation = content.Load<Dictionary<int, string>>("Data\\ObjectInformation");
			clothingInformation = content.Load<Dictionary<int, string>>("Data\\ClothingInformation");
			bigCraftablesInformation = content.Load<Dictionary<int, string>>("Data\\BigCraftablesInformation");
			achievements = content.Load<Dictionary<int, string>>("Data\\Achievements");
			CraftingRecipe.craftingRecipes = content.Load<Dictionary<string, string>>("Data\\CraftingRecipes");
			CraftingRecipe.cookingRecipes = content.Load<Dictionary<string, string>>("Data\\CookingRecipes");
			MovieTheater.ClearCachedLocalizedData();
			mouseCursors = content.Load<Texture2D>("LooseSprites\\Cursors");
			mouseCursors2 = content.Load<Texture2D>("LooseSprites\\Cursors2");
			giftboxTexture = content.Load<Texture2D>("LooseSprites\\Giftbox");
			controllerMaps = content.Load<Texture2D>("LooseSprites\\ControllerMaps");
			NPCGiftTastes = content.Load<Dictionary<string, string>>("Data\\NPCGiftTastes");
			_shortDayDisplayName[0] = content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.3042");
			_shortDayDisplayName[1] = content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.3043");
			_shortDayDisplayName[2] = content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.3044");
			_shortDayDisplayName[3] = content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.3045");
			_shortDayDisplayName[4] = content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.3046");
			_shortDayDisplayName[5] = content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.3047");
			_shortDayDisplayName[6] = content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.3048");
		}

		public void exitEvent(object sender, EventArgs e)
		{
			multiplayer.Disconnect(Multiplayer.DisconnectType.ClosedGame);
			keyboardDispatcher.Cleanup();
		}

		public void refreshWindowSettings()
		{
			GameRunner.instance.OnWindowSizeChange(null, null);
		}

		public void Window_ClientSizeChanged(object sender, EventArgs e)
		{
			if (_windowResizing)
			{
				return;
			}
			Console.WriteLine("Window_ClientSizeChanged(); Window.ClientBounds={0}", base.Window.ClientBounds);
			if (options == null)
			{
				Console.WriteLine("Window_ClientSizeChanged(); options is null, returning.");
				return;
			}
			_windowResizing = true;
			int w = (graphics.IsFullScreen ? graphics.PreferredBackBufferWidth : base.Window.ClientBounds.Width);
			int h = (graphics.IsFullScreen ? graphics.PreferredBackBufferHeight : base.Window.ClientBounds.Height);
			GameRunner.instance.ExecuteForInstances(delegate(Game1 instance)
			{
				instance.SetWindowSize(w, h);
			});
			_windowResizing = false;
		}

		public virtual void SetWindowSize(int w, int h)
		{
			Microsoft.Xna.Framework.Rectangle oldWindow = new Microsoft.Xna.Framework.Rectangle(viewport.X, viewport.Y, viewport.Width, viewport.Height);
			_ = base.Window.ClientBounds;
			bool recalculateClientBounds = false;
			if (Environment.OSVersion.Platform == PlatformID.Win32NT)
			{
				if (w < 1280 && !graphics.IsFullScreen)
				{
					w = 1280;
					recalculateClientBounds = true;
				}
				if (h < 720 && !graphics.IsFullScreen)
				{
					h = 720;
					recalculateClientBounds = true;
				}
			}
			if (!graphics.IsFullScreen && base.Window.AllowUserResizing)
			{
				graphics.PreferredBackBufferWidth = w;
				graphics.PreferredBackBufferHeight = h;
			}
			if (recalculateClientBounds)
			{
				_ = base.Window.ClientBounds;
			}
			if (base.IsMainInstance && graphics.SynchronizeWithVerticalRetrace != options.vsyncEnabled)
			{
				graphics.SynchronizeWithVerticalRetrace = options.vsyncEnabled;
				Console.WriteLine("Vsync toggled: " + graphics.SynchronizeWithVerticalRetrace);
			}
			graphics.ApplyChanges();
			try
			{
				if (graphics.IsFullScreen)
				{
					localMultiplayerWindow = new Microsoft.Xna.Framework.Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
				}
				else
				{
					localMultiplayerWindow = new Microsoft.Xna.Framework.Rectangle(0, 0, w, h);
				}
			}
			catch (Exception)
			{
			}
			defaultDeviceViewport = new Viewport(localMultiplayerWindow);
			List<Vector4> screen_splits = new List<Vector4>();
			if (GameRunner.instance.gameInstances.Count <= 1)
			{
				screen_splits.Add(new Vector4(0f, 0f, 1f, 1f));
			}
			else if (GameRunner.instance.gameInstances.Count == 2)
			{
				screen_splits.Add(new Vector4(0f, 0f, 0.5f, 1f));
				screen_splits.Add(new Vector4(0.5f, 0f, 0.5f, 1f));
			}
			else if (GameRunner.instance.gameInstances.Count == 3)
			{
				screen_splits.Add(new Vector4(0f, 0f, 1f, 0.5f));
				screen_splits.Add(new Vector4(0f, 0.5f, 0.5f, 0.5f));
				screen_splits.Add(new Vector4(0.5f, 0.5f, 0.5f, 0.5f));
			}
			else if (GameRunner.instance.gameInstances.Count == 4)
			{
				screen_splits.Add(new Vector4(0f, 0f, 0.5f, 0.5f));
				screen_splits.Add(new Vector4(0.5f, 0f, 0.5f, 0.5f));
				screen_splits.Add(new Vector4(0f, 0.5f, 0.5f, 0.5f));
				screen_splits.Add(new Vector4(0.5f, 0.5f, 0.5f, 0.5f));
			}
			if (GameRunner.instance.gameInstances.Count <= 1)
			{
				zoomModifier = 1f;
			}
			else
			{
				zoomModifier = 0.5f;
			}
			Vector4 current_screen_split = screen_splits[game1.instanceIndex];
			Vector2? old_ui_dimensions = null;
			if (uiScreen != null)
			{
				old_ui_dimensions = new Vector2(uiScreen.Width, uiScreen.Height);
			}
			localMultiplayerWindow.X = (int)((float)w * current_screen_split.X);
			localMultiplayerWindow.Y = (int)((float)h * current_screen_split.Y);
			localMultiplayerWindow.Width = (int)Math.Ceiling((float)w * current_screen_split.Z);
			localMultiplayerWindow.Height = (int)Math.Ceiling((float)h * current_screen_split.W);
			try
			{
				int sw = (int)Math.Ceiling((float)localMultiplayerWindow.Width * (1f / options.zoomLevel));
				int sh = (int)Math.Ceiling((float)localMultiplayerWindow.Height * (1f / options.zoomLevel));
				screen = new RenderTarget2D(graphics.GraphicsDevice, sw, sh, mipMap: false, SurfaceFormat.Color, DepthFormat.None, 0, RenderTargetUsage.PreserveContents);
				screen.Name = "Screen";
				int uw = (int)Math.Ceiling((float)localMultiplayerWindow.Width / options.uiScale);
				int uh = (int)Math.Ceiling((float)localMultiplayerWindow.Height / options.uiScale);
				uiScreen = new RenderTarget2D(graphics.GraphicsDevice, uw, uh, mipMap: false, SurfaceFormat.Color, DepthFormat.None, 0, RenderTargetUsage.PreserveContents);
				uiScreen.Name = "UI Screen";
			}
			catch (Exception)
			{
			}
			updateViewportForScreenSizeChange(fullscreenChange: false, localMultiplayerWindow.Width, localMultiplayerWindow.Height);
			if (old_ui_dimensions.HasValue && old_ui_dimensions.Value.X == (float)uiScreen.Width && old_ui_dimensions.Value.Y == (float)uiScreen.Height)
			{
				return;
			}
			PushUIMode();
			if (textEntry != null)
			{
				textEntry.gameWindowSizeChanged(oldWindow, new Microsoft.Xna.Framework.Rectangle(viewport.X, viewport.Y, viewport.Width, viewport.Height));
			}
			foreach (IClickableMenu onScreenMenu in onScreenMenus)
			{
				onScreenMenu.gameWindowSizeChanged(oldWindow, new Microsoft.Xna.Framework.Rectangle(viewport.X, viewport.Y, viewport.Width, viewport.Height));
			}
			if (currentMinigame != null)
			{
				currentMinigame.changeScreenSize();
			}
			if (activeClickableMenu != null)
			{
				activeClickableMenu.gameWindowSizeChanged(oldWindow, new Microsoft.Xna.Framework.Rectangle(viewport.X, viewport.Y, viewport.Width, viewport.Height));
			}
			if (activeClickableMenu is GameMenu && !overrideGameMenuReset)
			{
				if ((activeClickableMenu as GameMenu).GetCurrentPage() is OptionsPage)
				{
					((activeClickableMenu as GameMenu).GetCurrentPage() as OptionsPage).preWindowSizeChange();
				}
				activeClickableMenu = new GameMenu((activeClickableMenu as GameMenu).currentTab);
				if ((activeClickableMenu as GameMenu).GetCurrentPage() is OptionsPage)
				{
					((activeClickableMenu as GameMenu).GetCurrentPage() as OptionsPage).postWindowSizeChange();
				}
			}
			PopUIMode();
		}

		private void Game1_Exiting(object sender, EventArgs e)
		{
			Program.sdk.Shutdown();
		}

		public static void setGameMode(byte mode)
		{
			Console.WriteLine("setGameMode( '{0}' )", GameModeToString(mode));
			_gameMode = mode;
			if (temporaryContent != null)
			{
				temporaryContent.Unload();
			}
			switch (mode)
			{
			case 0:
			{
				bool skip = false;
				if (activeClickableMenu != null && currentGameTime != null && currentGameTime.TotalGameTime.TotalSeconds > 10.0)
				{
					skip = true;
				}
				if (game1.instanceIndex <= 0)
				{
					activeClickableMenu = new TitleMenu();
					if (skip)
					{
						(activeClickableMenu as TitleMenu).skipToTitleButtons();
					}
				}
				break;
			}
			case 3:
				hasApplied1_3_UpdateChanges = true;
				hasApplied1_4_UpdateChanges = false;
				break;
			}
		}

		public static void updateViewportForScreenSizeChange(bool fullscreenChange, int width, int height)
		{
			forceSnapOnNextViewportUpdate = true;
			if (graphics.GraphicsDevice != null)
			{
				allocateLightmap(width, height);
			}
			width = (int)Math.Ceiling((float)width / options.zoomLevel);
			height = (int)Math.Ceiling((float)height / options.zoomLevel);
			Point center = new Point(viewport.X + viewport.Width / 2, viewport.Y + viewport.Height / 2);
			bool size_dirty = false;
			if (viewport.Width != width || viewport.Height != height)
			{
				size_dirty = true;
			}
			viewport = new xTile.Dimensions.Rectangle(center.X - width / 2, center.Y - height / 2, width, height);
			if (currentLocation == null)
			{
				return;
			}
			if (eventUp)
			{
				if (!IsFakedBlackScreen() && currentLocation.IsOutdoors)
				{
					clampViewportToGameMap();
				}
				return;
			}
			if (viewport.X >= 0 || !currentLocation.IsOutdoors || fullscreenChange)
			{
				center = new Point(viewport.X + viewport.Width / 2, viewport.Y + viewport.Height / 2);
				viewport = new xTile.Dimensions.Rectangle(center.X - width / 2, center.Y - height / 2, width, height);
				UpdateViewPort(overrideFreeze: true, center);
			}
			if (size_dirty)
			{
				forceSnapOnNextViewportUpdate = true;
				randomizeRainPositions();
				randomizeDebrisWeatherPositions(debrisWeather);
			}
		}

		public void Instance_Initialize()
		{
			Initialize();
		}

		public static bool IsFading()
		{
			if (!globalFade && (!fadeIn || !(fadeToBlackAlpha > 0f)))
			{
				if (fadeToBlack)
				{
					return fadeToBlackAlpha < 1f;
				}
				return false;
			}
			return true;
		}

		public static bool IsFakedBlackScreen()
		{
			if (currentMinigame != null)
			{
				return false;
			}
			if (CurrentEvent != null && CurrentEvent.currentCustomEventScript != null)
			{
				return false;
			}
			if (!eventUp)
			{
				return false;
			}
			return (float)(int)Math.Floor((float)new Point(viewport.X + viewport.Width / 2, viewport.Y + viewport.Height / 2).X / 64f) <= -200f;
		}

		/// <summary>
		/// Allows the game to perform any initialization it needs to before starting to run.
		/// This is where it can query for any required services and load any non-graphic
		/// related content.  Calling base.Initialize will enumerate through any components
		/// and initialize them as well.
		/// </summary>
		protected override void Initialize()
		{
			keyboardDispatcher = new KeyboardDispatcher(base.Window);
			screenFade = new ScreenFade(onFadeToBlackComplete, onFadedBackInComplete);
			options = new Options();
			options.musicVolumeLevel = 1f;
			options.soundVolumeLevel = 1f;
			otherFarmers = new NetRootDictionary<long, Farmer>();
			otherFarmers.Serializer = SaveGame.farmerSerializer;
			viewport = new xTile.Dimensions.Rectangle(new Size(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight));
			string rootpath = base.Content.RootDirectory;
			if (!File.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "Resources", rootpath, "XACT", "FarmerSounds.xgs")))
			{
				File.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, rootpath, "XACT", "FarmerSounds.xgs"));
			}
			if (base.IsMainInstance)
			{
				try
				{
					AudioEngine obj = new AudioEngine(Path.Combine(rootpath, "XACT", "FarmerSounds.xgs"));
					obj.GetReverbSettings()[18] = 4f;
					obj.GetReverbSettings()[17] = -12f;
					audioEngine = new AudioEngineWrapper(obj);
					waveBank = new WaveBank(audioEngine.Engine, Path.Combine(rootpath, "XACT", "Wave Bank.xwb"));
					waveBank1_4 = new WaveBank(audioEngine.Engine, Path.Combine(rootpath, "XACT", "Wave Bank(1.4).xwb"));
					soundBank = new SoundBankWrapper(new SoundBank(audioEngine.Engine, Path.Combine(rootpath, "XACT", "Sound Bank.xsb")));
				}
				catch (Exception e)
				{
					Console.WriteLine("Game.Initialize() caught exception initializing XACT:\n{0}", e);
					audioEngine = new DummyAudioEngine();
					soundBank = new DummySoundBank();
				}
			}
			audioEngine.Update();
			musicCategory = audioEngine.GetCategory("Music");
			soundCategory = audioEngine.GetCategory("Sound");
			ambientCategory = audioEngine.GetCategory("Ambient");
			footstepCategory = audioEngine.GetCategory("Footsteps");
			currentSong = null;
			if (soundBank != null)
			{
				wind = soundBank.GetCue("wind");
				chargeUpSound = soundBank.GetCue("toolCharge");
			}
			int width = graphics.GraphicsDevice.Viewport.Width;
			int height = graphics.GraphicsDevice.Viewport.Height;
			screen = new RenderTarget2D(graphics.GraphicsDevice, width, height, mipMap: false, SurfaceFormat.Color, DepthFormat.None, 0, RenderTargetUsage.PreserveContents);
			allocateLightmap(width, height);
			AmbientLocationSounds.InitShared();
			previousViewportPosition = Vector2.Zero;
			PushUIMode();
			PopUIMode();
			setRichPresence("menus");
		}

		/// <summary>
		/// one-time changes for loaded files for the 1.3 update
		/// </summary>
		public static void apply1_3_UpdateChanges()
		{
			if (IsMasterGame)
			{
				if (!player.craftingRecipes.ContainsKey("Wood Sign"))
				{
					player.craftingRecipes.Add("Wood Sign", 0);
				}
				if (!player.craftingRecipes.ContainsKey("Stone Sign"))
				{
					player.craftingRecipes.Add("Stone Sign", 0);
				}
				FarmHouse farmHouse = getLocationFromName("FarmHouse") as FarmHouse;
				farmHouse.furniture.Add(new Furniture(1792, Utility.PointToVector2(farmHouse.getFireplacePoint())));
				if (!MasterPlayer.mailReceived.Contains("JojaMember") && !getLocationFromName("Town").isTileOccupiedForPlacement(new Vector2(57f, 16f)))
				{
					getLocationFromName("Town").objects.Add(new Vector2(57f, 16f), new Object(Vector2.Zero, 55));
				}
				MarkFloorChestAsCollectedIfNecessary(10);
				MarkFloorChestAsCollectedIfNecessary(20);
				MarkFloorChestAsCollectedIfNecessary(40);
				MarkFloorChestAsCollectedIfNecessary(50);
				MarkFloorChestAsCollectedIfNecessary(60);
				MarkFloorChestAsCollectedIfNecessary(70);
				MarkFloorChestAsCollectedIfNecessary(80);
				MarkFloorChestAsCollectedIfNecessary(90);
				MarkFloorChestAsCollectedIfNecessary(100);
				hasApplied1_3_UpdateChanges = true;
			}
		}

		/// <summary>
		/// one-time changes for loaded files for the 1.4 update
		/// </summary>
		public static void apply1_4_UpdateChanges()
		{
			if (!IsMasterGame)
			{
				return;
			}
			foreach (Farmer f in getAllFarmers())
			{
				foreach (string v2 in f.friendshipData.Keys)
				{
					f.friendshipData[v2].Points = Math.Min(f.friendshipData[v2].Points, 3125);
				}
				if (f.ForagingLevel >= 7 && !f.craftingRecipes.ContainsKey("Tree Fertilizer"))
				{
					f.craftingRecipes.Add("Tree Fertilizer", 0);
				}
			}
			foreach (KeyValuePair<string, string> v in netWorldState.Value.BundleData)
			{
				int key = Convert.ToInt32(v.Key.Split('/')[1]);
				if (!netWorldState.Value.Bundles.ContainsKey(key))
				{
					netWorldState.Value.Bundles.Add(key, new NetArray<bool, NetBool>(v.Value.Split('/')[2].Split(' ').Length));
				}
				if (!netWorldState.Value.BundleRewards.ContainsKey(key))
				{
					netWorldState.Value.BundleRewards.Add(key, new NetBool(value: false));
				}
			}
			foreach (Farmer allFarmer in getAllFarmers())
			{
				foreach (Item i in allFarmer.items)
				{
					if (i != null)
					{
						i.HasBeenInInventory = true;
					}
				}
			}
			recalculateLostBookCount();
			Utility.iterateChestsAndStorage(delegate(Item item)
			{
				item.HasBeenInInventory = true;
			});
			foreach (TerrainFeature feature in getLocationFromName("Greenhouse").terrainFeatures.Values)
			{
				if (feature is HoeDirt)
				{
					((HoeDirt)feature).isGreenhouseDirt.Value = true;
				}
			}
			hasApplied1_4_UpdateChanges = true;
		}

		public static void applySaveFix(SaveGame.SaveFixes save_fix)
		{
			switch (save_fix)
			{
			case SaveGame.SaveFixes.AddTownBush:
				if (getLocationFromName("Town") is Town town)
				{
					Vector2 tile = new Vector2(61f, 93f);
					if (town.getLargeTerrainFeatureAt((int)tile.X, (int)tile.Y) == null)
					{
						town.largeTerrainFeatures.Add(new Bush(tile, 2, town));
					}
				}
				break;
			case SaveGame.SaveFixes.AddBirdie:
				addBirdieIfNecessary();
				break;
			case SaveGame.SaveFixes.AddBugSteakRecipe:
				if (player.combatLevel.Value >= 2 && !player.craftingRecipes.ContainsKey("Bug Steak"))
				{
					player.craftingRecipes.Add("Bug Steak", 0);
				}
				break;
			case SaveGame.SaveFixes.StoredBigCraftablesStackFix:
				Utility.iterateChestsAndStorage(delegate(Item item)
				{
					if (item is Object)
					{
						Object @object = item as Object;
						if ((bool)@object.bigCraftable && @object.Stack == 0)
						{
							@object.Stack = 1;
						}
					}
				});
				break;
			case SaveGame.SaveFixes.PorchedCabinBushesFix:
			{
				foreach (Building building in getFarm().buildings)
				{
					if ((int)building.daysOfConstructionLeft <= 0 && building.indoors.Value is Cabin)
					{
						building.removeOverlappingBushes(getFarm());
					}
				}
				break;
			}
			case SaveGame.SaveFixes.ChangeObeliskFootprintHeight:
			{
				foreach (Building building2 in getFarm().buildings)
				{
					if (building2.buildingType.Value.Contains("Obelisk"))
					{
						building2.tilesHigh.Value = 2;
						building2.tileY.Value++;
					}
				}
				break;
			}
			case SaveGame.SaveFixes.CreateStorageDressers:
			{
				Utility.iterateChestsAndStorage(delegate(Item item)
				{
					if (item is Clothing)
					{
						item.Category = -100;
					}
				});
				List<DecoratableLocation> decoratable_locations = new List<DecoratableLocation>();
				foreach (GameLocation location4 in locations)
				{
					if (location4 is DecoratableLocation)
					{
						decoratable_locations.Add(location4 as DecoratableLocation);
					}
				}
				foreach (Building building3 in getFarm().buildings)
				{
					if (building3.indoors != null)
					{
						GameLocation location3 = building3.indoors;
						if (location3 is DecoratableLocation)
						{
							decoratable_locations.Add(location3 as DecoratableLocation);
						}
					}
				}
				{
					foreach (DecoratableLocation location2 in decoratable_locations)
					{
						List<Furniture> furniture_to_add = new List<Furniture>();
						for (int j = 0; j < location2.furniture.Count; j++)
						{
							Furniture old_furniture = location2.furniture[j];
							if (old_furniture.ParentSheetIndex == 704 || old_furniture.ParentSheetIndex == 709 || old_furniture.ParentSheetIndex == 714 || old_furniture.ParentSheetIndex == 719)
							{
								StorageFurniture storage_furniture = new StorageFurniture(old_furniture.ParentSheetIndex, old_furniture.TileLocation, old_furniture.currentRotation);
								furniture_to_add.Add(storage_furniture);
								location2.furniture.RemoveAt(j);
								j--;
							}
						}
						for (int i = 0; i < furniture_to_add.Count; i++)
						{
							location2.furniture.Add(furniture_to_add[i]);
						}
					}
					break;
				}
			}
			case SaveGame.SaveFixes.InferPreserves:
			{
				int[] preserve_item_indices = new int[4] { 350, 348, 344, 342 };
				string[] suffixes = new string[3] { " Juice", " Wine", " Jelly" };
				Object.PreserveType[] suffix_preserve_types = new Object.PreserveType[3]
				{
					Object.PreserveType.Juice,
					Object.PreserveType.Wine,
					Object.PreserveType.Jelly
				};
				string[] prefixes = new string[1] { "Pickled " };
				Object.PreserveType[] prefix_preserve_types = new Object.PreserveType[1] { Object.PreserveType.Pickle };
				Utility.iterateAllItems(delegate(Item item)
				{
					if (item is Object && Utility.IsNormalObjectAtParentSheetIndex(item, item.ParentSheetIndex) && preserve_item_indices.Contains(item.ParentSheetIndex) && !(item as Object).preserve.Value.HasValue)
					{
						for (int n = 0; n < suffixes.Length; n++)
						{
							string text = suffixes[n];
							if (item.Name.EndsWith(text))
							{
								string value = item.Name.Substring(0, item.Name.Length - text.Length);
								int num = -1;
								foreach (int current in objectInformation.Keys)
								{
									if (objectInformation[current].Substring(0, objectInformation[current].IndexOf('/')).Equals(value))
									{
										num = current;
										break;
									}
								}
								if (num >= 0)
								{
									(item as Object).preservedParentSheetIndex.Value = num;
									(item as Object).preserve.Value = suffix_preserve_types[n];
									return;
								}
							}
						}
						for (int num2 = 0; num2 < prefixes.Length; num2++)
						{
							string text2 = prefixes[num2];
							if (item.Name.StartsWith(text2))
							{
								string value2 = item.Name.Substring(text2.Length);
								int num3 = -1;
								foreach (int current2 in objectInformation.Keys)
								{
									if (objectInformation[current2].Substring(0, objectInformation[current2].IndexOf('/')).Equals(value2))
									{
										num3 = current2;
										break;
									}
								}
								if (num3 >= 0)
								{
									(item as Object).preservedParentSheetIndex.Value = num3;
									(item as Object).preserve.Value = prefix_preserve_types[num2];
									break;
								}
							}
						}
					}
				});
				break;
			}
			case SaveGame.SaveFixes.TransferHatSkipHairFlag:
				Utility.iterateAllItems(delegate(Item item)
				{
					if (item is Hat)
					{
						Hat hat = item as Hat;
						if (hat.skipHairDraw)
						{
							hat.hairDrawType.Set(0);
							hat.skipHairDraw = false;
						}
					}
				});
				break;
			case SaveGame.SaveFixes.RevealSecretNoteItemTastes:
			{
				Dictionary<int, string> notes_data = content.Load<Dictionary<int, string>>("Data\\SecretNotes");
				for (int k = 0; k < 21; k++)
				{
					if (notes_data.ContainsKey(k) && player.secretNotesSeen.Contains(k))
					{
						Utility.ParseGiftReveals(notes_data[k]);
					}
				}
				break;
			}
			case SaveGame.SaveFixes.TransferHoneyTypeToPreserves:
				(new int[1])[0] = 340;
				Utility.iterateAllItems(delegate(Item item)
				{
					if (item is Object && Utility.IsNormalObjectAtParentSheetIndex(item, item.ParentSheetIndex) && item.ParentSheetIndex == 340 && (item as Object).preservedParentSheetIndex.Value <= 0)
					{
						if ((item as Object).honeyType.Value.HasValue && (item as Object).honeyType.Value.Value >= (Object.HoneyType)0)
						{
							(item as Object).preservedParentSheetIndex.Value = (int)(item as Object).honeyType.Value.Value;
						}
						else
						{
							(item as Object).honeyType.Value = Object.HoneyType.Wild;
							(item as Object).preservedParentSheetIndex.Value = -1;
						}
					}
				});
				break;
			case SaveGame.SaveFixes.TransferNoteBlockScale:
				Utility.iterateAllItems(delegate(Item item)
				{
					if (item is Object && Utility.IsNormalObjectAtParentSheetIndex(item, item.ParentSheetIndex) && (item.ParentSheetIndex == 363 || item.ParentSheetIndex == 464))
					{
						(item as Object).preservedParentSheetIndex.Value = (int)(item as Object).scale.X;
					}
				});
				break;
			case SaveGame.SaveFixes.FixCropHarvestAmountsAndInferSeedIndex:
				Utility.iterateAllCrops(delegate(Crop crop)
				{
					crop.ResetCropYield();
				});
				break;
			case SaveGame.SaveFixes.Level9PuddingFishingRecipe2:
			case SaveGame.SaveFixes.Level9PuddingFishingRecipe3:
				if (player.cookingRecipes.ContainsKey("Ocean Mineral Pudding"))
				{
					player.cookingRecipes.Remove("Ocean Mineral Pudding");
				}
				if (player.fishingLevel.Value >= 9 && !player.cookingRecipes.ContainsKey("Seafoam Pudding"))
				{
					player.cookingRecipes.Add("Seafoam Pudding", 0);
				}
				break;
			case SaveGame.SaveFixes.quarryMineBushes:
			{
				GameLocation l = getLocationFromName("Mountain");
				l.largeTerrainFeatures.Add(new Bush(new Vector2(101f, 18f), 1, l));
				l.largeTerrainFeatures.Add(new Bush(new Vector2(104f, 21f), 0, l));
				l.largeTerrainFeatures.Add(new Bush(new Vector2(105f, 18f), 0, l));
				break;
			}
			case SaveGame.SaveFixes.MissingQisChallenge:
			{
				foreach (Farmer farmer in getAllFarmers())
				{
					if (farmer.mailReceived.Contains("skullCave") && !farmer.hasQuest(20) && !farmer.hasOrWillReceiveMail("QiChallengeComplete"))
					{
						farmer.addQuest(20);
					}
				}
				break;
			}
			case SaveGame.SaveFixes.BedsToFurniture:
			{
				List<GameLocation> house_locations = new List<GameLocation>();
				house_locations.Add(getLocationFromName("FarmHouse"));
				foreach (Building b in getFarm().buildings)
				{
					if ((b.indoors.Value != null) & (b.indoors.Value is FarmHouse))
					{
						house_locations.Add(b.indoors);
					}
				}
				{
					foreach (GameLocation item in house_locations)
					{
						if (!(item is FarmHouse house))
						{
							continue;
						}
						for (int x = 0; x < house.map.Layers[0].TileWidth; x++)
						{
							for (int y = 0; y < house.map.Layers[0].TileHeight; y++)
							{
								if (house.doesTileHaveProperty(x, y, "DefaultBedPosition", "Back") == null)
								{
									continue;
								}
								if (house.upgradeLevel == 0)
								{
									house.furniture.Add(new BedFurniture(BedFurniture.DEFAULT_BED_INDEX, new Vector2(x, y)));
									continue;
								}
								int bed_index = BedFurniture.DOUBLE_BED_INDEX;
								if (!house.owner.activeDialogueEvents.ContainsKey("pennyRedecorating"))
								{
									if (house.owner.mailReceived.Contains("pennyQuilt0"))
									{
										bed_index = 2058;
									}
									if (house.owner.mailReceived.Contains("pennyQuilt1"))
									{
										bed_index = 2064;
									}
									if (house.owner.mailReceived.Contains("pennyQuilt2"))
									{
										bed_index = 2070;
									}
								}
								house.furniture.Add(new BedFurniture(bed_index, new Vector2(x, y)));
							}
						}
					}
					break;
				}
			}
			case SaveGame.SaveFixes.ChildBedsToFurniture:
			{
				List<GameLocation> house_locations2 = new List<GameLocation>();
				house_locations2.Add(getLocationFromName("FarmHouse"));
				foreach (Building b2 in getFarm().buildings)
				{
					if ((b2.indoors.Value != null) & (b2.indoors.Value is FarmHouse))
					{
						house_locations2.Add(b2.indoors);
					}
				}
				{
					foreach (GameLocation item2 in house_locations2)
					{
						if (!(item2 is FarmHouse house2))
						{
							continue;
						}
						for (int x2 = 0; x2 < house2.map.Layers[0].TileWidth; x2++)
						{
							for (int y2 = 0; y2 < house2.map.Layers[0].TileHeight; y2++)
							{
								if (house2.doesTileHaveProperty(x2, y2, "DefaultChildBedPosition", "Back") != null)
								{
									house2.furniture.Add(new BedFurniture(BedFurniture.CHILD_BED_INDEX, new Vector2(x2, y2)));
								}
							}
						}
					}
					break;
				}
			}
			case SaveGame.SaveFixes.ModularizeFarmStructures:
				getFarm().AddModularShippingBin();
				break;
			case SaveGame.SaveFixes.FixFlooringFlags:
				Utility.ForAllLocations(delegate(GameLocation location)
				{
					foreach (TerrainFeature current3 in location.terrainFeatures.Values)
					{
						if (current3 is Flooring)
						{
							(current3 as Flooring).ApplyFlooringFlags();
						}
					}
				});
				break;
			case SaveGame.SaveFixes.AddNewRingRecipes1_5:
				if (player.combatLevel.Value >= 7 && !player.craftingRecipes.ContainsKey("Thorns Ring"))
				{
					player.craftingRecipes.Add("Thorns Ring", 0);
				}
				if (player.miningLevel.Value >= 4 && !player.craftingRecipes.ContainsKey("Glowstone Ring"))
				{
					player.craftingRecipes.Add("Glowstone Ring", 0);
				}
				break;
			case SaveGame.SaveFixes.ResetForges:
				Utility.iterateAllItems(delegate(Item item)
				{
					if (item is MeleeWeapon)
					{
						(item as MeleeWeapon).RecalculateAppliedForges();
					}
				});
				break;
			case SaveGame.SaveFixes.AddSquidInkRavioli:
				if (player.combatLevel.Value >= 9 && !player.cookingRecipes.ContainsKey("Squid Ink Ravioli"))
				{
					player.cookingRecipes.Add("Squid Ink Ravioli", 0);
				}
				break;
			case SaveGame.SaveFixes.MakeDarkSwordVampiric:
				Utility.iterateAllItems(delegate(Item item)
				{
					if (item is MeleeWeapon && (item as MeleeWeapon).InitialParentTileIndex == 2)
					{
						(item as MeleeWeapon).AddEnchantment(new VampiricEnchantment());
					}
				});
				break;
			case SaveGame.SaveFixes.FixRingSheetIndex:
				Utility.iterateAllItems(delegate(Item item)
				{
					if (item is Ring && item.ParentSheetIndex == -1)
					{
						item.ParentSheetIndex = (item as Ring).indexInTileSheet.Value;
					}
				});
				break;
			case SaveGame.SaveFixes.FixBeachFarmBushes:
			{
				if (whichFarm != 6)
				{
					break;
				}
				Farm farm = getFarm();
				Vector2[] array = new Vector2[4]
				{
					new Vector2(77f, 4f),
					new Vector2(78f, 3f),
					new Vector2(83f, 4f),
					new Vector2(83f, 3f)
				};
				foreach (Vector2 bush_location in array)
				{
					foreach (LargeTerrainFeature feature in farm.largeTerrainFeatures)
					{
						if (feature.tilePosition.Value == bush_location)
						{
							if (feature is Bush && feature is Bush bush)
							{
								bush.tilePosition.Value = new Vector2(bush.tilePosition.X, bush.tilePosition.Y + 1f);
							}
							break;
						}
					}
				}
				break;
			}
			case SaveGame.SaveFixes.AddCampfireKit:
				if (player.foragingLevel.Value >= 9 && !player.craftingRecipes.ContainsKey("Cookout Kit"))
				{
					player.craftingRecipes.Add("Cookout Kit", 0);
				}
				break;
			case SaveGame.SaveFixes.OstrichIncubatorFragility:
				Utility.iterateAllItems(delegate(Item item)
				{
					if (item is Object && (item as Object).Fragility == 2 && item.Name == "Ostrich Incubator")
					{
						(item as Object).Fragility = 0;
					}
				});
				break;
			case SaveGame.SaveFixes.FixBotchedBundleData:
			{
				Dictionary<string, string> new_dictionary = new Dictionary<string, string>();
				foreach (string key in netWorldState.Value.BundleData.Keys)
				{
					List<string> split = new List<string>(netWorldState.Value.BundleData[key].Split('/'));
					int temp = 0;
					while (split.Count > 4 && !int.TryParse(split[split.Count - 1], out temp))
					{
						string last_value = split[split.Count - 1];
						if (char.IsDigit(last_value[last_value.Length - 1]) && last_value.Contains(":") && last_value.Contains("\\"))
						{
							break;
						}
						split.RemoveAt(split.Count - 1);
					}
					new_dictionary[key] = string.Join("/", split);
				}
				netWorldState.Value.SetBundleData(new_dictionary);
				break;
			}
			case SaveGame.SaveFixes.LeoChildrenFix:
				Utility.FixChildNameCollisions();
				break;
			case SaveGame.SaveFixes.Leo6HeartGermanFix:
				if (Utility.HasAnyPlayerSeenEvent(6497428) && !MasterPlayer.hasOrWillReceiveMail("leoMoved"))
				{
					addMailForTomorrow("leoMoved", noLetter: true, sendToEveryone: true);
					player.team.requestLeoMove.Fire();
				}
				break;
			case SaveGame.SaveFixes.BirdieQuestRemovedFix:
			{
				foreach (Farmer who in getAllFarmers())
				{
					if (who.hasQuest(130))
					{
						foreach (Quest quest in who.questLog)
						{
							if ((int)quest.id == 130)
							{
								quest.canBeCancelled.Value = true;
							}
						}
					}
					if (who.hasOrWillReceiveMail("birdieQuestBegun") && !who.hasOrWillReceiveMail("birdieQuestFinished") && !who.hasQuest(130))
					{
						who.addQuest(130);
					}
				}
				break;
			}
			case SaveGame.SaveFixes.SkippedSummit:
				if (!MasterPlayer.mailReceived.Contains("Farm_Eternal"))
				{
					break;
				}
				{
					foreach (Farmer who2 in getAllFarmers())
					{
						if (who2.mailReceived.Contains("Summit_event") && !who2.songsHeard.Contains("end_credits"))
						{
							who2.mailReceived.Remove("Summit_event");
						}
					}
					break;
				}
			case SaveGame.SaveFixes.Level9PuddingFishingRecipe:
				break;
			}
		}

		public static void recalculateLostBookCount()
		{
			int highestLostBookCount = 0;
			foreach (Farmer who in getAllFarmers())
			{
				if (who.archaeologyFound.ContainsKey(102) && who.archaeologyFound[102][0] > 0)
				{
					highestLostBookCount = Math.Max(highestLostBookCount, who.archaeologyFound[102][0]);
					if (!who.mailForTomorrow.Contains("lostBookFound%&NL&%"))
					{
						who.mailForTomorrow.Add("lostBookFound%&NL&%");
					}
				}
			}
			netWorldState.Value.LostBooksFound.Value = highestLostBookCount;
		}

		public static void MarkFloorChestAsCollectedIfNecessary(int floor_number)
		{
			if (MineShaft.permanentMineChanges != null && MineShaft.permanentMineChanges.ContainsKey(floor_number) && MineShaft.permanentMineChanges[floor_number].chestsLeft <= 0)
			{
				player.chestConsumedMineLevels[floor_number] = true;
			}
		}

		public static void pauseThenDoFunction(int pauseTime, afterFadeFunction function)
		{
			afterPause = function;
			pauseThenDoFunctionTimer = pauseTime;
		}

		public static string dayOrNight()
		{
			string dayOrNight = "_day";
			int dayOfYear = DateTime.Now.DayOfYear;
			int sunset = (int)(1.75 * Math.Sin(Math.PI * 2.0 / 365.0 * (double)dayOfYear - 79.0) + 18.75);
			if (DateTime.Now.TimeOfDay.TotalHours >= (double)sunset || DateTime.Now.TimeOfDay.TotalHours < 5.0)
			{
				dayOrNight = "_night";
			}
			return dayOrNight;
		}

		protected internal virtual LocalizedContentManager CreateContentManager(IServiceProvider serviceProvider, string rootDirectory)
		{
			return new LocalizedContentManager(serviceProvider, rootDirectory);
		}

		public void Instance_LoadContent()
		{
			LoadContent();
		}

		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		protected override void LoadContent()
		{
			content = CreateContentManager(base.Content.ServiceProvider, base.Content.RootDirectory);
			xTileContent = CreateContentManager(content.ServiceProvider, content.RootDirectory);
			mapDisplayDevice = new XnaDisplayDevice(content, base.GraphicsDevice);
			CraftingRecipe.InitShared();
			Critter.InitShared();
			spriteBatch = new SpriteBatch(base.GraphicsDevice);
			concessionsSpriteSheet = content.Load<Texture2D>("LooseSprites\\Concessions");
			birdsSpriteSheet = content.Load<Texture2D>("LooseSprites\\birds");
			daybg = content.Load<Texture2D>("LooseSprites\\daybg");
			nightbg = content.Load<Texture2D>("LooseSprites\\nightbg");
			menuTexture = content.Load<Texture2D>("Maps\\MenuTiles");
			uncoloredMenuTexture = content.Load<Texture2D>("Maps\\MenuTilesUncolored");
			lantern = content.Load<Texture2D>("LooseSprites\\Lighting\\lantern");
			windowLight = content.Load<Texture2D>("LooseSprites\\Lighting\\windowLight");
			sconceLight = content.Load<Texture2D>("LooseSprites\\Lighting\\sconceLight");
			cauldronLight = content.Load<Texture2D>("LooseSprites\\Lighting\\greenLight");
			indoorWindowLight = content.Load<Texture2D>("LooseSprites\\Lighting\\indoorWindowLight");
			shadowTexture = content.Load<Texture2D>("LooseSprites\\shadow");
			mouseCursors = content.Load<Texture2D>("LooseSprites\\Cursors");
			mouseCursors2 = content.Load<Texture2D>("LooseSprites\\Cursors2");
			giftboxTexture = content.Load<Texture2D>("LooseSprites\\Giftbox");
			controllerMaps = content.Load<Texture2D>("LooseSprites\\ControllerMaps");
			animations = content.Load<Texture2D>("TileSheets\\animations");
			achievements = content.Load<Dictionary<int, string>>("Data\\Achievements");
			fadeToBlackRect = new Texture2D(base.GraphicsDevice, 1, 1, mipmap: false, SurfaceFormat.Color);
			Color[] white = new Color[1] { Color.White };
			fadeToBlackRect.SetData(white);
			dialogueWidth = Math.Min(1024, graphics.GraphicsDevice.Viewport.GetTitleSafeArea().Width - 256);
			NameSelect.load();
			NPCGiftTastes = content.Load<Dictionary<string, string>>("Data\\NPCGiftTastes");
			white = new Color[1];
			staminaRect = new Texture2D(base.GraphicsDevice, 1, 1, mipmap: false, SurfaceFormat.Color);
			onScreenMenus.Clear();
			onScreenMenus.Add(new Toolbar());
			for (int k = 0; k < white.Length; k++)
			{
				white[k] = new Color(255, 255, 255, 255);
			}
			staminaRect.SetData(white);
			saveOnNewDay = true;
			littleEffect = new Texture2D(base.GraphicsDevice, 4, 4, mipmap: false, SurfaceFormat.Color);
			white = new Color[16];
			for (int j = 0; j < white.Length; j++)
			{
				white[j] = new Color(255, 255, 255, 255);
			}
			littleEffect.SetData(white);
			for (int i = 0; i < 70; i++)
			{
				rainDrops[i] = new RainDrop(random.Next(viewport.Width), random.Next(viewport.Height), random.Next(4), random.Next(70));
			}
			dayTimeMoneyBox = new DayTimeMoneyBox();
			onScreenMenus.Add(dayTimeMoneyBox);
			buffsDisplay = new BuffsDisplay();
			onScreenMenus.Add(buffsDisplay);
			dialogueFont = content.Load<SpriteFont>("Fonts\\SpriteFont1");
			dialogueFont.LineSpacing = 42;
			smallFont = content.Load<SpriteFont>("Fonts\\SmallFont");
			smallFont.LineSpacing = 26;
			tinyFont = content.Load<SpriteFont>("Fonts\\tinyFont");
			tinyFontBorder = content.Load<SpriteFont>("Fonts\\tinyFontBorder");
			objectSpriteSheet = content.Load<Texture2D>("Maps\\springobjects");
			cropSpriteSheet = content.Load<Texture2D>("TileSheets\\crops");
			emoteSpriteSheet = content.Load<Texture2D>("TileSheets\\emotes");
			debrisSpriteSheet = content.Load<Texture2D>("TileSheets\\debris");
			bigCraftableSpriteSheet = content.Load<Texture2D>("TileSheets\\Craftables");
			rainTexture = content.Load<Texture2D>("TileSheets\\rain");
			buffsIcons = content.Load<Texture2D>("TileSheets\\BuffsIcons");
			objectInformation = content.Load<Dictionary<int, string>>("Data\\ObjectInformation");
			clothingInformation = content.Load<Dictionary<int, string>>("Data\\ClothingInformation");
			objectContextTags = content.Load<Dictionary<string, string>>("Data\\ObjectContextTags");
			bigCraftablesInformation = content.Load<Dictionary<int, string>>("Data\\BigCraftablesInformation");
			if (gameMode == 4)
			{
				fadeToBlackAlpha = -0.5f;
				fadeIn = true;
			}
			if (random.NextDouble() < 0.7)
			{
				isDebrisWeather = true;
				populateDebrisWeatherArray();
			}
			FarmerRenderer.hairStylesTexture = content.Load<Texture2D>("Characters\\Farmer\\hairstyles");
			FarmerRenderer.shirtsTexture = content.Load<Texture2D>("Characters\\Farmer\\shirts");
			FarmerRenderer.pantsTexture = content.Load<Texture2D>("Characters\\Farmer\\pants");
			FarmerRenderer.hatsTexture = content.Load<Texture2D>("Characters\\Farmer\\hats");
			FarmerRenderer.accessoriesTexture = content.Load<Texture2D>("Characters\\Farmer\\accessories");
			Furniture.furnitureTexture = content.Load<Texture2D>("TileSheets\\furniture");
			Furniture.furnitureFrontTexture = content.Load<Texture2D>("TileSheets\\furnitureFront");
			MapSeat.mapChairTexture = content.Load<Texture2D>("TileSheets\\ChairTiles");
			SpriteText.spriteTexture = content.Load<Texture2D>("LooseSprites\\font_bold");
			SpriteText.coloredTexture = content.Load<Texture2D>("LooseSprites\\font_colored");
			Tool.weaponsTexture = content.Load<Texture2D>("TileSheets\\weapons");
			Projectile.projectileSheet = content.Load<Texture2D>("TileSheets\\Projectiles");
			_shortDayDisplayName[0] = content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.3042");
			_shortDayDisplayName[1] = content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.3043");
			_shortDayDisplayName[2] = content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.3044");
			_shortDayDisplayName[3] = content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.3045");
			_shortDayDisplayName[4] = content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.3046");
			_shortDayDisplayName[5] = content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.3047");
			_shortDayDisplayName[6] = content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.3048");
			netWorldState = new NetRoot<IWorldState>(new NetWorldState());
			resetPlayer();
			setGameMode(0);
		}

		public static void resetPlayer()
		{
			List<Item> farmersInitialTools = Farmer.initialTools();
			player = new Farmer(new FarmerSprite(null), new Vector2(192f, 192f), 1, "", farmersInitialTools, isMale: true);
		}

		public static void resetVariables()
		{
			xLocationAfterWarp = 0;
			yLocationAfterWarp = 0;
			gameTimeInterval = 0;
			currentQuestionChoice = 0;
			currentDialogueCharacterIndex = 0;
			dialogueTypingInterval = 0;
			dayOfMonth = 0;
			year = 1;
			timeOfDay = 600;
			timeOfDayAfterFade = -1;
			numberOfSelectedItems = -1;
			priceOfSelectedItem = 0;
			currentWallpaper = 0;
			farmerWallpaper = 22;
			wallpaperPrice = 75;
			currentFloor = 3;
			FarmerFloor = 29;
			floorPrice = 75;
			facingDirectionAfterWarp = 0;
			dialogueWidth = 0;
			menuChoice = 0;
			tvStation = -1;
			currentBillboard = 0;
			facingDirectionAfterWarp = 0;
			tmpTimeOfDay = 0;
			percentageToWinStardewHero = 70;
			mouseClickPolling = 0;
			weatherIcon = 0;
			hitShakeTimer = 0;
			staminaShakeTimer = 0;
			pauseThenDoFunctionTimer = 0;
			weatherForTomorrow = 0;
			currentSongIndex = 3;
		}

		public static void playSound(string cueName)
		{
			if (soundBank != null)
			{
				try
				{
					soundBank.PlayCue(cueName);
				}
				catch (Exception ex)
				{
					debugOutput = parseText(ex.Message);
					Console.WriteLine(ex);
				}
			}
		}

		public static void playSoundPitched(string cueName, int pitch)
		{
			if (soundBank == null)
			{
				return;
			}
			try
			{
				ICue cue = soundBank.GetCue(cueName);
				cue.SetVariable("Pitch", pitch);
				cue.Play();
				try
				{
					if (!cue.IsPitchBeingControlledByRPC)
					{
						cue.Pitch = Utility.Lerp(-1f, 1f, (float)pitch / 2400f);
					}
				}
				catch (Exception)
				{
				}
			}
			catch (Exception ex2)
			{
				debugOutput = parseText(ex2.Message);
				Console.WriteLine(ex2);
			}
		}

		public static void setRichPresence(string friendlyName, object argument = null)
		{
			switch (friendlyName)
			{
			case "menus":
				debugPresenceString = "In menus";
				break;
			case "location":
				debugPresenceString = $"At {argument}";
				break;
			case "festival":
				debugPresenceString = $"At {argument}";
				break;
			case "fishing":
				debugPresenceString = $"Fishing at {argument}";
				break;
			case "minigame":
				debugPresenceString = $"Playing {argument}";
				break;
			case "wedding":
				debugPresenceString = $"Getting married to {argument}";
				break;
			case "earnings":
				debugPresenceString = $"Made {argument}g last night";
				break;
			case "giantcrop":
				debugPresenceString = $"Just harvested a Giant {argument}";
				break;
			}
		}

		public static void GenerateBundles(BundleType bundle_type, bool use_seed = true)
		{
			Random r = null;
			r = ((!use_seed) ? new Random() : new Random((int)uniqueIDForThisGame * 9));
			if (bundle_type == BundleType.Remixed)
			{
				Dictionary<string, string> bundle_data = new BundleGenerator().Generate("Data\\RandomBundles", r);
				netWorldState.Value.SetBundleData(bundle_data);
			}
			else
			{
				netWorldState.Value.SetBundleData(content.LoadBase<Dictionary<string, string>>("Data\\Bundles"));
			}
		}

		public void SetNewGameOption<T>(string key, T val)
		{
			newGameSetupOptions[key] = val;
		}

		public T GetNewGameOption<T>(string key)
		{
			if (!newGameSetupOptions.ContainsKey(key))
			{
				return default(T);
			}
			return (T)newGameSetupOptions[key];
		}

		public static void loadForNewGame(bool loadedGame = false)
		{
			if (startingGameSeed.HasValue)
			{
				uniqueIDForThisGame = startingGameSeed.Value;
			}
			specialCurrencyDisplay = new SpecialCurrencyDisplay();
			flushLocationLookup();
			locations.Clear();
			mailbox.Clear();
			currentLightSources.Clear();
			if (dealerCalicoJackTotal != null)
			{
				dealerCalicoJackTotal.Clear();
			}
			questionChoices.Clear();
			hudMessages.Clear();
			weddingToday = false;
			timeOfDay = 600;
			currentSeason = "spring";
			if (!loadedGame)
			{
				year = 1;
			}
			dayOfMonth = 0;
			pickingTool = false;
			isQuestion = false;
			nonWarpFade = false;
			particleRaining = false;
			newDay = false;
			inMine = false;
			menuUp = false;
			eventUp = false;
			viewportFreeze = false;
			eventOver = false;
			nameSelectUp = false;
			screenGlow = false;
			screenGlowHold = false;
			screenGlowUp = false;
			progressBar = false;
			isRaining = false;
			killScreen = false;
			coopDwellerBorn = false;
			messagePause = false;
			isDebrisWeather = false;
			boardingBus = false;
			listeningForKeyControlDefinitions = false;
			weddingToday = false;
			exitToTitle = false;
			isRaining = false;
			dialogueUp = false;
			currentBillboard = 0;
			postExitToTitleCallback = null;
			displayHUD = true;
			messageAfterPause = "";
			fertilizer = "";
			samBandName = content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.2156");
			slotResult = "";
			background = null;
			currentCursorTile = Vector2.Zero;
			if (!loadedGame)
			{
				lastAppliedSaveFix = SaveGame.SaveFixes.SkippedSummit;
			}
			resetVariables();
			chanceToRainTomorrow = 0.0;
			player.team.sharedDailyLuck.Value = 0.001;
			if (!loadedGame)
			{
				options = new Options();
				options.LoadDefaultOptions();
				initializeVolumeLevels();
			}
			game1.CheckGamepadMode();
			cropsOfTheWeek = Utility.cropsOfTheWeek();
			onScreenMenus.Add(chatBox = new ChatBox());
			outdoorLight = Color.White;
			ambientLight = Color.White;
			int dishOfTheDayIndex = random.Next(194, 240);
			while (Utility.getForbiddenDishesOfTheDay().Contains(dishOfTheDayIndex))
			{
				dishOfTheDayIndex = random.Next(194, 240);
			}
			int dish_of_the_day_count = random.Next(1, 4 + ((random.NextDouble() < 0.08) ? 10 : 0));
			netWorldState.Value.DishOfTheDay.Value = new Object(Vector2.Zero, dishOfTheDayIndex, dish_of_the_day_count);
			locations.Clear();
			locations.Add(new Farm("Maps\\" + Farm.getMapNameFromTypeInt(whichFarm), "Farm"));
			getFarm().BuildStartingCabins();
			forceSnapOnNextViewportUpdate = true;
			currentLocation = new FarmHouse("Maps\\FarmHouse", "FarmHouse");
			currentLocation.map.LoadTileSheets(mapDisplayDevice);
			locations.Add(currentLocation);
			if (whichFarm == 3 || getFarm().ShouldSpawnMountainOres())
			{
				for (int k = 0; k < 28; k++)
				{
					getFarm().doDailyMountainFarmUpdate();
				}
			}
			else if (whichFarm == 5)
			{
				for (int l = 0; l < 10; l++)
				{
					getFarm().doDailyMountainFarmUpdate();
				}
			}
			locations.Add(new FarmCave("Maps\\FarmCave", "FarmCave"));
			locations.Add(new Town("Maps\\Town", "Town"));
			locations.Add(new GameLocation("Maps\\JoshHouse", "JoshHouse"));
			locations[locations.Count - 1].addCharacter(new NPC(new AnimatedSprite("Characters\\George", 0, 16, 32), new Vector2(1024f, 1408f), "JoshHouse", 0, "George", datable: false, null, content.Load<Texture2D>("Portraits\\George")));
			locations[locations.Count - 1].addCharacter(new NPC(new AnimatedSprite("Characters\\Evelyn", 0, 16, 32), new Vector2(128f, 1088f), "JoshHouse", 1, "Evelyn", datable: false, null, content.Load<Texture2D>("Portraits\\Evelyn")));
			locations[locations.Count - 1].addCharacter(new NPC(new AnimatedSprite("Characters\\Alex", 0, 16, 32), new Vector2(1216f, 320f), "JoshHouse", 3, "Alex", datable: true, null, content.Load<Texture2D>("Portraits\\Alex")));
			locations.Add(new GameLocation("Maps\\HaleyHouse", "HaleyHouse"));
			locations[locations.Count - 1].addCharacter(new NPC(new AnimatedSprite("Characters\\Emily", 0, 16, 32), new Vector2(1024f, 320f), "HaleyHouse", 2, "Emily", datable: true, null, content.Load<Texture2D>("Portraits\\Emily")));
			locations[locations.Count - 1].addCharacter(new NPC(new AnimatedSprite("Characters\\Haley", 0, 16, 32), new Vector2(512f, 448f), "HaleyHouse", 1, "Haley", datable: true, null, content.Load<Texture2D>("Portraits\\Haley")));
			locations.Add(new GameLocation("Maps\\SamHouse", "SamHouse"));
			locations[locations.Count - 1].addCharacter(new NPC(new AnimatedSprite("Characters\\Jodi", 0, 16, 32), new Vector2(256f, 320f), "SamHouse", 0, "Jodi", datable: false, null, content.Load<Texture2D>("Portraits\\Jodi")));
			locations[locations.Count - 1].addCharacter(new NPC(new AnimatedSprite("Characters\\Sam", 0, 16, 32), new Vector2(1408f, 832f), "SamHouse", 1, "Sam", datable: true, null, content.Load<Texture2D>("Portraits\\Sam")));
			locations[locations.Count - 1].addCharacter(new NPC(new AnimatedSprite("Characters\\Vincent", 0, 16, 32), new Vector2(640f, 1472f), "SamHouse", 2, "Vincent", datable: false, null, content.Load<Texture2D>("Portraits\\Vincent")));
			addKentIfNecessary();
			locations.Add(new GameLocation("Maps\\Blacksmith", "Blacksmith"));
			locations[locations.Count - 1].addCharacter(new NPC(new AnimatedSprite("Characters\\Clint", 0, 16, 32), new Vector2(192f, 832f), "Blacksmith", 2, "Clint", datable: false, null, content.Load<Texture2D>("Portraits\\Clint")));
			locations.Add(new ManorHouse("Maps\\ManorHouse", "ManorHouse"));
			locations[locations.Count - 1].addCharacter(new NPC(new AnimatedSprite("Characters\\Lewis", 0, 16, 32), new Vector2(512f, 320f), "ManorHouse", 0, "Lewis", datable: false, null, content.Load<Texture2D>("Portraits\\Lewis")));
			locations.Add(new SeedShop("Maps\\SeedShop", "SeedShop"));
			locations[locations.Count - 1].addCharacter(new NPC(new AnimatedSprite("Characters\\Caroline", 0, 16, 32), new Vector2(1408f, 320f), "SeedShop", 2, "Caroline", datable: false, null, content.Load<Texture2D>("Portraits\\Caroline")));
			locations[locations.Count - 1].addCharacter(new NPC(new AnimatedSprite("Characters\\Abigail", 0, 16, 32), new Vector2(64f, 580f), "SeedShop", 3, "Abigail", datable: true, null, content.Load<Texture2D>("Portraits\\Abigail")));
			locations[locations.Count - 1].addCharacter(new NPC(new AnimatedSprite("Characters\\Pierre", 0, 16, 32), new Vector2(256f, 1088f), "SeedShop", 2, "Pierre", datable: false, null, content.Load<Texture2D>("Portraits\\Pierre")));
			locations.Add(new GameLocation("Maps\\Saloon", "Saloon"));
			locations[locations.Count - 1].addCharacter(new NPC(new AnimatedSprite("Characters\\Gus", 0, 16, 32), new Vector2(1152f, 384f), "Saloon", 2, "Gus", datable: false, null, content.Load<Texture2D>("Portraits\\Gus")));
			locations.Add(new GameLocation("Maps\\Trailer", "Trailer"));
			locations[locations.Count - 1].addCharacter(new NPC(new AnimatedSprite("Characters\\Pam", 0, 16, 32), new Vector2(960f, 256f), "Trailer", 2, "Pam", datable: false, null, content.Load<Texture2D>("Portraits\\Pam")));
			locations[locations.Count - 1].addCharacter(new NPC(new AnimatedSprite("Characters\\Penny", 0, 16, 32), new Vector2(256f, 576f), "Trailer", 1, "Penny", datable: true, null, content.Load<Texture2D>("Portraits\\Penny")));
			locations.Add(new GameLocation("Maps\\Hospital", "Hospital"));
			locations.Add(new GameLocation("Maps\\HarveyRoom", "HarveyRoom"));
			locations[locations.Count - 1].addCharacter(new NPC(new AnimatedSprite("Characters\\Harvey", 0, 16, 32), new Vector2(832f, 256f), "HarveyRoom", 1, "Harvey", datable: true, null, content.Load<Texture2D>("Portraits\\Harvey")));
			locations.Add(new Beach("Maps\\Beach", "Beach"));
			locations.Add(new GameLocation("Maps\\ElliottHouse", "ElliottHouse"));
			locations[locations.Count - 1].addCharacter(new NPC(new AnimatedSprite("Characters\\Elliott", 0, 16, 32), new Vector2(64f, 320f), "ElliottHouse", 0, "Elliott", datable: true, null, content.Load<Texture2D>("Portraits\\Elliott")));
			locations.Add(new Mountain("Maps\\Mountain", "Mountain"));
			locations.Add(new GameLocation("Maps\\ScienceHouse", "ScienceHouse"));
			locations[locations.Count - 1].addCharacter(new NPC(new AnimatedSprite("Characters\\Maru", 0, 16, 32), new Vector2(128f, 256f), "ScienceHouse", 3, "Maru", datable: true, null, content.Load<Texture2D>("Portraits\\Maru")));
			locations[locations.Count - 1].addCharacter(new NPC(new AnimatedSprite("Characters\\Robin", 0, 16, 32), new Vector2(1344f, 256f), "ScienceHouse", 1, "Robin", datable: false, null, content.Load<Texture2D>("Portraits\\Robin")));
			locations[locations.Count - 1].addCharacter(new NPC(new AnimatedSprite("Characters\\Demetrius", 0, 16, 32), new Vector2(1216f, 256f), "ScienceHouse", 1, "Demetrius", datable: false, null, content.Load<Texture2D>("Portraits\\Demetrius")));
			locations.Add(new GameLocation("Maps\\SebastianRoom", "SebastianRoom"));
			locations[locations.Count - 1].addCharacter(new NPC(new AnimatedSprite("Characters\\Sebastian", 0, 16, 32), new Vector2(640f, 576f), "SebastianRoom", 1, "Sebastian", datable: true, null, content.Load<Texture2D>("Portraits\\Sebastian")));
			GameLocation tent = new GameLocation("Maps\\Tent", "Tent");
			locations.Add(tent);
			tent.addCharacter(new NPC(new AnimatedSprite("Characters\\Linus", 0, 16, 32), new Vector2(2f, 2f) * 64f, "Tent", 2, "Linus", datable: false, null, content.Load<Texture2D>("Portraits\\Linus")));
			locations.Add(new Forest("Maps\\Forest", "Forest"));
			locations.Add(new WizardHouse("Maps\\WizardHouse", "WizardHouse"));
			locations[locations.Count - 1].addCharacter(new NPC(new AnimatedSprite("Characters\\Wizard", 0, 16, 32), new Vector2(192f, 1088f), "WizardHouse", 2, "Wizard", datable: false, null, content.Load<Texture2D>("Portraits\\Wizard")));
			locations.Add(new GameLocation("Maps\\AnimalShop", "AnimalShop"));
			locations[locations.Count - 1].addCharacter(new NPC(new AnimatedSprite("Characters\\Marnie", 0, 16, 32), new Vector2(768f, 896f), "AnimalShop", 2, "Marnie", datable: false, null, content.Load<Texture2D>("Portraits\\Marnie")));
			locations[locations.Count - 1].addCharacter(new NPC(new AnimatedSprite("Characters\\Shane", 0, 16, 32), new Vector2(1600f, 384f), "AnimalShop", 3, "Shane", datable: true, null, content.Load<Texture2D>("Portraits\\Shane")));
			locations[locations.Count - 1].addCharacter(new NPC(new AnimatedSprite("Characters\\Jas", 0, 16, 32), new Vector2(256f, 384f), "AnimalShop", 2, "Jas", datable: false, null, content.Load<Texture2D>("Portraits\\Jas")));
			locations.Add(new GameLocation("Maps\\LeahHouse", "LeahHouse"));
			locations[locations.Count - 1].addCharacter(new NPC(new AnimatedSprite("Characters\\Leah", 0, 16, 32), new Vector2(192f, 448f), "LeahHouse", 3, "Leah", datable: true, null, content.Load<Texture2D>("Portraits\\Leah")));
			locations.Add(new BusStop("Maps\\BusStop", "BusStop"));
			locations.Add(new Mine("Maps\\Mine", "Mine"));
			locations[locations.Count - 1].objects.Add(new Vector2(27f, 8f), new Object(Vector2.Zero, 78));
			locations[locations.Count - 1].addCharacter(new NPC(new AnimatedSprite("Characters\\Dwarf", 0, 16, 24), new Vector2(2752f, 384f), "Mine", 2, "Dwarf", datable: false, null, content.Load<Texture2D>("Portraits\\Dwarf"))
			{
				Breather = false
			});
			locations.Add(new Sewer("Maps\\Sewer", "Sewer"));
			locations[locations.Count - 1].addCharacter(new NPC(new AnimatedSprite("Characters\\Krobus", 0, 16, 24), new Vector2(31f, 17f) * 64f, "Sewer", 2, "Krobus", datable: false, null, content.Load<Texture2D>("Portraits\\Krobus")));
			locations.Add(new BugLand("Maps\\BugLand", "BugLand"));
			locations.Add(new Desert("Maps\\Desert", "Desert"));
			locations.Add(new Club("Maps\\Club", "Club"));
			locations[locations.Count - 1].addCharacter(new NPC(new AnimatedSprite("Characters\\MrQi", 0, 16, 32), new Vector2(512f, 256f), "Club", 0, "Mister Qi", datable: false, null, content.Load<Texture2D>("Portraits\\MrQi")));
			locations.Add(new GameLocation("Maps\\SandyHouse", "SandyHouse"));
			locations[locations.Count - 1].addCharacter(new NPC(new AnimatedSprite("Characters\\Sandy", 0, 16, 32), new Vector2(128f, 320f), "SandyHouse", 2, "Sandy", datable: false, null, content.Load<Texture2D>("Portraits\\Sandy")));
			locations[locations.Count - 1].addCharacter(new NPC(new AnimatedSprite("Characters\\Bouncer", 0, 16, 32), new Vector2(1088f, 192f), "SandyHouse", 2, "Bouncer", datable: false, null, content.Load<Texture2D>("Portraits\\Bouncer")));
			locations.Add(new LibraryMuseum("Maps\\ArchaeologyHouse", "ArchaeologyHouse"));
			locations[locations.Count - 1].addCharacter(new NPC(new AnimatedSprite("Characters\\Gunther", 0, 16, 32), new Vector2(192f, 512f), "ArchaeologyHouse", 2, "Gunther", datable: false, null, content.Load<Texture2D>("Portraits\\Gunther")));
			locations.Add(new GameLocation("Maps\\WizardHouseBasement", "WizardHouseBasement"));
			locations.Add(new AdventureGuild("Maps\\AdventureGuild", "AdventureGuild"));
			locations[locations.Count - 1].addCharacter(new NPC(new AnimatedSprite("Characters\\Marlon", 0, 16, 32), new Vector2(320f, 704f), "AdventureGuild", 2, "Marlon", datable: false, null, content.Load<Texture2D>("Portraits\\Marlon")));
			locations.Add(new Woods("Maps\\Woods", "Woods"));
			locations.Add(new Railroad("Maps\\Railroad", "Railroad"));
			locations.Add(new GameLocation("Maps\\WitchSwamp", "WitchSwamp"));
			locations[locations.Count - 1].addCharacter(new NPC(new AnimatedSprite("Characters\\Henchman", 0, 16, 32), new Vector2(1280f, 1856f), "WitchSwamp", 2, "Henchman", datable: false, null, content.Load<Texture2D>("Portraits\\Henchman")));
			locations.Add(new GameLocation("Maps\\WitchHut", "WitchHut"));
			locations.Add(new GameLocation("Maps\\WitchWarpCave", "WitchWarpCave"));
			locations.Add(new Summit("Maps\\Summit", "Summit"));
			locations.Add(new FishShop("Maps\\FishShop", "FishShop"));
			locations[locations.Count - 1].addCharacter(new NPC(new AnimatedSprite("Characters\\Willy", 0, 16, 32), new Vector2(320f, 256f), "FishShop", 2, "Willy", datable: false, null, content.Load<Texture2D>("Portraits\\Willy")));
			locations.Add(new GameLocation("Maps\\BathHouse_Entry", "BathHouse_Entry"));
			locations.Add(new GameLocation("Maps\\BathHouse_MensLocker", "BathHouse_MensLocker"));
			locations.Add(new GameLocation("Maps\\BathHouse_WomensLocker", "BathHouse_WomensLocker"));
			locations.Add(new BathHousePool("Maps\\BathHouse_Pool", "BathHouse_Pool"));
			locations.Add(new CommunityCenter("CommunityCenter"));
			locations.Add(new JojaMart("Maps\\JojaMart", "JojaMart"));
			locations.Add(new GameLocation("Maps\\Greenhouse", "Greenhouse"));
			locations.Add(new GameLocation("Maps\\SkullCave", "SkullCave"));
			locations.Add(new GameLocation("Maps\\Backwoods", "Backwoods"));
			locations.Add(new GameLocation("Maps\\Tunnel", "Tunnel"));
			locations.Add(new GameLocation("Maps\\Trailer_big", "Trailer_Big"));
			locations.Add(new Cellar("Maps\\Cellar", "Cellar"));
			for (int j = 1; j < (int)netWorldState.Value.HighestPlayerLimit; j++)
			{
				locations.Add(new Cellar("Maps\\Cellar", "Cellar" + (j + 1)));
			}
			locations.Add(new BeachNightMarket("Maps\\Beach-NightMarket", "BeachNightMarket"));
			locations.Add(new MermaidHouse("Maps\\MermaidHouse", "MermaidHouse"));
			locations.Add(new Submarine("Maps\\Submarine", "Submarine"));
			locations.Add(new AbandonedJojaMart("Maps\\AbandonedJojaMart", "AbandonedJojaMart"));
			locations.Add(new MovieTheater("Maps\\MovieTheater", "MovieTheater"));
			locations.Add(new GameLocation("Maps\\Sunroom", "Sunroom"));
			locations.Add(new BoatTunnel("Maps\\BoatTunnel", "BoatTunnel"));
			locations.Add(new IslandSouth("Maps\\Island_S", "IslandSouth"));
			locations.Add(new IslandSouthEast("Maps\\Island_SE", "IslandSouthEast"));
			locations.Add(new IslandSouthEastCave("Maps\\IslandSouthEastCave", "IslandSouthEastCave"));
			locations.Add(new IslandEast("Maps\\Island_E", "IslandEast"));
			locations.Add(new IslandWest("Maps\\Island_W", "IslandWest"));
			addBirdieIfNecessary();
			locations.Add(new IslandNorth("Maps\\Island_N", "IslandNorth"));
			locations.Add(new IslandHut("Maps\\Island_Hut", "IslandHut"));
			locations.Add(new IslandWestCave1("Maps\\IslandWestCave1", "IslandWestCave1"));
			locations.Add(new IslandLocation("Maps\\IslandNorthCave1", "IslandNorthCave1"));
			locations.Add(new IslandFieldOffice("Maps\\Island_FieldOffice", "IslandFieldOffice"));
			locations.Add(new IslandFarmHouse("Maps\\IslandFarmHouse", "IslandFarmHouse"));
			locations.Add(new IslandLocation("Maps\\Island_CaptainRoom", "CaptainRoom"));
			locations.Add(new IslandShrine("Maps\\Island_Shrine", "IslandShrine"));
			locations.Add(new IslandFarmCave("Maps\\Island_FarmCave", "IslandFarmCave"));
			locations.Add(new Caldera("Maps\\Caldera", "Caldera"));
			locations.Add(new GameLocation("Maps\\LeoTreeHouse", "LeoTreeHouse"));
			locations.Add(new IslandLocation("Maps\\QiNutRoom", "QiNutRoom"));
			locations[locations.Count - 1].addCharacter(new NPC(new AnimatedSprite("Characters\\MrQi", 0, 16, 32), new Vector2(448f, 256f), "QiNutRoom", 0, "Mister Qi", datable: false, null, content.Load<Texture2D>("Portraits\\MrQi")));
			if (!loadedGame)
			{
				foreach (GameLocation location in locations)
				{
					if (location is IslandLocation)
					{
						(location as IslandLocation).AddAdditionalWalnutBushes();
					}
				}
			}
			AddModNPCs();
			NPC.populateRoutesFromLocationToLocationList();
			if (!loadedGame)
			{
				GenerateBundles(bundleType);
				foreach (string value in netWorldState.Value.BundleData.Values)
				{
					string[] item_split = value.Split('/')[2].Split(' ');
					if (!game1.GetNewGameOption<bool>("YearOneCompletable"))
					{
						continue;
					}
					for (int i = 0; i < item_split.Length; i += 3)
					{
						if (item_split[i] == "266")
						{
							int visits = (16 - 2) * 2;
							visits += 3;
							Random r = new Random((int)uniqueIDForThisGame * 12);
							netWorldState.Value.VisitsUntilY1Guarantee = r.Next(2, visits);
						}
					}
				}
				netWorldState.Value.ShuffleMineChests = game1.GetNewGameOption<MineChestType>("MineChests");
				if (game1.newGameSetupOptions.ContainsKey("SpawnMonstersAtNight"))
				{
					spawnMonstersAtNight = game1.GetNewGameOption<bool>("SpawnMonstersAtNight");
				}
			}
			player.ConvertClothingOverrideToClothesItems();
			player.addQuest(9);
			player.currentLocation = getLocationFromName("FarmHouse");
			player.gameVersion = version;
			hudMessages.Clear();
			hasLoadedGame = true;
			setGraphicsForSeason();
			if (!loadedGame)
			{
				_setSaveName = false;
			}
			game1.newGameSetupOptions.Clear();
			updateCellarAssignments();
			if (!loadedGame && netWorldState != null && netWorldState.Value != null)
			{
				netWorldState.Value.RegisterSpecialCurrencies();
			}
		}

		public bool IsFirstInstanceAtThisLocation(GameLocation location, Func<Game1, bool> additional_check = null)
		{
			if (GameRunner.instance.GetFirstInstanceAtThisLocation(location, additional_check) == this)
			{
				return true;
			}
			return false;
		}

		public bool IsLocalCoopJoinable()
		{
			if (GameRunner.instance.gameInstances.Count >= GameRunner.instance.GetMaxSimultaneousPlayers())
			{
				return false;
			}
			if (IsClient)
			{
				return false;
			}
			return true;
		}

		public static void StartLocalMultiplayerIfNecessary()
		{
			if (multiplayerMode == 0)
			{
				Console.WriteLine("Starting multiplayer server for local multiplayer...");
				multiplayerMode = 2;
				if (server == null)
				{
					multiplayer.StartLocalMultiplayerServer();
				}
			}
		}

		public static void EndLocalMultiplayer()
		{
		}

		public static void addParrotBoyIfNecessary()
		{
			if (MasterPlayer.hasOrWillReceiveMail("addedParrotBoy"))
			{
				if (getCharacterFromName("Leo", mustBeVillager: true, useLocationsListOnly: true) == null)
				{
					NPC i = new NPC(new AnimatedSprite("Characters\\ParrotBoy", 0, 16, 32), new Vector2(320f, 384f), "IslandHut", 2, "Leo", datable: false, null, content.Load<Texture2D>("Portraits\\ParrotBoy"));
					i.Breather = false;
					getLocationFromNameInLocationsList("IslandHut").addCharacter(i);
				}
				if (!player.friendshipData.ContainsKey("Leo"))
				{
					player.friendshipData.Add("Leo", new Friendship());
				}
			}
		}

		public static void addBirdieIfNecessary()
		{
			if (getCharacterFromName("Birdie", mustBeVillager: true, useLocationsListOnly: true) == null)
			{
				getLocationFromNameInLocationsList("IslandWest").addCharacter(new NPC(new AnimatedSprite("Characters\\Birdie", 0, 16, 32), new Vector2(1088f, 3712f), "IslandWest", 3, "Birdie", datable: false, null, content.Load<Texture2D>("Portraits\\Birdie")));
			}
		}

		public static void addKentIfNecessary()
		{
			if (year > 1)
			{
				if (getCharacterFromName("Kent", mustBeVillager: true, useLocationsListOnly: true) == null)
				{
					getLocationFromNameInLocationsList("SamHouse").addCharacter(new NPC(new AnimatedSprite("Characters\\Kent", 0, 16, 32), new Vector2(512f, 832f), "SamHouse", 2, "Kent", datable: false, null, content.Load<Texture2D>("Portraits\\Kent")));
				}
				if (!player.friendshipData.ContainsKey("Kent"))
				{
					player.friendshipData.Add("Kent", new Friendship());
				}
			}
		}

		public void Instance_UnloadContent()
		{
			UnloadContent();
		}

		/// <summary>
		/// UnloadContent will be called once per game and is the place to unload
		/// all content.
		/// </summary>
		protected override void UnloadContent()
		{
			base.UnloadContent();
			spriteBatch.Dispose();
			content.Unload();
			xTileContent.Unload();
			if (server != null)
			{
				server.stopServer();
			}
		}

		public void errorUpdateLoop()
		{
			if (GetKeyboardState().IsKeyDown(Keys.B))
			{
				Program.GameTesterMode = false;
				gameMode = 3;
			}
			if (GetKeyboardState().IsKeyDown(Keys.Escape))
			{
				Program.gamePtr.Exit();
				Environment.Exit(1);
			}
			Update(new GameTime());
			BeginDraw();
			Draw(new GameTime());
			EndDraw();
		}

		public static void showRedMessage(string message)
		{
			addHUDMessage(new HUDMessage(message, 3));
			if (!message.Contains("Inventory"))
			{
				playSound("cancel");
			}
			else if (!player.mailReceived.Contains("BackpackTip"))
			{
				player.mailReceived.Add("BackpackTip");
				addMailForTomorrow("pierreBackpack");
			}
		}

		public static void showRedMessageUsingLoadString(string loadString)
		{
			showRedMessage(content.LoadString(loadString));
		}

		public static bool didPlayerJustLeftClick(bool ignoreNonMouseHeldInput = false)
		{
			if (input.GetMouseState().LeftButton == ButtonState.Pressed && oldMouseState.LeftButton != ButtonState.Pressed)
			{
				return true;
			}
			if (input.GetGamePadState().Buttons.X == ButtonState.Pressed && (!ignoreNonMouseHeldInput || !oldPadState.IsButtonDown(Buttons.X)))
			{
				return true;
			}
			if (isOneOfTheseKeysDown(input.GetKeyboardState(), options.useToolButton) && (!ignoreNonMouseHeldInput || areAllOfTheseKeysUp(oldKBState, options.useToolButton)))
			{
				return true;
			}
			return false;
		}

		public static bool didPlayerJustRightClick(bool ignoreNonMouseHeldInput = false)
		{
			if (input.GetMouseState().RightButton == ButtonState.Pressed && oldMouseState.RightButton != ButtonState.Pressed)
			{
				return true;
			}
			if (input.GetGamePadState().Buttons.A == ButtonState.Pressed && (!ignoreNonMouseHeldInput || !oldPadState.IsButtonDown(Buttons.A)))
			{
				return true;
			}
			if (isOneOfTheseKeysDown(input.GetKeyboardState(), options.actionButton) && (!ignoreNonMouseHeldInput || !isOneOfTheseKeysDown(oldKBState, options.actionButton)))
			{
				return true;
			}
			return false;
		}

		public static bool didPlayerJustClickAtAll(bool ignoreNonMouseHeldInput = false)
		{
			if (!didPlayerJustLeftClick(ignoreNonMouseHeldInput))
			{
				return didPlayerJustRightClick(ignoreNonMouseHeldInput);
			}
			return true;
		}

		public static void showGlobalMessage(string message)
		{
			addHUDMessage(new HUDMessage(message, ""));
		}

		public static void globalFadeToBlack(afterFadeFunction afterFade = null, float fadeSpeed = 0.02f)
		{
			screenFade.GlobalFadeToBlack(afterFade, fadeSpeed);
		}

		public static void globalFadeToClear(afterFadeFunction afterFade = null, float fadeSpeed = 0.02f)
		{
			screenFade.GlobalFadeToClear(afterFade, fadeSpeed);
		}

		public void CheckGamepadMode()
		{
			bool old_gamepad_active_state = options.gamepadControls;
			if (options.gamepadMode == Options.GamepadModes.ForceOn)
			{
				options.gamepadControls = true;
				return;
			}
			if (options.gamepadMode == Options.GamepadModes.ForceOff)
			{
				options.gamepadControls = false;
				return;
			}
			MouseState mouseState = input.GetMouseState();
			KeyboardState keyState = GetKeyboardState();
			GamePadState padState = input.GetGamePadState();
			bool non_gamepad_control_was_used = false;
			bool gamepad_control_was_used = false;
			if ((mouseState.LeftButton == ButtonState.Pressed || mouseState.MiddleButton == ButtonState.Pressed || mouseState.RightButton == ButtonState.Pressed || mouseState.ScrollWheelValue != _oldScrollWheelValue || ((mouseState.X != _oldMousePosition.X || mouseState.Y != _oldMousePosition.Y) && lastCursorMotionWasMouse) || keyState.GetPressedKeys().Length != 0) && (keyState.GetPressedKeys().Length != 1 || keyState.GetPressedKeys()[0] != Keys.Pause))
			{
				non_gamepad_control_was_used = true;
				if (Program.sdk is SteamHelper && (Program.sdk as SteamHelper).IsRunningOnSteamDeck())
				{
					non_gamepad_control_was_used = false;
				}
			}
			_oldScrollWheelValue = mouseState.ScrollWheelValue;
			_oldMousePosition.X = mouseState.X;
			_oldMousePosition.Y = mouseState.Y;
			gamepad_control_was_used = isAnyGamePadButtonBeingPressed() || isDPadPressed() || isGamePadThumbstickInMotion() || padState.Triggers.Left != 0f || padState.Triggers.Right != 0f;
			if (_oldGamepadConnectedState != padState.IsConnected)
			{
				_oldGamepadConnectedState = padState.IsConnected;
				if (_oldGamepadConnectedState)
				{
					options.gamepadControls = true;
					showGlobalMessage(content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.2574"));
				}
				else
				{
					options.gamepadControls = false;
					if (instancePlayerOneIndex != (PlayerIndex)(-1))
					{
						showGlobalMessage(content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.2575"));
						if (CanShowPauseMenu() && activeClickableMenu == null)
						{
							activeClickableMenu = new GameMenu();
						}
					}
				}
			}
			if (non_gamepad_control_was_used && options.gamepadControls)
			{
				options.gamepadControls = false;
			}
			if (!options.gamepadControls && gamepad_control_was_used)
			{
				options.gamepadControls = true;
			}
			if (old_gamepad_active_state == options.gamepadControls || !options.gamepadControls)
			{
				return;
			}
			lastMousePositionBeforeFade = new Point(localMultiplayerWindow.Width / 2, localMultiplayerWindow.Height / 2);
			if (activeClickableMenu != null)
			{
				activeClickableMenu.setUpForGamePadMode();
				if (options.SnappyMenus)
				{
					activeClickableMenu.populateClickableComponentList();
					activeClickableMenu.snapToDefaultClickableComponent();
				}
			}
			timerUntilMouseFade = 0;
		}

		public void Instance_Update(GameTime gameTime)
		{
			Update(gameTime);
		}

		protected override void Update(GameTime gameTime)
		{
			GameTime time = gameTime;
			DebugTools.BeforeGameUpdate(this, ref time);
			input.UpdateStates();
			if (input.GetGamePadState().IsButtonDown(Buttons.RightStick))
			{
				rightStickHoldTime += gameTime.ElapsedGameTime.Milliseconds;
			}
			GameMenu.bundleItemHovered = false;
			_update(time);
			if (IsMultiplayer && player != null)
			{
				player.requestingTimePause.Value = !shouldTimePass(LocalMultiplayer.IsLocalMultiplayer(is_local_only: true));
				if (IsMasterGame)
				{
					bool should_time_pause = false;
					if (LocalMultiplayer.IsLocalMultiplayer(is_local_only: true))
					{
						should_time_pause = true;
						foreach (Farmer onlineFarmer in getOnlineFarmers())
						{
							if (!onlineFarmer.requestingTimePause.Value)
							{
								should_time_pause = false;
								break;
							}
						}
					}
					netWorldState.Value.IsTimePaused = should_time_pause;
				}
			}
			Rumble.update(gameTime.ElapsedGameTime.Milliseconds);
			if (options.gamepadControls && thumbstickMotionMargin > 0)
			{
				thumbstickMotionMargin -= gameTime.ElapsedGameTime.Milliseconds;
			}
			if (!input.GetGamePadState().IsButtonDown(Buttons.RightStick))
			{
				rightStickHoldTime = 0;
			}
			base.Update(gameTime);
		}

		public void Instance_OnActivated(object sender, EventArgs args)
		{
			OnActivated(sender, args);
		}

		protected override void OnActivated(object sender, EventArgs args)
		{
			base.OnActivated(sender, args);
			_activatedTick = ticks + 1;
			input.IgnoreKeys(GetKeyboardState().GetPressedKeys());
		}

		public bool HasKeyboardFocus()
		{
			if (keyboardFocusInstance == null)
			{
				return base.IsMainInstance;
			}
			return keyboardFocusInstance == this;
		}

		/// <summary>
		/// Allows the game to run logic such as updating the world,
		/// checking for collisions, gathering input, and playing audio.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		private void _update(GameTime gameTime)
		{
			if (graphics.GraphicsDevice == null)
			{
				return;
			}
			bool zoom_dirty = false;
			if (options != null && !takingMapScreenshot)
			{
				if (options.baseUIScale != options.desiredUIScale)
				{
					if (options.desiredUIScale < 0f)
					{
						options.desiredUIScale = options.desiredBaseZoomLevel;
					}
					options.baseUIScale = options.desiredUIScale;
					zoom_dirty = true;
				}
				if (options.desiredBaseZoomLevel != options.baseZoomLevel)
				{
					options.baseZoomLevel = options.desiredBaseZoomLevel;
					forceSnapOnNextViewportUpdate = true;
					zoom_dirty = true;
				}
			}
			if (zoom_dirty)
			{
				refreshWindowSettings();
			}
			CheckGamepadMode();
			FarmAnimal.NumPathfindingThisTick = 0;
			options.reApplySetOptions();
			if (toggleFullScreen)
			{
				toggleFullscreen();
				toggleFullScreen = false;
			}
			input.Update();
			if (frameByFrame)
			{
				if (GetKeyboardState().IsKeyDown(Keys.Escape) && oldKBState.IsKeyUp(Keys.Escape))
				{
					frameByFrame = false;
				}
				bool advanceFrame = false;
				if (GetKeyboardState().IsKeyDown(Keys.G) && oldKBState.IsKeyUp(Keys.G))
				{
					advanceFrame = true;
				}
				if (!advanceFrame)
				{
					oldKBState = GetKeyboardState();
					return;
				}
			}
			if (client != null && client.timedOut)
			{
				multiplayer.clientRemotelyDisconnected(client.pendingDisconnect);
			}
			if (_newDayTask != null)
			{
				if (_newDayTask.Status == TaskStatus.Created)
				{
					hooks.StartTask(_newDayTask, "NewDay");
				}
				if (_newDayTask.Status >= TaskStatus.RanToCompletion)
				{
					if (_newDayTask.IsFaulted)
					{
						Exception e = _newDayTask.Exception.GetBaseException();
						Console.WriteLine("_newDayTask failed with an exception");
						Console.WriteLine(e);
						throw new Exception("Error on new day: \n---------------\n" + e.Message + "\n" + e.StackTrace + "\n---------------\n");
					}
					_newDayTask = null;
					Utility.CollectGarbage();
				}
				UpdateChatBox();
				return;
			}
			if (isLocalMultiplayerNewDayActive)
			{
				UpdateChatBox();
				return;
			}
			if (IsSaving)
			{
				PushUIMode();
				activeClickableMenu?.update(gameTime);
				if (overlayMenu != null)
				{
					overlayMenu.update(gameTime);
					if (overlayMenu == null)
					{
						PopUIMode();
						return;
					}
				}
				PopUIMode();
				UpdateChatBox();
				return;
			}
			if (exitToTitle)
			{
				exitToTitle = false;
				CleanupReturningToTitle();
				Utility.CollectGarbage();
				if (postExitToTitleCallback != null)
				{
					postExitToTitleCallback();
				}
			}
			SetFreeCursorElapsed((float)gameTime.ElapsedGameTime.TotalSeconds);
			Program.sdk.Update();
			if (game1.IsMainInstance)
			{
				keyboardFocusInstance = game1;
				foreach (Game1 instance in GameRunner.instance.gameInstances)
				{
					if (instance.instanceKeyboardDispatcher.Subscriber != null && instance.instanceTextEntry != null)
					{
						keyboardFocusInstance = instance;
						break;
					}
				}
			}
			if (base.IsMainInstance)
			{
				int current_display_index = base.Window.GetDisplayIndex();
				if (_lastUsedDisplay != -1 && _lastUsedDisplay != current_display_index)
				{
					StartupPreferences startupPreferences = new StartupPreferences();
					startupPreferences.loadPreferences(async: false, applyLanguage: false);
					startupPreferences.displayIndex = current_display_index;
					startupPreferences.savePreferences(async: false);
				}
				_lastUsedDisplay = current_display_index;
			}
			if (HasKeyboardFocus())
			{
				keyboardDispatcher.Poll();
			}
			else
			{
				keyboardDispatcher.Discard();
			}
			if (gameMode == 6)
			{
				multiplayer.UpdateLoading();
			}
			if (gameMode == 3)
			{
				multiplayer.UpdateEarly();
				if (player != null && player.team != null)
				{
					player.team.Update();
				}
			}
			if ((paused || (!IsActiveNoOverlay && Program.releaseBuild)) && (options == null || options.pauseWhenOutOfFocus || paused) && multiplayerMode == 0)
			{
				UpdateChatBox();
				return;
			}
			if (quit)
			{
				Exit();
			}
			currentGameTime = gameTime;
			if (gameMode == 11)
			{
				return;
			}
			ticks++;
			if (IsActiveNoOverlay)
			{
				checkForEscapeKeys();
			}
			updateMusic();
			updateRaindropPosition();
			if (globalFade)
			{
				screenFade.UpdateGlobalFade();
			}
			else if (pauseThenDoFunctionTimer > 0)
			{
				freezeControls = true;
				pauseThenDoFunctionTimer -= gameTime.ElapsedGameTime.Milliseconds;
				if (pauseThenDoFunctionTimer <= 0)
				{
					freezeControls = false;
					if (afterPause != null)
					{
						afterPause();
					}
				}
			}
			bool should_clamp_cursor = false;
			if (options.gamepadControls && activeClickableMenu != null && activeClickableMenu.shouldClampGamePadCursor())
			{
				should_clamp_cursor = true;
			}
			if (should_clamp_cursor)
			{
				Point pos = getMousePositionRaw();
				Microsoft.Xna.Framework.Rectangle rect = new Microsoft.Xna.Framework.Rectangle(0, 0, localMultiplayerWindow.Width, localMultiplayerWindow.Height);
				if (pos.X < rect.X)
				{
					pos.X = rect.X;
				}
				else if (pos.X > rect.Right)
				{
					pos.X = rect.Right;
				}
				if (pos.Y < rect.Y)
				{
					pos.Y = rect.Y;
				}
				else if (pos.Y > rect.Bottom)
				{
					pos.Y = rect.Bottom;
				}
				setMousePositionRaw(pos.X, pos.Y);
			}
			if (gameMode == 3 || gameMode == 2)
			{
				if (!warpingForForcedRemoteEvent && !eventUp && remoteEventQueue.Count > 0 && player != null && player.isCustomized.Value && (!fadeIn || !(fadeToBlackAlpha > 0f)))
				{
					if (activeClickableMenu != null)
					{
						activeClickableMenu.emergencyShutDown();
						exitActiveMenu();
					}
					else if (currentMinigame != null && currentMinigame.forceQuit())
					{
						currentMinigame = null;
					}
					if (activeClickableMenu == null && currentMinigame == null && player.freezePause <= 0)
					{
						Action action = remoteEventQueue[0];
						remoteEventQueue.RemoveAt(0);
						action();
					}
				}
				player.millisecondsPlayed += (uint)gameTime.ElapsedGameTime.Milliseconds;
				bool doMainGameUpdates = true;
				if (currentMinigame != null && !HostPaused)
				{
					if (pauseTime > 0f)
					{
						updatePause(gameTime);
					}
					if (fadeToBlack)
					{
						screenFade.UpdateFadeAlpha(gameTime);
						if (fadeToBlackAlpha >= 1f)
						{
							fadeToBlack = false;
						}
					}
					else
					{
						if (thumbstickMotionMargin > 0)
						{
							thumbstickMotionMargin -= gameTime.ElapsedGameTime.Milliseconds;
						}
						KeyboardState currentKBState = default(KeyboardState);
						MouseState currentMouseState = default(MouseState);
						GamePadState currentPadState = default(GamePadState);
						if (base.IsActive)
						{
							currentKBState = GetKeyboardState();
							currentMouseState = input.GetMouseState();
							currentPadState = input.GetGamePadState();
							bool ignore_controls = false;
							if (chatBox != null && chatBox.isActive())
							{
								ignore_controls = true;
							}
							else if (textEntry != null)
							{
								ignore_controls = true;
							}
							if (ignore_controls)
							{
								currentKBState = default(KeyboardState);
								currentPadState = default(GamePadState);
							}
							else
							{
								Keys[] pressedKeys = currentKBState.GetPressedKeys();
								foreach (Keys i in pressedKeys)
								{
									if (!oldKBState.IsKeyDown(i) && currentMinigame != null)
									{
										currentMinigame.receiveKeyPress(i);
									}
								}
								if (options.gamepadControls)
								{
									if (currentMinigame == null)
									{
										oldMouseState = currentMouseState;
										oldKBState = currentKBState;
										oldPadState = currentPadState;
										UpdateChatBox();
										return;
									}
									ButtonCollection.ButtonEnumerator enumerator2 = Utility.getPressedButtons(currentPadState, oldPadState).GetEnumerator();
									while (enumerator2.MoveNext())
									{
										Buttons b2 = enumerator2.Current;
										if (currentMinigame != null)
										{
											currentMinigame.receiveKeyPress(Utility.mapGamePadButtonToKey(b2));
										}
									}
									if (currentMinigame == null)
									{
										oldMouseState = currentMouseState;
										oldKBState = currentKBState;
										oldPadState = currentPadState;
										UpdateChatBox();
										return;
									}
									if (currentPadState.ThumbSticks.Right.Y < -0.2f && oldPadState.ThumbSticks.Right.Y >= -0.2f)
									{
										currentMinigame.receiveKeyPress(Keys.Down);
									}
									if (currentPadState.ThumbSticks.Right.Y > 0.2f && oldPadState.ThumbSticks.Right.Y <= 0.2f)
									{
										currentMinigame.receiveKeyPress(Keys.Up);
									}
									if (currentPadState.ThumbSticks.Right.X < -0.2f && oldPadState.ThumbSticks.Right.X >= -0.2f)
									{
										currentMinigame.receiveKeyPress(Keys.Left);
									}
									if (currentPadState.ThumbSticks.Right.X > 0.2f && oldPadState.ThumbSticks.Right.X <= 0.2f)
									{
										currentMinigame.receiveKeyPress(Keys.Right);
									}
									if (oldPadState.ThumbSticks.Right.Y < -0.2f && currentPadState.ThumbSticks.Right.Y >= -0.2f)
									{
										currentMinigame.receiveKeyRelease(Keys.Down);
									}
									if (oldPadState.ThumbSticks.Right.Y > 0.2f && currentPadState.ThumbSticks.Right.Y <= 0.2f)
									{
										currentMinigame.receiveKeyRelease(Keys.Up);
									}
									if (oldPadState.ThumbSticks.Right.X < -0.2f && currentPadState.ThumbSticks.Right.X >= -0.2f)
									{
										currentMinigame.receiveKeyRelease(Keys.Left);
									}
									if (oldPadState.ThumbSticks.Right.X > 0.2f && currentPadState.ThumbSticks.Right.X <= 0.2f)
									{
										currentMinigame.receiveKeyRelease(Keys.Right);
									}
									if (isGamePadThumbstickInMotion() && currentMinigame != null && !currentMinigame.overrideFreeMouseMovement())
									{
										setMousePosition(getMouseX() + (int)(currentPadState.ThumbSticks.Left.X * thumbstickToMouseModifier), getMouseY() - (int)(currentPadState.ThumbSticks.Left.Y * thumbstickToMouseModifier));
									}
									else if (getMouseX() != getOldMouseX() || getMouseY() != getOldMouseY())
									{
										lastCursorMotionWasMouse = true;
									}
								}
								pressedKeys = oldKBState.GetPressedKeys();
								foreach (Keys j in pressedKeys)
								{
									if (!currentKBState.IsKeyDown(j) && currentMinigame != null)
									{
										currentMinigame.receiveKeyRelease(j);
									}
								}
								if (options.gamepadControls)
								{
									if (currentMinigame == null)
									{
										oldMouseState = currentMouseState;
										oldKBState = currentKBState;
										oldPadState = currentPadState;
										UpdateChatBox();
										return;
									}
									if (currentPadState.IsConnected && currentPadState.IsButtonDown(Buttons.X) && !oldPadState.IsButtonDown(Buttons.X))
									{
										currentMinigame.receiveRightClick(getMouseX(), getMouseY());
									}
									else if (currentPadState.IsConnected && currentPadState.IsButtonDown(Buttons.A) && !oldPadState.IsButtonDown(Buttons.A))
									{
										currentMinigame.receiveLeftClick(getMouseX(), getMouseY());
									}
									else if (currentPadState.IsConnected && !currentPadState.IsButtonDown(Buttons.X) && oldPadState.IsButtonDown(Buttons.X))
									{
										currentMinigame.releaseRightClick(getMouseX(), getMouseY());
									}
									else if (currentPadState.IsConnected && !currentPadState.IsButtonDown(Buttons.A) && oldPadState.IsButtonDown(Buttons.A))
									{
										currentMinigame.releaseLeftClick(getMouseX(), getMouseY());
									}
									ButtonCollection.ButtonEnumerator enumerator2 = Utility.getPressedButtons(oldPadState, currentPadState).GetEnumerator();
									while (enumerator2.MoveNext())
									{
										Buttons b = enumerator2.Current;
										if (currentMinigame != null)
										{
											currentMinigame.receiveKeyRelease(Utility.mapGamePadButtonToKey(b));
										}
									}
									if (currentPadState.IsConnected && currentPadState.IsButtonDown(Buttons.A) && currentMinigame != null)
									{
										currentMinigame.leftClickHeld(0, 0);
									}
								}
								if (currentMinigame == null)
								{
									oldMouseState = currentMouseState;
									oldKBState = currentKBState;
									oldPadState = currentPadState;
									UpdateChatBox();
									return;
								}
								if (currentMinigame != null && currentMouseState.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton != ButtonState.Pressed)
								{
									currentMinigame.receiveLeftClick(getMouseX(), getMouseY());
								}
								if (currentMinigame != null && currentMouseState.RightButton == ButtonState.Pressed && oldMouseState.RightButton != ButtonState.Pressed)
								{
									currentMinigame.receiveRightClick(getMouseX(), getMouseY());
								}
								if (currentMinigame != null && currentMouseState.LeftButton == ButtonState.Released && oldMouseState.LeftButton == ButtonState.Pressed)
								{
									currentMinigame.releaseLeftClick(getMouseX(), getMouseY());
								}
								if (currentMinigame != null && currentMouseState.RightButton == ButtonState.Released && oldMouseState.RightButton == ButtonState.Pressed)
								{
									currentMinigame.releaseLeftClick(getMouseX(), getMouseY());
								}
								if (currentMinigame != null && currentMouseState.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton == ButtonState.Pressed)
								{
									currentMinigame.leftClickHeld(getMouseX(), getMouseY());
								}
							}
						}
						if (currentMinigame != null && currentMinigame.tick(gameTime))
						{
							oldMouseState = currentMouseState;
							oldKBState = currentKBState;
							oldPadState = currentPadState;
							if (currentMinigame != null)
							{
								currentMinigame.unload();
							}
							currentMinigame = null;
							fadeIn = true;
							fadeToBlackAlpha = 1f;
							UpdateChatBox();
							return;
						}
						if (currentMinigame == null && IsMusicContextActive(MusicContext.MiniGame))
						{
							stopMusicTrack(MusicContext.MiniGame);
						}
						oldMouseState = currentMouseState;
						oldKBState = currentKBState;
						oldPadState = currentPadState;
					}
					doMainGameUpdates = IsMultiplayer || currentMinigame == null || currentMinigame.doMainGameUpdates();
				}
				else if (farmEvent != null && !HostPaused && farmEvent.tickUpdate(gameTime))
				{
					farmEvent.makeChangesToLocation();
					timeOfDay = 600;
					UpdateOther(gameTime);
					displayHUD = true;
					farmEvent = null;
					netWorldState.Value.WriteToGame1();
					currentLocation = player.currentLocation;
					if (currentLocation is FarmHouse farmHouse)
					{
						player.Position = Utility.PointToVector2(farmHouse.GetPlayerBedSpot()) * 64f;
						BedFurniture.ShiftPositionForBed(player);
					}
					else
					{
						BedFurniture.ApplyWakeUpPosition(player);
					}
					changeMusicTrack("none");
					currentLocation.resetForPlayerEntry();
					if (player.IsSitting())
					{
						player.StopSitting(animate: false);
					}
					player.forceCanMove();
					freezeControls = false;
					displayFarmer = true;
					outdoorLight = Color.White;
					viewportFreeze = false;
					fadeToBlackAlpha = 0f;
					fadeToBlack = false;
					globalFadeToClear();
					RemoveDeliveredMailForTomorrow();
					handlePostFarmEventActions();
					showEndOfNightStuff();
				}
				if (doMainGameUpdates)
				{
					if (endOfNightMenus.Count > 0 && activeClickableMenu == null)
					{
						activeClickableMenu = endOfNightMenus.Pop();
						if (activeClickableMenu != null && options.SnappyMenus)
						{
							activeClickableMenu.snapToDefaultClickableComponent();
						}
					}
					if (specialCurrencyDisplay != null)
					{
						specialCurrencyDisplay.Update(gameTime);
					}
					if (currentLocation != null && currentMinigame == null)
					{
						if (emoteMenu != null)
						{
							emoteMenu.update(gameTime);
							if (emoteMenu != null)
							{
								PushUIMode();
								emoteMenu.performHoverAction(getMouseX(), getMouseY());
								KeyboardState currentState = GetKeyboardState();
								if (input.GetMouseState().LeftButton == ButtonState.Pressed && oldMouseState.LeftButton == ButtonState.Released)
								{
									emoteMenu.receiveLeftClick(getMouseX(), getMouseY());
								}
								else if (input.GetMouseState().RightButton == ButtonState.Pressed && oldMouseState.RightButton == ButtonState.Released)
								{
									emoteMenu.receiveRightClick(getMouseX(), getMouseY());
								}
								else if (isOneOfTheseKeysDown(currentState, options.menuButton) || (isOneOfTheseKeysDown(currentState, options.emoteButton) && areAllOfTheseKeysUp(oldKBState, options.emoteButton)))
								{
									emoteMenu.exitThisMenu(playSound: false);
								}
								PopUIMode();
								oldKBState = currentState;
								oldMouseState = input.GetMouseState();
							}
						}
						else if (textEntry != null)
						{
							PushUIMode();
							updateTextEntry(gameTime);
							PopUIMode();
						}
						else if (activeClickableMenu != null)
						{
							PushUIMode();
							updateActiveMenu(gameTime);
							PopUIMode();
						}
						else
						{
							if (pauseTime > 0f)
							{
								updatePause(gameTime);
							}
							if (!globalFade && !freezeControls && activeClickableMenu == null && (IsActiveNoOverlay || inputSimulator != null))
							{
								UpdateControlInput(gameTime);
							}
						}
					}
					if (showingEndOfNightStuff && endOfNightMenus.Count == 0 && activeClickableMenu == null)
					{
						if (newDaySync != null)
						{
							newDaySync = null;
						}
						player.team.endOfNightStatus.WithdrawState();
						showingEndOfNightStuff = false;
						Action afterAction = _afterNewDayAction;
						if (afterAction != null)
						{
							_afterNewDayAction = null;
							afterAction();
						}
						player.ReequipEnchantments();
						globalFadeToClear(doMorningStuff);
					}
					if (currentLocation != null)
					{
						if (!HostPaused && !showingEndOfNightStuff)
						{
							if (IsMultiplayer || (activeClickableMenu == null && currentMinigame == null))
							{
								UpdateGameClock(gameTime);
							}
							UpdateCharacters(gameTime);
							UpdateLocations(gameTime);
							if (currentMinigame == null)
							{
								UpdateViewPort(overrideFreeze: false, getViewportCenter());
							}
							else
							{
								previousViewportPosition.X = viewport.X;
								previousViewportPosition.Y = viewport.Y;
							}
							UpdateOther(gameTime);
						}
						if (messagePause)
						{
							KeyboardState tmp = GetKeyboardState();
							MouseState tmp2 = input.GetMouseState();
							GamePadState tmp3 = input.GetGamePadState();
							if (isOneOfTheseKeysDown(tmp, options.actionButton) && !isOneOfTheseKeysDown(oldKBState, options.actionButton))
							{
								pressActionButton(tmp, tmp2, tmp3);
							}
							oldKBState = tmp;
							oldPadState = tmp3;
						}
					}
				}
				else if (textEntry != null)
				{
					PushUIMode();
					updateTextEntry(gameTime);
					PopUIMode();
				}
			}
			else
			{
				UpdateTitleScreen(gameTime);
				if (textEntry != null)
				{
					PushUIMode();
					updateTextEntry(gameTime);
					PopUIMode();
				}
				else if (activeClickableMenu != null)
				{
					PushUIMode();
					updateActiveMenu(gameTime);
					PopUIMode();
				}
				if (gameMode == 10)
				{
					UpdateOther(gameTime);
				}
			}
			if (audioEngine != null)
			{
				audioEngine.Update();
			}
			UpdateChatBox();
			if (gameMode != 6)
			{
				multiplayer.UpdateLate();
			}
		}

		public static void showTextEntry(TextBox text_box)
		{
			timerUntilMouseFade = 0;
			PushUIMode();
			textEntry = new TextEntryMenu(text_box);
			PopUIMode();
		}

		public static void closeTextEntry()
		{
			if (textEntry != null)
			{
				textEntry = null;
			}
			if (activeClickableMenu != null && options.SnappyMenus)
			{
				if (activeClickableMenu is TitleMenu && TitleMenu.subMenu != null)
				{
					TitleMenu.subMenu.snapCursorToCurrentSnappedComponent();
				}
				else
				{
					activeClickableMenu.snapCursorToCurrentSnappedComponent();
				}
			}
		}

		public static bool isDarkOut()
		{
			return timeOfDay >= getTrulyDarkTime();
		}

		public static bool isStartingToGetDarkOut()
		{
			return timeOfDay >= getStartingToGetDarkTime();
		}

		public static int getStartingToGetDarkTime()
		{
			switch (currentSeason)
			{
			case "spring":
			case "summer":
				return 1800;
			case "fall":
				return 1700;
			case "winter":
				return 1600;
			default:
				return 1800;
			}
		}

		public static void updateCellarAssignments()
		{
			if (!IsMasterGame)
			{
				return;
			}
			player.team.cellarAssignments[1] = MasterPlayer.UniqueMultiplayerID;
			for (int i = 2; i <= netWorldState.Value.HighestPlayerLimit.Value; i++)
			{
				string cellar_name = "Cellar" + i;
				if (i == 1 || getLocationFromName(cellar_name) == null)
				{
					continue;
				}
				if (player.team.cellarAssignments.ContainsKey(i) && getFarmerMaybeOffline(player.team.cellarAssignments[i]) == null)
				{
					player.team.cellarAssignments.Remove(i);
				}
				if (player.team.cellarAssignments.ContainsKey(i))
				{
					continue;
				}
				foreach (Farmer farmer in getAllFarmers())
				{
					if (!player.team.cellarAssignments.Values.Contains(farmer.UniqueMultiplayerID))
					{
						player.team.cellarAssignments[i] = farmer.UniqueMultiplayerID;
						break;
					}
				}
			}
		}

		public static int getModeratelyDarkTime()
		{
			return (getTrulyDarkTime() + getStartingToGetDarkTime()) / 2;
		}

		public static int getTrulyDarkTime()
		{
			return getStartingToGetDarkTime() + 200;
		}

		public static void playMorningSong()
		{
			if (IsRainingHere() || IsLightningHere() || eventUp || dayOfMonth <= 0 || currentLocation.Name.Equals("Desert"))
			{
				return;
			}
			if (currentLocation.GetLocationContext() == GameLocation.LocationContext.Island)
			{
				if (MasterPlayer.hasOrWillReceiveMail("Island_FirstParrot"))
				{
					morningSongPlayAction = DelayedAction.playMusicAfterDelay("IslandMusic", 500);
				}
			}
			else
			{
				morningSongPlayAction = DelayedAction.playMusicAfterDelay(currentSeason + Math.Max(1, currentSongIndex), 500);
			}
		}

		public static void doMorningStuff()
		{
			playMorningSong();
			DelayedAction.functionAfterDelay(delegate
			{
				while (morningQueue.Count > 0)
				{
					morningQueue.Dequeue()();
				}
			}, 1000);
			if (player.hasPendingCompletedQuests)
			{
				dayTimeMoneyBox.PingQuestLog();
			}
		}

		/// <summary>
		/// adds a function that will be called 1 second after fully waking up in the morning. These will not be saved, so only use for "fluff" functions, like sending multiplayer chat messages, etc.
		/// </summary>
		/// <param name="func"></param>
		public static void addMorningFluffFunction(DelayedAction.delayedBehavior func)
		{
			morningQueue.Enqueue(func);
		}

		private Point getViewportCenter()
		{
			if (viewportTarget.X != -2.1474836E+09f)
			{
				if (!(Math.Abs((float)viewportCenter.X - viewportTarget.X) <= viewportSpeed) || !(Math.Abs((float)viewportCenter.Y - viewportTarget.Y) <= viewportSpeed))
				{
					Vector2 velocity = Utility.getVelocityTowardPoint(viewportCenter, viewportTarget, viewportSpeed);
					viewportCenter.X += (int)Math.Round(velocity.X);
					viewportCenter.Y += (int)Math.Round(velocity.Y);
				}
				else
				{
					if (viewportReachedTarget != null)
					{
						viewportReachedTarget();
						viewportReachedTarget = null;
					}
					viewportHold -= currentGameTime.ElapsedGameTime.Milliseconds;
					if (viewportHold <= 0)
					{
						viewportTarget = new Vector2(-2.1474836E+09f, -2.1474836E+09f);
						if (afterViewport != null)
						{
							afterViewport();
						}
					}
				}
				return viewportCenter;
			}
			Farmer farmer = getPlayerOrEventFarmer();
			viewportCenter.X = farmer.getStandingX();
			viewportCenter.Y = farmer.getStandingY();
			return viewportCenter;
		}

		public static void afterFadeReturnViewportToPlayer()
		{
			viewportTarget = new Vector2(-2.1474836E+09f, -2.1474836E+09f);
			viewportHold = 0;
			viewportFreeze = false;
			viewportCenter.X = player.getStandingX();
			viewportCenter.Y = player.getStandingY();
			globalFadeToClear();
		}

		public static bool isViewportOnCustomPath()
		{
			return viewportTarget.X != -2.1474836E+09f;
		}

		public static void moveViewportTo(Vector2 target, float speed, int holdTimer = 0, afterFadeFunction reachedTarget = null, afterFadeFunction endFunction = null)
		{
			viewportTarget = target;
			viewportSpeed = speed;
			viewportHold = holdTimer;
			afterViewport = endFunction;
			viewportReachedTarget = reachedTarget;
		}

		public static Farm getFarm()
		{
			return getLocationFromName("Farm") as Farm;
		}

		public static void setMousePosition(int x, int y, bool ui_scale)
		{
			if (ui_scale)
			{
				setMousePositionRaw((int)((float)x * options.uiScale), (int)((float)y * options.uiScale));
			}
			else
			{
				setMousePositionRaw((int)((float)x * options.zoomLevel), (int)((float)y * options.zoomLevel));
			}
		}

		public static void setMousePosition(int x, int y)
		{
			setMousePosition(x, y, uiMode);
		}

		public static void setMousePosition(Point position, bool ui_scale)
		{
			setMousePosition(position.X, position.Y, ui_scale);
		}

		public static void setMousePosition(Point position)
		{
			setMousePosition(position, uiMode);
		}

		public static void setMousePositionRaw(Point position)
		{
			setMousePositionRaw(position.X, position.Y);
		}

		public static void setMousePositionRaw(int x, int y)
		{
			input.SetMousePosition(x, y);
			InvalidateOldMouseMovement();
			lastCursorMotionWasMouse = false;
		}

		public static Point getMousePositionRaw()
		{
			return new Point(getMouseXRaw(), getMouseYRaw());
		}

		public static Point getMousePosition(bool ui_scale)
		{
			return new Point(getMouseX(ui_scale), getMouseY(ui_scale));
		}

		public static Point getMousePosition()
		{
			return getMousePosition(uiMode);
		}

		private static void ComputeCursorSpeed()
		{
			_cursorSpeedDirty = false;
			GamePadState p = input.GetGamePadState();
			float accellTol = 0.9f;
			bool isAccell = false;
			float num = p.ThumbSticks.Left.Length();
			float rlen = p.ThumbSticks.Right.Length();
			if (num > accellTol || rlen > accellTol)
			{
				isAccell = true;
			}
			float min = 0.7f;
			float max = 2f;
			float rate = 1f;
			if (_cursorDragEnabled)
			{
				min = 0.5f;
				max = 2f;
				rate = 1f;
			}
			if (!isAccell)
			{
				rate = -5f;
			}
			if (_cursorDragPrevEnabled != _cursorDragEnabled)
			{
				_cursorSpeedScale *= 0.5f;
			}
			_cursorDragPrevEnabled = _cursorDragEnabled;
			_cursorSpeedScale += _cursorUpdateElapsedSec * rate;
			_cursorSpeedScale = MathHelper.Clamp(_cursorSpeedScale, min, max);
			float num2 = 16f / (float)game1.TargetElapsedTime.TotalSeconds * _cursorSpeedScale;
			float deltaSpeed = num2 - _cursorSpeed;
			_cursorSpeed = num2;
			_cursorUpdateElapsedSec = 0f;
			if (debugMode)
			{
				Console.WriteLine("_cursorSpeed={0}, _cursorSpeedScale={1}, deltaSpeed={2}", _cursorSpeed.ToString("0.0"), _cursorSpeedScale.ToString("0.0"), deltaSpeed.ToString("0.0"));
			}
		}

		private static void SetFreeCursorElapsed(float elapsedSec)
		{
			if (elapsedSec != _cursorUpdateElapsedSec)
			{
				_cursorUpdateElapsedSec = elapsedSec;
				_cursorSpeedDirty = true;
			}
		}

		public static void ResetFreeCursorDrag()
		{
			if (_cursorDragEnabled)
			{
				_cursorSpeedDirty = true;
			}
			_cursorDragEnabled = false;
		}

		public static void SetFreeCursorDrag()
		{
			if (!_cursorDragEnabled)
			{
				_cursorSpeedDirty = true;
			}
			_cursorDragEnabled = true;
		}

		public static void updateActiveMenu(GameTime gameTime)
		{
			IClickableMenu active_menu = activeClickableMenu;
			while (active_menu.GetChildMenu() != null)
			{
				active_menu = active_menu.GetChildMenu();
			}
			if (!Program.gamePtr.IsActiveNoOverlay && Program.releaseBuild)
			{
				if (active_menu != null && active_menu.IsActive())
				{
					active_menu.update(gameTime);
				}
				return;
			}
			MouseState mouseState = input.GetMouseState();
			KeyboardState keyState = GetKeyboardState();
			GamePadState padState = input.GetGamePadState();
			if (CurrentEvent != null)
			{
				if ((mouseState.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton == ButtonState.Released) || (options.gamepadControls && padState.IsButtonDown(Buttons.A) && oldPadState.IsButtonUp(Buttons.A)))
				{
					CurrentEvent.receiveMouseClick(getMouseX(), getMouseY());
				}
				else if (options.gamepadControls && padState.IsButtonDown(Buttons.Back) && oldPadState.IsButtonUp(Buttons.Back) && !CurrentEvent.skipped && CurrentEvent.skippable)
				{
					CurrentEvent.skipped = true;
					CurrentEvent.skipEvent();
					freezeControls = false;
				}
				if (CurrentEvent != null && CurrentEvent.skipped)
				{
					oldMouseState = input.GetMouseState();
					oldKBState = keyState;
					oldPadState = padState;
					return;
				}
			}
			if (options.gamepadControls && active_menu != null && active_menu.IsActive())
			{
				if (isGamePadThumbstickInMotion() && (!options.snappyMenus || active_menu.overrideSnappyMenuCursorMovementBan()))
				{
					setMousePositionRaw((int)((float)mouseState.X + padState.ThumbSticks.Left.X * thumbstickToMouseModifier), (int)((float)mouseState.Y - padState.ThumbSticks.Left.Y * thumbstickToMouseModifier));
				}
				if (active_menu != null && active_menu.IsActive() && (chatBox == null || !chatBox.isActive()))
				{
					ButtonCollection.ButtonEnumerator enumerator = Utility.getPressedButtons(padState, oldPadState).GetEnumerator();
					while (enumerator.MoveNext())
					{
						Buttons b2 = enumerator.Current;
						active_menu.receiveGamePadButton(b2);
						if (active_menu == null || !active_menu.IsActive())
						{
							break;
						}
					}
					enumerator = Utility.getHeldButtons(padState).GetEnumerator();
					while (enumerator.MoveNext())
					{
						Buttons b3 = enumerator.Current;
						if (active_menu != null && active_menu.IsActive())
						{
							active_menu.gamePadButtonHeld(b3);
						}
						if (active_menu == null || !active_menu.IsActive())
						{
							break;
						}
					}
				}
			}
			if ((getMouseX() != getOldMouseX() || getMouseY() != getOldMouseY()) && !isGamePadThumbstickInMotion() && !isDPadPressed())
			{
				lastCursorMotionWasMouse = true;
			}
			ResetFreeCursorDrag();
			if (active_menu != null && active_menu.IsActive())
			{
				active_menu.performHoverAction(getMouseX(), getMouseY());
			}
			if (active_menu != null && active_menu.IsActive())
			{
				active_menu.update(gameTime);
			}
			if (active_menu != null && active_menu.IsActive() && mouseState.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton == ButtonState.Released)
			{
				if (chatBox != null && chatBox.isActive() && chatBox.isWithinBounds(getMouseX(), getMouseY()))
				{
					chatBox.receiveLeftClick(getMouseX(), getMouseY());
				}
				else
				{
					active_menu.receiveLeftClick(getMouseX(), getMouseY());
				}
			}
			else if (active_menu != null && active_menu.IsActive() && mouseState.RightButton == ButtonState.Pressed && (oldMouseState.RightButton == ButtonState.Released || ((float)mouseClickPolling > 650f && !(active_menu is DialogueBox))))
			{
				active_menu.receiveRightClick(getMouseX(), getMouseY());
				if ((float)mouseClickPolling > 650f)
				{
					mouseClickPolling = 600;
				}
				if ((active_menu == null || !active_menu.IsActive()) && activeClickableMenu == null)
				{
					rightClickPolling = 500;
					mouseClickPolling = 0;
				}
			}
			if (mouseState.ScrollWheelValue != oldMouseState.ScrollWheelValue && active_menu != null && active_menu.IsActive())
			{
				if (chatBox != null && chatBox.choosingEmoji && chatBox.emojiMenu.isWithinBounds(getOldMouseX(), getOldMouseY()))
				{
					chatBox.receiveScrollWheelAction(mouseState.ScrollWheelValue - oldMouseState.ScrollWheelValue);
				}
				else
				{
					active_menu.receiveScrollWheelAction(mouseState.ScrollWheelValue - oldMouseState.ScrollWheelValue);
				}
			}
			if (options.gamepadControls && active_menu != null && active_menu.IsActive())
			{
				thumbstickPollingTimer -= currentGameTime.ElapsedGameTime.Milliseconds;
				if (thumbstickPollingTimer <= 0)
				{
					if (padState.ThumbSticks.Right.Y > 0.2f)
					{
						active_menu.receiveScrollWheelAction(1);
					}
					else if (padState.ThumbSticks.Right.Y < -0.2f)
					{
						active_menu.receiveScrollWheelAction(-1);
					}
				}
				if (thumbstickPollingTimer <= 0)
				{
					thumbstickPollingTimer = 220 - (int)(Math.Abs(padState.ThumbSticks.Right.Y) * 170f);
				}
				if (Math.Abs(padState.ThumbSticks.Right.Y) < 0.2f)
				{
					thumbstickPollingTimer = 0;
				}
			}
			if (active_menu != null && active_menu.IsActive() && mouseState.LeftButton == ButtonState.Released && oldMouseState.LeftButton == ButtonState.Pressed)
			{
				active_menu.releaseLeftClick(getMouseX(), getMouseY());
			}
			else if (active_menu != null && active_menu.IsActive() && mouseState.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton == ButtonState.Pressed)
			{
				active_menu.leftClickHeld(getMouseX(), getMouseY());
			}
			Keys[] pressedKeys = keyState.GetPressedKeys();
			foreach (Keys i in pressedKeys)
			{
				if (active_menu != null && active_menu.IsActive() && !oldKBState.GetPressedKeys().Contains(i))
				{
					active_menu.receiveKeyPress(i);
				}
			}
			if (chatBox == null || !chatBox.isActive())
			{
				if (isOneOfTheseKeysDown(oldKBState, options.moveUpButton) || (options.snappyMenus && options.gamepadControls && (Math.Abs(padState.ThumbSticks.Left.X) < padState.ThumbSticks.Left.Y || padState.IsButtonDown(Buttons.DPadUp))))
				{
					directionKeyPolling[0] -= currentGameTime.ElapsedGameTime.Milliseconds;
				}
				else if (isOneOfTheseKeysDown(oldKBState, options.moveRightButton) || (options.snappyMenus && options.gamepadControls && (padState.ThumbSticks.Left.X > Math.Abs(padState.ThumbSticks.Left.Y) || padState.IsButtonDown(Buttons.DPadRight))))
				{
					directionKeyPolling[1] -= currentGameTime.ElapsedGameTime.Milliseconds;
				}
				else if (isOneOfTheseKeysDown(oldKBState, options.moveDownButton) || (options.snappyMenus && options.gamepadControls && (Math.Abs(padState.ThumbSticks.Left.X) < Math.Abs(padState.ThumbSticks.Left.Y) || padState.IsButtonDown(Buttons.DPadDown))))
				{
					directionKeyPolling[2] -= currentGameTime.ElapsedGameTime.Milliseconds;
				}
				else if (isOneOfTheseKeysDown(oldKBState, options.moveLeftButton) || (options.snappyMenus && options.gamepadControls && (Math.Abs(padState.ThumbSticks.Left.X) > Math.Abs(padState.ThumbSticks.Left.Y) || padState.IsButtonDown(Buttons.DPadLeft))))
				{
					directionKeyPolling[3] -= currentGameTime.ElapsedGameTime.Milliseconds;
				}
				if (areAllOfTheseKeysUp(oldKBState, options.moveUpButton) && (!options.snappyMenus || !options.gamepadControls || ((double)padState.ThumbSticks.Left.Y < 0.1 && padState.IsButtonUp(Buttons.DPadUp))))
				{
					directionKeyPolling[0] = 250;
				}
				if (areAllOfTheseKeysUp(oldKBState, options.moveRightButton) && (!options.snappyMenus || !options.gamepadControls || ((double)padState.ThumbSticks.Left.X < 0.1 && padState.IsButtonUp(Buttons.DPadRight))))
				{
					directionKeyPolling[1] = 250;
				}
				if (areAllOfTheseKeysUp(oldKBState, options.moveDownButton) && (!options.snappyMenus || !options.gamepadControls || ((double)padState.ThumbSticks.Left.Y > -0.1 && padState.IsButtonUp(Buttons.DPadDown))))
				{
					directionKeyPolling[2] = 250;
				}
				if (areAllOfTheseKeysUp(oldKBState, options.moveLeftButton) && (!options.snappyMenus || !options.gamepadControls || ((double)padState.ThumbSticks.Left.X > -0.1 && padState.IsButtonUp(Buttons.DPadLeft))))
				{
					directionKeyPolling[3] = 250;
				}
				if (directionKeyPolling[0] <= 0 && active_menu != null && active_menu.IsActive())
				{
					active_menu.receiveKeyPress(options.getFirstKeyboardKeyFromInputButtonList(options.moveUpButton));
					directionKeyPolling[0] = 70;
				}
				if (directionKeyPolling[1] <= 0 && active_menu != null && active_menu.IsActive())
				{
					active_menu.receiveKeyPress(options.getFirstKeyboardKeyFromInputButtonList(options.moveRightButton));
					directionKeyPolling[1] = 70;
				}
				if (directionKeyPolling[2] <= 0 && active_menu != null && active_menu.IsActive())
				{
					active_menu.receiveKeyPress(options.getFirstKeyboardKeyFromInputButtonList(options.moveDownButton));
					directionKeyPolling[2] = 70;
				}
				if (directionKeyPolling[3] <= 0 && active_menu != null && active_menu.IsActive())
				{
					active_menu.receiveKeyPress(options.getFirstKeyboardKeyFromInputButtonList(options.moveLeftButton));
					directionKeyPolling[3] = 70;
				}
				if (options.gamepadControls && active_menu != null && active_menu.IsActive())
				{
					if (!active_menu.areGamePadControlsImplemented() && padState.IsButtonDown(Buttons.A) && (!oldPadState.IsButtonDown(Buttons.A) || ((float)gamePadAButtonPolling > 650f && !(active_menu is DialogueBox))))
					{
						active_menu.receiveLeftClick(getMousePosition().X, getMousePosition().Y);
						if ((float)gamePadAButtonPolling > 650f)
						{
							gamePadAButtonPolling = 600;
						}
					}
					else if (!active_menu.areGamePadControlsImplemented() && !padState.IsButtonDown(Buttons.A) && oldPadState.IsButtonDown(Buttons.A))
					{
						active_menu.releaseLeftClick(getMousePosition().X, getMousePosition().Y);
					}
					else if (!active_menu.areGamePadControlsImplemented() && padState.IsButtonDown(Buttons.X) && (!oldPadState.IsButtonDown(Buttons.X) || ((float)gamePadXButtonPolling > 650f && !(active_menu is DialogueBox))))
					{
						active_menu.receiveRightClick(getMousePosition().X, getMousePosition().Y);
						if ((float)gamePadXButtonPolling > 650f)
						{
							gamePadXButtonPolling = 600;
						}
					}
					ButtonCollection.ButtonEnumerator enumerator = Utility.getPressedButtons(padState, oldPadState).GetEnumerator();
					while (enumerator.MoveNext())
					{
						Buttons b = enumerator.Current;
						if (active_menu == null || !active_menu.IsActive())
						{
							break;
						}
						Keys key = Utility.mapGamePadButtonToKey(b);
						if (!(active_menu is FarmhandMenu) || game1.IsMainInstance || !options.doesInputListContain(options.menuButton, key))
						{
							active_menu.receiveKeyPress(key);
						}
					}
					if (active_menu != null && active_menu.IsActive() && !active_menu.areGamePadControlsImplemented() && padState.IsButtonDown(Buttons.A) && oldPadState.IsButtonDown(Buttons.A))
					{
						active_menu.leftClickHeld(getMousePosition().X, getMousePosition().Y);
					}
					if (padState.IsButtonDown(Buttons.X))
					{
						gamePadXButtonPolling += gameTime.ElapsedGameTime.Milliseconds;
					}
					else
					{
						gamePadXButtonPolling = 0;
					}
					if (padState.IsButtonDown(Buttons.A))
					{
						gamePadAButtonPolling += gameTime.ElapsedGameTime.Milliseconds;
					}
					else
					{
						gamePadAButtonPolling = 0;
					}
					if (!active_menu.IsActive() && activeClickableMenu == null)
					{
						rightClickPolling = 500;
						gamePadXButtonPolling = 0;
						gamePadAButtonPolling = 0;
					}
				}
			}
			else
			{
				_ = options.SnappyMenus;
			}
			if (mouseState.RightButton == ButtonState.Pressed)
			{
				mouseClickPolling += gameTime.ElapsedGameTime.Milliseconds;
			}
			else
			{
				mouseClickPolling = 0;
			}
			oldMouseState = input.GetMouseState();
			oldKBState = keyState;
			oldPadState = padState;
		}

		public static void AdjustScreenScale(float offset)
		{
		}

		public void ShowScreenScaleMenu()
		{
			if (activeClickableMenu != null && !(activeClickableMenu is ScreenSizeAdjustMenu))
			{
				activeClickableMenu.SetChildMenu(new ScreenSizeAdjustMenu());
			}
			else if (activeClickableMenu == null)
			{
				activeClickableMenu = new ScreenSizeAdjustMenu();
			}
		}

		public bool ShowLocalCoopJoinMenu()
		{
			if (!base.IsMainInstance)
			{
				return false;
			}
			if (gameMode != 3)
			{
				return false;
			}
			int free_farmhands = 0;
			foreach (Building building in getFarm().buildings)
			{
				if (building.indoors.Value is Cabin)
				{
					Farmer farmhand = (building.indoors.Value as Cabin).farmhand.Value;
					if (farmhand == null)
					{
						free_farmhands++;
					}
					else if (!farmhand.isActive())
					{
						free_farmhands++;
					}
				}
			}
			if (free_farmhands == 0)
			{
				showRedMessage(content.LoadString("Strings\\UI:CoopMenu_NoSlots"));
				return false;
			}
			if (currentMinigame != null)
			{
				return false;
			}
			if (activeClickableMenu != null)
			{
				return false;
			}
			if (!IsLocalCoopJoinable())
			{
				return false;
			}
			playSound("bigSelect");
			activeClickableMenu = new LocalCoopJoinMenu();
			return true;
		}

		public static void updateTextEntry(GameTime gameTime)
		{
			MouseState mouseState = input.GetMouseState();
			KeyboardState keyState = GetKeyboardState();
			GamePadState padState = input.GetGamePadState();
			if (options.gamepadControls && textEntry != null && textEntry != null)
			{
				ButtonCollection.ButtonEnumerator enumerator = Utility.getPressedButtons(padState, oldPadState).GetEnumerator();
				while (enumerator.MoveNext())
				{
					Buttons b2 = enumerator.Current;
					textEntry.receiveGamePadButton(b2);
					if (textEntry == null)
					{
						break;
					}
				}
				enumerator = Utility.getHeldButtons(padState).GetEnumerator();
				while (enumerator.MoveNext())
				{
					Buttons b3 = enumerator.Current;
					if (textEntry != null)
					{
						textEntry.gamePadButtonHeld(b3);
					}
					if (textEntry == null)
					{
						break;
					}
				}
			}
			if (textEntry != null)
			{
				textEntry.performHoverAction(getMouseX(), getMouseY());
			}
			if (textEntry != null)
			{
				textEntry.update(gameTime);
			}
			if (textEntry != null && mouseState.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton == ButtonState.Released)
			{
				textEntry.receiveLeftClick(getMouseX(), getMouseY());
			}
			else if (textEntry != null && mouseState.RightButton == ButtonState.Pressed && (oldMouseState.RightButton == ButtonState.Released || (float)mouseClickPolling > 650f))
			{
				textEntry.receiveRightClick(getMouseX(), getMouseY());
				if ((float)mouseClickPolling > 650f)
				{
					mouseClickPolling = 600;
				}
				if (textEntry == null)
				{
					rightClickPolling = 500;
					mouseClickPolling = 0;
				}
			}
			if (mouseState.ScrollWheelValue != oldMouseState.ScrollWheelValue && textEntry != null)
			{
				if (chatBox != null && chatBox.choosingEmoji && chatBox.emojiMenu.isWithinBounds(getOldMouseX(), getOldMouseY()))
				{
					chatBox.receiveScrollWheelAction(mouseState.ScrollWheelValue - oldMouseState.ScrollWheelValue);
				}
				else
				{
					textEntry.receiveScrollWheelAction(mouseState.ScrollWheelValue - oldMouseState.ScrollWheelValue);
				}
			}
			if (options.gamepadControls && textEntry != null)
			{
				thumbstickPollingTimer -= currentGameTime.ElapsedGameTime.Milliseconds;
				if (thumbstickPollingTimer <= 0)
				{
					if (padState.ThumbSticks.Right.Y > 0.2f)
					{
						textEntry.receiveScrollWheelAction(1);
					}
					else if (padState.ThumbSticks.Right.Y < -0.2f)
					{
						textEntry.receiveScrollWheelAction(-1);
					}
				}
				if (thumbstickPollingTimer <= 0)
				{
					thumbstickPollingTimer = 220 - (int)(Math.Abs(padState.ThumbSticks.Right.Y) * 170f);
				}
				if (Math.Abs(padState.ThumbSticks.Right.Y) < 0.2f)
				{
					thumbstickPollingTimer = 0;
				}
			}
			if (textEntry != null && mouseState.LeftButton == ButtonState.Released && oldMouseState.LeftButton == ButtonState.Pressed)
			{
				textEntry.releaseLeftClick(getMouseX(), getMouseY());
			}
			else if (textEntry != null && mouseState.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton == ButtonState.Pressed)
			{
				textEntry.leftClickHeld(getMouseX(), getMouseY());
			}
			Keys[] pressedKeys = keyState.GetPressedKeys();
			foreach (Keys i in pressedKeys)
			{
				if (textEntry != null && !oldKBState.GetPressedKeys().Contains(i))
				{
					textEntry.receiveKeyPress(i);
				}
			}
			if (isOneOfTheseKeysDown(oldKBState, options.moveUpButton) || (options.snappyMenus && options.gamepadControls && (Math.Abs(padState.ThumbSticks.Left.X) < padState.ThumbSticks.Left.Y || padState.IsButtonDown(Buttons.DPadUp))))
			{
				directionKeyPolling[0] -= currentGameTime.ElapsedGameTime.Milliseconds;
			}
			else if (isOneOfTheseKeysDown(oldKBState, options.moveRightButton) || (options.snappyMenus && options.gamepadControls && (padState.ThumbSticks.Left.X > Math.Abs(padState.ThumbSticks.Left.Y) || padState.IsButtonDown(Buttons.DPadRight))))
			{
				directionKeyPolling[1] -= currentGameTime.ElapsedGameTime.Milliseconds;
			}
			else if (isOneOfTheseKeysDown(oldKBState, options.moveDownButton) || (options.snappyMenus && options.gamepadControls && (Math.Abs(padState.ThumbSticks.Left.X) < Math.Abs(padState.ThumbSticks.Left.Y) || padState.IsButtonDown(Buttons.DPadDown))))
			{
				directionKeyPolling[2] -= currentGameTime.ElapsedGameTime.Milliseconds;
			}
			else if (isOneOfTheseKeysDown(oldKBState, options.moveLeftButton) || (options.snappyMenus && options.gamepadControls && (Math.Abs(padState.ThumbSticks.Left.X) > Math.Abs(padState.ThumbSticks.Left.Y) || padState.IsButtonDown(Buttons.DPadLeft))))
			{
				directionKeyPolling[3] -= currentGameTime.ElapsedGameTime.Milliseconds;
			}
			if (areAllOfTheseKeysUp(oldKBState, options.moveUpButton) && (!options.snappyMenus || !options.gamepadControls || ((double)padState.ThumbSticks.Left.Y < 0.1 && padState.IsButtonUp(Buttons.DPadUp))))
			{
				directionKeyPolling[0] = 250;
			}
			if (areAllOfTheseKeysUp(oldKBState, options.moveRightButton) && (!options.snappyMenus || !options.gamepadControls || ((double)padState.ThumbSticks.Left.X < 0.1 && padState.IsButtonUp(Buttons.DPadRight))))
			{
				directionKeyPolling[1] = 250;
			}
			if (areAllOfTheseKeysUp(oldKBState, options.moveDownButton) && (!options.snappyMenus || !options.gamepadControls || ((double)padState.ThumbSticks.Left.Y > -0.1 && padState.IsButtonUp(Buttons.DPadDown))))
			{
				directionKeyPolling[2] = 250;
			}
			if (areAllOfTheseKeysUp(oldKBState, options.moveLeftButton) && (!options.snappyMenus || !options.gamepadControls || ((double)padState.ThumbSticks.Left.X > -0.1 && padState.IsButtonUp(Buttons.DPadLeft))))
			{
				directionKeyPolling[3] = 250;
			}
			if (directionKeyPolling[0] <= 0 && textEntry != null)
			{
				textEntry.receiveKeyPress(options.getFirstKeyboardKeyFromInputButtonList(options.moveUpButton));
				directionKeyPolling[0] = 70;
			}
			if (directionKeyPolling[1] <= 0 && textEntry != null)
			{
				textEntry.receiveKeyPress(options.getFirstKeyboardKeyFromInputButtonList(options.moveRightButton));
				directionKeyPolling[1] = 70;
			}
			if (directionKeyPolling[2] <= 0 && textEntry != null)
			{
				textEntry.receiveKeyPress(options.getFirstKeyboardKeyFromInputButtonList(options.moveDownButton));
				directionKeyPolling[2] = 70;
			}
			if (directionKeyPolling[3] <= 0 && textEntry != null)
			{
				textEntry.receiveKeyPress(options.getFirstKeyboardKeyFromInputButtonList(options.moveLeftButton));
				directionKeyPolling[3] = 70;
			}
			if (options.gamepadControls && textEntry != null)
			{
				if (!textEntry.areGamePadControlsImplemented() && padState.IsButtonDown(Buttons.A) && (!oldPadState.IsButtonDown(Buttons.A) || (float)gamePadAButtonPolling > 650f))
				{
					textEntry.receiveLeftClick(getMousePosition().X, getMousePosition().Y);
					if ((float)gamePadAButtonPolling > 650f)
					{
						gamePadAButtonPolling = 600;
					}
				}
				else if (!textEntry.areGamePadControlsImplemented() && !padState.IsButtonDown(Buttons.A) && oldPadState.IsButtonDown(Buttons.A))
				{
					textEntry.releaseLeftClick(getMousePosition().X, getMousePosition().Y);
				}
				else if (!textEntry.areGamePadControlsImplemented() && padState.IsButtonDown(Buttons.X) && (!oldPadState.IsButtonDown(Buttons.X) || (float)gamePadXButtonPolling > 650f))
				{
					textEntry.receiveRightClick(getMousePosition().X, getMousePosition().Y);
					if ((float)gamePadXButtonPolling > 650f)
					{
						gamePadXButtonPolling = 600;
					}
				}
				ButtonCollection.ButtonEnumerator enumerator = Utility.getPressedButtons(padState, oldPadState).GetEnumerator();
				while (enumerator.MoveNext())
				{
					Buttons b = enumerator.Current;
					if (textEntry == null)
					{
						break;
					}
					textEntry.receiveKeyPress(Utility.mapGamePadButtonToKey(b));
				}
				if (textEntry != null && !textEntry.areGamePadControlsImplemented() && padState.IsButtonDown(Buttons.A) && oldPadState.IsButtonDown(Buttons.A))
				{
					textEntry.leftClickHeld(getMousePosition().X, getMousePosition().Y);
				}
				if (padState.IsButtonDown(Buttons.X))
				{
					gamePadXButtonPolling += gameTime.ElapsedGameTime.Milliseconds;
				}
				else
				{
					gamePadXButtonPolling = 0;
				}
				if (padState.IsButtonDown(Buttons.A))
				{
					gamePadAButtonPolling += gameTime.ElapsedGameTime.Milliseconds;
				}
				else
				{
					gamePadAButtonPolling = 0;
				}
				if (textEntry == null)
				{
					rightClickPolling = 500;
					gamePadAButtonPolling = 0;
					gamePadXButtonPolling = 0;
				}
			}
			if (mouseState.RightButton == ButtonState.Pressed)
			{
				mouseClickPolling += gameTime.ElapsedGameTime.Milliseconds;
			}
			else
			{
				mouseClickPolling = 0;
			}
			oldMouseState = input.GetMouseState();
			oldKBState = keyState;
			oldPadState = padState;
		}

		public static string DateCompiled()
		{
			Version version = Assembly.GetExecutingAssembly().GetName().Version;
			return version.Major + "." + version.Minor + "." + version.Build + "." + version.Revision;
		}

		public static void updatePause(GameTime gameTime)
		{
			pauseTime -= gameTime.ElapsedGameTime.Milliseconds;
			if (player.isCrafting && random.NextDouble() < 0.007)
			{
				playSound("crafting");
			}
			if (!(pauseTime <= 0f))
			{
				return;
			}
			if (currentObjectDialogue.Count == 0)
			{
				messagePause = false;
			}
			pauseTime = 0f;
			if (messageAfterPause != null && !messageAfterPause.Equals(""))
			{
				player.isCrafting = false;
				drawObjectDialogue(messageAfterPause);
				messageAfterPause = "";
				if (player.ActiveObject != null)
				{
					_ = (bool)player.ActiveObject.bigCraftable;
				}
				if (killScreen)
				{
					killScreen = false;
					player.health = 10;
				}
			}
			else if (killScreen)
			{
				multiplayer.globalChatInfoMessage("PlayerDeath", player.Name);
				screenGlow = false;
				if (currentLocation.Name.StartsWith("UndergroundMine") && mine.getMineArea() != 121)
				{
					warpFarmer("Mine", 22, 9, flip: false);
				}
				else if (currentLocation is IslandLocation)
				{
					warpFarmer("IslandSouth", 13, 33, flip: false);
				}
				else
				{
					warpFarmer("Hospital", 20, 12, flip: false);
				}
			}
			progressBar = false;
			if (currentLocation.currentEvent != null)
			{
				currentLocation.currentEvent.CurrentCommand++;
			}
		}

		public static void CheckValidFullscreenResolution(ref int width, ref int height)
		{
			int preferredW = width;
			int preferredH = height;
			foreach (DisplayMode v3 in graphics.GraphicsDevice.Adapter.SupportedDisplayModes)
			{
				if (v3.Width >= 1280 && v3.Width == preferredW && v3.Height == preferredH)
				{
					width = preferredW;
					height = preferredH;
					return;
				}
			}
			foreach (DisplayMode v2 in graphics.GraphicsDevice.Adapter.SupportedDisplayModes)
			{
				if (v2.Width >= 1280 && v2.Width == graphics.GraphicsDevice.Adapter.CurrentDisplayMode.Width && v2.Height == graphics.GraphicsDevice.Adapter.CurrentDisplayMode.Height)
				{
					width = graphics.GraphicsDevice.Adapter.CurrentDisplayMode.Width;
					height = graphics.GraphicsDevice.Adapter.CurrentDisplayMode.Height;
					return;
				}
			}
			bool found_resolution = false;
			foreach (DisplayMode v in graphics.GraphicsDevice.Adapter.SupportedDisplayModes)
			{
				if (v.Width >= 1280 && preferredW > v.Width)
				{
					width = v.Width;
					height = v.Height;
					found_resolution = true;
				}
			}
			if (!found_resolution)
			{
				Console.WriteLine("Requested fullscreen resolution not valid, switching to windowed.");
				width = 1280;
				height = 720;
				options.fullscreen = false;
			}
		}

		public static void toggleNonBorderlessWindowedFullscreen()
		{
			int width = options.preferredResolutionX;
			int height = options.preferredResolutionY;
			graphics.HardwareModeSwitch = options.fullscreen && !options.windowedBorderlessFullscreen;
			if (options.fullscreen && !options.windowedBorderlessFullscreen)
			{
				CheckValidFullscreenResolution(ref width, ref height);
			}
			if (!options.fullscreen && !options.windowedBorderlessFullscreen)
			{
				width = 1280;
				height = 720;
			}
			graphics.PreferredBackBufferWidth = width;
			graphics.PreferredBackBufferHeight = height;
			if (options.fullscreen != graphics.IsFullScreen)
			{
				graphics.ToggleFullScreen();
			}
			graphics.ApplyChanges();
			updateViewportForScreenSizeChange(fullscreenChange: true, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
			GameRunner.instance.OnWindowSizeChange(null, null);
		}

		public static void toggleFullscreen()
		{
			if (options.windowedBorderlessFullscreen)
			{
				graphics.HardwareModeSwitch = false;
				graphics.IsFullScreen = true;
				graphics.ApplyChanges();
				graphics.PreferredBackBufferWidth = Program.gamePtr.Window.ClientBounds.Width;
				graphics.PreferredBackBufferHeight = Program.gamePtr.Window.ClientBounds.Height;
			}
			else
			{
				toggleNonBorderlessWindowedFullscreen();
			}
			GameRunner.instance.OnWindowSizeChange(null, null);
		}

		private void checkForEscapeKeys()
		{
			KeyboardState kbState = input.GetKeyboardState();
			if (!base.IsMainInstance)
			{
				return;
			}
			if (kbState.IsKeyDown(Keys.LeftAlt) && kbState.IsKeyDown(Keys.Enter) && (oldKBState.IsKeyUp(Keys.LeftAlt) || oldKBState.IsKeyUp(Keys.Enter)))
			{
				if (options.isCurrentlyFullscreen() || options.isCurrentlyWindowedBorderless())
				{
					options.setWindowedOption(1);
				}
				else
				{
					options.setWindowedOption(0);
				}
			}
			if ((player.UsingTool || freezeControls) && kbState.IsKeyDown(Keys.RightShift) && kbState.IsKeyDown(Keys.R) && kbState.IsKeyDown(Keys.Delete))
			{
				freezeControls = false;
				player.forceCanMove();
				player.completelyStopAnimatingOrDoingAction();
				player.UsingTool = false;
			}
		}

		public static bool IsPressEvent(ref KeyboardState state, Keys key)
		{
			if (state.IsKeyDown(key) && !oldKBState.IsKeyDown(key))
			{
				oldKBState = state;
				return true;
			}
			return false;
		}

		public static bool IsPressEvent(ref GamePadState state, Buttons btn)
		{
			if (state.IsConnected && state.IsButtonDown(btn) && !oldPadState.IsButtonDown(btn))
			{
				oldPadState = state;
				return true;
			}
			return false;
		}

		public static bool isOneOfTheseKeysDown(KeyboardState state, InputButton[] keys)
		{
			for (int j = 0; j < keys.Length; j++)
			{
				InputButton i = keys[j];
				if (i.key != 0 && state.IsKeyDown(i.key))
				{
					return true;
				}
			}
			return false;
		}

		public static bool areAllOfTheseKeysUp(KeyboardState state, InputButton[] keys)
		{
			for (int j = 0; j < keys.Length; j++)
			{
				InputButton i = keys[j];
				if (i.key != 0 && !state.IsKeyUp(i.key))
				{
					return false;
				}
			}
			return true;
		}

		private void UpdateTitleScreen(GameTime time)
		{
			if (quit)
			{
				Exit();
				changeMusicTrack("none");
			}
			if (gameMode == 6)
			{
				_requestedMusicTracks = new Dictionary<MusicContext, KeyValuePair<string, bool>>();
				requestedMusicTrack = "none";
				requestedMusicTrackOverrideable = false;
				requestedMusicDirty = true;
				if (currentLoader != null && !currentLoader.MoveNext())
				{
					if (gameMode == 3)
					{
						setGameMode(3);
						fadeIn = true;
						fadeToBlackAlpha = 0.99f;
					}
					else
					{
						ExitToTitle();
					}
				}
				return;
			}
			if (gameMode == 7)
			{
				currentLoader.MoveNext();
				return;
			}
			if (gameMode == 8)
			{
				pauseAccumulator -= time.ElapsedGameTime.Milliseconds;
				if (pauseAccumulator <= 0f)
				{
					pauseAccumulator = 0f;
					setGameMode(3);
					if (currentObjectDialogue.Count > 0)
					{
						messagePause = true;
						pauseTime = 1E+10f;
						fadeToBlackAlpha = 1f;
						player.CanMove = false;
					}
				}
				return;
			}
			if (game1.instanceIndex > 0)
			{
				if (activeClickableMenu == null && ticks > 1)
				{
					activeClickableMenu = new FarmhandMenu(multiplayer.InitClient(new LidgrenClient("localhost")));
					activeClickableMenu.populateClickableComponentList();
					if (options.SnappyMenus)
					{
						activeClickableMenu.snapToDefaultClickableComponent();
					}
				}
				return;
			}
			if (fadeToBlackAlpha < 1f && fadeIn)
			{
				fadeToBlackAlpha += 0.02f;
			}
			else if (fadeToBlackAlpha > 0f && fadeToBlack)
			{
				fadeToBlackAlpha -= 0.02f;
			}
			if (pauseTime > 0f)
			{
				pauseTime = Math.Max(0f, pauseTime - (float)time.ElapsedGameTime.Milliseconds);
			}
			if (gameMode == 0 && (double)fadeToBlackAlpha >= 0.98)
			{
				_ = fadeToBlackAlpha;
				_ = 1f;
			}
			if (fadeToBlackAlpha >= 1f)
			{
				if (gameMode == 4 && !fadeToBlack)
				{
					fadeIn = false;
					fadeToBlack = true;
					fadeToBlackAlpha = 2.5f;
				}
				else if (gameMode == 0 && currentSong == null && soundBank != null && pauseTime <= 0f && base.IsMainInstance && soundBank != null)
				{
					currentSong = soundBank.GetCue("spring_day_ambient");
					currentSong.Play();
				}
				if (gameMode == 0 && activeClickableMenu == null && !quit)
				{
					activeClickableMenu = new TitleMenu();
				}
			}
			else if (fadeToBlackAlpha <= 0f)
			{
				if (gameMode == 4 && fadeToBlack)
				{
					fadeIn = true;
					fadeToBlack = false;
					setGameMode(0);
					pauseTime = 2000f;
				}
				else if (gameMode == 0 && fadeToBlack && menuChoice == 0)
				{
					currentLoader = Utility.generateNewFarm(IsClient);
					setGameMode(6);
					loadingMessage = (IsClient ? content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.2574", client.serverName) : content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.2575"));
					exitActiveMenu();
				}
			}
		}

		private void UpdateLocations(GameTime time)
		{
			locationCues.Update(currentLocation);
			if (menuUp && !IsMultiplayer)
			{
				return;
			}
			if (IsClient)
			{
				currentLocation.UpdateWhenCurrentLocation(time);
				{
					foreach (GameLocation location2 in multiplayer.activeLocations())
					{
						location2.updateEvenIfFarmerIsntHere(time);
						if (!(location2 is BuildableGameLocation))
						{
							continue;
						}
						foreach (Building building in (location2 as BuildableGameLocation).buildings)
						{
							if (building.indoors.Value != null && building.indoors.Value != currentLocation)
							{
								building.indoors.Value.updateEvenIfFarmerIsntHere(time);
							}
						}
					}
					return;
				}
			}
			foreach (GameLocation location in locations)
			{
				bool shouldUpdate = location.farmers.Any();
				if (!shouldUpdate && location.CanBeRemotedlyViewed())
				{
					if (player.currentLocation == location)
					{
						shouldUpdate = true;
					}
					else
					{
						foreach (Farmer who in otherFarmers.Values)
						{
							if (who.viewingLocation.Value != null && who.viewingLocation.Value.Equals(location.Name))
							{
								shouldUpdate = true;
								break;
							}
						}
					}
				}
				if (shouldUpdate)
				{
					location.UpdateWhenCurrentLocation(time);
				}
				location.updateEvenIfFarmerIsntHere(time);
				if (location.wasInhabited != shouldUpdate)
				{
					location.wasInhabited = shouldUpdate;
					if (IsMasterGame)
					{
						location.cleanupForVacancy();
					}
				}
				if (!(location is BuildableGameLocation))
				{
					continue;
				}
				foreach (Building building2 in (location as BuildableGameLocation).buildings)
				{
					GameLocation interior = building2.indoors.Value;
					if (interior != null)
					{
						if (interior.farmers.Any())
						{
							interior.UpdateWhenCurrentLocation(time);
						}
						interior.updateEvenIfFarmerIsntHere(time);
					}
				}
			}
			if (currentLocation.isTemp())
			{
				currentLocation.UpdateWhenCurrentLocation(time);
				currentLocation.updateEvenIfFarmerIsntHere(time);
			}
			MineShaft.UpdateMines(time);
			VolcanoDungeon.UpdateLevels(time);
		}

		public static void performTenMinuteClockUpdate()
		{
			hooks.OnGame1_PerformTenMinuteClockUpdate(delegate
			{
				int trulyDarkTime = getTrulyDarkTime();
				gameTimeInterval = 0;
				if (IsMasterGame)
				{
					timeOfDay += 10;
				}
				if (timeOfDay % 100 >= 60)
				{
					timeOfDay = timeOfDay - timeOfDay % 100 + 100;
				}
				timeOfDay = Math.Min(timeOfDay, 2600);
				if (isLightning && timeOfDay < 2400 && IsMasterGame)
				{
					Utility.performLightningUpdate(timeOfDay);
				}
				if (timeOfDay == trulyDarkTime)
				{
					currentLocation.switchOutNightTiles();
				}
				else if (timeOfDay == getModeratelyDarkTime())
				{
					if (currentLocation.IsOutdoors && !IsRainingHere())
					{
						ambientLight = Color.White;
					}
					if (!IsRainingHere() && !(currentLocation is MineShaft) && currentSong != null && !currentSong.Name.Contains("ambient") && currentLocation is Town)
					{
						changeMusicTrack("none");
					}
				}
				if (getMusicTrackName().StartsWith(currentSeason) && !getMusicTrackName().Contains("ambient") && !eventUp && isDarkOut())
				{
					changeMusicTrack("none", track_interruptable: true);
				}
				if ((bool)currentLocation.isOutdoors && !IsRainingHere() && !eventUp && getMusicTrackName().Contains("day") && isDarkOut())
				{
					changeMusicTrack("none", track_interruptable: true);
				}
				if (weatherIcon == 1)
				{
					int num = Convert.ToInt32(temporaryContent.Load<Dictionary<string, string>>("Data\\Festivals\\" + currentSeason + dayOfMonth)["conditions"].Split('/')[1].Split(' ')[0]);
					if (whereIsTodaysFest == null)
					{
						whereIsTodaysFest = temporaryContent.Load<Dictionary<string, string>>("Data\\Festivals\\" + currentSeason + dayOfMonth)["conditions"].Split('/')[0];
					}
					if (timeOfDay == num)
					{
						Dictionary<string, string> dictionary = temporaryContent.Load<Dictionary<string, string>>("Data\\Festivals\\" + currentSeason + dayOfMonth);
						string text = dictionary["conditions"].Split('/')[0];
						if (dictionary.ContainsKey("locationDisplayName"))
						{
							text = dictionary["locationDisplayName"];
						}
						else
						{
							switch (text)
							{
							case "Forest":
								text = (currentSeason.Equals("winter") ? content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.2634") : content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.2635"));
								break;
							case "Town":
								text = content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.2637");
								break;
							case "Beach":
								text = content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.2639");
								break;
							}
						}
						showGlobalMessage(content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.2640", temporaryContent.Load<Dictionary<string, string>>("Data\\Festivals\\" + currentSeason + dayOfMonth)["name"]) + text);
					}
				}
				player.performTenMinuteUpdate();
				switch (timeOfDay)
				{
				case 1200:
					if ((bool)currentLocation.isOutdoors && !IsRainingHere() && (currentSong == null || currentSong.IsStopped || currentSong.Name.ToLower().Contains("ambient")))
					{
						playMorningSong();
					}
					break;
				case 2000:
					if (!IsRainingHere() && currentLocation is Town)
					{
						changeMusicTrack("none");
					}
					break;
				case 2400:
					dayTimeMoneyBox.timeShakeTimer = 2000;
					player.doEmote(24);
					showGlobalMessage(content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.2652"));
					break;
				case 2500:
					dayTimeMoneyBox.timeShakeTimer = 2000;
					player.doEmote(24);
					break;
				case 2600:
					dayTimeMoneyBox.timeShakeTimer = 2000;
					if (player.mount != null)
					{
						player.mount.dismount();
					}
					if (player.IsSitting())
					{
						player.StopSitting(animate: false);
					}
					if (player.UsingTool && (player.CurrentTool == null || !(player.CurrentTool is FishingRod fishingRod) || (!fishingRod.isReeling && !fishingRod.pullingOutOfWater)))
					{
						player.completelyStopAnimatingOrDoingAction();
					}
					break;
				case 2800:
					if (activeClickableMenu != null)
					{
						activeClickableMenu.emergencyShutDown();
						exitActiveMenu();
					}
					player.startToPassOut();
					if (player.mount != null)
					{
						player.mount.dismount();
					}
					break;
				}
				foreach (GameLocation location in locations)
				{
					GameLocation current = location;
					if (current.NameOrUniqueName == currentLocation.NameOrUniqueName)
					{
						current = currentLocation;
					}
					current.performTenMinuteUpdate(timeOfDay);
					if (current is Farm)
					{
						((Farm)current).timeUpdate(10);
					}
				}
				MineShaft.UpdateMines10Minutes(timeOfDay);
				VolcanoDungeon.UpdateLevels10Minutes(timeOfDay);
				if (IsMasterGame && farmEvent == null)
				{
					netWorldState.Value.UpdateFromGame1();
				}
			});
		}

		public static bool shouldPlayMorningSong(bool loading_game = false)
		{
			if (eventUp)
			{
				return false;
			}
			if ((double)options.musicVolumeLevel <= 0.025)
			{
				return false;
			}
			if (timeOfDay >= 1200)
			{
				return false;
			}
			if (!loading_game && currentSong != null && !requestedMusicTrack.ToLower().Contains("ambient"))
			{
				return false;
			}
			return true;
		}

		public static void UpdateGameClock(GameTime time)
		{
			if (shouldTimePass() && !IsClient)
			{
				gameTimeInterval += time.ElapsedGameTime.Milliseconds;
			}
			if (timeOfDay >= getTrulyDarkTime())
			{
				int adjustedTime2 = (int)((float)(timeOfDay - timeOfDay % 100) + (float)(timeOfDay % 100 / 10) * 16.66f);
				float transparency2 = Math.Min(0.93f, 0.75f + ((float)(adjustedTime2 - getTrulyDarkTime()) + (float)gameTimeInterval / 7000f * 16.6f) * 0.000625f);
				outdoorLight = (IsRainingHere() ? ambientLight : eveningColor) * transparency2;
			}
			else if (timeOfDay >= getStartingToGetDarkTime())
			{
				int adjustedTime = (int)((float)(timeOfDay - timeOfDay % 100) + (float)(timeOfDay % 100 / 10) * 16.66f);
				float transparency = Math.Min(0.93f, 0.3f + ((float)(adjustedTime - getStartingToGetDarkTime()) + (float)gameTimeInterval / 7000f * 16.6f) * 0.00225f);
				outdoorLight = (IsRainingHere() ? ambientLight : eveningColor) * transparency;
			}
			else if (IsRainingHere())
			{
				outdoorLight = ambientLight * 0.3f;
			}
			if (currentLocation != null && gameTimeInterval > 7000 + currentLocation.getExtraMillisecondsPerInGameMinuteForThisLocation())
			{
				if (panMode)
				{
					gameTimeInterval = 0;
				}
				else
				{
					performTenMinuteClockUpdate();
				}
			}
		}

		public static Event getAvailableWeddingEvent()
		{
			if (weddingsToday.Count > 0)
			{
				long id = weddingsToday[0];
				weddingsToday.RemoveAt(0);
				Farmer farmer = getFarmerMaybeOffline(id);
				if (farmer == null)
				{
					return null;
				}
				if (farmer.hasRoommate())
				{
					return null;
				}
				if (IsMultiplayer)
				{
					_ = (farmer.NetFields.Root as NetRoot<Farmer>).Clone().Value;
				}
				Event wedding_event = null;
				if (farmer.spouse != null)
				{
					return Utility.getWeddingEvent(farmer);
				}
				long? spouseID = farmer.team.GetSpouse(farmer.UniqueMultiplayerID);
				Farmer spouse = getFarmerMaybeOffline(spouseID.Value);
				if (spouse == null)
				{
					return null;
				}
				if (!getOnlineFarmers().Contains(farmer) || !getOnlineFarmers().Contains(spouse))
				{
					return null;
				}
				player.team.GetFriendship(farmer.UniqueMultiplayerID, spouseID.Value).Status = FriendshipStatus.Married;
				player.team.GetFriendship(farmer.UniqueMultiplayerID, spouseID.Value).WeddingDate = new WorldDate(Date);
				return Utility.getPlayerWeddingEvent(farmer, spouse);
			}
			return null;
		}

		public static void checkForNewLevelPerks()
		{
			Dictionary<string, string> cookingRecipes = content.Load<Dictionary<string, string>>("Data\\CookingRecipes");
			int farmerLevel = player.Level;
			foreach (string s2 in cookingRecipes.Keys)
			{
				string[] getConditions2 = cookingRecipes[s2].Split('/')[3].Split(' ');
				if (getConditions2[0].Equals("l") && Convert.ToInt32(getConditions2[1]) <= farmerLevel && !player.cookingRecipes.ContainsKey(s2))
				{
					player.cookingRecipes.Add(s2, 0);
					currentObjectDialogue.Enqueue(parseText(content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.2666") + s2));
					currentDialogueCharacterIndex = 1;
					dialogueUp = true;
					dialogueTyping = true;
				}
				else
				{
					if (!getConditions2[0].Equals("s"))
					{
						continue;
					}
					int levelRequired2 = Convert.ToInt32(getConditions2[2]);
					bool success2 = false;
					switch (getConditions2[1])
					{
					case "Farming":
						if (player.FarmingLevel >= levelRequired2)
						{
							success2 = true;
						}
						break;
					case "Fishing":
						if (player.FishingLevel >= levelRequired2)
						{
							success2 = true;
						}
						break;
					case "Mining":
						if (player.MiningLevel >= levelRequired2)
						{
							success2 = true;
						}
						break;
					case "Combat":
						if (player.CombatLevel >= levelRequired2)
						{
							success2 = true;
						}
						break;
					case "Foraging":
						if (player.ForagingLevel >= levelRequired2)
						{
							success2 = true;
						}
						break;
					case "Luck":
						if (player.LuckLevel >= levelRequired2)
						{
							success2 = true;
						}
						break;
					}
					if (success2 && !player.cookingRecipes.ContainsKey(s2))
					{
						player.cookingRecipes.Add(s2, 0);
						currentObjectDialogue.Enqueue(parseText(content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.2666") + s2));
						currentDialogueCharacterIndex = 1;
						dialogueUp = true;
						dialogueTyping = true;
					}
				}
			}
			Dictionary<string, string> craftingRecipes = content.Load<Dictionary<string, string>>("Data\\CraftingRecipes");
			foreach (string s in craftingRecipes.Keys)
			{
				string[] getConditions = craftingRecipes[s].Split('/')[4].Split(' ');
				if (getConditions[0].Equals("l") && Convert.ToInt32(getConditions[1]) <= farmerLevel && !player.craftingRecipes.ContainsKey(s))
				{
					player.craftingRecipes.Add(s, 0);
					currentObjectDialogue.Enqueue(parseText(content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.2677") + s));
					currentDialogueCharacterIndex = 1;
					dialogueUp = true;
					dialogueTyping = true;
				}
				else
				{
					if (!getConditions[0].Equals("s"))
					{
						continue;
					}
					int levelRequired = Convert.ToInt32(getConditions[2]);
					bool success = false;
					switch (getConditions[1])
					{
					case "Farming":
						if (player.FarmingLevel >= levelRequired)
						{
							success = true;
						}
						break;
					case "Fishing":
						if (player.FishingLevel >= levelRequired)
						{
							success = true;
						}
						break;
					case "Mining":
						if (player.MiningLevel >= levelRequired)
						{
							success = true;
						}
						break;
					case "Combat":
						if (player.CombatLevel >= levelRequired)
						{
							success = true;
						}
						break;
					case "Foraging":
						if (player.ForagingLevel >= levelRequired)
						{
							success = true;
						}
						break;
					case "Luck":
						if (player.LuckLevel >= levelRequired)
						{
							success = true;
						}
						break;
					}
					if (success && !player.craftingRecipes.ContainsKey(s))
					{
						player.craftingRecipes.Add(s, 0);
						currentObjectDialogue.Enqueue(parseText(content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.2677") + s));
						currentDialogueCharacterIndex = 1;
						dialogueUp = true;
						dialogueTyping = true;
					}
				}
			}
		}

		public static void exitActiveMenu()
		{
			activeClickableMenu = null;
		}

		public static void fadeScreenToBlack()
		{
			screenFade.FadeScreenToBlack();
		}

		public static void fadeClear()
		{
			screenFade.FadeClear();
		}

		private bool onFadeToBlackComplete()
		{
			bool should_halt = false;
			if (killScreen)
			{
				viewportFreeze = true;
				viewport.X = -10000;
			}
			if (exitToTitle)
			{
				menuUp = false;
				setGameMode(4);
				menuChoice = 0;
				fadeIn = false;
				fadeToBlack = true;
				fadeToBlackAlpha = 0.01f;
				exitToTitle = false;
				changeMusicTrack("none");
				debrisWeather.Clear();
				return true;
			}
			if (timeOfDayAfterFade != -1)
			{
				timeOfDay = timeOfDayAfterFade;
				timeOfDayAfterFade = -1;
			}
			if (!nonWarpFade && locationRequest != null && !menuUp)
			{
				GameLocation previousLocation = currentLocation;
				if (emoteMenu != null)
				{
					emoteMenu.exitThisMenuNoSound();
				}
				if (client != null && currentLocation != null)
				{
					currentLocation.StoreCachedMultiplayerMap(multiplayer.cachedMultiplayerMaps);
				}
				currentLocation.cleanupBeforePlayerExit();
				multiplayer.broadcastLocationDelta(currentLocation);
				bool hasResetLocation = false;
				displayFarmer = true;
				if (eventOver)
				{
					eventFinished();
					if (dayOfMonth == 0)
					{
						newDayAfterFade(delegate
						{
							player.Position = new Vector2(320f, 320f);
						});
					}
					return true;
				}
				if (locationRequest.IsRequestFor(currentLocation) && player.previousLocationName != "" && !eventUp && !currentLocation.Name.StartsWith("UndergroundMine"))
				{
					player.Position = new Vector2(xLocationAfterWarp * 64, yLocationAfterWarp * 64 - (player.Sprite.getHeight() - 32) + 16);
					viewportFreeze = false;
					currentLocation.resetForPlayerEntry();
					hasResetLocation = true;
				}
				else
				{
					if (locationRequest.Name.StartsWith("UndergroundMine"))
					{
						if (!currentLocation.Name.StartsWith("UndergroundMine"))
						{
							changeMusicTrack("none");
						}
						MineShaft mine = locationRequest.Location as MineShaft;
						if (player.IsSitting())
						{
							player.StopSitting(animate: false);
						}
						player.Halt();
						player.forceCanMove();
						if (!IsClient || (locationRequest.Location != null && locationRequest.Location.Root != null))
						{
							mine.resetForPlayerEntry();
							hasResetLocation = true;
						}
						currentLocation = mine;
						currentLocation.Map.LoadTileSheets(mapDisplayDevice);
						checkForRunButton(GetKeyboardState());
					}
					if (!eventUp && !menuUp)
					{
						player.Position = new Vector2(xLocationAfterWarp * 64, yLocationAfterWarp * 64 - (player.Sprite.getHeight() - 32) + 16);
					}
					if (!locationRequest.Name.StartsWith("UndergroundMine"))
					{
						currentLocation = locationRequest.Location;
						if (!IsClient)
						{
							locationRequest.Loaded(locationRequest.Location);
							currentLocation.resetForPlayerEntry();
							hasResetLocation = true;
						}
						currentLocation.Map.LoadTileSheets(mapDisplayDevice);
						if (!viewportFreeze && currentLocation.Map.DisplayWidth <= viewport.Width)
						{
							viewport.X = (currentLocation.Map.DisplayWidth - viewport.Width) / 2;
						}
						if (!viewportFreeze && currentLocation.Map.DisplayHeight <= viewport.Height)
						{
							viewport.Y = (currentLocation.Map.DisplayHeight - viewport.Height) / 2;
						}
						checkForRunButton(GetKeyboardState(), ignoreKeyPressQualifier: true);
					}
					if (!eventUp)
					{
						viewportFreeze = false;
					}
				}
				forceSnapOnNextViewportUpdate = true;
				player.FarmerSprite.PauseForSingleAnimation = false;
				player.faceDirection(facingDirectionAfterWarp);
				_isWarping = false;
				if (player.ActiveObject != null)
				{
					player.showCarrying();
				}
				else
				{
					player.showNotCarrying();
				}
				if (IsClient)
				{
					if (locationRequest.Location != null && locationRequest.Location.Root != null && multiplayer.isActiveLocation(locationRequest.Location))
					{
						currentLocation = locationRequest.Location;
						locationRequest.Loaded(locationRequest.Location);
						if (!hasResetLocation)
						{
							currentLocation.resetForPlayerEntry();
						}
						player.currentLocation = currentLocation;
						locationRequest.Warped(currentLocation);
						currentLocation.updateSeasonalTileSheets();
						if (IsDebrisWeatherHere())
						{
							populateDebrisWeatherArray();
						}
						warpingForForcedRemoteEvent = false;
						locationRequest = null;
					}
					else
					{
						requestLocationInfoFromServer();
						if (currentLocation == null)
						{
							return true;
						}
					}
				}
				else
				{
					player.currentLocation = locationRequest.Location;
					locationRequest.Warped(locationRequest.Location);
					locationRequest = null;
				}
				if (locationRequest == null && currentLocation.Name == "Farm" && !eventUp)
				{
					if (player.position.X / 64f >= (float)(currentLocation.map.Layers[0].LayerWidth - 1))
					{
						player.position.X -= 64f;
					}
					else if (player.position.Y / 64f >= (float)(currentLocation.map.Layers[0].LayerHeight - 1))
					{
						player.position.Y -= 32f;
					}
					if (player.position.Y / 64f >= (float)(currentLocation.map.Layers[0].LayerHeight - 2))
					{
						player.position.X -= 48f;
					}
				}
				if (previousLocation != null && previousLocation.Name.StartsWith("UndergroundMine") && currentLocation != null && !currentLocation.Name.StartsWith("UndergroundMine"))
				{
					MineShaft.OnLeftMines();
				}
				should_halt = true;
			}
			if (newDay)
			{
				newDayAfterFade(delegate
				{
					if (eventOver)
					{
						eventFinished();
						if (dayOfMonth == 0)
						{
							newDayAfterFade(delegate
							{
								player.Position = new Vector2(320f, 320f);
							});
						}
					}
					nonWarpFade = false;
					fadeIn = false;
				});
				return true;
			}
			if (eventOver)
			{
				eventFinished();
				if (dayOfMonth == 0)
				{
					newDayAfterFade(delegate
					{
						currentLocation.resetForPlayerEntry();
						nonWarpFade = false;
						fadeIn = false;
					});
				}
				return true;
			}
			if (boardingBus)
			{
				boardingBus = false;
				drawObjectDialogue(content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.2694") + (currentLocation.Name.Equals("Desert") ? content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.2696") : content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.2697")));
				messagePause = true;
				viewportFreeze = false;
			}
			if (IsRainingHere() && currentSong != null && currentSong != null && currentSong.Name.Equals("rain"))
			{
				if (currentLocation.IsOutdoors)
				{
					currentSong.SetVariable("Frequency", 100f);
				}
				else if (!currentLocation.Name.StartsWith("UndergroundMine"))
				{
					currentSong.SetVariable("Frequency", 15f);
				}
			}
			return should_halt;
		}

		private static void onFadedBackInComplete()
		{
			if (killScreen)
			{
				pauseThenMessage(1500, "..." + player.Name + "?", showProgressBar: false);
			}
			else if (!eventUp)
			{
				player.CanMove = true;
			}
			checkForRunButton(oldKBState, ignoreKeyPressQualifier: true);
		}

		public static void UpdateOther(GameTime time)
		{
			if (currentLocation == null || (!player.passedOut && screenFade.UpdateFade(time)))
			{
				return;
			}
			if (dialogueUp || currentBillboard != 0)
			{
				player.CanMove = false;
			}
			for (int i = delayedActions.Count - 1; i >= 0; i--)
			{
				DelayedAction action = delayedActions[i];
				if (action.update(time) && delayedActions.Contains(action))
				{
					delayedActions.Remove(action);
				}
			}
			if (timeOfDay >= 2600 || player.stamina <= -15f)
			{
				if (currentMinigame != null && currentMinigame.forceQuit())
				{
					currentMinigame = null;
				}
				if (currentMinigame == null && player.canMove && player.freezePause <= 0 && !player.UsingTool && !eventUp && (IsMasterGame || (bool)player.isCustomized) && locationRequest == null && activeClickableMenu == null)
				{
					player.startToPassOut();
					player.freezePause = 7000;
				}
			}
			for (int j = screenOverlayTempSprites.Count - 1; j >= 0; j--)
			{
				if (screenOverlayTempSprites[j].update(time))
				{
					screenOverlayTempSprites.RemoveAt(j);
				}
			}
			for (int k = uiOverlayTempSprites.Count - 1; k >= 0; k--)
			{
				if (uiOverlayTempSprites[k].update(time))
				{
					uiOverlayTempSprites.RemoveAt(k);
				}
			}
			if (pickingTool)
			{
				pickToolInterval += time.ElapsedGameTime.Milliseconds;
				if (pickToolInterval > 500f)
				{
					pickingTool = false;
					pickToolInterval = 0f;
					if (!eventUp)
					{
						player.CanMove = true;
					}
					player.UsingTool = false;
					switch (player.FacingDirection)
					{
					case 0:
						player.Sprite.currentFrame = 16;
						break;
					case 1:
						player.Sprite.currentFrame = 8;
						break;
					case 2:
						player.Sprite.currentFrame = 0;
						break;
					case 3:
						player.Sprite.currentFrame = 24;
						break;
					}
					if (!GetKeyboardState().IsKeyDown(Keys.LeftShift))
					{
						player.setRunning(options.autoRun);
					}
				}
				else if (pickToolInterval > 83.333336f)
				{
					switch (player.FacingDirection)
					{
					case 0:
						player.FarmerSprite.setCurrentFrame(196);
						break;
					case 1:
						player.FarmerSprite.setCurrentFrame(194);
						break;
					case 2:
						player.FarmerSprite.setCurrentFrame(192);
						break;
					case 3:
						player.FarmerSprite.setCurrentFrame(198);
						break;
					}
				}
			}
			if ((player.CanMove || player.UsingTool) && shouldTimePass())
			{
				buffsDisplay.update(time);
			}
			if (player.CurrentItem != null)
			{
				player.CurrentItem.actionWhenBeingHeld(player);
			}
			float tmp = dialogueButtonScale;
			dialogueButtonScale = (float)(16.0 * Math.Sin(time.TotalGameTime.TotalMilliseconds % 1570.0 / 500.0));
			if (tmp > dialogueButtonScale && !dialogueButtonShrinking)
			{
				dialogueButtonShrinking = true;
			}
			else if (tmp < dialogueButtonScale && dialogueButtonShrinking)
			{
				dialogueButtonShrinking = false;
			}
			if (player.currentUpgrade != null && currentLocation.Name.Equals("Farm") && player.currentUpgrade.daysLeftTillUpgradeDone <= 3)
			{
				player.currentUpgrade.update(time.ElapsedGameTime.Milliseconds);
			}
			if (screenGlow)
			{
				if (screenGlowUp || screenGlowHold)
				{
					if (screenGlowHold)
					{
						screenGlowAlpha = Math.Min(screenGlowAlpha + screenGlowRate, screenGlowMax);
					}
					else
					{
						screenGlowAlpha = Math.Min(screenGlowAlpha + 0.03f, 0.6f);
						if (screenGlowAlpha >= 0.6f)
						{
							screenGlowUp = false;
						}
					}
				}
				else
				{
					screenGlowAlpha -= 0.01f;
					if (screenGlowAlpha <= 0f)
					{
						screenGlow = false;
					}
				}
			}
			for (int m = hudMessages.Count - 1; m >= 0; m--)
			{
				if (hudMessages.ElementAt(m).update(time))
				{
					hudMessages.RemoveAt(m);
				}
			}
			updateWeather(time);
			if (!fadeToBlack)
			{
				currentLocation.checkForMusic(time);
			}
			if (debrisSoundInterval > 0f)
			{
				debrisSoundInterval -= time.ElapsedGameTime.Milliseconds;
			}
			noteBlockTimer += time.ElapsedGameTime.Milliseconds;
			if (noteBlockTimer > 1000f)
			{
				noteBlockTimer = 0f;
				if (player.health < 20 && CurrentEvent == null)
				{
					hitShakeTimer = 250;
					if (player.health <= 10)
					{
						hitShakeTimer = 500;
						if (showingHealthBar && fadeToBlackAlpha <= 0f)
						{
							for (int l = 0; l < 3; l++)
							{
								uiOverlayTempSprites.Add(new TemporaryAnimatedSprite("LooseSprites\\Cursors", new Microsoft.Xna.Framework.Rectangle(366, 412, 5, 6), new Vector2(random.Next(32) + uiViewport.Width - 112, uiViewport.Height - 224 - (player.maxHealth - 100) - 16 + 4), flipped: false, 0.017f, Color.Red)
								{
									motion = new Vector2(-1.5f, -8 + random.Next(-1, 2)),
									acceleration = new Vector2(0f, 0.5f),
									local = true,
									scale = 4f,
									delayBeforeAnimationStart = l * 150
								});
							}
						}
					}
				}
			}
			if (showKeyHelp && !eventUp)
			{
				keyHelpString = "";
				if (dialogueUp)
				{
					keyHelpString += content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.2716");
				}
				else if (menuUp)
				{
					keyHelpString += content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.2719");
					keyHelpString += content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.2720");
				}
				else if (player.ActiveObject != null)
				{
					keyHelpString += content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.2727");
					keyHelpString = keyHelpString + Environment.NewLine + content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.2728");
					if (player.numberOfItemsInInventory() < (int)player.maxItems)
					{
						keyHelpString = keyHelpString + Environment.NewLine + content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.2729");
					}
					if (player.numberOfItemsInInventory() > 0)
					{
						keyHelpString = keyHelpString + Environment.NewLine + content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.2730");
					}
					keyHelpString = keyHelpString + Environment.NewLine + content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.2731");
					keyHelpString = keyHelpString + Environment.NewLine + content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.2732");
				}
				else
				{
					keyHelpString += content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.2733");
					if (player.CurrentTool != null)
					{
						keyHelpString = keyHelpString + Environment.NewLine + content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.2734", player.CurrentTool.DisplayName);
					}
					if (player.numberOfItemsInInventory() > 0)
					{
						keyHelpString = keyHelpString + Environment.NewLine + content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.2735");
					}
					keyHelpString = keyHelpString + Environment.NewLine + content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.2731");
					keyHelpString = keyHelpString + Environment.NewLine + content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.2732");
				}
			}
			drawLighting = (currentLocation.IsOutdoors && !outdoorLight.Equals(Color.White)) || !ambientLight.Equals(Color.White) || (currentLocation is MineShaft && !((MineShaft)currentLocation).getLightingColor(time).Equals(Color.White));
			if (player.hasBuff(26))
			{
				drawLighting = true;
			}
			if (hitShakeTimer > 0)
			{
				hitShakeTimer -= time.ElapsedGameTime.Milliseconds;
			}
			if (staminaShakeTimer > 0)
			{
				staminaShakeTimer -= time.ElapsedGameTime.Milliseconds;
			}
			if (background != null)
			{
				background.update(viewport);
			}
			cursorTileHintCheckTimer -= (int)time.ElapsedGameTime.TotalMilliseconds;
			currentCursorTile.X = (viewport.X + getOldMouseX()) / 64;
			currentCursorTile.Y = (viewport.Y + getOldMouseY()) / 64;
			if (cursorTileHintCheckTimer <= 0 || !currentCursorTile.Equals(lastCursorTile))
			{
				cursorTileHintCheckTimer = 250;
				updateCursorTileHint();
				if (player.CanMove)
				{
					checkForRunButton(oldKBState, ignoreKeyPressQualifier: true);
				}
			}
			if (!currentLocation.Name.StartsWith("UndergroundMine"))
			{
				MineShaft.timeSinceLastMusic = 200000;
			}
			if (activeClickableMenu == null && farmEvent == null && keyboardDispatcher != null && !IsChatting)
			{
				keyboardDispatcher.Subscriber = null;
			}
		}

		public static void updateWeather(GameTime time)
		{
			if (IsSnowingHere() && (bool)currentLocation.isOutdoors && !(currentLocation is Desert))
			{
				snowPos = updateFloatingObjectPositionForMovement(current: new Vector2(viewport.X, viewport.Y), w: snowPos, previous: previousViewportPosition, speed: -1f);
			}
			if (IsRainingHere() && currentLocation.IsOutdoors)
			{
				for (int i = 0; i < rainDrops.Length; i++)
				{
					if (rainDrops[i].frame == 0)
					{
						rainDrops[i].accumulator += time.ElapsedGameTime.Milliseconds;
						if (rainDrops[i].accumulator < 70)
						{
							continue;
						}
						rainDrops[i].position += new Vector2(-16 + i * 8 / rainDrops.Length, 32 - i * 8 / rainDrops.Length);
						rainDrops[i].accumulator = 0;
						if (random.NextDouble() < 0.1)
						{
							rainDrops[i].frame++;
						}
						if (currentLocation is IslandNorth || currentLocation is Caldera)
						{
							Point p = new Point((int)(rainDrops[i].position.X + (float)viewport.X) / 64, (int)(rainDrops[i].position.Y + (float)viewport.Y) / 64);
							p.Y--;
							if (currentLocation.isTileOnMap(p.X, p.Y) && currentLocation.getTileIndexAt(p, "Back") == -1 && currentLocation.getTileIndexAt(p, "Buildings") == -1)
							{
								rainDrops[i].frame = 0;
							}
						}
						if (rainDrops[i].position.Y > (float)(viewport.Height + 64))
						{
							rainDrops[i].position.Y = -64f;
						}
						continue;
					}
					rainDrops[i].accumulator += time.ElapsedGameTime.Milliseconds;
					if (rainDrops[i].accumulator > 70)
					{
						rainDrops[i].frame = (rainDrops[i].frame + 1) % 4;
						rainDrops[i].accumulator = 0;
						if (rainDrops[i].frame == 0)
						{
							rainDrops[i].position = new Vector2(random.Next(viewport.Width), random.Next(viewport.Height));
						}
					}
				}
			}
			else if (IsDebrisWeatherHere() && currentLocation.IsOutdoors && !currentLocation.ignoreDebrisWeather)
			{
				if (currentSeason.Equals("fall") && random.NextDouble() < 0.001 && windGust == 0f && WeatherDebris.globalWind >= -0.5f)
				{
					windGust += (float)random.Next(-10, -1) / 100f;
					if (soundBank != null)
					{
						wind = soundBank.GetCue("wind");
						wind.Play();
					}
				}
				else if (windGust != 0f)
				{
					windGust = Math.Max(-5f, windGust * 1.02f);
					WeatherDebris.globalWind = -0.5f + windGust;
					if (windGust < -0.2f && random.NextDouble() < 0.007)
					{
						windGust = 0f;
					}
				}
				foreach (WeatherDebris item in debrisWeather)
				{
					item.update();
				}
			}
			if (WeatherDebris.globalWind < -0.5f && wind != null)
			{
				WeatherDebris.globalWind = Math.Min(-0.5f, WeatherDebris.globalWind + 0.015f);
				wind.SetVariable("Volume", (0f - WeatherDebris.globalWind) * 20f);
				wind.SetVariable("Frequency", (0f - WeatherDebris.globalWind) * 20f);
				if (WeatherDebris.globalWind == -0.5f)
				{
					wind.Stop(AudioStopOptions.AsAuthored);
				}
			}
		}

		public static void updateCursorTileHint()
		{
			if (activeClickableMenu != null)
			{
				return;
			}
			mouseCursorTransparency = 1f;
			isActionAtCurrentCursorTile = false;
			isInspectionAtCurrentCursorTile = false;
			isSpeechAtCurrentCursorTile = false;
			int xTile = (viewport.X + getOldMouseX()) / 64;
			int yTile = (viewport.Y + getOldMouseY()) / 64;
			if (currentLocation != null)
			{
				isActionAtCurrentCursorTile = currentLocation.isActionableTile(xTile, yTile, player);
				if (!isActionAtCurrentCursorTile)
				{
					isActionAtCurrentCursorTile = currentLocation.isActionableTile(xTile, yTile + 1, player);
				}
			}
			lastCursorTile = currentCursorTile;
		}

		public static void updateMusic()
		{
			if (soundBank == null)
			{
				return;
			}
			if (game1.IsMainInstance)
			{
				Game1 important_music_instance = null;
				string important_instance_music = null;
				int sub_location_priority = 1;
				int non_ambient_world_priority = 2;
				int minigame_priority = 5;
				int event_priority = 6;
				int mermaid_show = 7;
				int priority = 0;
				float default_context_priority = GetDefaultSongPriority(getMusicTrackName(), game1.instanceIsOverridingTrack);
				MusicContext primary_music_context = MusicContext.Default;
				foreach (Game1 instance in GameRunner.instance.gameInstances)
				{
					MusicContext active_context = instance._instanceActiveMusicContext;
					if (instance.IsMainInstance)
					{
						primary_music_context = active_context;
					}
					string track_name = null;
					string actual_track_name = null;
					if (instance._instanceRequestedMusicTracks.ContainsKey(active_context))
					{
						track_name = instance._instanceRequestedMusicTracks[active_context].Key;
					}
					if (instance.instanceIsOverridingTrack && instance.instanceCurrentSong != null)
					{
						actual_track_name = instance.instanceCurrentSong.Name;
					}
					if (active_context == MusicContext.Event && priority < event_priority)
					{
						if (track_name != null)
						{
							priority = event_priority;
							important_music_instance = instance;
							important_instance_music = track_name;
						}
					}
					else if (active_context == MusicContext.MiniGame && priority < minigame_priority)
					{
						if (track_name != null)
						{
							priority = minigame_priority;
							important_music_instance = instance;
							important_instance_music = track_name;
						}
					}
					else if (active_context == MusicContext.SubLocation && priority < sub_location_priority)
					{
						if (track_name != null)
						{
							priority = sub_location_priority;
							important_music_instance = instance;
							important_instance_music = ((actual_track_name == null) ? track_name : actual_track_name);
						}
					}
					else if (track_name == "mermaidSong")
					{
						priority = mermaid_show;
						important_music_instance = instance;
						important_instance_music = track_name;
					}
					if (active_context == MusicContext.Default && primary_music_context <= active_context && track_name != null)
					{
						float instance_default_context_priority = GetDefaultSongPriority(track_name, instance.instanceIsOverridingTrack);
						if (default_context_priority < instance_default_context_priority)
						{
							default_context_priority = instance_default_context_priority;
							priority = non_ambient_world_priority;
							important_music_instance = instance;
							important_instance_music = ((actual_track_name == null) ? track_name : actual_track_name);
						}
					}
				}
				if (important_music_instance == null || important_music_instance == game1)
				{
					if (doesMusicContextHaveTrack(MusicContext.ImportantSplitScreenMusic))
					{
						stopMusicTrack(MusicContext.ImportantSplitScreenMusic);
					}
				}
				else if (important_instance_music == null && doesMusicContextHaveTrack(MusicContext.ImportantSplitScreenMusic))
				{
					stopMusicTrack(MusicContext.ImportantSplitScreenMusic);
				}
				else if (important_instance_music != null && getMusicTrackName(MusicContext.ImportantSplitScreenMusic) != important_instance_music)
				{
					changeMusicTrack(important_instance_music, track_interruptable: false, MusicContext.ImportantSplitScreenMusic);
				}
			}
			string song_to_play = "";
			bool track_overrideable = false;
			bool song_overridden = false;
			if (currentLocation != null && currentLocation.IsMiniJukeboxPlaying() && (!requestedMusicDirty || requestedMusicTrackOverrideable) && currentTrackOverrideable)
			{
				song_to_play = "";
				song_overridden = true;
				string mini_jukebox_track = currentLocation.miniJukeboxTrack.Value;
				if (mini_jukebox_track == "random")
				{
					mini_jukebox_track = ((currentLocation.randomMiniJukeboxTrack.Value != null) ? currentLocation.randomMiniJukeboxTrack.Value : "");
				}
				if (currentSong == null || !currentSong.IsPlaying || currentSong.Name != mini_jukebox_track)
				{
					song_to_play = mini_jukebox_track;
					requestedMusicDirty = false;
					track_overrideable = true;
				}
			}
			if (isOverridingTrack != song_overridden)
			{
				isOverridingTrack = song_overridden;
				if (!isOverridingTrack)
				{
					requestedMusicDirty = true;
				}
			}
			if (requestedMusicDirty)
			{
				song_to_play = requestedMusicTrack;
				track_overrideable = requestedMusicTrackOverrideable;
			}
			if (!song_to_play.Equals(""))
			{
				musicPlayerVolume = Math.Max(0f, Math.Min(options.musicVolumeLevel, musicPlayerVolume - 0.01f));
				ambientPlayerVolume = Math.Max(0f, Math.Min(options.musicVolumeLevel, ambientPlayerVolume - 0.01f));
				if (game1.IsMainInstance)
				{
					musicCategory.SetVolume(musicPlayerVolume);
					ambientCategory.SetVolume(ambientPlayerVolume);
				}
				if (musicPlayerVolume != 0f || ambientPlayerVolume != 0f || currentSong == null)
				{
					return;
				}
				if (song_to_play.Equals("none"))
				{
					jukeboxPlaying = false;
					currentSong.Stop(AudioStopOptions.Immediate);
				}
				else if ((options.musicVolumeLevel != 0f || options.ambientVolumeLevel != 0f) && (!song_to_play.Equals("rain") || endOfNightMenus.Count == 0))
				{
					if (game1.IsMainInstance)
					{
						currentSong.Stop(AudioStopOptions.Immediate);
						currentSong.Dispose();
					}
					currentSong = soundBank.GetCue(song_to_play);
					if (game1.IsMainInstance)
					{
						currentSong.Play();
					}
					if (game1.IsMainInstance && currentSong != null && currentSong.Name.Equals("rain") && currentLocation != null)
					{
						if (IsRainingHere())
						{
							if (currentLocation.IsOutdoors)
							{
								currentSong.SetVariable("Frequency", 100f);
							}
							else if (!currentLocation.Name.StartsWith("UndergroundMine"))
							{
								currentSong.SetVariable("Frequency", 15f);
							}
						}
						else if (eventUp)
						{
							currentSong.SetVariable("Frequency", 100f);
						}
					}
				}
				else
				{
					currentSong.Stop(AudioStopOptions.Immediate);
				}
				currentTrackOverrideable = track_overrideable;
				requestedMusicDirty = false;
			}
			else if (musicPlayerVolume < options.musicVolumeLevel || ambientPlayerVolume < options.ambientVolumeLevel)
			{
				if (musicPlayerVolume < options.musicVolumeLevel)
				{
					musicPlayerVolume = Math.Min(1f, musicPlayerVolume += 0.01f);
					if (game1.IsMainInstance)
					{
						musicCategory.SetVolume(options.musicVolumeLevel);
					}
				}
				if (ambientPlayerVolume < options.ambientVolumeLevel)
				{
					ambientPlayerVolume = Math.Min(1f, ambientPlayerVolume += 0.015f);
					if (game1.IsMainInstance)
					{
						ambientCategory.SetVolume(ambientPlayerVolume);
					}
				}
			}
			else if (currentSong != null && !currentSong.IsPlaying && !currentSong.IsStopped)
			{
				currentSong = soundBank.GetCue(currentSong.Name);
				if (game1.IsMainInstance)
				{
					currentSong.Play();
				}
			}
		}

		public static int GetDefaultSongPriority(string song_name, bool is_playing_override)
		{
			if (is_playing_override)
			{
				return 9;
			}
			if (song_name.Equals("none"))
			{
				return 0;
			}
			if (song_name.EndsWith("_day_ambient") || song_name.EndsWith("_night_ambient") || song_name.Equals("rain"))
			{
				return 1;
			}
			if (song_name.StartsWith(currentSeason))
			{
				return 2;
			}
			if (song_name.Contains("town"))
			{
				return 3;
			}
			if (song_name.Equals("jungle_ambience") || song_name.Contains("Ambient"))
			{
				return 7;
			}
			if (song_name.Equals("IslandMusic"))
			{
				return 8;
			}
			if (song_name.EndsWith("Mine"))
			{
				return 20;
			}
			return 10;
		}

		public static void updateRainDropPositionForPlayerMovement(int direction)
		{
			updateRainDropPositionForPlayerMovement(direction, overrideConstraints: false);
		}

		public static void updateRainDropPositionForPlayerMovement(int direction, bool overrideConstraints)
		{
			updateRainDropPositionForPlayerMovement(direction, overrideConstraints, player.speed);
		}

		public static void updateRainDropPositionForPlayerMovement(int direction, bool overrideConstraints, float speed)
		{
			if (!overrideConstraints && ((!IsRainingHere() && !IsDebrisWeatherHere()) || !currentLocation.IsOutdoors || (direction != 0 && direction != 2 && (player.getStandingX() < viewport.Width / 2 || player.getStandingX() > currentLocation.Map.DisplayWidth - viewport.Width / 2)) || (direction != 1 && direction != 3 && (player.getStandingY() < viewport.Height / 2 || player.getStandingY() > currentLocation.Map.DisplayHeight - viewport.Height / 2))))
			{
				return;
			}
			if (IsRainingHere())
			{
				for (int i = 0; i < rainDrops.Length; i++)
				{
					switch (direction)
					{
					case 0:
						rainDrops[i].position.Y += speed;
						if (rainDrops[i].position.Y > (float)(viewport.Height + 64))
						{
							rainDrops[i].position.Y = -64f;
						}
						break;
					case 1:
						rainDrops[i].position.X -= speed;
						if (rainDrops[i].position.X < -64f)
						{
							rainDrops[i].position.X = viewport.Width;
						}
						break;
					case 2:
						rainDrops[i].position.Y -= speed;
						if (rainDrops[i].position.Y < -64f)
						{
							rainDrops[i].position.Y = viewport.Height;
						}
						break;
					case 3:
						rainDrops[i].position.X += speed;
						if (rainDrops[i].position.X > (float)(viewport.Width + 64))
						{
							rainDrops[i].position.X = -64f;
						}
						break;
					}
				}
			}
			else
			{
				updateDebrisWeatherForMovement(debrisWeather, direction, overrideConstraints, speed);
			}
		}

		public static void initializeVolumeLevels()
		{
			if (!LocalMultiplayer.IsLocalMultiplayer() || game1.IsMainInstance)
			{
				soundCategory.SetVolume(options.soundVolumeLevel);
				musicCategory.SetVolume(options.musicVolumeLevel);
				ambientCategory.SetVolume(options.ambientVolumeLevel);
				footstepCategory.SetVolume(options.footstepVolumeLevel);
			}
		}

		public static void updateDebrisWeatherForMovement(List<WeatherDebris> debris, int direction, bool overrideConstraints, float speed)
		{
			if (!(fadeToBlackAlpha <= 0f) || debris == null)
			{
				return;
			}
			foreach (WeatherDebris w in debris)
			{
				switch (direction)
				{
				case 0:
					w.position.Y += speed;
					if (w.position.Y > (float)(viewport.Height + 64))
					{
						w.position.Y = -64f;
					}
					break;
				case 1:
					w.position.X -= speed;
					if (w.position.X < -64f)
					{
						w.position.X = viewport.Width;
					}
					break;
				case 2:
					w.position.Y -= speed;
					if (w.position.Y < -64f)
					{
						w.position.Y = viewport.Height;
					}
					break;
				case 3:
					w.position.X += speed;
					if (w.position.X > (float)(viewport.Width + 64))
					{
						w.position.X = -64f;
					}
					break;
				}
			}
		}

		public static Vector2 updateFloatingObjectPositionForMovement(Vector2 w, Vector2 current, Vector2 previous, float speed)
		{
			if (current.Y < previous.Y)
			{
				w.Y -= Math.Abs(current.Y - previous.Y) * speed;
			}
			else if (current.Y > previous.Y)
			{
				w.Y += Math.Abs(current.Y - previous.Y) * speed;
			}
			if (current.X > previous.X)
			{
				w.X += Math.Abs(current.X - previous.X) * speed;
			}
			else if (current.X < previous.X)
			{
				w.X -= Math.Abs(current.X - previous.X) * speed;
			}
			return w;
		}

		public static void updateRaindropPosition()
		{
			if (IsRainingHere())
			{
				int xOffset = viewport.X - (int)previousViewportPosition.X;
				int yOffset = viewport.Y - (int)previousViewportPosition.Y;
				for (int i = 0; i < rainDrops.Length; i++)
				{
					rainDrops[i].position.X -= (float)xOffset * 1f;
					rainDrops[i].position.Y -= (float)yOffset * 1f;
					if (rainDrops[i].position.Y > (float)(viewport.Height + 64))
					{
						rainDrops[i].position.Y = -64f;
					}
					else if (rainDrops[i].position.X < -64f)
					{
						rainDrops[i].position.X = viewport.Width;
					}
					else if (rainDrops[i].position.Y < -64f)
					{
						rainDrops[i].position.Y = viewport.Height;
					}
					else if (rainDrops[i].position.X > (float)(viewport.Width + 64))
					{
						rainDrops[i].position.X = -64f;
					}
				}
			}
			else
			{
				updateDebrisWeatherForMovement(debrisWeather);
			}
		}

		public static void updateDebrisWeatherForMovement(List<WeatherDebris> debris)
		{
			if (debris == null || !(fadeToBlackAlpha < 1f))
			{
				return;
			}
			int xOffset = viewport.X - (int)previousViewportPosition.X;
			int yOffset = viewport.Y - (int)previousViewportPosition.Y;
			int wrapBuffer = 16;
			foreach (WeatherDebris w in debris)
			{
				w.position.X -= (float)xOffset * 1f;
				w.position.Y -= (float)yOffset * 1f;
				if (w.position.Y > (float)(viewport.Height + 64 + wrapBuffer))
				{
					w.position.Y = -64f;
				}
				else if (w.position.X < (float)(-64 - wrapBuffer))
				{
					w.position.X = viewport.Width;
				}
				else if (w.position.Y < (float)(-64 - wrapBuffer))
				{
					w.position.Y = viewport.Height;
				}
				else if (w.position.X > (float)(viewport.Width + 64 + wrapBuffer))
				{
					w.position.X = -64f;
				}
			}
		}

		public static void randomizeRainPositions()
		{
			for (int i = 0; i < 70; i++)
			{
				rainDrops[i] = new RainDrop(random.Next(viewport.Width), random.Next(viewport.Height), random.Next(4), random.Next(70));
			}
		}

		public static void randomizeDebrisWeatherPositions(List<WeatherDebris> debris)
		{
			if (debris == null)
			{
				return;
			}
			foreach (WeatherDebris debri in debris)
			{
				debri.position = Utility.getRandomPositionOnScreen();
			}
		}

		public static void eventFinished()
		{
			player.canOnlyWalk = false;
			if (player.bathingClothes.Value)
			{
				player.canOnlyWalk = true;
			}
			eventOver = false;
			eventUp = false;
			player.CanMove = true;
			displayHUD = true;
			player.faceDirection(player.orientationBeforeEvent);
			player.completelyStopAnimatingOrDoingAction();
			viewportFreeze = false;
			Action callback = null;
			if (currentLocation.currentEvent.onEventFinished != null)
			{
				callback = currentLocation.currentEvent.onEventFinished;
				currentLocation.currentEvent.onEventFinished = null;
			}
			LocationRequest exitLocation = null;
			if (currentLocation.currentEvent != null)
			{
				exitLocation = currentLocation.currentEvent.exitLocation;
				currentLocation.currentEvent.cleanup();
				currentLocation.currentEvent = null;
			}
			if (player.ActiveObject != null)
			{
				player.showCarrying();
			}
			if (IsRainingHere() && (currentSong == null || !currentSong.Name.Equals("rain")) && !currentLocation.Name.StartsWith("UndergroundMine"))
			{
				changeMusicTrack("rain", track_interruptable: true);
			}
			else if (!IsRainingHere() && (currentSong == null || currentSong.Name == null || !currentSong.Name.Contains(currentSeason)))
			{
				changeMusicTrack("none", track_interruptable: true);
			}
			if (dayOfMonth != 0)
			{
				currentLightSources.Clear();
			}
			if (exitLocation == null && currentLocation != null && locationRequest == null)
			{
				exitLocation = new LocationRequest(currentLocation.NameOrUniqueName, currentLocation.isStructure, currentLocation);
			}
			if (exitLocation != null)
			{
				if (exitLocation.Location is Farm && player.positionBeforeEvent.Y == 64f)
				{
					player.positionBeforeEvent.X += 1f;
				}
				exitLocation.OnWarp += delegate
				{
					player.locationBeforeForcedEvent.Value = null;
				};
				warpFarmer(exitLocation, (int)player.positionBeforeEvent.X, (int)player.positionBeforeEvent.Y, player.orientationBeforeEvent);
			}
			else
			{
				player.setTileLocation(player.positionBeforeEvent);
				player.locationBeforeForcedEvent.Value = null;
			}
			nonWarpFade = false;
			fadeToBlackAlpha = 1f;
			callback?.Invoke();
		}

		public static void populateDebrisWeatherArray()
		{
			debrisWeather.Clear();
			isDebrisWeather = true;
			int debrisToMake = random.Next(16, 64);
			int baseIndex = ((!currentSeason.Equals("spring")) ? (currentSeason.Equals("winter") ? 3 : 2) : 0);
			for (int i = 0; i < debrisToMake; i++)
			{
				debrisWeather.Add(new WeatherDebris(new Vector2(random.Next(0, viewport.Width), random.Next(0, viewport.Height)), baseIndex, (float)random.Next(15) / 500f, (float)random.Next(-10, 0) / 50f, (float)random.Next(10) / 50f));
			}
		}

		private static void newSeason()
		{
			switch (currentSeason)
			{
			case "spring":
				currentSeason = "summer";
				break;
			case "summer":
				currentSeason = "fall";
				break;
			case "fall":
				currentSeason = "winter";
				break;
			case "winter":
				currentSeason = "spring";
				break;
			}
			setGraphicsForSeason();
			dayOfMonth = 1;
			Utility.ForAllLocations(delegate(GameLocation l)
			{
				l.seasonUpdate(GetSeasonForLocation(l));
			});
		}

		public static void playItemNumberSelectSound()
		{
			if (selectedItemsType.Equals("flutePitch"))
			{
				if (soundBank != null)
				{
					ICue cue = soundBank.GetCue("flute");
					cue.SetVariable("Pitch", 100 * numberOfSelectedItems);
					cue.Play();
				}
			}
			else if (selectedItemsType.Equals("drumTone"))
			{
				playSound("drumkit" + numberOfSelectedItems);
			}
			else
			{
				playSound("toolSwap");
			}
		}

		public static void slotsDone()
		{
			Response[] playAgainOptions = new Response[2]
			{
				new Response("Play", content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.2766")),
				new Response("Leave", content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.2768"))
			};
			if (slotResult[3] == 'x')
			{
				currentLocation.createQuestionDialogue(content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.2769", player.clubCoins), playAgainOptions, content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.2771") + currentLocation.map.GetLayer("Buildings").PickTile(new Location((int)(player.GetGrabTile().X * 64f), (int)(player.GetGrabTile().Y * 64f)), viewport.Size).Properties["Action"].ToString().Split(' ')[1]);
				currentDialogueCharacterIndex = currentObjectDialogue.Peek().Length - 1;
				return;
			}
			playSound("money");
			string specialMessage = (slotResult.Substring(0, 3).Equals("===") ? content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.2776") : "");
			player.clubCoins += Convert.ToInt32(slotResult.Substring(3));
			currentLocation.createQuestionDialogue(parseText(specialMessage + content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.2777", slotResult.Substring(3))), playAgainOptions, content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.2771") + currentLocation.map.GetLayer("Buildings").PickTile(new Location((int)(player.GetGrabTile().X * 64f), (int)(player.GetGrabTile().Y * 64f)), viewport.Size).Properties["Action"].ToString().Split(' ')[1]);
			currentDialogueCharacterIndex = currentObjectDialogue.Peek().Length - 1;
		}

		public static void prepareMultiplayerWedding(Farmer farmer)
		{
		}

		public static void prepareSpouseForWedding(Farmer farmer)
		{
			NPC characterFromName = getCharacterFromName(farmer.spouse);
			characterFromName.Schedule = null;
			characterFromName.DefaultMap = farmer.homeLocation.Value;
			characterFromName.DefaultPosition = Utility.PointToVector2((getLocationFromName(farmer.homeLocation.Value) as FarmHouse).getSpouseBedSpot(farmer.spouse)) * 64f;
			characterFromName.DefaultFacingDirection = 2;
		}

		public static void AddModNPCs()
		{
			LocalizedContentManager vanilla_content_manager = new LocalizedContentManager(game1.Content.ServiceProvider, game1.Content.RootDirectory);
			Dictionary<string, string> vanilla_dispositions_data = vanilla_content_manager.Load<Dictionary<string, string>>("Data\\NPCDispositions");
			Dictionary<string, string> dispositions_data = content.Load<Dictionary<string, string>>("Data\\NPCDispositions");
			foreach (string character_name in dispositions_data.Keys)
			{
				if (vanilla_dispositions_data.ContainsKey(character_name))
				{
					continue;
				}
				try
				{
					if (getCharacterFromName(character_name, mustBeVillager: true, useLocationsListOnly: true) == null)
					{
						string character_data = dispositions_data[character_name];
						getLocationFromNameInLocationsList(character_data.Split('/')[10].Split(' ')[0])?.addCharacter(new NPC(new AnimatedSprite("Characters\\" + NPC.getTextureNameForCharacter(character_name), 0, 16, 32), new Vector2(Convert.ToInt32(character_data.Split('/')[10].Split(' ')[1]) * 64, Convert.ToInt32(character_data.Split('/')[10].Split(' ')[2]) * 64), character_data.Split('/')[10].Split(' ')[0], 0, character_name, null, content.Load<Texture2D>("Portraits\\" + NPC.getTextureNameForCharacter(character_name)), eventActor: false));
					}
				}
				catch (Exception)
				{
				}
			}
			vanilla_content_manager.Unload();
		}

		public static void fixProblems()
		{
			if (!IsMasterGame)
			{
				return;
			}
			List<NPC> allCharacters = Utility.getPooledList();
			try
			{
				Utility.getAllCharacters(allCharacters);
				Dictionary<string, string> NPCDispositions = content.Load<Dictionary<string, string>>("Data\\NPCDispositions");
				foreach (string s in NPCDispositions.Keys)
				{
					bool found = false;
					if ((s == "Kent" && year <= 1) || (s == "Leo" && !MasterPlayer.hasOrWillReceiveMail("addedParrotBoy")))
					{
						continue;
					}
					foreach (NPC n2 in allCharacters)
					{
						if (!n2.isVillager() || !n2.Name.Equals(s))
						{
							continue;
						}
						found = true;
						if ((bool)n2.datable && n2.getSpouse() == null)
						{
							string defaultMap = NPCDispositions[s].Split('/')[10].Split(' ')[0];
							if (n2.DefaultMap != defaultMap && (n2.DefaultMap.ToLower().Contains("cabin") || n2.DefaultMap.Equals("FarmHouse")))
							{
								Console.WriteLine("Fixing " + n2.Name + " who was improperly divorced and left stranded");
								n2.PerformDivorce();
							}
						}
						break;
					}
					if (!found)
					{
						try
						{
							getLocationFromName(NPCDispositions[s].Split('/')[10].Split(' ')[0]).addCharacter(new NPC(new AnimatedSprite("Characters\\" + NPC.getTextureNameForCharacter(s), 0, 16, 32), new Vector2(Convert.ToInt32(NPCDispositions[s].Split('/')[10].Split(' ')[1]) * 64, Convert.ToInt32(NPCDispositions[s].Split('/')[10].Split(' ')[2]) * 64), NPCDispositions[s].Split('/')[10].Split(' ')[0], 0, s, null, content.Load<Texture2D>("Portraits\\" + NPC.getTextureNameForCharacter(s)), eventActor: false));
						}
						catch (Exception)
						{
						}
					}
				}
			}
			finally
			{
				Utility.returnPooledList(allCharacters);
				allCharacters = null;
			}
			int playerCount = getAllFarmers().Count();
			Dictionary<Type, int> missingTools = new Dictionary<Type, int>();
			missingTools.Add(typeof(Axe), playerCount);
			missingTools.Add(typeof(Pickaxe), playerCount);
			missingTools.Add(typeof(Hoe), playerCount);
			missingTools.Add(typeof(WateringCan), playerCount);
			missingTools.Add(typeof(Wand), 0);
			foreach (Farmer allFarmer in getAllFarmers())
			{
				if (allFarmer.hasOrWillReceiveMail("ReturnScepter"))
				{
					missingTools[typeof(Wand)]++;
				}
			}
			int missingScythes = playerCount;
			foreach (Farmer who in getAllFarmers())
			{
				if (who.toolBeingUpgraded.Value != null && missingTools.ContainsKey(who.toolBeingUpgraded.Value.GetType()))
				{
					missingTools[who.toolBeingUpgraded.Value.GetType()]--;
				}
				for (int l = 0; l < who.items.Count; l++)
				{
					if (who.items[l] != null)
					{
						checkIsMissingTool(missingTools, ref missingScythes, who.items[l]);
					}
				}
			}
			bool allFound = true;
			for (int j2 = 0; j2 < missingTools.Count; j2++)
			{
				if (missingTools.ElementAt(j2).Value > 0)
				{
					allFound = false;
					break;
				}
			}
			if (missingScythes > 0)
			{
				allFound = false;
			}
			if (allFound)
			{
				return;
			}
			foreach (GameLocation l2 in locations)
			{
				List<Debris> debrisToDelete = new List<Debris>();
				foreach (Debris d2 in l2.debris)
				{
					Item item2 = d2.item;
					if (item2 == null)
					{
						continue;
					}
					for (int n = 0; n < missingTools.Count; n++)
					{
						if (item2.GetType() == missingTools.ElementAt(n).Key)
						{
							debrisToDelete.Add(d2);
						}
					}
					if (item2 is MeleeWeapon && (item2 as MeleeWeapon).Name.Equals("Scythe"))
					{
						debrisToDelete.Add(d2);
					}
				}
				foreach (Debris d in debrisToDelete)
				{
					l2.debris.Remove(d);
				}
			}
			Utility.iterateChestsAndStorage(delegate(Item item)
			{
				checkIsMissingTool(missingTools, ref missingScythes, item);
			});
			List<string> toAdd = new List<string>();
			for (int m = 0; m < missingTools.Count; m++)
			{
				if (missingTools.ElementAt(m).Value > 0)
				{
					for (int i = 0; i < missingTools.ElementAt(m).Value; i++)
					{
						toAdd.Add(missingTools.ElementAt(m).Key.ToString());
					}
				}
			}
			for (int k = 0; k < missingScythes; k++)
			{
				toAdd.Add("Scythe");
			}
			if (toAdd.Count > 0)
			{
				addMailForTomorrow("foundLostTools");
			}
			for (int j = 0; j < toAdd.Count; j++)
			{
				Item tool = null;
				switch (toAdd[j])
				{
				case "StardewValley.Tools.Axe":
					tool = new Axe();
					break;
				case "StardewValley.Tools.Hoe":
					tool = new Hoe();
					break;
				case "StardewValley.Tools.WateringCan":
					tool = new WateringCan();
					break;
				case "Scythe":
					tool = new MeleeWeapon(47);
					break;
				case "StardewValley.Tools.Pickaxe":
					tool = new Pickaxe();
					break;
				case "StardewValley.Tools.Wand":
					tool = new Wand();
					break;
				}
				if (tool != null)
				{
					if (newDaySync != null)
					{
						player.team.newLostAndFoundItems.Value = true;
					}
					player.team.returnedDonations.Add(tool);
				}
			}
		}

		private static void checkIsMissingTool(Dictionary<Type, int> missingTools, ref int missingScythes, Item item)
		{
			for (int i = 0; i < missingTools.Count; i++)
			{
				if (item.GetType() == missingTools.ElementAt(i).Key)
				{
					missingTools[missingTools.ElementAt(i).Key]--;
				}
			}
			if (item is MeleeWeapon && (item as MeleeWeapon).Name.Equals("Scythe"))
			{
				missingScythes--;
			}
		}

		public static void newDayAfterFade(Action after)
		{
			if (player.currentLocation != null)
			{
				if (player.rightRing.Value != null)
				{
					player.rightRing.Value.onLeaveLocation(player, player.currentLocation);
				}
				if (player.leftRing.Value != null)
				{
					player.leftRing.Value.onLeaveLocation(player, player.currentLocation);
				}
			}
			if (LocalMultiplayer.IsLocalMultiplayer())
			{
				_ = game1;
				hooks.OnGame1_NewDayAfterFade(delegate
				{
					game1.isLocalMultiplayerNewDayActive = true;
					_afterNewDayAction = after;
					GameRunner.instance.activeNewDayProcesses.Add(new KeyValuePair<Game1, IEnumerator<int>>(game1, _newDayAfterFade()));
				});
				return;
			}
			hooks.OnGame1_NewDayAfterFade(delegate
			{
				_afterNewDayAction = after;
				if (_newDayTask != null)
				{
					Console.WriteLine("Warning: There is already a _newDayTask; unusual code path.");
					Console.WriteLine(Environment.StackTrace);
					Console.WriteLine();
				}
				else
				{
					_newDayTask = new Task(delegate
					{
						Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
						IEnumerator<int> enumerator = _newDayAfterFade();
						while (enumerator.MoveNext())
						{
						}
					});
				}
			});
		}

		public static bool CanAcceptDailyQuest()
		{
			if (questOfTheDay == null)
			{
				return false;
			}
			if (player.acceptedDailyQuest.Value)
			{
				return false;
			}
			if (questOfTheDay.questDescription == null || questOfTheDay.questDescription.Length == 0)
			{
				return false;
			}
			return true;
		}

		private static IEnumerator<int> _newDayAfterFade()
		{
			newDaySync.start();
			flushLocationLookup();
			try
			{
				fixProblems();
			}
			catch (Exception)
			{
			}
			foreach (Farmer allFarmer in getAllFarmers())
			{
				allFarmer.FarmerSprite.PauseForSingleAnimation = false;
			}
			whereIsTodaysFest = null;
			if (wind != null)
			{
				wind.Stop(AudioStopOptions.Immediate);
				wind = null;
			}
			foreach (int key in new List<int>(player.chestConsumedMineLevels.Keys))
			{
				if (key > 120)
				{
					player.chestConsumedMineLevels.Remove(key);
				}
			}
			player.currentEyes = 0;
			int seed;
			if (IsMasterGame)
			{
				player.team.announcedSleepingFarmers.Clear();
				seed = (int)uniqueIDForThisGame / 100 + (int)(stats.DaysPlayed * 10) + 1 + (int)stats.StepsTaken;
				newDaySync.sendVar<NetInt, int>("seed", seed);
			}
			else
			{
				while (!newDaySync.isVarReady("seed"))
				{
					yield return 0;
				}
				seed = newDaySync.waitForVar<NetInt, int>("seed");
			}
			random = new Random(seed);
			for (int i = 0; i < dayOfMonth; i++)
			{
				random.Next();
			}
			player.team.endOfNightStatus.UpdateState("sleep");
			newDaySync.barrier("sleep");
			while (!newDaySync.isBarrierReady("sleep"))
			{
				yield return 0;
			}
			gameTimeInterval = 0;
			game1.wasAskedLeoMemory = false;
			player.team.Update();
			player.team.NewDay();
			player.passedOut = false;
			player.CanMove = true;
			player.FarmerSprite.PauseForSingleAnimation = false;
			player.FarmerSprite.StopAnimation();
			player.completelyStopAnimatingOrDoingAction();
			changeMusicTrack("none");
			int dishOfTheDayIndex = random.Next(194, 240);
			while (Utility.getForbiddenDishesOfTheDay().Contains(dishOfTheDayIndex))
			{
				dishOfTheDayIndex = random.Next(194, 240);
			}
			int dish_of_the_day_count = random.Next(1, 4 + ((random.NextDouble() < 0.08) ? 10 : 0));
			if (IsMasterGame)
			{
				dishOfTheDay = new Object(Vector2.Zero, dishOfTheDayIndex, dish_of_the_day_count);
			}
			newDaySync.barrier("dishOfTheDay");
			while (!newDaySync.isBarrierReady("dishOfTheDay"))
			{
				yield return 0;
			}
			npcDialogues = null;
			foreach (NPC allCharacter in Utility.getAllCharacters())
			{
				allCharacter.updatedDialogueYet = false;
			}
			int overnightMinutesElapsed = Utility.CalculateMinutesUntilMorning(timeOfDay);
			foreach (GameLocation n in locations)
			{
				n.currentEvent = null;
				if (IsMasterGame)
				{
					n.passTimeForObjects(overnightMinutesElapsed);
				}
			}
			if (IsMasterGame)
			{
				foreach (Building b in getFarm().buildings)
				{
					if (b.indoors.Value != null)
					{
						b.indoors.Value.passTimeForObjects(overnightMinutesElapsed);
					}
				}
			}
			globalOutdoorLighting = 0f;
			outdoorLight = Color.White;
			ambientLight = Color.White;
			if (isLightning && IsMasterGame)
			{
				Utility.overnightLightning();
			}
			tmpTimeOfDay = timeOfDay;
			if (MasterPlayer.hasOrWillReceiveMail("ccBulletinThankYou") && !player.hasOrWillReceiveMail("ccBulletinThankYou"))
			{
				addMailForTomorrow("ccBulletinThankYou");
			}
			ReceiveMailForTomorrow();
			if (player.friendshipData.Count() > 0)
			{
				string whichFriend = player.friendshipData.Keys.ElementAt(random.Next(player.friendshipData.Keys.Count()));
				if (random.NextDouble() < (double)(player.friendshipData[whichFriend].Points / 250) * 0.1 && (player.spouse == null || !player.spouse.Equals(whichFriend)) && content.Load<Dictionary<string, string>>("Data\\mail").ContainsKey(whichFriend))
				{
					mailbox.Add(whichFriend);
				}
			}
			MineShaft.clearActiveMines();
			VolcanoDungeon.ClearAllLevels();
			for (int l = player.enchantments.Count - 1; l >= 0; l--)
			{
				player.enchantments[l].OnUnequip(player);
			}
			player.dayupdate();
			if (IsMasterGame)
			{
				player.team.sharedDailyLuck.Value = Math.Min(0.10000000149011612, (double)random.Next(-100, 101) / 1000.0);
			}
			dayOfMonth++;
			stats.DaysPlayed++;
			startedJukeboxMusic = false;
			player.dayOfMonthForSaveGame = dayOfMonth;
			player.seasonForSaveGame = Utility.getSeasonNumber(currentSeason);
			player.yearForSaveGame = year;
			player.showToolUpgradeAvailability();
			if (IsMasterGame)
			{
				queueWeddingsForToday();
				newDaySync.sendVar<NetRef<NetLongList>, NetLongList>("weddingsToday", new NetLongList(weddingsToday));
			}
			else
			{
				while (!newDaySync.isVarReady("weddingsToday"))
				{
					yield return 0;
				}
				weddingsToday = new List<long>(newDaySync.waitForVar<NetRef<NetLongList>, NetLongList>("weddingsToday"));
			}
			weddingToday = false;
			foreach (long item4 in weddingsToday)
			{
				Farmer spouse_farmer = getFarmer(item4);
				if (spouse_farmer != null && !spouse_farmer.hasCurrentOrPendingRoommate())
				{
					weddingToday = true;
					break;
				}
			}
			if (player.spouse != null && player.isEngaged() && weddingsToday.Contains(player.UniqueMultiplayerID))
			{
				Friendship friendship = player.friendshipData[player.spouse];
				if (friendship.CountdownToWedding <= 1)
				{
					friendship.Status = FriendshipStatus.Married;
					friendship.WeddingDate = new WorldDate(Date);
					prepareSpouseForWedding(player);
				}
			}
			NetLongDictionary<NetList<Item, NetRef<Item>>, NetRef<NetList<Item, NetRef<Item>>>> additional_shipped_items = new NetLongDictionary<NetList<Item, NetRef<Item>>, NetRef<NetList<Item, NetRef<Item>>>>();
			if (IsMasterGame)
			{
				Utility.ForAllLocations(delegate(GameLocation location)
				{
					foreach (Object current3 in location.objects.Values)
					{
						if (current3 is Chest && current3 is Chest chest && chest.SpecialChestType == Chest.SpecialChestTypes.MiniShippingBin)
						{
							if ((bool)player.team.useSeparateWallets)
							{
								foreach (long current4 in chest.separateWalletItems.Keys)
								{
									if (!additional_shipped_items.ContainsKey(current4))
									{
										additional_shipped_items[current4] = new NetList<Item, NetRef<Item>>();
									}
									List<Item> list = new List<Item>(chest.separateWalletItems[current4]);
									chest.separateWalletItems[current4].Clear();
									foreach (Item current5 in list)
									{
										additional_shipped_items[current4].Add(current5);
									}
								}
							}
							else
							{
								NetCollection<Item> shippingBin2 = getFarm().getShippingBin(player);
								List<Item> list2 = new List<Item>(chest.items);
								chest.items.Clear();
								foreach (Item current6 in list2)
								{
									shippingBin2.Add(current6);
								}
							}
							chest.items.Clear();
							chest.separateWalletItems.Clear();
						}
					}
				});
			}
			if (IsMasterGame)
			{
				newDaySync.sendVar<NetRef<NetLongDictionary<NetList<Item, NetRef<Item>>, NetRef<NetList<Item, NetRef<Item>>>>>, NetLongDictionary<NetList<Item, NetRef<Item>>, NetRef<NetList<Item, NetRef<Item>>>>>("additional_shipped_items", additional_shipped_items);
			}
			else
			{
				while (!newDaySync.isVarReady("additional_shipped_items"))
				{
					yield return 0;
				}
				additional_shipped_items = newDaySync.waitForVar<NetRef<NetLongDictionary<NetList<Item, NetRef<Item>>, NetRef<NetList<Item, NetRef<Item>>>>>, NetLongDictionary<NetList<Item, NetRef<Item>>, NetRef<NetList<Item, NetRef<Item>>>>>("additional_shipped_items");
			}
			if (player.team.useSeparateWallets.Value)
			{
				NetCollection<Item> shipping_bin = getFarm().getShippingBin(player);
				if (additional_shipped_items.ContainsKey(player.UniqueMultiplayerID))
				{
					foreach (Item item2 in additional_shipped_items[player.UniqueMultiplayerID])
					{
						shipping_bin.Add(item2);
					}
				}
			}
			newDaySync.barrier("handleMiniShippingBins");
			while (!newDaySync.isBarrierReady("handleMiniShippingBins"))
			{
				yield return 0;
			}
			NetCollection<Item> shippingBin = getFarm().getShippingBin(player);
			foreach (Item m in shippingBin)
			{
				player.displayedShippedItems.Add(m);
			}
			if (player.useSeparateWallets || (!player.useSeparateWallets && player.IsMainPlayer))
			{
				int total2 = 0;
				foreach (Item item3 in shippingBin)
				{
					int item_value2 = 0;
					if (item3 is Object)
					{
						item_value2 = (item3 as Object).sellToStorePrice(-1L) * item3.Stack;
						total2 += item_value2;
					}
					if (player.team.specialOrders == null)
					{
						continue;
					}
					foreach (SpecialOrder order4 in player.team.specialOrders)
					{
						if (order4.onItemShipped != null)
						{
							order4.onItemShipped(player, item3, item_value2);
						}
					}
				}
				player.Money += total2;
			}
			if (IsMasterGame)
			{
				if (currentSeason.Equals("winter") && dayOfMonth == 18)
				{
					GameLocation source3 = getLocationFromName("Submarine");
					if (source3.objects.Count() >= 0)
					{
						Utility.transferPlacedObjectsFromOneLocationToAnother(source3, null, new Vector2(20f, 20f), getLocationFromName("Beach"));
					}
					source3 = getLocationFromName("MermaidHouse");
					if (source3.objects.Count() >= 0)
					{
						Utility.transferPlacedObjectsFromOneLocationToAnother(source3, null, new Vector2(21f, 20f), getLocationFromName("Beach"));
					}
				}
				if (player.hasOrWillReceiveMail("pamHouseUpgrade") && !player.hasOrWillReceiveMail("transferredObjectsPamHouse"))
				{
					addMailForTomorrow("transferredObjectsPamHouse", noLetter: true);
					GameLocation source2 = getLocationFromName("Trailer");
					GameLocation destination = getLocationFromName("Trailer_Big");
					if (source2.objects.Count() >= 0)
					{
						Utility.transferPlacedObjectsFromOneLocationToAnother(source2, destination, new Vector2(14f, 23f));
					}
				}
				if (Utility.HasAnyPlayerSeenEvent(191393) && !player.hasOrWillReceiveMail("transferredObjectsJojaMart"))
				{
					addMailForTomorrow("transferredObjectsJojaMart", noLetter: true);
					GameLocation source = getLocationFromName("JojaMart");
					if (source.objects.Count() >= 0)
					{
						Utility.transferPlacedObjectsFromOneLocationToAnother(source, null, new Vector2(89f, 51f), getLocationFromName("Town"));
					}
				}
			}
			if (player.useSeparateWallets && player.IsMainPlayer)
			{
				foreach (Farmer who2 in getAllFarmhands())
				{
					if (who2.isActive() || who2.isUnclaimedFarmhand)
					{
						continue;
					}
					int total = 0;
					foreach (Item item in getFarm().getShippingBin(who2))
					{
						int item_value = 0;
						if (item is Object)
						{
							item_value = (item as Object).sellToStorePrice(who2.UniqueMultiplayerID) * item.Stack;
							total += item_value;
						}
						if (player.team.specialOrders == null)
						{
							continue;
						}
						foreach (SpecialOrder order3 in player.team.specialOrders)
						{
							if (order3.onItemShipped != null)
							{
								order3.onItemShipped(player, item, item_value);
							}
						}
					}
					player.team.AddIndividualMoney(who2, total);
					getFarm().getShippingBin(who2).Clear();
				}
			}
			List<NPC> divorceNPCs = new List<NPC>();
			if (IsMasterGame)
			{
				foreach (Farmer who in getAllFarmers())
				{
					if (who.isActive() && (bool)who.divorceTonight && who.getSpouse() != null)
					{
						divorceNPCs.Add(who.getSpouse());
					}
				}
			}
			newDaySync.barrier("player.dayupdate");
			while (!newDaySync.isBarrierReady("player.dayupdate"))
			{
				yield return 0;
			}
			if ((bool)player.divorceTonight)
			{
				player.doDivorce();
			}
			newDaySync.barrier("player.divorce");
			while (!newDaySync.isBarrierReady("player.divorce"))
			{
				yield return 0;
			}
			if (IsMasterGame)
			{
				foreach (NPC npc in divorceNPCs)
				{
					if (npc.getSpouse() == null)
					{
						npc.PerformDivorce();
					}
				}
			}
			newDaySync.barrier("player.finishDivorce");
			while (!newDaySync.isBarrierReady("player.finishDivorce"))
			{
				yield return 0;
			}
			if (IsMasterGame && (bool)player.changeWalletTypeTonight)
			{
				if (player.useSeparateWallets)
				{
					ManorHouse.MergeWallets();
				}
				else
				{
					ManorHouse.SeparateWallets();
				}
			}
			newDaySync.barrier("player.wallets");
			while (!newDaySync.isBarrierReady("player.wallets"))
			{
				yield return 0;
			}
			getFarm().lastItemShipped = null;
			getFarm().getShippingBin(player).Clear();
			newDaySync.barrier("clearShipping");
			while (!newDaySync.isBarrierReady("clearShipping"))
			{
				yield return 0;
			}
			if (IsClient)
			{
				multiplayer.sendFarmhand();
				newDaySync.processMessages();
			}
			newDaySync.barrier("sendFarmhands");
			while (!newDaySync.isBarrierReady("sendFarmhands"))
			{
				yield return 0;
			}
			if (IsMasterGame)
			{
				multiplayer.saveFarmhands();
			}
			newDaySync.barrier("saveFarmhands");
			while (!newDaySync.isBarrierReady("saveFarmhands"))
			{
				yield return 0;
			}
			if (IsMasterGame && dayOfMonth >= 15 && dayOfMonth <= 17 && currentSeason.Equals("winter") && IsMasterGame && netWorldState.Value.VisitsUntilY1Guarantee >= 0)
			{
				netWorldState.Value.VisitsUntilY1Guarantee--;
			}
			if (dayOfMonth == 27 && currentSeason.Equals("spring"))
			{
				_ = year;
				_ = 1;
			}
			if (dayOfMonth == 29)
			{
				newSeason();
				if (!currentSeason.Equals("winter"))
				{
					cropsOfTheWeek = Utility.cropsOfTheWeek();
				}
				if (currentSeason.Equals("spring"))
				{
					year++;
					if (year == 2)
					{
						addKentIfNecessary();
					}
				}
				_ = year;
				_ = 3;
			}
			if (IsMasterGame && (dayOfMonth == 1 || dayOfMonth == 8 || dayOfMonth == 15 || dayOfMonth == 22))
			{
				SpecialOrder.UpdateAvailableSpecialOrders(force_refresh: true);
			}
			if (IsMasterGame)
			{
				netWorldState.Value.UpdateFromGame1();
			}
			newDaySync.barrier("date");
			while (!newDaySync.isBarrierReady("date"))
			{
				yield return 0;
			}
			if (IsMasterGame)
			{
				for (int k = 0; k < player.team.specialOrders.Count; k++)
				{
					SpecialOrder order2 = player.team.specialOrders[k];
					if ((SpecialOrder.QuestState)order2.questState != SpecialOrder.QuestState.Complete && order2.GetDaysLeft() <= 0)
					{
						order2.OnFail();
						player.team.specialOrders.RemoveAt(k);
						k--;
					}
				}
			}
			newDaySync.barrier("processOrders");
			while (!newDaySync.isBarrierReady("processOrders"))
			{
				yield return 0;
			}
			List<string> mail_to_remove_overnight = new List<string>(player.team.mailToRemoveOvernight);
			List<int> items_to_remove_from_world = new List<int>(player.team.itemsToRemoveOvernight);
			if (IsMasterGame)
			{
				foreach (string item5 in player.team.specialRulesRemovedToday)
				{
					SpecialOrder.RemoveSpecialRuleAtEndOfDay(item5);
				}
			}
			player.team.specialRulesRemovedToday.Clear();
			foreach (int index in items_to_remove_from_world)
			{
				if (IsMasterGame)
				{
					game1._PerformRemoveNormalItemFromWorldOvernight(index);
					foreach (Farmer farmer2 in getAllFarmers())
					{
						game1._PerformRemoveNormalItemFromFarmerOvernight(farmer2, index);
					}
				}
				else
				{
					game1._PerformRemoveNormalItemFromFarmerOvernight(player, index);
				}
			}
			foreach (string mail_key in mail_to_remove_overnight)
			{
				if (IsMasterGame)
				{
					foreach (Farmer farmer in getAllFarmers())
					{
						farmer.RemoveMail(mail_key, farmer == MasterPlayer);
					}
				}
				else
				{
					player.RemoveMail(mail_key);
				}
			}
			newDaySync.barrier("removeItemsFromWorld");
			while (!newDaySync.isBarrierReady("removeItemsFromWorld"))
			{
				yield return 0;
			}
			if (IsMasterGame)
			{
				player.team.itemsToRemoveOvernight.Clear();
				player.team.mailToRemoveOvernight.Clear();
			}
			if (content.Load<Dictionary<string, string>>("Data\\mail").ContainsKey(currentSeason + "_" + dayOfMonth + "_" + year))
			{
				mailbox.Add(currentSeason + "_" + dayOfMonth + "_" + year);
			}
			else if (content.Load<Dictionary<string, string>>("Data\\mail").ContainsKey(currentSeason + "_" + dayOfMonth))
			{
				mailbox.Add(currentSeason + "_" + dayOfMonth);
			}
			if (IsMasterGame && player.team.toggleMineShrineOvernight.Value)
			{
				player.team.toggleMineShrineOvernight.Value = false;
				player.team.mineShrineActivated.Value = !player.team.mineShrineActivated.Value;
				if (player.team.mineShrineActivated.Value)
				{
					netWorldState.Value.MinesDifficulty++;
				}
				else
				{
					netWorldState.Value.MinesDifficulty--;
				}
			}
			if (IsMasterGame)
			{
				if (!player.team.SpecialOrderRuleActive("MINE_HARD") && netWorldState.Value.MinesDifficulty > 1)
				{
					netWorldState.Value.MinesDifficulty = 1;
				}
				if (!player.team.SpecialOrderRuleActive("SC_HARD") && netWorldState.Value.SkullCavesDifficulty > 0)
				{
					netWorldState.Value.SkullCavesDifficulty = 0;
				}
			}
			RefreshQuestOfTheDay();
			weatherForTomorrow = getWeatherModificationsForDate(Date, weatherForTomorrow);
			if (weddingToday)
			{
				weatherForTomorrow = 6;
			}
			wasRainingYesterday = isRaining || isLightning;
			if (weatherForTomorrow == 1 || weatherForTomorrow == 3)
			{
				isRaining = true;
			}
			if (weatherForTomorrow == 3)
			{
				isLightning = true;
			}
			if (weatherForTomorrow == 0 || weatherForTomorrow == 2 || weatherForTomorrow == 4 || weatherForTomorrow == 5 || weatherForTomorrow == 6)
			{
				isRaining = false;
				isLightning = false;
				isSnowing = false;
				if (weatherForTomorrow == 5)
				{
					isSnowing = true;
				}
			}
			if (!isRaining && !isLightning)
			{
				currentSongIndex++;
				if (currentSongIndex > 3 || dayOfMonth == 1)
				{
					currentSongIndex = 1;
				}
			}
			if (IsMasterGame)
			{
				game1.SetOtherLocationWeatherForTomorrow(random);
			}
			if ((isRaining || isSnowing || isLightning) && currentLocation.GetLocationContext() == GameLocation.LocationContext.Default)
			{
				changeMusicTrack("none");
			}
			else if (weatherForTomorrow == 4 && weatherForTomorrow == 6)
			{
				changeMusicTrack("none");
			}
			debrisWeather.Clear();
			isDebrisWeather = false;
			if (weatherForTomorrow == 2)
			{
				populateDebrisWeatherArray();
			}
			if (currentSeason.Equals("summer"))
			{
				chanceToRainTomorrow = ((dayOfMonth > 1) ? (0.12 + (double)((float)dayOfMonth * 0.003f)) : 0.0);
			}
			else if (currentSeason.Equals("winter"))
			{
				chanceToRainTomorrow = 0.63;
			}
			else
			{
				chanceToRainTomorrow = 0.183;
			}
			if (random.NextDouble() < chanceToRainTomorrow)
			{
				weatherForTomorrow = 1;
				if ((currentSeason.Equals("summer") && random.NextDouble() < 0.85) || (!currentSeason.Equals("winter") && random.NextDouble() < 0.25 && dayOfMonth > 2 && stats.DaysPlayed > 27))
				{
					weatherForTomorrow = 3;
				}
				if (currentSeason.Equals("winter"))
				{
					weatherForTomorrow = 5;
				}
			}
			else if (stats.DaysPlayed > 2 && ((currentSeason.Equals("spring") && random.NextDouble() < 0.2) || (currentSeason.Equals("fall") && random.NextDouble() < 0.6)) && !weddingToday)
			{
				weatherForTomorrow = 2;
			}
			else
			{
				weatherForTomorrow = 0;
			}
			if (Utility.isFestivalDay(dayOfMonth + 1, currentSeason))
			{
				weatherForTomorrow = 4;
			}
			if (stats.DaysPlayed == 2)
			{
				weatherForTomorrow = 1;
			}
			if (IsMasterGame)
			{
				netWorldState.Value.GetWeatherForLocation(GameLocation.LocationContext.Default).weatherForTomorrow.Value = weatherForTomorrow;
				netWorldState.Value.GetWeatherForLocation(GameLocation.LocationContext.Default).isRaining.Value = isRaining;
				netWorldState.Value.GetWeatherForLocation(GameLocation.LocationContext.Default).isSnowing.Value = isSnowing;
				netWorldState.Value.GetWeatherForLocation(GameLocation.LocationContext.Default).isLightning.Value = isLightning;
				netWorldState.Value.GetWeatherForLocation(GameLocation.LocationContext.Default).isDebrisWeather.Value = isDebrisWeather;
			}
			foreach (NPC n2 in Utility.getAllCharacters())
			{
				player.mailReceived.Remove(n2.Name);
				player.mailReceived.Remove(n2.Name + "Cooking");
				n2.drawOffset.Value = Vector2.Zero;
			}
			FarmAnimal.reservedGrass.Clear();
			if (IsMasterGame)
			{
				NPC.hasSomeoneWateredCrops = (NPC.hasSomeoneFedThePet = (NPC.hasSomeoneFedTheAnimals = (NPC.hasSomeoneRepairedTheFences = false)));
				foreach (GameLocation location3 in locations)
				{
					location3.ResetCharacterDialogues();
					location3.DayUpdate(dayOfMonth);
				}
				UpdateHorseOwnership();
				foreach (NPC allCharacter2 in Utility.getAllCharacters())
				{
					allCharacter2.islandScheduleName.Value = null;
					allCharacter2.currentScheduleDelay = 0f;
				}
				foreach (NPC allCharacter3 in Utility.getAllCharacters())
				{
					allCharacter3.dayUpdate(dayOfMonth);
				}
				IslandSouth.SetupIslandSchedules();
				HashSet<NPC> purchased_item_npcs = new HashSet<NPC>();
				UpdateShopPlayerItemInventory("SeedShop", purchased_item_npcs);
				UpdateShopPlayerItemInventory("FishShop", purchased_item_npcs);
			}
			if (IsMasterGame && (bool)netWorldState.Value.GetWeatherForLocation(GameLocation.LocationContext.Island).isRaining)
			{
				Vector2 tile_location = new Vector2(0f, 0f);
				IslandLocation island_location = null;
				List<int> order = new List<int>();
				for (int j = 0; j < 4; j++)
				{
					order.Add(j);
				}
				Utility.Shuffle(new Random((int)uniqueIDForThisGame), order);
				switch (order[currentGemBirdIndex])
				{
				case 0:
					island_location = getLocationFromName("IslandSouth") as IslandLocation;
					tile_location = new Vector2(10f, 30f);
					break;
				case 1:
					island_location = getLocationFromName("IslandNorth") as IslandLocation;
					tile_location = new Vector2(56f, 56f);
					break;
				case 2:
					island_location = getLocationFromName("Islandwest") as IslandLocation;
					tile_location = new Vector2(53f, 51f);
					break;
				case 3:
					island_location = getLocationFromName("IslandEast") as IslandLocation;
					tile_location = new Vector2(21f, 35f);
					break;
				}
				currentGemBirdIndex = (currentGemBirdIndex + 1) % 4;
				if (island_location != null)
				{
					island_location.locationGemBird.Value = new IslandGemBird(tile_location, IslandGemBird.GetBirdTypeForLocation(island_location.Name));
				}
			}
			if (IsMasterGame)
			{
				foreach (GameLocation location2 in locations)
				{
					if (!IsRainingHere(location2) || !location2.IsOutdoors)
					{
						continue;
					}
					foreach (KeyValuePair<Vector2, TerrainFeature> kvp in location2.terrainFeatures.Pairs)
					{
						if (kvp.Value is HoeDirt && (int)((HoeDirt)kvp.Value).state != 2)
						{
							((HoeDirt)kvp.Value).state.Value = 1;
						}
					}
				}
				GameLocation farm = getLocationFromName("Farm");
				if (IsRainingHere(farm))
				{
					(farm as Farm).petBowlWatered.Value = true;
				}
			}
			if (player.currentUpgrade != null)
			{
				player.currentUpgrade.daysLeftTillUpgradeDone--;
				if (getLocationFromName("Farm").objects.ContainsKey(new Vector2(player.currentUpgrade.positionOfCarpenter.X / 64f, player.currentUpgrade.positionOfCarpenter.Y / 64f)))
				{
					getLocationFromName("Farm").objects.Remove(new Vector2(player.currentUpgrade.positionOfCarpenter.X / 64f, player.currentUpgrade.positionOfCarpenter.Y / 64f));
				}
				if (player.currentUpgrade.daysLeftTillUpgradeDone == 0)
				{
					switch (player.currentUpgrade.whichBuilding)
					{
					case "House":
						player.HouseUpgradeLevel++;
						currentHouseTexture = content.Load<Texture2D>("Buildings\\House" + player.HouseUpgradeLevel);
						break;
					case "Coop":
						player.CoopUpgradeLevel++;
						currentCoopTexture = content.Load<Texture2D>("BuildingUpgrades\\Coop" + player.CoopUpgradeLevel);
						break;
					case "Barn":
						player.BarnUpgradeLevel++;
						currentBarnTexture = content.Load<Texture2D>("BuildingUpgrades\\Barn" + player.BarnUpgradeLevel);
						break;
					case "Greenhouse":
						player.hasGreenhouse = true;
						greenhouseTexture = content.Load<Texture2D>("BuildingUpgrades\\Greenhouse");
						break;
					}
					stats.checkForBuildingUpgradeAchievements();
					removeFrontLayerForFarmBuildings();
					addNewFarmBuildingMaps();
					player.currentUpgrade = null;
					changeInvisibility("Robin", invisibility: false);
				}
				else if (player.currentUpgrade.daysLeftTillUpgradeDone == 3)
				{
					changeInvisibility("Robin", invisibility: true);
				}
			}
			newDaySync.barrier("buildingUpgrades");
			while (!newDaySync.isBarrierReady("buildingUpgrades"))
			{
				yield return 0;
			}
			stats.AverageBedtime = (uint)timeOfDay;
			timeOfDay = 600;
			newDay = false;
			if (IsMasterGame)
			{
				netWorldState.Value.UpdateFromGame1();
			}
			if (player.currentLocation != null)
			{
				player.currentLocation.resetForPlayerEntry();
				BedFurniture.ApplyWakeUpPosition(player);
				forceSnapOnNextViewportUpdate = true;
				UpdateViewPort(overrideFreeze: false, new Point(player.getStandingX(), player.getStandingY()));
				previousViewportPosition = new Vector2(viewport.X, viewport.Y);
			}
			player.sleptInTemporaryBed.Value = false;
			_ = currentWallpaper;
			wallpaperPrice = random.Next(75, 500) + player.HouseUpgradeLevel * 100;
			wallpaperPrice -= wallpaperPrice % 5;
			_ = currentFloor;
			floorPrice = random.Next(75, 500) + player.HouseUpgradeLevel * 100;
			floorPrice -= floorPrice % 5;
			updateWeatherIcon();
			freezeControls = false;
			if (stats.DaysPlayed > 1 || !IsMasterGame)
			{
				farmEvent = null;
				if (IsMasterGame)
				{
					farmEvent = Utility.pickFarmEvent();
					newDaySync.sendVar<NetRef<FarmEvent>, FarmEvent>("farmEvent", farmEvent);
				}
				else
				{
					while (!newDaySync.isVarReady("farmEvent"))
					{
						yield return 0;
					}
					farmEvent = newDaySync.waitForVar<NetRef<FarmEvent>, FarmEvent>("farmEvent");
				}
				if (farmEvent == null)
				{
					farmEvent = Utility.pickPersonalFarmEvent();
				}
				if (farmEvent != null && farmEvent.setUp())
				{
					farmEvent = null;
				}
			}
			if (farmEvent == null)
			{
				RemoveDeliveredMailForTomorrow();
			}
			if (player.team.newLostAndFoundItems.Value)
			{
				morningQueue.Enqueue(delegate
				{
					showGlobalMessage(content.LoadString("Strings\\StringsFromCSFiles:NewLostAndFoundItems"));
				});
			}
			newDaySync.barrier("mail");
			while (!newDaySync.isBarrierReady("mail"))
			{
				yield return 0;
			}
			if (IsMasterGame)
			{
				player.team.newLostAndFoundItems.Value = false;
			}
			foreach (Building building in getFarm().buildings)
			{
				if ((int)building.daysOfConstructionLeft <= 0 && building.indoors.Value is Cabin)
				{
					player.slotCanHost = true;
					break;
				}
			}
			if (Utility.percentGameComplete() >= 1f)
			{
				player.team.farmPerfect.Value = true;
			}
			newDaySync.barrier("checkcompletion");
			while (!newDaySync.isBarrierReady("checkcompletion"))
			{
				yield return 0;
			}
			UpdateFarmPerfection();
			if (farmEvent == null)
			{
				handlePostFarmEventActions();
				showEndOfNightStuff();
			}
			if (server != null)
			{
				server.updateLobbyData();
			}
		}

		public virtual void SetOtherLocationWeatherForTomorrow(Random random)
		{
			netWorldState.Value.GetWeatherForLocation(GameLocation.LocationContext.Island).InitializeDayWeather();
			if (netWorldState.Value.GetWeatherForLocation(GameLocation.LocationContext.Island).weatherForTomorrow.Value == 1)
			{
				netWorldState.Value.GetWeatherForLocation(GameLocation.LocationContext.Island).isRaining.Value = true;
			}
			netWorldState.Value.GetWeatherForLocation(GameLocation.LocationContext.Island).weatherForTomorrow.Value = 0;
			if (random.NextDouble() < 0.24)
			{
				netWorldState.Value.GetWeatherForLocation(GameLocation.LocationContext.Island).weatherForTomorrow.Value = 1;
			}
			if (!Utility.doesAnyFarmerHaveOrWillReceiveMail("Visited_Island"))
			{
				netWorldState.Value.GetWeatherForLocation(GameLocation.LocationContext.Island).weatherForTomorrow.Value = 0;
			}
		}

		public static void UpdateFarmPerfection()
		{
			if (MasterPlayer.mailReceived.Contains("Farm_Eternal") || (!MasterPlayer.hasCompletedCommunityCenter() && !Utility.hasFinishedJojaRoute()) || !player.team.farmPerfect.Value)
			{
				return;
			}
			addMorningFluffFunction(delegate
			{
				changeMusicTrack("none");
				if (IsMasterGame)
				{
					multiplayer.globalChatInfoMessageEvenInSinglePlayer("Eternal1");
				}
				playSound("discoverMineral");
				if (IsMasterGame)
				{
					DelayedAction.functionAfterDelay(delegate
					{
						multiplayer.globalChatInfoMessageEvenInSinglePlayer("Eternal2", MasterPlayer.farmName);
					}, 4000);
				}
				player.mailReceived.Add("Farm_Eternal");
				DelayedAction.functionAfterDelay(delegate
				{
					playSound("thunder_small");
					if (IsMultiplayer)
					{
						if (IsMasterGame)
						{
							multiplayer.globalChatInfoMessage("Eternal3");
						}
					}
					else
					{
						showGlobalMessage(content.LoadString("Strings\\UI:Chat_Eternal3"));
					}
				}, 12000);
			});
		}

		public static bool IsRainingHere(GameLocation location = null)
		{
			if (netWorldState == null)
			{
				return false;
			}
			if (location == null)
			{
				location = currentLocation;
			}
			if (location == null)
			{
				return false;
			}
			return netWorldState.Value.GetWeatherForLocation(location.GetLocationContext()).isRaining;
		}

		public static bool IsLightningHere(GameLocation location = null)
		{
			if (netWorldState == null)
			{
				return false;
			}
			if (location == null)
			{
				location = currentLocation;
			}
			if (location == null)
			{
				return false;
			}
			return netWorldState.Value.GetWeatherForLocation(location.GetLocationContext()).isLightning;
		}

		public static bool IsSnowingHere(GameLocation location = null)
		{
			if (netWorldState == null)
			{
				return false;
			}
			if (location == null)
			{
				location = currentLocation;
			}
			if (location == null)
			{
				return false;
			}
			return netWorldState.Value.GetWeatherForLocation(location.GetLocationContext()).isSnowing;
		}

		public static bool IsDebrisWeatherHere(GameLocation location = null)
		{
			if (netWorldState == null)
			{
				return false;
			}
			if (location == null)
			{
				location = currentLocation;
			}
			if (location == null)
			{
				return false;
			}
			return netWorldState.Value.GetWeatherForLocation(location.GetLocationContext()).isDebrisWeather;
		}

		public static int getWeatherModificationsForDate(WorldDate date, int default_weather)
		{
			int weather = default_weather;
			int day_offset = date.TotalDays - Date.TotalDays;
			if (date.DayOfMonth == 1 || stats.DaysPlayed + day_offset <= 4)
			{
				weather = 0;
			}
			if (stats.DaysPlayed + day_offset == 3)
			{
				weather = 1;
			}
			if (date.Season.Equals("summer") && date.DayOfMonth % 13 == 0)
			{
				weather = 3;
			}
			if (Utility.isFestivalDay(date.DayOfMonth, date.Season))
			{
				weather = 4;
			}
			if (date.Season.Equals("winter") && date.DayOfMonth >= 14 && date.DayOfMonth <= 16)
			{
				weather = 0;
			}
			return weather;
		}

		public static void UpdateShopPlayerItemInventory(string location_name, HashSet<NPC> purchased_item_npcs)
		{
			GameLocation shop = getLocationFromName(location_name);
			if (shop == null)
			{
				return;
			}
			ShopLocation shopLocation = shop as ShopLocation;
			for (int i = shopLocation.itemsFromPlayerToSell.Count - 1; i >= 0; i--)
			{
				for (int j = 0; j < shopLocation.itemsFromPlayerToSell[i].Stack; j++)
				{
					if (random.NextDouble() < 0.04 && shopLocation.itemsFromPlayerToSell[i] is Object && (int)(shopLocation.itemsFromPlayerToSell[i] as Object).edibility != -300)
					{
						NPC k = Utility.getRandomTownNPC();
						if (k.Age != 2 && k.getSpouse() == null)
						{
							if (!purchased_item_npcs.Contains(k))
							{
								k.addExtraDialogues(shopLocation.getPurchasedItemDialogueForNPC(shopLocation.itemsFromPlayerToSell[i] as Object, k));
								purchased_item_npcs.Add(k);
							}
							shopLocation.itemsFromPlayerToSell[i].Stack--;
						}
					}
					else if (random.NextDouble() < 0.15)
					{
						shopLocation.itemsFromPlayerToSell[i].Stack--;
					}
					if (shopLocation.itemsFromPlayerToSell[i].Stack <= 0)
					{
						shopLocation.itemsFromPlayerToSell.RemoveAt(i);
						break;
					}
				}
			}
		}

		private static void handlePostFarmEventActions()
		{
			foreach (GameLocation i in locations)
			{
				if (i is BuildableGameLocation)
				{
					foreach (Building building in (i as BuildableGameLocation).buildings)
					{
						if (building.indoors.Value == null)
						{
							continue;
						}
						foreach (Action postFarmEventOvernightAction in building.indoors.Value.postFarmEventOvernightActions)
						{
							postFarmEventOvernightAction();
						}
						building.indoors.Value.postFarmEventOvernightActions.Clear();
					}
				}
				foreach (Action postFarmEventOvernightAction2 in i.postFarmEventOvernightActions)
				{
					postFarmEventOvernightAction2();
				}
				i.postFarmEventOvernightActions.Clear();
			}
			if (IsMasterGame)
			{
				Mountain mountain = getLocationFromName("Mountain") as Mountain;
				mountain.ApplyTreehouseIfNecessary();
				if (mountain.treehouseDoorDirty)
				{
					mountain.treehouseDoorDirty = false;
					NPC.populateRoutesFromLocationToLocationList();
				}
			}
		}

		public static void ReceiveMailForTomorrow(string mail_to_transfer = null)
		{
			foreach (string s in player.mailForTomorrow)
			{
				if (s == null)
				{
					continue;
				}
				string stripped = s.Replace("%&NL&%", "");
				if (mail_to_transfer != null && mail_to_transfer != s && mail_to_transfer != stripped)
				{
					continue;
				}
				mailDeliveredFromMailForTomorrow.Add(s);
				if (s.Contains("%&NL&%"))
				{
					if (!player.mailReceived.Contains(stripped))
					{
						player.mailReceived.Add(stripped);
					}
				}
				else
				{
					mailbox.Add(s);
				}
			}
		}

		public static void RemoveDeliveredMailForTomorrow()
		{
			ReceiveMailForTomorrow("abandonedJojaMartAccessible");
			foreach (string s in mailDeliveredFromMailForTomorrow)
			{
				if (player.mailForTomorrow.Contains(s))
				{
					player.mailForTomorrow.Remove(s);
				}
			}
			mailDeliveredFromMailForTomorrow.Clear();
		}

		public static void queueWeddingsForToday()
		{
			weddingsToday.Clear();
			weddingToday = false;
			if (!canHaveWeddingOnDay(dayOfMonth, currentSeason))
			{
				return;
			}
			foreach (Farmer farmer2 in from farmer in getOnlineFarmers()
				orderby farmer.UniqueMultiplayerID
				select farmer)
			{
				if (farmer2.spouse != null && farmer2.isEngaged() && farmer2.friendshipData[farmer2.spouse].CountdownToWedding <= 1)
				{
					weddingsToday.Add(farmer2.UniqueMultiplayerID);
				}
				if (!farmer2.team.IsEngaged(farmer2.UniqueMultiplayerID))
				{
					continue;
				}
				long? spouse = farmer2.team.GetSpouse(farmer2.UniqueMultiplayerID);
				if (spouse.HasValue && !weddingsToday.Contains(spouse.Value))
				{
					Farmer spouse_farmer = getFarmerMaybeOffline(spouse.Value);
					if (spouse_farmer != null && getOnlineFarmers().Contains(spouse_farmer) && getOnlineFarmers().Contains(farmer2) && player.team.GetFriendship(farmer2.UniqueMultiplayerID, spouse.Value).CountdownToWedding <= 1)
					{
						weddingsToday.Add(farmer2.UniqueMultiplayerID);
					}
				}
			}
		}

		public static bool PollForEndOfNewDaySync()
		{
			if (!IsMultiplayer)
			{
				newDaySync = null;
				currentLocation.resetForPlayerEntry();
				return true;
			}
			if (newDaySync.readyForFinish())
			{
				if (IsMasterGame && newDaySync != null && !newDaySync.hasFinished())
				{
					newDaySync.finish();
				}
				if (newDaySync != null && newDaySync.hasFinished())
				{
					newDaySync = null;
					currentLocation.resetForPlayerEntry();
					return true;
				}
			}
			return false;
		}

		public static void FinishNewDaySync()
		{
			if (IsMasterGame && newDaySync != null && !newDaySync.hasFinished())
			{
				newDaySync.finish();
			}
			newDaySync = null;
		}

		public static void updateWeatherIcon()
		{
			if (IsSnowingHere())
			{
				weatherIcon = 7;
			}
			else if (IsRainingHere())
			{
				weatherIcon = 4;
			}
			else if (IsDebrisWeatherHere() && currentSeason.Equals("spring"))
			{
				weatherIcon = 3;
			}
			else if (IsDebrisWeatherHere() && currentSeason.Equals("fall"))
			{
				weatherIcon = 6;
			}
			else if (IsDebrisWeatherHere() && currentSeason.Equals("winter"))
			{
				weatherIcon = 7;
			}
			else if (weddingToday)
			{
				weatherIcon = 0;
			}
			else
			{
				weatherIcon = 2;
			}
			if (IsLightningHere())
			{
				weatherIcon = 5;
			}
			if (Utility.isFestivalDay(dayOfMonth, currentSeason))
			{
				weatherIcon = 1;
			}
		}

		public static void showEndOfNightStuff()
		{
			hooks.OnGame1_ShowEndOfNightStuff(delegate
			{
				bool flag = false;
				if (player.displayedShippedItems.Count > 0)
				{
					endOfNightMenus.Push(new ShippingMenu(player.displayedShippedItems));
					player.displayedShippedItems.Clear();
					flag = true;
				}
				bool flag2 = false;
				if (player.newLevels.Count > 0 && !flag)
				{
					endOfNightMenus.Push(new SaveGameMenu());
				}
				for (int num = player.newLevels.Count - 1; num >= 0; num--)
				{
					_ = player.newLevels.Count;
					endOfNightMenus.Push(new LevelUpMenu(player.newLevels[num].X, player.newLevels[num].Y));
					flag2 = true;
				}
				if (flag2)
				{
					playSound("newRecord");
				}
				if (client == null || !client.timedOut)
				{
					if (endOfNightMenus.Count > 0)
					{
						showingEndOfNightStuff = true;
						activeClickableMenu = endOfNightMenus.Pop();
					}
					else
					{
						showingEndOfNightStuff = true;
						activeClickableMenu = new SaveGameMenu();
					}
				}
			});
		}

		private static void updateWallpaperInSeedShop()
		{
			GameLocation seedShop = getLocationFromName("SeedShop");
			for (int i = 9; i < 12; i++)
			{
				seedShop.Map.GetLayer("Back").Tiles[i, 15] = new StaticTile(seedShop.Map.GetLayer("Back"), seedShop.Map.GetTileSheet("Wallpapers"), BlendMode.Alpha, currentWallpaper);
				seedShop.Map.GetLayer("Back").Tiles[i, 16] = new StaticTile(seedShop.Map.GetLayer("Back"), seedShop.Map.GetTileSheet("Wallpapers"), BlendMode.Alpha, currentWallpaper);
			}
		}

		public static void setGraphicsForSeason()
		{
			foreach (GameLocation j in locations)
			{
				string season = GetSeasonForLocation(j);
				j.seasonUpdate(season, onLoad: true);
				j.updateSeasonalTileSheets();
				if (!j.IsOutdoors)
				{
					continue;
				}
				if (season.Equals("spring"))
				{
					foreach (KeyValuePair<Vector2, Object> o4 in j.Objects.Pairs)
					{
						if ((o4.Value.Name.Contains("Stump") || o4.Value.Name.Contains("Boulder") || o4.Value.Name.Equals("Stick") || o4.Value.Name.Equals("Stone")) && o4.Value.ParentSheetIndex >= 378 && o4.Value.ParentSheetIndex <= 391)
						{
							o4.Value.ParentSheetIndex -= 376;
						}
					}
					eveningColor = new Color(255, 255, 0);
				}
				else if (season.Equals("summer"))
				{
					foreach (KeyValuePair<Vector2, Object> o3 in j.Objects.Pairs)
					{
						if (o3.Value.Name.Contains("Weed") && o3.Value.ParentSheetIndex != 882 && o3.Value.ParentSheetIndex != 883 && o3.Value.ParentSheetIndex != 884)
						{
							if ((int)o3.Value.parentSheetIndex == 792)
							{
								o3.Value.ParentSheetIndex++;
							}
							else if (random.NextDouble() < 0.3)
							{
								o3.Value.ParentSheetIndex = 676;
							}
							else if (random.NextDouble() < 0.3)
							{
								o3.Value.ParentSheetIndex = 677;
							}
						}
					}
					eveningColor = new Color(255, 255, 0);
				}
				else if (season.Equals("fall"))
				{
					foreach (KeyValuePair<Vector2, Object> o2 in j.Objects.Pairs)
					{
						if (o2.Value.Name.Contains("Weed") && o2.Value.ParentSheetIndex != 882 && o2.Value.ParentSheetIndex != 883 && o2.Value.ParentSheetIndex != 884)
						{
							if ((int)o2.Value.parentSheetIndex == 793)
							{
								o2.Value.ParentSheetIndex++;
							}
							else if (random.NextDouble() < 0.5)
							{
								o2.Value.ParentSheetIndex = 678;
							}
							else
							{
								o2.Value.ParentSheetIndex = 679;
							}
						}
					}
					eveningColor = new Color(255, 255, 0);
					foreach (WeatherDebris item in debrisWeather)
					{
						item.which = 2;
					}
				}
				else
				{
					if (!season.Equals("winter"))
					{
						continue;
					}
					for (int i = j.Objects.Count() - 1; i >= 0; i--)
					{
						Object o = j.Objects[j.Objects.Keys.ElementAt(i)];
						if (o.Name.Contains("Weed"))
						{
							if (o.ParentSheetIndex != 882 && o.ParentSheetIndex != 883 && o.ParentSheetIndex != 884)
							{
								j.Objects.Remove(j.Objects.Keys.ElementAt(i));
							}
						}
						else if (((!o.Name.Contains("Stump") && !o.Name.Contains("Boulder") && !o.Name.Equals("Stick") && !o.Name.Equals("Stone")) || o.ParentSheetIndex > 100) && j.IsOutdoors && !o.isHoedirt)
						{
							o.name.Equals("HoeDirt");
						}
					}
					foreach (WeatherDebris item2 in debrisWeather)
					{
						item2.which = 3;
					}
					eveningColor = new Color(245, 225, 170);
				}
			}
		}

		private static void updateFloorInSeedShop()
		{
			GameLocation seedShop = getLocationFromName("SeedShop");
			for (int i = 9; i < 12; i++)
			{
				seedShop.Map.GetLayer("Back").Tiles[i, 17] = new StaticTile(seedShop.Map.GetLayer("Back"), seedShop.Map.GetTileSheet("Floors"), BlendMode.Alpha, currentFloor);
				seedShop.Map.GetLayer("Back").Tiles[i, 18] = new StaticTile(seedShop.Map.GetLayer("Back"), seedShop.Map.GetTileSheet("Floors"), BlendMode.Alpha, currentFloor);
			}
		}

		public static void pauseThenMessage(int millisecondsPause, string message, bool showProgressBar)
		{
			messageAfterPause = message;
			pauseTime = millisecondsPause;
			progressBar = showProgressBar;
		}

		public static void updateWallpaperInFarmHouse(int wallpaper)
		{
			GameLocation farmhouse = getLocationFromName("FarmHouse");
			farmhouse.Map.Properties.TryGetValue("Wallpaper", out var wallpaperArea);
			if (wallpaperArea == null)
			{
				return;
			}
			string[] split = wallpaperArea.ToString().Split(' ');
			for (int i = 0; i < split.Length; i += 4)
			{
				int topLeftX = Convert.ToInt32(split[i]);
				int topLeftY = Convert.ToInt32(split[i + 1]);
				int width = Convert.ToInt32(split[i + 2]);
				int height = Convert.ToInt32(split[i + 3]);
				for (int j = topLeftX; j < topLeftX + width; j++)
				{
					for (int k = topLeftY; k < topLeftY + height; k++)
					{
						farmhouse.Map.GetLayer("Back").Tiles[j, k] = new StaticTile(farmhouse.Map.GetLayer("Back"), farmhouse.Map.GetTileSheet("Wallpapers"), BlendMode.Alpha, wallpaper);
					}
				}
			}
		}

		public static void updateFloorInFarmHouse(int floor)
		{
			GameLocation farmhouse = getLocationFromName("FarmHouse");
			farmhouse.Map.Properties.TryGetValue("Floor", out var floorArea);
			if (floorArea == null)
			{
				return;
			}
			string[] split = floorArea.ToString().Split(' ');
			for (int i = 0; i < split.Length; i += 4)
			{
				int topLeftX = Convert.ToInt32(split[i]);
				int topLeftY = Convert.ToInt32(split[i + 1]);
				int width = Convert.ToInt32(split[i + 2]);
				int height = Convert.ToInt32(split[i + 3]);
				for (int j = topLeftX; j < topLeftX + width; j++)
				{
					for (int k = topLeftY; k < topLeftY + height; k++)
					{
						farmhouse.Map.GetLayer("Back").Tiles[j, k] = new StaticTile(farmhouse.Map.GetLayer("Back"), farmhouse.Map.GetTileSheet("Floors"), BlendMode.Alpha, floor);
					}
				}
			}
		}

		public static bool IsVisitingIslandToday(string npc_name)
		{
			return netWorldState.Value.IslandVisitors.ContainsKey(npc_name);
		}

		public static bool shouldTimePass(bool ignore_multiplayer = false)
		{
			if (isFestival())
			{
				return false;
			}
			if (CurrentEvent != null && CurrentEvent.isWedding)
			{
				return false;
			}
			if (farmEvent != null)
			{
				return false;
			}
			if (IsMultiplayer && !ignore_multiplayer)
			{
				return !netWorldState.Value.IsTimePaused;
			}
			if (paused || freezeControls || overlayMenu != null || isTimePaused)
			{
				return false;
			}
			if (eventUp)
			{
				return false;
			}
			if (activeClickableMenu != null && !(activeClickableMenu is BobberBar))
			{
				return false;
			}
			if (!player.CanMove && !player.UsingTool)
			{
				return player.forceTimePass;
			}
			return true;
		}

		public static Farmer getPlayerOrEventFarmer()
		{
			if (eventUp && CurrentEvent != null && !CurrentEvent.isFestival && CurrentEvent.farmer != null)
			{
				return CurrentEvent.farmer;
			}
			return player;
		}

		public static void UpdateViewPort(bool overrideFreeze, Point centerPoint)
		{
			previousViewportPosition.X = viewport.X;
			previousViewportPosition.Y = viewport.Y;
			Farmer farmer = getPlayerOrEventFarmer();
			if (currentLocation == null)
			{
				return;
			}
			if (!viewportFreeze || overrideFreeze)
			{
				bool snapBack = Math.Abs(currentViewportTarget.X + (float)(viewport.Width / 2) - (float)farmer.getStandingX()) > 64f || Math.Abs(currentViewportTarget.Y + (float)(viewport.Height / 2) - (float)farmer.getStandingY()) > 64f;
				if (forceSnapOnNextViewportUpdate)
				{
					snapBack = true;
				}
				if (centerPoint.X >= viewport.Width / 2 && centerPoint.X <= currentLocation.Map.DisplayWidth - viewport.Width / 2)
				{
					if (farmer.isRafting || snapBack)
					{
						currentViewportTarget.X = centerPoint.X - viewport.Width / 2;
					}
					else if (Math.Abs(currentViewportTarget.X - (currentViewportTarget.X = centerPoint.X - viewport.Width / 2)) > farmer.getMovementSpeed())
					{
						currentViewportTarget.X += (float)Math.Sign(currentViewportTarget.X - (currentViewportTarget.X = centerPoint.X - viewport.Width / 2)) * farmer.getMovementSpeed();
					}
				}
				else if (centerPoint.X < viewport.Width / 2 && viewport.Width <= currentLocation.Map.DisplayWidth)
				{
					if (farmer.isRafting || snapBack)
					{
						currentViewportTarget.X = 0f;
					}
					else if (Math.Abs(currentViewportTarget.X - 0f) > farmer.getMovementSpeed())
					{
						currentViewportTarget.X -= (float)Math.Sign(currentViewportTarget.X - 0f) * farmer.getMovementSpeed();
					}
				}
				else if (viewport.Width <= currentLocation.Map.DisplayWidth)
				{
					if (farmer.isRafting || snapBack)
					{
						currentViewportTarget.X = currentLocation.Map.DisplayWidth - viewport.Width;
					}
					else if (Math.Abs(currentViewportTarget.X - (float)(currentLocation.Map.DisplayWidth - viewport.Width)) > farmer.getMovementSpeed())
					{
						currentViewportTarget.X += (float)Math.Sign(currentViewportTarget.X - (float)(currentLocation.Map.DisplayWidth - viewport.Width)) * farmer.getMovementSpeed();
					}
				}
				else if (currentLocation.Map.DisplayWidth < viewport.Width)
				{
					if (farmer.isRafting || snapBack)
					{
						currentViewportTarget.X = (currentLocation.Map.DisplayWidth - viewport.Width) / 2;
					}
					else if (Math.Abs(currentViewportTarget.X - (float)((currentLocation.Map.DisplayWidth - viewport.Width) / 2)) > farmer.getMovementSpeed())
					{
						currentViewportTarget.X -= (float)Math.Sign(currentViewportTarget.X - (float)((currentLocation.Map.DisplayWidth - viewport.Width) / 2)) * farmer.getMovementSpeed();
					}
				}
				if (centerPoint.Y >= viewport.Height / 2 && centerPoint.Y <= currentLocation.Map.DisplayHeight - viewport.Height / 2)
				{
					if (farmer.isRafting || snapBack)
					{
						currentViewportTarget.Y = centerPoint.Y - viewport.Height / 2;
					}
					else if (Math.Abs(currentViewportTarget.Y - (float)(centerPoint.Y - viewport.Height / 2)) >= farmer.getMovementSpeed())
					{
						currentViewportTarget.Y -= (float)Math.Sign(currentViewportTarget.Y - (float)(centerPoint.Y - viewport.Height / 2)) * farmer.getMovementSpeed();
					}
				}
				else if (centerPoint.Y < viewport.Height / 2 && viewport.Height <= currentLocation.Map.DisplayHeight)
				{
					if (farmer.isRafting || snapBack)
					{
						currentViewportTarget.Y = 0f;
					}
					else if (Math.Abs(currentViewportTarget.Y - 0f) > farmer.getMovementSpeed())
					{
						currentViewportTarget.Y -= (float)Math.Sign(currentViewportTarget.Y - 0f) * farmer.getMovementSpeed();
					}
					currentViewportTarget.Y = 0f;
				}
				else if (viewport.Height <= currentLocation.Map.DisplayHeight)
				{
					if (farmer.isRafting || snapBack)
					{
						currentViewportTarget.Y = currentLocation.Map.DisplayHeight - viewport.Height;
					}
					else if (Math.Abs(currentViewportTarget.Y - (float)(currentLocation.Map.DisplayHeight - viewport.Height)) > farmer.getMovementSpeed())
					{
						currentViewportTarget.Y -= (float)Math.Sign(currentViewportTarget.Y - (float)(currentLocation.Map.DisplayHeight - viewport.Height)) * farmer.getMovementSpeed();
					}
				}
				else if (currentLocation.Map.DisplayHeight < viewport.Height)
				{
					if (farmer.isRafting || snapBack)
					{
						currentViewportTarget.Y = (currentLocation.Map.DisplayHeight - viewport.Height) / 2;
					}
					else if (Math.Abs(currentViewportTarget.Y - (float)((currentLocation.Map.DisplayHeight - viewport.Height) / 2)) > farmer.getMovementSpeed())
					{
						currentViewportTarget.Y -= (float)Math.Sign(currentViewportTarget.Y - (float)((currentLocation.Map.DisplayHeight - viewport.Height) / 2)) * farmer.getMovementSpeed();
					}
				}
			}
			if (currentLocation.forceViewportPlayerFollow)
			{
				currentViewportTarget.X = farmer.Position.X - (float)(viewport.Width / 2);
				currentViewportTarget.Y = farmer.Position.Y - (float)(viewport.Height / 2);
			}
			bool force_snap = false;
			if (forceSnapOnNextViewportUpdate)
			{
				force_snap = true;
				forceSnapOnNextViewportUpdate = false;
			}
			if (currentViewportTarget.X != -2.1474836E+09f && (!viewportFreeze || overrideFreeze))
			{
				int difference = (int)(currentViewportTarget.X - (float)viewport.X);
				if (Math.Abs(difference) > 128)
				{
					viewportPositionLerp.X = currentViewportTarget.X;
				}
				else
				{
					viewportPositionLerp.X += (float)difference * farmer.getMovementSpeed() * 0.03f;
				}
				difference = (int)(currentViewportTarget.Y - (float)viewport.Y);
				if (Math.Abs(difference) > 128)
				{
					viewportPositionLerp.Y = (int)currentViewportTarget.Y;
				}
				else
				{
					viewportPositionLerp.Y += (float)difference * farmer.getMovementSpeed() * 0.03f;
				}
				if (force_snap)
				{
					viewportPositionLerp.X = (int)currentViewportTarget.X;
					viewportPositionLerp.Y = (int)currentViewportTarget.Y;
				}
				viewport.X = (int)viewportPositionLerp.X;
				viewport.Y = (int)viewportPositionLerp.Y;
			}
		}

		private void UpdateCharacters(GameTime time)
		{
			if (CurrentEvent != null && CurrentEvent.farmer != null && CurrentEvent.farmer != player)
			{
				CurrentEvent.farmer.Update(time, currentLocation);
			}
			player.Update(time, currentLocation);
			foreach (KeyValuePair<long, Farmer> v in otherFarmers)
			{
				if (v.Key != player.UniqueMultiplayerID)
				{
					v.Value.UpdateIfOtherPlayer(time);
				}
			}
		}

		public static void addMail(string mailName, bool noLetter = false, bool sendToEveryone = false)
		{
			if (sendToEveryone)
			{
				multiplayer.broadcastPartyWideMail(mailName, Multiplayer.PartyWideMessageQueue.SeenMail, noLetter);
				return;
			}
			mailName = mailName.Trim();
			mailName = mailName.Replace(Environment.NewLine, "");
			if (!player.hasOrWillReceiveMail(mailName))
			{
				if (noLetter)
				{
					player.mailReceived.Add(mailName);
				}
				else
				{
					player.mailbox.Add(mailName);
				}
			}
		}

		public static void addMailForTomorrow(string mailName, bool noLetter = false, bool sendToEveryone = false)
		{
			if (sendToEveryone)
			{
				multiplayer.broadcastPartyWideMail(mailName, Multiplayer.PartyWideMessageQueue.MailForTomorrow, noLetter);
				return;
			}
			mailName = mailName.Trim();
			mailName = mailName.Replace(Environment.NewLine, "");
			if (player.hasOrWillReceiveMail(mailName))
			{
				return;
			}
			if (noLetter)
			{
				mailName += "%&NL&%";
			}
			player.mailForTomorrow.Add(mailName);
			if (!sendToEveryone || !IsMultiplayer)
			{
				return;
			}
			foreach (Farmer farmer in otherFarmers.Values)
			{
				if (farmer != player && !player.hasOrWillReceiveMail(mailName))
				{
					farmer.mailForTomorrow.Add(mailName);
				}
			}
		}

		public static void drawDialogue(NPC speaker)
		{
			if (speaker.CurrentDialogue.Count != 0)
			{
				activeClickableMenu = new DialogueBox(speaker.CurrentDialogue.Peek());
				dialogueUp = true;
				if (!eventUp)
				{
					player.Halt();
					player.CanMove = false;
				}
				if (speaker != null)
				{
					currentSpeaker = speaker;
				}
			}
		}

		public static void drawDialogueNoTyping(NPC speaker, string dialogue)
		{
			if (speaker == null)
			{
				currentObjectDialogue.Enqueue(dialogue);
			}
			else if (dialogue != null)
			{
				speaker.CurrentDialogue.Push(new Dialogue(dialogue, speaker));
			}
			activeClickableMenu = new DialogueBox(speaker.CurrentDialogue.Peek());
			dialogueUp = true;
			player.CanMove = false;
			if (speaker != null)
			{
				currentSpeaker = speaker;
			}
		}

		public static void multipleDialogues(string[] messages)
		{
			activeClickableMenu = new DialogueBox(messages.ToList());
			dialogueUp = true;
			player.CanMove = false;
		}

		public static void drawDialogueNoTyping(string dialogue)
		{
			drawObjectDialogue(dialogue);
			if (activeClickableMenu != null && activeClickableMenu is DialogueBox)
			{
				(activeClickableMenu as DialogueBox).finishTyping();
			}
		}

		public static void drawDialogue(NPC speaker, string dialogue)
		{
			speaker.CurrentDialogue.Push(new Dialogue(dialogue, speaker));
			drawDialogue(speaker);
		}

		public static void drawDialogue(NPC speaker, string dialogue, Texture2D overridePortrait)
		{
			speaker.CurrentDialogue.Push(new Dialogue(dialogue, speaker)
			{
				overridePortrait = overridePortrait
			});
			drawDialogue(speaker);
		}

		public static void drawItemNumberSelection(string itemType, int price)
		{
			selectedItemsType = itemType;
			numberOfSelectedItems = 0;
			priceOfSelectedItem = price;
			if (itemType.Equals("calicoJackBet"))
			{
				currentObjectDialogue.Enqueue(content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.2946", player.clubCoins));
			}
			else if (itemType.Equals("flutePitch"))
			{
				currentObjectDialogue.Enqueue(content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.2949"));
				numberOfSelectedItems = (int)currentLocation.actionObjectForQuestionDialogue.scale.X / 100;
			}
			else if (itemType.Equals("drumTone"))
			{
				currentObjectDialogue.Enqueue(content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.2951"));
				numberOfSelectedItems = (int)currentLocation.actionObjectForQuestionDialogue.scale.X;
			}
			else if (itemType.Equals("jukebox"))
			{
				currentObjectDialogue.Enqueue(content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.2953"));
			}
			else if (itemType.Equals("Fuel"))
			{
				drawObjectDialogue(content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.2955"));
			}
			else if (currentSpeaker != null)
			{
				setDialogue(content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.2956"), typing: false);
			}
			else
			{
				currentObjectDialogue.Enqueue(content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.2957"));
			}
		}

		public static void setDialogue(string dialogue, bool typing)
		{
			if (currentSpeaker != null)
			{
				currentSpeaker.CurrentDialogue.Peek().setCurrentDialogue(dialogue);
				if (typing)
				{
					drawDialogue(currentSpeaker);
				}
				else
				{
					drawDialogueNoTyping(currentSpeaker, null);
				}
			}
			else if (typing)
			{
				drawObjectDialogue(dialogue);
			}
			else
			{
				drawDialogueNoTyping(dialogue);
			}
		}

		private static void checkIfDialogueIsQuestion()
		{
			if (currentSpeaker != null && currentSpeaker.CurrentDialogue.Count > 0 && currentSpeaker.CurrentDialogue.Peek().isCurrentDialogueAQuestion())
			{
				questionChoices.Clear();
				isQuestion = true;
				List<NPCDialogueResponse> questions = currentSpeaker.CurrentDialogue.Peek().getNPCResponseOptions();
				for (int i = 0; i < questions.Count; i++)
				{
					questionChoices.Add(questions[i]);
				}
			}
		}

		public static void drawLetterMessage(string message)
		{
			activeClickableMenu = new LetterViewerMenu(message);
		}

		public static void drawObjectDialogue(string dialogue)
		{
			if (activeClickableMenu != null)
			{
				activeClickableMenu.emergencyShutDown();
			}
			activeClickableMenu = new DialogueBox(dialogue);
			player.CanMove = false;
			dialogueUp = true;
		}

		public static void drawObjectQuestionDialogue(string dialogue, List<Response> choices, int width)
		{
			activeClickableMenu = new DialogueBox(dialogue, choices, width);
			dialogueUp = true;
			player.CanMove = false;
		}

		public static void drawObjectQuestionDialogue(string dialogue, List<Response> choices)
		{
			activeClickableMenu = new DialogueBox(dialogue, choices);
			dialogueUp = true;
			player.CanMove = false;
		}

		public static void removeThisCharacterFromAllLocations(NPC toDelete)
		{
			for (int i = 0; i < locations.Count; i++)
			{
				if (locations[i].characters.Contains(toDelete))
				{
					locations[i].characters.Remove(toDelete);
				}
			}
		}

		public static void warpCharacter(NPC character, string targetLocationName, Point position)
		{
			warpCharacter(character, targetLocationName, new Vector2(position.X, position.Y));
		}

		public static void warpCharacter(NPC character, string targetLocationName, Vector2 position)
		{
			warpCharacter(character, getLocationFromName(targetLocationName), position);
		}

		public static void warpCharacter(NPC character, GameLocation targetLocation, Vector2 position)
		{
			if (character.currentLocation == null)
			{
				throw new ArgumentException("In warpCharacter, the character's currentLocation must not be null");
			}
			if (currentSeason.Equals("winter") && dayOfMonth >= 15 && dayOfMonth <= 17 && targetLocation.name.Equals("Beach"))
			{
				targetLocation = getLocationFromName("BeachNightMarket");
			}
			if (targetLocation.name.Equals("Trailer") && MasterPlayer.mailReceived.Contains("pamHouseUpgrade"))
			{
				targetLocation = getLocationFromName("Trailer_Big");
				if (position.X == 12f && position.Y == 9f)
				{
					position.X = 13f;
					position.Y = 24f;
				}
			}
			if (IsClient)
			{
				multiplayer.requestCharacterWarp(character, targetLocation, position);
				return;
			}
			if (!targetLocation.characters.Contains(character))
			{
				character.currentLocation.characters.Remove(character);
				targetLocation.addCharacter(character);
			}
			character.isCharging = false;
			character.speed = 2;
			character.blockedInterval = 0;
			string textureFileName = NPC.getTextureNameForCharacter(character.Name);
			bool load = false;
			if (character.isVillager())
			{
				if (character.Name.Equals("Maru"))
				{
					if (targetLocation.Name.Equals("Hospital"))
					{
						textureFileName = character.Name + "_" + targetLocation.Name;
						load = true;
					}
					else if (!targetLocation.Name.Equals("Hospital") && character.Sprite.textureName.Value != character.Name)
					{
						textureFileName = character.Name;
						load = true;
					}
				}
				else if (character.Name.Equals("Shane"))
				{
					if (targetLocation.Name.Equals("JojaMart"))
					{
						textureFileName = character.Name + "_" + targetLocation.Name;
						load = true;
					}
					else if (!targetLocation.Name.Equals("JojaMart") && character.Sprite.textureName.Value != character.Name)
					{
						textureFileName = character.Name;
						load = true;
					}
				}
			}
			if (load)
			{
				character.Sprite.LoadTexture("Characters\\" + textureFileName);
			}
			character.position.X = position.X * 64f;
			character.position.Y = position.Y * 64f;
			if (character.CurrentDialogue.Count > 0 && character.CurrentDialogue.Peek().removeOnNextMove && !character.getTileLocation().Equals(character.DefaultPosition / 64f))
			{
				character.CurrentDialogue.Pop();
			}
			if (targetLocation is FarmHouse)
			{
				character.arriveAtFarmHouse(targetLocation as FarmHouse);
			}
			else
			{
				character.arriveAt(targetLocation);
			}
			if (character.currentLocation != null && !character.currentLocation.Equals(targetLocation))
			{
				character.currentLocation.characters.Remove(character);
			}
			character.currentLocation = targetLocation;
		}

		public static LocationRequest getLocationRequest(string locationName, bool isStructure = false)
		{
			if (locationName == null)
			{
				throw new ArgumentException();
			}
			return new LocationRequest(locationName, isStructure, getLocationFromName(locationName, isStructure));
		}

		public static void warpHome()
		{
			LocationRequest obj = getLocationRequest(player.homeLocation.Value);
			obj.OnWarp += delegate
			{
				player.position.Set(Utility.PointToVector2((currentLocation as FarmHouse).GetPlayerBedSpot()) * 64f);
			};
			warpFarmer(obj, 5, 9, player.FacingDirection);
		}

		public static void warpFarmer(string locationName, int tileX, int tileY, bool flip)
		{
			warpFarmer(getLocationRequest(locationName), tileX, tileY, flip ? ((player.FacingDirection + 2) % 4) : player.FacingDirection);
		}

		public static void warpFarmer(string locationName, int tileX, int tileY, int facingDirectionAfterWarp)
		{
			warpFarmer(getLocationRequest(locationName), tileX, tileY, facingDirectionAfterWarp);
		}

		public static void warpFarmer(string locationName, int tileX, int tileY, int facingDirectionAfterWarp, bool isStructure)
		{
			warpFarmer(getLocationRequest(locationName, isStructure), tileX, tileY, facingDirectionAfterWarp);
		}

		public virtual bool ShouldDismountOnWarp(Horse mount, GameLocation old_location, GameLocation new_location)
		{
			if (mount == null)
			{
				return false;
			}
			if (currentLocation != null && currentLocation.IsOutdoors && new_location != null)
			{
				return !new_location.IsOutdoors;
			}
			return false;
		}

		public static void warpFarmer(LocationRequest locationRequest, int tileX, int tileY, int facingDirectionAfterWarp)
		{
			int warp_offset_x = nextFarmerWarpOffsetX;
			int warp_offset_y = nextFarmerWarpOffsetY;
			nextFarmerWarpOffsetX = 0;
			nextFarmerWarpOffsetY = 0;
			if (locationRequest.Name.Equals("Beach") && currentSeason.Equals("winter") && dayOfMonth >= 15 && dayOfMonth <= 17 && !eventUp)
			{
				locationRequest = getLocationRequest("BeachNightMarket");
			}
			if (locationRequest.Name.Equals("Farm") && currentLocation.NameOrUniqueName == "Greenhouse")
			{
				bool was_map_exit = false;
				foreach (Warp warp in currentLocation.warps)
				{
					if (warp.TargetX == tileX && warp.TargetY == tileY)
					{
						was_map_exit = true;
						break;
					}
				}
				if (was_map_exit)
				{
					Building target_building = null;
					foreach (Building building in getFarm().buildings)
					{
						if (building is GreenhouseBuilding)
						{
							target_building = building;
							break;
						}
					}
					if (target_building != null)
					{
						tileX = target_building.getPointForHumanDoor().X;
						tileY = target_building.getPointForHumanDoor().Y + 1;
					}
				}
			}
			if (locationRequest.Name == "IslandSouth" && tileX <= 15 && tileY <= 6)
			{
				tileX = 21;
				tileY = 43;
			}
			if (locationRequest.Name.StartsWith("VolcanoDungeon"))
			{
				warp_offset_x = 0;
				warp_offset_y = 0;
			}
			if (player.isRidingHorse() && currentLocation != null)
			{
				GameLocation next_location = locationRequest.Location;
				if (next_location == null)
				{
					next_location = getLocationFromName(locationRequest.Name);
				}
				if (game1.ShouldDismountOnWarp(player.mount, currentLocation, next_location))
				{
					player.mount.dismount();
					warp_offset_x = 0;
					warp_offset_y = 0;
				}
			}
			if (locationRequest.Name.Equals("Trailer") && MasterPlayer.mailReceived.Contains("pamHouseUpgrade"))
			{
				locationRequest = getLocationRequest("Trailer_Big");
				tileX = 13;
				tileY = 24;
			}
			if (locationRequest.Name.Equals("Farm"))
			{
				Farm farm = getFarm();
				if (currentLocation.NameOrUniqueName == "FarmCave" && tileX == 34 && tileY == 6)
				{
					if (whichFarm == 6)
					{
						tileX = 34;
						tileY = 16;
					}
					else if (whichFarm == 5)
					{
						tileX = 30;
						tileY = 36;
					}
					Point exit_location5 = farm.GetMapPropertyPosition("FarmCaveEntry", tileX, tileY);
					tileX = exit_location5.X;
					tileY = exit_location5.Y;
				}
				else if (currentLocation.NameOrUniqueName == "Forest" && tileX == 41 && tileY == 64)
				{
					if (whichFarm == 6)
					{
						tileX = 82;
						tileY = 103;
					}
					else if (whichFarm == 5)
					{
						tileX = 40;
						tileY = 64;
					}
					Point exit_location4 = farm.GetMapPropertyPosition("ForestEntry", tileX, tileY);
					tileX = exit_location4.X;
					tileY = exit_location4.Y;
				}
				else if (currentLocation.NameOrUniqueName == "BusStop" && tileX == 79 && tileY == 17)
				{
					Point exit_location3 = farm.GetMapPropertyPosition("BusStopEntry", tileX, tileY);
					tileX = exit_location3.X;
					tileY = exit_location3.Y;
				}
				else if (currentLocation.NameOrUniqueName == "Backwoods" && tileX == 40 && tileY == 0)
				{
					Point exit_location2 = farm.GetMapPropertyPosition("BackwoodsEntry", tileX, tileY);
					tileX = exit_location2.X;
					tileY = exit_location2.Y;
				}
				else if (currentLocation.NameOrUniqueName == "FarmHouse" && tileX == 64 && tileY == 15)
				{
					Point exit_location = farm.GetMainFarmHouseEntry();
					tileX = exit_location.X;
					tileY = exit_location.Y;
				}
			}
			if (locationRequest.Name.Equals("Club") && !player.hasClubCard)
			{
				locationRequest = getLocationRequest("SandyHouse");
				locationRequest.OnWarp += delegate
				{
					NPC characterFromName = currentLocation.getCharacterFromName("Bouncer");
					if (characterFromName != null)
					{
						Vector2 vector = new Vector2(17f, 4f);
						characterFromName.showTextAboveHead(content.LoadString("Strings\\Locations:Club_Bouncer_TextAboveHead" + (random.Next(2) + 1)));
						int num = random.Next();
						currentLocation.playSound("thudStep");
						multiplayer.broadcastSprites(currentLocation, new TemporaryAnimatedSprite(288, 100f, 1, 24, vector * 64f, flicker: true, flipped: false, currentLocation, player)
						{
							shakeIntensity = 0.5f,
							shakeIntensityChange = 0.002f,
							extraInfoForEndBehavior = num,
							endFunction = currentLocation.removeTemporarySpritesWithID
						}, new TemporaryAnimatedSprite("LooseSprites\\Cursors", new Microsoft.Xna.Framework.Rectangle(598, 1279, 3, 4), 53f, 5, 9, vector * 64f + new Vector2(5f, 0f) * 4f, flicker: true, flipped: false, 0.0263f, 0f, Color.Yellow, 4f, 0f, 0f, 0f)
						{
							id = num
						}, new TemporaryAnimatedSprite("LooseSprites\\Cursors", new Microsoft.Xna.Framework.Rectangle(598, 1279, 3, 4), 53f, 5, 9, vector * 64f + new Vector2(5f, 0f) * 4f, flicker: true, flipped: true, 0.0263f, 0f, Color.Orange, 4f, 0f, 0f, 0f)
						{
							delayBeforeAnimationStart = 100,
							id = num
						}, new TemporaryAnimatedSprite("LooseSprites\\Cursors", new Microsoft.Xna.Framework.Rectangle(598, 1279, 3, 4), 53f, 5, 9, vector * 64f + new Vector2(5f, 0f) * 4f, flicker: true, flipped: false, 0.0263f, 0f, Color.White, 3f, 0f, 0f, 0f)
						{
							delayBeforeAnimationStart = 200,
							id = num
						});
						currentLocation.netAudio.StartPlaying("fuse");
					}
				};
				tileX = 17;
				tileY = 4;
			}
			if (weatherIcon == 1 && whereIsTodaysFest != null && locationRequest.Name.Equals(whereIsTodaysFest) && !warpingForForcedRemoteEvent && timeOfDay <= Convert.ToInt32(temporaryContent.Load<Dictionary<string, string>>("Data\\Festivals\\" + currentSeason + dayOfMonth)["conditions"].Split('/')[1].Split(' ')[1]))
			{
				if (timeOfDay < Convert.ToInt32(temporaryContent.Load<Dictionary<string, string>>("Data\\Festivals\\" + currentSeason + dayOfMonth)["conditions"].Split('/')[1].Split(' ')[0]))
				{
					if (!currentLocation.Name.Equals("Hospital"))
					{
						player.Position = player.lastPosition;
						drawObjectDialogue(content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.2973"));
						return;
					}
					locationRequest = getLocationRequest("BusStop");
					tileX = 34;
					tileY = 23;
				}
				else
				{
					if (IsMultiplayer)
					{
						player.team.SetLocalReady("festivalStart", ready: true);
						activeClickableMenu = new ReadyCheckDialog("festivalStart", allowCancel: true, delegate
						{
							exitActiveMenu();
							if (player.mount != null)
							{
								player.mount.dismount();
								warp_offset_x = 0;
								warp_offset_y = 0;
							}
							performWarpFarmer(locationRequest, tileX, tileY, facingDirectionAfterWarp);
						});
						return;
					}
					if (player.mount != null)
					{
						player.mount.dismount();
						warp_offset_x = 0;
						warp_offset_y = 0;
					}
				}
			}
			tileX += warp_offset_x;
			tileY += warp_offset_y;
			performWarpFarmer(locationRequest, tileX, tileY, facingDirectionAfterWarp);
		}

		private static void performWarpFarmer(LocationRequest locationRequest, int tileX, int tileY, int facingDirectionAfterWarp)
		{
			if ((currentLocation.Name.Equals("Town") || jukeboxPlaying) && getLocationFromName(locationRequest.Name).IsOutdoors && currentSong != null && (currentSong.Name.Contains("town") || jukeboxPlaying))
			{
				changeMusicTrack("none");
			}
			if (locationRequest.Location != null)
			{
				if (tileX >= locationRequest.Location.Map.Layers[0].LayerWidth - 1)
				{
					tileX--;
				}
				if (IsMasterGame)
				{
					locationRequest.Location.hostSetup();
				}
			}
			Console.WriteLine("Warping to " + locationRequest.Name);
			if (player.IsSitting())
			{
				player.StopSitting(animate: false);
			}
			player.previousLocationName = ((player.currentLocation != null) ? ((string)player.currentLocation.name) : "");
			Game1.locationRequest = locationRequest;
			xLocationAfterWarp = tileX;
			yLocationAfterWarp = tileY;
			_isWarping = true;
			Game1.facingDirectionAfterWarp = facingDirectionAfterWarp;
			fadeScreenToBlack();
			setRichPresence("location", locationRequest.Name);
		}

		public static void requestLocationInfoFromServer()
		{
			if (locationRequest != null)
			{
				client.sendMessage(5, (short)xLocationAfterWarp, (short)yLocationAfterWarp, locationRequest.Name, (byte)(locationRequest.IsStructure ? 1 : 0));
			}
			currentLocation = null;
			player.Position = new Vector2(xLocationAfterWarp * 64, yLocationAfterWarp * 64 - (player.Sprite.getHeight() - 32) + 16);
			player.faceDirection(facingDirectionAfterWarp);
		}

		public static void changeInvisibility(string name, bool invisibility)
		{
			getCharacterFromName(name).IsInvisible = invisibility;
		}

		public static T getCharacterFromName<T>(string name, bool mustBeVillager = true) where T : NPC
		{
			if (currentLocation != null)
			{
				foreach (NPC ch2 in currentLocation.getCharacters())
				{
					if (ch2 is T && ch2.Name.Equals(name) && (!mustBeVillager || ch2.isVillager()))
					{
						return (T)ch2;
					}
				}
			}
			for (int i = 0; i < locations.Count; i++)
			{
				foreach (NPC ch in locations[i].getCharacters())
				{
					if (ch is T && !(locations[i] is MovieTheater) && ch.Name.Equals(name) && (!mustBeVillager || ch.isVillager()))
					{
						return (T)ch;
					}
				}
			}
			if (getFarm() != null)
			{
				foreach (Building b in getFarm().buildings)
				{
					if (b.indoors.Value == null)
					{
						continue;
					}
					foreach (NPC j in b.indoors.Value.characters)
					{
						if (j is T && j.Name.Equals(name) && (!mustBeVillager || j.isVillager()))
						{
							return (T)j;
						}
					}
				}
			}
			return null;
		}

		public static NPC getCharacterFromName(string name, bool mustBeVillager = true, bool useLocationsListOnly = false)
		{
			if (!useLocationsListOnly && currentLocation != null && !(currentLocation is MovieTheater))
			{
				foreach (NPC ch2 in currentLocation.getCharacters())
				{
					if (!ch2.eventActor && ch2.Name.Equals(name) && (!mustBeVillager || ch2.isVillager()))
					{
						return ch2;
					}
				}
			}
			for (int i = 0; i < locations.Count; i++)
			{
				if (locations[i] is MovieTheater)
				{
					continue;
				}
				foreach (NPC ch in locations[i].getCharacters())
				{
					if (!ch.eventActor && ch.Name.Equals(name) && (!mustBeVillager || ch.isVillager()))
					{
						return ch;
					}
				}
			}
			if (getFarm() != null)
			{
				foreach (Building b in getFarm().buildings)
				{
					if (b.indoors.Value == null)
					{
						continue;
					}
					foreach (NPC j in b.indoors.Value.characters)
					{
						if (j.Name.Equals(name) && (!mustBeVillager || j.isVillager()))
						{
							return j;
						}
					}
				}
			}
			return null;
		}

		public static NPC removeCharacterFromItsLocation(string name, bool must_be_villager = true)
		{
			if (!IsMasterGame)
			{
				return null;
			}
			for (int i = 0; i < locations.Count; i++)
			{
				if (locations[i] is MovieTheater)
				{
					continue;
				}
				for (int j = 0; j < locations[i].getCharacters().Count; j++)
				{
					if (locations[i].getCharacters()[j].Name.Equals(name) && (!must_be_villager || locations[i].getCharacters()[j].isVillager()))
					{
						NPC result = locations[i].characters[j];
						locations[i].characters.RemoveAt(j);
						return result;
					}
				}
			}
			return null;
		}

		public static GameLocation getLocationFromName(string name)
		{
			return getLocationFromName(name, isStructure: false);
		}

		public static GameLocation getLocationFromName(string name, bool isStructure)
		{
			if (string.IsNullOrEmpty(name))
			{
				return null;
			}
			if (currentLocation != null)
			{
				if (!isStructure && string.Equals(currentLocation.name, name, StringComparison.OrdinalIgnoreCase))
				{
					return currentLocation;
				}
				if (!isStructure && (bool)currentLocation.isStructure && currentLocation.Root != null && string.Equals(currentLocation.Root.Value.name, name, StringComparison.OrdinalIgnoreCase))
				{
					return currentLocation.Root.Value;
				}
				if (isStructure && currentLocation.uniqueName == name)
				{
					return currentLocation;
				}
			}
			if (_locationLookup.TryGetValue(name, out var cached_location))
			{
				return cached_location;
			}
			return getLocationFromNameInLocationsList(name, isStructure);
		}

		public static GameLocation getLocationFromNameInLocationsList(string name, bool isStructure = false)
		{
			for (int i = 0; i < locations.Count; i++)
			{
				if (!isStructure)
				{
					if (string.Equals(locations[i].Name, name, StringComparison.OrdinalIgnoreCase))
					{
						_locationLookup[locations[i].Name] = locations[i];
						return locations[i];
					}
					continue;
				}
				GameLocation buildingIndoors = findStructure(locations[i], name);
				if (buildingIndoors != null)
				{
					_locationLookup[name] = buildingIndoors;
					return buildingIndoors;
				}
			}
			if (name.StartsWith("UndergroundMine", StringComparison.OrdinalIgnoreCase))
			{
				return MineShaft.GetMine(name);
			}
			if (name.StartsWith("VolcanoDungeon", StringComparison.OrdinalIgnoreCase))
			{
				return VolcanoDungeon.GetLevel(name);
			}
			if (!isStructure)
			{
				return getLocationFromName(name, isStructure: true);
			}
			return null;
		}

		public static void flushLocationLookup()
		{
			_locationLookup.Clear();
		}

		public static void removeLocationFromLocationLookup(string name_or_unique_name)
		{
			List<string> keys_to_remove = new List<string>();
			foreach (string key2 in _locationLookup.Keys)
			{
				if (_locationLookup[key2].NameOrUniqueName == name_or_unique_name)
				{
					keys_to_remove.Add(key2);
				}
			}
			foreach (string key in keys_to_remove)
			{
				_locationLookup.Remove(key);
			}
		}

		public static void removeLocationFromLocationLookup(GameLocation location)
		{
			List<string> keys_to_remove = new List<string>();
			foreach (string key2 in _locationLookup.Keys)
			{
				if (_locationLookup[key2] == location)
				{
					keys_to_remove.Add(key2);
				}
			}
			foreach (string key in keys_to_remove)
			{
				_locationLookup.Remove(key);
			}
		}

		public static GameLocation findStructure(GameLocation parentLocation, string name)
		{
			if (!(parentLocation is BuildableGameLocation))
			{
				return null;
			}
			foreach (Building building in (parentLocation as BuildableGameLocation).buildings)
			{
				if (building.indoors.Value != null && building.indoors.Value.uniqueName.Equals(name))
				{
					return building.indoors;
				}
			}
			return null;
		}

		public static void addNewFarmBuildingMaps()
		{
			if (player.CoopUpgradeLevel >= 1 && getLocationFromName("Coop") == null)
			{
				locations.Add(new GameLocation("Maps\\Coop" + player.CoopUpgradeLevel, "Coop"));
				getLocationFromName("Farm").setTileProperty(21, 10, "Buildings", "Action", "Warp 2 9 Coop");
				currentCoopTexture = content.Load<Texture2D>("BuildingUpgrades\\Coop" + player.coopUpgradeLevel);
			}
			else if (getLocationFromName("Coop") != null)
			{
				getLocationFromName("Coop").map = content.Load<Map>("Maps\\Coop" + player.CoopUpgradeLevel);
				currentCoopTexture = content.Load<Texture2D>("BuildingUpgrades\\Coop" + player.coopUpgradeLevel);
			}
			if (player.BarnUpgradeLevel >= 1 && getLocationFromName("Barn") == null)
			{
				locations.Add(new GameLocation("Maps\\Barn" + player.BarnUpgradeLevel, "Barn"));
				getLocationFromName("Farm").warps.Add(new Warp(14, 9, "Barn", 11, 14, flipFarmer: false));
				currentBarnTexture = content.Load<Texture2D>("BuildingUpgrades\\Barn" + player.barnUpgradeLevel);
			}
			else if (getLocationFromName("Barn") != null)
			{
				getLocationFromName("Barn").map = content.Load<Map>("Maps\\Barn" + player.BarnUpgradeLevel);
				currentBarnTexture = content.Load<Texture2D>("BuildingUpgrades\\Barn" + player.barnUpgradeLevel);
			}
			FarmHouse home = Utility.getHomeOfFarmer(player);
			if (player.HouseUpgradeLevel >= 1 && home.Map.Id.Equals("FarmHouse"))
			{
				home.updateMap();
				int num = currentWallpaper;
				int curFloor = currentFloor;
				currentWallpaper = farmerWallpaper;
				currentFloor = FarmerFloor;
				updateFloorInFarmHouse(currentFloor);
				updateWallpaperInFarmHouse(currentWallpaper);
				currentWallpaper = num;
				currentFloor = curFloor;
			}
			if (player.hasGreenhouse && getLocationFromName("FarmGreenHouse") == null)
			{
				locations.Add(new GameLocation("Maps\\FarmGreenHouse", "FarmGreenHouse"));
				getLocationFromName("Farm").setTileProperty(3, 10, "Buildings", "Action", "Warp 5 15 FarmGreenHouse");
				greenhouseTexture = content.Load<Texture2D>("BuildingUpgrades\\Greenhouse");
			}
		}

		public static bool waitingToPassOut()
		{
			if (activeClickableMenu is ReadyCheckDialog && (activeClickableMenu as ReadyCheckDialog).checkName == "sleep")
			{
				return !(activeClickableMenu as ReadyCheckDialog).isCancelable();
			}
			return false;
		}

		public static void PassOutNewDay()
		{
			player.lastSleepLocation.Value = currentLocation.NameOrUniqueName;
			player.lastSleepPoint.Value = player.getTileLocationPoint();
			if (!IsMultiplayer)
			{
				NewDay(0f);
				return;
			}
			player.FarmerSprite.setCurrentSingleFrame(5, 3000);
			player.FarmerSprite.PauseForSingleAnimation = true;
			player.passedOut = true;
			if (activeClickableMenu != null)
			{
				activeClickableMenu.emergencyShutDown();
				exitActiveMenu();
			}
			activeClickableMenu = new ReadyCheckDialog("sleep", allowCancel: false, delegate
			{
				NewDay(0f);
			});
		}

		public static void NewDay(float timeToPause)
		{
			currentMinigame = null;
			newDay = true;
			newDaySync = new NewDaySynchronizer();
			if ((bool)player.isInBed || player.passedOut)
			{
				nonWarpFade = true;
				screenFade.FadeScreenToBlack(player.passedOut ? 1.1f : 0f);
				player.Halt();
				player.currentEyes = 1;
				player.blinkTimer = -4000;
				player.CanMove = false;
				player.passedOut = false;
				pauseTime = timeToPause;
			}
			if (activeClickableMenu != null && !dialogueUp)
			{
				activeClickableMenu.emergencyShutDown();
				exitActiveMenu();
			}
		}

		public static void screenGlowOnce(Color glowColor, bool hold, float rate = 0.005f, float maxAlpha = 0.3f)
		{
			screenGlowMax = maxAlpha;
			screenGlowRate = rate;
			screenGlowAlpha = 0f;
			screenGlowUp = true;
			screenGlowColor = glowColor;
			screenGlow = true;
			screenGlowHold = hold;
		}

		public static void removeTilesFromLayer(GameLocation l, string layer, Microsoft.Xna.Framework.Rectangle area)
		{
			for (int i = area.X; i < area.Right; i++)
			{
				for (int j = area.Y; j < area.Bottom; j++)
				{
					l.Map.GetLayer(layer).Tiles[i, j] = null;
				}
			}
		}

		public static void removeFrontLayerForFarmBuildings()
		{
		}

		public static string shortDayNameFromDayOfSeason(int dayOfSeason)
		{
			return (dayOfSeason % 7) switch
			{
				0 => "Sun", 
				1 => "Mon", 
				2 => "Tue", 
				3 => "Wed", 
				4 => "Thu", 
				5 => "Fri", 
				6 => "Sat", 
				_ => "", 
			};
		}

		public static string shortDayDisplayNameFromDayOfSeason(int dayOfSeason)
		{
			if (dayOfSeason < 0)
			{
				return string.Empty;
			}
			return _shortDayDisplayName[dayOfSeason % 7];
		}

		public static void showNameSelectScreen(string type)
		{
			nameSelectType = type;
			nameSelectUp = true;
		}

		public static void nameSelectionDone()
		{
		}

		public static void tryToBuySelectedItems()
		{
			if (selectedItemsType.Equals("flutePitch"))
			{
				currentObjectDialogue.Clear();
				currentLocation.actionObjectForQuestionDialogue.scale.X = numberOfSelectedItems * 100;
				dialogueUp = false;
				player.CanMove = true;
				numberOfSelectedItems = -1;
			}
			else if (selectedItemsType.Equals("drumTone"))
			{
				currentObjectDialogue.Clear();
				currentLocation.actionObjectForQuestionDialogue.scale.X = numberOfSelectedItems;
				dialogueUp = false;
				player.CanMove = true;
				numberOfSelectedItems = -1;
			}
			else if (selectedItemsType.Equals("jukebox"))
			{
				changeMusicTrack(player.songsHeard.ElementAt(numberOfSelectedItems));
				dialogueUp = false;
				player.CanMove = true;
				numberOfSelectedItems = -1;
			}
			else if (player.Money >= priceOfSelectedItem * numberOfSelectedItems && numberOfSelectedItems > 0)
			{
				bool success = true;
				switch (selectedItemsType)
				{
				case "Animal Food":
					player.Feed += numberOfSelectedItems;
					setDialogue(content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.3072"), typing: false);
					break;
				case "Fuel":
					((Lantern)player.getToolFromName("Lantern")).fuelLeft += numberOfSelectedItems;
					break;
				case "Star Token":
					player.festivalScore += numberOfSelectedItems;
					dialogueUp = false;
					player.canMove = true;
					break;
				}
				if (success)
				{
					player.Money -= priceOfSelectedItem * numberOfSelectedItems;
					numberOfSelectedItems = -1;
					playSound("purchase");
				}
			}
			else if (player.Money < priceOfSelectedItem * numberOfSelectedItems)
			{
				currentObjectDialogue.Dequeue();
				setDialogue(content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.3078"), typing: false);
				numberOfSelectedItems = -1;
			}
		}

		public static void throwActiveObjectDown()
		{
			player.CanMove = false;
			switch (player.FacingDirection)
			{
			case 0:
				((FarmerSprite)player.Sprite).animateBackwardsOnce(80, 50f);
				break;
			case 1:
				((FarmerSprite)player.Sprite).animateBackwardsOnce(72, 50f);
				break;
			case 2:
				((FarmerSprite)player.Sprite).animateBackwardsOnce(64, 50f);
				break;
			case 3:
				((FarmerSprite)player.Sprite).animateBackwardsOnce(88, 50f);
				break;
			}
			player.reduceActiveItemByOne();
			playSound("throwDownITem");
		}

		public static void runTestEvent()
		{
			StreamReader file = new StreamReader("test_event.txt");
			string? text = file.ReadLine();
			string event_string = file.ReadToEnd();
			event_string = event_string.Replace("\r\n", "/");
			Console.WriteLine(event_string);
			LocationRequest location_request = getLocationRequest(text);
			location_request.OnWarp += delegate
			{
				currentLocation.currentEvent = new Event(event_string);
				currentLocation.checkForEvents();
			};
			int x = 8;
			int y = 8;
			Utility.getDefaultWarpLocation(text, ref x, ref y);
			warpFarmer(location_request, x, y, player.FacingDirection);
		}

		public static bool isMusicContextActiveButNotPlaying(MusicContext music_context = MusicContext.Default)
		{
			if (_activeMusicContext != music_context)
			{
				return false;
			}
			if (getMusicTrackName() == "none")
			{
				return true;
			}
			if (currentSong != null && currentSong.Name == getMusicTrackName() && !currentSong.IsPlaying)
			{
				return true;
			}
			return false;
		}

		public static bool IsMusicContextActive(MusicContext music_context = MusicContext.Default)
		{
			if (_activeMusicContext != music_context)
			{
				return true;
			}
			return false;
		}

		public static bool doesMusicContextHaveTrack(MusicContext music_context = MusicContext.Default)
		{
			return _requestedMusicTracks.ContainsKey(music_context);
		}

		public static string getMusicTrackName(MusicContext music_context = MusicContext.Default)
		{
			if (_requestedMusicTracks.ContainsKey(music_context))
			{
				return _requestedMusicTracks[music_context].Key;
			}
			return "none";
		}

		public static void stopMusicTrack(MusicContext music_context)
		{
			if (_requestedMusicTracks.ContainsKey(music_context))
			{
				_requestedMusicTracks.Remove(music_context);
				UpdateRequestedMusicTrack();
			}
		}

		public static void changeMusicTrack(string newTrackName, bool track_interruptable = false, MusicContext music_context = MusicContext.Default)
		{
			if (music_context == MusicContext.Default && morningSongPlayAction != null)
			{
				if (delayedActions.Contains(morningSongPlayAction))
				{
					delayedActions.Remove(morningSongPlayAction);
				}
				morningSongPlayAction = null;
			}
			if (music_context != MusicContext.ImportantSplitScreenMusic && !player.songsHeard.Contains(newTrackName))
			{
				Utility.farmerHeardSong(newTrackName);
			}
			_requestedMusicTracks[music_context] = new KeyValuePair<string, bool>(newTrackName, track_interruptable);
			UpdateRequestedMusicTrack();
		}

		public static void UpdateRequestedMusicTrack()
		{
			_activeMusicContext = MusicContext.Default;
			KeyValuePair<string, bool> requested_track_data = new KeyValuePair<string, bool>("none", value: true);
			for (int i = 0; i < 5; i++)
			{
				if (_requestedMusicTracks.ContainsKey((MusicContext)i))
				{
					if (i != 4)
					{
						_activeMusicContext = (MusicContext)i;
					}
					requested_track_data = _requestedMusicTracks[(MusicContext)i];
				}
			}
			if (requested_track_data.Key != requestedMusicTrack || requested_track_data.Value != requestedMusicTrackOverrideable)
			{
				requestedMusicDirty = true;
				requestedMusicTrack = requested_track_data.Key;
				requestedMusicTrackOverrideable = requested_track_data.Value;
			}
		}

		public static void enterMine(int whatLevel)
		{
			inMine = true;
			warpFarmer("UndergroundMine" + whatLevel, 6, 6, 2);
		}

		public static string GetSeasonForLocation(GameLocation location)
		{
			if (location != null)
			{
				if (location.Name == "Greenhouse")
				{
					return "spring";
				}
				return location.GetSeasonForLocation();
			}
			return currentSeason;
		}

		public static void getSteamAchievement(string which)
		{
			if (which.Equals("0"))
			{
				which = "a0";
			}
			Program.sdk.GetAchievement(which);
		}

		public static void getAchievement(int which, bool allowBroadcasting = true)
		{
			if (player.achievements.Contains(which) || gameMode != 3)
			{
				return;
			}
			Dictionary<int, string> achievementData = content.Load<Dictionary<int, string>>("Data\\Achievements");
			if (!achievementData.ContainsKey(which))
			{
				return;
			}
			string achievementName = achievementData[which].Split('^')[0];
			player.achievements.Add(which);
			if (which < 32 && allowBroadcasting)
			{
				if (stats.isSharedAchievement(which))
				{
					multiplayer.sendSharedAchievementMessage(which);
				}
				else
				{
					string farmerName = player.Name;
					if (farmerName == "")
					{
						farmerName = content.LoadString("Strings\\UI:Chat_PlayerJoinedNewName");
					}
					multiplayer.globalChatInfoMessage("Achievement", farmerName, "achievement:" + which);
				}
			}
			playSound("achievement");
			Program.sdk.GetAchievement(which.ToString() ?? "");
			addHUDMessage(new HUDMessage(achievementName, achievement: true));
			if (!player.hasOrWillReceiveMail("hatter"))
			{
				addMailForTomorrow("hatter");
			}
		}

		public static void createMultipleObjectDebris(int index, int xTile, int yTile, int number)
		{
			for (int i = 0; i < number; i++)
			{
				createObjectDebris(index, xTile, yTile);
			}
		}

		public static void createMultipleObjectDebris(int index, int xTile, int yTile, int number, GameLocation location)
		{
			for (int i = 0; i < number; i++)
			{
				createObjectDebris(index, xTile, yTile, -1, 0, 1f, location);
			}
		}

		public static void createMultipleObjectDebris(int index, int xTile, int yTile, int number, float velocityMultiplier)
		{
			for (int i = 0; i < number; i++)
			{
				createObjectDebris(index, xTile, yTile, -1, 0, velocityMultiplier);
			}
		}

		public static void createMultipleObjectDebris(int index, int xTile, int yTile, int number, long who)
		{
			for (int i = 0; i < number; i++)
			{
				createObjectDebris(index, xTile, yTile, who);
			}
		}

		public static void createMultipleObjectDebris(int index, int xTile, int yTile, int number, long who, GameLocation location)
		{
			for (int i = 0; i < number; i++)
			{
				createObjectDebris(index, xTile, yTile, who, location);
			}
		}

		public static void createDebris(int debrisType, int xTile, int yTile, int numberOfChunks)
		{
			createDebris(debrisType, xTile, yTile, numberOfChunks, currentLocation);
		}

		public static void createDebris(int debrisType, int xTile, int yTile, int numberOfChunks, GameLocation location)
		{
			if (location == null)
			{
				location = currentLocation;
			}
			location.debris.Add(new Debris(debrisType, numberOfChunks, new Vector2(xTile * 64 + 32, yTile * 64 + 32), new Vector2(player.getStandingX(), player.getStandingY())));
		}

		public static Debris createItemDebris(Item item, Vector2 origin, int direction, GameLocation location = null, int groundLevel = -1)
		{
			if (location == null)
			{
				location = currentLocation;
			}
			Vector2 targetLocation = new Vector2(origin.X, origin.Y);
			switch (direction)
			{
			case 0:
				origin.X -= 32f;
				origin.Y -= 128 + recentMultiplayerRandom.Next(32);
				targetLocation.Y -= 192f;
				break;
			case 1:
				origin.X += 42f;
				origin.Y -= 32 - recentMultiplayerRandom.Next(8);
				targetLocation.X += 256f;
				break;
			case 2:
				origin.X -= 32f;
				origin.Y += recentMultiplayerRandom.Next(32);
				targetLocation.Y += 96f;
				break;
			case 3:
				origin.X -= 64f;
				origin.Y -= 32 - recentMultiplayerRandom.Next(8);
				targetLocation.X -= 256f;
				break;
			case -1:
				targetLocation = player.getStandingPosition();
				break;
			}
			Debris d = new Debris(item, origin, targetLocation);
			if (groundLevel != -1)
			{
				d.chunkFinalYLevel = groundLevel;
			}
			location.debris.Add(d);
			return d;
		}

		public static void createRadialDebris(GameLocation location, int debrisType, int xTile, int yTile, int numberOfChunks, bool resource, int groundLevel = -1, bool item = false, int color = -1)
		{
			if (groundLevel == -1)
			{
				groundLevel = yTile * 64 + 32;
			}
			Vector2 debrisOrigin = new Vector2(xTile * 64 + 64, yTile * 64 + 64);
			if (item)
			{
				while (numberOfChunks > 0)
				{
					switch (random.Next(4))
					{
					case 0:
						location.debris.Add(new Debris(new Object(Vector2.Zero, debrisType, 1), debrisOrigin, debrisOrigin + new Vector2(-64f, 0f)));
						break;
					case 1:
						location.debris.Add(new Debris(new Object(Vector2.Zero, debrisType, 1), debrisOrigin, debrisOrigin + new Vector2(64f, 0f)));
						break;
					case 2:
						location.debris.Add(new Debris(new Object(Vector2.Zero, debrisType, 1), debrisOrigin, debrisOrigin + new Vector2(0f, 64f)));
						break;
					case 3:
						location.debris.Add(new Debris(new Object(Vector2.Zero, debrisType, 1), debrisOrigin, debrisOrigin + new Vector2(0f, -64f)));
						break;
					}
					numberOfChunks--;
				}
			}
			if (resource)
			{
				location.debris.Add(new Debris(debrisType, numberOfChunks / 4, debrisOrigin, debrisOrigin + new Vector2(-64f, 0f)));
				numberOfChunks++;
				location.debris.Add(new Debris(debrisType, numberOfChunks / 4, debrisOrigin, debrisOrigin + new Vector2(64f, 0f)));
				numberOfChunks++;
				location.debris.Add(new Debris(debrisType, numberOfChunks / 4, debrisOrigin, debrisOrigin + new Vector2(0f, -64f)));
				numberOfChunks++;
				location.debris.Add(new Debris(debrisType, numberOfChunks / 4, debrisOrigin, debrisOrigin + new Vector2(0f, 64f)));
			}
			else
			{
				location.debris.Add(new Debris(debrisType, numberOfChunks / 4, debrisOrigin, debrisOrigin + new Vector2(-64f, 0f), groundLevel, color));
				numberOfChunks++;
				location.debris.Add(new Debris(debrisType, numberOfChunks / 4, debrisOrigin, debrisOrigin + new Vector2(64f, 0f), groundLevel, color));
				numberOfChunks++;
				location.debris.Add(new Debris(debrisType, numberOfChunks / 4, debrisOrigin, debrisOrigin + new Vector2(0f, -64f), groundLevel, color));
				numberOfChunks++;
				location.debris.Add(new Debris(debrisType, numberOfChunks / 4, debrisOrigin, debrisOrigin + new Vector2(0f, 64f), groundLevel, color));
			}
		}

		public static void createRadialDebris(GameLocation location, string texture, Microsoft.Xna.Framework.Rectangle sourcerectangle, int xTile, int yTile, int numberOfChunks)
		{
			createRadialDebris(location, texture, sourcerectangle, xTile, yTile, numberOfChunks, yTile);
		}

		public static void createWaterDroplets(string texture, Microsoft.Xna.Framework.Rectangle sourcerectangle, int xPosition, int yPosition, int numberOfChunks, int groundLevelTile)
		{
			Vector2 debrisOrigin = new Vector2(xPosition, yPosition);
			currentLocation.debris.Add(new Debris(texture, sourcerectangle, numberOfChunks / 4, debrisOrigin, debrisOrigin + new Vector2(-64f, 0f), groundLevelTile * 64));
			currentLocation.debris.Add(new Debris(texture, sourcerectangle, numberOfChunks / 4, debrisOrigin, debrisOrigin + new Vector2(64f, 0f), groundLevelTile * 64));
			currentLocation.debris.Add(new Debris(texture, sourcerectangle, numberOfChunks / 4, debrisOrigin, debrisOrigin + new Vector2(0f, -64f), groundLevelTile * 64));
			currentLocation.debris.Add(new Debris(texture, sourcerectangle, numberOfChunks / 4, debrisOrigin, debrisOrigin + new Vector2(0f, 64f), groundLevelTile * 64));
		}

		public static void createRadialDebris(GameLocation location, string texture, Microsoft.Xna.Framework.Rectangle sourcerectangle, int xTile, int yTile, int numberOfChunks, int groundLevelTile)
		{
			createRadialDebris(location, texture, sourcerectangle, 8, xTile * 64 + 32 + random.Next(32), yTile * 64 + 32 + random.Next(32), numberOfChunks, groundLevelTile);
		}

		public static void createRadialDebris(GameLocation location, string texture, Microsoft.Xna.Framework.Rectangle sourcerectangle, int sizeOfSourceRectSquares, int xPosition, int yPosition, int numberOfChunks, int groundLevelTile)
		{
			Vector2 debrisOrigin = new Vector2(xPosition, yPosition);
			location.debris.Add(new Debris(texture, sourcerectangle, numberOfChunks / 4, debrisOrigin, debrisOrigin + new Vector2(-64f, 0f), groundLevelTile * 64, sizeOfSourceRectSquares));
			location.debris.Add(new Debris(texture, sourcerectangle, numberOfChunks / 4, debrisOrigin, debrisOrigin + new Vector2(64f, 0f), groundLevelTile * 64, sizeOfSourceRectSquares));
			location.debris.Add(new Debris(texture, sourcerectangle, numberOfChunks / 4, debrisOrigin, debrisOrigin + new Vector2(0f, -64f), groundLevelTile * 64, sizeOfSourceRectSquares));
			location.debris.Add(new Debris(texture, sourcerectangle, numberOfChunks / 4, debrisOrigin, debrisOrigin + new Vector2(0f, 64f), groundLevelTile * 64, sizeOfSourceRectSquares));
		}

		public static void createRadialDebris(GameLocation location, string texture, Microsoft.Xna.Framework.Rectangle sourcerectangle, int sizeOfSourceRectSquares, int xPosition, int yPosition, int numberOfChunks, int groundLevelTile, Color color)
		{
			createRadialDebris(location, texture, sourcerectangle, sizeOfSourceRectSquares, xPosition, yPosition, numberOfChunks, groundLevelTile, color, 1f);
		}

		public static void createRadialDebris(GameLocation location, string texture, Microsoft.Xna.Framework.Rectangle sourcerectangle, int sizeOfSourceRectSquares, int xPosition, int yPosition, int numberOfChunks, int groundLevelTile, Color color, float scale)
		{
			Vector2 debrisOrigin = new Vector2(xPosition, yPosition);
			while (numberOfChunks > 0)
			{
				switch (random.Next(4))
				{
				case 0:
				{
					Debris d = new Debris(texture, sourcerectangle, 1, debrisOrigin, debrisOrigin + new Vector2(-64f, 0f), groundLevelTile * 64, sizeOfSourceRectSquares);
					d.nonSpriteChunkColor.Value = color;
					location?.debris.Add(d);
					d.Chunks[0].scale = scale;
					break;
				}
				case 1:
				{
					Debris d = new Debris(texture, sourcerectangle, 1, debrisOrigin, debrisOrigin + new Vector2(64f, 0f), groundLevelTile * 64, sizeOfSourceRectSquares);
					d.nonSpriteChunkColor.Value = color;
					location?.debris.Add(d);
					d.Chunks[0].scale = scale;
					break;
				}
				case 2:
				{
					Debris d = new Debris(texture, sourcerectangle, 1, debrisOrigin, debrisOrigin + new Vector2(random.Next(-64, 64), -64f), groundLevelTile * 64, sizeOfSourceRectSquares);
					d.nonSpriteChunkColor.Value = color;
					location?.debris.Add(d);
					d.Chunks[0].scale = scale;
					break;
				}
				case 3:
				{
					Debris d = new Debris(texture, sourcerectangle, 1, debrisOrigin, debrisOrigin + new Vector2(random.Next(-64, 64), 64f), groundLevelTile * 64, sizeOfSourceRectSquares);
					d.nonSpriteChunkColor.Value = color;
					location?.debris.Add(d);
					d.Chunks[0].scale = scale;
					break;
				}
				}
				numberOfChunks--;
			}
		}

		public static void createObjectDebris(int objectIndex, int xTile, int yTile, long whichPlayer)
		{
			currentLocation.debris.Add(new Debris(objectIndex, new Vector2(xTile * 64 + 32, yTile * 64 + 32), getFarmer(whichPlayer).getStandingPosition()));
		}

		public static void createObjectDebris(int objectIndex, int xTile, int yTile, long whichPlayer, GameLocation location)
		{
			location.debris.Add(new Debris(objectIndex, new Vector2(xTile * 64 + 32, yTile * 64 + 32), getFarmer(whichPlayer).getStandingPosition()));
		}

		public static void createObjectDebris(int objectIndex, int xTile, int yTile, GameLocation location)
		{
			createObjectDebris(objectIndex, xTile, yTile, -1, 0, 1f, location);
		}

		public static void createObjectDebris(int objectIndex, int xTile, int yTile, int groundLevel = -1, int itemQuality = 0, float velocityMultiplyer = 1f, GameLocation location = null)
		{
			if (location == null)
			{
				location = currentLocation;
			}
			Debris d = new Debris(objectIndex, new Vector2(xTile * 64 + 32, yTile * 64 + 32), new Vector2(player.getStandingX(), player.getStandingY()))
			{
				itemQuality = itemQuality
			};
			foreach (Chunk chunk in d.Chunks)
			{
				chunk.xVelocity.Value *= velocityMultiplyer;
				chunk.yVelocity.Value *= velocityMultiplyer;
			}
			if (groundLevel != -1)
			{
				d.chunkFinalYLevel = groundLevel;
			}
			location.debris.Add(d);
		}

		public static Farmer getFarmer(long id)
		{
			if (player.UniqueMultiplayerID == id)
			{
				return player;
			}
			foreach (Farmer f in otherFarmers.Values)
			{
				if (f.UniqueMultiplayerID == id)
				{
					return f;
				}
			}
			if (!IsMultiplayer)
			{
				return player;
			}
			return MasterPlayer;
		}

		public static Farmer getFarmerMaybeOffline(long id)
		{
			foreach (Farmer f in getAllFarmers())
			{
				if (f.UniqueMultiplayerID == id)
				{
					return f;
				}
			}
			return null;
		}

		public static IEnumerable<Farmer> getAllFarmers()
		{
			return Enumerable.Repeat(MasterPlayer, 1).Concat(getAllFarmhands());
		}

		public static IEnumerable<Farmer> getAllFarmhands()
		{
			if (getFarm() == null)
			{
				yield break;
			}
			foreach (Building building in getFarm().buildings)
			{
				if (!(building.indoors.Value is Cabin))
				{
					continue;
				}
				Farmer farmhand = (building.indoors.Value as Cabin).farmhand.Value;
				if (farmhand != null)
				{
					if (farmhand.isActive())
					{
						farmhand = otherFarmers[farmhand.UniqueMultiplayerID];
					}
					yield return farmhand;
				}
			}
		}

		public static FarmerCollection getOnlineFarmers()
		{
			return _onlineFarmers;
		}

		public static void farmerFindsArtifact(int objectIndex)
		{
			player.addItemToInventoryBool(new Object(objectIndex, 1));
		}

		public static bool doesHUDMessageExist(string s)
		{
			for (int i = 0; i < hudMessages.Count; i++)
			{
				if (s.Equals(hudMessages[i].message))
				{
					return true;
				}
			}
			return false;
		}

		public static void addHUDMessage(HUDMessage message)
		{
			if (message.type != null || message.whatType != 0)
			{
				for (int j = 0; j < hudMessages.Count; j++)
				{
					if (message.type != null && hudMessages[j].type != null && hudMessages[j].type.Equals(message.type) && hudMessages[j].add == message.add)
					{
						hudMessages[j].number = (message.add ? (hudMessages[j].number + message.number) : (hudMessages[j].number - message.number));
						hudMessages[j].timeLeft = 3500f;
						hudMessages[j].transparency = 1f;
						return;
					}
					if (message.whatType == hudMessages[j].whatType && message.whatType != 1 && message.message != null && message.message.Equals(hudMessages[j].message))
					{
						hudMessages[j].timeLeft = message.timeLeft;
						hudMessages[j].transparency = 1f;
						return;
					}
				}
			}
			hudMessages.Add(message);
			for (int i = hudMessages.Count - 1; i >= 0; i--)
			{
				if (hudMessages[i].noIcon)
				{
					HUDMessage tmp = hudMessages[i];
					hudMessages.RemoveAt(i);
					hudMessages.Add(tmp);
				}
			}
		}

		public static void nextMineLevel()
		{
			warpFarmer("UndergroundMine" + (CurrentMineLevel + 1), 16, 16, flip: false);
		}

		public static void showSwordswipeAnimation(int direction, Vector2 source, float animationSpeed, bool flip)
		{
			switch (direction)
			{
			case 0:
				currentLocation.TemporarySprites.Add(new TemporaryAnimatedSprite(-1, animationSpeed, 5, 1, new Vector2(source.X + 32f, source.Y), flicker: false, flipped: false, !flip, -(float)Math.PI / 2f));
				break;
			case 1:
				currentLocation.TemporarySprites.Add(new TemporaryAnimatedSprite(-1, animationSpeed, 5, 1, new Vector2(source.X + 96f + 16f, source.Y + 48f), flicker: false, flip, verticalFlipped: false, flip ? (-(float)Math.PI) : 0f));
				break;
			case 2:
				currentLocation.TemporarySprites.Add(new TemporaryAnimatedSprite(-1, animationSpeed, 5, 1, new Vector2(source.X + 32f, source.Y + 128f), flicker: false, flipped: false, !flip, (float)Math.PI / 2f));
				break;
			case 3:
				currentLocation.TemporarySprites.Add(new TemporaryAnimatedSprite(-1, animationSpeed, 5, 1, new Vector2(source.X - 32f - 16f, source.Y + 48f), flicker: false, !flip, verticalFlipped: false, flip ? (-(float)Math.PI) : 0f));
				break;
			}
		}

		public static void removeSquareDebrisFromTile(int tileX, int tileY)
		{
			currentLocation.debris.Filter((Debris debris) => (Debris.DebrisType)debris.debrisType != Debris.DebrisType.SQUARES || (int)(debris.Chunks[0].position.X / 64f) != tileX || debris.chunkFinalYLevel / 64 != tileY);
		}

		public static void removeDebris(Debris.DebrisType type)
		{
			currentLocation.debris.Filter((Debris debris) => (Debris.DebrisType)debris.debrisType != type);
		}

		public static void toolAnimationDone(Farmer who)
		{
			float oldStamina = player.Stamina;
			if (who.CurrentTool == null)
			{
				return;
			}
			if (who.Stamina > 0f)
			{
				int powerupLevel = (int)((toolHold + 20f) / 600f) + 1;
				Vector2 actionTile = who.GetToolLocation();
				if (who.CurrentTool is FishingRod && ((FishingRod)who.CurrentTool).isFishing)
				{
					who.canReleaseTool = false;
				}
				else if (!(who.CurrentTool is FishingRod))
				{
					who.UsingTool = false;
					if (who.CurrentTool.Name.Contains("Seeds"))
					{
						if (!eventUp)
						{
							who.CurrentTool.DoFunction(currentLocation, who.getStandingX(), who.getStandingY(), powerupLevel, who);
							if (((Seeds)who.CurrentTool).NumberInStack <= 0)
							{
								who.removeItemFromInventory(who.CurrentTool);
							}
						}
					}
					else if (who.CurrentTool.Name.Equals("Watering Can"))
					{
						switch (who.FacingDirection)
						{
						case 0:
						case 2:
							who.CurrentTool.DoFunction(currentLocation, (int)actionTile.X, (int)actionTile.Y, powerupLevel, who);
							break;
						case 1:
						case 3:
							who.CurrentTool.DoFunction(currentLocation, (int)actionTile.X, (int)actionTile.Y, powerupLevel, who);
							break;
						}
					}
					else if (who.CurrentTool is MeleeWeapon)
					{
						who.CurrentTool.CurrentParentTileIndex = who.CurrentTool.IndexOfMenuItemView;
					}
					else
					{
						if (who.CurrentTool.Name.Equals("Wand"))
						{
							who.CurrentTool.CurrentParentTileIndex = who.CurrentTool.IndexOfMenuItemView;
						}
						who.CurrentTool.DoFunction(currentLocation, (int)actionTile.X, (int)actionTile.Y, powerupLevel, who);
					}
				}
				else
				{
					who.UsingTool = false;
				}
			}
			else if ((bool)who.CurrentTool.instantUse)
			{
				who.CurrentTool.DoFunction(currentLocation, 0, 0, 0, who);
			}
			else
			{
				who.UsingTool = false;
			}
			who.lastClick = Vector2.Zero;
			toolHold = 0f;
			if (who.IsLocalPlayer && !GetKeyboardState().IsKeyDown(Keys.LeftShift))
			{
				who.setRunning(options.autoRun);
			}
			if (!who.UsingTool && who.FarmerSprite.PauseForSingleAnimation)
			{
				who.FarmerSprite.StopAnimation();
			}
			if (player.Stamina <= 0f && oldStamina > 0f)
			{
				player.doEmote(36);
			}
		}

		public static bool pressActionButton(KeyboardState currentKBState, MouseState currentMouseState, GamePadState currentPadState)
		{
			if (IsChatting)
			{
				currentKBState = default(KeyboardState);
			}
			if (dialogueTyping)
			{
				bool consume = true;
				dialogueTyping = false;
				if (currentSpeaker != null)
				{
					currentDialogueCharacterIndex = currentSpeaker.CurrentDialogue.Peek().getCurrentDialogue().Length;
				}
				else if (currentObjectDialogue.Count > 0)
				{
					currentDialogueCharacterIndex = currentObjectDialogue.Peek().Length;
				}
				else
				{
					consume = false;
				}
				dialogueTypingInterval = 0;
				oldKBState = currentKBState;
				oldMouseState = input.GetMouseState();
				oldPadState = currentPadState;
				if (consume)
				{
					playSound("dialogueCharacterClose");
					return false;
				}
			}
			if (dialogueUp && numberOfSelectedItems == -1)
			{
				if (isQuestion)
				{
					isQuestion = false;
					if (currentSpeaker != null)
					{
						if (currentSpeaker.CurrentDialogue.Peek().chooseResponse(questionChoices[currentQuestionChoice]))
						{
							currentDialogueCharacterIndex = 1;
							dialogueTyping = true;
							oldKBState = currentKBState;
							oldMouseState = input.GetMouseState();
							oldPadState = currentPadState;
							return false;
						}
					}
					else
					{
						dialogueUp = false;
						if (eventUp && currentLocation.afterQuestion == null)
						{
							currentLocation.currentEvent.answerDialogue(currentLocation.lastQuestionKey, currentQuestionChoice);
							currentQuestionChoice = 0;
							oldKBState = currentKBState;
							oldMouseState = input.GetMouseState();
							oldPadState = currentPadState;
						}
						else if (currentLocation.answerDialogue(questionChoices[currentQuestionChoice]))
						{
							currentQuestionChoice = 0;
							oldKBState = currentKBState;
							oldMouseState = input.GetMouseState();
							oldPadState = currentPadState;
							return false;
						}
						if (dialogueUp)
						{
							currentDialogueCharacterIndex = 1;
							dialogueTyping = true;
							oldKBState = currentKBState;
							oldMouseState = input.GetMouseState();
							oldPadState = currentPadState;
							return false;
						}
					}
					currentQuestionChoice = 0;
				}
				string exitDialogue = null;
				if (currentSpeaker != null)
				{
					if (currentSpeaker.immediateSpeak)
					{
						currentSpeaker.immediateSpeak = false;
						return false;
					}
					exitDialogue = ((currentSpeaker.CurrentDialogue.Count > 0) ? currentSpeaker.CurrentDialogue.Peek().exitCurrentDialogue() : null);
				}
				if (exitDialogue == null)
				{
					if (currentSpeaker != null && currentSpeaker.CurrentDialogue.Count > 0 && currentSpeaker.CurrentDialogue.Peek().isOnFinalDialogue() && currentSpeaker.CurrentDialogue.Count > 0)
					{
						currentSpeaker.CurrentDialogue.Pop();
					}
					dialogueUp = false;
					if (messagePause)
					{
						pauseTime = 500f;
					}
					if (currentObjectDialogue.Count > 0)
					{
						currentObjectDialogue.Dequeue();
					}
					currentDialogueCharacterIndex = 0;
					if (currentObjectDialogue.Count > 0)
					{
						dialogueUp = true;
						questionChoices.Clear();
						oldKBState = currentKBState;
						oldMouseState = input.GetMouseState();
						oldPadState = currentPadState;
						dialogueTyping = true;
						return false;
					}
					tvStation = -1;
					if (currentSpeaker != null && !currentSpeaker.Name.Equals("Gunther") && !eventUp && !currentSpeaker.doingEndOfRouteAnimation)
					{
						currentSpeaker.doneFacingPlayer(player);
					}
					currentSpeaker = null;
					if (!eventUp)
					{
						player.CanMove = true;
					}
					else if (currentLocation.currentEvent.CurrentCommand > 0 || currentLocation.currentEvent.specialEventVariable1)
					{
						if (!isFestival() || !currentLocation.currentEvent.canMoveAfterDialogue())
						{
							currentLocation.currentEvent.CurrentCommand++;
						}
						else
						{
							player.CanMove = true;
						}
					}
					questionChoices.Clear();
					playSound("smallSelect");
				}
				else
				{
					playSound("smallSelect");
					currentDialogueCharacterIndex = 0;
					dialogueTyping = true;
					checkIfDialogueIsQuestion();
				}
				oldKBState = currentKBState;
				oldMouseState = input.GetMouseState();
				oldPadState = currentPadState;
				if (questOfTheDay != null && (bool)questOfTheDay.accepted && questOfTheDay is SocializeQuest)
				{
					((SocializeQuest)questOfTheDay).checkIfComplete(null, -1, -1);
				}
				_ = afterDialogues;
				return false;
			}
			if (currentBillboard != 0)
			{
				currentBillboard = 0;
				player.CanMove = true;
				oldKBState = currentKBState;
				oldMouseState = input.GetMouseState();
				oldPadState = currentPadState;
				return false;
			}
			if (!player.UsingTool && !pickingTool && !menuUp && (!eventUp || (currentLocation.currentEvent != null && currentLocation.currentEvent.playerControlSequence)) && !nameSelectUp && numberOfSelectedItems == -1 && !fadeToBlack)
			{
				if (wasMouseVisibleThisFrame && currentLocation is IAnimalLocation)
				{
					Vector2 mousePosition = new Vector2(getOldMouseX() + viewport.X, getOldMouseY() + viewport.Y);
					if (Utility.withinRadiusOfPlayer((int)mousePosition.X, (int)mousePosition.Y, 1, player))
					{
						if ((currentLocation as IAnimalLocation).CheckPetAnimal(mousePosition, player))
						{
							return true;
						}
						if (didPlayerJustRightClick(ignoreNonMouseHeldInput: true) && (currentLocation as IAnimalLocation).CheckInspectAnimal(mousePosition, player))
						{
							return true;
						}
					}
				}
				Vector2 grabTile = new Vector2(getOldMouseX() + viewport.X, getOldMouseY() + viewport.Y) / 64f;
				Vector2 cursorTile = grabTile;
				if (!wasMouseVisibleThisFrame || mouseCursorTransparency == 0f || !Utility.tileWithinRadiusOfPlayer((int)grabTile.X, (int)grabTile.Y, 1, player))
				{
					grabTile = player.GetGrabTile();
				}
				bool was_character_at_grab_tile = false;
				if (eventUp && !isFestival())
				{
					if (CurrentEvent != null)
					{
						CurrentEvent.receiveActionPress((int)grabTile.X, (int)grabTile.Y);
					}
					oldKBState = currentKBState;
					oldMouseState = input.GetMouseState();
					oldPadState = currentPadState;
					return false;
				}
				if (tryToCheckAt(grabTile, player))
				{
					return false;
				}
				if (player.isRidingHorse())
				{
					player.mount.checkAction(player, player.currentLocation);
					return false;
				}
				if (!player.canMove)
				{
					return false;
				}
				if (!was_character_at_grab_tile)
				{
					NPC grab_tile_character = player.currentLocation.isCharacterAtTile(grabTile);
					if (grab_tile_character != null && grab_tile_character != null)
					{
						was_character_at_grab_tile = true;
					}
				}
				bool isPlacingObject = false;
				if (player.ActiveObject != null && !(player.ActiveObject is Furniture))
				{
					if (player.ActiveObject.performUseAction(currentLocation))
					{
						player.reduceActiveItemByOne();
						oldKBState = currentKBState;
						oldMouseState = input.GetMouseState();
						oldPadState = currentPadState;
						return false;
					}
					int stack = player.ActiveObject.Stack;
					isCheckingNonMousePlacement = !IsPerformingMousePlacement();
					if (isOneOfTheseKeysDown(currentKBState, options.actionButton))
					{
						isCheckingNonMousePlacement = true;
					}
					Vector2 valid_position = Utility.GetNearbyValidPlacementPosition(player, currentLocation, player.ActiveObject, (int)grabTile.X * 64 + 32, (int)grabTile.Y * 64 + 32);
					if (!isCheckingNonMousePlacement && player.ActiveObject is Wallpaper && Utility.tryToPlaceItem(currentLocation, player.ActiveObject, (int)cursorTile.X * 64, (int)cursorTile.Y * 64))
					{
						isCheckingNonMousePlacement = false;
						return true;
					}
					if (Utility.tryToPlaceItem(currentLocation, player.ActiveObject, (int)valid_position.X, (int)valid_position.Y))
					{
						isCheckingNonMousePlacement = false;
						return true;
					}
					if (!eventUp && (player.ActiveObject == null || player.ActiveObject.Stack < stack || player.ActiveObject.isPlaceable()))
					{
						isPlacingObject = true;
					}
					isCheckingNonMousePlacement = false;
				}
				if (!isPlacingObject && !was_character_at_grab_tile)
				{
					grabTile.Y += 1f;
					if (player.FacingDirection >= 0 && player.FacingDirection <= 3)
					{
						Vector2 normalized_offset2 = grabTile - player.getTileLocation();
						if (normalized_offset2.X > 0f || normalized_offset2.Y > 0f)
						{
							normalized_offset2.Normalize();
						}
						if (Vector2.Dot(Utility.DirectionsTileVectors[player.FacingDirection], normalized_offset2) >= 0f && tryToCheckAt(grabTile, player))
						{
							return false;
						}
					}
					if (player.ActiveObject != null && player.ActiveObject is Furniture && !eventUp)
					{
						(player.ActiveObject as Furniture).rotate();
						playSound("dwoop");
						oldKBState = currentKBState;
						oldMouseState = input.GetMouseState();
						oldPadState = currentPadState;
						return false;
					}
					grabTile.Y -= 2f;
					if (player.FacingDirection >= 0 && player.FacingDirection <= 3 && !was_character_at_grab_tile)
					{
						Vector2 normalized_offset = grabTile - player.getTileLocation();
						if (normalized_offset.X > 0f || normalized_offset.Y > 0f)
						{
							normalized_offset.Normalize();
						}
						if (Vector2.Dot(Utility.DirectionsTileVectors[player.FacingDirection], normalized_offset) >= 0f && tryToCheckAt(grabTile, player))
						{
							return false;
						}
					}
					if (player.ActiveObject != null && player.ActiveObject is Furniture && !eventUp)
					{
						(player.ActiveObject as Furniture).rotate();
						playSound("dwoop");
						oldKBState = currentKBState;
						oldMouseState = input.GetMouseState();
						oldPadState = currentPadState;
						return false;
					}
					grabTile = player.getTileLocation();
					if (tryToCheckAt(grabTile, player))
					{
						return false;
					}
					if (player.ActiveObject != null && player.ActiveObject is Furniture && !eventUp)
					{
						(player.ActiveObject as Furniture).rotate();
						playSound("dwoop");
						oldKBState = currentKBState;
						oldMouseState = input.GetMouseState();
						oldPadState = currentPadState;
						return false;
					}
				}
				if (!player.isEating && player.ActiveObject != null && !dialogueUp && !eventUp && !player.canOnlyWalk && !player.FarmerSprite.PauseForSingleAnimation && !fadeToBlack && player.ActiveObject.Edibility != -300 && didPlayerJustRightClick(ignoreNonMouseHeldInput: true))
				{
					if (player.team.SpecialOrderRuleActive("SC_NO_FOOD") && player.currentLocation is MineShaft && (player.currentLocation as MineShaft).getMineArea() == 121)
					{
						addHUDMessage(new HUDMessage(content.LoadString("Strings\\StringsFromCSFiles:Object.cs.13053"), 3));
						return false;
					}
					if (buffsDisplay.hasBuff(25) && player.ActiveObject != null && !player.ActiveObject.HasContextTag("ginger_item"))
					{
						addHUDMessage(new HUDMessage(content.LoadString("Strings\\StringsFromCSFiles:Nauseous_CantEat"), 3));
						return false;
					}
					player.faceDirection(2);
					player.itemToEat = player.ActiveObject;
					player.FarmerSprite.setCurrentSingleAnimation(304);
					currentLocation.createQuestionDialogue((objectInformation[player.ActiveObject.parentSheetIndex].Split('/').Length > 6 && objectInformation[player.ActiveObject.parentSheetIndex].Split('/')[6].Equals("drink")) ? content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.3159", player.ActiveObject.DisplayName) : content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.3160", player.ActiveObject.DisplayName), currentLocation.createYesNoResponses(), "Eat");
					oldKBState = currentKBState;
					oldMouseState = input.GetMouseState();
					oldPadState = currentPadState;
					return false;
				}
			}
			else if (numberOfSelectedItems != -1)
			{
				tryToBuySelectedItems();
				playSound("smallSelect");
				oldKBState = currentKBState;
				oldMouseState = input.GetMouseState();
				oldPadState = currentPadState;
				return false;
			}
			if (player.CurrentTool != null && player.CurrentTool is MeleeWeapon && player.CanMove && !player.canOnlyWalk && !eventUp && !player.onBridge && didPlayerJustRightClick(ignoreNonMouseHeldInput: true))
			{
				((MeleeWeapon)player.CurrentTool).animateSpecialMove(player);
				return false;
			}
			return true;
		}

		public static bool IsPerformingMousePlacement()
		{
			if (mouseCursorTransparency == 0f || !wasMouseVisibleThisFrame || (!lastCursorMotionWasMouse && (player.ActiveObject == null || (!player.ActiveObject.isPlaceable() && player.ActiveObject.Category != -74 && !player.ActiveObject.isSapling()))))
			{
				return false;
			}
			return true;
		}

		public static Vector2 GetPlacementGrabTile()
		{
			if (!IsPerformingMousePlacement())
			{
				return player.GetGrabTile();
			}
			return new Vector2(getOldMouseX() + viewport.X, getOldMouseY() + viewport.Y) / 64f;
		}

		public static bool tryToCheckAt(Vector2 grabTile, Farmer who)
		{
			if (player.onBridge.Value)
			{
				return false;
			}
			haltAfterCheck = true;
			if (Utility.tileWithinRadiusOfPlayer((int)grabTile.X, (int)grabTile.Y, 1, player) && hooks.OnGameLocation_CheckAction(currentLocation, new Location((int)grabTile.X, (int)grabTile.Y), viewport, who, () => currentLocation.checkAction(new Location((int)grabTile.X, (int)grabTile.Y), viewport, who)))
			{
				updateCursorTileHint();
				who.lastGrabTile = grabTile;
				if (who.CanMove && haltAfterCheck)
				{
					who.faceGeneralDirection(grabTile * 64f);
					who.Halt();
				}
				oldKBState = GetKeyboardState();
				oldMouseState = input.GetMouseState();
				oldPadState = input.GetGamePadState();
				return true;
			}
			return false;
		}

		public static void pressSwitchToolButton()
		{
			if (player.netItemStowed.Value)
			{
				player.netItemStowed.Set(newValue: false);
				player.UpdateItemStow();
			}
			int whichWay = ((input.GetMouseState().ScrollWheelValue > oldMouseState.ScrollWheelValue) ? (-1) : ((input.GetMouseState().ScrollWheelValue < oldMouseState.ScrollWheelValue) ? 1 : 0));
			if (options.gamepadControls && whichWay == 0)
			{
				if (input.GetGamePadState().IsButtonDown(Buttons.LeftTrigger))
				{
					whichWay = -1;
				}
				else if (input.GetGamePadState().IsButtonDown(Buttons.RightTrigger))
				{
					whichWay = 1;
				}
			}
			if (options.invertScrollDirection)
			{
				whichWay *= -1;
			}
			if (whichWay == 0)
			{
				return;
			}
			player.CurrentToolIndex = (player.CurrentToolIndex + whichWay) % 12;
			if (player.CurrentToolIndex < 0)
			{
				player.CurrentToolIndex = 11;
			}
			for (int i = 0; i < 12; i++)
			{
				if (player.CurrentItem != null)
				{
					break;
				}
				player.CurrentToolIndex = (whichWay + player.CurrentToolIndex) % 12;
				if (player.CurrentToolIndex < 0)
				{
					player.CurrentToolIndex = 11;
				}
			}
			playSound("toolSwap");
			if (player.ActiveObject != null)
			{
				player.showCarrying();
			}
			else
			{
				player.showNotCarrying();
			}
			if (player.CurrentTool != null && !player.CurrentTool.Name.Equals("Seeds") && !player.CurrentTool.Name.Contains("Sword") && !player.CurrentTool.instantUse)
			{
				player.CurrentTool.CurrentParentTileIndex = player.CurrentTool.CurrentParentTileIndex - player.CurrentTool.CurrentParentTileIndex % 8 + 2;
			}
		}

		public static void switchToolAnimation()
		{
			pickToolInterval = 0f;
			player.CanMove = false;
			pickingTool = true;
			playSound("toolSwap");
			switch (player.FacingDirection)
			{
			case 0:
				player.FarmerSprite.setCurrentFrame(196);
				break;
			case 1:
				player.FarmerSprite.setCurrentFrame(194);
				break;
			case 2:
				player.FarmerSprite.setCurrentFrame(192);
				break;
			case 3:
				player.FarmerSprite.setCurrentFrame(198);
				break;
			}
			if (player.CurrentTool != null && !player.CurrentTool.Name.Equals("Seeds") && !player.CurrentTool.Name.Contains("Sword") && !player.CurrentTool.instantUse)
			{
				player.CurrentTool.CurrentParentTileIndex = player.CurrentTool.CurrentParentTileIndex - player.CurrentTool.CurrentParentTileIndex % 8 + 2;
			}
			if (player.ActiveObject != null)
			{
				player.showCarrying();
			}
		}

		public static bool pressUseToolButton()
		{
			bool stow_was_initialized = game1._didInitiateItemStow;
			game1._didInitiateItemStow = false;
			if (fadeToBlack)
			{
				return false;
			}
			player.toolPower = 0;
			player.toolHold = 0;
			bool did_attempt_object_removal = false;
			if (player.CurrentTool == null && player.ActiveObject == null)
			{
				Vector2 c = player.GetToolLocation() / 64f;
				c.X = (int)c.X;
				c.Y = (int)c.Y;
				if (currentLocation.Objects.ContainsKey(c))
				{
					Object o = currentLocation.Objects[c];
					if (!o.readyForHarvest && o.heldObject.Value == null && !(o is Fence) && !(o is CrabPot) && o.type != null && (o.type.Equals("Crafting") || o.type.Equals("interactive")) && !o.name.Equals("Twig"))
					{
						did_attempt_object_removal = true;
						o.setHealth(o.getHealth() - 1);
						o.shakeTimer = 300;
						currentLocation.playSound("hammer");
						if (o.getHealth() < 2)
						{
							currentLocation.playSound("hammer");
							if (o.getHealth() < 1)
							{
								Tool t = new Pickaxe();
								t.DoFunction(currentLocation, -1, -1, 0, player);
								if (o.performToolAction(t, currentLocation))
								{
									o.performRemoveAction(o.tileLocation, currentLocation);
									if (o.type.Equals("Crafting") && (int)o.fragility != 2)
									{
										currentLocation.debris.Add(new Debris(o.bigCraftable ? (-o.ParentSheetIndex) : o.ParentSheetIndex, player.GetToolLocation(), new Vector2(player.GetBoundingBox().Center.X, player.GetBoundingBox().Center.Y)));
									}
									currentLocation.Objects.Remove(c);
									return true;
								}
							}
						}
					}
				}
			}
			if (currentMinigame == null && !player.UsingTool && (player.IsSitting() || player.isRidingHorse() || player.onBridge.Value || dialogueUp || (eventUp && !CurrentEvent.canPlayerUseTool() && (!currentLocation.currentEvent.playerControlSequence || (activeClickableMenu == null && currentMinigame == null))) || (player.CurrentTool != null && currentLocation.doesPositionCollideWithCharacter(Utility.getRectangleCenteredAt(player.GetToolLocation(), 64), ignoreMonsters: true) != null && currentLocation.doesPositionCollideWithCharacter(Utility.getRectangleCenteredAt(player.GetToolLocation(), 64), ignoreMonsters: true).isVillager())))
			{
				pressActionButton(GetKeyboardState(), input.GetMouseState(), input.GetGamePadState());
				return false;
			}
			if (player.canOnlyWalk)
			{
				return true;
			}
			Vector2 position = ((!wasMouseVisibleThisFrame) ? player.GetToolLocation() : new Vector2(getOldMouseX() + viewport.X, getOldMouseY() + viewport.Y));
			if (Utility.canGrabSomethingFromHere((int)position.X, (int)position.Y, player))
			{
				Vector2 tile = new Vector2(position.X / 64f, position.Y / 64f);
				if (hooks.OnGameLocation_CheckAction(currentLocation, new Location((int)tile.X, (int)tile.Y), viewport, player, () => currentLocation.checkAction(new Location((int)tile.X, (int)tile.Y), viewport, player)))
				{
					updateCursorTileHint();
					return true;
				}
				if (currentLocation.terrainFeatures.ContainsKey(tile))
				{
					currentLocation.terrainFeatures[tile].performUseAction(tile, currentLocation);
					return true;
				}
				return false;
			}
			if (currentLocation.leftClick((int)position.X, (int)position.Y, player))
			{
				return true;
			}
			isCheckingNonMousePlacement = !IsPerformingMousePlacement();
			if (player.ActiveObject != null)
			{
				if (options.allowStowing && CanPlayerStowItem(GetPlacementGrabTile()))
				{
					if (didPlayerJustLeftClick() || stow_was_initialized)
					{
						game1._didInitiateItemStow = true;
						playSound("stoneStep");
						player.netItemStowed.Set(newValue: true);
						return true;
					}
					return true;
				}
				if (Utility.withinRadiusOfPlayer((int)position.X, (int)position.Y, 1, player) && hooks.OnGameLocation_CheckAction(currentLocation, new Location((int)position.X / 64, (int)position.Y / 64), viewport, player, () => currentLocation.checkAction(new Location((int)position.X / 64, (int)position.Y / 64), viewport, player)))
				{
					return true;
				}
				Vector2 grabTile = GetPlacementGrabTile();
				Vector2 valid_position = Utility.GetNearbyValidPlacementPosition(player, currentLocation, player.ActiveObject, (int)grabTile.X * 64, (int)grabTile.Y * 64);
				if (Utility.tryToPlaceItem(currentLocation, player.ActiveObject, (int)valid_position.X, (int)valid_position.Y))
				{
					isCheckingNonMousePlacement = false;
					return true;
				}
				isCheckingNonMousePlacement = false;
			}
			if (currentLocation.LowPriorityLeftClick((int)position.X, (int)position.Y, player))
			{
				return true;
			}
			if (options.allowStowing && player.netItemStowed.Value && !did_attempt_object_removal && (stow_was_initialized || didPlayerJustLeftClick(ignoreNonMouseHeldInput: true)))
			{
				game1._didInitiateItemStow = true;
				playSound("toolSwap");
				player.netItemStowed.Set(newValue: false);
				return true;
			}
			if (player.UsingTool)
			{
				player.lastClick = new Vector2((int)position.X, (int)position.Y);
				player.CurrentTool.DoFunction(player.currentLocation, (int)player.lastClick.X, (int)player.lastClick.Y, 1, player);
				return true;
			}
			if (player.ActiveObject == null && !player.isEating && player.CurrentTool != null)
			{
				if (player.Stamina <= 20f && player.CurrentTool != null && !(player.CurrentTool is MeleeWeapon) && !eventUp)
				{
					staminaShakeTimer = 1000;
					for (int i = 0; i < 4; i++)
					{
						uiOverlayTempSprites.Add(new TemporaryAnimatedSprite("LooseSprites\\Cursors", new Microsoft.Xna.Framework.Rectangle(366, 412, 5, 6), new Vector2(random.Next(32) + uiViewport.Width - 56, uiViewport.Height - 224 - 16 - (int)((double)(player.MaxStamina - 270) * 0.715)), flipped: false, 0.012f, Color.SkyBlue)
						{
							motion = new Vector2(-2f, -10f),
							acceleration = new Vector2(0f, 0.5f),
							local = true,
							scale = 4 + random.Next(-1, 0),
							delayBeforeAnimationStart = i * 30
						});
					}
				}
				if (player.CurrentTool == null || !(player.CurrentTool is MeleeWeapon) || didPlayerJustLeftClick(ignoreNonMouseHeldInput: true))
				{
					int old_direction = player.FacingDirection;
					Vector2 tool_location = player.GetToolLocation(position);
					player.FacingDirection = player.getGeneralDirectionTowards(new Vector2((int)tool_location.X, (int)tool_location.Y));
					player.lastClick = new Vector2((int)position.X, (int)position.Y);
					player.BeginUsingTool();
					if (!player.usingTool)
					{
						player.FacingDirection = old_direction;
					}
					else if (player.FarmerSprite.IsPlayingBasicAnimation(old_direction, carrying: true) || player.FarmerSprite.IsPlayingBasicAnimation(old_direction, carrying: false))
					{
						player.FarmerSprite.StopAnimation();
					}
				}
			}
			return false;
		}

		public static bool CanPlayerStowItem(Vector2 position)
		{
			if (player.ActiveObject == null)
			{
				return false;
			}
			if ((bool)player.ActiveObject.bigCraftable)
			{
				return false;
			}
			if (player.ActiveObject is Furniture)
			{
				return false;
			}
			if (player.ActiveObject != null && (player.ActiveObject.Category == -74 || player.ActiveObject.Category == -19))
			{
				Vector2 valid_position = Utility.GetNearbyValidPlacementPosition(player, currentLocation, player.ActiveObject, (int)position.X * 64, (int)position.Y * 64);
				if (Utility.playerCanPlaceItemHere(player.currentLocation, player.ActiveObject, (int)valid_position.X, (int)valid_position.Y, player) && ((!Object.isWildTreeSeed(player.ActiveObject.ParentSheetIndex) && !player.ActiveObject.isSapling()) || IsPerformingMousePlacement()))
				{
					return false;
				}
			}
			return true;
		}

		public static int getMouseXRaw()
		{
			return input.GetMouseState().X;
		}

		public static int getMouseYRaw()
		{
			return input.GetMouseState().Y;
		}

		public static bool IsOnMainThread()
		{
			if (Thread.CurrentThread != null)
			{
				return !Thread.CurrentThread.IsBackground;
			}
			return false;
		}

		public static void PushUIMode()
		{
			if (!IsOnMainThread())
			{
				return;
			}
			uiModeCount++;
			if (uiModeCount <= 0 || uiMode)
			{
				return;
			}
			uiMode = true;
			if (game1.isDrawing && IsOnMainThread())
			{
				if (game1.uiScreen != null && !game1.uiScreen.IsDisposed)
				{
					RenderTargetBinding[] render_targets = graphics.GraphicsDevice.GetRenderTargets();
					if (render_targets.Length != 0)
					{
						nonUIRenderTarget = render_targets[0].RenderTarget as RenderTarget2D;
					}
					else
					{
						nonUIRenderTarget = null;
					}
					SetRenderTarget(game1.uiScreen);
				}
				if (isRenderingScreenBuffer)
				{
					SetRenderTarget(null);
				}
			}
			xTile.Dimensions.Rectangle ui_viewport_rect = new xTile.Dimensions.Rectangle(0, 0, (int)Math.Ceiling((float)viewport.Width * options.zoomLevel / options.uiScale), (int)Math.Ceiling((float)viewport.Height * options.zoomLevel / options.uiScale));
			ui_viewport_rect.X = viewport.X;
			ui_viewport_rect.Y = viewport.Y;
			uiViewport = ui_viewport_rect;
		}

		public static void PopUIMode()
		{
			if (!IsOnMainThread())
			{
				return;
			}
			uiModeCount--;
			if (uiModeCount > 0 || !uiMode)
			{
				return;
			}
			if (game1.isDrawing)
			{
				if (graphics.GraphicsDevice.GetRenderTargets().Length != 0 && graphics.GraphicsDevice.GetRenderTargets()[0].RenderTarget == game1.uiScreen)
				{
					if (nonUIRenderTarget != null && !nonUIRenderTarget.IsDisposed)
					{
						SetRenderTarget(nonUIRenderTarget);
					}
					else
					{
						SetRenderTarget(null);
					}
				}
				if (isRenderingScreenBuffer)
				{
					SetRenderTarget(null);
				}
			}
			nonUIRenderTarget = null;
			uiMode = false;
		}

		public static void SetRenderTarget(RenderTarget2D target)
		{
			if (!isRenderingScreenBuffer && IsOnMainThread())
			{
				graphics.GraphicsDevice.SetRenderTarget(target);
			}
		}

		public static void InUIMode(Action action)
		{
			PushUIMode();
			try
			{
				action();
			}
			finally
			{
				PopUIMode();
			}
		}

		public static void StartWorldDrawInUI(SpriteBatch b)
		{
			_oldUIModeCount = 0;
			if (uiMode)
			{
				_oldUIModeCount = uiModeCount;
				b?.End();
				while (uiModeCount > 0)
				{
					PopUIMode();
				}
				b?.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);
			}
		}

		public static void EndWorldDrawInUI(SpriteBatch b)
		{
			if (_oldUIModeCount > 0)
			{
				b?.End();
				for (int i = 0; i < _oldUIModeCount; i++)
				{
					PushUIMode();
				}
				b?.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);
			}
			_oldUIModeCount = 0;
		}

		public static int getMouseX()
		{
			return getMouseX(uiMode);
		}

		public static int getMouseX(bool ui_scale)
		{
			if (ui_scale)
			{
				return (int)((float)input.GetMouseState().X / options.uiScale);
			}
			return (int)((float)input.GetMouseState().X * (1f / options.zoomLevel));
		}

		public static int getOldMouseX()
		{
			return getOldMouseX(uiMode);
		}

		public static int getOldMouseX(bool ui_scale)
		{
			if (ui_scale)
			{
				return (int)((float)oldMouseState.X / options.uiScale);
			}
			return (int)((float)oldMouseState.X * (1f / options.zoomLevel));
		}

		public static int getMouseY()
		{
			return getMouseY(uiMode);
		}

		public static int getMouseY(bool ui_scale)
		{
			if (ui_scale)
			{
				return (int)((float)input.GetMouseState().Y / options.uiScale);
			}
			return (int)((float)input.GetMouseState().Y * (1f / options.zoomLevel));
		}

		public static int getOldMouseY()
		{
			return getOldMouseY(uiMode);
		}

		public static int getOldMouseY(bool ui_scale)
		{
			if (ui_scale)
			{
				return (int)((float)oldMouseState.Y / options.uiScale);
			}
			return (int)((float)oldMouseState.Y * (1f / options.zoomLevel));
		}

		public static void pressAddItemToInventoryButton()
		{
		}

		public static int numberOfPlayers()
		{
			return _onlineFarmers.Count;
		}

		public static bool isFestival()
		{
			if (currentLocation != null && currentLocation.currentEvent != null)
			{
				return currentLocation.currentEvent.isFestival;
			}
			return false;
		}

		public bool parseDebugInput(string debugInput)
		{
			lastDebugInput = debugInput;
			debugInput = debugInput.Trim();
			string[] debugSplit = debugInput.Split(' ');
			try
			{
				if (panMode)
				{
					if (debugSplit[0].Equals("exit") || debugSplit[0].ToLower().Equals("panmode"))
					{
						panMode = false;
						viewportFreeze = false;
						panModeString = "";
						debugMode = false;
						debugOutput = "";
						panFacingDirectionWait = false;
						inputSimulator = null;
						return true;
					}
					if (debugSplit[0].Equals("clear"))
					{
						panModeString = "";
						debugOutput = "";
						panFacingDirectionWait = false;
						return true;
					}
					if (!panFacingDirectionWait)
					{
						int time = 0;
						if (int.TryParse(debugSplit[0], out time))
						{
							panModeString = panModeString + ((panModeString.Length > 0) ? "/" : "") + time + " ";
							debugOutput = panModeString + content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.3191");
						}
						return true;
					}
					return false;
				}
				switch (debugSplit[0].ToLowerInvariant())
				{
				case "growwildtrees":
				{
					for (int i = currentLocation.terrainFeatures.Count() - 1; i >= 0; i--)
					{
						Vector2 v = currentLocation.terrainFeatures.Keys.ElementAt(i);
						if (currentLocation.terrainFeatures[v] is Tree)
						{
							(currentLocation.terrainFeatures[v] as Tree).growthStage.Value = 4;
							(currentLocation.terrainFeatures[v] as Tree).fertilized.Value = true;
							(currentLocation.terrainFeatures[v] as Tree).dayUpdate(currentLocation, v);
							(currentLocation.terrainFeatures[v] as Tree).fertilized.Value = false;
						}
					}
					break;
				}
				case "changestat":
					stats.stat_dictionary[debugSplit[1]] = Convert.ToUInt32(debugSplit[2]);
					break;
				case "eventtestspecific":
					eventTest = new EventTest(debugSplit);
					break;
				case "eventtest":
					eventTest = new EventTest((debugSplit.Count() > 1) ? debugSplit[1] : "", (debugSplit.Count() > 2) ? Convert.ToInt32(debugSplit[2]) : 0);
					break;
				case "getallquests":
					foreach (KeyValuePair<int, string> v2 in content.Load<Dictionary<int, string>>("Data\\Quests"))
					{
						player.addQuest(v2.Key);
					}
					break;
				case "movie":
				{
					List<List<Character>> group1 = new List<List<Character>>();
					List<List<Character>> group2 = new List<List<Character>>();
					int npcIndex = random.Next(20);
					Character second_character = null;
					string movie_title = ((debugSplit.Count() > 1) ? debugSplit[1] : "fall_movie_1");
					if (debugSplit.Length > 1)
					{
						second_character = Utility.fuzzyCharacterSearch(debugSplit[1]);
					}
					if (debugSplit.Length > 2)
					{
						movie_title = debugSplit[2];
					}
					if (second_character == null)
					{
						second_character = Utility.getTownNPCByGiftTasteIndex(npcIndex);
					}
					group1.Add(new List<Character> { player, second_character });
					npcIndex = (npcIndex + 1) % 25;
					int number = random.Next(3);
					for (int j3 = 0; j3 < number; j3++)
					{
						if (random.NextDouble() < 0.8)
						{
							if (random.NextDouble() < 0.5)
							{
								group1.Add(new List<Character>
								{
									Utility.getTownNPCByGiftTasteIndex(npcIndex),
									Utility.getTownNPCByGiftTasteIndex(npcIndex + 1)
								});
								npcIndex = (npcIndex + 2) % 25;
							}
							else
							{
								group1.Add(new List<Character> { Utility.getTownNPCByGiftTasteIndex(npcIndex) });
								npcIndex = (npcIndex + 1) % 25;
							}
						}
					}
					for (int j2 = 0; j2 < 2; j2++)
					{
						if (random.NextDouble() < 0.8)
						{
							if (random.NextDouble() < 0.33)
							{
								group2.Add(new List<Character>
								{
									Utility.getTownNPCByGiftTasteIndex(npcIndex),
									Utility.getTownNPCByGiftTasteIndex(npcIndex + 1)
								});
								npcIndex = (npcIndex + 2) % 25;
							}
							else if (random.NextDouble() < 5.0)
							{
								group2.Add(new List<Character>
								{
									Utility.getTownNPCByGiftTasteIndex(npcIndex),
									Utility.getTownNPCByGiftTasteIndex(npcIndex + 1),
									Utility.getTownNPCByGiftTasteIndex(npcIndex + 2)
								});
								npcIndex = (npcIndex + 3) % 25;
							}
							else
							{
								group2.Add(new List<Character> { Utility.getTownNPCByGiftTasteIndex(npcIndex) });
								npcIndex = (npcIndex + 1) % 25;
							}
						}
					}
					MovieTheaterScreeningEvent event_generator = new MovieTheaterScreeningEvent();
					globalFadeToBlack(delegate
					{
						currentLocation.startEvent(event_generator.getMovieEvent(movie_title, group1, group2));
					});
					break;
				}
				case "everythingshop":
				{
					Dictionary<ISalable, int[]> items = new Dictionary<ISalable, int[]>();
					items.Add(new Furniture(1226, Vector2.Zero), new int[2] { 0, 2147483647 });
					foreach (KeyValuePair<int, string> v6 in objectInformation)
					{
						try
						{
							items.Add(new Object(v6.Key, 1), new int[2] { 0, 2147483647 });
						}
						catch (Exception)
						{
						}
					}
					foreach (KeyValuePair<int, string> v5 in bigCraftablesInformation)
					{
						try
						{
							items.Add(new Object(Vector2.Zero, v5.Key), new int[2] { 0, 2147483647 });
						}
						catch (Exception)
						{
						}
					}
					foreach (KeyValuePair<int, string> v4 in content.Load<Dictionary<int, string>>("Data\\weapons"))
					{
						try
						{
							items.Add(new MeleeWeapon(v4.Key), new int[2] { 0, 2147483647 });
						}
						catch (Exception)
						{
						}
					}
					foreach (KeyValuePair<int, string> v3 in content.Load<Dictionary<int, string>>("Data\\furniture"))
					{
						try
						{
							items.Add(new Furniture(v3.Key, Vector2.Zero), new int[2] { 0, 2147483647 });
						}
						catch (Exception)
						{
						}
					}
					activeClickableMenu = new ShopMenu(items);
					break;
				}
				case "dating":
					player.friendshipData[debugSplit[1]].Status = FriendshipStatus.Dating;
					break;
				case "buff":
					buffsDisplay.addOtherBuff(new Buff(Convert.ToInt32(debugSplit[1])));
					break;
				case "clearbuffs":
					player.ClearBuffs();
					break;
				case "pausetime":
					isTimePaused = !isTimePaused;
					if (isTimePaused)
					{
						playSound("bigSelect");
					}
					else
					{
						playSound("bigDeSelect");
					}
					break;
				case "framebyframe":
				case "fbf":
					frameByFrame = !frameByFrame;
					if (frameByFrame)
					{
						playSound("bigSelect");
					}
					else
					{
						playSound("bigDeSelect");
					}
					break;
				case "fbp":
				case "fill":
				case "fillbp":
				case "fillbackpack":
				{
					for (int j = 0; j < player.items.Count; j++)
					{
						if (player.items[j] != null)
						{
							continue;
						}
						int item = -1;
						while (!objectInformation.ContainsKey(item))
						{
							item = random.Next(1000);
							if (item != 390 && (!objectInformation.ContainsKey(item) || objectInformation[item].Split('/')[0] == "Stone"))
							{
								item = -1;
							}
							else if (!objectInformation.ContainsKey(item) || objectInformation[item].Split('/')[0].Contains("Weed"))
							{
								item = -1;
							}
							else if (!objectInformation.ContainsKey(item) || objectInformation[item].Split('/')[3].Contains("Crafting"))
							{
								item = -1;
							}
							else if (!objectInformation.ContainsKey(item) || objectInformation[item].Split('/')[3].Contains("Seed"))
							{
								item = -1;
							}
						}
						bool isRing = false;
						if (item >= 516 && item <= 534)
						{
							isRing = true;
						}
						if (isRing)
						{
							player.items[j] = new Ring(item);
						}
						else
						{
							player.items[j] = new Object(item, 1);
						}
					}
					break;
				}
				case "sl":
					player.shiftToolbar(right: false);
					break;
				case "sr":
					player.shiftToolbar(right: true);
					break;
				case "characterinfo":
					showGlobalMessage(currentLocation.characters.Count + " characters on this map");
					break;
				case "doesitemexist":
					showGlobalMessage(Utility.doesItemWithThisIndexExistAnywhere(Convert.ToInt32(debugSplit[1]), debugSplit.Length > 2) ? "Yes" : "No");
					break;
				case "specialitem":
					player.specialItems.Add(Convert.ToInt32(debugSplit[1]));
					break;
				case "animalinfo":
					showGlobalMessage(getFarm().getAllFarmAnimals().Count.ToString() ?? "");
					break;
				case "clearchildren":
					player.getRidOfChildren();
					break;
				case "createsplash":
				{
					Point offset = default(Point);
					if ((int)player.facingDirection == 3)
					{
						offset.X = -4;
					}
					else if ((int)player.facingDirection == 1)
					{
						offset.X = 4;
					}
					else if ((int)player.facingDirection == 0)
					{
						offset.Y = 4;
					}
					else if ((int)player.facingDirection == 2)
					{
						offset.Y = -4;
					}
					player.currentLocation.fishSplashPoint.Set(new Point(player.getTileX() + offset.X, player.getTileX() + offset.Y));
					break;
				}
				case "pregnant":
				{
					WorldDate birthingDate = Date;
					birthingDate.TotalDays++;
					player.GetSpouseFriendship().NextBirthingDate = birthingDate;
					break;
				}
				case "spreadseeds":
				{
					Farm farm2 = getFarm();
					foreach (KeyValuePair<Vector2, TerrainFeature> t in farm2.terrainFeatures.Pairs)
					{
						if (t.Value is HoeDirt)
						{
							(t.Value as HoeDirt).crop = new Crop(Convert.ToInt32(debugSplit[1]), (int)t.Key.X, (int)t.Key.Y);
						}
					}
					break;
				}
				case "spreaddirt":
				{
					Farm farm2 = getFarm();
					for (int x = 0; x < farm2.map.Layers[0].LayerWidth; x++)
					{
						for (int y = 0; y < farm2.map.Layers[0].LayerHeight; y++)
						{
							if (!farm2.terrainFeatures.ContainsKey(new Vector2(x, y)) && farm2.doesTileHaveProperty(x, y, "Diggable", "Back") != null && farm2.isTileLocationTotallyClearAndPlaceable(new Vector2(x, y)))
							{
								farm2.terrainFeatures.Add(new Vector2(x, y), new HoeDirt());
							}
						}
					}
					break;
				}
				case "removefurniture":
					currentLocation.furniture.Clear();
					break;
				case "makeex":
					player.friendshipData[debugSplit[1]].RoommateMarriage = false;
					player.friendshipData[debugSplit[1]].Status = FriendshipStatus.Divorced;
					break;
				case "darktalisman":
					player.hasDarkTalisman = true;
					getLocationFromName("Railroad").setMapTile(54, 35, 287, "Buildings", "", 1);
					getLocationFromName("Railroad").setMapTile(54, 34, 262, "Front", "", 1);
					getLocationFromName("WitchHut").setMapTile(4, 11, 114, "Buildings", "", 1);
					getLocationFromName("WitchHut").setTileProperty(4, 11, "Buildings", "Action", "MagicInk");
					player.hasMagicInk = false;
					player.mailReceived.Clear();
					break;
				case "conventionmode":
					conventionMode = !conventionMode;
					break;
				case "farmmap":
				{
					for (int k = 0; k < locations.Count; k++)
					{
						if (locations[k] is Farm)
						{
							locations.RemoveAt(k);
						}
						if (locations[k] is FarmHouse)
						{
							locations.RemoveAt(k);
						}
					}
					whichFarm = Convert.ToInt32(debugSplit[1]);
					locations.Add(new Farm("Maps\\" + Farm.getMapNameFromTypeInt(whichFarm), "Farm"));
					locations.Add(new FarmHouse("Maps\\FarmHouse", "FarmHouse"));
					break;
				}
				case "clearmuseum":
					(getLocationFromName("ArchaeologyHouse") as LibraryMuseum).museumPieces.Clear();
					break;
				case "clone":
					currentLocation.characters.Add(Utility.fuzzyCharacterSearch(debugSplit[1]));
					break;
				case "ee":
					pauseTime = 0f;
					nonWarpFade = true;
					eventFinished();
					fadeScreenToBlack();
					viewportFreeze = false;
					break;
				case "zl":
				case "zoomlevel":
					options.desiredBaseZoomLevel = (float)Convert.ToInt32(debugSplit[1]) / 100f;
					break;
				case "us":
				case "uiscale":
					options.desiredUIScale = (float)Convert.ToInt32(debugSplit[1]) / 100f;
					break;
				case "deletearch":
					player.archaeologyFound.Clear();
					player.fishCaught.Clear();
					player.mineralsFound.Clear();
					player.mailReceived.Clear();
					break;
				case "save":
					saveOnNewDay = !saveOnNewDay;
					if (saveOnNewDay)
					{
						playSound("bigSelect");
					}
					else
					{
						playSound("bigDeSelect");
					}
					break;
				case "removelargetf":
					currentLocation.largeTerrainFeatures.Clear();
					break;
				case "test":
					currentMinigame = new Test();
					break;
				case "fencedecay":
					foreach (Object o in currentLocation.objects.Values)
					{
						if (o is Fence)
						{
							(o as Fence).health.Value -= Convert.ToInt32(debugSplit[1]);
						}
					}
					break;
				case "sb":
					Utility.fuzzyCharacterSearch(debugSplit[1]).showTextAboveHead(content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.3206"));
					break;
				case "pan":
					player.addItemToInventoryBool(new Pan());
					break;
				case "gamepad":
					options.gamepadControls = !options.gamepadControls;
					options.mouseControls = !options.gamepadControls;
					showGlobalMessage(options.gamepadControls ? content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.3209") : content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.3210"));
					break;
				case "slimecraft":
					player.craftingRecipes.Add("Slime Incubator", 0);
					player.craftingRecipes.Add("Slime Egg-Press", 0);
					playSound("crystal");
					break;
				case "kms":
				case "killmonsterstat":
				{
					string monster = debugSplit[1].Replace("0", " ");
					int kills = Convert.ToInt32(debugSplit[2]);
					if (stats.specificMonstersKilled.ContainsKey(monster))
					{
						stats.specificMonstersKilled[monster] = kills;
					}
					else
					{
						stats.specificMonstersKilled.Add(monster, kills);
					}
					debugOutput = content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.3159", monster, kills);
					break;
				}
				case "fixanimals":
				{
					Farm farm = getFarm();
					foreach (Building building2 in farm.buildings)
					{
						if (building2.indoors.Value == null || !(building2.indoors.Value is AnimalHouse))
						{
							continue;
						}
						foreach (FarmAnimal a in (building2.indoors.Value as AnimalHouse).animals.Values)
						{
							foreach (Building building3 in farm.buildings)
							{
								if (building3.indoors.Value == null || !(building3.indoors.Value is AnimalHouse) || !(building3.indoors.Value as AnimalHouse).animalsThatLiveHere.Contains(a.myID) || building3.Equals(a.home))
								{
									continue;
								}
								for (int k3 = (building3.indoors.Value as AnimalHouse).animalsThatLiveHere.Count - 1; k3 >= 0; k3--)
								{
									if ((building3.indoors.Value as AnimalHouse).animalsThatLiveHere[k3] == (long)a.myID)
									{
										(building3.indoors.Value as AnimalHouse).animalsThatLiveHere.RemoveAt(k3);
										playSound("crystal");
									}
								}
							}
						}
						for (int k2 = (building2.indoors.Value as AnimalHouse).animalsThatLiveHere.Count - 1; k2 >= 0; k2--)
						{
							if (Utility.getAnimal((building2.indoors.Value as AnimalHouse).animalsThatLiveHere[k2]) == null)
							{
								(building2.indoors.Value as AnimalHouse).animalsThatLiveHere.RemoveAt(k2);
								playSound("crystal");
							}
						}
					}
					break;
				}
				case "steaminfo":
				case "sdkinfo":
					Program.sdk.DebugInfo();
					break;
				case "achieve":
					Program.sdk.GetAchievement(debugSplit[1]);
					break;
				case "resetachievements":
					Program.sdk.ResetAchievements();
					break;
				case "divorce":
					player.divorceTonight.Value = true;
					break;
				case "befriendanimals":
					if (!(currentLocation is AnimalHouse))
					{
						break;
					}
					foreach (FarmAnimal value in (currentLocation as AnimalHouse).animals.Values)
					{
						value.friendshipTowardFarmer.Value = ((debugSplit.Length > 1) ? Convert.ToInt32(debugSplit[1]) : 1000);
					}
					break;
				case "pettofarm":
					getCharacterFromName<Pet>(player.getPetName(), mustBeVillager: false).setAtFarmPosition();
					break;
				case "version":
					debugOutput = typeof(Game1).Assembly.GetName().Version?.ToString() ?? "";
					break;
				case "nosave":
				case "ns":
					saveOnNewDay = !saveOnNewDay;
					if (!saveOnNewDay)
					{
						playSound("bigDeSelect");
					}
					else
					{
						playSound("bigSelect");
					}
					debugOutput = "Saving is now " + (saveOnNewDay ? "enabled" : "disabled");
					break;
				case "rfh":
				case "readyforharvest":
					currentLocation.objects[new Vector2(Convert.ToInt32(debugSplit[1]), Convert.ToInt32(debugSplit[2]))].minutesUntilReady.Value = 1;
					break;
				case "beachbridge":
					(getLocationFromName("Beach") as Beach).bridgeFixed.Value = !(getLocationFromName("Beach") as Beach).bridgeFixed;
					if (!(getLocationFromName("Beach") as Beach).bridgeFixed)
					{
						(getLocationFromName("Beach") as Beach).setMapTile(58, 13, 284, "Buildings", null, 1);
					}
					break;
				case "dp":
					stats.daysPlayed = (uint)Convert.ToInt32(debugSplit[1]);
					break;
				case "fo":
				case "frameoffset":
				{
					int modifier = ((!debugSplit[2].Contains('s')) ? 1 : (-1));
					FarmerRenderer.featureXOffsetPerFrame[Convert.ToInt32(debugSplit[1])] = (short)(modifier * Convert.ToInt32(debugSplit[2].Last().ToString() ?? ""));
					modifier = ((!debugSplit[3].Contains('s')) ? 1 : (-1));
					FarmerRenderer.featureYOffsetPerFrame[Convert.ToInt32(debugSplit[1])] = (short)(modifier * Convert.ToInt32(debugSplit[3].Last().ToString() ?? ""));
					if (debugSplit.Length > 4)
					{
						modifier = ((!debugSplit[4].Contains('s')) ? 1 : (-1));
					}
					break;
				}
				case "horse":
					currentLocation.characters.Add(new Horse(GuidHelper.NewGuid(), Convert.ToInt32(debugSplit[1]), Convert.ToInt32(debugSplit[2])));
					break;
				case "owl":
					currentLocation.addOwl();
					break;
				case "pole":
					player.addItemToInventoryBool(new FishingRod((debugSplit.Length > 1) ? Convert.ToInt32(debugSplit[1]) : 0));
					break;
				case "removequest":
					player.removeQuest(Convert.ToInt32(debugSplit[1]));
					break;
				case "completequest":
					player.completeQuest(Convert.ToInt32(debugSplit[1]));
					break;
				case "togglecatperson":
					player.catPerson = !player.catPerson;
					break;
				case "clearcharacters":
					currentLocation.characters.Clear();
					break;
				case "cat":
					currentLocation.characters.Add(new Cat(Convert.ToInt32(debugSplit[1]), Convert.ToInt32(debugSplit[2]), (debugSplit.Count() > 3) ? Convert.ToInt32(debugSplit[3]) : 0));
					break;
				case "dog":
					currentLocation.characters.Add(new Dog(Convert.ToInt32(debugSplit[1]), Convert.ToInt32(debugSplit[2]), (debugSplit.Count() > 3) ? Convert.ToInt32(debugSplit[3]) : 0));
					break;
				case "quest":
					player.questLog.Add(Quest.getQuestFromId(Convert.ToInt32(debugSplit[1])));
					break;
				case "deliveryquest":
					player.questLog.Add(new ItemDeliveryQuest());
					break;
				case "collectquest":
					player.questLog.Add(new ResourceCollectionQuest());
					break;
				case "slayquest":
					player.questLog.Add(new SlayMonsterQuest());
					break;
				case "quests":
					foreach (int id in content.Load<Dictionary<int, string>>("Data\\Quests").Keys)
					{
						if (!player.hasQuest(id))
						{
							player.addQuest(id);
						}
					}
					player.questLog.Add(new ItemDeliveryQuest());
					player.questLog.Add(new SlayMonsterQuest());
					break;
				case "clearquests":
					player.questLog.Clear();
					break;
				case "fb":
				case "fillbin":
					getFarm().getShippingBin(player).Add(new Object(24, 1));
					getFarm().getShippingBin(player).Add(new Object(82, 1));
					getFarm().getShippingBin(player).Add(new Object(136, 1));
					getFarm().getShippingBin(player).Add(new Object(16, 1));
					getFarm().getShippingBin(player).Add(new Object(388, 1));
					break;
				case "gold":
					player.Money += 1000000;
					break;
				case "clearfarm":
				{
					for (int x2 = 0; x2 < getFarm().map.Layers[0].LayerWidth; x2++)
					{
						for (int y2 = 0; y2 < getFarm().map.Layers[0].LayerHeight; y2++)
						{
							getFarm().removeEverythingExceptCharactersFromThisTile(x2, y2);
						}
					}
					break;
				}
				case "setupfarm":
				{
					getFarm().buildings.Clear();
					for (int x4 = 0; x4 < getFarm().map.Layers[0].LayerWidth; x4++)
					{
						for (int y3 = 0; y3 < 16 + ((debugSplit.Length > 1) ? 32 : 0); y3++)
						{
							getFarm().removeEverythingExceptCharactersFromThisTile(x4, y3);
						}
					}
					for (int x3 = 56; x3 < 71; x3++)
					{
						for (int y4 = 17; y4 < 34; y4++)
						{
							getFarm().removeEverythingExceptCharactersFromThisTile(x3, y4);
							if (x3 > 57 && y4 > 18 && x3 < 70 && y4 < 29)
							{
								getFarm().terrainFeatures.Add(new Vector2(x3, y4), new HoeDirt());
							}
						}
					}
					getFarm().buildStructure(new BluePrint("Coop"), new Vector2(52f, 11f), player);
					getFarm().buildings.Last().daysOfConstructionLeft.Value = 0;
					getFarm().buildStructure(new BluePrint("Silo"), new Vector2(36f, 9f), player);
					getFarm().buildings.Last().daysOfConstructionLeft.Value = 0;
					getFarm().buildStructure(new BluePrint("Barn"), new Vector2(42f, 10f), player);
					getFarm().buildings.Last().daysOfConstructionLeft.Value = 0;
					player.getToolFromName("Ax").UpgradeLevel = 4;
					player.getToolFromName("Watering Can").UpgradeLevel = 4;
					player.getToolFromName("Hoe").UpgradeLevel = 4;
					player.getToolFromName("Pickaxe").UpgradeLevel = 4;
					player.Money += 20000;
					player.addItemToInventoryBool(new Shears());
					player.addItemToInventoryBool(new MilkPail());
					player.addItemToInventoryBool(new Object(472, 999));
					player.addItemToInventoryBool(new Object(473, 999));
					player.addItemToInventoryBool(new Object(322, 999));
					player.addItemToInventoryBool(new Object(388, 999));
					player.addItemToInventoryBool(new Object(390, 999));
					break;
				}
				case "shears":
				case "scissors":
					player.addItemToInventoryBool(new Shears());
					break;
				case "mp":
					player.addItemToInventoryBool(new MilkPail());
					break;
				case "removebuildings":
					getFarm().buildings.Clear();
					break;
				case "build":
					getFarm().buildStructure(new BluePrint(debugSplit[1].Replace('9', ' ')), (debugSplit.Length > 3) ? new Vector2(Convert.ToInt32(debugSplit[2]), Convert.ToInt32(debugSplit[3])) : new Vector2(player.getTileX() + 1, player.getTileY()), player);
					getFarm().buildings.Last().daysOfConstructionLeft.Value = 0;
					break;
				case "bc":
				case "buildcoop":
					getFarm().buildStructure(new BluePrint("Coop"), new Vector2(Convert.ToInt32(debugSplit[1]), Convert.ToInt32(debugSplit[2])), player);
					getFarm().buildings.Last().daysOfConstructionLeft.Value = 0;
					break;
				case "localInfo":
				{
					debugOutput = "";
					int grass = 0;
					int trees = 0;
					int other = 0;
					foreach (TerrainFeature t2 in currentLocation.terrainFeatures.Values)
					{
						if (t2 is Grass)
						{
							grass++;
						}
						else if (t2 is Tree)
						{
							trees++;
						}
						else
						{
							other++;
						}
					}
					debugOutput = debugOutput + "Grass:" + grass + ",  ";
					debugOutput = debugOutput + "Trees:" + trees + ",  ";
					debugOutput = debugOutput + "Other Terrain Features:" + other + ",  ";
					debugOutput = debugOutput + "Objects: " + currentLocation.objects.Count() + ",  ";
					debugOutput = debugOutput + "temporarySprites: " + currentLocation.temporarySprites.Count + ",  ";
					drawObjectDialogue(debugOutput);
					break;
				}
				case "al":
				case "ambientlight":
					ambientLight = new Color(Convert.ToInt32(debugSplit[1]), Convert.ToInt32(debugSplit[2]), Convert.ToInt32(debugSplit[3]));
					break;
				case "resetmines":
					MineShaft.permanentMineChanges.Clear();
					playSound("jingle1");
					break;
				case "db":
					activeClickableMenu = new DialogueBox(Utility.fuzzyCharacterSearch((debugSplit.Length > 1) ? debugSplit[1] : "Pierre").CurrentDialogue.Peek());
					break;
				case "skullkey":
					player.hasSkullKey = true;
					break;
				case "townkey":
					player.HasTownKey = true;
					break;
				case "specials":
					player.hasRustyKey = true;
					player.hasSkullKey = true;
					player.hasSpecialCharm = true;
					player.hasDarkTalisman = true;
					player.hasMagicInk = true;
					player.hasClubCard = true;
					player.canUnderstandDwarves = true;
					player.hasMagnifyingGlass = true;
					player.eventsSeen.Add(2120303);
					player.eventsSeen.Add(3910979);
					player.HasTownKey = true;
					break;
				case "skullgear":
				{
					player.hasSkullKey = true;
					player.MaxItems = 32;
					player.leftRing.Value = new Ring(527);
					player.rightRing.Value = new Ring(523);
					player.boots.Value = new Boots(514);
					player.clearBackpack();
					Pickaxe p = new Pickaxe();
					p.UpgradeLevel = 4;
					player.addItemToInventory(p);
					player.addItemToInventory(new MeleeWeapon(4));
					player.addItemToInventory(new Object(226, 20));
					player.addItemToInventory(new Object(288, 20));
					player.professions.Add(24);
					player.maxHealth = 75;
					break;
				}
				case "clearspecials":
					player.hasRustyKey = false;
					player.hasSkullKey = false;
					player.hasSpecialCharm = false;
					player.hasDarkTalisman = false;
					player.hasMagicInk = false;
					player.hasClubCard = false;
					player.canUnderstandDwarves = false;
					player.hasMagnifyingGlass = false;
					break;
				case "tv":
					player.addItemToInventoryBool(new TV((random.NextDouble() < 0.5) ? 1466 : 1468, Vector2.Zero));
					break;
				case "sn":
					player.hasMagnifyingGlass = true;
					if (debugSplit.Length > 1)
					{
						int whichNote = Convert.ToInt32(debugSplit[1]);
						Object note = new Object(79, 1);
						note.name = note.name + " #" + whichNote;
						player.addItemToInventory(note);
					}
					else
					{
						player.addItemToInventory(currentLocation.tryToCreateUnseenSecretNote(player));
					}
					break;
				case "child2":
					if (player.getChildrenCount() > 1)
					{
						player.getChildren()[1].Age++;
						player.getChildren()[1].reloadSprite();
					}
					else
					{
						(getLocationFromName("FarmHouse") as FarmHouse).characters.Add(new Child("Baby2", random.NextDouble() < 0.5, random.NextDouble() < 0.5, player));
					}
					break;
				case "child":
				case "kid":
					if (player.getChildren().Count > 0)
					{
						player.getChildren()[0].Age++;
						player.getChildren()[0].reloadSprite();
					}
					else
					{
						(getLocationFromName("FarmHouse") as FarmHouse).characters.Add(new Child("Baby", random.NextDouble() < 0.5, random.NextDouble() < 0.5, player));
					}
					break;
				case "killall":
				{
					string safeCharacter = debugSplit[1];
					foreach (GameLocation l2 in locations)
					{
						if (!l2.Equals(currentLocation))
						{
							l2.characters.Clear();
							continue;
						}
						for (int l = l2.characters.Count - 1; l >= 0; l--)
						{
							if (!l2.characters[l].Name.Equals(safeCharacter))
							{
								l2.characters.RemoveAt(l);
							}
						}
					}
					break;
				}
				case "resetworldstate":
					worldStateIDs.Clear();
					netWorldState.Value = new NetWorldState();
					parseDebugInput("deleteArch");
					player.mailReceived.Clear();
					player.eventsSeen.Clear();
					break;
				case "killallhorses":
					foreach (GameLocation l3 in locations)
					{
						for (int m = l3.characters.Count - 1; m >= 0; m--)
						{
							if (l3.characters[m] is Horse)
							{
								l3.characters.RemoveAt(m);
								playSound("drumkit0");
							}
						}
					}
					break;
				case "dateplayer":
					foreach (Farmer farmer in getAllFarmers())
					{
						if (farmer != player && (bool)farmer.isCustomized)
						{
							player.team.GetFriendship(player.UniqueMultiplayerID, farmer.UniqueMultiplayerID).Status = FriendshipStatus.Dating;
							break;
						}
					}
					break;
				case "engageplayer":
					foreach (Farmer farmer2 in getAllFarmers())
					{
						if (farmer2 != player && (bool)farmer2.isCustomized)
						{
							Friendship friendship2 = player.team.GetFriendship(player.UniqueMultiplayerID, farmer2.UniqueMultiplayerID);
							friendship2.Status = FriendshipStatus.Engaged;
							friendship2.WeddingDate = Date;
							friendship2.WeddingDate.TotalDays++;
							break;
						}
					}
					break;
				case "marryplayer":
					foreach (Farmer farmer3 in getOnlineFarmers())
					{
						if (farmer3 != player && (bool)farmer3.isCustomized)
						{
							Friendship friendship = player.team.GetFriendship(player.UniqueMultiplayerID, farmer3.UniqueMultiplayerID);
							friendship.Status = FriendshipStatus.Married;
							friendship.WeddingDate = Date;
							break;
						}
					}
					break;
				case "marry":
				{
					NPC married_character = Utility.fuzzyCharacterSearch(debugSplit[1]);
					if (married_character != null && !player.friendshipData.ContainsKey(married_character.Name))
					{
						player.friendshipData.Add(married_character.Name, new Friendship());
					}
					player.changeFriendship(2500, married_character);
					player.spouse = married_character.Name;
					player.friendshipData[married_character.Name].WeddingDate = new WorldDate(Date);
					player.friendshipData[married_character.Name].Status = FriendshipStatus.Married;
					prepareSpouseForWedding(player);
					break;
				}
				case "engaged":
				{
					player.changeFriendship(2500, Utility.fuzzyCharacterSearch(debugSplit[1]));
					player.spouse = debugSplit[1];
					player.friendshipData[debugSplit[1]].Status = FriendshipStatus.Engaged;
					WorldDate weddingDate = Date;
					weddingDate.TotalDays++;
					player.friendshipData[debugSplit[1]].WeddingDate = weddingDate;
					break;
				}
				case "clearlightglows":
					currentLocation.lightGlows.Clear();
					break;
				case "wp":
				case "wallpaper":
				{
					if (debugSplit.Count() > 1)
					{
						player.addItemToInventoryBool(new Wallpaper(Convert.ToInt32(debugSplit[1])));
						break;
					}
					bool floor = random.NextDouble() < 0.5;
					player.addItemToInventoryBool(new Wallpaper(floor ? random.Next(40) : random.Next(112), floor));
					break;
				}
				case "clearfurniture":
					(currentLocation as FarmHouse).furniture.Clear();
					break;
				case "ff":
				case "furniture":
					if (debugSplit.Length < 2)
					{
						Furniture fu = null;
						while (fu == null)
						{
							try
							{
								fu = new Furniture(random.Next(1613), Vector2.Zero);
							}
							catch (Exception)
							{
							}
						}
						player.addItemToInventoryBool(fu);
					}
					else
					{
						player.addItemToInventoryBool(new Furniture(Convert.ToInt32(debugSplit[1]), Vector2.Zero));
					}
					break;
				case "spawncoopsandbarns":
				{
					if (!(currentLocation is Farm))
					{
						break;
					}
					int num = Convert.ToInt32(debugSplit[1]);
					for (int n = 0; n < num; n++)
					{
						for (int j4 = 0; j4 < 20; j4++)
						{
							bool coop = random.NextDouble() < 0.5;
							if (getFarm().buildStructure(new BluePrint(coop ? "Deluxe Coop" : "Deluxe Barn"), getFarm().getRandomTile(), player))
							{
								getFarm().buildings.Last().daysOfConstructionLeft.Value = 0;
								getFarm().buildings.Last().doAction(Utility.PointToVector2(getFarm().buildings.Last().animalDoor) + new Vector2((int)getFarm().buildings.Last().tileX, (int)getFarm().buildings.Last().tileY), player);
								for (int k4 = 0; k4 < 16; k4++)
								{
									Utility.addAnimalToFarm(new FarmAnimal(coop ? "White Chicken" : "Cow", random.Next(int.MaxValue), player.uniqueMultiplayerID));
								}
								break;
							}
						}
					}
					break;
				}
				case "setupfishpondfarm":
				{
					int population = ((debugSplit.Count() > 1) ? Convert.ToInt32(debugSplit[1]) : 10);
					parseDebugInput("clearFarm");
					for (int x5 = 4; x5 < 77; x5 += 6)
					{
						for (int y5 = 9; y5 < 60; y5 += 6)
						{
							parseDebugInput("build Fish9Pond " + x5 + " " + y5);
						}
					}
					foreach (Building b in getFarm().buildings)
					{
						int fish = random.Next(128, 159);
						if (random.NextDouble() < 0.15)
						{
							fish = random.Next(698, 724);
						}
						if (random.NextDouble() < 0.05)
						{
							fish = random.Next(796, 801);
						}
						if (objectInformation.ContainsKey(fish) && objectInformation[fish].Split('/')[3].Contains("-4"))
						{
							(b as FishPond).fishType.Value = fish;
						}
						else
						{
							(b as FishPond).fishType.Value = ((random.NextDouble() < 0.5) ? 393 : 397);
						}
						(b as FishPond).maxOccupants.Value = 10;
						(b as FishPond).currentOccupants.Value = population;
						(b as FishPond).GetFishObject();
					}
					parseDebugInput("dayUpdate 1");
					break;
				}
				case "grass":
				{
					for (int x6 = 0; x6 < getFarm().Map.Layers[0].LayerWidth; x6++)
					{
						for (int y6 = 0; y6 < getFarm().Map.Layers[0].LayerHeight; y6++)
						{
							if (getFarm().isTileLocationTotallyClearAndPlaceable(new Vector2(x6, y6)))
							{
								getFarm().terrainFeatures.Add(new Vector2(x6, y6), new Grass(1, 4));
							}
						}
					}
					break;
				}
				case "setupbigfarm":
				{
					parseDebugInput("clearFarm");
					parseDebugInput("build Deluxe9Coop 4 9");
					parseDebugInput("build Deluxe9Coop 10 9");
					parseDebugInput("build Deluxe9Coop 36 11");
					parseDebugInput("build Deluxe9Barn 16 9");
					parseDebugInput("build Deluxe9Barn 3 16");
					for (int i4 = 0; i4 < 48; i4++)
					{
						parseDebugInput("animal White Chicken");
					}
					for (int i3 = 0; i3 < 32; i3++)
					{
						parseDebugInput("animal Cow");
					}
					for (int i2 = 0; i2 < getFarm().buildings.Count(); i2++)
					{
						getFarm().buildings[i2].doAction(Utility.PointToVector2(getFarm().buildings[i2].animalDoor) + new Vector2((int)getFarm().buildings[i2].tileX, (int)getFarm().buildings[i2].tileY), player);
					}
					parseDebugInput("build Mill 30 20");
					parseDebugInput("build Stable 46 10");
					parseDebugInput("build Silo 54 14");
					parseDebugInput("build Junimo9Hut 48 52");
					parseDebugInput("build Junimo9Hut 55 52");
					parseDebugInput("build Junimo9Hut 59 52");
					parseDebugInput("build Junimo9Hut 65 52");
					for (int x15 = 11; x15 < 23; x15++)
					{
						for (int y7 = 14; y7 < 25; y7++)
						{
							getFarm().terrainFeatures.Add(new Vector2(x15, y7), new Grass(1, 4));
						}
					}
					for (int x14 = 3; x14 < 23; x14++)
					{
						for (int y8 = 57; y8 < 61; y8++)
						{
							getFarm().terrainFeatures.Add(new Vector2(x14, y8), new Grass(1, 4));
						}
					}
					for (int y15 = 17; y15 < 25; y15++)
					{
						getFarm().terrainFeatures.Add(new Vector2(64f, y15), new Flooring(6));
					}
					for (int x13 = 35; x13 < 64; x13++)
					{
						getFarm().terrainFeatures.Add(new Vector2(x13, 24f), new Flooring(6));
					}
					for (int x12 = 38; x12 < 76; x12++)
					{
						for (int y9 = 18; y9 < 52; y9++)
						{
							if (getFarm().isTileLocationTotallyClearAndPlaceable(new Vector2(x12, y9)))
							{
								getFarm().terrainFeatures.Add(new Vector2(x12, y9), new HoeDirt());
								(getFarm().terrainFeatures[new Vector2(x12, y9)] as HoeDirt).plant(472 + random.Next(5), x12, y9, player, isFertilizer: false, getFarm());
							}
						}
					}
					parseDebugInput("growCrops 8");
					getFarm().terrainFeatures.Add(new Vector2(8f, 25f), new FruitTree(628 + random.Next(2), 4));
					getFarm().terrainFeatures.Add(new Vector2(11f, 25f), new FruitTree(628 + random.Next(2), 4));
					getFarm().terrainFeatures.Add(new Vector2(14f, 25f), new FruitTree(628 + random.Next(2), 4));
					getFarm().terrainFeatures.Add(new Vector2(17f, 25f), new FruitTree(628 + random.Next(2), 4));
					getFarm().terrainFeatures.Add(new Vector2(20f, 25f), new FruitTree(628 + random.Next(2), 4));
					getFarm().terrainFeatures.Add(new Vector2(23f, 25f), new FruitTree(628 + random.Next(2), 4));
					getFarm().terrainFeatures.Add(new Vector2(8f, 28f), new FruitTree(628 + random.Next(2), 4));
					getFarm().terrainFeatures.Add(new Vector2(11f, 28f), new FruitTree(628 + random.Next(2), 4));
					getFarm().terrainFeatures.Add(new Vector2(14f, 28f), new FruitTree(628 + random.Next(2), 4));
					getFarm().terrainFeatures.Add(new Vector2(17f, 28f), new FruitTree(628 + random.Next(2), 4));
					getFarm().terrainFeatures.Add(new Vector2(20f, 28f), new FruitTree(628 + random.Next(2), 4));
					getFarm().terrainFeatures.Add(new Vector2(23f, 28f), new FruitTree(628 + random.Next(2), 4));
					getFarm().terrainFeatures.Add(new Vector2(8f, 31f), new FruitTree(628 + random.Next(2), 4));
					getFarm().terrainFeatures.Add(new Vector2(11f, 31f), new FruitTree(628 + random.Next(2), 4));
					getFarm().terrainFeatures.Add(new Vector2(14f, 31f), new FruitTree(628 + random.Next(2), 4));
					getFarm().terrainFeatures.Add(new Vector2(17f, 31f), new FruitTree(628 + random.Next(2), 4));
					getFarm().terrainFeatures.Add(new Vector2(20f, 31f), new FruitTree(628 + random.Next(2), 4));
					getFarm().terrainFeatures.Add(new Vector2(23f, 31f), new FruitTree(628 + random.Next(2), 4));
					for (int x11 = 3; x11 < 15; x11++)
					{
						for (int y10 = 36; y10 < 45; y10++)
						{
							if (getFarm().isTileLocationTotallyClearAndPlaceable(new Vector2(x11, y10)))
							{
								getFarm().objects.Add(new Vector2(x11, y10), new Object(new Vector2(x11, y10), 12));
								getFarm().objects[new Vector2(x11, y10)].performObjectDropInAction(new Object(454, 1), probe: false, player);
							}
						}
					}
					for (int x10 = 16; x10 < 26; x10++)
					{
						for (int y11 = 36; y11 < 45; y11++)
						{
							if (getFarm().isTileLocationTotallyClearAndPlaceable(new Vector2(x10, y11)))
							{
								getFarm().objects.Add(new Vector2(x10, y11), new Object(new Vector2(x10, y11), 13));
							}
						}
					}
					for (int x9 = 3; x9 < 15; x9++)
					{
						for (int y12 = 47; y12 < 57; y12++)
						{
							if (getFarm().isTileLocationTotallyClearAndPlaceable(new Vector2(x9, y12)))
							{
								getFarm().objects.Add(new Vector2(x9, y12), new Object(new Vector2(x9, y12), 16));
							}
						}
					}
					for (int x8 = 16; x8 < 26; x8++)
					{
						for (int y13 = 47; y13 < 57; y13++)
						{
							if (getFarm().isTileLocationTotallyClearAndPlaceable(new Vector2(x8, y13)))
							{
								getFarm().objects.Add(new Vector2(x8, y13), new Object(new Vector2(x8, y13), 15));
							}
						}
					}
					for (int x7 = 28; x7 < 38; x7++)
					{
						for (int y14 = 26; y14 < 46; y14++)
						{
							if (getFarm().isTileLocationTotallyClearAndPlaceable(new Vector2(x7, y14)))
							{
								new Torch(new Vector2(x7, y14), 1, 93).placementAction(getFarm(), x7 * 64, y14 * 64, null);
							}
						}
					}
					break;
				}
				case "houseupgrade":
				case "hu":
				case "house":
					Utility.getHomeOfFarmer(player).moveObjectsForHouseUpgrade(Convert.ToInt32(debugSplit[1]));
					Utility.getHomeOfFarmer(player).setMapForUpgradeLevel(Convert.ToInt32(debugSplit[1]));
					player.HouseUpgradeLevel = Convert.ToInt32(debugSplit[1]);
					removeFrontLayerForFarmBuildings();
					addNewFarmBuildingMaps();
					Utility.getHomeOfFarmer(player).ReadWallpaperAndFloorTileData();
					Utility.getHomeOfFarmer(player).RefreshFloorObjectNeighbors();
					break;
				case "ci":
				case "clear":
					player.clearBackpack();
					break;
				case "w":
				case "wall":
					(getLocationFromName("FarmHouse") as FarmHouse).SetWallpaper(debugSplit[1], null);
					break;
				case "floor":
					(getLocationFromName("FarmHouse") as FarmHouse).SetFloor(debugSplit[1], null);
					break;
				case "sprinkle":
					Utility.addSprinklesToLocation(currentLocation, player.getTileX(), player.getTileY(), 7, 7, 2000, 100, Color.White);
					break;
				case "clearmail":
					player.mailReceived.Clear();
					break;
				case "broadcastmailbox":
					addMail(debugSplit[1], noLetter: false, sendToEveryone: true);
					break;
				case "mft":
				case "mailfortomorrow":
					addMailForTomorrow(debugSplit[1].Replace('0', '_'), debugSplit.Length > 2);
					break;
				case "allmail":
					foreach (string key4 in content.Load<Dictionary<string, string>>("Data\\mail").Keys)
					{
						addMailForTomorrow(key4);
					}
					break;
				case "allmailread":
					foreach (string key2 in content.Load<Dictionary<string, string>>("Data\\mail").Keys)
					{
						player.mailReceived.Add(key2);
					}
					break;
				case "showmail":
				case "showMail":
				{
					if (debugSplit.Length < 2)
					{
						debugOutput = "Not enough parameters, expecting: showMail <mailTitle>";
						break;
					}
					string mailTitle = debugSplit[1];
					Dictionary<string, string> mails = content.Load<Dictionary<string, string>>("Data\\mail");
					activeClickableMenu = new LetterViewerMenu(mails.ContainsKey(mailTitle) ? mails[mailTitle] : "", mailTitle);
					break;
				}
				case "whereis":
				case "where":
				{
					NPC character = Utility.fuzzyCharacterSearch(debugSplit[1], must_be_villager: false);
					debugOutput = character.Name + " is at " + Utility.getGameLocationOfCharacter(character).NameOrUniqueName + ", " + character.getTileX() + "," + character.getTileY();
					break;
				}
				case "removenpc":
					foreach (GameLocation l4 in locations)
					{
						foreach (NPC c2 in l4.characters)
						{
							if (c2.Name == debugSplit[1])
							{
								l4.characters.Remove(c2);
								debugOutput = "Removed " + debugSplit[1] + " from " + l4.Name;
								return true;
							}
						}
						if (!(l4 is BuildableGameLocation))
						{
							continue;
						}
						foreach (Building b2 in (l4 as BuildableGameLocation).buildings)
						{
							if (b2.indoors.Value == null)
							{
								continue;
							}
							foreach (NPC c in b2.indoors.Value.characters)
							{
								if (c.Name == debugSplit[1])
								{
									b2.indoors.Value.characters.Remove(c);
									debugOutput = "Removed " + debugSplit[1] + " from " + b2.indoors.Value.uniqueName;
									return true;
								}
							}
						}
					}
					debugOutput = "Couldn't find " + debugSplit[1];
					break;
				case "panmode":
				case "pm":
					panMode = true;
					viewportFreeze = true;
					debugMode = true;
					panFacingDirectionWait = false;
					panModeString = "";
					break;
				case "inputsim":
				case "is":
				{
					if (inputSimulator != null)
					{
						inputSimulator = null;
					}
					if (debugSplit.Length < 2)
					{
						debugOutput = "Invalid arguments, call as: inputSim <simType>";
						break;
					}
					string text = debugSplit[1].ToLower();
					if (!(text == "spamtool"))
					{
						if (text == "spamlr")
						{
							inputSimulator = new LeftRightClickSpamInputSimulator();
						}
						else
						{
							debugOutput = "No input simulator found for " + debugSplit[1];
						}
					}
					else
					{
						inputSimulator = new ToolSpamInputSimulator();
					}
					break;
				}
				case "hurry":
					Utility.fuzzyCharacterSearch(debugSplit[1]).warpToPathControllerDestination();
					break;
				case "morepollen":
				{
					for (int i5 = 0; i5 < Convert.ToInt32(debugSplit[1]); i5++)
					{
						debrisWeather.Add(new WeatherDebris(new Vector2(random.Next(0, graphics.GraphicsDevice.Viewport.Width), random.Next(0, graphics.GraphicsDevice.Viewport.Height)), 0, (float)random.Next(15) / 500f, (float)random.Next(-10, 0) / 50f, (float)random.Next(10) / 50f));
					}
					break;
				}
				case "fillwithobject":
				{
					int index = Convert.ToInt32(debugSplit[1]);
					bool bigCraftable = debugSplit.Count() > 2 && Convert.ToBoolean(debugSplit[2]);
					for (int y16 = 0; y16 < currentLocation.map.Layers[0].LayerHeight; y16++)
					{
						for (int x16 = 0; x16 < currentLocation.map.Layers[0].LayerWidth; x16++)
						{
							Vector2 loc = new Vector2(x16, y16);
							if (currentLocation.isTileLocationTotallyClearAndPlaceable(loc))
							{
								currentLocation.setObject(loc, bigCraftable ? new Object(loc, index) : new Object(index, 1));
							}
						}
					}
					break;
				}
				case "spawnweeds":
				{
					for (int i6 = 0; i6 < Convert.ToInt32(debugSplit[1]); i6++)
					{
						currentLocation.spawnWeedsAndStones(1);
					}
					break;
				}
				case "busdriveback":
					(getLocationFromName("BusStop") as BusStop).busDriveBack();
					break;
				case "busdriveoff":
					(getLocationFromName("BusStop") as BusStop).busDriveOff();
					break;
				case "completejoja":
					player.mailReceived.Add("ccCraftsRoom");
					player.mailReceived.Add("ccVault");
					player.mailReceived.Add("ccFishTank");
					player.mailReceived.Add("ccBoilerRoom");
					player.mailReceived.Add("ccPantry");
					player.mailReceived.Add("jojaCraftsRoom");
					player.mailReceived.Add("jojaVault");
					player.mailReceived.Add("jojaFishTank");
					player.mailReceived.Add("jojaBoilerRoom");
					player.mailReceived.Add("jojaPantry");
					player.mailReceived.Add("JojaMember");
					break;
				case "completecc":
				{
					player.mailReceived.Add("ccCraftsRoom");
					player.mailReceived.Add("ccVault");
					player.mailReceived.Add("ccFishTank");
					player.mailReceived.Add("ccBoilerRoom");
					player.mailReceived.Add("ccPantry");
					player.mailReceived.Add("ccBulletin");
					player.mailReceived.Add("ccBoilerRoom");
					player.mailReceived.Add("ccPantry");
					player.mailReceived.Add("ccBulletin");
					CommunityCenter ccc = getLocationFromName("CommunityCenter") as CommunityCenter;
					for (int i7 = 0; i7 < ccc.areasComplete.Count; i7++)
					{
						ccc.markAreaAsComplete(i7);
					}
					break;
				}
				case "whereore":
					debugOutput = Convert.ToString(currentLocation.orePanPoint.Value);
					break;
				case "allbundles":
					foreach (KeyValuePair<int, NetArray<bool, NetBool>> b3 in (getLocationFromName("CommunityCenter") as CommunityCenter).bundles.FieldDict)
					{
						for (int j5 = 0; j5 < b3.Value.Count; j5++)
						{
							b3.Value[j5] = true;
						}
					}
					playSound("crystal");
					break;
				case "junimogoodbye":
					(currentLocation as CommunityCenter).junimoGoodbyeDance();
					break;
				case "bundle":
				{
					CommunityCenter obj2 = getLocationFromName("CommunityCenter") as CommunityCenter;
					int key3 = Convert.ToInt32(debugSplit[1]);
					foreach (KeyValuePair<int, NetArray<bool, NetBool>> b4 in obj2.bundles.FieldDict)
					{
						if (b4.Key == key3)
						{
							for (int j6 = 0; j6 < b4.Value.Count; j6++)
							{
								b4.Value[j6] = true;
							}
						}
					}
					playSound("crystal");
					break;
				}
				case "lookup":
				case "lu":
					foreach (int i8 in objectInformation.Keys)
					{
						if (objectInformation[i8].Substring(0, objectInformation[i8].IndexOf('/')).ToLower().Equals(debugInput.Substring(debugInput.IndexOf(' ') + 1)))
						{
							debugOutput = debugSplit[1] + " " + i8;
						}
					}
					break;
				case "ccloadcutscene":
					(getLocationFromName("CommunityCenter") as CommunityCenter).restoreAreaCutscene(Convert.ToInt32(debugSplit[1]));
					break;
				case "ccload":
					(getLocationFromName("CommunityCenter") as CommunityCenter).loadArea(Convert.ToInt32(debugSplit[1]));
					(getLocationFromName("CommunityCenter") as CommunityCenter).markAreaAsComplete(Convert.ToInt32(debugSplit[1]));
					break;
				case "plaque":
					(getLocationFromName("CommunityCenter") as CommunityCenter).addStarToPlaque();
					break;
				case "junimostar":
					((getLocationFromName("CommunityCenter") as CommunityCenter).characters[0] as Junimo).returnToJunimoHutToFetchStar(getLocationFromName("CommunityCenter") as CommunityCenter);
					break;
				case "j":
				case "aj":
				case "addjunimo":
					(getLocationFromName("CommunityCenter") as CommunityCenter).addCharacter(new Junimo(new Vector2(Convert.ToInt32(debugSplit[1]), Convert.ToInt32(debugSplit[2])) * 64f, Convert.ToInt32(debugSplit[3])));
					break;
				case "resetjunimonotes":
					foreach (NetArray<bool, NetBool> b5 in (getLocationFromName("CommunityCenter") as CommunityCenter).bundles.FieldDict.Values)
					{
						for (int i9 = 0; i9 < b5.Count; i9++)
						{
							b5[i9] = false;
						}
					}
					break;
				case "jn":
				case "junimonote":
					(getLocationFromName("CommunityCenter") as CommunityCenter).addJunimoNote(Convert.ToInt32(debugSplit[1]));
					break;
				case "watercolor":
					currentLocation.waterColor.Value = new Color(Convert.ToInt32(debugSplit[1]), Convert.ToInt32(debugSplit[2]), Convert.ToInt32(debugSplit[3])) * 0.5f;
					break;
				case "festivalscore":
					player.festivalScore += Convert.ToInt32(debugSplit[1]);
					break;
				case "addotherfarmer":
				{
					Farmer f = new Farmer(new FarmerSprite("Characters\\Farmer\\farmer_base"), new Vector2(player.Position.X - 64f, player.Position.Y), 2, Dialogue.randomName(), null, isMale: true);
					f.changeShirt(random.Next(40));
					f.changePants(new Color(random.Next(255), random.Next(255), random.Next(255)));
					f.changeHairStyle(random.Next(FarmerRenderer.hairStylesTexture.Height / 96 * 8));
					if (random.NextDouble() < 0.5)
					{
						f.changeHat(random.Next(-1, FarmerRenderer.hatsTexture.Height / 80 * 12));
					}
					else
					{
						player.changeHat(-1);
					}
					f.changeHairColor(new Color(random.Next(255), random.Next(255), random.Next(255)));
					f.changeSkinColor(random.Next(16));
					f.FarmerSprite.setOwner(f);
					f.currentLocation = currentLocation;
					otherFarmers.Add(random.Next(), f);
					break;
				}
				case "addkent":
					addKentIfNecessary();
					break;
				case "playmusic":
					changeMusicTrack(debugSplit[1]);
					break;
				case "jump":
				{
					float jumpV = 8f;
					if (debugSplit.Length > 2)
					{
						jumpV = (float)Convert.ToDouble(debugSplit[2]);
					}
					if (debugSplit[1].Equals("farmer"))
					{
						player.jump(jumpV);
					}
					else
					{
						Utility.fuzzyCharacterSearch(debugSplit[1]).jump(jumpV);
					}
					break;
				}
				case "toss":
					currentLocation.TemporarySprites.Add(new TemporaryAnimatedSprite(738, 2700f, 1, 0, player.getTileLocation() * 64f, flicker: false, flipped: false)
					{
						rotationChange = (float)Math.PI / 32f,
						motion = new Vector2(0f, -6f),
						acceleration = new Vector2(0f, 0.08f)
					});
					break;
				case "rain":
					isRaining = !isRaining;
					isDebrisWeather = false;
					break;
				case "sf":
				case "setframe":
					player.FarmerSprite.PauseForSingleAnimation = true;
					player.FarmerSprite.setCurrentSingleAnimation(Convert.ToInt32(debugSplit[1]));
					break;
				case "endevent":
				case "leaveevent":
					pauseTime = 0f;
					player.eventsSeen.Clear();
					player.dialogueQuestionsAnswered.Clear();
					player.mailReceived.Clear();
					nonWarpFade = true;
					eventFinished();
					fadeScreenToBlack();
					viewportFreeze = false;
					break;
				case "language":
					activeClickableMenu = new LanguageSelectionMenu();
					break;
				case "runtestevent":
				case "rte":
					runTestEvent();
					break;
				case "qb":
				case "qiboard":
					activeClickableMenu = new SpecialOrdersBoard("Qi");
					break;
				case "ordersboard":
					activeClickableMenu = new SpecialOrdersBoard();
					break;
				case "returneddonations":
					player.team.CheckReturnedDonations();
					break;
				case "completespecialorders":
				case "cso":
					foreach (SpecialOrder specialOrder in player.team.specialOrders)
					{
						foreach (OrderObjective objective in specialOrder.objectives)
						{
							objective.SetCount(objective.maxCount.Value);
						}
					}
					break;
				case "specialorder":
				{
					SpecialOrder order = SpecialOrder.GetSpecialOrder(debugSplit[1], null);
					if (order != null)
					{
						player.team.specialOrders.Add(order);
					}
					break;
				}
				case "boatjourney":
					currentMinigame = new BoatJourney();
					break;
				case "minigame":
					switch (debugSplit[1])
					{
					case "cowboy":
						updateViewportForScreenSizeChange(fullscreenChange: false, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
						currentMinigame = new AbigailGame();
						break;
					case "blastoff":
						currentMinigame = new RobotBlastoff();
						break;
					case "minecart":
						currentMinigame = new MineCart(0, 3);
						break;
					case "grandpa":
						currentMinigame = new GrandpaStory();
						break;
					case "marucomet":
						currentMinigame = new MaruComet();
						break;
					case "haleyCows":
						currentMinigame = new HaleyCowPictures();
						break;
					case "plane":
						currentMinigame = new PlaneFlyBy();
						break;
					case "slots":
						currentMinigame = new Slots();
						break;
					case "target":
						currentMinigame = new TargetGame();
						break;
					case "fishing":
						currentMinigame = new FishingGame();
						break;
					}
					break;
				case "event":
				{
					if (debugSplit.Length <= 3)
					{
						player.eventsSeen.Clear();
					}
					GameLocation location = Utility.fuzzyLocationSearch(debugSplit[1]);
					if (location == null)
					{
						debugOutput = "No location with name " + debugSplit[1];
						break;
					}
					string locationName = location.Name;
					if (locationName == "Pool")
					{
						locationName = "BathHouse_Pool";
					}
					if (content.Load<Dictionary<string, string>>("Data\\Events\\" + locationName).ElementAt(Convert.ToInt32(debugSplit[2])).Key.Contains('/'))
					{
						LocationRequest obj = getLocationRequest(locationName);
						obj.OnLoad += delegate
						{
							currentLocation.currentEvent = new Event(content.Load<Dictionary<string, string>>("Data\\Events\\" + locationName).ElementAt(Convert.ToInt32(debugSplit[2])).Value, Convert.ToInt32(content.Load<Dictionary<string, string>>("Data\\Events\\" + locationName).ElementAt(Convert.ToInt32(debugSplit[2])).Key.Split('/')[0]));
						};
						warpFarmer(obj, 8, 8, player.FacingDirection);
					}
					break;
				}
				case "ebi":
				case "eventbyid":
					if (debugSplit.Length < 1)
					{
						debugOutput = "Event ID not specified";
						return true;
					}
					foreach (GameLocation location3 in locations)
					{
						string locationName2 = location3.Name;
						if (locationName2 == "Pool")
						{
							locationName2 = "BathHouse_Pool";
						}
						Dictionary<string, string> location_events = null;
						try
						{
							location_events = content.Load<Dictionary<string, string>>("Data\\Events\\" + locationName2);
						}
						catch (Exception)
						{
							continue;
						}
						if (location_events == null)
						{
							continue;
						}
						foreach (string key in location_events.Keys)
						{
							string[] key_data = key.Split('/');
							if (!(key_data[0] == debugSplit[1]))
							{
								continue;
							}
							int event_id = -1;
							if (int.TryParse(key_data[0], out event_id))
							{
								while (player.eventsSeen.Contains(event_id))
								{
									player.eventsSeen.Remove(event_id);
								}
							}
							LocationRequest obj3 = getLocationRequest(locationName2);
							obj3.OnLoad += delegate
							{
								currentLocation.currentEvent = new Event(location_events[key], event_id);
							};
							int x17 = 8;
							int y17 = 8;
							Utility.getDefaultWarpLocation(obj3.Name, ref x17, ref y17);
							warpFarmer(obj3, x17, y17, player.FacingDirection);
							debugOutput = "Starting event " + key;
							return true;
						}
					}
					debugOutput = "Event not found.";
					break;
				case "festival":
				{
					Dictionary<string, string> festivalData = temporaryContent.Load<Dictionary<string, string>>("Data\\Festivals\\" + debugSplit[1]);
					if (festivalData != null)
					{
						string season = new string(debugSplit[1].Where(char.IsLetter).ToArray());
						int day = Convert.ToInt32(new string(debugSplit[1].Where(char.IsDigit).ToArray()));
						parseDebugInput("season " + season);
						parseDebugInput("day " + day);
						parseDebugInput("time " + Convert.ToInt32(festivalData["conditions"].Split('/')[1].Split(' ')[0]));
						string where = festivalData["conditions"].Split('/')[0];
						parseDebugInput("warp " + where + " 1 1");
					}
					break;
				}
				case "ps":
				case "playsound":
					playSound(debugSplit[1]);
					break;
				case "crafting":
					foreach (string s in CraftingRecipe.craftingRecipes.Keys)
					{
						if (!player.craftingRecipes.ContainsKey(s))
						{
							player.craftingRecipes.Add(s, 0);
						}
					}
					break;
				case "cooking":
					foreach (string s2 in CraftingRecipe.cookingRecipes.Keys)
					{
						if (!player.cookingRecipes.ContainsKey(s2))
						{
							player.cookingRecipes.Add(s2, 0);
						}
					}
					break;
				case "experience":
				{
					int which = 0;
					if (debugSplit[1].Count() > 1)
					{
						switch (debugSplit[1].ToLower())
						{
						case "farming":
							which = 0;
							break;
						case "fishing":
							which = 1;
							break;
						case "mining":
							which = 3;
							break;
						case "foraging":
							which = 2;
							break;
						case "combat":
							which = 4;
							break;
						}
					}
					else
					{
						which = Convert.ToInt32(debugSplit[1]);
					}
					player.gainExperience(which, Convert.ToInt32(debugSplit[2]));
					break;
				}
				case "showexperience":
					debugOutput = Convert.ToString(player.experiencePoints[Convert.ToInt32(debugSplit[1])]);
					break;
				case "profession":
					player.professions.Add(Convert.ToInt32(debugSplit[1]));
					break;
				case "clearfishcaught":
					player.fishCaught.Clear();
					break;
				case "fishcaught":
				case "caughtfish":
					stats.FishCaught = (uint)Convert.ToInt32(debugSplit[1]);
					break;
				case "r":
					currentLocation.cleanupBeforePlayerExit();
					currentLocation.resetForPlayerEntry();
					break;
				case "fish":
					activeClickableMenu = new BobberBar(Convert.ToInt32(debugSplit[1]), 0.5f, treasure: true, ((player.CurrentTool as FishingRod).attachments[1] != null) ? (player.CurrentTool as FishingRod).attachments[1].ParentSheetIndex : (-1));
					break;
				case "growanimals":
					foreach (FarmAnimal a2 in (currentLocation as AnimalHouse).animals.Values)
					{
						a2.age.Value = (byte)a2.ageWhenMature - 1;
						a2.dayUpdate(currentLocation);
					}
					break;
				case "growanimalsfarm":
					foreach (FarmAnimal a3 in (currentLocation as Farm).animals.Values)
					{
						if (a3.isBaby())
						{
							a3.age.Value = (byte)a3.ageWhenMature - 1;
							a3.dayUpdate(currentLocation);
						}
					}
					break;
				case "pauseanimals":
					if (!(currentLocation is IAnimalLocation))
					{
						break;
					}
					foreach (FarmAnimal value2 in (currentLocation as IAnimalLocation).Animals.Values)
					{
						value2.pauseTimer = int.MaxValue;
					}
					break;
				case "unpauseanimals":
					if (!(currentLocation is IAnimalLocation))
					{
						break;
					}
					foreach (FarmAnimal value3 in (currentLocation as IAnimalLocation).Animals.Values)
					{
						value3.pauseTimer = 0;
					}
					break;
				case "removeterrainfeatures":
				case "removetf":
					currentLocation.terrainFeatures.Clear();
					break;
				case "mushroomtrees":
					foreach (TerrainFeature tf in currentLocation.terrainFeatures.Values)
					{
						if (tf is Tree)
						{
							(tf as Tree).treeType.Value = 7;
						}
					}
					break;
				case "trashcan":
					player.trashCanLevel = Convert.ToInt32(debugSplit[1]);
					break;
				case "addquartz":
				{
					if (debugSplit.Length <= 1)
					{
						break;
					}
					for (int i10 = 0; i10 < Convert.ToInt32(debugSplit[1]) - 1; i10++)
					{
						Vector2 place = getFarm().getRandomTile();
						if (!getFarm().terrainFeatures.ContainsKey(place))
						{
							getFarm().terrainFeatures.Add(place, new Quartz(1 + random.Next(2), Utility.getRandomRainbowColor()));
						}
					}
					break;
				}
				case "fruittrees":
					foreach (KeyValuePair<Vector2, TerrainFeature> t3 in currentLocation.terrainFeatures.Pairs)
					{
						if (t3.Value is FruitTree)
						{
							(t3.Value as FruitTree).daysUntilMature.Value -= 27;
							t3.Value.dayUpdate(currentLocation, t3.Key);
						}
					}
					break;
				case "train":
					(getLocationFromName("Railroad") as Railroad).setTrainComing(7500);
					break;
				case "debrisweather":
					debrisWeather.Clear();
					isDebrisWeather = !isDebrisWeather;
					if (isDebrisWeather)
					{
						populateDebrisWeatherArray();
					}
					break;
				case "speed":
				{
					if (debugSplit.Length < 2)
					{
						debugOutput = "Missing parameters. Run as: 'speed <value> (minutes=30)'";
						break;
					}
					for (int i11 = buffsDisplay.otherBuffs.Count - 1; i11 >= 0; i11--)
					{
						if (buffsDisplay.otherBuffs[i11].source == "Debug Speed")
						{
							buffsDisplay.otherBuffs[i11].removeBuff();
							buffsDisplay.otherBuffs.RemoveAt(i11);
						}
					}
					int minutes = 30;
					if (debugSplit.Length > 2)
					{
						minutes = Convert.ToInt32(debugSplit[2]);
					}
					buffsDisplay.addOtherBuff(new Buff(0, 0, 0, 0, 0, 0, 0, 0, 0, Convert.ToInt32(debugSplit[1]), 0, 0, minutes, "Debug Speed", "Debug Speed"));
					break;
				}
				case "dayupdate":
					currentLocation.DayUpdate(dayOfMonth);
					if (debugSplit.Length > 1)
					{
						for (int i12 = 0; i12 < Convert.ToInt32(debugSplit[1]) - 1; i12++)
						{
							currentLocation.DayUpdate(dayOfMonth);
						}
					}
					break;
				case "museumloot":
					foreach (KeyValuePair<int, string> v7 in objectInformation)
					{
						string type = v7.Value.Split('/')[3];
						if ((type.Contains("Arch") || type.Contains("Minerals")) && !player.mineralsFound.ContainsKey(v7.Key) && !player.archaeologyFound.ContainsKey(v7.Key))
						{
							if (type.Contains("Arch"))
							{
								player.foundArtifact(v7.Key, 1);
							}
							else
							{
								player.addItemToInventoryBool(new Object(v7.Key, 1));
							}
						}
						if (player.freeSpotsInInventory() == 0)
						{
							return true;
						}
					}
					break;
				case "newmuseumloot":
					foreach (KeyValuePair<int, string> v8 in objectInformation)
					{
						string type2 = v8.Value.Split('/')[3];
						if ((type2.Contains("Arch") || type2.Contains("Minerals")) && !netWorldState.Value.MuseumPieces.Values.Contains(v8.Key))
						{
							player.addItemToInventoryBool(new Object(v8.Key, 1));
						}
						if (player.freeSpotsInInventory() == 0)
						{
							return true;
						}
					}
					break;
				case "slingshot":
					player.addItemToInventoryBool(new Slingshot());
					playSound("coin");
					break;
				case "ring":
					player.addItemToInventoryBool(new Ring(Convert.ToInt32(debugSplit[1])));
					playSound("coin");
					break;
				case "boots":
					player.addItemToInventoryBool(new Boots(Convert.ToInt32(debugSplit[1])));
					playSound("coin");
					break;
				case "mainmenu":
				case "createdebris":
					if (debugSplit.Length < 2)
					{
						debugOutput = "Invalid parameters; call like: createDebris <itemId>";
					}
					else
					{
						createObjectDebris(Convert.ToInt32(debugSplit[1]), player.getTileX(), player.getTileY());
					}
					break;
				case "removedebris":
					currentLocation.debris.Clear();
					break;
				case "removedirt":
				{
					for (int i13 = currentLocation.terrainFeatures.Count() - 1; i13 >= 0; i13--)
					{
						if (currentLocation.terrainFeatures.Pairs.ElementAt(i13).Value is HoeDirt)
						{
							currentLocation.terrainFeatures.Remove(currentLocation.terrainFeatures.Pairs.ElementAt(i13).Key);
						}
					}
					break;
				}
				case "dyeAll":
					activeClickableMenu = new CharacterCustomization(CharacterCustomization.Source.DyePots);
					break;
				case "dyeshirt":
					activeClickableMenu = new CharacterCustomization(player.shirtItem.Value);
					break;
				case "dyepants":
					activeClickableMenu = new CharacterCustomization(player.pantsItem.Value);
					break;
				case "cmenu":
				case "customize":
				case "customizemenu":
					activeClickableMenu = new CharacterCustomization(CharacterCustomization.Source.NewGame);
					break;
				case "skincolor":
					player.changeSkinColor(Convert.ToInt32(debugSplit[1]));
					break;
				case "hat":
					player.changeHat(Convert.ToInt32(debugSplit[1]));
					playSound("coin");
					break;
				case "pants":
					player.changePants(new Color(Convert.ToInt32(debugSplit[1]), Convert.ToInt32(debugSplit[2]), Convert.ToInt32(debugSplit[3])));
					break;
				case "hairstyle":
					player.changeHairStyle(Convert.ToInt32(debugSplit[1]));
					break;
				case "haircolor":
					player.changeHairColor(new Color(Convert.ToInt32(debugSplit[1]), Convert.ToInt32(debugSplit[2]), Convert.ToInt32(debugSplit[3])));
					break;
				case "shirt":
					player.changeShirt(Convert.ToInt32(debugSplit[1]));
					break;
				case "mv":
				case "m":
				case "musicvolume":
					musicPlayerVolume = (float)Convert.ToDouble(debugSplit[1]);
					options.musicVolumeLevel = (float)Convert.ToDouble(debugSplit[1]);
					musicCategory.SetVolume(options.musicVolumeLevel);
					break;
				case "removeobjects":
					currentLocation.objects.Clear();
					break;
				case "removelights":
					currentLightSources.Clear();
					break;
				case "i":
				case "item":
					if (objectInformation.ContainsKey(Convert.ToInt32(debugSplit[1])))
					{
						playSound("coin");
						player.addItemToInventoryBool(new Object(Convert.ToInt32(debugSplit[1]), (debugSplit.Length < 3) ? 1 : Convert.ToInt32(debugSplit[2]), isRecipe: false, -1, (debugSplit.Length >= 4) ? Convert.ToInt32(debugSplit[3]) : 0));
					}
					break;
				case "dyemenu":
					activeClickableMenu = new DyeMenu();
					break;
				case "tailor":
					activeClickableMenu = new TailoringMenu();
					break;
				case "forge":
					activeClickableMenu = new ForgeMenu();
					break;
				case "listtags":
				{
					if (player.CurrentItem == null)
					{
						break;
					}
					string out_string = "Tags on " + player.CurrentItem.DisplayName + ": ";
					foreach (string tag in player.CurrentItem.GetContextTagList())
					{
						out_string = out_string + tag + " ";
					}
					debugOutput = out_string.Trim();
					break;
				}
				case "dye":
				{
					Color target = Color.White;
					switch (debugSplit[2].ToLower().Trim())
					{
					case "black":
						target = Color.Black;
						break;
					case "red":
						target = new Color(220, 0, 0);
						break;
					case "blue":
						target = new Color(0, 100, 220);
						break;
					case "yellow":
						target = new Color(255, 230, 0);
						break;
					case "white":
						target = Color.White;
						break;
					case "green":
						target = new Color(10, 143, 0);
						break;
					}
					float dye_strength = 1f;
					if (debugSplit.Length > 2)
					{
						dye_strength = float.Parse(debugSplit[3]);
					}
					string text = debugSplit[1].ToLower().Trim();
					if (!(text == "shirt"))
					{
						if (text == "pants" && player.pantsItem.Value != null)
						{
							player.pantsItem.Value.Dye(target, dye_strength);
						}
					}
					else if (player.shirtItem.Value != null)
					{
						player.shirtItem.Value.Dye(target, dye_strength);
					}
					break;
				}
				case "clothes":
					playSound("coin");
					player.addItemToInventoryBool(new Clothing(Convert.ToInt32(debugSplit[1])));
					break;
				case "getindex":
				{
					Item item2 = Utility.fuzzyItemSearch(debugSplit[1]);
					if (item2 != null)
					{
						debugOutput = item2.DisplayName + "'s index is " + item2.ParentSheetIndex;
					}
					else
					{
						debugOutput = "No item found with name " + debugSplit[1];
					}
					break;
				}
				case "fuzzyitemnamed":
				case "fin":
				case "f":
				{
					int quality = -1;
					int stack_count = 1;
					if (debugSplit.Length > 2)
					{
						int.TryParse(debugSplit[2], out stack_count);
					}
					if (debugSplit.Length > 3)
					{
						int.TryParse(debugSplit[3], out quality);
					}
					Item item3 = Utility.fuzzyItemSearch(debugSplit[1], stack_count);
					if (item3 != null)
					{
						if (quality >= 0 && item3 is Object)
						{
							(item3 as Object).quality.Value = quality;
						}
						player.addItemToInventory(item3);
						playSound("coin");
						string type_name = item3.GetType().ToString();
						if (type_name.Contains('.'))
						{
							type_name = type_name.Substring(type_name.LastIndexOf('.') + 1);
							if (item3 is Object && (bool)(item3 as Object).bigCraftable)
							{
								type_name = "Big Craftable";
							}
						}
						debugOutput = "Added " + item3.DisplayName + " (" + type_name + ")";
					}
					else
					{
						debugOutput = "No item found with name " + debugSplit[1];
					}
					break;
				}
				case "in":
				case "itemnamed":
					foreach (int i14 in objectInformation.Keys)
					{
						if (objectInformation[i14].Substring(0, objectInformation[i14].IndexOf('/')).ToLower().Replace(" ", "")
							.Equals(debugSplit[1].ToLower()))
						{
							player.addItemToInventory(new Object(i14, (debugSplit.Length < 3) ? 1 : Convert.ToInt32(debugSplit[2]), isRecipe: false, -1, (debugSplit.Length >= 4) ? Convert.ToInt32(debugSplit[3]) : 0));
							playSound("coin");
						}
					}
					break;
				case "achievement":
					getAchievement(Convert.ToInt32(debugSplit[1]));
					break;
				case "heal":
					player.health = player.maxHealth;
					break;
				case "die":
					player.health = 0;
					break;
				case "energize":
					player.Stamina = player.MaxStamina;
					if (debugSplit.Length > 1)
					{
						player.Stamina = Convert.ToInt32(debugSplit[1]);
					}
					break;
				case "exhaust":
					player.Stamina = -15f;
					break;
				case "warp":
				{
					GameLocation location2 = Utility.fuzzyLocationSearch(debugSplit[1]);
					if (location2 != null)
					{
						int x18 = 0;
						int y18 = 0;
						if (debugSplit.Length >= 4)
						{
							x18 = Convert.ToInt32(debugSplit[2]);
							y18 = Convert.ToInt32(debugSplit[3]);
						}
						else
						{
							Utility.getDefaultWarpLocation(location2.Name, ref x18, ref y18);
						}
						warpFarmer(new LocationRequest(location2.NameOrUniqueName, location2.uniqueName.Value != null, location2), x18, y18, 2);
						debugOutput = "Warping player to " + location2.NameOrUniqueName + " at " + x18 + ", " + y18;
					}
					else
					{
						debugOutput = "No location with name " + debugSplit[1];
					}
					break;
				}
				case "wh":
				case "warphome":
					warpHome();
					break;
				case "money":
					player.Money = Convert.ToInt32(debugSplit[1]);
					break;
				case "perfection":
				{
					parseDebugInput("friendAll");
					parseDebugInput("cooking");
					parseDebugInput("crafting");
					for (int i17 = player.craftingRecipes.Count() - 1; i17 >= 0; i17--)
					{
						player.craftingRecipes[player.craftingRecipes.Pairs.ElementAt(i17).Key] = 1;
					}
					foreach (KeyValuePair<int, string> v10 in objectInformation)
					{
						if (v10.Value.Split('/')[3].Contains("Fish"))
						{
							player.fishCaught.Add(v10.Key, new int[3]);
						}
						string typeString = v10.Value.Split('/')[3];
						if (Object.isPotentialBasicShippedCategory(v10.Key, typeString.Substring(typeString.Length - 3)))
						{
							player.basicShipped.Add(v10.Key, 1);
						}
						player.recipesCooked.Add(v10.Key, 1);
					}
					parseDebugInput("walnut 130");
					player.mailReceived.Add("CF_Fair");
					player.mailReceived.Add("CF_Fish");
					player.mailReceived.Add("CF_Sewer");
					player.mailReceived.Add("CF_Mines");
					player.mailReceived.Add("CF_Spouse");
					player.mailReceived.Add("CF_Statue");
					player.mailReceived.Add("museumComplete");
					player.miningLevel.Value = 10;
					player.fishingLevel.Value = 10;
					player.foragingLevel.Value = 10;
					player.combatLevel.Value = 10;
					player.farmingLevel.Value = 10;
					getFarm().buildStructure(new BluePrint("Water Obelisk"), new Vector2(0f, 0f), player, magicalConstruction: true, skipSafetyChecks: true);
					getFarm().buildStructure(new BluePrint("Earth Obelisk"), new Vector2(4f, 0f), player, magicalConstruction: true, skipSafetyChecks: true);
					getFarm().buildStructure(new BluePrint("Desert Obelisk"), new Vector2(8f, 0f), player, magicalConstruction: true, skipSafetyChecks: true);
					getFarm().buildStructure(new BluePrint("Island Obelisk"), new Vector2(12f, 0f), player, magicalConstruction: true, skipSafetyChecks: true);
					getFarm().buildStructure(new BluePrint("Gold Clock"), new Vector2(16f, 0f), player, magicalConstruction: true, skipSafetyChecks: true);
					foreach (KeyValuePair<string, string> v9 in content.Load<Dictionary<string, string>>("Data\\Monsters"))
					{
						for (int i15 = 0; i15 < 500; i15++)
						{
							stats.monsterKilled(v9.Key);
						}
					}
					break;
				}
				case "walnut":
					netWorldState.Value.GoldenWalnuts.Value += Convert.ToInt32(debugSplit[1]);
					netWorldState.Value.GoldenWalnutsFound.Value += Convert.ToInt32(debugSplit[1]);
					break;
				case "gem":
					player.QiGems += Convert.ToInt32(debugSplit[1]);
					break;
				case "killnpc":
				{
					for (int i18 = locations.Count - 1; i18 >= 0; i18--)
					{
						for (int j7 = 0; j7 < locations[i18].characters.Count; j7++)
						{
							if (locations[i18].characters[j7].Name.Equals(debugSplit[1]))
							{
								locations[i18].characters.RemoveAt(j7);
								break;
							}
						}
					}
					break;
				}
				case "dap":
				case "daysplayed":
					showGlobalMessage(content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.3332", (int)stats.DaysPlayed));
					break;
				case "friendall":
					foreach (NPC n2 in Utility.getAllCharacters())
					{
						if ((n2.CanSocialize || !(n2.Name != "Sandy") || !(n2.Name == "Krobus")) && !(n2.Name == "Marlon"))
						{
							if (n2 != null && !player.friendshipData.ContainsKey(n2.Name))
							{
								player.friendshipData.Add(n2.Name, new Friendship());
							}
							player.changeFriendship((debugSplit.Count() > 1) ? Convert.ToInt32(debugSplit[1]) : 2500, n2);
						}
					}
					break;
				case "friend":
				case "friendship":
				{
					NPC npc = Utility.fuzzyCharacterSearch(debugSplit[1]);
					if (npc != null && !player.friendshipData.ContainsKey(npc.Name))
					{
						player.friendshipData.Add(npc.Name, new Friendship());
					}
					player.friendshipData[npc.Name].Points = Convert.ToInt32(debugSplit[2]);
					break;
				}
				case "getstat":
					debugOutput = stats.GetType().GetProperty(debugSplit[1]).GetValue(stats, null)
						.ToString();
					break;
				case "setstat":
					stats.GetType().GetProperty(debugSplit[1]).SetValue(stats, Convert.ToUInt32(debugSplit[2]), null);
					break;
				case "eventseen":
				case "seenevent":
					player.eventsSeen.Add(Convert.ToInt32(debugSplit[1]));
					break;
				case "seenmail":
					player.mailReceived.Add(debugSplit[1]);
					break;
				case "cookingrecipe":
					player.cookingRecipes.Add(debugInput.Substring(debugInput.IndexOf(' ')).Trim(), 0);
					break;
				case "craftingrecipe":
					player.craftingRecipes.Add(debugInput.Substring(debugInput.IndexOf(' ')).Trim(), 0);
					break;
				case "upgradehouse":
					player.HouseUpgradeLevel = Math.Min(3, player.HouseUpgradeLevel + 1);
					removeFrontLayerForFarmBuildings();
					addNewFarmBuildingMaps();
					break;
				case "coop":
				case "upgradecoop":
					player.CoopUpgradeLevel = Math.Min(3, player.CoopUpgradeLevel + 1);
					removeFrontLayerForFarmBuildings();
					addNewFarmBuildingMaps();
					break;
				case "barn":
				case "upgradebarn":
					player.BarnUpgradeLevel = Math.Min(3, player.BarnUpgradeLevel + 1);
					removeFrontLayerForFarmBuildings();
					addNewFarmBuildingMaps();
					break;
				case "resource":
					Debris.getDebris(Convert.ToInt32(debugSplit[1]), Convert.ToInt32(debugSplit[2]));
					break;
				case "weapon":
					player.addItemToInventoryBool(new MeleeWeapon(Convert.ToInt32(debugSplit[1])));
					break;
				case "stoprafting":
					player.isRafting = false;
					break;
				case "time":
					timeOfDay = Convert.ToInt32(debugSplit[1]);
					outdoorLight = Color.White;
					break;
				case "addminute":
					addMinute();
					break;
				case "addhour":
					addHour();
					break;
				case "water":
					foreach (TerrainFeature t5 in currentLocation.terrainFeatures.Values)
					{
						if (t5 is HoeDirt)
						{
							(t5 as HoeDirt).state.Value = 1;
						}
					}
					break;
				case "growcrops":
					foreach (KeyValuePair<Vector2, TerrainFeature> t4 in currentLocation.terrainFeatures.Pairs)
					{
						if (!(t4.Value is HoeDirt) || (t4.Value as HoeDirt).crop == null)
						{
							continue;
						}
						for (int i16 = 0; i16 < Convert.ToInt32(debugSplit[1]); i16++)
						{
							if ((t4.Value as HoeDirt).crop != null)
							{
								(t4.Value as HoeDirt).crop.newDay(1, -1, (int)t4.Key.X, (int)t4.Key.Y, currentLocation);
							}
						}
					}
					break;
				case "b":
				case "bi":
				case "big":
				case "bigitem":
					if (bigCraftablesInformation.ContainsKey(Convert.ToInt32(debugSplit[1])))
					{
						playSound("coin");
						player.addItemToInventory(new Object(Vector2.Zero, Convert.ToInt32(debugSplit[1]))
						{
							Stack = ((debugSplit.Count() <= 2) ? 1 : Convert.ToInt32(debugSplit[2]))
						});
					}
					break;
				case "cm":
				case "c":
				case "canmove":
					player.isEating = false;
					player.CanMove = true;
					player.UsingTool = false;
					player.usingSlingshot = false;
					player.FarmerSprite.PauseForSingleAnimation = false;
					if (player.CurrentTool is FishingRod)
					{
						(player.CurrentTool as FishingRod).isFishing = false;
					}
					if (player.mount != null)
					{
						player.mount.dismount();
					}
					break;
				case "backpack":
					player.increaseBackpackSize(Math.Min(36 - player.items.Count(), Convert.ToInt32(debugSplit[1])));
					break;
				case "question":
					player.dialogueQuestionsAnswered.Add(Convert.ToInt32(debugSplit[1]));
					break;
				case "year":
					year = Convert.ToInt32(debugSplit[1]);
					break;
				case "day":
					stats.DaysPlayed = (uint)(Utility.getSeasonNumber(currentSeason) * 28 + Convert.ToInt32(debugSplit[1]) + (year - 1) * 4 * 28);
					dayOfMonth = Convert.ToInt32(debugSplit[1]);
					break;
				case "season":
					if (debugSplit.Length >= 1 && Utility.getSeasonNumber(debugSplit[1].ToLower()) >= 0)
					{
						currentSeason = debugSplit[1].ToLower();
						setGraphicsForSeason();
					}
					break;
				case "dialogue":
					Utility.fuzzyCharacterSearch(debugSplit[1]).CurrentDialogue.Push(new Dialogue(debugInput.Substring(debugInput.IndexOf("0") + 1), Utility.fuzzyCharacterSearch(debugSplit[1])));
					break;
				case "speech":
					Utility.fuzzyCharacterSearch(debugSplit[1]).CurrentDialogue.Push(new Dialogue(debugInput.Substring(debugInput.IndexOf("0") + 1), Utility.fuzzyCharacterSearch(debugSplit[1])));
					drawDialogue(Utility.fuzzyCharacterSearch(debugSplit[1]));
					break;
				case "loaddialogue":
				{
					NPC char1 = Utility.fuzzyCharacterSearch(debugSplit[1]);
					string locKey = debugSplit[2];
					string locString = content.LoadString(locKey);
					locString = locString.Replace("{", "<");
					locString = locString.Replace("}", ">");
					char1.CurrentDialogue.Push(new Dialogue(locString, char1));
					drawDialogue(Utility.fuzzyCharacterSearch(debugSplit[1]));
					break;
				}
				case "wedding":
					player.spouse = debugSplit[1];
					weddingsToday.Add(player.UniqueMultiplayerID);
					break;
				case "end":
					warpFarmer("Town", 20, 20, flip: false);
					getLocationFromName("Town").currentEvent = new Event(Utility.getStardewHeroCelebrationEventString(90));
					makeCelebrationWeatherDebris();
					Utility.perpareDayForStardewCelebration(90);
					break;
				case "gamemode":
					setGameMode(Convert.ToByte(debugSplit[1]));
					break;
				case "volcano":
					warpFarmer("VolcanoDungeon" + Convert.ToInt32(debugSplit[1]), 0, 1, 2);
					break;
				case "minelevel":
					enterMine(Convert.ToInt32(debugSplit[1]));
					break;
				case "mineinfo":
					debugOutput = "MineShaft.lowestLevelReached = " + MineShaft.lowestLevelReached + "\nplayer.deepestMineLevel = " + player.deepestMineLevel;
					break;
				case "tool":
					player.getToolFromName(debugSplit[1]).UpgradeLevel = Convert.ToInt32(debugSplit[2]);
					break;
				case "viewport":
					viewport.X = Convert.ToInt32(debugSplit[1]) * 64;
					viewport.Y = Convert.ToInt32(debugSplit[2]) * 64;
					break;
				case "makeinedible":
					if (player.ActiveObject != null)
					{
						player.ActiveObject.edibility.Value = -300;
					}
					break;
				case "watm":
				case "warpanimaltome":
				{
					if (!(currentLocation is IAnimalLocation))
					{
						debugOutput = "Animals not allowed in current location.";
						break;
					}
					IAnimalLocation current_animal_location = currentLocation as IAnimalLocation;
					FarmAnimal target_character5 = Utility.fuzzyAnimalSearch(debugSplit[1]);
					if (target_character5 != null)
					{
						debugOutput = "Warping " + target_character5.displayName;
						(target_character5.currentLocation as IAnimalLocation).Animals.Remove(target_character5.myID);
						current_animal_location.Animals.Add(target_character5.myID, target_character5);
						target_character5.Position = player.Position;
						target_character5.controller = null;
					}
					else
					{
						debugOutput = "Couldn't find character named " + debugSplit[1];
					}
					break;
				}
				case "wctm":
				case "warpcharactertome":
				{
					NPC target_character4 = Utility.fuzzyCharacterSearch(debugSplit[1], must_be_villager: false);
					if (target_character4 != null)
					{
						debugOutput = "Warping " + target_character4.displayName;
						warpCharacter(target_character4, currentLocation.Name, new Vector2(player.getTileX(), player.getTileY()));
						target_character4.controller = null;
						target_character4.Halt();
					}
					else
					{
						debugOutput = "Couldn't find character named " + debugSplit[1];
					}
					break;
				}
				case "wc":
				case "warpcharacter":
				{
					NPC target_character3 = Utility.fuzzyCharacterSearch(debugSplit[1], must_be_villager: false);
					if (target_character3 == null)
					{
						break;
					}
					if (debugSplit.Length < 4)
					{
						debugOutput = "Missing parameters, run as: 'wc <npcName> <x> <y> [facingDirection=1]'";
						break;
					}
					int facingDirection2 = 2;
					if (debugSplit.Length >= 5)
					{
						facingDirection2 = Convert.ToInt32(debugSplit[4]);
					}
					warpCharacter(target_character3, currentLocation.Name, new Vector2(Convert.ToInt32(debugSplit[2]), Convert.ToInt32(debugSplit[3])));
					target_character3.faceDirection(facingDirection2);
					target_character3.controller = null;
					target_character3.Halt();
					break;
				}
				case "wtp":
				case "warptoplayer":
				{
					if (debugSplit.Length < 2)
					{
						debugOutput = "Missing parameters, run as: 'wtp <playerName>'";
						break;
					}
					string cleanedName = debugSplit[1].ToLower().Replace(" ", "");
					Farmer otherFarmer = null;
					foreach (Farmer farmer4 in getOnlineFarmers())
					{
						if (farmer4.displayName.Replace(" ", "").ToLower() == cleanedName)
						{
							otherFarmer = farmer4;
							break;
						}
					}
					if (otherFarmer == null)
					{
						debugOutput = "Could not find other farmer " + debugSplit[1];
						break;
					}
					parseDebugInput("warp " + otherFarmer.currentLocation.NameOrUniqueName + " " + otherFarmer.getTileX() + " " + otherFarmer.getTileY());
					break;
				}
				case "wtc":
				case "warptocharacter":
				{
					if (debugSplit.Length < 2)
					{
						debugOutput = "Missing parameters, run as: 'wtc <npcName>'";
						break;
					}
					NPC target_character2 = Utility.fuzzyCharacterSearch(debugSplit[1]);
					if (target_character2 == null)
					{
						debugOutput = "Could not find valid character " + debugSplit[1];
						break;
					}
					parseDebugInput("warp " + Utility.getGameLocationOfCharacter(target_character2).Name + " " + target_character2.getTileX() + " " + target_character2.getTileY());
					break;
				}
				case "wct":
				case "warpcharacterto":
				{
					NPC target_character = Utility.fuzzyCharacterSearch(debugSplit[1]);
					if (target_character == null)
					{
						break;
					}
					if (debugSplit.Length < 5)
					{
						debugOutput = "Missing parameters, run as: 'wct <npcName> <locationName> <x> <y> [facingDirection=1]'";
						break;
					}
					int facingDirection = 2;
					if (debugSplit.Length >= 6)
					{
						facingDirection = Convert.ToInt32(debugSplit[4]);
					}
					warpCharacter(target_character, debugSplit[2], new Vector2(Convert.ToInt32(debugSplit[3]), Convert.ToInt32(debugSplit[4])));
					target_character.faceDirection(facingDirection);
					target_character.controller = null;
					target_character.Halt();
					break;
				}
				case "ws":
				case "warpshop":
					if (debugSplit.Length < 2)
					{
						debugOutput = "Missing argument. Run as: 'warpshop <npcname>'";
						break;
					}
					switch (debugSplit[1].ToLower())
					{
					case "pierre":
						parseDebugInput("warp SeedShop 4 19");
						parseDebugInput("wct Pierre SeedShop 4 17");
						break;
					case "robin":
						parseDebugInput("warp ScienceHouse 8 20");
						parseDebugInput("wct Robin ScienceHouse 8 18");
						break;
					case "krobus":
						parseDebugInput("warp Sewer 31 19");
						break;
					case "sandy":
						parseDebugInput("warp SandyHouse 2 7");
						parseDebugInput("wct Sandy SandyHouse 2 5");
						break;
					case "marnie":
						parseDebugInput("warp AnimalShop 12 16");
						parseDebugInput("wct Marnie AnimalShop 12 14");
						break;
					case "clint":
						parseDebugInput("warp Blacksmith 3 15");
						parseDebugInput("wct Clint Blacksmith 3 13");
						break;
					case "gus":
						parseDebugInput("warp Saloon 10 20");
						parseDebugInput("wct Gus Saloon 10 18");
						break;
					case "willy":
						parseDebugInput("warp FishShop 6 6");
						parseDebugInput("wct Willy FishShop 6 4");
						break;
					case "pam":
						parseDebugInput("warp BusStop 7 12");
						parseDebugInput("wct Pam BusStop 11 10");
						break;
					case "dwarf":
						parseDebugInput("warp Mine 43 7");
						break;
					case "wizard":
						if (!player.eventsSeen.Contains(418172))
						{
							player.eventsSeen.Add(418172);
						}
						player.hasMagicInk = true;
						parseDebugInput("warp WizardHouse 2 14");
						break;
					default:
						debugOutput = "That npc doesn't have a shop or it isn't handled by this command";
						break;
					}
					break;
				case "faceplayer":
					Utility.fuzzyCharacterSearch(debugSplit[1]).faceTowardFarmer = true;
					break;
				case "refuel":
					if (player.getToolFromName("Lantern") != null)
					{
						((Lantern)player.getToolFromName("Lantern")).fuelLeft = 100;
					}
					break;
				case "lantern":
					player.items.Add(new Lantern());
					break;
				case "growgrass":
					currentLocation.spawnWeeds(weedsOnly: false);
					currentLocation.growWeedGrass(Convert.ToInt32(debugSplit[1]));
					break;
				case "blueprint":
					player.blueprints.Add(debugInput.Substring(debugInput.IndexOf(' ')).Trim());
					break;
				case "bluebook":
					player.items.Add(new Blueprints());
					break;
				case "addallcrafting":
					foreach (string s3 in CraftingRecipe.craftingRecipes.Keys)
					{
						player.craftingRecipes.Add(s3, 0);
					}
					break;
				case "animal":
					Utility.addAnimalToFarm(new FarmAnimal(debugInput.Substring(debugInput.IndexOf(' ')).Trim(), multiplayer.getNewID(), player.UniqueMultiplayerID));
					break;
				case "movebuilding":
					getFarm().getBuildingAt(new Vector2(Convert.ToInt32(debugSplit[1]), Convert.ToInt32(debugSplit[2]))).tileX.Value = Convert.ToInt32(debugSplit[3]);
					getFarm().getBuildingAt(new Vector2(Convert.ToInt32(debugSplit[1]), Convert.ToInt32(debugSplit[2]))).tileY.Value = Convert.ToInt32(debugSplit[4]);
					break;
				case "ax":
					player.addItemToInventoryBool(new Axe());
					playSound("coin");
					break;
				case "hoe":
					player.addItemToInventoryBool(new Hoe());
					playSound("coin");
					break;
				case "wateringcan":
				case "can":
					player.addItemToInventoryBool(new WateringCan());
					playSound("coin");
					break;
				case "pickaxe":
				case "pickax":
				case "pick":
					player.addItemToInventoryBool(new Pickaxe());
					playSound("coin");
					break;
				case "wand":
					player.addItemToInventoryBool(new Wand());
					playSound("coin");
					break;
				case "fishing":
					player.FishingLevel = Convert.ToInt32(debugSplit[1]);
					break;
				case "eventover":
					eventFinished();
					break;
				case "fd":
				case "facedirection":
				case "face":
					if (debugSplit[1].Equals("farmer"))
					{
						player.Halt();
						player.completelyStopAnimatingOrDoingAction();
						player.faceDirection(Convert.ToInt32(debugSplit[2]));
					}
					else
					{
						Utility.fuzzyCharacterSearch(debugSplit[1]).faceDirection(Convert.ToInt32(debugSplit[2]));
					}
					break;
				case "note":
					if (!player.archaeologyFound.ContainsKey(102))
					{
						player.archaeologyFound.Add(102, new int[2]);
					}
					player.archaeologyFound[102][0] = 18;
					netWorldState.Value.LostBooksFound.Value = 18;
					currentLocation.readNote(Convert.ToInt32(debugSplit[1]));
					break;
				case "nethost":
					multiplayer.StartServer();
					break;
				case "netjoin":
					activeClickableMenu = new FarmhandMenu();
					break;
				case "levelup":
					if (debugSplit.Length > 3)
					{
						activeClickableMenu = new LevelUpMenu(Convert.ToInt32(debugSplit[1]), Convert.ToInt32(debugSplit[2]));
					}
					else
					{
						activeClickableMenu = new LevelUpMenu(Convert.ToInt32(debugSplit[1]), Convert.ToInt32(debugSplit[2]));
					}
					break;
				case "drawbounds":
					drawbounds = !drawbounds;
					break;
				case "darts":
					currentMinigame = new Darts();
					break;
				case "minegame":
				{
					int game_mode = 3;
					if (debugSplit.Length >= 2 && debugSplit[1] == "infinite")
					{
						game_mode = 2;
					}
					currentMinigame = new MineCart(0, game_mode);
					break;
				}
				case "oldminegame":
					currentMinigame = new OldMineCart(0, 3);
					break;
				case "crane":
					currentMinigame = new CraneGame();
					break;
				case "tailorrecipelisttool":
				case "trlt":
					activeClickableMenu = new TailorRecipeListTool();
					break;
				case "animationpreviewtool":
				case "apt":
					activeClickableMenu = new AnimationPreviewTool();
					break;
				case "createdino":
					currentLocation.characters.Add(new DinoMonster(player.position.Value + new Vector2(100f, 0f)));
					break;
				case "broadcastmail":
					if (debugSplit.Length > 1)
					{
						addMailForTomorrow(string.Join(" ", debugSplit.Skip(1)), noLetter: false, sendToEveryone: true);
					}
					break;
				case "phone":
					ShowTelephoneMenu();
					break;
				case "renovate":
					HouseRenovation.ShowRenovationMenu();
					break;
				case "crib":
					if (getLocationFromName(player.homeLocation.Value) is FarmHouse house)
					{
						int style = Convert.ToInt32(debugSplit[1]);
						house.cribStyle.Value = style;
					}
					break;
				case "testnut":
					createItemDebris(new Object(73, 1), Vector2.Zero, 2);
					break;
				case "pstm":
				case "pathspousetome":
					if (player.getSpouse() != null)
					{
						NPC spouse = player.getSpouse();
						if (spouse.currentLocation != currentLocation)
						{
							warpCharacter(spouse, currentLocation.NameOrUniqueName, player.getTileLocationPoint());
						}
						spouse.exploreFarm.Value = true;
						player.getSpouse().PathToOnFarm(player.getTileLocationPoint());
					}
					break;
				case "shufflebundles":
					GenerateBundles(BundleType.Remixed, use_seed: false);
					break;
				case "split":
					if (debugSplit.Length >= 2)
					{
						int player_index = int.Parse(debugSplit[1]);
						GameRunner.instance.AddGameInstance((PlayerIndex)player_index);
					}
					else
					{
						ShowLocalCoopJoinMenu();
					}
					break;
				case "bpm":
				{
					Building building = getFarm().getBuildingAt(player.getTileLocation() + new Vector2(0f, -1f));
					if (building != null)
					{
						activeClickableMenu = new BuildingPaintMenu(building);
						break;
					}
					Farm farm_location = getFarm();
					activeClickableMenu = new BuildingPaintMenu("House", () => (farm_location.paintedHouseTexture != null) ? farm_location.paintedHouseTexture : Farm.houseTextures, farm_location.houseSource.Value, farm_location.housePaintColor.Value);
					break;
				}
				case "minedifficulty":
				case "md":
					if (debugSplit.Length > 1)
					{
						netWorldState.Value.MinesDifficulty = Convert.ToInt32(debugSplit[1]);
					}
					debugOutput = "Mine difficulty: " + netWorldState.Value.MinesDifficulty;
					break;
				case "skullcavedifficulty":
				case "scd":
					netWorldState.Value.SkullCavesDifficulty = Convert.ToInt32(debugSplit[1]);
					debugOutput = "Skull Cave difficulty: " + netWorldState.Value.SkullCavesDifficulty;
					break;
				case "tls":
					useUnscaledLighting = !useUnscaledLighting;
					debugOutput = "Toggled Lighting Scale: useUnscaledLighting: " + useUnscaledLighting;
					break;
				case "fixweapons":
					applySaveFix(SaveGame.SaveFixes.ResetForges);
					debugOutput = "Reset forged weapon attributes.";
					break;
				case "pgb":
					debugOutput = "Gem birds: North " + IslandGemBird.GetBirdTypeForLocation("IslandNorth").ToString() + " South " + IslandGemBird.GetBirdTypeForLocation("IslandSouth").ToString() + " East " + IslandGemBird.GetBirdTypeForLocation("IslandEast").ToString() + " West " + IslandGemBird.GetBirdTypeForLocation("IslandWest");
					break;
				case "ppp":
				case "printplayerpos":
					debugOutput = "Player tile position is " + player.getTileLocation().ToString() + " (World position: " + player.Position.ToString() + ")";
					break;
				case "showplurals":
				{
					List<string> item_names = new List<string>();
					foreach (string data in objectInformation.Values)
					{
						item_names.Add(data.Split('/')[0]);
					}
					foreach (string data2 in bigCraftablesInformation.Values)
					{
						item_names.Add(data2.Split('/')[0]);
					}
					item_names.Sort();
					foreach (string item4 in item_names)
					{
						Console.WriteLine(Lexicon.makePlural(item4));
					}
					break;
				}
				case "rm":
				case "runmacro":
				{
					if (isRunningMacro)
					{
						debugOutput = "You cannot run a macro from within a macro.";
						break;
					}
					isRunningMacro = true;
					string macro_file = "macro.txt";
					if (debugSplit.Length > 1)
					{
						macro_file = string.Join(" ", debugSplit.Skip(1)) + ".txt";
					}
					try
					{
						StreamReader file = new StreamReader(macro_file);
						string line = "";
						while ((line = file.ReadLine()) != null)
						{
							chatBox.textBoxEnter(line);
						}
						debugOutput = "Executed macro file " + macro_file;
						file.Close();
					}
					catch (Exception e2)
					{
						debugOutput = "Error running macro file " + macro_file + "(" + e2.Message + ")";
					}
					isRunningMacro = false;
					break;
				}
				case "invitemovie":
				{
					if (debugSplit.Length < 2)
					{
						debugOutput = "/inviteMovie (npc)";
						break;
					}
					NPC invited_npc = Utility.fuzzyCharacterSearch(debugSplit[1]);
					if (invited_npc == null)
					{
						debugOutput = "Invalid NPC";
					}
					else
					{
						MovieTheater.Invite(player, invited_npc);
					}
					break;
				}
				case "monster":
				{
					Type monsterType = Type.GetType("StardewValley.Monsters." + debugSplit[1]);
					Vector2 pos = new Vector2(Convert.ToSingle(debugSplit[2]), Convert.ToSingle(debugSplit[3])) * 64f;
					object[] args;
					if (debugSplit.Length > 4)
					{
						string monster_name = string.Join(" ", debugSplit.Skip(4));
						int number_arg = -1;
						args = ((!int.TryParse(monster_name, out number_arg)) ? new object[2] { pos, monster_name } : new object[2] { pos, number_arg });
					}
					else
					{
						args = new object[1] { pos };
					}
					Monster mon = Activator.CreateInstance(monsterType, args) as Monster;
					currentLocation.characters.Add(mon);
					break;
				}
				case "shaft":
				case "ladder":
					if (debugSplit.Length > 1)
					{
						mine.createLadderDown(Convert.ToInt32(debugSplit[1]), Convert.ToInt32(debugSplit[2]), debugSplit[0] == "shaft");
					}
					else
					{
						mine.createLadderDown(player.getTileX(), player.getTileY() + 1, debugSplit[0] == "shaft");
					}
					break;
				case "netlog":
					multiplayer.logging.IsLogging = !multiplayer.logging.IsLogging;
					debugOutput = "Turned " + (multiplayer.logging.IsLogging ? "on" : "off") + " network write logging";
					break;
				case "netclear":
					multiplayer.logging.Clear();
					break;
				case "netdump":
					debugOutput = "Wrote log to " + multiplayer.logging.Dump();
					break;
				case "logbandwidth":
					if (IsServer)
					{
						server.LogBandwidth = !server.LogBandwidth;
						debugOutput = "Turned " + (server.LogBandwidth ? "on" : "off") + " server bandwidth logging";
					}
					else if (IsClient)
					{
						client.LogBandwidth = !client.LogBandwidth;
						debugOutput = "Turned " + (client.LogBandwidth ? "on" : "off") + " client bandwidth logging";
					}
					else
					{
						debugOutput = "Cannot toggle bandwidth logging in non-multiplayer games";
					}
					break;
				case "changewallet":
					if (IsMasterGame)
					{
						player.changeWalletTypeTonight.Value = true;
					}
					break;
				case "separatewallets":
					if (IsMasterGame)
					{
						ManorHouse.SeparateWallets();
					}
					break;
				case "mergewallets":
					if (IsMasterGame)
					{
						ManorHouse.MergeWallets();
					}
					break;
				case "nd":
				case "newday":
				case "sleep":
					player.isInBed.Value = true;
					player.sleptInTemporaryBed.Value = true;
					currentLocation.answerDialogueAction("Sleep_Yes", null);
					break;
				case "gm":
				case "inv":
				case "invincible":
					if (player.temporarilyInvincible)
					{
						player.temporaryInvincibilityTimer = 0;
						playSound("bigDeSelect");
					}
					else
					{
						player.temporarilyInvincible = true;
						player.temporaryInvincibilityTimer = -1000000000;
						playSound("bigSelect");
					}
					break;
				default:
					return false;
				case "break":
					break;
				}
				return true;
			}
			catch (Exception e)
			{
				Console.WriteLine("Debug command error: " + e);
				debugOutput = e.Message;
				return false;
			}
		}

		public void RecountWalnuts()
		{
			if (!IsMasterGame || !(getLocationFromName("IslandHut") is IslandHut hut))
			{
				return;
			}
			int missing_nuts = hut.ShowNutHint();
			int current_nut_count = 130 - missing_nuts;
			netWorldState.Value.GoldenWalnutsFound.Value = current_nut_count;
			foreach (GameLocation location in locations)
			{
				if (!(location is IslandLocation))
				{
					continue;
				}
				foreach (ParrotUpgradePerch perch in (location as IslandLocation).parrotUpgradePerches)
				{
					if (perch.currentState.Value == ParrotUpgradePerch.UpgradeState.Complete)
					{
						current_nut_count -= (int)perch.requiredNuts;
					}
				}
			}
			if (MasterPlayer.hasOrWillReceiveMail("Island_VolcanoShortcutOut"))
			{
				current_nut_count -= 5;
			}
			if (MasterPlayer.hasOrWillReceiveMail("Island_VolcanoBridge"))
			{
				current_nut_count -= 5;
			}
			netWorldState.Value.GoldenWalnuts.Value = current_nut_count;
		}

		public void ResetIslandLocations()
		{
			netWorldState.Value.GoldenWalnutsFound.Value = 0;
			string[] other_mails_to_remove = new string[14]
			{
				"birdieQuestBegun", "birdieQuestFinished", "tigerSlimeNut", "Island_W_BuriedTreasureNut", "Island_W_BuriedTreasure", "islandNorthCaveOpened", "Saw_Flame_Sprite_North_North", "Saw_Flame_Sprite_North_South", "Island_N_BuriedTreasureNut", "Island_W_BuriedTreasure",
				"Saw_Flame_Sprite_South", "Visited_Island", "Island_FirstParrot", "gotBirdieReward"
			};
			player.team.collectedNutTracker.Clear();
			for (int m = 0; m < player.mailReceived.Count; m++)
			{
				if (player.mailReceived[m].StartsWith("Island_Upgrade"))
				{
					player.mailReceived.RemoveAt(m);
					m--;
					continue;
				}
				for (int n = 0; n < other_mails_to_remove.Length; n++)
				{
					if (player.mailReceived[m].Contains(other_mails_to_remove[n]))
					{
						player.mailReceived.RemoveAt(m);
						m--;
						break;
					}
				}
			}
			for (int l = 0; l < player.mailForTomorrow.Count; l++)
			{
				if (player.mailForTomorrow[l].StartsWith("Island_Upgrade"))
				{
					player.mailForTomorrow.RemoveAt(l);
					l--;
				}
				for (int j2 = 0; j2 < other_mails_to_remove.Length; j2++)
				{
					if (player.mailForTomorrow[l].Contains(other_mails_to_remove[j2]))
					{
						player.mailForTomorrow.RemoveAt(l);
						l--;
						break;
					}
				}
			}
			for (int k = 0; k < player.team.broadcastedMail.Count; k++)
			{
				if (player.team.broadcastedMail[k].StartsWith("Island_Upgrade"))
				{
					player.team.broadcastedMail.RemoveAt(k);
					k--;
				}
				for (int j3 = 0; j3 < other_mails_to_remove.Length; j3++)
				{
					if (player.team.broadcastedMail[k].Contains(other_mails_to_remove[j3]))
					{
						player.team.broadcastedMail.RemoveAt(k);
						k--;
						break;
					}
				}
			}
			for (int j = 0; j < player.secretNotesSeen.Count; j++)
			{
				if (player.secretNotesSeen[j] >= 1000)
				{
					player.secretNotesSeen.RemoveAt(j);
					j--;
				}
			}
			player.team.limitedNutDrops.Clear();
			netWorldState.Value.GoldenCoconutCracked.Value = false;
			netWorldState.Value.GoldenWalnuts.Set(0);
			netWorldState.Value.ParrotPlatformsUnlocked.Value = false;
			netWorldState.Value.FoundBuriedNuts.Clear();
			for (int i = 0; i < locations.Count; i++)
			{
				GameLocation location = locations[i];
				if (location.GetLocationContext() == GameLocation.LocationContext.Island)
				{
					_locationLookup.Clear();
					string map_path = location.mapPath.Value;
					string location_name = location.name.Value;
					object[] args = new object[2] { map_path, location_name };
					GameLocation new_instance = null;
					try
					{
						new_instance = Activator.CreateInstance(location.GetType(), args) as GameLocation;
					}
					catch (Exception)
					{
						new_instance = Activator.CreateInstance(location.GetType()) as GameLocation;
					}
					locations[i] = new_instance;
					_locationLookup.Clear();
				}
			}
			addBirdieIfNecessary();
		}

		public void ShowTelephoneMenu()
		{
			playSound("openBox");
			List<Response> responses = new List<Response>();
			responses.Add(new Response("Carpenter", getCharacterFromName("Robin").displayName));
			responses.Add(new Response("Blacksmith", getCharacterFromName("Clint").displayName));
			responses.Add(new Response("SeedShop", getCharacterFromName("Pierre").displayName));
			responses.Add(new Response("AnimalShop", getCharacterFromName("Marnie").displayName));
			responses.Add(new Response("Saloon", getCharacterFromName("Gus").displayName));
			if (player.mailReceived.Contains("Gil_Telephone"))
			{
				responses.Add(new Response("AdventureGuild", getCharacterFromName("Marlon").displayName));
			}
			responses.Add(new Response("HangUp", content.LoadString("Strings\\Locations:MineCart_Destination_Cancel")));
			currentLocation.createQuestionDialogue(content.LoadString("Strings\\Characters:Phone_SelectNumber"), responses.ToArray(), "telephone");
		}

		public void requestDebugInput()
		{
			chatBox.activate();
			chatBox.setText("/");
		}

		private void makeCelebrationWeatherDebris()
		{
			debrisWeather.Clear();
			isDebrisWeather = true;
			int debrisToMake = random.Next(80, 100);
			int baseIndex = 22;
			for (int i = 0; i < debrisToMake; i++)
			{
				debrisWeather.Add(new WeatherDebris(new Vector2(random.Next(0, graphics.GraphicsDevice.Viewport.Width), random.Next(0, graphics.GraphicsDevice.Viewport.Height)), baseIndex + random.Next(2), (float)random.Next(15) / 500f, (float)random.Next(-10, 0) / 50f, (float)random.Next(10) / 50f));
			}
		}

		private void panModeSuccess(KeyboardState currentKBState)
		{
			panFacingDirectionWait = false;
			playSound("smallSelect");
			if (currentKBState.IsKeyDown(Keys.LeftShift))
			{
				panModeString += " (animation_name_here)";
			}
			debugOutput = panModeString;
		}

		private void updatePanModeControls(MouseState currentMouseState, KeyboardState currentKBState)
		{
			if (currentKBState.IsKeyDown(Keys.F8) && !oldKBState.IsKeyDown(Keys.F8))
			{
				requestDebugInput();
				return;
			}
			if (!panFacingDirectionWait)
			{
				if (currentKBState.IsKeyDown(Keys.W))
				{
					viewport.Y -= 16;
				}
				if (currentKBState.IsKeyDown(Keys.A))
				{
					viewport.X -= 16;
				}
				if (currentKBState.IsKeyDown(Keys.S))
				{
					viewport.Y += 16;
				}
				if (currentKBState.IsKeyDown(Keys.D))
				{
					viewport.X += 16;
				}
			}
			else
			{
				if (currentKBState.IsKeyDown(Keys.W))
				{
					panModeString += "0";
					panModeSuccess(currentKBState);
				}
				if (currentKBState.IsKeyDown(Keys.A))
				{
					panModeString += "3";
					panModeSuccess(currentKBState);
				}
				if (currentKBState.IsKeyDown(Keys.S))
				{
					panModeString += "2";
					panModeSuccess(currentKBState);
				}
				if (currentKBState.IsKeyDown(Keys.D))
				{
					panModeString += "1";
					panModeSuccess(currentKBState);
				}
			}
			if (getMouseX(ui_scale: false) < 192)
			{
				viewport.X -= 8;
				viewport.X -= (192 - getMouseX()) / 8;
			}
			if (getMouseX(ui_scale: false) > viewport.Width - 192)
			{
				viewport.X += 8;
				viewport.X += (getMouseX() - viewport.Width + 192) / 8;
			}
			if (getMouseY(ui_scale: false) < 192)
			{
				viewport.Y -= 8;
				viewport.Y -= (192 - getMouseY()) / 8;
			}
			if (getMouseY(ui_scale: false) > viewport.Height - 192)
			{
				viewport.Y += 8;
				viewport.Y += (getMouseY() - viewport.Height + 192) / 8;
			}
			if (currentMouseState.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton == ButtonState.Released && panModeString != null && panModeString.Length > 0)
			{
				int x2 = (getMouseX() + viewport.X) / 64;
				int y2 = (getMouseY() + viewport.Y) / 64;
				panModeString = panModeString + currentLocation.Name + " " + x2 + " " + y2 + " ";
				panFacingDirectionWait = true;
				currentLocation.playTerrainSound(new Vector2(x2, y2));
				debugOutput = panModeString;
			}
			if (currentMouseState.RightButton == ButtonState.Pressed && oldMouseState.RightButton == ButtonState.Released)
			{
				int x = getMouseX() + viewport.X;
				int y = getMouseY() + viewport.Y;
				Warp w2 = currentLocation.isCollidingWithWarpOrDoor(new Microsoft.Xna.Framework.Rectangle(x, y, 1, 1));
				if (w2 != null)
				{
					currentLocation = getLocationFromName(w2.TargetName);
					currentLocation.map.LoadTileSheets(mapDisplayDevice);
					viewport.X = w2.TargetX * 64 - viewport.Width / 2;
					viewport.Y = w2.TargetY * 64 - viewport.Height / 2;
					playSound("dwop");
				}
			}
			if (currentKBState.IsKeyDown(Keys.Escape) && !oldKBState.IsKeyDown(Keys.Escape))
			{
				Warp w = currentLocation.warps[0];
				currentLocation = getLocationFromName(w.TargetName);
				currentLocation.map.LoadTileSheets(mapDisplayDevice);
				viewport.X = w.TargetX * 64 - viewport.Width / 2;
				viewport.Y = w.TargetY * 64 - viewport.Height / 2;
				playSound("dwop");
			}
			if (viewport.X < -64)
			{
				viewport.X = -64;
			}
			if (viewport.X + viewport.Width > currentLocation.Map.Layers[0].LayerWidth * 64 + 128)
			{
				viewport.X = currentLocation.Map.Layers[0].LayerWidth * 64 + 128 - viewport.Width;
			}
			if (viewport.Y < -64)
			{
				viewport.Y = -64;
			}
			if (viewport.Y + viewport.Height > currentLocation.Map.Layers[0].LayerHeight * 64 + 128)
			{
				viewport.Y = currentLocation.Map.Layers[0].LayerHeight * 64 + 128 - viewport.Height;
			}
			oldMouseState = input.GetMouseState();
			oldKBState = currentKBState;
		}

		public static bool isLocationAccessible(string locationName)
		{
			switch (locationName)
			{
			case "CommunityCenter":
				if (player.eventsSeen.Contains(191393))
				{
					return true;
				}
				break;
			case "JojaMart":
				if (!Utility.HasAnyPlayerSeenEvent(191393))
				{
					return true;
				}
				break;
			case "Railroad":
				if (stats.DaysPlayed > 31)
				{
					return true;
				}
				break;
			default:
				return true;
			}
			return false;
		}

		public static bool isDPadPressed()
		{
			return isDPadPressed(input.GetGamePadState());
		}

		public static bool isDPadPressed(GamePadState pad_state)
		{
			if (pad_state.DPad.Up == ButtonState.Pressed || pad_state.DPad.Down == ButtonState.Pressed || pad_state.DPad.Left == ButtonState.Pressed || pad_state.DPad.Right == ButtonState.Pressed)
			{
				return true;
			}
			return false;
		}

		public static bool isGamePadThumbstickInMotion(double threshold = 0.2)
		{
			bool inMotion = false;
			GamePadState p = input.GetGamePadState();
			if ((double)p.ThumbSticks.Left.X < 0.0 - threshold || p.IsButtonDown(Buttons.LeftThumbstickLeft))
			{
				inMotion = true;
			}
			if ((double)p.ThumbSticks.Left.X > threshold || p.IsButtonDown(Buttons.LeftThumbstickRight))
			{
				inMotion = true;
			}
			if ((double)p.ThumbSticks.Left.Y < 0.0 - threshold || p.IsButtonDown(Buttons.LeftThumbstickUp))
			{
				inMotion = true;
			}
			if ((double)p.ThumbSticks.Left.Y > threshold || p.IsButtonDown(Buttons.LeftThumbstickDown))
			{
				inMotion = true;
			}
			if ((double)p.ThumbSticks.Right.X < 0.0 - threshold)
			{
				inMotion = true;
			}
			if ((double)p.ThumbSticks.Right.X > threshold)
			{
				inMotion = true;
			}
			if ((double)p.ThumbSticks.Right.Y < 0.0 - threshold)
			{
				inMotion = true;
			}
			if ((double)p.ThumbSticks.Right.Y > threshold)
			{
				inMotion = true;
			}
			if (inMotion)
			{
				thumbstickMotionMargin = 50;
			}
			return thumbstickMotionMargin > 0;
		}

		public static bool isAnyGamePadButtonBeingPressed()
		{
			return Utility.getPressedButtons(input.GetGamePadState(), oldPadState).Count > 0;
		}

		public static bool isAnyGamePadButtonBeingHeld()
		{
			return Utility.getHeldButtons(input.GetGamePadState()).Count > 0;
		}

		private static void UpdateChatBox()
		{
			if (chatBox == null)
			{
				return;
			}
			KeyboardState keyState = input.GetKeyboardState();
			GamePadState padState = input.GetGamePadState();
			if (IsChatting)
			{
				if (textEntry != null)
				{
					return;
				}
				if (padState.IsButtonDown(Buttons.A))
				{
					MouseState mouse = input.GetMouseState();
					if (chatBox != null && chatBox.isActive() && !chatBox.isHoveringOverClickable(mouse.X, mouse.Y))
					{
						oldPadState = padState;
						oldKBState = keyState;
						showTextEntry(chatBox.chatBox);
					}
				}
				if (keyState.IsKeyDown(Keys.Escape) || padState.IsButtonDown(Buttons.B) || padState.IsButtonDown(Buttons.Back))
				{
					chatBox.clickAway();
					oldKBState = keyState;
				}
			}
			else if (keyboardDispatcher.Subscriber == null && ((isOneOfTheseKeysDown(keyState, options.chatButton) && game1.HasKeyboardFocus()) || (!padState.IsButtonDown(Buttons.RightStick) && rightStickHoldTime > 0 && rightStickHoldTime < emoteMenuShowTime)))
			{
				chatBox.activate();
				if (keyState.IsKeyDown(Keys.OemQuestion))
				{
					chatBox.setText("/");
				}
			}
		}

		public static KeyboardState GetKeyboardState()
		{
			KeyboardState keyState = input.GetKeyboardState();
			if (chatBox != null)
			{
				if (IsChatting)
				{
					return default(KeyboardState);
				}
				if (keyboardDispatcher.Subscriber == null && isOneOfTheseKeysDown(keyState, options.chatButton) && game1.HasKeyboardFocus())
				{
					return default(KeyboardState);
				}
			}
			return keyState;
		}

		private void UpdateControlInput(GameTime time)
		{
			KeyboardState currentKBState = GetKeyboardState();
			MouseState currentMouseState = input.GetMouseState();
			GamePadState currentPadState = input.GetGamePadState();
			if (ticks < _activatedTick + 2 && oldKBState.IsKeyDown(Keys.Tab) != currentKBState.IsKeyDown(Keys.Tab))
			{
				List<Keys> keys = oldKBState.GetPressedKeys().ToList();
				if (currentKBState.IsKeyDown(Keys.Tab))
				{
					keys.Add(Keys.Tab);
				}
				else
				{
					keys.Remove(Keys.Tab);
				}
				oldKBState = new KeyboardState(keys.ToArray());
			}
			hooks.OnGame1_UpdateControlInput(ref currentKBState, ref currentMouseState, ref currentPadState, delegate
			{
				if (options.gamepadControls)
				{
					bool flag = false;
					if (Math.Abs(currentPadState.ThumbSticks.Right.X) > 0f || Math.Abs(currentPadState.ThumbSticks.Right.Y) > 0f)
					{
						setMousePositionRaw((int)((float)currentMouseState.X + currentPadState.ThumbSticks.Right.X * thumbstickToMouseModifier), (int)((float)currentMouseState.Y - currentPadState.ThumbSticks.Right.Y * thumbstickToMouseModifier));
						flag = true;
					}
					if (IsChatting)
					{
						flag = true;
					}
					if (((getMouseX() != getOldMouseX() || getMouseY() != getOldMouseY()) && getMouseX() != 0 && getMouseY() != 0) || flag)
					{
						if (flag)
						{
							if (timerUntilMouseFade <= 0)
							{
								lastMousePositionBeforeFade = new Point(localMultiplayerWindow.Width / 2, localMultiplayerWindow.Height / 2);
							}
						}
						else
						{
							lastCursorMotionWasMouse = true;
						}
						if (timerUntilMouseFade <= 0 && !lastCursorMotionWasMouse)
						{
							setMousePositionRaw(lastMousePositionBeforeFade.X, lastMousePositionBeforeFade.Y);
						}
						timerUntilMouseFade = 4000;
					}
				}
				else if (getMouseX() != getOldMouseX() || getMouseY() != getOldMouseY())
				{
					lastCursorMotionWasMouse = true;
				}
				bool actionButtonPressed = false;
				bool switchToolButtonPressed = false;
				bool useToolButtonPressed = false;
				bool useToolButtonReleased = false;
				bool addItemToInventoryButtonPressed = false;
				bool cancelButtonPressed = false;
				bool moveUpPressed = false;
				bool moveRightPressed = false;
				bool moveLeftPressed = false;
				bool moveDownPressed = false;
				bool moveUpReleased = false;
				bool moveRightReleased = false;
				bool moveDownReleased = false;
				bool moveLeftReleased = false;
				bool moveUpHeld = false;
				bool moveRightHeld = false;
				bool moveDownHeld = false;
				bool moveLeftHeld = false;
				bool flag2 = false;
				if ((isOneOfTheseKeysDown(currentKBState, options.actionButton) && areAllOfTheseKeysUp(oldKBState, options.actionButton)) || (currentMouseState.RightButton == ButtonState.Pressed && oldMouseState.RightButton == ButtonState.Released))
				{
					actionButtonPressed = true;
					rightClickPolling = 250;
				}
				if ((isOneOfTheseKeysDown(currentKBState, options.useToolButton) && areAllOfTheseKeysUp(oldKBState, options.useToolButton)) || (currentMouseState.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton == ButtonState.Released))
				{
					useToolButtonPressed = true;
				}
				if ((areAllOfTheseKeysUp(currentKBState, options.useToolButton) && isOneOfTheseKeysDown(oldKBState, options.useToolButton)) || (currentMouseState.LeftButton == ButtonState.Released && oldMouseState.LeftButton == ButtonState.Pressed))
				{
					useToolButtonReleased = true;
				}
				if (currentMouseState.ScrollWheelValue != oldMouseState.ScrollWheelValue)
				{
					switchToolButtonPressed = true;
				}
				if ((isOneOfTheseKeysDown(currentKBState, options.cancelButton) && areAllOfTheseKeysUp(oldKBState, options.cancelButton)) || (currentMouseState.RightButton == ButtonState.Pressed && oldMouseState.RightButton == ButtonState.Released))
				{
					cancelButtonPressed = true;
				}
				if (isOneOfTheseKeysDown(currentKBState, options.moveUpButton) && areAllOfTheseKeysUp(oldKBState, options.moveUpButton))
				{
					moveUpPressed = true;
				}
				if (isOneOfTheseKeysDown(currentKBState, options.moveRightButton) && areAllOfTheseKeysUp(oldKBState, options.moveRightButton))
				{
					moveRightPressed = true;
				}
				if (isOneOfTheseKeysDown(currentKBState, options.moveDownButton) && areAllOfTheseKeysUp(oldKBState, options.moveDownButton))
				{
					moveDownPressed = true;
				}
				if (isOneOfTheseKeysDown(currentKBState, options.moveLeftButton) && areAllOfTheseKeysUp(oldKBState, options.moveLeftButton))
				{
					moveLeftPressed = true;
				}
				if (areAllOfTheseKeysUp(currentKBState, options.moveUpButton) && isOneOfTheseKeysDown(oldKBState, options.moveUpButton))
				{
					moveUpReleased = true;
				}
				if (areAllOfTheseKeysUp(currentKBState, options.moveRightButton) && isOneOfTheseKeysDown(oldKBState, options.moveRightButton))
				{
					moveRightReleased = true;
				}
				if (areAllOfTheseKeysUp(currentKBState, options.moveDownButton) && isOneOfTheseKeysDown(oldKBState, options.moveDownButton))
				{
					moveDownReleased = true;
				}
				if (areAllOfTheseKeysUp(currentKBState, options.moveLeftButton) && isOneOfTheseKeysDown(oldKBState, options.moveLeftButton))
				{
					moveLeftReleased = true;
				}
				if (isOneOfTheseKeysDown(currentKBState, options.moveUpButton))
				{
					moveUpHeld = true;
				}
				if (isOneOfTheseKeysDown(currentKBState, options.moveRightButton))
				{
					moveRightHeld = true;
				}
				if (isOneOfTheseKeysDown(currentKBState, options.moveDownButton))
				{
					moveDownHeld = true;
				}
				if (isOneOfTheseKeysDown(currentKBState, options.moveLeftButton))
				{
					moveLeftHeld = true;
				}
				if (isOneOfTheseKeysDown(currentKBState, options.useToolButton) || currentMouseState.LeftButton == ButtonState.Pressed)
				{
					flag2 = true;
				}
				if (isOneOfTheseKeysDown(currentKBState, options.actionButton) || currentMouseState.RightButton == ButtonState.Pressed)
				{
					rightClickPolling -= time.ElapsedGameTime.Milliseconds;
					if (rightClickPolling <= 0)
					{
						rightClickPolling = 100;
						actionButtonPressed = true;
					}
				}
				if (options.gamepadControls)
				{
					if (currentKBState.GetPressedKeys().Length != 0 || currentMouseState.LeftButton == ButtonState.Pressed || currentMouseState.RightButton == ButtonState.Pressed)
					{
						timerUntilMouseFade = 4000;
					}
					if (currentPadState.IsButtonDown(Buttons.A) && !oldPadState.IsButtonDown(Buttons.A))
					{
						actionButtonPressed = true;
						lastCursorMotionWasMouse = false;
						rightClickPolling = 250;
					}
					if (currentPadState.IsButtonDown(Buttons.X) && !oldPadState.IsButtonDown(Buttons.X))
					{
						useToolButtonPressed = true;
						lastCursorMotionWasMouse = false;
					}
					if (!currentPadState.IsButtonDown(Buttons.X) && oldPadState.IsButtonDown(Buttons.X))
					{
						useToolButtonReleased = true;
					}
					if (currentPadState.IsButtonDown(Buttons.RightTrigger) && !oldPadState.IsButtonDown(Buttons.RightTrigger))
					{
						switchToolButtonPressed = true;
						triggerPolling = 300;
					}
					else if (currentPadState.IsButtonDown(Buttons.LeftTrigger) && !oldPadState.IsButtonDown(Buttons.LeftTrigger))
					{
						switchToolButtonPressed = true;
						triggerPolling = 300;
					}
					if (currentPadState.IsButtonDown(Buttons.X))
					{
						flag2 = true;
					}
					if (currentPadState.IsButtonDown(Buttons.A))
					{
						rightClickPolling -= time.ElapsedGameTime.Milliseconds;
						if (rightClickPolling <= 0)
						{
							rightClickPolling = 100;
							actionButtonPressed = true;
						}
					}
					if (currentPadState.IsButtonDown(Buttons.RightTrigger) || currentPadState.IsButtonDown(Buttons.LeftTrigger))
					{
						triggerPolling -= time.ElapsedGameTime.Milliseconds;
						if (triggerPolling <= 0)
						{
							triggerPolling = 100;
							switchToolButtonPressed = true;
						}
					}
					if (currentPadState.IsButtonDown(Buttons.RightShoulder) && !oldPadState.IsButtonDown(Buttons.RightShoulder))
					{
						player.shiftToolbar(right: true);
					}
					if (currentPadState.IsButtonDown(Buttons.LeftShoulder) && !oldPadState.IsButtonDown(Buttons.LeftShoulder))
					{
						player.shiftToolbar(right: false);
					}
					if (currentPadState.IsButtonDown(Buttons.DPadUp) && !oldPadState.IsButtonDown(Buttons.DPadUp))
					{
						moveUpPressed = true;
					}
					else if (!currentPadState.IsButtonDown(Buttons.DPadUp) && oldPadState.IsButtonDown(Buttons.DPadUp))
					{
						moveUpReleased = true;
					}
					if (currentPadState.IsButtonDown(Buttons.DPadRight) && !oldPadState.IsButtonDown(Buttons.DPadRight))
					{
						moveRightPressed = true;
					}
					else if (!currentPadState.IsButtonDown(Buttons.DPadRight) && oldPadState.IsButtonDown(Buttons.DPadRight))
					{
						moveRightReleased = true;
					}
					if (currentPadState.IsButtonDown(Buttons.DPadDown) && !oldPadState.IsButtonDown(Buttons.DPadDown))
					{
						moveDownPressed = true;
					}
					else if (!currentPadState.IsButtonDown(Buttons.DPadDown) && oldPadState.IsButtonDown(Buttons.DPadDown))
					{
						moveDownReleased = true;
					}
					if (currentPadState.IsButtonDown(Buttons.DPadLeft) && !oldPadState.IsButtonDown(Buttons.DPadLeft))
					{
						moveLeftPressed = true;
					}
					else if (!currentPadState.IsButtonDown(Buttons.DPadLeft) && oldPadState.IsButtonDown(Buttons.DPadLeft))
					{
						moveLeftReleased = true;
					}
					if (currentPadState.IsButtonDown(Buttons.DPadUp))
					{
						moveUpHeld = true;
					}
					if (currentPadState.IsButtonDown(Buttons.DPadRight))
					{
						moveRightHeld = true;
					}
					if (currentPadState.IsButtonDown(Buttons.DPadDown))
					{
						moveDownHeld = true;
					}
					if (currentPadState.IsButtonDown(Buttons.DPadLeft))
					{
						moveLeftHeld = true;
					}
					if ((double)currentPadState.ThumbSticks.Left.X < -0.2)
					{
						moveLeftPressed = true;
						moveLeftHeld = true;
					}
					else if ((double)currentPadState.ThumbSticks.Left.X > 0.2)
					{
						moveRightPressed = true;
						moveRightHeld = true;
					}
					if ((double)currentPadState.ThumbSticks.Left.Y < -0.2)
					{
						moveDownPressed = true;
						moveDownHeld = true;
					}
					else if ((double)currentPadState.ThumbSticks.Left.Y > 0.2)
					{
						moveUpPressed = true;
						moveUpHeld = true;
					}
					if ((double)oldPadState.ThumbSticks.Left.X < -0.2 && !moveLeftHeld)
					{
						moveLeftReleased = true;
					}
					if ((double)oldPadState.ThumbSticks.Left.X > 0.2 && !moveRightHeld)
					{
						moveRightReleased = true;
					}
					if ((double)oldPadState.ThumbSticks.Left.Y < -0.2 && !moveDownHeld)
					{
						moveDownReleased = true;
					}
					if ((double)oldPadState.ThumbSticks.Left.Y > 0.2 && !moveUpHeld)
					{
						moveUpReleased = true;
					}
					if (controllerSlingshotSafeTime > 0f)
					{
						if (!currentPadState.IsButtonDown(Buttons.DPadUp) && !currentPadState.IsButtonDown(Buttons.DPadDown) && !currentPadState.IsButtonDown(Buttons.DPadLeft) && !currentPadState.IsButtonDown(Buttons.DPadRight) && (double)Math.Abs(currentPadState.ThumbSticks.Left.X) < 0.04 && (double)Math.Abs(currentPadState.ThumbSticks.Left.Y) < 0.04)
						{
							controllerSlingshotSafeTime = 0f;
						}
						if (controllerSlingshotSafeTime <= 0f)
						{
							controllerSlingshotSafeTime = 0f;
						}
						else
						{
							controllerSlingshotSafeTime -= (float)time.ElapsedGameTime.TotalSeconds;
							moveUpPressed = false;
							moveDownPressed = false;
							moveLeftPressed = false;
							moveRightPressed = false;
							moveUpHeld = false;
							moveDownHeld = false;
							moveLeftHeld = false;
							moveRightHeld = false;
						}
					}
				}
				else
				{
					controllerSlingshotSafeTime = 0f;
				}
				ResetFreeCursorDrag();
				if (flag2)
				{
					mouseClickPolling += time.ElapsedGameTime.Milliseconds;
				}
				else
				{
					mouseClickPolling = 0;
				}
				if (isOneOfTheseKeysDown(currentKBState, options.toolbarSwap) && areAllOfTheseKeysUp(oldKBState, options.toolbarSwap))
				{
					player.shiftToolbar((!currentKBState.IsKeyDown(Keys.LeftControl)) ? true : false);
				}
				if (mouseClickPolling > 250 && (player.CurrentTool == null || !(player.CurrentTool is FishingRod) || (int)player.CurrentTool.upgradeLevel <= 0))
				{
					useToolButtonPressed = true;
					mouseClickPolling = 100;
				}
				PushUIMode();
				foreach (IClickableMenu current in onScreenMenus)
				{
					if ((displayHUD || current == chatBox) && wasMouseVisibleThisFrame && current.isWithinBounds(getMouseX(), getMouseY()))
					{
						current.performHoverAction(getMouseX(), getMouseY());
					}
				}
				PopUIMode();
				if (chatBox != null && chatBox.chatBox.Selected && oldMouseState.ScrollWheelValue != currentMouseState.ScrollWheelValue)
				{
					chatBox.receiveScrollWheelAction(currentMouseState.ScrollWheelValue - oldMouseState.ScrollWheelValue);
				}
				if (panMode)
				{
					updatePanModeControls(currentMouseState, currentKBState);
				}
				else
				{
					if (inputSimulator != null)
					{
						if (currentKBState.IsKeyDown(Keys.Escape))
						{
							inputSimulator = null;
						}
						else
						{
							inputSimulator.SimulateInput(ref actionButtonPressed, ref switchToolButtonPressed, ref useToolButtonPressed, ref useToolButtonReleased, ref addItemToInventoryButtonPressed, ref cancelButtonPressed, ref moveUpPressed, ref moveRightPressed, ref moveLeftPressed, ref moveDownPressed, ref moveUpReleased, ref moveRightReleased, ref moveLeftReleased, ref moveDownReleased, ref moveUpHeld, ref moveRightHeld, ref moveLeftHeld, ref moveDownHeld);
						}
					}
					if (useToolButtonReleased && player.CurrentTool != null && CurrentEvent == null && pauseTime <= 0f && player.CurrentTool.onRelease(currentLocation, getMouseX(), getMouseY(), player))
					{
						oldMouseState = input.GetMouseState();
						oldKBState = currentKBState;
						oldPadState = currentPadState;
						player.usingSlingshot = false;
						player.canReleaseTool = true;
						player.UsingTool = false;
						player.CanMove = true;
					}
					else
					{
						if (((useToolButtonPressed && !isAnyGamePadButtonBeingPressed()) || (actionButtonPressed && isAnyGamePadButtonBeingPressed())) && pauseTime <= 0f && wasMouseVisibleThisFrame)
						{
							PushUIMode();
							foreach (IClickableMenu current2 in onScreenMenus)
							{
								if (displayHUD || current2 == chatBox)
								{
									if ((!IsChatting || current2 == chatBox) && (!(current2 is LevelUpMenu) || (current2 as LevelUpMenu).informationUp) && current2.isWithinBounds(getMouseX(), getMouseY()))
									{
										current2.receiveLeftClick(getMouseX(), getMouseY());
										PopUIMode();
										oldMouseState = input.GetMouseState();
										oldKBState = currentKBState;
										oldPadState = currentPadState;
										return;
									}
									if (current2 == chatBox && options.gamepadControls && IsChatting)
									{
										oldMouseState = input.GetMouseState();
										oldKBState = currentKBState;
										oldPadState = currentPadState;
										PopUIMode();
										return;
									}
									current2.clickAway();
								}
							}
							PopUIMode();
						}
						if (IsChatting || player.freezePause > 0)
						{
							if (IsChatting)
							{
								ButtonCollection.ButtonEnumerator enumerator2 = Utility.getPressedButtons(currentPadState, oldPadState).GetEnumerator();
								while (enumerator2.MoveNext())
								{
									Buttons current3 = enumerator2.Current;
									chatBox.receiveGamePadButton(current3);
								}
							}
							oldMouseState = input.GetMouseState();
							oldKBState = currentKBState;
							oldPadState = currentPadState;
						}
						else
						{
							if (paused || HostPaused)
							{
								if (!HostPaused || !IsMasterGame || (!isOneOfTheseKeysDown(currentKBState, options.menuButton) && !currentPadState.IsButtonDown(Buttons.B) && !currentPadState.IsButtonDown(Buttons.Back)))
								{
									oldMouseState = input.GetMouseState();
									return;
								}
								netWorldState.Value.IsPaused = false;
								if (chatBox != null)
								{
									chatBox.globalInfoMessage("Resumed");
								}
							}
							if (eventUp)
							{
								if (currentLocation.currentEvent == null && locationRequest == null)
								{
									eventUp = false;
								}
								else if (actionButtonPressed || useToolButtonPressed)
								{
									CurrentEvent?.receiveMouseClick(getMouseX(), getMouseY());
								}
							}
							bool flag3 = eventUp || farmEvent != null;
							if (actionButtonPressed || (dialogueUp && useToolButtonPressed))
							{
								PushUIMode();
								foreach (IClickableMenu current4 in onScreenMenus)
								{
									if (wasMouseVisibleThisFrame && (displayHUD || current4 == chatBox) && current4.isWithinBounds(getMouseX(), getMouseY()) && (!(current4 is LevelUpMenu) || (current4 as LevelUpMenu).informationUp))
									{
										current4.receiveRightClick(getMouseX(), getMouseY());
										oldMouseState = input.GetMouseState();
										if (!isAnyGamePadButtonBeingPressed())
										{
											PopUIMode();
											oldKBState = currentKBState;
											oldPadState = currentPadState;
											return;
										}
									}
								}
								PopUIMode();
								if (!pressActionButton(currentKBState, currentMouseState, currentPadState))
								{
									oldKBState = currentKBState;
									oldMouseState = input.GetMouseState();
									oldPadState = currentPadState;
									return;
								}
							}
							if (useToolButtonPressed && (!player.UsingTool || (player.CurrentTool != null && player.CurrentTool is MeleeWeapon)) && !player.isEating && !pickingTool && !dialogueUp && !menuUp && farmEvent == null && (player.CanMove || (player.CurrentTool != null && (player.CurrentTool.Name.Equals("Fishing Rod") || player.CurrentTool is MeleeWeapon))))
							{
								if (player.CurrentTool != null && (!(player.CurrentTool is MeleeWeapon) || didPlayerJustLeftClick(ignoreNonMouseHeldInput: true)))
								{
									player.FireTool();
								}
								if (!pressUseToolButton() && player.canReleaseTool && player.UsingTool)
								{
									_ = player.CurrentTool;
								}
								if (player.UsingTool)
								{
									oldMouseState = input.GetMouseState();
									oldKBState = currentKBState;
									oldPadState = currentPadState;
									return;
								}
							}
							if (useToolButtonReleased && _didInitiateItemStow)
							{
								_didInitiateItemStow = false;
							}
							if (useToolButtonReleased && player.canReleaseTool && player.UsingTool && player.CurrentTool != null)
							{
								player.EndUsingTool();
							}
							if (switchToolButtonPressed && !player.UsingTool && !dialogueUp && (pickingTool || player.CanMove) && !player.areAllItemsNull() && !flag3)
							{
								pressSwitchToolButton();
							}
							if (cancelButtonPressed)
							{
								if (numberOfSelectedItems != -1)
								{
									numberOfSelectedItems = -1;
									dialogueUp = false;
									player.CanMove = true;
								}
								else if (nameSelectUp && NameSelect.cancel())
								{
									nameSelectUp = false;
									playSound("bigDeSelect");
								}
							}
							if (player.CurrentTool != null && flag2 && player.canReleaseTool && !flag3 && !dialogueUp && !menuUp && player.Stamina >= 1f && !(player.CurrentTool is FishingRod))
							{
								int num = (player.CurrentTool.hasEnchantmentOfType<ReachingToolEnchantment>() ? 1 : 0);
								if (player.toolHold <= 0 && (int)player.CurrentTool.upgradeLevel + num > player.toolPower)
								{
									float num2 = 1f;
									if (player.CurrentTool != null)
									{
										num2 = player.CurrentTool.AnimationSpeedModifier;
									}
									player.toolHold = (int)(600f * num2);
								}
								else if ((int)player.CurrentTool.upgradeLevel + num > player.toolPower)
								{
									player.toolHold -= time.ElapsedGameTime.Milliseconds;
									if (player.toolHold <= 0)
									{
										player.toolPowerIncrease();
									}
								}
							}
							if (upPolling >= 650f)
							{
								moveUpPressed = true;
								upPolling -= 100f;
							}
							else if (downPolling >= 650f)
							{
								moveDownPressed = true;
								downPolling -= 100f;
							}
							else if (rightPolling >= 650f)
							{
								moveRightPressed = true;
								rightPolling -= 100f;
							}
							else if (leftPolling >= 650f)
							{
								moveLeftPressed = true;
								leftPolling -= 100f;
							}
							else if (!nameSelectUp && pauseTime <= 0f && locationRequest == null && !player.UsingTool && (!flag3 || (CurrentEvent != null && CurrentEvent.playerControlSequence)))
							{
								if (player.movementDirections.Count < 2)
								{
									_ = player.movementDirections.Count;
									if (moveUpHeld)
									{
										player.setMoving(1);
									}
									if (moveRightHeld)
									{
										player.setMoving(2);
									}
									if (moveDownHeld)
									{
										player.setMoving(4);
									}
									if (moveLeftHeld)
									{
										player.setMoving(8);
									}
								}
								if (moveUpReleased || (player.movementDirections.Contains(0) && !moveUpHeld))
								{
									player.setMoving(33);
									if (player.movementDirections.Count == 0)
									{
										player.setMoving(64);
									}
								}
								if (moveRightReleased || (player.movementDirections.Contains(1) && !moveRightHeld))
								{
									player.setMoving(34);
									if (player.movementDirections.Count == 0)
									{
										player.setMoving(64);
									}
								}
								if (moveDownReleased || (player.movementDirections.Contains(2) && !moveDownHeld))
								{
									player.setMoving(36);
									if (player.movementDirections.Count == 0)
									{
										player.setMoving(64);
									}
								}
								if (moveLeftReleased || (player.movementDirections.Contains(3) && !moveLeftHeld))
								{
									player.setMoving(40);
									if (player.movementDirections.Count == 0)
									{
										player.setMoving(64);
									}
								}
								if ((!moveUpHeld && !moveRightHeld && !moveDownHeld && !moveLeftHeld && !player.UsingTool) || activeClickableMenu != null)
								{
									player.Halt();
								}
							}
							else if (isQuestion)
							{
								if (moveUpPressed)
								{
									currentQuestionChoice = Math.Max(currentQuestionChoice - 1, 0);
									playSound("toolSwap");
								}
								else if (moveDownPressed)
								{
									currentQuestionChoice = Math.Min(currentQuestionChoice + 1, questionChoices.Count - 1);
									playSound("toolSwap");
								}
							}
							else if (numberOfSelectedItems != -1 && !dialogueTyping)
							{
								int val = 99;
								if (selectedItemsType.Equals("Animal Food"))
								{
									val = 999 - player.Feed;
								}
								else if (selectedItemsType.Equals("calicoJackBet"))
								{
									val = Math.Min(player.clubCoins, 999);
								}
								else if (selectedItemsType.Equals("flutePitch"))
								{
									val = 26;
								}
								else if (selectedItemsType.Equals("drumTone"))
								{
									val = 6;
								}
								else if (selectedItemsType.Equals("jukebox"))
								{
									val = player.songsHeard.Count - 1;
								}
								else if (selectedItemsType.Equals("Fuel"))
								{
									val = 100 - ((Lantern)player.getToolFromName("Lantern")).fuelLeft;
								}
								if (moveRightPressed)
								{
									numberOfSelectedItems = Math.Min(numberOfSelectedItems + 1, val);
									playItemNumberSelectSound();
								}
								else if (moveLeftPressed)
								{
									numberOfSelectedItems = Math.Max(numberOfSelectedItems - 1, 0);
									playItemNumberSelectSound();
								}
								else if (moveUpPressed)
								{
									numberOfSelectedItems = Math.Min(numberOfSelectedItems + 10, val);
									playItemNumberSelectSound();
								}
								else if (moveDownPressed)
								{
									numberOfSelectedItems = Math.Max(numberOfSelectedItems - 10, 0);
									playItemNumberSelectSound();
								}
							}
							if (moveUpHeld && !player.CanMove)
							{
								upPolling += time.ElapsedGameTime.Milliseconds;
							}
							else if (moveDownHeld && !player.CanMove)
							{
								downPolling += time.ElapsedGameTime.Milliseconds;
							}
							else if (moveRightHeld && !player.CanMove)
							{
								rightPolling += time.ElapsedGameTime.Milliseconds;
							}
							else if (moveLeftHeld && !player.CanMove)
							{
								leftPolling += time.ElapsedGameTime.Milliseconds;
							}
							else if (moveUpReleased)
							{
								upPolling = 0f;
							}
							else if (moveDownReleased)
							{
								downPolling = 0f;
							}
							else if (moveRightReleased)
							{
								rightPolling = 0f;
							}
							else if (moveLeftReleased)
							{
								leftPolling = 0f;
							}
							if (debugMode)
							{
								if (currentKBState.IsKeyDown(Keys.Q))
								{
									oldKBState.IsKeyDown(Keys.Q);
								}
								if (currentKBState.IsKeyDown(Keys.P) && !oldKBState.IsKeyDown(Keys.P))
								{
									NewDay(0f);
								}
								if (currentKBState.IsKeyDown(Keys.M) && !oldKBState.IsKeyDown(Keys.M))
								{
									dayOfMonth = 28;
									NewDay(0f);
								}
								if (currentKBState.IsKeyDown(Keys.T) && !oldKBState.IsKeyDown(Keys.T))
								{
									addHour();
								}
								if (currentKBState.IsKeyDown(Keys.Y) && !oldKBState.IsKeyDown(Keys.Y))
								{
									addMinute();
								}
								if (currentKBState.IsKeyDown(Keys.D1) && !oldKBState.IsKeyDown(Keys.D1))
								{
									warpFarmer("Mountain", 15, 35, flip: false);
								}
								if (currentKBState.IsKeyDown(Keys.D2) && !oldKBState.IsKeyDown(Keys.D2))
								{
									warpFarmer("Town", 35, 35, flip: false);
								}
								if (currentKBState.IsKeyDown(Keys.D3) && !oldKBState.IsKeyDown(Keys.D3))
								{
									warpFarmer("Farm", 64, 15, flip: false);
								}
								if (currentKBState.IsKeyDown(Keys.D4) && !oldKBState.IsKeyDown(Keys.D4))
								{
									warpFarmer("Forest", 34, 13, flip: false);
								}
								if (currentKBState.IsKeyDown(Keys.D5) && !oldKBState.IsKeyDown(Keys.D4))
								{
									warpFarmer("Beach", 34, 10, flip: false);
								}
								if (currentKBState.IsKeyDown(Keys.D6) && !oldKBState.IsKeyDown(Keys.D6))
								{
									warpFarmer("Mine", 18, 12, flip: false);
								}
								if (currentKBState.IsKeyDown(Keys.D7) && !oldKBState.IsKeyDown(Keys.D7))
								{
									warpFarmer("SandyHouse", 16, 3, flip: false);
								}
								if (currentKBState.IsKeyDown(Keys.K) && !oldKBState.IsKeyDown(Keys.K))
								{
									enterMine(mine.mineLevel + 1);
								}
								if (currentKBState.IsKeyDown(Keys.H) && !oldKBState.IsKeyDown(Keys.H))
								{
									player.changeHat(random.Next(FarmerRenderer.hatsTexture.Height / 80 * 12));
								}
								if (currentKBState.IsKeyDown(Keys.I) && !oldKBState.IsKeyDown(Keys.I))
								{
									player.changeHairStyle(random.Next(FarmerRenderer.hairStylesTexture.Height / 96 * 8));
								}
								if (currentKBState.IsKeyDown(Keys.J) && !oldKBState.IsKeyDown(Keys.J))
								{
									player.changeShirt(random.Next(40));
									player.changePants(new Color(random.Next(255), random.Next(255), random.Next(255)));
								}
								if (currentKBState.IsKeyDown(Keys.L) && !oldKBState.IsKeyDown(Keys.L))
								{
									player.changeShirt(random.Next(40));
									player.changePants(new Color(random.Next(255), random.Next(255), random.Next(255)));
									player.changeHairStyle(random.Next(FarmerRenderer.hairStylesTexture.Height / 96 * 8));
									if (random.NextDouble() < 0.5)
									{
										player.changeHat(random.Next(-1, FarmerRenderer.hatsTexture.Height / 80 * 12));
									}
									else
									{
										player.changeHat(-1);
									}
									player.changeHairColor(new Color(random.Next(255), random.Next(255), random.Next(255)));
									player.changeSkinColor(random.Next(16));
								}
								if (currentKBState.IsKeyDown(Keys.U) && !oldKBState.IsKeyDown(Keys.U))
								{
									(getLocationFromName("FarmHouse") as FarmHouse).setWallpaper(random.Next(112), -1, persist: true);
									(getLocationFromName("FarmHouse") as FarmHouse).setFloor(random.Next(40), -1, persist: true);
								}
								if (currentKBState.IsKeyDown(Keys.F2))
								{
									oldKBState.IsKeyDown(Keys.F2);
								}
								if (currentKBState.IsKeyDown(Keys.F5) && !oldKBState.IsKeyDown(Keys.F5))
								{
									displayFarmer = !displayFarmer;
								}
								if (currentKBState.IsKeyDown(Keys.F6))
								{
									oldKBState.IsKeyDown(Keys.F6);
								}
								if (currentKBState.IsKeyDown(Keys.F7) && !oldKBState.IsKeyDown(Keys.F7))
								{
									drawGrid = !drawGrid;
								}
								if (currentKBState.IsKeyDown(Keys.B) && !oldKBState.IsKeyDown(Keys.B))
								{
									player.shiftToolbar(right: false);
								}
								if (currentKBState.IsKeyDown(Keys.N) && !oldKBState.IsKeyDown(Keys.N))
								{
									player.shiftToolbar(right: true);
								}
								if (currentKBState.IsKeyDown(Keys.F10) && !oldKBState.IsKeyDown(Keys.F10) && server == null)
								{
									multiplayer.StartServer();
								}
							}
							else if (!player.UsingTool)
							{
								if (isOneOfTheseKeysDown(currentKBState, options.inventorySlot1) && areAllOfTheseKeysUp(oldKBState, options.inventorySlot1))
								{
									player.CurrentToolIndex = 0;
								}
								else if (isOneOfTheseKeysDown(currentKBState, options.inventorySlot2) && areAllOfTheseKeysUp(oldKBState, options.inventorySlot2))
								{
									player.CurrentToolIndex = 1;
								}
								else if (isOneOfTheseKeysDown(currentKBState, options.inventorySlot3) && areAllOfTheseKeysUp(oldKBState, options.inventorySlot3))
								{
									player.CurrentToolIndex = 2;
								}
								else if (isOneOfTheseKeysDown(currentKBState, options.inventorySlot4) && areAllOfTheseKeysUp(oldKBState, options.inventorySlot4))
								{
									player.CurrentToolIndex = 3;
								}
								else if (isOneOfTheseKeysDown(currentKBState, options.inventorySlot5) && areAllOfTheseKeysUp(oldKBState, options.inventorySlot5))
								{
									player.CurrentToolIndex = 4;
								}
								else if (isOneOfTheseKeysDown(currentKBState, options.inventorySlot6) && areAllOfTheseKeysUp(oldKBState, options.inventorySlot6))
								{
									player.CurrentToolIndex = 5;
								}
								else if (isOneOfTheseKeysDown(currentKBState, options.inventorySlot7) && areAllOfTheseKeysUp(oldKBState, options.inventorySlot7))
								{
									player.CurrentToolIndex = 6;
								}
								else if (isOneOfTheseKeysDown(currentKBState, options.inventorySlot8) && areAllOfTheseKeysUp(oldKBState, options.inventorySlot8))
								{
									player.CurrentToolIndex = 7;
								}
								else if (isOneOfTheseKeysDown(currentKBState, options.inventorySlot9) && areAllOfTheseKeysUp(oldKBState, options.inventorySlot9))
								{
									player.CurrentToolIndex = 8;
								}
								else if (isOneOfTheseKeysDown(currentKBState, options.inventorySlot10) && areAllOfTheseKeysUp(oldKBState, options.inventorySlot10))
								{
									player.CurrentToolIndex = 9;
								}
								else if (isOneOfTheseKeysDown(currentKBState, options.inventorySlot11) && areAllOfTheseKeysUp(oldKBState, options.inventorySlot11))
								{
									player.CurrentToolIndex = 10;
								}
								else if (isOneOfTheseKeysDown(currentKBState, options.inventorySlot12) && areAllOfTheseKeysUp(oldKBState, options.inventorySlot12))
								{
									player.CurrentToolIndex = 11;
								}
							}
							if (((options.gamepadControls && rightStickHoldTime >= emoteMenuShowTime && activeClickableMenu == null) || (isOneOfTheseKeysDown(input.GetKeyboardState(), options.emoteButton) && areAllOfTheseKeysUp(oldKBState, options.emoteButton))) && !debugMode && player.CanEmote())
							{
								if (player.CanMove)
								{
									player.Halt();
								}
								emoteMenu = new EmoteMenu();
								emoteMenu.gamepadMode = options.gamepadControls && rightStickHoldTime >= emoteMenuShowTime;
								timerUntilMouseFade = 0;
							}
							if (!Program.releaseBuild)
							{
								if (IsPressEvent(ref currentKBState, Keys.F3) || IsPressEvent(ref currentPadState, Buttons.LeftStick))
								{
									debugMode = !debugMode;
									if (gameMode == 11)
									{
										gameMode = 3;
									}
								}
								if (IsPressEvent(ref currentKBState, Keys.F8))
								{
									requestDebugInput();
								}
							}
							if (currentKBState.IsKeyDown(Keys.F4) && !oldKBState.IsKeyDown(Keys.F4))
							{
								displayHUD = !displayHUD;
								playSound("smallSelect");
								if (!displayHUD)
								{
									showGlobalMessage(content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.3666"));
								}
							}
							bool flag4 = isOneOfTheseKeysDown(currentKBState, options.menuButton) && areAllOfTheseKeysUp(oldKBState, options.menuButton);
							bool flag5 = isOneOfTheseKeysDown(currentKBState, options.journalButton) && areAllOfTheseKeysUp(oldKBState, options.journalButton);
							bool flag6 = isOneOfTheseKeysDown(currentKBState, options.mapButton) && areAllOfTheseKeysUp(oldKBState, options.mapButton);
							if (options.gamepadControls && !flag4)
							{
								flag4 = (currentPadState.IsButtonDown(Buttons.Start) && !oldPadState.IsButtonDown(Buttons.Start)) || (currentPadState.IsButtonDown(Buttons.B) && !oldPadState.IsButtonDown(Buttons.B));
							}
							if (options.gamepadControls && !flag5)
							{
								flag5 = currentPadState.IsButtonDown(Buttons.Back) && !oldPadState.IsButtonDown(Buttons.Back);
							}
							if (options.gamepadControls && !flag6)
							{
								flag6 = currentPadState.IsButtonDown(Buttons.Y) && !oldPadState.IsButtonDown(Buttons.Y);
							}
							if (flag4 && CanShowPauseMenu())
							{
								if (activeClickableMenu == null)
								{
									PushUIMode();
									activeClickableMenu = new GameMenu();
									PopUIMode();
								}
								else if (activeClickableMenu.readyToClose())
								{
									exitActiveMenu();
								}
							}
							if (dayOfMonth > 0 && player.CanMove && flag5 && !dialogueUp && !flag3)
							{
								if (activeClickableMenu == null)
								{
									activeClickableMenu = new QuestLog();
								}
							}
							else if (flag3 && CurrentEvent != null && flag5 && !CurrentEvent.skipped && CurrentEvent.skippable)
							{
								CurrentEvent.skipped = true;
								CurrentEvent.skipEvent();
								freezeControls = false;
							}
							if (options.gamepadControls && dayOfMonth > 0 && player.CanMove && isAnyGamePadButtonBeingPressed() && flag6 && !dialogueUp && !flag3)
							{
								if (activeClickableMenu == null)
								{
									PushUIMode();
									activeClickableMenu = new GameMenu(4);
									PopUIMode();
								}
							}
							else if (dayOfMonth > 0 && player.CanMove && flag6 && !dialogueUp && !flag3 && activeClickableMenu == null)
							{
								PushUIMode();
								activeClickableMenu = new GameMenu(3);
								PopUIMode();
							}
							checkForRunButton(currentKBState);
							oldKBState = currentKBState;
							oldMouseState = input.GetMouseState();
							oldPadState = currentPadState;
						}
					}
				}
			});
		}

		public static bool CanShowPauseMenu()
		{
			if (dayOfMonth > 0 && player.CanMove && !dialogueUp && (!eventUp || (isFestival() && CurrentEvent.festivalTimer <= 0)) && currentMinigame == null)
			{
				return farmEvent == null;
			}
			return false;
		}

		private static void addHour()
		{
			timeOfDay += 100;
			foreach (GameLocation g in locations)
			{
				for (int i = 0; i < g.getCharacters().Count; i++)
				{
					g.getCharacters()[i].checkSchedule(timeOfDay);
					g.getCharacters()[i].checkSchedule(timeOfDay - 50);
					g.getCharacters()[i].checkSchedule(timeOfDay - 60);
					g.getCharacters()[i].checkSchedule(timeOfDay - 70);
					g.getCharacters()[i].checkSchedule(timeOfDay - 80);
					g.getCharacters()[i].checkSchedule(timeOfDay - 90);
				}
			}
			switch (timeOfDay)
			{
			case 1900:
				globalOutdoorLighting = 0.5f;
				currentLocation.switchOutNightTiles();
				break;
			case 2000:
				globalOutdoorLighting = 0.7f;
				if (!IsRainingHere())
				{
					changeMusicTrack("none");
				}
				break;
			case 2100:
				globalOutdoorLighting = 0.9f;
				break;
			case 2200:
				globalOutdoorLighting = 1f;
				break;
			}
		}

		private static void addMinute()
		{
			if (GetKeyboardState().IsKeyDown(Keys.LeftShift))
			{
				timeOfDay -= 10;
			}
			else
			{
				timeOfDay += 10;
			}
			if (timeOfDay % 100 == 60)
			{
				timeOfDay += 40;
			}
			if (timeOfDay % 100 == 90)
			{
				timeOfDay -= 40;
			}
			currentLocation.performTenMinuteUpdate(timeOfDay);
			foreach (GameLocation g in locations)
			{
				for (int i = 0; i < g.getCharacters().Count; i++)
				{
					g.getCharacters()[i].checkSchedule(timeOfDay);
				}
			}
			if (isLightning && IsMasterGame)
			{
				Utility.performLightningUpdate(timeOfDay);
			}
			switch (timeOfDay)
			{
			case 1750:
				globalOutdoorLighting = 0f;
				outdoorLight = Color.White;
				break;
			case 1900:
				globalOutdoorLighting = 0.5f;
				currentLocation.switchOutNightTiles();
				break;
			case 2000:
				globalOutdoorLighting = 0.7f;
				if (!IsRainingHere())
				{
					changeMusicTrack("none");
				}
				break;
			case 2100:
				globalOutdoorLighting = 0.9f;
				break;
			case 2200:
				globalOutdoorLighting = 1f;
				break;
			}
		}

		public static void checkForRunButton(KeyboardState kbState, bool ignoreKeyPressQualifier = false)
		{
			bool wasRunning = player.running;
			bool runPressed = isOneOfTheseKeysDown(kbState, options.runButton) && (!isOneOfTheseKeysDown(oldKBState, options.runButton) || ignoreKeyPressQualifier);
			bool runReleased = !isOneOfTheseKeysDown(kbState, options.runButton) && (isOneOfTheseKeysDown(oldKBState, options.runButton) || ignoreKeyPressQualifier);
			if (options.gamepadControls)
			{
				if (!options.autoRun && Math.Abs(Vector2.Distance(input.GetGamePadState().ThumbSticks.Left, Vector2.Zero)) > 0.9f)
				{
					runPressed = true;
				}
				else if (Math.Abs(Vector2.Distance(oldPadState.ThumbSticks.Left, Vector2.Zero)) > 0.9f && Math.Abs(Vector2.Distance(input.GetGamePadState().ThumbSticks.Left, Vector2.Zero)) <= 0.9f)
				{
					runReleased = true;
				}
			}
			if (runPressed && !player.canOnlyWalk)
			{
				player.setRunning(!options.autoRun);
				player.setMoving((byte)(player.running ? 16u : 48u));
			}
			else if (runReleased && !player.canOnlyWalk)
			{
				player.setRunning(options.autoRun);
				player.setMoving((byte)(player.running ? 16u : 48u));
			}
			if (player.running != wasRunning && !player.UsingTool)
			{
				player.Halt();
			}
		}

		public static void drawTitleScreenBackground(GameTime gameTime, string dayNight, int weatherDebrisOffsetDay)
		{
		}

		public static Vector2 getMostRecentViewportMotion()
		{
			return new Vector2((float)viewport.X - previousViewportPosition.X, (float)viewport.Y - previousViewportPosition.Y);
		}

		protected virtual void drawOverlays(SpriteBatch spriteBatch)
		{
			if (!takingMapScreenshot)
			{
				spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);
				if (overlayMenu != null)
				{
					overlayMenu.draw(spriteBatch);
				}
				if (chatBox != null)
				{
					chatBox.update(currentGameTime);
					chatBox.draw(spriteBatch);
				}
				if (textEntry != null)
				{
					textEntry.draw(spriteBatch);
				}
				if ((displayHUD || eventUp || currentLocation is Summit) && currentBillboard == 0 && gameMode == 3 && !freezeControls && !panMode)
				{
					drawMouseCursor();
				}
				spriteBatch.End();
			}
		}

		public static void setBGColor(byte r, byte g, byte b)
		{
			bgColor.R = r;
			bgColor.G = g;
			bgColor.B = b;
		}

		public void Instance_Draw(GameTime gameTime)
		{
			Draw(gameTime);
		}

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw(GameTime gameTime)
		{
			isDrawing = true;
			RenderTarget2D target_screen = null;
			if (ShouldDrawOnBuffer())
			{
				target_screen = screen;
			}
			if (uiScreen != null)
			{
				SetRenderTarget(uiScreen);
				base.GraphicsDevice.Clear(Color.Transparent);
				SetRenderTarget(target_screen);
			}
			GameTime time = gameTime;
			DebugTools.BeforeGameDraw(this, ref time);
			_draw(time, target_screen);
			isRenderingScreenBuffer = true;
			renderScreenBuffer(target_screen);
			isRenderingScreenBuffer = false;
			if (uiModeCount != 0)
			{
				Console.WriteLine("WARNING: Mismatched UI Mode Push/Pop counts. Correcting.");
				while (uiModeCount < 0)
				{
					PushUIMode();
				}
				while (uiModeCount > 0)
				{
					PopUIMode();
				}
			}
			base.Draw(gameTime);
			isDrawing = false;
		}

		public virtual bool ShouldDrawOnBuffer()
		{
			if (LocalMultiplayer.IsLocalMultiplayer())
			{
				return true;
			}
			if (options.zoomLevel != 1f)
			{
				return true;
			}
			return false;
		}

		public static bool ShouldShowOnscreenUsernames()
		{
			return false;
		}

		public virtual bool checkCharacterTilesForShadowDrawFlag(Character character)
		{
			if (character is Farmer && (character as Farmer).onBridge.Value)
			{
				return true;
			}
			Microsoft.Xna.Framework.Rectangle bounding_box = character.GetBoundingBox();
			bounding_box.Height += 8;
			int right = bounding_box.Right / 64;
			int bottom = bounding_box.Bottom / 64;
			int num = bounding_box.Left / 64;
			int top = bounding_box.Top / 64;
			for (int x = num; x <= right; x++)
			{
				for (int y = top; y <= bottom; y++)
				{
					if (currentLocation.shouldShadowBeDrawnAboveBuildingsLayer(new Vector2(x, y)))
					{
						return true;
					}
				}
			}
			return false;
		}

		protected virtual void _draw(GameTime gameTime, RenderTarget2D target_screen)
		{
			showingHealthBar = false;
			if (_newDayTask != null || isLocalMultiplayerNewDayActive)
			{
				base.GraphicsDevice.Clear(bgColor);
				return;
			}
			if (target_screen != null)
			{
				SetRenderTarget(target_screen);
			}
			if (IsSaving)
			{
				base.GraphicsDevice.Clear(bgColor);
				PushUIMode();
				IClickableMenu menu = activeClickableMenu;
				if (menu != null)
				{
					spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);
					menu.draw(spriteBatch);
					spriteBatch.End();
				}
				if (overlayMenu != null)
				{
					spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);
					overlayMenu.draw(spriteBatch);
					spriteBatch.End();
				}
				PopUIMode();
				return;
			}
			base.GraphicsDevice.Clear(bgColor);
			if (activeClickableMenu != null && options.showMenuBackground && activeClickableMenu.showWithoutTransparencyIfOptionIsSet() && !takingMapScreenshot)
			{
				PushUIMode();
				spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);
				activeClickableMenu.drawBackground(spriteBatch);
				for (IClickableMenu menu2 = activeClickableMenu; menu2 != null; menu2 = menu2.GetChildMenu())
				{
					menu2.draw(spriteBatch);
				}
				if (specialCurrencyDisplay != null)
				{
					specialCurrencyDisplay.Draw(spriteBatch);
				}
				spriteBatch.End();
				drawOverlays(spriteBatch);
				PopUIMode();
				return;
			}
			if (gameMode == 11)
			{
				spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);
				spriteBatch.DrawString(dialogueFont, content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.3685"), new Vector2(16f, 16f), Color.HotPink);
				spriteBatch.DrawString(dialogueFont, content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.3686"), new Vector2(16f, 32f), new Color(0, 255, 0));
				spriteBatch.DrawString(dialogueFont, parseText(errorMessage, dialogueFont, graphics.GraphicsDevice.Viewport.Width), new Vector2(16f, 48f), Color.White);
				spriteBatch.End();
				return;
			}
			if (currentMinigame != null)
			{
				currentMinigame.draw(spriteBatch);
				if (globalFade && !menuUp && (!nameSelectUp || messagePause))
				{
					PushUIMode();
					spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);
					spriteBatch.Draw(fadeToBlackRect, graphics.GraphicsDevice.Viewport.Bounds, Color.Black * ((gameMode == 0) ? (1f - fadeToBlackAlpha) : fadeToBlackAlpha));
					spriteBatch.End();
					PopUIMode();
				}
				PushUIMode();
				drawOverlays(spriteBatch);
				PopUIMode();
				SetRenderTarget(target_screen);
				return;
			}
			if (showingEndOfNightStuff)
			{
				PushUIMode();
				spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);
				if (activeClickableMenu != null)
				{
					for (IClickableMenu menu3 = activeClickableMenu; menu3 != null; menu3 = menu3.GetChildMenu())
					{
						menu3.draw(spriteBatch);
					}
				}
				spriteBatch.End();
				drawOverlays(spriteBatch);
				PopUIMode();
				return;
			}
			if (gameMode == 6 || (gameMode == 3 && currentLocation == null))
			{
				PushUIMode();
				base.GraphicsDevice.Clear(bgColor);
				spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);
				string addOn = "";
				for (int i = 0; (double)i < gameTime.TotalGameTime.TotalMilliseconds % 999.0 / 333.0; i++)
				{
					addOn += ".";
				}
				string text = content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.3688");
				string msg = text + addOn;
				string largestMessage = text + "... ";
				int msgw = SpriteText.getWidthOfString(largestMessage);
				int msgh = 64;
				int msgx = 64;
				int msgy = graphics.GraphicsDevice.Viewport.GetTitleSafeArea().Bottom - msgh;
				SpriteText.drawString(spriteBatch, msg, msgx, msgy, 999999, msgw, msgh, 1f, 0.88f, junimoText: false, 0, largestMessage);
				spriteBatch.End();
				drawOverlays(spriteBatch);
				PopUIMode();
				return;
			}
			if (gameMode == 0)
			{
				spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);
			}
			else
			{
				if (gameMode == 3 && dayOfMonth == 0 && newDay)
				{
					base.Draw(gameTime);
					return;
				}
				if (drawLighting)
				{
					SetRenderTarget(lightmap);
					base.GraphicsDevice.Clear(Color.White * 0f);
					Matrix lighting_matrix = Matrix.Identity;
					if (useUnscaledLighting)
					{
						lighting_matrix = Matrix.CreateScale(options.zoomLevel);
					}
					spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, null, null, null, lighting_matrix);
					Color lighting = ((currentLocation.Name.StartsWith("UndergroundMine") && currentLocation is MineShaft) ? (currentLocation as MineShaft).getLightingColor(gameTime) : ((ambientLight.Equals(Color.White) || (IsRainingHere() && (bool)currentLocation.isOutdoors)) ? outdoorLight : ambientLight));
					float light_multiplier = 1f;
					if (player.hasBuff(26))
					{
						if (lighting == Color.White)
						{
							lighting = new Color(0.75f, 0.75f, 0.75f);
						}
						else
						{
							lighting.R = (byte)Utility.Lerp((int)lighting.R, 255f, 0.5f);
							lighting.G = (byte)Utility.Lerp((int)lighting.G, 255f, 0.5f);
							lighting.B = (byte)Utility.Lerp((int)lighting.B, 255f, 0.5f);
						}
						light_multiplier = 0.33f;
					}
					spriteBatch.Draw(staminaRect, lightmap.Bounds, lighting);
					foreach (LightSource lightSource in currentLightSources)
					{
						if ((IsRainingHere() || isDarkOut()) && lightSource.lightContext.Value == LightSource.LightContext.WindowLight)
						{
							continue;
						}
						if (lightSource.PlayerID != 0L && lightSource.PlayerID != player.UniqueMultiplayerID)
						{
							Farmer farmer = getFarmerMaybeOffline(lightSource.PlayerID);
							if (farmer == null || (farmer.currentLocation != null && farmer.currentLocation.Name != currentLocation.Name) || (bool)farmer.hidden)
							{
								continue;
							}
						}
						if (Utility.isOnScreen(lightSource.position, (int)((float)lightSource.radius * 64f * 4f)))
						{
							spriteBatch.Draw(lightSource.lightTexture, GlobalToLocal(viewport, lightSource.position) / (options.lightingQuality / 2), lightSource.lightTexture.Bounds, lightSource.color.Value * light_multiplier, 0f, new Vector2(lightSource.lightTexture.Bounds.Width / 2, lightSource.lightTexture.Bounds.Height / 2), (float)lightSource.radius / (float)(options.lightingQuality / 2), SpriteEffects.None, 0.9f);
						}
					}
					spriteBatch.End();
					SetRenderTarget(target_screen);
				}
				base.GraphicsDevice.Clear(bgColor);
				spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);
				if (background != null)
				{
					background.draw(spriteBatch);
				}
				currentLocation.drawBackground(spriteBatch);
				mapDisplayDevice.BeginScene(spriteBatch);
				currentLocation.Map.GetLayer("Back").Draw(mapDisplayDevice, viewport, Location.Origin, wrapAround: false, 4);
				currentLocation.drawWater(spriteBatch);
				spriteBatch.End();
				spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.PointClamp);
				currentLocation.drawFloorDecorations(spriteBatch);
				spriteBatch.End();
				spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);
				_farmerShadows.Clear();
				if (currentLocation.currentEvent != null && !currentLocation.currentEvent.isFestival && currentLocation.currentEvent.farmerActors.Count > 0)
				{
					foreach (Farmer f in currentLocation.currentEvent.farmerActors)
					{
						if ((f.IsLocalPlayer && displayFarmer) || !f.hidden)
						{
							_farmerShadows.Add(f);
						}
					}
				}
				else
				{
					foreach (Farmer f2 in currentLocation.farmers)
					{
						if ((f2.IsLocalPlayer && displayFarmer) || !f2.hidden)
						{
							_farmerShadows.Add(f2);
						}
					}
				}
				if (!currentLocation.shouldHideCharacters())
				{
					if (CurrentEvent == null)
					{
						foreach (NPC k in currentLocation.characters)
						{
							if (!k.swimming && !k.HideShadow && !k.IsInvisible && !checkCharacterTilesForShadowDrawFlag(k))
							{
								spriteBatch.Draw(shadowTexture, GlobalToLocal(viewport, k.GetShadowOffset() + k.Position + new Vector2((float)(k.GetSpriteWidthForPositioning() * 4) / 2f, k.GetBoundingBox().Height + ((!k.IsMonster) ? 12 : 0))), shadowTexture.Bounds, Color.White, 0f, new Vector2(shadowTexture.Bounds.Center.X, shadowTexture.Bounds.Center.Y), Math.Max(0f, (4f + (float)k.yJumpOffset / 40f) * (float)k.scale), SpriteEffects.None, Math.Max(0f, (float)k.getStandingY() / 10000f) - 1E-06f);
							}
						}
					}
					else
					{
						foreach (NPC l in CurrentEvent.actors)
						{
							if ((CurrentEvent == null || !CurrentEvent.ShouldHideCharacter(l)) && !l.swimming && !l.HideShadow && !checkCharacterTilesForShadowDrawFlag(l))
							{
								spriteBatch.Draw(shadowTexture, GlobalToLocal(viewport, l.GetShadowOffset() + l.Position + new Vector2((float)(l.GetSpriteWidthForPositioning() * 4) / 2f, l.GetBoundingBox().Height + ((!l.IsMonster) ? ((l.Sprite.SpriteHeight <= 16) ? (-4) : 12) : 0))), shadowTexture.Bounds, Color.White, 0f, new Vector2(shadowTexture.Bounds.Center.X, shadowTexture.Bounds.Center.Y), Math.Max(0f, 4f + (float)l.yJumpOffset / 40f) * (float)l.scale, SpriteEffects.None, Math.Max(0f, (float)l.getStandingY() / 10000f) - 1E-06f);
							}
						}
					}
					foreach (Farmer f3 in _farmerShadows)
					{
						if (!multiplayer.isDisconnecting(f3.UniqueMultiplayerID) && !f3.swimming && !f3.isRidingHorse() && !f3.IsSitting() && (currentLocation == null || !checkCharacterTilesForShadowDrawFlag(f3)))
						{
							spriteBatch.Draw(shadowTexture, GlobalToLocal(f3.GetShadowOffset() + f3.Position + new Vector2(32f, 24f)), shadowTexture.Bounds, Color.White, 0f, new Vector2(shadowTexture.Bounds.Center.X, shadowTexture.Bounds.Center.Y), 4f - (((f3.running || f3.UsingTool) && f3.FarmerSprite.currentAnimationIndex > 1) ? ((float)Math.Abs(FarmerRenderer.featureYOffsetPerFrame[f3.FarmerSprite.CurrentFrame]) * 0.5f) : 0f), SpriteEffects.None, 0f);
						}
					}
				}
				Layer building_layer = currentLocation.Map.GetLayer("Buildings");
				building_layer.Draw(mapDisplayDevice, viewport, Location.Origin, wrapAround: false, 4);
				mapDisplayDevice.EndScene();
				spriteBatch.End();
				spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.PointClamp);
				if (!currentLocation.shouldHideCharacters())
				{
					if (CurrentEvent == null)
					{
						foreach (NPC m in currentLocation.characters)
						{
							if (!m.swimming && !m.HideShadow && !m.isInvisible && checkCharacterTilesForShadowDrawFlag(m))
							{
								spriteBatch.Draw(shadowTexture, GlobalToLocal(viewport, m.GetShadowOffset() + m.Position + new Vector2((float)(m.GetSpriteWidthForPositioning() * 4) / 2f, m.GetBoundingBox().Height + ((!m.IsMonster) ? 12 : 0))), shadowTexture.Bounds, Color.White, 0f, new Vector2(shadowTexture.Bounds.Center.X, shadowTexture.Bounds.Center.Y), Math.Max(0f, (4f + (float)m.yJumpOffset / 40f) * (float)m.scale), SpriteEffects.None, Math.Max(0f, (float)m.getStandingY() / 10000f) - 1E-06f);
							}
						}
					}
					else
					{
						foreach (NPC n in CurrentEvent.actors)
						{
							if ((CurrentEvent == null || !CurrentEvent.ShouldHideCharacter(n)) && !n.swimming && !n.HideShadow && checkCharacterTilesForShadowDrawFlag(n))
							{
								spriteBatch.Draw(shadowTexture, GlobalToLocal(viewport, n.GetShadowOffset() + n.Position + new Vector2((float)(n.GetSpriteWidthForPositioning() * 4) / 2f, n.GetBoundingBox().Height + ((!n.IsMonster) ? 12 : 0))), shadowTexture.Bounds, Color.White, 0f, new Vector2(shadowTexture.Bounds.Center.X, shadowTexture.Bounds.Center.Y), Math.Max(0f, (4f + (float)n.yJumpOffset / 40f) * (float)n.scale), SpriteEffects.None, Math.Max(0f, (float)n.getStandingY() / 10000f) - 1E-06f);
							}
						}
					}
					foreach (Farmer f4 in _farmerShadows)
					{
						float draw_layer = Math.Max(0.0001f, f4.getDrawLayer() + 0.00011f) - 0.0001f;
						if (!f4.swimming && !f4.isRidingHorse() && !f4.IsSitting() && currentLocation != null && checkCharacterTilesForShadowDrawFlag(f4))
						{
							spriteBatch.Draw(shadowTexture, GlobalToLocal(f4.GetShadowOffset() + f4.Position + new Vector2(32f, 24f)), shadowTexture.Bounds, Color.White, 0f, new Vector2(shadowTexture.Bounds.Center.X, shadowTexture.Bounds.Center.Y), 4f - (((f4.running || f4.UsingTool) && f4.FarmerSprite.currentAnimationIndex > 1) ? ((float)Math.Abs(FarmerRenderer.featureYOffsetPerFrame[f4.FarmerSprite.CurrentFrame]) * 0.5f) : 0f), SpriteEffects.None, draw_layer);
						}
					}
				}
				if ((eventUp || killScreen) && !killScreen && currentLocation.currentEvent != null)
				{
					currentLocation.currentEvent.draw(spriteBatch);
				}
				if (player.currentUpgrade != null && player.currentUpgrade.daysLeftTillUpgradeDone <= 3 && currentLocation.Name.Equals("Farm"))
				{
					spriteBatch.Draw(player.currentUpgrade.workerTexture, GlobalToLocal(viewport, player.currentUpgrade.positionOfCarpenter), player.currentUpgrade.getSourceRectangle(), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, (player.currentUpgrade.positionOfCarpenter.Y + 48f) / 10000f);
				}
				currentLocation.draw(spriteBatch);
				foreach (Vector2 tile_position in crabPotOverlayTiles.Keys)
				{
					Tile tile = building_layer.Tiles[(int)tile_position.X, (int)tile_position.Y];
					if (tile != null)
					{
						Vector2 vector_draw_position = GlobalToLocal(viewport, tile_position * 64f);
						Location draw_location = new Location((int)vector_draw_position.X, (int)vector_draw_position.Y);
						mapDisplayDevice.DrawTile(tile, draw_location, (tile_position.Y * 64f - 1f) / 10000f);
					}
				}
				if (eventUp && currentLocation.currentEvent != null)
				{
					_ = currentLocation.currentEvent.messageToScreen;
				}
				if (player.ActiveObject == null && (player.UsingTool || pickingTool) && player.CurrentTool != null && (!player.CurrentTool.Name.Equals("Seeds") || pickingTool))
				{
					drawTool(player);
				}
				if (currentLocation.Name.Equals("Farm"))
				{
					drawFarmBuildings();
				}
				if (tvStation >= 0)
				{
					spriteBatch.Draw(tvStationTexture, GlobalToLocal(viewport, new Vector2(400f, 160f)), new Microsoft.Xna.Framework.Rectangle(tvStation * 24, 0, 24, 15), Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, 1E-08f);
				}
				if (panMode)
				{
					spriteBatch.Draw(fadeToBlackRect, new Microsoft.Xna.Framework.Rectangle((int)Math.Floor((double)(getOldMouseX() + viewport.X) / 64.0) * 64 - viewport.X, (int)Math.Floor((double)(getOldMouseY() + viewport.Y) / 64.0) * 64 - viewport.Y, 64, 64), Color.Lime * 0.75f);
					foreach (Warp w in currentLocation.warps)
					{
						spriteBatch.Draw(fadeToBlackRect, new Microsoft.Xna.Framework.Rectangle(w.X * 64 - viewport.X, w.Y * 64 - viewport.Y, 64, 64), Color.Red * 0.75f);
					}
				}
				mapDisplayDevice.BeginScene(spriteBatch);
				currentLocation.Map.GetLayer("Front").Draw(mapDisplayDevice, viewport, Location.Origin, wrapAround: false, 4);
				mapDisplayDevice.EndScene();
				currentLocation.drawAboveFrontLayer(spriteBatch);
				spriteBatch.End();
				spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);
				if (currentLocation.Map.GetLayer("AlwaysFront") != null)
				{
					mapDisplayDevice.BeginScene(spriteBatch);
					currentLocation.Map.GetLayer("AlwaysFront").Draw(mapDisplayDevice, viewport, Location.Origin, wrapAround: false, 4);
					mapDisplayDevice.EndScene();
				}
				if (toolHold > 400f && player.CurrentTool.UpgradeLevel >= 1 && player.canReleaseTool)
				{
					Color barColor = Color.White;
					switch ((int)(toolHold / 600f))
					{
					case -1:
						barColor = Tool.copperColor;
						break;
					case 0:
						barColor = Tool.steelColor;
						break;
					case 1:
						barColor = Tool.goldColor;
						break;
					case 2:
						barColor = Tool.iridiumColor;
						break;
					}
					spriteBatch.Draw(littleEffect, new Microsoft.Xna.Framework.Rectangle((int)player.getLocalPosition(viewport).X - 2, (int)player.getLocalPosition(viewport).Y - ((!player.CurrentTool.Name.Equals("Watering Can")) ? 64 : 0) - 2, (int)(toolHold % 600f * 0.08f) + 4, 12), Color.Black);
					spriteBatch.Draw(littleEffect, new Microsoft.Xna.Framework.Rectangle((int)player.getLocalPosition(viewport).X, (int)player.getLocalPosition(viewport).Y - ((!player.CurrentTool.Name.Equals("Watering Can")) ? 64 : 0), (int)(toolHold % 600f * 0.08f), 8), barColor);
				}
				if (!IsFakedBlackScreen())
				{
					drawWeather(gameTime, target_screen);
				}
				if (farmEvent != null)
				{
					farmEvent.draw(spriteBatch);
				}
				if (currentLocation.LightLevel > 0f && timeOfDay < 2000)
				{
					spriteBatch.Draw(fadeToBlackRect, graphics.GraphicsDevice.Viewport.Bounds, Color.Black * currentLocation.LightLevel);
				}
				if (screenGlow)
				{
					spriteBatch.Draw(fadeToBlackRect, graphics.GraphicsDevice.Viewport.Bounds, screenGlowColor * screenGlowAlpha);
				}
				currentLocation.drawAboveAlwaysFrontLayer(spriteBatch);
				if (player.CurrentTool != null && player.CurrentTool is FishingRod && ((player.CurrentTool as FishingRod).isTimingCast || (player.CurrentTool as FishingRod).castingChosenCountdown > 0f || (player.CurrentTool as FishingRod).fishCaught || (player.CurrentTool as FishingRod).showingTreasure))
				{
					player.CurrentTool.draw(spriteBatch);
				}
				spriteBatch.End();
				spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.PointClamp);
				if (eventUp && currentLocation.currentEvent != null)
				{
					foreach (NPC n2 in currentLocation.currentEvent.actors)
					{
						if (n2.isEmoting)
						{
							Vector2 emotePosition = n2.getLocalPosition(viewport);
							if (n2.NeedsBirdieEmoteHack())
							{
								emotePosition.X += 64f;
							}
							emotePosition.Y -= 140f;
							if (n2.Age == 2)
							{
								emotePosition.Y += 32f;
							}
							else if (n2.Gender == 1)
							{
								emotePosition.Y += 10f;
							}
							spriteBatch.Draw(emoteSpriteSheet, emotePosition, new Microsoft.Xna.Framework.Rectangle(n2.CurrentEmoteIndex * 16 % emoteSpriteSheet.Width, n2.CurrentEmoteIndex * 16 / emoteSpriteSheet.Width * 16, 16, 16), Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, (float)n2.getStandingY() / 10000f);
						}
					}
				}
				spriteBatch.End();
				if (drawLighting && !IsFakedBlackScreen())
				{
					spriteBatch.Begin(SpriteSortMode.Deferred, lightingBlend, SamplerState.LinearClamp);
					Viewport vp = base.GraphicsDevice.Viewport;
					vp.Bounds = target_screen?.Bounds ?? base.GraphicsDevice.PresentationParameters.Bounds;
					base.GraphicsDevice.Viewport = vp;
					float render_zoom = options.lightingQuality / 2;
					if (useUnscaledLighting)
					{
						render_zoom /= options.zoomLevel;
					}
					spriteBatch.Draw(lightmap, Vector2.Zero, lightmap.Bounds, Color.White, 0f, Vector2.Zero, render_zoom, SpriteEffects.None, 1f);
					if (IsRainingHere() && (bool)currentLocation.isOutdoors && !(currentLocation is Desert))
					{
						spriteBatch.Draw(staminaRect, vp.Bounds, Color.OrangeRed * 0.45f);
					}
					spriteBatch.End();
				}
				spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);
				if (drawGrid)
				{
					int startingX = -viewport.X % 64;
					float startingY = -viewport.Y % 64;
					for (int x = startingX; x < graphics.GraphicsDevice.Viewport.Width; x += 64)
					{
						spriteBatch.Draw(staminaRect, new Microsoft.Xna.Framework.Rectangle(x, (int)startingY, 1, graphics.GraphicsDevice.Viewport.Height), Color.Red * 0.5f);
					}
					for (float y = startingY; y < (float)graphics.GraphicsDevice.Viewport.Height; y += 64f)
					{
						spriteBatch.Draw(staminaRect, new Microsoft.Xna.Framework.Rectangle(startingX, (int)y, graphics.GraphicsDevice.Viewport.Width, 1), Color.Red * 0.5f);
					}
				}
				if (ShouldShowOnscreenUsernames() && currentLocation != null)
				{
					currentLocation.DrawFarmerUsernames(spriteBatch);
				}
				if (currentBillboard != 0 && !takingMapScreenshot)
				{
					drawBillboard();
				}
				if (!eventUp && farmEvent == null && currentBillboard == 0 && gameMode == 3 && !takingMapScreenshot && isOutdoorMapSmallerThanViewport())
				{
					spriteBatch.Draw(fadeToBlackRect, new Microsoft.Xna.Framework.Rectangle(0, 0, -viewport.X, graphics.GraphicsDevice.Viewport.Height), Color.Black);
					spriteBatch.Draw(fadeToBlackRect, new Microsoft.Xna.Framework.Rectangle(-viewport.X + currentLocation.map.Layers[0].LayerWidth * 64, 0, graphics.GraphicsDevice.Viewport.Width - (-viewport.X + currentLocation.map.Layers[0].LayerWidth * 64), graphics.GraphicsDevice.Viewport.Height), Color.Black);
					spriteBatch.Draw(fadeToBlackRect, new Microsoft.Xna.Framework.Rectangle(0, 0, graphics.GraphicsDevice.Viewport.Width, -viewport.Y), Color.Black);
					spriteBatch.Draw(fadeToBlackRect, new Microsoft.Xna.Framework.Rectangle(0, -viewport.Y + currentLocation.map.Layers[0].LayerHeight * 64, graphics.GraphicsDevice.Viewport.Width, graphics.GraphicsDevice.Viewport.Height - (-viewport.Y + currentLocation.map.Layers[0].LayerHeight * 64)), Color.Black);
				}
				spriteBatch.End();
				PushUIMode();
				spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);
				if ((displayHUD || eventUp) && currentBillboard == 0 && gameMode == 3 && !freezeControls && !panMode && !HostPaused && !takingMapScreenshot)
				{
					drawHUD();
				}
				else if (activeClickableMenu == null)
				{
					_ = farmEvent;
				}
				if (hudMessages.Count > 0 && !takingMapScreenshot)
				{
					for (int j = hudMessages.Count - 1; j >= 0; j--)
					{
						hudMessages[j].draw(spriteBatch, j);
					}
				}
				spriteBatch.End();
				PopUIMode();
				spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);
			}
			if (farmEvent != null)
			{
				farmEvent.draw(spriteBatch);
				spriteBatch.End();
				spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);
			}
			PushUIMode();
			if (dialogueUp && !nameSelectUp && !messagePause && (activeClickableMenu == null || !(activeClickableMenu is DialogueBox)) && !takingMapScreenshot)
			{
				drawDialogueBox();
			}
			if (progressBar && !takingMapScreenshot)
			{
				spriteBatch.Draw(fadeToBlackRect, new Microsoft.Xna.Framework.Rectangle((graphics.GraphicsDevice.Viewport.GetTitleSafeArea().Width - dialogueWidth) / 2, graphics.GraphicsDevice.Viewport.GetTitleSafeArea().Bottom - 128, dialogueWidth, 32), Color.LightGray);
				spriteBatch.Draw(staminaRect, new Microsoft.Xna.Framework.Rectangle((graphics.GraphicsDevice.Viewport.GetTitleSafeArea().Width - dialogueWidth) / 2, graphics.GraphicsDevice.Viewport.GetTitleSafeArea().Bottom - 128, (int)(pauseAccumulator / pauseTime * (float)dialogueWidth), 32), Color.DimGray);
			}
			spriteBatch.End();
			PopUIMode();
			spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);
			if (eventUp && currentLocation != null && currentLocation.currentEvent != null)
			{
				currentLocation.currentEvent.drawAfterMap(spriteBatch);
			}
			if (!IsFakedBlackScreen() && IsRainingHere() && currentLocation != null && (bool)currentLocation.isOutdoors && !(currentLocation is Desert))
			{
				spriteBatch.Draw(staminaRect, graphics.GraphicsDevice.Viewport.Bounds, Color.Blue * 0.2f);
			}
			if ((fadeToBlack || globalFade) && !menuUp && (!nameSelectUp || messagePause) && !takingMapScreenshot)
			{
				spriteBatch.End();
				PushUIMode();
				spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);
				spriteBatch.Draw(fadeToBlackRect, graphics.GraphicsDevice.Viewport.Bounds, Color.Black * ((gameMode == 0) ? (1f - fadeToBlackAlpha) : fadeToBlackAlpha));
				spriteBatch.End();
				PopUIMode();
				spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);
			}
			else if (flashAlpha > 0f && !takingMapScreenshot)
			{
				if (options.screenFlash)
				{
					spriteBatch.Draw(fadeToBlackRect, graphics.GraphicsDevice.Viewport.Bounds, Color.White * Math.Min(1f, flashAlpha));
				}
				flashAlpha -= 0.1f;
			}
			if ((messagePause || globalFade) && dialogueUp && !takingMapScreenshot)
			{
				drawDialogueBox();
			}
			if (!takingMapScreenshot)
			{
				foreach (TemporaryAnimatedSprite screenOverlayTempSprite in screenOverlayTempSprites)
				{
					screenOverlayTempSprite.draw(spriteBatch, localPosition: true);
				}
				spriteBatch.End();
				PushUIMode();
				spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);
				foreach (TemporaryAnimatedSprite uiOverlayTempSprite in uiOverlayTempSprites)
				{
					uiOverlayTempSprite.draw(spriteBatch, localPosition: true);
				}
				spriteBatch.End();
				PopUIMode();
				spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);
			}
			if (debugMode)
			{
				StringBuilder sb = _debugStringBuilder;
				sb.Clear();
				if (panMode)
				{
					sb.Append((getOldMouseX() + viewport.X) / 64);
					sb.Append(",");
					sb.Append((getOldMouseY() + viewport.Y) / 64);
				}
				else
				{
					sb.Append("player: ");
					sb.Append(player.getStandingX() / 64);
					sb.Append(", ");
					sb.Append(player.getStandingY() / 64);
				}
				sb.Append(" mouseTransparency: ");
				sb.Append(mouseCursorTransparency);
				sb.Append(" mousePosition: ");
				sb.Append(getMouseX());
				sb.Append(",");
				sb.Append(getMouseY());
				sb.Append(Environment.NewLine);
				sb.Append(" mouseWorldPosition: ");
				sb.Append(getMouseX() + viewport.X);
				sb.Append(",");
				sb.Append(getMouseY() + viewport.Y);
				sb.Append("  debugOutput: ");
				sb.Append(debugOutput);
				spriteBatch.DrawString(smallFont, sb, new Vector2(base.GraphicsDevice.Viewport.GetTitleSafeArea().X, base.GraphicsDevice.Viewport.GetTitleSafeArea().Y + smallFont.LineSpacing * 8), Color.Red, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.9999999f);
			}
			spriteBatch.End();
			PushUIMode();
			spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);
			if (showKeyHelp && !takingMapScreenshot)
			{
				spriteBatch.DrawString(smallFont, keyHelpString, new Vector2(64f, (float)(viewport.Height - 64 - (dialogueUp ? (192 + (isQuestion ? (questionChoices.Count * 64) : 0)) : 0)) - smallFont.MeasureString(keyHelpString).Y), Color.LightGray, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.9999999f);
			}
			if (activeClickableMenu != null && !takingMapScreenshot)
			{
				for (IClickableMenu menu4 = activeClickableMenu; menu4 != null; menu4 = menu4.GetChildMenu())
				{
					menu4.draw(spriteBatch);
				}
			}
			else if (farmEvent != null)
			{
				farmEvent.drawAboveEverything(spriteBatch);
			}
			if (specialCurrencyDisplay != null)
			{
				specialCurrencyDisplay.Draw(spriteBatch);
			}
			if (emoteMenu != null && !takingMapScreenshot)
			{
				emoteMenu.draw(spriteBatch);
			}
			if (HostPaused && !takingMapScreenshot)
			{
				string msg2 = content.LoadString("Strings\\StringsFromCSFiles:DayTimeMoneyBox.cs.10378");
				SpriteText.drawStringWithScrollBackground(spriteBatch, msg2, 96, 32);
			}
			spriteBatch.End();
			drawOverlays(spriteBatch);
			PopUIMode();
		}

		public virtual void drawWeather(GameTime time, RenderTarget2D target_screen)
		{
			if (IsSnowingHere() && (bool)currentLocation.isOutdoors && !(currentLocation is Desert))
			{
				snowPos.X %= 64f;
				Vector2 v = default(Vector2);
				for (float x = -64f + snowPos.X % 64f; x < (float)viewport.Width; x += 64f)
				{
					for (float y = -64f + snowPos.Y % 64f; y < (float)viewport.Height; y += 64f)
					{
						v.X = (int)x;
						v.Y = (int)y;
						spriteBatch.Draw(mouseCursors, v, new Microsoft.Xna.Framework.Rectangle(368 + (int)(currentGameTime.TotalGameTime.TotalMilliseconds % 1200.0) / 75 * 16, 192, 16, 16), Color.White * 0.8f * options.snowTransparency, 0f, Vector2.Zero, 4.001f, SpriteEffects.None, 1f);
					}
				}
			}
			if (currentLocation.IsOutdoors && !currentLocation.ignoreDebrisWeather && IsDebrisWeatherHere() && !currentLocation.Name.Equals("Desert"))
			{
				if (takingMapScreenshot)
				{
					if (debrisWeather != null)
					{
						foreach (WeatherDebris w in debrisWeather)
						{
							Vector2 position = w.position;
							w.position = new Vector2(random.Next(viewport.Width - w.sourceRect.Width * 3), random.Next(viewport.Height - w.sourceRect.Height * 3));
							w.draw(spriteBatch);
							w.position = position;
						}
					}
				}
				else if (viewport.X > -viewport.Width)
				{
					foreach (WeatherDebris item in debrisWeather)
					{
						item.draw(spriteBatch);
					}
				}
			}
			if (!IsRainingHere() || !currentLocation.IsOutdoors || currentLocation.Name.Equals("Desert") || currentLocation is Summit)
			{
				return;
			}
			if (takingMapScreenshot)
			{
				for (int j = 0; j < rainDrops.Length; j++)
				{
					Vector2 drop_position = new Vector2(random.Next(viewport.Width - 64), random.Next(viewport.Height - 64));
					spriteBatch.Draw(rainTexture, drop_position, getSourceRectForStandardTileSheet(rainTexture, rainDrops[j].frame), Color.White);
				}
			}
			else if (!eventUp || currentLocation.isTileOnMap(new Vector2(viewport.X / 64, viewport.Y / 64)))
			{
				for (int i = 0; i < rainDrops.Length; i++)
				{
					spriteBatch.Draw(rainTexture, rainDrops[i].position, getSourceRectForStandardTileSheet(rainTexture, rainDrops[i].frame), Color.White);
				}
			}
		}

		protected virtual void renderScreenBuffer(RenderTarget2D target_screen)
		{
			graphics.GraphicsDevice.SetRenderTarget(null);
			if (!takingMapScreenshot && !LocalMultiplayer.IsLocalMultiplayer() && (target_screen == null || !target_screen.IsContentLost))
			{
				if (ShouldDrawOnBuffer() && target_screen != null)
				{
					base.GraphicsDevice.Clear(bgColor);
					spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Opaque, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone);
					spriteBatch.Draw(target_screen, new Vector2(0f, 0f), target_screen.Bounds, Color.White, 0f, Vector2.Zero, options.zoomLevel, SpriteEffects.None, 1f);
					spriteBatch.End();
					spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone);
					spriteBatch.Draw(uiScreen, new Vector2(0f, 0f), uiScreen.Bounds, Color.White, 0f, Vector2.Zero, options.uiScale, SpriteEffects.None, 1f);
					spriteBatch.End();
				}
				else
				{
					spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone);
					spriteBatch.Draw(uiScreen, new Vector2(0f, 0f), uiScreen.Bounds, Color.White, 0f, Vector2.Zero, options.uiScale, SpriteEffects.None, 1f);
					spriteBatch.End();
				}
			}
		}

		public virtual void DrawSplitScreenWindow()
		{
			if (!LocalMultiplayer.IsLocalMultiplayer())
			{
				return;
			}
			graphics.GraphicsDevice.SetRenderTarget(null);
			if (screen == null || !screen.IsContentLost)
			{
				Viewport old_viewport = base.GraphicsDevice.Viewport;
				GraphicsDevice graphicsDevice = base.GraphicsDevice;
				Viewport viewport2 = (base.GraphicsDevice.Viewport = defaultDeviceViewport);
				graphicsDevice.Viewport = viewport2;
				spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone);
				spriteBatch.Draw(screen, new Vector2(localMultiplayerWindow.X, localMultiplayerWindow.Y), screen.Bounds, Color.White, 0f, Vector2.Zero, instanceOptions.zoomLevel, SpriteEffects.None, 1f);
				if (uiScreen != null)
				{
					spriteBatch.Draw(uiScreen, new Vector2(localMultiplayerWindow.X, localMultiplayerWindow.Y), uiScreen.Bounds, Color.White, 0f, Vector2.Zero, instanceOptions.uiScale, SpriteEffects.None, 1f);
				}
				spriteBatch.End();
				base.GraphicsDevice.Viewport = old_viewport;
			}
		}

		/// ###########################
		/// METHODS FOR DRAWING THINGS.
		/// ############################
		public static void drawWithBorder(string message, Color borderColor, Color insideColor, Vector2 position)
		{
			drawWithBorder(message, borderColor, insideColor, position, 0f, 1f, 1f, tiny: false);
		}

		public static void drawWithBorder(string message, Color borderColor, Color insideColor, Vector2 position, float rotate, float scale, float layerDepth)
		{
			drawWithBorder(message, borderColor, insideColor, position, rotate, scale, layerDepth, tiny: false);
		}

		public static void drawWithBorder(string message, Color borderColor, Color insideColor, Vector2 position, float rotate, float scale, float layerDepth, bool tiny)
		{
			string[] words = message.Split(Utility.CharSpace);
			int offset = 0;
			for (int i = 0; i < words.Length; i++)
			{
				if (words[i].Contains("="))
				{
					spriteBatch.DrawString(tiny ? tinyFont : dialogueFont, words[i], new Vector2(position.X + (float)offset, position.Y), Color.Purple, rotate, Vector2.Zero, scale, SpriteEffects.None, layerDepth);
					offset += (int)((tiny ? tinyFont : dialogueFont).MeasureString(words[i]).X + 8f);
				}
				else
				{
					spriteBatch.DrawString(tiny ? tinyFont : dialogueFont, words[i], new Vector2(position.X + (float)offset, position.Y), insideColor, rotate, Vector2.Zero, scale, SpriteEffects.None, layerDepth);
					offset += (int)((tiny ? tinyFont : dialogueFont).MeasureString(words[i]).X + 8f);
				}
			}
		}

		public static bool isOutdoorMapSmallerThanViewport()
		{
			if (uiMode)
			{
				return false;
			}
			if (currentLocation != null && currentLocation.IsOutdoors && !(currentLocation is Summit))
			{
				if (currentLocation.map.Layers[0].LayerWidth * 64 >= viewport.Width)
				{
					return currentLocation.map.Layers[0].LayerHeight * 64 < viewport.Height;
				}
				return true;
			}
			return false;
		}

		protected virtual void drawHUD()
		{
			if (eventUp || farmEvent != null)
			{
				return;
			}
			float modifier = 0.625f;
			Vector2 topOfBar = new Vector2(graphics.GraphicsDevice.Viewport.GetTitleSafeArea().Right - 48 - 8, graphics.GraphicsDevice.Viewport.GetTitleSafeArea().Bottom - 224 - 16 - (int)((float)(player.MaxStamina - 270) * modifier));
			if (isOutdoorMapSmallerThanViewport())
			{
				topOfBar.X = Math.Min(topOfBar.X, -viewport.X + currentLocation.map.Layers[0].LayerWidth * 64 - 48);
			}
			if (staminaShakeTimer > 0)
			{
				topOfBar.X += random.Next(-3, 4);
				topOfBar.Y += random.Next(-3, 4);
			}
			spriteBatch.Draw(mouseCursors, topOfBar, new Microsoft.Xna.Framework.Rectangle(256, 408, 12, 16), Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, 1f);
			spriteBatch.Draw(mouseCursors, new Microsoft.Xna.Framework.Rectangle((int)topOfBar.X, (int)(topOfBar.Y + 64f), 48, graphics.GraphicsDevice.Viewport.GetTitleSafeArea().Bottom - 64 - 16 - (int)(topOfBar.Y + 64f - 8f)), new Microsoft.Xna.Framework.Rectangle(256, 424, 12, 16), Color.White);
			spriteBatch.Draw(mouseCursors, new Vector2(topOfBar.X, topOfBar.Y + 224f + (float)(int)((float)(player.MaxStamina - 270) * modifier) - 64f), new Microsoft.Xna.Framework.Rectangle(256, 448, 12, 16), Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, 1f);
			Microsoft.Xna.Framework.Rectangle r = new Microsoft.Xna.Framework.Rectangle((int)topOfBar.X + 12, (int)topOfBar.Y + 16 + 32 + (int)((float)(player.MaxStamina - (int)Math.Max(0f, player.Stamina)) * modifier), 24, (int)(player.Stamina * modifier));
			if ((float)getOldMouseX() >= topOfBar.X && (float)getOldMouseY() >= topOfBar.Y)
			{
				drawWithBorder((int)Math.Max(0f, player.Stamina) + "/" + player.MaxStamina, Color.Black * 0f, Color.White, topOfBar + new Vector2(0f - dialogueFont.MeasureString("999/999").X - 16f - (float)(showingHealth ? 64 : 0), 64f));
			}
			Color c = Utility.getRedToGreenLerpColor(player.stamina / (float)(int)player.maxStamina);
			spriteBatch.Draw(staminaRect, r, c);
			r.Height = 4;
			c.R = (byte)Math.Max(0, c.R - 50);
			c.G = (byte)Math.Max(0, c.G - 50);
			spriteBatch.Draw(staminaRect, r, c);
			if ((bool)player.exhausted)
			{
				spriteBatch.Draw(mouseCursors, topOfBar - new Vector2(0f, 11f) * 4f, new Microsoft.Xna.Framework.Rectangle(191, 406, 12, 11), Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, 1f);
				if ((float)getOldMouseX() >= topOfBar.X && (float)getOldMouseY() >= topOfBar.Y - 44f)
				{
					drawWithBorder(content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.3747"), Color.Black * 0f, Color.White, topOfBar + new Vector2(0f - dialogueFont.MeasureString(content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.3747")).X - 16f - (float)(showingHealth ? 64 : 0), 96f));
				}
			}
			if (currentLocation is MineShaft || currentLocation is Woods || currentLocation is SlimeHutch || currentLocation is VolcanoDungeon || player.health < player.maxHealth)
			{
				showingHealthBar = true;
				showingHealth = true;
				int bar_full_height = 168 + (player.maxHealth - 100);
				int height = (int)((float)player.health / (float)player.maxHealth * (float)bar_full_height);
				topOfBar.X -= 56 + ((hitShakeTimer > 0) ? random.Next(-3, 4) : 0);
				topOfBar.Y = graphics.GraphicsDevice.Viewport.GetTitleSafeArea().Bottom - 224 - 16 - (player.maxHealth - 100);
				spriteBatch.Draw(mouseCursors, topOfBar, new Microsoft.Xna.Framework.Rectangle(268, 408, 12, 16), (player.health < 20) ? (Color.Pink * ((float)Math.Sin(currentGameTime.TotalGameTime.TotalMilliseconds / (double)((float)player.health * 50f)) / 4f + 0.9f)) : Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, 1f);
				spriteBatch.Draw(mouseCursors, new Microsoft.Xna.Framework.Rectangle((int)topOfBar.X, (int)(topOfBar.Y + 64f), 48, graphics.GraphicsDevice.Viewport.GetTitleSafeArea().Bottom - 64 - 16 - (int)(topOfBar.Y + 64f)), new Microsoft.Xna.Framework.Rectangle(268, 424, 12, 16), (player.health < 20) ? (Color.Pink * ((float)Math.Sin(currentGameTime.TotalGameTime.TotalMilliseconds / (double)((float)player.health * 50f)) / 4f + 0.9f)) : Color.White);
				spriteBatch.Draw(mouseCursors, new Vector2(topOfBar.X, topOfBar.Y + 224f + (float)(player.maxHealth - 100) - 64f), new Microsoft.Xna.Framework.Rectangle(268, 448, 12, 16), (player.health < 20) ? (Color.Pink * ((float)Math.Sin(currentGameTime.TotalGameTime.TotalMilliseconds / (double)((float)player.health * 50f)) / 4f + 0.9f)) : Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, 1f);
				Microsoft.Xna.Framework.Rectangle health_bar_rect = new Microsoft.Xna.Framework.Rectangle((int)topOfBar.X + 12, (int)topOfBar.Y + 16 + 32 + bar_full_height - height, 24, height);
				c = Utility.getRedToGreenLerpColor((float)player.health / (float)player.maxHealth);
				spriteBatch.Draw(staminaRect, health_bar_rect, staminaRect.Bounds, c, 0f, Vector2.Zero, SpriteEffects.None, 1f);
				c.R = (byte)Math.Max(0, c.R - 50);
				c.G = (byte)Math.Max(0, c.G - 50);
				if ((float)getOldMouseX() >= topOfBar.X && (float)getOldMouseY() >= topOfBar.Y && (float)getOldMouseX() < topOfBar.X + 32f)
				{
					drawWithBorder(Math.Max(0, player.health) + "/" + player.maxHealth, Color.Black * 0f, Color.Red, topOfBar + new Vector2(0f - dialogueFont.MeasureString("999/999").X - 32f, 64f));
				}
				health_bar_rect.Height = 4;
				spriteBatch.Draw(staminaRect, health_bar_rect, staminaRect.Bounds, c, 0f, Vector2.Zero, SpriteEffects.None, 1f);
			}
			else
			{
				showingHealth = false;
			}
			_ = player.ActiveObject;
			foreach (IClickableMenu menu in onScreenMenus)
			{
				if (menu != chatBox)
				{
					menu.update(currentGameTime);
					menu.draw(spriteBatch);
				}
			}
			if (!player.professions.Contains(17) || !currentLocation.IsOutdoors)
			{
				return;
			}
			foreach (KeyValuePair<Vector2, Object> v in currentLocation.objects.Pairs)
			{
				if (((bool)v.Value.isSpawnedObject || v.Value.ParentSheetIndex == 590) && !Utility.isOnScreen(v.Key * 64f + new Vector2(32f, 32f), 64))
				{
					Microsoft.Xna.Framework.Rectangle vpbounds = graphics.GraphicsDevice.Viewport.Bounds;
					Vector2 onScreenPosition2 = default(Vector2);
					float rotation2 = 0f;
					if (v.Key.X * 64f > (float)(viewport.MaxCorner.X - 64))
					{
						onScreenPosition2.X = vpbounds.Right - 8;
						rotation2 = (float)Math.PI / 2f;
					}
					else if (v.Key.X * 64f < (float)viewport.X)
					{
						onScreenPosition2.X = 8f;
						rotation2 = -(float)Math.PI / 2f;
					}
					else
					{
						onScreenPosition2.X = v.Key.X * 64f - (float)viewport.X;
					}
					if (v.Key.Y * 64f > (float)(viewport.MaxCorner.Y - 64))
					{
						onScreenPosition2.Y = vpbounds.Bottom - 8;
						rotation2 = (float)Math.PI;
					}
					else if (v.Key.Y * 64f < (float)viewport.Y)
					{
						onScreenPosition2.Y = 8f;
					}
					else
					{
						onScreenPosition2.Y = v.Key.Y * 64f - (float)viewport.Y;
					}
					if (onScreenPosition2.X == 8f && onScreenPosition2.Y == 8f)
					{
						rotation2 += (float)Math.PI / 4f;
					}
					if (onScreenPosition2.X == 8f && onScreenPosition2.Y == (float)(vpbounds.Bottom - 8))
					{
						rotation2 += (float)Math.PI / 4f;
					}
					if (onScreenPosition2.X == (float)(vpbounds.Right - 8) && onScreenPosition2.Y == 8f)
					{
						rotation2 -= (float)Math.PI / 4f;
					}
					if (onScreenPosition2.X == (float)(vpbounds.Right - 8) && onScreenPosition2.Y == (float)(vpbounds.Bottom - 8))
					{
						rotation2 -= (float)Math.PI / 4f;
					}
					Microsoft.Xna.Framework.Rectangle srcRect = new Microsoft.Xna.Framework.Rectangle(412, 495, 5, 4);
					float renderScale = 4f;
					Vector2 safePos = Utility.makeSafe(renderSize: new Vector2((float)srcRect.Width * renderScale, (float)srcRect.Height * renderScale), renderPos: onScreenPosition2);
					spriteBatch.Draw(mouseCursors, safePos, srcRect, Color.White, rotation2, new Vector2(2f, 2f), renderScale, SpriteEffects.None, 1f);
				}
			}
			if (!currentLocation.orePanPoint.Equals(Point.Zero) && !Utility.isOnScreen(Utility.PointToVector2(currentLocation.orePanPoint) * 64f + new Vector2(32f, 32f), 64))
			{
				Vector2 onScreenPosition = default(Vector2);
				float rotation = 0f;
				if (currentLocation.orePanPoint.X * 64 > viewport.MaxCorner.X - 64)
				{
					onScreenPosition.X = graphics.GraphicsDevice.Viewport.Bounds.Right - 8;
					rotation = (float)Math.PI / 2f;
				}
				else if (currentLocation.orePanPoint.X * 64 < viewport.X)
				{
					onScreenPosition.X = 8f;
					rotation = -(float)Math.PI / 2f;
				}
				else
				{
					onScreenPosition.X = currentLocation.orePanPoint.X * 64 - viewport.X;
				}
				if (currentLocation.orePanPoint.Y * 64 > viewport.MaxCorner.Y - 64)
				{
					onScreenPosition.Y = graphics.GraphicsDevice.Viewport.Bounds.Bottom - 8;
					rotation = (float)Math.PI;
				}
				else if (currentLocation.orePanPoint.Y * 64 < viewport.Y)
				{
					onScreenPosition.Y = 8f;
				}
				else
				{
					onScreenPosition.Y = currentLocation.orePanPoint.Y * 64 - viewport.Y;
				}
				if (onScreenPosition.X == 8f && onScreenPosition.Y == 8f)
				{
					rotation += (float)Math.PI / 4f;
				}
				if (onScreenPosition.X == 8f && onScreenPosition.Y == (float)(graphics.GraphicsDevice.Viewport.Bounds.Bottom - 8))
				{
					rotation += (float)Math.PI / 4f;
				}
				if (onScreenPosition.X == (float)(graphics.GraphicsDevice.Viewport.Bounds.Right - 8) && onScreenPosition.Y == 8f)
				{
					rotation -= (float)Math.PI / 4f;
				}
				if (onScreenPosition.X == (float)(graphics.GraphicsDevice.Viewport.Bounds.Right - 8) && onScreenPosition.Y == (float)(graphics.GraphicsDevice.Viewport.Bounds.Bottom - 8))
				{
					rotation -= (float)Math.PI / 4f;
				}
				spriteBatch.Draw(mouseCursors, onScreenPosition, new Microsoft.Xna.Framework.Rectangle(412, 495, 5, 4), Color.Cyan, rotation, new Vector2(2f, 2f), 4f, SpriteEffects.None, 1f);
			}
		}

		public static void InvalidateOldMouseMovement()
		{
			MouseState input = Game1.input.GetMouseState();
			oldMouseState = new MouseState(input.X, input.Y, oldMouseState.ScrollWheelValue, oldMouseState.LeftButton, oldMouseState.MiddleButton, oldMouseState.RightButton, oldMouseState.XButton1, oldMouseState.XButton2);
		}

		public static bool IsRenderingNonNativeUIScale()
		{
			return options.uiScale != options.zoomLevel;
		}

		public virtual void drawMouseCursor()
		{
			if (activeClickableMenu == null && timerUntilMouseFade > 0)
			{
				timerUntilMouseFade -= currentGameTime.ElapsedGameTime.Milliseconds;
				lastMousePositionBeforeFade = getMousePosition();
			}
			if (options.gamepadControls && timerUntilMouseFade <= 0 && activeClickableMenu == null && (emoteMenu == null || emoteMenu.gamepadMode))
			{
				mouseCursorTransparency = 0f;
			}
			if (activeClickableMenu == null && mouseCursor > -1 && currentLocation != null)
			{
				if (IsRenderingNonNativeUIScale())
				{
					spriteBatch.End();
					PopUIMode();
					if (ShouldDrawOnBuffer())
					{
						SetRenderTarget(screen);
					}
					else
					{
						SetRenderTarget(null);
					}
					spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);
				}
				if (!(mouseCursorTransparency > 0f) || !Utility.canGrabSomethingFromHere(getOldMouseX() + viewport.X, getOldMouseY() + viewport.Y, player) || mouseCursor == 3)
				{
					if (player.ActiveObject != null && mouseCursor != 3 && !eventUp && currentMinigame == null && !player.isRidingHorse() && player.CanMove && displayFarmer)
					{
						if (mouseCursorTransparency > 0f || options.showPlacementTileForGamepad)
						{
							player.ActiveObject.drawPlacementBounds(spriteBatch, currentLocation);
							if (mouseCursorTransparency > 0f)
							{
								spriteBatch.End();
								PushUIMode();
								spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);
								bool canPlace = Utility.playerCanPlaceItemHere(currentLocation, player.CurrentItem, getMouseX() + viewport.X, getMouseY() + viewport.Y, player) || (Utility.isThereAnObjectHereWhichAcceptsThisItem(currentLocation, player.CurrentItem, getMouseX() + viewport.X, getMouseY() + viewport.Y) && Utility.withinRadiusOfPlayer(getMouseX() + viewport.X, getMouseY() + viewport.Y, 1, player));
								player.CurrentItem.drawInMenu(spriteBatch, new Vector2(getMouseX() + 16, getMouseY() + 16), canPlace ? (dialogueButtonScale / 75f + 1f) : 1f, canPlace ? 1f : 0.5f, 0.999f);
								spriteBatch.End();
								PopUIMode();
								spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);
							}
						}
					}
					else if (mouseCursor == 0 && isActionAtCurrentCursorTile && currentMinigame == null)
					{
						mouseCursor = (isSpeechAtCurrentCursorTile ? 4 : (isInspectionAtCurrentCursorTile ? 5 : 2));
					}
					else if (mouseCursorTransparency > 0f)
					{
						NetLongDictionary<FarmAnimal, NetRef<FarmAnimal>> animals = null;
						if (currentLocation is Farm)
						{
							animals = (currentLocation as Farm).animals;
						}
						if (currentLocation is AnimalHouse)
						{
							animals = (currentLocation as AnimalHouse).animals;
						}
						if (animals != null)
						{
							Vector2 mousePos = new Vector2(getOldMouseX() + uiViewport.X, getOldMouseY() + uiViewport.Y);
							player.getGeneralDirectionTowards(mousePos);
							bool mouseWithinRadiusOfPlayer = Utility.withinRadiusOfPlayer((int)mousePos.X, (int)mousePos.Y, 1, player);
							foreach (KeyValuePair<long, FarmAnimal> kvp in animals.Pairs)
							{
								Microsoft.Xna.Framework.Rectangle animalBounds = kvp.Value.GetCursorPetBoundingBox();
								if (!kvp.Value.wasPet && animalBounds.Contains((int)mousePos.X, (int)mousePos.Y))
								{
									mouseCursor = 2;
									if (!mouseWithinRadiusOfPlayer)
									{
										mouseCursorTransparency = 0.5f;
									}
									break;
								}
							}
						}
					}
				}
				if (IsRenderingNonNativeUIScale())
				{
					spriteBatch.End();
					PushUIMode();
					SetRenderTarget(uiScreen);
					spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);
				}
				if (currentMinigame != null)
				{
					mouseCursor = 0;
				}
				if (!freezeControls && !options.hardwareCursor)
				{
					spriteBatch.Draw(mouseCursors, new Vector2(getMouseX(), getMouseY()), getSourceRectForStandardTileSheet(mouseCursors, mouseCursor, 16, 16), Color.White * mouseCursorTransparency, 0f, Vector2.Zero, 4f + dialogueButtonScale / 150f, SpriteEffects.None, 1f);
				}
				wasMouseVisibleThisFrame = mouseCursorTransparency > 0f;
				_lastDrewMouseCursor = wasMouseVisibleThisFrame;
			}
			mouseCursor = 0;
			if (!isActionAtCurrentCursorTile && activeClickableMenu == null)
			{
				mouseCursorTransparency = 1f;
			}
		}

		public static void panScreen(int x, int y)
		{
			int old_ui_mode_count = uiModeCount;
			while (uiModeCount > 0)
			{
				PopUIMode();
			}
			previousViewportPosition.X = viewport.Location.X;
			previousViewportPosition.Y = viewport.Location.Y;
			viewport.X += x;
			viewport.Y += y;
			clampViewportToGameMap();
			updateRaindropPosition();
			for (int i = 0; i < old_ui_mode_count; i++)
			{
				PushUIMode();
			}
		}

		public static void clampViewportToGameMap()
		{
			if (viewport.X < 0)
			{
				viewport.X = 0;
			}
			if (viewport.X > currentLocation.map.DisplayWidth - viewport.Width)
			{
				viewport.X = currentLocation.map.DisplayWidth - viewport.Width;
			}
			if (viewport.Y < 0)
			{
				viewport.Y = 0;
			}
			if (viewport.Y > currentLocation.map.DisplayHeight - viewport.Height)
			{
				viewport.Y = currentLocation.map.DisplayHeight - viewport.Height;
			}
		}

		public void drawBillboard()
		{
		}

		protected void drawDialogueBox()
		{
			int messageHeight = 320;
			if (currentSpeaker != null)
			{
				messageHeight = (int)dialogueFont.MeasureString(currentSpeaker.CurrentDialogue.Peek().getCurrentDialogue()).Y;
				messageHeight = Math.Max(messageHeight, 320);
				drawDialogueBox((base.GraphicsDevice.Viewport.GetTitleSafeArea().Width - Math.Min(1280, base.GraphicsDevice.Viewport.GetTitleSafeArea().Width - 128)) / 2, base.GraphicsDevice.Viewport.GetTitleSafeArea().Height - messageHeight, Math.Min(1280, base.GraphicsDevice.Viewport.GetTitleSafeArea().Width - 128), messageHeight, speaker: true, drawOnlyBox: false, null, objectDialoguePortraitPerson != null && currentSpeaker == null);
			}
			else
			{
				_ = currentObjectDialogue.Count;
				_ = 0;
			}
		}

		public static void drawDialogueBox(string message)
		{
			drawDialogueBox(viewport.Width / 2, viewport.Height / 2, speaker: false, drawOnlyBox: false, message);
		}

		public static void drawDialogueBox(int centerX, int centerY, bool speaker, bool drawOnlyBox, string message)
		{
			string text = null;
			if (speaker && currentSpeaker != null)
			{
				text = currentSpeaker.CurrentDialogue.Peek().getCurrentDialogue();
			}
			else if (message != null)
			{
				text = message;
			}
			else if (currentObjectDialogue.Count > 0)
			{
				text = currentObjectDialogue.Peek();
			}
			if (text != null)
			{
				Vector2 vector = dialogueFont.MeasureString(text);
				int width = (int)vector.X + 128;
				int height = (int)vector.Y + 128;
				int x = centerX - width / 2;
				int y = centerY - height / 2;
				drawDialogueBox(x, y, width, height, speaker, drawOnlyBox, message, objectDialoguePortraitPerson != null && !speaker);
			}
		}

		public static void DrawBox(int x, int y, int width, int height, Color? color = null)
		{
			Microsoft.Xna.Framework.Rectangle sourceRect = new Microsoft.Xna.Framework.Rectangle(0, 0, 64, 64);
			sourceRect.X = 64;
			sourceRect.Y = 128;
			Texture2D menu_texture = menuTexture;
			Color draw_color = Color.White;
			Color inner_color = Color.White;
			if (color.HasValue)
			{
				draw_color = color.Value;
				menu_texture = uncoloredMenuTexture;
				inner_color = new Color((int)Utility.Lerp((int)draw_color.R, Math.Min(255, draw_color.R + 150), 0.65f), (int)Utility.Lerp((int)draw_color.G, Math.Min(255, draw_color.G + 150), 0.65f), (int)Utility.Lerp((int)draw_color.B, Math.Min(255, draw_color.B + 150), 0.65f));
			}
			spriteBatch.Draw(menu_texture, new Microsoft.Xna.Framework.Rectangle(x, y, width, height), sourceRect, inner_color);
			sourceRect.Y = 0;
			Vector2 offset = new Vector2((float)(-sourceRect.Width) * 0.5f, (float)(-sourceRect.Height) * 0.5f);
			sourceRect.X = 0;
			spriteBatch.Draw(menu_texture, new Vector2((float)x + offset.X, (float)y + offset.Y), sourceRect, draw_color);
			sourceRect.X = 192;
			spriteBatch.Draw(menu_texture, new Vector2((float)x + offset.X + (float)width, (float)y + offset.Y), sourceRect, draw_color);
			sourceRect.Y = 192;
			spriteBatch.Draw(menu_texture, new Vector2((float)(x + width) + offset.X, (float)(y + height) + offset.Y), sourceRect, draw_color);
			sourceRect.X = 0;
			spriteBatch.Draw(menu_texture, new Vector2((float)x + offset.X, (float)(y + height) + offset.Y), sourceRect, draw_color);
			sourceRect.X = 128;
			sourceRect.Y = 0;
			spriteBatch.Draw(menu_texture, new Microsoft.Xna.Framework.Rectangle(64 + x + (int)offset.X, y + (int)offset.Y, width - 64, 64), sourceRect, draw_color);
			sourceRect.Y = 192;
			spriteBatch.Draw(menu_texture, new Microsoft.Xna.Framework.Rectangle(64 + x + (int)offset.X, y + (int)offset.Y + height, width - 64, 64), sourceRect, draw_color);
			sourceRect.Y = 128;
			sourceRect.X = 0;
			spriteBatch.Draw(menu_texture, new Microsoft.Xna.Framework.Rectangle(x + (int)offset.X, y + (int)offset.Y + 64, 64, height - 64), sourceRect, draw_color);
			sourceRect.X = 192;
			spriteBatch.Draw(menu_texture, new Microsoft.Xna.Framework.Rectangle(x + width + (int)offset.X, y + (int)offset.Y + 64, 64, height - 64), sourceRect, draw_color);
		}

		public static void drawDialogueBox(int x, int y, int width, int height, bool speaker, bool drawOnlyBox, string message = null, bool objectDialogueWithPortrait = false, bool ignoreTitleSafe = true, int r = -1, int g = -1, int b = -1)
		{
			if (!drawOnlyBox)
			{
				return;
			}
			Microsoft.Xna.Framework.Rectangle titleSafeArea = graphics.GraphicsDevice.Viewport.GetTitleSafeArea();
			int screenHeight = titleSafeArea.Height;
			int screenWidth = titleSafeArea.Width;
			int dialogueX = 0;
			int dialogueY = 0;
			if (!ignoreTitleSafe)
			{
				dialogueY = ((y <= titleSafeArea.Y) ? (titleSafeArea.Y - y) : 0);
			}
			int everythingYOffset = 0;
			width = Math.Min(titleSafeArea.Width, width);
			if (!isQuestion && currentSpeaker == null && currentObjectDialogue.Count > 0 && !drawOnlyBox)
			{
				width = (int)dialogueFont.MeasureString(currentObjectDialogue.Peek()).X + 128;
				height = (int)dialogueFont.MeasureString(currentObjectDialogue.Peek()).Y + 64;
				x = screenWidth / 2 - width / 2;
				everythingYOffset = ((height > 256) ? (-(height - 256)) : 0);
			}
			Microsoft.Xna.Framework.Rectangle sourceRect = new Microsoft.Xna.Framework.Rectangle(0, 0, 64, 64);
			int addedTileHeightForQuestions = -1;
			if (questionChoices.Count >= 3)
			{
				addedTileHeightForQuestions = questionChoices.Count - 3;
			}
			if (!drawOnlyBox && currentObjectDialogue.Count > 0)
			{
				if (dialogueFont.MeasureString(currentObjectDialogue.Peek()).Y >= (float)(height - 128))
				{
					addedTileHeightForQuestions -= (int)(((float)(height - 128) - dialogueFont.MeasureString(currentObjectDialogue.Peek()).Y) / 64f) - 1;
				}
				else
				{
					height += (int)dialogueFont.MeasureString(currentObjectDialogue.Peek()).Y / 2;
					everythingYOffset -= (int)dialogueFont.MeasureString(currentObjectDialogue.Peek()).Y / 2;
					if ((int)dialogueFont.MeasureString(currentObjectDialogue.Peek()).Y / 2 > 64)
					{
						addedTileHeightForQuestions = 0;
					}
				}
			}
			if (currentSpeaker != null && isQuestion && currentSpeaker.CurrentDialogue.Peek().getCurrentDialogue().Substring(0, currentDialogueCharacterIndex)
				.Contains(Environment.NewLine))
			{
				addedTileHeightForQuestions++;
			}
			sourceRect.Width = 64;
			sourceRect.Height = 64;
			sourceRect.X = 64;
			sourceRect.Y = 128;
			Color tint = ((r == -1) ? Color.White : new Color(r, g, b));
			Texture2D texture = ((r == -1) ? menuTexture : uncoloredMenuTexture);
			spriteBatch.Draw(texture, new Microsoft.Xna.Framework.Rectangle(28 + x + dialogueX, 28 + y - 64 * addedTileHeightForQuestions + dialogueY + everythingYOffset, width - 64, height - 64 + addedTileHeightForQuestions * 64), sourceRect, (r == -1) ? tint : new Color((int)Utility.Lerp(r, Math.Min(255, r + 150), 0.65f), (int)Utility.Lerp(g, Math.Min(255, g + 150), 0.65f), (int)Utility.Lerp(b, Math.Min(255, b + 150), 0.65f)));
			sourceRect.Y = 0;
			sourceRect.X = 0;
			spriteBatch.Draw(texture, new Vector2(x + dialogueX, y - 64 * addedTileHeightForQuestions + dialogueY + everythingYOffset), sourceRect, tint);
			sourceRect.X = 192;
			spriteBatch.Draw(texture, new Vector2(x + width + dialogueX - 64, y - 64 * addedTileHeightForQuestions + dialogueY + everythingYOffset), sourceRect, tint);
			sourceRect.Y = 192;
			spriteBatch.Draw(texture, new Vector2(x + width + dialogueX - 64, y + height + dialogueY - 64 + everythingYOffset), sourceRect, tint);
			sourceRect.X = 0;
			spriteBatch.Draw(texture, new Vector2(x + dialogueX, y + height + dialogueY - 64 + everythingYOffset), sourceRect, tint);
			sourceRect.X = 128;
			sourceRect.Y = 0;
			spriteBatch.Draw(texture, new Microsoft.Xna.Framework.Rectangle(64 + x + dialogueX, y - 64 * addedTileHeightForQuestions + dialogueY + everythingYOffset, width - 128, 64), sourceRect, tint);
			sourceRect.Y = 192;
			spriteBatch.Draw(texture, new Microsoft.Xna.Framework.Rectangle(64 + x + dialogueX, y + height + dialogueY - 64 + everythingYOffset, width - 128, 64), sourceRect, tint);
			sourceRect.Y = 128;
			sourceRect.X = 0;
			spriteBatch.Draw(texture, new Microsoft.Xna.Framework.Rectangle(x + dialogueX, y - 64 * addedTileHeightForQuestions + dialogueY + 64 + everythingYOffset, 64, height - 128 + addedTileHeightForQuestions * 64), sourceRect, tint);
			sourceRect.X = 192;
			spriteBatch.Draw(texture, new Microsoft.Xna.Framework.Rectangle(x + width + dialogueX - 64, y - 64 * addedTileHeightForQuestions + dialogueY + 64 + everythingYOffset, 64, height - 128 + addedTileHeightForQuestions * 64), sourceRect, tint);
			if ((objectDialogueWithPortrait && objectDialoguePortraitPerson != null) || (speaker && currentSpeaker != null && currentSpeaker.CurrentDialogue.Count > 0 && currentSpeaker.CurrentDialogue.Peek().showPortrait))
			{
				Microsoft.Xna.Framework.Rectangle portraitRect = new Microsoft.Xna.Framework.Rectangle(0, 0, 64, 64);
				NPC theSpeaker = (objectDialogueWithPortrait ? objectDialoguePortraitPerson : currentSpeaker);
				switch ((!objectDialogueWithPortrait) ? theSpeaker.CurrentDialogue.Peek().CurrentEmotion : (objectDialoguePortraitPerson.Name.Equals(player.spouse) ? "$l" : "$neutral"))
				{
				case "$h":
					portraitRect = new Microsoft.Xna.Framework.Rectangle(64, 0, 64, 64);
					break;
				case "$s":
					portraitRect = new Microsoft.Xna.Framework.Rectangle(0, 64, 64, 64);
					break;
				case "$u":
					portraitRect = new Microsoft.Xna.Framework.Rectangle(64, 64, 64, 64);
					break;
				case "$l":
					portraitRect = new Microsoft.Xna.Framework.Rectangle(0, 128, 64, 64);
					break;
				case "$a":
					portraitRect = new Microsoft.Xna.Framework.Rectangle(64, 128, 64, 64);
					break;
				case "$k":
				case "$neutral":
					portraitRect = new Microsoft.Xna.Framework.Rectangle(0, 0, 64, 64);
					break;
				default:
					portraitRect = getSourceRectForStandardTileSheet(theSpeaker.Portrait, Convert.ToInt32(theSpeaker.CurrentDialogue.Peek().CurrentEmotion.Substring(1)));
					break;
				}
				spriteBatch.End();
				spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp);
				if (theSpeaker.Portrait != null)
				{
					spriteBatch.Draw(mouseCursors, new Vector2(dialogueX + x + 768, screenHeight - 320 - 64 * addedTileHeightForQuestions - 256 + dialogueY + 16 - 60 + everythingYOffset), new Microsoft.Xna.Framework.Rectangle(333, 305, 80, 87), Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, 0.98f);
					spriteBatch.Draw(theSpeaker.Portrait, new Vector2(dialogueX + x + 768 + 32, screenHeight - 320 - 64 * addedTileHeightForQuestions - 256 + dialogueY + 16 - 60 + everythingYOffset), portraitRect, Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, 0.99f);
				}
				spriteBatch.End();
				spriteBatch.Begin();
				if (isQuestion)
				{
					spriteBatch.DrawString(dialogueFont, theSpeaker.displayName, new Vector2(928f - dialogueFont.MeasureString(theSpeaker.displayName).X / 2f + (float)dialogueX + (float)x, (float)(screenHeight - 320 - 64 * addedTileHeightForQuestions) - dialogueFont.MeasureString(theSpeaker.displayName).Y + (float)dialogueY + 21f + (float)everythingYOffset) + new Vector2(2f, 2f), new Color(150, 150, 150));
				}
				spriteBatch.DrawString(dialogueFont, theSpeaker.Name.Equals("DwarfKing") ? content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.3754") : (theSpeaker.Name.Equals("Lewis") ? content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.3756") : theSpeaker.displayName), new Vector2((float)(dialogueX + x + 896 + 32) - dialogueFont.MeasureString(theSpeaker.Name.Equals("Lewis") ? content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.3756") : theSpeaker.displayName).X / 2f, (float)(screenHeight - 320 - 64 * addedTileHeightForQuestions) - dialogueFont.MeasureString(theSpeaker.Name.Equals("Lewis") ? content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.3756") : theSpeaker.displayName).Y + (float)dialogueY + 21f + 8f + (float)everythingYOffset), textColor);
			}
			if (drawOnlyBox || (nameSelectUp && (!messagePause || currentObjectDialogue == null)))
			{
				return;
			}
			string text = "";
			if (currentSpeaker != null && currentSpeaker.CurrentDialogue.Count > 0)
			{
				if (currentSpeaker.CurrentDialogue.Peek() == null || currentSpeaker.CurrentDialogue.Peek().getCurrentDialogue().Length < currentDialogueCharacterIndex - 1)
				{
					dialogueUp = false;
					currentDialogueCharacterIndex = 0;
					playSound("dialogueCharacterClose");
					player.forceCanMove();
					return;
				}
				text = currentSpeaker.CurrentDialogue.Peek().getCurrentDialogue().Substring(0, currentDialogueCharacterIndex);
			}
			else if (message != null)
			{
				text = message;
			}
			else if (currentObjectDialogue.Count > 0)
			{
				text = ((currentObjectDialogue.Peek().Length <= 1) ? "" : currentObjectDialogue.Peek().Substring(0, currentDialogueCharacterIndex));
			}
			Vector2 textPosition = ((dialogueFont.MeasureString(text).X > (float)(screenWidth - 256 - dialogueX)) ? new Vector2(128 + dialogueX, screenHeight - 64 * addedTileHeightForQuestions - 256 - 16 + dialogueY + everythingYOffset) : ((currentSpeaker != null && currentSpeaker.CurrentDialogue.Count > 0) ? new Vector2((float)(screenWidth / 2) - dialogueFont.MeasureString(currentSpeaker.CurrentDialogue.Peek().getCurrentDialogue()).X / 2f + (float)dialogueX, screenHeight - 64 * addedTileHeightForQuestions - 256 - 16 + dialogueY + everythingYOffset) : ((message != null) ? new Vector2((float)(screenWidth / 2) - dialogueFont.MeasureString(text).X / 2f + (float)dialogueX, y + 96 + 4) : ((!isQuestion) ? new Vector2((float)(screenWidth / 2) - dialogueFont.MeasureString((currentObjectDialogue.Count == 0) ? "" : currentObjectDialogue.Peek()).X / 2f + (float)dialogueX, y + 4 + everythingYOffset) : new Vector2((float)(screenWidth / 2) - dialogueFont.MeasureString((currentObjectDialogue.Count == 0) ? "" : currentObjectDialogue.Peek()).X / 2f + (float)dialogueX, screenHeight - 64 * addedTileHeightForQuestions - 256 - (16 + (questionChoices.Count - 2) * 64) + dialogueY + everythingYOffset)))));
			if (!drawOnlyBox)
			{
				spriteBatch.DrawString(dialogueFont, text, textPosition + new Vector2(3f, 0f), textShadowColor);
				spriteBatch.DrawString(dialogueFont, text, textPosition + new Vector2(3f, 3f), textShadowColor);
				spriteBatch.DrawString(dialogueFont, text, textPosition + new Vector2(0f, 3f), textShadowColor);
				spriteBatch.DrawString(dialogueFont, text, textPosition, textColor);
			}
			if (dialogueFont.MeasureString(text).Y <= 64f)
			{
				dialogueY += 64;
			}
			if (isQuestion && !dialogueTyping)
			{
				for (int i = 0; i < questionChoices.Count; i++)
				{
					if (currentQuestionChoice == i)
					{
						textPosition.X = 80 + dialogueX + x;
						textPosition.Y = (float)(screenHeight - (5 + addedTileHeightForQuestions + 1) * 64) + ((text.Trim().Length > 0) ? dialogueFont.MeasureString(text).Y : 0f) + 128f + (float)(48 * i) - (float)(16 + (questionChoices.Count - 2) * 64) + (float)dialogueY + (float)everythingYOffset;
						spriteBatch.End();
						spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp);
						spriteBatch.Draw(objectSpriteSheet, textPosition + new Vector2((float)Math.Cos((double)currentGameTime.TotalGameTime.Milliseconds * Math.PI / 512.0) * 3f, 0f), GameLocation.getSourceRectForObject(26), Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, 1f);
						spriteBatch.End();
						spriteBatch.Begin();
						textPosition.X = 160 + dialogueX + x;
						textPosition.Y = (float)(screenHeight - (5 + addedTileHeightForQuestions + 1) * 64) + ((text.Trim().Length > 1) ? dialogueFont.MeasureString(text).Y : 0f) + 128f - (float)((questionChoices.Count - 2) * 64) + (float)(48 * i) + (float)dialogueY + (float)everythingYOffset;
						spriteBatch.DrawString(dialogueFont, questionChoices[i].responseText, textPosition, textColor);
					}
					else
					{
						textPosition.X = 128 + dialogueX + x;
						textPosition.Y = (float)(screenHeight - (5 + addedTileHeightForQuestions + 1) * 64) + ((text.Trim().Length > 1) ? dialogueFont.MeasureString(text).Y : 0f) + 128f - (float)((questionChoices.Count - 2) * 64) + (float)(48 * i) + (float)dialogueY + (float)everythingYOffset;
						spriteBatch.DrawString(dialogueFont, questionChoices[i].responseText, textPosition, unselectedOptionColor);
					}
				}
			}
			else if (numberOfSelectedItems != -1 && !dialogueTyping)
			{
				drawItemSelectDialogue(x, y, dialogueX, dialogueY + everythingYOffset, screenHeight, addedTileHeightForQuestions, text);
			}
			if (!drawOnlyBox && !dialogueTyping && message == null)
			{
				spriteBatch.Draw(mouseCursors, new Vector2(x + dialogueX + width - 96, (float)(y + height + dialogueY + everythingYOffset - 96) - dialogueButtonScale), getSourceRectForStandardTileSheet(mouseCursors, (!dialogueButtonShrinking && dialogueButtonScale < 8f) ? 3 : 2), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.9999999f);
			}
		}

		private static void drawItemSelectDialogue(int x, int y, int dialogueX, int dialogueY, int screenHeight, int addedTileHeightForQuestions, string text)
		{
			string whatToDraw = "";
			switch (selectedItemsType)
			{
			case "flutePitch":
			case "drumTome":
				whatToDraw = "@ " + numberOfSelectedItems + " >  ";
				break;
			case "jukebox":
				whatToDraw = "@ " + player.songsHeard.ElementAt(numberOfSelectedItems) + " >  ";
				break;
			default:
				whatToDraw = "@ " + numberOfSelectedItems + " >  " + priceOfSelectedItem * numberOfSelectedItems + "g";
				break;
			}
			if (currentLocation.Name.Equals("Club"))
			{
				whatToDraw = "@ " + numberOfSelectedItems + " >  ";
			}
			spriteBatch.DrawString(dialogueFont, whatToDraw, new Vector2(dialogueX + x + 64, (float)(screenHeight - (5 + addedTileHeightForQuestions + 1) * 64) + dialogueFont.MeasureString(text).Y + 104f + (float)dialogueY), textColor);
		}

		protected void drawFarmBuildings()
		{
			_ = player.CoopUpgradeLevel;
			_ = 0;
			switch (player.BarnUpgradeLevel)
			{
			case 1:
				spriteBatch.Draw(currentBarnTexture, GlobalToLocal(viewport, new Vector2(768f, 320f)), currentBarnTexture.Bounds, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, Math.Max(0f, 0.0576f));
				break;
			case 2:
				spriteBatch.Draw(currentBarnTexture, GlobalToLocal(viewport, new Vector2(640f, 256f)), currentBarnTexture.Bounds, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, Math.Max(0f, 0.0576f));
				break;
			}
			if (player.hasGreenhouse)
			{
				spriteBatch.Draw(greenhouseTexture, GlobalToLocal(viewport, new Vector2(64f, 320f)), greenhouseTexture.Bounds, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, Math.Max(0f, 0.0576f));
			}
		}

		public static void drawPlayerHeldObject(Farmer f)
		{
			if ((!eventUp || (currentLocation.currentEvent != null && currentLocation.currentEvent.showActiveObject)) && !f.FarmerSprite.PauseForSingleAnimation && !f.isRidingHorse() && !f.bathingClothes && !f.onBridge.Value)
			{
				float xPosition = f.getLocalPosition(viewport).X + (float)((f.rotation < 0f) ? (-8) : ((f.rotation > 0f) ? 8 : 0)) + (float)(f.FarmerSprite.CurrentAnimationFrame.xOffset * 4);
				float objectYLoc = f.getLocalPosition(viewport).Y - 128f + (float)(f.FarmerSprite.CurrentAnimationFrame.positionOffset * 4) + (float)(FarmerRenderer.featureYOffsetPerFrame[f.FarmerSprite.CurrentFrame] * 4);
				if ((bool)f.ActiveObject.bigCraftable)
				{
					objectYLoc -= 64f;
				}
				if (f.isEating)
				{
					xPosition = f.getLocalPosition(viewport).X - 21f;
					objectYLoc = f.getLocalPosition(viewport).Y - 128f + 12f;
				}
				if (!f.isEating || (f.isEating && f.Sprite.currentFrame <= 218))
				{
					f.ActiveObject.drawWhenHeld(spriteBatch, new Vector2((int)xPosition, (int)objectYLoc), f);
				}
			}
		}

		public static void drawTool(Farmer f)
		{
			drawTool(f, f.CurrentTool.CurrentParentTileIndex);
		}

		public static void drawTool(Farmer f, int currentToolIndex)
		{
			Microsoft.Xna.Framework.Rectangle sourceRectangleForTool = new Microsoft.Xna.Framework.Rectangle(currentToolIndex * 16 % toolSpriteSheet.Width, currentToolIndex * 16 / toolSpriteSheet.Width * 16, 16, 32);
			Vector2 fPosition = f.getLocalPosition(viewport) + f.jitter + f.armOffset;
			float tool_draw_layer_offset = 0f;
			if (f.FacingDirection == 0)
			{
				tool_draw_layer_offset = -0.002f;
			}
			if (pickingTool)
			{
				int yLocation = (int)fPosition.Y - 128;
				spriteBatch.Draw(toolSpriteSheet, new Vector2(fPosition.X, yLocation), sourceRectangleForTool, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, Math.Max(0f, tool_draw_layer_offset + (float)(f.getStandingY() + 32) / 10000f));
				return;
			}
			if (f.CurrentTool is MeleeWeapon)
			{
				((MeleeWeapon)f.CurrentTool).drawDuringUse(((FarmerSprite)f.Sprite).currentAnimationIndex, f.FacingDirection, spriteBatch, fPosition, f);
				return;
			}
			if (f.FarmerSprite.isUsingWeapon())
			{
				MeleeWeapon.drawDuringUse(((FarmerSprite)f.Sprite).currentAnimationIndex, f.FacingDirection, spriteBatch, fPosition, f, MeleeWeapon.getSourceRect(f.FarmerSprite.CurrentToolIndex), f.FarmerSprite.getWeaponTypeFromAnimation(), isOnSpecial: false);
				return;
			}
			if (f.CurrentTool is FishingRod)
			{
				if ((f.CurrentTool as FishingRod).fishCaught || (f.CurrentTool as FishingRod).showingTreasure)
				{
					f.CurrentTool.draw(spriteBatch);
					return;
				}
				sourceRectangleForTool = new Microsoft.Xna.Framework.Rectangle(((FarmerSprite)f.Sprite).currentAnimationIndex * 48, 288, 48, 48);
				if (f.FacingDirection == 2 || f.FacingDirection == 0)
				{
					sourceRectangleForTool.Y += 48;
				}
				else if ((f.CurrentTool as FishingRod).isFishing && (!(f.CurrentTool as FishingRod).isReeling || (f.CurrentTool as FishingRod).hit))
				{
					fPosition.Y += 8f;
				}
				if ((f.CurrentTool as FishingRod).isFishing)
				{
					sourceRectangleForTool.X += (5 - ((FarmerSprite)f.Sprite).currentAnimationIndex) * 48;
				}
				if ((f.CurrentTool as FishingRod).isReeling)
				{
					if (f.FacingDirection == 2 || f.FacingDirection == 0)
					{
						sourceRectangleForTool.X = 288;
						if (f.IsLocalPlayer && didPlayerJustClickAtAll())
						{
							sourceRectangleForTool.X = 0;
						}
					}
					else
					{
						sourceRectangleForTool.X = 288;
						sourceRectangleForTool.Y = 240;
						if (f.IsLocalPlayer && didPlayerJustClickAtAll())
						{
							sourceRectangleForTool.Y += 48;
						}
					}
				}
				if (f.FarmerSprite.CurrentFrame == 57)
				{
					sourceRectangleForTool.Height = 0;
				}
				if (f.FacingDirection == 0)
				{
					fPosition.X += 16f;
				}
			}
			if (f.CurrentTool != null)
			{
				f.CurrentTool.draw(spriteBatch);
			}
			if (f.CurrentTool is Slingshot || f.CurrentTool is Shears || f.CurrentTool is MilkPail || f.CurrentTool is Pan)
			{
				return;
			}
			int toolYOffset = 0;
			int toolXOffset = 0;
			if (f.CurrentTool is WateringCan)
			{
				toolYOffset += 80;
				toolXOffset = ((f.FacingDirection == 1) ? 32 : ((f.FacingDirection == 3) ? (-32) : 0));
				if (((FarmerSprite)f.Sprite).currentAnimationIndex == 0 || ((FarmerSprite)f.Sprite).currentAnimationIndex == 1)
				{
					toolXOffset = toolXOffset * 3 / 2;
				}
			}
			if (f.FacingDirection == 1)
			{
				int layerDepthOffset = 0;
				if (((FarmerSprite)f.Sprite).currentAnimationIndex > 2)
				{
					Point tileLocation3 = f.getTileLocationPoint();
					tileLocation3.X++;
					tileLocation3.Y--;
					if (!(f.CurrentTool is WateringCan) && f.currentLocation.getTileIndexAt(tileLocation3, "Front") != -1)
					{
						return;
					}
					tileLocation3.Y++;
					if (f.currentLocation.getTileIndexAt(tileLocation3, "Front") == -1)
					{
						layerDepthOffset += 16;
					}
				}
				else if (f.CurrentTool is WateringCan && ((FarmerSprite)f.Sprite).currentAnimationIndex == 1)
				{
					Point tileLocation4 = f.getTileLocationPoint();
					tileLocation4.X--;
					tileLocation4.Y--;
					if (f.currentLocation.getTileIndexAt(tileLocation4, "Front") != -1 && f.Position.Y % 64f < 32f)
					{
						return;
					}
				}
				if (f.CurrentTool != null && f.CurrentTool is FishingRod)
				{
					Color color2 = (f.CurrentTool as FishingRod).getColor();
					switch (((FarmerSprite)f.Sprite).currentAnimationIndex)
					{
					case 0:
						if ((f.CurrentTool as FishingRod).isReeling)
						{
							spriteBatch.Draw(toolSpriteSheet, new Vector2(fPosition.X - 64f + (float)toolXOffset, fPosition.Y - 160f + (float)toolYOffset), sourceRectangleForTool, color2, 0f, Vector2.Zero, 4f, SpriteEffects.None, Math.Max(0f, tool_draw_layer_offset + (float)(f.getStandingY() + 64) / 10000f));
						}
						else if ((f.CurrentTool as FishingRod).isFishing || (f.CurrentTool as FishingRod).doneWithAnimation)
						{
							spriteBatch.Draw(toolSpriteSheet, new Vector2(fPosition.X - 64f + (float)toolXOffset, fPosition.Y - 160f + (float)toolYOffset), sourceRectangleForTool, color2, 0f, Vector2.Zero, 4f, SpriteEffects.None, Math.Max(0f, tool_draw_layer_offset + (float)(f.getStandingY() + 64) / 10000f));
						}
						else if (!(f.CurrentTool as FishingRod).hasDoneFucntionYet || (f.CurrentTool as FishingRod).pullingOutOfWater)
						{
							spriteBatch.Draw(toolSpriteSheet, new Vector2(fPosition.X - 64f + (float)toolXOffset, fPosition.Y - 160f + (float)toolYOffset), sourceRectangleForTool, color2, 0f, Vector2.Zero, 4f, SpriteEffects.None, Math.Max(0f, tool_draw_layer_offset + (float)(f.getStandingY() + 64) / 10000f));
						}
						break;
					case 1:
						spriteBatch.Draw(toolSpriteSheet, new Vector2(fPosition.X - 64f + (float)toolXOffset, fPosition.Y - 160f + 8f + (float)toolYOffset), sourceRectangleForTool, color2, 0f, Vector2.Zero, 4f, SpriteEffects.None, Math.Max(0f, tool_draw_layer_offset + (float)(f.getStandingY() + 64) / 10000f));
						break;
					case 2:
						spriteBatch.Draw(toolSpriteSheet, new Vector2(fPosition.X - 96f + 32f + (float)toolXOffset, fPosition.Y - 128f - 24f + (float)toolYOffset), sourceRectangleForTool, color2, 0f, Vector2.Zero, 4f, SpriteEffects.None, Math.Max(0f, tool_draw_layer_offset + (float)(f.getStandingY() + 64) / 10000f));
						break;
					case 3:
						spriteBatch.Draw(toolSpriteSheet, new Vector2(fPosition.X - 96f + 24f + (float)toolXOffset, fPosition.Y - 128f - 32f + (float)toolYOffset), sourceRectangleForTool, color2, 0f, Vector2.Zero, 4f, SpriteEffects.None, Math.Max(0f, tool_draw_layer_offset + (float)(f.getStandingY() + 64) / 10000f));
						break;
					case 4:
						if ((f.CurrentTool as FishingRod).isFishing || (f.CurrentTool as FishingRod).doneWithAnimation)
						{
							spriteBatch.Draw(toolSpriteSheet, new Vector2(fPosition.X - 64f + (float)toolXOffset, fPosition.Y - 160f + (float)toolYOffset), sourceRectangleForTool, color2, 0f, Vector2.Zero, 4f, SpriteEffects.None, Math.Max(0f, tool_draw_layer_offset + (float)(f.getStandingY() + 64) / 10000f));
						}
						else
						{
							spriteBatch.Draw(toolSpriteSheet, new Vector2(fPosition.X - 64f + (float)toolXOffset, fPosition.Y - 160f + 4f + (float)toolYOffset), sourceRectangleForTool, color2, 0f, Vector2.Zero, 4f, SpriteEffects.None, Math.Max(0f, tool_draw_layer_offset + (float)(f.getStandingY() + 64) / 10000f));
						}
						break;
					case 5:
						spriteBatch.Draw(toolSpriteSheet, new Vector2(fPosition.X - 64f + (float)toolXOffset, fPosition.Y - 160f + (float)toolYOffset), sourceRectangleForTool, color2, 0f, Vector2.Zero, 4f, SpriteEffects.None, Math.Max(0f, tool_draw_layer_offset + (float)(f.getStandingY() + 64) / 10000f));
						break;
					}
				}
				else if (f.CurrentTool != null && f.CurrentTool.Name.Contains("Sword"))
				{
					switch (((FarmerSprite)f.Sprite).currentAnimationIndex)
					{
					case 0:
						spriteBatch.Draw(toolSpriteSheet, Utility.snapToInt(new Vector2(fPosition.X + 64f - 20f, fPosition.Y + 28f)), sourceRectangleForTool, Color.White, -(float)Math.PI / 8f, new Vector2(4f, 60f), 1f, SpriteEffects.None, Math.Max(0f, tool_draw_layer_offset + (float)(f.getStandingY() + 64) / 10000f));
						break;
					case 1:
						spriteBatch.Draw(toolSpriteSheet, Utility.snapToInt(new Vector2(fPosition.X + 64f - 12f, fPosition.Y + 64f - 8f)), sourceRectangleForTool, Color.White, 0f, new Vector2(4f, 60f), 1f, SpriteEffects.None, Math.Max(0f, tool_draw_layer_offset + (float)(f.getStandingY() + 64) / 10000f));
						break;
					case 2:
						spriteBatch.Draw(toolSpriteSheet, Utility.snapToInt(new Vector2(fPosition.X + 64f - 12f, fPosition.Y + 64f - 4f)), sourceRectangleForTool, Color.White, (float)Math.PI / 8f, new Vector2(4f, 60f), 1f, SpriteEffects.None, Math.Max(0f, tool_draw_layer_offset + (float)(f.getStandingY() + 64) / 10000f));
						break;
					case 3:
						spriteBatch.Draw(toolSpriteSheet, Utility.snapToInt(new Vector2(fPosition.X + 64f - 12f, fPosition.Y + 64f)), sourceRectangleForTool, Color.White, 0.7853981f, new Vector2(4f, 60f), 1f, SpriteEffects.None, Math.Max(0f, tool_draw_layer_offset + (float)(f.getStandingY() + 64) / 10000f));
						break;
					case 4:
						spriteBatch.Draw(toolSpriteSheet, Utility.snapToInt(new Vector2(fPosition.X + 64f - 16f, fPosition.Y + 64f + 4f)), sourceRectangleForTool, Color.White, (float)Math.PI * 3f / 8f, new Vector2(4f, 60f), 1f, SpriteEffects.None, Math.Max(0f, tool_draw_layer_offset + (float)(f.getStandingY() + 64) / 10000f));
						break;
					case 5:
						spriteBatch.Draw(toolSpriteSheet, Utility.snapToInt(new Vector2(fPosition.X + 64f - 16f, fPosition.Y + 64f + 8f)), sourceRectangleForTool, Color.White, (float)Math.PI / 2f, new Vector2(4f, 60f), 1f, SpriteEffects.None, Math.Max(0f, tool_draw_layer_offset + (float)(f.getStandingY() + 64) / 10000f));
						break;
					case 6:
						spriteBatch.Draw(toolSpriteSheet, Utility.snapToInt(new Vector2(fPosition.X + 64f - 16f, fPosition.Y + 64f + 12f)), sourceRectangleForTool, Color.White, 1.9634954f, new Vector2(4f, 60f), 1f, SpriteEffects.None, Math.Max(0f, tool_draw_layer_offset + (float)(f.getStandingY() + 64) / 10000f));
						break;
					case 7:
						spriteBatch.Draw(toolSpriteSheet, Utility.snapToInt(new Vector2(fPosition.X + 64f - 16f, fPosition.Y + 64f + 12f)), sourceRectangleForTool, Color.White, 1.9634954f, new Vector2(4f, 60f), 1f, SpriteEffects.None, Math.Max(0f, tool_draw_layer_offset + (float)(f.getStandingY() + 64) / 10000f));
						break;
					}
				}
				else if (f.CurrentTool != null && f.CurrentTool is WateringCan)
				{
					switch (((FarmerSprite)f.Sprite).currentAnimationIndex)
					{
					case 0:
					case 1:
						spriteBatch.Draw(toolSpriteSheet, new Vector2((int)(fPosition.X + (float)toolXOffset - 4f), (int)(fPosition.Y - 128f + 8f + (float)toolYOffset)), sourceRectangleForTool, Color.White, 0f, new Vector2(0f, 16f), 4f, SpriteEffects.None, Math.Max(0f, tool_draw_layer_offset + (float)(f.GetBoundingBox().Bottom + layerDepthOffset) / 10000f));
						break;
					case 2:
						spriteBatch.Draw(toolSpriteSheet, new Vector2((int)fPosition.X + toolXOffset + 24, (int)(fPosition.Y - 128f - 8f + (float)toolYOffset)), sourceRectangleForTool, Color.White, (float)Math.PI / 12f, new Vector2(0f, 16f), 4f, SpriteEffects.None, Math.Max(0f, tool_draw_layer_offset + (float)(f.GetBoundingBox().Bottom + layerDepthOffset) / 10000f));
						break;
					case 3:
						sourceRectangleForTool.X += 16;
						spriteBatch.Draw(toolSpriteSheet, new Vector2((int)(fPosition.X + (float)toolXOffset + 8f), (int)(fPosition.Y - 128f - 24f + (float)toolYOffset)), sourceRectangleForTool, Color.White, 0f, new Vector2(0f, 16f), 4f, SpriteEffects.None, Math.Max(0f, tool_draw_layer_offset + (float)(f.GetBoundingBox().Bottom + layerDepthOffset) / 10000f));
						break;
					}
				}
				else
				{
					switch (((FarmerSprite)f.Sprite).currentAnimationIndex)
					{
					case 0:
						spriteBatch.Draw(toolSpriteSheet, Utility.snapToInt(new Vector2(fPosition.X - 32f - 4f + (float)toolXOffset - (float)Math.Min(8, f.toolPower * 4), fPosition.Y - 128f + 24f + (float)toolYOffset + (float)Math.Min(8, f.toolPower * 4))), sourceRectangleForTool, Color.White, -(float)Math.PI / 12f - (float)Math.Min(f.toolPower, 2) * ((float)Math.PI / 64f), new Vector2(0f, 16f), 4f, SpriteEffects.None, Math.Max(0f, tool_draw_layer_offset + (float)(f.GetBoundingBox().Bottom + layerDepthOffset) / 10000f));
						break;
					case 1:
						spriteBatch.Draw(toolSpriteSheet, Utility.snapToInt(new Vector2(fPosition.X + 32f - 24f + (float)toolXOffset, fPosition.Y - 124f + (float)toolYOffset + 64f)), sourceRectangleForTool, Color.White, (float)Math.PI / 12f, new Vector2(0f, 32f), 4f, SpriteEffects.None, Math.Max(0f, tool_draw_layer_offset + (float)(f.GetBoundingBox().Bottom + layerDepthOffset) / 10000f));
						break;
					case 2:
						spriteBatch.Draw(toolSpriteSheet, Utility.snapToInt(new Vector2(fPosition.X + 32f + (float)toolXOffset - 4f, fPosition.Y - 132f + (float)toolYOffset + 64f)), sourceRectangleForTool, Color.White, (float)Math.PI / 4f, new Vector2(0f, 32f), 4f, SpriteEffects.None, Math.Max(0f, tool_draw_layer_offset + (float)(f.GetBoundingBox().Bottom + layerDepthOffset) / 10000f));
						break;
					case 3:
						spriteBatch.Draw(toolSpriteSheet, Utility.snapToInt(new Vector2(fPosition.X + 32f + 28f + (float)toolXOffset, fPosition.Y - 64f + (float)toolYOffset)), sourceRectangleForTool, Color.White, (float)Math.PI * 7f / 12f, new Vector2(0f, 32f), 4f, SpriteEffects.None, Math.Max(0f, tool_draw_layer_offset + (float)(f.GetBoundingBox().Bottom + layerDepthOffset) / 10000f));
						break;
					case 4:
						spriteBatch.Draw(toolSpriteSheet, Utility.snapToInt(new Vector2(fPosition.X + 32f + 28f + (float)toolXOffset, fPosition.Y - 64f + 4f + (float)toolYOffset)), sourceRectangleForTool, Color.White, (float)Math.PI * 7f / 12f, new Vector2(0f, 32f), 4f, SpriteEffects.None, Math.Max(0f, tool_draw_layer_offset + (float)(f.GetBoundingBox().Bottom + layerDepthOffset) / 10000f));
						break;
					case 5:
						spriteBatch.Draw(toolSpriteSheet, Utility.snapToInt(new Vector2(fPosition.X + 64f + 12f + (float)toolXOffset, fPosition.Y - 128f + 32f + (float)toolYOffset + 128f)), sourceRectangleForTool, Color.White, (float)Math.PI / 4f, new Vector2(0f, 32f), 4f, SpriteEffects.None, Math.Max(0f, tool_draw_layer_offset + (float)(f.GetBoundingBox().Bottom + layerDepthOffset) / 10000f));
						break;
					case 6:
						spriteBatch.Draw(toolSpriteSheet, Utility.snapToInt(new Vector2(fPosition.X + 42f + 8f + (float)toolXOffset, fPosition.Y - 64f + 24f + (float)toolYOffset + 128f)), sourceRectangleForTool, Color.White, 0f, new Vector2(0f, 128f), 4f, SpriteEffects.None, Math.Max(0f, tool_draw_layer_offset + (float)(f.GetBoundingBox().Bottom + layerDepthOffset) / 10000f));
						break;
					}
				}
			}
			else if (f.FacingDirection == 3)
			{
				int layerDepthOffset2 = 0;
				if (((FarmerSprite)f.Sprite).currentAnimationIndex > 2)
				{
					Point tileLocation6 = f.getTileLocationPoint();
					tileLocation6.X--;
					tileLocation6.Y--;
					if (!(f.CurrentTool is WateringCan) && f.currentLocation.getTileIndexAt(tileLocation6, "Front") != -1 && f.Position.Y % 64f < 32f)
					{
						return;
					}
					tileLocation6.Y++;
					if (f.currentLocation.getTileIndexAt(tileLocation6, "Front") == -1)
					{
						layerDepthOffset2 += 16;
					}
				}
				else if (f.CurrentTool is WateringCan && ((FarmerSprite)f.Sprite).currentAnimationIndex == 1)
				{
					Point tileLocation5 = f.getTileLocationPoint();
					tileLocation5.X--;
					tileLocation5.Y--;
					if (f.currentLocation.getTileIndexAt(tileLocation5, "Front") != -1 && f.Position.Y % 64f < 32f)
					{
						return;
					}
				}
				if (f.CurrentTool != null && f.CurrentTool is FishingRod)
				{
					Color color3 = (f.CurrentTool as FishingRod).getColor();
					switch (((FarmerSprite)f.Sprite).currentAnimationIndex)
					{
					case 0:
						if ((f.CurrentTool as FishingRod).isReeling)
						{
							spriteBatch.Draw(toolSpriteSheet, Utility.snapToInt(new Vector2(fPosition.X - 64f + (float)toolXOffset, fPosition.Y - 160f + (float)toolYOffset)), sourceRectangleForTool, color3, 0f, Vector2.Zero, 4f, SpriteEffects.FlipHorizontally, Math.Max(0f, tool_draw_layer_offset + (float)(f.getStandingY() + 64) / 10000f));
						}
						else if ((f.CurrentTool as FishingRod).isFishing || (f.CurrentTool as FishingRod).doneWithAnimation)
						{
							spriteBatch.Draw(toolSpriteSheet, Utility.snapToInt(new Vector2(fPosition.X - 64f + (float)toolXOffset, fPosition.Y - 160f + (float)toolYOffset)), sourceRectangleForTool, color3, 0f, Vector2.Zero, 4f, SpriteEffects.FlipHorizontally, Math.Max(0f, tool_draw_layer_offset + (float)(f.getStandingY() + 64) / 10000f));
						}
						else if (!(f.CurrentTool as FishingRod).hasDoneFucntionYet || (f.CurrentTool as FishingRod).pullingOutOfWater)
						{
							spriteBatch.Draw(toolSpriteSheet, Utility.snapToInt(new Vector2(fPosition.X - 64f + (float)toolXOffset, fPosition.Y - 160f + (float)toolYOffset)), sourceRectangleForTool, color3, 0f, Vector2.Zero, 4f, SpriteEffects.FlipHorizontally, Math.Max(0f, tool_draw_layer_offset + (float)(f.getStandingY() + 64) / 10000f));
						}
						break;
					case 1:
						spriteBatch.Draw(toolSpriteSheet, Utility.snapToInt(new Vector2(fPosition.X - 64f + (float)toolXOffset, fPosition.Y - 160f + 8f + (float)toolYOffset)), sourceRectangleForTool, color3, 0f, Vector2.Zero, 4f, SpriteEffects.FlipHorizontally, Math.Max(0f, tool_draw_layer_offset + (float)(f.getStandingY() + 64) / 10000f));
						break;
					case 2:
						spriteBatch.Draw(toolSpriteSheet, Utility.snapToInt(new Vector2(fPosition.X - 96f + 32f + (float)toolXOffset, fPosition.Y - 128f - 24f + (float)toolYOffset)), sourceRectangleForTool, color3, 0f, Vector2.Zero, 4f, SpriteEffects.FlipHorizontally, Math.Max(0f, tool_draw_layer_offset + (float)(f.getStandingY() + 64) / 10000f));
						break;
					case 3:
						spriteBatch.Draw(toolSpriteSheet, Utility.snapToInt(new Vector2(fPosition.X - 96f + 24f + (float)toolXOffset, fPosition.Y - 128f - 32f + (float)toolYOffset)), sourceRectangleForTool, color3, 0f, Vector2.Zero, 4f, SpriteEffects.FlipHorizontally, Math.Max(0f, tool_draw_layer_offset + (float)(f.getStandingY() + 64) / 10000f));
						break;
					case 4:
						if ((f.CurrentTool as FishingRod).isFishing || (f.CurrentTool as FishingRod).doneWithAnimation)
						{
							spriteBatch.Draw(toolSpriteSheet, Utility.snapToInt(new Vector2(fPosition.X - 64f + (float)toolXOffset, fPosition.Y - 160f + (float)toolYOffset)), sourceRectangleForTool, color3, 0f, Vector2.Zero, 4f, SpriteEffects.FlipHorizontally, Math.Max(0f, tool_draw_layer_offset + (float)(f.getStandingY() + 64) / 10000f));
						}
						else
						{
							spriteBatch.Draw(toolSpriteSheet, Utility.snapToInt(new Vector2(fPosition.X - 64f + (float)toolXOffset, fPosition.Y - 160f + 4f + (float)toolYOffset)), sourceRectangleForTool, color3, 0f, Vector2.Zero, 4f, SpriteEffects.FlipHorizontally, Math.Max(0f, tool_draw_layer_offset + (float)(f.getStandingY() + 64) / 10000f));
						}
						break;
					case 5:
						spriteBatch.Draw(toolSpriteSheet, Utility.snapToInt(new Vector2(fPosition.X - 64f + (float)toolXOffset, fPosition.Y - 160f + (float)toolYOffset)), sourceRectangleForTool, color3, 0f, Vector2.Zero, 4f, SpriteEffects.FlipHorizontally, Math.Max(0f, tool_draw_layer_offset + (float)(f.getStandingY() + 64) / 10000f));
						break;
					}
				}
				else if (f.CurrentTool != null && f.CurrentTool is WateringCan)
				{
					switch (((FarmerSprite)f.Sprite).currentAnimationIndex)
					{
					case 0:
					case 1:
						spriteBatch.Draw(toolSpriteSheet, Utility.snapToInt(new Vector2(fPosition.X + (float)toolXOffset - 4f, fPosition.Y - 128f + 8f + (float)toolYOffset)), sourceRectangleForTool, Color.White, 0f, new Vector2(0f, 16f), 4f, SpriteEffects.FlipHorizontally, Math.Max(0f, tool_draw_layer_offset + (float)(f.GetBoundingBox().Bottom + layerDepthOffset2) / 10000f));
						break;
					case 2:
						spriteBatch.Draw(toolSpriteSheet, Utility.snapToInt(new Vector2(fPosition.X + (float)toolXOffset - 16f, fPosition.Y - 128f + (float)toolYOffset)), sourceRectangleForTool, Color.White, -(float)Math.PI / 12f, new Vector2(0f, 16f), 4f, SpriteEffects.FlipHorizontally, Math.Max(0f, tool_draw_layer_offset + (float)(f.GetBoundingBox().Bottom + layerDepthOffset2) / 10000f));
						break;
					case 3:
						sourceRectangleForTool.X += 16;
						spriteBatch.Draw(toolSpriteSheet, Utility.snapToInt(new Vector2(fPosition.X + (float)toolXOffset - 16f, fPosition.Y - 128f - 24f + (float)toolYOffset)), sourceRectangleForTool, Color.White, 0f, new Vector2(0f, 16f), 4f, SpriteEffects.FlipHorizontally, Math.Max(0f, tool_draw_layer_offset + (float)(f.GetBoundingBox().Bottom + layerDepthOffset2) / 10000f));
						break;
					}
				}
				else
				{
					switch (((FarmerSprite)f.Sprite).currentAnimationIndex)
					{
					case 0:
						spriteBatch.Draw(toolSpriteSheet, Utility.snapToInt(new Vector2(fPosition.X + 32f + 8f + (float)toolXOffset + (float)Math.Min(8, f.toolPower * 4), fPosition.Y - 128f + 8f + (float)toolYOffset + (float)Math.Min(8, f.toolPower * 4))), sourceRectangleForTool, Color.White, (float)Math.PI / 12f + (float)Math.Min(f.toolPower, 2) * ((float)Math.PI / 64f), new Vector2(0f, 16f), 4f, SpriteEffects.FlipHorizontally, Math.Max(0f, tool_draw_layer_offset + (float)(f.GetBoundingBox().Bottom + layerDepthOffset2) / 10000f));
						break;
					case 1:
						spriteBatch.Draw(toolSpriteSheet, Utility.snapToInt(new Vector2(fPosition.X - 16f + (float)toolXOffset, fPosition.Y - 128f + 16f + (float)toolYOffset)), sourceRectangleForTool, Color.White, -(float)Math.PI / 12f, new Vector2(0f, 16f), 4f, SpriteEffects.FlipHorizontally, Math.Max(0f, tool_draw_layer_offset + (float)(f.GetBoundingBox().Bottom + layerDepthOffset2) / 10000f));
						break;
					case 2:
						spriteBatch.Draw(toolSpriteSheet, Utility.snapToInt(new Vector2(fPosition.X - 64f + 4f + (float)toolXOffset, fPosition.Y - 128f + 60f + (float)toolYOffset)), sourceRectangleForTool, Color.White, -(float)Math.PI / 4f, new Vector2(0f, 16f), 4f, SpriteEffects.FlipHorizontally, Math.Max(0f, tool_draw_layer_offset + (float)(f.GetBoundingBox().Bottom + layerDepthOffset2) / 10000f));
						break;
					case 3:
						spriteBatch.Draw(toolSpriteSheet, Utility.snapToInt(new Vector2(fPosition.X - 64f + 20f + (float)toolXOffset, fPosition.Y - 64f + 76f + (float)toolYOffset)), sourceRectangleForTool, Color.White, (float)Math.PI * -7f / 12f, new Vector2(0f, 16f), 4f, SpriteEffects.FlipHorizontally, Math.Max(0f, tool_draw_layer_offset + (float)(f.GetBoundingBox().Bottom + layerDepthOffset2) / 10000f));
						break;
					case 4:
						spriteBatch.Draw(toolSpriteSheet, Utility.snapToInt(new Vector2(fPosition.X - 64f + 24f + (float)toolXOffset, fPosition.Y + 24f + (float)toolYOffset)), sourceRectangleForTool, Color.White, (float)Math.PI * -7f / 12f, new Vector2(0f, 16f), 4f, SpriteEffects.FlipHorizontally, Math.Max(0f, tool_draw_layer_offset + (float)(f.GetBoundingBox().Bottom + layerDepthOffset2) / 10000f));
						break;
					}
				}
			}
			else
			{
				if (f.CurrentTool is MeleeWeapon && f.FacingDirection == 0)
				{
					return;
				}
				if (((FarmerSprite)f.Sprite).currentAnimationIndex > 2 && (!(f.CurrentTool is FishingRod) || (f.CurrentTool as FishingRod).isCasting || (f.CurrentTool as FishingRod).castedButBobberStillInAir || (f.CurrentTool as FishingRod).isTimingCast))
				{
					Point tileLocation2 = f.getTileLocationPoint();
					if (f.currentLocation.getTileIndexAt(tileLocation2, "Front") != -1 && f.Position.Y % 64f < 32f && f.Position.Y % 64f > 16f)
					{
						return;
					}
				}
				else if (f.CurrentTool is FishingRod && ((FarmerSprite)f.Sprite).currentAnimationIndex <= 2)
				{
					Point tileLocation = f.getTileLocationPoint();
					tileLocation.Y--;
					if (f.currentLocation.getTileIndexAt(tileLocation, "Front") != -1)
					{
						return;
					}
				}
				if ((f.CurrentTool != null && f.CurrentTool is FishingRod) || (currentToolIndex >= 48 && currentToolIndex <= 55 && !(f.CurrentTool as FishingRod).fishCaught))
				{
					Color color = (f.CurrentTool as FishingRod).getColor();
					switch (((FarmerSprite)f.Sprite).currentAnimationIndex)
					{
					case 0:
						if (!(f.CurrentTool as FishingRod).showingTreasure && !(f.CurrentTool as FishingRod).fishCaught && (f.FacingDirection != 0 || !(f.CurrentTool as FishingRod).isFishing || (f.CurrentTool as FishingRod).isReeling))
						{
							spriteBatch.Draw(toolSpriteSheet, Utility.snapToInt(new Vector2(fPosition.X - 64f, fPosition.Y - 128f + 4f)), sourceRectangleForTool, color, 0f, Vector2.Zero, 4f, SpriteEffects.None, Math.Max(0f, tool_draw_layer_offset + (float)(f.getStandingY() + ((f.FacingDirection != 0) ? 128 : 0)) / 10000f));
						}
						break;
					case 1:
						spriteBatch.Draw(toolSpriteSheet, Utility.snapToInt(new Vector2(fPosition.X - 64f, fPosition.Y - 128f + 4f)), sourceRectangleForTool, color, 0f, Vector2.Zero, 4f, SpriteEffects.None, Math.Max(0f, tool_draw_layer_offset + (float)(f.getStandingY() + ((f.FacingDirection != 0) ? 128 : 0)) / 10000f));
						break;
					case 2:
						spriteBatch.Draw(toolSpriteSheet, Utility.snapToInt(new Vector2(fPosition.X - 64f, fPosition.Y - 128f + 4f)), sourceRectangleForTool, color, 0f, Vector2.Zero, 4f, SpriteEffects.None, Math.Max(0f, tool_draw_layer_offset + (float)(f.getStandingY() + ((f.FacingDirection != 0) ? 128 : 0)) / 10000f));
						break;
					case 3:
						if (f.FacingDirection == 2)
						{
							spriteBatch.Draw(toolSpriteSheet, Utility.snapToInt(new Vector2(fPosition.X - 64f, fPosition.Y - 128f + 4f)), sourceRectangleForTool, color, 0f, Vector2.Zero, 4f, SpriteEffects.None, Math.Max(0f, tool_draw_layer_offset + (float)(f.getStandingY() + ((f.FacingDirection != 0) ? 128 : 0)) / 10000f));
						}
						break;
					case 4:
						if (f.FacingDirection == 0 && (f.CurrentTool as FishingRod).isFishing)
						{
							spriteBatch.Draw(toolSpriteSheet, Utility.snapToInt(new Vector2(fPosition.X - 80f, fPosition.Y - 96f)), sourceRectangleForTool, color, 0f, Vector2.Zero, 4f, SpriteEffects.FlipVertically, Math.Max(0f, tool_draw_layer_offset + (float)(f.getStandingY() + 128) / 10000f));
						}
						else if (f.FacingDirection == 2)
						{
							spriteBatch.Draw(toolSpriteSheet, Utility.snapToInt(new Vector2(fPosition.X - 64f, fPosition.Y - 128f + 4f)), sourceRectangleForTool, color, 0f, Vector2.Zero, 4f, SpriteEffects.None, Math.Max(0f, tool_draw_layer_offset + (float)(f.getStandingY() + ((f.FacingDirection != 0) ? 128 : 0)) / 10000f));
						}
						break;
					case 5:
						if (f.FacingDirection == 2 && !(f.CurrentTool as FishingRod).showingTreasure && !(f.CurrentTool as FishingRod).fishCaught)
						{
							spriteBatch.Draw(toolSpriteSheet, Utility.snapToInt(new Vector2(fPosition.X - 64f, fPosition.Y - 128f + 4f)), sourceRectangleForTool, color, 0f, Vector2.Zero, 4f, SpriteEffects.None, Math.Max(0f, tool_draw_layer_offset + (float)(f.getStandingY() + ((f.FacingDirection != 0) ? 128 : 0)) / 10000f));
						}
						break;
					}
					return;
				}
				if (f.CurrentTool != null && f.CurrentTool is WateringCan)
				{
					switch (((FarmerSprite)f.Sprite).currentAnimationIndex)
					{
					case 0:
					case 1:
						spriteBatch.Draw(toolSpriteSheet, Utility.snapToInt(new Vector2(fPosition.X + (float)toolXOffset, fPosition.Y - 128f + 16f + (float)toolYOffset)), sourceRectangleForTool, Color.White, 0f, new Vector2(0f, 16f), 4f, SpriteEffects.None, Math.Max(0f, tool_draw_layer_offset + (float)f.GetBoundingBox().Bottom / 10000f));
						break;
					case 2:
						spriteBatch.Draw(toolSpriteSheet, Utility.snapToInt(new Vector2(fPosition.X + (float)toolXOffset, fPosition.Y - 128f - (float)((f.FacingDirection == 2) ? (-4) : 32) + (float)toolYOffset)), sourceRectangleForTool, Color.White, 0f, new Vector2(0f, 16f), 4f, SpriteEffects.None, Math.Max(0f, tool_draw_layer_offset + (float)f.GetBoundingBox().Bottom / 10000f));
						break;
					case 3:
						if (f.FacingDirection == 2)
						{
							sourceRectangleForTool.X += 16;
						}
						spriteBatch.Draw(toolSpriteSheet, Utility.snapToInt(new Vector2(fPosition.X + (float)toolXOffset - (float)((f.FacingDirection == 2) ? 4 : 0), fPosition.Y - 128f - (float)((f.FacingDirection == 2) ? (-24) : 64) + (float)toolYOffset)), sourceRectangleForTool, Color.White, 0f, new Vector2(0f, 16f), 4f, SpriteEffects.None, Math.Max(0f, tool_draw_layer_offset + (float)f.GetBoundingBox().Bottom / 10000f));
						break;
					}
					return;
				}
				switch (((FarmerSprite)f.Sprite).currentAnimationIndex)
				{
				case 0:
					if (f.FacingDirection == 0)
					{
						spriteBatch.Draw(toolSpriteSheet, Utility.snapToInt(new Vector2(fPosition.X + (float)toolXOffset, fPosition.Y - 128f - 8f + (float)toolYOffset + (float)Math.Min(8, f.toolPower * 4))), sourceRectangleForTool, Color.White, 0f, new Vector2(0f, 16f), 4f, SpriteEffects.None, Math.Max(0f, tool_draw_layer_offset + (float)(f.getStandingY() - 8) / 10000f));
					}
					else
					{
						spriteBatch.Draw(toolSpriteSheet, Utility.snapToInt(new Vector2(fPosition.X + (float)toolXOffset - 20f, fPosition.Y - 128f + 12f + (float)toolYOffset + (float)Math.Min(8, f.toolPower * 4))), sourceRectangleForTool, Color.White, 0f, new Vector2(0f, 16f), 4f, SpriteEffects.None, Math.Max(0f, tool_draw_layer_offset + (float)(f.getStandingY() + 8) / 10000f));
					}
					break;
				case 1:
					if (f.FacingDirection == 0)
					{
						spriteBatch.Draw(toolSpriteSheet, Utility.snapToInt(new Vector2(fPosition.X + (float)toolXOffset + 4f, fPosition.Y - 128f + 40f + (float)toolYOffset)), sourceRectangleForTool, Color.White, 0f, new Vector2(0f, 16f), 4f, SpriteEffects.None, Math.Max(0f, tool_draw_layer_offset + (float)(f.getStandingY() - 8) / 10000f));
					}
					else
					{
						spriteBatch.Draw(toolSpriteSheet, Utility.snapToInt(new Vector2(fPosition.X + (float)toolXOffset - 12f, fPosition.Y - 128f + 32f + (float)toolYOffset)), sourceRectangleForTool, Color.White, -(float)Math.PI / 24f, new Vector2(0f, 16f), 4f, SpriteEffects.None, Math.Max(0f, tool_draw_layer_offset + (float)(f.getStandingY() + 8) / 10000f));
					}
					break;
				case 2:
					spriteBatch.Draw(toolSpriteSheet, Utility.snapToInt(new Vector2(fPosition.X + (float)toolXOffset, fPosition.Y - 128f + 64f + (float)toolYOffset)), sourceRectangleForTool, Color.White, 0f, new Vector2(0f, 16f), 4f, SpriteEffects.None, Math.Max(0f, tool_draw_layer_offset + (float)((f.getStandingY() + f.FacingDirection == 0) ? (-8) : 8) / 10000f));
					break;
				case 3:
					if (f.FacingDirection != 0)
					{
						spriteBatch.Draw(toolSpriteSheet, Utility.snapToInt(new Vector2(fPosition.X + (float)toolXOffset, fPosition.Y - 64f + 44f + (float)toolYOffset)), sourceRectangleForTool, Color.White, 0f, new Vector2(0f, 16f), 4f, SpriteEffects.None, Math.Max(0f, tool_draw_layer_offset + (float)(f.getStandingY() + 8) / 10000f));
					}
					break;
				case 4:
					if (f.FacingDirection != 0)
					{
						spriteBatch.Draw(toolSpriteSheet, Utility.snapToInt(new Vector2(fPosition.X + (float)toolXOffset, fPosition.Y - 64f + 48f + (float)toolYOffset)), sourceRectangleForTool, Color.White, 0f, new Vector2(0f, 16f), 4f, SpriteEffects.None, Math.Max(0f, tool_draw_layer_offset + (float)(f.getStandingY() + 8) / 10000f));
					}
					break;
				case 5:
					spriteBatch.Draw(toolSpriteSheet, Utility.snapToInt(new Vector2(fPosition.X + (float)toolXOffset, fPosition.Y - 64f + 32f + (float)toolYOffset)), sourceRectangleForTool, Color.White, 0f, new Vector2(0f, 16f), 4f, SpriteEffects.None, Math.Max(0f, tool_draw_layer_offset + (float)(f.getStandingY() + 8) / 10000f));
					break;
				}
			}
		}

		/// ####################
		/// OTHER HELPER METHODS
		/// ####################
		public static Vector2 GlobalToLocal(xTile.Dimensions.Rectangle viewport, Vector2 globalPosition)
		{
			return new Vector2(globalPosition.X - (float)viewport.X, globalPosition.Y - (float)viewport.Y);
		}

		public static bool IsEnglish()
		{
			return content.GetCurrentLanguage() == LocalizedContentManager.LanguageCode.en;
		}

		public static Vector2 GlobalToLocal(Vector2 globalPosition)
		{
			return new Vector2(globalPosition.X - (float)viewport.X, globalPosition.Y - (float)viewport.Y);
		}

		public static Microsoft.Xna.Framework.Rectangle GlobalToLocal(xTile.Dimensions.Rectangle viewport, Microsoft.Xna.Framework.Rectangle globalPosition)
		{
			return new Microsoft.Xna.Framework.Rectangle(globalPosition.X - viewport.X, globalPosition.Y - viewport.Y, globalPosition.Width, globalPosition.Height);
		}

		public static string parseText(string text, SpriteFont whichFont, int width)
		{
			if (text == null)
			{
				return "";
			}
			string line = string.Empty;
			string returnString = string.Empty;
			if (LocalizedContentManager.CurrentLanguageCode == LocalizedContentManager.LanguageCode.ja || LocalizedContentManager.CurrentLanguageCode == LocalizedContentManager.LanguageCode.zh || LocalizedContentManager.CurrentLanguageCode == LocalizedContentManager.LanguageCode.th)
			{
				string text2 = text;
				for (int i = 0; i < text2.Length; i++)
				{
					char c = text2[i];
					if (whichFont.MeasureString(line + c).Length() > (float)width || c.Equals(Environment.NewLine))
					{
						returnString = returnString + line + Environment.NewLine;
						line = string.Empty;
					}
					if (!c.Equals(Environment.NewLine))
					{
						line += c;
					}
				}
				return returnString + line;
			}
			if (LocalizedContentManager.CurrentLanguageCode == LocalizedContentManager.LanguageCode.fr && text.Contains("^"))
			{
				string[] genderText = text.Split('^');
				text = ((!player.IsMale) ? genderText[1] : genderText[0]);
			}
			string[] array = text.Split(' ');
			foreach (string word in array)
			{
				try
				{
					if (LocalizedContentManager.CurrentLanguageCode == LocalizedContentManager.LanguageCode.fr && word.StartsWith("\n-"))
					{
						returnString = returnString + line + Environment.NewLine;
						line = string.Empty;
					}
					if (whichFont.MeasureString(line + word).X > (float)width || word.Equals(Environment.NewLine))
					{
						returnString = returnString + line + Environment.NewLine;
						line = string.Empty;
					}
					if (!word.Equals(Environment.NewLine))
					{
						line = line + word + " ";
					}
				}
				catch (Exception ex)
				{
					Console.WriteLine("Exception measuring string: " + ex);
				}
			}
			return returnString + line;
		}

		public static void UpdateHorseOwnership()
		{
			bool verbose = false;
			Dictionary<long, Horse> horse_lookup = new Dictionary<long, Horse>();
			HashSet<Horse> claimed_horses = new HashSet<Horse>();
			List<Stable> stables = new List<Stable>();
			foreach (Building b in getFarm().buildings)
			{
				if (b is Stable && (int)b.daysOfConstructionLeft <= 0)
				{
					Stable stable5 = b as Stable;
					stables.Add(stable5);
				}
			}
			foreach (Stable stable4 in stables)
			{
				if (stable4.owner.Value == -6666666 && getFarmerMaybeOffline(-6666666L) == null)
				{
					stable4.owner.Value = player.UniqueMultiplayerID;
				}
				stable4.grabHorse();
			}
			foreach (Stable item in stables)
			{
				Horse horse4 = item.getStableHorse();
				if (horse4 != null && !claimed_horses.Contains(horse4) && horse4.getOwner() != null && !horse_lookup.ContainsKey(horse4.getOwner().UniqueMultiplayerID) && horse4.getOwner().horseName.Value != null && horse4.getOwner().horseName.Value.Length > 0 && horse4.Name == horse4.getOwner().horseName.Value)
				{
					horse_lookup[horse4.getOwner().UniqueMultiplayerID] = horse4;
					claimed_horses.Add(horse4);
					if (verbose)
					{
						Console.WriteLine("Assigned horse " + horse4.Name + " to " + horse4.getOwner().Name + " (Exact match)");
					}
				}
			}
			Dictionary<string, Farmer> horse_name_lookup = new Dictionary<string, Farmer>();
			foreach (Farmer farmer in getAllFarmers())
			{
				if (farmer == null || farmer.horseName.Value == null || farmer.horseName.Value.Length == 0)
				{
					continue;
				}
				bool fail = false;
				foreach (Horse item2 in claimed_horses)
				{
					if (item2.getOwner() == farmer)
					{
						fail = true;
						break;
					}
				}
				if (!fail)
				{
					horse_name_lookup[farmer.horseName] = farmer;
				}
			}
			foreach (Stable stable3 in stables)
			{
				Horse horse3 = stable3.getStableHorse();
				if (horse3 != null && !claimed_horses.Contains(horse3) && horse3.getOwner() != null && horse3.Name != null && horse3.Name.Length > 0 && horse_name_lookup.ContainsKey(horse3.Name) && !horse_lookup.ContainsKey(horse_name_lookup[horse3.Name].UniqueMultiplayerID))
				{
					stable3.owner.Value = horse_name_lookup[horse3.Name].UniqueMultiplayerID;
					stable3.updateHorseOwnership();
					horse_lookup[horse3.getOwner().UniqueMultiplayerID] = horse3;
					claimed_horses.Add(horse3);
					if (verbose)
					{
						Console.WriteLine("Assigned horse " + horse3.Name + " to " + horse3.getOwner().Name + " (Name match from different owner.)");
					}
				}
			}
			foreach (Stable stable2 in stables)
			{
				Horse horse2 = stable2.getStableHorse();
				if (horse2 != null && !claimed_horses.Contains(horse2) && horse2.getOwner() != null && !horse_lookup.ContainsKey(horse2.getOwner().UniqueMultiplayerID))
				{
					horse_lookup[horse2.getOwner().UniqueMultiplayerID] = horse2;
					claimed_horses.Add(horse2);
					stable2.updateHorseOwnership();
					if (verbose)
					{
						Console.WriteLine("Assigned horse " + horse2.Name + " to " + horse2.getOwner().Name + " (Owner's only stable)");
					}
				}
			}
			foreach (Stable stable in stables)
			{
				Horse horse = stable.getStableHorse();
				if (horse == null || claimed_horses.Contains(horse))
				{
					continue;
				}
				foreach (Horse claimed_horse in claimed_horses)
				{
					if (horse.ownerId == claimed_horse.ownerId)
					{
						stable.owner.Value = 0L;
						stable.updateHorseOwnership();
						if (verbose)
						{
							Console.WriteLine("Unassigned horse (stable owner already has a horse).");
						}
						break;
					}
				}
			}
		}

		public static string LoadStringByGender(int npcGender, string key)
		{
			if (npcGender == 0)
			{
				return content.LoadString(key).Split('/').First();
			}
			return content.LoadString(key).Split('/').Last();
		}

		public static string LoadStringByGender(int npcGender, string key, params object[] substitutions)
		{
			string sentence = "";
			if (npcGender == 0)
			{
				sentence = content.LoadString(key).Split('/').First();
				if (substitutions.Length != 0)
				{
					try
					{
						return string.Format(sentence, substitutions);
					}
					catch (Exception)
					{
						return sentence;
					}
				}
			}
			sentence = content.LoadString(key).Split('/').Last();
			if (substitutions.Length != 0)
			{
				try
				{
					return string.Format(sentence, substitutions);
				}
				catch (Exception)
				{
					return sentence;
				}
			}
			return sentence;
		}

		public static string parseText(string text)
		{
			return parseText(text, dialogueFont, dialogueWidth);
		}

		public static bool isThisPositionVisibleToPlayer(string locationName, Vector2 position)
		{
			if (locationName.Equals(currentLocation.Name) && new Microsoft.Xna.Framework.Rectangle((int)(player.Position.X - (float)(viewport.Width / 2)), (int)(player.Position.Y - (float)(viewport.Height / 2)), viewport.Width, viewport.Height).Contains(new Point((int)position.X, (int)position.Y)))
			{
				return true;
			}
			return false;
		}

		public static Microsoft.Xna.Framework.Rectangle getSourceRectForStandardTileSheet(Texture2D tileSheet, int tilePosition, int width = -1, int height = -1)
		{
			if (width == -1)
			{
				width = 64;
			}
			if (height == -1)
			{
				height = 64;
			}
			return new Microsoft.Xna.Framework.Rectangle(tilePosition * width % tileSheet.Width, tilePosition * width / tileSheet.Width * height, width, height);
		}

		public static Microsoft.Xna.Framework.Rectangle getSquareSourceRectForNonStandardTileSheet(Texture2D tileSheet, int tileWidth, int tileHeight, int tilePosition)
		{
			return new Microsoft.Xna.Framework.Rectangle(tilePosition * tileWidth % tileSheet.Width, tilePosition * tileWidth / tileSheet.Width * tileHeight, tileWidth, tileHeight);
		}

		public static Microsoft.Xna.Framework.Rectangle getArbitrarySourceRect(Texture2D tileSheet, int tileWidth, int tileHeight, int tilePosition)
		{
			if (tileSheet != null)
			{
				return new Microsoft.Xna.Framework.Rectangle(tilePosition * tileWidth % tileSheet.Width, tilePosition * tileWidth / tileSheet.Width * tileHeight, tileWidth, tileHeight);
			}
			return Microsoft.Xna.Framework.Rectangle.Empty;
		}

		public static string getTimeOfDayString(int time)
		{
			string zeroPad = ((time % 100 == 0) ? "0" : string.Empty);
			string hours;
			switch (LocalizedContentManager.CurrentLanguageCode)
			{
			default:
				hours = ((time / 100 % 12 == 0) ? "12" : (time / 100 % 12).ToString());
				break;
			case LocalizedContentManager.LanguageCode.ja:
				hours = ((time / 100 % 12 == 0) ? "0" : (time / 100 % 12).ToString());
				break;
			case LocalizedContentManager.LanguageCode.zh:
				hours = ((time / 100 % 24 == 0) ? "00" : ((time / 100 % 12 == 0) ? "12" : (time / 100 % 12).ToString()));
				break;
			case LocalizedContentManager.LanguageCode.ru:
			case LocalizedContentManager.LanguageCode.pt:
			case LocalizedContentManager.LanguageCode.es:
			case LocalizedContentManager.LanguageCode.de:
			case LocalizedContentManager.LanguageCode.th:
			case LocalizedContentManager.LanguageCode.fr:
			case LocalizedContentManager.LanguageCode.tr:
			case LocalizedContentManager.LanguageCode.hu:
				hours = (time / 100 % 24).ToString();
				hours = ((time / 100 % 24 <= 9) ? ("0" + hours) : hours);
				break;
			}
			string timeText = hours + ":" + time % 100 + zeroPad;
			if (LocalizedContentManager.CurrentLanguageCode == LocalizedContentManager.LanguageCode.en)
			{
				timeText = timeText + " " + ((time < 1200 || time >= 2400) ? content.LoadString("Strings\\StringsFromCSFiles:DayTimeMoneyBox.cs.10370") : content.LoadString("Strings\\StringsFromCSFiles:DayTimeMoneyBox.cs.10371"));
			}
			else if (LocalizedContentManager.CurrentLanguageCode == LocalizedContentManager.LanguageCode.ja)
			{
				timeText = ((time < 1200 || time >= 2400) ? (content.LoadString("Strings\\StringsFromCSFiles:DayTimeMoneyBox.cs.10370") + " " + timeText) : (content.LoadString("Strings\\StringsFromCSFiles:DayTimeMoneyBox.cs.10371") + " " + timeText));
			}
			else if (LocalizedContentManager.CurrentLanguageCode == LocalizedContentManager.LanguageCode.zh)
			{
				timeText = ((time < 600 || time >= 2400) ? ("凌晨 " + timeText) : ((time < 1200) ? (content.LoadString("Strings\\StringsFromCSFiles:DayTimeMoneyBox.cs.10370") + " " + timeText) : ((time < 1300) ? ("中午  " + timeText) : ((time < 1900) ? (content.LoadString("Strings\\StringsFromCSFiles:DayTimeMoneyBox.cs.10371") + " " + timeText) : ("晚上  " + timeText)))));
			}
			else if (LocalizedContentManager.CurrentLanguageCode == LocalizedContentManager.LanguageCode.fr)
			{
				timeText = ((time % 100 != 0) ? ((time / 100 == 24) ? "00h" : (time / 100 + "h" + time % 100)) : ((time / 100 == 24) ? "00h" : (time / 100 + "h")));
			}
			else if (LocalizedContentManager.CurrentLanguageCode == LocalizedContentManager.LanguageCode.mod)
			{
				return LocalizedContentManager.FormatTimeString(time, LocalizedContentManager.CurrentModLanguage.TimeFormat).ToString();
			}
			return timeText;
		}

		public bool checkBigCraftableBoundariesForFrontLayer()
		{
			if (currentLocation.Map.GetLayer("Front").PickTile(new Location(player.getStandingX() - 32, (int)player.Position.Y - 38), viewport.Size) == null && currentLocation.Map.GetLayer("Front").PickTile(new Location(player.getStandingX() + 32, (int)player.Position.Y - 38), viewport.Size) == null && currentLocation.Map.GetLayer("Front").PickTile(new Location(player.getStandingX() - 32, (int)player.Position.Y - 38 - 64), viewport.Size) == null)
			{
				return currentLocation.Map.GetLayer("Front").PickTile(new Location(player.getStandingX() + 32, (int)player.Position.Y - 38 - 64), viewport.Size) != null;
			}
			return true;
		}

		public static bool[,] getCircleOutlineGrid(int radius)
		{
			bool[,] circleGrid = new bool[radius * 2 + 1, radius * 2 + 1];
			int f = 1 - radius;
			int ddF_x = 1;
			int ddF_y = -2 * radius;
			int x = 0;
			int y = radius;
			circleGrid[radius, radius + radius] = true;
			circleGrid[radius, radius - radius] = true;
			circleGrid[radius + radius, radius] = true;
			circleGrid[radius - radius, radius] = true;
			while (x < y)
			{
				if (f >= 0)
				{
					y--;
					ddF_y += 2;
					f += ddF_y;
				}
				x++;
				ddF_x += 2;
				f += ddF_x;
				circleGrid[radius + x, radius + y] = true;
				circleGrid[radius - x, radius + y] = true;
				circleGrid[radius + x, radius - y] = true;
				circleGrid[radius - x, radius - y] = true;
				circleGrid[radius + y, radius + x] = true;
				circleGrid[radius - y, radius + x] = true;
				circleGrid[radius + y, radius - x] = true;
				circleGrid[radius - y, radius - x] = true;
			}
			return circleGrid;
		}

		public static Color getColorForTreasureType(string type)
		{
			return type switch
			{
				"Copper" => Color.Sienna, 
				"Iron" => Color.LightSlateGray, 
				"Coal" => Color.Black, 
				"Gold" => Color.Gold, 
				"Iridium" => Color.Purple, 
				"Coins" => Color.Yellow, 
				"Arch" => Color.White, 
				_ => Color.SaddleBrown, 
			};
		}

		public static string GetFarmTypeID()
		{
			if (whichFarm == 7 && whichModFarm != null)
			{
				return whichModFarm.ID;
			}
			return whichFarm.ToString();
		}

		public static string GetFarmTypeModData(string key)
		{
			if (whichFarm == 7 && whichModFarm != null && whichModFarm.ModData != null && whichModFarm.ModData.ContainsKey(key))
			{
				return whichModFarm.ModData[key];
			}
			return null;
		}

		public void _PerformRemoveNormalItemFromWorldOvernight(int parent_sheet_index)
		{
			foreach (GameLocation l in locations)
			{
				_RecursiveRemoveThisNormalItemLocation(l, parent_sheet_index);
			}
			foreach (MineShaft mine in MineShaft.activeMines)
			{
				_RecursiveRemoveThisNormalItemLocation(mine, parent_sheet_index);
			}
			foreach (VolcanoDungeon level in VolcanoDungeon.activeLevels)
			{
				_RecursiveRemoveThisNormalItemLocation(level, parent_sheet_index);
			}
			for (int k = 0; k < player.team.returnedDonations.Count; k++)
			{
				if (_RecursiveRemoveThisNormalItemItem(player.team.returnedDonations[k], parent_sheet_index))
				{
					player.team.returnedDonations.RemoveAt(k);
					k--;
				}
			}
			for (int j = 0; j < player.team.junimoChest.Count; j++)
			{
				if (_RecursiveRemoveThisNormalItemItem(player.team.junimoChest[j], parent_sheet_index))
				{
					player.team.junimoChest.RemoveAt(j);
					j--;
				}
			}
			foreach (SpecialOrder order in player.team.specialOrders)
			{
				for (int i = 0; i < order.donatedItems.Count; i++)
				{
					Item item = order.donatedItems[i];
					if (_RecursiveRemoveThisNormalItemItem(item, parent_sheet_index))
					{
						order.donatedItems[i] = null;
					}
				}
			}
		}

		protected virtual void _PerformRemoveNormalItemFromFarmerOvernight(Farmer farmer, int parent_sheet_index)
		{
			for (int j = 0; j < farmer.items.Count; j++)
			{
				if (_RecursiveRemoveThisNormalItemItem(farmer.items[j], parent_sheet_index))
				{
					farmer.items[j] = null;
				}
			}
			for (int i = 0; i < farmer.itemsLostLastDeath.Count; i++)
			{
				if (_RecursiveRemoveThisNormalItemItem(farmer.itemsLostLastDeath[i], parent_sheet_index))
				{
					farmer.itemsLostLastDeath.RemoveAt(i);
					i--;
				}
			}
			if (farmer.recoveredItem != null && _RecursiveRemoveThisNormalItemItem(farmer.recoveredItem, parent_sheet_index))
			{
				farmer.recoveredItem = null;
				farmer.mailbox.Remove("MarlonRecovery");
				farmer.mailForTomorrow.Remove("MarlonRecovery");
			}
		}

		protected virtual bool _RecursiveRemoveThisNormalItemItem(Item this_item, int parent_sheet_index)
		{
			if (this_item == null)
			{
				return false;
			}
			if (this_item is Object)
			{
				Object o = this_item as Object;
				if (o.heldObject.Value != null && _RecursiveRemoveThisNormalItemItem(o.heldObject.Value, parent_sheet_index))
				{
					o.heldObject.Value = null;
					o.readyForHarvest.Value = false;
					o.showNextIndex.Value = false;
				}
				if (o is StorageFurniture)
				{
					bool removed_item2 = false;
					for (int j = 0; j < (o as StorageFurniture).heldItems.Count; j++)
					{
						Item item2 = (o as StorageFurniture).heldItems[j];
						if (item2 != null && _RecursiveRemoveThisNormalItemItem(item2, parent_sheet_index))
						{
							(o as StorageFurniture).heldItems[j] = null;
							removed_item2 = true;
						}
					}
					if (removed_item2)
					{
						(o as StorageFurniture).ClearNulls();
					}
				}
				if (o is IndoorPot)
				{
					IndoorPot pot = o as IndoorPot;
					if (pot.hoeDirt != null)
					{
						_RecursiveRemoveThisNormalItemDirt(pot.hoeDirt, null, Vector2.Zero, parent_sheet_index);
					}
				}
				if (o is Chest)
				{
					bool removed_item = false;
					for (int i = 0; i < (o as Chest).items.Count; i++)
					{
						Item item = (o as Chest).items[i];
						if (item != null && _RecursiveRemoveThisNormalItemItem(item, parent_sheet_index))
						{
							(o as Chest).items[i] = null;
							removed_item = true;
						}
					}
					if (removed_item)
					{
						(o as Chest).clearNulls();
					}
				}
				if (o.heldObject.Value != null && _RecursiveRemoveThisNormalItemItem((Object)o.heldObject, parent_sheet_index))
				{
					o.heldObject.Value = null;
				}
			}
			return Utility.IsNormalObjectAtParentSheetIndex(this_item, parent_sheet_index);
		}

		protected virtual void _RecursiveRemoveThisNormalItemDirt(HoeDirt dirt, GameLocation location, Vector2 coord, int parent_sheet_index)
		{
			if (dirt.crop != null && dirt.crop.indexOfHarvest.Value == parent_sheet_index)
			{
				dirt.destroyCrop(coord, showAnimation: false, location);
			}
		}

		protected virtual void _RecursiveRemoveThisNormalItemLocation(GameLocation l, int parent_sheet_index)
		{
			if (l == null)
			{
				return;
			}
			if (l != null)
			{
				List<Guid> removed_items = new List<Guid>();
				foreach (Furniture furniture in l.furniture)
				{
					if (_RecursiveRemoveThisNormalItemItem(furniture, parent_sheet_index))
					{
						removed_items.Add(l.furniture.GuidOf(furniture));
					}
				}
				foreach (Guid guid in removed_items)
				{
					l.furniture.Remove(guid);
				}
				foreach (NPC character in l.characters)
				{
					if (!(character is Monster))
					{
						continue;
					}
					Monster monster = character as Monster;
					if (monster.objectsToDrop == null || monster.objectsToDrop.Count <= 0)
					{
						continue;
					}
					for (int i2 = monster.objectsToDrop.Count - 1; i2 >= 0; i2--)
					{
						if (monster.objectsToDrop[i2] == parent_sheet_index)
						{
							monster.objectsToDrop.RemoveAt(i2);
						}
					}
				}
			}
			if (l is IslandFarmHouse)
			{
				for (int n = 0; n < (l as IslandFarmHouse).fridge.Value.items.Count; n++)
				{
					Item item4 = (l as IslandFarmHouse).fridge.Value.items[n];
					if (item4 != null && _RecursiveRemoveThisNormalItemItem(item4, parent_sheet_index))
					{
						(l as IslandFarmHouse).fridge.Value.items[n] = null;
					}
				}
			}
			foreach (Vector2 coord in l.terrainFeatures.Keys)
			{
				TerrainFeature feature = l.terrainFeatures[coord];
				if (feature is HoeDirt)
				{
					HoeDirt dirt = feature as HoeDirt;
					_RecursiveRemoveThisNormalItemDirt(dirt, l, coord, parent_sheet_index);
				}
			}
			if (l is FarmHouse)
			{
				for (int m = 0; m < (l as FarmHouse).fridge.Value.items.Count; m++)
				{
					Item item3 = (l as FarmHouse).fridge.Value.items[m];
					if (item3 != null && _RecursiveRemoveThisNormalItemItem(item3, parent_sheet_index))
					{
						(l as FarmHouse).fridge.Value.items[m] = null;
					}
				}
			}
			if (l is BuildableGameLocation)
			{
				foreach (Building b in (l as BuildableGameLocation).buildings)
				{
					if (b.indoors.Value != null)
					{
						_RecursiveRemoveThisNormalItemLocation(b.indoors.Value, parent_sheet_index);
					}
					if (b is Mill)
					{
						for (int k = 0; k < (b as Mill).output.Value.items.Count; k++)
						{
							Item item2 = (b as Mill).output.Value.items[k];
							if (item2 != null && _RecursiveRemoveThisNormalItemItem(item2, parent_sheet_index))
							{
								(b as Mill).output.Value.items[k] = null;
							}
						}
					}
					else
					{
						if (!(b is JunimoHut))
						{
							continue;
						}
						bool removed_item = false;
						Chest chest = (b as JunimoHut).output.Value;
						for (int j = 0; j < chest.items.Count; j++)
						{
							Item item = chest.items[j];
							if (item != null && _RecursiveRemoveThisNormalItemItem(item, parent_sheet_index))
							{
								chest.items[j] = null;
								removed_item = true;
							}
						}
						if (removed_item)
						{
							chest.clearNulls();
						}
					}
				}
			}
			foreach (Vector2 key in new List<Vector2>(l.objects.Keys))
			{
				if (_RecursiveRemoveThisNormalItemItem(l.objects[key], parent_sheet_index))
				{
					l.objects.Remove(key);
				}
			}
			for (int i = 0; i < l.debris.Count; i++)
			{
				Debris d = l.debris[i];
				if (d.item != null && _RecursiveRemoveThisNormalItemItem(d.item, parent_sheet_index))
				{
					l.debris.RemoveAt(i);
					i--;
				}
			}
		}
	}
}
