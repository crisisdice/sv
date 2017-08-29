// Decompiled with JetBrains decompiler
// Type: StardewValley.Options
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewValley.Menus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace StardewValley
{
  public class Options
  {
    public InputButton[] actionButton = new InputButton[2]
    {
      new InputButton(Microsoft.Xna.Framework.Input.Keys.X),
      new InputButton(false)
    };
    public InputButton[] toolSwapButton = new InputButton[0];
    public InputButton[] cancelButton = new InputButton[1]
    {
      new InputButton(Microsoft.Xna.Framework.Input.Keys.V)
    };
    public InputButton[] useToolButton = new InputButton[2]
    {
      new InputButton(Microsoft.Xna.Framework.Input.Keys.C),
      new InputButton(true)
    };
    public InputButton[] moveUpButton = new InputButton[1]
    {
      new InputButton(Microsoft.Xna.Framework.Input.Keys.W)
    };
    public InputButton[] moveRightButton = new InputButton[1]
    {
      new InputButton(Microsoft.Xna.Framework.Input.Keys.D)
    };
    public InputButton[] moveDownButton = new InputButton[1]
    {
      new InputButton(Microsoft.Xna.Framework.Input.Keys.S)
    };
    public InputButton[] moveLeftButton = new InputButton[1]
    {
      new InputButton(Microsoft.Xna.Framework.Input.Keys.A)
    };
    public InputButton[] menuButton = new InputButton[2]
    {
      new InputButton(Microsoft.Xna.Framework.Input.Keys.E),
      new InputButton(Microsoft.Xna.Framework.Input.Keys.Escape)
    };
    public InputButton[] runButton = new InputButton[1]
    {
      new InputButton(Microsoft.Xna.Framework.Input.Keys.LeftShift)
    };
    public InputButton[] tmpKeyToReplace = new InputButton[1]
    {
      new InputButton(Microsoft.Xna.Framework.Input.Keys.None)
    };
    public InputButton[] chatButton = new InputButton[2]
    {
      new InputButton(Microsoft.Xna.Framework.Input.Keys.T),
      new InputButton(Microsoft.Xna.Framework.Input.Keys.OemQuestion)
    };
    public InputButton[] mapButton = new InputButton[1]
    {
      new InputButton(Microsoft.Xna.Framework.Input.Keys.M)
    };
    public InputButton[] journalButton = new InputButton[1]
    {
      new InputButton(Microsoft.Xna.Framework.Input.Keys.F)
    };
    public InputButton[] inventorySlot1 = new InputButton[1]
    {
      new InputButton(Microsoft.Xna.Framework.Input.Keys.D1)
    };
    public InputButton[] inventorySlot2 = new InputButton[1]
    {
      new InputButton(Microsoft.Xna.Framework.Input.Keys.D2)
    };
    public InputButton[] inventorySlot3 = new InputButton[1]
    {
      new InputButton(Microsoft.Xna.Framework.Input.Keys.D3)
    };
    public InputButton[] inventorySlot4 = new InputButton[1]
    {
      new InputButton(Microsoft.Xna.Framework.Input.Keys.D4)
    };
    public InputButton[] inventorySlot5 = new InputButton[1]
    {
      new InputButton(Microsoft.Xna.Framework.Input.Keys.D5)
    };
    public InputButton[] inventorySlot6 = new InputButton[1]
    {
      new InputButton(Microsoft.Xna.Framework.Input.Keys.D6)
    };
    public InputButton[] inventorySlot7 = new InputButton[1]
    {
      new InputButton(Microsoft.Xna.Framework.Input.Keys.D7)
    };
    public InputButton[] inventorySlot8 = new InputButton[1]
    {
      new InputButton(Microsoft.Xna.Framework.Input.Keys.D8)
    };
    public InputButton[] inventorySlot9 = new InputButton[1]
    {
      new InputButton(Microsoft.Xna.Framework.Input.Keys.D9)
    };
    public InputButton[] inventorySlot10 = new InputButton[1]
    {
      new InputButton(Microsoft.Xna.Framework.Input.Keys.D0)
    };
    public InputButton[] inventorySlot11 = new InputButton[1]
    {
      new InputButton(Microsoft.Xna.Framework.Input.Keys.OemMinus)
    };
    public InputButton[] inventorySlot12 = new InputButton[1]
    {
      new InputButton(Microsoft.Xna.Framework.Input.Keys.OemPlus)
    };
    private float appliedZoomLevel = 1f;
    private int appliedLightingQuality = 32;
    public const float minZoom = 0.75f;
    public const float maxZoom = 1.25f;
    public const int toggleAutoRun = 0;
    public const int musicVolume = 1;
    public const int soundVolume = 2;
    public const int toggleDialogueTypingSounds = 3;
    public const int toggleFullscreen = 4;
    public const int toggleWindowedOrTrueFullscreen = 5;
    public const int screenResolution = 6;
    public const int showPortraitsToggle = 7;
    public const int showMerchantPortraitsToggle = 8;
    public const int menuBG = 9;
    public const int toggleFootsteps = 10;
    public const int alwaysShowToolHitLocationToggle = 11;
    public const int hideToolHitLocationWhenInMotionToggle = 12;
    public const int windowMode = 13;
    public const int pauseWhenUnfocused = 14;
    public const int pinToolbar = 15;
    public const int toggleRumble = 16;
    public const int ambientOnly = 17;
    public const int zoom = 18;
    public const int zoomButtonsToggle = 19;
    public const int ambientVolume = 20;
    public const int footstepVolume = 21;
    public const int invertScrollDirectionToggle = 22;
    public const int snowTransparencyToggle = 23;
    public const int screenFlashToggle = 24;
    public const int lightingQualityToggle = 25;
    public const int toggleHardwareCursor = 26;
    public const int toggleShowPlacementTileGamepad = 27;
    public const int toggleSnappyMenus = 29;
    public const int input_actionButton = 7;
    public const int input_toolSwapButton = 8;
    public const int input_cancelButton = 9;
    public const int input_useToolButton = 10;
    public const int input_moveUpButton = 11;
    public const int input_moveRightButton = 12;
    public const int input_moveDownButton = 13;
    public const int input_moveLeftButton = 14;
    public const int input_menuButton = 15;
    public const int input_runButton = 16;
    public const int input_chatButton = 17;
    public const int input_journalButton = 18;
    public const int input_mapButton = 19;
    public const int input_slot1 = 20;
    public const int input_slot2 = 21;
    public const int input_slot3 = 22;
    public const int input_slot4 = 23;
    public const int input_slot5 = 24;
    public const int input_slot6 = 25;
    public const int input_slot7 = 26;
    public const int input_slot8 = 27;
    public const int input_slot9 = 28;
    public const int input_slot10 = 29;
    public const int input_slot11 = 30;
    public const int input_slot12 = 31;
    public const int checkBoxOption = 1;
    public const int sliderOption = 2;
    public const int dropDownOption = 3;
    public const float defaultZoomLevel = 1f;
    public const int defaultLightingQuality = 32;
    public bool autoRun;
    public bool dialogueTyping;
    public bool fullscreen;
    public bool windowedBorderlessFullscreen;
    public bool showPortraits;
    public bool showMerchantPortraits;
    public bool showMenuBackground;
    public bool playFootstepSounds;
    public bool alwaysShowToolHitLocation;
    public bool hideToolHitLocationWhenInMotion;
    public bool pauseWhenOutOfFocus;
    public bool pinToolbarToggle;
    public bool mouseControls;
    public bool keyboardControls;
    public bool gamepadControls;
    public bool rumble;
    public bool ambientOnlyToggle;
    public bool zoomButtons;
    public bool invertScrollDirection;
    public bool screenFlash;
    public bool hardwareCursor;
    public bool showPlacementTileForGamepad;
    public bool snappyMenus;
    public float musicVolumeLevel;
    public float soundVolumeLevel;
    public float zoomLevel;
    public float footstepVolumeLevel;
    public float ambientVolumeLevel;
    public float snowTransparency;
    public int preferredResolutionX;
    public int preferredResolutionY;
    public int lightingQuality;

    public Options()
    {
      this.setToDefaults();
    }

    public bool SnappyMenus
    {
      get
      {
        if (this.snappyMenus && this.gamepadControls)
        {
          MouseState state = Mouse.GetState();
          if (state.LeftButton != Microsoft.Xna.Framework.Input.ButtonState.Pressed)
          {
            state = Mouse.GetState();
            return state.RightButton != Microsoft.Xna.Framework.Input.ButtonState.Pressed;
          }
        }
        return false;
      }
    }

    public Microsoft.Xna.Framework.Input.Keys getFirstKeyboardKeyFromInputButtonList(InputButton[] inputButton)
    {
      for (int index = 0; index < inputButton.Length; ++index)
      {
        // ISSUE: explicit reference operation
        // ISSUE: variable of a reference type
        InputButton& local = @inputButton[index];
        if (inputButton[index].key != Microsoft.Xna.Framework.Input.Keys.None)
          return inputButton[index].key;
      }
      return Microsoft.Xna.Framework.Input.Keys.None;
    }

    public void reApplySetOptions()
    {
      if ((double) this.zoomLevel != (double) this.appliedZoomLevel || this.lightingQuality != this.appliedLightingQuality)
      {
        Program.gamePtr.refreshWindowSettings();
        this.appliedZoomLevel = this.zoomLevel;
        this.appliedLightingQuality = this.lightingQuality;
      }
      Program.gamePtr.IsMouseVisible = this.hardwareCursor;
    }

    public void setToDefaults()
    {
      this.playFootstepSounds = true;
      this.showMenuBackground = false;
      this.showMerchantPortraits = true;
      this.showPortraits = true;
      this.autoRun = true;
      this.alwaysShowToolHitLocation = false;
      this.hideToolHitLocationWhenInMotion = true;
      this.dialogueTyping = true;
      this.rumble = true;
      this.fullscreen = false;
      this.pinToolbarToggle = false;
      this.zoomLevel = 1f;
      this.zoomButtons = false;
      this.pauseWhenOutOfFocus = true;
      this.screenFlash = true;
      this.snowTransparency = 1f;
      this.invertScrollDirection = false;
      this.ambientOnlyToggle = false;
      this.windowedBorderlessFullscreen = true;
      this.showPlacementTileForGamepad = true;
      this.lightingQuality = 32;
      this.hardwareCursor = false;
      this.musicVolumeLevel = 0.75f;
      this.ambientVolumeLevel = 0.75f;
      this.footstepVolumeLevel = 0.9f;
      this.soundVolumeLevel = 1f;
      this.preferredResolutionX = GraphicsAdapter.DefaultAdapter.SupportedDisplayModes.Last<DisplayMode>().Width;
      this.preferredResolutionY = GraphicsAdapter.DefaultAdapter.SupportedDisplayModes.Last<DisplayMode>().Height;
      this.snappyMenus = true;
    }

    public void setControlsToDefault()
    {
      this.actionButton = new InputButton[2]
      {
        new InputButton(Microsoft.Xna.Framework.Input.Keys.X),
        new InputButton(false)
      };
      this.toolSwapButton = new InputButton[1]
      {
        new InputButton(Microsoft.Xna.Framework.Input.Keys.Z)
      };
      this.cancelButton = new InputButton[1]
      {
        new InputButton(Microsoft.Xna.Framework.Input.Keys.V)
      };
      this.useToolButton = new InputButton[2]
      {
        new InputButton(Microsoft.Xna.Framework.Input.Keys.C),
        new InputButton(true)
      };
      this.moveUpButton = new InputButton[1]
      {
        new InputButton(Microsoft.Xna.Framework.Input.Keys.W)
      };
      this.moveRightButton = new InputButton[1]
      {
        new InputButton(Microsoft.Xna.Framework.Input.Keys.D)
      };
      this.moveDownButton = new InputButton[1]
      {
        new InputButton(Microsoft.Xna.Framework.Input.Keys.S)
      };
      this.moveLeftButton = new InputButton[1]
      {
        new InputButton(Microsoft.Xna.Framework.Input.Keys.A)
      };
      this.menuButton = new InputButton[2]
      {
        new InputButton(Microsoft.Xna.Framework.Input.Keys.E),
        new InputButton(Microsoft.Xna.Framework.Input.Keys.Escape)
      };
      this.runButton = new InputButton[1]
      {
        new InputButton(Microsoft.Xna.Framework.Input.Keys.LeftShift)
      };
      this.tmpKeyToReplace = new InputButton[1]
      {
        new InputButton(Microsoft.Xna.Framework.Input.Keys.None)
      };
      this.chatButton = new InputButton[2]
      {
        new InputButton(Microsoft.Xna.Framework.Input.Keys.T),
        new InputButton(Microsoft.Xna.Framework.Input.Keys.OemQuestion)
      };
      this.mapButton = new InputButton[1]
      {
        new InputButton(Microsoft.Xna.Framework.Input.Keys.M)
      };
      this.journalButton = new InputButton[1]
      {
        new InputButton(Microsoft.Xna.Framework.Input.Keys.F)
      };
      this.inventorySlot1 = new InputButton[1]
      {
        new InputButton(Microsoft.Xna.Framework.Input.Keys.D1)
      };
      this.inventorySlot2 = new InputButton[1]
      {
        new InputButton(Microsoft.Xna.Framework.Input.Keys.D2)
      };
      this.inventorySlot3 = new InputButton[1]
      {
        new InputButton(Microsoft.Xna.Framework.Input.Keys.D3)
      };
      this.inventorySlot4 = new InputButton[1]
      {
        new InputButton(Microsoft.Xna.Framework.Input.Keys.D4)
      };
      this.inventorySlot5 = new InputButton[1]
      {
        new InputButton(Microsoft.Xna.Framework.Input.Keys.D5)
      };
      this.inventorySlot6 = new InputButton[1]
      {
        new InputButton(Microsoft.Xna.Framework.Input.Keys.D6)
      };
      this.inventorySlot7 = new InputButton[1]
      {
        new InputButton(Microsoft.Xna.Framework.Input.Keys.D7)
      };
      this.inventorySlot8 = new InputButton[1]
      {
        new InputButton(Microsoft.Xna.Framework.Input.Keys.D8)
      };
      this.inventorySlot9 = new InputButton[1]
      {
        new InputButton(Microsoft.Xna.Framework.Input.Keys.D9)
      };
      this.inventorySlot10 = new InputButton[1]
      {
        new InputButton(Microsoft.Xna.Framework.Input.Keys.D0)
      };
      this.inventorySlot11 = new InputButton[1]
      {
        new InputButton(Microsoft.Xna.Framework.Input.Keys.OemMinus)
      };
      this.inventorySlot12 = new InputButton[1]
      {
        new InputButton(Microsoft.Xna.Framework.Input.Keys.OemPlus)
      };
    }

    public string getNameOfOptionFromIndex(int index)
    {
      switch (index)
      {
        case 0:
          return Game1.content.LoadString("Strings\\StringsFromCSFiles:Options.cs.4556");
        case 1:
          return Game1.content.LoadString("Strings\\StringsFromCSFiles:Options.cs.4557");
        case 2:
          return Game1.content.LoadString("Strings\\StringsFromCSFiles:Options.cs.4558");
        case 3:
          return Game1.content.LoadString("Strings\\StringsFromCSFiles:Options.cs.4559");
        case 4:
          return Game1.content.LoadString("Strings\\StringsFromCSFiles:Options.cs.4560");
        case 5:
          return Game1.content.LoadString("Strings\\StringsFromCSFiles:Options.cs.4561");
        case 6:
          return Game1.content.LoadString("Strings\\StringsFromCSFiles:Options.cs.4562");
        default:
          return "";
      }
    }

    public int whatTypeOfOption(int index)
    {
      if (index == 1 || index == 2)
        return 2;
      return index == 6 ? 3 : 1;
    }

    public void changeCheckBoxOption(int which, bool value)
    {
      switch (which)
      {
        case 0:
          this.autoRun = value;
          Game1.player.setRunning(this.autoRun, false);
          break;
        case 3:
          this.dialogueTyping = value;
          break;
        case 7:
          this.showPortraits = value;
          break;
        case 8:
          this.showMerchantPortraits = value;
          break;
        case 9:
          this.showMenuBackground = value;
          break;
        case 10:
          this.playFootstepSounds = value;
          break;
        case 11:
          this.alwaysShowToolHitLocation = value;
          break;
        case 12:
          this.hideToolHitLocationWhenInMotion = value;
          break;
        case 14:
          this.pauseWhenOutOfFocus = value;
          break;
        case 15:
          this.pinToolbarToggle = value;
          break;
        case 16:
          this.rumble = value;
          break;
        case 17:
          this.ambientOnlyToggle = value;
          break;
        case 19:
          this.zoomButtons = value;
          break;
        case 22:
          this.invertScrollDirection = value;
          break;
        case 24:
          this.screenFlash = value;
          break;
        case 26:
          this.hardwareCursor = value;
          Program.gamePtr.IsMouseVisible = this.hardwareCursor;
          break;
        case 27:
          this.showPlacementTileForGamepad = value;
          break;
        case 29:
          this.snappyMenus = value;
          break;
      }
    }

    public void changeSliderOption(int which, int value)
    {
      switch (which)
      {
        case 1:
          this.musicVolumeLevel = (float) value / 100f;
          Game1.musicCategory.SetVolume(this.musicVolumeLevel);
          break;
        case 2:
          this.soundVolumeLevel = (float) value / 100f;
          Game1.soundCategory.SetVolume(this.soundVolumeLevel);
          break;
        case 18:
          int num1 = (int) ((double) this.zoomLevel * 100.0);
          int num2 = num1;
          int num3 = (int) ((double) value * 100.0);
          if (num3 >= num1 + 10 || num3 >= 100)
            num1 = Math.Min(100, num1 + 10);
          else if (num3 <= num1 - 10 || num3 <= 50)
            num1 = Math.Max(50, num1 - 10);
          if (num1 == num2)
            break;
          this.zoomLevel = (float) num1 / 100f;
          Game1.overrideGameMenuReset = true;
          Program.gamePtr.refreshWindowSettings();
          Game1.overrideGameMenuReset = false;
          Game1.showGlobalMessage(Game1.content.LoadString("Strings\\StringsFromCSFiles:Options.cs.4563") + (object) this.zoomLevel);
          break;
        case 20:
          this.ambientVolumeLevel = (float) value / 100f;
          Game1.ambientCategory.SetVolume(this.ambientVolumeLevel);
          break;
        case 21:
          this.footstepVolumeLevel = (float) value / 100f;
          Game1.footstepCategory.SetVolume(this.footstepVolumeLevel);
          break;
        case 23:
          this.snowTransparency = (float) value / 100f;
          break;
      }
    }

    public void setWindowedOption(string setting)
    {
      if (!(setting == "Windowed"))
      {
        if (!(setting == "Fullscreen"))
        {
          if (!(setting == "Windowed Borderless"))
            return;
          this.setWindowedOption(0);
        }
        else
          this.setWindowedOption(2);
      }
      else
        this.setWindowedOption(1);
    }

    public void setWindowedOption(int setting)
    {
      this.windowedBorderlessFullscreen = this.isCurrentlyWindowedBorderless();
      this.fullscreen = !this.windowedBorderlessFullscreen && Game1.graphics.IsFullScreen;
      int num1 = -1;
      switch (setting)
      {
        case 0:
          if (!this.windowedBorderlessFullscreen)
          {
            this.windowedBorderlessFullscreen = true;
            Game1.toggleFullscreen();
            this.fullscreen = false;
          }
          num1 = 0;
          break;
        case 1:
          if (Game1.graphics.IsFullScreen && !this.windowedBorderlessFullscreen)
          {
            Game1.toggleNonBorderlessWindowedFullscreen();
            this.fullscreen = false;
            this.windowedBorderlessFullscreen = false;
          }
          else if (this.windowedBorderlessFullscreen)
          {
            this.fullscreen = false;
            this.windowedBorderlessFullscreen = false;
            Game1.toggleFullscreen();
          }
          num1 = 1;
          break;
        case 2:
          if (this.windowedBorderlessFullscreen)
          {
            this.fullscreen = true;
            this.windowedBorderlessFullscreen = false;
            Game1.toggleFullscreen();
          }
          else if (!Game1.graphics.IsFullScreen)
          {
            Game1.toggleNonBorderlessWindowedFullscreen();
            this.fullscreen = true;
            this.windowedBorderlessFullscreen = false;
            this.hardwareCursor = false;
            Program.gamePtr.IsMouseVisible = false;
          }
          num1 = 2;
          break;
      }
      if ((int) Game1.gameMode == 3)
      {
        Game1.exitActiveMenu();
        Game1.activeClickableMenu = (IClickableMenu) new GameMenu(6, 6);
      }
      try
      {
        StartupPreferences startupPreferences = new StartupPreferences();
        startupPreferences.loadPreferences();
        int num2 = num1;
        startupPreferences.windowMode = num2;
        startupPreferences.savePreferences();
      }
      catch (Exception ex)
      {
      }
    }

    public void changeDropDownOption(int which, int selection, List<string> options)
    {
      if (which <= 13)
      {
        if (which != 6)
        {
          if (which != 13)
            return;
          this.setWindowedOption(options[selection]);
        }
        else
        {
          Rectangle oldBounds = new Rectangle(Game1.viewport.X, Game1.viewport.Y, Game1.viewport.Width, Game1.viewport.Height);
          string option = options[selection];
          int int32_1 = Convert.ToInt32(option.Split(' ')[0]);
          int int32_2 = Convert.ToInt32(option.Split(' ')[2]);
          this.preferredResolutionX = int32_1;
          this.preferredResolutionY = int32_2;
          Game1.graphics.PreferredBackBufferWidth = int32_1;
          Game1.graphics.PreferredBackBufferHeight = int32_2;
          Game1.graphics.ApplyChanges();
          Game1.updateViewportForScreenSizeChange(true, int32_1, int32_2);
          foreach (IClickableMenu onScreenMenu in Game1.onScreenMenus)
            onScreenMenu.gameWindowSizeChanged(oldBounds, new Rectangle(Game1.viewport.X, Game1.viewport.Y, Game1.viewport.Width, Game1.viewport.Height));
          if (Game1.currentMinigame != null)
            Game1.currentMinigame.changeScreenSize();
          Game1.exitActiveMenu();
          Game1.activeClickableMenu = (IClickableMenu) new GameMenu(6, 6);
        }
      }
      else if (which != 18)
      {
        if (which != 25)
          return;
        string option = options[selection];
        if (!(option == "Lowest"))
        {
          if (!(option == "Low"))
          {
            if (!(option == "Med."))
            {
              if (!(option == "High"))
              {
                if (option == "Ultra")
                  this.lightingQuality = 8;
              }
              else
                this.lightingQuality = 16;
            }
            else
              this.lightingQuality = 32;
          }
          else
            this.lightingQuality = 64;
        }
        else
          this.lightingQuality = 128;
        int id = Game1.activeClickableMenu.getCurrentlySnappedComponent() != null ? Game1.activeClickableMenu.getCurrentlySnappedComponent().myID : -1;
        Game1.overrideGameMenuReset = true;
        Program.gamePtr.refreshWindowSettings();
        Game1.overrideGameMenuReset = false;
        Game1.activeClickableMenu = (IClickableMenu) new GameMenu(6, 19);
        if (!this.snappyMenus)
          return;
        Game1.activeClickableMenu.setCurrentlySnappedComponentTo(id);
      }
      else
      {
        this.zoomLevel = (float) Convert.ToInt32(options[selection].Replace("%", "")) / 100f;
        Game1.overrideGameMenuReset = true;
        Program.gamePtr.refreshWindowSettings();
        Game1.overrideGameMenuReset = false;
        Game1.activeClickableMenu = (IClickableMenu) new GameMenu(6, 14);
        if (Game1.debrisWeather == null)
          return;
        Game1.randomizeDebrisWeatherPositions(Game1.debrisWeather);
      }
    }

    public bool isKeyInUse(Microsoft.Xna.Framework.Input.Keys key)
    {
      foreach (InputButton allUsedInputButton in this.getAllUsedInputButtons())
      {
        if (allUsedInputButton.key == key)
          return true;
      }
      return false;
    }

    public List<InputButton> getAllUsedInputButtons()
    {
      List<InputButton> inputButtonList = new List<InputButton>();
      InputButton[] useToolButton = this.useToolButton;
      inputButtonList.AddRange((IEnumerable<InputButton>) useToolButton);
      InputButton[] actionButton = this.actionButton;
      inputButtonList.AddRange((IEnumerable<InputButton>) actionButton);
      InputButton[] moveUpButton = this.moveUpButton;
      inputButtonList.AddRange((IEnumerable<InputButton>) moveUpButton);
      InputButton[] moveRightButton = this.moveRightButton;
      inputButtonList.AddRange((IEnumerable<InputButton>) moveRightButton);
      InputButton[] moveDownButton = this.moveDownButton;
      inputButtonList.AddRange((IEnumerable<InputButton>) moveDownButton);
      InputButton[] moveLeftButton = this.moveLeftButton;
      inputButtonList.AddRange((IEnumerable<InputButton>) moveLeftButton);
      InputButton[] runButton = this.runButton;
      inputButtonList.AddRange((IEnumerable<InputButton>) runButton);
      InputButton[] menuButton = this.menuButton;
      inputButtonList.AddRange((IEnumerable<InputButton>) menuButton);
      InputButton[] journalButton = this.journalButton;
      inputButtonList.AddRange((IEnumerable<InputButton>) journalButton);
      InputButton[] mapButton = this.mapButton;
      inputButtonList.AddRange((IEnumerable<InputButton>) mapButton);
      InputButton[] toolSwapButton = this.toolSwapButton;
      inputButtonList.AddRange((IEnumerable<InputButton>) toolSwapButton);
      InputButton[] chatButton = this.chatButton;
      inputButtonList.AddRange((IEnumerable<InputButton>) chatButton);
      InputButton[] inventorySlot1 = this.inventorySlot1;
      inputButtonList.AddRange((IEnumerable<InputButton>) inventorySlot1);
      InputButton[] inventorySlot2 = this.inventorySlot2;
      inputButtonList.AddRange((IEnumerable<InputButton>) inventorySlot2);
      InputButton[] inventorySlot3 = this.inventorySlot3;
      inputButtonList.AddRange((IEnumerable<InputButton>) inventorySlot3);
      InputButton[] inventorySlot4 = this.inventorySlot4;
      inputButtonList.AddRange((IEnumerable<InputButton>) inventorySlot4);
      InputButton[] inventorySlot5 = this.inventorySlot5;
      inputButtonList.AddRange((IEnumerable<InputButton>) inventorySlot5);
      InputButton[] inventorySlot6 = this.inventorySlot6;
      inputButtonList.AddRange((IEnumerable<InputButton>) inventorySlot6);
      InputButton[] inventorySlot7 = this.inventorySlot7;
      inputButtonList.AddRange((IEnumerable<InputButton>) inventorySlot7);
      InputButton[] inventorySlot8 = this.inventorySlot8;
      inputButtonList.AddRange((IEnumerable<InputButton>) inventorySlot8);
      InputButton[] inventorySlot9 = this.inventorySlot9;
      inputButtonList.AddRange((IEnumerable<InputButton>) inventorySlot9);
      InputButton[] inventorySlot10 = this.inventorySlot10;
      inputButtonList.AddRange((IEnumerable<InputButton>) inventorySlot10);
      InputButton[] inventorySlot11 = this.inventorySlot11;
      inputButtonList.AddRange((IEnumerable<InputButton>) inventorySlot11);
      InputButton[] inventorySlot12 = this.inventorySlot12;
      inputButtonList.AddRange((IEnumerable<InputButton>) inventorySlot12);
      return inputButtonList;
    }

    public void setCheckBoxToProperValue(OptionsCheckbox checkbox)
    {
      switch (checkbox.whichOption)
      {
        case 0:
          checkbox.isChecked = this.autoRun;
          break;
        case 3:
          checkbox.isChecked = this.dialogueTyping;
          break;
        case 4:
          this.windowedBorderlessFullscreen = Control.FromHandle(Program.gamePtr.Window.Handle).FindForm().FormBorderStyle == FormBorderStyle.None;
          this.fullscreen = Game1.graphics.IsFullScreen || this.windowedBorderlessFullscreen;
          checkbox.isChecked = this.fullscreen;
          break;
        case 5:
          checkbox.isChecked = this.windowedBorderlessFullscreen;
          checkbox.greyedOut = !this.fullscreen;
          break;
        case 7:
          checkbox.isChecked = this.showPortraits;
          break;
        case 8:
          checkbox.isChecked = this.showMerchantPortraits;
          break;
        case 9:
          checkbox.isChecked = this.showMenuBackground;
          break;
        case 10:
          checkbox.isChecked = this.playFootstepSounds;
          break;
        case 11:
          checkbox.isChecked = this.alwaysShowToolHitLocation;
          break;
        case 12:
          checkbox.isChecked = this.hideToolHitLocationWhenInMotion;
          break;
        case 14:
          checkbox.isChecked = this.pauseWhenOutOfFocus;
          break;
        case 15:
          checkbox.isChecked = this.pinToolbarToggle;
          break;
        case 16:
          checkbox.isChecked = this.rumble;
          checkbox.greyedOut = !this.gamepadControls;
          break;
        case 17:
          checkbox.isChecked = this.ambientOnlyToggle;
          break;
        case 19:
          checkbox.isChecked = this.zoomButtons;
          break;
        case 22:
          checkbox.isChecked = this.invertScrollDirection;
          break;
        case 24:
          checkbox.isChecked = this.screenFlash;
          break;
        case 26:
          checkbox.isChecked = this.hardwareCursor;
          checkbox.greyedOut = this.fullscreen;
          break;
        case 27:
          checkbox.isChecked = this.showPlacementTileForGamepad;
          checkbox.greyedOut = !this.gamepadControls;
          break;
        case 29:
          checkbox.isChecked = this.snappyMenus;
          break;
      }
    }

    public void setPlusMinusToProperValue(OptionsPlusMinus plusMinus)
    {
      switch (plusMinus.whichOption)
      {
        case 18:
          string str1 = Math.Round((double) this.zoomLevel * 100.0).ToString() + "%";
          for (int index = 0; index < plusMinus.options.Count; ++index)
          {
            if (plusMinus.options[index].Equals(str1))
            {
              plusMinus.selected = index;
              break;
            }
          }
          break;
        case 25:
          string str2 = "";
          switch (this.lightingQuality)
          {
            case 32:
              str2 = "Med.";
              break;
            case 64:
              str2 = "Low";
              break;
            case 128:
              str2 = "Lowest";
              break;
            case 8:
              str2 = "Ultra";
              break;
            case 16:
              str2 = "High";
              break;
          }
          for (int index = 0; index < plusMinus.options.Count; ++index)
          {
            if (plusMinus.options[index].Equals(str2))
            {
              plusMinus.selected = index;
              break;
            }
          }
          break;
      }
    }

    public void setSliderToProperValue(OptionsSlider slider)
    {
      switch (slider.whichOption)
      {
        case 1:
          slider.value = (int) ((double) this.musicVolumeLevel * 100.0);
          break;
        case 2:
          slider.value = (int) ((double) this.soundVolumeLevel * 100.0);
          break;
        case 18:
          slider.value = (int) ((double) this.zoomLevel * 100.0);
          break;
        case 20:
          slider.value = (int) ((double) this.ambientVolumeLevel * 100.0);
          break;
        case 21:
          slider.value = (int) ((double) this.footstepVolumeLevel * 100.0);
          break;
        case 23:
          slider.value = (int) ((double) this.snowTransparency * 100.0);
          break;
      }
    }

    public bool doesInputListContain(InputButton[] list, Microsoft.Xna.Framework.Input.Keys key)
    {
      for (int index = 0; index < list.Length; ++index)
      {
        if (list[index].key == key)
          return true;
      }
      return false;
    }

    public void changeInputListenerValue(int whichListener, Microsoft.Xna.Framework.Input.Keys key)
    {
      switch (whichListener)
      {
        case 7:
          this.actionButton[0] = new InputButton(key);
          break;
        case 8:
          this.toolSwapButton[0] = new InputButton(key);
          break;
        case 10:
          this.useToolButton[0] = new InputButton(key);
          break;
        case 11:
          this.moveUpButton[0] = new InputButton(key);
          break;
        case 12:
          this.moveRightButton[0] = new InputButton(key);
          break;
        case 13:
          this.moveDownButton[0] = new InputButton(key);
          break;
        case 14:
          this.moveLeftButton[0] = new InputButton(key);
          break;
        case 15:
          this.menuButton[0] = new InputButton(key);
          break;
        case 16:
          this.runButton[0] = new InputButton(key);
          break;
        case 17:
          this.chatButton[0] = new InputButton(key);
          break;
        case 18:
          this.journalButton[0] = new InputButton(key);
          break;
        case 19:
          this.mapButton[0] = new InputButton(key);
          break;
        case 20:
          this.inventorySlot1[0] = new InputButton(key);
          break;
        case 21:
          this.inventorySlot2[0] = new InputButton(key);
          break;
        case 22:
          this.inventorySlot3[0] = new InputButton(key);
          break;
        case 23:
          this.inventorySlot4[0] = new InputButton(key);
          break;
        case 24:
          this.inventorySlot5[0] = new InputButton(key);
          break;
        case 25:
          this.inventorySlot6[0] = new InputButton(key);
          break;
        case 26:
          this.inventorySlot7[0] = new InputButton(key);
          break;
        case 27:
          this.inventorySlot8[0] = new InputButton(key);
          break;
        case 28:
          this.inventorySlot9[0] = new InputButton(key);
          break;
        case 29:
          this.inventorySlot10[0] = new InputButton(key);
          break;
        case 30:
          this.inventorySlot11[0] = new InputButton(key);
          break;
        case 31:
          this.inventorySlot12[0] = new InputButton(key);
          break;
      }
    }

    public void setInputListenerToProperValue(OptionsInputListener inputListener)
    {
      inputListener.buttonNames.Clear();
      switch (inputListener.whichOption)
      {
        case 7:
          foreach (InputButton inputButton in this.actionButton)
            inputListener.buttonNames.Add(inputButton.ToString());
          break;
        case 8:
          foreach (InputButton inputButton in this.toolSwapButton)
            inputListener.buttonNames.Add(inputButton.ToString());
          break;
        case 10:
          foreach (InputButton inputButton in this.useToolButton)
            inputListener.buttonNames.Add(inputButton.ToString());
          break;
        case 11:
          foreach (InputButton inputButton in this.moveUpButton)
            inputListener.buttonNames.Add(inputButton.ToString());
          break;
        case 12:
          foreach (InputButton inputButton in this.moveRightButton)
            inputListener.buttonNames.Add(inputButton.ToString());
          break;
        case 13:
          foreach (InputButton inputButton in this.moveDownButton)
            inputListener.buttonNames.Add(inputButton.ToString());
          break;
        case 14:
          foreach (InputButton inputButton in this.moveLeftButton)
            inputListener.buttonNames.Add(inputButton.ToString());
          break;
        case 15:
          foreach (InputButton inputButton in this.menuButton)
            inputListener.buttonNames.Add(inputButton.ToString());
          break;
        case 16:
          foreach (InputButton inputButton in this.runButton)
            inputListener.buttonNames.Add(inputButton.ToString());
          break;
        case 17:
          foreach (InputButton inputButton in this.chatButton)
            inputListener.buttonNames.Add(inputButton.ToString());
          break;
        case 18:
          foreach (InputButton inputButton in this.journalButton)
            inputListener.buttonNames.Add(inputButton.ToString());
          break;
        case 19:
          foreach (InputButton inputButton in this.mapButton)
            inputListener.buttonNames.Add(inputButton.ToString());
          break;
        case 20:
          foreach (InputButton inputButton in this.inventorySlot1)
            inputListener.buttonNames.Add(inputButton.ToString());
          break;
        case 21:
          foreach (InputButton inputButton in this.inventorySlot2)
            inputListener.buttonNames.Add(inputButton.ToString());
          break;
        case 22:
          foreach (InputButton inputButton in this.inventorySlot3)
            inputListener.buttonNames.Add(inputButton.ToString());
          break;
        case 23:
          foreach (InputButton inputButton in this.inventorySlot4)
            inputListener.buttonNames.Add(inputButton.ToString());
          break;
        case 24:
          foreach (InputButton inputButton in this.inventorySlot5)
            inputListener.buttonNames.Add(inputButton.ToString());
          break;
        case 25:
          foreach (InputButton inputButton in this.inventorySlot6)
            inputListener.buttonNames.Add(inputButton.ToString());
          break;
        case 26:
          foreach (InputButton inputButton in this.inventorySlot7)
            inputListener.buttonNames.Add(inputButton.ToString());
          break;
        case 27:
          foreach (InputButton inputButton in this.inventorySlot8)
            inputListener.buttonNames.Add(inputButton.ToString());
          break;
        case 28:
          foreach (InputButton inputButton in this.inventorySlot9)
            inputListener.buttonNames.Add(inputButton.ToString());
          break;
        case 29:
          foreach (InputButton inputButton in this.inventorySlot10)
            inputListener.buttonNames.Add(inputButton.ToString());
          break;
        case 30:
          foreach (InputButton inputButton in this.inventorySlot11)
            inputListener.buttonNames.Add(inputButton.ToString());
          break;
        case 31:
          foreach (InputButton inputButton in this.inventorySlot12)
            inputListener.buttonNames.Add(inputButton.ToString());
          break;
      }
    }

    public void setDropDownToProperValue(OptionsDropDown dropDown)
    {
      switch (dropDown.whichOption)
      {
        case 6:
          int num = 0;
          foreach (DisplayMode supportedDisplayMode in GraphicsAdapter.DefaultAdapter.SupportedDisplayModes)
          {
            if (supportedDisplayMode.Width >= 1280)
            {
              dropDown.dropDownOptions.Add(supportedDisplayMode.Width.ToString() + " x " + (object) supportedDisplayMode.Height);
              dropDown.dropDownDisplayOptions.Add(supportedDisplayMode.Width.ToString() + " x " + (object) supportedDisplayMode.Height);
              if (supportedDisplayMode.Width == this.preferredResolutionX && supportedDisplayMode.Height == this.preferredResolutionY)
                dropDown.selectedOption = num;
              ++num;
            }
          }
          dropDown.greyedOut = !this.fullscreen || this.windowedBorderlessFullscreen;
          break;
        case 13:
          this.windowedBorderlessFullscreen = this.isCurrentlyWindowedBorderless();
          this.fullscreen = Game1.graphics.IsFullScreen && !this.windowedBorderlessFullscreen;
          dropDown.dropDownOptions.Add("Windowed");
          if (!this.windowedBorderlessFullscreen)
            dropDown.dropDownOptions.Add("Fullscreen");
          if (!this.fullscreen)
            dropDown.dropDownOptions.Add("Windowed Borderless");
          dropDown.dropDownDisplayOptions.Add(Game1.content.LoadString("Strings\\StringsFromCSFiles:Options.cs.4564"));
          if (!this.windowedBorderlessFullscreen)
            dropDown.dropDownDisplayOptions.Add(Game1.content.LoadString("Strings\\StringsFromCSFiles:Options.cs.4560"));
          if (!this.fullscreen)
            dropDown.dropDownDisplayOptions.Add(Game1.content.LoadString("Strings\\StringsFromCSFiles:Options.cs.4561"));
          if (Game1.graphics.IsFullScreen || this.windowedBorderlessFullscreen)
          {
            dropDown.selectedOption = 1;
            break;
          }
          dropDown.selectedOption = 0;
          break;
      }
    }

    public bool isCurrentlyWindowedBorderless()
    {
      Form form = Control.FromHandle(Program.gamePtr.Window.Handle).FindForm();
      if (!Game1.graphics.IsFullScreen)
        return form.FormBorderStyle == FormBorderStyle.None;
      return false;
    }

    public bool isCurrentlyFullscreen()
    {
      if (Game1.graphics.IsFullScreen)
        return !this.windowedBorderlessFullscreen;
      return false;
    }

    public bool isCurrentlyWindowed()
    {
      if (!this.isCurrentlyWindowedBorderless())
        return !this.isCurrentlyFullscreen();
      return false;
    }
  }
}
