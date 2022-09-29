using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Netcode;
using StardewValley.BellsAndWhistles;
using StardewValley.Characters;
using StardewValley.Menus;
using StardewValley.Monsters;
using StardewValley.Objects;
using StardewValley.TerrainFeatures;
using xTile;
using xTile.Dimensions;
using xTile.Layers;
using xTile.Tiles;

namespace StardewValley.Locations
{
	public class FarmHouse : DecoratableLocation
	{
		public int farmerNumberOfOwner;

		[XmlElement("fireplaceOn")]
		public readonly NetBool fireplaceOn = new NetBool();

		[XmlElement("fridge")]
		public readonly NetRef<Chest> fridge = new NetRef<Chest>(new Chest(playerChest: true));

		[XmlIgnore]
		public readonly NetInt synchronizedDisplayedLevel = new NetInt(-1);

		public Point fridgePosition;

		[XmlIgnore]
		public Point spouseRoomSpot;

		[XmlIgnore]
		private LocalizedContentManager mapLoader;

		public List<Warp> cellarWarps;

		[XmlElement("cribStyle")]
		public readonly NetInt cribStyle = new NetInt(1);

		[XmlIgnore]
		public int previousUpgradeLevel = -1;

		private int currentlyDisplayedUpgradeLevel;

		private bool displayingSpouseRoom;

		[XmlIgnore]
		public virtual Farmer owner => Game1.MasterPlayer;

		[XmlIgnore]
		public virtual int upgradeLevel
		{
			get
			{
				if (owner == null)
				{
					return 0;
				}
				return owner.houseUpgradeLevel;
			}
			set
			{
				if (owner != null)
				{
					owner.houseUpgradeLevel.Value = value;
				}
			}
		}

		public FarmHouse()
		{
		}

		public FarmHouse(int ownerNumber = 1)
		{
			farmerNumberOfOwner = ownerNumber;
		}

		protected override void initNetFields()
		{
			base.initNetFields();
			base.NetFields.AddFields(fireplaceOn, fridge, cribStyle, synchronizedDisplayedLevel);
			fireplaceOn.fieldChangeVisibleEvent += delegate(NetBool field, bool oldValue, bool newValue)
			{
				Point fireplacePoint = getFireplacePoint();
				setFireplace(newValue, fireplacePoint.X, fireplacePoint.Y);
			};
			cribStyle.InterpolationEnabled = false;
			cribStyle.fieldChangeVisibleEvent += delegate
			{
				if (map != null)
				{
					if (_appliedMapOverrides != null && _appliedMapOverrides.Contains("crib"))
					{
						_appliedMapOverrides.Remove("crib");
					}
					UpdateChildRoom();
					ReadWallpaperAndFloorTileData();
					setWallpapers();
					setFloors();
				}
			};
		}

		public List<Child> getChildren()
		{
			return (from n in characters
				where n is Child
				select n as Child).ToList();
		}

		public int getChildrenCount()
		{
			int count = 0;
			foreach (NPC character in characters)
			{
				if (character is Child)
				{
					count++;
				}
			}
			return count;
		}

		public override bool isCollidingPosition(Microsoft.Xna.Framework.Rectangle position, xTile.Dimensions.Rectangle viewport, bool isFarmer, int damagesFarmer, bool glider, Character character, bool pathfinding, bool projectile = false, bool ignoreCharacterRequirement = false)
		{
			return base.isCollidingPosition(position, viewport, isFarmer, damagesFarmer, glider, character, pathfinding);
		}

		public override bool isCollidingPosition(Microsoft.Xna.Framework.Rectangle position, xTile.Dimensions.Rectangle viewport, bool isFarmer, int damagesFarmer, bool glider, Character character)
		{
			return base.isCollidingPosition(position, viewport, isFarmer, damagesFarmer, glider, character);
		}

		public override bool isTileLocationTotallyClearAndPlaceable(Vector2 v)
		{
			return base.isTileLocationTotallyClearAndPlaceable(v);
		}

		public override void performTenMinuteUpdate(int timeOfDay)
		{
			base.performTenMinuteUpdate(timeOfDay);
			foreach (NPC c in characters)
			{
				if (c.isMarried())
				{
					if (c.getSpouse() == Game1.player)
					{
						c.checkForMarriageDialogue(timeOfDay, this);
					}
					if (Game1.IsMasterGame && Game1.timeOfDay >= 2200 && Game1.IsMasterGame && c.getTileLocationPoint() != getSpouseBedSpot(c.Name) && (timeOfDay == 2200 || (c.controller == null && timeOfDay % 100 % 30 == 0)))
					{
						Point bed_spot = getSpouseBedSpot(c.Name);
						c.controller = null;
						PathFindController.endBehavior end_behavior = null;
						bool found_bed = GetSpouseBed() != null;
						if (found_bed)
						{
							end_behavior = spouseSleepEndFunction;
						}
						c.controller = new PathFindController(c, this, bed_spot, 0, end_behavior);
						if (c.controller.pathToEndPoint == null || !isTileOnMap(c.controller.pathToEndPoint.Last().X, c.controller.pathToEndPoint.Last().Y))
						{
							c.controller = null;
						}
						else if (found_bed)
						{
							foreach (Furniture furniture in base.furniture)
							{
								if (furniture is BedFurniture && furniture.getBoundingBox(furniture.TileLocation).Intersects(new Microsoft.Xna.Framework.Rectangle(bed_spot.X * 64, bed_spot.Y * 64, 64, 64)))
								{
									(furniture as BedFurniture).ReserveForNPC();
									break;
								}
							}
						}
					}
				}
				if (c is Child)
				{
					(c as Child).tenMinuteUpdate();
				}
			}
		}

		public static void spouseSleepEndFunction(Character c, GameLocation location)
		{
			if (c == null || !(c is NPC))
			{
				return;
			}
			if (Game1.content.Load<Dictionary<string, string>>("Data\\animationDescriptions").ContainsKey(c.name.Value.ToLower() + "_sleep"))
			{
				(c as NPC).playSleepingAnimation();
			}
			foreach (Furniture furniture in location.furniture)
			{
				if (furniture is BedFurniture && furniture.getBoundingBox(furniture.TileLocation).Intersects(c.GetBoundingBox()))
				{
					(furniture as BedFurniture).ReserveForNPC();
					break;
				}
			}
		}

		public virtual Point getFrontDoorSpot()
		{
			foreach (Warp warp in warps)
			{
				if (warp.TargetName == "Farm")
				{
					if (this is Cabin)
					{
						return new Point(warp.TargetX, warp.TargetY);
					}
					if (warp.TargetX == 64 && warp.TargetY == 15)
					{
						return Game1.getFarm().GetMainFarmHouseEntry();
					}
					return new Point(warp.TargetX, warp.TargetY);
				}
			}
			return Game1.getFarm().GetMainFarmHouseEntry();
		}

		public virtual Point getPorchStandingSpot()
		{
			int num = farmerNumberOfOwner;
			if ((uint)num <= 1u)
			{
				Point p = Game1.getFarm().GetMainFarmHouseEntry();
				p.X += 2;
				return p;
			}
			return new Point(-1000, -1000);
		}

		public Point getKitchenStandingSpot()
		{
			switch (upgradeLevel)
			{
			case 1:
				return GetMapPropertyPosition("KitchenStandingLocation", 4, 5);
			case 2:
			case 3:
				return GetMapPropertyPosition("KitchenStandingLocation", 7, 14);
			default:
				return GetMapPropertyPosition("KitchenStandingLocation", -1000, -1000);
			}
		}

		public virtual BedFurniture GetSpouseBed()
		{
			if (owner.getSpouse() != null && owner.getSpouse().Name == "Krobus")
			{
				return null;
			}
			if (owner != null && owner.hasCurrentOrPendingRoommate() && GetBed(BedFurniture.BedType.Single) != null)
			{
				return GetBed(BedFurniture.BedType.Single);
			}
			return GetBed(BedFurniture.BedType.Double);
		}

		public Point getSpouseBedSpot(string spouseName)
		{
			if ((spouseName == "Krobus" && SocialPage.isRoommateOfAnyone(spouseName)) || GetSpouseBed() == null)
			{
				return GetSpouseRoomSpot();
			}
			BedFurniture spouseBed = GetSpouseBed();
			Point bed_spot = GetSpouseBed().GetBedSpot();
			if (spouseBed.bedType == BedFurniture.BedType.Double)
			{
				bed_spot.X++;
			}
			return bed_spot;
		}

		public Point GetSpouseRoomSpot()
		{
			if (upgradeLevel == 0)
			{
				return new Point(-1000, -1000);
			}
			return spouseRoomSpot;
		}

		public BedFurniture GetBed(BedFurniture.BedType bed_type = BedFurniture.BedType.Any, int index = 0)
		{
			foreach (Furniture f in furniture)
			{
				if (!(f is BedFurniture))
				{
					continue;
				}
				BedFurniture bed = f as BedFurniture;
				if (bed_type == BedFurniture.BedType.Any || bed.bedType == bed_type)
				{
					if (index == 0)
					{
						return bed;
					}
					index--;
				}
			}
			return null;
		}

		public Point GetPlayerBedSpot()
		{
			return GetPlayerBed()?.GetBedSpot() ?? getEntryLocation();
		}

		public BedFurniture GetPlayerBed()
		{
			if (upgradeLevel == 0)
			{
				return GetBed(BedFurniture.BedType.Single);
			}
			return GetBed(BedFurniture.BedType.Double);
		}

		public Point getBedSpot(BedFurniture.BedType bed_type = BedFurniture.BedType.Any)
		{
			return GetBed(bed_type)?.GetBedSpot() ?? new Point(-1000, -1000);
		}

		public Point getEntryLocation()
		{
			switch (upgradeLevel)
			{
			case 0:
				return GetMapPropertyPosition("EntryLocation", 3, 11);
			case 1:
				return GetMapPropertyPosition("EntryLocation", 9, 11);
			case 2:
			case 3:
				return GetMapPropertyPosition("EntryLocation", 12, 20);
			default:
				return new Point(-1000, -1000);
			}
		}

		public BedFurniture GetChildBed(int index)
		{
			return GetBed(BedFurniture.BedType.Child, index);
		}

		public Point GetChildBedSpot(int index)
		{
			return GetChildBed(index)?.GetBedSpot() ?? Point.Zero;
		}

		public override bool isTilePlaceable(Vector2 v, Item item = null)
		{
			if (isTileOnMap(v) && getTileIndexAt((int)v.X, (int)v.Y, "Back") == 0 && getTileSheetIDAt((int)v.X, (int)v.Y, "Back") == "indoor")
			{
				return false;
			}
			return base.isTilePlaceable(v, item);
		}

		public Point getRandomOpenPointInHouse(Random r, int buffer = 0, int tries = 30)
		{
			Point point = Point.Zero;
			for (int numTries = 0; numTries < tries; numTries++)
			{
				point = new Point(r.Next(map.Layers[0].LayerWidth), r.Next(map.Layers[0].LayerHeight));
				Microsoft.Xna.Framework.Rectangle zone = new Microsoft.Xna.Framework.Rectangle(point.X - buffer, point.Y - buffer, 1 + buffer * 2, 1 + buffer * 2);
				bool obstacleFound = false;
				for (int x = zone.X; x < zone.Right; x++)
				{
					for (int y = zone.Y; y < zone.Bottom; y++)
					{
						obstacleFound = getTileIndexAt(x, y, "Back") == -1 || !isTileLocationTotallyClearAndPlaceable(x, y) || isTileOnWall(x, y);
						if (getTileIndexAt(x, y, "Back") == 0 && getTileSheetIDAt(x, y, "Back") == "indoor")
						{
							obstacleFound = true;
						}
						if (obstacleFound)
						{
							break;
						}
					}
					if (obstacleFound)
					{
						break;
					}
				}
				if (!obstacleFound)
				{
					return point;
				}
			}
			return Point.Zero;
		}

		public override bool performAction(string action, Farmer who, Location tileLocation)
		{
			if (action != null && who.IsLocalPlayer && action.Split(' ')[0] == "kitchen")
			{
				ActivateKitchen(fridge);
				return true;
			}
			return base.performAction(action, who, tileLocation);
		}

		public override bool checkAction(Location tileLocation, xTile.Dimensions.Rectangle viewport, Farmer who)
		{
			if (map.GetLayer("Buildings").Tiles[tileLocation] != null)
			{
				switch (map.GetLayer("Buildings").Tiles[tileLocation].TileIndex)
				{
				case 173:
					fridge.Value.fridge.Value = true;
					fridge.Value.checkForAction(who);
					return true;
				case 794:
				case 795:
				case 796:
				case 797:
					fireplaceOn.Value = !fireplaceOn;
					return true;
				case 2173:
					if (Game1.player.eventsSeen.Contains(463391) && Game1.player.spouse != null && Game1.player.spouse.Equals("Emily"))
					{
						TemporaryAnimatedSprite t = getTemporarySpriteByID(5858585);
						if (t != null && t is EmilysParrot)
						{
							(t as EmilysParrot).doAction();
						}
					}
					return true;
				}
			}
			if (base.checkAction(tileLocation, viewport, who))
			{
				return true;
			}
			return false;
		}

		public FarmHouse(string m, string name)
			: base(m, name)
		{
			ReadWallpaperAndFloorTileData();
			furniture.Add(new BedFurniture(BedFurniture.DEFAULT_BED_INDEX, new Vector2(9f, 8f)));
			if (Game1.getFarm().getMapProperty("FarmHouseFurniture") != "")
			{
				if (!int.TryParse(Game1.getFarm().getMapProperty("FarmHouseWallpaper"), out var parsed_int))
				{
					parsed_int = -1;
				}
				if (parsed_int >= 0)
				{
					setWallpaper(parsed_int, -1, persist: true);
				}
				if (!int.TryParse(Game1.getFarm().getMapProperty("FarmHouseFlooring"), out parsed_int))
				{
					parsed_int = -1;
				}
				if (parsed_int >= 0)
				{
					setFloor(parsed_int, -1, persist: true);
				}
				if (Game1.getFarm().getMapProperty("FarmHouseStarterSeedsPosition") != "")
				{
					int x2 = 3;
					int y2 = 7;
					string[] split = Game1.getFarm().getMapProperty("FarmHouseStarterSeedsPosition").Split(' ');
					if (split.Length == 2)
					{
						if (!int.TryParse(split[0], out x2))
						{
							x2 = 3;
						}
						if (!int.TryParse(split[1], out y2))
						{
							y2 = 7;
						}
					}
					objects.Add(new Vector2(x2, y2), new Chest(0, new List<Item>
					{
						new Object(472, 15)
					}, new Vector2(x2, y2), giftbox: true));
				}
				else
				{
					objects.Add(new Vector2(3f, 7f), new Chest(0, new List<Item>
					{
						new Object(472, 15)
					}, new Vector2(3f, 7f), giftbox: true));
				}
				string[] furniture_split = Game1.getFarm().getMapProperty("FarmHouseFurniture").Split(' ');
				for (int i = 0; i < furniture_split.Length; i += 4)
				{
					int index = -1;
					int rotations = 0;
					if (!int.TryParse(furniture_split[i], out index))
					{
						index = -1;
					}
					if (!int.TryParse(furniture_split[i + 1], out var x))
					{
						index = -1;
					}
					if (!int.TryParse(furniture_split[i + 2], out var y))
					{
						index = -1;
					}
					if (!int.TryParse(furniture_split[i + 3], out rotations))
					{
						index = -1;
					}
					if (index >= 0)
					{
						Furniture target_furniture = GetFurnitureAt(new Vector2(x, y));
						if (target_furniture != null)
						{
							target_furniture.heldObject.Value = new Furniture(index, new Vector2(x, y), rotations);
						}
						else
						{
							furniture.Add(new Furniture(index, new Vector2(x, y), rotations));
						}
					}
				}
				return;
			}
			switch (Game1.whichFarm)
			{
			case 0:
				furniture.Add(new Furniture(1120, new Vector2(5f, 4f)));
				furniture.Last().heldObject.Value = new Furniture(1364, new Vector2(5f, 4f));
				furniture.Add(new Furniture(1376, new Vector2(1f, 10f)));
				furniture.Add(new Furniture(0, new Vector2(4f, 4f)));
				furniture.Add(new TV(1466, new Vector2(1f, 4f)));
				furniture.Add(new Furniture(1614, new Vector2(3f, 1f)));
				furniture.Add(new Furniture(1618, new Vector2(6f, 8f)));
				furniture.Add(new Furniture(1602, new Vector2(5f, 1f)));
				furniture.Add(new Furniture(1792, Utility.PointToVector2(getFireplacePoint())));
				objects.Add(new Vector2(3f, 7f), new Chest(0, new List<Item>
				{
					new Object(472, 15)
				}, new Vector2(3f, 7f), giftbox: true));
				break;
			case 1:
				setWallpaper(11, -1, persist: true);
				setFloor(1, -1, persist: true);
				furniture.Add(new Furniture(1122, new Vector2(1f, 6f)));
				furniture.Last().heldObject.Value = new Furniture(1367, new Vector2(1f, 6f));
				furniture.Add(new Furniture(3, new Vector2(1f, 5f)));
				furniture.Add(new TV(1680, new Vector2(5f, 4f)));
				furniture.Add(new Furniture(1673, new Vector2(1f, 1f)));
				furniture.Add(new Furniture(1673, new Vector2(3f, 1f)));
				furniture.Add(new Furniture(1676, new Vector2(5f, 1f)));
				furniture.Add(new Furniture(1737, new Vector2(6f, 8f)));
				furniture.Add(new Furniture(1742, new Vector2(5f, 5f)));
				furniture.Add(new Furniture(1792, Utility.PointToVector2(getFireplacePoint())));
				furniture.Add(new Furniture(1675, new Vector2(10f, 1f)));
				objects.Add(new Vector2(4f, 7f), new Chest(0, new List<Item>
				{
					new Object(472, 15)
				}, new Vector2(4f, 7f), giftbox: true));
				break;
			case 2:
				setWallpaper(92, -1, persist: true);
				setFloor(34, -1, persist: true);
				furniture.Add(new Furniture(1134, new Vector2(1f, 7f)));
				furniture.Last().heldObject.Value = new Furniture(1748, new Vector2(1f, 7f));
				furniture.Add(new Furniture(3, new Vector2(1f, 6f)));
				furniture.Add(new TV(1680, new Vector2(6f, 4f)));
				furniture.Add(new Furniture(1296, new Vector2(1f, 4f)));
				furniture.Add(new Furniture(1682, new Vector2(3f, 1f)));
				furniture.Add(new Furniture(1777, new Vector2(6f, 5f)));
				furniture.Add(new Furniture(1745, new Vector2(6f, 1f)));
				furniture.Add(new Furniture(1792, Utility.PointToVector2(getFireplacePoint())));
				furniture.Add(new Furniture(1747, new Vector2(5f, 4f)));
				furniture.Add(new Furniture(1296, new Vector2(10f, 4f)));
				objects.Add(new Vector2(4f, 7f), new Chest(0, new List<Item>
				{
					new Object(472, 15)
				}, new Vector2(4f, 7f), giftbox: true));
				break;
			case 3:
				setWallpaper(12, -1, persist: true);
				setFloor(18, -1, persist: true);
				furniture.Add(new Furniture(1218, new Vector2(1f, 6f)));
				furniture.Last().heldObject.Value = new Furniture(1368, new Vector2(1f, 6f));
				furniture.Add(new Furniture(1755, new Vector2(1f, 5f)));
				furniture.Add(new Furniture(1755, new Vector2(3f, 6f), 1));
				furniture.Add(new TV(1680, new Vector2(5f, 4f)));
				furniture.Add(new Furniture(1751, new Vector2(5f, 10f)));
				furniture.Add(new Furniture(1749, new Vector2(3f, 1f)));
				furniture.Add(new Furniture(1753, new Vector2(5f, 1f)));
				furniture.Add(new Furniture(1742, new Vector2(5f, 5f)));
				objects.Add(new Vector2(2f, 9f), new Chest(0, new List<Item>
				{
					new Object(472, 15)
				}, new Vector2(2f, 9f), giftbox: true));
				furniture.Add(new Furniture(1794, Utility.PointToVector2(getFireplacePoint())));
				break;
			case 4:
				setWallpaper(95, -1, persist: true);
				setFloor(4, -1, persist: true);
				furniture.Add(new TV(1680, new Vector2(1f, 4f)));
				furniture.Add(new Furniture(1628, new Vector2(1f, 5f)));
				furniture.Add(new Furniture(1393, new Vector2(3f, 4f)));
				furniture.Last().heldObject.Value = new Furniture(1369, new Vector2(3f, 4f));
				furniture.Add(new Furniture(1678, new Vector2(10f, 1f)));
				furniture.Add(new Furniture(1812, new Vector2(3f, 1f)));
				furniture.Add(new Furniture(1630, new Vector2(1f, 1f)));
				furniture.Add(new Furniture(1794, Utility.PointToVector2(getFireplacePoint())));
				furniture.Add(new Furniture(1811, new Vector2(6f, 1f)));
				furniture.Add(new Furniture(1389, new Vector2(10f, 4f)));
				objects.Add(new Vector2(4f, 7f), new Chest(0, new List<Item>
				{
					new Object(472, 15)
				}, new Vector2(4f, 7f), giftbox: true));
				furniture.Add(new Furniture(1758, new Vector2(1f, 10f)));
				break;
			case 5:
				setWallpaper(65, -1, persist: true);
				setFloor(5, -1, persist: true);
				furniture.Add(new TV(1466, new Vector2(1f, 4f)));
				furniture.Add(new Furniture(1792, Utility.PointToVector2(getFireplacePoint())));
				furniture.Add(new Furniture(1614, new Vector2(3f, 1f)));
				furniture.Add(new Furniture(1614, new Vector2(6f, 1f)));
				furniture.Add(new Furniture(1601, new Vector2(10f, 1f)));
				furniture.Add(new Furniture(202, new Vector2(3f, 4f), 1));
				furniture.Add(new Furniture(1124, new Vector2(4f, 4f), 1));
				furniture.Last().heldObject.Value = new Furniture(1379, new Vector2(5f, 4f));
				furniture.Add(new Furniture(202, new Vector2(6f, 4f), 3));
				furniture.Add(new Furniture(1378, new Vector2(10f, 4f)));
				furniture.Add(new Furniture(1377, new Vector2(1f, 9f)));
				furniture.Add(new Furniture(1445, new Vector2(1f, 10f)));
				furniture.Add(new Furniture(1618, new Vector2(2f, 9f)));
				objects.Add(new Vector2(3f, 7f), new Chest(0, new List<Item>
				{
					new Object(472, 15)
				}, new Vector2(3f, 7f), giftbox: true));
				break;
			case 6:
				setWallpaper(106, -1, persist: true);
				setFloor(35, -1, persist: true);
				furniture.Add(new TV(1680, new Vector2(4f, 4f)));
				furniture.Add(new Furniture(1614, new Vector2(7f, 1f)));
				furniture.Add(new Furniture(1294, new Vector2(3f, 4f)));
				furniture.Add(new Furniture(1283, new Vector2(1f, 4f)));
				furniture.Add(new Furniture(1614, new Vector2(8f, 1f)));
				furniture.Add(new Furniture(202, new Vector2(7f, 4f)));
				furniture.Add(new Furniture(1294, new Vector2(10f, 4f)));
				furniture.Add(new Furniture(6, new Vector2(2f, 6f), 1));
				furniture.Add(new Furniture(6, new Vector2(5f, 7f), 3));
				furniture.Add(new Furniture(1124, new Vector2(3f, 6f)));
				furniture.Last().heldObject.Value = new Furniture(1362, new Vector2(4f, 6f));
				objects.Add(new Vector2(8f, 6f), new Chest(0, new List<Item>
				{
					new Object(472, 15)
				}, new Vector2(8f, 6f), giftbox: true));
				furniture.Add(new Furniture(1228, new Vector2(2f, 9f)));
				break;
			}
		}

		public bool hasActiveFireplace()
		{
			for (int i = 0; i < furniture.Count(); i++)
			{
				if ((int)furniture[i].furniture_type == 14 && (bool)furniture[i].isOn)
				{
					return true;
				}
			}
			return false;
		}

		public override void updateEvenIfFarmerIsntHere(GameTime time, bool ignoreWasUpdatedFlush = false)
		{
			base.updateEvenIfFarmerIsntHere(time, ignoreWasUpdatedFlush);
			if (!Game1.IsMasterGame)
			{
				return;
			}
			foreach (NPC npc in characters)
			{
				Farmer spouse_farmer = npc.getSpouse();
				if (spouse_farmer == null || spouse_farmer != owner)
				{
					continue;
				}
				NPC spouse = npc;
				if (spouse == null || Game1.timeOfDay >= 1500 || !(Game1.random.NextDouble() < 0.0006) || spouse.controller != null || spouse.Schedule != null || spouse.getTileLocation().Equals(Utility.PointToVector2(getSpouseBedSpot(Game1.player.spouse))) || furniture.Count <= 0)
				{
					continue;
				}
				Furniture f = furniture[Game1.random.Next(furniture.Count)];
				Microsoft.Xna.Framework.Rectangle b = f.boundingBox;
				Vector2 possibleLocation = new Vector2(b.X / 64, b.Y / 64);
				if (f.furniture_type.Value == 15 || f.furniture_type.Value == 12)
				{
					continue;
				}
				int tries = 0;
				int facingDirection = -3;
				for (; tries < 3; tries++)
				{
					int xMove = Game1.random.Next(-1, 2);
					int yMove = Game1.random.Next(-1, 2);
					possibleLocation.X += xMove;
					if (xMove == 0)
					{
						possibleLocation.Y += yMove;
					}
					switch (xMove)
					{
					case -1:
						facingDirection = 1;
						break;
					case 1:
						facingDirection = 3;
						break;
					default:
						switch (yMove)
						{
						case -1:
							facingDirection = 2;
							break;
						case 1:
							facingDirection = 0;
							break;
						}
						break;
					}
					if (isTileLocationTotallyClearAndPlaceable(possibleLocation))
					{
						break;
					}
				}
				if (tries < 3)
				{
					spouse.controller = new PathFindController(spouse, this, new Point((int)possibleLocation.X, (int)possibleLocation.Y), facingDirection, eraseOldPathController: false, clearMarriageDialogues: false);
				}
			}
		}

		public override void UpdateWhenCurrentLocation(GameTime time)
		{
			if (wasUpdated)
			{
				return;
			}
			base.UpdateWhenCurrentLocation(time);
			fridge.Value.updateWhenCurrentLocation(time, this);
			if (!Game1.player.isMarried() || Game1.player.spouse == null)
			{
				return;
			}
			NPC spouse = getCharacterFromName(Game1.player.spouse);
			if (spouse == null || spouse.isEmoting)
			{
				return;
			}
			Vector2 spousePos = spouse.getTileLocation();
			Vector2[] adjacentTilesOffsets = Character.AdjacentTilesOffsets;
			foreach (Vector2 offset in adjacentTilesOffsets)
			{
				Vector2 v = spousePos + offset;
				NPC i = isCharacterAtTile(v);
				if (i != null && i.IsMonster && !i.Name.Equals("Cat"))
				{
					spouse.faceGeneralDirection(v * new Vector2(64f, 64f));
					Game1.showSwordswipeAnimation(spouse.FacingDirection, spouse.Position, 60f, flip: false);
					localSound("swordswipe");
					spouse.shake(500);
					spouse.showTextAboveHead(Game1.content.LoadString("Strings\\Locations:FarmHouse_SpouseAttacked" + (Game1.random.Next(12) + 1)));
					((Monster)i).takeDamage(50, (int)Utility.getAwayFromPositionTrajectory(i.GetBoundingBox(), spouse.Position).X, (int)Utility.getAwayFromPositionTrajectory(i.GetBoundingBox(), spouse.Position).Y, isBomb: false, 1.0, Game1.player);
					if (((Monster)i).Health <= 0)
					{
						debris.Add(new Debris(i.Sprite.textureName, Game1.random.Next(6, 16), new Vector2(i.getStandingX(), i.getStandingY())));
						monsterDrop((Monster)i, i.getStandingX(), i.getStandingY(), owner);
						characters.Remove(i);
						Game1.stats.MonstersKilled++;
						Game1.player.changeFriendship(-10, spouse);
					}
					else
					{
						((Monster)i).shedChunks(4);
					}
					spouse.CurrentDialogue.Clear();
					spouse.CurrentDialogue.Push(new Dialogue(Game1.content.LoadString("Data\\ExtraDialogue:Spouse_MonstersInHouse"), spouse));
				}
			}
		}

		public Point getFireplacePoint()
		{
			switch (upgradeLevel)
			{
			case 0:
				return new Point(8, 4);
			case 1:
				return new Point(26, 4);
			case 2:
			case 3:
				return new Point(2, 13);
			default:
				return new Point(-50, -50);
			}
		}

		public bool shouldShowSpouseRoom()
		{
			return owner.isMarried();
		}

		public virtual void showSpouseRoom()
		{
			bool showSpouse = owner.isMarried() && owner.spouse != null;
			bool num = displayingSpouseRoom;
			displayingSpouseRoom = showSpouse;
			updateMap();
			if (num && !displayingSpouseRoom)
			{
				Point corner = GetSpouseRoomCorner();
				Microsoft.Xna.Framework.Rectangle spouseRoomBounds = new Microsoft.Xna.Framework.Rectangle(corner.X, corner.Y, GetSpouseRoomWidth(), GetSpouseRoomHeight());
				spouseRoomBounds.X--;
				List<Item> collected_items = new List<Item>();
				Microsoft.Xna.Framework.Rectangle room_bounds = new Microsoft.Xna.Framework.Rectangle(spouseRoomBounds.X * 64, spouseRoomBounds.Y * 64, spouseRoomBounds.Width * 64, spouseRoomBounds.Height * 64);
				foreach (Furniture placed_furniture in new List<Furniture>(furniture))
				{
					if (placed_furniture.getBoundingBox(placed_furniture.tileLocation).Intersects(room_bounds))
					{
						if (placed_furniture is StorageFurniture)
						{
							StorageFurniture storage_furniture = placed_furniture as StorageFurniture;
							collected_items.AddRange(storage_furniture.heldItems);
							storage_furniture.heldItems.Clear();
						}
						if (placed_furniture.heldObject.Value != null)
						{
							collected_items.Add((Object)placed_furniture.heldObject);
							placed_furniture.heldObject.Value = null;
						}
						collected_items.Add(placed_furniture);
						furniture.Remove(placed_furniture);
					}
				}
				for (int x = spouseRoomBounds.X; x <= spouseRoomBounds.Right; x++)
				{
					for (int y = spouseRoomBounds.Y; y <= spouseRoomBounds.Bottom; y++)
					{
						Object tile_object = getObjectAtTile(x, y);
						if (tile_object == null || tile_object is Furniture)
						{
							continue;
						}
						tile_object.performRemoveAction(new Vector2(x, y), this);
						if (tile_object is Fence)
						{
							tile_object = new Object(Vector2.Zero, (tile_object as Fence).GetItemParentSheetIndex(), 1);
						}
						if (tile_object is IndoorPot)
						{
							IndoorPot garden_pot = tile_object as IndoorPot;
							if (garden_pot.hoeDirt.Value != null && garden_pot.hoeDirt.Value.crop != null)
							{
								garden_pot.hoeDirt.Value.destroyCrop(garden_pot.tileLocation, showAnimation: false, this);
							}
						}
						else if (tile_object is Chest)
						{
							Chest chest = tile_object as Chest;
							collected_items.AddRange(chest.items);
							chest.items.Clear();
						}
						if (tile_object.heldObject != null)
						{
							tile_object.heldObject.Value = null;
						}
						tile_object.minutesUntilReady.Value = -1;
						if (tile_object.readyForHarvest.Value)
						{
							tile_object.readyForHarvest.Value = false;
						}
						collected_items.Add(tile_object);
						objects.Remove(new Vector2(x, y));
					}
				}
				if (upgradeLevel >= 2)
				{
					Utility.createOverflowChest(this, new Vector2(24f, 22f), collected_items);
				}
				else
				{
					Utility.createOverflowChest(this, new Vector2(21f, 10f), collected_items);
				}
			}
			loadObjects();
			if (upgradeLevel == 3)
			{
				AddCellarTiles();
				createCellarWarps();
				if (!Game1.player.craftingRecipes.ContainsKey("Cask"))
				{
					Game1.player.craftingRecipes.Add("Cask", 0);
				}
			}
			if (showSpouse)
			{
				loadSpouseRoom();
			}
		}

		public virtual void AddCellarTiles()
		{
			if (_appliedMapOverrides.Contains("cellar"))
			{
				_appliedMapOverrides.Remove("cellar");
			}
			ApplyMapOverride("FarmHouse_Cellar", "cellar");
		}

		public string GetCellarName()
		{
			int cellar_number = -1;
			if (owner != null)
			{
				foreach (int i in Game1.player.team.cellarAssignments.Keys)
				{
					if (Game1.player.team.cellarAssignments[i] == owner.UniqueMultiplayerID)
					{
						cellar_number = i;
					}
				}
			}
			if (cellar_number >= 0 && cellar_number <= 1)
			{
				return "Cellar";
			}
			return "Cellar" + cellar_number;
		}

		protected override void resetSharedState()
		{
			base.resetSharedState();
			if (Game1.timeOfDay >= 2200 && owner.spouse != null && getCharacterFromName(owner.spouse) != null && !owner.isEngaged())
			{
				Game1.player.team.requestSpouseSleepEvent.Fire(owner.UniqueMultiplayerID);
			}
			if (Game1.timeOfDay >= 2000 && owner.UniqueMultiplayerID == Game1.player.UniqueMultiplayerID && Game1.getFarm().farmers.Count <= 1)
			{
				Game1.player.team.requestPetWarpHomeEvent.Fire(owner.UniqueMultiplayerID);
			}
			if (!Game1.IsMasterGame)
			{
				return;
			}
			Farm farm = Game1.getFarm();
			for (int j = characters.Count - 1; j >= 0; j--)
			{
				if (characters[j] is Pet && (!isTileOnMap(characters[j].getTileX(), characters[j].getTileY()) || getTileIndexAt(characters[j].GetBoundingBox().Left / 64, characters[j].getTileY(), "Buildings") != -1 || getTileIndexAt(characters[j].GetBoundingBox().Right / 64, characters[j].getTileY(), "Buildings") != -1))
				{
					characters[j].faceDirection(2);
					Game1.warpCharacter(characters[j], "Farm", farm.GetPetStartLocation());
					break;
				}
			}
			for (int i = characters.Count - 1; i >= 0; i--)
			{
				for (int l = i - 1; l >= 0; l--)
				{
					if (i < characters.Count && l < characters.Count && (characters[l].Equals(characters[i]) || (characters[l].Name.Equals(characters[i].Name) && characters[l].isVillager() && characters[i].isVillager())) && l != i)
					{
						characters.RemoveAt(l);
					}
				}
				for (int k = farm.characters.Count - 1; k >= 0; k--)
				{
					if (i < characters.Count && k < characters.Count && farm.characters[k].Equals(characters[i]))
					{
						farm.characters.RemoveAt(k);
					}
				}
			}
		}

		public void UpdateForRenovation()
		{
			updateFarmLayout();
			setWallpapers();
			setFloors();
		}

		public void updateFarmLayout()
		{
			if (currentlyDisplayedUpgradeLevel != upgradeLevel)
			{
				setMapForUpgradeLevel(upgradeLevel);
			}
			_ApplyRenovations();
			if ((!displayingSpouseRoom && shouldShowSpouseRoom()) || (displayingSpouseRoom && !shouldShowSpouseRoom()))
			{
				showSpouseRoom();
			}
			UpdateChildRoom();
			ReadWallpaperAndFloorTileData();
		}

		protected virtual void _ApplyRenovations()
		{
			if (upgradeLevel >= 2)
			{
				if (_appliedMapOverrides.Contains("bedroom_open"))
				{
					_appliedMapOverrides.Remove("bedroom_open");
				}
				if (owner.mailReceived.Contains("renovation_bedroom_open"))
				{
					ApplyMapOverride("FarmHouse_Bedroom_Open", "bedroom_open");
				}
				else
				{
					ApplyMapOverride("FarmHouse_Bedroom_Normal", "bedroom_open");
				}
				if (_appliedMapOverrides.Contains("southernroom_open"))
				{
					_appliedMapOverrides.Remove("southernroom_open");
				}
				if (owner.mailReceived.Contains("renovation_southern_open"))
				{
					ApplyMapOverride("FarmHouse_SouthernRoom_Add", "southernroom_open");
				}
				else
				{
					ApplyMapOverride("FarmHouse_SouthernRoom_Remove", "southernroom_open");
				}
				if (_appliedMapOverrides.Contains("cornerroom_open"))
				{
					_appliedMapOverrides.Remove("cornerroom_open");
				}
				if (owner.mailReceived.Contains("renovation_corner_open"))
				{
					ApplyMapOverride("FarmHouse_CornerRoom_Add", "cornerroom_open");
					if (displayingSpouseRoom)
					{
						setMapTile(34, 9, 229, "Front", null, 2);
					}
				}
				else
				{
					ApplyMapOverride("FarmHouse_CornerRoom_Remove", "cornerroom_open");
					if (displayingSpouseRoom)
					{
						setMapTile(34, 9, 87, "Front", null, 2);
					}
				}
			}
			if (!map.Properties.ContainsKey("AdditionalRenovations"))
			{
				return;
			}
			string[] array = map.Properties["AdditionalRenovations"].ToString().Split(',');
			for (int i = 0; i < array.Length; i++)
			{
				string[] data_split = array[i].Trim().Split(' ');
				if (data_split.Length < 4)
				{
					continue;
				}
				string map_patch_id = data_split[0];
				string required_mail = data_split[1];
				string add_map_override = data_split[2];
				string remove_map_override = data_split[3];
				Microsoft.Xna.Framework.Rectangle? destination_rect = null;
				if (data_split.Length >= 8)
				{
					try
					{
						Microsoft.Xna.Framework.Rectangle rectangle = default(Microsoft.Xna.Framework.Rectangle);
						rectangle.X = int.Parse(data_split[4]);
						rectangle.Y = int.Parse(data_split[5]);
						rectangle.Width = int.Parse(data_split[6]);
						rectangle.Height = int.Parse(data_split[7]);
						destination_rect = rectangle;
					}
					catch (Exception)
					{
						destination_rect = null;
					}
				}
				if (_appliedMapOverrides.Contains(map_patch_id))
				{
					_appliedMapOverrides.Remove(map_patch_id);
				}
				if (owner.mailReceived.Contains(required_mail))
				{
					ApplyMapOverride(add_map_override, map_patch_id, null, destination_rect);
				}
				else
				{
					ApplyMapOverride(remove_map_override, map_patch_id, null, destination_rect);
				}
			}
		}

		public override void MakeMapModifications(bool force = false)
		{
			base.MakeMapModifications(force);
			updateFarmLayout();
			setWallpapers();
			setFloors();
			if (owner.getSpouse() != null && owner.getSpouse().name.Equals("Sebastian") && Game1.netWorldState.Value.hasWorldStateID("sebastianFrog"))
			{
				Point frog_spot = GetSpouseRoomCorner();
				frog_spot.X++;
				frog_spot.Y += 6;
				Vector2 spot = Utility.PointToVector2(frog_spot);
				removeTile((int)spot.X, (int)spot.Y - 1, "Front");
				removeTile((int)spot.X + 1, (int)spot.Y - 1, "Front");
				removeTile((int)spot.X + 2, (int)spot.Y - 1, "Front");
			}
		}

		protected override void resetLocalState()
		{
			base.resetLocalState();
			if (owner.isMarried() && owner.spouse != null && owner.spouse.Equals("Emily") && Game1.player.eventsSeen.Contains(463391))
			{
				Vector2 parrotSpot = new Vector2(2064f, 160f);
				int num = upgradeLevel;
				if ((uint)(num - 2) <= 1u)
				{
					parrotSpot = new Vector2(2448f, 736f);
				}
				temporarySprites.Add(new EmilysParrot(parrotSpot));
			}
			if (Game1.player.currentLocation == null || (!Game1.player.currentLocation.Equals(this) && !Game1.player.currentLocation.name.Value.StartsWith("Cellar")))
			{
				Game1.player.Position = Utility.PointToVector2(getEntryLocation()) * 64f;
				Game1.xLocationAfterWarp = Game1.player.getTileX();
				Game1.yLocationAfterWarp = Game1.player.getTileY();
				Game1.player.currentLocation = this;
			}
			foreach (NPC i in characters)
			{
				if (i is Child)
				{
					(i as Child).resetForPlayerEntry(this);
				}
				if (Game1.IsMasterGame && Game1.timeOfDay >= 2000 && !(i is Pet))
				{
					i.controller = null;
					i.Halt();
				}
			}
			if (owner == Game1.player && Game1.player.team.GetSpouse(Game1.player.UniqueMultiplayerID).HasValue && Game1.player.team.IsMarried(Game1.player.UniqueMultiplayerID) && !Game1.player.mailReceived.Contains("CF_Spouse"))
			{
				Vector2 chestPosition = Utility.PointToVector2(getEntryLocation()) + new Vector2(0f, -1f);
				Chest chest = new Chest(0, new List<Item>
				{
					new Object(434, 1)
				}, chestPosition, giftbox: true, 1);
				overlayObjects[chestPosition] = chest;
			}
			if (owner != null && !owner.activeDialogueEvents.ContainsKey("pennyRedecorating") && !owner.mailReceived.Contains("pennyQuilt0") && !owner.mailReceived.Contains("pennyQuilt1"))
			{
				owner.mailReceived.Contains("pennyQuilt2");
			}
			if (owner.Equals(Game1.player) && !Game1.player.activeDialogueEvents.ContainsKey("pennyRedecorating"))
			{
				int whichQuilt = -1;
				if (Game1.player.mailReceived.Contains("pennyQuilt0"))
				{
					whichQuilt = 0;
				}
				else if (Game1.player.mailReceived.Contains("pennyQuilt1"))
				{
					whichQuilt = 1;
				}
				else if (Game1.player.mailReceived.Contains("pennyQuilt2"))
				{
					whichQuilt = 2;
				}
				if (whichQuilt != -1 && !Game1.player.mailReceived.Contains("pennyRefurbished"))
				{
					List<Object> objectsPickedUp = new List<Object>();
					foreach (Furniture f in furniture)
					{
						if (!(f is BedFurniture))
						{
							continue;
						}
						BedFurniture bed_furniture = f as BedFurniture;
						if (bed_furniture.bedType == BedFurniture.BedType.Double)
						{
							int bed_index = -1;
							if (owner.mailReceived.Contains("pennyQuilt0"))
							{
								bed_index = 2058;
							}
							if (owner.mailReceived.Contains("pennyQuilt1"))
							{
								bed_index = 2064;
							}
							if (owner.mailReceived.Contains("pennyQuilt2"))
							{
								bed_index = 2070;
							}
							if (bed_index != -1)
							{
								Vector2 tile_location = bed_furniture.TileLocation;
								bed_furniture.performRemoveAction(bed_furniture.tileLocation, this);
								objectsPickedUp.Add(bed_furniture);
								Guid guid = furniture.GuidOf(bed_furniture);
								furniture.Remove(guid);
								furniture.Add(new BedFurniture(bed_index, new Vector2(tile_location.X, tile_location.Y)));
							}
							break;
						}
					}
					Game1.player.mailReceived.Add("pennyRefurbished");
					Microsoft.Xna.Framework.Rectangle roomToRedecorate = Microsoft.Xna.Framework.Rectangle.Empty;
					roomToRedecorate = ((upgradeLevel < 2) ? new Microsoft.Xna.Framework.Rectangle(20, 1, 8, 10) : new Microsoft.Xna.Framework.Rectangle(23, 10, 11, 13));
					for (int x = roomToRedecorate.X; x <= roomToRedecorate.Right; x++)
					{
						for (int y = roomToRedecorate.Y; y <= roomToRedecorate.Bottom; y++)
						{
							if (getObjectAtTile(x, y) == null)
							{
								continue;
							}
							Object o = null;
							o = getObjectAtTile(x, y);
							if (o != null && !(o is Chest) && !(o is StorageFurniture) && !(o is IndoorPot) && !(o is BedFurniture))
							{
								if (o.Name != null && o.Name.Contains("Table") && o.heldObject.Value != null)
								{
									Object held_object = o.heldObject.Value;
									o.heldObject.Value = null;
									objectsPickedUp.Add(held_object);
								}
								o.performRemoveAction(new Vector2(x, y), this);
								if (o is Fence)
								{
									o = new Object(Vector2.Zero, (o as Fence).GetItemParentSheetIndex(), 1);
								}
								objectsPickedUp.Add(o);
								objects.Remove(new Vector2(x, y));
								if (o is Furniture)
								{
									furniture.Remove(o as Furniture);
								}
							}
						}
					}
					decoratePennyRoom(whichQuilt, objectsPickedUp);
				}
			}
			if (owner.getSpouse() == null || !owner.getSpouse().name.Equals("Sebastian") || !Game1.netWorldState.Value.hasWorldStateID("sebastianFrog"))
			{
				return;
			}
			Point frog_spot = GetSpouseRoomCorner();
			frog_spot.X++;
			frog_spot.Y += 6;
			Vector2 spot = Utility.PointToVector2(frog_spot);
			temporarySprites.Add(new TemporaryAnimatedSprite
			{
				texture = Game1.mouseCursors,
				sourceRect = new Microsoft.Xna.Framework.Rectangle(641, 1534, 48, 37),
				animationLength = 1,
				sourceRectStartingPos = new Vector2(641f, 1534f),
				interval = 5000f,
				totalNumberOfLoops = 9999,
				position = spot * 64f + new Vector2(0f, -5f) * 4f,
				scale = 4f,
				layerDepth = (spot.Y + 2f + 0.1f) * 64f / 10000f
			});
			if (Game1.random.NextDouble() < 0.85)
			{
				Texture2D crittersText3 = Game1.temporaryContent.Load<Texture2D>("TileSheets\\critters");
				base.TemporarySprites.Add(new SebsFrogs
				{
					texture = crittersText3,
					sourceRect = new Microsoft.Xna.Framework.Rectangle(64, 224, 16, 16),
					animationLength = 1,
					sourceRectStartingPos = new Vector2(64f, 224f),
					interval = 100f,
					totalNumberOfLoops = 9999,
					position = spot * 64f + new Vector2((Game1.random.NextDouble() < 0.5) ? 22 : 25, (!(Game1.random.NextDouble() < 0.5)) ? 1 : 2) * 4f,
					scale = 4f,
					flipped = (Game1.random.NextDouble() < 0.5),
					layerDepth = (spot.Y + 2f + 0.11f) * 64f / 10000f,
					Parent = this
				});
			}
			if (!Game1.player.activeDialogueEvents.ContainsKey("sebastianFrog2") && Game1.random.NextDouble() < 0.5)
			{
				Texture2D crittersText2 = Game1.temporaryContent.Load<Texture2D>("TileSheets\\critters");
				base.TemporarySprites.Add(new SebsFrogs
				{
					texture = crittersText2,
					sourceRect = new Microsoft.Xna.Framework.Rectangle(64, 240, 16, 16),
					animationLength = 1,
					sourceRectStartingPos = new Vector2(64f, 240f),
					interval = 150f,
					totalNumberOfLoops = 9999,
					position = spot * 64f + new Vector2(8f, 3f) * 4f,
					scale = 4f,
					layerDepth = (spot.Y + 2f + 0.11f) * 64f / 10000f,
					flipped = (Game1.random.NextDouble() < 0.5),
					pingPong = false,
					Parent = this
				});
				if (Game1.random.NextDouble() < 0.1 && Game1.timeOfDay > 610)
				{
					DelayedAction.playSoundAfterDelay("croak", 1000);
				}
			}
		}

		private void addFurnitureIfSpaceIsFreePenny(List<Object> objectsToStoreInChests, Furniture f, Furniture heldObject = null)
		{
			bool fail = false;
			foreach (Furniture furniture in base.furniture)
			{
				if (f.getBoundingBox(f.tileLocation).Intersects(furniture.getBoundingBox(furniture.tileLocation)))
				{
					fail = true;
					break;
				}
			}
			if (objects.ContainsKey(f.TileLocation))
			{
				fail = true;
			}
			if (!fail)
			{
				base.furniture.Add(f);
				if (heldObject != null)
				{
					base.furniture.Last().heldObject.Value = heldObject;
				}
			}
			else
			{
				objectsToStoreInChests.Add(f);
				if (heldObject != null)
				{
					objectsToStoreInChests.Add(heldObject);
				}
			}
		}

		private void addFurnitureIfSpaceIsFree(Furniture f, Furniture heldObject = null)
		{
			if (!objects.ContainsKey(f.TileLocation))
			{
				furniture.Add(f);
				if (heldObject != null)
				{
					furniture.Last().heldObject.Value = heldObject;
				}
			}
		}

		private void decoratePennyRoom(int whichStyle, List<Object> objectsToStoreInChests)
		{
			List<Chest> chests = new List<Chest>();
			List<Vector2> chest_positions = new List<Vector2>();
			Color chest_color = default(Color);
			switch (whichStyle)
			{
			case 0:
				if (upgradeLevel == 1)
				{
					addFurnitureIfSpaceIsFreePenny(objectsToStoreInChests, new Furniture(1916, new Vector2(20f, 1f)));
					addFurnitureIfSpaceIsFreePenny(objectsToStoreInChests, new Furniture(1914, new Vector2(21f, 1f)));
					addFurnitureIfSpaceIsFreePenny(objectsToStoreInChests, new Furniture(1915, new Vector2(22f, 1f)));
					addFurnitureIfSpaceIsFreePenny(objectsToStoreInChests, new Furniture(1914, new Vector2(23f, 1f)));
					addFurnitureIfSpaceIsFreePenny(objectsToStoreInChests, new Furniture(1916, new Vector2(24f, 1f)));
					addFurnitureIfSpaceIsFreePenny(objectsToStoreInChests, new Furniture(1682, new Vector2(26f, 1f)));
					addFurnitureIfSpaceIsFreePenny(objectsToStoreInChests, new Furniture(1747, new Vector2(25f, 4f)));
					addFurnitureIfSpaceIsFreePenny(objectsToStoreInChests, new Furniture(1395, new Vector2(26f, 4f)), new Furniture(1363, Vector2.Zero));
					addFurnitureIfSpaceIsFreePenny(objectsToStoreInChests, new Furniture(1443, new Vector2(27f, 4f)));
					addFurnitureIfSpaceIsFreePenny(objectsToStoreInChests, new Furniture(1664, new Vector2(27f, 5f), 1));
					addFurnitureIfSpaceIsFreePenny(objectsToStoreInChests, new Furniture(1978, new Vector2(21f, 6f)));
					addFurnitureIfSpaceIsFreePenny(objectsToStoreInChests, new Furniture(1124, new Vector2(26f, 9f)), new Furniture(1368, Vector2.Zero));
					addFurnitureIfSpaceIsFreePenny(objectsToStoreInChests, new Furniture(6, new Vector2(25f, 10f), 1));
					addFurnitureIfSpaceIsFreePenny(objectsToStoreInChests, new Furniture(1296, new Vector2(28f, 10f)));
					addFurnitureIfSpaceIsFreePenny(objectsToStoreInChests, new Furniture(1747, new Vector2(24f, 10f)));
					SetWallpaper("107", "Bedroom");
					SetFloor("2", "Bedroom");
					chest_color = new Color(85, 85, 255);
					chest_positions.Add(new Vector2(21f, 10f));
					chest_positions.Add(new Vector2(22f, 10f));
				}
				else
				{
					addFurnitureIfSpaceIsFreePenny(objectsToStoreInChests, new Furniture(1916, new Vector2(23f, 10f)));
					addFurnitureIfSpaceIsFreePenny(objectsToStoreInChests, new Furniture(1914, new Vector2(24f, 10f)));
					addFurnitureIfSpaceIsFreePenny(objectsToStoreInChests, new Furniture(1604, new Vector2(26f, 10f)));
					addFurnitureIfSpaceIsFreePenny(objectsToStoreInChests, new Furniture(1915, new Vector2(28f, 10f)));
					addFurnitureIfSpaceIsFreePenny(objectsToStoreInChests, new Furniture(1916, new Vector2(30f, 10f)));
					addFurnitureIfSpaceIsFreePenny(objectsToStoreInChests, new Furniture(1914, new Vector2(32f, 10f)));
					addFurnitureIfSpaceIsFreePenny(objectsToStoreInChests, new Furniture(1916, new Vector2(33f, 10f)));
					addFurnitureIfSpaceIsFreePenny(objectsToStoreInChests, new Furniture(1443, new Vector2(23f, 13f)));
					addFurnitureIfSpaceIsFreePenny(objectsToStoreInChests, new Furniture(1747, new Vector2(24f, 13f)));
					addFurnitureIfSpaceIsFreePenny(objectsToStoreInChests, new Furniture(1395, new Vector2(25f, 13f)), new Furniture(1363, Vector2.Zero));
					addFurnitureIfSpaceIsFreePenny(objectsToStoreInChests, new Furniture(714, new Vector2(31f, 13f)));
					addFurnitureIfSpaceIsFreePenny(objectsToStoreInChests, new Furniture(1443, new Vector2(33f, 13f)));
					addFurnitureIfSpaceIsFreePenny(objectsToStoreInChests, new Furniture(1978, new Vector2(27f, 15f)));
					addFurnitureIfSpaceIsFreePenny(objectsToStoreInChests, new Furniture(1664, new Vector2(32f, 15f), 1));
					addFurnitureIfSpaceIsFreePenny(objectsToStoreInChests, new Furniture(1664, new Vector2(23f, 17f), 1));
					addFurnitureIfSpaceIsFreePenny(objectsToStoreInChests, new Furniture(1124, new Vector2(31f, 21f)), new Furniture(1368, Vector2.Zero));
					addFurnitureIfSpaceIsFreePenny(objectsToStoreInChests, new Furniture(416, new Vector2(25f, 22f), 2));
					addFurnitureIfSpaceIsFreePenny(objectsToStoreInChests, new Furniture(1296, new Vector2(23f, 22f)));
					addFurnitureIfSpaceIsFreePenny(objectsToStoreInChests, new Furniture(6, new Vector2(30f, 22f), 1));
					addFurnitureIfSpaceIsFreePenny(objectsToStoreInChests, new Furniture(1296, new Vector2(33f, 22f)));
					SetWallpaper("107", "Bedroom");
					SetFloor("2", "Bedroom");
					chest_color = new Color(85, 85, 255);
					chest_positions.Add(new Vector2(23f, 14f));
					chest_positions.Add(new Vector2(24f, 14f));
				}
				break;
			case 1:
				if (upgradeLevel == 1)
				{
					addFurnitureIfSpaceIsFreePenny(objectsToStoreInChests, new Furniture(1678, new Vector2(20f, 1f)));
					addFurnitureIfSpaceIsFreePenny(objectsToStoreInChests, new Furniture(1814, new Vector2(21f, 1f)));
					addFurnitureIfSpaceIsFreePenny(objectsToStoreInChests, new Furniture(1814, new Vector2(22f, 1f)));
					addFurnitureIfSpaceIsFreePenny(objectsToStoreInChests, new Furniture(1814, new Vector2(23f, 1f)));
					addFurnitureIfSpaceIsFreePenny(objectsToStoreInChests, new Furniture(1907, new Vector2(24f, 1f)));
					addFurnitureIfSpaceIsFreePenny(objectsToStoreInChests, new Furniture(1400, new Vector2(25f, 4f)), new Furniture(1365, Vector2.Zero));
					addFurnitureIfSpaceIsFreePenny(objectsToStoreInChests, new Furniture(1866, new Vector2(26f, 4f)));
					addFurnitureIfSpaceIsFreePenny(objectsToStoreInChests, new Furniture(1909, new Vector2(27f, 6f), 1));
					addFurnitureIfSpaceIsFreePenny(objectsToStoreInChests, new Furniture(1451, new Vector2(21f, 6f)));
					addFurnitureIfSpaceIsFreePenny(objectsToStoreInChests, new Furniture(1138, new Vector2(27f, 9f)), new Furniture(1378, Vector2.Zero));
					addFurnitureIfSpaceIsFreePenny(objectsToStoreInChests, new Furniture(12, new Vector2(26f, 10f), 1));
					addFurnitureIfSpaceIsFreePenny(objectsToStoreInChests, new Furniture(1758, new Vector2(24f, 10f)));
					addFurnitureIfSpaceIsFreePenny(objectsToStoreInChests, new Furniture(1618, new Vector2(21f, 9f)));
					addFurnitureIfSpaceIsFreePenny(objectsToStoreInChests, new Furniture(1390, new Vector2(22f, 10f)));
					SetWallpaper("84", "Bedroom");
					SetFloor("35", "Bedroom");
					chest_color = new Color(255, 85, 85);
					chest_positions.Add(new Vector2(21f, 10f));
					chest_positions.Add(new Vector2(23f, 10f));
				}
				else
				{
					addFurnitureIfSpaceIsFreePenny(objectsToStoreInChests, new Furniture(1678, new Vector2(24f, 10f)));
					addFurnitureIfSpaceIsFreePenny(objectsToStoreInChests, new Furniture(1907, new Vector2(25f, 10f)));
					addFurnitureIfSpaceIsFreePenny(objectsToStoreInChests, new Furniture(1814, new Vector2(27f, 10f)));
					addFurnitureIfSpaceIsFreePenny(objectsToStoreInChests, new Furniture(1814, new Vector2(28f, 10f)));
					addFurnitureIfSpaceIsFreePenny(objectsToStoreInChests, new Furniture(1814, new Vector2(29f, 10f)));
					addFurnitureIfSpaceIsFreePenny(objectsToStoreInChests, new Furniture(1907, new Vector2(30f, 10f)));
					addFurnitureIfSpaceIsFreePenny(objectsToStoreInChests, new Furniture(1916, new Vector2(33f, 10f)));
					addFurnitureIfSpaceIsFreePenny(objectsToStoreInChests, new Furniture(1758, new Vector2(23f, 13f)));
					addFurnitureIfSpaceIsFreePenny(objectsToStoreInChests, new Furniture(1400, new Vector2(25f, 13f)), new Furniture(1365, Vector2.Zero));
					addFurnitureIfSpaceIsFreePenny(objectsToStoreInChests, new Furniture(1390, new Vector2(31f, 13f)));
					addFurnitureIfSpaceIsFreePenny(objectsToStoreInChests, new Furniture(1866, new Vector2(32f, 13f)));
					addFurnitureIfSpaceIsFreePenny(objectsToStoreInChests, new Furniture(1387, new Vector2(23f, 14f)));
					addFurnitureIfSpaceIsFreePenny(objectsToStoreInChests, new Furniture(1909, new Vector2(32f, 14f), 1));
					addFurnitureIfSpaceIsFreePenny(objectsToStoreInChests, new Furniture(719, new Vector2(23f, 15f), 1));
					addFurnitureIfSpaceIsFreePenny(objectsToStoreInChests, new Furniture(1451, new Vector2(27f, 15f)));
					addFurnitureIfSpaceIsFreePenny(objectsToStoreInChests, new Furniture(1909, new Vector2(23f, 17f), 1));
					addFurnitureIfSpaceIsFreePenny(objectsToStoreInChests, new Furniture(1389, new Vector2(32f, 19f)));
					addFurnitureIfSpaceIsFreePenny(objectsToStoreInChests, new Furniture(1377, new Vector2(33f, 19f)));
					addFurnitureIfSpaceIsFreePenny(objectsToStoreInChests, new Furniture(1758, new Vector2(26f, 20f)));
					addFurnitureIfSpaceIsFreePenny(objectsToStoreInChests, new Furniture(424, new Vector2(27f, 20f), 1));
					addFurnitureIfSpaceIsFreePenny(objectsToStoreInChests, new Furniture(1618, new Vector2(29f, 20f)));
					addFurnitureIfSpaceIsFreePenny(objectsToStoreInChests, new Furniture(536, new Vector2(32f, 20f), 3));
					addFurnitureIfSpaceIsFreePenny(objectsToStoreInChests, new Furniture(1138, new Vector2(23f, 21f)), new Furniture(1378, Vector2.Zero));
					addFurnitureIfSpaceIsFreePenny(objectsToStoreInChests, new Furniture(1383, new Vector2(26f, 21f)));
					addFurnitureIfSpaceIsFreePenny(objectsToStoreInChests, new Furniture(1449, new Vector2(33f, 22f)));
					SetWallpaper("84", "Bedroom");
					SetFloor("35", "Bedroom");
					chest_color = new Color(255, 85, 85);
					chest_positions.Add(new Vector2(24f, 13f));
					chest_positions.Add(new Vector2(28f, 15f));
				}
				break;
			case 2:
				if (upgradeLevel == 1)
				{
					addFurnitureIfSpaceIsFreePenny(objectsToStoreInChests, new Furniture(1673, new Vector2(20f, 1f)));
					addFurnitureIfSpaceIsFreePenny(objectsToStoreInChests, new Furniture(1547, new Vector2(21f, 1f)));
					addFurnitureIfSpaceIsFreePenny(objectsToStoreInChests, new Furniture(1675, new Vector2(24f, 1f)));
					addFurnitureIfSpaceIsFreePenny(objectsToStoreInChests, new Furniture(1900, new Vector2(25f, 1f)));
					addFurnitureIfSpaceIsFreePenny(objectsToStoreInChests, new Furniture(1393, new Vector2(25f, 4f)), new Furniture(1367, Vector2.Zero));
					addFurnitureIfSpaceIsFreePenny(objectsToStoreInChests, new Furniture(1798, new Vector2(26f, 4f)));
					addFurnitureIfSpaceIsFreePenny(objectsToStoreInChests, new Furniture(1902, new Vector2(25f, 5f)));
					addFurnitureIfSpaceIsFreePenny(objectsToStoreInChests, new Furniture(1751, new Vector2(22f, 6f)));
					addFurnitureIfSpaceIsFreePenny(objectsToStoreInChests, new Furniture(1122, new Vector2(26f, 9f)), new Furniture(1378, Vector2.Zero));
					addFurnitureIfSpaceIsFreePenny(objectsToStoreInChests, new Furniture(197, new Vector2(28f, 9f), 3));
					addFurnitureIfSpaceIsFreePenny(objectsToStoreInChests, new Furniture(3, new Vector2(25f, 10f), 1));
					addFurnitureIfSpaceIsFreePenny(objectsToStoreInChests, new Furniture(1294, new Vector2(20f, 10f)));
					addFurnitureIfSpaceIsFreePenny(objectsToStoreInChests, new Furniture(1294, new Vector2(24f, 10f)));
					addFurnitureIfSpaceIsFreePenny(objectsToStoreInChests, new Furniture(1964, new Vector2(21f, 8f)));
					SetWallpaper("95", "Bedroom");
					SetFloor("1", "Bedroom");
					chest_color = new Color(85, 85, 85);
					chest_positions.Add(new Vector2(22f, 10f));
					chest_positions.Add(new Vector2(23f, 10f));
				}
				else
				{
					addFurnitureIfSpaceIsFreePenny(objectsToStoreInChests, new Furniture(1673, new Vector2(23f, 10f)));
					addFurnitureIfSpaceIsFreePenny(objectsToStoreInChests, new Furniture(1675, new Vector2(25f, 10f)));
					addFurnitureIfSpaceIsFreePenny(objectsToStoreInChests, new Furniture(1547, new Vector2(27f, 10f)));
					addFurnitureIfSpaceIsFreePenny(objectsToStoreInChests, new Furniture(1900, new Vector2(30f, 10f)));
					addFurnitureIfSpaceIsFreePenny(objectsToStoreInChests, new Furniture(1751, new Vector2(23f, 13f)));
					addFurnitureIfSpaceIsFreePenny(objectsToStoreInChests, new Furniture(1393, new Vector2(25f, 13f)), new Furniture(1367, Vector2.Zero));
					addFurnitureIfSpaceIsFreePenny(objectsToStoreInChests, new Furniture(1798, new Vector2(32f, 13f)));
					addFurnitureIfSpaceIsFreePenny(objectsToStoreInChests, new Furniture(1902, new Vector2(31f, 14f)));
					addFurnitureIfSpaceIsFreePenny(objectsToStoreInChests, new Furniture(1964, new Vector2(27f, 15f)));
					addFurnitureIfSpaceIsFreePenny(objectsToStoreInChests, new Furniture(1294, new Vector2(23f, 16f)));
					addFurnitureIfSpaceIsFreePenny(objectsToStoreInChests, new Furniture(3, new Vector2(31f, 19f)));
					addFurnitureIfSpaceIsFreePenny(objectsToStoreInChests, new Furniture(1294, new Vector2(23f, 20f)));
					addFurnitureIfSpaceIsFreePenny(objectsToStoreInChests, new Furniture(1122, new Vector2(31f, 20f)), new Furniture(1369, Vector2.Zero));
					addFurnitureIfSpaceIsFreePenny(objectsToStoreInChests, new Furniture(197, new Vector2(33f, 20f), 3));
					addFurnitureIfSpaceIsFreePenny(objectsToStoreInChests, new Furniture(709, new Vector2(23f, 21f), 1));
					addFurnitureIfSpaceIsFreePenny(objectsToStoreInChests, new Furniture(3, new Vector2(32f, 22f), 2));
					SetWallpaper("95", "Bedroom");
					SetFloor("1", "Bedroom");
					chest_color = new Color(85, 85, 85);
					chest_positions.Add(new Vector2(24f, 13f));
					chest_positions.Add(new Vector2(31f, 13f));
				}
				break;
			}
			if (objectsToStoreInChests != null)
			{
				foreach (Object o in objectsToStoreInChests)
				{
					if (chests.Count == 0)
					{
						chests.Add(new Chest(playerChest: true));
					}
					bool found_chest_to_stash_in = false;
					foreach (Chest item in chests)
					{
						if (item.addItem(o) == null)
						{
							found_chest_to_stash_in = true;
						}
					}
					if (!found_chest_to_stash_in)
					{
						Chest new_chest = new Chest(playerChest: true);
						chests.Add(new_chest);
						new_chest.addItem(o);
					}
				}
			}
			for (int i = 0; i < chests.Count; i++)
			{
				Chest chest = chests[i];
				chest.playerChoiceColor.Value = chest_color;
				Vector2 chest_position = chest_positions[Math.Min(i, chest_positions.Count - 1)];
				PlaceInNearbySpace(chest_position, chest);
			}
		}

		public void PlaceInNearbySpace(Vector2 tileLocation, Object o)
		{
			if (o == null || tileLocation.Equals(Vector2.Zero))
			{
				return;
			}
			int attempts = 0;
			Queue<Vector2> open_list = new Queue<Vector2>();
			HashSet<Vector2> closed_list = new HashSet<Vector2>();
			open_list.Enqueue(tileLocation);
			Vector2 current = Vector2.Zero;
			for (; attempts < 100; attempts++)
			{
				current = open_list.Dequeue();
				if (!isTileOccupiedForPlacement(current) && isTileLocationTotallyClearAndPlaceable(current) && !isOpenWater((int)current.X, (int)current.Y))
				{
					break;
				}
				closed_list.Add(current);
				foreach (Vector2 v in Utility.getAdjacentTileLocations(current))
				{
					if (!closed_list.Contains(v))
					{
						open_list.Enqueue(v);
					}
				}
			}
			if (!current.Equals(Vector2.Zero) && !isTileOccupiedForPlacement(current) && !isOpenWater((int)current.X, (int)current.Y) && isTileLocationTotallyClearAndPlaceable(current))
			{
				o.tileLocation.Value = current;
				objects.Add(current, o);
			}
		}

		public virtual void RefreshFloorObjectNeighbors()
		{
			foreach (Vector2 key in terrainFeatures.Keys)
			{
				TerrainFeature t = terrainFeatures[key];
				if (t is Flooring)
				{
					(t as Flooring).OnAdded(this, key);
				}
			}
		}

		public void moveObjectsForHouseUpgrade(int whichUpgrade)
		{
			previousUpgradeLevel = upgradeLevel;
			overlayObjects.Clear();
			switch (whichUpgrade)
			{
			case 0:
				if (upgradeLevel == 1)
				{
					shiftObjects(-6, 0);
				}
				break;
			case 1:
				if (upgradeLevel == 0)
				{
					shiftObjects(6, 0);
				}
				if (upgradeLevel == 2)
				{
					shiftObjects(-3, 0);
				}
				break;
			case 2:
			case 3:
				if (upgradeLevel == 1)
				{
					shiftObjects(3, 9);
					foreach (Furniture v in furniture)
					{
						if (v.tileLocation.X >= 10f && v.tileLocation.X <= 13f && v.tileLocation.Y >= 10f && v.tileLocation.Y <= 11f)
						{
							v.tileLocation.X -= 3f;
							v.boundingBox.X -= 192;
							v.tileLocation.Y -= 9f;
							v.boundingBox.Y -= 576;
							v.updateDrawPosition();
						}
					}
					moveFurniture(27, 13, 1, 4);
					moveFurniture(28, 13, 2, 4);
					moveFurniture(29, 13, 3, 4);
					moveFurniture(28, 14, 7, 4);
					moveFurniture(29, 14, 8, 4);
					moveFurniture(27, 14, 4, 4);
					moveFurniture(28, 15, 5, 4);
					moveFurniture(29, 16, 6, 4);
				}
				if (upgradeLevel == 0)
				{
					shiftObjects(9, 9);
				}
				break;
			}
		}

		protected override LocalizedContentManager getMapLoader()
		{
			if (mapLoader == null)
			{
				mapLoader = Game1.game1.xTileContent.CreateTemporary();
			}
			return mapLoader;
		}

		public override void drawAboveFrontLayer(SpriteBatch b)
		{
			base.drawAboveFrontLayer(b);
			if (fridge.Value.mutex.IsLocked())
			{
				b.Draw(Game1.mouseCursors2, Game1.GlobalToLocal(Game1.viewport, new Vector2(fridgePosition.X, fridgePosition.Y - 1) * 64f), new Microsoft.Xna.Framework.Rectangle(0, 192, 16, 32), Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, (float)((fridgePosition.Y + 1) * 64 + 1) / 10000f);
			}
		}

		public override void updateMap()
		{
			bool showSpouse = owner.spouse != null && owner.isMarried();
			mapPath.Value = "Maps\\FarmHouse" + ((upgradeLevel == 0) ? "" : ((upgradeLevel == 3) ? "2" : (upgradeLevel.ToString() ?? ""))) + (showSpouse ? "_marriage" : "");
			base.updateMap();
		}

		public virtual void setMapForUpgradeLevel(int level)
		{
			upgradeLevel = level;
			int previous_synchronized_displayed_level = synchronizedDisplayedLevel.Value;
			currentlyDisplayedUpgradeLevel = level;
			synchronizedDisplayedLevel.Value = level;
			bool showSpouse = owner.isMarried() && owner.spouse != null;
			if (displayingSpouseRoom && !showSpouse)
			{
				displayingSpouseRoom = false;
			}
			updateMap();
			RefreshFloorObjectNeighbors();
			if (showSpouse)
			{
				showSpouseRoom();
			}
			loadObjects();
			if (level == 3)
			{
				AddCellarTiles();
				createCellarWarps();
				if (!Game1.player.craftingRecipes.ContainsKey("Cask"))
				{
					Game1.player.craftingRecipes.Add("Cask", 0);
				}
			}
			bool need_bed_upgrade = false;
			if (previousUpgradeLevel == 0 && upgradeLevel >= 0)
			{
				need_bed_upgrade = true;
			}
			if (previousUpgradeLevel >= 0)
			{
				if (previousUpgradeLevel < 2 && upgradeLevel >= 2)
				{
					for (int x3 = 0; x3 < map.Layers[0].TileWidth; x3++)
					{
						for (int y3 = 0; y3 < map.Layers[0].TileHeight; y3++)
						{
							if (doesTileHaveProperty(x3, y3, "DefaultChildBedPosition", "Back") != null)
							{
								int bed_index2 = BedFurniture.CHILD_BED_INDEX;
								furniture.Add(new BedFurniture(bed_index2, new Vector2(x3, y3)));
								break;
							}
						}
					}
				}
				Furniture bed_furniture = null;
				if (previousUpgradeLevel == 0)
				{
					foreach (Furniture f2 in furniture)
					{
						if (f2 is BedFurniture && (f2 as BedFurniture).bedType == BedFurniture.BedType.Single)
						{
							bed_furniture = f2;
							break;
						}
					}
				}
				else
				{
					foreach (Furniture f in furniture)
					{
						if (f is BedFurniture && (f as BedFurniture).bedType == BedFurniture.BedType.Double)
						{
							bed_furniture = f;
							break;
						}
					}
				}
				if (upgradeLevel != 3 || need_bed_upgrade)
				{
					for (int x2 = 0; x2 < map.Layers[0].TileWidth; x2++)
					{
						for (int y2 = 0; y2 < map.Layers[0].TileHeight; y2++)
						{
							if (doesTileHaveProperty(x2, y2, "DefaultBedPosition", "Back") == null)
							{
								continue;
							}
							int bed_index = BedFurniture.DEFAULT_BED_INDEX;
							if (previousUpgradeLevel != 1 || bed_furniture == null || (bed_furniture.tileLocation.X == 24f && bed_furniture.tileLocation.Y == 12f))
							{
								if (bed_furniture != null)
								{
									bed_index = bed_furniture.ParentSheetIndex;
								}
								if (previousUpgradeLevel == 0 && bed_furniture != null)
								{
									bed_furniture.performRemoveAction(bed_furniture.tileLocation, this);
									Guid guid2 = furniture.GuidOf(bed_furniture);
									furniture.Remove(guid2);
									bed_index = Utility.GetDoubleWideVersionOfBed(bed_index);
									furniture.Add(new BedFurniture(bed_index, new Vector2(x2, y2)));
								}
								else if (bed_furniture != null)
								{
									bed_furniture.performRemoveAction(bed_furniture.tileLocation, this);
									Guid guid = furniture.GuidOf(bed_furniture);
									furniture.Remove(guid);
									furniture.Add(new BedFurniture(bed_furniture.ParentSheetIndex, new Vector2(x2, y2)));
								}
							}
							break;
						}
					}
				}
				previousUpgradeLevel = -1;
			}
			if (previous_synchronized_displayed_level != level)
			{
				lightGlows.Clear();
			}
			fridgePosition = default(Point);
			bool found_fridge = false;
			for (int x = 0; x < map.GetLayer("Buildings").LayerWidth; x++)
			{
				for (int y = 0; y < map.GetLayer("Buildings").LayerHeight; y++)
				{
					if (map.GetLayer("Buildings").Tiles[x, y] != null && map.GetLayer("Buildings").Tiles[x, y].TileIndex == 173)
					{
						fridgePosition = new Point(x, y);
						found_fridge = true;
						break;
					}
				}
				if (found_fridge)
				{
					break;
				}
			}
		}

		public void createCellarWarps()
		{
			updateCellarWarps();
		}

		public void updateCellarWarps()
		{
			Layer back_layer = map.GetLayer("Back");
			for (int x = 0; x < back_layer.LayerWidth; x++)
			{
				for (int y = 0; y < back_layer.LayerHeight; y++)
				{
					string touch_action = doesTileHaveProperty(x, y, "TouchAction", "Back");
					if (touch_action != null && touch_action.StartsWith("Warp "))
					{
						string[] split = touch_action.Split(' ');
						if (split.Length >= 2 && split[1].StartsWith("Cellar"))
						{
							split[1] = GetCellarName();
							setTileProperty(x, y, "Back", "TouchAction", string.Join(" ", split));
						}
					}
				}
			}
			if (cellarWarps == null)
			{
				return;
			}
			foreach (Warp warp in cellarWarps)
			{
				if (!warps.Contains(warp))
				{
					warps.Add(warp);
				}
				warp.TargetName = GetCellarName();
			}
		}

		public virtual int GetSpouseRoomWidth()
		{
			return 6;
		}

		public virtual int GetSpouseRoomHeight()
		{
			return 9;
		}

		public virtual Point GetSpouseRoomCorner()
		{
			if (upgradeLevel == 1)
			{
				return new Point(29, 1);
			}
			return new Point(35, 10);
		}

		public virtual void loadSpouseRoom()
		{
			NPC spouse = owner.getSpouse();
			spouseRoomSpot = GetSpouseRoomCorner();
			spouseRoomSpot.X += 3;
			spouseRoomSpot.Y += 4;
			if (spouse == null)
			{
				return;
			}
			Dictionary<string, string> room_data = Game1.content.Load<Dictionary<string, string>>("Data\\SpouseRooms");
			int indexInSpouseMapSheet = -1;
			string map_path = "spouseRooms";
			if (room_data != null && room_data.ContainsKey(spouse.Name))
			{
				try
				{
					string[] array = room_data[spouse.Name].Split('/');
					map_path = array[0];
					indexInSpouseMapSheet = int.Parse(array[1]);
				}
				catch (Exception)
				{
				}
			}
			if (indexInSpouseMapSheet == -1)
			{
				switch (spouse.Name)
				{
				case "Abigail":
					indexInSpouseMapSheet = 0;
					break;
				case "Penny":
					indexInSpouseMapSheet = 1;
					break;
				case "Leah":
					indexInSpouseMapSheet = 2;
					break;
				case "Haley":
					indexInSpouseMapSheet = 3;
					break;
				case "Maru":
					indexInSpouseMapSheet = 4;
					break;
				case "Sebastian":
					indexInSpouseMapSheet = 5;
					break;
				case "Alex":
					indexInSpouseMapSheet = 6;
					break;
				case "Harvey":
					indexInSpouseMapSheet = 7;
					break;
				case "Elliott":
					indexInSpouseMapSheet = 8;
					break;
				case "Sam":
					indexInSpouseMapSheet = 9;
					break;
				case "Shane":
					indexInSpouseMapSheet = 10;
					break;
				case "Emily":
					indexInSpouseMapSheet = 11;
					break;
				case "Krobus":
					indexInSpouseMapSheet = 12;
					break;
				}
			}
			int width = GetSpouseRoomWidth();
			int height = GetSpouseRoomHeight();
			Point corner = GetSpouseRoomCorner();
			Microsoft.Xna.Framework.Rectangle areaToRefurbish = new Microsoft.Xna.Framework.Rectangle(corner.X, corner.Y, width, height);
			Map refurbishedMap = Game1.game1.xTileContent.Load<Map>("Maps\\" + map_path);
			int columns = refurbishedMap.Layers[0].LayerWidth / width;
			_ = refurbishedMap.Layers[0].LayerHeight / height;
			Point mapReader = new Point(indexInSpouseMapSheet % columns * width, indexInSpouseMapSheet / columns * height);
			map.Properties.Remove("DayTiles");
			map.Properties.Remove("NightTiles");
			List<KeyValuePair<Point, Tile>> bottom_row_tiles = new List<KeyValuePair<Point, Tile>>();
			Layer front_layer = map.GetLayer("Front");
			for (int x3 = areaToRefurbish.Left; x3 < areaToRefurbish.Right; x3++)
			{
				Point point = new Point(x3, areaToRefurbish.Bottom - 1);
				Tile tile = front_layer.Tiles[point.X, point.Y];
				if (tile != null)
				{
					bottom_row_tiles.Add(new KeyValuePair<Point, Tile>(point, tile));
				}
			}
			if (_appliedMapOverrides.Contains("spouse_room"))
			{
				_appliedMapOverrides.Remove("spouse_room");
			}
			ApplyMapOverride(map_path, "spouse_room", new Microsoft.Xna.Framework.Rectangle(mapReader.X, mapReader.Y, areaToRefurbish.Width, areaToRefurbish.Height), areaToRefurbish);
			for (int x2 = 0; x2 < areaToRefurbish.Width; x2++)
			{
				for (int y = 0; y < areaToRefurbish.Height; y++)
				{
					if (refurbishedMap.GetLayer("Buildings").Tiles[mapReader.X + x2, mapReader.Y + y] != null)
					{
						adjustMapLightPropertiesForLamp(refurbishedMap.GetLayer("Buildings").Tiles[mapReader.X + x2, mapReader.Y + y].TileIndex, areaToRefurbish.X + x2, areaToRefurbish.Y + y, "Buildings");
					}
					if (y < areaToRefurbish.Height - 1 && refurbishedMap.GetLayer("Front").Tiles[mapReader.X + x2, mapReader.Y + y] != null)
					{
						adjustMapLightPropertiesForLamp(refurbishedMap.GetLayer("Front").Tiles[mapReader.X + x2, mapReader.Y + y].TileIndex, areaToRefurbish.X + x2, areaToRefurbish.Y + y, "Front");
					}
				}
			}
			bool spot_found = false;
			for (int x = areaToRefurbish.Left; x < areaToRefurbish.Right; x++)
			{
				for (int y2 = areaToRefurbish.Top; y2 < areaToRefurbish.Bottom; y2++)
				{
					if (getTileIndexAt(new Point(x, y2), "Paths") == 7)
					{
						spot_found = true;
						spouseRoomSpot = new Point(x, y2);
						break;
					}
				}
				if (spot_found)
				{
					break;
				}
			}
			Point spouse_room_spot = GetSpouseRoomSpot();
			setTileProperty(spouse_room_spot.X, spouse_room_spot.Y, "Back", "NoFurniture", "T");
			foreach (KeyValuePair<Point, Tile> kvp in bottom_row_tiles)
			{
				front_layer.Tiles[kvp.Key.X, kvp.Key.Y] = kvp.Value;
			}
		}

		public virtual Microsoft.Xna.Framework.Rectangle? GetCribBounds()
		{
			if (upgradeLevel < 2)
			{
				return null;
			}
			return new Microsoft.Xna.Framework.Rectangle(15, 2, 3, 4);
		}

		public virtual Microsoft.Xna.Framework.Rectangle? GetBedBounds(int child_index = 0)
		{
			if (upgradeLevel < 2)
			{
				return null;
			}
			return child_index switch
			{
				0 => new Microsoft.Xna.Framework.Rectangle(22, 3, 2, 4), 
				1 => new Microsoft.Xna.Framework.Rectangle(26, 3, 2, 4), 
				_ => null, 
			};
		}

		public virtual Microsoft.Xna.Framework.Rectangle? GetChildBedBounds(int child_index = 0)
		{
			if (upgradeLevel < 2)
			{
				return null;
			}
			return child_index switch
			{
				0 => new Microsoft.Xna.Framework.Rectangle(22, 3, 2, 4), 
				1 => new Microsoft.Xna.Framework.Rectangle(26, 3, 2, 4), 
				_ => null, 
			};
		}

		public virtual void UpdateChildRoom()
		{
			Microsoft.Xna.Framework.Rectangle? crib_location = GetCribBounds();
			if (crib_location.HasValue)
			{
				if (_appliedMapOverrides.Contains("crib"))
				{
					_appliedMapOverrides.Remove("crib");
				}
				ApplyMapOverride("FarmHouse_Crib_" + cribStyle.Value, "crib", null, crib_location);
			}
		}

		public void playerDivorced()
		{
			displayingSpouseRoom = false;
		}

		public virtual List<Microsoft.Xna.Framework.Rectangle> getForbiddenPetWarpTiles()
		{
			List<Microsoft.Xna.Framework.Rectangle> forbidden_tiles = new List<Microsoft.Xna.Framework.Rectangle>();
			switch (upgradeLevel)
			{
			case 0:
				forbidden_tiles.Add(new Microsoft.Xna.Framework.Rectangle(2, 8, 3, 4));
				break;
			case 1:
				forbidden_tiles.Add(new Microsoft.Xna.Framework.Rectangle(8, 8, 3, 4));
				forbidden_tiles.Add(new Microsoft.Xna.Framework.Rectangle(17, 8, 4, 3));
				break;
			case 2:
			case 3:
				forbidden_tiles.Add(new Microsoft.Xna.Framework.Rectangle(11, 17, 3, 4));
				forbidden_tiles.Add(new Microsoft.Xna.Framework.Rectangle(20, 17, 4, 3));
				forbidden_tiles.Add(new Microsoft.Xna.Framework.Rectangle(12, 5, 4, 3));
				forbidden_tiles.Add(new Microsoft.Xna.Framework.Rectangle(11, 7, 2, 6));
				break;
			}
			return forbidden_tiles;
		}

		public bool canPetWarpHere(Vector2 tile_position)
		{
			foreach (Microsoft.Xna.Framework.Rectangle forbiddenPetWarpTile in getForbiddenPetWarpTiles())
			{
				if (forbiddenPetWarpTile.Contains((int)tile_position.X, (int)tile_position.Y))
				{
					return false;
				}
			}
			return true;
		}

		public override List<Microsoft.Xna.Framework.Rectangle> getWalls()
		{
			List<Microsoft.Xna.Framework.Rectangle> walls = new List<Microsoft.Xna.Framework.Rectangle>();
			switch (upgradeLevel)
			{
			case 0:
				walls.Add(new Microsoft.Xna.Framework.Rectangle(1, 1, 10, 3));
				break;
			case 1:
				walls.Add(new Microsoft.Xna.Framework.Rectangle(1, 1, 17, 3));
				walls.Add(new Microsoft.Xna.Framework.Rectangle(18, 6, 2, 2));
				walls.Add(new Microsoft.Xna.Framework.Rectangle(20, 1, 9, 3));
				break;
			case 2:
			case 3:
			{
				walls.Add(new Microsoft.Xna.Framework.Rectangle(1, 1, 12, 3));
				walls.Add(new Microsoft.Xna.Framework.Rectangle(15, 1, 13, 3));
				walls.Add(new Microsoft.Xna.Framework.Rectangle(13, 3, 2, 2));
				walls.Add(new Microsoft.Xna.Framework.Rectangle(1, 10, 10, 3));
				walls.Add(new Microsoft.Xna.Framework.Rectangle(13, 10, 8, 3));
				int bedroomWidthReduction = (owner.hasOrWillReceiveMail("renovation_corner_open") ? (-3) : 0);
				if (owner.hasOrWillReceiveMail("renovation_bedroom_open"))
				{
					walls.Add(new Microsoft.Xna.Framework.Rectangle(21, 15, 0, 2));
					walls.Add(new Microsoft.Xna.Framework.Rectangle(21, 10, 13 + bedroomWidthReduction, 3));
				}
				else
				{
					walls.Add(new Microsoft.Xna.Framework.Rectangle(21, 15, 2, 2));
					walls.Add(new Microsoft.Xna.Framework.Rectangle(23, 10, 11 + bedroomWidthReduction, 3));
				}
				if (owner.hasOrWillReceiveMail("renovation_southern_open"))
				{
					walls.Add(new Microsoft.Xna.Framework.Rectangle(23, 24, 3, 3));
					walls.Add(new Microsoft.Xna.Framework.Rectangle(31, 24, 3, 3));
				}
				else
				{
					walls.Add(new Microsoft.Xna.Framework.Rectangle(0, 0, 0, 0));
					walls.Add(new Microsoft.Xna.Framework.Rectangle(0, 0, 0, 0));
				}
				if (owner.hasOrWillReceiveMail("renovation_corner_open"))
				{
					walls.Add(new Microsoft.Xna.Framework.Rectangle(30, 1, 9, 3));
					walls.Add(new Microsoft.Xna.Framework.Rectangle(28, 3, 2, 2));
				}
				else
				{
					walls.Add(new Microsoft.Xna.Framework.Rectangle(0, 0, 0, 0));
					walls.Add(new Microsoft.Xna.Framework.Rectangle(0, 0, 0, 0));
				}
				break;
			}
			}
			return walls;
		}

		public override void TransferDataFromSavedLocation(GameLocation l)
		{
			if (l is FarmHouse)
			{
				FarmHouse farmhouse = l as FarmHouse;
				cribStyle.Value = farmhouse.cribStyle.Value;
			}
			base.TransferDataFromSavedLocation(l);
		}

		public override List<Microsoft.Xna.Framework.Rectangle> getFloors()
		{
			List<Microsoft.Xna.Framework.Rectangle> floors = new List<Microsoft.Xna.Framework.Rectangle>();
			switch (upgradeLevel)
			{
			case 0:
				floors.Add(new Microsoft.Xna.Framework.Rectangle(1, 3, 10, 9));
				break;
			case 1:
				floors.Add(new Microsoft.Xna.Framework.Rectangle(1, 3, 6, 9));
				floors.Add(new Microsoft.Xna.Framework.Rectangle(7, 3, 11, 9));
				floors.Add(new Microsoft.Xna.Framework.Rectangle(18, 8, 2, 2));
				floors.Add(new Microsoft.Xna.Framework.Rectangle(20, 3, 9, 8));
				break;
			case 2:
			case 3:
				floors.Add(new Microsoft.Xna.Framework.Rectangle(1, 3, 12, 6));
				floors.Add(new Microsoft.Xna.Framework.Rectangle(15, 3, 13, 6));
				floors.Add(new Microsoft.Xna.Framework.Rectangle(13, 5, 2, 2));
				floors.Add(new Microsoft.Xna.Framework.Rectangle(0, 12, 10, 11));
				floors.Add(new Microsoft.Xna.Framework.Rectangle(10, 12, 11, 9));
				if (owner.mailReceived.Contains("renovation_bedroom_open"))
				{
					floors.Add(new Microsoft.Xna.Framework.Rectangle(21, 17, 0, 2));
					floors.Add(new Microsoft.Xna.Framework.Rectangle(21, 12, 14, 11));
				}
				else
				{
					floors.Add(new Microsoft.Xna.Framework.Rectangle(21, 17, 2, 2));
					floors.Add(new Microsoft.Xna.Framework.Rectangle(23, 12, 12, 11));
				}
				if (owner.hasOrWillReceiveMail("renovation_southern_open"))
				{
					floors.Add(new Microsoft.Xna.Framework.Rectangle(23, 26, 11, 8));
				}
				else
				{
					floors.Add(new Microsoft.Xna.Framework.Rectangle(0, 0, 0, 0));
				}
				if (owner.hasOrWillReceiveMail("renovation_corner_open"))
				{
					floors.Add(new Microsoft.Xna.Framework.Rectangle(28, 5, 2, 3));
					floors.Add(new Microsoft.Xna.Framework.Rectangle(30, 3, 9, 6));
				}
				else
				{
					floors.Add(new Microsoft.Xna.Framework.Rectangle(0, 0, 0, 0));
					floors.Add(new Microsoft.Xna.Framework.Rectangle(0, 0, 0, 0));
				}
				break;
			}
			return floors;
		}

		public virtual bool CanModifyCrib()
		{
			if (owner == null)
			{
				return false;
			}
			if (owner.isMarried() && owner.GetSpouseFriendship().DaysUntilBirthing != -1)
			{
				return false;
			}
			foreach (Child child in owner.getChildren())
			{
				if (child.Age < 3)
				{
					return false;
				}
			}
			return true;
		}
	}
}
