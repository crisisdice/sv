using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StardewValley.Menus
{
	public class ExitPage : IClickableMenu
	{
		public ClickableComponent exitToTitle;

		public ClickableComponent exitToDesktop;

		public ExitPage(int x, int y, int width, int height)
			: base(x, y, width, height)
		{
			string exit_to_title_string = Game1.content.LoadString("Strings\\UI:ExitToTitle");
			if (!Game1.game1.IsMainInstance)
			{
				exit_to_title_string = Game1.content.LoadString("Strings\\UI:DropOutLocalMulti");
			}
			Vector2 exitPos = new Vector2(xPositionOnScreen + width / 2 - (int)((Game1.dialogueFont.MeasureString(exit_to_title_string).X + 64f) / 2f), yPositionOnScreen + 256 - 32);
			exitToTitle = new ClickableComponent(new Rectangle((int)exitPos.X, (int)exitPos.Y, (int)Game1.dialogueFont.MeasureString(exit_to_title_string).X + 64, 96), "", exit_to_title_string)
			{
				myID = 535,
				upNeighborID = 12347,
				downNeighborID = 536
			};
			exitPos = new Vector2(xPositionOnScreen + width / 2 - (int)((Game1.dialogueFont.MeasureString(Game1.content.LoadString("Strings\\UI:ExitToDesktop")).X + 64f) / 2f), yPositionOnScreen + 384 + 8 - 32);
			exitToDesktop = new ClickableComponent(new Rectangle((int)exitPos.X, (int)exitPos.Y, (int)Game1.dialogueFont.MeasureString(Game1.content.LoadString("Strings\\UI:ExitToDesktop")).X + 64, 96), "", Game1.content.LoadString("Strings\\UI:ExitToDesktop"))
			{
				myID = 536,
				upNeighborID = 535
			};
			if (!Game1.game1.IsMainInstance)
			{
				exitToDesktop.visible = false;
			}
			if (Game1.options.SnappyMenus)
			{
				populateClickableComponentList();
				snapToDefaultClickableComponent();
			}
		}

		public override void snapToDefaultClickableComponent()
		{
			currentlySnappedComponent = getComponentWithID(12347);
			snapCursorToCurrentSnappedComponent();
		}

		public override void receiveLeftClick(int x, int y, bool playSound = true)
		{
			if (Game1.conventionMode)
			{
				return;
			}
			if (exitToTitle.containsPoint(x, y) && exitToTitle.visible)
			{
				if (Game1.options.optionsDirty)
				{
					Game1.options.SaveDefaultOptions();
				}
				Game1.playSound("bigDeSelect");
				Game1.ExitToTitle();
			}
			if (exitToDesktop.containsPoint(x, y) && exitToDesktop.visible)
			{
				if (Game1.options.optionsDirty)
				{
					Game1.options.SaveDefaultOptions();
				}
				Game1.playSound("bigDeSelect");
				Game1.quit = true;
			}
		}

		public override void receiveRightClick(int x, int y, bool playSound = true)
		{
		}

		public override void performHoverAction(int x, int y)
		{
			if (exitToTitle.containsPoint(x, y) && exitToTitle.visible)
			{
				if (exitToTitle.scale == 0f)
				{
					Game1.playSound("Cowboy_gunshot");
				}
				exitToTitle.scale = 1f;
			}
			else
			{
				exitToTitle.scale = 0f;
			}
			if (exitToDesktop.containsPoint(x, y) && exitToDesktop.visible)
			{
				if (exitToDesktop.scale == 0f)
				{
					Game1.playSound("Cowboy_gunshot");
				}
				exitToDesktop.scale = 1f;
			}
			else
			{
				exitToDesktop.scale = 0f;
			}
		}

		public override void draw(SpriteBatch b)
		{
			if (exitToTitle.visible)
			{
				IClickableMenu.drawTextureBox(b, Game1.mouseCursors, new Rectangle(432, 439, 9, 9), exitToTitle.bounds.X, exitToTitle.bounds.Y, exitToTitle.bounds.Width, exitToTitle.bounds.Height, (exitToTitle.scale > 0f) ? Color.Wheat : Color.White, 4f);
				Utility.drawTextWithShadow(b, exitToTitle.label, Game1.dialogueFont, new Vector2(exitToTitle.bounds.Center.X, exitToTitle.bounds.Center.Y + 4) - Game1.dialogueFont.MeasureString(exitToTitle.label) / 2f, Game1.textColor, 1f, -1f, -1, -1, 0f);
			}
			if (exitToDesktop.visible)
			{
				IClickableMenu.drawTextureBox(b, Game1.mouseCursors, new Rectangle(432, 439, 9, 9), exitToDesktop.bounds.X, exitToDesktop.bounds.Y, exitToDesktop.bounds.Width, exitToDesktop.bounds.Height, (exitToDesktop.scale > 0f) ? Color.Wheat : Color.White, 4f);
				Utility.drawTextWithShadow(b, exitToDesktop.label, Game1.dialogueFont, new Vector2(exitToDesktop.bounds.Center.X, exitToDesktop.bounds.Center.Y + 4) - Game1.dialogueFont.MeasureString(exitToDesktop.label) / 2f, Game1.textColor, 1f, -1f, -1, -1, 0f);
			}
		}
	}
}
