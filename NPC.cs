// Decompiled with JetBrains decompiler
// Type: StardewValley.NPC
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.BellsAndWhistles;
using StardewValley.Buildings;
using StardewValley.Characters;
using StardewValley.Locations;
using StardewValley.Menus;
using StardewValley.Objects;
using StardewValley.Projectiles;
using StardewValley.TerrainFeatures;
using StardewValley.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using xTile.Dimensions;
using xTile.ObjectModel;
using xTile.Tiles;

namespace StardewValley
{
  public class NPC : Character, IComparable
  {
    protected int idForClones = -1;
    private Stack<StardewValley.Dialogue> currentDialogue = new Stack<StardewValley.Dialogue>();
    public int id = -1;
    public int daysUntilBirthing = -1;
    public int daysAfterLastBirth = -1;
    [XmlIgnore]
    public bool breather = true;
    public bool followSchedule = true;
    protected int scheduleTimeToTry = 9999999;
    private string nameOfTodaysSchedule = "";
    private int married = -1;
    public const int minimum_square_pause = 6000;
    public const int maximum_square_pause = 12000;
    public const int portrait_width = 64;
    public const int portrait_height = 64;
    public const int portrait_neutral_index = 0;
    public const int portrait_happy_index = 1;
    public const int portrait_sad_index = 2;
    public const int portrait_custom_index = 3;
    public const int portrait_blush_index = 4;
    public const int portrait_angry_index = 5;
    public const int startingFriendship = 0;
    public const int defaultSpeed = 2;
    public const int maxGiftsPerWeek = 2;
    public const int friendshipPointsPerHeartLevel = 250;
    public const int maxFriendshipPoints = 2500;
    public const int gift_taste_love = 0;
    public const int gift_taste_like = 2;
    public const int gift_taste_neutral = 8;
    public const int gift_taste_dislike = 4;
    public const int gift_taste_hate = 6;
    public const int textStyle_shake = 0;
    public const int textStyle_fade = 1;
    public const int textStyle_none = 2;
    public const int adult = 0;
    public const int teen = 1;
    public const int child = 2;
    public const int neutral = 0;
    public const int polite = 1;
    public const int rude = 2;
    public const int outgoing = 0;
    public const int shy = 1;
    public const int positive = 0;
    public const int negative = 1;
    public const int male = 0;
    public const int female = 1;
    public const int undefined = 2;
    public const int other = 0;
    public const int desert = 1;
    public const int town = 2;
    private Dictionary<int, SchedulePathDescription> schedule;
    private Dictionary<string, string> dialogue;
    private SchedulePathDescription directionsToNewLocation;
    private int directionIndex;
    private int lengthOfWalkingSquareX;
    private int lengthOfWalkingSquareY;
    private int squarePauseAccumulation;
    private int squarePauseTotal;
    private int squarePauseOffset;
    protected Microsoft.Xna.Framework.Rectangle lastCrossroad;
    public string defaultMap;
    public string loveInterest;
    public string birthday_Season;
    private Texture2D portrait;
    private Vector2 defaultPosition;
    private Vector2 nextSquarePosition;
    protected int defaultFacingDirection;
    protected int shakeTimer;
    private bool isWalkingInSquare;
    private bool isWalkingTowardPlayer;
    private static List<List<string>> routesFromLocationToLocation;
    protected string textAboveHead;
    protected int textAboveHeadPreTimer;
    protected int textAboveHeadTimer;
    protected int textAboveHeadStyle;
    protected int textAboveHeadColor;
    protected float textAboveHeadAlpha;
    public int age;
    public int manners;
    public int socialAnxiety;
    public int optimism;
    public int gender;
    public int homeRegion;
    public int birthday_Day;
    private string extraDialogueMessageToAddThisMorning;
    [XmlIgnore]
    public PathFindController temporaryController;
    [XmlIgnore]
    public GameLocation currentLocation;
    [XmlIgnore]
    public bool updatedDialogueYet;
    [XmlIgnore]
    public bool uniqueSpriteActive;
    [XmlIgnore]
    public bool uniquePortraitActive;
    [XmlIgnore]
    public bool hideShadow;
    [XmlIgnore]
    public bool hasPartnerForDance;
    [XmlIgnore]
    public bool immediateSpeak;
    [XmlIgnore]
    public bool ignoreScheduleToday;
    public int moveTowardPlayerThreshold;
    [XmlIgnore]
    public float rotation;
    [XmlIgnore]
    public float yOffset;
    [XmlIgnore]
    public float swimTimer;
    [XmlIgnore]
    public float timerSinceLastMovement;
    [XmlIgnore]
    public string mapBeforeEvent;
    [XmlIgnore]
    public Vector2 positionBeforeEvent;
    [XmlIgnore]
    public Vector2 lastPosition;
    public bool isInvisible;
    public bool datable;
    public bool datingFarmer;
    public bool divorcedFromFarmer;
    private bool hasBeenKissedToday;
    private int timeAfterSquare;
    [XmlIgnore]
    public bool doingEndOfRouteAnimation;
    [XmlIgnore]
    public bool goingToDoEndOfRouteAnimation;
    private int[] routeEndIntro;
    private int[] routeEndAnimation;
    private int[] routeEndOutro;
    [XmlIgnore]
    public string endOfRouteMessage;
    [XmlIgnore]
    public string nextEndOfRouteMessage;
    private string endOfRouteBehaviorName;
    private Point previousEndPoint;
    protected int squareMovementFacingPreference;
    private const int NO_TRY = 9999999;
    private bool returningToEndPoint;
    private bool hasSaidAfternoonDialogue;
    public int daysMarried;

    [XmlIgnore]
    public SchedulePathDescription DirectionsToNewLocation
    {
      get
      {
        return this.directionsToNewLocation;
      }
      set
      {
        this.directionsToNewLocation = value;
      }
    }

    [XmlIgnore]
    public int DirectionIndex
    {
      get
      {
        return this.directionIndex;
      }
      set
      {
        this.directionIndex = value;
      }
    }

    public int DefaultFacingDirection
    {
      get
      {
        return this.defaultFacingDirection;
      }
      set
      {
        this.defaultFacingDirection = value;
      }
    }

    [XmlIgnore]
    public Dictionary<string, string> Dialogue
    {
      get
      {
        return this.dialogue;
      }
      set
      {
        this.dialogue = value;
      }
    }

    public string DefaultMap
    {
      get
      {
        return this.defaultMap;
      }
      set
      {
        this.defaultMap = value;
      }
    }

    public Vector2 DefaultPosition
    {
      get
      {
        return this.defaultPosition;
      }
      set
      {
        this.defaultPosition = value;
      }
    }

    [XmlIgnore]
    public Texture2D Portrait
    {
      get
      {
        return this.portrait;
      }
      set
      {
        this.portrait = value;
      }
    }

    [XmlIgnore]
    public Dictionary<int, SchedulePathDescription> Schedule
    {
      get
      {
        return this.schedule;
      }
      set
      {
        this.schedule = value;
      }
    }

    public bool IsWalkingInSquare
    {
      get
      {
        return this.isWalkingInSquare;
      }
      set
      {
        this.isWalkingInSquare = value;
      }
    }

    public bool IsWalkingTowardPlayer
    {
      get
      {
        return this.isWalkingTowardPlayer;
      }
      set
      {
        this.isWalkingTowardPlayer = value;
      }
    }

    [XmlIgnore]
    public Stack<StardewValley.Dialogue> CurrentDialogue
    {
      get
      {
        return this.currentDialogue;
      }
      set
      {
        this.currentDialogue = value;
      }
    }

    public NPC()
    {
    }

    public NPC(AnimatedSprite sprite, Vector2 position, int facingDir, string name, LocalizedContentManager content = null)
      : base(sprite, position, 2, name)
    {
      this.faceDirection(facingDir);
      sprite.standAndFaceDirection(facingDir);
      this.defaultPosition = position;
      this.defaultFacingDirection = facingDir;
      this.lastCrossroad = new Microsoft.Xna.Framework.Rectangle((int) position.X, (int) position.Y + Game1.tileSize, Game1.tileSize, Game1.tileSize);
      if (content == null)
        return;
      try
      {
        this.portrait = content.Load<Texture2D>("Portraits\\" + name);
      }
      catch (Exception ex)
      {
      }
    }

    public NPC(AnimatedSprite sprite, Vector2 position, string defaultMap, int facingDirection, string name, bool datable, Dictionary<int, int[]> schedule, Texture2D portrait)
      : this(sprite, position, defaultMap, facingDirection, name, schedule, portrait, false)
    {
      this.datable = datable;
    }

    public NPC(AnimatedSprite sprite, Vector2 position, string defaultMap, int facingDir, string name, Dictionary<int, int[]> schedule, Texture2D portrait, bool eventActor)
      : base(sprite, position, 2, name)
    {
      this.portrait = portrait;
      this.faceDirection(facingDir);
      if (sprite != null)
        sprite.faceDirectionStandard(facingDir);
      this.defaultPosition = position;
      this.defaultMap = defaultMap;
      this.currentLocation = Game1.getLocationFromName(defaultMap);
      this.defaultFacingDirection = facingDir;
      if (!eventActor)
      {
        if ((name.Equals("Lewis") || name.Equals("Robin")) && (Game1.NPCGiftTastes.ContainsKey(name) && !Game1.player.friendships.ContainsKey(name)))
          Game1.player.friendships.Add(name, new int[6]);
        this.loadSeasonalDialogue();
        this.lastCrossroad = new Microsoft.Xna.Framework.Rectangle((int) position.X, (int) position.Y + Game1.tileSize, Game1.tileSize, Game1.tileSize);
      }
      try
      {
        Dictionary<string, string> source = Game1.content.Load<Dictionary<string, string>>("Data\\NPCDispositions");
        if (!source.ContainsKey(name))
          return;
        string[] strArray = source[name].Split('/');
        string str1 = strArray[0];
        if (!(str1 == nameof (teen)))
        {
          if (str1 == nameof (child))
            this.age = 2;
        }
        else
          this.age = 1;
        string str2 = strArray[1];
        if (!(str2 == nameof (rude)))
        {
          if (str2 == nameof (polite))
            this.manners = 1;
        }
        else
          this.manners = 2;
        string str3 = strArray[2];
        if (!(str3 == nameof (shy)))
        {
          if (str3 == nameof (outgoing))
            this.socialAnxiety = 0;
        }
        else
          this.socialAnxiety = 1;
        string str4 = strArray[3];
        if (!(str4 == nameof (positive)))
        {
          if (str4 == nameof (negative))
            this.optimism = 1;
        }
        else
          this.optimism = 0;
        string str5 = strArray[4];
        if (!(str5 == nameof (female)))
        {
          if (str5 == nameof (undefined))
            this.gender = 2;
        }
        else
          this.gender = 1;
        string str6 = strArray[5];
        if (!(str6 == nameof (datable)))
        {
          if (str6 == "not-datable")
            this.datable = false;
        }
        else
          this.datable = true;
        this.loveInterest = strArray[6];
        string str7 = strArray[7];
        if (!(str7 == "Desert"))
        {
          if (!(str7 == "Other"))
          {
            if (str7 == "Town")
              this.homeRegion = 2;
          }
          else
            this.homeRegion = 0;
        }
        else
          this.homeRegion = 1;
        if (strArray.Length > 8)
        {
          this.birthday_Season = strArray[8].Split(' ')[0];
          this.birthday_Day = Convert.ToInt32(strArray[8].Split(' ')[1]);
        }
        for (int index = 0; index < source.Count; ++index)
        {
          if (source.ElementAt<KeyValuePair<string, string>>(index).Key.Equals(name))
          {
            this.id = index;
            break;
          }
        }
        this.displayName = strArray[11];
      }
      catch (Exception ex)
      {
      }
    }

    protected override string translateName(string name)
    {
      // ISSUE: reference to a compiler-generated method
      uint stringHash = \u003CPrivateImplementationDetails\u003E.ComputeStringHash(name);
      if (stringHash <= 2721151973U)
      {
        if (stringHash <= 2668186459U)
        {
          if ((int) stringHash != 689942318)
          {
            if ((int) stringHash != 764468226)
            {
              if ((int) stringHash == -1626780837 && name == "Mister Qi")
                return Game1.content.LoadString("Strings\\NPCNames:MisterQi");
            }
            else if (name == "Grandpa")
              return Game1.content.LoadString("Strings\\NPCNames:Grandpa");
          }
          else if (name == "Old Mariner")
            return Game1.content.LoadString("Strings\\NPCNames:OldMariner");
        }
        else if ((int) stringHash != -1583169328)
        {
          if ((int) stringHash != -1580323331)
          {
            if ((int) stringHash == -1573815323 && name == "Morris")
              return Game1.content.LoadString("Strings\\NPCNames:Morris");
          }
          else if (name == "Bouncer")
            return Game1.content.LoadString("Strings\\NPCNames:Bouncer");
        }
        else if (name == "Marlon")
          return Game1.content.LoadString("Strings\\NPCNames:Marlon");
      }
      else if (stringHash <= 3659193731U)
      {
        if ((int) stringHash != -1095409501)
        {
          if ((int) stringHash != -870902742)
          {
            if ((int) stringHash == -635773565 && name == "Kel")
              return Game1.content.LoadString("Strings\\NPCNames:Kel");
          }
          else if (name == "Gunther")
            return Game1.content.LoadString("Strings\\NPCNames:Gunther");
        }
        else if (name == "Gil")
          return Game1.content.LoadString("Strings\\NPCNames:Gil");
      }
      else if ((int) stringHash != -544040015)
      {
        if ((int) stringHash != -453459147)
        {
          if ((int) stringHash == -360051349 && name == "Governor")
            return Game1.content.LoadString("Strings\\NPCNames:Governor");
        }
        else if (name == "Henchman")
          return Game1.content.LoadString("Strings\\NPCNames:Henchman");
      }
      else if (name == "Welwick")
        return Game1.content.LoadString("Strings\\NPCNames:Welwick");
      return name;
    }

    public string getName()
    {
      if (this.displayName != null && this.displayName.Length > 0)
        return this.displayName;
      return this.name;
    }

    public virtual void reloadSprite()
    {
      string name = this.name;
      string str = name == "Old Mariner" ? "Mariner" : (name == "Dwarf King" ? "DwarfKing" : (name == "Mister Qi" ? "MrQi" : (name == "???" ? "Monsters\\Shadow Guy" : this.name)));
      if (this.name.Equals(Utility.getOtherFarmerNames()[0]))
        str = Game1.player.isMale ? "maleRival" : "femaleRival";
      if (!this.IsMonster)
      {
        this.sprite = new AnimatedSprite(Game1.content.Load<Texture2D>("Characters\\" + str));
        if (!this.name.Contains("Dwarf"))
          this.sprite.spriteHeight = 32;
      }
      else
        this.sprite = new AnimatedSprite(Game1.content.Load<Texture2D>("Monsters\\" + str));
      try
      {
        this.portrait = Game1.content.Load<Texture2D>("Portraits\\" + str);
      }
      catch (Exception ex)
      {
        this.portrait = (Texture2D) null;
      }
      int num = this.isInvisible ? 1 : 0;
      if (!Game1.newDay && (int) Game1.gameMode != 6)
        return;
      this.faceDirection(this.DefaultFacingDirection);
      this.scheduleTimeToTry = 9999999;
      this.previousEndPoint = new Point((int) this.defaultPosition.X / Game1.tileSize, (int) this.defaultPosition.Y / Game1.tileSize);
      this.Schedule = this.getSchedule(Game1.dayOfMonth);
      this.faceDirection(this.defaultFacingDirection);
      this.sprite.standAndFaceDirection(this.defaultFacingDirection);
      this.loadSeasonalDialogue();
      this.updateDialogue();
      if (this.isMarried())
        this.marriageDuties();
      bool flag = Utility.isFestivalDay(Game1.dayOfMonth, Game1.currentSeason);
      if (this.name.Equals("Robin") && Game1.player.daysUntilHouseUpgrade > 0 && !flag)
      {
        this.setTilePosition(68, 14);
        this.ignoreMultiplayerUpdates = true;
        this.sprite.setCurrentAnimation(new List<FarmerSprite.AnimationFrame>()
        {
          new FarmerSprite.AnimationFrame(24, 75),
          new FarmerSprite.AnimationFrame(25, 75),
          new FarmerSprite.AnimationFrame(26, 300, false, false, new AnimatedSprite.endOfAnimationBehavior(this.robinHammerSound), false),
          new FarmerSprite.AnimationFrame(27, 1000, false, false, new AnimatedSprite.endOfAnimationBehavior(this.robinVariablePause), false)
        });
        this.ignoreScheduleToday = true;
        this.CurrentDialogue.Clear();
        this.currentDialogue.Push(new StardewValley.Dialogue(Game1.player.daysUntilHouseUpgrade == 2 ? Game1.content.LoadString("Strings\\StringsFromCSFiles:NPC.cs.3926") : Game1.content.LoadString("Strings\\StringsFromCSFiles:NPC.cs.3927"), this));
      }
      else if (this.name.Equals("Robin") && Game1.getFarm().isThereABuildingUnderConstruction() && !flag)
      {
        this.ignoreMultiplayerUpdates = true;
        this.sprite.setCurrentAnimation(new List<FarmerSprite.AnimationFrame>()
        {
          new FarmerSprite.AnimationFrame(24, 75),
          new FarmerSprite.AnimationFrame(25, 75),
          new FarmerSprite.AnimationFrame(26, 300, false, false, new AnimatedSprite.endOfAnimationBehavior(this.robinHammerSound), false),
          new FarmerSprite.AnimationFrame(27, 1000, false, false, new AnimatedSprite.endOfAnimationBehavior(this.robinVariablePause), false)
        });
        this.ignoreScheduleToday = true;
        Building underConstruction = Game1.getFarm().getBuildingUnderConstruction();
        if (underConstruction.daysUntilUpgrade > 0)
        {
          if (!underConstruction.indoors.characters.Contains(this))
            underConstruction.indoors.addCharacter(this);
          if (this.currentLocation != null)
            this.currentLocation.characters.Remove(this);
          this.currentLocation = underConstruction.indoors;
          this.setTilePosition(1, 5);
        }
        else
        {
          Game1.warpCharacter(this, "Farm", new Vector2((float) (underConstruction.tileX + underConstruction.tilesWide / 2), (float) (underConstruction.tileY + underConstruction.tilesHigh / 2)), false, false);
          this.position.X += (float) (Game1.tileSize / 4);
          this.position.Y -= (float) (Game1.tileSize / 2);
        }
        this.CurrentDialogue.Clear();
        this.currentDialogue.Push(new StardewValley.Dialogue(Game1.content.LoadString("Strings\\StringsFromCSFiles:NPC.cs.3926"), this));
      }
      if (this.name.Equals("Shane") || this.name.Equals("Emily"))
        this.datable = true;
      try
      {
        this.displayName = Game1.content.Load<Dictionary<string, string>>("Data\\NPCDispositions")[this.name].Split('/')[11];
      }
      catch (Exception ex)
      {
      }
    }

    public void showTextAboveHead(string Text, int spriteTextColor = -1, int style = 2, int duration = 3000, int preTimer = 0)
    {
      this.textAboveHeadAlpha = 0.0f;
      this.textAboveHead = Text;
      this.textAboveHeadPreTimer = preTimer;
      this.textAboveHeadTimer = duration;
      this.textAboveHeadStyle = style;
      this.textAboveHeadColor = spriteTextColor;
    }

    public void loadSeasonalDialogue()
    {
      try
      {
        this.dialogue = Game1.content.Load<Dictionary<string, string>>("Characters\\Dialogue\\" + this.name);
      }
      catch (Exception ex)
      {
      }
    }

    public void moveToNewPlaceForEvent(int xTile, int yTile, string oldMap)
    {
      this.mapBeforeEvent = oldMap;
      this.positionBeforeEvent = this.position;
      this.position = new Vector2((float) (xTile * Game1.tileSize), (float) (yTile * Game1.tileSize - Game1.tileSize * 3 / 2));
    }

    public virtual bool hitWithTool(Tool t)
    {
      return false;
    }

    public bool canReceiveThisItemAsGift(Item i)
    {
      return i is Object || i is Ring || (i is Hat || i is Boots) || i is MeleeWeapon;
    }

    public int getGiftTasteForThisItem(Item item)
    {
      int num1 = 8;
      if (item is Object)
      {
        Object @object = item as Object;
        string str1;
        Game1.NPCGiftTastes.TryGetValue(this.name, out str1);
        string[] strArray1 = str1.Split('/');
        int parentSheetIndex = @object.ParentSheetIndex;
        int category = @object.Category;
        string str2 = string.Concat((object) parentSheetIndex);
        string str3 = string.Concat((object) category);
        if (((IEnumerable<string>) Game1.NPCGiftTastes["Universal_Love"].Split(' ')).Contains<string>(str3))
          num1 = 0;
        else if (((IEnumerable<string>) Game1.NPCGiftTastes["Universal_Hate"].Split(' ')).Contains<string>(str3))
          num1 = 6;
        else if (((IEnumerable<string>) Game1.NPCGiftTastes["Universal_Like"].Split(' ')).Contains<string>(str3))
          num1 = 2;
        else if (((IEnumerable<string>) Game1.NPCGiftTastes["Universal_Dislike"].Split(' ')).Contains<string>(str3))
          num1 = 4;
        bool flag1 = false;
        bool flag2 = false;
        if (((IEnumerable<string>) Game1.NPCGiftTastes["Universal_Love"].Split(' ')).Contains<string>(str2))
        {
          num1 = 0;
          flag1 = true;
        }
        else if (((IEnumerable<string>) Game1.NPCGiftTastes["Universal_Hate"].Split(' ')).Contains<string>(str2))
        {
          num1 = 6;
          flag1 = true;
        }
        else if (((IEnumerable<string>) Game1.NPCGiftTastes["Universal_Like"].Split(' ')).Contains<string>(str2))
        {
          num1 = 2;
          flag1 = true;
        }
        else if (((IEnumerable<string>) Game1.NPCGiftTastes["Universal_Dislike"].Split(' ')).Contains<string>(str2))
        {
          num1 = 4;
          flag1 = true;
        }
        else if (((IEnumerable<string>) Game1.NPCGiftTastes["Universal_Neutral"].Split(' ')).Contains<string>(str2))
        {
          num1 = 8;
          flag1 = true;
          flag2 = true;
        }
        if (num1 == 8 && !flag2)
        {
          if (@object.edibility != -300 && @object.edibility < 0)
            num1 = 6;
          else if (@object.price < 20)
            num1 = 4;
          else if (@object.type.Contains("Arch"))
          {
            num1 = 4;
            if (this.name.Equals("Penny"))
              num1 = 2;
          }
        }
        if (str1 != null)
        {
          List<int[]> numArrayList = new List<int[]>();
          int num2 = 0;
          while (num2 < 10)
          {
            string[] strArray2 = strArray1[num2 + 1].Split(' ');
            int[] numArray = new int[strArray2.Length];
            for (int index = 0; index < strArray2.Length; ++index)
            {
              if (strArray2[index].Length > 0)
                numArray[index] = Convert.ToInt32(strArray2[index]);
            }
            numArrayList.Add(numArray);
            num2 += 2;
          }
          if ((((IEnumerable<int>) numArrayList[0]).Contains<int>(parentSheetIndex) || category != 0 && ((IEnumerable<int>) numArrayList[0]).Contains<int>(category)) && (category == 0 || !((IEnumerable<int>) numArrayList[0]).Contains<int>(category) || !flag1))
            return 0;
          if ((((IEnumerable<int>) numArrayList[3]).Contains<int>(parentSheetIndex) || category != 0 && ((IEnumerable<int>) numArrayList[3]).Contains<int>(category)) && (category == 0 || !((IEnumerable<int>) numArrayList[3]).Contains<int>(category) || !flag1))
            return 6;
          if ((((IEnumerable<int>) numArrayList[1]).Contains<int>(parentSheetIndex) || category != 0 && ((IEnumerable<int>) numArrayList[1]).Contains<int>(category)) && (category == 0 || !((IEnumerable<int>) numArrayList[1]).Contains<int>(category) || !flag1))
            return 2;
          if ((((IEnumerable<int>) numArrayList[2]).Contains<int>(parentSheetIndex) || category != 0 && ((IEnumerable<int>) numArrayList[2]).Contains<int>(category)) && (category == 0 || !((IEnumerable<int>) numArrayList[2]).Contains<int>(category) || !flag1))
            return 4;
          if ((((IEnumerable<int>) numArrayList[4]).Contains<int>(parentSheetIndex) || category != 0 && ((IEnumerable<int>) numArrayList[4]).Contains<int>(category)) && (category == 0 || !((IEnumerable<int>) numArrayList[4]).Contains<int>(category) || !flag1))
            return 8;
        }
      }
      return num1;
    }

    private void goblinDoorEndBehavior(Character c, GameLocation l)
    {
      l.characters.Remove(this);
      Game1.playSound("doorClose");
    }

    public virtual void tryToReceiveActiveObject(Farmer who)
    {
      who.Halt();
      who.faceGeneralDirection(this.getStandingPosition(), 0);
      if (this.name.Equals("Henchman") && Game1.currentLocation.name.Equals("WitchSwamp"))
      {
        if (who.ActiveObject != null && who.ActiveObject.parentSheetIndex == 308)
        {
          if (this.controller != null)
            return;
          Game1.playSound("coin");
          who.reduceActiveItemByOne();
          this.CurrentDialogue.Push(new StardewValley.Dialogue(Game1.content.LoadString("Strings\\Characters:Henchman5"), this));
          Game1.drawDialogue(this);
          this.sprite.CurrentFrame = 4;
          Game1.player.removeQuest(27);
          Stack<Point> pathToEndPoint = new Stack<Point>();
          pathToEndPoint.Push(new Point(20, 21));
          pathToEndPoint.Push(new Point(20, 22));
          pathToEndPoint.Push(new Point(20, 23));
          pathToEndPoint.Push(new Point(20, 24));
          pathToEndPoint.Push(new Point(20, 25));
          pathToEndPoint.Push(new Point(20, 26));
          pathToEndPoint.Push(new Point(20, 27));
          pathToEndPoint.Push(new Point(20, 28));
          this.addedSpeed = 2;
          this.controller = new PathFindController(pathToEndPoint, (Character) this, Game1.currentLocation);
          this.controller.endBehaviorFunction = new PathFindController.endBehavior(this.goblinDoorEndBehavior);
          this.showTextAboveHead(Game1.content.LoadString("Strings\\Characters:Henchman6"), -1, 2, 3000, 0);
          Game1.player.mailReceived.Add("henchmanGone");
          Game1.currentLocation.removeTile(20, 29, "Buildings");
          who.freezePause = 2000;
        }
        else
        {
          if (who.ActiveObject == null)
            return;
          if (who.ActiveObject.parentSheetIndex == 684)
            this.CurrentDialogue.Push(new StardewValley.Dialogue(Game1.content.LoadString("Strings\\Characters:Henchman4"), this));
          else
            this.CurrentDialogue.Push(new StardewValley.Dialogue(Game1.content.LoadString("Strings\\Characters:Henchman3"), this));
          Game1.drawDialogue(this);
        }
      }
      else if (Game1.questOfTheDay != null && Game1.questOfTheDay.accepted && (!Game1.questOfTheDay.completed && Game1.questOfTheDay.GetType().Name.Equals("ItemDeliveryQuest")) && Game1.questOfTheDay.checkIfComplete(this, -1, -1, (Item) who.ActiveObject, (string) null))
      {
        who.reduceActiveItemByOne();
        who.completelyStopAnimatingOrDoingAction();
        if (Game1.random.NextDouble() >= 0.3 || this.name.Equals("Wizard"))
          return;
        this.doEmote(32, true);
      }
      else if (Game1.questOfTheDay != null && Game1.questOfTheDay.GetType().Name.Equals("FishingQuest") && Game1.questOfTheDay.checkIfComplete(this, who.ActiveObject.ParentSheetIndex, -1, (Item) null, (string) null))
      {
        who.reduceActiveItemByOne();
        who.completelyStopAnimatingOrDoingAction();
        if (Game1.random.NextDouble() >= 0.3 || this.name.Equals("Wizard"))
          return;
        this.doEmote(32, true);
      }
      else if (who.ActiveObject != null && who.ActiveObject.questItem)
      {
        if (who.checkForQuestComplete(this, -1, -1, (Item) who.ActiveObject, "", 9, 3))
          return;
        Game1.showRedMessage(Game1.content.LoadString("Strings\\StringsFromCSFiles:NPC.cs.3954"));
      }
      else
      {
        if (who.checkForQuestComplete(this, -1, -1, (Item) null, "", 10, -1) || !Game1.NPCGiftTastes.ContainsKey(this.name))
          return;
        who.completeQuest(25);
        if (who.ActiveObject.ParentSheetIndex == 458)
        {
          if (!this.datable)
          {
            if (Game1.random.NextDouble() < 0.5)
            {
              Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\StringsFromCSFiles:NPC.cs.3955", (object) this.displayName));
            }
            else
            {
              this.CurrentDialogue.Push(new StardewValley.Dialogue(Game1.random.NextDouble() < 0.5 ? Game1.content.LoadString("Strings\\StringsFromCSFiles:NPC.cs.3956") : Game1.LoadStringByGender(this.gender, "Strings\\StringsFromCSFiles:NPC.cs.3957"), this));
              Game1.drawDialogue(this);
            }
          }
          else if (this.datable && this.divorcedFromFarmer)
          {
            this.CurrentDialogue.Push(new StardewValley.Dialogue(Game1.content.LoadString("Strings\\Characters:Divorced_bouquet"), this));
            Game1.drawDialogue(this);
          }
          else if (this.datable && who.friendships.ContainsKey(this.name) && who.friendships[this.name][0] < 1000)
          {
            this.CurrentDialogue.Push(new StardewValley.Dialogue(Game1.random.NextDouble() < 0.5 ? Game1.content.LoadString("Strings\\StringsFromCSFiles:NPC.cs.3958") : Game1.LoadStringByGender(this.gender, "Strings\\StringsFromCSFiles:NPC.cs.3959"), this));
            Game1.drawDialogue(this);
          }
          else if (this.datable && who.friendships.ContainsKey(this.name) && who.friendships[this.name][0] < 2000)
          {
            this.CurrentDialogue.Push(new StardewValley.Dialogue(Game1.random.NextDouble() < 0.5 ? Game1.content.LoadString("Strings\\StringsFromCSFiles:NPC.cs.3960") : Game1.content.LoadString("Strings\\StringsFromCSFiles:NPC.cs.3961"), this));
            Game1.drawDialogue(this);
          }
          else
          {
            this.datingFarmer = true;
            this.CurrentDialogue.Push(new StardewValley.Dialogue(Game1.random.NextDouble() < 0.5 ? Game1.LoadStringByGender(this.gender, "Strings\\StringsFromCSFiles:NPC.cs.3962") : Game1.LoadStringByGender(this.gender, "Strings\\StringsFromCSFiles:NPC.cs.3963"), this));
            who.changeFriendship(25, this);
            who.reduceActiveItemByOne();
            who.completelyStopAnimatingOrDoingAction();
            this.doEmote(20, true);
            Game1.drawDialogue(this);
          }
        }
        else if (who.ActiveObject.ParentSheetIndex == 460)
        {
          if (who.spouse != null)
          {
            if (who.spouse.Contains("engaged"))
            {
              this.CurrentDialogue.Push(new StardewValley.Dialogue(Game1.random.NextDouble() < 0.5 ? Game1.LoadStringByGender(this.gender, "Strings\\StringsFromCSFiles:NPC.cs.3965") : Game1.LoadStringByGender(this.gender, "Strings\\StringsFromCSFiles:NPC.cs.3966"), this));
              Game1.drawDialogue(this);
            }
            else
            {
              this.CurrentDialogue.Push(new StardewValley.Dialogue(Game1.random.NextDouble() < 0.5 ? Game1.content.LoadString("Strings\\StringsFromCSFiles:NPC.cs.3967") : Game1.LoadStringByGender(this.gender, "Strings\\StringsFromCSFiles:NPC.cs.3968"), this));
              Game1.drawDialogue(this);
            }
          }
          else if (!this.datable || this.divorcedFromFarmer || who.friendships.ContainsKey(this.name) && who.friendships[this.name][0] < 1500)
          {
            if (Game1.random.NextDouble() < 0.5)
            {
              Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\StringsFromCSFiles:NPC.cs.3969", (object) this.displayName));
            }
            else
            {
              this.CurrentDialogue.Push(new StardewValley.Dialogue(this.gender == 1 ? Game1.content.LoadString("Strings\\StringsFromCSFiles:NPC.cs.3970") : Game1.content.LoadString("Strings\\StringsFromCSFiles:NPC.cs.3971"), this));
              Game1.drawDialogue(this);
            }
          }
          else if (this.datable && who.friendships.ContainsKey(this.name) && who.friendships[this.name][0] < 2500)
          {
            if (who.friendships[this.name][4] == 0)
            {
              this.CurrentDialogue.Push(new StardewValley.Dialogue(Game1.random.NextDouble() < 0.5 ? Game1.content.LoadString("Strings\\StringsFromCSFiles:NPC.cs.3972") : Game1.content.LoadString("Strings\\StringsFromCSFiles:NPC.cs.3973"), this));
              Game1.drawDialogue(this);
              who.changeFriendship(-20, this);
              who.friendships[this.name][4] = 1;
            }
            else
            {
              this.CurrentDialogue.Push(new StardewValley.Dialogue(Game1.random.NextDouble() < 0.5 ? Game1.LoadStringByGender(this.gender, "Strings\\StringsFromCSFiles:NPC.cs.3974") : Game1.LoadStringByGender(this.gender, "Strings\\StringsFromCSFiles:NPC.cs.3975"), this));
              Game1.drawDialogue(this);
              who.changeFriendship(-50, this);
            }
          }
          else
          {
            Game1.changeMusicTrack("none");
            who.spouse = this.name + "engaged";
            Game1.countdownToWedding = 3;
            this.datingFarmer = true;
            this.CurrentDialogue.Clear();
            this.CurrentDialogue.Push(new StardewValley.Dialogue(Game1.content.Load<Dictionary<string, string>>("Data\\EngagementDialogue")[this.name + "0"], this));
            this.CurrentDialogue.Push(new StardewValley.Dialogue(Game1.content.LoadString("Strings\\StringsFromCSFiles:NPC.cs.3980"), this));
            who.changeFriendship(1, this);
            who.reduceActiveItemByOne();
            who.completelyStopAnimatingOrDoingAction();
            Game1.drawDialogue(this);
          }
        }
        else if (who.friendships.ContainsKey(this.name) && who.friendships[this.name][1] < 2 || who.spouse != null && who.spouse.Equals(this.name) || (this is Child || this.isBirthday(Game1.currentSeason, Game1.dayOfMonth)))
        {
          if (this.divorcedFromFarmer)
          {
            this.CurrentDialogue.Push(new StardewValley.Dialogue(Game1.content.LoadString("Strings\\Characters:Divorced_gift"), this));
            Game1.drawDialogue(this);
          }
          else if (who.friendships[this.name][3] == 1)
          {
            Game1.drawObjectDialogue(Game1.parseText(Game1.content.LoadString("Strings\\StringsFromCSFiles:NPC.cs.3981", (object) this.displayName)));
          }
          else
          {
            this.receiveGift(who.ActiveObject, who, true, 1f, true);
            who.reduceActiveItemByOne();
            who.completelyStopAnimatingOrDoingAction();
            this.faceTowardFarmerForPeriod(4000, 3, false, who);
            if (!this.datable || who.spouse == null || (who.spouse.Contains(this.name) || Utility.isMale(who.spouse.Replace("engaged", "")) != Utility.isMale(this.name)) || (Game1.random.NextDouble() >= 0.3 - (double) who.LuckLevel / 100.0 - Game1.dailyLuck || this.isBirthday(Game1.currentSeason, Game1.dayOfMonth)))
              return;
            NPC characterFromName = Game1.getCharacterFromName(who.spouse.Replace("engaged", ""), false);
            who.changeFriendship(-30, characterFromName);
            characterFromName.CurrentDialogue.Clear();
            characterFromName.CurrentDialogue.Push(new StardewValley.Dialogue(Game1.content.LoadString("Strings\\StringsFromCSFiles:NPC.cs.3985", (object) this.displayName), characterFromName));
          }
        }
        else
          Game1.drawObjectDialogue(Game1.parseText(Game1.content.LoadString("Strings\\StringsFromCSFiles:NPC.cs.3987", (object) this.displayName, (object) 2)));
      }
    }

    public void haltMe(Farmer who)
    {
      this.Halt();
    }

    public virtual bool checkAction(Farmer who, GameLocation l)
    {
      if (this.isInvisible)
        return false;
      if (who.isRidingHorse())
        who.Halt();
      if (this.name.Equals("Henchman") && l.name.Equals("WitchSwamp"))
      {
        if (!Game1.player.mailReceived.Contains("Henchman1"))
        {
          Game1.player.mailReceived.Add("Henchman1");
          this.CurrentDialogue.Push(new StardewValley.Dialogue(Game1.content.LoadString("Strings\\Characters:Henchman1"), this));
          Game1.drawDialogue(this);
          Game1.player.addQuest(27);
          Game1.player.friendships.Add("Henchman", new int[6]);
        }
        else
        {
          if (who.ActiveObject != null && who.ActiveObject.canBeGivenAsGift())
          {
            this.tryToReceiveActiveObject(who);
            return true;
          }
          if (this.controller == null)
          {
            this.CurrentDialogue.Push(new StardewValley.Dialogue(Game1.content.LoadString("Strings\\Characters:Henchman2"), this));
            Game1.drawDialogue(this);
          }
        }
        return true;
      }
      if (Game1.NPCGiftTastes.ContainsKey(this.name) && !Game1.player.friendships.ContainsKey(this.name))
      {
        Game1.player.friendships.Add(this.name, new int[6]);
        if (this.name.Equals("Krobus"))
        {
          this.currentDialogue.Push(new StardewValley.Dialogue(Game1.content.LoadString("Strings\\StringsFromCSFiles:NPC.cs.3990"), this));
          Game1.drawDialogue(this);
          return true;
        }
      }
      if (who.checkForQuestComplete(this, -1, -1, (Item) who.ActiveObject, (string) null, -1, 5))
      {
        this.faceTowardFarmerForPeriod(6000, 3, false, who);
        return true;
      }
      if (this.name.Equals("Dwarf") && this.currentDialogue.Count <= 0 && (who.canUnderstandDwarves && l.name.Equals("Mine")))
        Game1.activeClickableMenu = (IClickableMenu) new ShopMenu(Utility.getDwarfShopStock(), 0, "Dwarf");
      if (this.name.Equals("Krobus"))
      {
        if (who.hasQuest(28))
        {
          this.CurrentDialogue.Push(new StardewValley.Dialogue(Game1.content.LoadString("Strings\\Characters:KrobusDarkTalisman"), this));
          Game1.drawDialogue(this);
          who.removeQuest(28);
          who.mailReceived.Add("krobusUnseal");
          TemporaryAnimatedSprite t1 = new TemporaryAnimatedSprite(Projectile.projectileSheet, new Microsoft.Xna.Framework.Rectangle(0, 0, 16, 16), 3000f, 1, 0, new Vector2(31f, 17f) * (float) Game1.tileSize, false, false);
          t1.scale = (float) Game1.pixelZoom;
          t1.delayBeforeAnimationStart = 1;
          t1.startSound = "debuffSpell";
          t1.motion = new Vector2(-9f, 1f);
          t1.rotationChange = (float) Math.PI / 64f;
          int num1 = 1;
          t1.light = num1 != 0;
          double num2 = 1.0;
          t1.lightRadius = (float) num2;
          Color color1 = new Color(150, 0, 50);
          t1.lightcolor = color1;
          double num3 = 1.0;
          t1.layerDepth = (float) num3;
          double num4 = 3.0 / 1000.0;
          t1.alphaFade = (float) num4;
          GameLocation l1 = l;
          int timer1 = 200;
          int num5 = 1;
          DelayedAction.addTemporarySpriteAfterDelay(t1, l1, timer1, num5 != 0);
          TemporaryAnimatedSprite t2 = new TemporaryAnimatedSprite(Projectile.projectileSheet, new Microsoft.Xna.Framework.Rectangle(0, 0, 16, 16), 3000f, 1, 0, new Vector2(31f, 17f) * (float) Game1.tileSize, false, false);
          t2.startSound = "debuffSpell";
          t2.delayBeforeAnimationStart = 1;
          double pixelZoom = (double) Game1.pixelZoom;
          t2.scale = (float) pixelZoom;
          Vector2 vector2 = new Vector2(-9f, 1f);
          t2.motion = vector2;
          double num6 = 0.0490873865783215;
          t2.rotationChange = (float) num6;
          int num7 = 1;
          t2.light = num7 != 0;
          double num8 = 1.0;
          t2.lightRadius = (float) num8;
          Color color2 = new Color(150, 0, 50);
          t2.lightcolor = color2;
          double num9 = 1.0;
          t2.layerDepth = (float) num9;
          double num10 = 3.0 / 1000.0;
          t2.alphaFade = (float) num10;
          GameLocation l2 = l;
          int timer2 = 700;
          int num11 = 1;
          DelayedAction.addTemporarySpriteAfterDelay(t2, l2, timer2, num11 != 0);
          return true;
        }
        if (this.currentDialogue.Count <= 0 && l is Sewer)
          Game1.activeClickableMenu = (IClickableMenu) new ShopMenu((l as Sewer).getShadowShopStock(), 0, "Krobus");
      }
      if (this.name.Equals(who.spouse) && who.IsMainPlayer)
      {
        int timeOfDay = Game1.timeOfDay;
        if (this.sprite.currentAnimation == null)
          this.faceDirection(-3);
        if (this.sprite.currentAnimation == null && who.friendships.ContainsKey(this.name) && (who.friendships[this.name][0] >= 3375 && !who.mailReceived.Contains("CF_Spouse")))
        {
          this.CurrentDialogue.Push(new StardewValley.Dialogue(Game1.content.LoadString("Strings\\StringsFromCSFiles:NPC.cs.4001"), this));
          Game1.player.addItemByMenuIfNecessary((Item) new Object(Vector2.Zero, 434, "Cosmic Fruit", false, false, false, false), (ItemGrabMenu.behaviorOnItemSelect) null);
          who.mailReceived.Add("CF_Spouse");
          return true;
        }
        if (this.sprite.currentAnimation == null && !this.hasTemporaryMessageAvailable() && (this.CurrentDialogue.Count == 0 && Game1.timeOfDay < 2200) && (this.controller == null && who.ActiveObject == null))
        {
          this.faceGeneralDirection(who.getStandingPosition(), 0);
          who.faceGeneralDirection(this.getStandingPosition(), 0);
          if (this.facingDirection == 3 || this.facingDirection == 1)
          {
            int frame = 28;
            bool flag = true;
            string name = this.name;
            // ISSUE: reference to a compiler-generated method
            uint stringHash = \u003CPrivateImplementationDetails\u003E.ComputeStringHash(name);
            if (stringHash <= 1708213605U)
            {
              if (stringHash <= 587846041U)
              {
                if ((int) stringHash != 161540545)
                {
                  if ((int) stringHash == 587846041 && name == "Penny")
                  {
                    frame = 35;
                    flag = true;
                  }
                }
                else if (name == "Sebastian")
                {
                  frame = 40;
                  flag = false;
                }
              }
              else if ((int) stringHash != 1067922812)
              {
                if ((int) stringHash != 1281010426)
                {
                  if ((int) stringHash == 1708213605 && name == "Alex")
                  {
                    frame = 42;
                    flag = true;
                  }
                }
                else if (name == "Maru")
                {
                  frame = 28;
                  flag = false;
                }
              }
              else if (name == "Sam")
              {
                frame = 36;
                flag = true;
              }
            }
            else if (stringHash <= 2571828641U)
            {
              if ((int) stringHash != 1866496948)
              {
                if ((int) stringHash != 2010304804)
                {
                  if ((int) stringHash == -1723138655 && name == "Emily")
                  {
                    frame = 33;
                    flag = false;
                  }
                }
                else if (name == "Harvey")
                {
                  frame = 31;
                  flag = false;
                }
              }
              else if (name == "Shane")
              {
                frame = 34;
                flag = false;
              }
            }
            else if ((int) stringHash != -1562053956)
            {
              if ((int) stringHash != -1468719973)
              {
                if ((int) stringHash == -1228790996 && name == "Elliott")
                {
                  frame = 35;
                  flag = false;
                }
              }
              else if (name == "Leah")
              {
                frame = 25;
                flag = true;
              }
            }
            else if (name == "Abigail")
            {
              frame = 33;
              flag = false;
            }
            bool flip = flag && this.facingDirection == 3 || !flag && this.facingDirection == 1;
            if (who.getFriendshipHeartLevelForNPC(this.name) > 9)
            {
              this.sprite.setCurrentAnimation(new List<FarmerSprite.AnimationFrame>()
              {
                new FarmerSprite.AnimationFrame(frame, Game1.IsMultiplayer ? 1000 : 10, false, flip, new AnimatedSprite.endOfAnimationBehavior(this.haltMe), true)
              });
              if (!this.hasBeenKissedToday)
              {
                who.changeFriendship(10, this);
                who.currentLocation.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(211, 428, 7, 6), 2000f, 1, 0, new Vector2((float) this.getTileX(), (float) this.getTileY()) * (float) Game1.tileSize + new Vector2((float) (Game1.tileSize / 4), (float) -Game1.tileSize), false, false, 1f, 0.0f, Color.White, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false)
                {
                  motion = new Vector2(0.0f, -0.5f),
                  alphaFade = 0.01f
                });
                Game1.playSound("dwop");
                who.exhausted = false;
              }
              this.hasBeenKissedToday = true;
            }
            else
            {
              this.faceDirection(Game1.random.NextDouble() < 0.5 ? 2 : 0);
              this.doEmote(12, true);
            }
            who.CanMove = false;
            who.FarmerSprite.pauseForSingleAnimation = false;
            if (flag && !flip || !flag & flip)
              who.faceDirection(3);
            else
              who.faceDirection(1);
            who.FarmerSprite.animateOnce(new List<FarmerSprite.AnimationFrame>()
            {
              new FarmerSprite.AnimationFrame(101, 1000, 0, false, who.facingDirection == 3, (AnimatedSprite.endOfAnimationBehavior) null, false, 0),
              new FarmerSprite.AnimationFrame(6, 1, false, who.facingDirection == 3, new AnimatedSprite.endOfAnimationBehavior(Farmer.completelyStopAnimating), false)
            }.ToArray());
            return true;
          }
        }
      }
      bool flag1 = false;
      if (who.friendships.ContainsKey(this.name))
      {
        flag1 = this.checkForNewCurrentDialogue(who.friendships[this.name][0], false);
        if (!flag1)
          flag1 = this.checkForNewCurrentDialogue(who.friendships[this.name][0], true);
      }
      if (who.IsMainPlayer && who.friendships.ContainsKey(this.name) && this.endOfRouteMessage != null | flag1)
      {
        if (!flag1 && this.setTemporaryMessages(who))
        {
          Game1.player.checkForQuestComplete(this, -1, -1, (Item) null, (string) null, 5, -1);
          return false;
        }
        if (this.sprite.Texture.Bounds.Height > 32)
          this.faceTowardFarmerForPeriod(5000, 4, false, who);
        if (who.ActiveObject != null && who.ActiveObject.canBeGivenAsGift())
        {
          this.tryToReceiveActiveObject(who);
          Game1.stats.checkForFriendshipAchievements();
          this.faceTowardFarmerForPeriod(3000, 4, false, who);
          return true;
        }
        if (!this.name.Contains("King") && !who.hasPlayerTalkedToNPC(this.name) && who.friendships.ContainsKey(this.name))
        {
          who.friendships[this.name][2] = 1;
          who.changeFriendship(10, this);
          Game1.stats.checkForFriendshipAchievements();
          Game1.player.checkForQuestComplete(this, -1, -1, (Item) null, (string) null, 5, -1);
        }
        Game1.drawDialogue(this);
      }
      else if (this.CurrentDialogue.Count > 0)
      {
        if (!this.name.Contains("King") && who.ActiveObject != null && who.ActiveObject.canBeGivenAsGift())
        {
          if (who.IsMainPlayer)
          {
            this.tryToReceiveActiveObject(who);
            Game1.stats.checkForFriendshipAchievements();
          }
          else
            this.faceTowardFarmerForPeriod(3000, 4, false, who);
        }
        else if (who.hasClubCard && this.name.Equals("Bouncer") && who.IsMainPlayer)
        {
          Response[] answerChoices = new Response[2]
          {
            new Response("Yes.", Game1.content.LoadString("Strings\\StringsFromCSFiles:NPC.cs.4018")),
            new Response("That's", Game1.content.LoadString("Strings\\StringsFromCSFiles:NPC.cs.4020"))
          };
          l.createQuestionDialogue(Game1.content.LoadString("Strings\\StringsFromCSFiles:NPC.cs.4021"), answerChoices, "ClubCard");
        }
        else if (this.CurrentDialogue.Count >= 1 || this.endOfRouteMessage != null)
        {
          if (this.setTemporaryMessages(who))
          {
            Game1.player.checkForQuestComplete(this, -1, -1, (Item) null, (string) null, 5, -1);
            return false;
          }
          if (this.sprite.Texture.Bounds.Height > 32)
            this.faceTowardFarmerForPeriod(5000, 4, false, who);
          if (who.IsMainPlayer)
          {
            if (!this.name.Contains("King") && !who.hasPlayerTalkedToNPC(this.name) && who.friendships.ContainsKey(this.name))
            {
              who.friendships[this.name][2] = 1;
              Game1.player.checkForQuestComplete(this, -1, -1, (Item) null, (string) null, 5, -1);
              who.changeFriendship(20, this);
              Game1.stats.checkForFriendshipAchievements();
            }
            Game1.drawDialogue(this);
          }
        }
        else if (!this.doingEndOfRouteAnimation)
        {
          try
          {
            if (who.friendships.ContainsKey(this.name))
              this.faceTowardFarmerForPeriod(who.friendships[this.name][0] / 125 * 1000 + 1000, 4, false, who);
          }
          catch (Exception ex)
          {
          }
          if (Game1.random.NextDouble() < 0.1)
            this.doEmote(8, true);
        }
      }
      else if (this.name.Equals("Cat") && !(this as StardewValley.Monsters.Cat).wasPet)
      {
        (this as StardewValley.Monsters.Cat).wasPet = true;
        (this as StardewValley.Monsters.Cat).loveForMaster += 10;
        this.doEmote(20, true);
        Game1.playSound("purr");
      }
      else if (who.ActiveObject != null && who.ActiveObject.canBeGivenAsGift())
      {
        this.tryToReceiveActiveObject(who);
        Game1.stats.checkForFriendshipAchievements();
        this.faceTowardFarmerForPeriod(3000, 4, false, who);
        return true;
      }
      if (this.setTemporaryMessages(who) || !this.doingEndOfRouteAnimation && this.goingToDoEndOfRouteAnimation || this.endOfRouteMessage == null)
        return false;
      Game1.drawDialogue(this);
      return false;
    }

    public override void MovePosition(GameTime time, xTile.Dimensions.Rectangle viewport, GameLocation currentLocation)
    {
      if (this.movementPause > 0)
        return;
      base.MovePosition(time, viewport, currentLocation);
    }

    public GameLocation getHome()
    {
      return Game1.getLocationFromName(this.defaultMap);
    }

    public override bool canPassThroughActionTiles()
    {
      return true;
    }

    public virtual void behaviorOnFarmerPushing()
    {
    }

    public virtual void behaviorOnFarmerLocationEntry(GameLocation location, Farmer who)
    {
      if (this.sprite == null || this.sprite.currentAnimation != null || this.sprite.sourceRect.Height <= 32)
        return;
      this.sprite.spriteWidth = 16;
      this.sprite.spriteHeight = 16;
      this.sprite.CurrentFrame = 0;
    }

    public override void updateMovement(GameLocation location, GameTime time)
    {
      this.lastPosition = this.position;
      if (this.DirectionsToNewLocation != null && !Game1.newDay)
      {
        if (this.getStandingX() < -Game1.tileSize || this.getStandingX() > location.map.DisplayWidth + Game1.tileSize || (this.getStandingY() < -Game1.tileSize || this.getStandingY() > location.map.DisplayHeight + Game1.tileSize))
        {
          this.IsWalkingInSquare = false;
          Game1.warpCharacter(this, this.DefaultMap, this.DefaultPosition, true, true);
          location.characters.Remove(this);
        }
        else if (this.IsWalkingInSquare)
        {
          this.returnToEndPoint();
          this.MovePosition(time, Game1.viewport, location);
        }
        else
        {
          if (!this.followSchedule)
            return;
          this.MovePosition(time, Game1.viewport, location);
          Warp warp = location.isCollidingWithWarp(this.GetBoundingBox());
          PropertyValue propertyValue = (PropertyValue) null;
          Tile tile1 = location.map.GetLayer("Buildings").PickTile(this.nextPositionPoint(), Game1.viewport.Size);
          if (tile1 != null)
            tile1.Properties.TryGetValue("Action", out propertyValue);
          string[] strArray1;
          if (propertyValue != null)
            strArray1 = propertyValue.ToString().Split(' ');
          else
            strArray1 = (string[]) null;
          string[] strArray2 = strArray1;
          if (warp != null)
          {
            if (location is BusStop && warp.TargetName.Equals("Farm"))
            {
              Point entryLocation = ((this.isMarried() ? (GameLocation) (this.getHome() as FarmHouse) : Game1.getLocationFromName("FarmHouse")) as FarmHouse).getEntryLocation();
              warp = new Warp(warp.X, warp.Y, "FarmHouse", entryLocation.X, entryLocation.Y, false);
            }
            else if (location is FarmHouse && warp.TargetName.Equals("Farm"))
              warp = new Warp(warp.X, warp.Y, "BusStop", 0, 23, false);
            Game1.warpCharacter(this, warp.TargetName, new Vector2((float) (warp.TargetX * Game1.tileSize), (float) (warp.TargetY * Game1.tileSize - this.Sprite.getHeight() / 2 - Game1.tileSize / 4)), false, location.IsOutdoors);
            location.characters.Remove(this);
          }
          else if (strArray2 != null && strArray2.Length >= 1 && strArray2[0].Contains("Warp"))
          {
            Game1.warpCharacter(this, strArray2[3], new Vector2((float) Convert.ToInt32(strArray2[1]), (float) Convert.ToInt32(strArray2[2])), false, location.IsOutdoors);
            if (Game1.currentLocation.name.Equals(location.name) && Utility.isOnScreen(this.getStandingPosition(), Game1.tileSize * 3))
              Game1.playSound("doorClose");
            location.characters.Remove(this);
          }
          else if (strArray2 != null && strArray2.Length >= 1 && strArray2[0].Contains("Door"))
          {
            location.openDoor(new Location(this.nextPositionPoint().X / Game1.tileSize, this.nextPositionPoint().Y / Game1.tileSize), Game1.player.currentLocation.Equals((object) location));
          }
          else
          {
            if (location.map.GetLayer("Paths") == null)
              return;
            Tile tile2 = location.map.GetLayer("Paths").PickTile(new Location(this.getStandingX(), this.getStandingY()), Game1.viewport.Size);
            Microsoft.Xna.Framework.Rectangle boundingBox = this.GetBoundingBox();
            boundingBox.Inflate(2, 2);
            if (tile2 == null || !new Microsoft.Xna.Framework.Rectangle(this.getStandingX() - this.getStandingX() % Game1.tileSize, this.getStandingY() - this.getStandingY() % Game1.tileSize, Game1.tileSize, Game1.tileSize).Contains(boundingBox))
              return;
            switch (tile2.TileIndex)
            {
              case 0:
                if (this.getDirection() == 3)
                {
                  this.SetMovingOnlyUp();
                  break;
                }
                if (this.getDirection() != 2)
                  break;
                this.SetMovingOnlyRight();
                break;
              case 1:
                if (this.getDirection() == 3)
                {
                  this.SetMovingOnlyDown();
                  break;
                }
                if (this.getDirection() != 0)
                  break;
                this.SetMovingOnlyRight();
                break;
              case 2:
                if (this.getDirection() == 1)
                {
                  this.SetMovingOnlyDown();
                  break;
                }
                if (this.getDirection() != 0)
                  break;
                this.SetMovingOnlyLeft();
                break;
              case 3:
                if (this.getDirection() == 1)
                {
                  this.SetMovingOnlyUp();
                  break;
                }
                if (this.getDirection() != 2)
                  break;
                this.SetMovingOnlyLeft();
                break;
              case 4:
                this.changeSchedulePathDirection();
                this.moveCharacterOnSchedulePath();
                break;
              case 7:
                this.ReachedEndPoint();
                break;
            }
          }
        }
      }
      else
      {
        if (!this.IsWalkingInSquare)
          return;
        this.randomSquareMovement(time);
        this.MovePosition(time, Game1.viewport, location);
      }
    }

    public void facePlayer(Farmer who)
    {
      if (this.facingDirectionBeforeSpeakingToPlayer == -1)
        this.facingDirectionBeforeSpeakingToPlayer = this.getFacingDirection();
      this.faceDirection((who.FacingDirection + 2) % 4);
    }

    public void doneFacingPlayer(Farmer who)
    {
    }

    public override void update(GameTime time, GameLocation location)
    {
      if (this.returningToEndPoint)
      {
        this.returnToEndPoint();
        this.MovePosition(time, Game1.viewport, location);
      }
      else if (this.temporaryController != null)
      {
        if (this.temporaryController.update(time))
          this.temporaryController = (PathFindController) null;
        this.updateEmote(time);
      }
      else
        base.update(time, location);
      if (this.textAboveHeadTimer > 0)
      {
        if (this.textAboveHeadPreTimer > 0)
        {
          this.textAboveHeadPreTimer = this.textAboveHeadPreTimer - time.ElapsedGameTime.Milliseconds;
        }
        else
        {
          this.textAboveHeadTimer = this.textAboveHeadTimer - time.ElapsedGameTime.Milliseconds;
          this.textAboveHeadAlpha = this.textAboveHeadTimer <= 500 ? Math.Max(0.0f, this.textAboveHeadAlpha - 0.04f) : Math.Min(1f, this.textAboveHeadAlpha + 0.1f);
        }
      }
      if (this.isWalkingInSquare && !this.returningToEndPoint)
        this.randomSquareMovement(time);
      if (this.Sprite != null && this.Sprite.currentAnimation != null && (!Game1.eventUp && this.Sprite.animateOnce(time)))
        this.Sprite.currentAnimation = (List<FarmerSprite.AnimationFrame>) null;
      TimeSpan timeSpan;
      if (this.movementPause > 0 && (!Game1.dialogueUp || this.controller != null))
      {
        this.freezeMotion = true;
        int movementPause = this.movementPause;
        timeSpan = time.ElapsedGameTime;
        int milliseconds = timeSpan.Milliseconds;
        this.movementPause = movementPause - milliseconds;
        if (this.movementPause <= 0)
          this.freezeMotion = false;
      }
      if (this.shakeTimer > 0)
      {
        int shakeTimer = this.shakeTimer;
        timeSpan = time.ElapsedGameTime;
        int milliseconds = timeSpan.Milliseconds;
        this.shakeTimer = shakeTimer - milliseconds;
      }
      if (this.lastPosition.Equals(this.position))
      {
        double sinceLastMovement = (double) this.timerSinceLastMovement;
        timeSpan = time.ElapsedGameTime;
        double milliseconds = (double) timeSpan.Milliseconds;
        this.timerSinceLastMovement = (float) (sinceLastMovement + milliseconds);
      }
      else
        this.timerSinceLastMovement = 0.0f;
      if (!this.swimming)
        return;
      timeSpan = time.TotalGameTime;
      this.yOffset = (float) Math.Cos(timeSpan.TotalMilliseconds / 2000.0) * (float) Game1.pixelZoom;
      float swimTimer1 = this.swimTimer;
      double swimTimer2 = (double) this.swimTimer;
      timeSpan = time.ElapsedGameTime;
      double milliseconds1 = (double) timeSpan.Milliseconds;
      this.swimTimer = (float) (swimTimer2 - milliseconds1);
      if ((double) this.timerSinceLastMovement == 0.0)
      {
        if ((double) swimTimer1 > 400.0 && (double) this.swimTimer <= 400.0 && location.Equals((object) Game1.currentLocation))
        {
          location.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.animations, new Microsoft.Xna.Framework.Rectangle(0, 0, Game1.tileSize, Game1.tileSize), (float) (150.0 - ((double) Math.Abs(this.xVelocity) + (double) Math.Abs(this.yVelocity)) * 3.0), 8, 0, new Vector2(this.position.X, (float) (this.getStandingY() - Game1.tileSize / 2)), false, Game1.random.NextDouble() < 0.5, 0.01f, 0.01f, Color.White, 1f, 3f / 1000f, 0.0f, 0.0f, false));
          Game1.playSound("slosh");
        }
        if ((double) this.swimTimer >= 0.0)
          return;
        this.swimTimer = 800f;
        if (!location.Equals((object) Game1.currentLocation))
          return;
        Game1.playSound("slosh");
        location.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.animations, new Microsoft.Xna.Framework.Rectangle(0, 0, Game1.tileSize, Game1.tileSize), (float) (150.0 - ((double) Math.Abs(this.xVelocity) + (double) Math.Abs(this.yVelocity)) * 3.0), 8, 0, new Vector2(this.position.X, (float) (this.getStandingY() - Game1.tileSize / 2)), false, Game1.random.NextDouble() < 0.5, 0.01f, 0.01f, Color.White, 1f, 3f / 1000f, 0.0f, 0.0f, false));
      }
      else
      {
        if ((double) this.swimTimer >= 0.0)
          return;
        this.swimTimer = 100f;
      }
    }

    public virtual void performTenMinuteUpdate(int timeOfDay, GameLocation l)
    {
      if (Game1.random.NextDouble() < 0.1 && this.dialogue != null && this.dialogue.ContainsKey(l.name + "_Ambient"))
      {
        string[] strArray = this.dialogue[l.name + "_Ambient"].Split('/');
        int preTimer = Game1.random.Next(4) * 1000;
        this.showTextAboveHead(strArray[Game1.random.Next(strArray.Length)], -1, 2, 3000, preTimer);
      }
      else
      {
        if (!this.isMoving() || !l.isOutdoors || timeOfDay >= 1800 || (Game1.random.NextDouble() >= 0.3 + (this.socialAnxiety == 0 ? 0.25 : (this.socialAnxiety == 1 ? (this.manners == 2 ? -1.0 : -0.2) : 0.0)) || this.age == 1 && (this.manners != 1 || this.socialAnxiety != 0) || this.isMarried()))
          return;
        Character c = Utility.isThereAFarmerOrCharacterWithinDistance(this.getTileLocation(), 4, l);
        if (c.name.Equals(this.name) || c is Horse)
          return;
        Dictionary<string, string> dictionary = Game1.temporaryContent.Load<Dictionary<string, string>>("Data\\NPCDispositions");
        if (!dictionary.ContainsKey(this.name))
          return;
        if (dictionary[this.name].Split('/')[9].Contains(c.name) || !this.isFacingToward(c.getTileLocation()))
          return;
        this.sayHiTo(c);
      }
    }

    public void sayHiTo(Character c)
    {
      if (this.getHi(c.displayName) == null)
        return;
      this.showTextAboveHead(this.getHi(c.displayName), -1, 2, 3000, 0);
      if ((object) (c as NPC) == null || Game1.random.NextDouble() >= 0.66 || (c as NPC).getHi(this.displayName) == null)
        return;
      (c as NPC).showTextAboveHead((c as NPC).getHi(this.displayName), -1, 2, 3000, 1000 + Game1.random.Next(500));
    }

    public string getHi(string nameToGreet)
    {
      if (this.age == 2)
      {
        if (this.socialAnxiety != 1)
          return Game1.content.LoadString("Strings\\StringsFromCSFiles:NPC.cs.4059");
        return Game1.content.LoadString("Strings\\StringsFromCSFiles:NPC.cs.4058");
      }
      if (this.socialAnxiety == 1)
      {
        if (Game1.random.NextDouble() >= 0.5)
          return Game1.content.LoadString("Strings\\StringsFromCSFiles:NPC.cs.4061");
        return Game1.content.LoadString("Strings\\StringsFromCSFiles:NPC.cs.4060");
      }
      if (this.socialAnxiety == 0)
      {
        if (Game1.random.NextDouble() < 0.33)
          return Game1.content.LoadString("Strings\\StringsFromCSFiles:NPC.cs.4062");
        if (Game1.random.NextDouble() >= 0.5)
          return Game1.content.LoadString("Strings\\StringsFromCSFiles:NPC.cs.4068", (object) nameToGreet);
        return (Game1.timeOfDay < 1200 ? Game1.content.LoadString("Strings\\StringsFromCSFiles:NPC.cs.4063") : (Game1.timeOfDay < 1700 ? Game1.content.LoadString("Strings\\StringsFromCSFiles:NPC.cs.4064") : Game1.content.LoadString("Strings\\StringsFromCSFiles:NPC.cs.4065"))) + ", " + Game1.content.LoadString("Strings\\StringsFromCSFiles:NPC.cs.4066", (object) nameToGreet);
      }
      if (Game1.random.NextDouble() < 0.33)
        return Game1.content.LoadString("Strings\\StringsFromCSFiles:NPC.cs.4060");
      if (Game1.random.NextDouble() >= 0.5)
        return Game1.content.LoadString("Strings\\StringsFromCSFiles:NPC.cs.4072");
      return Game1.content.LoadString("Strings\\StringsFromCSFiles:NPC.cs.4071", (object) nameToGreet);
    }

    public bool isFacingToward(Vector2 tileLocation)
    {
      switch (this.facingDirection)
      {
        case 0:
          return (double) this.getTileY() > (double) tileLocation.Y;
        case 1:
          return (double) this.getTileX() < (double) tileLocation.X;
        case 2:
          return (double) this.getTileY() < (double) tileLocation.Y;
        case 3:
          return (double) this.getTileX() > (double) tileLocation.X;
        default:
          return false;
      }
    }

    public void arriveAt(GameLocation l)
    {
      if (Game1.random.NextDouble() >= 0.5 || this.dialogue == null || !this.dialogue.ContainsKey(l.name + "_Entry"))
        return;
      string[] strArray = this.dialogue[l.name + "_Entry"].Split('/');
      this.showTextAboveHead(strArray[Game1.random.Next(strArray.Length)], -1, 2, 3000, 0);
    }

    public override void Halt()
    {
      base.Halt();
      this.isCharging = false;
      this.speed = 2;
      this.addedSpeed = 0;
    }

    public void addExtraDialogues(string dialogues)
    {
      if (this.updatedDialogueYet)
      {
        if (dialogues == null)
          return;
        this.currentDialogue.Push(new StardewValley.Dialogue(dialogues, this));
      }
      else
        this.extraDialogueMessageToAddThisMorning = dialogues;
    }

    public string tryToGetMarriageSpecificDialogueElseReturnDefault(string dialogueKey, string defaultMessage = "")
    {
      Dictionary<string, string> dictionary1 = (Dictionary<string, string>) null;
      try
      {
        dictionary1 = Game1.content.Load<Dictionary<string, string>>("Characters\\Dialogue\\MarriageDialogue" + this.name);
      }
      catch (Exception ex)
      {
      }
      if (dictionary1 != null && dictionary1.ContainsKey(dialogueKey))
        return dictionary1[dialogueKey];
      Dictionary<string, string> dictionary2 = Game1.content.Load<Dictionary<string, string>>("Characters\\Dialogue\\MarriageDialogue");
      if (dictionary2 != null && dictionary2.ContainsKey(dialogueKey))
        return dictionary2[dialogueKey];
      return defaultMessage;
    }

    public void updateDialogue()
    {
      this.updatedDialogueYet = true;
      int[] numArray;
      int heartLevel = Game1.player.friendships.TryGetValue(this.name, out numArray) ? numArray[0] / 250 : 0;
      if (this.currentDialogue == null)
        this.currentDialogue = new Stack<StardewValley.Dialogue>();
      Random random = new Random((int) Game1.stats.DaysPlayed * 77 + (int) Game1.uniqueIDForThisGame / 2 + 2 + (int) this.defaultPosition.X * 77 + (int) this.defaultPosition.Y * 777);
      if (random.NextDouble() < 0.025 && heartLevel >= 1)
      {
        Dictionary<string, string> dictionary1 = Game1.content.Load<Dictionary<string, string>>("Data\\NPCDispositions");
        string str1;
        if (dictionary1.TryGetValue(this.name, out str1))
        {
          string[] strArray1 = str1.Split('/')[9].Split(' ');
          if (strArray1.Length > 1)
          {
            int index1 = random.Next(strArray1.Length / 2) * 2;
            string index2 = strArray1[index1];
            string str2 = index2;
            if (LocalizedContentManager.CurrentLanguageCode != LocalizedContentManager.LanguageCode.en && Game1.getCharacterFromName(index2, false) != null)
              str2 = Game1.getCharacterFromName(index2, false).displayName;
            string str3 = strArray1[index1 + 1].Replace("'", "").Replace("_", " ");
            string str4;
            int num;
            if (dictionary1.TryGetValue(index2, out str4))
              num = str4.Split('/')[4].Equals("male") ? 1 : 0;
            else
              num = 0;
            bool flag = num != 0;
            Dictionary<string, string> dictionary2 = Game1.content.Load<Dictionary<string, string>>("Data\\NPCGiftTastes");
            if (dictionary2.ContainsKey(index2))
            {
              string str5 = (string) null;
              string str6;
              if (str3.Length <= 2 || LocalizedContentManager.CurrentLanguageCode == LocalizedContentManager.LanguageCode.ja)
                str6 = str2;
              else if (!flag)
                str6 = Game1.LoadStringByGender(this.gender, "Strings\\StringsFromCSFiles:NPC.cs.4080", (object) str3);
              else
                str6 = Game1.LoadStringByGender(this.gender, "Strings\\StringsFromCSFiles:NPC.cs.4079", (object) str3);
              string str7 = str6;
              string masterDialogue = Game1.content.LoadString("Strings\\StringsFromCSFiles:NPC.cs.4083", (object) str7);
              if (random.NextDouble() < 0.5)
              {
                string[] strArray2 = dictionary2[index2].Split('/')[1].Split(' ');
                int int32 = Convert.ToInt32(strArray2[random.Next(strArray2.Length)]);
                string str8;
                if (Game1.objectInformation.TryGetValue(int32, out str8))
                {
                  str5 = str8.Split('/')[4];
                  masterDialogue += Game1.content.LoadString("Strings\\StringsFromCSFiles:NPC.cs.4084", (object) str5);
                  if (this.age == 2)
                  {
                    masterDialogue = Game1.content.LoadString("Strings\\StringsFromCSFiles:NPC.cs.4086", (object) str2, (object) str5) + (flag ? Game1.LoadStringByGender(this.gender, "Strings\\StringsFromCSFiles:NPC.cs.4088") : Game1.LoadStringByGender(this.gender, "Strings\\StringsFromCSFiles:NPC.cs.4089"));
                  }
                  else
                  {
                    switch (random.Next(5))
                    {
                      case 0:
                        masterDialogue = Game1.LoadStringByGender(this.gender, "Strings\\StringsFromCSFiles:NPC.cs.4091", (object) str7, (object) str5);
                        break;
                      case 1:
                        string str9;
                        if (!flag)
                          str9 = Game1.content.LoadString("Strings\\StringsFromCSFiles:NPC.cs.4097", (object) str7, (object) str5);
                        else
                          str9 = Game1.content.LoadString("Strings\\StringsFromCSFiles:NPC.cs.4094", (object) str7, (object) str5);
                        masterDialogue = str9;
                        break;
                      case 2:
                        string str10;
                        if (!flag)
                          str10 = Game1.content.LoadString("Strings\\StringsFromCSFiles:NPC.cs.4103", (object) str7, (object) str5);
                        else
                          str10 = Game1.content.LoadString("Strings\\StringsFromCSFiles:NPC.cs.4100", (object) str7, (object) str5);
                        masterDialogue = str10;
                        break;
                      case 3:
                        masterDialogue = Game1.content.LoadString("Strings\\StringsFromCSFiles:NPC.cs.4106", (object) str7, (object) str5);
                        break;
                    }
                    if (random.NextDouble() < 0.65)
                    {
                      switch (random.Next(5))
                      {
                        case 0:
                          masterDialogue += flag ? Game1.content.LoadString("Strings\\StringsFromCSFiles:NPC.cs.4109") : Game1.content.LoadString("Strings\\StringsFromCSFiles:NPC.cs.4111");
                          break;
                        case 1:
                          masterDialogue += flag ? (random.NextDouble() < 0.5 ? Game1.LoadStringByGender(this.gender, "Strings\\StringsFromCSFiles:NPC.cs.4113") : Game1.content.LoadString("Strings\\StringsFromCSFiles:NPC.cs.4114")) : (random.NextDouble() < 0.5 ? Game1.LoadStringByGender(this.gender, "Strings\\StringsFromCSFiles:NPC.cs.4115") : Game1.content.LoadString("Strings\\StringsFromCSFiles:NPC.cs.4116"));
                          break;
                        case 2:
                          masterDialogue += flag ? Game1.content.LoadString("Strings\\StringsFromCSFiles:NPC.cs.4118") : Game1.content.LoadString("Strings\\StringsFromCSFiles:NPC.cs.4120");
                          break;
                        case 3:
                          masterDialogue += Game1.content.LoadString("Strings\\StringsFromCSFiles:NPC.cs.4125");
                          break;
                        case 4:
                          masterDialogue += flag ? Game1.content.LoadString("Strings\\StringsFromCSFiles:NPC.cs.4126") : Game1.content.LoadString("Strings\\StringsFromCSFiles:NPC.cs.4128");
                          break;
                      }
                      if (index2.Equals("Abigail") && random.NextDouble() < 0.5)
                        masterDialogue = Game1.content.LoadString("Strings\\StringsFromCSFiles:NPC.cs.4128", (object) str2, (object) str5);
                    }
                  }
                }
              }
              else
              {
                int int32;
                try
                {
                  int32 = Convert.ToInt32(dictionary2[index2].Split('/')[7].Split(' ')[random.Next(dictionary2[index2].Split('/')[7].Split(' ').Length)]);
                }
                catch (Exception ex)
                {
                  int32 = Convert.ToInt32(dictionary2["Universal_Hate"].Split(' ')[random.Next(dictionary2["Universal_Hate"].Split(' ').Length)]);
                }
                if (Game1.objectInformation.ContainsKey(int32))
                {
                  str5 = Game1.objectInformation[int32].Split('/')[4];
                  string str8 = masterDialogue;
                  string str9;
                  if (!flag)
                    str9 = Game1.content.LoadString("Strings\\StringsFromCSFiles:NPC.cs.4138", (object) str5, (object) Lexicon.getRandomNegativeFoodAdjective((NPC) null));
                  else
                    str9 = Game1.content.LoadString("Strings\\StringsFromCSFiles:NPC.cs.4135", (object) str5, (object) Lexicon.getRandomNegativeFoodAdjective((NPC) null));
                  masterDialogue = str8 + str9;
                  if (this.age == 2)
                  {
                    string str10;
                    if (!flag)
                      str10 = Game1.LoadStringByGender(this.gender, "Strings\\StringsFromCSFiles:NPC.cs.4144", (object) str2, (object) str5);
                    else
                      str10 = Game1.LoadStringByGender(this.gender, "Strings\\StringsFromCSFiles:NPC.cs.4141", (object) str2, (object) str5);
                    masterDialogue = str10;
                  }
                  else
                  {
                    switch (random.Next(4))
                    {
                      case 0:
                        masterDialogue = (random.NextDouble() < 0.5 ? Game1.LoadStringByGender(this.gender, "Strings\\StringsFromCSFiles:NPC.cs.4146") : "") + Game1.LoadStringByGender(this.gender, "Strings\\StringsFromCSFiles:NPC.cs.4147", (object) str7, (object) str5);
                        break;
                      case 1:
                        string str10;
                        if (!flag)
                        {
                          if (random.NextDouble() >= 0.5)
                            str10 = Game1.LoadStringByGender(this.gender, "Strings\\StringsFromCSFiles:NPC.cs.4154", (object) str7, (object) str5);
                          else
                            str10 = Game1.LoadStringByGender(this.gender, "Strings\\StringsFromCSFiles:NPC.cs.4153", (object) str7, (object) str5);
                        }
                        else if (random.NextDouble() >= 0.5)
                          str10 = Game1.LoadStringByGender(this.gender, "Strings\\StringsFromCSFiles:NPC.cs.4152", (object) str7, (object) str5);
                        else
                          str10 = Game1.LoadStringByGender(this.gender, "Strings\\StringsFromCSFiles:NPC.cs.4149", (object) str7, (object) str5);
                        masterDialogue = str10;
                        break;
                      case 2:
                        string str11;
                        if (!flag)
                          str11 = Game1.content.LoadString("Strings\\StringsFromCSFiles:NPC.cs.4164", (object) str7, (object) str5);
                        else
                          str11 = Game1.content.LoadString("Strings\\StringsFromCSFiles:NPC.cs.4161", (object) str7, (object) str5);
                        masterDialogue = str11;
                        break;
                    }
                    if (random.NextDouble() < 0.65)
                    {
                      switch (random.Next(5))
                      {
                        case 0:
                          masterDialogue += Game1.content.LoadString("Strings\\StringsFromCSFiles:NPC.cs.4170");
                          break;
                        case 1:
                          masterDialogue += Game1.content.LoadString("Strings\\StringsFromCSFiles:NPC.cs.4171");
                          break;
                        case 2:
                          masterDialogue += flag ? Game1.content.LoadString("Strings\\StringsFromCSFiles:NPC.cs.4172") : Game1.content.LoadString("Strings\\StringsFromCSFiles:NPC.cs.4174");
                          break;
                        case 3:
                          masterDialogue += flag ? Game1.content.LoadString("Strings\\StringsFromCSFiles:NPC.cs.4176") : Game1.content.LoadString("Strings\\StringsFromCSFiles:NPC.cs.4178");
                          break;
                        case 4:
                          masterDialogue += Game1.content.LoadString("Strings\\StringsFromCSFiles:NPC.cs.4180");
                          break;
                      }
                      if (this.name.Equals("Lewis") && random.NextDouble() < 0.5)
                        masterDialogue = Game1.content.LoadString("Strings\\StringsFromCSFiles:NPC.cs.4182", (object) str2, (object) str5);
                    }
                  }
                }
              }
              if (str5 != null)
              {
                this.currentDialogue.Clear();
                if (masterDialogue.Length > 0)
                {
                  try
                  {
                    masterDialogue = masterDialogue.Substring(0, 1).ToUpper() + masterDialogue.Substring(1, masterDialogue.Length - 1);
                  }
                  catch (Exception ex)
                  {
                  }
                }
                this.currentDialogue.Push(new StardewValley.Dialogue(masterDialogue, this));
                return;
              }
            }
          }
        }
      }
      if (this.dialogue != null)
      {
        string masterDialogue = "";
        this.currentDialogue.Clear();
        if (Game1.player.spouse != null && Game1.player.spouse.Contains(this.name))
        {
          if (Game1.player.spouse.Equals(this.name + "engaged"))
            this.currentDialogue.Push(new StardewValley.Dialogue(Game1.content.Load<Dictionary<string, string>>("Data\\EngagementDialogue")[this.name + (object) random.Next(2)], this));
          else if (Game1.isRaining)
            this.currentDialogue.Push(new StardewValley.Dialogue(this.tryToGetMarriageSpecificDialogueElseReturnDefault("Rainy_Day_" + (object) random.Next(5), ""), this));
          else
            this.currentDialogue.Push(new StardewValley.Dialogue(this.tryToGetMarriageSpecificDialogueElseReturnDefault("Indoor_Day_" + (object) random.Next(5), ""), this));
        }
        else if (this.idForClones == -1)
        {
          if (this.divorcedFromFarmer)
          {
            try
            {
              this.currentDialogue.Push(new StardewValley.Dialogue(Game1.content.Load<Dictionary<string, string>>("Characters\\Dialogue\\" + this.name)["divorced"], this));
              return;
            }
            catch (Exception ex)
            {
            }
          }
          if (Game1.isRaining)
          {
            if (random.NextDouble() < 0.5)
            {
              try
              {
                this.currentDialogue.Push(new StardewValley.Dialogue(Game1.content.Load<Dictionary<string, string>>("Characters\\Dialogue\\rainy")[this.name], this));
                return;
              }
              catch (Exception ex)
              {
              }
            }
          }
          this.currentDialogue.Push((this.tryToRetrieveDialogue(Game1.currentSeason + "_", heartLevel, "") ?? this.tryToRetrieveDialogue("", heartLevel, "")) ?? new StardewValley.Dialogue(Game1.content.LoadString("Strings\\StringsFromCSFiles:NPC.cs.4061"), this));
        }
        else
        {
          this.dialogue.TryGetValue(string.Concat((object) this.idForClones), out masterDialogue);
          this.currentDialogue.Push(new StardewValley.Dialogue(masterDialogue, this));
        }
      }
      else if (this.name.Equals("Bouncer"))
        this.currentDialogue.Push(new StardewValley.Dialogue(Game1.content.LoadString("Strings\\StringsFromCSFiles:NPC.cs.4192"), this));
      if (this.extraDialogueMessageToAddThisMorning == null)
        return;
      this.currentDialogue.Push(new StardewValley.Dialogue(this.extraDialogueMessageToAddThisMorning, this));
    }

    public bool checkForNewCurrentDialogue(int heartLevel, bool noPreface = false)
    {
      string index = "";
      foreach (string key in Game1.player.activeDialogueEvents.Keys)
      {
        if (this.dialogue.ContainsKey(key))
        {
          index = key;
          break;
        }
      }
      string str = Game1.currentSeason.Equals("spring") || noPreface ? "" : Game1.currentSeason;
      if (!index.Equals("") && !Game1.player.mailReceived.Contains(this.name + "_" + index))
      {
        this.currentDialogue.Clear();
        this.currentDialogue.Push(new StardewValley.Dialogue(this.dialogue[index], this));
        Game1.player.mailReceived.Add(this.name + "_" + index);
        return true;
      }
      if (this.dialogue.ContainsKey(str + Game1.currentLocation.name + "_" + (object) this.getTileX() + "_" + (object) this.getTileY()))
      {
        this.currentDialogue.Push(new StardewValley.Dialogue(this.dialogue[str + Game1.currentLocation.name + "_" + (object) this.getTileX() + "_" + (object) this.getTileY()], this));
        return true;
      }
      if (this.dialogue.ContainsKey(str + Game1.currentLocation.name + "_" + Game1.shortDayNameFromDayOfSeason(Game1.dayOfMonth)))
      {
        this.currentDialogue.Push(new StardewValley.Dialogue(this.dialogue[str + Game1.currentLocation.name + "_" + Game1.shortDayNameFromDayOfSeason(Game1.dayOfMonth)], this));
        return true;
      }
      if (heartLevel >= 10 && this.dialogue.ContainsKey(str + Game1.currentLocation.name + "10"))
      {
        this.currentDialogue.Push(new StardewValley.Dialogue(this.dialogue[str + Game1.currentLocation.name + "10"], this));
        return true;
      }
      if (heartLevel >= 8 && this.dialogue.ContainsKey(str + Game1.currentLocation.name + "8"))
      {
        this.currentDialogue.Push(new StardewValley.Dialogue(this.dialogue[str + Game1.currentLocation.name + "8"], this));
        return true;
      }
      if (heartLevel >= 6 && this.dialogue.ContainsKey(str + Game1.currentLocation.name + "6"))
      {
        this.currentDialogue.Push(new StardewValley.Dialogue(this.dialogue[str + Game1.currentLocation.name + "6"], this));
        return true;
      }
      if (heartLevel >= 4 && this.dialogue.ContainsKey(str + Game1.currentLocation.name + "4"))
      {
        this.currentDialogue.Push(new StardewValley.Dialogue(this.dialogue[str + Game1.currentLocation.name + "4"], this));
        return true;
      }
      if (heartLevel >= 2 && this.dialogue.ContainsKey(str + Game1.currentLocation.name + "2"))
      {
        this.currentDialogue.Push(new StardewValley.Dialogue(this.dialogue[str + Game1.currentLocation.name + "2"], this));
        return true;
      }
      if (!this.dialogue.ContainsKey(str + Game1.currentLocation.name))
        return false;
      this.currentDialogue.Push(new StardewValley.Dialogue(this.dialogue[str + Game1.currentLocation.name], this));
      return true;
    }

    public StardewValley.Dialogue tryToRetrieveDialogue(string preface, int heartLevel, string appendToEnd = "")
    {
      int num = Game1.year;
      if (Game1.year > 2)
        num = 2;
      if (Game1.player.spouse != null && Game1.player.spouse.Length > 0 && appendToEnd.Equals(""))
      {
        StardewValley.Dialogue retrieveDialogue = this.tryToRetrieveDialogue(preface, heartLevel, "_inlaw_" + Game1.player.spouse);
        if (retrieveDialogue != null)
          return retrieveDialogue;
      }
      if (this.dialogue.ContainsKey(preface + (object) Game1.dayOfMonth + appendToEnd) && num == 1)
        return new StardewValley.Dialogue(this.dialogue[preface + (object) Game1.dayOfMonth + appendToEnd], this);
      if (this.dialogue.ContainsKey(preface + (object) Game1.dayOfMonth + "_" + (object) num + appendToEnd))
        return new StardewValley.Dialogue(this.dialogue[preface + (object) Game1.dayOfMonth + "_" + (object) num + appendToEnd], this);
      if (heartLevel >= 10 && this.dialogue.ContainsKey(preface + Game1.shortDayNameFromDayOfSeason(Game1.dayOfMonth) + "10" + appendToEnd))
      {
        if (!this.dialogue.ContainsKey(preface + Game1.shortDayNameFromDayOfSeason(Game1.dayOfMonth) + "10_" + (object) num + appendToEnd))
          return new StardewValley.Dialogue(this.dialogue[preface + Game1.shortDayNameFromDayOfSeason(Game1.dayOfMonth) + "10" + appendToEnd], this);
        return new StardewValley.Dialogue(this.dialogue[preface + Game1.shortDayNameFromDayOfSeason(Game1.dayOfMonth) + "10_" + (object) num + appendToEnd], this);
      }
      if (heartLevel >= 8 && this.dialogue.ContainsKey(preface + Game1.shortDayNameFromDayOfSeason(Game1.dayOfMonth) + "8" + appendToEnd))
      {
        if (!this.dialogue.ContainsKey(preface + Game1.shortDayNameFromDayOfSeason(Game1.dayOfMonth) + "8_" + (object) num + appendToEnd))
          return new StardewValley.Dialogue(this.dialogue[preface + Game1.shortDayNameFromDayOfSeason(Game1.dayOfMonth) + "8" + appendToEnd], this);
        return new StardewValley.Dialogue(this.dialogue[preface + Game1.shortDayNameFromDayOfSeason(Game1.dayOfMonth) + "8_" + (object) num + appendToEnd], this);
      }
      if (heartLevel >= 6 && this.dialogue.ContainsKey(preface + Game1.shortDayNameFromDayOfSeason(Game1.dayOfMonth) + "6" + appendToEnd))
      {
        if (!this.dialogue.ContainsKey(preface + Game1.shortDayNameFromDayOfSeason(Game1.dayOfMonth) + "6_" + (object) num))
          return new StardewValley.Dialogue(this.dialogue[preface + Game1.shortDayNameFromDayOfSeason(Game1.dayOfMonth) + "6" + appendToEnd], this);
        return new StardewValley.Dialogue(this.dialogue[preface + Game1.shortDayNameFromDayOfSeason(Game1.dayOfMonth) + "6_" + (object) num + appendToEnd], this);
      }
      if (heartLevel >= 4 && this.dialogue.ContainsKey(preface + Game1.shortDayNameFromDayOfSeason(Game1.dayOfMonth) + "4" + appendToEnd))
      {
        if (!this.dialogue.ContainsKey(preface + Game1.shortDayNameFromDayOfSeason(Game1.dayOfMonth) + "4_" + (object) num))
          return new StardewValley.Dialogue(this.dialogue[preface + Game1.shortDayNameFromDayOfSeason(Game1.dayOfMonth) + "4" + appendToEnd], this);
        return new StardewValley.Dialogue(this.dialogue[preface + Game1.shortDayNameFromDayOfSeason(Game1.dayOfMonth) + "4_" + (object) num + appendToEnd], this);
      }
      if (heartLevel >= 2 && this.dialogue.ContainsKey(preface + Game1.shortDayNameFromDayOfSeason(Game1.dayOfMonth) + "2" + appendToEnd))
      {
        if (!this.dialogue.ContainsKey(preface + Game1.shortDayNameFromDayOfSeason(Game1.dayOfMonth) + "2_" + (object) num))
          return new StardewValley.Dialogue(this.dialogue[preface + Game1.shortDayNameFromDayOfSeason(Game1.dayOfMonth) + "2" + appendToEnd], this);
        return new StardewValley.Dialogue(this.dialogue[preface + Game1.shortDayNameFromDayOfSeason(Game1.dayOfMonth) + "2_" + (object) num + appendToEnd], this);
      }
      if (!this.dialogue.ContainsKey(preface + Game1.shortDayNameFromDayOfSeason(Game1.dayOfMonth) + appendToEnd))
        return (StardewValley.Dialogue) null;
      if (!this.dialogue.ContainsKey(preface + Game1.shortDayNameFromDayOfSeason(Game1.dayOfMonth) + "_" + (object) num + appendToEnd))
        return new StardewValley.Dialogue(this.dialogue[preface + Game1.shortDayNameFromDayOfSeason(Game1.dayOfMonth) + appendToEnd], this);
      return new StardewValley.Dialogue(this.dialogue[preface + Game1.shortDayNameFromDayOfSeason(Game1.dayOfMonth) + "_" + (object) num + appendToEnd], this);
    }

    public void checkSchedule(int timeOfDay)
    {
      this.updatedDialogueYet = false;
      this.extraDialogueMessageToAddThisMorning = (string) null;
      if (this.ignoreScheduleToday || this.schedule == null)
        return;
      SchedulePathDescription schedulePathDescription;
      this.schedule.TryGetValue(this.scheduleTimeToTry == 9999999 ? timeOfDay : this.scheduleTimeToTry, out schedulePathDescription);
      if (schedulePathDescription == null)
        return;
      if (!this.isMarried() && (!this.isWalkingInSquare || this.lastCrossroad.Center.X / Game1.tileSize != this.previousEndPoint.X && this.lastCrossroad.Y / Game1.tileSize != this.previousEndPoint.Y))
      {
        Point previousEndPoint = this.previousEndPoint;
        if (!this.previousEndPoint.Equals(Point.Zero) && !this.previousEndPoint.Equals(this.getTileLocationPoint()))
        {
          if (this.scheduleTimeToTry != 9999999)
            return;
          this.scheduleTimeToTry = timeOfDay;
          return;
        }
      }
      this.directionsToNewLocation = schedulePathDescription;
      this.prepareToDisembarkOnNewSchedulePath();
      if (this.schedule == null)
        return;
      if (this.directionsToNewLocation != null && this.directionsToNewLocation.route != null && this.directionsToNewLocation.route.Count > 0 && ((Math.Abs(this.getTileLocationPoint().X - this.directionsToNewLocation.route.Peek().X) > 1 || Math.Abs(this.getTileLocationPoint().Y - this.directionsToNewLocation.route.Peek().Y) > 1) && this.temporaryController == null))
      {
        this.scheduleTimeToTry = 9999999;
      }
      else
      {
        this.controller = new PathFindController(this.directionsToNewLocation.route, (Character) this, Utility.getGameLocationOfCharacter(this))
        {
          finalFacingDirection = this.directionsToNewLocation.facingDirection,
          endBehaviorFunction = this.getRouteEndBehaviorFunction(this.directionsToNewLocation.endOfRouteBehavior, this.directionsToNewLocation.endOfRouteMessage)
        };
        this.scheduleTimeToTry = 9999999;
        try
        {
          this.previousEndPoint = this.directionsToNewLocation.route.Count > 0 ? this.directionsToNewLocation.route.Last<Point>() : Point.Zero;
        }
        catch (Exception ex)
        {
        }
      }
    }

    private void prepareToDisembarkOnNewSchedulePath()
    {
      while (this.CurrentDialogue.Count > 0 && this.CurrentDialogue.Peek().removeOnNextMove)
        this.CurrentDialogue.Pop();
      this.nextEndOfRouteMessage = (string) null;
      this.endOfRouteMessage = (string) null;
      if (this.doingEndOfRouteAnimation)
      {
        List<FarmerSprite.AnimationFrame> animation = new List<FarmerSprite.AnimationFrame>();
        for (int index = 0; index < this.routeEndOutro.Length; ++index)
        {
          if (index == this.routeEndOutro.Length - 1)
            animation.Add(new FarmerSprite.AnimationFrame(this.routeEndOutro[index], 100, 0, false, false, new AnimatedSprite.endOfAnimationBehavior(this.routeEndAnimationFinished), true, 0));
          else
            animation.Add(new FarmerSprite.AnimationFrame(this.routeEndOutro[index], 100, 0, false, false, (AnimatedSprite.endOfAnimationBehavior) null, false, 0));
        }
        if (animation.Count > 0)
          this.sprite.setCurrentAnimation(animation);
        else
          this.routeEndAnimationFinished((Farmer) null);
        if (this.endOfRouteBehaviorName != null)
          this.finishRouteBehavior(this.endOfRouteBehaviorName);
      }
      else
        this.routeEndAnimationFinished((Farmer) null);
      if (!this.isMarried())
        return;
      if (this.temporaryController == null && Utility.getGameLocationOfCharacter(this) is FarmHouse)
      {
        this.temporaryController = new PathFindController((Character) this, this.getHome(), new Point(this.getHome().warps[0].X, this.getHome().warps[0].Y), 2, true)
        {
          NPCSchedule = true
        };
        if (this.temporaryController.pathToEndPoint == null || this.temporaryController.pathToEndPoint.Count <= 0)
        {
          this.temporaryController = (PathFindController) null;
          this.schedule = (Dictionary<int, SchedulePathDescription>) null;
        }
        else
          this.followSchedule = true;
      }
      else
      {
        if (!(Utility.getGameLocationOfCharacter(this) is Farm))
          return;
        this.temporaryController = (PathFindController) null;
        this.schedule = (Dictionary<int, SchedulePathDescription>) null;
      }
    }

    public void checkForMarriageDialogue(int timeOfDay, GameLocation location)
    {
      if (timeOfDay == 1100)
      {
        this.setRandomAfternoonMarriageDialogue(1100, location, true);
      }
      else
      {
        if (Game1.timeOfDay != 1800 || !(location is FarmHouse))
          return;
        Random random = new Random((int) Game1.uniqueIDForThisGame / 2 + (int) Game1.stats.DaysPlayed + timeOfDay);
        this.setNewDialogue("MarriageDialogue", (Game1.isRaining ? "Rainy" : "Indoor") + "_Night_", random.Next(6) - 1, false, false);
      }
    }

    private void routeEndAnimationFinished(Farmer who)
    {
      this.doingEndOfRouteAnimation = false;
      this.freezeMotion = false;
      this.sprite.spriteHeight = 32;
      this.sprite.StopAnimation();
      this.endOfRouteMessage = (string) null;
      this.isCharging = false;
      this.speed = 2;
      this.addedSpeed = 0;
      this.goingToDoEndOfRouteAnimation = false;
      if (!this.isWalkingInSquare)
        return;
      this.returningToEndPoint = true;
      this.timeAfterSquare = Game1.timeOfDay;
    }

    private void setMessageAtEndOfScheduleRoute(Character c, GameLocation l)
    {
      this.endOfRouteMessage = this.nextEndOfRouteMessage;
    }

    public bool isOnSilentTemporaryMessage()
    {
      return (this.doingEndOfRouteAnimation || !this.goingToDoEndOfRouteAnimation) && (this.endOfRouteMessage != null && this.endOfRouteMessage.ToLower().Equals("silent"));
    }

    public bool hasTemporaryMessageAvailable()
    {
      return this.endOfRouteMessage != null && (this.doingEndOfRouteAnimation || !this.goingToDoEndOfRouteAnimation);
    }

    public bool setTemporaryMessages(Farmer who)
    {
      if (this.isOnSilentTemporaryMessage())
        return true;
      if (this.endOfRouteMessage != null && (this.doingEndOfRouteAnimation || !this.goingToDoEndOfRouteAnimation))
        this.CurrentDialogue.Push(new StardewValley.Dialogue(this.endOfRouteMessage, this)
        {
          removeOnNextMove = true
        });
      return false;
    }

    private void walkInSquareAtEndOfRoute(Character c, GameLocation l)
    {
      this.startRouteBehavior(this.endOfRouteBehaviorName);
    }

    private void doAnimationAtEndOfScheduleRoute(Character c, GameLocation l)
    {
      List<FarmerSprite.AnimationFrame> animation = new List<FarmerSprite.AnimationFrame>();
      for (int index = 0; index < this.routeEndIntro.Length; ++index)
      {
        if (index == this.routeEndIntro.Length - 1)
          animation.Add(new FarmerSprite.AnimationFrame(this.routeEndIntro[index], 100, 0, false, false, new AnimatedSprite.endOfAnimationBehavior(this.doMiddleAnimation), true, 0));
        else
          animation.Add(new FarmerSprite.AnimationFrame(this.routeEndIntro[index], 100, 0, false, false, (AnimatedSprite.endOfAnimationBehavior) null, false, 0));
      }
      this.doingEndOfRouteAnimation = true;
      this.freezeMotion = true;
      this.sprite.setCurrentAnimation(animation);
    }

    private void doMiddleAnimation(Farmer who)
    {
      List<FarmerSprite.AnimationFrame> animation = new List<FarmerSprite.AnimationFrame>();
      for (int index = 0; index < this.routeEndAnimation.Length; ++index)
        animation.Add(new FarmerSprite.AnimationFrame(this.routeEndAnimation[index], 100, 0, false, false, (AnimatedSprite.endOfAnimationBehavior) null, false, 0));
      this.sprite.setCurrentAnimation(animation);
      this.sprite.loop = true;
      if (this.endOfRouteBehaviorName == null)
        return;
      this.startRouteBehavior(this.endOfRouteBehaviorName);
    }

    private void startRouteBehavior(string behaviorName)
    {
      if (behaviorName.Length > 0 && (int) behaviorName[0] == 34)
      {
        this.endOfRouteMessage = behaviorName.Replace("\"", "");
      }
      else
      {
        if (behaviorName.Contains("square_"))
        {
          this.lastCrossroad = new Microsoft.Xna.Framework.Rectangle(this.getTileX() * Game1.tileSize, this.getTileY() * Game1.tileSize, Game1.tileSize, Game1.tileSize);
          string[] strArray = behaviorName.Split('_');
          this.walkInSquare(Convert.ToInt32(strArray[1]), Convert.ToInt32(strArray[2]), 6000);
          this.squareMovementFacingPreference = strArray.Length <= 3 ? -1 : Convert.ToInt32(strArray[3]);
        }
        if (!(behaviorName == "abigail_videogames"))
        {
          if (!(behaviorName == "dick_fish"))
          {
            if (!(behaviorName == "clint_hammer"))
              return;
            this.extendSourceRect(16, 0, true);
            this.sprite.spriteWidth = 32;
            this.sprite.ignoreSourceRectUpdates = false;
            this.sprite.CurrentFrame = 8;
            this.sprite.currentAnimation[14] = new FarmerSprite.AnimationFrame(9, 100, 0, false, false, new AnimatedSprite.endOfAnimationBehavior(this.clintHammerSound), false, 0);
          }
          else
          {
            this.extendSourceRect(0, 32, true);
            if (!Utility.isOnScreen(Utility.Vector2ToPoint(this.position), Game1.tileSize, this.currentLocation))
              return;
            Game1.playSound("slosh");
          }
        }
        else
        {
          Utility.getGameLocationOfCharacter(this).temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(167, 1714, 19, 14), 100f, 3, 999999, new Vector2(2f, 3f) * (float) Game1.tileSize + new Vector2(7f, 12f) * (float) Game1.pixelZoom, false, false, 0.0002f, 0.0f, Color.White, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false)
          {
            id = 688f
          });
          this.doEmote(52, true);
        }
      }
    }

    private void finishRouteBehavior(string behaviorName)
    {
      if (!(behaviorName == "abigail_videogames"))
      {
        if (!(behaviorName == "clint_hammer") && !(behaviorName == "dick_fish"))
          return;
        this.reloadSprite();
        this.sprite.spriteWidth = 16;
        this.sprite.spriteHeight = 32;
        this.sprite.UpdateSourceRect();
        this.Halt();
        this.movementPause = 1;
      }
      else
        Utility.getGameLocationOfCharacter(this).removeTemporarySpritesWithID(688);
    }

    private PathFindController.endBehavior getRouteEndBehaviorFunction(string behaviorName, string endMessage)
    {
      if (endMessage != null || behaviorName != null && behaviorName.Length > 0 && (int) behaviorName[0] == 34)
        this.nextEndOfRouteMessage = endMessage.Replace("\"", "");
      if (behaviorName == null)
        return (PathFindController.endBehavior) null;
      if (behaviorName.Length > 0 && behaviorName.Contains("square_"))
      {
        this.endOfRouteBehaviorName = behaviorName;
        return new PathFindController.endBehavior(this.walkInSquareAtEndOfRoute);
      }
      Dictionary<string, string> dictionary = Game1.content.Load<Dictionary<string, string>>("Data\\animationDescriptions");
      if (!dictionary.ContainsKey(behaviorName))
        return (PathFindController.endBehavior) null;
      this.endOfRouteBehaviorName = behaviorName;
      string[] strArray = dictionary[behaviorName].Split('/');
      this.routeEndIntro = Utility.parseStringToIntArray(strArray[0], ' ');
      this.routeEndAnimation = Utility.parseStringToIntArray(strArray[1], ' ');
      this.routeEndOutro = Utility.parseStringToIntArray(strArray[2], ' ');
      if (strArray.Length > 3)
        this.nextEndOfRouteMessage = strArray[3];
      this.goingToDoEndOfRouteAnimation = true;
      return new PathFindController.endBehavior(this.doAnimationAtEndOfScheduleRoute);
    }

    public void warp(bool wasOutdoors)
    {
    }

    public void shake(int duration)
    {
      this.shakeTimer = duration;
    }

    public void setNewDialogue(string s, bool add = false, bool clearOnMovement = false)
    {
      if (!add)
        this.CurrentDialogue.Clear();
      this.CurrentDialogue.Push(new StardewValley.Dialogue(s, this)
      {
        removeOnNextMove = clearOnMovement
      });
    }

    public void setNewDialogue(string dialogueSheetName, string dialogueSheetKey, int numberToAppend = -1, bool add = false, bool clearOnMovement = false)
    {
      if (!add)
        this.CurrentDialogue.Clear();
      string str = numberToAppend == -1 ? this.name : "";
      if (dialogueSheetName.Contains("Marriage"))
      {
        this.CurrentDialogue.Push(new StardewValley.Dialogue(this.tryToGetMarriageSpecificDialogueElseReturnDefault(dialogueSheetKey + (numberToAppend != -1 ? string.Concat((object) numberToAppend) : "") + str, ""), this)
        {
          removeOnNextMove = clearOnMovement
        });
      }
      else
      {
        if (!Game1.content.Load<Dictionary<string, string>>("Characters\\Dialogue\\" + dialogueSheetName).ContainsKey(dialogueSheetKey + (numberToAppend != -1 ? string.Concat((object) numberToAppend) : "") + str))
          return;
        this.CurrentDialogue.Push(new StardewValley.Dialogue(Game1.content.Load<Dictionary<string, string>>("Characters\\Dialogue\\" + dialogueSheetName)[dialogueSheetKey + (numberToAppend != -1 ? string.Concat((object) numberToAppend) : "") + str], this)
        {
          removeOnNextMove = clearOnMovement
        });
      }
    }

    public void setSpouseRoomMarriageDialogue()
    {
      this.setNewDialogue("MarriageDialogue", "spouseRoom_", -1, false, true);
    }

    public void setRandomAfternoonMarriageDialogue(int time, GameLocation location, bool countAsDailyAfternoon = false)
    {
      if (this.hasSaidAfternoonDialogue || this.getSpouse() == null)
        return;
      if (countAsDailyAfternoon)
        this.hasSaidAfternoonDialogue = true;
      Random random = new Random((int) Game1.uniqueIDForThisGame / 2 + (int) Game1.stats.DaysPlayed + time);
      int heartLevelForNpc = this.getSpouse().getFriendshipHeartLevelForNPC(this.name);
      if (location is FarmHouse && random.NextDouble() < 0.5)
      {
        if (heartLevelForNpc < 9)
          this.setNewDialogue("MarriageDialogue", random.NextDouble() < (double) heartLevelForNpc / 11.0 ? "Neutral_" : "Bad_", random.Next(10), false, false);
        else if (random.NextDouble() < 0.05)
          this.setNewDialogue("MarriageDialogue", Game1.currentSeason + "_", -1, false, false);
        else if (heartLevelForNpc >= 10 && random.NextDouble() < 0.5 || heartLevelForNpc >= 11 && random.NextDouble() < 0.75)
          this.setNewDialogue("MarriageDialogue", "Good_", random.Next(10), false, false);
        else
          this.setNewDialogue("MarriageDialogue", "Neutral_", random.Next(10), false, false);
      }
      else
      {
        if (!(location is Farm))
          return;
        if (random.NextDouble() < 0.2)
          this.setNewDialogue("MarriageDialogue", "Outdoor_", -1, false, false);
        else
          this.setNewDialogue("MarriageDialogue", "Outdoor_", random.Next(5), false, false);
      }
    }

    public bool isBirthday(string season, int day)
    {
      return this.birthday_Season != null && this.birthday_Season.Equals(season) && this.birthday_Day == day;
    }

    public Object getFavoriteItem()
    {
      string str;
      Game1.NPCGiftTastes.TryGetValue(this.name, out str);
      if (str == null)
        return (Object) null;
      return new Object(Convert.ToInt32(str.Split('/')[1].Split(' ')[0]), 1, false, -1, 0);
    }

    public void receiveGift(Object o, Farmer giver, bool updateGiftLimitInfo = true, float friendshipChangeMultiplier = 1f, bool showResponse = true)
    {
      string str1;
      Game1.NPCGiftTastes.TryGetValue(this.name, out str1);
      string[] strArray = str1.Split('/');
      float num = 1f;
      switch (o.quality)
      {
        case 1:
          num = 1.1f;
          break;
        case 2:
          num = 1.25f;
          break;
        case 4:
          num = 1.5f;
          break;
      }
      if ((this.birthday_Season == null || !Game1.currentSeason.Equals(this.birthday_Season) ? 0 : (Game1.dayOfMonth == this.birthday_Day ? 1 : 0)) != 0)
      {
        friendshipChangeMultiplier = 8f;
        string str2 = this.manners == 2 ? Game1.LoadStringByGender(this.gender, "Strings\\StringsFromCSFiles:NPC.cs.4274") : Game1.content.LoadString("Strings\\StringsFromCSFiles:NPC.cs.4275");
        if (Game1.random.NextDouble() < 0.5)
          str2 = this.manners == 2 ? Game1.LoadStringByGender(this.gender, "Strings\\StringsFromCSFiles:NPC.cs.4276") : Game1.LoadStringByGender(this.gender, "Strings\\StringsFromCSFiles:NPC.cs.4277");
        string str3 = this.manners == 2 ? Game1.LoadStringByGender(this.gender, "Strings\\StringsFromCSFiles:NPC.cs.4278") : Game1.LoadStringByGender(this.gender, "Strings\\StringsFromCSFiles:NPC.cs.4279");
        strArray[0] = str2;
        strArray[2] = str2;
        strArray[4] = str3;
        strArray[6] = str3;
        strArray[8] = this.manners == 2 ? Game1.content.LoadString("Strings\\StringsFromCSFiles:NPC.cs.4280") : Game1.LoadStringByGender(this.gender, "Strings\\StringsFromCSFiles:NPC.cs.4281");
      }
      if (str1 == null)
        return;
      ++Game1.stats.GiftsGiven;
      Game1.playSound("give_gift");
      if (updateGiftLimitInfo)
        giver.friendships[this.name][3] = 1;
      int tasteForThisItem = this.getGiftTasteForThisItem((Item) o);
      switch (giver.facingDirection)
      {
        case 0:
          ((FarmerSprite) giver.Sprite).animateBackwardsOnce(80, 50f);
          break;
        case 1:
          ((FarmerSprite) giver.Sprite).animateBackwardsOnce(72, 50f);
          break;
        case 2:
          ((FarmerSprite) giver.Sprite).animateBackwardsOnce(64, 50f);
          break;
        case 3:
          ((FarmerSprite) giver.Sprite).animateBackwardsOnce(88, 50f);
          break;
      }
      if (updateGiftLimitInfo)
        ++giver.friendships[this.name][1];
      List<string> stringList = new List<string>();
      int index = 0;
      while (index < 8)
      {
        stringList.Add(strArray[index]);
        index += 2;
      }
      if (tasteForThisItem == 0)
      {
        if (this.name.Contains("Dwarf"))
        {
          if (showResponse)
            Game1.drawDialogue(this, giver.canUnderstandDwarves ? stringList[0] : StardewValley.Dialogue.convertToDwarvish(stringList[0]));
        }
        else if (showResponse)
          Game1.drawDialogue(this, stringList[0] + "$h");
        giver.changeFriendship((int) (80.0 * (double) friendshipChangeMultiplier * (double) num), this);
        this.doEmote(20, true);
        this.faceTowardFarmerForPeriod(15000, 4, false, giver);
      }
      else if (tasteForThisItem == 6)
      {
        if (this.name.Contains("Dwarf"))
        {
          if (showResponse)
            Game1.drawDialogue(this, giver.canUnderstandDwarves ? stringList[3] : StardewValley.Dialogue.convertToDwarvish(stringList[3]));
        }
        else if (showResponse)
          Game1.drawDialogue(this, stringList[3] + "$s");
        giver.changeFriendship((int) (-40.0 * (double) friendshipChangeMultiplier), this);
        this.faceTowardFarmerForPeriod(15000, 4, true, giver);
        this.doEmote(12, true);
      }
      else if (tasteForThisItem == 2)
      {
        if (this.name.Contains("Dwarf"))
        {
          if (showResponse)
            Game1.drawDialogue(this, giver.canUnderstandDwarves ? stringList[1] : StardewValley.Dialogue.convertToDwarvish(stringList[1]));
        }
        else if (showResponse)
          Game1.drawDialogue(this, stringList[1] + "$h");
        giver.changeFriendship((int) (45.0 * (double) friendshipChangeMultiplier * (double) num), this);
        this.faceTowardFarmerForPeriod(7000, 3, true, giver);
      }
      else if (tasteForThisItem == 4)
      {
        if (this.name.Contains("Dwarf"))
        {
          if (showResponse)
            Game1.drawDialogue(this, giver.canUnderstandDwarves ? stringList[2] : StardewValley.Dialogue.convertToDwarvish(stringList[2]));
        }
        else if (showResponse)
          Game1.drawDialogue(this, stringList[2] + "$s");
        giver.changeFriendship((int) (-20.0 * (double) friendshipChangeMultiplier), this);
      }
      else
      {
        if (this.name.Contains("Dwarf"))
        {
          if (showResponse)
            Game1.drawDialogue(this, giver.canUnderstandDwarves ? stringList[2] : StardewValley.Dialogue.convertToDwarvish(stringList[2]));
        }
        else if (showResponse)
          Game1.drawDialogue(this, strArray[8]);
        giver.changeFriendship((int) (20.0 * (double) friendshipChangeMultiplier), this);
      }
    }

    public override void draw(SpriteBatch b, float alpha = 1f)
    {
      if (this.sprite == null || this.isInvisible || !Utility.isOnScreen(this.position, 2 * Game1.tileSize))
        return;
      if (this.swimming)
      {
        b.Draw(this.Sprite.Texture, this.getLocalPosition(Game1.viewport) + new Vector2((float) (Game1.tileSize / 2), (float) (Game1.tileSize + Game1.tileSize / 4 + this.yJumpOffset * 2)) + (this.shakeTimer > 0 ? new Vector2((float) Game1.random.Next(-1, 2), (float) Game1.random.Next(-1, 2)) : Vector2.Zero) - new Vector2(0.0f, this.yOffset), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(this.sprite.SourceRect.X, this.sprite.SourceRect.Y, this.sprite.SourceRect.Width, this.sprite.SourceRect.Height / 2 - (int) ((double) this.yOffset / (double) Game1.pixelZoom))), Color.White, this.rotation, new Vector2((float) (Game1.tileSize / 2), (float) (Game1.tileSize * 3 / 2)) / 4f, Math.Max(0.2f, this.scale) * (float) Game1.pixelZoom, this.flip ? SpriteEffects.FlipHorizontally : SpriteEffects.None, Math.Max(0.0f, this.drawOnTop ? 0.991f : (float) this.getStandingY() / 10000f));
        Vector2 localPosition = this.getLocalPosition(Game1.viewport);
        b.Draw(Game1.staminaRect, new Microsoft.Xna.Framework.Rectangle((int) localPosition.X + (int) this.yOffset + Game1.pixelZoom * 2, (int) localPosition.Y - 32 * Game1.pixelZoom + this.sprite.SourceRect.Height * Game1.pixelZoom + Game1.tileSize * 3 / 4 + this.yJumpOffset * 2 - (int) this.yOffset, this.sprite.SourceRect.Width * Game1.pixelZoom - (int) this.yOffset * 2 - Game1.pixelZoom * 4, Game1.pixelZoom), new Microsoft.Xna.Framework.Rectangle?(Game1.staminaRect.Bounds), Color.White * 0.75f, 0.0f, Vector2.Zero, SpriteEffects.None, (float) ((double) this.getStandingY() / 10000.0 + 1.0 / 1000.0));
      }
      else
        b.Draw(this.Sprite.Texture, this.getLocalPosition(Game1.viewport) + new Vector2((float) (this.sprite.spriteWidth * Game1.pixelZoom / 2), (float) (this.GetBoundingBox().Height / 2)) + (this.shakeTimer > 0 ? new Vector2((float) Game1.random.Next(-1, 2), (float) Game1.random.Next(-1, 2)) : Vector2.Zero), new Microsoft.Xna.Framework.Rectangle?(this.Sprite.SourceRect), Color.White * alpha, this.rotation, new Vector2((float) (this.sprite.spriteWidth / 2), (float) ((double) this.sprite.spriteHeight * 3.0 / 4.0)), Math.Max(0.2f, this.scale) * (float) Game1.pixelZoom, this.flip || this.sprite.currentAnimation != null && this.sprite.currentAnimation[this.sprite.currentAnimationIndex].flip ? SpriteEffects.FlipHorizontally : SpriteEffects.None, Math.Max(0.0f, this.drawOnTop ? 0.991f : (float) this.getStandingY() / 10000f));
      if (this.breather && this.shakeTimer <= 0 && (!this.swimming && this.sprite.CurrentFrame < 16) && !this.farmerPassesThrough)
      {
        Microsoft.Xna.Framework.Rectangle sourceRect = this.sprite.SourceRect;
        sourceRect.Y += this.sprite.spriteHeight / 2 + this.sprite.spriteHeight / 32;
        sourceRect.Height = this.sprite.spriteHeight / 4;
        sourceRect.X += this.sprite.spriteWidth / 4;
        sourceRect.Width = this.sprite.spriteWidth / 2;
        Vector2 vector2 = new Vector2((float) (this.sprite.spriteWidth * Game1.pixelZoom / 2), (float) (Game1.tileSize / 8));
        if (this.age == 2)
        {
          sourceRect.Y += this.sprite.spriteHeight / 6 + 1;
          sourceRect.Height /= 2;
          vector2.Y += (float) (this.sprite.spriteHeight / 8 * Game1.pixelZoom);
          if (this is Child)
          {
            if ((this as Child).age == 0)
              vector2.X -= (float) (Game1.pixelZoom * 3);
            else if ((this as Child).age == 1)
              vector2.X -= (float) Game1.pixelZoom;
          }
        }
        else if (this.gender == 1)
        {
          ++sourceRect.Y;
          vector2.Y -= (float) Game1.pixelZoom;
          sourceRect.Height /= 2;
        }
        float num = Math.Max(0.0f, (float) (Math.Ceiling(Math.Sin(Game1.currentGameTime.TotalGameTime.TotalMilliseconds / 600.0 + (double) this.defaultPosition.X * 20.0)) / 4.0));
        b.Draw(this.Sprite.Texture, this.getLocalPosition(Game1.viewport) + vector2 + (this.shakeTimer > 0 ? new Vector2((float) Game1.random.Next(-1, 2), (float) Game1.random.Next(-1, 2)) : Vector2.Zero), new Microsoft.Xna.Framework.Rectangle?(sourceRect), Color.White * alpha, this.rotation, new Vector2((float) (sourceRect.Width / 2), (float) (sourceRect.Height / 2 + 1)), Math.Max(0.2f, this.scale) * (float) Game1.pixelZoom + num, this.flip ? SpriteEffects.FlipHorizontally : SpriteEffects.None, Math.Max(0.0f, this.drawOnTop ? 0.992f : (float) ((double) this.getStandingY() / 10000.0 + 1.0 / 1000.0)));
      }
      if (this.isGlowing)
        b.Draw(this.Sprite.Texture, this.getLocalPosition(Game1.viewport) + new Vector2((float) (this.sprite.spriteWidth * Game1.pixelZoom / 2), (float) (this.GetBoundingBox().Height / 2)) + (this.shakeTimer > 0 ? new Vector2((float) Game1.random.Next(-1, 2), (float) Game1.random.Next(-1, 2)) : Vector2.Zero), new Microsoft.Xna.Framework.Rectangle?(this.Sprite.SourceRect), this.glowingColor * this.glowingTransparency, this.rotation, new Vector2((float) (this.sprite.spriteWidth / 2), (float) ((double) this.sprite.spriteHeight * 3.0 / 4.0)), Math.Max(0.2f, this.scale) * 4f, this.flip ? SpriteEffects.FlipHorizontally : SpriteEffects.None, Math.Max(0.0f, this.drawOnTop ? 0.99f : (float) ((double) this.getStandingY() / 10000.0 + 1.0 / 1000.0)));
      if (!this.IsEmoting || Game1.eventUp || (this is Child || this is Pet))
        return;
      Vector2 localPosition1 = this.getLocalPosition(Game1.viewport);
      localPosition1.Y -= (float) (Game1.tileSize / 2 + this.sprite.spriteHeight * Game1.pixelZoom);
      b.Draw(Game1.emoteSpriteSheet, localPosition1, new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(this.CurrentEmoteIndex * 16 % Game1.emoteSpriteSheet.Width, this.CurrentEmoteIndex * 16 / Game1.emoteSpriteSheet.Width * 16, 16, 16)), Color.White, 0.0f, Vector2.Zero, (float) Game1.pixelZoom, SpriteEffects.None, (float) this.getStandingY() / 10000f);
    }

    public override void drawAboveAlwaysFrontLayer(SpriteBatch b)
    {
      if (this.textAboveHeadTimer <= 0 || this.textAboveHead == null)
        return;
      Vector2 local = Game1.GlobalToLocal(new Vector2((float) this.getStandingX(), (float) (this.getStandingY() - Game1.tileSize * 3 + this.yJumpOffset)));
      if (this.textAboveHeadStyle == 0)
        local += new Vector2((float) Game1.random.Next(-1, 2), (float) Game1.random.Next(-1, 2));
      SpriteText.drawStringWithScrollCenteredAt(b, this.textAboveHead, (int) local.X, (int) local.Y, "", this.textAboveHeadAlpha, this.textAboveHeadColor, 1, (float) ((double) (this.getTileY() * Game1.tileSize) / 10000.0 + 1.0 / 1000.0 + (double) this.getTileX() / 10000.0), false);
    }

    public void warpToPathControllerDestination()
    {
      if (this.controller == null)
        return;
      while (this.controller.pathToEndPoint.Count > 2)
      {
        this.controller.pathToEndPoint.Pop();
        this.position = new Vector2((float) (this.controller.pathToEndPoint.Peek().X * Game1.tileSize), (float) (this.controller.pathToEndPoint.Peek().Y * Game1.tileSize + Game1.tileSize / 4));
        this.Halt();
      }
    }

    public virtual Microsoft.Xna.Framework.Rectangle getMugShotSourceRect()
    {
      return new Microsoft.Xna.Framework.Rectangle(0, this.age == 2 ? 4 : 0, 16, 24);
    }

    public void getHitByPlayer(Farmer who, GameLocation location)
    {
      this.doEmote(12, true);
      if (who == null)
      {
        if (Game1.IsMultiplayer)
          return;
        who = Game1.player;
      }
      if (who.friendships.ContainsKey(this.name))
      {
        who.friendships[this.name][0] -= 30;
        if (who.IsMainPlayer)
        {
          this.CurrentDialogue.Clear();
          this.CurrentDialogue.Push(new StardewValley.Dialogue(Game1.random.NextDouble() < 0.5 ? Game1.LoadStringByGender(this.gender, "Strings\\StringsFromCSFiles:NPC.cs.4293") : Game1.content.LoadString("Strings\\StringsFromCSFiles:NPC.cs.4294"), this));
        }
        location.debris.Add(new Debris(this.sprite.Texture, Game1.random.Next(3, 8), new Vector2((float) this.GetBoundingBox().Center.X, (float) this.GetBoundingBox().Center.Y)));
      }
      if (this.name.Equals("Bouncer"))
        Game1.playSound("crafting");
      else
        Game1.playSound("hitEnemy");
    }

    public void walkInSquare(int squareWidth, int squareHeight, int squarePauseOffset)
    {
      this.isWalkingInSquare = true;
      this.lengthOfWalkingSquareX = squareWidth;
      this.lengthOfWalkingSquareY = squareHeight;
      this.squarePauseOffset = squarePauseOffset;
    }

    public void moveTowardPlayer(int threshold)
    {
      this.isWalkingTowardPlayer = true;
      this.moveTowardPlayerThreshold = threshold;
    }

    public virtual bool withinPlayerThreshold()
    {
      return this.withinPlayerThreshold(this.moveTowardPlayerThreshold);
    }

    public bool withinPlayerThreshold(int threshold)
    {
      if (this.currentLocation != null && !this.currentLocation.Equals((object) Game1.currentLocation))
        return false;
      Vector2 tileLocation1 = Game1.player.getTileLocation();
      Vector2 tileLocation2 = this.getTileLocation();
      return (double) Math.Abs(tileLocation2.X - tileLocation1.X) <= (double) threshold && (double) Math.Abs(tileLocation2.Y - tileLocation1.Y) <= (double) threshold;
    }

    private Stack<Point> addToStackForSchedule(Stack<Point> original, Stack<Point> toAdd)
    {
      if (toAdd == null)
        return original;
      original = new Stack<Point>((IEnumerable<Point>) original);
      while (original.Count > 0)
        toAdd.Push(original.Pop());
      return toAdd;
    }

    private SchedulePathDescription pathfindToNextScheduleLocation(string startingLocation, int startingX, int startingY, string endingLocation, int endingX, int endingY, int finalFacingDirection, string endBehavior, string endMessage)
    {
      Stack<Point> pointStack = new Stack<Point>();
      Point startPoint = new Point(startingX, startingY);
      List<string> stringList = !startingLocation.Equals(endingLocation) ? this.getLocationRoute(startingLocation, endingLocation) : (List<string>) null;
      if (stringList != null)
      {
        for (int index = 0; index < stringList.Count; ++index)
        {
          GameLocation locationFromName = Game1.getLocationFromName(stringList[index]);
          if (index < stringList.Count - 1)
          {
            Point warpPointTo = locationFromName.getWarpPointTo(stringList[index + 1]);
            if (warpPointTo.Equals(Point.Zero) || startPoint.Equals(Point.Zero))
              throw new Exception("schedule pathing tried to find a warp point that doesn't exist.");
            pointStack = this.addToStackForSchedule(pointStack, PathFindController.findPathForNPCSchedules(startPoint, warpPointTo, locationFromName, 30000));
            startPoint = locationFromName.getWarpPointTarget(warpPointTo);
          }
          else
            pointStack = this.addToStackForSchedule(pointStack, PathFindController.findPathForNPCSchedules(startPoint, new Point(endingX, endingY), locationFromName, 30000));
        }
      }
      else if (startingLocation.Equals(endingLocation))
        pointStack = PathFindController.findPathForNPCSchedules(startPoint, new Point(endingX, endingY), Game1.getLocationFromName(startingLocation), 30000);
      return new SchedulePathDescription(pointStack, finalFacingDirection, endBehavior, endMessage);
    }

    private List<string> getLocationRoute(string startingLocation, string endingLocation)
    {
      foreach (List<string> source in NPC.routesFromLocationToLocation)
      {
        if (source.First<string>().Equals(startingLocation) && source.Last<string>().Equals(endingLocation) && (this.gender == 0 || !source.Contains("BathHouse_MensLocker")) && (this.gender != 0 || !source.Contains("BathHouse_WomensLocker")))
          return source;
      }
      return (List<string>) null;
    }

    private bool changeScheduleForLocationAccessibility(ref string locationName, ref int tileX, ref int tileY, ref int facingDirection)
    {
      string str = locationName;
      if (!(str == "JojaMart") && !(str == "Railroad"))
      {
        if (str == "CommunityCenter")
          return !Game1.isLocationAccessible(locationName);
      }
      else if (!Game1.isLocationAccessible(locationName))
      {
        if (!Game1.content.Load<Dictionary<string, string>>("Characters\\schedules\\" + this.name).ContainsKey(locationName + "_Replacement"))
          return true;
        string[] strArray = Game1.content.Load<Dictionary<string, string>>("Characters\\schedules\\" + this.name)[locationName + "_Replacement"].Split(' ');
        locationName = strArray[0];
        tileX = Convert.ToInt32(strArray[1]);
        tileY = Convert.ToInt32(strArray[2]);
        facingDirection = Convert.ToInt32(strArray[3]);
      }
      return false;
    }

    private Dictionary<int, SchedulePathDescription> parseMasterSchedule(string rawData)
    {
      string[] strArray1 = rawData.Split('/');
      Dictionary<int, SchedulePathDescription> dictionary = new Dictionary<int, SchedulePathDescription>();
      int index1 = 0;
      if (strArray1[0].Contains("GOTO"))
      {
        string currentSeason = strArray1[0].Split(' ')[1];
        if (currentSeason.ToLower().Equals("season"))
          currentSeason = Game1.currentSeason;
        try
        {
          strArray1 = Game1.content.Load<Dictionary<string, string>>("Characters\\schedules\\" + this.name)[currentSeason].Split('/');
        }
        catch (Exception ex)
        {
          return this.parseMasterSchedule(Game1.content.Load<Dictionary<string, string>>("Characters\\schedules\\" + this.name)["spring"]);
        }
      }
      if (strArray1[0].Contains("NOT"))
      {
        string[] strArray2 = strArray1[0].Split(' ');
        if (strArray2[1].ToLower() == "friendship")
        {
          string name = strArray2[2];
          int int32 = Convert.ToInt32(strArray2[3]);
          bool flag = false;
          foreach (Farmer allFarmer in Game1.getAllFarmers())
          {
            if (allFarmer.getFriendshipLevelForNPC(name) >= int32)
            {
              flag = true;
              break;
            }
          }
          if (flag)
            return this.parseMasterSchedule(Game1.content.Load<Dictionary<string, string>>("Characters\\schedules\\" + this.name)["spring"]);
          ++index1;
        }
      }
      if (strArray1[index1].Contains("GOTO"))
      {
        string currentSeason = strArray1[index1].Split(' ')[1];
        if (currentSeason.ToLower().Equals("season"))
          currentSeason = Game1.currentSeason;
        strArray1 = Game1.content.Load<Dictionary<string, string>>("Characters\\schedules\\" + this.name)[currentSeason].Split('/');
        index1 = 1;
      }
      Point point = this.isMarried() ? new Point(0, 23) : new Point((int) this.defaultPosition.X / Game1.tileSize, (int) this.defaultPosition.Y / Game1.tileSize);
      string startingLocation = this.isMarried() ? "BusStop" : this.defaultMap;
      for (int index2 = index1; index2 < strArray1.Length && strArray1.Length > 1; ++index2)
      {
        int index3 = 0;
        string[] strArray2 = strArray1[index2].Split(' ');
        int int32_1 = Convert.ToInt32(strArray2[index3]);
        int index4 = index3 + 1;
        string locationName = strArray2[index4];
        string endBehavior = (string) null;
        string endMessage = (string) null;
        int result;
        if (int.TryParse(locationName, out result))
        {
          locationName = startingLocation;
          --index4;
        }
        int index5 = index4 + 1;
        int int32_2 = Convert.ToInt32(strArray2[index5]);
        int index6 = index5 + 1;
        int int32_3 = Convert.ToInt32(strArray2[index6]);
        int index7 = index6 + 1;
        int facingDirection;
        try
        {
          facingDirection = Convert.ToInt32(strArray2[index7]);
          ++index7;
        }
        catch (Exception ex)
        {
          facingDirection = 2;
        }
        if (this.changeScheduleForLocationAccessibility(ref locationName, ref int32_2, ref int32_3, ref facingDirection))
        {
          if (Game1.content.Load<Dictionary<string, string>>("Characters\\schedules\\" + this.name).ContainsKey("default"))
            return this.parseMasterSchedule(Game1.content.Load<Dictionary<string, string>>("Characters\\schedules\\" + this.name)["default"]);
          return this.parseMasterSchedule(Game1.content.Load<Dictionary<string, string>>("Characters\\schedules\\" + this.name)["spring"]);
        }
        if (index7 < strArray2.Length)
        {
          if (strArray2[index7].Length > 0 && (int) strArray2[index7][0] == 34)
          {
            endMessage = strArray1[index2].Substring(strArray1[index2].IndexOf('"'));
          }
          else
          {
            endBehavior = strArray2[index7];
            int index8 = index7 + 1;
            if (index8 < strArray2.Length && strArray2[index8].Length > 0 && (int) strArray2[index8][0] == 34)
              endMessage = strArray1[index2].Substring(strArray1[index2].IndexOf('"')).Replace("\"", "");
          }
        }
        dictionary.Add(int32_1, this.pathfindToNextScheduleLocation(startingLocation, point.X, point.Y, locationName, int32_2, int32_3, facingDirection, endBehavior, endMessage));
        point.X = int32_2;
        point.Y = int32_3;
        startingLocation = locationName;
      }
      return dictionary;
    }

    public Dictionary<int, SchedulePathDescription> getSchedule(int dayOfMonth)
    {
      if (!this.name.Equals("Robin") || Game1.player.currentUpgrade != null)
        this.isInvisible = false;
      if (this.name.Equals("Willy") && Game1.stats.DaysPlayed < 2U)
        this.isInvisible = true;
      else if (this.Schedule != null)
        this.followSchedule = true;
      Dictionary<string, string> dictionary;
      try
      {
        dictionary = Game1.content.Load<Dictionary<string, string>>("Characters\\schedules\\" + this.name);
      }
      catch (Exception ex)
      {
        return (Dictionary<int, SchedulePathDescription>) null;
      }
      if (this.isMarried())
      {
        string str = Game1.shortDayNameFromDayOfSeason(dayOfMonth);
        if (this.name.Equals("Penny") && (str.Equals("Tue") || str.Equals("Wed") || str.Equals("Fri")) || (this.name.Equals("Maru") && (str.Equals("Tue") || str.Equals("Thu")) || this.name.Equals("Harvey") && (str.Equals("Tue") || str.Equals("Thu"))))
        {
          this.nameOfTodaysSchedule = "marriageJob";
          return this.parseMasterSchedule(dictionary["marriageJob"]);
        }
        if (!Game1.isRaining && dictionary.ContainsKey("marriage_" + Game1.shortDayNameFromDayOfSeason(Game1.dayOfMonth)))
        {
          this.nameOfTodaysSchedule = "marriage_" + Game1.shortDayNameFromDayOfSeason(Game1.dayOfMonth);
          return this.parseMasterSchedule(dictionary["marriage_" + Game1.shortDayNameFromDayOfSeason(Game1.dayOfMonth)]);
        }
        this.followSchedule = false;
        return (Dictionary<int, SchedulePathDescription>) null;
      }
      if (dictionary.ContainsKey(Game1.currentSeason + "_" + (object) Game1.dayOfMonth))
        return this.parseMasterSchedule(dictionary[Game1.currentSeason + "_" + (object) Game1.dayOfMonth]);
      for (int index = Game1.player.friendships.ContainsKey(this.name) ? Game1.player.friendships[this.name][0] / 250 : -1; index > 0; --index)
      {
        if (dictionary.ContainsKey(Game1.dayOfMonth.ToString() + "_" + (object) index))
          return this.parseMasterSchedule(dictionary[Game1.dayOfMonth.ToString() + "_" + (object) index]);
      }
      if (dictionary.ContainsKey(string.Empty + (object) Game1.dayOfMonth))
        return this.parseMasterSchedule(dictionary[string.Empty + (object) Game1.dayOfMonth]);
      if (this.name.Equals("Pam") && Game1.player.mailReceived.Contains("ccVault"))
        return this.parseMasterSchedule(dictionary["bus"]);
      if (Game1.isRaining)
      {
        if (Game1.random.NextDouble() < 0.5 && dictionary.ContainsKey("rain2"))
          return this.parseMasterSchedule(dictionary["rain2"]);
        if (dictionary.ContainsKey("rain"))
          return this.parseMasterSchedule(dictionary["rain"]);
      }
      List<string> stringList = new List<string>()
      {
        Game1.currentSeason,
        Game1.shortDayNameFromDayOfSeason(Game1.dayOfMonth)
      };
      int num1 = Game1.player.friendships.ContainsKey(this.name) ? Game1.player.friendships[this.name][0] / 250 : -1;
      while (num1 > 0)
      {
        stringList.Add(string.Empty + (object) num1);
        if (dictionary.ContainsKey(string.Join("_", (IEnumerable<string>) stringList)))
          return this.parseMasterSchedule(dictionary[string.Join("_", (IEnumerable<string>) stringList)]);
        --num1;
        stringList.RemoveAt(stringList.Count - 1);
      }
      if (dictionary.ContainsKey(string.Join("_", (IEnumerable<string>) stringList)))
        return this.parseMasterSchedule(dictionary[string.Join("_", (IEnumerable<string>) stringList)]);
      if (dictionary.ContainsKey(Game1.shortDayNameFromDayOfSeason(Game1.dayOfMonth)))
        return this.parseMasterSchedule(dictionary[Game1.shortDayNameFromDayOfSeason(Game1.dayOfMonth)]);
      if (dictionary.ContainsKey(Game1.currentSeason))
        return this.parseMasterSchedule(dictionary[Game1.currentSeason]);
      if (dictionary.ContainsKey("spring_" + Game1.shortDayNameFromDayOfSeason(Game1.dayOfMonth)))
        return this.parseMasterSchedule(dictionary["spring_" + Game1.shortDayNameFromDayOfSeason(Game1.dayOfMonth)]);
      stringList.RemoveAt(stringList.Count - 1);
      stringList.Add("spring");
      int num2 = Game1.player.friendships.ContainsKey(this.name) ? Game1.player.friendships[this.name][0] / 250 : -1;
      while (num2 > 0)
      {
        stringList.Add(string.Empty + (object) num2);
        if (dictionary.ContainsKey(string.Join("_", (IEnumerable<string>) stringList)))
          return this.parseMasterSchedule(dictionary[string.Join("_", (IEnumerable<string>) stringList)]);
        --num2;
        stringList.RemoveAt(stringList.Count - 1);
      }
      if (dictionary.ContainsKey("spring"))
        return this.parseMasterSchedule(dictionary["spring"]);
      return (Dictionary<int, SchedulePathDescription>) null;
    }

    public void setMarried(bool isMarried)
    {
      this.married = isMarried ? 1 : 0;
    }

    public bool isMarried()
    {
      if (!this.isVillager())
        return false;
      if (this.married != -1)
        return this.married == 1;
      foreach (Farmer allFarmer in Game1.getAllFarmers())
      {
        if (allFarmer.spouse != null && allFarmer.spouse.Equals(this.name))
        {
          this.married = 1;
          return true;
        }
      }
      this.married = 0;
      return false;
    }

    public virtual void dayUpdate(int dayOfMonth)
    {
      if (this.currentLocation != null)
        Game1.warpCharacter(this, this.defaultMap, this.defaultPosition / (float) Game1.tileSize, true, false);
      if (this.name.Equals("Maru") || this.name.Equals("Shane"))
        this.sprite.Texture = Game1.content.Load<Texture2D>("Characters\\" + this.name);
      if (this.name.Equals("Willy") || this.name.Equals("Clint"))
      {
        this.sprite.spriteWidth = 16;
        this.sprite.spriteHeight = 32;
        this.sprite.ignoreSourceRectUpdates = false;
        this.sprite.UpdateSourceRect();
        this.isInvisible = false;
      }
      Game1.player.mailReceived.Remove(this.name);
      Game1.player.mailReceived.Remove(this.name + "Cooking");
      this.doingEndOfRouteAnimation = false;
      this.Halt();
      this.hasBeenKissedToday = false;
      this.faceTowardFarmer = false;
      this.faceTowardFarmerTimer = 0;
      this.drawOffset = Vector2.Zero;
      this.hasSaidAfternoonDialogue = false;
      this.ignoreScheduleToday = false;
      this.Halt();
      this.controller = (PathFindController) null;
      this.temporaryController = (PathFindController) null;
      this.directionsToNewLocation = (SchedulePathDescription) null;
      this.faceDirection(this.DefaultFacingDirection);
      this.scheduleTimeToTry = 9999999;
      this.previousEndPoint = new Point((int) this.defaultPosition.X / Game1.tileSize, (int) this.defaultPosition.Y / Game1.tileSize);
      this.isWalkingInSquare = false;
      this.returningToEndPoint = false;
      this.lastCrossroad = Microsoft.Xna.Framework.Rectangle.Empty;
      if (this.isVillager())
        this.Schedule = this.getSchedule(dayOfMonth);
      this.endOfRouteMessage = (string) null;
      bool flag = Utility.isFestivalDay(dayOfMonth, Game1.currentSeason);
      if (this.name.Equals("Robin") && Game1.player.daysUntilHouseUpgrade > 0 && !flag)
      {
        this.ignoreMultiplayerUpdates = true;
        Game1.warpCharacter(this, "Farm", new Vector2(68f, 14f), false, false);
        this.sprite.setCurrentAnimation(new List<FarmerSprite.AnimationFrame>()
        {
          new FarmerSprite.AnimationFrame(24, 75),
          new FarmerSprite.AnimationFrame(25, 75),
          new FarmerSprite.AnimationFrame(26, 300, false, false, new AnimatedSprite.endOfAnimationBehavior(this.robinHammerSound), false),
          new FarmerSprite.AnimationFrame(27, 1000, false, false, new AnimatedSprite.endOfAnimationBehavior(this.robinVariablePause), false)
        });
        this.ignoreScheduleToday = true;
        this.CurrentDialogue.Clear();
        this.currentDialogue.Push(new StardewValley.Dialogue(Game1.player.daysUntilHouseUpgrade == 2 ? Game1.content.LoadString("Strings\\StringsFromCSFiles:NPC.cs.3926") : Game1.content.LoadString("Strings\\StringsFromCSFiles:NPC.cs.3927"), this));
      }
      else if (this.name.Equals("Robin") && Game1.getFarm().isThereABuildingUnderConstruction() && !flag)
      {
        Building underConstruction = Game1.getFarm().getBuildingUnderConstruction();
        if (underConstruction.daysUntilUpgrade > 0)
        {
          if (!underConstruction.indoors.characters.Contains(this))
            underConstruction.indoors.addCharacter(this);
          if (this.currentLocation != null)
            this.currentLocation.characters.Remove(this);
          this.currentLocation = underConstruction.indoors;
          this.setTilePosition(1, 5);
        }
        else
        {
          Game1.warpCharacter(this, "Farm", new Vector2((float) (underConstruction.tileX + underConstruction.tilesWide / 2), (float) (underConstruction.tileY + underConstruction.tilesHigh / 2)), false, false);
          this.position.X += (float) (Game1.tileSize / 4);
          this.position.Y -= (float) (Game1.tileSize / 2);
        }
        this.ignoreMultiplayerUpdates = true;
        this.sprite.setCurrentAnimation(new List<FarmerSprite.AnimationFrame>()
        {
          new FarmerSprite.AnimationFrame(24, 75),
          new FarmerSprite.AnimationFrame(25, 75),
          new FarmerSprite.AnimationFrame(26, 300, false, false, new AnimatedSprite.endOfAnimationBehavior(this.robinHammerSound), false),
          new FarmerSprite.AnimationFrame(27, 1000, false, false, new AnimatedSprite.endOfAnimationBehavior(this.robinVariablePause), false)
        });
        this.ignoreScheduleToday = true;
        this.CurrentDialogue.Clear();
        this.currentDialogue.Push(new StardewValley.Dialogue(Game1.content.LoadString("Strings\\StringsFromCSFiles:NPC.cs.3926"), this));
      }
      if (!this.isMarried())
        return;
      this.marriageDuties();
      this.daysMarried = this.daysMarried + 1;
    }

    public void returnHomeFromFarmPosition(Farm farm)
    {
      if (!this.getTileLocationPoint().Equals(((FarmHouse) Game1.getLocationFromName("FarmHouse")).getPorchStandingSpot()))
        return;
      this.drawOffset = Vector2.Zero;
      string name = this.getHome().name;
      this.willDestroyObjectsUnderfoot = true;
      this.controller = new PathFindController((Character) this, (GameLocation) farm, farm.getWarpPointTo(name), 0)
      {
        NPCSchedule = true
      };
    }

    public void setUpForOutdoorPatioActivity()
    {
      Game1.warpCharacter(this, "Farm", new Vector2(71f, 10f), false, false);
      this.setNewDialogue("MarriageDialogue", "patio_", -1, false, true);
      string name = this.name;
      // ISSUE: reference to a compiler-generated method
      uint stringHash = \u003CPrivateImplementationDetails\u003E.ComputeStringHash(name);
      if (stringHash <= 1866496948U)
      {
        if (stringHash <= 1067922812U)
        {
          if ((int) stringHash != 161540545)
          {
            if ((int) stringHash != 587846041)
            {
              if ((int) stringHash != 1067922812 || !(name == "Sam"))
                return;
              this.setTilePosition(71, 8);
              this.sprite.setCurrentAnimation(new List<FarmerSprite.AnimationFrame>()
              {
                new FarmerSprite.AnimationFrame(25, 3000),
                new FarmerSprite.AnimationFrame(27, 500),
                new FarmerSprite.AnimationFrame(26, 100),
                new FarmerSprite.AnimationFrame(28, 100),
                new FarmerSprite.AnimationFrame(27, 500),
                new FarmerSprite.AnimationFrame(25, 2000),
                new FarmerSprite.AnimationFrame(27, 500),
                new FarmerSprite.AnimationFrame(26, 100),
                new FarmerSprite.AnimationFrame(29, 100),
                new FarmerSprite.AnimationFrame(30, 100),
                new FarmerSprite.AnimationFrame(32, 500),
                new FarmerSprite.AnimationFrame(31, 1000),
                new FarmerSprite.AnimationFrame(30, 100),
                new FarmerSprite.AnimationFrame(29, 100)
              });
            }
            else
            {
              if (!(name == "Penny"))
                return;
              this.setTilePosition(71, 8);
              this.sprite.setCurrentAnimation(new List<FarmerSprite.AnimationFrame>()
              {
                new FarmerSprite.AnimationFrame(18, 6000),
                new FarmerSprite.AnimationFrame(19, 500)
              });
            }
          }
          else
          {
            if (!(name == "Sebastian"))
              return;
            this.setTilePosition(71, 9);
            this.drawOffset = new Vector2((float) (Game1.tileSize / 4), (float) (Game1.tileSize / 2 + Game1.pixelZoom * 2));
            this.sprite.setCurrentAnimation(new List<FarmerSprite.AnimationFrame>()
            {
              new FarmerSprite.AnimationFrame(32, 500, Game1.tileSize, false, false, (AnimatedSprite.endOfAnimationBehavior) null, false, 0),
              new FarmerSprite.AnimationFrame(36, 500, Game1.tileSize, false, false, (AnimatedSprite.endOfAnimationBehavior) null, false, 0),
              new FarmerSprite.AnimationFrame(32, 500, Game1.tileSize, false, false, (AnimatedSprite.endOfAnimationBehavior) null, false, 0),
              new FarmerSprite.AnimationFrame(36, 500, Game1.tileSize, false, false, (AnimatedSprite.endOfAnimationBehavior) null, false, 0),
              new FarmerSprite.AnimationFrame(32, 500, Game1.tileSize, false, false, (AnimatedSprite.endOfAnimationBehavior) null, false, 0),
              new FarmerSprite.AnimationFrame(36, 500, Game1.tileSize, false, false, (AnimatedSprite.endOfAnimationBehavior) null, false, 0),
              new FarmerSprite.AnimationFrame(32, 500, Game1.tileSize, false, false, (AnimatedSprite.endOfAnimationBehavior) null, false, 0),
              new FarmerSprite.AnimationFrame(36, 2000, Game1.tileSize, false, false, (AnimatedSprite.endOfAnimationBehavior) null, false, 0),
              new FarmerSprite.AnimationFrame(33, 100, Game1.tileSize, false, false, (AnimatedSprite.endOfAnimationBehavior) null, false, 0),
              new FarmerSprite.AnimationFrame(34, 100, Game1.tileSize, false, false, (AnimatedSprite.endOfAnimationBehavior) null, false, 0),
              new FarmerSprite.AnimationFrame(35, 3000, Game1.tileSize, false, false, (AnimatedSprite.endOfAnimationBehavior) null, false, 0),
              new FarmerSprite.AnimationFrame(34, 100, Game1.tileSize, false, false, (AnimatedSprite.endOfAnimationBehavior) null, false, 0),
              new FarmerSprite.AnimationFrame(33, 100, Game1.tileSize, false, false, (AnimatedSprite.endOfAnimationBehavior) null, false, 0),
              new FarmerSprite.AnimationFrame(32, 1500, Game1.tileSize, false, false, (AnimatedSprite.endOfAnimationBehavior) null, false, 0)
            });
          }
        }
        else if ((int) stringHash != 1281010426)
        {
          if ((int) stringHash != 1708213605)
          {
            if ((int) stringHash != 1866496948 || !(name == "Shane"))
              return;
            this.setTilePosition(69, 9);
            this.sprite.setCurrentAnimation(new List<FarmerSprite.AnimationFrame>()
            {
              new FarmerSprite.AnimationFrame(28, 4000, Game1.tileSize, false, false, (AnimatedSprite.endOfAnimationBehavior) null, false, 0),
              new FarmerSprite.AnimationFrame(29, 800, Game1.tileSize, false, false, (AnimatedSprite.endOfAnimationBehavior) null, false, 0)
            });
          }
          else
          {
            if (!(name == "Alex"))
              return;
            this.setTilePosition(71, 8);
            this.sprite.setCurrentAnimation(new List<FarmerSprite.AnimationFrame>()
            {
              new FarmerSprite.AnimationFrame(34, 4000),
              new FarmerSprite.AnimationFrame(33, 300),
              new FarmerSprite.AnimationFrame(28, 200),
              new FarmerSprite.AnimationFrame(29, 100),
              new FarmerSprite.AnimationFrame(30, 100),
              new FarmerSprite.AnimationFrame(31, 100),
              new FarmerSprite.AnimationFrame(32, 100),
              new FarmerSprite.AnimationFrame(31, 100),
              new FarmerSprite.AnimationFrame(30, 100),
              new FarmerSprite.AnimationFrame(29, 100),
              new FarmerSprite.AnimationFrame(28, 800),
              new FarmerSprite.AnimationFrame(29, 100),
              new FarmerSprite.AnimationFrame(30, 100),
              new FarmerSprite.AnimationFrame(31, 100),
              new FarmerSprite.AnimationFrame(32, 100),
              new FarmerSprite.AnimationFrame(31, 100),
              new FarmerSprite.AnimationFrame(30, 100),
              new FarmerSprite.AnimationFrame(29, 100),
              new FarmerSprite.AnimationFrame(28, 800),
              new FarmerSprite.AnimationFrame(33, 200)
            });
          }
        }
        else
        {
          if (!(name == "Maru"))
            return;
          this.setTilePosition(70, 8);
          this.sprite.setCurrentAnimation(new List<FarmerSprite.AnimationFrame>()
          {
            new FarmerSprite.AnimationFrame(16, 4000),
            new FarmerSprite.AnimationFrame(17, 200),
            new FarmerSprite.AnimationFrame(18, 200),
            new FarmerSprite.AnimationFrame(19, 200),
            new FarmerSprite.AnimationFrame(20, 200),
            new FarmerSprite.AnimationFrame(21, 200),
            new FarmerSprite.AnimationFrame(22, 200),
            new FarmerSprite.AnimationFrame(23, 200)
          });
        }
      }
      else if (stringHash <= 2571828641U)
      {
        if ((int) stringHash != 2010304804)
        {
          if ((int) stringHash != -1860673204)
          {
            if ((int) stringHash != -1723138655 || !(name == "Emily"))
              return;
            this.setTilePosition(70, 9);
            this.sprite.setCurrentAnimation(new List<FarmerSprite.AnimationFrame>()
            {
              new FarmerSprite.AnimationFrame(54, 4000, Game1.tileSize, false, false, (AnimatedSprite.endOfAnimationBehavior) null, false, 0)
            });
          }
          else
          {
            if (!(name == "Haley"))
              return;
            this.setTilePosition(70, 8);
            this.sprite.setCurrentAnimation(new List<FarmerSprite.AnimationFrame>()
            {
              new FarmerSprite.AnimationFrame(30, 2000),
              new FarmerSprite.AnimationFrame(31, 200),
              new FarmerSprite.AnimationFrame(24, 2000),
              new FarmerSprite.AnimationFrame(25, 1000),
              new FarmerSprite.AnimationFrame(32, 200),
              new FarmerSprite.AnimationFrame(33, 2000),
              new FarmerSprite.AnimationFrame(32, 200),
              new FarmerSprite.AnimationFrame(25, 2000),
              new FarmerSprite.AnimationFrame(32, 200),
              new FarmerSprite.AnimationFrame(33, 2000)
            });
          }
        }
        else
        {
          if (!(name == "Harvey"))
            return;
          this.setTilePosition(71, 8);
          this.sprite.setCurrentAnimation(new List<FarmerSprite.AnimationFrame>()
          {
            new FarmerSprite.AnimationFrame(42, 6000),
            new FarmerSprite.AnimationFrame(43, 1000),
            new FarmerSprite.AnimationFrame(39, 100),
            new FarmerSprite.AnimationFrame(43, 500),
            new FarmerSprite.AnimationFrame(39, 100),
            new FarmerSprite.AnimationFrame(43, 1000),
            new FarmerSprite.AnimationFrame(42, 5000),
            new FarmerSprite.AnimationFrame(43, 3000)
          });
        }
      }
      else if ((int) stringHash != -1562053956)
      {
        if ((int) stringHash != -1468719973)
        {
          if ((int) stringHash != -1228790996 || !(name == "Elliott"))
            return;
          this.setTilePosition(71, 8);
          this.sprite.setCurrentAnimation(new List<FarmerSprite.AnimationFrame>()
          {
            new FarmerSprite.AnimationFrame(33, 3000),
            new FarmerSprite.AnimationFrame(32, 500),
            new FarmerSprite.AnimationFrame(33, 3000),
            new FarmerSprite.AnimationFrame(32, 500),
            new FarmerSprite.AnimationFrame(33, 2000),
            new FarmerSprite.AnimationFrame(34, 1500)
          });
        }
        else
        {
          if (!(name == "Leah"))
            return;
          this.setTilePosition(71, 8);
          this.sprite.setCurrentAnimation(new List<FarmerSprite.AnimationFrame>()
          {
            new FarmerSprite.AnimationFrame(16, 100),
            new FarmerSprite.AnimationFrame(17, 100),
            new FarmerSprite.AnimationFrame(18, 100),
            new FarmerSprite.AnimationFrame(19, 300),
            new FarmerSprite.AnimationFrame(16, 100),
            new FarmerSprite.AnimationFrame(17, 100),
            new FarmerSprite.AnimationFrame(18, 100),
            new FarmerSprite.AnimationFrame(19, 1000),
            new FarmerSprite.AnimationFrame(16, 100),
            new FarmerSprite.AnimationFrame(17, 100),
            new FarmerSprite.AnimationFrame(18, 100),
            new FarmerSprite.AnimationFrame(19, 300),
            new FarmerSprite.AnimationFrame(16, 100),
            new FarmerSprite.AnimationFrame(17, 100),
            new FarmerSprite.AnimationFrame(18, 100),
            new FarmerSprite.AnimationFrame(19, 300),
            new FarmerSprite.AnimationFrame(16, 100),
            new FarmerSprite.AnimationFrame(17, 100),
            new FarmerSprite.AnimationFrame(18, 100),
            new FarmerSprite.AnimationFrame(19, 2000)
          });
        }
      }
      else
      {
        if (!(name == "Abigail"))
          return;
        this.setTilePosition(71, 8);
        this.sprite.setCurrentAnimation(new List<FarmerSprite.AnimationFrame>()
        {
          new FarmerSprite.AnimationFrame(16, 500),
          new FarmerSprite.AnimationFrame(17, 500),
          new FarmerSprite.AnimationFrame(18, 500),
          new FarmerSprite.AnimationFrame(19, 500)
        });
      }
    }

    public bool isGaySpouse()
    {
      Farmer spouse = this.getSpouse();
      if (spouse == null)
        return false;
      if (this.gender == 0 && spouse.isMale)
        return true;
      if (this.gender == 1)
        return !spouse.isMale;
      return false;
    }

    public bool canGetPregnant()
    {
      if (this is Horse)
        return false;
      Farmer spouse = this.getSpouse();
      if (spouse == null)
        return false;
      int heartLevelForNpc = spouse.getFriendshipHeartLevelForNPC(this.name);
      List<Child> children = spouse.getChildren();
      if (this.defaultMap == null)
        this.defaultMap = "FarmHouse";
      if ((Game1.getLocationFromName(this.defaultMap) as FarmHouse).upgradeLevel < 2 || this.daysUntilBirthing >= 0 || (heartLevelForNpc < 10 || spouse.daysMarried < 7))
        return false;
      if (children.Count == 0)
        return true;
      if (children.Count < 2)
        return children[0].age > 2;
      return false;
    }

    public void marriageDuties()
    {
      if (!Game1.newDay && (int) Game1.gameMode != 6 || this.getSpouse() == null)
        return;
      FarmHouse farmHouse = (FarmHouse) null;
      if (this.defaultMap.Contains("FarmHouse"))
        farmHouse = Game1.getLocationFromName(this.defaultMap) as FarmHouse;
      Random r;
      try
      {
        r = new Random((int) Game1.stats.DaysPlayed + (int) Game1.uniqueIDForThisGame / 2 + farmHouse.farmerNumberOfOwner);
      }
      catch (Exception ex)
      {
        this.defaultMap = "FarmHouse";
        farmHouse = Game1.getLocationFromName(this.defaultMap) as FarmHouse;
        r = new Random((int) Game1.stats.DaysPlayed + (int) Game1.uniqueIDForThisGame / 2 + farmHouse.farmerNumberOfOwner);
      }
      int heartLevelForNpc = this.getSpouse().getFriendshipHeartLevelForNPC(this.name);
      if (this.currentLocation == null || !this.currentLocation.Equals((object) farmHouse))
      {
        Game1.removeThisCharacterFromAllLocations(this);
        for (int index = farmHouse.characters.Count - 1; index >= 0; --index)
        {
          if (farmHouse.characters[index].name.Equals(this.name))
            farmHouse.characters.RemoveAt(index);
        }
        Game1.warpCharacter(this, "FarmHouse", farmHouse.getBedSpot(), false, false);
      }
      if (this.daysUntilBirthing > 0)
        this.daysUntilBirthing = this.daysUntilBirthing - 1;
      if (this.daysAfterLastBirth >= 0)
      {
        this.daysAfterLastBirth = this.daysAfterLastBirth - 1;
        List<Child> children = this.getSpouse().getChildren();
        if (children.Count == 1)
        {
          this.setTilePosition(farmHouse.getKitchenStandingSpot());
          if (this.spouseObstacleCheck(Game1.content.LoadString("Strings\\StringsFromCSFiles:NPC.cs.4406"), (GameLocation) farmHouse, false))
            return;
          this.setNewDialogue("MarriageDialogue", "OneKid_", r.Next(4), false, false);
          return;
        }
        if (children.Count == 2)
        {
          this.setTilePosition(farmHouse.getKitchenStandingSpot());
          if (this.spouseObstacleCheck(Game1.content.LoadString("Strings\\StringsFromCSFiles:NPC.cs.4406"), (GameLocation) farmHouse, false))
            return;
          this.setNewDialogue("MarriageDialogue", "TwoKids_", r.Next(4), false, false);
          return;
        }
      }
      this.setTilePosition(farmHouse.getKitchenStandingSpot());
      if (this.tryToGetMarriageSpecificDialogueElseReturnDefault(Game1.currentSeason + "_" + (object) Game1.dayOfMonth, "").Length > 0)
      {
        this.currentDialogue.Clear();
        this.currentDialogue.Push(new StardewValley.Dialogue(this.tryToGetMarriageSpecificDialogueElseReturnDefault(Game1.currentSeason + "_" + (object) Game1.dayOfMonth, ""), this));
      }
      else if (this.schedule != null)
      {
        if (this.nameOfTodaysSchedule.Equals("marriage_" + Game1.shortDayNameFromDayOfSeason(Game1.dayOfMonth)))
        {
          this.setNewDialogue("MarriageDialogue", "funLeave_", -1, false, true);
        }
        else
        {
          if (!this.nameOfTodaysSchedule.Equals("marriageJob"))
            return;
          this.setNewDialogue("MarriageDialogue", "jobLeave_", -1, false, true);
        }
      }
      else if (!Game1.isRaining && !Game1.IsWinter && Game1.shortDayNameFromDayOfSeason(Game1.dayOfMonth).Equals("Sat"))
        this.setUpForOutdoorPatioActivity();
      else if (this.daysMarried >= 1 && r.NextDouble() < 1.0 - (double) Math.Max(1, heartLevelForNpc) / 12.0)
      {
        Furniture randomFurniture = farmHouse.getRandomFurniture(r);
        if (randomFurniture != null && randomFurniture.isGroundFurniture())
        {
          Point p = new Point((int) randomFurniture.tileLocation.X - 1, (int) randomFurniture.tileLocation.Y);
          if (farmHouse.isTileLocationTotallyClearAndPlaceable(p.X, p.Y))
          {
            this.setTilePosition(p);
            this.faceDirection(1);
            string s = "";
            switch (r.Next(10))
            {
              case 0:
                s = Game1.content.LoadString("Strings\\StringsFromCSFiles:NPC.cs.4420");
                break;
              case 1:
                s = Game1.content.LoadString("Strings\\StringsFromCSFiles:NPC.cs.4421");
                break;
              case 2:
                s = Game1.LoadStringByGender(this.gender, "Strings\\StringsFromCSFiles:NPC.cs.4422");
                break;
              case 3:
                s = Game1.content.LoadString("Strings\\StringsFromCSFiles:NPC.cs.4423");
                break;
              case 4:
                s = Game1.content.LoadString("Strings\\StringsFromCSFiles:NPC.cs.4424");
                break;
              case 5:
                s = Game1.content.LoadString("Strings\\StringsFromCSFiles:NPC.cs.4425");
                break;
              case 6:
                s = Game1.content.LoadString("Strings\\StringsFromCSFiles:NPC.cs.4426");
                break;
              case 7:
                s = this.gender == 1 ? (r.NextDouble() < 0.5 ? Game1.content.LoadString("Strings\\StringsFromCSFiles:NPC.cs.4427") : Game1.content.LoadString("Strings\\StringsFromCSFiles:NPC.cs.4429")) : Game1.content.LoadString("Strings\\StringsFromCSFiles:NPC.cs.4431");
                break;
              case 8:
                s = Game1.content.LoadString("Strings\\StringsFromCSFiles:NPC.cs.4432");
                break;
              case 9:
                s = Game1.content.LoadString("Strings\\StringsFromCSFiles:NPC.cs.4433");
                break;
            }
            this.setNewDialogue(s, false, false);
            return;
          }
        }
        string backToBedMessage = "";
        switch (r.Next(5))
        {
          case 0:
            backToBedMessage = Game1.content.LoadString("Strings\\StringsFromCSFiles:NPC.cs.4434");
            break;
          case 1:
            backToBedMessage = Game1.content.LoadString("Strings\\StringsFromCSFiles:NPC.cs.4435");
            break;
          case 2:
            backToBedMessage = Game1.content.LoadString("Strings\\StringsFromCSFiles:NPC.cs.4436");
            break;
          case 3:
            backToBedMessage = Game1.LoadStringByGender(this.gender, "Strings\\StringsFromCSFiles:NPC.cs.4437");
            break;
          case 4:
            backToBedMessage = Game1.content.LoadString("Strings\\StringsFromCSFiles:NPC.cs.4438");
            break;
        }
        this.spouseObstacleCheck(backToBedMessage, (GameLocation) farmHouse, true);
      }
      else if (this.daysUntilBirthing != -1 && this.daysUntilBirthing <= 7 && r.NextDouble() < 0.5)
      {
        if (this.isGaySpouse())
        {
          this.setTilePosition(farmHouse.getKitchenStandingSpot());
          if (this.spouseObstacleCheck(Game1.content.LoadString("Strings\\StringsFromCSFiles:NPC.cs.4439"), (GameLocation) farmHouse, false))
            return;
          string s;
          if (r.NextDouble() >= 0.5)
            s = Game1.content.LoadString("Strings\\StringsFromCSFiles:NPC.cs.4441", (object) this.getTermOfSpousalEndearment(true));
          else
            s = Game1.content.LoadString("Strings\\StringsFromCSFiles:NPC.cs.4440", (object) this.getSpouse().displayName);
          int num1 = r.NextDouble() < 0.5 ? 1 : 0;
          int num2 = 0;
          this.setNewDialogue(s, num1 != 0, num2 != 0);
        }
        else if (this.gender == 1)
        {
          this.setTilePosition(farmHouse.getKitchenStandingSpot());
          if (this.spouseObstacleCheck(r.NextDouble() < 0.5 ? Game1.content.LoadString("Strings\\StringsFromCSFiles:NPC.cs.4442") : Game1.content.LoadString("Strings\\StringsFromCSFiles:NPC.cs.4443"), (GameLocation) farmHouse, false))
            return;
          string s;
          if (r.NextDouble() >= 0.5)
            s = Game1.content.LoadString("Strings\\StringsFromCSFiles:NPC.cs.4445", (object) this.getTermOfSpousalEndearment(true));
          else
            s = Game1.content.LoadString("Strings\\StringsFromCSFiles:NPC.cs.4444", (object) this.getSpouse().displayName);
          int num1 = r.NextDouble() < 0.5 ? 1 : 0;
          int num2 = 0;
          this.setNewDialogue(s, num1 != 0, num2 != 0);
        }
        else
        {
          this.setTilePosition(farmHouse.getKitchenStandingSpot());
          if (this.spouseObstacleCheck(Game1.LoadStringByGender(this.gender, "Strings\\StringsFromCSFiles:NPC.cs.4446"), (GameLocation) farmHouse, false))
            return;
          string s;
          if (r.NextDouble() >= 0.5)
            s = Game1.content.LoadString("Strings\\StringsFromCSFiles:NPC.cs.4448", (object) this.getTermOfSpousalEndearment(true));
          else
            s = Game1.LoadStringByGender(this.gender, "Strings\\StringsFromCSFiles:NPC.cs.4447", (object) this.getSpouse().displayName);
          int num1 = r.NextDouble() < 0.5 ? 1 : 0;
          int num2 = 0;
          this.setNewDialogue(s, num1 != 0, num2 != 0);
        }
      }
      else
      {
        if (r.NextDouble() < 0.07)
        {
          List<Child> children = this.getSpouse().getChildren();
          if (children.Count == 1)
          {
            this.setTilePosition(farmHouse.getKitchenStandingSpot());
            if (this.spouseObstacleCheck(Game1.LoadStringByGender(this.gender, "Strings\\StringsFromCSFiles:NPC.cs.4449"), (GameLocation) farmHouse, false))
              return;
            this.setNewDialogue("MarriageDialogue", "OneKid_", r.Next(4), false, false);
            return;
          }
          if (children.Count == 2)
          {
            this.setTilePosition(farmHouse.getKitchenStandingSpot());
            if (this.spouseObstacleCheck(Game1.LoadStringByGender(this.gender, "Strings\\StringsFromCSFiles:NPC.cs.4452"), (GameLocation) farmHouse, false))
              return;
            this.setNewDialogue("MarriageDialogue", "TwoKids_", r.Next(4), false, false);
            return;
          }
        }
        if (this.CurrentDialogue.Count > 0 && this.CurrentDialogue.Peek().isItemGrabDialogue())
        {
          this.setTilePosition(farmHouse.getKitchenStandingSpot());
          this.spouseObstacleCheck(Game1.LoadStringByGender(this.gender, "Strings\\StringsFromCSFiles:NPC.cs.4455"), (GameLocation) farmHouse, false);
        }
        else if (!Game1.isRaining && r.NextDouble() < 0.4)
        {
          Farm locationFromName = Game1.getLocationFromName("Farm") as Farm;
          bool flag1 = false;
          if (locationFromName.getTileIndexAt(54, 7, "Buildings") == 1938)
          {
            flag1 = true;
            locationFromName.setMapTileIndex(54, 7, 1939, "Buildings", 0);
          }
          if (r.NextDouble() < 0.6 && !Game1.currentSeason.Equals("winter"))
          {
            Vector2 vector2 = Vector2.Zero;
            int num = 0;
            bool flag2 = false;
            for (; num < Math.Min(50, locationFromName.terrainFeatures.Count) && vector2.Equals(Vector2.Zero); ++num)
            {
              int index = r.Next(locationFromName.terrainFeatures.Count);
              KeyValuePair<Vector2, TerrainFeature> keyValuePair = locationFromName.terrainFeatures.ElementAt<KeyValuePair<Vector2, TerrainFeature>>(index);
              if (keyValuePair.Value is HoeDirt)
              {
                keyValuePair = locationFromName.terrainFeatures.ElementAt<KeyValuePair<Vector2, TerrainFeature>>(index);
                if ((keyValuePair.Value as HoeDirt).needsWatering())
                {
                  keyValuePair = locationFromName.terrainFeatures.ElementAt<KeyValuePair<Vector2, TerrainFeature>>(index);
                  vector2 = keyValuePair.Key;
                }
                else
                {
                  keyValuePair = locationFromName.terrainFeatures.ElementAt<KeyValuePair<Vector2, TerrainFeature>>(index);
                  if ((keyValuePair.Value as HoeDirt).crop != null)
                    flag2 = true;
                }
              }
            }
            if (!vector2.Equals(Vector2.Zero))
            {
              Microsoft.Xna.Framework.Rectangle rectangle = new Microsoft.Xna.Framework.Rectangle((int) vector2.X - 30, (int) vector2.Y - 30, 60, 60);
              Vector2 index = new Vector2();
              for (int x = rectangle.X; x < rectangle.Right; ++x)
              {
                for (int y = rectangle.Y; y < rectangle.Bottom; ++y)
                {
                  index.X = (float) x;
                  index.Y = (float) y;
                  if (locationFromName.isTileOnMap(index) && locationFromName.terrainFeatures.ContainsKey(index) && (locationFromName.terrainFeatures[index] is HoeDirt && (locationFromName.terrainFeatures[index] as HoeDirt).needsWatering()))
                    (locationFromName.terrainFeatures[index] as HoeDirt).state = 1;
                }
              }
              this.faceDirection(2);
              this.currentDialogue.Clear();
              this.setNewDialogue("MarriageDialogue", "Outdoor_", r.Next(5), false, false);
              string str1 = Game1.LoadStringByGender(this.gender, "Strings\\StringsFromCSFiles:NPC.cs.4462");
              string str2;
              if (!flag1)
                str2 = "";
              else
                str2 = Game1.content.LoadString("Strings\\StringsFromCSFiles:NPC.cs.4463", (object) Game1.player.getPetDisplayName());
              this.setNewDialogue(str1 + str2, true, false);
            }
            else
            {
              this.currentDialogue.Clear();
              this.faceDirection(2);
              if (flag2)
              {
                if ((int) Game1.gameMode == 6)
                {
                  string str1;
                  if (r.NextDouble() >= 0.5)
                    str1 = Game1.content.LoadString("Strings\\StringsFromCSFiles:NPC.cs.4466", (object) this.getTermOfSpousalEndearment(true));
                  else
                    str1 = Game1.content.LoadString("Strings\\StringsFromCSFiles:NPC.cs.4465", (object) this.getTermOfSpousalEndearment(true));
                  string str2 = Game1.content.LoadString("Strings\\StringsFromCSFiles:NPC.cs.4462");
                  string str3;
                  if (!flag1)
                    str3 = "";
                  else
                    str3 = Game1.content.LoadString("Strings\\StringsFromCSFiles:NPC.cs.4463", (object) Game1.player.getPetDisplayName());
                  this.setNewDialogue(str1 + str2 + str3, true, false);
                }
                else
                  this.setNewDialogue(Game1.LoadStringByGender(this.gender, "Strings\\StringsFromCSFiles:NPC.cs.4470"), false, false);
              }
              else
                this.setNewDialogue(this.tryToGetMarriageSpecificDialogueElseReturnDefault("Outdoor_" + (object) r.Next(5), ""), false, false);
            }
          }
          else if (r.NextDouble() < 0.6)
          {
            bool flag2 = false;
            foreach (Building building in locationFromName.buildings)
            {
              if (building is Barn || building is Coop)
              {
                (building.indoors as AnimalHouse).feedAllAnimals();
                flag2 = true;
              }
            }
            this.faceDirection(2);
            this.currentDialogue.Clear();
            if (flag2)
            {
              this.setNewDialogue("MarriageDialogue", "Outdoor_" + (object) r.Next(5), -1, false, false);
              string str1 = Game1.LoadStringByGender(this.gender, "Strings\\StringsFromCSFiles:NPC.cs.4474");
              string str2;
              if (!flag1)
                str2 = "";
              else
                str2 = Game1.content.LoadString("Strings\\StringsFromCSFiles:NPC.cs.4463", (object) Game1.player.getPetDisplayName());
              this.setNewDialogue(str1 + str2, true, false);
            }
            else
              this.setNewDialogue(this.tryToGetMarriageSpecificDialogueElseReturnDefault("Outdoor_" + (object) r.Next(5), ""), false, false);
            locationFromName.setMapTileIndex(54, 7, 1939, "Buildings", 0);
          }
          else
          {
            int num = 0;
            this.faceDirection(2);
            Vector2 vector2;
            for (vector2 = Vector2.Zero; num < Math.Min(50, locationFromName.objects.Count) && vector2.Equals(Vector2.Zero); ++num)
            {
              int index = r.Next(locationFromName.objects.Count);
              KeyValuePair<Vector2, Object> keyValuePair = locationFromName.objects.ElementAt<KeyValuePair<Vector2, Object>>(index);
              if (keyValuePair.Value is Fence)
              {
                keyValuePair = locationFromName.objects.ElementAt<KeyValuePair<Vector2, Object>>(index);
                vector2 = keyValuePair.Key;
              }
            }
            if (!vector2.Equals(Vector2.Zero))
            {
              Microsoft.Xna.Framework.Rectangle rectangle = new Microsoft.Xna.Framework.Rectangle((int) vector2.X - 10, (int) vector2.Y - 10, 20, 20);
              Vector2 index = new Vector2();
              for (int x = rectangle.X; x < rectangle.Right; ++x)
              {
                for (int y = rectangle.Y; y < rectangle.Bottom; ++y)
                {
                  index.X = (float) x;
                  index.Y = (float) y;
                  if (locationFromName.isTileOnMap(index) && locationFromName.objects.ContainsKey(index) && locationFromName.objects[index] is Fence)
                    (locationFromName.objects[index] as Fence).repair();
                }
              }
              this.currentDialogue.Clear();
              this.setNewDialogue("MarriageDialogue", "Outdoor_", r.Next(5), false, false);
              string str1 = Game1.LoadStringByGender(this.gender, "Strings\\StringsFromCSFiles:NPC.cs.4481");
              string str2;
              if (!flag1)
                str2 = "";
              else
                str2 = Game1.content.LoadString("Strings\\StringsFromCSFiles:NPC.cs.4463", (object) Game1.player.getPetDisplayName());
              this.setNewDialogue(str1 + str2, true, false);
            }
            else
            {
              this.currentDialogue.Clear();
              this.setNewDialogue(this.tryToGetMarriageSpecificDialogueElseReturnDefault("Outdoor_" + (object) r.Next(5), ""), false, false);
            }
          }
          Game1.warpCharacter(this, "Farm", farmHouse.getPorchStandingSpot(), false, false);
          this.faceDirection(2);
        }
        else if (this.daysMarried >= 1 && r.NextDouble() < 0.045)
        {
          if (r.NextDouble() < 0.75)
          {
            Point openPointInHouse = farmHouse.getRandomOpenPointInHouse(r, 1, 30);
            if (openPointInHouse.X > 0 && farmHouse.isTileLocationOpen(new Location(openPointInHouse.X - 1, openPointInHouse.Y)))
            {
              farmHouse.furniture.Add(new Furniture(Utility.getRandomSingleTileFurniture(r), new Vector2((float) openPointInHouse.X, (float) openPointInHouse.Y)));
              this.setTilePosition(openPointInHouse.X - 1, openPointInHouse.Y);
              this.faceDirection(1);
              this.setNewDialogue(Game1.content.LoadString("Strings\\StringsFromCSFiles:NPC.cs.4486", (object) this.getTermOfSpousalEndearment(true).ToLower()) + (r.NextDouble() < 0.5 ? Game1.LoadStringByGender(this.gender, "Strings\\StringsFromCSFiles:NPC.cs.4488") : Game1.content.LoadString("Strings\\StringsFromCSFiles:NPC.cs.4489")), true, true);
            }
            else
            {
              this.setTilePosition(farmHouse.getKitchenStandingSpot());
              this.spouseObstacleCheck(Game1.content.LoadString("Strings\\StringsFromCSFiles:NPC.cs.4490"), (GameLocation) farmHouse, false);
            }
          }
          else
          {
            Point openPointInHouse = farmHouse.getRandomOpenPointInHouse(r, 0, 30);
            if (openPointInHouse.X <= 0)
              return;
            this.setTilePosition(openPointInHouse.X, openPointInHouse.Y);
            this.faceDirection(0);
            if (r.NextDouble() < 0.5)
            {
              int wallForRoomAt = farmHouse.getWallForRoomAt(openPointInHouse);
              if (wallForRoomAt == -1)
                return;
              int which = r.Next(112);
              List<int> intList = new List<int>();
              string name = this.name;
              if (!(name == "Sebastian"))
              {
                if (!(name == "Haley"))
                {
                  if (!(name == "Abigail"))
                  {
                    if (!(name == "Leah"))
                    {
                      if (!(name == "Alex"))
                      {
                        if (name == "Shane")
                          intList.AddRange((IEnumerable<int>) new int[3]
                          {
                            6,
                            21,
                            60
                          });
                      }
                      else
                        intList.AddRange((IEnumerable<int>) new int[1]
                        {
                          6
                        });
                    }
                    else
                      intList.AddRange((IEnumerable<int>) new int[7]
                      {
                        44,
                        108,
                        43,
                        45,
                        92,
                        37,
                        29
                      });
                  }
                  else
                    intList.AddRange((IEnumerable<int>) new int[10]
                    {
                      2,
                      13,
                      23,
                      26,
                      46,
                      45,
                      64,
                      77,
                      106,
                      107
                    });
                }
                else
                  intList.AddRange((IEnumerable<int>) new int[7]
                  {
                    1,
                    7,
                    10,
                    35,
                    49,
                    84,
                    99
                  });
              }
              else
                intList.AddRange((IEnumerable<int>) new int[11]
                {
                  3,
                  4,
                  12,
                  14,
                  30,
                  46,
                  47,
                  56,
                  58,
                  59,
                  107
                });
              if (intList.Count > 0)
                which = intList[r.Next(intList.Count)];
              farmHouse.setWallpaper(which, wallForRoomAt, true);
              this.setNewDialogue(Game1.content.LoadString("Strings\\StringsFromCSFiles:NPC.cs.4496"), true, false);
            }
            else
            {
              int floorAt = farmHouse.getFloorAt(openPointInHouse);
              if (floorAt == -1)
                return;
              farmHouse.setFloor(r.Next(40), floorAt, true);
              this.setNewDialogue(Game1.content.LoadString("Strings\\StringsFromCSFiles:NPC.cs.4497"), true, false);
            }
          }
        }
        else if (Game1.isRaining && r.NextDouble() < 0.08 && heartLevelForNpc < 11)
        {
          foreach (Furniture furniture in farmHouse.furniture)
          {
            if (furniture.furniture_type == 13 && farmHouse.isTileLocationTotallyClearAndPlaceable((int) furniture.tileLocation.X, (int) furniture.tileLocation.Y + 1))
            {
              this.setTilePosition((int) furniture.tileLocation.X, (int) furniture.tileLocation.Y + 1);
              this.faceDirection(0);
              this.setNewDialogue(Game1.LoadStringByGender(this.gender, "Strings\\StringsFromCSFiles:NPC.cs.4498"), false, false);
              return;
            }
          }
          this.spouseObstacleCheck(Game1.content.LoadString("Strings\\StringsFromCSFiles:NPC.cs.4499"), (GameLocation) farmHouse, true);
        }
        else if (r.NextDouble() < 0.45)
        {
          Vector2 vector2 = farmHouse.upgradeLevel == 1 ? new Vector2(32f, 5f) : new Vector2(38f, 14f);
          this.setTilePosition((int) vector2.X, (int) vector2.Y);
          this.faceDirection(0);
          this.setSpouseRoomMarriageDialogue();
        }
        else
        {
          this.setTilePosition(farmHouse.getKitchenStandingSpot());
          this.faceDirection(0);
          if (r.NextDouble() >= 0.2)
            return;
          this.setRandomAfternoonMarriageDialogue(Game1.timeOfDay, (GameLocation) farmHouse, false);
        }
      }
    }

    public void clearTextAboveHead()
    {
      this.textAboveHead = (string) null;
      this.textAboveHeadPreTimer = -1;
      this.textAboveHeadTimer = -1;
    }

    public bool isVillager()
    {
      if (!this.IsMonster && !(this is Child) && (!(this is Pet) && !(this is Horse)) && !(this is Junimo))
        return !(this is JunimoHarvester);
      return false;
    }

    public override bool shouldCollideWithBuildingLayer(GameLocation location)
    {
      if (this.isMarried() && (this.Schedule == null || location is FarmHouse))
        return true;
      return base.shouldCollideWithBuildingLayer(location);
    }

    public void arriveAtFarmHouse()
    {
      if (Game1.newDay || !this.isMarried() || Game1.timeOfDay <= 630 || this.getTileLocationPoint().Equals((this.getHome() as FarmHouse).getSpouseBedSpot()))
        return;
      this.setTilePosition((this.getHome() as FarmHouse).getEntryLocation());
      this.temporaryController = (PathFindController) null;
      this.controller = (PathFindController) null;
      this.controller = new PathFindController((Character) this, this.getHome(), Game1.timeOfDay >= 2130 ? (this.getHome() as FarmHouse).getSpouseBedSpot() : (this.getHome() as FarmHouse).getKitchenStandingSpot(), 0);
      if (this.controller.pathToEndPoint == null)
      {
        this.willDestroyObjectsUnderfoot = true;
        this.controller = new PathFindController((Character) this, this.getHome(), (this.getHome() as FarmHouse).getKitchenStandingSpot(), 0);
        this.setNewDialogue(Game1.LoadStringByGender(this.gender, "Strings\\StringsFromCSFiles:NPC.cs.4500"), false, false);
      }
      else if (Game1.timeOfDay > 1300)
      {
        if (Game1.shortDayNameFromDayOfSeason(Game1.dayOfMonth).ToLower().Equals("mon") || Game1.shortDayNameFromDayOfSeason(Game1.dayOfMonth).ToLower().Equals("fri") && !this.name.Equals("Maru") && (!this.name.Equals("Penny") && !this.name.Equals("Harvey")))
          this.setNewDialogue("MarriageDialogue", "funReturn_", -1, false, true);
        else
          this.setNewDialogue("MarriageDialogue", "jobReturn_", -1, false, false);
      }
      if (!(Game1.currentLocation is FarmHouse))
        return;
      Game1.playSound("doorClose");
    }

    public Farmer getSpouse()
    {
      foreach (Farmer allFarmer in Game1.getAllFarmers())
      {
        if (allFarmer.spouse != null && allFarmer.spouse.Equals(this.name))
          return allFarmer;
      }
      return (Farmer) null;
    }

    public string getTermOfSpousalEndearment(bool happy = true)
    {
      Farmer spouse = this.getSpouse();
      if (spouse != null)
      {
        if (spouse.getFriendshipHeartLevelForNPC(this.name) < 9)
          return spouse.displayName;
        if (happy && Game1.random.NextDouble() < 0.08)
        {
          switch (Game1.random.Next(8))
          {
            case 0:
              return Game1.content.LoadString("Strings\\StringsFromCSFiles:NPC.cs.4507");
            case 1:
              return Game1.content.LoadString("Strings\\StringsFromCSFiles:NPC.cs.4508");
            case 2:
              return Game1.content.LoadString("Strings\\StringsFromCSFiles:NPC.cs.4509");
            case 3:
              return Game1.content.LoadString("Strings\\StringsFromCSFiles:NPC.cs.4510");
            case 4:
              if (!spouse.isMale)
                return Game1.content.LoadString("Strings\\StringsFromCSFiles:NPC.cs.4512");
              return Game1.content.LoadString("Strings\\StringsFromCSFiles:NPC.cs.4511");
            case 5:
              return Game1.content.LoadString("Strings\\StringsFromCSFiles:NPC.cs.4513");
            case 6:
              return Game1.content.LoadString("Strings\\StringsFromCSFiles:NPC.cs.4514");
            case 7:
              if (!spouse.isMale)
                return Game1.content.LoadString("Strings\\StringsFromCSFiles:NPC.cs.4516");
              return Game1.content.LoadString("Strings\\StringsFromCSFiles:NPC.cs.4515");
          }
        }
        if (!happy)
        {
          switch (Game1.random.Next(2))
          {
            case 0:
              return Game1.content.LoadString("Strings\\StringsFromCSFiles:NPC.cs.4517");
            case 1:
              return Game1.content.LoadString("Strings\\StringsFromCSFiles:NPC.cs.4518");
            case 2:
              return spouse.displayName;
          }
        }
        switch (Game1.random.Next(5))
        {
          case 0:
            return Game1.content.LoadString("Strings\\StringsFromCSFiles:NPC.cs.4519");
          case 1:
            return Game1.content.LoadString("Strings\\StringsFromCSFiles:NPC.cs.4518");
          case 2:
            return Game1.content.LoadString("Strings\\StringsFromCSFiles:NPC.cs.4517");
          case 3:
            return Game1.content.LoadString("Strings\\StringsFromCSFiles:NPC.cs.4522");
          case 4:
            return Game1.content.LoadString("Strings\\StringsFromCSFiles:NPC.cs.4523");
        }
      }
      return Game1.content.LoadString("Strings\\StringsFromCSFiles:NPC.cs.4517");
    }

    public bool spouseObstacleCheck(string backToBedMessage, GameLocation currentLocation, bool force = false)
    {
      if (!force && !currentLocation.isTileOccupied(this.getTileLocation(), this.name))
        return false;
      Game1.warpCharacter(this, this.defaultMap, (Game1.getLocationFromName(this.defaultMap) as FarmHouse).getSpouseBedSpot(), false, false);
      this.faceDirection(1);
      this.setNewDialogue(backToBedMessage, false, true);
      return true;
    }

    public void setTilePosition(Point p)
    {
      this.setTilePosition(p.X, p.Y);
    }

    public void setTilePosition(int x, int y)
    {
      this.position = new Vector2((float) (x * Game1.tileSize), (float) (y * Game1.tileSize));
    }

    private void clintHammerSound(Farmer who)
    {
      if (!Game1.currentLocation.name.Equals("Blacksmith"))
        return;
      Game1.playSound("hammer");
      this.sprite.currentAnimationIndex = Game1.random.Next(11);
    }

    private void robinHammerSound(Farmer who)
    {
      if (!Game1.currentLocation.Equals((object) this.currentLocation) || !Utility.isOnScreen(this.position, Game1.tileSize * 4))
        return;
      Game1.playSound(Game1.random.NextDouble() < 0.1 ? "clank" : "axchop");
      this.shakeTimer = 250;
    }

    private void robinVariablePause(Farmer who)
    {
      if (Game1.random.NextDouble() < 0.4)
        this.sprite.currentAnimation[this.sprite.currentAnimationIndex] = new FarmerSprite.AnimationFrame(27, 300, false, false, new AnimatedSprite.endOfAnimationBehavior(this.robinVariablePause), false);
      else if (Game1.random.NextDouble() < 0.25)
        this.sprite.currentAnimation[this.sprite.currentAnimationIndex] = new FarmerSprite.AnimationFrame(23, Game1.random.Next(500, 4000), false, false, new AnimatedSprite.endOfAnimationBehavior(this.robinVariablePause), false);
      else
        this.sprite.currentAnimation[this.sprite.currentAnimationIndex] = new FarmerSprite.AnimationFrame(27, Game1.random.Next(1000, 4000), false, false, new AnimatedSprite.endOfAnimationBehavior(this.robinVariablePause), false);
    }

    public void ReachedEndPoint()
    {
    }

    public void changeSchedulePathDirection()
    {
      Microsoft.Xna.Framework.Rectangle boundingBox = this.GetBoundingBox();
      boundingBox.Inflate(2, 2);
      Microsoft.Xna.Framework.Rectangle lastCrossroad = this.lastCrossroad;
      if (this.lastCrossroad.Intersects(boundingBox))
        return;
      this.isCharging = false;
      this.speed = 2;
      this.directionIndex = this.directionIndex + 1;
      this.lastCrossroad = new Microsoft.Xna.Framework.Rectangle(this.getStandingX() - this.getStandingX() % Game1.tileSize, this.getStandingY() - this.getStandingY() % Game1.tileSize, Game1.tileSize, Game1.tileSize);
      this.moveCharacterOnSchedulePath();
    }

    public void moveCharacterOnSchedulePath()
    {
    }

    public void randomSquareMovement(GameTime time)
    {
      Microsoft.Xna.Framework.Rectangle boundingBox = this.GetBoundingBox();
      boundingBox.Inflate(2, 2);
      Microsoft.Xna.Framework.Rectangle rectangle = new Microsoft.Xna.Framework.Rectangle((int) this.nextSquarePosition.X * Game1.tileSize, (int) this.nextSquarePosition.Y * Game1.tileSize, Game1.tileSize, Game1.tileSize);
      Vector2 nextSquarePosition = this.nextSquarePosition;
      if (this.nextSquarePosition.Equals(Vector2.Zero))
      {
        this.squarePauseAccumulation = 0;
        this.squarePauseTotal = Game1.random.Next(6000 + this.squarePauseOffset, 12000 + this.squarePauseOffset);
        this.nextSquarePosition = new Vector2((float) (this.lastCrossroad.X / Game1.tileSize - this.lengthOfWalkingSquareX / 2 + Game1.random.Next(this.lengthOfWalkingSquareX)), (float) (this.lastCrossroad.Y / Game1.tileSize - this.lengthOfWalkingSquareY / 2 + Game1.random.Next(this.lengthOfWalkingSquareY)));
      }
      else if (rectangle.Contains(boundingBox))
      {
        this.Halt();
        if (this.squareMovementFacingPreference != -1)
          this.faceDirection(this.squareMovementFacingPreference);
        this.isCharging = false;
        this.speed = 2;
      }
      else if (boundingBox.Left <= rectangle.Left)
        this.SetMovingOnlyRight();
      else if (boundingBox.Right >= rectangle.Right)
        this.SetMovingOnlyLeft();
      else if (boundingBox.Top <= rectangle.Top)
        this.SetMovingOnlyDown();
      else if (boundingBox.Bottom >= rectangle.Bottom)
        this.SetMovingOnlyUp();
      this.squarePauseAccumulation = this.squarePauseAccumulation + time.ElapsedGameTime.Milliseconds;
      if (this.squarePauseAccumulation < this.squarePauseTotal || !rectangle.Contains(boundingBox))
        return;
      this.nextSquarePosition = Vector2.Zero;
      this.isCharging = false;
      this.speed = 2;
    }

    public void returnToEndPoint()
    {
      Microsoft.Xna.Framework.Rectangle boundingBox = this.GetBoundingBox();
      boundingBox.Inflate(2, 2);
      if (boundingBox.Left <= this.lastCrossroad.Left)
        this.SetMovingOnlyRight();
      else if (boundingBox.Right >= this.lastCrossroad.Right)
        this.SetMovingOnlyLeft();
      else if (boundingBox.Top <= this.lastCrossroad.Top)
        this.SetMovingOnlyDown();
      else if (boundingBox.Bottom >= this.lastCrossroad.Bottom)
        this.SetMovingOnlyUp();
      boundingBox.Inflate(-2, -2);
      if (!this.lastCrossroad.Contains(boundingBox))
        return;
      this.isWalkingInSquare = false;
      this.nextSquarePosition = Vector2.Zero;
      this.returningToEndPoint = false;
      this.Halt();
      this.checkSchedule(this.timeAfterSquare);
    }

    public void SetMovingOnlyUp()
    {
      this.moveUp = true;
      this.moveDown = false;
      this.moveLeft = false;
      this.moveRight = false;
    }

    public void SetMovingOnlyRight()
    {
      this.moveUp = false;
      this.moveDown = false;
      this.moveLeft = false;
      this.moveRight = true;
    }

    public void SetMovingOnlyDown()
    {
      this.moveUp = false;
      this.moveDown = true;
      this.moveLeft = false;
      this.moveRight = false;
    }

    public void SetMovingOnlyLeft()
    {
      this.moveUp = false;
      this.moveDown = false;
      this.moveLeft = true;
      this.moveRight = false;
    }

    public static void populateRoutesFromLocationToLocationList()
    {
      NPC.routesFromLocationToLocation = new List<List<string>>();
      foreach (GameLocation location in Game1.locations)
      {
        if (!(location is Farm) && !location.name.Equals("Backwoods"))
        {
          List<string> route = new List<string>();
          NPC.exploreWarpPoints(location, route);
        }
      }
    }

    private static bool exploreWarpPoints(GameLocation l, List<string> route)
    {
      bool flag = false;
      if (l != null && !route.Contains(l.name))
      {
        route.Add(l.name);
        if (route.Count == 1 || !NPC.doesRoutesListContain(route))
        {
          if (route.Count > 1)
          {
            NPC.routesFromLocationToLocation.Add(route.ToList<string>());
            flag = true;
          }
          foreach (Warp warp in l.warps)
          {
            if (!warp.TargetName.Equals("Farm") && !warp.TargetName.Equals("Woods") && (!warp.TargetName.Equals("Backwoods") && !warp.TargetName.Equals("Tunnel")))
              NPC.exploreWarpPoints(Game1.getLocationFromName(warp.TargetName), route);
          }
          foreach (Point key in l.doors.Keys)
            NPC.exploreWarpPoints(Game1.getLocationFromName(l.doors[key]), route);
        }
        if (route.Count > 0)
          route.RemoveAt(route.Count - 1);
      }
      return flag;
    }

    private static bool doesRoutesListContain(List<string> route)
    {
      foreach (List<string> stringList in NPC.routesFromLocationToLocation)
      {
        if (stringList.Count == route.Count)
        {
          bool flag = true;
          for (int index = 0; index < route.Count; ++index)
          {
            if (!stringList[index].Equals(route[index]))
            {
              flag = false;
              break;
            }
          }
          if (flag)
            return true;
        }
      }
      return false;
    }

    public int CompareTo(object obj)
    {
      if (obj is NPC)
        return (obj as NPC).id - this.id;
      return 0;
    }
  }
}
