// Decompiled with JetBrains decompiler
// Type: StardewValley.Objects.Ring
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.Locations;
using StardewValley.Monsters;
using System;
using System.Xml.Serialization;

namespace StardewValley.Objects
{
  public class Ring : Item
  {
    public const int ringLowerIndexRange = 516;
    public const int slimeCharmer = 520;
    public const int yobaRing = 524;
    public const int sturdyRing = 525;
    public const int burglarsRing = 526;
    public const int jukeboxRing = 528;
    public const int ringUpperIndexRange = 534;
    [XmlIgnore]
    public string description;
    [XmlIgnore]
    public string displayName;
    public string name;
    public int price;
    public int indexInTileSheet;
    public int uniqueID;

    public override int parentSheetIndex
    {
      get
      {
        return this.indexInTileSheet;
      }
    }

    public Ring()
    {
    }

    public Ring(int which)
    {
      string[] strArray = Game1.objectInformation[which].Split('/');
      this.category = -96;
      this.name = strArray[0];
      this.price = Convert.ToInt32(strArray[1]);
      this.indexInTileSheet = which;
      this.uniqueID = Game1.year + Game1.dayOfMonth + Game1.timeOfDay + this.indexInTileSheet + Game1.player.getTileX() + (int) Game1.stats.MonstersKilled + (int) Game1.stats.itemsCrafted;
      this.loadDisplayFields();
    }

    public void onEquip(Farmer who)
    {
      switch (this.indexInTileSheet)
      {
        case 516:
          Game1.currentLightSources.Add(new LightSource(Game1.lantern, new Vector2(Game1.player.position.X + (float) (Game1.tileSize / 3), who.position.Y + (float) Game1.tileSize), 5f, new Color(0, 50, 170), this.uniqueID));
          break;
        case 517:
          Game1.currentLightSources.Add(new LightSource(Game1.lantern, new Vector2(Game1.player.position.X + (float) (Game1.tileSize / 3), who.position.Y + (float) Game1.tileSize), 10f, new Color(0, 30, 150), this.uniqueID));
          break;
        case 518:
          who.magneticRadius += 64;
          break;
        case 519:
          who.magneticRadius += 128;
          break;
        case 527:
          Game1.currentLightSources.Add(new LightSource(Game1.lantern, new Vector2(Game1.player.position.X + (float) (Game1.tileSize / 3), who.position.Y + (float) Game1.tileSize), 10f, new Color(0, 80, 0), this.uniqueID));
          who.magneticRadius += 128;
          who.attackIncreaseModifier += 0.1f;
          break;
        case 529:
          who.knockbackModifier += 0.1f;
          break;
        case 530:
          who.weaponPrecisionModifier += 0.1f;
          break;
        case 531:
          who.critChanceModifier += 0.1f;
          break;
        case 532:
          who.critPowerModifier += 0.1f;
          break;
        case 533:
          who.weaponSpeedModifier += 0.1f;
          break;
        case 534:
          who.attackIncreaseModifier += 0.1f;
          break;
      }
    }

    public void onUnequip(Farmer who)
    {
      switch (this.indexInTileSheet)
      {
        case 516:
        case 517:
          Utility.removeLightSource(this.uniqueID);
          break;
        case 518:
          who.magneticRadius -= 64;
          break;
        case 519:
          who.magneticRadius -= 128;
          break;
        case 527:
          who.magneticRadius -= 128;
          Utility.removeLightSource(this.uniqueID);
          who.attackIncreaseModifier -= 0.1f;
          break;
        case 529:
          who.knockbackModifier -= 0.1f;
          break;
        case 530:
          who.weaponPrecisionModifier -= 0.1f;
          break;
        case 531:
          who.critChanceModifier -= 0.1f;
          break;
        case 532:
          who.critPowerModifier -= 0.1f;
          break;
        case 533:
          who.weaponSpeedModifier -= 0.1f;
          break;
        case 534:
          who.attackIncreaseModifier -= 0.1f;
          break;
      }
    }

    public override string getCategoryName()
    {
      return Game1.content.LoadString("Strings\\StringsFromCSFiles:Ring.cs.1");
    }

    public void onNewLocation(Farmer who, GameLocation environment)
    {
      switch (this.indexInTileSheet)
      {
        case 516:
        case 517:
          this.onEquip(who);
          break;
        case 527:
          Game1.currentLightSources.Add(new LightSource(Game1.lantern, new Vector2(Game1.player.position.X + (float) (Game1.tileSize / 3), who.position.Y + (float) Game1.tileSize), 10f, new Color(0, 30, 150), this.uniqueID));
          break;
      }
    }

    public void onLeaveLocation(Farmer who, GameLocation environment)
    {
      switch (this.indexInTileSheet)
      {
        case 516:
        case 517:
          this.onUnequip(who);
          break;
        case 527:
          Utility.removeLightSource(this.uniqueID);
          break;
      }
    }

    public override int salePrice()
    {
      return this.price;
    }

    public void onMonsterSlay(Monster m)
    {
      switch (this.indexInTileSheet)
      {
        case 521:
          if (Game1.random.NextDouble() >= 0.1 + (double) Game1.player.LuckLevel / 100.0)
            break;
          Game1.buffsDisplay.addOtherBuff(new Buff(20));
          Game1.playSound("warrior");
          break;
        case 522:
          Game1.player.health = Math.Min(Game1.player.maxHealth, Game1.player.health + 2);
          break;
        case 523:
          Game1.buffsDisplay.addOtherBuff(new Buff(22));
          break;
      }
    }

    public override void drawInMenu(SpriteBatch spriteBatch, Vector2 location, float scaleSize, float transparency, float layerDepth, bool drawStackNumber)
    {
      spriteBatch.Draw(Game1.objectSpriteSheet, location + new Vector2((float) (Game1.tileSize / 2), (float) (Game1.tileSize / 2)) * scaleSize, new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, this.indexInTileSheet, 16, 16)), Color.White * transparency, 0.0f, new Vector2(8f, 8f) * scaleSize, scaleSize * (float) Game1.pixelZoom, SpriteEffects.None, layerDepth);
    }

    public void update(GameTime time, GameLocation environment, Farmer who)
    {
      int indexInTileSheet = this.indexInTileSheet;
      if (indexInTileSheet <= 517)
      {
        if (indexInTileSheet != 516 && indexInTileSheet != 517)
          return;
      }
      else if (indexInTileSheet != 527)
        return;
      Utility.repositionLightSource(this.uniqueID, new Vector2(Game1.player.position.X + (float) (Game1.tileSize / 3), who.position.Y));
      if (environment.isOutdoors || environment is MineShaft)
        return;
      LightSource lightSource = Utility.getLightSource(this.uniqueID);
      if (lightSource == null)
        return;
      lightSource.radius = 3f;
    }

    public override int maximumStackSize()
    {
      return 1;
    }

    public override int getStack()
    {
      return 1;
    }

    public override int addToStack(int amount)
    {
      return 1;
    }

    public override string getDescription()
    {
      if (this.description == null)
        this.loadDisplayFields();
      return Game1.parseText(this.description, Game1.smallFont, Game1.tileSize * 4 + Game1.tileSize / 4);
    }

    public override bool isPlaceable()
    {
      return false;
    }

    [XmlIgnore]
    public override string DisplayName
    {
      get
      {
        if (this.displayName == null)
          this.loadDisplayFields();
        return this.displayName;
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
        return this.name;
      }
    }

    [XmlIgnore]
    public override int Stack
    {
      get
      {
        return 1;
      }
      set
      {
      }
    }

    public override Item getOne()
    {
      return (Item) new Ring(this.indexInTileSheet);
    }

    private bool loadDisplayFields()
    {
      if (Game1.objectInformation == null)
        return false;
      int indexInTileSheet = this.indexInTileSheet;
      string[] strArray = Game1.objectInformation[this.indexInTileSheet].Split('/');
      this.displayName = strArray[4];
      this.description = strArray[5];
      return true;
    }
  }
}
