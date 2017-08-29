// Decompiled with JetBrains decompiler
// Type: StardewValley.Objects.ObjectFactory
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using StardewValley.Tools;
using System;

namespace StardewValley.Objects
{
  public class ObjectFactory
  {
    public const byte regularObject = 0;
    public const byte bigCraftable = 1;
    public const byte weapon = 2;
    public const byte specialItem = 3;
    public const byte regularObjectRecipe = 4;
    public const byte bigCraftableRecipe = 5;

    public static ItemDescription getDescriptionFromItem(Item i)
    {
      if (i is StardewValley.Object && (i as StardewValley.Object).bigCraftable)
        return new ItemDescription((byte) 1, (i as StardewValley.Object).ParentSheetIndex, i.Stack);
      if (i is StardewValley.Object)
        return new ItemDescription((byte) 0, (i as StardewValley.Object).ParentSheetIndex, i.Stack);
      if (i is MeleeWeapon)
        return new ItemDescription((byte) 2, (i as MeleeWeapon).currentParentTileIndex, i.Stack);
      throw new Exception("ItemFactory trying to create item description from unknown item");
    }

    public static Item getItemFromDescription(byte type, int index, int stack)
    {
      switch (type)
      {
        case 0:
          return (Item) new StardewValley.Object(Vector2.Zero, index, stack);
        case 1:
          return (Item) new StardewValley.Object(Vector2.Zero, index, false);
        case 2:
          return (Item) new MeleeWeapon(index);
        case 4:
          return (Item) new StardewValley.Object(index, stack, true, -1, 0);
        case 5:
          return (Item) new StardewValley.Object(Vector2.Zero, index, true);
        default:
          throw new Exception("ItemFactory trying to create item from unknown description");
      }
    }
  }
}
