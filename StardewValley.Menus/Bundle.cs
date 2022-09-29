using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.Locations;
using StardewValley.Objects;

namespace StardewValley.Menus
{
	public class Bundle : ClickableComponent
	{
		public const float shakeRate = (float)Math.PI / 200f;

		public const float shakeDecayRate = 0.0030679617f;

		public const int Color_Green = 0;

		public const int Color_Purple = 1;

		public const int Color_Orange = 2;

		public const int Color_Yellow = 3;

		public const int Color_Red = 4;

		public const int Color_Blue = 5;

		public const int Color_Teal = 6;

		public const float DefaultShakeForce = (float)Math.PI * 3f / 128f;

		public string rewardDescription;

		public List<BundleIngredientDescription> ingredients;

		public int bundleColor;

		public int numberOfIngredientSlots;

		public int bundleIndex;

		public int completionTimer;

		public bool complete;

		public bool depositsAllowed = true;

		public Texture2D bundleTextureOverride;

		public int bundleTextureIndexOverride = -1;

		public TemporaryAnimatedSprite sprite;

		private float maxShake;

		private bool shakeLeft;

		public Bundle(int bundleIndex, string rawBundleInfo, bool[] completedIngredientsList, Point position, string textureName, JunimoNoteMenu menu)
			: base(new Rectangle(position.X, position.Y, 64, 64), "")
		{
			if (menu.fromGameMenu)
			{
				depositsAllowed = false;
			}
			this.bundleIndex = bundleIndex;
			string[] split = rawBundleInfo.Split('/');
			name = split[0];
			label = split[0];
			if (LocalizedContentManager.CurrentLanguageCode != 0)
			{
				label = split[^1];
			}
			rewardDescription = split[1];
			if (split.Length > 5 && (LocalizedContentManager.CurrentLanguageCode == LocalizedContentManager.LanguageCode.en || split.Length > 6))
			{
				string bundle_image_override = split[5];
				try
				{
					if (bundle_image_override.IndexOf(':') >= 0)
					{
						string[] bundle_image_parts = bundle_image_override.Split(':');
						bundleTextureOverride = Game1.content.Load<Texture2D>(bundle_image_parts[0]);
						bundleTextureIndexOverride = int.Parse(bundle_image_parts[1]);
					}
					else
					{
						bundleTextureIndexOverride = int.Parse(bundle_image_override);
					}
				}
				catch (Exception)
				{
					bundleTextureOverride = null;
					bundleTextureIndexOverride = -1;
				}
			}
			string[] ingredientsSplit = split[2].Split(' ');
			complete = true;
			ingredients = new List<BundleIngredientDescription>();
			int tally = 0;
			for (int i = 0; i < ingredientsSplit.Length; i += 3)
			{
				ingredients.Add(new BundleIngredientDescription(Convert.ToInt32(ingredientsSplit[i]), Convert.ToInt32(ingredientsSplit[i + 1]), Convert.ToInt32(ingredientsSplit[i + 2]), completedIngredientsList[i / 3]));
				if (!completedIngredientsList[i / 3])
				{
					complete = false;
				}
				else
				{
					tally++;
				}
			}
			bundleColor = Convert.ToInt32(split[3]);
			int count = 4;
			if (LocalizedContentManager.CurrentLanguageCode != 0)
			{
				count = 5;
			}
			numberOfIngredientSlots = ((split.Length > count) ? Convert.ToInt32(split[4]) : ingredients.Count());
			if (tally >= numberOfIngredientSlots)
			{
				complete = true;
			}
			sprite = new TemporaryAnimatedSprite(textureName, new Rectangle(bundleColor * 256 % 512, 244 + bundleColor * 256 / 512 * 16, 16, 16), 70f, 3, 99999, new Vector2(bounds.X, bounds.Y), flicker: false, flipped: false, 0.8f, 0f, Color.White, 4f, 0f, 0f, 0f)
			{
				pingPong = true
			};
			sprite.paused = true;
			sprite.sourceRect.X += sprite.sourceRect.Width;
			if (name.ToLower().Contains(Game1.currentSeason) && !complete)
			{
				shake();
			}
			if (complete)
			{
				completionAnimation(menu, playSound: false);
			}
		}

		public Item getReward()
		{
			return Utility.getItemFromStandardTextDescription(rewardDescription, Game1.player);
		}

		public void shake(float force = (float)Math.PI * 3f / 128f)
		{
			if (sprite.paused)
			{
				maxShake = force;
			}
		}

		public void shake(int extraInfo)
		{
			maxShake = (float)Math.PI * 3f / 128f;
			if (extraInfo == 1)
			{
				Game1.playSound("leafrustle");
				JunimoNoteMenu.tempSprites.Add(new TemporaryAnimatedSprite(50, sprite.position, getColorFromColorIndex(bundleColor))
				{
					motion = new Vector2(-1f, 0.5f),
					acceleration = new Vector2(0f, 0.02f)
				});
				JunimoNoteMenu.tempSprites.Last().sourceRect.Y++;
				JunimoNoteMenu.tempSprites.Last().sourceRect.Height--;
				JunimoNoteMenu.tempSprites.Add(new TemporaryAnimatedSprite(50, sprite.position, getColorFromColorIndex(bundleColor))
				{
					motion = new Vector2(1f, 0.5f),
					acceleration = new Vector2(0f, 0.02f),
					flipped = true,
					delayBeforeAnimationStart = 50
				});
				JunimoNoteMenu.tempSprites.Last().sourceRect.Y++;
				JunimoNoteMenu.tempSprites.Last().sourceRect.Height--;
			}
		}

		public void shakeAndAllowClicking(int extraInfo)
		{
			maxShake = (float)Math.PI * 3f / 128f;
			JunimoNoteMenu.canClick = true;
		}

		public void tryHoverAction(int x, int y)
		{
			if (bounds.Contains(x, y) && !complete)
			{
				sprite.paused = false;
				JunimoNoteMenu.hoverText = Game1.content.LoadString("Strings\\UI:JunimoNote_BundleName", label);
			}
			else if (!complete)
			{
				sprite.reset();
				sprite.sourceRect.X += sprite.sourceRect.Width;
				sprite.paused = true;
			}
		}

		public bool IsValidItemForThisIngredientDescription(Item item, BundleIngredientDescription ingredient)
		{
			if (item is Object)
			{
				Object o = item as Object;
				if (!ingredient.completed && ingredient.quality <= (int)o.quality)
				{
					if (ingredient.index < 0)
					{
						if (item.Category == ingredient.index)
						{
							return true;
						}
					}
					else if (ingredient.index == (int)item.parentSheetIndex)
					{
						return true;
					}
				}
				return false;
			}
			return false;
		}

		public int GetBundleIngredientDescriptionIndexForItem(Item item)
		{
			if (item is Object)
			{
				Object o = item as Object;
				for (int i = 0; i < ingredients.Count; i++)
				{
					if (IsValidItemForThisIngredientDescription(o, ingredients[i]))
					{
						return i;
					}
				}
				return -1;
			}
			return -1;
		}

		public bool canAcceptThisItem(Item item, ClickableTextureComponent slot)
		{
			return canAcceptThisItem(item, slot, ignore_stack_count: false);
		}

		public bool canAcceptThisItem(Item item, ClickableTextureComponent slot, bool ignore_stack_count = false)
		{
			if (!depositsAllowed)
			{
				return false;
			}
			if (item is Object)
			{
				Object o = item as Object;
				for (int i = 0; i < ingredients.Count; i++)
				{
					if (IsValidItemForThisIngredientDescription(o, ingredients[i]) && (ignore_stack_count || ingredients[i].stack <= item.Stack) && (slot == null || slot.item == null))
					{
						return true;
					}
				}
				return false;
			}
			return false;
		}

		public Item tryToDepositThisItem(Item item, ClickableTextureComponent slot, string noteTextureName)
		{
			if (!depositsAllowed)
			{
				if (Game1.player.hasCompletedCommunityCenter())
				{
					Game1.showRedMessage(Game1.content.LoadString("Strings\\UI:JunimoNote_MustBeAtAJM"));
				}
				else
				{
					Game1.showRedMessage(Game1.content.LoadString("Strings\\UI:JunimoNote_MustBeAtCC"));
				}
				return item;
			}
			if (!(item is Object) || item is Furniture)
			{
				return item;
			}
			for (int i = 0; i < ingredients.Count; i++)
			{
				if (IsValidItemForThisIngredientDescription(item, ingredients[i]) && slot.item == null)
				{
					item.Stack -= ingredients[i].stack;
					ingredients[i] = new BundleIngredientDescription(ingredients[i].index, ingredients[i].stack, ingredients[i].quality, completed: true);
					ingredientDepositAnimation(slot, noteTextureName);
					int index = ingredients[i].index;
					if (index < 0)
					{
						index = JunimoNoteMenu.GetObjectOrCategoryIndex(index);
					}
					slot.item = new Object(index, ingredients[i].stack, isRecipe: false, -1, ingredients[i].quality);
					Game1.playSound("newArtifact");
					(Game1.getLocationFromName("CommunityCenter") as CommunityCenter).bundles.FieldDict[bundleIndex][i] = true;
					slot.sourceRect.X = 512;
					slot.sourceRect.Y = 244;
					Game1.multiplayer.globalChatInfoMessage("BundleDonate", Game1.player.displayName, slot.item.DisplayName);
					break;
				}
			}
			if (item.Stack > 0)
			{
				return item;
			}
			return null;
		}

		public bool couldThisItemBeDeposited(Item item)
		{
			if (!(item is Object) || item is Furniture)
			{
				return false;
			}
			for (int i = 0; i < ingredients.Count; i++)
			{
				if (!IsValidItemForThisIngredientDescription(item, ingredients[i]))
				{
					return true;
				}
			}
			return false;
		}

		public void ingredientDepositAnimation(ClickableTextureComponent slot, string noteTextureName, bool skipAnimation = false)
		{
			TemporaryAnimatedSprite t = new TemporaryAnimatedSprite(noteTextureName, new Rectangle(530, 244, 18, 18), 50f, 6, 1, new Vector2(slot.bounds.X, slot.bounds.Y), flicker: false, flipped: false, 0.88f, 0f, Color.White, 4f, 0f, 0f, 0f, local: true)
			{
				holdLastFrame = true,
				endSound = "cowboy_monsterhit"
			};
			if (skipAnimation)
			{
				t.sourceRect.Offset(t.sourceRect.Width * 5, 0);
				t.sourceRectStartingPos = new Vector2(t.sourceRect.X, t.sourceRect.Y);
				t.animationLength = 1;
			}
			JunimoNoteMenu.tempSprites.Add(t);
		}

		public bool canBeClicked()
		{
			return !complete;
		}

		public void completionAnimation(JunimoNoteMenu menu, bool playSound = true, int delay = 0)
		{
			if (delay <= 0)
			{
				completionAnimation(playSound);
			}
			else
			{
				completionTimer = delay;
			}
		}

		private void completionAnimation(bool playSound = true)
		{
			if (Game1.activeClickableMenu != null && Game1.activeClickableMenu is JunimoNoteMenu)
			{
				(Game1.activeClickableMenu as JunimoNoteMenu).takeDownBundleSpecificPage();
			}
			sprite.pingPong = false;
			sprite.paused = false;
			sprite.sourceRect.X = (int)sprite.sourceRectStartingPos.X;
			sprite.sourceRect.X += sprite.sourceRect.Width;
			sprite.animationLength = 15;
			sprite.interval = 50f;
			sprite.totalNumberOfLoops = 0;
			sprite.holdLastFrame = true;
			sprite.endFunction = shake;
			sprite.extraInfoForEndBehavior = 1;
			if (complete)
			{
				sprite.sourceRect.X += sprite.sourceRect.Width * 14;
				sprite.sourceRectStartingPos = new Vector2(sprite.sourceRect.X, sprite.sourceRect.Y);
				sprite.currentParentTileIndex = 14;
				sprite.interval = 0f;
				sprite.animationLength = 1;
				sprite.extraInfoForEndBehavior = 0;
			}
			else
			{
				if (playSound)
				{
					Game1.playSound("dwop");
				}
				bounds.Inflate(64, 64);
				JunimoNoteMenu.tempSprites.AddRange(Utility.sparkleWithinArea(bounds, 8, getColorFromColorIndex(bundleColor) * 0.5f));
				bounds.Inflate(-64, -64);
			}
			complete = true;
		}

		public void update(GameTime time)
		{
			sprite.update(time);
			if (completionTimer > 0 && JunimoNoteMenu.screenSwipe == null)
			{
				completionTimer -= time.ElapsedGameTime.Milliseconds;
				if (completionTimer <= 0)
				{
					completionAnimation();
				}
			}
			if (Game1.random.NextDouble() < 0.005 && (complete || name.ToLower().Contains(Game1.currentSeason)))
			{
				shake();
			}
			if (maxShake > 0f)
			{
				if (shakeLeft)
				{
					sprite.rotation -= (float)Math.PI / 200f;
					if (sprite.rotation <= 0f - maxShake)
					{
						shakeLeft = false;
					}
				}
				else
				{
					sprite.rotation += (float)Math.PI / 200f;
					if (sprite.rotation >= maxShake)
					{
						shakeLeft = true;
					}
				}
			}
			if (maxShake > 0f)
			{
				maxShake = Math.Max(0f, maxShake - 0.0007669904f);
			}
		}

		public void draw(SpriteBatch b)
		{
			sprite.draw(b, localPosition: true);
		}

		public static Color getColorFromColorIndex(int color)
		{
			return color switch
			{
				5 => Color.LightBlue, 
				0 => Color.Lime, 
				2 => Color.Orange, 
				1 => Color.DeepPink, 
				4 => Color.Red, 
				6 => Color.Cyan, 
				3 => Color.Orange, 
				_ => Color.Lime, 
			};
		}
	}
}
