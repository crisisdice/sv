// Decompiled with JetBrains decompiler
// Type: StardewValley.Objects.SwitchFloor
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.TerrainFeatures;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StardewValley.Objects
{
  public class SwitchFloor : StardewValley.Object
  {
    public static Color successColor = Color.LightBlue;
    private int ticksToSuccess = -1;
    public Color onColor;
    public Color offColor;
    private bool readyToflip;
    public bool finished;
    private float glow;

    public SwitchFloor()
    {
    }

    public SwitchFloor(Vector2 tileLocation, Color onColor, Color offColor, bool on)
    {
      this.tileLocation = tileLocation;
      this.onColor = onColor;
      this.offColor = offColor;
      this.isOn = on;
      this.fragility = 2;
      this.name = "Switch Floor";
    }

    protected override string loadDisplayName()
    {
      return Game1.content.LoadString("Strings\\StringsFromCSFiles:SwitchFloor.cs.13097");
    }

    public void flip(GameLocation environment)
    {
      this.isOn = !this.isOn;
      this.glow = 0.65f;
      foreach (Vector2 adjacentTileLocation in Utility.getAdjacentTileLocations(this.tileLocation))
      {
        if (environment.objects.ContainsKey(adjacentTileLocation) && environment.objects[adjacentTileLocation] is SwitchFloor)
        {
          environment.objects[adjacentTileLocation].isOn = !environment.objects[adjacentTileLocation].isOn;
          (environment.objects[adjacentTileLocation] as SwitchFloor).glow = 0.3f;
        }
      }
      Game1.playSound("shiny4");
    }

    public void setSuccessCountdown(int ticks)
    {
      this.ticksToSuccess = ticks;
      this.glow = 0.5f;
    }

    public void checkForCompleteness()
    {
      Queue<Vector2> vector2Queue = new Queue<Vector2>();
      HashSet<Vector2> source = new HashSet<Vector2>();
      vector2Queue.Enqueue(this.tileLocation);
      Vector2 vector2 = new Vector2();
      List<Vector2> vector2List = new List<Vector2>();
      while (vector2Queue.Count > 0)
      {
        Vector2 index1 = vector2Queue.Dequeue();
        if (Game1.currentLocation.objects.ContainsKey(index1) && Game1.currentLocation.objects[index1] is SwitchFloor && (Game1.currentLocation.objects[index1] as SwitchFloor).isOn != this.isOn)
          return;
        source.Add(index1);
        List<Vector2> adjacentTileLocations = Utility.getAdjacentTileLocations(index1);
        for (int index2 = 0; index2 < adjacentTileLocations.Count; ++index2)
        {
          if (!source.Contains(adjacentTileLocations[index2]) && Game1.currentLocation.objects.ContainsKey(index1) && Game1.currentLocation.objects[index1] is SwitchFloor)
            vector2Queue.Enqueue(adjacentTileLocations[index2]);
        }
        adjacentTileLocations.Clear();
      }
      int ticks = 5;
      foreach (Vector2 key in source)
      {
        if (Game1.currentLocation.objects.ContainsKey(key) && Game1.currentLocation.objects[key] is SwitchFloor)
          (Game1.currentLocation.objects[key] as SwitchFloor).setSuccessCountdown(ticks);
        ticks += 2;
      }
      int coins = (int) Math.Sqrt((double) source.Count) * 2;
      Vector2 index = source.Last<Vector2>();
      while (Game1.currentLocation.isTileOccupiedByFarmer(index) != null)
      {
        source.Remove(index);
        if (source.Count > 0)
          index = source.Last<Vector2>();
      }
      Game1.currentLocation.objects[index] = (StardewValley.Object) new Chest(coins, (List<Item>) null, index, false);
      Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.animations, new Rectangle(0, 320, 64, 64), 50f, 8, 0, index * (float) Game1.tileSize, false, false));
      Game1.playSound("coin");
    }

    public override bool isPassable()
    {
      return true;
    }

    public override void draw(SpriteBatch spriteBatch, int x, int y, float alpha = 1f)
    {
      spriteBatch.Draw(Flooring.floorsTexture, Game1.GlobalToLocal(Game1.viewport, new Vector2((float) (x * Game1.tileSize), (float) (y * Game1.tileSize))), new Rectangle?(new Rectangle(0, 1280, Game1.tileSize, Game1.tileSize)), this.finished ? SwitchFloor.successColor : (this.isOn ? this.onColor : this.offColor), 0.0f, Vector2.Zero, (float) Game1.pixelZoom, SpriteEffects.None, 1E-08f);
      if ((double) this.glow <= 0.0)
        return;
      spriteBatch.Draw(Flooring.floorsTexture, Game1.GlobalToLocal(Game1.viewport, new Vector2((float) (x * Game1.tileSize), (float) (y * Game1.tileSize))), new Rectangle?(new Rectangle(0, 1280, Game1.tileSize, Game1.tileSize)), Color.White * this.glow, 0.0f, Vector2.Zero, (float) Game1.pixelZoom, SpriteEffects.None, 2E-08f);
    }

    public override void updateWhenCurrentLocation(GameTime time)
    {
      if ((double) this.glow > 0.0)
        this.glow = this.glow - 0.04f;
      if (this.ticksToSuccess > 0)
      {
        this.ticksToSuccess = this.ticksToSuccess - 1;
        if (this.ticksToSuccess != 0)
          return;
        this.finished = true;
        this.glow = this.glow + 0.2f;
        Game1.playSound("boulderCrack");
      }
      else
      {
        if (this.finished)
          return;
        foreach (Character farmer in Game1.currentLocation.getFarmers())
        {
          if (farmer.getTileLocation().Equals(this.tileLocation))
          {
            if (this.readyToflip)
            {
              this.flip(Game1.currentLocation);
              this.checkForCompleteness();
            }
            this.readyToflip = false;
            return;
          }
        }
        this.readyToflip = true;
      }
    }
  }
}
