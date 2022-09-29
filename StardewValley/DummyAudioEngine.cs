using System;
using Microsoft.Xna.Framework.Audio;

namespace StardewValley
{
	internal class DummyAudioEngine : IAudioEngine, IDisposable
	{
		private IAudioCategory category = new DummyAudioCategory();

		public bool IsDisposed => true;

		public AudioEngine Engine => null;

		public void Update()
		{
		}

		public IAudioCategory GetCategory(string name)
		{
			return category;
		}

		public int GetCategoryIndex(string name)
		{
			return -1;
		}

		public void Dispose()
		{
		}
	}
}
