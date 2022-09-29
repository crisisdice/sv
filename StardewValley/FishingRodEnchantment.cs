using System.Xml.Serialization;
using StardewValley.Tools;

namespace StardewValley
{
	[XmlInclude(typeof(FishingRodEnchantment))]
	public class FishingRodEnchantment : BaseEnchantment
	{
		public override bool CanApplyTo(Item item)
		{
			if (item is FishingRod)
			{
				return true;
			}
			return false;
		}
	}
}
