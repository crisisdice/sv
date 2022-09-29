using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StardewValley.Menus
{
	public class SkillsPage : IClickableMenu
	{
		public const int region_special1 = 10201;

		public const int region_special2 = 10202;

		public const int region_special3 = 10203;

		public const int region_special4 = 10204;

		public const int region_special5 = 10205;

		public const int region_special6 = 10206;

		public const int region_special7 = 10207;

		public const int region_special8 = 10208;

		public const int region_special9 = 10209;

		public const int region_special_skullkey = 10210;

		public const int region_special_townkey = 10211;

		public const int region_skillArea1 = 0;

		public const int region_skillArea2 = 1;

		public const int region_skillArea3 = 2;

		public const int region_skillArea4 = 3;

		public const int region_skillArea5 = 4;

		public List<ClickableTextureComponent> skillBars = new List<ClickableTextureComponent>();

		public List<ClickableTextureComponent> skillAreas = new List<ClickableTextureComponent>();

		public List<ClickableTextureComponent> specialItems = new List<ClickableTextureComponent>();

		private string hoverText = "";

		private string hoverTitle = "";

		private int professionImage = -1;

		private int playerPanelIndex;

		private int playerPanelTimer;

		private Rectangle playerPanel;

		private int[] playerPanelFrames = new int[4] { 0, 1, 0, 2 };

		public SkillsPage(int x, int y, int width, int height)
			: base(x, y, width, height)
		{
			int baseX = xPositionOnScreen + IClickableMenu.spaceToClearSideBorder + 80;
			int baseY = yPositionOnScreen + IClickableMenu.spaceToClearTopBorder + (int)((float)height / 2f) + 80;
			playerPanel = new Rectangle(xPositionOnScreen + 64, yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder, 128, 192);
			if (Game1.player.canUnderstandDwarves)
			{
				specialItems.Add(new ClickableTextureComponent("", new Rectangle(baseX, baseY, 64, 64), null, Game1.content.LoadString("Strings\\StringsFromCSFiles:SkillsPage.cs.11587"), Game1.mouseCursors, new Rectangle(129, 320, 16, 16), 4f, drawShadow: true)
				{
					myID = 10201,
					rightNeighborID = -99998,
					leftNeighborID = -99998
				});
			}
			else
			{
				specialItems.Add(null);
			}
			if (Game1.player.hasRustyKey)
			{
				specialItems.Add(new ClickableTextureComponent("", new Rectangle(baseX + 68, baseY, 64, 64), null, Game1.content.LoadString("Strings\\StringsFromCSFiles:SkillsPage.cs.11588"), Game1.mouseCursors, new Rectangle(145, 320, 16, 16), 4f, drawShadow: true)
				{
					myID = 10202,
					rightNeighborID = -99998,
					leftNeighborID = -99998,
					upNeighborID = 4
				});
			}
			else
			{
				specialItems.Add(null);
			}
			if (Game1.player.hasClubCard)
			{
				specialItems.Add(new ClickableTextureComponent("", new Rectangle(baseX + 136, baseY, 64, 64), null, Game1.content.LoadString("Strings\\StringsFromCSFiles:SkillsPage.cs.11589"), Game1.mouseCursors, new Rectangle(161, 320, 16, 16), 4f, drawShadow: true)
				{
					myID = 10203,
					rightNeighborID = -99998,
					leftNeighborID = -99998,
					upNeighborID = 4
				});
			}
			else
			{
				specialItems.Add(null);
			}
			if (Game1.player.hasSpecialCharm)
			{
				specialItems.Add(new ClickableTextureComponent("", new Rectangle(baseX + 204, baseY, 64, 64), null, Game1.content.LoadString("Strings\\StringsFromCSFiles:SkillsPage.cs.11590"), Game1.mouseCursors, new Rectangle(176, 320, 16, 16), 4f, drawShadow: true)
				{
					myID = 10204,
					rightNeighborID = -99998,
					leftNeighborID = -99998,
					upNeighborID = 4
				});
			}
			else
			{
				specialItems.Add(null);
			}
			if (Game1.player.hasSkullKey)
			{
				specialItems.Add(new ClickableTextureComponent("", new Rectangle(baseX + 272, baseY, 64, 64), null, Game1.content.LoadString("Strings\\StringsFromCSFiles:SkillsPage.cs.11591"), Game1.mouseCursors, new Rectangle(192, 320, 16, 16), 4f, drawShadow: true)
				{
					myID = 10210,
					rightNeighborID = -99998,
					leftNeighborID = -99998,
					upNeighborID = 4
				});
			}
			else
			{
				specialItems.Add(null);
			}
			if (Game1.player.hasMagnifyingGlass)
			{
				specialItems.Add(new ClickableTextureComponent("", new Rectangle(baseX + 340, baseY, 64, 64), null, Game1.content.LoadString("Strings\\StringsFromCSFiles:SkillsPage.cs.magnifyingglass"), Game1.mouseCursors, new Rectangle(208, 320, 16, 16), 4f, drawShadow: true)
				{
					myID = 10205,
					rightNeighborID = -99998,
					leftNeighborID = -99998,
					upNeighborID = 4
				});
			}
			else
			{
				specialItems.Add(null);
			}
			if (Game1.player.hasDarkTalisman)
			{
				specialItems.Add(new ClickableTextureComponent("", new Rectangle(baseX + 408, baseY, 64, 64), null, Game1.content.LoadString("Strings\\Objects:DarkTalisman"), Game1.mouseCursors, new Rectangle(225, 320, 16, 16), 4f, drawShadow: true)
				{
					myID = 10206,
					rightNeighborID = -99998,
					leftNeighborID = -99998,
					upNeighborID = 4
				});
			}
			else
			{
				specialItems.Add(null);
			}
			if (Game1.player.hasMagicInk)
			{
				specialItems.Add(new ClickableTextureComponent("", new Rectangle(baseX + 476, baseY, 64, 64), null, Game1.content.LoadString("Strings\\Objects:MagicInk"), Game1.mouseCursors, new Rectangle(241, 320, 16, 16), 4f, drawShadow: true)
				{
					myID = 10207,
					rightNeighborID = -99998,
					leftNeighborID = -99998,
					upNeighborID = 4
				});
			}
			else
			{
				specialItems.Add(null);
			}
			if (Game1.player.eventsSeen.Contains(2120303))
			{
				specialItems.Add(new ClickableTextureComponent("", new Rectangle(baseX + 544, baseY, 64, 64), null, Game1.content.LoadString("Strings\\Objects:BearPaw"), Game1.mouseCursors, new Rectangle(192, 336, 16, 16), 4f, drawShadow: true)
				{
					myID = 10208,
					rightNeighborID = -99998,
					leftNeighborID = -99998,
					upNeighborID = 4
				});
			}
			else
			{
				specialItems.Add(null);
			}
			if (Game1.player.eventsSeen.Contains(3910979))
			{
				specialItems.Add(new ClickableTextureComponent("", new Rectangle(baseX + 612, baseY, 64, 64), null, Game1.content.LoadString("Strings\\Objects:SpringOnionBugs"), Game1.mouseCursors, new Rectangle(208, 336, 16, 16), 4f, drawShadow: true)
				{
					myID = 10209,
					rightNeighborID = -99998,
					leftNeighborID = -99998,
					upNeighborID = 4
				});
			}
			else
			{
				specialItems.Add(null);
			}
			if (Game1.player.HasTownKey)
			{
				specialItems.Add(new ClickableTextureComponent("", new Rectangle(baseX, baseY, 64, 64), null, Game1.content.LoadString("Strings\\StringsFromCSFiles:KeyToTheTown"), Game1.objectSpriteSheet, Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, 912, 16, 16), 4f, drawShadow: true)
				{
					myID = 10211,
					rightNeighborID = -99998,
					leftNeighborID = -99998,
					upNeighborID = 4
				});
			}
			else
			{
				specialItems.Add(null);
			}
			int spacing = 680 / specialItems.Count;
			for (int l = 0; l < specialItems.Count; l++)
			{
				if (specialItems[l] != null)
				{
					specialItems[l].bounds.X = baseX + l * spacing;
				}
			}
			ClickableComponent.SetUpNeighbors(specialItems, 4);
			ClickableComponent.ChainNeighborsLeftRight(specialItems);
			int addedX = 0;
			int drawX = ((LocalizedContentManager.CurrentLanguageCode == LocalizedContentManager.LanguageCode.ru || LocalizedContentManager.CurrentLanguageCode == LocalizedContentManager.LanguageCode.it) ? (xPositionOnScreen + width - 448 - 48 + 4) : (xPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder + 256 - 4));
			int drawY = yPositionOnScreen + IClickableMenu.spaceToClearTopBorder + IClickableMenu.borderWidth - 12;
			for (int k = 4; k < 10; k += 5)
			{
				for (int m = 0; m < 5; m++)
				{
					string professionBlurb = "";
					string professionTitle = "";
					bool drawRed = false;
					int professionNumber = -1;
					switch (m)
					{
					case 0:
						drawRed = Game1.player.FarmingLevel > k;
						professionNumber = Game1.player.getProfessionForSkill(0, k + 1);
						parseProfessionDescription(ref professionBlurb, ref professionTitle, LevelUpMenu.getProfessionDescription(professionNumber));
						break;
					case 1:
						drawRed = Game1.player.MiningLevel > k;
						professionNumber = Game1.player.getProfessionForSkill(3, k + 1);
						parseProfessionDescription(ref professionBlurb, ref professionTitle, LevelUpMenu.getProfessionDescription(professionNumber));
						break;
					case 2:
						drawRed = Game1.player.ForagingLevel > k;
						professionNumber = Game1.player.getProfessionForSkill(2, k + 1);
						parseProfessionDescription(ref professionBlurb, ref professionTitle, LevelUpMenu.getProfessionDescription(professionNumber));
						break;
					case 3:
						drawRed = Game1.player.FishingLevel > k;
						professionNumber = Game1.player.getProfessionForSkill(1, k + 1);
						parseProfessionDescription(ref professionBlurb, ref professionTitle, LevelUpMenu.getProfessionDescription(professionNumber));
						break;
					case 4:
						drawRed = Game1.player.CombatLevel > k;
						professionNumber = Game1.player.getProfessionForSkill(4, k + 1);
						parseProfessionDescription(ref professionBlurb, ref professionTitle, LevelUpMenu.getProfessionDescription(professionNumber));
						break;
					case 5:
						drawRed = Game1.player.LuckLevel > k;
						professionNumber = Game1.player.getProfessionForSkill(5, k + 1);
						parseProfessionDescription(ref professionBlurb, ref professionTitle, LevelUpMenu.getProfessionDescription(professionNumber));
						break;
					}
					if (drawRed && (k + 1) % 5 == 0)
					{
						skillBars.Add(new ClickableTextureComponent(professionNumber.ToString() ?? "", new Rectangle(addedX + drawX - 4 + k * 36, drawY + m * 56, 56, 36), null, professionBlurb, Game1.mouseCursors, new Rectangle(159, 338, 14, 9), 4f, drawShadow: true)
						{
							myID = ((k + 1 == 5) ? (100 + m) : (200 + m)),
							leftNeighborID = ((k + 1 == 5) ? m : (100 + m)),
							rightNeighborID = ((k + 1 == 5) ? (200 + m) : (-1)),
							downNeighborID = -99998
						});
					}
				}
				addedX += 24;
			}
			for (int j = 0; j < skillBars.Count; j++)
			{
				if (j < skillBars.Count - 1 && Math.Abs(skillBars[j + 1].myID - skillBars[j].myID) < 50)
				{
					skillBars[j].downNeighborID = skillBars[j + 1].myID;
					skillBars[j + 1].upNeighborID = skillBars[j].myID;
				}
			}
			if (skillBars.Count > 1 && skillBars.Last().myID >= 200 && skillBars[skillBars.Count - 2].myID >= 200)
			{
				skillBars.Last().upNeighborID = skillBars[skillBars.Count - 2].myID;
			}
			for (int i = 0; i < 5; i++)
			{
				int index = i;
				switch (index)
				{
				case 1:
					index = 3;
					break;
				case 3:
					index = 1;
					break;
				}
				string text = "";
				switch (index)
				{
				case 0:
					if (Game1.player.FarmingLevel > 0)
					{
						text = Game1.content.LoadString("Strings\\StringsFromCSFiles:SkillsPage.cs.11592", Game1.player.FarmingLevel) + Environment.NewLine + Game1.content.LoadString("Strings\\StringsFromCSFiles:SkillsPage.cs.11594", Game1.player.FarmingLevel);
					}
					break;
				case 2:
					if (Game1.player.ForagingLevel > 0)
					{
						text = Game1.content.LoadString("Strings\\StringsFromCSFiles:SkillsPage.cs.11596", Game1.player.ForagingLevel);
					}
					break;
				case 1:
					if (Game1.player.FishingLevel > 0)
					{
						text = Game1.content.LoadString("Strings\\StringsFromCSFiles:SkillsPage.cs.11598", Game1.player.FishingLevel);
					}
					break;
				case 3:
					if (Game1.player.MiningLevel > 0)
					{
						text = Game1.content.LoadString("Strings\\StringsFromCSFiles:SkillsPage.cs.11600", Game1.player.MiningLevel);
					}
					break;
				case 4:
					if (Game1.player.CombatLevel > 0)
					{
						text = Game1.content.LoadString("Strings\\StringsFromCSFiles:SkillsPage.cs.11602", Game1.player.CombatLevel * 5);
					}
					break;
				}
				skillAreas.Add(new ClickableTextureComponent(index.ToString() ?? "", new Rectangle(drawX - 128 - 48, drawY + i * 56, 148, 36), index.ToString() ?? "", text, null, Rectangle.Empty, 1f)
				{
					myID = i,
					downNeighborID = ((i < 4) ? (i + 1) : (-99998)),
					upNeighborID = ((i > 0) ? (i - 1) : 12341),
					rightNeighborID = 100 + i
				});
			}
		}

		private void parseProfessionDescription(ref string professionBlurb, ref string professionTitle, List<string> professionDescription)
		{
			if (professionDescription.Count <= 0)
			{
				return;
			}
			professionTitle = professionDescription[0];
			for (int i = 1; i < professionDescription.Count; i++)
			{
				professionBlurb += professionDescription[i];
				if (i < professionDescription.Count - 1)
				{
					professionBlurb += Environment.NewLine;
				}
			}
		}

		public override void snapToDefaultClickableComponent()
		{
			currentlySnappedComponent = ((skillAreas.Count > 0) ? getComponentWithID(0) : null);
			snapCursorToCurrentSnappedComponent();
		}

		public override void receiveLeftClick(int x, int y, bool playSound = true)
		{
		}

		public override void receiveRightClick(int x, int y, bool playSound = true)
		{
		}

		public override void performHoverAction(int x, int y)
		{
			hoverText = "";
			hoverTitle = "";
			professionImage = -1;
			foreach (ClickableTextureComponent c3 in specialItems)
			{
				if (c3 != null && c3.containsPoint(x, y))
				{
					hoverText = c3.hoverText;
					break;
				}
			}
			foreach (ClickableTextureComponent c2 in skillBars)
			{
				c2.scale = 4f;
				if (c2.containsPoint(x, y) && c2.hoverText.Length > 0 && !c2.name.Equals("-1"))
				{
					hoverText = c2.hoverText;
					hoverTitle = LevelUpMenu.getProfessionTitleFromNumber(Convert.ToInt32(c2.name));
					professionImage = Convert.ToInt32(c2.name);
					c2.scale = 0f;
				}
			}
			foreach (ClickableTextureComponent c in skillAreas)
			{
				if (c.containsPoint(x, y) && c.hoverText.Length > 0)
				{
					hoverText = c.hoverText;
					hoverTitle = Farmer.getSkillDisplayNameFromIndex(Convert.ToInt32(c.name));
					break;
				}
			}
			if (playerPanel.Contains(x, y))
			{
				playerPanelTimer -= Game1.currentGameTime.ElapsedGameTime.Milliseconds;
				if (playerPanelTimer <= 0)
				{
					playerPanelIndex = (playerPanelIndex + 1) % 4;
					playerPanelTimer = 150;
				}
			}
			else
			{
				playerPanelIndex = 0;
			}
		}

		public override void draw(SpriteBatch b)
		{
			int x = xPositionOnScreen + 64 - 12;
			int y = yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder;
			b.Draw((Game1.timeOfDay >= 1900) ? Game1.nightbg : Game1.daybg, new Vector2(x, y), Color.White);
			FarmerRenderer.isDrawingForUI = true;
			Game1.player.FarmerRenderer.draw(b, new FarmerSprite.AnimationFrame(Game1.player.bathingClothes ? 108 : playerPanelFrames[playerPanelIndex], 0, secondaryArm: false, flip: false), Game1.player.bathingClothes ? 108 : playerPanelFrames[playerPanelIndex], new Rectangle(playerPanelFrames[playerPanelIndex] * 16, Game1.player.bathingClothes ? 576 : 0, 16, 32), new Vector2(x + 32, y + 32), Vector2.Zero, 0.8f, 2, Color.White, 0f, 1f, Game1.player);
			if (Game1.timeOfDay >= 1900)
			{
				Game1.player.FarmerRenderer.draw(b, new FarmerSprite.AnimationFrame(playerPanelFrames[playerPanelIndex], 0, secondaryArm: false, flip: false), playerPanelFrames[playerPanelIndex], new Rectangle(playerPanelFrames[playerPanelIndex] * 16, 0, 16, 32), new Vector2(x + 32, y + 32), Vector2.Zero, 0.8f, 2, Color.DarkBlue * 0.3f, 0f, 1f, Game1.player);
			}
			FarmerRenderer.isDrawingForUI = false;
			b.DrawString(Game1.smallFont, Game1.player.Name, new Vector2((float)(x + 64) - Game1.smallFont.MeasureString(Game1.player.Name).X / 2f, y + 192 + 4), Game1.textColor);
			b.DrawString(Game1.smallFont, Game1.player.getTitle(), new Vector2((float)(x + 64) - Game1.smallFont.MeasureString(Game1.player.getTitle()).X / 2f, y + 256 - 32), Game1.textColor);
			x = ((LocalizedContentManager.CurrentLanguageCode == LocalizedContentManager.LanguageCode.ru || LocalizedContentManager.CurrentLanguageCode == LocalizedContentManager.LanguageCode.it) ? (xPositionOnScreen + width - 448 - 48) : (xPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder + 256 - 8));
			y = yPositionOnScreen + IClickableMenu.spaceToClearTopBorder + IClickableMenu.borderWidth - 8;
			int addedX = 0;
			for (int i = 0; i < 10; i++)
			{
				for (int j = 0; j < 5; j++)
				{
					bool drawRed = false;
					bool addedSkill = false;
					string skill = "";
					int skillLevel = 0;
					Rectangle iconSource = Rectangle.Empty;
					switch (j)
					{
					case 0:
						drawRed = Game1.player.FarmingLevel > i;
						if (i == 0)
						{
							skill = Game1.content.LoadString("Strings\\StringsFromCSFiles:SkillsPage.cs.11604");
						}
						skillLevel = Game1.player.FarmingLevel;
						addedSkill = (int)Game1.player.addedFarmingLevel > 0;
						iconSource = new Rectangle(10, 428, 10, 10);
						break;
					case 1:
						drawRed = Game1.player.MiningLevel > i;
						if (i == 0)
						{
							skill = Game1.content.LoadString("Strings\\StringsFromCSFiles:SkillsPage.cs.11605");
						}
						skillLevel = Game1.player.MiningLevel;
						addedSkill = (int)Game1.player.addedMiningLevel > 0;
						iconSource = new Rectangle(30, 428, 10, 10);
						break;
					case 2:
						drawRed = Game1.player.ForagingLevel > i;
						if (i == 0)
						{
							skill = Game1.content.LoadString("Strings\\StringsFromCSFiles:SkillsPage.cs.11606");
						}
						skillLevel = Game1.player.ForagingLevel;
						addedSkill = (int)Game1.player.addedForagingLevel > 0;
						iconSource = new Rectangle(60, 428, 10, 10);
						break;
					case 3:
						drawRed = Game1.player.FishingLevel > i;
						if (i == 0)
						{
							skill = Game1.content.LoadString("Strings\\StringsFromCSFiles:SkillsPage.cs.11607");
						}
						skillLevel = Game1.player.FishingLevel;
						addedSkill = (int)Game1.player.addedFishingLevel > 0 || (Game1.player.CurrentTool != null && Game1.player.CurrentTool.hasEnchantmentOfType<MasterEnchantment>());
						iconSource = new Rectangle(20, 428, 10, 10);
						break;
					case 4:
						drawRed = Game1.player.CombatLevel > i;
						if (i == 0)
						{
							skill = Game1.content.LoadString("Strings\\StringsFromCSFiles:SkillsPage.cs.11608");
						}
						skillLevel = Game1.player.CombatLevel;
						addedSkill = (int)Game1.player.addedCombatLevel > 0;
						iconSource = new Rectangle(120, 428, 10, 10);
						break;
					case 5:
						drawRed = Game1.player.LuckLevel > i;
						if (i == 0)
						{
							skill = Game1.content.LoadString("Strings\\StringsFromCSFiles:SkillsPage.cs.11609");
						}
						skillLevel = Game1.player.LuckLevel;
						addedSkill = (int)Game1.player.addedLuckLevel > 0;
						iconSource = new Rectangle(50, 428, 10, 10);
						break;
					}
					if (!skill.Equals(""))
					{
						b.DrawString(Game1.smallFont, skill, new Vector2((float)x - Game1.smallFont.MeasureString(skill).X + 4f - 64f, y + 4 + j * 56), Game1.textColor);
						b.Draw(Game1.mouseCursors, new Vector2(x - 56, y + j * 56), iconSource, Color.Black * 0.3f, 0f, Vector2.Zero, 4f, SpriteEffects.None, 0.85f);
						b.Draw(Game1.mouseCursors, new Vector2(x - 52, y - 4 + j * 56), iconSource, Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, 0.87f);
					}
					if (!drawRed && (i + 1) % 5 == 0)
					{
						b.Draw(Game1.mouseCursors, new Vector2(addedX + x - 4 + i * 36, y + j * 56), new Rectangle(145, 338, 14, 9), Color.Black * 0.35f, 0f, Vector2.Zero, 4f, SpriteEffects.None, 0.87f);
						b.Draw(Game1.mouseCursors, new Vector2(addedX + x + i * 36, y - 4 + j * 56), new Rectangle(145 + (drawRed ? 14 : 0), 338, 14, 9), Color.White * (drawRed ? 1f : 0.65f), 0f, Vector2.Zero, 4f, SpriteEffects.None, 0.87f);
					}
					else if ((i + 1) % 5 != 0)
					{
						b.Draw(Game1.mouseCursors, new Vector2(addedX + x - 4 + i * 36, y + j * 56), new Rectangle(129, 338, 8, 9), Color.Black * 0.35f, 0f, Vector2.Zero, 4f, SpriteEffects.None, 0.85f);
						b.Draw(Game1.mouseCursors, new Vector2(addedX + x + i * 36, y - 4 + j * 56), new Rectangle(129 + (drawRed ? 8 : 0), 338, 8, 9), Color.White * (drawRed ? 1f : 0.65f), 0f, Vector2.Zero, 4f, SpriteEffects.None, 0.87f);
					}
					if (i == 9)
					{
						NumberSprite.draw(skillLevel, b, new Vector2(addedX + x + (i + 2) * 36 + 12 + ((skillLevel >= 10) ? 12 : 0), y + 16 + j * 56), Color.Black * 0.35f, 1f, 0.85f, 1f, 0);
						NumberSprite.draw(skillLevel, b, new Vector2(addedX + x + (i + 2) * 36 + 16 + ((skillLevel >= 10) ? 12 : 0), y + 12 + j * 56), (addedSkill ? Color.LightGreen : Color.SandyBrown) * ((skillLevel == 0) ? 0.75f : 1f), 1f, 0.87f, 1f, 0);
					}
				}
				if ((i + 1) % 5 == 0)
				{
					addedX += 24;
				}
			}
			foreach (ClickableTextureComponent skillBar in skillBars)
			{
				skillBar.draw(b);
			}
			foreach (ClickableTextureComponent c in skillBars)
			{
				if (c.scale == 0f)
				{
					IClickableMenu.drawTextureBox(b, c.bounds.X - 16 - 8, c.bounds.Y - 16 - 16, 96, 96, Color.White);
					b.Draw(Game1.mouseCursors, new Vector2(c.bounds.X - 8, c.bounds.Y - 32 + 16), new Rectangle(professionImage % 6 * 16, 624 + professionImage / 6 * 16, 16, 16), Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, 1f);
				}
			}
			x = xPositionOnScreen + IClickableMenu.spaceToClearSideBorder + 32 + 16;
			y = yPositionOnScreen + IClickableMenu.spaceToClearTopBorder + 320 - 8;
			if ((int)Game1.netWorldState.Value.GoldenWalnuts > 0)
			{
				b.Draw(Game1.objectSpriteSheet, new Vector2(x, y), Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, 73, 16, 16), Color.White, 0f, Vector2.Zero, 2f, SpriteEffects.None, 0f);
				x += 48;
				b.DrawString(Game1.smallFont, Game1.netWorldState.Value.GoldenWalnuts?.ToString() ?? "", new Vector2(x, y), Game1.textColor);
				x += 64;
			}
			if (Game1.player.QiGems > 0)
			{
				b.Draw(Game1.objectSpriteSheet, new Vector2(x, y), Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, 858, 16, 16), Color.White, 0f, Vector2.Zero, 2f, SpriteEffects.None, 0f);
				x += 48;
				b.DrawString(Game1.smallFont, Game1.player.QiGems.ToString() ?? "", new Vector2(x, y), Game1.textColor);
				x += 64;
			}
			Game1.drawDialogueBox(xPositionOnScreen + IClickableMenu.spaceToClearSideBorder + 32, yPositionOnScreen + IClickableMenu.spaceToClearTopBorder + (int)((float)height / 2f) - 32, width - 64 - IClickableMenu.spaceToClearSideBorder * 2, height / 4 + 64, speaker: false, drawOnlyBox: true);
			drawBorderLabel(b, Game1.content.LoadString("Strings\\StringsFromCSFiles:SkillsPage.cs.11610"), Game1.smallFont, xPositionOnScreen + IClickableMenu.spaceToClearSideBorder + 96, yPositionOnScreen + IClickableMenu.spaceToClearTopBorder + (int)((float)height / 2f) - 32);
			foreach (ClickableTextureComponent specialItem in specialItems)
			{
				specialItem?.draw(b);
			}
			if (hoverText.Length > 0)
			{
				IClickableMenu.drawHoverText(b, hoverText, Game1.smallFont, 0, 0, -1, (hoverTitle.Length > 0) ? hoverTitle : null);
			}
		}
	}
}
