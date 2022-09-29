using System;
using System.Collections.ObjectModel;
using System.IO;
using Microsoft.Xna.Framework;
using Netcode;

namespace StardewValley.Network
{
	public struct OutgoingMessage
	{
		private byte messageType;

		private long farmerID;

		private object[] data;

		public byte MessageType => messageType;

		public long FarmerID => farmerID;

		public Farmer SourceFarmer => Game1.getFarmer(farmerID);

		public ReadOnlyCollection<object> Data => Array.AsReadOnly(data);

		public OutgoingMessage(byte messageType, long farmerID, params object[] data)
		{
			this.messageType = messageType;
			this.farmerID = farmerID;
			this.data = data;
		}

		public OutgoingMessage(byte messageType, Farmer sourceFarmer, params object[] data)
			: this(messageType, sourceFarmer.UniqueMultiplayerID, data)
		{
		}

		public OutgoingMessage(IncomingMessage message)
			: this(message.MessageType, message.FarmerID, message.Data)
		{
		}

		public void Write(BinaryWriter writer)
		{
			writer.Write(messageType);
			writer.Write(farmerID);
			object[] data = this.data;
			writer.WriteSkippable(delegate
			{
				object[] array = data;
				foreach (object obj in array)
				{
					if (obj is Vector2)
					{
						writer.Write(((Vector2)obj).X);
						writer.Write(((Vector2)obj).Y);
					}
					else if (obj is Guid)
					{
						writer.Write(((Guid)obj).ToByteArray());
					}
					else if (obj is byte[])
					{
						writer.Write((byte[])obj);
					}
					else if (obj is bool)
					{
						writer.Write((byte)(((bool)obj) ? 1u : 0u));
					}
					else if (obj is byte)
					{
						writer.Write((byte)obj);
					}
					else if (obj is int)
					{
						writer.Write((int)obj);
					}
					else if (obj is short)
					{
						writer.Write((short)obj);
					}
					else if (obj is float)
					{
						writer.Write((float)obj);
					}
					else if (obj is long)
					{
						writer.Write((long)obj);
					}
					else if (obj is string)
					{
						writer.Write((string)obj);
					}
					else if (obj is string[])
					{
						string[] array2 = (string[])obj;
						writer.Write((byte)array2.Length);
						for (int j = 0; j < array2.Length; j++)
						{
							writer.Write(array2[j]);
						}
					}
					else
					{
						if (!(obj is IConvertible) || !obj.GetType().IsValueType)
						{
							throw new InvalidDataException();
						}
						writer.WriteEnum(obj);
					}
				}
			});
		}
	}
}
