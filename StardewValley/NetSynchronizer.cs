using System;
using System.Collections.Generic;
using System.IO;
using Netcode;
using StardewValley.Network;

namespace StardewValley
{
	public abstract class NetSynchronizer
	{
		private const byte MessageTypeVar = 0;

		private const byte MessageTypeBarrier = 1;

		private Dictionary<string, INetObject<INetSerializable>> variables = new Dictionary<string, INetObject<INetSerializable>>();

		private Dictionary<string, HashSet<long>> barriers = new Dictionary<string, HashSet<long>>();

		public Action<bool> barrierPoll;

		private HashSet<long> barrierPlayers(string name)
		{
			if (!barriers.ContainsKey(name))
			{
				barriers[name] = new HashSet<long>();
			}
			return barriers[name];
		}

		private bool barrierReady(string name)
		{
			HashSet<long> playersReady = barrierPlayers(name);
			foreach (long id in Game1.otherFarmers.Keys)
			{
				if (!playersReady.Contains(id))
				{
					return false;
				}
			}
			return true;
		}

		private bool shouldAbort()
		{
			if (Game1.client != null)
			{
				return Game1.client.timedOut;
			}
			return false;
		}

		public void barrier(string name)
		{
			barrierPlayers(name).Add(Game1.player.UniqueMultiplayerID);
			sendMessage((byte)1, name);
			while (!barrierReady(name))
			{
				processMessages();
				if (shouldAbort())
				{
					throw new AbortNetSynchronizerException();
				}
				if (LocalMultiplayer.IsLocalMultiplayer())
				{
					break;
				}
			}
		}

		public bool isBarrierReady(string name)
		{
			if (!barrierReady(name))
			{
				processMessages();
				if (shouldAbort())
				{
					throw new AbortNetSynchronizerException();
				}
				return false;
			}
			return true;
		}

		public bool isVarReady(string varName)
		{
			if (!variables.ContainsKey(varName))
			{
				processMessages();
				if (shouldAbort())
				{
					throw new AbortNetSynchronizerException();
				}
				LocalMultiplayer.IsLocalMultiplayer();
				return false;
			}
			return true;
		}

		public T waitForVar<TField, T>(string varName) where TField : NetFieldBase<T, TField>, new()
		{
			while (!variables.ContainsKey(varName))
			{
				processMessages();
				if (shouldAbort())
				{
					throw new AbortNetSynchronizerException();
				}
			}
			return (variables[varName] as TField).Value;
		}

		public void sendVar<TField, T>(string varName, T value) where TField : NetFieldBase<T, TField>, new()
		{
			using MemoryStream stream = new MemoryStream();
			using BinaryWriter writer = new BinaryWriter(stream);
			NetRoot<TField> root = new NetRoot<TField>(new TField());
			root.Value.Value = value;
			root.WriteFull(writer);
			variables[varName] = root.Value;
			stream.Seek(0L, SeekOrigin.Begin);
			sendMessage((byte)0, varName, stream.ToArray());
		}

		public bool hasVar(string varName)
		{
			return variables.ContainsKey(varName);
		}

		public abstract void processMessages();

		protected abstract void sendMessage(params object[] data);

		public void receiveMessage(IncomingMessage msg)
		{
			switch (msg.Reader.ReadByte())
			{
			case 0:
			{
				string varName = msg.Reader.ReadString();
				NetRoot<INetObject<INetSerializable>> root = new NetRoot<INetObject<INetSerializable>>();
				root.ReadFull(msg.Reader, default(NetVersion));
				variables[varName] = root.Value;
				break;
			}
			case 1:
			{
				string barrierName = msg.Reader.ReadString();
				barrierPlayers(barrierName).Add(msg.FarmerID);
				break;
			}
			}
		}
	}
}
