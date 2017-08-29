// Decompiled with JetBrains decompiler
// Type: StardewValley.Objects.SpecialItem
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Xml.Serialization;

namespace StardewValley.Objects
{
  public class SpecialItem : Item
  {
    public const int permanentChangeItem = 1;
    public const int skullKey = 4;
    public const int clubCard = 2;
    public const int backpack = 99;
    public const int darkTalisman = 6;
    public const int magicInk = 7;
    public string name;
    [XmlIgnore]
    private string _displayName;
    public int which;
    public new int category;

    [XmlIgnore]
    private string displayName
    {
      get
      {
        if (string.IsNullOrEmpty(this._displayName))
        {
          switch (this.which)
          {
            case 2:
              this._displayName = Game1.content.LoadString("Strings\\StringsFromCSFiles:SpecialItem.cs.13089");
              break;
            case 4:
              this._displayName = Game1.content.LoadString("Strings\\StringsFromCSFiles:SpecialItem.cs.13088");
              break;
            case 6:
              this._displayName = Game1.content.LoadString("Strings\\Objects:DarkTalisman");
              break;
            case 7:
              this._displayName = Game1.content.LoadString("Strings\\Objects:MagicInk");
              break;
            case 99:
              this._displayName = Game1.player.maxItems != 36 ? Game1.content.LoadString("Strings\\StringsFromCSFiles:GameLocation.cs.8708") : Game1.content.LoadString("Strings\\StringsFromCSFiles:GameLocation.cs.8709");
              break;
          }
        }
        return this._displayName;
      }
      set
      {
        if (string.IsNullOrEmpty(value) && !string.IsNullOrEmpty(this._displayName))
        {
          switch (this.which)
          {
            case 2:
              this._displayName = Game1.content.LoadString("Strings\\StringsFromCSFiles:SpecialItem.cs.13089");
              break;
            case 4:
              this._displayName = Game1.content.LoadString("Strings\\StringsFromCSFiles:SpecialItem.cs.13088");
              break;
            case 6:
              this._displayName = Game1.content.LoadString("Strings\\Objects:DarkTalisman");
              break;
            case 7:
              this._displayName = Game1.content.LoadString("Strings\\Objects:MagicInk");
              break;
            case 99:
              if (Game1.player.maxItems == 36)
              {
                this._displayName = Game1.content.LoadString("Strings\\StringsFromCSFiles:GameLocation.cs.8709");
                break;
              }
              this._displayName = Game1.content.LoadString("Strings\\StringsFromCSFiles:GameLocation.cs.8708");
              break;
          }
        }
        else
          this._displayName = value;
      }
    }

    public SpecialItem(int category, int which, string name = "")
    {
      this.category = category;
      this.which = which;
      this.name = name;
      this.displayName = name;
      if (name.Length >= 1)
        return;
      switch (which)
      {
        case 2:
          this.displayName = Game1.content.LoadString("Strings\\StringsFromCSFiles:SpecialItem.cs.13089");
          break;
        case 4:
          this.displayName = Game1.content.LoadString("Strings\\StringsFromCSFiles:SpecialItem.cs.13088");
          break;
        case 6:
          this.displayName = Game1.content.LoadString("Strings\\Objects:DarkTalisman");
          break;
        case 7:
          this.displayName = Game1.content.LoadString("Strings\\Objects:MagicInk");
          break;
      }
    }

    public SpecialItem()
    {
    }

    public void actionWhenReceived(Farmer who)
    {
      switch (this.which)
      {
        case 4:
          who.hasSkullKey = true;
          who.addQuest(19);
          break;
        case 6:
          who.hasDarkTalisman = true;
          break;
        case 7:
          who.hasMagicInk = true;
          break;
      }
    }

    public TemporaryAnimatedSprite getTemporarySpriteForHoldingUp(Vector2 position)
    {
      if (this.category == 1)
        return new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(129 + 16 * this.which, 320, 16, 16), position, false, 0.0f, Color.White)
        {
          layerDepth = 1f
        };
      if (this.which != 99)
        return (TemporaryAnimatedSprite) null;
      return new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(Game1.player.maxItems == 36 ? 268 : 257, 1436, Game1.player.maxItems == 36 ? 11 : 9, 13), position + new Vector2((float) (Game1.tileSize / 4), 0.0f), false, 0.0f, Color.White)
      {
        scale = (float) Game1.pixelZoom,
        layerDepth = 1f
      };
    }

    public override string checkForSpecialItemHoldUpMeessage()
    {
      if (this.category == 1)
      {
        switch (this.which)
        {
          case 2:
            return Game1.content.LoadString("Strings\\StringsFromCSFiles:SpecialItem.cs.13090", (object) this.displayName);
          case 4:
            return Game1.content.LoadString("Strings\\StringsFromCSFiles:SpecialItem.cs.13092", (object) this.displayName);
          case 6:
            return Game1.content.LoadString("Strings\\Objects:DarkTalismanDescription", (object) this.displayName);
          case 7:
            return Game1.content.LoadString("Strings\\Objects:MagicInkDescription", (object) this.displayName);
        }
      }
      if (this.which != 99)
        return base.checkForSpecialItemHoldUpMeessage();
      return Game1.content.LoadString("Strings\\StringsFromCSFiles:SpecialItem.cs.13094", (object) this.displayName, (object) Game1.player.maxItems);
    }

    public override void drawInMenu(SpriteBatch spriteBatch, Vector2 location, float scaleSize, float transparency, float layerDepth, bool drawStackNumber)
    {
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
      return -1;
    }

    public override string getDescription()
    {
      return (string) null;
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
        return this.displayName;
      }
      set
      {
        this.displayName = value;
      }
    }

    public override string Name
    {
      get
      {
        if (this.name.Length < 1)
        {
          switch (this.which)
          {
            case 2:
              return "Club Card";
            case 4:
              return "Skull Key";
            case 6:
              return Game1.content.LoadString("Strings\\Objects:DarkTalisman");
            case 7:
              return Game1.content.LoadString("Strings\\Objects:MagicInk");
          }
        }
        return this.name;
      }
    }

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
      throw new NotImplementedException();
    }
  }
}
