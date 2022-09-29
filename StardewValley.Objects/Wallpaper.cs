using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Netcode;
using StardewValley.GameData;
using StardewValley.Locations;

namespace StardewValley.Objects
{
	public class Wallpaper : Object
	{
		[XmlElement("sourceRect")]
		public readonly NetRectangle sourceRect = new NetRectangle();

		[XmlElement("isFloor")]
		public readonly NetBool isFloor = new NetBool(value: false);

		[XmlElement("sourceTexture")]
		public readonly NetString modDataID = new NetString(null);

		protected ModWallpaperOrFlooring _modData;

		private static readonly Rectangle wallpaperContainerRect = new Rectangle(39, 31, 16, 16);

		private static readonly Rectangle floorContainerRect = new Rectangle(55, 31, 16, 16);

		public override string Name => base.name;

		public Wallpaper()
		{
			base.NetFields.AddFields(sourceRect, isFloor, modDataID);
		}

		public Wallpaper(int which, bool isFloor = false)
			: this()
		{
			this.isFloor.Value = isFloor;
			base.ParentSheetIndex = which;
			base.name = (isFloor ? "Flooring" : "Wallpaper");
			sourceRect.Value = (isFloor ? new Rectangle(which % 8 * 32, 336 + which / 8 * 32, 28, 26) : new Rectangle(which % 16 * 16, which / 16 * 48 + 8, 16, 28));
			price.Value = 100;
		}

		public Wallpaper(string mod_id, int which)
			: this()
		{
			modDataID.Value = mod_id;
			base.ParentSheetIndex = which;
			if (GetModData() != null)
			{
				isFloor.Value = GetModData().IsFlooring;
			}
			else
			{
				modDataID.Value = null;
			}
			sourceRect.Value = (isFloor ? new Rectangle(which % 8 * 32, 336 + which / 8 * 32, 28, 26) : new Rectangle(which % 16 * 16, which / 16 * 48 + 8, 16, 28));
			if (GetModData() != null && isFloor.Value)
			{
				sourceRect.Y = which / 8 * 32;
			}
			base.name = (isFloor ? "Flooring" : "Wallpaper");
			price.Value = 100;
		}

		public virtual ModWallpaperOrFlooring GetModData()
		{
			if (modDataID.Value == null)
			{
				return null;
			}
			if (_modData != null)
			{
				return _modData;
			}
			foreach (ModWallpaperOrFlooring mod_data_entry in Game1.content.Load<List<ModWallpaperOrFlooring>>("Data\\AdditionalWallpaperFlooring"))
			{
				if (mod_data_entry.ID == modDataID.Value)
				{
					_modData = mod_data_entry;
					return mod_data_entry;
				}
			}
			return null;
		}

		protected override string loadDisplayName()
		{
			if (!isFloor)
			{
				return Game1.content.LoadString("Strings\\StringsFromCSFiles:Wallpaper.cs.13204");
			}
			return Game1.content.LoadString("Strings\\StringsFromCSFiles:Wallpaper.cs.13203");
		}

		public override string getDescription()
		{
			if (!isFloor)
			{
				return Game1.content.LoadString("Strings\\StringsFromCSFiles:Wallpaper.cs.13206");
			}
			return Game1.content.LoadString("Strings\\StringsFromCSFiles:Wallpaper.cs.13205");
		}

		public override bool performDropDownAction(Farmer who)
		{
			return true;
		}

		public override bool performObjectDropInAction(Item dropIn, bool probe, Farmer who)
		{
			return false;
		}

		public override bool canBePlacedHere(GameLocation l, Vector2 tile)
		{
			Vector2 nonTile = tile * 64f;
			nonTile.X += 32f;
			nonTile.Y += 32f;
			foreach (Furniture f in l.furniture)
			{
				if ((int)f.furniture_type != 12 && f.getBoundingBox(f.tileLocation).Contains((int)nonTile.X, (int)nonTile.Y))
				{
					return false;
				}
			}
			return true;
		}

		public override bool placementAction(GameLocation location, int x, int y, Farmer who = null)
		{
			if (who == null)
			{
				who = Game1.player;
			}
			if (who.currentLocation is DecoratableLocation)
			{
				Point tile = new Point(x / 64, y / 64);
				DecoratableLocation farmHouse = who.currentLocation as DecoratableLocation;
				if ((bool)isFloor)
				{
					string floor_id = farmHouse.GetFloorID(tile.X, tile.Y);
					if (floor_id != null)
					{
						if (GetModData() != null)
						{
							farmHouse.SetFloor(GetModData().ID + ":" + parentSheetIndex.ToString(), floor_id);
						}
						else
						{
							farmHouse.SetFloor(parentSheetIndex.ToString(), floor_id);
						}
						location.playSound("coin");
						return true;
					}
				}
				else
				{
					string wall_id = farmHouse.GetWallpaperID(tile.X, tile.Y);
					if (wall_id != null)
					{
						if (GetModData() != null)
						{
							farmHouse.SetWallpaper(GetModData().ID + ":" + parentSheetIndex.ToString(), wall_id);
						}
						else
						{
							farmHouse.SetWallpaper(parentSheetIndex.ToString(), wall_id);
						}
						location.playSound("coin");
						return true;
					}
				}
			}
			return false;
		}

		public override bool isPlaceable()
		{
			return true;
		}

		public override Rectangle getBoundingBox(Vector2 tileLocation)
		{
			return boundingBox;
		}

		public override int salePrice()
		{
			return price;
		}

		public override int maximumStackSize()
		{
			return 1;
		}

		public override int addToStack(Item stack)
		{
			return 1;
		}

		public override void drawWhenHeld(SpriteBatch spriteBatch, Vector2 objectPosition, Farmer f)
		{
			drawInMenu(spriteBatch, objectPosition, 1f);
		}

		public override void drawInMenu(SpriteBatch spriteBatch, Vector2 location, float scaleSize, float transparency, float layerDepth, StackDrawType drawStackNumber, Color color, bool drawShadow)
		{
			Texture2D wallpaperTexture;
			if (GetModData() != null)
			{
				try
				{
					wallpaperTexture = Game1.content.Load<Texture2D>(GetModData().Texture);
				}
				catch (Exception)
				{
					wallpaperTexture = Game1.content.Load<Texture2D>("Maps\\walls_and_floors");
				}
			}
			else
			{
				wallpaperTexture = Game1.content.Load<Texture2D>("Maps\\walls_and_floors");
			}
			if ((bool)isFloor)
			{
				spriteBatch.Draw(Game1.mouseCursors2, location + new Vector2(32f, 32f), floorContainerRect, color * transparency, 0f, new Vector2(8f, 8f), 4f * scaleSize, SpriteEffects.None, layerDepth);
				spriteBatch.Draw(wallpaperTexture, location + new Vector2(32f, 30f), sourceRect, color * transparency, 0f, new Vector2(14f, 13f), 2f * scaleSize, SpriteEffects.None, layerDepth + 0.001f);
			}
			else
			{
				spriteBatch.Draw(Game1.mouseCursors2, location + new Vector2(32f, 32f), wallpaperContainerRect, color * transparency, 0f, new Vector2(8f, 8f), 4f * scaleSize, SpriteEffects.None, layerDepth);
				spriteBatch.Draw(wallpaperTexture, location + new Vector2(32f, 32f), sourceRect, color * transparency, 0f, new Vector2(8f, 14f), 2f * scaleSize, SpriteEffects.None, layerDepth + 0.001f);
			}
		}

		public override Item getOne()
		{
			Wallpaper w = ((GetModData() == null) ? new Wallpaper(parentSheetIndex, isFloor) : new Wallpaper(GetModData().ID, parentSheetIndex));
			w._GetOneFrom(this);
			return w;
		}
	}
}
