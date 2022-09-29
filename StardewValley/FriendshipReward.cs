using System.Collections.Generic;
using System.Xml.Serialization;
using Netcode;

namespace StardewValley
{
	public class FriendshipReward : OrderReward
	{
		[XmlElement("targetName")]
		public NetString targetName = new NetString();

		[XmlElement("amount")]
		public NetInt amount = new NetInt();

		public override void InitializeNetFields()
		{
			base.InitializeNetFields();
			base.NetFields.AddFields(targetName, amount);
		}

		public override void Load(SpecialOrder order, Dictionary<string, string> data)
		{
			string target_name = order.requester;
			if (data.ContainsKey("TargetName"))
			{
				target_name = data["TargetName"];
			}
			target_name = order.Parse(target_name);
			targetName.Value = target_name;
			string amount_string = "250";
			if (data.ContainsKey("Amount"))
			{
				amount_string = data["Amount"];
			}
			amount_string = order.Parse(amount_string);
			amount.Value = int.Parse(amount_string);
		}

		public override void Grant()
		{
			NPC i = Game1.getCharacterFromName(targetName.Value);
			if (i != null)
			{
				Game1.player.changeFriendship(amount.Value, i);
			}
		}
	}
}
