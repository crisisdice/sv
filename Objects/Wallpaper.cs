// Decompiled with JetBrains decompiler
// Type: StardewValley.Objects.Wallpaper
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.Locations;
using System.Collections.Generic;

namespace StardewValley.Objects
{
  public class Wallpaper : Object
  {
    private static readonly Rectangle wallpaperContainerRect = new Rectangle(193, 496, 16, 16);
    private static readonly Rectangle floorContainerRect = new Rectangle(209, 496, 16, 16);
    public Rectangle sourceRect;
    public static Texture2D wallpaperTexture;
    public bool isFloor;

    public Wallpaper()
    {
    }

    public Wallpaper(int which, bool isFloor = false)
    {
      if (Wallpaper.wallpaperTexture == null)
        Wallpaper.wallpaperTexture = Game1.content.Load<Texture2D>("Maps\\walls_and_floors");
      this.isFloor = isFloor;
      this.parentSheetIndex = which;
      this.name = isFloor ? "Flooring" : nameof (Wallpaper);
      this.sourceRect = isFloor ? new Rectangle(which % 8 * 32, 336 + which / 8 * 32, 28, 26) : new Rectangle(which % 16 * 16, which / 16 * 48 + 8, 16, 28);
      this.price = 100;
    }

    protected override string loadDisplayName()
    {
      if (!this.isFloor)
        return Game1.content.LoadString("Strings\\StringsFromCSFiles:Wallpaper.cs.13204");
      return Game1.content.LoadString("Strings\\StringsFromCSFiles:Wallpaper.cs.13203");
    }

    public override string getDescription()
    {
      if (!this.isFloor)
        return Game1.content.LoadString("Strings\\StringsFromCSFiles:Wallpaper.cs.13206");
      return Game1.content.LoadString("Strings\\StringsFromCSFiles:Wallpaper.cs.13205");
    }

    public override bool performDropDownAction(Farmer who)
    {
      return true;
    }

    public override bool performObjectDropInAction(Object dropIn, bool probe, Farmer who)
    {
      return false;
    }

    public override bool canBePlacedHere(GameLocation l, Vector2 tile)
    {
      Vector2 vector2 = tile * (float) Game1.tileSize;
      vector2.X += (float) (Game1.tileSize / 2);
      vector2.Y += (float) (Game1.tileSize / 2);
      if (l is DecoratableLocation)
      {
        foreach (Furniture furniture in (l as DecoratableLocation).furniture)
        {
          if (furniture.furniture_type != 12 && furniture.getBoundingBox(furniture.tileLocation).Contains((int) vector2.X, (int) vector2.Y))
            return false;
        }
      }
      return base.canBePlacedHere(l, tile);
    }

    public override bool placementAction(GameLocation location, int x, int y, Farmer who = null)
    {
      if (who == null)
        who = Game1.player;
      if (who.currentLocation is DecoratableLocation)
      {
        Point point = new Point(x / Game1.tileSize, y / Game1.tileSize);
        DecoratableLocation currentLocation = who.currentLocation as DecoratableLocation;
        if (this.isFloor)
        {
          List<Rectangle> rectangleList = !(currentLocation is FarmHouse) ? DecoratableLocation.getFloors() : FarmHouse.getFloors((currentLocation as FarmHouse).upgradeLevel);
          for (int whichRoom = 0; whichRoom < rectangleList.Count; ++whichRoom)
          {
            if (rectangleList[whichRoom].Contains(point))
            {
              currentLocation.setFloor(this.parentSheetIndex, whichRoom, true);
              Game1.playSound("coin");
              return true;
            }
          }
        }
        else
        {
          List<Rectangle> rectangleList = !(currentLocation is FarmHouse) ? DecoratableLocation.getWalls() : FarmHouse.getWalls((currentLocation as FarmHouse).upgradeLevel);
          for (int whichRoom = 0; whichRoom < rectangleList.Count; ++whichRoom)
          {
            if (rectangleList[whichRoom].Contains(point))
            {
              currentLocation.setWallpaper(this.parentSheetIndex, whichRoom, true);
              Game1.playSound("coin");
              return true;
            }
          }
        }
      }
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

    public override void drawWhenHeld(SpriteBatch spriteBatch, Vector2 objectPosition, Farmer f)
    {
      this.drawInMenu(spriteBatch, objectPosition, 1f);
    }

    public override void drawInMenu(SpriteBatch spriteBatch, Vector2 location, float scaleSize, float transparency, float layerDepth, bool drawStackNumber)
    {
      if (Wallpaper.wallpaperTexture == null)
        Wallpaper.wallpaperTexture = Game1.content.Load<Texture2D>("Maps\\walls_and_floors");
      if (this.isFloor)
      {
        spriteBatch.Draw(Wallpaper.wallpaperTexture, location + new Vector2((float) (Game1.tileSize / 2), (float) (Game1.tileSize / 2)), new Rectangle?(Wallpaper.floorContainerRect), Color.White * transparency, 0.0f, new Vector2(8f, 8f), 4f * scaleSize, SpriteEffects.None, layerDepth);
        spriteBatch.Draw(Wallpaper.wallpaperTexture, location + new Vector2((float) (Game1.tileSize / 2), (float) (Game1.tileSize / 2 - 2)), new Rectangle?(this.sourceRect), Color.White * transparency, 0.0f, new Vector2(14f, 13f), 2f * scaleSize, SpriteEffects.None, layerDepth + 1f / 1000f);
      }
      else
      {
        spriteBatch.Draw(Wallpaper.wallpaperTexture, location + new Vector2((float) (Game1.tileSize / 2), (float) (Game1.tileSize / 2)), new Rectangle?(Wallpaper.wallpaperContainerRect), Color.White * transparency, 0.0f, new Vector2(8f, 8f), 4f * scaleSize, SpriteEffects.None, layerDepth);
        spriteBatch.Draw(Wallpaper.wallpaperTexture, location + new Vector2((float) (Game1.tileSize / 2), (float) (Game1.tileSize / 2)), new Rectangle?(this.sourceRect), Color.White * transparency, 0.0f, new Vector2(8f, 14f), 2f * scaleSize, SpriteEffects.None, layerDepth + 1f / 1000f);
      }
    }

    public override Item getOne()
    {
      return (Item) new Wallpaper(this.parentSheetIndex, this.isFloor);
    }
  }
}
