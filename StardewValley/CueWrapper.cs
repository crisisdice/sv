using System;
using Microsoft.Xna.Framework.Audio;

namespace StardewValley
{
	public class CueWrapper : ICue, IDisposable
	{
		private Cue cue;

		public bool IsStopped => cue.IsStopped;

		public bool IsStopping => cue.IsStopping;

		public bool IsPlaying => cue.IsPlaying;

		public bool IsPaused => cue.IsPaused;

		public string Name => cue.Name;

		public float Volume
		{
			get
			{
				return cue.Volume;
			}
			set
			{
				cue.Volume = value;
			}
		}

		public float Pitch
		{
			get
			{
				return cue.Pitch;
			}
			set
			{
				cue.Pitch = value;
			}
		}

		public bool IsPitchBeingControlledByRPC => cue.IsPitchBeingControlledByRPC;

		public CueWrapper(Cue cue)
		{
			this.cue = cue;
		}

		public void Play()
		{
			cue.Play();
		}

		public void Pause()
		{
			cue.Pause();
		}

		public void Resume()
		{
			cue.Resume();
		}

		public void Stop(AudioStopOptions options)
		{
			cue.Stop(options);
		}

		public void SetVariable(string var, int val)
		{
			cue.SetVariable(var, val);
		}

		public void SetVariable(string var, float val)
		{
			cue.SetVariable(var, val);
		}

		public float GetVariable(string var)
		{
			return cue.GetVariable(var);
		}

		public void Dispose()
		{
			cue.Dispose();
			cue = null;
		}
	}
}
