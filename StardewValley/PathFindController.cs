using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.Xna.Framework;
using StardewValley.Locations;
using StardewValley.Network;
using StardewValley.TerrainFeatures;
using xTile.ObjectModel;
using xTile.Tiles;

namespace StardewValley
{
	/// This class finds a path from one point to another using the A* pathfinding algorithm. Then it will guide the given character along that path.
	/// Can only be used on maps where the tile width and height are each 127 or less. 
	[InstanceStatics]
	public class PathFindController
	{
		public delegate bool isAtEnd(PathNode currentNode, Point endPoint, GameLocation location, Character c);

		public delegate void endBehavior(Character c, GameLocation location);

		public const byte impassable = byte.MaxValue;

		public const int timeToWaitBeforeCancelling = 5000;

		private Character character;

		public GameLocation location;

		public Stack<Point> pathToEndPoint;

		public Point endPoint;

		public int finalFacingDirection;

		public int pausedTimer;

		public int limit;

		private isAtEnd endFunction;

		public endBehavior endBehaviorFunction;

		public bool nonDestructivePathing;

		public bool allowPlayerPathingInEvent;

		public bool NPCSchedule;

		private static readonly sbyte[,] Directions = new sbyte[4, 2]
		{
			{ -1, 0 },
			{ 1, 0 },
			{ 0, 1 },
			{ 0, -1 }
		};

		private static PriorityQueue _openList = new PriorityQueue();

		private static HashSet<int> _closedList = new HashSet<int>();

		private static int _counter = 0;

		public int timerSinceLastCheckPoint;

		public PathFindController(Character c, GameLocation location, Point endPoint, int finalFacingDirection)
			: this(c, location, isAtEndPoint, finalFacingDirection, eraseOldPathController: false, null, 10000, endPoint)
		{
		}

		public PathFindController(Character c, GameLocation location, Point endPoint, int finalFacingDirection, endBehavior endBehaviorFunction)
			: this(c, location, isAtEndPoint, finalFacingDirection, eraseOldPathController: false, null, 10000, endPoint)
		{
			this.endPoint = endPoint;
			this.endBehaviorFunction = endBehaviorFunction;
		}

		public PathFindController(Character c, GameLocation location, Point endPoint, int finalFacingDirection, endBehavior endBehaviorFunction, int limit)
			: this(c, location, isAtEndPoint, finalFacingDirection, eraseOldPathController: false, null, limit, endPoint)
		{
			this.endPoint = endPoint;
			this.endBehaviorFunction = endBehaviorFunction;
		}

		public PathFindController(Character c, GameLocation location, Point endPoint, int finalFacingDirection, bool eraseOldPathController, bool clearMarriageDialogues = true)
			: this(c, location, isAtEndPoint, finalFacingDirection, eraseOldPathController, null, 10000, endPoint, clearMarriageDialogues)
		{
		}

		public static bool isAtEndPoint(PathNode currentNode, Point endPoint, GameLocation location, Character c)
		{
			if (currentNode.x == endPoint.X)
			{
				return currentNode.y == endPoint.Y;
			}
			return false;
		}

		public PathFindController(Stack<Point> pathToEndPoint, GameLocation location, Character c, Point endPoint)
		{
			this.pathToEndPoint = pathToEndPoint;
			this.location = location;
			character = c;
			this.endPoint = endPoint;
		}

		public PathFindController(Stack<Point> pathToEndPoint, Character c, GameLocation l)
		{
			this.pathToEndPoint = pathToEndPoint;
			character = c;
			location = l;
			NPCSchedule = true;
		}

		public PathFindController(Character c, GameLocation location, isAtEnd endFunction, int finalFacingDirection, bool eraseOldPathController, endBehavior endBehaviorFunction, int limit, Point endPoint, bool clearMarriageDialogues = true)
		{
			this.limit = limit;
			character = c;
			NPC npc = c as NPC;
			if (npc != null && npc.CurrentDialogue.Count > 0 && npc.CurrentDialogue.Peek().removeOnNextMove)
			{
				npc.CurrentDialogue.Pop();
			}
			if (npc != null && clearMarriageDialogues)
			{
				if (npc.currentMarriageDialogue.Count > 0)
				{
					npc.currentMarriageDialogue.Clear();
				}
				npc.shouldSayMarriageDialogue.Value = false;
			}
			this.location = location;
			this.endFunction = ((endFunction == null) ? new isAtEnd(isAtEndPoint) : endFunction);
			this.endBehaviorFunction = endBehaviorFunction;
			if (endPoint == Point.Zero)
			{
				endPoint = new Point((int)c.getTileLocation().X, (int)c.getTileLocation().Y);
			}
			this.finalFacingDirection = finalFacingDirection;
			if (!(character is NPC) && !isPlayerPresent() && endFunction == new isAtEnd(isAtEndPoint) && endPoint.X > 0 && endPoint.Y > 0)
			{
				character.Position = new Vector2(endPoint.X * 64, endPoint.Y * 64 - 32);
				return;
			}
			pathToEndPoint = findPath(new Point((int)c.getTileLocation().X, (int)c.getTileLocation().Y), endPoint, endFunction, location, character, limit);
			if (pathToEndPoint == null)
			{
				_ = location is FarmHouse;
			}
		}

		public bool isPlayerPresent()
		{
			return location.farmers.Any();
		}

		public bool update(GameTime time)
		{
			if (pathToEndPoint == null || pathToEndPoint.Count == 0)
			{
				return true;
			}
			if (!NPCSchedule && !isPlayerPresent() && endPoint.X > 0 && endPoint.Y > 0)
			{
				character.Position = new Vector2(endPoint.X * 64, endPoint.Y * 64 - 32);
				return true;
			}
			if (Game1.activeClickableMenu == null || Game1.IsMultiplayer)
			{
				timerSinceLastCheckPoint += time.ElapsedGameTime.Milliseconds;
				Vector2 position = character.Position;
				moveCharacter(time);
				if (character.Position.Equals(position))
				{
					pausedTimer += time.ElapsedGameTime.Milliseconds;
				}
				else
				{
					pausedTimer = 0;
				}
				if (!NPCSchedule && pausedTimer > 5000)
				{
					return true;
				}
			}
			return false;
		}

		public static Stack<Point> findPath(Point startPoint, Point endPoint, isAtEnd endPointFunction, GameLocation location, Character character, int limit)
		{
			if (Interlocked.Increment(ref _counter) != 1)
			{
				throw new Exception();
			}
			try
			{
				bool ignore_obstructions = false;
				if (character is FarmAnimal && (character as FarmAnimal).type.Value == "Duck" && (character as FarmAnimal).isSwimming.Value)
				{
					ignore_obstructions = true;
				}
				_openList.Clear();
				_closedList.Clear();
				PriorityQueue openList = _openList;
				HashSet<int> closedList = _closedList;
				int iterations = 0;
				openList.Enqueue(new PathNode(startPoint.X, startPoint.Y, 0, null), Math.Abs(endPoint.X - startPoint.X) + Math.Abs(endPoint.Y - startPoint.Y));
				int layerWidth = location.map.Layers[0].LayerWidth;
				int layerHeight = location.map.Layers[0].LayerHeight;
				while (!openList.IsEmpty())
				{
					PathNode currentNode = openList.Dequeue();
					if (endPointFunction(currentNode, endPoint, location, character))
					{
						return reconstructPath(currentNode);
					}
					closedList.Add(currentNode.id);
					int ng = (byte)(currentNode.g + 1);
					for (int i = 0; i < 4; i++)
					{
						int nx = currentNode.x + Directions[i, 0];
						int ny = currentNode.y + Directions[i, 1];
						int nid = PathNode.ComputeHash(nx, ny);
						if (closedList.Contains(nid))
						{
							continue;
						}
						if ((nx != endPoint.X || ny != endPoint.Y) && (nx < 0 || ny < 0 || nx >= layerWidth || ny >= layerHeight))
						{
							closedList.Add(nid);
							continue;
						}
						PathNode neighbor = new PathNode(nx, ny, currentNode);
						neighbor.g = (byte)(currentNode.g + 1);
						if (!ignore_obstructions && location.isCollidingPosition(new Rectangle(neighbor.x * 64 + 1, neighbor.y * 64 + 1, 62, 62), Game1.viewport, character is Farmer, 0, glider: false, character, pathfinding: true))
						{
							closedList.Add(nid);
							continue;
						}
						int f = ng + (Math.Abs(endPoint.X - nx) + Math.Abs(endPoint.Y - ny));
						closedList.Add(nid);
						openList.Enqueue(neighbor, f);
					}
					iterations++;
					if (iterations >= limit)
					{
						return null;
					}
				}
				return null;
			}
			finally
			{
				if (Interlocked.Decrement(ref _counter) != 0)
				{
					throw new Exception();
				}
			}
		}

		public static Stack<Point> reconstructPath(PathNode finalNode)
		{
			Stack<Point> path = new Stack<Point>();
			path.Push(new Point(finalNode.x, finalNode.y));
			for (PathNode walk = finalNode.parent; walk != null; walk = walk.parent)
			{
				path.Push(new Point(walk.x, walk.y));
			}
			return path;
		}

		private byte[,] createMapGrid(GameLocation location, Point endPoint)
		{
			byte[,] mapGrid = new byte[location.map.Layers[0].LayerWidth, location.map.Layers[0].LayerHeight];
			for (int x = 0; x < location.map.Layers[0].LayerWidth; x++)
			{
				for (int y = 0; y < location.map.Layers[0].LayerHeight; y++)
				{
					if (!location.isCollidingPosition(new Rectangle(x * 64 + 1, y * 64 + 1, 62, 62), Game1.viewport, isFarmer: false, 0, glider: false, character, pathfinding: true))
					{
						mapGrid[x, y] = (byte)(Math.Abs(endPoint.X - x) + Math.Abs(endPoint.Y - y));
					}
					else
					{
						mapGrid[x, y] = byte.MaxValue;
					}
				}
			}
			return mapGrid;
		}

		private void moveCharacter(GameTime time)
		{
			Point peek = pathToEndPoint.Peek();
			Rectangle targetTile = new Rectangle(peek.X * 64, peek.Y * 64, 64, 64);
			targetTile.Inflate(-2, 0);
			Rectangle bbox = character.GetBoundingBox();
			if ((targetTile.Contains(bbox) || (bbox.Width > targetTile.Width && targetTile.Contains(bbox.Center))) && targetTile.Bottom - bbox.Bottom >= 2)
			{
				timerSinceLastCheckPoint = 0;
				pathToEndPoint.Pop();
				character.stopWithoutChangingFrame();
				if (pathToEndPoint.Count == 0)
				{
					character.Halt();
					if (finalFacingDirection != -1)
					{
						character.faceDirection(finalFacingDirection);
					}
					if (NPCSchedule)
					{
						NPC npc = character as NPC;
						npc.DirectionsToNewLocation = null;
						npc.endOfRouteMessage.Value = npc.nextEndOfRouteMessage;
					}
					if (endBehaviorFunction != null)
					{
						endBehaviorFunction(character, location);
					}
				}
				return;
			}
			if (character is Farmer farmer)
			{
				farmer.movementDirections.Clear();
			}
			else if (!(location is MovieTheater))
			{
				string name = character.Name;
				foreach (NPC c in location.characters)
				{
					if (!c.Equals(character) && c.GetBoundingBox().Intersects(bbox) && c.isMoving() && string.Compare(c.Name, name, StringComparison.Ordinal) < 0)
					{
						character.Halt();
						return;
					}
				}
			}
			if (bbox.Left < targetTile.Left && bbox.Right < targetTile.Right)
			{
				character.SetMovingRight(b: true);
			}
			else if (bbox.Right > targetTile.Right && bbox.Left > targetTile.Left)
			{
				character.SetMovingLeft(b: true);
			}
			else if (bbox.Top <= targetTile.Top)
			{
				character.SetMovingDown(b: true);
			}
			else if (bbox.Bottom >= targetTile.Bottom - 2)
			{
				character.SetMovingUp(b: true);
			}
			character.MovePosition(time, Game1.viewport, location);
			if (nonDestructivePathing)
			{
				if (targetTile.Intersects(character.nextPosition(character.facingDirection)))
				{
					Vector2 next_position = character.nextPositionVector2();
					Object next_tile_object = location.getObjectAt((int)next_position.X, (int)next_position.Y);
					if (next_tile_object != null)
					{
						if (next_tile_object is Fence fence && (bool)fence.isGate)
						{
							fence.toggleGate(location, open: true);
						}
						else if (!next_tile_object.isPassable())
						{
							character.Halt();
							character.controller = null;
							return;
						}
					}
				}
				handleWarps(character.nextPosition(character.getDirection()));
			}
			else if (NPCSchedule)
			{
				handleWarps(character.nextPosition(character.getDirection()));
			}
		}

		public void handleWarps(Rectangle position)
		{
			Warp w = location.isCollidingWithWarpOrDoor(position, character);
			if (w == null)
			{
				return;
			}
			if (w.TargetName == "Trailer" && Game1.MasterPlayer.mailReceived.Contains("pamHouseUpgrade"))
			{
				w = new Warp(w.X, w.Y, "Trailer_Big", 13, 24, flipFarmer: false);
			}
			if (character is NPC && (character as NPC).isMarried() && (character as NPC).followSchedule)
			{
				NPC i = character as NPC;
				if (location is FarmHouse)
				{
					w = new Warp(w.X, w.Y, "BusStop", 0, 23, flipFarmer: false);
				}
				if (location is BusStop && w.X <= 0)
				{
					w = new Warp(w.X, w.Y, i.getHome().name, (i.getHome() as FarmHouse).getEntryLocation().X, (i.getHome() as FarmHouse).getEntryLocation().Y, flipFarmer: false);
				}
				if (i.temporaryController != null && i.controller != null)
				{
					i.controller.location = Game1.getLocationFromName(w.TargetName);
				}
			}
			location = Game1.getLocationFromName(w.TargetName);
			if (character is NPC && (w.TargetName == "FarmHouse" || w.TargetName == "Cabin") && (character as NPC).isMarried() && (character as NPC).getSpouse() != null)
			{
				location = Utility.getHomeOfFarmer((character as NPC).getSpouse());
				w = new Warp(w.X, w.Y, location.name, (location as FarmHouse).getEntryLocation().X, (location as FarmHouse).getEntryLocation().Y, flipFarmer: false);
				if ((character as NPC).temporaryController != null && (character as NPC).controller != null)
				{
					(character as NPC).controller.location = location;
				}
				Game1.warpCharacter(character as NPC, location, new Vector2(w.TargetX, w.TargetY));
			}
			else
			{
				Game1.warpCharacter(character as NPC, w.TargetName, new Vector2(w.TargetX, w.TargetY));
			}
			if (isPlayerPresent() && location.doors.ContainsKey(new Point(w.X, w.Y)))
			{
				location.playSoundAt("doorClose", new Vector2(w.X, w.Y), NetAudio.SoundContext.NPC);
			}
			if (isPlayerPresent() && location.doors.ContainsKey(new Point(w.TargetX, w.TargetY - 1)))
			{
				location.playSoundAt("doorClose", new Vector2(w.TargetX, w.TargetY), NetAudio.SoundContext.NPC);
			}
			if (pathToEndPoint.Count > 0)
			{
				pathToEndPoint.Pop();
			}
			while (pathToEndPoint.Count > 0 && (Math.Abs(pathToEndPoint.Peek().X - character.getTileX()) > 1 || Math.Abs(pathToEndPoint.Peek().Y - character.getTileY()) > 1))
			{
				pathToEndPoint.Pop();
			}
		}

		public static bool IsPositionImpassableOnFarm(GameLocation loc, int x, int y)
		{
			if (loc is Farm farm)
			{
				NPC.isCheckingSpouseTileOccupancy = true;
				if (farm.isTileOccupied(new Vector2(x, y), "", ignoreAllCharacters: true))
				{
					NPC.isCheckingSpouseTileOccupancy = false;
					return true;
				}
				NPC.isCheckingSpouseTileOccupancy = false;
				if (farm.getBuildingAt(new Vector2(x, y)) != null)
				{
					return true;
				}
			}
			return isPositionImpassableForNPCSchedule(loc, x, y);
		}

		public static Stack<Point> FindPathOnFarm(Point startPoint, Point endPoint, GameLocation location, int limit)
		{
			Dictionary<Vector2, int> weight_map = new Dictionary<Vector2, int>();
			PriorityQueue openList = new PriorityQueue();
			HashSet<int> closedList = new HashSet<int>();
			int iterations = 0;
			openList.Enqueue(new PathNode(startPoint.X, startPoint.Y, 0, null), Math.Abs(endPoint.X - startPoint.X) + Math.Abs(endPoint.Y - startPoint.Y));
			PathNode previousNode = (PathNode)openList.Peek();
			int layerWidth = location.map.Layers[0].LayerWidth;
			int layerHeight = location.map.Layers[0].LayerHeight;
			while (!openList.IsEmpty())
			{
				PathNode currentNode = openList.Dequeue();
				if (currentNode.x == endPoint.X && currentNode.y == endPoint.Y)
				{
					return reconstructPath(currentNode);
				}
				closedList.Add(currentNode.id);
				for (int i = 0; i < 4; i++)
				{
					int neighbor_tile_x = currentNode.x + Directions[i, 0];
					int neighbor_tile_y = currentNode.y + Directions[i, 1];
					int nid = PathNode.ComputeHash(neighbor_tile_x, neighbor_tile_y);
					if (closedList.Contains(nid))
					{
						continue;
					}
					PathNode neighbor = new PathNode(neighbor_tile_x, neighbor_tile_y, currentNode);
					neighbor.g = (byte)(currentNode.g + 1);
					if ((neighbor.x == endPoint.X && neighbor.y == endPoint.Y) || (neighbor.x >= 0 && neighbor.y >= 0 && neighbor.x < layerWidth && neighbor.y < layerHeight && !IsPositionImpassableOnFarm(location, neighbor.x, neighbor.y)))
					{
						int f = neighbor.g + GetFarmTileWeight(location, neighbor.x, neighbor.y, weight_map) + (Math.Abs(endPoint.X - neighbor.x) + Math.Abs(endPoint.Y - neighbor.y));
						if (previousNode.x - currentNode.x == currentNode.x - neighbor_tile_x && previousNode.y - currentNode.y == currentNode.y - neighbor_tile_y)
						{
							f -= 25;
						}
						if (!openList.Contains(neighbor, f))
						{
							openList.Enqueue(neighbor, f);
						}
					}
				}
				previousNode = currentNode;
				iterations++;
				if (iterations >= limit)
				{
					return null;
				}
			}
			return null;
		}

		public static int GetFarmTileWeight(GameLocation location, int x, int y, Dictionary<Vector2, int> weight_map)
		{
			Vector2 tile_position = new Vector2(x, y);
			if (weight_map.ContainsKey(tile_position))
			{
				return weight_map[tile_position];
			}
			int weight = 0;
			Object tile_object = location.getObjectAtTile(x, y);
			if (tile_object != null && !tile_object.isPassable() && (!(tile_object is Fence fence) || !fence.isGate))
			{
				weight = 9999;
			}
			if (location.terrainFeatures.TryGetValue(tile_position, out var feature) && feature is Flooring)
			{
				weight -= 50;
			}
			weight_map[tile_position] = weight;
			return weight;
		}

		public static int CheckClearance(GameLocation location, Rectangle rect)
		{
			int i = 0;
			for (i = 0; i < 5; i++)
			{
				bool fail = false;
				for (int x = rect.Left; x < rect.Right; x++)
				{
					for (int y = rect.Top; y < rect.Bottom; y++)
					{
						Object tile_object = location.getObjectAtTile(x, y);
						if (tile_object != null && !tile_object.isPassable())
						{
							fail = true;
							break;
						}
					}
				}
				if (fail)
				{
					return i;
				}
			}
			return i;
		}

		public static Stack<Point> findPathForNPCSchedules(Point startPoint, Point endPoint, GameLocation location, int limit)
		{
			PriorityQueue openList = new PriorityQueue();
			HashSet<int> closedList = new HashSet<int>();
			int iterations = 0;
			openList.Enqueue(new PathNode(startPoint.X, startPoint.Y, 0, null), Math.Abs(endPoint.X - startPoint.X) + Math.Abs(endPoint.Y - startPoint.Y));
			PathNode previousNode = (PathNode)openList.Peek();
			int layerWidth = location.map.Layers[0].LayerWidth;
			int layerHeight = location.map.Layers[0].LayerHeight;
			while (!openList.IsEmpty())
			{
				PathNode currentNode = openList.Dequeue();
				if (currentNode.x == endPoint.X && currentNode.y == endPoint.Y)
				{
					return reconstructPath(currentNode);
				}
				closedList.Add(currentNode.id);
				for (int i = 0; i < 4; i++)
				{
					int neighbor_tile_x = currentNode.x + Directions[i, 0];
					int neighbor_tile_y = currentNode.y + Directions[i, 1];
					int nid = PathNode.ComputeHash(neighbor_tile_x, neighbor_tile_y);
					if (closedList.Contains(nid))
					{
						continue;
					}
					PathNode neighbor = new PathNode(neighbor_tile_x, neighbor_tile_y, currentNode);
					neighbor.g = (byte)(currentNode.g + 1);
					if ((neighbor.x == endPoint.X && neighbor.y == endPoint.Y) || (neighbor.x >= 0 && neighbor.y >= 0 && neighbor.x < layerWidth && neighbor.y < layerHeight && !isPositionImpassableForNPCSchedule(location, neighbor.x, neighbor.y)))
					{
						int f = neighbor.g + getPreferenceValueForTerrainType(location, neighbor.x, neighbor.y) + (Math.Abs(endPoint.X - neighbor.x) + Math.Abs(endPoint.Y - neighbor.y) + (((neighbor.x == currentNode.x && neighbor.x == previousNode.x) || (neighbor.y == currentNode.y && neighbor.y == previousNode.y)) ? (-2) : 0));
						if (!openList.Contains(neighbor, f))
						{
							openList.Enqueue(neighbor, f);
						}
					}
				}
				previousNode = currentNode;
				iterations++;
				if (iterations >= limit)
				{
					return null;
				}
			}
			return null;
		}

		private static bool isPositionImpassableForNPCSchedule(GameLocation loc, int x, int y)
		{
			Tile tile = loc.Map.GetLayer("Buildings").Tiles[x, y];
			if (tile != null && tile.TileIndex != -1)
			{
				PropertyValue property = null;
				string value = null;
				tile.TileIndexProperties.TryGetValue("Action", out property);
				if (property == null)
				{
					tile.Properties.TryGetValue("Action", out property);
				}
				if (property != null)
				{
					value = property.ToString();
					if (value.StartsWith("LockedDoorWarp"))
					{
						return true;
					}
					if (!value.Contains("Door") && !value.Contains("Passable"))
					{
						return true;
					}
				}
				else if (loc.doesTileHaveProperty(x, y, "Passable", "Buildings") == null && loc.doesTileHaveProperty(x, y, "NPCPassable", "Buildings") == null)
				{
					return true;
				}
			}
			if (loc.doesTileHaveProperty(x, y, "NoPath", "Back") != null)
			{
				return true;
			}
			foreach (Warp warp in loc.warps)
			{
				if (warp.X == x && warp.Y == y)
				{
					return true;
				}
			}
			if (loc.isTerrainFeatureAt(x, y))
			{
				return true;
			}
			return false;
		}

		private static int getPreferenceValueForTerrainType(GameLocation l, int x, int y)
		{
			string type = l.doesTileHaveProperty(x, y, "Type", "Back");
			if (type != null)
			{
				switch (type.ToLower())
				{
				case "stone":
					return -7;
				case "wood":
					return -4;
				case "dirt":
					return -2;
				case "grass":
					return -1;
				}
			}
			return 0;
		}
	}
}
