// Decompiled with JetBrains decompiler
// Type: StardewValley.Objects.Hat
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace StardewValley.Objects
{
  public class Hat : Item
  {
    public const int widthOfTileSheetSquare = 20;
    public const int heightOfTileSheetSquare = 20;
    public int which;
    [XmlIgnore]
    public string displayName;
    [XmlIgnore]
    public string description;
    public string name;
    public bool skipHairDraw;
    public bool ignoreHairstyleOffset;

    public Hat()
    {
      this.load(this.which);
    }

    public void load(int which)
    {
      Dictionary<int, string> dictionary = Game1.content.Load<Dictionary<int, string>>("Data\\hats");
      int key = which;
      if (!dictionary.ContainsKey(key))
        which = 0;
      int index = which;
      string[] strArray = dictionary[index].Split('/');
      this.name = strArray[0];
      this.skipHairDraw = Convert.ToBoolean(strArray[2]);
      this.ignoreHairstyleOffset = Convert.ToBoolean(strArray[3]);
      this.category = -95;
    }

    public Hat(int which)
    {
      this.which = which;
      this.load(which);
    }

    public override void drawInMenu(SpriteBatch spriteBatch, Vector2 location, float scaleSize, float transparency, float layerDepth, bool drawStackNumber)
    {
      spriteBatch.Draw(FarmerRenderer.hatsTexture, location + new Vector2(10f, 10f), new Rectangle?(new Rectangle(this.which * 20 % FarmerRenderer.hatsTexture.Width, this.which * 20 / FarmerRenderer.hatsTexture.Width * 20 * 4, 20, 20)), Color.White * transparency, 0.0f, new Vector2(3f, 3f), 3f * scaleSize, SpriteEffects.None, layerDepth);
    }

    public override string getDescription()
    {
      if (this.description == null)
        this.loadDisplayFields();
      return Game1.parseText(this.description, Game1.smallFont, Game1.tileSize * 4 + Game1.tileSize / 4);
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
      return (Item) new Hat(this.which);
    }

    private bool loadDisplayFields()
    {
      if (this.name != null)
      {
        foreach (KeyValuePair<int, string> keyValuePair in Game1.content.Load<Dictionary<int, string>>("Data\\hats"))
        {
          string[] strArray = keyValuePair.Value.Split('/');
          if (strArray[0] == this.name)
          {
            this.displayName = this.name;
            if (LocalizedContentManager.CurrentLanguageCode != LocalizedContentManager.LanguageCode.en)
              this.displayName = strArray[strArray.Length - 1];
            this.description = strArray[1];
            return true;
          }
        }
      }
      return false;
    }
  }
}
