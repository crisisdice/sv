// Decompiled with JetBrains decompiler
// Type: StardewValley.Object
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.Locations;
using StardewValley.Minigames;
using StardewValley.Monsters;
using StardewValley.Objects;
using StardewValley.TerrainFeatures;
using StardewValley.Tools;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using xTile.Dimensions;

namespace StardewValley
{
  [XmlInclude(typeof (Fence))]
  [XmlInclude(typeof (Torch))]
  [XmlInclude(typeof (SpecialItem))]
  [XmlInclude(typeof (Wallpaper))]
  [XmlInclude(typeof (Boots))]
  [XmlInclude(typeof (Hat))]
  [XmlInclude(typeof (Ring))]
  [XmlInclude(typeof (TV))]
  [XmlInclude(typeof (CrabPot))]
  [XmlInclude(typeof (Chest))]
  [XmlInclude(typeof (Door))]
  [XmlInclude(typeof (Cask))]
  [XmlInclude(typeof (SwitchFloor))]
  [XmlInclude(typeof (WickedStatue))]
  [XmlInclude(typeof (ColoredObject))]
  public class Object : Item
  {
    public bool canBeGrabbed = true;
    public bool isOn = true;
    public int edibility = -300;
    public int stack = 1;
    protected int health = 10;
    public const int copperBar = 334;
    public const int ironBar = 335;
    public const int goldBar = 336;
    public const int iridiumBar = 337;
    public const int wood = 388;
    public const int stone = 390;
    public const int copper = 378;
    public const int iron = 380;
    public const int coal = 382;
    public const int gold = 384;
    public const int iridium = 386;
    public const int inedible = -300;
    public const int GreensCategory = -81;
    public const int GemCategory = -2;
    public const int VegetableCategory = -75;
    public const int FishCategory = -4;
    public const int EggCategory = -5;
    public const int MilkCategory = -6;
    public const int CookingCategory = -7;
    public const int CraftingCategory = -8;
    public const int BigCraftableCategory = -9;
    public const int FruitsCategory = -79;
    public const int SeedsCategory = -74;
    public const int mineralsCategory = -12;
    public const int flowersCategory = -80;
    public const int meatCategory = -14;
    public const int metalResources = -15;
    public const int buildingResources = -16;
    public const int sellAtPierres = -17;
    public const int sellAtPierresAndMarnies = -18;
    public const int fertilizerCategory = -19;
    public const int junkCategory = -20;
    public const int baitCategory = -21;
    public const int tackleCategory = -22;
    public const int sellAtFishShopCategory = -23;
    public const int furnitureCategory = -24;
    public const int ingredientsCategory = -25;
    public const int artisanGoodsCategory = -26;
    public const int syrupCategory = -27;
    public const int monsterLootCategory = -28;
    public const int equipmentCategory = -29;
    public const int hatCategory = -95;
    public const int ringCategory = -96;
    public const int weaponCategory = -98;
    public const int bootsCategory = -97;
    public const int toolCategory = -99;
    public const int objectInfoNameIndex = 0;
    public const int objectInfoPriceIndex = 1;
    public const int objectInfoEdibilityIndex = 2;
    public const int objectInfoTypeIndex = 3;
    public const int objectInfoDisplayNameIndex = 4;
    public const int objectInfoDescriptionIndex = 5;
    public const int objectInfoMiscIndex = 6;
    public const int objectInfoBuffTypesIndex = 7;
    public const int objectInfoBuffDurationIndex = 8;
    public const int WeedsIndex = 0;
    public const int StoneIndex = 2;
    public const int StickIndex = 4;
    public const int DryDirtTileIndex = 6;
    public const int WateredTileIndex = 7;
    public const int StumpTopLeftIndex = 8;
    public const int BoulderTopLeftIndex = 10;
    public const int StumpBottomLeftIndex = 12;
    public const int BoulderBottomLeftIndex = 14;
    public const int WildHorseradishIndex = 16;
    public const int TulipIndex = 18;
    public const int LeekIndex = 20;
    public const int DandelionIndex = 22;
    public const int ParsnipIndex = 24;
    public const int HandCursorIndex = 26;
    public const int WaterAnimationIndex = 28;
    public const int LumberIndex = 30;
    public const int mineStoneGrey1Index = 32;
    public const int mineStoneBlue1Index = 34;
    public const int mineStoneBlue2Index = 36;
    public const int mineStoneGrey2Index = 38;
    public const int mineStoneBrown1Index = 40;
    public const int mineStoneBrown2Index = 42;
    public const int mineStonePurpleIndex = 44;
    public const int mineStoneMysticIndex = 46;
    public const int mineStoneSnow1 = 48;
    public const int mineStoneSnow2 = 50;
    public const int mineStoneSnow3 = 52;
    public const int mineStonePurpleSnowIndex = 54;
    public const int mineStoneRed1Index = 56;
    public const int mineStoneRed2Index = 58;
    public const int emeraldIndex = 60;
    public const int aquamarineIndex = 62;
    public const int rubyIndex = 64;
    public const int amethystClusterIndex = 66;
    public const int topazIndex = 68;
    public const int sapphireIndex = 70;
    public const int diamondIndex = 72;
    public const int prismaticShardIndex = 74;
    public const int snowHoedDirtIndex = 76;
    public const int beachHoedDirtIndex = 77;
    public const int caveCarrotIndex = 78;
    public const int quartzIndex = 80;
    public const int bobberIndex = 133;
    public const int stardrop = 434;
    public const int spriteSheetTileSize = 16;
    public const int lowQuality = 0;
    public const int medQuality = 1;
    public const int highQuality = 2;
    public const int bestQuality = 4;
    public const int copperPerBar = 10;
    public const int ironPerBar = 10;
    public const int goldPerBar = 10;
    public const int iridiumPerBar = 10;
    public const float wobbleAmountWhenWorking = 10f;
    public const int fragility_Removable = 0;
    public const int fragility_Delicate = 1;
    public const int fragility_Indestructable = 2;
    public Vector2 tileLocation;
    public int parentSheetIndex;
    public long owner;
    public string name;
    public string type;
    public bool canBeSetDown;
    public bool isHoedirt;
    public bool isSpawnedObject;
    public bool questItem;
    public int fragility;
    private bool isActive;
    public int price;
    public int quality;
    public bool bigCraftable;
    public bool setOutdoors;
    public bool setIndoors;
    public bool readyForHarvest;
    public bool showNextIndex;
    public bool flipped;
    public bool hasBeenPickedUpByFarmer;
    public bool isRecipe;
    public bool isLamp;
    public Object heldObject;
    public int minutesUntilReady;
    public Microsoft.Xna.Framework.Rectangle boundingBox;
    public Vector2 scale;
    [XmlIgnore]
    public LightSource lightSource;
    [XmlIgnore]
    public int shakeTimer;
    [XmlIgnore]
    public Cue internalSound;
    public Object.PreserveType? preserve;
    public int preservedParentSheetIndex;
    public Object.HoneyType? honeyType;
    [XmlIgnore]
    public string displayName;

    [XmlIgnore]
    public int ParentSheetIndex
    {
      get
      {
        return this.parentSheetIndex;
      }
      set
      {
        this.parentSheetIndex = value;
      }
    }

    [XmlIgnore]
    public Vector2 TileLocation
    {
      get
      {
        return this.tileLocation;
      }
      set
      {
        this.tileLocation = value;
      }
    }

    [XmlIgnore]
    public override string DisplayName
    {
      get
      {
        if (Game1.objectInformation != null)
          this.displayName = this.loadDisplayName();
        return this.displayName + (this.isRecipe ? Game1.content.LoadString("Strings\\StringsFromCSFiles:Object.cs.12657") : "");
      }
      set
      {
        this.displayName = value;
      }
    }

    [XmlIgnore]
    public override string Name
    {
      get
      {
        return this.name + (this.isRecipe ? " Recipe" : "");
      }
    }

    [XmlIgnore]
    public string Type
    {
      get
      {
        return this.type;
      }
      set
      {
        this.type = value;
      }
    }

    [XmlIgnore]
    public override int Stack
    {
      get
      {
        return Math.Max(0, this.stack);
      }
      set
      {
        this.stack = Math.Min(Math.Max(0, value), this.maximumStackSize());
      }
    }

    [XmlIgnore]
    public int Category
    {
      get
      {
        return this.category;
      }
      set
      {
        this.category = value;
      }
    }

    [XmlIgnore]
    public bool CanBeSetDown
    {
      get
      {
        return this.canBeSetDown;
      }
      set
      {
        this.canBeSetDown = value;
      }
    }

    [XmlIgnore]
    public bool CanBeGrabbed
    {
      get
      {
        return this.canBeGrabbed;
      }
    }

    [XmlIgnore]
    public bool IsHoeDirt
    {
      get
      {
        return this.isHoedirt;
      }
    }

    [XmlIgnore]
    public bool IsSpawnedObject
    {
      get
      {
        return this.isSpawnedObject;
      }
    }

    [XmlIgnore]
    public int Price
    {
      get
      {
        return this.price;
      }
      set
      {
        this.price = value;
      }
    }

    [XmlIgnore]
    public int Edibility
    {
      get
      {
        return this.edibility;
      }
      set
      {
        this.edibility = value;
      }
    }

    public Object()
    {
    }

    public Object(Vector2 tileLocation, int parentSheetIndex, bool isRecipe = false)
    {
      this.isRecipe = isRecipe;
      this.tileLocation = tileLocation;
      this.parentSheetIndex = parentSheetIndex;
      this.canBeSetDown = true;
      this.bigCraftable = true;
      string str;
      Game1.bigCraftablesInformation.TryGetValue(parentSheetIndex, out str);
      if (str != null)
      {
        string[] strArray1 = str.Split('/');
        this.name = strArray1[0];
        this.price = Convert.ToInt32(strArray1[1]);
        this.edibility = Convert.ToInt32(strArray1[2]);
        string[] strArray2 = strArray1[3].Split(' ');
        this.type = strArray2[0];
        if (strArray2.Length > 1)
          this.category = Convert.ToInt32(strArray2[1]);
        this.setOutdoors = Convert.ToBoolean(strArray1[5]);
        this.setIndoors = Convert.ToBoolean(strArray1[6]);
        this.fragility = Convert.ToInt32(strArray1[7]);
        this.isLamp = strArray1.Length > 8 && strArray1[8].Equals("true");
      }
      this.scale = new Vector2(0.0f, 5f);
      this.initializeLightSource(this.tileLocation);
      this.boundingBox = new Microsoft.Xna.Framework.Rectangle((int) tileLocation.X * Game1.tileSize, (int) tileLocation.Y * Game1.tileSize, Game1.tileSize, Game1.tileSize);
    }

    public Object(int parentSheetIndex, int initialStack, bool isRecipe = false, int price = -1, int quality = 0)
      : this(Vector2.Zero, parentSheetIndex, initialStack)
    {
      this.isRecipe = isRecipe;
      if (price != -1)
        this.price = price;
      this.quality = quality;
    }

    public Object(Vector2 tileLocation, int parentSheetIndex, int initialStack)
      : this(tileLocation, parentSheetIndex, (string) null, true, true, false, false)
    {
      this.stack = initialStack;
    }

    public Object(Vector2 tileLocation, int parentSheetIndex, string Givenname, bool canBeSetDown, bool canBeGrabbed, bool isHoedirt, bool isSpawnedObject)
    {
      this.tileLocation = tileLocation;
      this.parentSheetIndex = parentSheetIndex;
      string str;
      Game1.objectInformation.TryGetValue(parentSheetIndex, out str);
      try
      {
        if (str != null)
        {
          string[] strArray1 = str.Split('/');
          this.name = strArray1[0];
          this.price = Convert.ToInt32(strArray1[1]);
          this.edibility = Convert.ToInt32(strArray1[2]);
          string[] strArray2 = strArray1[3].Split(' ');
          this.type = strArray2[0];
          if (strArray2.Length > 1)
            this.category = Convert.ToInt32(strArray2[1]);
        }
      }
      catch (Exception ex)
      {
      }
      if (this.name == null && Givenname != null)
        this.name = Givenname;
      else if (this.name == null)
        this.name = "Error Item";
      this.canBeSetDown = canBeSetDown;
      this.canBeGrabbed = canBeGrabbed;
      this.isHoedirt = isHoedirt;
      this.isSpawnedObject = isSpawnedObject;
      if (Game1.random.NextDouble() < 0.5 && parentSheetIndex > 52 && (parentSheetIndex < 8 || parentSheetIndex > 15) && (parentSheetIndex < 384 || parentSheetIndex > 391))
        this.flipped = true;
      if (this.name.Contains("Block"))
        this.scale = new Vector2(1f, 1f);
      if (parentSheetIndex == 449 || this.name.Contains("Weed") || this.name.Contains("Twig"))
        this.fragility = 2;
      else if (this.name.Contains("Fence"))
      {
        this.scale = new Vector2(10f, 0.0f);
        canBeSetDown = false;
      }
      else if (this.name.Contains("Stone"))
      {
        switch (parentSheetIndex)
        {
          case 8:
            this.minutesUntilReady = 4;
            break;
          case 10:
            this.minutesUntilReady = 8;
            break;
          case 12:
            this.minutesUntilReady = 16;
            break;
          case 14:
            this.minutesUntilReady = 12;
            break;
          default:
            this.minutesUntilReady = 1;
            break;
        }
      }
      if (parentSheetIndex >= 75 && parentSheetIndex <= 77)
        isSpawnedObject = false;
      this.initializeLightSource(this.tileLocation);
      if (this.category == -22)
        this.scale.Y = 1f;
      this.boundingBox = new Microsoft.Xna.Framework.Rectangle((int) tileLocation.X * Game1.tileSize, (int) tileLocation.Y * Game1.tileSize, Game1.tileSize, Game1.tileSize);
    }

    protected virtual string loadDisplayName()
    {
      if (LocalizedContentManager.CurrentLanguageCode != LocalizedContentManager.LanguageCode.en)
      {
        if (this.preserve.HasValue)
        {
          string str1;
          Game1.objectInformation.TryGetValue(this.preservedParentSheetIndex, out str1);
          if (!string.IsNullOrEmpty(str1))
          {
            string str2 = str1.Split('/')[4];
            Object.PreserveType? preserve = this.preserve;
            if (preserve.HasValue)
            {
              switch (preserve.GetValueOrDefault())
              {
                case Object.PreserveType.Wine:
                  return Game1.content.LoadString("Strings\\StringsFromCSFiles:Object.cs.12730", (object) str2);
                case Object.PreserveType.Jelly:
                  return Game1.content.LoadString("Strings\\StringsFromCSFiles:Object.cs.12739", (object) str2);
                case Object.PreserveType.Pickle:
                  return Game1.content.LoadString("Strings\\StringsFromCSFiles:Object.cs.12735", (object) str2);
                case Object.PreserveType.Juice:
                  return Game1.content.LoadString("Strings\\StringsFromCSFiles:Object.cs.12726", (object) str2);
              }
            }
          }
        }
        else
        {
          if (this.name != null && this.name.Contains("Honey"))
          {
            string name = this.name;
            if (!(name == "Wild Honey"))
            {
              if (!(name == "Poppy Honey"))
              {
                if (!(name == "Tulip Honey"))
                {
                  if (!(name == "Summer Spangle Honey"))
                  {
                    if (!(name == "Fairy Rose Honey"))
                    {
                      if (!(name == "Blue Jazz Honey"))
                        return Game1.content.LoadString("Strings\\StringsFromCSFiles:Object.cs.12750");
                      this.honeyType = new Object.HoneyType?(Object.HoneyType.BlueJazz);
                    }
                    else
                      this.honeyType = new Object.HoneyType?(Object.HoneyType.FairyRose);
                  }
                  else
                    this.honeyType = new Object.HoneyType?(Object.HoneyType.SummerSpangle);
                }
                else
                  this.honeyType = new Object.HoneyType?(Object.HoneyType.Tulip);
              }
              else
                this.honeyType = new Object.HoneyType?(Object.HoneyType.Poppy);
              string str = Game1.objectInformation[(int) this.honeyType.Value].Split('/')[4];
              return Game1.content.LoadString("Strings\\StringsFromCSFiles:Object.cs.12760", (object) str);
            }
            this.honeyType = new Object.HoneyType?(Object.HoneyType.Wild);
            return Game1.content.LoadString("Strings\\StringsFromCSFiles:Object.cs.12750");
          }
          if (this.bigCraftable)
          {
            string str;
            Game1.bigCraftablesInformation.TryGetValue(this.parentSheetIndex, out str);
            if (!string.IsNullOrEmpty(str))
            {
              string[] strArray = str.Split('/');
              int index = strArray.Length - 1;
              return strArray[index];
            }
          }
          else
          {
            string str;
            Game1.objectInformation.TryGetValue(this.parentSheetIndex, out str);
            if (!string.IsNullOrEmpty(str))
              return str.Split('/')[4];
          }
        }
      }
      return this.name;
    }

    public Vector2 getLocalPosition(xTile.Dimensions.Rectangle viewport)
    {
      return new Vector2(this.tileLocation.X * (float) Game1.tileSize - (float) viewport.X, this.tileLocation.Y * (float) Game1.tileSize - (float) viewport.Y);
    }

    public static Microsoft.Xna.Framework.Rectangle getSourceRectForBigCraftable(int index)
    {
      return new Microsoft.Xna.Framework.Rectangle(index % (Game1.bigCraftableSpriteSheet.Width / 16) * 16, index * 16 / Game1.bigCraftableSpriteSheet.Width * 16 * 2, 16, 32);
    }

    public virtual bool performToolAction(Tool t)
    {
      if (t == null)
      {
        if (Game1.currentLocation.objects.ContainsKey(this.tileLocation) && Game1.currentLocation.objects[this.tileLocation].Equals((object) this))
        {
          Game1.createRadialDebris(Game1.currentLocation, 12, (int) this.tileLocation.X, (int) this.tileLocation.Y, Game1.random.Next(4, 10), false, -1, false, -1);
          Game1.currentLocation.objects.Remove(this.tileLocation);
        }
        return false;
      }
      GameLocation currentLocation = t.getLastFarmerToUse().currentLocation;
      if (this.name.Equals("Stone") && t.GetType() == typeof (Pickaxe))
      {
        int num = 1;
        switch (t.upgradeLevel)
        {
          case 1:
            num = 2;
            break;
          case 2:
            num = 3;
            break;
          case 3:
            num = 4;
            break;
          case 4:
            num = 5;
            break;
        }
        if (this.parentSheetIndex == 12 && t.upgradeLevel == 1 || (this.parentSheetIndex == 12 || this.parentSheetIndex == 14) && t.upgradeLevel == 0)
        {
          num = 0;
          Game1.playSound("crafting");
        }
        this.minutesUntilReady = this.minutesUntilReady - num;
        if (this.minutesUntilReady <= 0)
          return true;
        Game1.playSound("hammer");
        this.shakeTimer = 100;
        return false;
      }
      if (this.name.Equals("Stone") && t.GetType() == typeof (Pickaxe))
        return false;
      if (this.name.Equals("Boulder") && (t.upgradeLevel != 4 || !(t is Pickaxe)))
      {
        if (t.isHeavyHitter())
          Game1.playSound("hammer");
        return false;
      }
      if (this.name.Contains("Weeds") && t.isHeavyHitter())
      {
        this.cutWeed(t.getLastFarmerToUse());
        return true;
      }
      if (this.name.Contains("Twig") && t is Axe)
      {
        this.fragility = 2;
        Game1.playSound("axchop");
        t.getLastFarmerToUse().currentLocation.debris.Add(new Debris((Item) new Object(388, 1, false, -1, 0), this.tileLocation * (float) Game1.tileSize + new Vector2((float) (Game1.tileSize / 2), (float) (Game1.tileSize / 2))));
        Game1.createRadialDebris(currentLocation, 12, (int) this.tileLocation.X, (int) this.tileLocation.Y, Game1.random.Next(4, 10), false, -1, false, -1);
        currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(12, new Vector2(this.tileLocation.X * (float) Game1.tileSize, this.tileLocation.Y * (float) Game1.tileSize), Color.White, 8, Game1.random.NextDouble() < 0.5, 50f, 0, -1, -1f, -1, 0));
        return true;
      }
      if (this.parentSheetIndex == 590)
      {
        if (t is Hoe)
        {
          currentLocation.digUpArtifactSpot((int) this.tileLocation.X, (int) this.tileLocation.Y, t.getLastFarmerToUse());
          if (!currentLocation.terrainFeatures.ContainsKey(this.tileLocation))
            currentLocation.terrainFeatures.Add(this.tileLocation, (TerrainFeature) new HoeDirt());
          Game1.playSound("hoeHit");
          if (currentLocation.objects.ContainsKey(this.tileLocation))
            currentLocation.objects.Remove(this.tileLocation);
        }
        return false;
      }
      if (this.fragility == 2 || this.type == null || (!this.type.Equals("Crafting") || !(t.GetType() != typeof (MeleeWeapon))) || !t.isHeavyHitter())
        return false;
      Game1.playSound("hammer");
      if (this.fragility == 1)
      {
        Game1.createRadialDebris(currentLocation, 12, (int) this.tileLocation.X, (int) this.tileLocation.Y, Game1.random.Next(4, 10), false, -1, false, -1);
        if (currentLocation.objects.ContainsKey(this.tileLocation))
          currentLocation.objects.Remove(this.tileLocation);
        return false;
      }
      if (this.name.Equals("Tapper") && t.getLastFarmerToUse().currentLocation.terrainFeatures.ContainsKey(this.tileLocation) && t.getLastFarmerToUse().currentLocation.terrainFeatures[this.tileLocation] is Tree)
        (t.getLastFarmerToUse().currentLocation.terrainFeatures[this.tileLocation] as Tree).tapped = false;
      if (this.heldObject != null && this.readyForHarvest)
        t.getLastFarmerToUse().currentLocation.debris.Add(new Debris((Item) this.heldObject, this.tileLocation * (float) Game1.tileSize + new Vector2((float) (Game1.tileSize / 2), (float) (Game1.tileSize / 2))));
      if (this.parentSheetIndex == 157)
      {
        this.parentSheetIndex = 156;
        this.heldObject = (Object) null;
        this.minutesUntilReady = -1;
      }
      return true;
    }

    private void cutWeed(Farmer who)
    {
      Color color = Color.Green;
      string cueName = "cut";
      int rowInAnimationTexture = 50;
      this.fragility = 2;
      int parentSheetIndex = -1;
      if (Game1.random.NextDouble() < 0.5)
        parentSheetIndex = 771;
      else if (Game1.random.NextDouble() < 0.05)
        parentSheetIndex = 770;
      switch (this.parentSheetIndex)
      {
        case 679:
          color = new Color(253, 191, 46);
          break;
        case 792:
        case 793:
        case 794:
          parentSheetIndex = 770;
          break;
        case 313:
        case 314:
        case 315:
          color = new Color(84, 101, 27);
          break;
        case 316:
        case 317:
        case 318:
          color = new Color(109, 49, 196);
          break;
        case 319:
          color = new Color(30, 216, (int) byte.MaxValue);
          cueName = "breakingGlass";
          rowInAnimationTexture = 47;
          Game1.playSound("drumkit2");
          parentSheetIndex = -1;
          break;
        case 320:
          color = new Color(175, 143, (int) byte.MaxValue);
          cueName = "breakingGlass";
          rowInAnimationTexture = 47;
          Game1.playSound("drumkit2");
          parentSheetIndex = -1;
          break;
        case 321:
          color = new Color(73, (int) byte.MaxValue, 158);
          cueName = "breakingGlass";
          rowInAnimationTexture = 47;
          Game1.playSound("drumkit2");
          parentSheetIndex = -1;
          break;
        case 678:
          color = new Color(228, 109, 159);
          break;
      }
      if (cueName.Equals("breakingGlass") && Game1.random.NextDouble() < 1.0 / 400.0)
        parentSheetIndex = 338;
      Game1.playSound(cueName);
      who.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(rowInAnimationTexture, this.tileLocation * (float) Game1.tileSize, color, 8, false, 100f, 0, -1, -1f, -1, 0));
      who.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(rowInAnimationTexture, this.tileLocation * (float) Game1.tileSize + new Vector2((float) Game1.random.Next(-Game1.tileSize / 4, Game1.tileSize / 4), (float) Game1.random.Next(-Game1.tileSize * 3 / 4, Game1.tileSize * 3 / 4)), color * 0.75f, 8, false, 100f, 0, -1, -1f, -1, 0)
      {
        scale = 0.75f,
        flipped = true
      });
      who.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(rowInAnimationTexture, this.tileLocation * (float) Game1.tileSize + new Vector2((float) Game1.random.Next(-Game1.tileSize / 4, Game1.tileSize / 4), (float) Game1.random.Next(-Game1.tileSize * 3 / 4, Game1.tileSize * 3 / 4)), color * 0.75f, 8, false, 100f, 0, -1, -1f, -1, 0)
      {
        scale = 0.75f,
        delayBeforeAnimationStart = 50
      });
      who.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(rowInAnimationTexture, this.tileLocation * (float) Game1.tileSize + new Vector2((float) Game1.random.Next(-Game1.tileSize / 4, Game1.tileSize / 4), (float) Game1.random.Next(-Game1.tileSize * 3 / 4, Game1.tileSize * 3 / 4)), color * 0.75f, 8, false, 100f, 0, -1, -1f, -1, 0)
      {
        scale = 0.75f,
        flipped = true,
        delayBeforeAnimationStart = 100
      });
      if (parentSheetIndex != -1)
        who.currentLocation.debris.Add(new Debris((Item) new Object(parentSheetIndex, 1, false, -1, 0), this.tileLocation * (float) Game1.tileSize + new Vector2((float) (Game1.tileSize / 2), (float) (Game1.tileSize / 2))));
      if (Game1.random.NextDouble() >= 0.02)
        return;
      who.currentLocation.addJumperFrog(this.tileLocation);
    }

    public bool isAnimalProduct()
    {
      if (this.Category != -18 && this.category != -5 && this.category != -6)
        return this.parentSheetIndex == 430;
      return true;
    }

    public virtual bool onExplosion(Farmer who, GameLocation location)
    {
      if (who == null)
        return false;
      if (this.name.Contains("Weed"))
      {
        this.fragility = 0;
        this.cutWeed(who);
        location.removeObject(this.tileLocation, false);
      }
      if (this.name.Contains("Twig"))
      {
        this.fragility = 0;
        Game1.createRadialDebris(location, 12, (int) this.tileLocation.X, (int) this.tileLocation.Y, Game1.random.Next(4, 10), false, -1, false, -1);
        location.debris.Add(new Debris((Item) new Object(388, 1, false, -1, 0), this.tileLocation * (float) Game1.tileSize + new Vector2((float) (Game1.tileSize / 2), (float) (Game1.tileSize / 2))));
      }
      if (this.name.Contains("Stone"))
      {
        this.fragility = 0;
        Game1.createRadialDebris(location, 14, (int) this.tileLocation.X, (int) this.tileLocation.Y, Game1.random.Next(4, 10), false, -1, false, -1);
        if (Game1.random.NextDouble() < 0.5)
          location.debris.Add(new Debris((Item) new Object(390, 1, false, -1, 0), this.tileLocation * (float) Game1.tileSize + new Vector2((float) (Game1.tileSize / 2), (float) (Game1.tileSize / 2))));
      }
      this.performRemoveAction(this.tileLocation, who.currentLocation);
      return true;
    }

    public bool canBeShipped()
    {
      if (!this.bigCraftable && this.type != null && !this.type.Equals("Quest"))
        return this.canBeTrashed();
      return false;
    }

    public virtual void DayUpdate(GameLocation location)
    {
      this.health = 10;
      if ((this.parentSheetIndex == 599 || this.parentSheetIndex == 621 || this.parentSheetIndex == 645) && (!Game1.isRaining || !location.isOutdoors))
      {
        switch (this.parentSheetIndex)
        {
          case 599:
            foreach (Vector2 adjacentTileLocation in Utility.getAdjacentTileLocations(this.tileLocation))
            {
              if (location.terrainFeatures.ContainsKey(adjacentTileLocation) && location.terrainFeatures[adjacentTileLocation] is HoeDirt)
                (location.terrainFeatures[adjacentTileLocation] as HoeDirt).state = 1;
            }
            int num1 = Game1.random.Next(1000);
            List<TemporaryAnimatedSprite> temporarySprites1 = location.temporarySprites;
            TemporaryAnimatedSprite temporaryAnimatedSprite1 = new TemporaryAnimatedSprite(29, this.tileLocation * (float) Game1.tileSize + new Vector2(0.0f, (float) (-Game1.tileSize * 3 / 4)), Color.White * 0.5f, 4, false, 60f, 100, -1, -1f, -1, 0);
            temporaryAnimatedSprite1.delayBeforeAnimationStart = num1;
            double num2 = (double) this.tileLocation.X * 4000.0 + (double) this.tileLocation.Y;
            temporaryAnimatedSprite1.id = (float) num2;
            temporarySprites1.Add(temporaryAnimatedSprite1);
            List<TemporaryAnimatedSprite> temporarySprites2 = location.temporarySprites;
            TemporaryAnimatedSprite temporaryAnimatedSprite2 = new TemporaryAnimatedSprite(29, this.tileLocation * (float) Game1.tileSize + new Vector2((float) (Game1.tileSize * 3 / 4), 0.0f), Color.White * 0.5f, 4, false, 60f, 100, -1, -1f, -1, 0);
            temporaryAnimatedSprite2.rotation = 1.570796f;
            temporaryAnimatedSprite2.delayBeforeAnimationStart = num1;
            double num3 = (double) this.tileLocation.X * 4000.0 + (double) this.tileLocation.Y;
            temporaryAnimatedSprite2.id = (float) num3;
            temporarySprites2.Add(temporaryAnimatedSprite2);
            List<TemporaryAnimatedSprite> temporarySprites3 = location.temporarySprites;
            TemporaryAnimatedSprite temporaryAnimatedSprite3 = new TemporaryAnimatedSprite(29, this.tileLocation * (float) Game1.tileSize + new Vector2(0.0f, (float) (Game1.tileSize * 3 / 4)), Color.White * 0.5f, 4, false, 60f, 100, -1, -1f, -1, 0);
            temporaryAnimatedSprite3.rotation = 3.141593f;
            temporaryAnimatedSprite3.delayBeforeAnimationStart = num1;
            double num4 = (double) this.tileLocation.X * 4000.0 + (double) this.tileLocation.Y;
            temporaryAnimatedSprite3.id = (float) num4;
            temporarySprites3.Add(temporaryAnimatedSprite3);
            List<TemporaryAnimatedSprite> temporarySprites4 = location.temporarySprites;
            TemporaryAnimatedSprite temporaryAnimatedSprite4 = new TemporaryAnimatedSprite(29, this.tileLocation * (float) Game1.tileSize + new Vector2((float) (-Game1.tileSize * 3 / 4), 0.0f), Color.White * 0.5f, 4, false, 60f, 100, -1, -1f, -1, 0);
            temporaryAnimatedSprite4.rotation = 4.712389f;
            temporaryAnimatedSprite4.delayBeforeAnimationStart = num1;
            double num5 = (double) this.tileLocation.X * 4000.0 + (double) this.tileLocation.Y;
            temporaryAnimatedSprite4.id = (float) num5;
            temporarySprites4.Add(temporaryAnimatedSprite4);
            break;
          case 621:
            foreach (Vector2 surroundingTileLocations in Utility.getSurroundingTileLocationsArray(this.tileLocation))
            {
              if (location.terrainFeatures.ContainsKey(surroundingTileLocations) && location.terrainFeatures[surroundingTileLocations] is HoeDirt)
                (location.terrainFeatures[surroundingTileLocations] as HoeDirt).state = 1;
            }
            location.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.animations, new Microsoft.Xna.Framework.Rectangle(0, 1984, Game1.tileSize * 3, Game1.tileSize * 3), 60f, 3, 100, this.tileLocation * (float) Game1.tileSize + new Vector2((float) -Game1.tileSize, (float) -Game1.tileSize), false, false)
            {
              color = Color.White * 0.4f,
              delayBeforeAnimationStart = Game1.random.Next(1000),
              id = (float) ((double) this.tileLocation.X * 4000.0 + (double) this.tileLocation.Y)
            });
            break;
          case 645:
            for (int index1 = (int) this.tileLocation.X - 2; (double) index1 <= (double) this.tileLocation.X + 2.0; ++index1)
            {
              for (int index2 = (int) this.tileLocation.Y - 2; (double) index2 <= (double) this.tileLocation.Y + 2.0; ++index2)
              {
                Vector2 key = new Vector2((float) index1, (float) index2);
                if (location.terrainFeatures.ContainsKey(key) && location.terrainFeatures[key] is HoeDirt)
                  (location.terrainFeatures[key] as HoeDirt).state = 1;
              }
            }
            location.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.animations, new Microsoft.Xna.Framework.Rectangle(0, 2176, Game1.tileSize * 5, Game1.tileSize * 5), 60f, 4, 100, this.tileLocation * (float) Game1.tileSize + new Vector2((float) (-Game1.tileSize * 3 + Game1.tileSize), (float) (-Game1.tileSize * 2)), false, false)
            {
              color = Color.White * 0.4f,
              delayBeforeAnimationStart = Game1.random.Next(1000),
              id = (float) ((double) this.tileLocation.X * 4000.0 + (double) this.tileLocation.Y)
            });
            break;
        }
      }
      if (this.bigCraftable)
      {
        switch (this.parentSheetIndex)
        {
          case 128:
            if (this.heldObject == null)
            {
              this.heldObject = new Object(Game1.random.NextDouble() >= 0.025 ? (Game1.random.NextDouble() >= 0.075 ? (Game1.random.NextDouble() >= 0.09 ? (Game1.random.NextDouble() >= 0.15 ? 404 : 420) : 257) : 281) : 422, 1, false, -1, 0);
              this.minutesUntilReady = 3000 - Game1.timeOfDay;
              break;
            }
            break;
          case 157:
            if (this.minutesUntilReady <= 0 && this.heldObject != null && location.canSlimeHatchHere())
            {
              GreenSlime greenSlime = (GreenSlime) null;
              Vector2 position = new Vector2((float) (int) this.tileLocation.X, (float) ((int) this.tileLocation.Y + 1)) * (float) Game1.tileSize;
              switch (this.heldObject.parentSheetIndex)
              {
                case 439:
                  greenSlime = new GreenSlime(position, 121);
                  break;
                case 680:
                  greenSlime = new GreenSlime(position, 0);
                  break;
                case 413:
                  greenSlime = new GreenSlime(position, 40);
                  break;
                case 437:
                  greenSlime = new GreenSlime(position, 80);
                  break;
              }
              if (greenSlime != null)
              {
                Game1.showGlobalMessage(greenSlime.cute ? Game1.content.LoadString("Strings\\StringsFromCSFiles:Object.cs.12689") : Game1.content.LoadString("Strings\\StringsFromCSFiles:Object.cs.12691"));
                greenSlime.setTilePosition((int) this.tileLocation.X, (int) this.tileLocation.Y + 1);
                location.characters.Add((NPC) greenSlime);
                this.heldObject = (Object) null;
                this.parentSheetIndex = 156;
                this.minutesUntilReady = -1;
                break;
              }
              break;
            }
            break;
          case 160:
            this.minutesUntilReady = 1;
            this.heldObject = new Object(386, Game1.random.Next(2, 9), false, -1, 0);
            break;
          case 117:
            this.heldObject = new Object(167, 1, false, -1, 0);
            break;
          case (int) sbyte.MaxValue:
            NPC todaysBirthdayNpc = Utility.getTodaysBirthdayNPC(Game1.currentSeason, Game1.dayOfMonth);
            this.minutesUntilReady = 1;
            if (todaysBirthdayNpc != null)
            {
              this.heldObject = todaysBirthdayNpc.getFavoriteItem();
              break;
            }
            int parentSheetIndex = 80;
            switch (Game1.random.Next(4))
            {
              case 0:
                parentSheetIndex = 72;
                break;
              case 1:
                parentSheetIndex = 337;
                break;
              case 2:
                parentSheetIndex = 749;
                break;
              case 3:
                parentSheetIndex = 336;
                break;
            }
            this.heldObject = new Object(parentSheetIndex, 1, false, -1, 0);
            break;
          case 108:
          case 109:
            this.parentSheetIndex = 108;
            if (Game1.currentSeason.Equals("winter") || Game1.currentSeason.Equals("fall"))
            {
              this.parentSheetIndex = 109;
              break;
            }
            break;
          case 10:
            if (Game1.currentSeason.Equals("winter"))
            {
              this.heldObject = (Object) null;
              this.readyForHarvest = false;
              this.showNextIndex = false;
              this.minutesUntilReady = -1;
              break;
            }
            if (this.heldObject == null)
            {
              this.heldObject = new Object(Vector2.Zero, 340, (string) null, false, true, false, false);
              this.minutesUntilReady = 2400 - Game1.timeOfDay + 4320;
              break;
            }
            break;
          case 104:
            this.minutesUntilReady = !Game1.currentSeason.Equals("winter") ? -1 : 9999;
            break;
        }
      }
      if (this.bigCraftable)
        return;
      switch (this.parentSheetIndex)
      {
        case 674:
        case 675:
          if (Game1.dayOfMonth != 1 || !Game1.currentSeason.Equals("summer") || !location.isOutdoors)
            break;
          this.parentSheetIndex = this.parentSheetIndex + 2;
          break;
        case 676:
        case 677:
          if (Game1.dayOfMonth != 1 || !Game1.currentSeason.Equals("fall") || !location.isOutdoors)
            break;
          this.parentSheetIndex = this.parentSheetIndex + 2;
          break;
        case 746:
          if (!Game1.currentSeason.Equals("winter"))
            break;
          this.rot();
          break;
        case 784:
        case 785:
        case 786:
          if (Game1.dayOfMonth != 1 || Game1.currentSeason.Equals("spring") || !location.isOutdoors)
            break;
          this.parentSheetIndex = this.parentSheetIndex + 1;
          break;
      }
    }

    public void rot()
    {
      this.ParentSheetIndex = new Random(Game1.year * 999 + Game1.dayOfMonth + Utility.getSeasonNumber(Game1.currentSeason)).Next(747, 749);
      this.price = 0;
      this.quality = 0;
      this.name = "Rotten Plant";
      this.displayName = (string) null;
      this.lightSource = (LightSource) null;
      this.bigCraftable = false;
    }

    public override void actionWhenBeingHeld(Farmer who)
    {
      if (this.lightSource == null || this.bigCraftable && !this.isLamp)
        return;
      if (!Utility.alreadyHasLightSourceWithThisID((int) who.uniqueMultiplayerID))
        Game1.currentLightSources.Add(new LightSource(this.lightSource.lightTexture, this.lightSource.position, this.lightSource.radius, this.lightSource.color, (int) who.uniqueMultiplayerID));
      Utility.repositionLightSource((int) who.uniqueMultiplayerID, who.position + new Vector2((float) (Game1.tileSize / 2), (float) -Game1.tileSize));
    }

    public override void actionWhenStopBeingHeld(Farmer who)
    {
      if (this.lightSource == null)
        return;
      Utility.removeLightSource((int) who.uniqueMultiplayerID);
    }

    public virtual bool performObjectDropInAction(Object dropIn, bool probe, Farmer who)
    {
      if (this.heldObject != null && !this.name.Equals("Recycling Machine") && !this.name.Equals("Crystalarium") || dropIn != null && dropIn.bigCraftable)
        return false;
      if (this.name.Equals("Incubator"))
      {
        if (this.heldObject == null && (dropIn.Category == -5 || dropIn.parentSheetIndex == 107))
        {
          this.heldObject = new Object(dropIn.parentSheetIndex, 1, false, -1, 0);
          if (!probe)
          {
            Game1.playSound("coin");
            this.minutesUntilReady = 9000 * (dropIn.parentSheetIndex == 107 ? 2 : 1);
            if (who.professions.Contains(2))
              this.minutesUntilReady = this.minutesUntilReady / 2;
            this.parentSheetIndex = dropIn.ParentSheetIndex == 180 || dropIn.ParentSheetIndex == 182 || dropIn.ParentSheetIndex == 305 ? this.parentSheetIndex + 2 : this.parentSheetIndex + 1;
          }
          return true;
        }
      }
      else if (this.name.Equals("Slime Incubator"))
      {
        if (this.heldObject == null && dropIn.name.Contains("Slime Egg"))
        {
          this.heldObject = new Object(dropIn.parentSheetIndex, 1, false, -1, 0);
          if (!probe)
          {
            Game1.playSound("coin");
            this.minutesUntilReady = 4000;
            if (who.professions.Contains(2))
              this.minutesUntilReady = this.minutesUntilReady / 2;
            this.parentSheetIndex = this.parentSheetIndex + 1;
          }
          return true;
        }
      }
      else if (this.name.Equals("Keg"))
      {
        switch (dropIn.parentSheetIndex)
        {
          case 340:
            this.heldObject = new Object(Vector2.Zero, 459, "Mead", false, true, false, false);
            if (!probe)
            {
              this.heldObject.name = "Mead";
              Game1.playSound("Ship");
              Game1.playSound("bubbles");
              this.minutesUntilReady = 600;
              who.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.animations, new Microsoft.Xna.Framework.Rectangle(256, 1856, 64, 128), 80f, 6, 999999, this.tileLocation * (float) Game1.tileSize + new Vector2(0.0f, (float) (-Game1.tileSize * 2)), false, false, (float) (((double) this.tileLocation.Y + 1.0) * (double) Game1.tileSize / 10000.0 + 9.99999974737875E-05), 0.0f, Color.Yellow * 0.75f, 1f, 0.0f, 0.0f, 0.0f, false)
              {
                alphaFade = 0.005f
              });
            }
            return true;
          case 433:
            if (dropIn.Stack < 5 && !probe)
            {
              Game1.showRedMessage(Game1.content.LoadString("Strings\\StringsFromCSFiles:Object.cs.12721"));
              return false;
            }
            this.heldObject = new Object(Vector2.Zero, 395, "Coffee", false, true, false, false);
            if (!probe)
            {
              this.heldObject.name = "Coffee";
              Game1.playSound("Ship");
              Game1.playSound("bubbles");
              dropIn.Stack -= 4;
              if (dropIn.Stack <= 0)
                who.removeItemFromInventory((Item) dropIn);
              this.minutesUntilReady = 120;
              who.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.animations, new Microsoft.Xna.Framework.Rectangle(256, 1856, 64, 128), 80f, 6, 999999, this.tileLocation * (float) Game1.tileSize + new Vector2(0.0f, (float) (-Game1.tileSize * 2)), false, false, (float) (((double) this.tileLocation.Y + 1.0) * (double) Game1.tileSize / 10000.0 + 9.99999974737875E-05), 0.0f, Color.DarkGray * 0.75f, 1f, 0.0f, 0.0f, 0.0f, false)
              {
                alphaFade = 0.005f
              });
            }
            return true;
          case 262:
            this.heldObject = new Object(Vector2.Zero, 346, "Beer", false, true, false, false);
            if (!probe)
            {
              this.heldObject.name = "Beer";
              Game1.playSound("Ship");
              Game1.playSound("bubbles");
              this.minutesUntilReady = 1750;
              who.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.animations, new Microsoft.Xna.Framework.Rectangle(256, 1856, 64, 128), 80f, 6, 999999, this.tileLocation * (float) Game1.tileSize + new Vector2(0.0f, (float) (-Game1.tileSize * 2)), false, false, (float) (((double) this.tileLocation.Y + 1.0) * (double) Game1.tileSize / 10000.0 + 9.99999974737875E-05), 0.0f, Color.Yellow * 0.75f, 1f, 0.0f, 0.0f, 0.0f, false)
              {
                alphaFade = 0.005f
              });
            }
            return true;
          case 304:
            this.heldObject = new Object(Vector2.Zero, 303, "Pale Ale", false, true, false, false);
            if (!probe)
            {
              this.heldObject.name = "Pale Ale";
              Game1.playSound("Ship");
              Game1.playSound("bubbles");
              this.minutesUntilReady = 2250;
              who.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.animations, new Microsoft.Xna.Framework.Rectangle(256, 1856, 64, 128), 80f, 6, 999999, this.tileLocation * (float) Game1.tileSize + new Vector2(0.0f, (float) (-Game1.tileSize * 2)), false, false, (float) (((double) this.tileLocation.Y + 1.0) * (double) Game1.tileSize / 10000.0 + 9.99999974737875E-05), 0.0f, Color.Yellow * 0.75f, 1f, 0.0f, 0.0f, 0.0f, false)
              {
                alphaFade = 0.005f
              });
            }
            return true;
          default:
            switch (dropIn.Category)
            {
              case -79:
                this.heldObject = new Object(Vector2.Zero, 348, dropIn.Name + " Wine", false, true, false, false);
                this.heldObject.Price = dropIn.Price * 3;
                if (!probe)
                {
                  this.heldObject.name = dropIn.Name + " Wine";
                  this.heldObject.preserve = new Object.PreserveType?(Object.PreserveType.Wine);
                  this.heldObject.preservedParentSheetIndex = dropIn.parentSheetIndex;
                  Game1.playSound("Ship");
                  Game1.playSound("bubbles");
                  this.minutesUntilReady = 10000;
                  who.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.animations, new Microsoft.Xna.Framework.Rectangle(256, 1856, 64, 128), 80f, 6, 999999, this.tileLocation * (float) Game1.tileSize + new Vector2(0.0f, (float) (-Game1.tileSize * 2)), false, false, (float) (((double) this.tileLocation.Y + 1.0) * (double) Game1.tileSize / 10000.0 + 9.99999974737875E-05), 0.0f, Color.Lavender * 0.75f, 1f, 0.0f, 0.0f, 0.0f, false)
                  {
                    alphaFade = 0.005f
                  });
                }
                return true;
              case -75:
                this.heldObject = new Object(Vector2.Zero, 350, dropIn.Name + " Juice", false, true, false, false);
                this.heldObject.Price = (int) ((double) dropIn.Price * 2.25);
                if (!probe)
                {
                  this.heldObject.name = dropIn.Name + " Juice";
                  this.heldObject.preserve = new Object.PreserveType?(Object.PreserveType.Juice);
                  this.heldObject.preservedParentSheetIndex = dropIn.parentSheetIndex;
                  Game1.playSound("bubbles");
                  Game1.playSound("Ship");
                  this.minutesUntilReady = 6000;
                  who.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.animations, new Microsoft.Xna.Framework.Rectangle(256, 1856, 64, 128), 80f, 6, 999999, this.tileLocation * (float) Game1.tileSize + new Vector2(0.0f, (float) (-Game1.tileSize * 2)), false, false, (float) (((double) this.tileLocation.Y + 1.0) * (double) Game1.tileSize / 10000.0 + 9.99999974737875E-05), 0.0f, Color.White * 0.75f, 1f, 0.0f, 0.0f, 0.0f, false)
                  {
                    alphaFade = 0.005f
                  });
                }
                return true;
            }
        }
      }
      else if (this.name.Equals("Preserves Jar"))
      {
        switch (dropIn.Category)
        {
          case -79:
            this.heldObject = new Object(Vector2.Zero, 344, dropIn.Name + " Jelly", false, true, false, false);
            this.heldObject.Price = 50 + dropIn.Price * 2;
            if (!probe)
            {
              this.minutesUntilReady = 4000;
              this.heldObject.name = dropIn.Name + " Jelly";
              this.heldObject.preserve = new Object.PreserveType?(Object.PreserveType.Jelly);
              this.heldObject.preservedParentSheetIndex = dropIn.parentSheetIndex;
              Game1.playSound("Ship");
              who.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.animations, new Microsoft.Xna.Framework.Rectangle(256, 1856, 64, 128), 80f, 6, 999999, this.tileLocation * (float) Game1.tileSize + new Vector2(0.0f, (float) (-Game1.tileSize * 2)), false, false, (float) (((double) this.tileLocation.Y + 1.0) * (double) Game1.tileSize / 10000.0 + 9.99999974737875E-05), 0.0f, Color.LightBlue * 0.75f, 1f, 0.0f, 0.0f, 0.0f, false)
              {
                alphaFade = 0.005f
              });
            }
            return true;
          case -75:
            this.heldObject = new Object(Vector2.Zero, 342, "Pickled " + dropIn.Name, false, true, false, false);
            this.heldObject.Price = 50 + dropIn.Price * 2;
            if (!probe)
            {
              this.heldObject.name = "Pickled " + dropIn.Name;
              this.heldObject.preserve = new Object.PreserveType?(Object.PreserveType.Pickle);
              this.heldObject.preservedParentSheetIndex = dropIn.parentSheetIndex;
              Game1.playSound("Ship");
              this.minutesUntilReady = 4000;
              who.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.animations, new Microsoft.Xna.Framework.Rectangle(256, 1856, 64, 128), 80f, 6, 999999, this.tileLocation * (float) Game1.tileSize + new Vector2(0.0f, (float) (-Game1.tileSize * 2)), false, false, (float) (((double) this.tileLocation.Y + 1.0) * (double) Game1.tileSize / 10000.0 + 9.99999974737875E-05), 0.0f, Color.White * 0.75f, 1f, 0.0f, 0.0f, 0.0f, false)
              {
                alphaFade = 0.005f
              });
            }
            return true;
        }
      }
      else if (this.name.Equals("Cheese Press"))
      {
        switch (dropIn.ParentSheetIndex)
        {
          case 436:
            this.heldObject = new Object(Vector2.Zero, 426, (string) null, false, true, false, false);
            if (!probe)
            {
              this.minutesUntilReady = 200;
              Game1.playSound("Ship");
            }
            return true;
          case 438:
            this.heldObject = new Object(Vector2.Zero, 426, (string) null, false, true, false, false)
            {
              quality = 2
            };
            if (!probe)
            {
              this.minutesUntilReady = 200;
              Game1.playSound("Ship");
            }
            return true;
          case 184:
            this.heldObject = new Object(Vector2.Zero, 424, (string) null, false, true, false, false);
            if (!probe)
            {
              this.minutesUntilReady = 200;
              Game1.playSound("Ship");
            }
            return true;
          case 186:
            this.heldObject = new Object(Vector2.Zero, 424, "Cheese (=)", false, true, false, false)
            {
              quality = 2
            };
            if (!probe)
            {
              this.minutesUntilReady = 200;
              Game1.playSound("Ship");
            }
            return true;
        }
      }
      else if (this.name.Equals("Mayonnaise Machine"))
      {
        switch (dropIn.ParentSheetIndex)
        {
          case 305:
            this.heldObject = new Object(Vector2.Zero, 308, (string) null, false, true, false, false);
            if (!probe)
            {
              this.minutesUntilReady = 180;
              Game1.playSound("Ship");
            }
            return true;
          case 442:
            this.heldObject = new Object(Vector2.Zero, 307, (string) null, false, true, false, false);
            if (!probe)
            {
              this.minutesUntilReady = 180;
              Game1.playSound("Ship");
            }
            return true;
          case 180:
          case 176:
            this.heldObject = new Object(Vector2.Zero, 306, (string) null, false, true, false, false);
            if (!probe)
            {
              this.minutesUntilReady = 180;
              Game1.playSound("Ship");
            }
            return true;
          case 182:
          case 107:
          case 174:
            this.heldObject = new Object(Vector2.Zero, 306, (string) null, false, true, false, false)
            {
              quality = 2
            };
            if (!probe)
            {
              this.minutesUntilReady = 180;
              Game1.playSound("Ship");
            }
            return true;
        }
      }
      else if (this.name.Equals("Loom"))
      {
        if (dropIn.ParentSheetIndex == 440)
        {
          this.heldObject = new Object(Vector2.Zero, 428, (string) null, false, true, false, false);
          if (!probe)
          {
            this.minutesUntilReady = 240;
            Game1.playSound("Ship");
          }
          return true;
        }
      }
      else if (this.name.Equals("Oil Maker"))
      {
        switch (dropIn.ParentSheetIndex)
        {
          case 430:
            this.heldObject = new Object(Vector2.Zero, 432, (string) null, false, true, false, false);
            if (!probe)
            {
              this.minutesUntilReady = 360;
              Game1.playSound("bubbles");
              Game1.playSound("sipTea");
              who.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.animations, new Microsoft.Xna.Framework.Rectangle(256, 1856, 64, 128), 80f, 6, 999999, this.tileLocation * (float) Game1.tileSize + new Vector2(0.0f, (float) (-Game1.tileSize * 2)), false, false, (float) (((double) this.tileLocation.Y + 1.0) * (double) Game1.tileSize / 10000.0 + 9.99999974737875E-05), 0.0f, Color.Yellow * 0.75f, 1f, 0.0f, 0.0f, 0.0f, false)
              {
                alphaFade = 0.005f
              });
            }
            return true;
          case 431:
            this.heldObject = new Object(247, 1, false, -1, 0);
            if (!probe)
            {
              this.minutesUntilReady = 3200;
              Game1.playSound("bubbles");
              Game1.playSound("sipTea");
              who.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.animations, new Microsoft.Xna.Framework.Rectangle(256, 1856, 64, 128), 80f, 6, 999999, this.tileLocation * (float) Game1.tileSize + new Vector2(0.0f, (float) (-Game1.tileSize * 2)), false, false, (float) (((double) this.tileLocation.Y + 1.0) * (double) Game1.tileSize / 10000.0 + 9.99999974737875E-05), 0.0f, Color.Yellow * 0.75f, 1f, 0.0f, 0.0f, 0.0f, false)
              {
                alphaFade = 0.005f
              });
            }
            return true;
          case 270:
            this.heldObject = new Object(Vector2.Zero, 247, (string) null, false, true, false, false);
            if (!probe)
            {
              this.minutesUntilReady = 1000;
              Game1.playSound("bubbles");
              Game1.playSound("sipTea");
              who.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.animations, new Microsoft.Xna.Framework.Rectangle(256, 1856, 64, 128), 80f, 6, 999999, this.tileLocation * (float) Game1.tileSize + new Vector2(0.0f, (float) (-Game1.tileSize * 2)), false, false, (float) (((double) this.tileLocation.Y + 1.0) * (double) Game1.tileSize / 10000.0 + 9.99999974737875E-05), 0.0f, Color.Yellow * 0.75f, 1f, 0.0f, 0.0f, 0.0f, false)
              {
                alphaFade = 0.005f
              });
            }
            return true;
          case 421:
            this.heldObject = new Object(Vector2.Zero, 247, (string) null, false, true, false, false);
            if (!probe)
            {
              this.minutesUntilReady = 60;
              Game1.playSound("bubbles");
              Game1.playSound("sipTea");
              who.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.animations, new Microsoft.Xna.Framework.Rectangle(256, 1856, 64, 128), 80f, 6, 999999, this.tileLocation * (float) Game1.tileSize + new Vector2(0.0f, (float) (-Game1.tileSize * 2)), false, false, (float) (((double) this.tileLocation.Y + 1.0) * (double) Game1.tileSize / 10000.0 + 9.99999974737875E-05), 0.0f, Color.Yellow * 0.75f, 1f, 0.0f, 0.0f, 0.0f, false)
              {
                alphaFade = 0.005f
              });
            }
            return true;
        }
      }
      else if (this.name.Equals("Seed Maker"))
      {
        if (dropIn != null && dropIn.parentSheetIndex == 433)
          return false;
        Dictionary<int, string> dictionary = Game1.temporaryContent.Load<Dictionary<int, string>>("Data\\Crops");
        bool flag = false;
        int parentSheetIndex = -1;
        foreach (KeyValuePair<int, string> keyValuePair in dictionary)
        {
          if (Convert.ToInt32(keyValuePair.Value.Split('/')[3]) == dropIn.ParentSheetIndex)
          {
            flag = true;
            parentSheetIndex = keyValuePair.Key;
            break;
          }
        }
        if (flag)
        {
          Random random = new Random((int) Game1.stats.DaysPlayed + (int) Game1.uniqueIDForThisGame / 2 + (int) this.tileLocation.X + (int) this.tileLocation.Y * 77 + Game1.timeOfDay);
          this.heldObject = new Object(parentSheetIndex, random.Next(1, 4), false, -1, 0);
          if (!probe)
          {
            if (random.NextDouble() < 0.005)
              this.heldObject = new Object(499, 1, false, -1, 0);
            else if (random.NextDouble() < 0.02)
              this.heldObject = new Object(770, random.Next(1, 5), false, -1, 0);
            this.minutesUntilReady = 20;
            Game1.playSound("Ship");
            DelayedAction.playSoundAfterDelay("dirtyHit", 250);
          }
          return true;
        }
      }
      else if (this.name.Equals("Crystalarium"))
      {
        if ((dropIn.Category == -2 || dropIn.Category == -12) && dropIn.ParentSheetIndex != 74 && (this.heldObject == null || this.heldObject.ParentSheetIndex != dropIn.ParentSheetIndex))
        {
          this.heldObject = (Object) dropIn.getOne();
          if (!probe)
          {
            Game1.playSound("select");
            this.minutesUntilReady = this.getMinutesForCrystalarium(dropIn.ParentSheetIndex);
          }
          return true;
        }
      }
      else if (this.name.Equals("Recycling Machine"))
      {
        if (dropIn.ParentSheetIndex >= 168 && dropIn.ParentSheetIndex <= 172 && this.heldObject == null)
        {
          Random random = new Random((int) Game1.uniqueIDForThisGame / 2 + (int) Game1.stats.DaysPlayed + Game1.timeOfDay + (int) this.tileLocation.X * 200 + (int) this.tileLocation.Y);
          switch (dropIn.ParentSheetIndex)
          {
            case 168:
              this.heldObject = new Object(random.NextDouble() < 0.3 ? 382 : (random.NextDouble() < 0.3 ? 380 : 390), random.Next(1, 4), false, -1, 0);
              break;
            case 169:
              this.heldObject = new Object(random.NextDouble() < 0.25 ? 382 : 388, random.Next(1, 4), false, -1, 0);
              break;
            case 170:
              this.heldObject = new Object(338, 1, false, -1, 0);
              break;
            case 171:
              this.heldObject = new Object(338, 1, false, -1, 0);
              break;
            case 172:
              this.heldObject = random.NextDouble() < 0.1 ? new Object(428, 1, false, -1, 0) : (Object) new Torch(Vector2.Zero, 3);
              break;
          }
          if (!probe)
          {
            Game1.playSound("trashcan");
            this.minutesUntilReady = 60;
            ++Game1.stats.PiecesOfTrashRecycled;
          }
          return true;
        }
      }
      else if (this.name.Equals("Furnace"))
      {
        if (who.IsMainPlayer && who.getTallyOfObject(382, false) <= 0)
        {
          if (!probe && who.IsMainPlayer)
            Game1.showRedMessage(Game1.content.LoadString("Strings\\StringsFromCSFiles:Object.cs.12772"));
          return false;
        }
        if (this.heldObject == null && !probe)
        {
          if (dropIn.stack < 5 && dropIn.parentSheetIndex != 80 && dropIn.parentSheetIndex != 330)
          {
            Game1.showRedMessage(Game1.content.LoadString("Strings\\StringsFromCSFiles:Object.cs.12777"));
            return false;
          }
          int num = 5;
          switch (dropIn.ParentSheetIndex)
          {
            case 380:
              this.heldObject = new Object(Vector2.Zero, 335, 1);
              this.minutesUntilReady = 120;
              break;
            case 384:
              this.heldObject = new Object(Vector2.Zero, 336, 1);
              this.minutesUntilReady = 300;
              break;
            case 386:
              this.heldObject = new Object(Vector2.Zero, 337, 1);
              this.minutesUntilReady = 480;
              break;
            case 80:
              this.heldObject = new Object(Vector2.Zero, 338, "Refined Quartz", false, true, false, false);
              this.minutesUntilReady = 90;
              num = 1;
              break;
            case 378:
              this.heldObject = new Object(Vector2.Zero, 334, 1);
              this.minutesUntilReady = 30;
              break;
            default:
              return false;
          }
          Game1.playSound("openBox");
          DelayedAction.playSoundAfterDelay("furnace", 50);
          this.initializeLightSource(this.tileLocation);
          this.showNextIndex = true;
          who.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(30, this.tileLocation * (float) Game1.tileSize + new Vector2(0.0f, (float) (-Game1.tileSize / 4)), Color.White, 4, false, 50f, 10, Game1.tileSize, (float) (((double) this.tileLocation.Y + 1.0) * (double) Game1.tileSize / 10000.0 + 9.99999974737875E-05), -1, 0)
          {
            alphaFade = 0.005f
          });
          for (int index = who.items.Count - 1; index >= 0; --index)
          {
            if (who.Items[index] is Object && (who.Items[index] as Object).ParentSheetIndex == 382)
            {
              --who.Items[index].Stack;
              if (who.Items[index].Stack <= 0)
              {
                who.Items[index] = (Item) null;
                break;
              }
              break;
            }
          }
          dropIn.Stack -= num;
          return dropIn.Stack <= 0;
        }
        if (this.heldObject == null & probe)
        {
          switch (dropIn.ParentSheetIndex)
          {
            case 380:
            case 384:
            case 386:
            case 80:
            case 378:
              this.heldObject = new Object();
              return true;
          }
        }
      }
      else if (this.name.Equals("Charcoal Kiln"))
      {
        if (who.IsMainPlayer && (dropIn.parentSheetIndex != 388 || dropIn.Stack < 10))
        {
          if (!probe && who.IsMainPlayer)
            Game1.showRedMessage(Game1.content.LoadString("Strings\\StringsFromCSFiles:Object.cs.12783"));
          return false;
        }
        if (this.heldObject == null && !probe && (dropIn.parentSheetIndex == 388 && dropIn.Stack >= 10))
        {
          dropIn.Stack -= 10;
          if (dropIn.Stack <= 0)
            who.removeItemFromInventory((Item) dropIn);
          Game1.playSound("openBox");
          DelayedAction.playSoundAfterDelay("fireball", 50);
          this.showNextIndex = true;
          who.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(27, this.tileLocation * (float) Game1.tileSize + new Vector2((float) (-Game1.tileSize / 4), (float) (-Game1.tileSize * 2)), Color.White, 4, false, 50f, 10, Game1.tileSize, (float) (((double) this.tileLocation.Y + 1.0) * (double) Game1.tileSize / 10000.0 + 9.99999974737875E-05), -1, 0)
          {
            alphaFade = 0.005f
          });
          this.heldObject = new Object(382, 1, false, -1, 0);
          this.minutesUntilReady = 30;
        }
        else if (this.heldObject == null & probe && dropIn.parentSheetIndex == 388 && dropIn.Stack >= 10)
        {
          this.heldObject = new Object();
          return true;
        }
      }
      else if (this.name.Equals("Slime Egg-Press"))
      {
        if (who.IsMainPlayer && (dropIn.parentSheetIndex != 766 || dropIn.Stack < 100))
        {
          if (!probe && who.IsMainPlayer)
            Game1.showRedMessage(Game1.content.LoadString("Strings\\StringsFromCSFiles:Object.cs.12787"));
          return false;
        }
        if (this.heldObject == null && !probe && (dropIn.parentSheetIndex == 766 && dropIn.Stack >= 100))
        {
          dropIn.Stack -= 100;
          if (dropIn.Stack <= 0)
            who.removeItemFromInventory((Item) dropIn);
          Game1.playSound("slimeHit");
          DelayedAction.playSoundAfterDelay("bubbles", 50);
          who.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.animations, new Microsoft.Xna.Framework.Rectangle(256, 1856, 64, 128), 80f, 6, 999999, this.tileLocation * (float) Game1.tileSize + new Vector2(0.0f, (float) -Game1.tileSize * 2.5f), false, false, (float) (((double) this.tileLocation.Y + 1.0) * (double) Game1.tileSize / 10000.0 + 9.99999974737875E-05), 0.0f, Color.Lime, 1f, 0.0f, 0.0f, 0.0f, false)
          {
            alphaFade = 0.005f
          });
          int parentSheetIndex = 680;
          if (Game1.random.NextDouble() < 0.05)
            parentSheetIndex = 439;
          else if (Game1.random.NextDouble() < 0.1)
            parentSheetIndex = 437;
          else if (Game1.random.NextDouble() < 0.25)
            parentSheetIndex = 413;
          this.heldObject = new Object(parentSheetIndex, 1, false, -1, 0);
          this.minutesUntilReady = 1200;
        }
        else if (this.heldObject == null & probe && dropIn.parentSheetIndex == 766 && dropIn.Stack >= 100)
        {
          this.heldObject = new Object();
          return true;
        }
      }
      else if (this.name.Contains("Hopper") && dropIn.ParentSheetIndex == 178)
      {
        if (!probe)
        {
          if (Utility.numSilos() <= 0)
          {
            Game1.showRedMessage(Game1.content.LoadString("Strings\\Buildings:NeedSilo"));
            return false;
          }
          Game1.playSound("Ship");
          DelayedAction.playSoundAfterDelay("grassyStep", 100);
          if (dropIn.Stack == 0)
            dropIn.Stack = 1;
          int piecesOfHay1 = (Game1.getLocationFromName("Farm") as Farm).piecesOfHay;
          int addHay = (Game1.getLocationFromName("Farm") as Farm).tryToAddHay(dropIn.Stack);
          int piecesOfHay2 = (Game1.getLocationFromName("Farm") as Farm).piecesOfHay;
          int num = 0;
          if (piecesOfHay1 <= num && piecesOfHay2 > 0)
            this.showNextIndex = true;
          else if (piecesOfHay2 <= 0)
            this.showNextIndex = false;
          dropIn.Stack = addHay;
          if (addHay <= 0)
            return true;
        }
        else
        {
          this.heldObject = new Object();
          return true;
        }
      }
      if (this.name.Contains("Table") && this.heldObject == null && (!dropIn.bigCraftable && !dropIn.Name.Contains("Table")))
      {
        this.heldObject = (Object) dropIn.getOne();
        if (!probe)
          Game1.playSound("woodyStep");
        return true;
      }
      Object heldObject = this.heldObject;
      return false;
    }

    public virtual void updateWhenCurrentLocation(GameTime time)
    {
      if (this.lightSource != null && this.isOn)
        Game1.currentLightSources.Add(this.lightSource);
      if (this.heldObject != null && this.heldObject.lightSource != null)
        Game1.currentLightSources.Add(this.heldObject.lightSource);
      if (this.shakeTimer > 0)
      {
        this.shakeTimer = this.shakeTimer - time.ElapsedGameTime.Milliseconds;
        if (this.shakeTimer <= 0)
          this.health = 10;
      }
      if (this.parentSheetIndex == 590 && Game1.random.NextDouble() < 0.01)
        this.shakeTimer = 100;
      if (!this.bigCraftable || !this.name.Equals("Slime Ball"))
        return;
      this.parentSheetIndex = 56 + (int) (time.TotalGameTime.TotalMilliseconds % 600.0 / 100.0);
    }

    public virtual void actionOnPlayerEntry()
    {
      this.health = 10;
      if (this.name == null || !this.name.Contains("Hopper"))
        return;
      this.showNextIndex = (Game1.getLocationFromName("Farm") as Farm).piecesOfHay > 0;
    }

    public override bool canBeTrashed()
    {
      if (this.questItem && Game1.player.isThereALostItemQuestThatTakesThisItem(this.parentSheetIndex) || !this.bigCraftable && this.parentSheetIndex == 460)
        return false;
      return base.canBeTrashed();
    }

    public bool isForage(GameLocation location)
    {
      if (this.Category != -79 && this.Category != -81 && (this.Category != -80 && this.Category != -75) && !(location is Beach))
        return this.parentSheetIndex == 430;
      return true;
    }

    public void initializeLightSource(Vector2 tileLocation)
    {
      if (this.name == null)
        return;
      if (this.bigCraftable)
      {
        if (this is Torch && this.isOn)
          this.lightSource = new LightSource(4, new Vector2(tileLocation.X * (float) Game1.tileSize + (float) (Game1.tileSize / 2), tileLocation.Y * (float) Game1.tileSize - (float) Game1.tileSize), 2.5f, new Color(0, 80, 160), (int) ((double) tileLocation.X * 2000.0 + (double) tileLocation.Y));
        else if (this.isLamp)
          this.lightSource = new LightSource(4, new Vector2(tileLocation.X * (float) Game1.tileSize + (float) (Game1.tileSize / 2), tileLocation.Y * (float) Game1.tileSize - (float) Game1.tileSize), 3f, new Color(0, 40, 80), (int) ((double) tileLocation.X * 2000.0 + (double) tileLocation.Y));
        else if (this.name.Equals("Furnace") && this.minutesUntilReady > 0 || this.name.Equals("Bonfire"))
        {
          this.lightSource = new LightSource(4, new Vector2(tileLocation.X * (float) Game1.tileSize + (float) (Game1.tileSize / 2), tileLocation.Y * (float) Game1.tileSize), 1.5f, Color.DarkCyan, (int) ((double) tileLocation.X * 2000.0 + (double) tileLocation.Y));
        }
        else
        {
          if (!this.name.Equals("Strange Capsule"))
            return;
          this.lightSource = new LightSource(4, new Vector2(tileLocation.X * (float) Game1.tileSize + (float) (Game1.tileSize / 2), tileLocation.Y * (float) Game1.tileSize), 1f, Color.HotPink * 0.75f, (int) ((double) tileLocation.X * 2000.0 + (double) tileLocation.Y));
        }
      }
      else if (this.parentSheetIndex == 93 || this.parentSheetIndex == 94)
      {
        Color color = Color.White;
        switch (this.parentSheetIndex)
        {
          case 93:
          case 746:
            color = new Color(1, 1, 1) * 0.9f;
            break;
          case 94:
            color = Color.Yellow;
            break;
        }
        this.lightSource = new LightSource(4, new Vector2(tileLocation.X * (float) Game1.tileSize + (float) (Game1.tileSize / 4), tileLocation.Y * (float) Game1.tileSize + (float) (Game1.tileSize / 4)), Game1.currentLocation.GetType() == typeof (MineShaft) ? 1.5f : 1.25f, color, (int) ((double) this.tileLocation.X * 2000.0 + (double) this.tileLocation.Y));
      }
      else
      {
        if (this.parentSheetIndex != 746)
          return;
        this.lightSource = new LightSource(4, new Vector2(tileLocation.X * (float) Game1.tileSize + (float) (Game1.tileSize / 2), tileLocation.Y * (float) Game1.tileSize + (float) (Game1.tileSize * 3 / 4)), 0.5f, new Color(1, 1, 1) * 0.65f, (int) ((double) this.tileLocation.X * 2000.0 + (double) this.tileLocation.Y));
      }
    }

    public virtual void performRemoveAction(Vector2 tileLocation, GameLocation environment)
    {
      if (this.lightSource != null)
      {
        Utility.removeLightSource((int) ((double) this.tileLocation.X * 2000.0 + (double) this.tileLocation.Y));
        Utility.removeLightSource((int) Game1.player.uniqueMultiplayerID);
      }
      if (this.bigCraftable)
      {
        if (this.parentSheetIndex == 126 && this.quality != 0)
          Game1.createItemDebris((Item) new Hat(this.quality - 1), tileLocation * (float) Game1.tileSize, (Game1.player.facingDirection + 2) % 4, (GameLocation) null);
        this.quality = 0;
      }
      if (this.name == null || !this.name.Contains("Sprinkler"))
        return;
      environment.removeTemporarySpritesWithID((int) tileLocation.X * 4000 + (int) tileLocation.Y);
    }

    public virtual bool isPassable()
    {
      if (this.bigCraftable)
        return false;
      switch (this.parentSheetIndex)
      {
        case 415:
        case 590:
        case 401:
        case 405:
        case 407:
        case 409:
        case 411:
        case 286:
        case 287:
        case 288:
        case 297:
        case 328:
        case 329:
        case 331:
        case 333:
          return true;
        default:
          return false;
      }
    }

    public virtual void reloadSprite()
    {
      this.initializeLightSource(this.tileLocation);
    }

    public void consumeRecipe(Farmer who)
    {
      if (!this.isRecipe)
        return;
      if (this.category == -7)
        who.cookingRecipes.Add(this.name, 0);
      else
        who.craftingRecipes.Add(this.name, 0);
    }

    public virtual Microsoft.Xna.Framework.Rectangle getBoundingBox(Vector2 tileLocation)
    {
      this.boundingBox.X = (int) tileLocation.X * Game1.tileSize;
      this.boundingBox.Y = (int) tileLocation.Y * Game1.tileSize;
      if (this is Torch && !this.bigCraftable || this.parentSheetIndex == 590)
      {
        this.boundingBox.X = (int) tileLocation.X * Game1.tileSize + Game1.tileSize * 3 / 8;
        this.boundingBox.Y = (int) tileLocation.Y * Game1.tileSize + Game1.tileSize * 3 / 8;
      }
      return this.boundingBox;
    }

    public override bool canBeGivenAsGift()
    {
      if (!this.bigCraftable && !(this is Furniture))
        return !(this is Wallpaper);
      return false;
    }

    public virtual bool performDropDownAction(Farmer who)
    {
      if (who == null)
        who = Game1.getFarmer(this.owner);
      if (this.name.Equals("Worm Bin"))
      {
        if (this.heldObject == null)
        {
          this.heldObject = new Object(685, Game1.random.Next(2, 6), false, -1, 0);
          this.minutesUntilReady = 2600 - Game1.timeOfDay;
        }
        return false;
      }
      if (this.name.Equals("Bee House"))
      {
        if (this.heldObject == null)
        {
          this.heldObject = new Object(Vector2.Zero, 340, (string) null, false, true, false, false);
          this.minutesUntilReady = 2400 - Game1.timeOfDay + 4320;
        }
        return false;
      }
      if (this.name.Contains("Strange Capsule"))
        this.minutesUntilReady = 6000 - Game1.timeOfDay;
      else if (this.name.Contains("Hopper"))
      {
        this.showNextIndex = false;
        if ((Game1.getLocationFromName("Farm") as Farm).piecesOfHay >= 0)
          this.showNextIndex = true;
      }
      return false;
    }

    private void totemWarp(Farmer who)
    {
      for (int index = 0; index < 12; ++index)
        who.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(354, (float) Game1.random.Next(25, 75), 6, 1, new Vector2((float) Game1.random.Next((int) who.position.X - Game1.tileSize * 4, (int) who.position.X + Game1.tileSize * 3), (float) Game1.random.Next((int) who.position.Y - Game1.tileSize * 4, (int) who.position.Y + Game1.tileSize * 3)), false, Game1.random.NextDouble() < 0.5));
      Game1.playSound("wand");
      Game1.displayFarmer = false;
      Game1.player.freezePause = 1000;
      Game1.flashAlpha = 1f;
      DelayedAction.fadeAfterDelay(new Game1.afterFadeFunction(this.totemWarpForReal), 1000);
      new Microsoft.Xna.Framework.Rectangle(who.GetBoundingBox().X, who.GetBoundingBox().Y, Game1.tileSize, Game1.tileSize).Inflate(Game1.tileSize * 3, Game1.tileSize * 3);
      int num1 = 0;
      for (int index = who.getTileX() + 8; index >= who.getTileX() - 8; --index)
      {
        List<TemporaryAnimatedSprite> temporarySprites = who.currentLocation.temporarySprites;
        TemporaryAnimatedSprite temporaryAnimatedSprite = new TemporaryAnimatedSprite(6, new Vector2((float) index, (float) who.getTileY()) * (float) Game1.tileSize, Color.White, 8, false, 50f, 0, -1, -1f, -1, 0);
        temporaryAnimatedSprite.layerDepth = 1f;
        int num2 = num1 * 25;
        temporaryAnimatedSprite.delayBeforeAnimationStart = num2;
        Vector2 vector2 = new Vector2(-0.25f, 0.0f);
        temporaryAnimatedSprite.motion = vector2;
        temporarySprites.Add(temporaryAnimatedSprite);
        ++num1;
      }
    }

    private void totemWarpForReal()
    {
      switch (this.parentSheetIndex)
      {
        case 688:
          Game1.warpFarmer("Farm", 48, 7, false);
          break;
        case 689:
          Game1.warpFarmer("Mountain", 31, 20, false);
          break;
        case 690:
          Game1.warpFarmer("Beach", 20, 4, false);
          break;
      }
      Game1.fadeToBlackAlpha = 0.99f;
      Game1.screenGlow = false;
      Game1.player.temporarilyInvincible = false;
      Game1.player.temporaryInvincibilityTimer = 0;
      Game1.displayFarmer = true;
    }

    private void rainTotem(Farmer who)
    {
      if (!Game1.IsMultiplayer && !Utility.isFestivalDay(Game1.dayOfMonth + 1, Game1.currentSeason))
      {
        Game1.weatherForTomorrow = 1;
        Game1.pauseThenMessage(2000, Game1.content.LoadString("Strings\\StringsFromCSFiles:Object.cs.12822"), false);
      }
      Game1.screenGlow = false;
      Game1.playSound("thunder");
      who.canMove = false;
      Game1.screenGlowOnce(Color.SlateBlue, false, 0.005f, 0.3f);
      Game1.player.faceDirection(2);
      Game1.player.FarmerSprite.animateOnce(new FarmerSprite.AnimationFrame[1]
      {
        new FarmerSprite.AnimationFrame(57, 2000, false, false, new AnimatedSprite.endOfAnimationBehavior(Farmer.canMoveNow), true)
      });
      for (int index = 0; index < 6; ++index)
      {
        who.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(648, 1045, 52, 33), 9999f, 1, 999, who.position + new Vector2(0.0f, (float) (-Game1.tileSize * 2)), false, false, 1f, 0.01f, Color.White * 0.8f, (float) (Game1.pixelZoom / 2), 0.01f, 0.0f, 0.0f, false)
        {
          motion = new Vector2((float) Game1.random.Next(-10, 11) / 10f, -2f),
          delayBeforeAnimationStart = index * 200
        });
        who.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(648, 1045, 52, 33), 9999f, 1, 999, who.position + new Vector2(0.0f, (float) (-Game1.tileSize * 2)), false, false, 1f, 0.01f, Color.White * 0.8f, (float) (Game1.pixelZoom / 4), 0.01f, 0.0f, 0.0f, false)
        {
          motion = new Vector2((float) Game1.random.Next(-30, -10) / 10f, -1f),
          delayBeforeAnimationStart = 100 + index * 200
        });
        who.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(648, 1045, 52, 33), 9999f, 1, 999, who.position + new Vector2(0.0f, (float) (-Game1.tileSize * 2)), false, false, 1f, 0.01f, Color.White * 0.8f, (float) (Game1.pixelZoom / 4), 0.01f, 0.0f, 0.0f, false)
        {
          motion = new Vector2((float) Game1.random.Next(10, 30) / 10f, -1f),
          delayBeforeAnimationStart = 200 + index * 200
        });
      }
      List<TemporaryAnimatedSprite> temporarySprites = Game1.currentLocation.temporarySprites;
      TemporaryAnimatedSprite temporaryAnimatedSprite = new TemporaryAnimatedSprite(this.parentSheetIndex, 9999f, 1, 999, Game1.player.position + new Vector2(0.0f, (float) (-Game1.tileSize * 3 / 2)), false, false, false, 0.0f);
      temporaryAnimatedSprite.motion = new Vector2(0.0f, -7f);
      temporaryAnimatedSprite.acceleration = new Vector2(0.0f, 0.1f);
      temporaryAnimatedSprite.scaleChange = 0.015f;
      temporaryAnimatedSprite.alpha = 1f;
      temporaryAnimatedSprite.alphaFade = 0.0075f;
      temporaryAnimatedSprite.shakeIntensity = 1f;
      temporaryAnimatedSprite.initialPosition = Game1.player.position + new Vector2(0.0f, (float) (-Game1.tileSize * 3 / 2));
      int num1 = 1;
      temporaryAnimatedSprite.xPeriodic = num1 != 0;
      double num2 = 1000.0;
      temporaryAnimatedSprite.xPeriodicLoopTime = (float) num2;
      double num3 = (double) (Game1.tileSize / 16);
      temporaryAnimatedSprite.xPeriodicRange = (float) num3;
      double num4 = 1.0;
      temporaryAnimatedSprite.layerDepth = (float) num4;
      temporarySprites.Add(temporaryAnimatedSprite);
      DelayedAction.playSoundAfterDelay("rainsound", 2000);
    }

    public bool performUseAction()
    {
      if (!Game1.player.canMove)
        return false;
      if (this.name != null && this.name.Contains("Totem") && (!Game1.eventUp && !Game1.isFestival()) && (!Game1.fadeToBlack && !Game1.player.swimming && !Game1.player.bathingClothes))
      {
        switch (this.parentSheetIndex)
        {
          case 681:
            this.rainTotem(Game1.player);
            return true;
          case 688:
          case 689:
          case 690:
            Game1.player.jitterStrength = 1f;
            Color glowColor = this.parentSheetIndex == 681 ? Color.SlateBlue : (this.parentSheetIndex == 688 ? Color.LimeGreen : (this.parentSheetIndex == 689 ? Color.OrangeRed : Color.LightBlue));
            Game1.playSound("warrior");
            Game1.player.faceDirection(2);
            Game1.player.CanMove = false;
            Game1.player.temporarilyInvincible = true;
            Game1.player.temporaryInvincibilityTimer = -4000;
            Game1.changeMusicTrack("none");
            if (this.parentSheetIndex == 681)
              Game1.player.FarmerSprite.animateOnce(new FarmerSprite.AnimationFrame[2]
              {
                new FarmerSprite.AnimationFrame(57, 2000, false, false, (AnimatedSprite.endOfAnimationBehavior) null, false),
                new FarmerSprite.AnimationFrame((int) (short) Game1.player.FarmerSprite.currentFrame, 0, false, false, new AnimatedSprite.endOfAnimationBehavior(this.rainTotem), true)
              });
            else
              Game1.player.FarmerSprite.animateOnce(new FarmerSprite.AnimationFrame[2]
              {
                new FarmerSprite.AnimationFrame(57, 2000, false, false, (AnimatedSprite.endOfAnimationBehavior) null, false),
                new FarmerSprite.AnimationFrame((int) (short) Game1.player.FarmerSprite.currentFrame, 0, false, false, new AnimatedSprite.endOfAnimationBehavior(this.totemWarp), true)
              });
            List<TemporaryAnimatedSprite> temporarySprites1 = Game1.currentLocation.temporarySprites;
            TemporaryAnimatedSprite temporaryAnimatedSprite1 = new TemporaryAnimatedSprite(this.parentSheetIndex, 9999f, 1, 999, Game1.player.position + new Vector2(0.0f, (float) (-Game1.tileSize * 3 / 2)), false, false, false, 0.0f);
            temporaryAnimatedSprite1.motion = new Vector2(0.0f, -1f);
            temporaryAnimatedSprite1.scaleChange = 0.01f;
            temporaryAnimatedSprite1.alpha = 1f;
            temporaryAnimatedSprite1.alphaFade = 0.0075f;
            temporaryAnimatedSprite1.shakeIntensity = 1f;
            temporaryAnimatedSprite1.initialPosition = Game1.player.position + new Vector2(0.0f, (float) (-Game1.tileSize * 3 / 2));
            int num1 = 1;
            temporaryAnimatedSprite1.xPeriodic = num1 != 0;
            double num2 = 1000.0;
            temporaryAnimatedSprite1.xPeriodicLoopTime = (float) num2;
            double num3 = (double) (Game1.tileSize / 16);
            temporaryAnimatedSprite1.xPeriodicRange = (float) num3;
            double num4 = 1.0;
            temporaryAnimatedSprite1.layerDepth = (float) num4;
            temporarySprites1.Add(temporaryAnimatedSprite1);
            List<TemporaryAnimatedSprite> temporarySprites2 = Game1.currentLocation.temporarySprites;
            TemporaryAnimatedSprite temporaryAnimatedSprite2 = new TemporaryAnimatedSprite(this.parentSheetIndex, 9999f, 1, 999, Game1.player.position + new Vector2((float) -Game1.tileSize, (float) (-Game1.tileSize * 3 / 2)), false, false, false, 0.0f);
            temporaryAnimatedSprite2.motion = new Vector2(0.0f, -0.5f);
            temporaryAnimatedSprite2.scaleChange = 0.005f;
            temporaryAnimatedSprite2.scale = 0.5f;
            temporaryAnimatedSprite2.alpha = 1f;
            temporaryAnimatedSprite2.alphaFade = 0.0075f;
            temporaryAnimatedSprite2.shakeIntensity = 1f;
            temporaryAnimatedSprite2.delayBeforeAnimationStart = 10;
            temporaryAnimatedSprite2.initialPosition = Game1.player.position + new Vector2((float) -Game1.tileSize, (float) (-Game1.tileSize * 3 / 2));
            int num5 = 1;
            temporaryAnimatedSprite2.xPeriodic = num5 != 0;
            double num6 = 1000.0;
            temporaryAnimatedSprite2.xPeriodicLoopTime = (float) num6;
            double num7 = (double) (Game1.tileSize / 16);
            temporaryAnimatedSprite2.xPeriodicRange = (float) num7;
            double num8 = 0.999899983406067;
            temporaryAnimatedSprite2.layerDepth = (float) num8;
            temporarySprites2.Add(temporaryAnimatedSprite2);
            List<TemporaryAnimatedSprite> temporarySprites3 = Game1.currentLocation.temporarySprites;
            TemporaryAnimatedSprite temporaryAnimatedSprite3 = new TemporaryAnimatedSprite(this.parentSheetIndex, 9999f, 1, 999, Game1.player.position + new Vector2((float) Game1.tileSize, (float) (-Game1.tileSize * 3 / 2)), false, false, false, 0.0f);
            temporaryAnimatedSprite3.motion = new Vector2(0.0f, -0.5f);
            temporaryAnimatedSprite3.scaleChange = 0.005f;
            temporaryAnimatedSprite3.scale = 0.5f;
            temporaryAnimatedSprite3.alpha = 1f;
            temporaryAnimatedSprite3.alphaFade = 0.0075f;
            temporaryAnimatedSprite3.delayBeforeAnimationStart = 20;
            temporaryAnimatedSprite3.shakeIntensity = 1f;
            temporaryAnimatedSprite3.initialPosition = Game1.player.position + new Vector2((float) Game1.tileSize, (float) (-Game1.tileSize * 3 / 2));
            int num9 = 1;
            temporaryAnimatedSprite3.xPeriodic = num9 != 0;
            double num10 = 1000.0;
            temporaryAnimatedSprite3.xPeriodicLoopTime = (float) num10;
            double num11 = (double) (Game1.tileSize / 16);
            temporaryAnimatedSprite3.xPeriodicRange = (float) num11;
            double num12 = 0.998799979686737;
            temporaryAnimatedSprite3.layerDepth = (float) num12;
            temporarySprites3.Add(temporaryAnimatedSprite3);
            Game1.screenGlowOnce(glowColor, false, 0.005f, 0.3f);
            Utility.addSprinklesToLocation(Game1.currentLocation, Game1.player.getTileX(), Game1.player.getTileY(), 16, 16, 1300, 20, Color.White, (string) null, true);
            return true;
        }
      }
      string name = this.name;
      return false;
    }

    public override Color getCategoryColor()
    {
      if (this is Furniture)
        return new Color(100, 25, 190);
      if (this.type != null && this.type.Equals("Arch"))
        return new Color(110, 0, 90);
      switch (this.category)
      {
        case -81:
          return new Color(10, 130, 50);
        case -80:
          return new Color(219, 54, 211);
        case -79:
          return Color.DeepPink;
        case -75:
          return Color.Green;
        case -74:
          return Color.Brown;
        case -28:
          return new Color(50, 10, 70);
        case -27:
        case -26:
          return new Color(0, 155, 111);
        case -24:
          return Color.Plum;
        case -22:
          return Color.DarkCyan;
        case -21:
          return Color.DarkRed;
        case -20:
          return Color.DarkGray;
        case -19:
          return Color.SlateGray;
        case -18:
        case -14:
        case -6:
        case -5:
          return new Color((int) byte.MaxValue, 0, 100);
        case -16:
        case -15:
          return new Color(64, 102, 114);
        case -12:
        case -2:
          return new Color(110, 0, 90);
        case -8:
          return new Color(148, 61, 40);
        case -7:
          return new Color(220, 60, 0);
        case -4:
          return Color.DarkBlue;
        default:
          return Color.Black;
      }
    }

    public override string getCategoryName()
    {
      if (this is Furniture)
        return Game1.content.LoadString("Strings\\StringsFromCSFiles:Object.cs.12847");
      if (this.type != null && this.type.Equals("Arch"))
        return Game1.content.LoadString("Strings\\StringsFromCSFiles:Object.cs.12849");
      switch (this.category)
      {
        case -81:
          return Game1.content.LoadString("Strings\\StringsFromCSFiles:Object.cs.12869");
        case -80:
          return Game1.content.LoadString("Strings\\StringsFromCSFiles:Object.cs.12866");
        case -79:
          return Game1.content.LoadString("Strings\\StringsFromCSFiles:Object.cs.12854");
        case -75:
          return Game1.content.LoadString("Strings\\StringsFromCSFiles:Object.cs.12851");
        case -74:
          return Game1.content.LoadString("Strings\\StringsFromCSFiles:Object.cs.12855");
        case -28:
          return Game1.content.LoadString("Strings\\StringsFromCSFiles:Object.cs.12867");
        case -27:
        case -26:
          return Game1.content.LoadString("Strings\\StringsFromCSFiles:Object.cs.12862");
        case -25:
        case -7:
          return Game1.content.LoadString("Strings\\StringsFromCSFiles:Object.cs.12853");
        case -24:
          return Game1.content.LoadString("Strings\\StringsFromCSFiles:Object.cs.12859");
        case -22:
          return Game1.content.LoadString("Strings\\StringsFromCSFiles:Object.cs.12858");
        case -21:
          return Game1.content.LoadString("Strings\\StringsFromCSFiles:Object.cs.12857");
        case -20:
          return Game1.content.LoadString("Strings\\StringsFromCSFiles:Object.cs.12860");
        case -19:
          return Game1.content.LoadString("Strings\\StringsFromCSFiles:Object.cs.12856");
        case -18:
        case -14:
        case -6:
        case -5:
          return Game1.content.LoadString("Strings\\StringsFromCSFiles:Object.cs.12864");
        case -16:
        case -15:
          return Game1.content.LoadString("Strings\\StringsFromCSFiles:Object.cs.12868");
        case -12:
        case -2:
          return Game1.content.LoadString("Strings\\StringsFromCSFiles:Object.cs.12850");
        case -8:
          return Game1.content.LoadString("Strings\\StringsFromCSFiles:Object.cs.12863");
        case -4:
          return Game1.content.LoadString("Strings\\StringsFromCSFiles:Object.cs.12852");
        default:
          return "";
      }
    }

    public virtual bool isActionable(Farmer who)
    {
      return this.checkForAction(who, true);
    }

    public int getHealth()
    {
      return this.health;
    }

    public void setHealth(int health)
    {
      this.health = health;
    }

    public virtual bool checkForAction(Farmer who, bool justCheckingForActivity = false)
    {
      if (!justCheckingForActivity && who != null && (who.currentLocation.isObjectAt(who.getTileX(), who.getTileY() - 1) && who.currentLocation.isObjectAt(who.getTileX(), who.getTileY() + 1)) && (who.currentLocation.isObjectAt(who.getTileX() + 1, who.getTileY()) && who.currentLocation.isObjectAt(who.getTileX() - 1, who.getTileY())))
        this.performToolAction((Tool) null);
      if (this.name.Equals("Prairie King Arcade System"))
      {
        if (justCheckingForActivity)
          return true;
        Game1.currentMinigame = (IMinigame) new AbigailGame(false);
      }
      else if (this.name.Equals("Junimo Kart Arcade System"))
      {
        if (justCheckingForActivity)
          return true;
        Response[] answerChoices = new Response[3]
        {
          new Response("Progress", Game1.content.LoadString("Strings\\StringsFromCSFiles:Object.cs.12873")),
          new Response("Endless", Game1.content.LoadString("Strings\\StringsFromCSFiles:Object.cs.12875")),
          new Response("Exit", Game1.content.LoadString("Strings\\StringsFromCSFiles:TitleMenu.cs.11738"))
        };
        Game1.currentLocation.createQuestionDialogue(Game1.content.LoadString("Strings\\Locations:Saloon_Arcade_Minecart_Menu"), answerChoices, "MinecartGame");
      }
      else if (this.name.Equals("Staircase"))
      {
        if (who.currentLocation is MineShaft && (who.currentLocation as MineShaft).mineLevel != 120)
        {
          if (justCheckingForActivity)
            return true;
          Game1.enterMine(false, Game1.mine.mineLevel + 1, (string) null);
          Game1.playSound("stairsdown");
        }
      }
      else
      {
        if (this.name.Equals("Slime Ball"))
        {
          if (justCheckingForActivity)
            return true;
          who.currentLocation.objects.Remove(this.tileLocation);
          DelayedAction.playSoundAfterDelay("slimedead", 40);
          DelayedAction.playSoundAfterDelay("slimeHit", 100);
          Game1.playSound("slimeHit");
          Random random = new Random((int) Game1.stats.daysPlayed + (int) Game1.uniqueIDForThisGame + (int) this.tileLocation.X * 77 + (int) this.tileLocation.Y * 777 + 2);
          Game1.createMultipleObjectDebris(766, (int) this.tileLocation.X, (int) this.tileLocation.Y, random.Next(10, 21), (float) (1.0 + (who.facingDirection == 2 ? 0.0 : Game1.random.NextDouble())));
          TemporaryAnimatedSprite t = new TemporaryAnimatedSprite(44, this.tileLocation * (float) Game1.tileSize, Color.Lime, 10, false, 100f, 0, -1, -1f, -1, 0);
          t.interval = 70f;
          t.holdLastFrame = true;
          t.alphaFade = 0.01f;
          GameLocation currentLocation = Game1.currentLocation;
          int numAddOns = 4;
          int xRange = 64;
          int yRange = 64;
          Utility.makeTemporarySpriteJuicier(t, currentLocation, numAddOns, xRange, yRange);
          who.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(44, this.tileLocation * (float) Game1.tileSize + new Vector2((float) (-Game1.tileSize / 4), 0.0f), Color.Lime, 10, false, 100f, 0, -1, -1f, -1, 0)
          {
            interval = 70f,
            delayBeforeAnimationStart = 0,
            holdLastFrame = true,
            alphaFade = 0.01f
          });
          who.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(44, this.tileLocation * (float) Game1.tileSize + new Vector2(0.0f, (float) (Game1.tileSize / 4)), Color.Lime, 10, false, 100f, 0, -1, -1f, -1, 0)
          {
            interval = 70f,
            delayBeforeAnimationStart = 100,
            holdLastFrame = true,
            alphaFade = 0.01f
          });
          who.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(44, this.tileLocation * (float) Game1.tileSize + new Vector2((float) (Game1.tileSize / 4), 0.0f), Color.Lime, 10, false, 100f, 0, -1, -1f, -1, 0)
          {
            interval = 70f,
            delayBeforeAnimationStart = 200,
            holdLastFrame = true,
            alphaFade = 0.01f
          });
          while (random.NextDouble() < 0.33)
            Game1.createObjectDebris(557, (int) this.tileLocation.X, (int) this.tileLocation.Y, who.uniqueMultiplayerID);
          return false;
        }
        if (this.name.Equals("Furnace") && who.ActiveObject == null && !this.readyForHarvest)
        {
          if (this.heldObject != null)
          {
            int num = justCheckingForActivity ? 1 : 0;
            return true;
          }
        }
        else
        {
          if (this.name.Contains("Table"))
          {
            if (this.heldObject != null)
            {
              if (justCheckingForActivity || !who.addItemToInventoryBool((Item) this.heldObject, false))
                return true;
              this.heldObject = (Object) null;
              Game1.playSound("coin");
              return true;
            }
            if (!this.name.Equals("Tile Table"))
              return false;
            if (justCheckingForActivity)
              return true;
            this.parentSheetIndex = this.parentSheetIndex + 1;
            if (this.parentSheetIndex != 322)
              return true;
            this.parentSheetIndex = this.parentSheetIndex - 9;
            return false;
          }
          if (this.name.Contains("Stool"))
          {
            if (justCheckingForActivity)
              return true;
            this.parentSheetIndex = this.parentSheetIndex + 1;
            if (this.parentSheetIndex != 305)
              return true;
            this.parentSheetIndex = this.parentSheetIndex - 9;
            return false;
          }
          if (this.bigCraftable && (this.name.Contains("Chair") || this.name.Contains("Painting") || this.name.Equals("House Plant")))
          {
            if (justCheckingForActivity)
              return true;
            this.parentSheetIndex = this.parentSheetIndex + 1;
            int num1 = -1;
            int num2 = -1;
            string name = this.name;
            if (!(name == "Red Chair"))
            {
              if (!(name == "Patio Chair"))
              {
                if (!(name == "Dark Chair"))
                {
                  if (!(name == "Wood Chair"))
                  {
                    if (!(name == "House Plant"))
                    {
                      if (name == "Painting")
                      {
                        num1 = 8;
                        num2 = 32;
                      }
                    }
                    else
                    {
                      num1 = 8;
                      num2 = 0;
                    }
                  }
                  else
                  {
                    num1 = 4;
                    num2 = 24;
                  }
                }
                else
                {
                  num1 = 4;
                  num2 = 60;
                }
              }
              else
              {
                num1 = 4;
                num2 = 52;
              }
            }
            else
            {
              num1 = 4;
              num2 = 44;
            }
            if (this.parentSheetIndex != num2 + num1)
              return true;
            this.parentSheetIndex = this.parentSheetIndex - num1;
            return false;
          }
          if (this.name.Equals("Flute Block"))
          {
            if (justCheckingForActivity)
              return true;
            this.scale.X += 100f;
            this.scale.X %= 2400f;
            this.shakeTimer = 200;
            if (Game1.soundBank != null)
            {
              if (this.internalSound != null)
              {
                this.internalSound.Stop(AudioStopOptions.Immediate);
                this.internalSound = Game1.soundBank.GetCue("flute");
              }
              else
                this.internalSound = Game1.soundBank.GetCue("flute");
              this.internalSound.SetVariable("Pitch", this.scale.X);
              this.internalSound.Play();
            }
            this.scale.Y = 1.3f;
            this.shakeTimer = 200;
            return true;
          }
          if (this.name.Equals("Drum Block"))
          {
            if (justCheckingForActivity)
              return true;
            ++this.scale.X;
            this.scale.X %= 7f;
            this.shakeTimer = 200;
            if (Game1.soundBank != null)
            {
              if (this.internalSound != null)
              {
                this.internalSound.Stop(AudioStopOptions.Immediate);
                this.internalSound = Game1.soundBank.GetCue("drumkit" + (object) this.scale.X);
              }
              else
                this.internalSound = Game1.soundBank.GetCue("drumkit" + (object) this.scale.X);
              this.internalSound.Play();
            }
            this.scale.Y = 1.3f;
            this.shakeTimer = 200;
            return true;
          }
          if (this.name.Contains("arecrow"))
          {
            if (justCheckingForActivity)
              return true;
            this.shakeTimer = 100;
            if (this.parentSheetIndex == 126 && who.CurrentItem != null && who.CurrentItem is Hat)
            {
              if (this.quality != 0)
                Game1.createItemDebris((Item) new Hat(this.quality - 1), this.tileLocation * (float) Game1.tileSize, (who.facingDirection + 2) % 4, (GameLocation) null);
              this.quality = (who.CurrentItem as Hat).which + 1;
              who.items[who.CurrentToolIndex] = (Item) null;
              Game1.playSound("dirtyHit");
              return true;
            }
            if (this.specialVariable == 0)
            {
              Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\StringsFromCSFiles:Object.cs.12926"));
            }
            else
            {
              string dialogue;
              if (this.specialVariable != 1)
                dialogue = Game1.content.LoadString("Strings\\StringsFromCSFiles:Object.cs.12929", (object) this.specialVariable);
              else
                dialogue = Game1.content.LoadString("Strings\\StringsFromCSFiles:Object.cs.12927");
              Game1.drawObjectDialogue(dialogue);
            }
            return true;
          }
          if (this.name.Equals("Singing Stone"))
          {
            if (justCheckingForActivity)
              return true;
            if (Game1.soundBank != null)
            {
              Cue cue = Game1.soundBank.GetCue("crystal");
              int num1 = Game1.random.Next(2400);
              int num2 = num1 - num1 % 100;
              string name = "Pitch";
              double num3 = (double) num2;
              cue.SetVariable(name, (float) num3);
              this.shakeTimer = 100;
              cue.Play();
            }
          }
          else if (this.name.Contains("Hopper") && who.ActiveObject == null)
          {
            if (justCheckingForActivity)
              return true;
            if (who.freeSpotsInInventory() > 0)
            {
              int piecesOfHay = (Game1.getLocationFromName("Farm") as Farm).piecesOfHay;
              if (piecesOfHay > 0)
              {
                if (Game1.currentLocation is AnimalHouse)
                {
                  int val1 = Math.Max(1, Math.Min((Game1.currentLocation as AnimalHouse).animalsThatLiveHere.Count, piecesOfHay));
                  AnimalHouse currentLocation = Game1.currentLocation as AnimalHouse;
                  int num1 = currentLocation.numberOfObjectsWithName("Hay");
                  int num2 = Math.Min(val1, currentLocation.animalLimit - num1);
                  if (num2 != 0 && Game1.player.couldInventoryAcceptThisObject(178, num2, 0))
                  {
                    (Game1.getLocationFromName("Farm") as Farm).piecesOfHay -= Math.Max(1, num2);
                    who.addItemToInventoryBool((Item) new Object(178, num2, false, -1, 0), false);
                    Game1.playSound("shwip");
                  }
                }
                else if (Game1.player.couldInventoryAcceptThisObject(178, 1, 0))
                {
                  --(Game1.getLocationFromName("Farm") as Farm).piecesOfHay;
                  who.addItemToInventoryBool((Item) new Object(178, 1, false, -1, 0), false);
                  Game1.playSound("shwip");
                }
                if ((Game1.getLocationFromName("Farm") as Farm).piecesOfHay <= 0)
                  this.showNextIndex = false;
              }
              else
                Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\StringsFromCSFiles:Object.cs.12942"));
            }
            else
              Game1.showRedMessage(Game1.content.LoadString("Strings\\StringsFromCSFiles:Crop.cs.588"));
          }
        }
      }
      if (!this.readyForHarvest)
        return false;
      if (justCheckingForActivity)
        return true;
      if (this.name.Equals("Bee House"))
      {
        this.honeyType = new Object.HoneyType?(Object.HoneyType.Wild);
        string str = "Wild";
        int num = 0;
        if (who.currentLocation is Farm)
        {
          Crop closeFlower = Utility.findCloseFlower(this.tileLocation);
          if (closeFlower != null)
          {
            str = Game1.objectInformation[closeFlower.indexOfHarvest].Split('/')[0];
            switch (closeFlower.indexOfHarvest)
            {
              case 376:
                this.honeyType = new Object.HoneyType?(Object.HoneyType.Poppy);
                break;
              case 591:
                this.honeyType = new Object.HoneyType?(Object.HoneyType.Tulip);
                break;
              case 593:
                this.honeyType = new Object.HoneyType?(Object.HoneyType.SummerSpangle);
                break;
              case 595:
                this.honeyType = new Object.HoneyType?(Object.HoneyType.FairyRose);
                break;
              case 597:
                this.honeyType = new Object.HoneyType?(Object.HoneyType.BlueJazz);
                break;
            }
            num = Convert.ToInt32(Game1.objectInformation[closeFlower.indexOfHarvest].Split('/')[1]) * 2;
          }
        }
        if (this.heldObject != null)
        {
          this.heldObject.name = str + " Honey";
          this.heldObject.displayName = this.loadDisplayName();
          this.heldObject.price += num;
          if (Game1.currentSeason.Equals("winter"))
          {
            this.heldObject = (Object) null;
            this.readyForHarvest = false;
            this.showNextIndex = false;
            return false;
          }
          if (who.IsMainPlayer && !who.addItemToInventoryBool((Item) this.heldObject, false))
          {
            Game1.showRedMessage(Game1.content.LoadString("Strings\\StringsFromCSFiles:Crop.cs.588"));
            return false;
          }
          Game1.playSound("coin");
        }
      }
      else
      {
        if (who.IsMainPlayer && !who.addItemToInventoryBool((Item) this.heldObject, false))
        {
          Game1.showRedMessage(Game1.content.LoadString("Strings\\StringsFromCSFiles:Crop.cs.588"));
          return false;
        }
        Game1.playSound("coin");
        string name = this.name;
        if (!(name == "Keg"))
        {
          if (!(name == "Preserves Jar"))
          {
            if (name == "Cheese Press")
            {
              if (this.heldObject.ParentSheetIndex == 426)
                ++Game1.stats.GoatCheeseMade;
              else
                ++Game1.stats.CheeseMade;
            }
          }
          else
            ++Game1.stats.PreservesMade;
        }
        else
          ++Game1.stats.BeveragesMade;
      }
      if (this.name.Equals("Crystalarium"))
      {
        this.minutesUntilReady = this.getMinutesForCrystalarium(this.heldObject.ParentSheetIndex);
        this.heldObject = (Object) this.heldObject.getOne();
      }
      else if (this.name.Equals("Tapper"))
      {
        switch (this.heldObject.ParentSheetIndex)
        {
          case 422:
            this.minutesUntilReady = 3000 - Game1.timeOfDay;
            this.heldObject = new Object(420, 1, false, -1, 0);
            break;
          case 724:
            this.minutesUntilReady = 16000 - Game1.timeOfDay;
            break;
          case 725:
            this.minutesUntilReady = 13000 - Game1.timeOfDay;
            break;
          case 726:
            this.minutesUntilReady = 10000 - Game1.timeOfDay;
            break;
          case 404:
          case 420:
            this.minutesUntilReady = 3000 - Game1.timeOfDay;
            if (!Game1.currentSeason.Equals("fall"))
            {
              this.heldObject = new Object(404, 1, false, -1, 0);
              this.minutesUntilReady = 6000 - Game1.timeOfDay;
            }
            if (Game1.dayOfMonth % 10 == 0)
              this.heldObject = new Object(422, 1, false, -1, 0);
            if (Game1.currentSeason.Equals("winter"))
            {
              this.minutesUntilReady = 80000 - Game1.timeOfDay;
              break;
            }
            break;
        }
        if (this.heldObject != null)
          this.heldObject = (Object) this.heldObject.getOne();
      }
      else
        this.heldObject = (Object) null;
      this.readyForHarvest = false;
      this.showNextIndex = false;
      if (this.name.Equals("Bee House") && !Game1.currentSeason.Equals("winter"))
      {
        this.heldObject = new Object(Vector2.Zero, 340, (string) null, false, true, false, false);
        this.minutesUntilReady = 2400 - Game1.timeOfDay + 4320;
      }
      else if (this.name.Equals("Worm Bin"))
      {
        this.heldObject = new Object(685, Game1.random.Next(2, 6), false, -1, 0);
        this.minutesUntilReady = 2600 - Game1.timeOfDay;
      }
      return true;
    }

    public void farmerAdjacentAction()
    {
      if (this.name == null)
        return;
      if (this.name.Equals("Flute Block") && (this.internalSound == null || (double) Game1.noteBlockTimer == 0.0 && !this.internalSound.IsPlaying) && !Game1.dialogueUp)
      {
        if (Game1.soundBank != null)
        {
          this.internalSound = Game1.soundBank.GetCue("flute");
          this.internalSound.SetVariable("Pitch", this.scale.X);
          this.internalSound.Play();
        }
        this.scale.Y = 1.3f;
        this.shakeTimer = 200;
      }
      else if (this.name.Equals("Drum Block") && (this.internalSound == null || (double) Game1.noteBlockTimer == 0.0 && !this.internalSound.IsPlaying) && !Game1.dialogueUp)
      {
        if (Game1.soundBank != null)
        {
          this.internalSound = Game1.soundBank.GetCue("drumkit" + (object) this.scale.X);
          this.internalSound.Play();
        }
        this.scale.Y = 1.3f;
        this.shakeTimer = 200;
      }
      else
      {
        if (!this.name.Equals("Obelisk"))
          return;
        ++this.scale.X;
        if ((double) this.scale.X > 30.0)
        {
          this.parentSheetIndex = this.parentSheetIndex == 29 ? 30 : 29;
          this.scale.X = 0.0f;
          this.scale.Y += 2f;
        }
        if ((double) this.scale.Y < 20.0 || Game1.random.NextDouble() >= 0.0001 || Game1.currentLocation.characters.Count >= 4)
          return;
        Vector2 tileLocation1 = Game1.player.getTileLocation();
        foreach (Vector2 adjacentTilesOffset in Character.AdjacentTilesOffsets)
        {
          Vector2 tileLocation2 = tileLocation1 + adjacentTilesOffset;
          if (!Game1.currentLocation.isTileOccupied(tileLocation2, "") && Game1.currentLocation.isTilePassable(new Location((int) tileLocation2.X, (int) tileLocation2.Y), Game1.viewport) && (object) Game1.currentLocation.isCharacterAtTile(tileLocation2) == null)
          {
            if (Game1.random.NextDouble() < 0.1)
              Game1.currentLocation.characters.Add((NPC) new GreenSlime(tileLocation2 * new Vector2((float) Game1.tileSize, (float) Game1.tileSize)));
            else if (Game1.random.NextDouble() < 0.5)
              Game1.currentLocation.characters.Add((NPC) new ShadowGuy(tileLocation2 * new Vector2((float) Game1.tileSize, (float) Game1.tileSize)));
            else
              Game1.currentLocation.characters.Add((NPC) new ShadowGirl(tileLocation2 * new Vector2((float) Game1.tileSize, (float) Game1.tileSize)));
            Game1.currentLocation.characters[Game1.currentLocation.characters.Count - 1].moveTowardPlayerThreshold = 4;
            Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(352, 400f, 2, 1, tileLocation2 * new Vector2((float) Game1.tileSize, (float) Game1.tileSize), false, false));
            Game1.playSound("shadowpeep");
            break;
          }
        }
      }
    }

    public void addWorkingAnimation(GameLocation environment)
    {
      if (environment == null || environment.getFarmersCount() == 0)
        return;
      string name = this.name;
      if (!(name == "Keg"))
      {
        if (!(name == "Preserves Jar"))
        {
          if (!(name == "Oil Maker"))
          {
            if (!(name == "Furnace"))
            {
              if (!(name == "Slime Egg-Press"))
                return;
              environment.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.animations, new Microsoft.Xna.Framework.Rectangle(256, 1856, 64, 128), 80f, 6, 999999, this.tileLocation * (float) Game1.tileSize + new Vector2(0.0f, (float) -Game1.tileSize * 2.5f), false, false, (float) (((double) this.tileLocation.Y + 1.0) * (double) Game1.tileSize / 10000.0 + 9.99999974737875E-05), 0.0f, Color.Lime, 1f, 0.0f, 0.0f, 0.0f, false)
              {
                alphaFade = 0.005f
              });
            }
            else
            {
              if (Game1.random.NextDouble() >= 0.5)
                return;
              environment.temporarySprites.Add(new TemporaryAnimatedSprite(30, this.tileLocation * (float) Game1.tileSize + new Vector2(0.0f, (float) (-Game1.tileSize / 4)), Color.White, 4, false, 50f, 10, Game1.tileSize, (float) (((double) this.tileLocation.Y + 1.0) * (double) Game1.tileSize / 10000.0 + 9.99999974737875E-05), -1, 0)
              {
                alphaFade = 0.005f,
                light = true,
                lightcolor = Color.Black
              });
              Game1.playSound("fireball");
            }
          }
          else
            environment.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.animations, new Microsoft.Xna.Framework.Rectangle(256, 1856, 64, 128), 80f, 6, 999999, this.tileLocation * (float) Game1.tileSize + new Vector2(0.0f, (float) (-Game1.tileSize * 2)), false, false, (float) (((double) this.tileLocation.Y + 1.0) * (double) Game1.tileSize / 10000.0 + 9.99999974737875E-05), 0.0f, Color.Yellow, 1f, 0.0f, 0.0f, 0.0f, false)
            {
              alphaFade = 0.005f
            });
        }
        else
        {
          Color color = Color.White;
          if (this.heldObject.Name.Contains("Pickled"))
            color = Color.White;
          else if (this.heldObject.Name.Contains("Jelly"))
            color = Color.LightBlue;
          environment.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.animations, new Microsoft.Xna.Framework.Rectangle(256, 1856, 64, 128), 80f, 6, 999999, this.tileLocation * (float) Game1.tileSize + new Vector2(0.0f, (float) (-Game1.tileSize * 2)), false, false, (float) (((double) this.tileLocation.Y + 1.0) * (double) Game1.tileSize / 10000.0 + 9.99999974737875E-05), 0.0f, color * 0.75f, 1f, 0.0f, 0.0f, 0.0f, false)
          {
            alphaFade = 0.005f
          });
        }
      }
      else
      {
        Color color = Color.DarkGray;
        if (this.heldObject.Name.Contains("Wine"))
          color = Color.Lavender;
        else if (this.heldObject.Name.Contains("Juice"))
          color = Color.White;
        else if (this.heldObject.name.Equals("Beer"))
          color = Color.Yellow;
        environment.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.animations, new Microsoft.Xna.Framework.Rectangle(256, 1856, 64, 128), 80f, 6, 999999, this.tileLocation * (float) Game1.tileSize + new Vector2(0.0f, (float) (-Game1.tileSize * 2)), false, false, (float) (((double) this.tileLocation.Y + 1.0) * (double) Game1.tileSize / 10000.0 + 9.99999974737875E-05), 0.0f, color * 0.75f, 1f, 0.0f, 0.0f, 0.0f, false)
        {
          alphaFade = 0.005f
        });
        Game1.playSound("bubbles");
      }
    }

    public virtual bool minutesElapsed(int minutes, GameLocation environment)
    {
      if (this.heldObject != null && !this.name.Contains("Table"))
      {
        if (this.name.Equals("Bee House") && !environment.IsOutdoors)
          return false;
        this.minutesUntilReady = this.minutesUntilReady - minutes;
        if (this.minutesUntilReady <= 0 && !this.name.Contains("Incubator"))
        {
          if (!this.readyForHarvest && Game1.currentLocation.Equals((object) environment))
            Game1.playSound("dwop");
          this.readyForHarvest = true;
          this.minutesUntilReady = 0;
          this.showNextIndex = false;
          if (this.name.Equals("Bee House") || this.name.Equals("Loom") || this.name.Equals("Mushroom Box"))
            this.showNextIndex = true;
          if (this.lightSource != null)
          {
            Utility.removeLightSource(this.lightSource.identifier);
            this.lightSource = (LightSource) null;
          }
        }
        if (!this.readyForHarvest && Game1.random.NextDouble() < 0.33)
          this.addWorkingAnimation(environment);
      }
      else if (this.bigCraftable)
      {
        switch (this.parentSheetIndex)
        {
          case 141:
          case 142:
            this.showNextIndex = this.parentSheetIndex == 141;
            break;
          case 96:
          case 97:
            this.minutesUntilReady = this.minutesUntilReady - minutes;
            this.showNextIndex = this.parentSheetIndex == 96;
            if (this.minutesUntilReady <= 0)
            {
              environment.objects.Remove(this.tileLocation);
              environment.objects.Add(this.tileLocation, new Object(this.tileLocation, 98, false));
              break;
            }
            break;
          case 29:
          case 30:
            this.showNextIndex = this.parentSheetIndex == 29;
            this.scale.Y = Math.Max(0.0f, this.scale.Y -= (float) (minutes / 2 + 1));
            break;
          case 83:
            this.showNextIndex = false;
            Utility.removeLightSource((int) ((double) this.tileLocation.X * 797.0 + (double) this.tileLocation.Y * 13.0 + 666.0));
            break;
        }
      }
      return false;
    }

    public override string checkForSpecialItemHoldUpMeessage()
    {
      if (!this.bigCraftable)
      {
        if (this.type != null && this.type.Equals("Arch"))
          return Game1.content.LoadString("Strings\\StringsFromCSFiles:Object.cs.12993");
        switch (this.parentSheetIndex)
        {
          case 102:
            return Game1.content.LoadString("Strings\\StringsFromCSFiles:Object.cs.12994");
          case 535:
            return Game1.content.LoadString("Strings\\StringsFromCSFiles:Object.cs.12995");
        }
      }
      else if (this.parentSheetIndex == 160)
        return Game1.content.LoadString("Strings\\StringsFromCSFiles:Object.cs.12996");
      return base.checkForSpecialItemHoldUpMeessage();
    }

    public bool countsForShippedCollection()
    {
      if (this.type == null || this.type.Contains("Arch") || this.bigCraftable)
        return false;
      if (this.parentSheetIndex == 433)
        return true;
      switch (this.Category)
      {
        case -2:
        case 0:
        case -8:
        case -7:
        case -14:
        case -12:
        case -74:
        case -29:
        case -24:
        case -22:
        case -21:
        case -20:
        case -19:
          return false;
        default:
          return Object.isIndexOkForBasicShippedCategory(this.parentSheetIndex);
      }
    }

    public static bool isIndexOkForBasicShippedCategory(int index)
    {
      return index != 434;
    }

    public static bool isPotentialBasicShippedCategory(int index, string category)
    {
      int result = 0;
      int.TryParse(category, out result);
      if (index == 433)
        return true;
      switch (result)
      {
        case 0:
          return false;
        case -7:
        case -2:
        case -12:
        case -8:
        case -74:
        case -29:
        case -24:
        case -22:
        case -21:
        case -20:
        case -19:
        case -14:
          return false;
        default:
          return Object.isIndexOkForBasicShippedCategory(index);
      }
    }

    public Vector2 getScale()
    {
      if (this.category == -22)
        return Vector2.Zero;
      if (!this.bigCraftable)
      {
        this.scale.Y = Math.Max((float) Game1.pixelZoom, this.scale.Y - (float) Game1.pixelZoom / 100f);
        return this.scale;
      }
      if (this.heldObject == null && this.minutesUntilReady <= 0 || (this.readyForHarvest || this.name.Equals("Bee House")) || (this.name.Contains("Table") || this.name.Equals("Tapper")))
        return Vector2.Zero;
      if (this.name.Equals("Loom"))
      {
        this.scale.X = (float) (((double) this.scale.X + (double) Game1.pixelZoom / 100.0) % (2.0 * Math.PI));
        return Vector2.Zero;
      }
      this.scale.X -= 0.1f;
      this.scale.Y += 0.1f;
      if ((double) this.scale.X <= 0.0)
        this.scale.X = 10f;
      if ((double) this.scale.Y >= 10.0)
        this.scale.Y = 0.0f;
      return new Vector2(Math.Abs(this.scale.X - 5f), Math.Abs(this.scale.Y - 5f));
    }

    public virtual void drawWhenHeld(SpriteBatch spriteBatch, Vector2 objectPosition, Farmer f)
    {
      if (f.ActiveObject.bigCraftable)
      {
        spriteBatch.Draw(Game1.bigCraftableSpriteSheet, objectPosition, new Microsoft.Xna.Framework.Rectangle?(Object.getSourceRectForBigCraftable(f.ActiveObject.ParentSheetIndex)), Color.White, 0.0f, Vector2.Zero, (float) Game1.pixelZoom, SpriteEffects.None, Math.Max(0.0f, (float) (f.getStandingY() + 2) / 10000f));
      }
      else
      {
        spriteBatch.Draw(Game1.objectSpriteSheet, objectPosition, new Microsoft.Xna.Framework.Rectangle?(Game1.currentLocation.getSourceRectForObject(f.ActiveObject.ParentSheetIndex)), Color.White, 0.0f, Vector2.Zero, (float) Game1.pixelZoom, SpriteEffects.None, Math.Max(0.0f, (float) (f.getStandingY() + 2) / 10000f));
        if (f.ActiveObject == null || !f.ActiveObject.Name.Contains("="))
          return;
        spriteBatch.Draw(Game1.objectSpriteSheet, objectPosition + new Vector2((float) (Game1.tileSize / 2), (float) (Game1.tileSize / 2)), new Microsoft.Xna.Framework.Rectangle?(Game1.currentLocation.getSourceRectForObject(f.ActiveObject.ParentSheetIndex)), Color.White, 0.0f, new Vector2((float) (Game1.tileSize / 2), (float) (Game1.tileSize / 2)), (float) Game1.pixelZoom + Math.Abs(Game1.starCropShimmerPause) / 8f, SpriteEffects.None, Math.Max(0.0f, (float) (f.getStandingY() + 2) / 10000f));
        if ((double) Math.Abs(Game1.starCropShimmerPause) <= 0.0500000007450581 && Game1.random.NextDouble() < 0.97)
          return;
        Game1.starCropShimmerPause += 0.04f;
        if ((double) Game1.starCropShimmerPause < 0.800000011920929)
          return;
        Game1.starCropShimmerPause = -0.8f;
      }
    }

    public virtual void drawPlacementBounds(SpriteBatch spriteBatch, GameLocation location)
    {
      if (!this.isPlaceable() || this is Wallpaper)
        return;
      int x = Game1.getOldMouseX() + Game1.viewport.X;
      int y = Game1.getOldMouseY() + Game1.viewport.Y;
      if ((double) Game1.mouseCursorTransparency == 0.0)
      {
        x = (int) Game1.player.GetGrabTile().X * Game1.tileSize;
        y = (int) Game1.player.GetGrabTile().Y * Game1.tileSize;
      }
      if (Game1.player.GetGrabTile().Equals(Game1.player.getTileLocation()) && (double) Game1.mouseCursorTransparency == 0.0)
      {
        Vector2 translatedVector2 = Utility.getTranslatedVector2(Game1.player.GetGrabTile(), Game1.player.facingDirection, 1f);
        x = (int) translatedVector2.X * Game1.tileSize;
        y = (int) translatedVector2.Y * Game1.tileSize;
      }
      bool flag = Utility.playerCanPlaceItemHere(location, (Item) this, x, y, Game1.player);
      spriteBatch.Draw(Game1.mouseCursors, new Vector2((float) (x / Game1.tileSize * Game1.tileSize - Game1.viewport.X), (float) (y / Game1.tileSize * Game1.tileSize - Game1.viewport.Y)), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(flag ? 194 : 210, 388, 16, 16)), Color.White, 0.0f, Vector2.Zero, (float) Game1.pixelZoom, SpriteEffects.None, 0.01f);
      if (!this.bigCraftable && !(this is Furniture) && (this.category == -74 || this.category == -19))
        return;
      this.draw(spriteBatch, x / Game1.tileSize, y / Game1.tileSize, 0.5f);
    }

    public override void drawInMenu(SpriteBatch spriteBatch, Vector2 location, float scaleSize, float transparency, float layerDepth, bool drawStackNumber)
    {
      if (this.isRecipe)
      {
        transparency = 0.5f;
        scaleSize *= 0.75f;
      }
      if (this.bigCraftable)
      {
        Microsoft.Xna.Framework.Rectangle rectForBigCraftable = Object.getSourceRectForBigCraftable(this.parentSheetIndex);
        spriteBatch.Draw(Game1.bigCraftableSpriteSheet, location + new Vector2((float) (Game1.tileSize / 2), (float) (Game1.tileSize / 2)), new Microsoft.Xna.Framework.Rectangle?(rectForBigCraftable), Color.White * transparency, 0.0f, new Vector2(8f, 16f), (float) Game1.pixelZoom * ((double) scaleSize < 0.2 ? scaleSize : scaleSize / 2f), SpriteEffects.None, layerDepth);
      }
      else
      {
        if (this.parentSheetIndex != 590)
          spriteBatch.Draw(Game1.shadowTexture, location + new Vector2((float) (Game1.tileSize / 2), (float) (Game1.tileSize * 3 / 4)), new Microsoft.Xna.Framework.Rectangle?(Game1.shadowTexture.Bounds), Color.White * 0.5f, 0.0f, new Vector2((float) Game1.shadowTexture.Bounds.Center.X, (float) Game1.shadowTexture.Bounds.Center.Y), 3f, SpriteEffects.None, layerDepth - 0.0001f);
        spriteBatch.Draw(Game1.objectSpriteSheet, location + new Vector2((float) (int) ((double) (Game1.tileSize / 2) * (double) scaleSize), (float) (int) ((double) (Game1.tileSize / 2) * (double) scaleSize)), new Microsoft.Xna.Framework.Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, this.parentSheetIndex, 16, 16)), Color.White * transparency, 0.0f, new Vector2(8f, 8f) * scaleSize, (float) Game1.pixelZoom * scaleSize, SpriteEffects.None, layerDepth);
        if (drawStackNumber && this.maximumStackSize() > 1 && ((double) scaleSize > 0.3 && this.Stack != int.MaxValue) && this.Stack > 1)
          Utility.drawTinyDigits(this.stack, spriteBatch, location + new Vector2((float) (Game1.tileSize - Utility.getWidthOfTinyDigitString(this.stack, 3f * scaleSize)) + 3f * scaleSize, (float) ((double) Game1.tileSize - 18.0 * (double) scaleSize + 2.0)), 3f * scaleSize, 1f, Color.White);
        if (drawStackNumber && this.quality > 0)
        {
          float num = this.quality < 4 ? 0.0f : (float) ((Math.Cos((double) Game1.currentGameTime.TotalGameTime.Milliseconds * Math.PI / 512.0) + 1.0) * 0.0500000007450581);
          spriteBatch.Draw(Game1.mouseCursors, location + new Vector2(12f, (float) (Game1.tileSize - 12) + num), new Microsoft.Xna.Framework.Rectangle?(this.quality < 4 ? new Microsoft.Xna.Framework.Rectangle(338 + (this.quality - 1) * 8, 400, 8, 8) : new Microsoft.Xna.Framework.Rectangle(346, 392, 8, 8)), Color.White * transparency, 0.0f, new Vector2(4f, 4f), (float) (3.0 * (double) scaleSize * (1.0 + (double) num)), SpriteEffects.None, layerDepth);
        }
        if (this.category == -22 && (double) this.scale.Y < 1.0)
          spriteBatch.Draw(Game1.staminaRect, new Microsoft.Xna.Framework.Rectangle((int) location.X, (int) ((double) location.Y + (double) (Game1.tileSize - 2 * Game1.pixelZoom) * (double) scaleSize), (int) ((double) Game1.tileSize * (double) scaleSize * (double) this.scale.Y), (int) ((double) (2 * Game1.pixelZoom) * (double) scaleSize)), Utility.getRedToGreenLerpColor(this.scale.Y));
      }
      if (!this.isRecipe)
        return;
      spriteBatch.Draw(Game1.objectSpriteSheet, location + new Vector2((float) (Game1.tileSize / 4), (float) (Game1.tileSize / 4)), new Microsoft.Xna.Framework.Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, 451, 16, 16)), Color.White, 0.0f, Vector2.Zero, (float) ((double) Game1.pixelZoom * 3.0 / 4.0), SpriteEffects.None, layerDepth + 0.0001f);
    }

    public void drawAsProp(SpriteBatch b)
    {
      int x1 = (int) this.tileLocation.X;
      int y1 = (int) this.tileLocation.Y;
      if (this.bigCraftable)
      {
        Vector2 vector2 = this.getScale() * (float) Game1.pixelZoom;
        Vector2 local = Game1.GlobalToLocal(Game1.viewport, new Vector2((float) (x1 * Game1.tileSize), (float) (y1 * Game1.tileSize - Game1.tileSize)));
        Microsoft.Xna.Framework.Rectangle destinationRectangle = new Microsoft.Xna.Framework.Rectangle((int) ((double) local.X - (double) vector2.X / 2.0), (int) ((double) local.Y - (double) vector2.Y / 2.0), (int) ((double) Game1.tileSize + (double) vector2.X), (int) ((double) (Game1.tileSize * 2) + (double) vector2.Y / 2.0));
        b.Draw(Game1.bigCraftableSpriteSheet, destinationRectangle, new Microsoft.Xna.Framework.Rectangle?(Object.getSourceRectForBigCraftable(this.showNextIndex ? this.ParentSheetIndex + 1 : this.ParentSheetIndex)), Color.White, 0.0f, Vector2.Zero, SpriteEffects.None, Math.Max(0.0f, (float) ((y1 + 1) * Game1.tileSize - 1) / 10000f) + (this.parentSheetIndex == 105 ? 0.0015f : 0.0f));
        if (!this.Name.Equals("Loom") || this.minutesUntilReady <= 0)
          return;
        b.Draw(Game1.objectSpriteSheet, this.getLocalPosition(Game1.viewport) + new Vector2((float) (Game1.tileSize / 2), 0.0f), new Microsoft.Xna.Framework.Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, 435, -1, -1)), Color.White, this.scale.X, new Vector2((float) (Game1.tileSize / 2), (float) (Game1.tileSize / 2)), 1f, SpriteEffects.None, Math.Max(0.0f, (float) ((double) ((y1 + 1) * Game1.tileSize - 1) / 10000.0 + 9.99999974737875E-05)));
      }
      else
      {
        if (this.parentSheetIndex != 590 && this.parentSheetIndex != 742)
        {
          SpriteBatch spriteBatch = b;
          Texture2D shadowTexture = Game1.shadowTexture;
          Vector2 position = this.getLocalPosition(Game1.viewport) + new Vector2((float) (Game1.tileSize / 2), (float) (Game1.tileSize * 5 / 6));
          Microsoft.Xna.Framework.Rectangle? sourceRectangle = new Microsoft.Xna.Framework.Rectangle?(Game1.shadowTexture.Bounds);
          Color white = Color.White;
          double num1 = 0.0;
          Microsoft.Xna.Framework.Rectangle bounds = Game1.shadowTexture.Bounds;
          double x2 = (double) bounds.Center.X;
          bounds = Game1.shadowTexture.Bounds;
          double y2 = (double) bounds.Center.Y;
          Vector2 origin = new Vector2((float) x2, (float) y2);
          double num2 = 4.0;
          int num3 = 0;
          double num4 = (double) this.getBoundingBox(new Vector2((float) x1, (float) y1)).Bottom / 15000.0;
          spriteBatch.Draw(shadowTexture, position, sourceRectangle, white, (float) num1, origin, (float) num2, (SpriteEffects) num3, (float) num4);
        }
        SpriteBatch spriteBatch1 = b;
        Texture2D objectSpriteSheet = Game1.objectSpriteSheet;
        Vector2 local = Game1.GlobalToLocal(Game1.viewport, new Vector2((float) (x1 * Game1.tileSize + Game1.tileSize / 2), (float) (y1 * Game1.tileSize + Game1.tileSize / 2)));
        Microsoft.Xna.Framework.Rectangle? sourceRectangle1 = new Microsoft.Xna.Framework.Rectangle?(Game1.currentLocation.getSourceRectForObject(this.ParentSheetIndex));
        Color white1 = Color.White;
        double num5 = 0.0;
        Vector2 origin1 = new Vector2(8f, 8f);
        Vector2 scale = this.scale;
        double num6 = (double) this.scale.Y > 1.0 ? (double) this.getScale().Y : (double) Game1.pixelZoom;
        int num7 = this.flipped ? 1 : 0;
        double num8 = (double) this.getBoundingBox(new Vector2((float) x1, (float) y1)).Bottom / 10000.0;
        spriteBatch1.Draw(objectSpriteSheet, local, sourceRectangle1, white1, (float) num5, origin1, (float) num6, (SpriteEffects) num7, (float) num8);
      }
    }

    public virtual void draw(SpriteBatch spriteBatch, int x, int y, float alpha = 1f)
    {
      if (this.bigCraftable)
      {
        Vector2 vector2 = this.getScale() * (float) Game1.pixelZoom;
        Vector2 local = Game1.GlobalToLocal(Game1.viewport, new Vector2((float) (x * Game1.tileSize), (float) (y * Game1.tileSize - Game1.tileSize)));
        Microsoft.Xna.Framework.Rectangle destinationRectangle = new Microsoft.Xna.Framework.Rectangle((int) ((double) local.X - (double) vector2.X / 2.0) + (this.shakeTimer > 0 ? Game1.random.Next(-1, 2) : 0), (int) ((double) local.Y - (double) vector2.Y / 2.0) + (this.shakeTimer > 0 ? Game1.random.Next(-1, 2) : 0), (int) ((double) Game1.tileSize + (double) vector2.X), (int) ((double) (Game1.tileSize * 2) + (double) vector2.Y / 2.0));
        spriteBatch.Draw(Game1.bigCraftableSpriteSheet, destinationRectangle, new Microsoft.Xna.Framework.Rectangle?(Object.getSourceRectForBigCraftable(this.showNextIndex ? this.ParentSheetIndex + 1 : this.ParentSheetIndex)), Color.White * alpha, 0.0f, Vector2.Zero, SpriteEffects.None, (float) ((double) Math.Max(0.0f, (float) ((y + 1) * Game1.tileSize - Game1.pixelZoom * 6) / 10000f) + (this.parentSheetIndex == 105 ? 0.00350000010803342 : 0.0) + (double) x * 9.99999974737875E-06));
        if (this.Name.Equals("Loom") && this.minutesUntilReady > 0)
          spriteBatch.Draw(Game1.objectSpriteSheet, this.getLocalPosition(Game1.viewport) + new Vector2((float) (Game1.tileSize / 2), 0.0f), new Microsoft.Xna.Framework.Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, 435, 16, 16)), Color.White * alpha, this.scale.X, new Vector2(8f, 8f), (float) Game1.pixelZoom, SpriteEffects.None, Math.Max(0.0f, (float) ((double) ((y + 1) * Game1.tileSize) / 10000.0 + 9.99999974737875E-05 + (double) x * 9.99999974737875E-06)));
        if (this.isLamp && Game1.isDarkOut())
          spriteBatch.Draw(Game1.mouseCursors, local + new Vector2((float) (-Game1.tileSize / 2), (float) (-Game1.tileSize / 2)), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(88, 1779, 32, 32)), Color.White * 0.75f, 0.0f, Vector2.Zero, (float) Game1.pixelZoom, SpriteEffects.None, Math.Max(0.0f, (float) ((y + 1) * Game1.tileSize - Game1.pixelZoom * 5) / 10000f));
        if (this.parentSheetIndex == 126 && this.quality != 0)
          spriteBatch.Draw(FarmerRenderer.hatsTexture, local + new Vector2(-3f, -6f) * (float) Game1.pixelZoom, new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle((this.quality - 1) * 20 % FarmerRenderer.hatsTexture.Width, (this.quality - 1) * 20 / FarmerRenderer.hatsTexture.Width * 20 * 4, 20, 20)), Color.White * alpha, 0.0f, Vector2.Zero, (float) Game1.pixelZoom, SpriteEffects.None, Math.Max(0.0f, (float) ((y + 1) * Game1.tileSize - Game1.pixelZoom * 5) / 10000f) + (float) x * 1E-05f);
      }
      else if (!Game1.eventUp || Game1.CurrentEvent != null && !Game1.CurrentEvent.isTileWalkedOn(x, y))
      {
        if (this.parentSheetIndex == 590)
        {
          SpriteBatch spriteBatch1 = spriteBatch;
          Texture2D mouseCursors = Game1.mouseCursors;
          Vector2 local = Game1.GlobalToLocal(Game1.viewport, new Vector2((float) (x * Game1.tileSize + Game1.tileSize / 2 + (this.shakeTimer > 0 ? Game1.random.Next(-1, 2) : 0)), (float) (y * Game1.tileSize + Game1.tileSize / 2 + (this.shakeTimer > 0 ? Game1.random.Next(-1, 2) : 0))));
          Microsoft.Xna.Framework.Rectangle? sourceRectangle = new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(368 + (Game1.currentGameTime.TotalGameTime.TotalMilliseconds % 1200.0 <= 400.0 ? (int) (Game1.currentGameTime.TotalGameTime.TotalMilliseconds % 400.0 / 100.0) * 16 : 0), 32, 16, 16));
          Color color = Color.White * alpha;
          double num1 = 0.0;
          Vector2 origin = new Vector2(8f, 8f);
          Vector2 scale = this.scale;
          double num2 = (double) this.scale.Y > 1.0 ? (double) this.getScale().Y : (double) Game1.pixelZoom;
          int num3 = this.flipped ? 1 : 0;
          double num4 = (this.isPassable() ? (double) this.getBoundingBox(new Vector2((float) x, (float) y)).Top : (double) this.getBoundingBox(new Vector2((float) x, (float) y)).Bottom) / 10000.0;
          spriteBatch1.Draw(mouseCursors, local, sourceRectangle, color, (float) num1, origin, (float) num2, (SpriteEffects) num3, (float) num4);
          return;
        }
        Microsoft.Xna.Framework.Rectangle boundingBox;
        if (this.fragility != 2)
        {
          SpriteBatch spriteBatch1 = spriteBatch;
          Texture2D shadowTexture = Game1.shadowTexture;
          Vector2 local = Game1.GlobalToLocal(Game1.viewport, new Vector2((float) (x * Game1.tileSize + Game1.tileSize / 2), (float) (y * Game1.tileSize + Game1.tileSize * 4 / 5 + Game1.pixelZoom)));
          Microsoft.Xna.Framework.Rectangle? sourceRectangle = new Microsoft.Xna.Framework.Rectangle?(Game1.shadowTexture.Bounds);
          Color color = Color.White * alpha;
          double num1 = 0.0;
          Vector2 origin = new Vector2((float) Game1.shadowTexture.Bounds.Center.X, (float) Game1.shadowTexture.Bounds.Center.Y);
          double num2 = 4.0;
          int num3 = 0;
          boundingBox = this.getBoundingBox(new Vector2((float) x, (float) y));
          double num4 = (double) boundingBox.Bottom / 15000.0;
          spriteBatch1.Draw(shadowTexture, local, sourceRectangle, color, (float) num1, origin, (float) num2, (SpriteEffects) num3, (float) num4);
        }
        SpriteBatch spriteBatch2 = spriteBatch;
        Texture2D objectSpriteSheet = Game1.objectSpriteSheet;
        Vector2 local1 = Game1.GlobalToLocal(Game1.viewport, new Vector2((float) (x * Game1.tileSize + Game1.tileSize / 2 + (this.shakeTimer > 0 ? Game1.random.Next(-1, 2) : 0)), (float) (y * Game1.tileSize + Game1.tileSize / 2 + (this.shakeTimer > 0 ? Game1.random.Next(-1, 2) : 0))));
        Microsoft.Xna.Framework.Rectangle? sourceRectangle1 = new Microsoft.Xna.Framework.Rectangle?(Game1.currentLocation.getSourceRectForObject(this.ParentSheetIndex));
        Color color1 = Color.White * alpha;
        double num5 = 0.0;
        Vector2 origin1 = new Vector2(8f, 8f);
        Vector2 scale1 = this.scale;
        double num6 = (double) this.scale.Y > 1.0 ? (double) this.getScale().Y : (double) Game1.pixelZoom;
        int num7 = this.flipped ? 1 : 0;
        int num8;
        if (!this.isPassable())
        {
          boundingBox = this.getBoundingBox(new Vector2((float) x, (float) y));
          num8 = boundingBox.Bottom;
        }
        else
        {
          boundingBox = this.getBoundingBox(new Vector2((float) x, (float) y));
          num8 = boundingBox.Top;
        }
        double num9 = (double) num8 / 10000.0;
        spriteBatch2.Draw(objectSpriteSheet, local1, sourceRectangle1, color1, (float) num5, origin1, (float) num6, (SpriteEffects) num7, (float) num9);
      }
      if (!this.readyForHarvest)
        return;
      float num = (float) (4.0 * Math.Round(Math.Sin(DateTime.Now.TimeOfDay.TotalMilliseconds / 250.0), 2));
      spriteBatch.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2((float) (x * Game1.tileSize - 8), (float) (y * Game1.tileSize - Game1.tileSize * 3 / 2 - 16) + num)), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(141, 465, 20, 24)), Color.White * 0.75f, 0.0f, Vector2.Zero, 4f, SpriteEffects.None, (float) ((double) ((y + 1) * Game1.tileSize) / 10000.0 + 9.99999997475243E-07 + (double) this.tileLocation.X / 10000.0 + (this.parentSheetIndex == 105 ? 0.00150000001303852 : 0.0)));
      spriteBatch.Draw(Game1.objectSpriteSheet, Game1.GlobalToLocal(Game1.viewport, new Vector2((float) (x * Game1.tileSize + Game1.tileSize / 2), (float) (y * Game1.tileSize - Game1.tileSize - Game1.tileSize / 8) + num)), new Microsoft.Xna.Framework.Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, this.heldObject.parentSheetIndex, 16, 16)), Color.White * 0.75f, 0.0f, new Vector2(8f, 8f), (float) Game1.pixelZoom, SpriteEffects.None, (float) ((double) ((y + 1) * Game1.tileSize) / 10000.0 + 9.99999974737875E-06 + (double) this.tileLocation.X / 10000.0 + (this.parentSheetIndex == 105 ? 0.00150000001303852 : 0.0)));
    }

    public virtual void draw(SpriteBatch spriteBatch, int xNonTile, int yNonTile, float layerDepth, float alpha = 1f)
    {
      if (this.bigCraftable)
      {
        Vector2 vector2 = this.getScale() * (float) Game1.pixelZoom;
        Vector2 local = Game1.GlobalToLocal(Game1.viewport, new Vector2((float) xNonTile, (float) yNonTile));
        Microsoft.Xna.Framework.Rectangle destinationRectangle = new Microsoft.Xna.Framework.Rectangle((int) ((double) local.X - (double) vector2.X / 2.0) + (this.shakeTimer > 0 ? Game1.random.Next(-1, 2) : 0), (int) ((double) local.Y - (double) vector2.Y / 2.0) + (this.shakeTimer > 0 ? Game1.random.Next(-1, 2) : 0), (int) ((double) Game1.tileSize + (double) vector2.X), (int) ((double) (Game1.tileSize * 2) + (double) vector2.Y / 2.0));
        spriteBatch.Draw(Game1.bigCraftableSpriteSheet, destinationRectangle, new Microsoft.Xna.Framework.Rectangle?(Object.getSourceRectForBigCraftable(this.showNextIndex ? this.ParentSheetIndex + 1 : this.ParentSheetIndex)), Color.White * alpha, 0.0f, Vector2.Zero, SpriteEffects.None, layerDepth);
        if (this.Name.Equals("Loom") && this.minutesUntilReady > 0)
          spriteBatch.Draw(Game1.objectSpriteSheet, Game1.GlobalToLocal(local) + new Vector2((float) (Game1.tileSize / 2), 0.0f), new Microsoft.Xna.Framework.Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, 435, 16, 16)), Color.White * alpha, this.scale.X, new Vector2(8f, 8f), (float) Game1.pixelZoom, SpriteEffects.None, layerDepth);
        if (!this.isLamp || !Game1.isDarkOut())
          return;
        spriteBatch.Draw(Game1.mouseCursors, local + new Vector2((float) (-Game1.tileSize / 2), (float) (-Game1.tileSize / 2)), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(88, 1779, 32, 32)), Color.White * 0.75f, 0.0f, Vector2.Zero, (float) Game1.pixelZoom, SpriteEffects.None, layerDepth);
      }
      else
      {
        if (Game1.eventUp && Game1.CurrentEvent.isTileWalkedOn(xNonTile / Game1.tileSize, yNonTile / Game1.tileSize))
          return;
        if (this.parentSheetIndex != 590 && this.fragility != 2)
        {
          SpriteBatch spriteBatch1 = spriteBatch;
          Texture2D shadowTexture = Game1.shadowTexture;
          Vector2 local = Game1.GlobalToLocal(Game1.viewport, new Vector2((float) (xNonTile + Game1.tileSize / 2), (float) (yNonTile + Game1.tileSize * 4 / 5 + Game1.pixelZoom)));
          Microsoft.Xna.Framework.Rectangle? sourceRectangle = new Microsoft.Xna.Framework.Rectangle?(Game1.shadowTexture.Bounds);
          Color color = Color.White * alpha;
          double num1 = 0.0;
          Microsoft.Xna.Framework.Rectangle bounds = Game1.shadowTexture.Bounds;
          double x = (double) bounds.Center.X;
          bounds = Game1.shadowTexture.Bounds;
          double y = (double) bounds.Center.Y;
          Vector2 origin = new Vector2((float) x, (float) y);
          double num2 = 4.0;
          int num3 = 0;
          double num4 = (double) layerDepth;
          spriteBatch1.Draw(shadowTexture, local, sourceRectangle, color, (float) num1, origin, (float) num2, (SpriteEffects) num3, (float) num4);
        }
        SpriteBatch spriteBatch2 = spriteBatch;
        Texture2D objectSpriteSheet = Game1.objectSpriteSheet;
        Vector2 local1 = Game1.GlobalToLocal(Game1.viewport, new Vector2((float) (xNonTile + Game1.tileSize / 2 + (this.shakeTimer > 0 ? Game1.random.Next(-1, 2) : 0)), (float) (yNonTile + Game1.tileSize / 2 + (this.shakeTimer > 0 ? Game1.random.Next(-1, 2) : 0))));
        Microsoft.Xna.Framework.Rectangle? sourceRectangle1 = new Microsoft.Xna.Framework.Rectangle?(Game1.currentLocation.getSourceRectForObject(this.ParentSheetIndex));
        Color color1 = Color.White * alpha;
        double num5 = 0.0;
        Vector2 origin1 = new Vector2(8f, 8f);
        Vector2 scale = this.scale;
        double num6 = (double) this.scale.Y > 1.0 ? (double) this.getScale().Y : (double) Game1.pixelZoom;
        int num7 = this.flipped ? 1 : 0;
        double num8 = (double) layerDepth;
        spriteBatch2.Draw(objectSpriteSheet, local1, sourceRectangle1, color1, (float) num5, origin1, (float) num6, (SpriteEffects) num7, (float) num8);
      }
    }

    private int getMinutesForCrystalarium(int whichGem)
    {
      switch (whichGem)
      {
        case 60:
          return 3000;
        case 62:
          return 2240;
        case 64:
          return 3000;
        case 66:
          return 1360;
        case 68:
          return 1120;
        case 70:
          return 2400;
        case 72:
          return 7200;
        case 80:
          return 420;
        case 82:
          return 1300;
        case 84:
          return 1120;
        case 86:
          return 800;
        default:
          return 5000;
      }
    }

    public override int maximumStackSize()
    {
      if (this.category == -22)
        return 1;
      return this.bigCraftable ? -1 : 999;
    }

    public override int getStack()
    {
      return this.stack;
    }

    public override int addToStack(int amount)
    {
      int num1 = this.maximumStackSize();
      if (num1 == 1)
        return amount;
      this.stack = this.stack + amount;
      if (this.stack <= num1)
        return 0;
      int num2 = this.stack - num1;
      this.stack = num1;
      return num2;
    }

    public virtual void hoverAction()
    {
    }

    public virtual bool clicked(Farmer who)
    {
      return false;
    }

    public override Item getOne()
    {
      if (!this.bigCraftable)
      {
        Object @object = new Object(this.tileLocation, this.parentSheetIndex, 1);
        @object.scale = this.scale;
        @object.quality = this.quality;
        int num1 = this.isSpawnedObject ? 1 : 0;
        @object.isSpawnedObject = num1 != 0;
        int num2 = this.isRecipe ? 1 : 0;
        @object.isRecipe = num2 != 0;
        int num3 = this.questItem ? 1 : 0;
        @object.questItem = num3 != 0;
        int num4 = 1;
        @object.stack = num4;
        string name = this.name;
        @object.name = name;
        int specialVariable = this.specialVariable;
        @object.specialVariable = specialVariable;
        int price = this.price;
        @object.price = price;
        return (Item) @object;
      }
      Object object1 = new Object(this.tileLocation, this.parentSheetIndex, false);
      int num = this.isRecipe ? 1 : 0;
      object1.isRecipe = num != 0;
      return (Item) object1;
    }

    public override bool canBePlacedHere(GameLocation l, Vector2 tile)
    {
      if (this.parentSheetIndex == 710 && l.doesTileHaveProperty((int) tile.X, (int) tile.Y, "Water", "Back") != null && (!l.objects.ContainsKey(tile) && l.doesTileHaveProperty((int) tile.X + 1, (int) tile.Y, "Water", "Back") != null) && l.doesTileHaveProperty((int) tile.X - 1, (int) tile.Y, "Water", "Back") != null || l.doesTileHaveProperty((int) tile.X, (int) tile.Y + 1, "Water", "Back") != null && l.doesTileHaveProperty((int) tile.X, (int) tile.Y - 1, "Water", "Back") != null || (this.parentSheetIndex == 105 && this.bigCraftable && (l.terrainFeatures.ContainsKey(tile) && l.terrainFeatures[tile] is Tree) && !l.objects.ContainsKey(tile) || this.name != null && this.name.Contains("Bomb") && (!l.isTileOccupiedForPlacement(tile, this) || l.isTileOccupiedByFarmer(tile) != null)))
        return true;
      if ((this.category == -74 || this.category == -19) && !l.isTileHoeDirt(tile))
      {
        switch (this.parentSheetIndex)
        {
          case 309:
          case 310:
          case 311:
          case 628:
          case 629:
          case 630:
          case 631:
          case 632:
          case 633:
            return true;
          default:
            return false;
        }
      }
      else
      {
        if (this.category == -19 && l.isTileHoeDirt(tile) && (l.terrainFeatures[tile] as HoeDirt).fertilizer != 0)
          return false;
        return !l.isTileOccupiedForPlacement(tile, this);
      }
    }

    public override bool isPlaceable()
    {
      int category = this.category;
      return this.type != null && (this.category == -8 || this.category == -9 || (this.type.Equals("Crafting") || this.name.Contains("Sapling")) || (this.parentSheetIndex == 710 || this.category == -74 || this.category == -19)) && this.edibility < 0;
    }

    public virtual bool placementAction(GameLocation location, int x, int y, Farmer who = null)
    {
      Vector2 index1 = new Vector2((float) (x / Game1.tileSize), (float) (y / Game1.tileSize));
      this.health = 10;
      this.owner = who == null ? Game1.player.uniqueMultiplayerID : who.uniqueMultiplayerID;
      if (!this.bigCraftable && !(this is Furniture))
      {
        switch (this.ParentSheetIndex)
        {
          case 405:
            if (location.terrainFeatures.ContainsKey(index1))
              return false;
            location.terrainFeatures.Add(index1, (TerrainFeature) new Flooring(6));
            Game1.playSound("woodyStep");
            return true;
          case 407:
            if (location.terrainFeatures.ContainsKey(index1))
              return false;
            location.terrainFeatures.Add(index1, (TerrainFeature) new Flooring(5));
            Game1.playSound("dirtyHit");
            return true;
          case 409:
            if (location.terrainFeatures.ContainsKey(index1))
              return false;
            location.terrainFeatures.Add(index1, (TerrainFeature) new Flooring(7));
            Game1.playSound("stoneStep");
            return true;
          case 411:
            if (location.terrainFeatures.ContainsKey(index1))
              return false;
            location.terrainFeatures.Add(index1, (TerrainFeature) new Flooring(8));
            Game1.playSound("stoneStep");
            return true;
          case 415:
            if (location.terrainFeatures.ContainsKey(index1))
              return false;
            location.terrainFeatures.Add(index1, (TerrainFeature) new Flooring(9));
            Game1.playSound("stoneStep");
            return true;
          case 710:
            if (location.objects.ContainsKey(index1) || location.doesTileHaveProperty((int) index1.X, (int) index1.Y, "Water", "Back") == null || location.doesTileHaveProperty((int) index1.X, (int) index1.Y, "Passable", "Buildings") != null)
              return false;
            new CrabPot(index1, 1).placementAction(location, x, y, who);
            return true;
          case 309:
          case 310:
          case 311:
            bool flag = location.terrainFeatures.ContainsKey(index1) && location.terrainFeatures[index1] is HoeDirt && (location.terrainFeatures[index1] as HoeDirt).crop == null;
            string str = location.doesTileHaveProperty((int) index1.X, (int) index1.Y, "NoSpawn", "Back");
            if (!flag && (location.objects.ContainsKey(index1) || location.terrainFeatures.ContainsKey(index1) || !(location is Farm) && !location.name.Contains("Greenhouse") || str != null && (str.Equals("Tree") || str.Equals("All") || str.Equals("True"))))
            {
              Game1.showRedMessage(Game1.content.LoadString("Strings\\StringsFromCSFiles:Object.cs.13021"));
              return false;
            }
            if (str != null && (str.Equals("Tree") || str.Equals("All") || str.Equals("True")))
              return false;
            if (flag || location.isTileLocationOpen(new Location(x * Game1.tileSize, y * Game1.tileSize)) && !location.isTileOccupied(new Vector2((float) x, (float) y), "") && location.doesTileHaveProperty(x, y, "Water", "Back") == null)
            {
              int which = 1;
              switch (this.parentSheetIndex)
              {
                case 310:
                  which = 2;
                  break;
                case 311:
                  which = 3;
                  break;
              }
              location.terrainFeatures.Remove(index1);
              location.terrainFeatures.Add(index1, (TerrainFeature) new Tree(which, 0));
              Game1.playSound("dirtyHit");
              return true;
            }
            break;
          case 322:
            if (location.objects.ContainsKey(index1))
              return false;
            location.objects.Add(index1, (Object) new Fence(index1, 1, false));
            Game1.playSound("axe");
            return true;
          case 323:
            if (location.objects.ContainsKey(index1))
              return false;
            location.objects.Add(index1, (Object) new Fence(index1, 2, false));
            Game1.playSound("stoneStep");
            return true;
          case 324:
            if (location.objects.ContainsKey(index1))
              return false;
            location.objects.Add(index1, (Object) new Fence(index1, 3, false));
            Game1.playSound("hammer");
            return true;
          case 325:
            if (location.objects.ContainsKey(index1))
              return false;
            location.objects.Add(index1, (Object) new Fence(index1, 4, true));
            Game1.playSound("axe");
            return true;
          case 328:
            if (location.terrainFeatures.ContainsKey(index1))
              return false;
            location.terrainFeatures.Add(index1, (TerrainFeature) new Flooring(0));
            Game1.playSound("axchop");
            return true;
          case 329:
            if (location.terrainFeatures.ContainsKey(index1))
              return false;
            location.terrainFeatures.Add(index1, (TerrainFeature) new Flooring(1));
            Game1.playSound("thudStep");
            return true;
          case 331:
            if (location.terrainFeatures.ContainsKey(index1))
              return false;
            location.terrainFeatures.Add(index1, (TerrainFeature) new Flooring(2));
            Game1.playSound("axchop");
            return true;
          case 333:
            if (location.terrainFeatures.ContainsKey(index1))
              return false;
            location.terrainFeatures.Add(index1, (TerrainFeature) new Flooring(3));
            Game1.playSound("thudStep");
            return true;
          case 401:
            if (location.terrainFeatures.ContainsKey(index1))
              return false;
            location.terrainFeatures.Add(index1, (TerrainFeature) new Flooring(4));
            Game1.playSound("thudStep");
            return true;
          case 286:
            foreach (TemporaryAnimatedSprite temporarySprite in Game1.currentLocation.temporarySprites)
            {
              if (temporarySprite.position.Equals(index1 * (float) Game1.tileSize))
                return false;
            }
            int num1 = Game1.random.Next();
            Game1.playSound("thudStep");
            List<TemporaryAnimatedSprite> temporarySprites1 = Game1.currentLocation.TemporarySprites;
            TemporaryAnimatedSprite temporaryAnimatedSprite1 = new TemporaryAnimatedSprite(this.parentSheetIndex, 100f, 1, 24, index1 * (float) Game1.tileSize, true, false, Game1.currentLocation, who);
            temporaryAnimatedSprite1.shakeIntensity = 0.5f;
            temporaryAnimatedSprite1.shakeIntensityChange = 1f / 500f;
            temporaryAnimatedSprite1.extraInfoForEndBehavior = num1;
            TemporaryAnimatedSprite.endBehavior endBehavior1 = new TemporaryAnimatedSprite.endBehavior(Game1.currentLocation.removeTemporarySpritesWithID);
            temporaryAnimatedSprite1.endFunction = endBehavior1;
            temporarySprites1.Add(temporaryAnimatedSprite1);
            Game1.currentLocation.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(598, 1279, 3, 4), 53f, 5, 9, index1 * (float) Game1.tileSize + new Vector2(5f, 3f) * (float) Game1.pixelZoom, true, false, (float) (y + 7) / 10000f, 0.0f, Color.Yellow, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false)
            {
              id = (float) num1
            });
            List<TemporaryAnimatedSprite> temporarySprites2 = Game1.currentLocation.TemporarySprites;
            TemporaryAnimatedSprite temporaryAnimatedSprite2 = new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(598, 1279, 3, 4), 53f, 5, 9, index1 * (float) Game1.tileSize + new Vector2(5f, 3f) * (float) Game1.pixelZoom, true, true, (float) (y + 7) / 10000f, 0.0f, Color.Orange, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false);
            temporaryAnimatedSprite2.delayBeforeAnimationStart = 100;
            double num2 = (double) num1;
            temporaryAnimatedSprite2.id = (float) num2;
            temporarySprites2.Add(temporaryAnimatedSprite2);
            List<TemporaryAnimatedSprite> temporarySprites3 = Game1.currentLocation.TemporarySprites;
            TemporaryAnimatedSprite temporaryAnimatedSprite3 = new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(598, 1279, 3, 4), 53f, 5, 9, index1 * (float) Game1.tileSize + new Vector2(5f, 3f) * (float) Game1.pixelZoom, true, false, (float) (y + 7) / 10000f, 0.0f, Color.White, (float) Game1.pixelZoom * 0.75f, 0.0f, 0.0f, 0.0f, false);
            temporaryAnimatedSprite3.delayBeforeAnimationStart = 200;
            double num3 = (double) num1;
            temporaryAnimatedSprite3.id = (float) num3;
            temporarySprites3.Add(temporaryAnimatedSprite3);
            if (Game1.fuseSound != null && !Game1.fuseSound.IsPlaying)
            {
              Game1.fuseSound = Game1.soundBank.GetCue("fuse");
              Game1.fuseSound.Play();
            }
            return true;
          case 287:
            foreach (TemporaryAnimatedSprite temporarySprite in Game1.currentLocation.temporarySprites)
            {
              if (temporarySprite.position.Equals(index1 * (float) Game1.tileSize))
                return false;
            }
            int num4 = Game1.random.Next();
            Game1.playSound("thudStep");
            List<TemporaryAnimatedSprite> temporarySprites4 = Game1.currentLocation.TemporarySprites;
            TemporaryAnimatedSprite temporaryAnimatedSprite4 = new TemporaryAnimatedSprite(this.parentSheetIndex, 100f, 1, 24, index1 * (float) Game1.tileSize, true, false, Game1.currentLocation, who);
            temporaryAnimatedSprite4.shakeIntensity = 0.5f;
            temporaryAnimatedSprite4.shakeIntensityChange = 1f / 500f;
            temporaryAnimatedSprite4.extraInfoForEndBehavior = num4;
            TemporaryAnimatedSprite.endBehavior endBehavior2 = new TemporaryAnimatedSprite.endBehavior(Game1.currentLocation.removeTemporarySpritesWithID);
            temporaryAnimatedSprite4.endFunction = endBehavior2;
            temporarySprites4.Add(temporaryAnimatedSprite4);
            Game1.currentLocation.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(598, 1279, 3, 4), 53f, 5, 9, index1 * (float) Game1.tileSize, true, false, (float) (y + 7) / 10000f, 0.0f, Color.Yellow, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false)
            {
              id = (float) num4
            });
            List<TemporaryAnimatedSprite> temporarySprites5 = Game1.currentLocation.TemporarySprites;
            TemporaryAnimatedSprite temporaryAnimatedSprite5 = new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(598, 1279, 3, 4), 53f, 5, 9, index1 * (float) Game1.tileSize, true, false, (float) (y + 7) / 10000f, 0.0f, Color.Orange, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false);
            temporaryAnimatedSprite5.delayBeforeAnimationStart = 100;
            double num5 = (double) num4;
            temporaryAnimatedSprite5.id = (float) num5;
            temporarySprites5.Add(temporaryAnimatedSprite5);
            List<TemporaryAnimatedSprite> temporarySprites6 = Game1.currentLocation.TemporarySprites;
            TemporaryAnimatedSprite temporaryAnimatedSprite6 = new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(598, 1279, 3, 4), 53f, 5, 9, index1 * (float) Game1.tileSize, true, false, (float) (y + 7) / 10000f, 0.0f, Color.White, (float) Game1.pixelZoom * 0.75f, 0.0f, 0.0f, 0.0f, false);
            temporaryAnimatedSprite6.delayBeforeAnimationStart = 200;
            double num6 = (double) num4;
            temporaryAnimatedSprite6.id = (float) num6;
            temporarySprites6.Add(temporaryAnimatedSprite6);
            if (Game1.fuseSound != null && !Game1.fuseSound.IsPlaying)
            {
              Game1.fuseSound = Game1.soundBank.GetCue("fuse");
              Game1.fuseSound.Play();
            }
            return true;
          case 288:
            foreach (TemporaryAnimatedSprite temporarySprite in Game1.currentLocation.temporarySprites)
            {
              if (temporarySprite.position.Equals(index1 * (float) Game1.tileSize))
                return false;
            }
            int num7 = Game1.random.Next();
            Game1.playSound("thudStep");
            List<TemporaryAnimatedSprite> temporarySprites7 = Game1.currentLocation.TemporarySprites;
            TemporaryAnimatedSprite temporaryAnimatedSprite7 = new TemporaryAnimatedSprite(this.parentSheetIndex, 100f, 1, 24, index1 * (float) Game1.tileSize, true, false, Game1.currentLocation, who);
            temporaryAnimatedSprite7.shakeIntensity = 0.5f;
            temporaryAnimatedSprite7.shakeIntensityChange = 1f / 500f;
            temporaryAnimatedSprite7.extraInfoForEndBehavior = num7;
            TemporaryAnimatedSprite.endBehavior endBehavior3 = new TemporaryAnimatedSprite.endBehavior(Game1.currentLocation.removeTemporarySpritesWithID);
            temporaryAnimatedSprite7.endFunction = endBehavior3;
            temporarySprites7.Add(temporaryAnimatedSprite7);
            Game1.currentLocation.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(598, 1279, 3, 4), 53f, 5, 9, index1 * (float) Game1.tileSize + new Vector2(5f, 0.0f) * (float) Game1.pixelZoom, true, false, (float) (y + 7) / 10000f, 0.0f, Color.Yellow, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false)
            {
              id = (float) num7
            });
            List<TemporaryAnimatedSprite> temporarySprites8 = Game1.currentLocation.TemporarySprites;
            TemporaryAnimatedSprite temporaryAnimatedSprite8 = new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(598, 1279, 3, 4), 53f, 5, 9, index1 * (float) Game1.tileSize + new Vector2(5f, 0.0f) * (float) Game1.pixelZoom, true, true, (float) (y + 7) / 10000f, 0.0f, Color.Orange, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false);
            temporaryAnimatedSprite8.delayBeforeAnimationStart = 100;
            double num8 = (double) num7;
            temporaryAnimatedSprite8.id = (float) num8;
            temporarySprites8.Add(temporaryAnimatedSprite8);
            List<TemporaryAnimatedSprite> temporarySprites9 = Game1.currentLocation.TemporarySprites;
            TemporaryAnimatedSprite temporaryAnimatedSprite9 = new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(598, 1279, 3, 4), 53f, 5, 9, index1 * (float) Game1.tileSize + new Vector2(5f, 0.0f) * (float) Game1.pixelZoom, true, false, (float) (y + 7) / 10000f, 0.0f, Color.White, (float) Game1.pixelZoom * 0.75f, 0.0f, 0.0f, 0.0f, false);
            temporaryAnimatedSprite9.delayBeforeAnimationStart = 200;
            double num9 = (double) num7;
            temporaryAnimatedSprite9.id = (float) num9;
            temporarySprites9.Add(temporaryAnimatedSprite9);
            if (Game1.fuseSound != null && !Game1.fuseSound.IsPlaying)
            {
              Game1.fuseSound = Game1.soundBank.GetCue("fuse");
              Game1.fuseSound.Play();
            }
            return true;
          case 297:
            if (location.objects.ContainsKey(index1) || location.terrainFeatures.ContainsKey(index1))
              return false;
            location.terrainFeatures.Add(index1, (TerrainFeature) new Grass(1, 4));
            Game1.playSound("dirtyHit");
            return true;
          case 298:
            if (location.objects.ContainsKey(index1))
              return false;
            location.objects.Add(index1, (Object) new Fence(index1, 5, false));
            Game1.playSound("axe");
            return true;
          case 93:
            if (location.objects.ContainsKey(index1))
              return false;
            Utility.removeLightSource((int) ((double) this.tileLocation.X * 2000.0 + (double) this.tileLocation.Y));
            Utility.removeLightSource((int) Game1.player.uniqueMultiplayerID);
            new Torch(index1, 1).placementAction(location, x, y, who == null ? Game1.player : who);
            return true;
          case 94:
            if (location.objects.ContainsKey(index1))
              return false;
            new Torch(index1, 1, 94).placementAction(location, x, y, who);
            return true;
        }
      }
      else
      {
        switch (this.ParentSheetIndex)
        {
          case 143:
          case 144:
          case 145:
          case 146:
          case 147:
          case 148:
          case 149:
          case 150:
          case 151:
            if (location.objects.ContainsKey(index1))
              return false;
            Torch torch = new Torch(index1, this.parentSheetIndex, true);
            int num10 = 25;
            torch.shakeTimer = num10;
            GameLocation location1 = location;
            int x1 = x;
            int y1 = y;
            Farmer who1 = who;
            torch.placementAction(location1, x1, y1, who1);
            return true;
          case 163:
            location.objects.Add(index1, (Object) new Cask(index1));
            Game1.playSound("hammer");
            break;
          case 71:
            if (location is MineShaft)
            {
              if ((location as MineShaft).mineLevel != 120 && (location as MineShaft).recursiveTryToCreateLadderDown(index1, "hoeHit", 16))
                return true;
              Game1.showRedMessage(Game1.content.LoadString("Strings\\StringsFromCSFiles:Object.cs.13053"));
            }
            return false;
          case 130:
            if (location.objects.ContainsKey(index1) || Game1.currentLocation is MineShaft)
            {
              Game1.showRedMessage(Game1.content.LoadString("Strings\\StringsFromCSFiles:Object.cs.13053"));
              return false;
            }
            SerializableDictionary<Vector2, Object> objects = location.objects;
            Vector2 key1 = index1;
            Chest chest = new Chest(true);
            int num11 = 50;
            chest.shakeTimer = num11;
            objects.Add(key1, (Object) chest);
            Game1.playSound("axe");
            return true;
        }
      }
      if (this.category == -19)
        return false;
      if (this.name.Equals("Tapper"))
      {
        if (!location.terrainFeatures.ContainsKey(index1) || !(location.terrainFeatures[index1] is Tree) || ((location.terrainFeatures[index1] as Tree).growthStage < 5 || (location.terrainFeatures[index1] as Tree).stump) || location.objects.ContainsKey(index1))
          return false;
        this.tileLocation = index1;
        location.objects.Add(index1, this);
        int treeType = (location.terrainFeatures[index1] as Tree).treeType;
        (location.terrainFeatures[index1] as Tree).tapped = true;
        switch (treeType)
        {
          case 1:
            this.heldObject = new Object(725, 1, false, -1, 0);
            this.minutesUntilReady = 13000 - Game1.timeOfDay;
            break;
          case 2:
            this.heldObject = new Object(724, 1, false, -1, 0);
            this.minutesUntilReady = 16000 - Game1.timeOfDay;
            break;
          case 3:
            this.heldObject = new Object(726, 1, false, -1, 0);
            this.minutesUntilReady = 10000 - Game1.timeOfDay;
            break;
          case 7:
            this.heldObject = new Object(420, 1, false, -1, 0);
            this.minutesUntilReady = 3000 - Game1.timeOfDay;
            if (!Game1.currentSeason.Equals("fall"))
            {
              this.heldObject = new Object(404, 1, false, -1, 0);
              this.minutesUntilReady = 6000 - Game1.timeOfDay;
              break;
            }
            break;
        }
        Game1.playSound("axe");
        return true;
      }
      if (this.name.Contains("Sapling"))
      {
        Vector2 key2 = new Vector2();
        for (int index2 = x / Game1.tileSize - 2; index2 <= x / Game1.tileSize + 2; ++index2)
        {
          for (int index3 = y / Game1.tileSize - 2; index3 <= y / Game1.tileSize + 2; ++index3)
          {
            key2.X = (float) index2;
            key2.Y = (float) index3;
            if (location.terrainFeatures.ContainsKey(key2) && (location.terrainFeatures[key2] is Tree || location.terrainFeatures[key2] is FruitTree))
            {
              Game1.showRedMessage(Game1.content.LoadString("Strings\\StringsFromCSFiles:Object.cs.13060"));
              return false;
            }
          }
        }
        if (location.terrainFeatures.ContainsKey(index1))
        {
          if (!(location.terrainFeatures[index1] is HoeDirt) || (location.terrainFeatures[index1] as HoeDirt).crop != null)
            return false;
          location.terrainFeatures.Remove(index1);
        }
        if (location is Farm && (location.doesTileHaveProperty((int) index1.X, (int) index1.Y, "Diggable", "Back") != null || location.doesTileHavePropertyNoNull((int) index1.X, (int) index1.Y, "Type", "Back").Equals("Grass")) && !location.doesTileHavePropertyNoNull((int) index1.X, (int) index1.Y, "NoSpawn", "Back").Equals("Tree") || location.name.Equals("Greenhouse") && (location.doesTileHaveProperty((int) index1.X, (int) index1.Y, "Diggable", "Back") != null || location.doesTileHavePropertyNoNull((int) index1.X, (int) index1.Y, "Type", "Back").Equals("Stone")))
        {
          Game1.playSound("dirtyHit");
          DelayedAction.playSoundAfterDelay("coin", 100);
          SerializableDictionary<Vector2, TerrainFeature> terrainFeatures = location.terrainFeatures;
          Vector2 key3 = index1;
          FruitTree fruitTree = new FruitTree(this.parentSheetIndex);
          int num12 = location.name.Equals("Greenhouse") ? 1 : 0;
          fruitTree.greenHouseTree = num12 != 0;
          int num13 = location.doesTileHavePropertyNoNull((int) index1.X, (int) index1.Y, "Type", "Back").Equals("Stone") ? 1 : 0;
          fruitTree.greenHouseTileTree = num13 != 0;
          terrainFeatures.Add(key3, (TerrainFeature) fruitTree);
          return true;
        }
        Game1.showRedMessage(Game1.content.LoadString("Strings\\StringsFromCSFiles:Object.cs.13068"));
        return false;
      }
      if (this.category == -74)
        return true;
      if (!this.performDropDownAction(who))
      {
        Object one = (Object) this.getOne();
        one.shakeTimer = 50;
        one.tileLocation = index1;
        one.performDropDownAction(who);
        if (location.objects.ContainsKey(index1))
        {
          if (location.objects[index1].ParentSheetIndex != this.parentSheetIndex)
          {
            Game1.createItemDebris((Item) location.objects[index1], index1 * (float) Game1.tileSize, Game1.random.Next(4), (GameLocation) null);
            location.objects[index1] = one;
          }
        }
        else if (one is Furniture)
          (location as DecoratableLocation).furniture.Add(this as Furniture);
        else
          location.objects.Add(index1, one);
        one.initializeLightSource(index1);
      }
      Game1.playSound("woodyStep");
      return true;
    }

    public override bool actionWhenPurchased()
    {
      if (this.type != null && this.type.Contains("Blueprint"))
      {
        string str = this.name.Substring(this.name.IndexOf(' ') + 1);
        if (!Game1.player.blueprints.Contains(this.name))
          Game1.player.blueprints.Add(str);
        return true;
      }
      if (this.parentSheetIndex == 434)
      {
        if (!Game1.isFestival())
          Game1.player.mailReceived.Add("CF_Sewer");
        else
          Game1.player.mailReceived.Add("CF_Fair");
        Game1.exitActiveMenu();
        Game1.playerEatObject(this, true);
      }
      return this.isRecipe;
    }

    public override bool canBePlacedInWater()
    {
      return this.parentSheetIndex == 710;
    }

    public override string getDescription()
    {
      if (this.isRecipe)
      {
        if (this.category == -7)
          return Game1.content.LoadString("Strings\\StringsFromCSFiles:Object.cs.13073", (object) this.loadDisplayName());
        return Game1.content.LoadString("Strings\\StringsFromCSFiles:Object.cs.13074", (object) this.loadDisplayName());
      }
      if (!this.bigCraftable && this.type != null && (this.type.Equals("Minerals") || this.type.Equals("Arch")) && !(Game1.getLocationFromName("ArchaeologyHouse") as LibraryMuseum).museumAlreadyHasArtifact(this.parentSheetIndex))
        return Game1.parseText(Game1.content.LoadString("Strings\\StringsFromCSFiles:Object.cs.13078"), Game1.smallFont, Game1.tileSize * 4 + Game1.tileSize / 4);
      string text;
      if (!this.bigCraftable)
      {
        if (!Game1.objectInformation.ContainsKey(this.parentSheetIndex))
          text = "???";
        else
          text = Game1.objectInformation[this.parentSheetIndex].Split('/')[5];
      }
      else
        text = Game1.bigCraftablesInformation[this.parentSheetIndex].Split('/')[4];
      SpriteFont smallFont = Game1.smallFont;
      int width = Math.Max(Game1.tileSize * 4 + Game1.tileSize / 4, (int) Game1.dialogueFont.MeasureString(this.DisplayName).X);
      return Game1.parseText(text, smallFont, width);
    }

    public int sellToStorePrice()
    {
      if (this is Fence)
        return this.price;
      if (this.category == -22)
        return (int) ((double) this.price * (1.0 + (double) this.quality * 0.25) * (double) this.scale.Y);
      float num = (float) (int) ((double) this.price * (1.0 + (double) this.quality * 0.25));
      bool flag = false;
      if (this.name.ToLower().Contains("mayonnaise") || this.name.ToLower().Contains("cheese") || (this.name.ToLower().Contains("cloth") || this.name.ToLower().Contains("wool")))
        flag = true;
      if (Game1.player.professions.Contains(0) && (flag || this.category == -5 || (this.category == -6 || this.category == -18)))
        num *= 1.2f;
      if (Game1.player.professions.Contains(1) && (this.category == -75 || this.category == -80 || this.category == -79 && !this.isSpawnedObject))
        num *= 1.1f;
      if (Game1.player.professions.Contains(4) && this.category == -26)
        num *= 1.4f;
      if (Game1.player.professions.Contains(6) && this.category == -4)
        num *= Game1.player.professions.Contains(8) ? 1.5f : 1.25f;
      if (Game1.player.professions.Contains(12) && (this.parentSheetIndex == 388 || this.parentSheetIndex == 709))
        num *= 1.5f;
      if (Game1.player.professions.Contains(15) && this.category == -27)
        num *= 1.25f;
      if (Game1.player.professions.Contains(20) && this.parentSheetIndex >= 334 && this.parentSheetIndex <= 337)
        num *= 1.5f;
      if (Game1.player.professions.Contains(23) && (this.category == -2 || this.category == -12))
        num *= 1.3f;
      if (this.parentSheetIndex == 493)
        num /= 2f;
      return (int) num;
    }

    public override int salePrice()
    {
      if (this is Fence)
        return this.price;
      if (this.isRecipe)
        return this.price * 10;
      switch (this.parentSheetIndex)
      {
        case 378:
          return 50;
        case 380:
          return 100;
        case 382:
          return 100;
        case 384:
          return 300;
        case 388:
          return 10;
        case 390:
          return 20;
        default:
          return (int) ((double) (this.price * 2) * (1.0 + (double) this.quality * 0.25));
      }
    }

    public enum PreserveType
    {
      Wine,
      Jelly,
      Pickle,
      Juice,
    }

    public enum HoneyType
    {
      Wild = -1,
      Poppy = 376,
      Tulip = 591,
      SummerSpangle = 593,
      FairyRose = 595,
      BlueJazz = 597,
    }
  }
}
