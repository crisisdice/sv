// Decompiled with JetBrains decompiler
// Type: StardewValley.Objects.TV
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StardewValley.Objects
{
  public class TV : Furniture
  {
    public const int customChannel = 1;
    public const int weatherChannel = 2;
    public const int fortuneTellerChannel = 3;
    public const int tipsChannel = 4;
    public const int cookingChannel = 5;
    private int currentChannel;
    private TemporaryAnimatedSprite screen;
    private TemporaryAnimatedSprite screenOverlay;

    public TV()
    {
    }

    public TV(int which, Vector2 tile)
      : base(which, tile)
    {
    }

    public override bool checkForAction(Farmer who, bool justCheckingForActivity = false)
    {
      if (justCheckingForActivity)
        return true;
      List<Response> responseList = new List<Response>();
      responseList.Add(new Response("Weather", Game1.content.LoadString("Strings\\StringsFromCSFiles:TV.cs.13105")));
      responseList.Add(new Response("Fortune", Game1.content.LoadString("Strings\\StringsFromCSFiles:TV.cs.13107")));
      string str = Game1.shortDayNameFromDayOfSeason(Game1.dayOfMonth);
      if (str.Equals("Mon") || str.Equals("Thu"))
        responseList.Add(new Response("Livin'", Game1.content.LoadString("Strings\\StringsFromCSFiles:TV.cs.13111")));
      if (str.Equals("Sun"))
        responseList.Add(new Response("The", Game1.content.LoadString("Strings\\StringsFromCSFiles:TV.cs.13114")));
      if (str.Equals("Wed") && Game1.stats.DaysPlayed > 7U)
        responseList.Add(new Response("The", Game1.content.LoadString("Strings\\StringsFromCSFiles:TV.cs.13117")));
      responseList.Add(new Response("(Leave)", Game1.content.LoadString("Strings\\StringsFromCSFiles:TV.cs.13118")));
      Game1.currentLocation.createQuestionDialogue(Game1.content.LoadString("Strings\\StringsFromCSFiles:TV.cs.13120"), responseList.ToArray(), new GameLocation.afterQuestionBehavior(this.selectChannel), (NPC) null);
      Game1.player.Halt();
      return true;
    }

    public override Item getOne()
    {
      TV tv = new TV(this.parentSheetIndex, this.tileLocation);
      Vector2 drawPosition = this.drawPosition;
      tv.drawPosition = drawPosition;
      Rectangle defaultBoundingBox = this.defaultBoundingBox;
      tv.defaultBoundingBox = defaultBoundingBox;
      Rectangle boundingBox = this.boundingBox;
      tv.boundingBox = boundingBox;
      int num = this.currentRotation - 1;
      tv.currentRotation = num;
      int rotations = this.rotations;
      tv.rotations = rotations;
      tv.rotate();
      return (Item) tv;
    }

    public override void updateWhenCurrentLocation(GameTime time)
    {
      base.updateWhenCurrentLocation(time);
    }

    public void selectChannel(Farmer who, string answer)
    {
      string str = answer.Split(' ')[0];
      if (!(str == "Weather"))
      {
        if (!(str == "Fortune"))
        {
          if (!(str == "Livin'"))
          {
            if (!(str == "The"))
              return;
            this.currentChannel = 5;
            this.screen = new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(602, 361, 42, 28), 150f, 2, 999999, this.getScreenPosition(), false, false, (float) ((double) (this.boundingBox.Bottom - 1) / 10000.0 + 9.99999974737875E-06), 0.0f, Color.White, this.getScreenSizeModifier(), 0.0f, 0.0f, 0.0f, false);
            Game1.drawObjectDialogue(Game1.parseText(Game1.content.LoadString("Strings\\StringsFromCSFiles:TV.cs.13127")));
            Game1.afterDialogues = new Game1.afterFadeFunction(this.proceedToNextScene);
          }
          else
          {
            this.currentChannel = 4;
            this.screen = new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(517, 361, 42, 28), 150f, 2, 999999, this.getScreenPosition(), false, false, (float) ((double) (this.boundingBox.Bottom - 1) / 10000.0 + 9.99999974737875E-06), 0.0f, Color.White, this.getScreenSizeModifier(), 0.0f, 0.0f, 0.0f, false);
            Game1.drawObjectDialogue(Game1.parseText(Game1.content.LoadString("Strings\\StringsFromCSFiles:TV.cs.13124")));
            Game1.afterDialogues = new Game1.afterFadeFunction(this.proceedToNextScene);
          }
        }
        else
        {
          this.currentChannel = 3;
          this.screen = new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(540, 305, 42, 28), 150f, 2, 999999, this.getScreenPosition(), false, false, (float) ((double) (this.boundingBox.Bottom - 1) / 10000.0 + 9.99999974737875E-06), 0.0f, Color.White, this.getScreenSizeModifier(), 0.0f, 0.0f, 0.0f, false);
          Game1.drawObjectDialogue(Game1.parseText(this.getFortuneTellerOpening()));
          Game1.afterDialogues = new Game1.afterFadeFunction(this.proceedToNextScene);
        }
      }
      else
      {
        this.currentChannel = 2;
        this.screen = new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(413, 305, 42, 28), 150f, 2, 999999, this.getScreenPosition(), false, false, (float) ((double) (this.boundingBox.Bottom - 1) / 10000.0 + 9.99999974737875E-06), 0.0f, Color.White, this.getScreenSizeModifier(), 0.0f, 0.0f, 0.0f, false);
        Game1.drawObjectDialogue(Game1.parseText(this.getWeatherChannelOpening()));
        Game1.afterDialogues = new Game1.afterFadeFunction(this.proceedToNextScene);
      }
    }

    private string getFortuneTellerOpening()
    {
      switch (Game1.random.Next(5))
      {
        case 0:
          if (!Game1.player.isMale)
            return Game1.content.LoadString("Strings\\StringsFromCSFiles:TV.cs.13130");
          return Game1.content.LoadString("Strings\\StringsFromCSFiles:TV.cs.13128");
        case 1:
          return Game1.content.LoadString("Strings\\StringsFromCSFiles:TV.cs.13132");
        case 2:
          return Game1.content.LoadString("Strings\\StringsFromCSFiles:TV.cs.13133");
        case 3:
          return Game1.content.LoadString("Strings\\StringsFromCSFiles:TV.cs.13134");
        case 4:
          return Game1.content.LoadString("Strings\\StringsFromCSFiles:TV.cs.13135");
        default:
          return "";
      }
    }

    private string getWeatherChannelOpening()
    {
      return Game1.content.LoadString("Strings\\StringsFromCSFiles:TV.cs.13136");
    }

    public float getScreenSizeModifier()
    {
      if (this.parentSheetIndex != 1468)
        return (float) Game1.pixelZoom / 2f;
      return (float) Game1.pixelZoom;
    }

    public Vector2 getScreenPosition()
    {
      if (this.parentSheetIndex == 1466)
        return new Vector2((float) (this.boundingBox.X + 6 * Game1.pixelZoom), (float) this.boundingBox.Y);
      if (this.parentSheetIndex == 1468)
        return new Vector2((float) (this.boundingBox.X + 3 * Game1.pixelZoom), (float) (this.boundingBox.Y - Game1.tileSize * 2 + Game1.pixelZoom * 8));
      if (this.parentSheetIndex == 1680)
        return new Vector2((float) (this.boundingBox.X + 6 * Game1.pixelZoom), (float) (this.boundingBox.Y - 3 * Game1.pixelZoom));
      return Vector2.Zero;
    }

    public void proceedToNextScene()
    {
      if (this.currentChannel == 2)
      {
        if (this.screenOverlay == null)
        {
          this.screen = new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(497, 305, 42, 28), 9999f, 1, 999999, this.getScreenPosition(), false, false, (float) ((double) (this.boundingBox.Bottom - 1) / 10000.0 + 9.99999974737875E-06), 0.0f, Color.White, this.getScreenSizeModifier(), 0.0f, 0.0f, 0.0f, false);
          Game1.drawObjectDialogue(Game1.parseText(this.getWeatherForecast()));
          this.setWeatherOverlay();
          Game1.afterDialogues = new Game1.afterFadeFunction(this.proceedToNextScene);
        }
        else
          this.turnOffTV();
      }
      else if (this.currentChannel == 3)
      {
        if (this.screenOverlay == null)
        {
          this.screen = new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(624, 305, 42, 28), 9999f, 1, 999999, this.getScreenPosition(), false, false, (float) ((double) (this.boundingBox.Bottom - 1) / 10000.0 + 9.99999974737875E-06), 0.0f, Color.White, this.getScreenSizeModifier(), 0.0f, 0.0f, 0.0f, false);
          Game1.drawObjectDialogue(Game1.parseText(this.getFortuneForecast()));
          this.setFortuneOverlay();
          Game1.afterDialogues = new Game1.afterFadeFunction(this.proceedToNextScene);
        }
        else
          this.turnOffTV();
      }
      else if (this.currentChannel == 4)
      {
        if (this.screenOverlay == null)
        {
          Game1.drawObjectDialogue(Game1.parseText(this.getTodaysTip()));
          Game1.afterDialogues = new Game1.afterFadeFunction(this.proceedToNextScene);
          this.screenOverlay = new TemporaryAnimatedSprite()
          {
            alpha = 1E-07f
          };
        }
        else
          this.turnOffTV();
      }
      else
      {
        if (this.currentChannel != 5)
          return;
        if (this.screenOverlay == null)
        {
          Game1.multipleDialogues(this.getWeeklyRecipe());
          Game1.afterDialogues = new Game1.afterFadeFunction(this.proceedToNextScene);
          this.screenOverlay = new TemporaryAnimatedSprite()
          {
            alpha = 1E-07f
          };
        }
        else
          this.turnOffTV();
      }
    }

    public void turnOffTV()
    {
      this.screen = (TemporaryAnimatedSprite) null;
      this.screenOverlay = (TemporaryAnimatedSprite) null;
    }

    private void setWeatherOverlay()
    {
      switch (Game1.weatherForTomorrow)
      {
        case 0:
        case 6:
          this.screenOverlay = new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(413, 333, 13, 13), 100f, 4, 999999, this.getScreenPosition() + new Vector2(3f, 3f) * this.getScreenSizeModifier(), false, false, (float) ((double) (this.boundingBox.Bottom - 1) / 10000.0 + 1.99999994947575E-05), 0.0f, Color.White, this.getScreenSizeModifier(), 0.0f, 0.0f, 0.0f, false);
          break;
        case 1:
          this.screenOverlay = new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(465, 333, 13, 13), 70f, 4, 999999, this.getScreenPosition() + new Vector2(3f, 3f) * this.getScreenSizeModifier(), false, false, (float) ((double) (this.boundingBox.Bottom - 1) / 10000.0 + 1.99999994947575E-05), 0.0f, Color.White, this.getScreenSizeModifier(), 0.0f, 0.0f, 0.0f, false);
          break;
        case 2:
          this.screenOverlay = new TemporaryAnimatedSprite(Game1.mouseCursors, Game1.currentSeason.Equals("spring") ? new Rectangle(465, 359, 13, 13) : (Game1.currentSeason.Equals("fall") ? new Rectangle(413, 359, 13, 13) : new Rectangle(465, 346, 13, 13)), 70f, 4, 999999, this.getScreenPosition() + new Vector2(3f, 3f) * this.getScreenSizeModifier(), false, false, (float) ((double) (this.boundingBox.Bottom - 1) / 10000.0 + 1.99999994947575E-05), 0.0f, Color.White, this.getScreenSizeModifier(), 0.0f, 0.0f, 0.0f, false);
          break;
        case 3:
          this.screenOverlay = new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(413, 346, 13, 13), 120f, 4, 999999, this.getScreenPosition() + new Vector2(3f, 3f) * this.getScreenSizeModifier(), false, false, (float) ((double) (this.boundingBox.Bottom - 1) / 10000.0 + 1.99999994947575E-05), 0.0f, Color.White, this.getScreenSizeModifier(), 0.0f, 0.0f, 0.0f, false);
          break;
        case 4:
          this.screenOverlay = new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(413, 372, 13, 13), 120f, 4, 999999, this.getScreenPosition() + new Vector2(3f, 3f) * this.getScreenSizeModifier(), false, false, (float) ((double) (this.boundingBox.Bottom - 1) / 10000.0 + 1.99999994947575E-05), 0.0f, Color.White, this.getScreenSizeModifier(), 0.0f, 0.0f, 0.0f, false);
          break;
        case 5:
          this.screenOverlay = new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(465, 346, 13, 13), 100f, 4, 999999, this.getScreenPosition() + new Vector2(3f, 3f) * this.getScreenSizeModifier(), false, false, (float) ((double) (this.boundingBox.Bottom - 1) / 10000.0 + 1.99999994947575E-05), 0.0f, Color.White, this.getScreenSizeModifier(), 0.0f, 0.0f, 0.0f, false);
          break;
      }
    }

    private string getTodaysTip()
    {
      Dictionary<string, string> dictionary = Game1.temporaryContent.Load<Dictionary<string, string>>("Data\\TV\\TipChannel");
      if (!dictionary.ContainsKey(string.Concat((object) (Game1.stats.DaysPlayed % 224U))))
        return Game1.content.LoadString("Strings\\StringsFromCSFiles:TV.cs.13148");
      return dictionary[string.Concat((object) (Game1.stats.DaysPlayed % 224U))];
    }

    private string[] getWeeklyRecipe()
    {
      string[] strArray1 = new string[2];
      int num = (int) (Game1.stats.DaysPlayed % 224U / 7U);
      Dictionary<string, string> dictionary = Game1.temporaryContent.Load<Dictionary<string, string>>("Data\\TV\\CookingChannel");
      if (Game1.shortDayNameFromDayOfSeason(Game1.dayOfMonth).Equals("Wed"))
        num = Math.Max(1, 1 + new Random((int) Game1.stats.DaysPlayed + (int) Game1.uniqueIDForThisGame / 2).Next((int) Game1.stats.DaysPlayed % 224) / 7);
      try
      {
        string key = dictionary[string.Concat((object) num)].Split('/')[0];
        strArray1[0] = dictionary[string.Concat((object) num)].Split('/')[1];
        if (CraftingRecipe.cookingRecipes.ContainsKey(key))
        {
          string[] strArray2 = CraftingRecipe.cookingRecipes[key].Split('/');
          string[] strArray3 = strArray1;
          int index = 1;
          string str;
          if (LocalizedContentManager.CurrentLanguageCode != LocalizedContentManager.LanguageCode.en)
          {
            if (!Game1.player.cookingRecipes.ContainsKey(dictionary[string.Concat((object) num)].Split('/')[0]))
              str = Game1.content.LoadString("Strings\\StringsFromCSFiles:TV.cs.13153", (object) strArray2[strArray2.Length - 1]);
            else
              str = Game1.content.LoadString("Strings\\StringsFromCSFiles:TV.cs.13151", (object) strArray2[strArray2.Length - 1]);
          }
          else if (!Game1.player.cookingRecipes.ContainsKey(dictionary[string.Concat((object) num)].Split('/')[0]))
            str = Game1.content.LoadString("Strings\\StringsFromCSFiles:TV.cs.13153", (object) key);
          else
            str = Game1.content.LoadString("Strings\\StringsFromCSFiles:TV.cs.13151", (object) key);
          strArray3[index] = str;
        }
        else
        {
          string[] strArray2 = strArray1;
          int index = 1;
          string str;
          if (LocalizedContentManager.CurrentLanguageCode != LocalizedContentManager.LanguageCode.en)
          {
            if (!Game1.player.cookingRecipes.ContainsKey(dictionary[string.Concat((object) num)].Split('/')[0]))
              str = Game1.content.LoadString("Strings\\StringsFromCSFiles:TV.cs.13153", (object) ((IEnumerable<string>) dictionary[string.Concat((object) num)].Split('/')).Last<string>());
            else
              str = Game1.content.LoadString("Strings\\StringsFromCSFiles:TV.cs.13151", (object) ((IEnumerable<string>) dictionary[string.Concat((object) num)].Split('/')).Last<string>());
          }
          else if (!Game1.player.cookingRecipes.ContainsKey(dictionary[string.Concat((object) num)].Split('/')[0]))
            str = Game1.content.LoadString("Strings\\StringsFromCSFiles:TV.cs.13153", (object) key);
          else
            str = Game1.content.LoadString("Strings\\StringsFromCSFiles:TV.cs.13151", (object) key);
          strArray2[index] = str;
        }
        if (!Game1.player.cookingRecipes.ContainsKey(key))
          Game1.player.cookingRecipes.Add(key, 0);
      }
      catch (Exception ex)
      {
        string key = dictionary["1"].Split('/')[0];
        strArray1[0] = dictionary["1"].Split('/')[1];
        string[] strArray2 = strArray1;
        int index = 1;
        string str;
        if (LocalizedContentManager.CurrentLanguageCode != LocalizedContentManager.LanguageCode.en)
        {
          if (!Game1.player.cookingRecipes.ContainsKey(dictionary["1"].Split('/')[0]))
            str = Game1.content.LoadString("Strings\\StringsFromCSFiles:TV.cs.13153", (object) ((IEnumerable<string>) dictionary["1"].Split('/')).Last<string>());
          else
            str = Game1.content.LoadString("Strings\\StringsFromCSFiles:TV.cs.13151", (object) ((IEnumerable<string>) dictionary["1"].Split('/')).Last<string>());
        }
        else if (!Game1.player.cookingRecipes.ContainsKey(dictionary["1"].Split('/')[0]))
          str = Game1.content.LoadString("Strings\\StringsFromCSFiles:TV.cs.13153", (object) key);
        else
          str = Game1.content.LoadString("Strings\\StringsFromCSFiles:TV.cs.13151", (object) key);
        strArray2[index] = str;
        if (!Game1.player.cookingRecipes.ContainsKey(key))
          Game1.player.cookingRecipes.Add(key, 0);
      }
      return strArray1;
    }

    private string getWeatherForecast()
    {
      if (Game1.currentSeason.Equals("summer") && Game1.dayOfMonth % 12 == 0)
        Game1.weatherForTomorrow = 3;
      if ((int) Game1.stats.DaysPlayed == 2)
        Game1.weatherForTomorrow = 1;
      if (Game1.dayOfMonth == 28)
        Game1.weatherForTomorrow = 0;
      switch (Game1.weatherForTomorrow)
      {
        case 0:
        case 6:
          if (Game1.random.NextDouble() >= 0.5)
            return Game1.content.LoadString("Strings\\StringsFromCSFiles:TV.cs.13183");
          return Game1.content.LoadString("Strings\\StringsFromCSFiles:TV.cs.13182");
        case 1:
          return Game1.content.LoadString("Strings\\StringsFromCSFiles:TV.cs.13184");
        case 2:
          if (Game1.currentSeason.Equals("spring"))
            return Game1.content.LoadString("Strings\\StringsFromCSFiles:TV.cs.13187");
          if (!Game1.currentSeason.Equals("fall"))
            return Game1.content.LoadString("Strings\\StringsFromCSFiles:TV.cs.13190");
          return Game1.content.LoadString("Strings\\StringsFromCSFiles:TV.cs.13189");
        case 3:
          return Game1.content.LoadString("Strings\\StringsFromCSFiles:TV.cs.13185");
        case 4:
          Dictionary<string, string> dictionary;
          try
          {
            dictionary = Game1.temporaryContent.Load<Dictionary<string, string>>("Data\\Festivals\\" + Game1.currentSeason + (object) (Game1.dayOfMonth + 1));
          }
          catch (Exception ex)
          {
            return Game1.content.LoadString("Strings\\StringsFromCSFiles:TV.cs.13164");
          }
          string str1 = dictionary["name"];
          string str2 = dictionary["conditions"].Split('/')[0];
          int int32_1 = Convert.ToInt32(dictionary["conditions"].Split('/')[1].Split(' ')[0]);
          int int32_2 = Convert.ToInt32(dictionary["conditions"].Split('/')[1].Split(' ')[1]);
          string str3 = "";
          if (!(str2 == "Town"))
          {
            if (!(str2 == "Beach"))
            {
              if (str2 == "Forest")
                str3 = Game1.content.LoadString("Strings\\StringsFromCSFiles:TV.cs.13174");
            }
            else
              str3 = Game1.content.LoadString("Strings\\StringsFromCSFiles:TV.cs.13172");
          }
          else
            str3 = Game1.content.LoadString("Strings\\StringsFromCSFiles:TV.cs.13170");
          return Game1.content.LoadString("Strings\\StringsFromCSFiles:TV.cs.13175", (object) str1, (object) str3, (object) Game1.getTimeOfDayString(int32_1), (object) Game1.getTimeOfDayString(int32_2));
        case 5:
          if (Game1.random.NextDouble() >= 0.5)
            return Game1.content.LoadString("Strings\\StringsFromCSFiles:TV.cs.13181");
          return Game1.content.LoadString("Strings\\StringsFromCSFiles:TV.cs.13180");
        default:
          return "";
      }
    }

    private void setFortuneOverlay()
    {
      if (Game1.dailyLuck < -0.07)
        this.screenOverlay = new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(592, 346, 13, 13), 100f, 4, 999999, this.getScreenPosition() + new Vector2(15f, 1f) * this.getScreenSizeModifier(), false, false, (float) ((double) (this.boundingBox.Bottom - 1) / 10000.0 + 1.99999994947575E-05), 0.0f, Color.White, this.getScreenSizeModifier(), 0.0f, 0.0f, 0.0f, false);
      else if (Game1.dailyLuck < -0.02)
        this.screenOverlay = new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(540, 346, 13, 13), 100f, 4, 999999, this.getScreenPosition() + new Vector2(15f, 1f) * this.getScreenSizeModifier(), false, false, (float) ((double) (this.boundingBox.Bottom - 1) / 10000.0 + 1.99999994947575E-05), 0.0f, Color.White, this.getScreenSizeModifier(), 0.0f, 0.0f, 0.0f, false);
      else if (Game1.dailyLuck > 0.07)
        this.screenOverlay = new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(644, 333, 13, 13), 100f, 4, 999999, this.getScreenPosition() + new Vector2(15f, 1f) * this.getScreenSizeModifier(), false, false, (float) ((double) (this.boundingBox.Bottom - 1) / 10000.0 + 1.99999994947575E-05), 0.0f, Color.White, this.getScreenSizeModifier(), 0.0f, 0.0f, 0.0f, false);
      else if (Game1.dailyLuck > 0.02)
        this.screenOverlay = new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(592, 333, 13, 13), 100f, 4, 999999, this.getScreenPosition() + new Vector2(15f, 1f) * this.getScreenSizeModifier(), false, false, (float) ((double) (this.boundingBox.Bottom - 1) / 10000.0 + 1.99999994947575E-05), 0.0f, Color.White, this.getScreenSizeModifier(), 0.0f, 0.0f, 0.0f, false);
      else
        this.screenOverlay = new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(540, 333, 13, 13), 100f, 4, 999999, this.getScreenPosition() + new Vector2(15f, 1f) * this.getScreenSizeModifier(), false, false, (float) ((double) (this.boundingBox.Bottom - 1) / 10000.0 + 1.99999994947575E-05), 0.0f, Color.White, this.getScreenSizeModifier(), 0.0f, 0.0f, 0.0f, false);
    }

    private string getFortuneForecast()
    {
      string str = Game1.dailyLuck != -0.12 ? (Game1.dailyLuck >= -0.07 ? (Game1.dailyLuck >= -0.02 ? (Game1.dailyLuck != 0.12 ? (Game1.dailyLuck <= 0.07 ? (Game1.dailyLuck <= 0.02 ? Game1.content.LoadString("Strings\\StringsFromCSFiles:TV.cs.13200") : Game1.content.LoadString("Strings\\StringsFromCSFiles:TV.cs.13199")) : Game1.content.LoadString("Strings\\StringsFromCSFiles:TV.cs.13198")) : Game1.content.LoadString("Strings\\StringsFromCSFiles:TV.cs.13197")) : (Game1.random.NextDouble() < 0.5 ? Game1.content.LoadString("Strings\\StringsFromCSFiles:TV.cs.13193") : Game1.content.LoadString("Strings\\StringsFromCSFiles:TV.cs.13195"))) : Game1.content.LoadString("Strings\\StringsFromCSFiles:TV.cs.13192")) : Game1.content.LoadString("Strings\\StringsFromCSFiles:TV.cs.13191");
      if (Game1.dailyLuck == 0.0)
        str = Game1.content.LoadString("Strings\\StringsFromCSFiles:TV.cs.13201");
      return str;
    }

    public override void draw(SpriteBatch spriteBatch, int x, int y, float alpha = 1f)
    {
      base.draw(spriteBatch, x, y, alpha);
      if (this.screen == null)
        return;
      this.screen.update(Game1.currentGameTime);
      this.screen.draw(spriteBatch, false, 0, 0);
      if (this.screenOverlay == null)
        return;
      this.screenOverlay.update(Game1.currentGameTime);
      this.screenOverlay.draw(spriteBatch, false, 0, 0);
    }
  }
}
