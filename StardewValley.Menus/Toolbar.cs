using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StardewValley.Menus
{
	public class Toolbar : IClickableMenu
	{
		private List<ClickableComponent> buttons = new List<ClickableComponent>();

		private new int yPositionOnScreen;

		private string hoverTitle = "";

		private Item hoverItem;

		private float transparency = 1f;

		public string[] slotText = new string[12]
		{
			"1", "2", "3", "4", "5", "6", "7", "8", "9", "0",
			"-", "="
		};

		public Rectangle toolbarTextSource = new Rectangle(0, 256, 60, 60);

		public Toolbar()
			: base(Game1.uiViewport.Width / 2 - 384 - 64, Game1.uiViewport.Height, 896, 208)
		{
			for (int i = 0; i < 12; i++)
			{
				buttons.Add(new ClickableComponent(new Rectangle(Game1.uiViewport.Width / 2 - 384 + i * 64, yPositionOnScreen - 96 + 8, 64, 64), i.ToString() ?? ""));
			}
		}

		public override void receiveLeftClick(int x, int y, bool playSound = true)
		{
			if (Game1.player.UsingTool || Game1.IsChatting)
			{
				return;
			}
			foreach (ClickableComponent c in buttons)
			{
				if (c.containsPoint(x, y))
				{
					Game1.player.CurrentToolIndex = Convert.ToInt32(c.name);
					if (Game1.player.ActiveObject != null)
					{
						Game1.player.showCarrying();
						Game1.playSound("pickUpItem");
					}
					else
					{
						Game1.player.showNotCarrying();
						Game1.playSound("stoneStep");
					}
					break;
				}
			}
		}

		public override void receiveRightClick(int x, int y, bool playSound = true)
		{
		}

		public override void performHoverAction(int x, int y)
		{
			hoverItem = null;
			foreach (ClickableComponent c in buttons)
			{
				if (c.containsPoint(x, y))
				{
					int slotNumber = Convert.ToInt32(c.name);
					if (slotNumber < Game1.player.items.Count && Game1.player.items[slotNumber] != null)
					{
						c.scale = Math.Min(c.scale + 0.05f, 1.1f);
						hoverTitle = Game1.player.items[slotNumber].DisplayName;
						hoverItem = Game1.player.items[slotNumber];
					}
				}
				else
				{
					c.scale = Math.Max(c.scale - 0.025f, 1f);
				}
			}
		}

		public void shifted(bool right)
		{
			if (right)
			{
				for (int j = 0; j < buttons.Count; j++)
				{
					buttons[j].scale = 1f + (float)j * 0.03f;
				}
				return;
			}
			for (int i = buttons.Count - 1; i >= 0; i--)
			{
				buttons[i].scale = 1f + (float)(11 - i) * 0.03f;
			}
		}

		public override void update(GameTime time)
		{
		}

		public override void gameWindowSizeChanged(Rectangle oldBounds, Rectangle newBounds)
		{
			for (int i = 0; i < 12; i++)
			{
				buttons[i].bounds = new Rectangle(Game1.uiViewport.Width / 2 - 384 + i * 64, yPositionOnScreen - 96 + 8, 64, 64);
			}
		}

		public override bool isWithinBounds(int x, int y)
		{
			return new Rectangle(buttons.First().bounds.X, buttons.First().bounds.Y, buttons.Last().bounds.X - buttons.First().bounds.X + 64, 64).Contains(x, y);
		}

		public override void draw(SpriteBatch b)
		{
			if (Game1.activeClickableMenu != null)
			{
				return;
			}
			bool alignTop = false;
			Point playerGlobalPos = Game1.player.GetBoundingBox().Center;
			Vector2 playerLocalVec = Game1.GlobalToLocal(globalPosition: new Vector2(playerGlobalPos.X, playerGlobalPos.Y), viewport: Game1.viewport);
			if (Game1.options.pinToolbarToggle)
			{
				alignTop = false;
				transparency = Math.Min(1f, transparency + 0.075f);
				if (playerLocalVec.Y > (float)(Game1.viewport.Height - 192))
				{
					transparency = Math.Max(0.33f, transparency - 0.15f);
				}
			}
			else
			{
				alignTop = ((playerLocalVec.Y > (float)(Game1.viewport.Height / 2 + 64)) ? true : false);
				transparency = 1f;
			}
			int margin = Utility.makeSafeMarginY(8);
			int num = yPositionOnScreen;
			if (!alignTop)
			{
				yPositionOnScreen = Game1.uiViewport.Height;
				yPositionOnScreen += 8;
				yPositionOnScreen -= margin;
			}
			else
			{
				yPositionOnScreen = 112;
				yPositionOnScreen -= 8;
				yPositionOnScreen += margin;
			}
			if (num != yPositionOnScreen)
			{
				for (int k = 0; k < 12; k++)
				{
					buttons[k].bounds.Y = yPositionOnScreen - 96 + 8;
				}
			}
			IClickableMenu.drawTextureBox(b, Game1.menuTexture, toolbarTextSource, Game1.uiViewport.Width / 2 - 384 - 16, yPositionOnScreen - 96 - 8, 800, 96, Color.White * transparency, 1f, drawShadow: false);
			for (int j = 0; j < 12; j++)
			{
				Vector2 toDraw = new Vector2(Game1.uiViewport.Width / 2 - 384 + j * 64, yPositionOnScreen - 96 + 8);
				b.Draw(Game1.menuTexture, toDraw, Game1.getSourceRectForStandardTileSheet(Game1.menuTexture, (Game1.player.CurrentToolIndex == j) ? 56 : 10), Color.White * transparency);
				if (!Game1.options.gamepadControls)
				{
					b.DrawString(Game1.tinyFont, slotText[j], toDraw + new Vector2(4f, -8f), Color.DimGray * transparency);
				}
			}
			for (int i = 0; i < 12; i++)
			{
				buttons[i].scale = Math.Max(1f, buttons[i].scale - 0.025f);
				Vector2 toDraw2 = new Vector2(Game1.uiViewport.Width / 2 - 384 + i * 64, yPositionOnScreen - 96 + 8);
				if (Game1.player.items.Count > i && Game1.player.items.ElementAt(i) != null)
				{
					Game1.player.items[i].drawInMenu(b, toDraw2, (Game1.player.CurrentToolIndex == i) ? 0.9f : (buttons.ElementAt(i).scale * 0.8f), transparency, 0.88f);
				}
			}
			if (hoverItem != null)
			{
				IClickableMenu.drawToolTip(b, hoverItem.getDescription(), hoverItem.DisplayName, hoverItem);
				hoverItem = null;
			}
		}
	}
}
