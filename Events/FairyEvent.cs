﻿// Decompiled with JetBrains decompiler
// Type: StardewValley.Events.FairyEvent
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.TerrainFeatures;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StardewValley.Events
{
  public class FairyEvent : FarmEvent
  {
    public const int identifier = 942069;
    private Vector2 fairyPosition;
    private Vector2 targetCrop;
    private Farm f;
    private int fairyFrame;
    private int fairyAnimationTimer;
    private int animationLoopsDone;
    private int timerSinceFade;
    private bool animateLeft;
    private bool terminate;

    public bool setUp()
    {
      this.f = Game1.getLocationFromName("Farm") as Farm;
      if (Game1.isRaining)
        return true;
      int num = 100;
      Random random = new Random((int) Game1.uniqueIDForThisGame + (int) Game1.stats.DaysPlayed);
      this.targetCrop = Vector2.Zero;
      while (num > 0 && this.targetCrop.Equals(Vector2.Zero))
      {
        --num;
        if (this.f.terrainFeatures.Count != 0)
        {
          KeyValuePair<Vector2, TerrainFeature> keyValuePair = this.f.terrainFeatures.ElementAt<KeyValuePair<Vector2, TerrainFeature>>(random.Next(this.f.terrainFeatures.Count));
          if (keyValuePair.Value is HoeDirt && (keyValuePair.Value as HoeDirt).crop != null && (!(keyValuePair.Value as HoeDirt).crop.isWildSeedCrop() && (keyValuePair.Value as HoeDirt).crop.currentPhase < (keyValuePair.Value as HoeDirt).crop.phaseDays.Count - 1))
            this.targetCrop = keyValuePair.Key;
        }
      }
      if (this.targetCrop.Equals(Vector2.Zero))
        return true;
      Game1.currentLightSources.Add(new LightSource(4, this.fairyPosition, 1f, Color.Black, 942069));
      Game1.currentLocation = (GameLocation) this.f;
      this.f.resetForPlayerEntry();
      Game1.fadeClear();
      Game1.nonWarpFade = true;
      Game1.timeOfDay = 2400;
      Game1.displayHUD = false;
      Game1.freezeControls = true;
      Game1.viewportFreeze = true;
      Game1.displayFarmer = false;
      Game1.viewport.X = Math.Max(0, Math.Min(this.f.map.DisplayWidth - Game1.viewport.Width, (int) this.targetCrop.X * Game1.tileSize - Game1.viewport.Width / 2));
      Game1.viewport.Y = Math.Max(0, Math.Min(this.f.map.DisplayHeight - Game1.viewport.Height, (int) this.targetCrop.Y * Game1.tileSize - Game1.viewport.Height / 2));
      this.fairyPosition = new Vector2((float) (Game1.viewport.X + Game1.viewport.Width + Game1.tileSize * 2), this.targetCrop.Y * (float) Game1.tileSize - (float) Game1.tileSize);
      Game1.changeMusicTrack("nightTime");
      return false;
    }

    public bool tickUpdate(GameTime time)
    {
      if (this.terminate)
        return true;
      Game1.UpdateGameClock(time);
      this.f.UpdateWhenCurrentLocation(time);
      this.f.updateEvenIfFarmerIsntHere(time, false);
      Game1.UpdateOther(time);
      Utility.repositionLightSource(942069, this.fairyPosition + new Vector2((float) (Game1.tileSize / 2), (float) (Game1.tileSize / 2)));
      TimeSpan timeSpan;
      if (this.animationLoopsDone < 1)
      {
        int timerSinceFade = this.timerSinceFade;
        timeSpan = time.ElapsedGameTime;
        int milliseconds = timeSpan.Milliseconds;
        this.timerSinceFade = timerSinceFade + milliseconds;
      }
      if ((double) this.fairyPosition.X > (double) this.targetCrop.X * (double) Game1.tileSize + (double) (Game1.tileSize / 2))
      {
        if (this.timerSinceFade < 2000)
          return false;
        double num1 = (double) this.fairyPosition.X;
        timeSpan = time.ElapsedGameTime;
        double num2 = (double) timeSpan.Milliseconds * 0.100000001490116;
        double num3 = num1 - num2;
        this.fairyPosition.X = (float) num3;
        double num4 = (double) this.fairyPosition.Y;
        timeSpan = time.TotalGameTime;
        double num5 = Math.Cos((double) timeSpan.Milliseconds * Math.PI / 512.0) * 1.0;
        double num6 = num4 + num5;
        this.fairyPosition.Y = (float) num6;
        int fairyFrame1 = this.fairyFrame;
        timeSpan = time.TotalGameTime;
        this.fairyFrame = timeSpan.Milliseconds % 500 <= 250 ? 0 : 1;
        int fairyFrame2 = this.fairyFrame;
        if (fairyFrame1 != fairyFrame2 && this.fairyFrame == 1)
        {
          Game1.playSound("batFlap");
          this.f.temporarySprites.Add(new TemporaryAnimatedSprite(11, this.fairyPosition + new Vector2((float) (Game1.tileSize / 2), 0.0f), Color.Purple, 8, false, 100f, 0, -1, -1f, -1, 0));
        }
        if ((double) this.fairyPosition.X <= (double) this.targetCrop.X * (double) Game1.tileSize + (double) (Game1.tileSize / 2))
          this.fairyFrame = 1;
      }
      else if (this.animationLoopsDone < 4)
      {
        int fairyAnimationTimer = this.fairyAnimationTimer;
        timeSpan = time.ElapsedGameTime;
        int milliseconds = timeSpan.Milliseconds;
        this.fairyAnimationTimer = fairyAnimationTimer + milliseconds;
        if (this.fairyAnimationTimer > 250)
        {
          this.fairyAnimationTimer = 0;
          if (!this.animateLeft)
          {
            this.fairyFrame = this.fairyFrame + 1;
            if (this.fairyFrame == 3)
            {
              this.animateLeft = true;
              this.f.temporarySprites.Add(new TemporaryAnimatedSprite(10, this.fairyPosition + new Vector2(-16f, (float) Game1.tileSize), Color.LightPink, 8, false, 100f, 0, -1, -1f, -1, 0));
              Game1.playSound("yoba");
              if (this.f.terrainFeatures.ContainsKey(this.targetCrop))
                (this.f.terrainFeatures[this.targetCrop] as HoeDirt).crop.currentPhase = Math.Min((this.f.terrainFeatures[this.targetCrop] as HoeDirt).crop.currentPhase + 1, (this.f.terrainFeatures[this.targetCrop] as HoeDirt).crop.phaseDays.Count - 1);
            }
          }
          else
          {
            this.fairyFrame = this.fairyFrame - 1;
            if (this.fairyFrame == 1)
            {
              this.animateLeft = false;
              this.animationLoopsDone = this.animationLoopsDone + 1;
              if (this.animationLoopsDone >= 4)
              {
                for (int index = 0; index < 10; ++index)
                  DelayedAction.playSoundAfterDelay("batFlap", 4000 + 500 * index);
              }
            }
          }
        }
      }
      else
      {
        int fairyAnimationTimer = this.fairyAnimationTimer;
        timeSpan = time.ElapsedGameTime;
        int milliseconds1 = timeSpan.Milliseconds;
        this.fairyAnimationTimer = fairyAnimationTimer + milliseconds1;
        timeSpan = time.TotalGameTime;
        this.fairyFrame = timeSpan.Milliseconds % 500 <= 250 ? 0 : 1;
        if (this.fairyAnimationTimer > 2000 && (double) this.fairyPosition.Y > -999999.0)
        {
          double num1 = (double) this.fairyPosition.X;
          timeSpan = time.TotalGameTime;
          double num2 = Math.Cos((double) timeSpan.Milliseconds * Math.PI / 256.0) * 2.0;
          double num3 = num1 + num2;
          this.fairyPosition.X = (float) num3;
          double num4 = (double) this.fairyPosition.Y;
          timeSpan = time.ElapsedGameTime;
          double num5 = (double) timeSpan.Milliseconds * 0.200000002980232;
          double num6 = num4 - num5;
          this.fairyPosition.Y = (float) num6;
        }
        if ((double) this.fairyPosition.Y < (double) (Game1.viewport.Y - Game1.tileSize * 2) || float.IsNaN(this.fairyPosition.Y))
        {
          if (!Game1.fadeToBlack && (double) this.fairyPosition.Y != -999999.0)
          {
            Game1.globalFadeToBlack(new Game1.afterFadeFunction(this.afterLastFade), 0.02f);
            Game1.changeMusicTrack("none");
            this.timerSinceFade = 0;
            this.fairyPosition.Y = -999999f;
          }
          int timerSinceFade = this.timerSinceFade;
          timeSpan = time.ElapsedGameTime;
          int milliseconds2 = timeSpan.Milliseconds;
          this.timerSinceFade = timerSinceFade + milliseconds2;
        }
      }
      return false;
    }

    public void afterLastFade()
    {
      this.terminate = true;
      Game1.globalFadeToClear((Game1.afterFadeFunction) null, 0.02f);
    }

    public void draw(SpriteBatch b)
    {
      b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, this.fairyPosition), new Rectangle?(new Rectangle(16 + this.fairyFrame * 16, 592, 16, 16)), Color.White, 0.0f, Vector2.Zero, 4f, SpriteEffects.None, 0.9999999f);
    }

    public void makeChangesToLocation()
    {
      for (int index1 = (int) this.targetCrop.X - 2; (double) index1 <= (double) this.targetCrop.X + 2.0; ++index1)
      {
        for (int index2 = (int) this.targetCrop.Y - 2; (double) index2 <= (double) this.targetCrop.Y + 2.0; ++index2)
        {
          Vector2 key = new Vector2((float) index1, (float) index2);
          if (this.f.terrainFeatures.ContainsKey(key) && this.f.terrainFeatures[key] is HoeDirt && (this.f.terrainFeatures[key] as HoeDirt).crop != null)
            (this.f.terrainFeatures[key] as HoeDirt).crop.growCompletely();
        }
      }
    }

    public void drawAboveEverything(SpriteBatch b)
    {
    }
  }
}
