using System;
using Microsoft.Xna.Framework.Audio;

namespace StardewValley
{
	internal class AudioEngineWrapper : IAudioEngine, IDisposable
	{
		private AudioEngine audioEngine;

		public bool IsDisposed => audioEngine.IsDisposed;

		public AudioEngine Engine => audioEngine;

		public AudioEngineWrapper(AudioEngine engine)
		{
			audioEngine = engine;
		}

		public void Dispose()
		{
			audioEngine.Dispose();
		}

		public IAudioCategory GetCategory(string name)
		{
			return new AudioCategoryWrapper(audioEngine.GetCategory(name));
		}

		public int GetCategoryIndex(string name)
		{
			return audioEngine.GetCategoryIndex(name);
		}

		public void Update()
		{
			audioEngine.Update();
		}
	}
}
