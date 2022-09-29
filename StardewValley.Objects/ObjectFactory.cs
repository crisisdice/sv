using System;
using Microsoft.Xna.Framework;
using StardewValley.Tools;

namespace StardewValley.Objects
{
	public class ObjectFactory
	{
		public const byte regularObject = 0;

		public const byte bigCraftable = 1;

		public const byte weapon = 2;

		public const byte specialItem = 3;

		public const byte regularObjectRecipe = 4;

		public const byte bigCraftableRecipe = 5;

		public static ItemDescription getDescriptionFromItem(Item i)
		{
			if (i is Object && (bool)(i as Object).bigCraftable)
			{
				return new ItemDescription(1, (i as Object).ParentSheetIndex, i.Stack);
			}
			if (i is Object)
			{
				return new ItemDescription(0, (i as Object).ParentSheetIndex, i.Stack);
			}
			if (i is MeleeWeapon)
			{
				return new ItemDescription(2, (i as MeleeWeapon).CurrentParentTileIndex, i.Stack);
			}
			throw new Exception("ItemFactory trying to create item description from unknown item");
		}

		public static Item getItemFromDescription(byte type, int index, int stack)
		{
			return type switch
			{
				0 => new Object(Vector2.Zero, index, stack), 
				4 => new Object(index, stack, isRecipe: true), 
				1 => new Object(Vector2.Zero, index), 
				5 => new Object(Vector2.Zero, index, isRecipe: true), 
				2 => new MeleeWeapon(index), 
				_ => throw new Exception("ItemFactory trying to create item from unknown description"), 
			};
		}
	}
}
