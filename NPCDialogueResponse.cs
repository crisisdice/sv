// Decompiled with JetBrains decompiler
// Type: StardewValley.NPCDialogueResponse
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

namespace StardewValley
{
  public class NPCDialogueResponse : Response
  {
    public int friendshipChange;
    public int id;

    public NPCDialogueResponse(int id, int friendshipChange, string keyToNPCresponse, string responseText)
      : base(keyToNPCresponse, responseText)
    {
      this.friendshipChange = friendshipChange;
      this.id = id;
    }
  }
}
