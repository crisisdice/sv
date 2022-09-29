public class WaterTiles
{
	public struct WaterTileData
	{
		public bool isWater;

		public bool isVisible;

		public WaterTileData(bool is_water, bool is_visible)
		{
			isWater = is_water;
			isVisible = is_visible;
		}
	}

	public WaterTileData[,] waterTiles;

	public bool this[int x, int y]
	{
		get
		{
			return waterTiles[x, y].isWater;
		}
		set
		{
			waterTiles[x, y] = new WaterTileData(value, is_visible: true);
		}
	}

	public static implicit operator WaterTiles(bool[,] source)
	{
		WaterTiles new_instance = new WaterTiles();
		new_instance.waterTiles = new WaterTileData[source.GetLength(0), source.GetLength(1)];
		for (int x = 0; x < source.GetLength(0); x++)
		{
			for (int y = 0; y < source.GetLength(1); y++)
			{
				new_instance.waterTiles[x, y] = new WaterTileData(source[x, y], is_visible: true);
			}
		}
		return new_instance;
	}
}
