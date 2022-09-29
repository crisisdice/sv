using System;
using Microsoft.Xna.Framework.Audio;

namespace StardewValley
{
	public class SoundBankWrapper : ISoundBank, IDisposable
	{
		private SoundBank soundBank;

		public bool IsInUse => soundBank.IsInUse;

		public bool IsDisposed => soundBank.IsDisposed;

		public SoundBankWrapper(SoundBank soundBank)
		{
			this.soundBank = soundBank;
		}

		public ICue GetCue(string name)
		{
			return new CueWrapper(soundBank.GetCue(name));
		}

		public void PlayCue(string name)
		{
			soundBank.PlayCue(name);
		}

		public void PlayCue(string name, AudioListener listener, AudioEmitter emitter)
		{
			soundBank.PlayCue(name, listener, emitter);
		}

		public void Dispose()
		{
			soundBank.Dispose();
		}

		public void AddCue(CueDefinition cue_definition)
		{
			soundBank.AddCue(cue_definition);
		}

		public CueDefinition GetCueDefinition(string name)
		{
			return soundBank.GetCueDefinition(name);
		}
	}
}
