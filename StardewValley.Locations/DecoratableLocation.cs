using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Netcode;
using StardewValley.GameData;
using StardewValley.Menus;
using StardewValley.Network;
using StardewValley.Objects;
using StardewValley.TerrainFeatures;
using xTile;
using xTile.Dimensions;
using xTile.Layers;
using xTile.ObjectModel;
using xTile.Tiles;

namespace StardewValley.Locations
{
	public class DecoratableLocation : GameLocation
	{
		public readonly DecorationFacade wallPaper = new DecorationFacade();

		[XmlIgnore]
		public readonly NetStringList wallpaperIDs = new NetStringList();

		public readonly NetStringDictionary<string, NetString> appliedWallpaper = new NetStringDictionary<string, NetString>();

		[XmlIgnore]
		public readonly Dictionary<string, List<Vector3>> wallpaperTiles = new Dictionary<string, List<Vector3>>();

		public readonly DecorationFacade floor = new DecorationFacade();

		[XmlIgnore]
		public readonly NetStringList floorIDs = new NetStringList();

		public readonly NetStringDictionary<string, NetString> appliedFloor = new NetStringDictionary<string, NetString>();

		[XmlIgnore]
		public readonly Dictionary<string, List<Vector3>> floorTiles = new Dictionary<string, List<Vector3>>();

		protected Dictionary<string, TileSheet> _wallAndFloorTileSheets = new Dictionary<string, TileSheet>();

		protected Map _wallAndFloorTileSheetMap;

		protected override void initNetFields()
		{
			base.initNetFields();
			appliedWallpaper.InterpolationWait = false;
			appliedFloor.InterpolationWait = false;
			base.NetFields.AddFields(appliedWallpaper, appliedFloor, floorIDs, wallpaperIDs);
			appliedWallpaper.OnValueAdded += delegate(string key, string value)
			{
				UpdateWallpaper(key);
			};
			appliedWallpaper.OnConflictResolve += delegate(string key, NetString rejected, NetString accepted)
			{
				UpdateWallpaper(key);
			};
			appliedWallpaper.OnValueTargetUpdated += delegate(string key, string old_value, string new_value)
			{
				if (appliedWallpaper.FieldDict.ContainsKey(key))
				{
					appliedWallpaper.FieldDict[key].CancelInterpolation();
				}
				UpdateWallpaper(key);
			};
			appliedFloor.OnValueAdded += delegate(string key, string value)
			{
				UpdateFloor(key);
			};
			appliedFloor.OnConflictResolve += delegate(string key, NetString rejected, NetString accepted)
			{
				UpdateFloor(key);
			};
			appliedFloor.OnValueTargetUpdated += delegate(string key, string old_value, string new_value)
			{
				if (appliedFloor.FieldDict.ContainsKey(key))
				{
					appliedFloor.FieldDict[key].CancelInterpolation();
				}
				UpdateFloor(key);
			};
		}

		public DecoratableLocation()
		{
		}

		public DecoratableLocation(string mapPath, string name)
			: base(mapPath, name)
		{
		}

		public virtual void ReadWallpaperAndFloorTileData()
		{
			updateMap();
			wallpaperTiles.Clear();
			floorTiles.Clear();
			wallpaperIDs.Clear();
			floorIDs.Clear();
			Dictionary<string, string> initial_values = new Dictionary<string, string>();
			if (map.Properties.ContainsKey("WallIDs"))
			{
				string[] array = map.Properties["WallIDs"].ToString().Split(',');
				for (int k = 0; k < array.Length; k++)
				{
					string[] data_split2 = array[k].Trim().Split(' ');
					if (data_split2.Length >= 1)
					{
						wallpaperIDs.Add(data_split2[0]);
					}
					if (data_split2.Length >= 2)
					{
						initial_values[data_split2[0]] = data_split2[1];
					}
				}
			}
			if (wallpaperIDs.Count == 0)
			{
				List<Microsoft.Xna.Framework.Rectangle> walls = getWalls();
				for (int j = 0; j < walls.Count; j++)
				{
					string id2 = "Wall_" + j;
					wallpaperIDs.Add(id2);
					Microsoft.Xna.Framework.Rectangle rect2 = walls[j];
					if (!wallpaperTiles.ContainsKey(j.ToString()))
					{
						wallpaperTiles[id2] = new List<Vector3>();
					}
					for (int x4 = rect2.Left; x4 < rect2.Right; x4++)
					{
						for (int y4 = rect2.Top; y4 < rect2.Bottom; y4++)
						{
							wallpaperTiles[id2].Add(new Vector3(x4, y4, y4 - rect2.Top));
						}
					}
				}
			}
			else
			{
				for (int x3 = 0; x3 < map.Layers[0].LayerWidth; x3++)
				{
					for (int y = 0; y < map.Layers[0].LayerHeight; y++)
					{
						string tile_property = doesTileHaveProperty(x3, y, "WallID", "Back");
						getTileIndexAt(new Point(x3, y), "Back");
						if (tile_property == null)
						{
							continue;
						}
						if (!wallpaperIDs.Contains(tile_property))
						{
							wallpaperIDs.Add(tile_property);
						}
						if (!appliedWallpaper.ContainsKey(tile_property))
						{
							appliedWallpaper[tile_property] = "0";
							if (initial_values.ContainsKey(tile_property))
							{
								string initial_value = initial_values[tile_property];
								if (appliedWallpaper.ContainsKey(initial_value))
								{
									appliedWallpaper[tile_property] = appliedWallpaper[initial_value];
								}
								else if (GetWallpaperSource(initial_value).Value >= 0)
								{
									appliedWallpaper[tile_property] = initial_value;
								}
							}
						}
						if (!wallpaperTiles.ContainsKey(tile_property))
						{
							wallpaperTiles[tile_property] = new List<Vector3>();
						}
						wallpaperTiles[tile_property].Add(new Vector3(x3, y, 0f));
						string tilesheet_id = getTileSheetIDAt(x3, y, "Back");
						_ = map.GetTileSheet(tilesheet_id).SheetWidth;
						if (IsFloorableOrWallpaperableTile(x3, y + 1, "Back"))
						{
							wallpaperTiles[tile_property].Add(new Vector3(x3, y + 1, 1f));
						}
						if (IsFloorableOrWallpaperableTile(x3, y + 2, "Buildings"))
						{
							wallpaperTiles[tile_property].Add(new Vector3(x3, y + 2, 2f));
						}
						else if (IsFloorableOrWallpaperableTile(x3, y + 2, "Back") && !IsFloorableTile(x3, y + 2, "Back"))
						{
							wallpaperTiles[tile_property].Add(new Vector3(x3, y + 2, 2f));
						}
					}
				}
			}
			initial_values.Clear();
			if (map.Properties.ContainsKey("FloorIDs"))
			{
				string[] array = map.Properties["FloorIDs"].ToString().Split(',');
				for (int k = 0; k < array.Length; k++)
				{
					string[] data_split = array[k].Trim().Split(' ');
					if (data_split.Length >= 1)
					{
						floorIDs.Add(data_split[0]);
					}
					if (data_split.Length >= 2)
					{
						initial_values[data_split[0]] = data_split[1];
					}
				}
			}
			if (floorIDs.Count == 0)
			{
				List<Microsoft.Xna.Framework.Rectangle> floors = getFloors();
				for (int i = 0; i < floors.Count; i++)
				{
					string id = "Floor_" + i;
					floorIDs.Add(id);
					Microsoft.Xna.Framework.Rectangle rect = floors[i];
					if (!floorTiles.ContainsKey(i.ToString()))
					{
						floorTiles[id] = new List<Vector3>();
					}
					for (int x2 = rect.Left; x2 < rect.Right; x2++)
					{
						for (int y3 = rect.Top; y3 < rect.Bottom; y3++)
						{
							floorTiles[id].Add(new Vector3(x2, y3, 0f));
						}
					}
				}
			}
			else
			{
				for (int x = 0; x < map.Layers[0].LayerWidth; x++)
				{
					for (int y2 = 0; y2 < map.Layers[0].LayerHeight; y2++)
					{
						string tile_property2 = doesTileHaveProperty(x, y2, "FloorID", "Back");
						getTileIndexAt(new Point(x, y2), "Back");
						if (tile_property2 == null)
						{
							continue;
						}
						if (!floorIDs.Contains(tile_property2))
						{
							floorIDs.Add(tile_property2);
						}
						if (!appliedFloor.ContainsKey(tile_property2))
						{
							appliedFloor[tile_property2] = "0";
							if (initial_values.ContainsKey(tile_property2))
							{
								string initial_value2 = initial_values[tile_property2];
								if (appliedFloor.ContainsKey(initial_value2))
								{
									appliedFloor[tile_property2] = appliedFloor[initial_value2];
								}
								else if (GetFloorSource(initial_value2).Value >= 0)
								{
									appliedFloor[tile_property2] = initial_value2;
								}
							}
						}
						if (!floorTiles.ContainsKey(tile_property2))
						{
							floorTiles[tile_property2] = new List<Vector3>();
						}
						floorTiles[tile_property2].Add(new Vector3(x, y2, 0f));
					}
				}
			}
			setFloors();
			setWallpapers();
		}

		public virtual TileSheet GetWallAndFloorTilesheet(string id)
		{
			if (map != _wallAndFloorTileSheetMap)
			{
				_wallAndFloorTileSheets.Clear();
				_wallAndFloorTileSheetMap = map;
			}
			if (_wallAndFloorTileSheets.ContainsKey(id))
			{
				return _wallAndFloorTileSheets[id];
			}
			try
			{
				List<ModWallpaperOrFlooring> list = Game1.content.Load<List<ModWallpaperOrFlooring>>("Data\\AdditionalWallpaperFlooring");
				ModWallpaperOrFlooring found_mod_data = null;
				foreach (ModWallpaperOrFlooring mod_data_entry in list)
				{
					if (mod_data_entry.ID == id)
					{
						found_mod_data = mod_data_entry;
						break;
					}
				}
				if (found_mod_data != null)
				{
					Texture2D texture = Game1.content.Load<Texture2D>(found_mod_data.Texture);
					if (texture.Width / 16 != 16)
					{
						Console.WriteLine("WARNING: Wallpaper/floor tilesheets must be 16 tiles wide.");
					}
					TileSheet tilesheet = new TileSheet("x_WallsAndFloors_" + id, map, found_mod_data.Texture, new Size(texture.Width / 16, texture.Height / 16), new Size(16, 16));
					map.AddTileSheet(tilesheet);
					map.LoadTileSheets(Game1.mapDisplayDevice);
					_wallAndFloorTileSheets[id] = tilesheet;
					return tilesheet;
				}
				Console.WriteLine("Error trying to load wallpaper/floor tilesheet: " + id);
				_wallAndFloorTileSheets[id] = null;
				return null;
			}
			catch (Exception)
			{
				Console.WriteLine("Error trying to load wallpaper/floor tilesheet: " + id);
				_wallAndFloorTileSheets[id] = null;
				return null;
			}
		}

		public virtual KeyValuePair<int, int> GetFloorSource(string pattern_id)
		{
			int pattern_index = -1;
			if (pattern_id.Contains(":"))
			{
				string[] pattern_split = pattern_id.Split(':');
				TileSheet tilesheet2 = GetWallAndFloorTilesheet(pattern_split[0]);
				if (int.TryParse(pattern_split[1], out pattern_index) && tilesheet2 != null)
				{
					return new KeyValuePair<int, int>(map.TileSheets.IndexOf(tilesheet2), pattern_index);
				}
			}
			if (int.TryParse(pattern_id, out pattern_index))
			{
				TileSheet tilesheet = map.GetTileSheet("walls_and_floors");
				return new KeyValuePair<int, int>(map.TileSheets.IndexOf(tilesheet), pattern_index);
			}
			return new KeyValuePair<int, int>(-1, -1);
		}

		public virtual KeyValuePair<int, int> GetWallpaperSource(string pattern_id)
		{
			int pattern_index = -1;
			if (pattern_id.Contains(":"))
			{
				string[] pattern_split = pattern_id.Split(':');
				TileSheet tilesheet2 = GetWallAndFloorTilesheet(pattern_split[0]);
				if (int.TryParse(pattern_split[1], out pattern_index) && tilesheet2 != null)
				{
					return new KeyValuePair<int, int>(map.TileSheets.IndexOf(tilesheet2), pattern_index);
				}
			}
			if (int.TryParse(pattern_id, out pattern_index))
			{
				TileSheet tilesheet = map.GetTileSheet("walls_and_floors");
				return new KeyValuePair<int, int>(map.TileSheets.IndexOf(tilesheet), pattern_index);
			}
			return new KeyValuePair<int, int>(-1, -1);
		}

		public virtual void UpdateFloor(string floor_id)
		{
			updateMap();
			if (!appliedFloor.ContainsKey(floor_id) || !floorTiles.ContainsKey(floor_id))
			{
				return;
			}
			string pattern_id = appliedFloor[floor_id];
			foreach (Vector3 item in floorTiles[floor_id])
			{
				int x = (int)item.X;
				int y = (int)item.Y;
				KeyValuePair<int, int> source = GetFloorSource(pattern_id);
				if (source.Value < 0)
				{
					continue;
				}
				int tilesheet_index = source.Key;
				int floor_pattern_id = source.Value;
				int tiles_wide = map.TileSheets[tilesheet_index].SheetWidth;
				string id = map.TileSheets[tilesheet_index].Id;
				string layer = "Back";
				floor_pattern_id = floor_pattern_id * 2 + floor_pattern_id / (tiles_wide / 2) * tiles_wide;
				if (id == "walls_and_floors")
				{
					floor_pattern_id += GetFirstFlooringTile();
				}
				if (!IsFloorableOrWallpaperableTile(x, y, layer))
				{
					continue;
				}
				Tile old_tile = map.GetLayer(layer).Tiles[x, y];
				setMapTile(x, y, GetFlooringIndex(floor_pattern_id, x, y), layer, null, tilesheet_index);
				Tile new_tile = map.GetLayer(layer).Tiles[x, y];
				if (old_tile == null)
				{
					continue;
				}
				foreach (KeyValuePair<string, PropertyValue> property in old_tile.Properties)
				{
					new_tile.Properties[property.Key] = property.Value;
				}
			}
		}

		public virtual void UpdateWallpaper(string wallpaper_id)
		{
			updateMap();
			if (!appliedWallpaper.ContainsKey(wallpaper_id) || !wallpaperTiles.ContainsKey(wallpaper_id))
			{
				return;
			}
			string pattern_id = appliedWallpaper[wallpaper_id];
			foreach (Vector3 item in wallpaperTiles[wallpaper_id])
			{
				int x = (int)item.X;
				int y = (int)item.Y;
				int type = (int)item.Z;
				KeyValuePair<int, int> source = GetWallpaperSource(pattern_id);
				if (source.Value < 0)
				{
					continue;
				}
				int tile_sheet_index = source.Key;
				int tile_id = source.Value;
				int tiles_wide = map.TileSheets[tile_sheet_index].SheetWidth;
				string layer = "Back";
				if (type == 2)
				{
					layer = "Buildings";
					if (!IsFloorableOrWallpaperableTile(x, y, "Buildings"))
					{
						layer = "Back";
					}
				}
				if (!IsFloorableOrWallpaperableTile(x, y, layer))
				{
					continue;
				}
				Tile old_tile = map.GetLayer(layer).Tiles[x, y];
				setMapTile(x, y, tile_id / tiles_wide * tiles_wide * 3 + tile_id % tiles_wide + type * tiles_wide, layer, null, tile_sheet_index);
				Tile new_tile = map.GetLayer(layer).Tiles[x, y];
				if (old_tile == null)
				{
					continue;
				}
				foreach (KeyValuePair<string, PropertyValue> property in old_tile.Properties)
				{
					new_tile.Properties[property.Key] = property.Value;
				}
			}
		}

		public override void UpdateWhenCurrentLocation(GameTime time)
		{
			if (!wasUpdated)
			{
				base.UpdateWhenCurrentLocation(time);
			}
		}

		public override void MakeMapModifications(bool force = false)
		{
			base.MakeMapModifications(force);
			if (!(this is FarmHouse))
			{
				ReadWallpaperAndFloorTileData();
				setWallpapers();
				setFloors();
			}
			if (getTileIndexAt(Game1.player.getTileX(), Game1.player.getTileY(), "Buildings") != -1)
			{
				Game1.player.position.Y += 64f;
			}
		}

		protected override void resetLocalState()
		{
			base.resetLocalState();
			if (!Game1.player.mailReceived.Contains("button_tut_1"))
			{
				Game1.player.mailReceived.Add("button_tut_1");
				Game1.onScreenMenus.Add(new ButtonTutorialMenu(0));
			}
		}

		public override void shiftObjects(int dx, int dy)
		{
			base.shiftObjects(dx, dy);
			foreach (Furniture v2 in furniture)
			{
				v2.removeLights(this);
				v2.tileLocation.X += dx;
				v2.tileLocation.Y += dy;
				v2.boundingBox.X += dx * 64;
				v2.boundingBox.Y += dy * 64;
				v2.updateDrawPosition();
				if (Game1.isDarkOut())
				{
					v2.addLights(this);
				}
			}
			List<KeyValuePair<Vector2, TerrainFeature>> list = new List<KeyValuePair<Vector2, TerrainFeature>>(terrainFeatures.Pairs);
			terrainFeatures.Clear();
			foreach (KeyValuePair<Vector2, TerrainFeature> v in list)
			{
				terrainFeatures.Add(new Vector2(v.Key.X + (float)dx, v.Key.Y + (float)dy), v.Value);
			}
		}

		public void moveFurniture(int oldX, int oldY, int newX, int newY)
		{
			Vector2 oldSpot = new Vector2(oldX, oldY);
			foreach (Furniture f in furniture)
			{
				if (f.tileLocation.Equals(oldSpot))
				{
					f.removeLights(this);
					f.tileLocation.Value = new Vector2(newX, newY);
					f.boundingBox.X = newX * 64;
					f.boundingBox.Y = newY * 64;
					f.updateDrawPosition();
					if (Game1.isDarkOut())
					{
						f.addLights(this);
					}
					return;
				}
			}
			if (objects.ContainsKey(oldSpot))
			{
				Object o = objects[oldSpot];
				objects.Remove(oldSpot);
				o.tileLocation.Value = new Vector2(newX, newY);
				objects.Add(new Vector2(newX, newY), o);
			}
		}

		public override bool CanFreePlaceFurniture()
		{
			return true;
		}

		public virtual bool isTileOnWall(int x, int y)
		{
			foreach (string id in wallpaperTiles.Keys)
			{
				foreach (Vector3 tile_data in wallpaperTiles[id])
				{
					if ((int)tile_data.X == x && (int)tile_data.Y == y)
					{
						return true;
					}
				}
			}
			return false;
		}

		public int GetWallTopY(int x, int y)
		{
			foreach (string id in wallpaperTiles.Keys)
			{
				foreach (Vector3 tile_data in wallpaperTiles[id])
				{
					if ((int)tile_data.X == x && (int)tile_data.Y == y)
					{
						return y - (int)tile_data.Z;
					}
				}
			}
			return -1;
		}

		public virtual void setFloors()
		{
			foreach (KeyValuePair<string, string> pair in appliedFloor.Pairs)
			{
				UpdateFloor(pair.Key);
			}
		}

		public virtual void setWallpapers()
		{
			foreach (KeyValuePair<string, string> pair in appliedWallpaper.Pairs)
			{
				UpdateWallpaper(pair.Key);
			}
		}

		public void SetFloor(string which, string which_room)
		{
			if (which_room == null)
			{
				foreach (string key in floorIDs)
				{
					appliedFloor[key] = which;
				}
				return;
			}
			appliedFloor[which_room] = which;
		}

		public void SetWallpaper(string which, string which_room)
		{
			if (which_room == null)
			{
				foreach (string key in wallpaperIDs)
				{
					appliedWallpaper[key] = which;
				}
				return;
			}
			appliedWallpaper[which_room] = which;
		}

		public string GetFloorID(int x, int y)
		{
			foreach (string id in floorTiles.Keys)
			{
				foreach (Vector3 tile_data in floorTiles[id])
				{
					if ((int)tile_data.X == x && (int)tile_data.Y == y)
					{
						return id;
					}
				}
			}
			return null;
		}

		public string GetWallpaperID(int x, int y)
		{
			foreach (string id in wallpaperTiles.Keys)
			{
				foreach (Vector3 tile_data in wallpaperTiles[id])
				{
					if ((int)tile_data.X == x && (int)tile_data.Y == y)
					{
						return id;
					}
				}
			}
			return null;
		}

		[Obsolete("Use string based SetFloor.")]
		public virtual void setFloor(int which, int whichRoom = -1, bool persist = false)
		{
			string room_name = null;
			if (whichRoom >= 0 && whichRoom < floorIDs.Count)
			{
				room_name = floorIDs[whichRoom];
			}
			SetFloor(which.ToString(), room_name);
		}

		[Obsolete("Use string based SetWallpaper.")]
		public void setWallpaper(int which, int whichRoom = -1, bool persist = false)
		{
			string room_name = null;
			if (whichRoom >= 0 && whichRoom < wallpaperIDs.Count)
			{
				room_name = wallpaperIDs[whichRoom];
			}
			SetWallpaper(which.ToString(), room_name);
		}

		protected bool IsFloorableTile(int x, int y, string layer_name)
		{
			int tile_index = getTileIndexAt(x, y, "Buildings");
			if (tile_index >= 197 && tile_index <= 199 && getTileSheetIDAt(x, y, "Buildings") == "untitled tile sheet")
			{
				return false;
			}
			return IsFloorableOrWallpaperableTile(x, y, layer_name);
		}

		public bool IsWallAndFloorTilesheet(string tilesheet_id)
		{
			if (tilesheet_id.StartsWith("x_WallsAndFloors_"))
			{
				return true;
			}
			return tilesheet_id == "walls_and_floors";
		}

		protected bool IsFloorableOrWallpaperableTile(int x, int y, string layer_name)
		{
			Layer layer = map.GetLayer(layer_name);
			if (layer != null && x < layer.LayerWidth && y < layer.LayerHeight && layer.Tiles[x, y] != null && layer.Tiles[x, y].TileSheet != null && IsWallAndFloorTilesheet(layer.Tiles[x, y].TileSheet.Id))
			{
				return true;
			}
			return false;
		}

		public override void drawFloorDecorations(SpriteBatch b)
		{
			base.drawFloorDecorations(b);
		}

		public override void TransferDataFromSavedLocation(GameLocation l)
		{
			if (l is DecoratableLocation decoratable_location)
			{
				if (!decoratable_location.appliedWallpaper.Keys.Any())
				{
					ReadWallpaperAndFloorTileData();
					for (int i = 0; i < decoratable_location.wallPaper.Count; i++)
					{
						try
						{
							string key3 = wallpaperIDs[i];
							string value = decoratable_location.wallPaper[i].ToString();
							appliedWallpaper[key3] = value;
						}
						catch (Exception)
						{
						}
					}
					for (int j = 0; j < decoratable_location.floor.Count; j++)
					{
						try
						{
							string key4 = floorIDs[j];
							string value2 = decoratable_location.floor[j].ToString();
							appliedFloor[key4] = value2;
						}
						catch (Exception)
						{
						}
					}
				}
				else
				{
					foreach (string key2 in decoratable_location.appliedWallpaper.Keys)
					{
						appliedWallpaper[key2] = decoratable_location.appliedWallpaper[key2];
					}
					foreach (string key in decoratable_location.appliedFloor.Keys)
					{
						appliedFloor[key] = decoratable_location.appliedFloor[key];
					}
				}
			}
			setWallpapers();
			setFloors();
			base.TransferDataFromSavedLocation(l);
		}

		public Furniture getRandomFurniture(Random r)
		{
			if (furniture.Count > 0)
			{
				return furniture.ElementAt(r.Next(furniture.Count));
			}
			return null;
		}

		public virtual int getFloorAt(Point p)
		{
			foreach (string key in floorTiles.Keys)
			{
				foreach (Vector3 tile_data in floorTiles[key])
				{
					if ((int)tile_data.X == p.X && (int)tile_data.Y == p.Y)
					{
						return floorIDs.IndexOf(key);
					}
				}
			}
			return -1;
		}

		public virtual int getWallForRoomAt(Point p)
		{
			foreach (string key in wallpaperTiles.Keys)
			{
				foreach (Vector3 tile_data in wallpaperTiles[key])
				{
					if ((int)tile_data.X == p.X && (int)tile_data.Y == p.Y)
					{
						return wallpaperIDs.IndexOf(key);
					}
				}
			}
			return -1;
		}

		public virtual int GetFirstFlooringTile()
		{
			return 336;
		}

		public virtual int GetFlooringIndex(int base_tile_sheet, int tile_x, int tile_y)
		{
			int replaced_tile_index = getTileIndexAt(tile_x, tile_y, "Back");
			if (replaced_tile_index < 0)
			{
				return 0;
			}
			string tilesheet_name = getTileSheetIDAt(tile_x, tile_y, "Back");
			TileSheet tilesheet = map.GetTileSheet(tilesheet_name);
			int tiles_wide = 16;
			if (tilesheet != null)
			{
				tiles_wide = tilesheet.SheetWidth;
			}
			if (tilesheet_name == "walls_and_floors")
			{
				replaced_tile_index -= GetFirstFlooringTile();
			}
			int x_offset = replaced_tile_index % 2;
			int y_offset = replaced_tile_index % (tiles_wide * 2) / tiles_wide;
			return base_tile_sheet + x_offset + tiles_wide * y_offset;
		}

		[Obsolete("Replaced by SetFloor.")]
		protected virtual void doSetVisibleFloor(int whichRoom, int which)
		{
			SetFloor(which.ToString(), whichRoom.ToString());
		}

		[Obsolete("Replaced by SetWallpaper.")]
		protected virtual void doSetVisibleWallpaper(int whichRoom, int which)
		{
			SetWallpaper(which.ToString(), whichRoom.ToString());
		}

		public virtual List<Microsoft.Xna.Framework.Rectangle> getFloors()
		{
			return new List<Microsoft.Xna.Framework.Rectangle>();
		}
	}
}
