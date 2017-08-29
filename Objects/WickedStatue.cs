// Decompiled with JetBrains decompiler
// Type: StardewValley.Objects.WickedStatue
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using StardewValley.Projectiles;
using StardewValley.Tools;

namespace StardewValley.Objects
{
  public class WickedStatue : Object
  {
    public WickedStatue()
    {
    }

    public WickedStatue(Vector2 tileLocation)
      : base(tileLocation, 84, false)
    {
      this.fragility = 2;
      this.boundingBox = new Rectangle((int) tileLocation.X * Game1.tileSize, (int) tileLocation.Y * Game1.tileSize, Game1.tileSize, Game1.tileSize);
    }

    public override bool performToolAction(Tool t)
    {
      if (!(t is Pickaxe))
        return false;
      Game1.createRadialDebris(Game1.currentLocation, 14, (int) this.tileLocation.X, (int) this.tileLocation.Y, Game1.random.Next(8, 16), false, -1, false, -1);
      Game1.createMultipleObjectDebris(390, (int) this.tileLocation.X, (int) this.tileLocation.Y, (int) this.tileLocation.X % 4 + 3, t.getLastFarmerToUse().uniqueMultiplayerID);
      if (Game1.currentLocation.objects.ContainsKey(this.tileLocation))
        Game1.currentLocation.objects.Remove(this.tileLocation);
      Game1.playSound("hammer");
      return false;
    }

    public override void updateWhenCurrentLocation(GameTime time)
    {
      base.updateWhenCurrentLocation(time);
      if (Game1.random.NextDouble() >= 0.001 || Utility.isThereAFarmerWithinDistance(this.tileLocation, 12) == null)
        return;
      Farmer inCurrentLocation = Utility.getNearestFarmerInCurrentLocation(this.tileLocation);
      Vector2 velocityTowardPlayer = Utility.getVelocityTowardPlayer(new Point((int) this.tileLocation.X * Game1.tileSize, (int) this.tileLocation.Y * Game1.tileSize), 12f, inCurrentLocation);
      Game1.currentLocation.projectiles.Add((Projectile) new BasicProjectile(8, 10, 2, 2, 0.1963495f, velocityTowardPlayer.X, velocityTowardPlayer.Y, this.tileLocation * (float) Game1.tileSize));
    }
  }
}
