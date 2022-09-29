using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.Menus;
using StardewValley.Objects;
using StardewValley.Tools;
using StardewValley.Util;
using xTile.Dimensions;

namespace StardewValley.Locations
{
	public class Sewer : GameLocation
	{
		public const float steamZoom = 4f;

		public const float steamYMotionPerMillisecond = 0.1f;

		public const float millisecondsPerSteamFrame = 50f;

		private Texture2D steamAnimation;

		private Vector2 steamPosition;

		private Color steamColor = new Color(200, 255, 200);

		public Sewer()
		{
		}

		public Sewer(string map, string name)
			: base(map, name)
		{
			waterColor.Value = Color.LimeGreen;
		}

		private Dictionary<ISalable, int[]> generateKrobusStock()
		{
			Dictionary<ISalable, int[]> krobusStock = new Dictionary<ISalable, int[]>();
			krobusStock.Add(new Object(769, 1), new int[2] { 100, 10 });
			krobusStock.Add(new Object(768, 1), new int[2] { 80, 10 });
			Random r = new Random((int)Game1.stats.DaysPlayed + (int)Game1.uniqueIDForThisGame / 2);
			switch (Game1.dayOfMonth % 7)
			{
			case 0:
				krobusStock.Add(new Object(767, 1), new int[2] { 30, 10 });
				break;
			case 1:
				krobusStock.Add(new Object(766, 1), new int[2] { 10, 50 });
				break;
			case 2:
				krobusStock.Add(new Object(749, 1), new int[2] { 300, 1 });
				break;
			case 3:
				krobusStock.Add(new Object(r.Next(698, 709), 1), new int[2] { 200, 5 });
				break;
			case 4:
				krobusStock.Add(new Object(770, 1), new int[2] { 30, 10 });
				break;
			case 5:
				krobusStock.Add(new Object(645, 1), new int[2] { 10000, 1 });
				break;
			case 6:
			{
				int index = r.Next(194, 245);
				if (index == 217)
				{
					index = 216;
				}
				krobusStock.Add(new Object(index, 1), new int[2]
				{
					r.Next(5, 51) * 10,
					5
				});
				break;
			}
			}
			krobusStock.Add(new Object(305, 1), new int[2] { 5000, 2147483647 });
			krobusStock.Add(new Object(Vector2.Zero, 34), new int[2] { 350, 2147483647 });
			krobusStock.Add(new Furniture(1800, Vector2.Zero), new int[2] { 20000, 2147483647 });
			return krobusStock;
		}

		public Dictionary<ISalable, int[]> getShadowShopStock()
		{
			Dictionary<ISalable, int[]> krobusStock = generateKrobusStock();
			Game1.player.team.synchronizedShopStock.UpdateLocalStockWithSyncedQuanitities(SynchronizedShopStock.SynchedShop.Krobus, krobusStock);
			if (!Game1.player.hasOrWillReceiveMail("CF_Sewer"))
			{
				Item starDrop = new Object(434, 1);
				krobusStock.Add(starDrop, new int[2] { 20000, 1 });
			}
			if (!Game1.player.craftingRecipes.ContainsKey("Crystal Floor"))
			{
				new Object(333, 1, isRecipe: true);
				krobusStock.Add(new Object(333, 1, isRecipe: true), new int[2] { 500, 1 });
			}
			if (!Game1.player.craftingRecipes.ContainsKey("Wicked Statue"))
			{
				Item wickedStatue = new Object(Vector2.Zero, 83, isRecipe: true);
				krobusStock.Add(wickedStatue, new int[2] { 1000, 1 });
			}
			if (!Game1.player.hasOrWillReceiveMail("ReturnScepter"))
			{
				Item returnScepter = new Wand();
				krobusStock.Add(returnScepter, new int[2] { 2000000, 1 });
			}
			return krobusStock;
		}

		public bool onShopPurchase(ISalable item, Farmer farmer, int amount)
		{
			Game1.player.team.synchronizedShopStock.OnItemPurchased(SynchronizedShopStock.SynchedShop.Krobus, item, amount);
			return false;
		}

		public override void drawAboveAlwaysFrontLayer(SpriteBatch b)
		{
			base.drawAboveAlwaysFrontLayer(b);
			for (float x = -1000f * Game1.options.zoomLevel + steamPosition.X; x < (float)Game1.graphics.GraphicsDevice.Viewport.Width + 256f; x += 256f)
			{
				for (float y = -256f + steamPosition.Y; y < (float)(Game1.graphics.GraphicsDevice.Viewport.Height + 128); y += 256f)
				{
					b.Draw(steamAnimation, new Vector2(x, y), new Microsoft.Xna.Framework.Rectangle(0, 0, 64, 64), steamColor * 0.75f, 0f, Vector2.Zero, 4f, SpriteEffects.None, 1f);
				}
			}
		}

		public override void UpdateWhenCurrentLocation(GameTime time)
		{
			base.UpdateWhenCurrentLocation(time);
			steamPosition.Y -= (float)time.ElapsedGameTime.Milliseconds * 0.1f;
			steamPosition.Y %= -256f;
			steamPosition -= Game1.getMostRecentViewportMotion();
			if (Game1.random.NextDouble() < 0.001)
			{
				localSound("cavedrip");
			}
		}

		public override bool checkAction(Location tileLocation, xTile.Dimensions.Rectangle viewport, Farmer who)
		{
			switch ((map.GetLayer("Buildings").Tiles[tileLocation] != null) ? map.GetLayer("Buildings").Tiles[tileLocation].TileIndex : (-1))
			{
			case 21:
				Game1.warpFarmer("Town", 35, 97, 2);
				DelayedAction.playSoundAfterDelay("stairsdown", 250);
				return true;
			case 84:
				Game1.activeClickableMenu = new ShopMenu(getShadowShopStock(), 0, "KrobusGone", onShopPurchase);
				return true;
			default:
				return base.checkAction(tileLocation, viewport, who);
			}
		}

		protected override void resetSharedState()
		{
			base.resetSharedState();
			waterColor.Value = Color.LimeGreen * 0.75f;
		}

		protected override void resetLocalState()
		{
			base.resetLocalState();
			steamPosition = new Vector2(0f, 0f);
			steamAnimation = Game1.temporaryContent.Load<Texture2D>("LooseSprites\\steamAnimation");
			Game1.ambientLight = new Color(250, 140, 160);
		}

		public override void MakeMapModifications(bool force = false)
		{
			base.MakeMapModifications(force);
			if (Game1.getCharacterFromName("Krobus").isMarried())
			{
				setMapTileIndex(31, 17, 84, "Buildings", 1);
				setMapTileIndex(31, 16, 1, "Front", 1);
			}
			else
			{
				setMapTileIndex(31, 17, -1, "Buildings");
				setMapTileIndex(31, 16, -1, "Front");
			}
		}

		public override Object getFish(float millisecondsAfterNibble, int bait, int waterDepth, Farmer who, double baitPotency, Vector2 bobberTile, string locationName = null)
		{
			float bobberAddition = 0f;
			if (who != null && who.CurrentTool is FishingRod && (who.CurrentTool as FishingRod).getBobberAttachmentIndex() == 856)
			{
				bobberAddition += 0.1f;
			}
			if (Game1.random.NextDouble() < 0.1 + (double)bobberAddition + ((who.getTileX() > 14 && who.getTileY() > 42) ? 0.08 : 0.0))
			{
				if (Game1.player.team.SpecialOrderRuleActive("LEGENDARY_FAMILY"))
				{
					return new Object(901, 1);
				}
				if (!who.fishCaught.ContainsKey(682))
				{
					return new Object(682, 1);
				}
			}
			return base.getFish(millisecondsAfterNibble, bait, waterDepth, who, baitPotency, bobberTile, locationName);
		}

		public override void cleanupBeforePlayerExit()
		{
			base.cleanupBeforePlayerExit();
			Game1.changeMusicTrack("none");
		}
	}
}
