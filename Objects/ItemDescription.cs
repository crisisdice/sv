// Decompiled with JetBrains decompiler
// Type: StardewValley.Objects.ItemDescription
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

namespace StardewValley.Objects
{
  public struct ItemDescription
  {
    public byte type;
    public int index;
    public int stack;

    public ItemDescription(byte type, int index, int stack)
    {
      this.type = type;
      this.index = index;
      this.stack = stack;
    }
  }
}
