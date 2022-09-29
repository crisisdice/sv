using Microsoft.Xna.Framework;
using StardewValley.Locations;

namespace StardewValley.Tools
{
	public class Wand : Tool
	{
		public bool charged;

		public Wand()
			: base("Return Scepter", 0, 2, 2, stackable: false)
		{
			base.UpgradeLevel = 0;
			base.CurrentParentTileIndex = base.IndexOfMenuItemView;
			base.InstantUse = true;
		}

		public override Item getOne()
		{
			Wand wand = new Wand();
			CopyEnchantments(this, wand);
			wand._GetOneFrom(this);
			return wand;
		}

		protected override string loadDisplayName()
		{
			return Game1.content.LoadString("Strings\\StringsFromCSFiles:Wand.cs.14318");
		}

		protected override string loadDescription()
		{
			return Game1.content.LoadString("Strings\\StringsFromCSFiles:Wand.cs.14319");
		}

		public override void DoFunction(GameLocation location, int x, int y, int power, Farmer who)
		{
			if (!who.bathingClothes && who.IsLocalPlayer && !who.onBridge.Value)
			{
				indexOfMenuItemView.Value = 2;
				base.CurrentParentTileIndex = 2;
				for (int i = 0; i < 12; i++)
				{
					Game1.multiplayer.broadcastSprites(who.currentLocation, new TemporaryAnimatedSprite(354, Game1.random.Next(25, 75), 6, 1, new Vector2(Game1.random.Next((int)who.position.X - 256, (int)who.position.X + 192), Game1.random.Next((int)who.position.Y - 256, (int)who.position.Y + 192)), flicker: false, Game1.random.NextDouble() < 0.5));
				}
				location.playSound("wand");
				Game1.displayFarmer = false;
				who.temporarilyInvincible = true;
				who.temporaryInvincibilityTimer = -2000;
				who.Halt();
				who.faceDirection(2);
				who.CanMove = false;
				who.freezePause = 2000;
				Game1.flashAlpha = 1f;
				DelayedAction.fadeAfterDelay(wandWarpForReal, 1000);
				new Rectangle(who.GetBoundingBox().X, who.GetBoundingBox().Y, 64, 64).Inflate(192, 192);
				int j = 0;
				for (int xTile = who.getTileX() + 8; xTile >= who.getTileX() - 8; xTile--)
				{
					Game1.multiplayer.broadcastSprites(who.currentLocation, new TemporaryAnimatedSprite(6, new Vector2(xTile, who.getTileY()) * 64f, Color.White, 8, flipped: false, 50f)
					{
						layerDepth = 1f,
						delayBeforeAnimationStart = j * 25,
						motion = new Vector2(-0.25f, 0f)
					});
					j++;
				}
				base.CurrentParentTileIndex = base.IndexOfMenuItemView;
			}
		}

		public override bool actionWhenPurchased()
		{
			Game1.player.mailReceived.Add("ReturnScepter");
			return base.actionWhenPurchased();
		}

		private void wandWarpForReal()
		{
			FarmHouse home = Utility.getHomeOfFarmer(Game1.player);
			if (home != null)
			{
				Point position = home.getFrontDoorSpot();
				Game1.warpFarmer("Farm", position.X, position.Y, flip: false);
				if (!Game1.isStartingToGetDarkOut() && !Game1.isRaining)
				{
					Game1.playMorningSong();
				}
				else
				{
					Game1.changeMusicTrack("none");
				}
				Game1.fadeToBlackAlpha = 0.99f;
				Game1.screenGlow = false;
				lastUser.temporarilyInvincible = false;
				lastUser.temporaryInvincibilityTimer = 0;
				Game1.displayFarmer = true;
				lastUser.CanMove = true;
			}
		}
	}
}
