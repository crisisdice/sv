// Decompiled with JetBrains decompiler
// Type: StardewValley.Objects.Furniture
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.Locations;
using StardewValley.Menus;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace StardewValley.Objects
{
  public class Furniture : StardewValley.Object
  {
    public const int chair = 0;
    public const int bench = 1;
    public const int couch = 2;
    public const int armchair = 3;
    public const int dresser = 4;
    public const int longTable = 5;
    public const int painting = 6;
    public const int lamp = 7;
    public const int decor = 8;
    public const int other = 9;
    public const int bookcase = 10;
    public const int table = 11;
    public const int rug = 12;
    public const int window = 13;
    public new int price;
    public int furniture_type;
    public int rotations;
    public int currentRotation;
    public new bool flipped;
    private int sourceIndexOffset;
    protected Vector2 drawPosition;
    public Rectangle sourceRect;
    public Rectangle defaultSourceRect;
    public Rectangle defaultBoundingBox;
    public static Texture2D furnitureTexture;
    public bool drawHeldObjectLow;
    [XmlIgnore]
    public bool flaggedForPickUp;
    [XmlIgnore]
    private string _description;
    private bool lightGlowAdded;

    [XmlIgnore]
    public string description
    {
      get
      {
        if (this._description == null)
          this._description = this.loadDescription();
        return this._description;
      }
    }

    public Furniture()
    {
      this.updateDrawPosition();
    }

    public Furniture(int which, Vector2 tile, int initialRotations)
      : this(which, tile)
    {
      for (int index = 0; index < initialRotations; ++index)
        this.rotate();
    }

    public Furniture(int which, Vector2 tile)
    {
      this.tileLocation = tile;
      this.parentSheetIndex = which;
      string[] data = this.getData();
      this.name = data[0];
      this.furniture_type = this.getTypeNumberFromName(data[1]);
      this.defaultSourceRect = new Rectangle(which * 16 % Furniture.furnitureTexture.Width, which * 16 / Furniture.furnitureTexture.Width * 16, 1, 1);
      this.drawHeldObjectLow = this.name.ToLower().Contains("tea");
      if (data[2].Equals("-1"))
      {
        this.sourceRect = this.getDefaultSourceRectForType(which, this.furniture_type);
        this.defaultSourceRect = this.sourceRect;
      }
      else
      {
        this.defaultSourceRect.Width = Convert.ToInt32(data[2].Split(' ')[0]);
        this.defaultSourceRect.Height = Convert.ToInt32(data[2].Split(' ')[1]);
        this.sourceRect = new Rectangle(which * 16 % Furniture.furnitureTexture.Width, which * 16 / Furniture.furnitureTexture.Width * 16, this.defaultSourceRect.Width * 16, this.defaultSourceRect.Height * 16);
        this.defaultSourceRect = this.sourceRect;
      }
      this.defaultBoundingBox = new Rectangle((int) this.tileLocation.X, (int) this.tileLocation.Y, 1, 1);
      if (data[3].Equals("-1"))
      {
        this.boundingBox = this.getDefaultBoundingBoxForType(this.furniture_type);
        this.defaultBoundingBox = this.boundingBox;
      }
      else
      {
        this.defaultBoundingBox.Width = Convert.ToInt32(data[3].Split(' ')[0]);
        this.defaultBoundingBox.Height = Convert.ToInt32(data[3].Split(' ')[1]);
        this.boundingBox = new Rectangle((int) this.tileLocation.X * Game1.tileSize, (int) this.tileLocation.Y * Game1.tileSize, this.defaultBoundingBox.Width * Game1.tileSize, this.defaultBoundingBox.Height * Game1.tileSize);
        this.defaultBoundingBox = this.boundingBox;
      }
      this.updateDrawPosition();
      this.rotations = Convert.ToInt32(data[4]);
      this.price = Convert.ToInt32(data[5]);
    }

    private string[] getData()
    {
      Dictionary<int, string> dictionary = Game1.content.Load<Dictionary<int, string>>("Data\\Furniture");
      if (!dictionary.ContainsKey(this.parentSheetIndex))
        dictionary = Game1.content.LoadBase<Dictionary<int, string>>("Data\\Furniture");
      return dictionary[this.parentSheetIndex].Split('/');
    }

    protected override string loadDisplayName()
    {
      if (LocalizedContentManager.CurrentLanguageCode == LocalizedContentManager.LanguageCode.en)
        return this.name;
      string[] data = this.getData();
      int index = data.Length - 1;
      return data[index];
    }

    protected string loadDescription()
    {
      if (this.parentSheetIndex == 1308)
        return Game1.parseText(Game1.content.LoadString("Strings\\Objects:CatalogueDescription"), Game1.smallFont, Game1.tileSize * 5);
      if (this.parentSheetIndex == 1226)
        return Game1.parseText(Game1.content.LoadString("Strings\\Objects:FurnitureCatalogueDescription"), Game1.smallFont, Game1.tileSize * 5);
      return Game1.content.LoadString("Strings\\StringsFromCSFiles:Furniture.cs.12623");
    }

    public override string getDescription()
    {
      return this.description;
    }

    public override bool performDropDownAction(Farmer who)
    {
      this.resetOnPlayerEntry(who == null ? Game1.currentLocation : who.currentLocation);
      return false;
    }

    public override void hoverAction()
    {
      base.hoverAction();
      if (Game1.player.isInventoryFull())
        return;
      Game1.mouseCursor = 2;
    }

    public override bool checkForAction(Farmer who, bool justCheckingForActivity = false)
    {
      if (justCheckingForActivity)
        return true;
      if (this.parentSheetIndex == 1402)
        Game1.activeClickableMenu = (IClickableMenu) new Billboard(false);
      else if (this.parentSheetIndex == 1308)
        Game1.activeClickableMenu = (IClickableMenu) new ShopMenu(Utility.getAllWallpapersAndFloorsForFree(), 0, (string) null);
      else if (this.parentSheetIndex == 1226)
        Game1.activeClickableMenu = (IClickableMenu) new ShopMenu(Utility.getAllFurnituresForFree(), 0, (string) null);
      return this.clicked(who);
    }

    public override bool clicked(Farmer who)
    {
      Game1.haltAfterCheck = false;
      if (this.furniture_type == 11 && who.ActiveObject != null && (who.ActiveObject != null && this.heldObject == null))
        return false;
      if (this.heldObject == null && (who.ActiveObject == null || !(who.ActiveObject is Furniture)))
      {
        this.flaggedForPickUp = true;
        return true;
      }
      if (this.heldObject == null || !who.addItemToInventoryBool((Item) this.heldObject, false))
        return false;
      this.heldObject.performRemoveAction(this.tileLocation, who.currentLocation);
      this.heldObject = (StardewValley.Object) null;
      Game1.playSound("coin");
      return true;
    }

    public override void DayUpdate(GameLocation location)
    {
      base.DayUpdate(location);
      this.lightGlowAdded = false;
      if (!Game1.isDarkOut() || Game1.newDay && !Game1.isRaining)
        this.removeLights(location);
      else
        this.addLights(location);
    }

    public void resetOnPlayerEntry(GameLocation environment)
    {
      this.removeLights(environment);
      if (!Game1.isDarkOut())
        return;
      this.addLights(environment);
    }

    public override bool performObjectDropInAction(StardewValley.Object dropIn, bool probe, Farmer who)
    {
      if (this.furniture_type != 11 && this.furniture_type != 5 || (this.heldObject != null || dropIn.bigCraftable) || dropIn is Furniture && ((dropIn as Furniture).getTilesWide() != 1 || (dropIn as Furniture).getTilesHigh() != 1))
        return false;
      this.heldObject = (StardewValley.Object) dropIn.getOne();
      this.heldObject.tileLocation = this.tileLocation;
      this.heldObject.boundingBox.X = this.boundingBox.X;
      this.heldObject.boundingBox.Y = this.boundingBox.Y;
      this.heldObject.performDropDownAction(who);
      if (!probe)
      {
        Game1.playSound("woodyStep");
        if (who != null)
          who.reduceActiveItemByOne();
      }
      return true;
    }

    private void addLights(GameLocation environment)
    {
      if (this.furniture_type == 7)
      {
        if (this.sourceIndexOffset == 0)
        {
          this.sourceRect = this.defaultSourceRect;
          this.sourceRect.X += this.sourceRect.Width;
        }
        this.sourceIndexOffset = 1;
        if (this.lightSource != null)
          return;
        Utility.removeLightSource((int) ((double) this.tileLocation.X * 2000.0 + (double) this.tileLocation.Y));
        this.lightSource = new LightSource(4, new Vector2((float) (this.boundingBox.X + Game1.tileSize / 2), (float) (this.boundingBox.Y - Game1.tileSize)), 2f, Color.Black, (int) ((double) this.tileLocation.X * 2000.0 + (double) this.tileLocation.Y));
        Game1.currentLightSources.Add(this.lightSource);
      }
      else
      {
        if (this.furniture_type != 13)
          return;
        if (this.sourceIndexOffset == 0)
        {
          this.sourceRect = this.defaultSourceRect;
          this.sourceRect.X += this.sourceRect.Width;
        }
        this.sourceIndexOffset = 1;
        if (!this.lightGlowAdded)
          return;
        environment.lightGlows.Remove(new Vector2((float) (this.boundingBox.X + Game1.tileSize / 2), (float) (this.boundingBox.Y + Game1.tileSize)));
        this.lightGlowAdded = false;
      }
    }

    private void removeLights(GameLocation environment)
    {
      if (this.furniture_type == 7)
      {
        if (this.sourceIndexOffset == 1)
          this.sourceRect = this.defaultSourceRect;
        this.sourceIndexOffset = 0;
        Utility.removeLightSource((int) ((double) this.tileLocation.X * 2000.0 + (double) this.tileLocation.Y));
        this.lightSource = (LightSource) null;
      }
      else
      {
        if (this.furniture_type != 13)
          return;
        if (this.sourceIndexOffset == 1)
          this.sourceRect = this.defaultSourceRect;
        this.sourceIndexOffset = 0;
        if (Game1.isRaining)
        {
          this.sourceRect = this.defaultSourceRect;
          this.sourceRect.X += this.sourceRect.Width;
          this.sourceIndexOffset = 1;
        }
        else
        {
          if (!this.lightGlowAdded && !environment.lightGlows.Contains(new Vector2((float) (this.boundingBox.X + Game1.tileSize / 2), (float) (this.boundingBox.Y + Game1.tileSize))))
            environment.lightGlows.Add(new Vector2((float) (this.boundingBox.X + Game1.tileSize / 2), (float) (this.boundingBox.Y + Game1.tileSize)));
          this.lightGlowAdded = true;
        }
      }
    }

    public override bool minutesElapsed(int minutes, GameLocation environment)
    {
      if (Game1.isDarkOut())
        this.addLights(environment);
      else
        this.removeLights(environment);
      return false;
    }

    public override void performRemoveAction(Vector2 tileLocation, GameLocation environment)
    {
      this.removeLights(environment);
      if (this.furniture_type == 13 && this.lightGlowAdded)
      {
        environment.lightGlows.Remove(new Vector2((float) (this.boundingBox.X + Game1.tileSize / 2), (float) (this.boundingBox.Y + Game1.tileSize)));
        this.lightGlowAdded = false;
      }
      base.performRemoveAction(tileLocation, environment);
    }

    public void rotate()
    {
      if (this.rotations < 2)
        return;
      int currentRotation = this.currentRotation;
      this.currentRotation = this.currentRotation + (this.rotations == 4 ? 1 : 2);
      this.currentRotation = this.currentRotation % 4;
      this.flipped = false;
      Point point1 = new Point();
      switch (this.furniture_type)
      {
        case 2:
          point1.Y = 1;
          point1.X = -1;
          break;
        case 3:
          point1.X = -1;
          point1.Y = 1;
          break;
        case 5:
          point1.Y = 0;
          point1.X = -1;
          break;
        case 12:
          point1.X = 0;
          point1.Y = 0;
          break;
      }
      bool flag1 = this.furniture_type == 5 || this.furniture_type == 12 || this.parentSheetIndex == 724 || this.parentSheetIndex == 727;
      bool flag2 = this.defaultBoundingBox.Width != this.defaultBoundingBox.Height;
      if (flag1 && this.currentRotation == 2)
        this.currentRotation = 1;
      if (flag2)
      {
        int height = this.boundingBox.Height;
        switch (this.currentRotation)
        {
          case 0:
          case 2:
            this.boundingBox.Height = this.defaultBoundingBox.Height;
            this.boundingBox.Width = this.defaultBoundingBox.Width;
            break;
          case 1:
          case 3:
            this.boundingBox.Height = this.boundingBox.Width + point1.X * Game1.tileSize;
            this.boundingBox.Width = height + point1.Y * Game1.tileSize;
            break;
        }
      }
      Point point2 = new Point();
      if (this.furniture_type == 12)
      {
        point2.X = 1;
        point2.Y = -1;
      }
      if (flag2)
      {
        switch (this.currentRotation)
        {
          case 0:
            this.sourceRect = this.defaultSourceRect;
            break;
          case 1:
            this.sourceRect = new Rectangle(this.defaultSourceRect.X + this.defaultSourceRect.Width, this.defaultSourceRect.Y, this.defaultSourceRect.Height - 16 + point1.Y * 16 + point2.X * 16, this.defaultSourceRect.Width + 16 + point1.X * 16 + point2.Y * 16);
            break;
          case 2:
            this.sourceRect = new Rectangle(this.defaultSourceRect.X + this.defaultSourceRect.Width + this.defaultSourceRect.Height - 16 + point1.Y * 16 + point2.X * 16, this.defaultSourceRect.Y, this.defaultSourceRect.Width, this.defaultSourceRect.Height);
            break;
          case 3:
            this.sourceRect = new Rectangle(this.defaultSourceRect.X + this.defaultSourceRect.Width, this.defaultSourceRect.Y, this.defaultSourceRect.Height - 16 + point1.Y * 16 + point2.X * 16, this.defaultSourceRect.Width + 16 + point1.X * 16 + point2.Y * 16);
            this.flipped = true;
            break;
        }
      }
      else
      {
        this.flipped = this.currentRotation == 3;
        this.sourceRect = this.rotations != 2 ? new Rectangle(this.defaultSourceRect.X + (this.currentRotation == 3 ? 1 : this.currentRotation) * this.defaultSourceRect.Width, this.defaultSourceRect.Y, this.defaultSourceRect.Width, this.defaultSourceRect.Height) : new Rectangle(this.defaultSourceRect.X + (this.currentRotation == 2 ? 1 : 0) * this.defaultSourceRect.Width, this.defaultSourceRect.Y, this.defaultSourceRect.Width, this.defaultSourceRect.Height);
      }
      if (flag1 && this.currentRotation == 1)
        this.currentRotation = 2;
      this.updateDrawPosition();
    }

    public bool isGroundFurniture()
    {
      if (this.furniture_type != 13 && this.furniture_type != 6)
        return this.furniture_type != 13;
      return false;
    }

    public override bool canBeGivenAsGift()
    {
      return false;
    }

    public override bool canBePlacedHere(GameLocation l, Vector2 tile)
    {
      if (!(l is DecoratableLocation))
        return false;
      for (int index1 = 0; index1 < this.boundingBox.Width / Game1.tileSize; ++index1)
      {
        for (int index2 = 0; index2 < this.boundingBox.Height / Game1.tileSize; ++index2)
        {
          Vector2 vector2 = tile * (float) Game1.tileSize + new Vector2((float) index1, (float) index2) * (float) Game1.tileSize;
          vector2.X += (float) (Game1.tileSize / 2);
          vector2.Y += (float) (Game1.tileSize / 2);
          foreach (Furniture furniture in (l as DecoratableLocation).furniture)
          {
            if (furniture.furniture_type == 11 && (furniture.getBoundingBox(furniture.tileLocation).Contains((int) vector2.X, (int) vector2.Y) && furniture.heldObject == null && this.getTilesWide() == 1))
              return true;
            if ((furniture.furniture_type != 12 || this.furniture_type == 12) && furniture.getBoundingBox(furniture.tileLocation).Contains((int) vector2.X, (int) vector2.Y))
              return false;
          }
          Vector2 key = tile + new Vector2((float) index1, (float) index2);
          if (l.Objects.ContainsKey(key))
            return false;
        }
      }
      return base.canBePlacedHere(l, tile);
    }

    public void updateDrawPosition()
    {
      this.drawPosition = new Vector2((float) this.boundingBox.X, (float) (this.boundingBox.Y - (this.sourceRect.Height * Game1.pixelZoom - this.boundingBox.Height)));
    }

    public int getTilesWide()
    {
      return this.boundingBox.Width / Game1.tileSize;
    }

    public int getTilesHigh()
    {
      return this.boundingBox.Height / Game1.tileSize;
    }

    public override bool placementAction(GameLocation location, int x, int y, Farmer who = null)
    {
      if (location is DecoratableLocation)
      {
        Point point = new Point(x / Game1.tileSize, y / Game1.tileSize);
        List<Rectangle> rectangles = !(location is FarmHouse) ? DecoratableLocation.getWalls() : FarmHouse.getWalls((location as FarmHouse).upgradeLevel);
        this.tileLocation = new Vector2((float) point.X, (float) point.Y);
        bool flag1 = false;
        if (this.furniture_type == 6 || this.furniture_type == 13 || this.parentSheetIndex == 1293)
        {
          int num = this.parentSheetIndex == 1293 ? 3 : 0;
          bool flag2 = false;
          foreach (Rectangle rectangle in rectangles)
          {
            if ((this.furniture_type == 6 || this.furniture_type == 13 || num != 0) && (rectangle.Y + num == point.Y && rectangle.Contains(point.X, point.Y - num)))
            {
              flag2 = true;
              break;
            }
          }
          if (!flag2)
          {
            Game1.showRedMessage(Game1.content.LoadString("Strings\\StringsFromCSFiles:Furniture.cs.12629"));
            return false;
          }
          flag1 = true;
        }
        for (int x1 = point.X; x1 < point.X + this.getTilesWide(); ++x1)
        {
          for (int y1 = point.Y; y1 < point.Y + this.getTilesHigh(); ++y1)
          {
            if (location.doesTileHaveProperty(x1, y1, "NoFurniture", "Back") != null)
            {
              Game1.showRedMessage(Game1.content.LoadString("Strings\\StringsFromCSFiles:Furniture.cs.12632"));
              return false;
            }
            if (!flag1 && Utility.pointInRectangles(rectangles, x1, y1))
            {
              Game1.showRedMessage(Game1.content.LoadString("Strings\\StringsFromCSFiles:Furniture.cs.12633"));
              return false;
            }
            if (location.getTileIndexAt(x1, y1, "Buildings") != -1)
              return false;
          }
        }
        this.boundingBox = new Rectangle(x / Game1.tileSize * Game1.tileSize, y / Game1.tileSize * Game1.tileSize, this.boundingBox.Width, this.boundingBox.Height);
        foreach (Furniture furniture in (location as DecoratableLocation).furniture)
        {
          if (furniture.furniture_type == 11 && furniture.heldObject == null && furniture.getBoundingBox(furniture.tileLocation).Intersects(this.boundingBox))
          {
            furniture.performObjectDropInAction((StardewValley.Object) this, false, who == null ? Game1.player : who);
            return true;
          }
        }
        foreach (Character farmer in location.getFarmers())
        {
          if (farmer.GetBoundingBox().Intersects(this.boundingBox))
          {
            Game1.showRedMessage(Game1.content.LoadString("Strings\\StringsFromCSFiles:Furniture.cs.12636"));
            return false;
          }
        }
        this.updateDrawPosition();
        return base.placementAction(location, x, y, who);
      }
      Game1.showRedMessage(Game1.content.LoadString("Strings\\StringsFromCSFiles:Furniture.cs.12628"));
      return false;
    }

    public override bool isPlaceable()
    {
      return true;
    }

    public override Rectangle getBoundingBox(Vector2 tileLocation)
    {
      return this.boundingBox;
    }

    private Rectangle getDefaultSourceRectForType(int tileIndex, int type)
    {
      int num1;
      int num2;
      switch (type)
      {
        case 0:
          num1 = 1;
          num2 = 2;
          break;
        case 1:
          num1 = 2;
          num2 = 2;
          break;
        case 2:
          num1 = 3;
          num2 = 2;
          break;
        case 3:
          num1 = 2;
          num2 = 2;
          break;
        case 4:
          num1 = 2;
          num2 = 2;
          break;
        case 5:
          num1 = 5;
          num2 = 3;
          break;
        case 6:
          num1 = 2;
          num2 = 2;
          break;
        case 7:
          num1 = 1;
          num2 = 3;
          break;
        case 8:
          num1 = 1;
          num2 = 2;
          break;
        case 10:
          num1 = 2;
          num2 = 3;
          break;
        case 11:
          num1 = 2;
          num2 = 3;
          break;
        case 12:
          num1 = 3;
          num2 = 2;
          break;
        case 13:
          num1 = 1;
          num2 = 2;
          break;
        default:
          num1 = 1;
          num2 = 2;
          break;
      }
      return new Rectangle(tileIndex * 16 % Furniture.furnitureTexture.Width, tileIndex * 16 / Furniture.furnitureTexture.Width * 16, num1 * 16, num2 * 16);
    }

    private Rectangle getDefaultBoundingBoxForType(int type)
    {
      int num1;
      int num2;
      switch (type)
      {
        case 0:
          num1 = 1;
          num2 = 1;
          break;
        case 1:
          num1 = 2;
          num2 = 1;
          break;
        case 2:
          num1 = 3;
          num2 = 1;
          break;
        case 3:
          num1 = 2;
          num2 = 1;
          break;
        case 4:
          num1 = 2;
          num2 = 1;
          break;
        case 5:
          num1 = 5;
          num2 = 2;
          break;
        case 6:
          num1 = 2;
          num2 = 2;
          break;
        case 7:
          num1 = 1;
          num2 = 1;
          break;
        case 8:
          num1 = 1;
          num2 = 1;
          break;
        case 10:
          num1 = 2;
          num2 = 1;
          break;
        case 11:
          num1 = 2;
          num2 = 2;
          break;
        case 12:
          num1 = 3;
          num2 = 2;
          break;
        case 13:
          num1 = 1;
          num2 = 2;
          break;
        default:
          num1 = 1;
          num2 = 1;
          break;
      }
      return new Rectangle((int) this.tileLocation.X * Game1.tileSize, (int) this.tileLocation.Y * Game1.tileSize, num1 * Game1.tileSize, num2 * Game1.tileSize);
    }

    private int getTypeNumberFromName(string typeName)
    {
      string lower = typeName.ToLower();
      // ISSUE: reference to a compiler-generated method
      uint stringHash = \u003CPrivateImplementationDetails\u003E.ComputeStringHash(lower);
      if (stringHash <= 1555340682U)
      {
        if (stringHash <= 732630053U)
        {
          if ((int) stringHash != 44871939)
          {
            if ((int) stringHash != 600654789)
            {
              if ((int) stringHash == 732630053 && lower == "couch")
                return 2;
            }
            else if (lower == "rug")
              return 12;
          }
          else if (lower == "long table")
            return 5;
        }
        else if ((int) stringHash != 1049849701)
        {
          if ((int) stringHash != 1251777503)
          {
            if ((int) stringHash == 1555340682 && lower == "chair")
              return 0;
          }
          else if (lower == "table")
            return 11;
        }
        else if (lower == "painting")
          return 6;
      }
      else if (stringHash <= 2058371002U)
      {
        if ((int) stringHash != 1651424953)
        {
          if ((int) stringHash != 1810951995)
          {
            if ((int) stringHash == 2058371002 && lower == "armchair")
              return 3;
          }
          else if (lower == "lamp")
            return 7;
        }
        else if (lower == "bench")
          return 1;
      }
      else if (stringHash <= 2708649949U)
      {
        if ((int) stringHash != -2058470841)
        {
          if ((int) stringHash == -1586317347 && lower == "window")
            return 13;
        }
        else if (lower == "dresser")
          return 4;
      }
      else if ((int) stringHash != -1190063004)
      {
        if ((int) stringHash == -936519438 && lower == "decor")
          return 8;
      }
      else if (lower == "bookcase")
        return 10;
      return 9;
    }

    public override int salePrice()
    {
      return this.price;
    }

    public override int maximumStackSize()
    {
      return 1;
    }

    public override int getStack()
    {
      return this.stack;
    }

    public override int addToStack(int amount)
    {
      return 1;
    }

    public override string Name
    {
      get
      {
        return this.name;
      }
    }

    private float getScaleSize()
    {
      int num1 = this.sourceRect.Width / 16;
      int num2 = this.sourceRect.Height / 16;
      if (num1 >= 5)
        return 0.75f;
      if (num2 >= 3)
        return 1f;
      if (num1 <= 2)
        return 2f;
      return num1 <= 4 ? 1f : 0.1f;
    }

    public override void drawWhenHeld(SpriteBatch spriteBatch, Vector2 objectPosition, Farmer f)
    {
    }

    public override void drawInMenu(SpriteBatch spriteBatch, Vector2 location, float scaleSize, float transparency, float layerDepth, bool drawStackNumber)
    {
      spriteBatch.Draw(Furniture.furnitureTexture, location + new Vector2((float) (Game1.tileSize / 2), (float) (Game1.tileSize / 2)), new Rectangle?(this.defaultSourceRect), Color.White * transparency, 0.0f, new Vector2((float) (this.defaultSourceRect.Width / 2), (float) (this.defaultSourceRect.Height / 2)), 1f * this.getScaleSize() * scaleSize, SpriteEffects.None, layerDepth);
    }

    public override void draw(SpriteBatch spriteBatch, int x, int y, float alpha = 1f)
    {
      if (x == -1)
        spriteBatch.Draw(Furniture.furnitureTexture, Game1.GlobalToLocal(Game1.viewport, this.drawPosition), new Rectangle?(this.sourceRect), Color.White * alpha, 0.0f, Vector2.Zero, (float) Game1.pixelZoom, this.flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None, this.furniture_type == 12 ? 0.0f : (float) (this.boundingBox.Bottom - 8) / 10000f);
      else
        spriteBatch.Draw(Furniture.furnitureTexture, Game1.GlobalToLocal(Game1.viewport, new Vector2((float) (x * Game1.tileSize), (float) (y * Game1.tileSize - (this.sourceRect.Height * Game1.pixelZoom - this.boundingBox.Height)))), new Rectangle?(this.sourceRect), Color.White * alpha, 0.0f, Vector2.Zero, (float) Game1.pixelZoom, this.flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None, this.furniture_type == 12 ? 0.0f : (float) (this.boundingBox.Bottom - 8) / 10000f);
      if (this.heldObject == null)
        return;
      if (this.heldObject is Furniture)
      {
        (this.heldObject as Furniture).drawAtNonTileSpot(spriteBatch, Game1.GlobalToLocal(Game1.viewport, new Vector2((float) (this.boundingBox.Center.X - Game1.tileSize / 2), (float) (this.boundingBox.Center.Y - (this.heldObject as Furniture).sourceRect.Height * Game1.pixelZoom - (this.drawHeldObjectLow ? -Game1.tileSize / 4 : Game1.tileSize / 4)))), (float) (this.boundingBox.Bottom - 7) / 10000f, alpha);
      }
      else
      {
        spriteBatch.Draw(Game1.shadowTexture, Game1.GlobalToLocal(Game1.viewport, new Vector2((float) (this.boundingBox.Center.X - Game1.tileSize / 2), (float) (this.boundingBox.Center.Y - (this.drawHeldObjectLow ? Game1.tileSize / 2 : Game1.tileSize * 4 / 3)))) + new Vector2((float) (Game1.tileSize / 2), (float) (Game1.tileSize * 5 / 6)), new Rectangle?(Game1.shadowTexture.Bounds), Color.White * alpha, 0.0f, new Vector2((float) Game1.shadowTexture.Bounds.Center.X, (float) Game1.shadowTexture.Bounds.Center.Y), 4f, SpriteEffects.None, (float) this.boundingBox.Bottom / 10000f);
        spriteBatch.Draw(Game1.objectSpriteSheet, Game1.GlobalToLocal(Game1.viewport, new Vector2((float) (this.boundingBox.Center.X - Game1.tileSize / 2), (float) (this.boundingBox.Center.Y - (this.drawHeldObjectLow ? Game1.tileSize / 2 : Game1.tileSize * 4 / 3)))), new Rectangle?(Game1.currentLocation.getSourceRectForObject(this.heldObject.ParentSheetIndex)), Color.White * alpha, 0.0f, Vector2.Zero, (float) Game1.pixelZoom, SpriteEffects.None, (float) (this.boundingBox.Bottom + 1) / 10000f);
      }
    }

    public void drawAtNonTileSpot(SpriteBatch spriteBatch, Vector2 location, float layerDepth, float alpha = 1f)
    {
      spriteBatch.Draw(Furniture.furnitureTexture, location, new Rectangle?(this.sourceRect), Color.White * alpha, 0.0f, Vector2.Zero, (float) Game1.pixelZoom, this.flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None, layerDepth);
    }

    public override Item getOne()
    {
      Furniture furniture = new Furniture(this.parentSheetIndex, this.tileLocation);
      furniture.drawPosition = this.drawPosition;
      furniture.defaultBoundingBox = this.defaultBoundingBox;
      Rectangle boundingBox = this.boundingBox;
      furniture.boundingBox = boundingBox;
      int num = this.currentRotation - 1;
      furniture.currentRotation = num;
      int rotations = this.rotations;
      furniture.rotations = rotations;
      furniture.rotate();
      return (Item) furniture;
    }
  }
}
