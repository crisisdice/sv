// Decompiled with JetBrains decompiler
// Type: StardewValley.Objects.Cask
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.Locations;
using StardewValley.Tools;
using System;

namespace StardewValley.Objects
{
  public class Cask : StardewValley.Object
  {
    public const int defaultDaysToMature = 56;
    public float agingRate;
    public float daysToMature;

    public Cask()
    {
    }

    public Cask(Vector2 v)
      : base(v, 163, false)
    {
    }

    public override bool performToolAction(Tool t)
    {
      if (t == null || !t.isHeavyHitter() || t is MeleeWeapon)
        return base.performToolAction(t);
      if (this.heldObject != null)
        Game1.createItemDebris((Item) this.heldObject, this.tileLocation * (float) Game1.tileSize, -1, (GameLocation) null);
      Game1.playSound("woodWhack");
      if (this.heldObject == null)
        return true;
      this.heldObject = (StardewValley.Object) null;
      this.minutesUntilReady = -1;
      return false;
    }

    public override bool performObjectDropInAction(StardewValley.Object dropIn, bool probe, Farmer who)
    {
      if (dropIn != null && dropIn.bigCraftable || this.heldObject != null)
        return false;
      if (!probe && (who == null || !(who.currentLocation is Cellar)))
      {
        Game1.showRedMessageUsingLoadString("Strings\\Objects:CaskNoCellar");
        return false;
      }
      if (this.quality >= 4)
        return false;
      bool flag = false;
      float num = 1f;
      switch (dropIn.parentSheetIndex)
      {
        case 424:
          flag = true;
          num = 4f;
          break;
        case 426:
          flag = true;
          num = 4f;
          break;
        case 459:
          flag = true;
          num = 2f;
          break;
        case 303:
          flag = true;
          num = 1.66f;
          break;
        case 346:
          flag = true;
          num = 2f;
          break;
        case 348:
          flag = true;
          num = 1f;
          break;
      }
      if (!flag)
        return false;
      this.heldObject = dropIn.getOne() as StardewValley.Object;
      if (!probe)
      {
        this.agingRate = num;
        this.daysToMature = 56f;
        this.minutesUntilReady = 999999;
        if (this.heldObject.quality == 1)
          this.daysToMature = 42f;
        else if (this.heldObject.quality == 2)
          this.daysToMature = 28f;
        else if (this.heldObject.quality == 4)
        {
          this.daysToMature = 0.0f;
          this.minutesUntilReady = 1;
        }
        Game1.playSound("Ship");
        Game1.playSound("bubbles");
        who.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.animations, new Rectangle(256, 1856, 64, 128), 80f, 6, 999999, this.tileLocation * (float) Game1.tileSize + new Vector2(0.0f, (float) (-Game1.tileSize * 2)), false, false, (float) (((double) this.tileLocation.Y + 1.0) * (double) Game1.tileSize / 10000.0 + 9.99999974737875E-05), 0.0f, Color.Yellow * 0.75f, 1f, 0.0f, 0.0f, 0.0f, false)
        {
          alphaFade = 0.005f
        });
      }
      return true;
    }

    public override bool checkForAction(Farmer who, bool justCheckingForActivity = false)
    {
      return base.checkForAction(who, justCheckingForActivity);
    }

    public override void DayUpdate(GameLocation location)
    {
      base.DayUpdate(location);
      if (this.heldObject == null)
        return;
      this.minutesUntilReady = 999999;
      this.daysToMature = this.daysToMature - this.agingRate;
      this.checkForMaturity();
    }

    public void checkForMaturity()
    {
      if ((double) this.daysToMature <= 0.0)
      {
        this.minutesUntilReady = 1;
        this.heldObject.quality = 4;
      }
      else if ((double) this.daysToMature <= 28.0)
      {
        this.heldObject.quality = 2;
      }
      else
      {
        if ((double) this.daysToMature > 42.0)
          return;
        this.heldObject.quality = 1;
      }
    }

    public override void draw(SpriteBatch spriteBatch, int x, int y, float alpha = 1f)
    {
      base.draw(spriteBatch, x, y, alpha);
      if (this.heldObject == null || this.heldObject.quality <= 0)
        return;
      Vector2 vector2 = (this.minutesUntilReady > 0 ? new Vector2(Math.Abs(this.scale.X - 5f), Math.Abs(this.scale.Y - 5f)) : Vector2.Zero) * (float) Game1.pixelZoom;
      Vector2 local = Game1.GlobalToLocal(Game1.viewport, new Vector2((float) (x * Game1.tileSize), (float) (y * Game1.tileSize - Game1.tileSize)));
      Rectangle destinationRectangle = new Rectangle((int) ((double) local.X + (double) (Game1.tileSize / 2) - (double) (Game1.pixelZoom * 2) - (double) vector2.X / 2.0) + (this.shakeTimer > 0 ? Game1.random.Next(-1, 2) : 0), (int) ((double) local.Y + (double) Game1.tileSize + (double) (Game1.pixelZoom * 2) - (double) vector2.Y / 2.0) + (this.shakeTimer > 0 ? Game1.random.Next(-1, 2) : 0), (int) ((double) (Game1.pixelZoom * 4) + (double) vector2.X), (int) ((double) (Game1.pixelZoom * 4) + (double) vector2.Y / 2.0));
      spriteBatch.Draw(Game1.mouseCursors, destinationRectangle, new Rectangle?(this.heldObject.quality < 4 ? new Rectangle(338 + (this.heldObject.quality - 1) * 8, 400, 8, 8) : new Rectangle(346, 392, 8, 8)), Color.White * 0.95f, 0.0f, Vector2.Zero, SpriteEffects.None, (float) ((y + 1) * Game1.tileSize) / 10000f);
    }
  }
}
