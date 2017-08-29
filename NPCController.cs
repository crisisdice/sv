// Decompiled with JetBrains decompiler
// Type: StardewValley.NPCController
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace StardewValley
{
  public class NPCController
  {
    private int pauseTime = -1;
    public NPC puppet;
    private bool loop;
    private List<Vector2> path;
    private Vector2 target;
    private int pathIndex;
    private int speed;
    private NPCController.endBehavior behaviorAtEnd;

    private int CurrentPathX
    {
      get
      {
        if (this.pathIndex >= this.path.Count)
          return 0;
        return (int) this.path[this.pathIndex].X;
      }
    }

    private int CurrentPathY
    {
      get
      {
        if (this.pathIndex >= this.path.Count)
          return 0;
        return (int) this.path[this.pathIndex].Y;
      }
    }

    private bool MovingHorizontally
    {
      get
      {
        return (uint) this.CurrentPathX > 0U;
      }
    }

    public NPCController(NPC n, List<Vector2> path, bool loop, NPCController.endBehavior endBehavior = null)
    {
      if (n == null)
        return;
      this.speed = n.speed;
      this.loop = loop;
      this.puppet = n;
      this.path = path;
      this.setMoving(true);
      this.behaviorAtEnd = endBehavior;
    }

    private bool setMoving(bool newTarget)
    {
      if (this.puppet == null || this.pathIndex >= this.path.Count)
        return false;
      int direction = 2;
      if (this.CurrentPathX > 0)
        direction = 1;
      else if (this.CurrentPathX < 0)
        direction = 3;
      else if (this.CurrentPathY < 0)
        direction = 0;
      else if (this.CurrentPathY > 0)
        direction = 2;
      this.puppet.Halt();
      this.puppet.faceDirection(direction);
      if (this.CurrentPathX != 0 && this.CurrentPathY != 0)
      {
        this.pauseTime = this.CurrentPathY;
        this.puppet.faceDirection(this.CurrentPathX % 4);
        return true;
      }
      this.puppet.setMovingInFacingDirection();
      if (newTarget)
        this.target = new Vector2(this.puppet.position.X + (float) (this.CurrentPathX * Game1.tileSize), this.puppet.Position.Y + (float) (this.CurrentPathY * Game1.tileSize));
      return true;
    }

    public bool update(GameTime time, GameLocation location, List<NPCController> allControllers)
    {
      this.puppet.speed = this.speed;
      bool flag = false;
      foreach (NPCController allController in allControllers)
      {
        if (allController.puppet != null)
        {
          if (allController.puppet.Equals((object) this.puppet))
            flag = true;
          if (allController.puppet.facingDirection == this.puppet.facingDirection && !allController.puppet.Equals((object) this.puppet) && allController.puppet.GetBoundingBox().Intersects(this.puppet.nextPosition(this.puppet.facingDirection)))
          {
            if (!flag)
              return false;
            break;
          }
        }
      }
      this.puppet.MovePosition(time, Game1.viewport, location);
      if (this.pauseTime < 0 && !this.puppet.isMoving())
        this.setMoving(false);
      if (this.pauseTime < 0 && (double) Math.Abs(Vector2.Distance(this.puppet.position, this.target)) <= (double) this.puppet.Speed)
      {
        this.pathIndex = this.pathIndex + 1;
        if (!this.setMoving(true))
        {
          if (this.loop)
          {
            this.pathIndex = 0;
            this.setMoving(true);
          }
          else
          {
            if (this.behaviorAtEnd != null)
              this.behaviorAtEnd();
            return true;
          }
        }
      }
      else if (this.pauseTime >= 0)
      {
        this.pauseTime = this.pauseTime - time.ElapsedGameTime.Milliseconds;
        if (this.pauseTime < 0)
        {
          this.pathIndex = this.pathIndex + 1;
          if (!this.setMoving(true))
          {
            if (this.loop)
            {
              this.pathIndex = 0;
              this.setMoving(true);
            }
            else
            {
              if (this.behaviorAtEnd != null)
                this.behaviorAtEnd();
              return true;
            }
          }
        }
      }
      return false;
    }

    public delegate void endBehavior();
  }
}
