// Decompiled with JetBrains decompiler
// Type: StardewValley.Objects.ColoredObject
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace StardewValley.Objects
{
  public class ColoredObject : StardewValley.Object
  {
    public Color color;

    public ColoredObject()
    {
    }

    public ColoredObject(int parentSheetIndex, int stack, Color color)
      : base(parentSheetIndex, stack, false, -1, 0)
    {
      this.color = color;
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
        spriteBatch.Draw(Game1.bigCraftableSpriteSheet, location + new Vector2((float) (Game1.tileSize / 2), (float) (Game1.tileSize / 2)) * scaleSize, new Rectangle?(StardewValley.Object.getSourceRectForBigCraftable(this.parentSheetIndex)), Color.White * transparency, 0.0f, new Vector2((float) (Game1.tileSize / 2), (float) Game1.tileSize) * scaleSize, (double) scaleSize < 0.200000002980232 ? scaleSize : scaleSize / 2f, SpriteEffects.None, layerDepth);
        spriteBatch.Draw(Game1.bigCraftableSpriteSheet, location + new Vector2((float) (Game1.tileSize / 2), (float) (Game1.tileSize / 2)) * scaleSize, new Rectangle?(StardewValley.Object.getSourceRectForBigCraftable(this.parentSheetIndex + 1)), this.color * transparency, 0.0f, new Vector2((float) (Game1.tileSize / 2), (float) Game1.tileSize) * scaleSize, (double) scaleSize < 0.200000002980232 ? scaleSize : scaleSize / 2f, SpriteEffects.None, layerDepth + 2E-05f);
      }
      else
      {
        spriteBatch.Draw(Game1.objectSpriteSheet, location + new Vector2((float) (Game1.tileSize / 2), (float) (Game1.tileSize / 2)) * scaleSize, new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, this.parentSheetIndex, 16, 16)), Color.White * transparency, 0.0f, new Vector2(8f, 8f) * scaleSize, (float) Game1.pixelZoom * scaleSize, SpriteEffects.None, layerDepth);
        spriteBatch.Draw(Game1.objectSpriteSheet, location + new Vector2((float) (Game1.tileSize / 2), (float) (Game1.tileSize / 2)) * scaleSize, new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, this.parentSheetIndex + 1, 16, 16)), this.color * transparency, 0.0f, new Vector2(8f, 8f) * scaleSize, (float) Game1.pixelZoom * scaleSize, SpriteEffects.None, layerDepth + 2E-05f);
        if (drawStackNumber && this.maximumStackSize() > 1 && ((double) scaleSize > 0.3 && this.Stack != int.MaxValue) && this.Stack > 1)
          Utility.drawTinyDigits(this.stack, spriteBatch, location + new Vector2((float) (Game1.tileSize - Utility.getWidthOfTinyDigitString(this.stack, 3f * scaleSize)) + 3f * scaleSize, (float) ((double) Game1.tileSize - 18.0 * (double) scaleSize + 2.0)), 3f * scaleSize, 1f, Color.White);
        if (drawStackNumber && this.quality > 0)
        {
          float num = this.quality < 2 ? 0.0f : (float) ((Math.Cos((double) Game1.currentGameTime.TotalGameTime.Milliseconds * Math.PI / 512.0) + 1.0) * 0.0500000007450581);
          spriteBatch.Draw(Game1.mouseCursors, location + new Vector2(12f, (float) (Game1.tileSize - 12) + num), new Rectangle?(new Rectangle(338 + (this.quality - 1) * 8, 400, 8, 8)), Color.White * transparency, 0.0f, new Vector2(4f, 4f), (float) (3.0 * (double) scaleSize * (1.0 + (double) num)), SpriteEffects.None, layerDepth);
        }
      }
      if (!this.isRecipe)
        return;
      spriteBatch.Draw(Game1.objectSpriteSheet, location + new Vector2((float) (Game1.tileSize / 4), (float) (Game1.tileSize / 4)), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, 451, 16, 16)), Color.White, 0.0f, Vector2.Zero, (float) ((double) Game1.pixelZoom * 3.0 / 4.0), SpriteEffects.None, layerDepth + 0.0001f);
    }

    public override void drawWhenHeld(SpriteBatch spriteBatch, Vector2 objectPosition, Farmer f)
    {
      base.drawWhenHeld(spriteBatch, objectPosition, f);
      spriteBatch.Draw(Game1.objectSpriteSheet, objectPosition, new Rectangle?(Game1.currentLocation.getSourceRectForObject(f.ActiveObject.ParentSheetIndex + 1)), this.color, 0.0f, Vector2.Zero, (float) Game1.pixelZoom, SpriteEffects.None, Math.Max(0.0f, (float) (f.getStandingY() + 3) / 10000f));
    }

    public override Item getOne()
    {
      ColoredObject coloredObject = new ColoredObject(this.parentSheetIndex, 1, this.color);
      int quality = this.quality;
      coloredObject.quality = quality;
      int price = this.price;
      coloredObject.price = price;
      int num1 = this.hasBeenInInventory ? 1 : 0;
      coloredObject.hasBeenInInventory = num1 != 0;
      int num2 = this.hasBeenPickedUpByFarmer ? 1 : 0;
      coloredObject.hasBeenPickedUpByFarmer = num2 != 0;
      int specialVariable = this.specialVariable;
      coloredObject.specialVariable = specialVariable;
      return (Item) coloredObject;
    }

    public override void draw(SpriteBatch spriteBatch, int x, int y, float alpha = 1f)
    {
      if (this.bigCraftable)
      {
        Vector2 scale = this.getScale();
        Vector2 local = Game1.GlobalToLocal(Game1.viewport, new Vector2((float) (x * Game1.tileSize), (float) (y * Game1.tileSize - Game1.tileSize)));
        Rectangle destinationRectangle = new Rectangle((int) ((double) local.X - (double) scale.X / 2.0), (int) ((double) local.Y - (double) scale.Y / 2.0), (int) ((double) Game1.tileSize + (double) scale.X), (int) ((double) (Game1.tileSize * 2) + (double) scale.Y / 2.0));
        spriteBatch.Draw(Game1.bigCraftableSpriteSheet, destinationRectangle, new Rectangle?(StardewValley.Object.getSourceRectForBigCraftable(this.showNextIndex ? this.ParentSheetIndex + 1 : this.ParentSheetIndex)), Color.White, 0.0f, Vector2.Zero, SpriteEffects.None, Math.Max(0.0f, (float) ((y + 1) * Game1.tileSize - 1) / 10000f));
        spriteBatch.Draw(Game1.bigCraftableSpriteSheet, destinationRectangle, new Rectangle?(StardewValley.Object.getSourceRectForBigCraftable(this.ParentSheetIndex + 1)), this.color, 0.0f, Vector2.Zero, SpriteEffects.None, Math.Max(0.0f, (float) ((y + 1) * Game1.tileSize - 1) / 10000f));
        if (this.Name.Equals("Loom") && this.minutesUntilReady > 0)
          spriteBatch.Draw(Game1.objectSpriteSheet, this.getLocalPosition(Game1.viewport) + new Vector2((float) (Game1.tileSize / 2), 0.0f), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, 435, 16, 16)), Color.White, this.scale.X, new Vector2((float) (Game1.tileSize / 2), (float) (Game1.tileSize / 2)), 1f, SpriteEffects.None, Math.Max(0.0f, (float) ((y + 1) * Game1.tileSize - 1) / 10000f));
      }
      else if (!Game1.eventUp || Game1.currentLocation.IsFarm)
      {
        if (this.parentSheetIndex != 590)
          spriteBatch.Draw(Game1.shadowTexture, this.getLocalPosition(Game1.viewport) + new Vector2((float) (Game1.tileSize / 2), (float) (Game1.tileSize * 5 / 6)), new Rectangle?(Game1.shadowTexture.Bounds), Color.White, 0.0f, new Vector2((float) Game1.shadowTexture.Bounds.Center.X, (float) Game1.shadowTexture.Bounds.Center.Y), 4f, SpriteEffects.None, 1E-07f);
        SpriteBatch spriteBatch1 = spriteBatch;
        Texture2D objectSpriteSheet1 = Game1.objectSpriteSheet;
        Vector2 local1 = Game1.GlobalToLocal(Game1.viewport, new Vector2((float) (x * Game1.tileSize + Game1.tileSize / 2), (float) (y * Game1.tileSize + Game1.tileSize / 2)));
        Rectangle? sourceRectangle1 = new Rectangle?(Game1.currentLocation.getSourceRectForObject(this.ParentSheetIndex));
        Color white = Color.White;
        double num1 = 0.0;
        Vector2 origin1 = new Vector2(8f, 8f);
        Vector2 scale1 = this.scale;
        double num2 = (double) this.scale.Y > 1.0 ? (double) this.getScale().Y : (double) Game1.pixelZoom;
        int num3 = this.flipped ? 1 : 0;
        double num4 = (double) this.getBoundingBox(new Vector2((float) x, (float) y)).Bottom / 10000.0;
        spriteBatch1.Draw(objectSpriteSheet1, local1, sourceRectangle1, white, (float) num1, origin1, (float) num2, (SpriteEffects) num3, (float) num4);
        SpriteBatch spriteBatch2 = spriteBatch;
        Texture2D objectSpriteSheet2 = Game1.objectSpriteSheet;
        Vector2 local2 = Game1.GlobalToLocal(Game1.viewport, new Vector2((float) (x * Game1.tileSize + Game1.tileSize / 2), (float) (y * Game1.tileSize + Game1.tileSize / 2)));
        Rectangle? sourceRectangle2 = new Rectangle?(Game1.currentLocation.getSourceRectForObject(this.ParentSheetIndex + 1));
        Color color = this.color;
        double num5 = 0.0;
        Vector2 origin2 = new Vector2(8f, 8f);
        Vector2 scale2 = this.scale;
        double num6 = (double) this.scale.Y > 1.0 ? (double) this.getScale().Y : (double) Game1.pixelZoom;
        int num7 = this.flipped ? 1 : 0;
        double num8 = (double) this.getBoundingBox(new Vector2((float) x, (float) y)).Bottom / 10000.0;
        spriteBatch2.Draw(objectSpriteSheet2, local2, sourceRectangle2, color, (float) num5, origin2, (float) num6, (SpriteEffects) num7, (float) num8);
      }
      if (this.Name == null || !this.Name.Contains("Table") || this.heldObject == null)
        return;
      spriteBatch.Draw(Game1.objectSpriteSheet, Game1.GlobalToLocal(Game1.viewport, new Vector2((float) (x * Game1.tileSize), (float) (y * Game1.tileSize - (this.bigCraftable ? Game1.tileSize * 3 / 4 : Game1.tileSize / 3)))), new Rectangle?(Game1.currentLocation.getSourceRectForObject(this.heldObject.ParentSheetIndex)), Color.White, 0.0f, Vector2.Zero, 1f, this.flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None, (float) (y * Game1.tileSize + Game1.tileSize + 1) / 10000f);
    }
  }
}
