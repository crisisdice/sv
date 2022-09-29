using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Netcode;

namespace StardewValley.Events
{
	public interface FarmEvent : INetObject<NetFields>
	{
		/// <summary>
		/// return true if the event wasn't able to set up and should be skipped
		/// </summary>
		bool setUp();

		bool tickUpdate(GameTime time);

		void draw(SpriteBatch b);

		void drawAboveEverything(SpriteBatch b);

		void makeChangesToLocation();
	}
}
