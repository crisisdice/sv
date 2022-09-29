using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using StardewValley.GameData;

namespace StardewValley
{
	public class StartupPreferences
	{
		public const int windowed_borderless = 0;

		public const int windowed = 1;

		public const int fullscreen = 2;

		private static readonly string _filename = "startup_preferences";

		public static XmlSerializer serializer = new XmlSerializer(typeof(StartupPreferences));

		public bool startMuted;

		public bool levelTenFishing;

		public bool levelTenMining;

		public bool levelTenForaging;

		public bool levelTenCombat;

		public bool skipWindowPreparation;

		public bool sawAdvancedCharacterCreationIndicator;

		public int timesPlayed;

		public int windowMode;

		public int displayIndex = -1;

		public Options.GamepadModes gamepadMode;

		public int playerLimit = -1;

		public int fullscreenResolutionX;

		public int fullscreenResolutionY;

		public string lastEnteredIP = "";

		public string languageCode;

		public Options clientOptions = new Options();

		[XmlIgnore]
		public bool isLoaded;

		private bool _isBusy;

		private bool _pendingApplyLanguage;

		private Task _task;

		[XmlIgnore]
		public bool IsBusy
		{
			get
			{
				lock (this)
				{
					if (!_isBusy)
					{
						return false;
					}
					if (_task == null)
					{
						throw new Exception("StartupPreferences.IsBusy; was busy but task is null?");
					}
					if (_task.IsFaulted)
					{
						Exception baseException = _task.Exception.GetBaseException();
						Console.WriteLine("StartupPreferences._task failed with an exception");
						Console.WriteLine(baseException);
						throw baseException;
					}
					if (_task.IsCompleted)
					{
						_task = null;
						_isBusy = false;
						if (_pendingApplyLanguage)
						{
							_SetLanguageFromCode(languageCode);
						}
					}
					return _isBusy;
				}
			}
		}

		private void Init()
		{
			isLoaded = false;
			ensureFolderStructureExists();
		}

		public void OnLanguageChange(LocalizedContentManager.LanguageCode code)
		{
			string language_id = code.ToString();
			if (code == LocalizedContentManager.LanguageCode.mod && LocalizedContentManager.CurrentModLanguage != null)
			{
				language_id = LocalizedContentManager.CurrentModLanguage.ID;
			}
			if (isLoaded && languageCode != language_id)
			{
				savePreferences(async: false, update_language_from_ingame_language: true);
			}
		}

		private void ensureFolderStructureExists()
		{
			FileInfo info = new FileInfo(Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "StardewValley", "placeholder")));
			if (!info.Directory.Exists)
			{
				info.Directory.Create();
			}
			info = null;
		}

		public void savePreferences(bool async, bool update_language_from_ingame_language = false)
		{
			lock (this)
			{
				if (update_language_from_ingame_language)
				{
					if (LocalizedContentManager.CurrentLanguageCode == LocalizedContentManager.LanguageCode.mod)
					{
						languageCode = LocalizedContentManager.CurrentModLanguage.ID;
					}
					else
					{
						languageCode = LocalizedContentManager.CurrentLanguageCode.ToString();
					}
				}
				Console.WriteLine("savePreferences(); async={0}, languageCode={1}", async, languageCode);
				try
				{
					_savePreferences();
				}
				catch (Exception ex)
				{
					Exception baseException = ex.GetBaseException();
					Console.WriteLine("StartupPreferences._task failed with an exception");
					Console.WriteLine(baseException.GetType());
					Console.WriteLine(baseException.Message);
					Console.WriteLine(baseException.StackTrace);
					throw ex;
				}
			}
		}

		private void _savePreferences()
		{
			string fullFilePath = Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "StardewValley"), _filename);
			try
			{
				ensureFolderStructureExists();
				if (File.Exists(fullFilePath))
				{
					File.Delete(fullFilePath);
				}
				using FileStream stream = File.Create(fullFilePath);
				writeSettings(stream);
			}
			catch (Exception ex)
			{
				Game1.debugOutput = Game1.parseText(ex.Message);
			}
		}

		private long writeSettings(Stream stream)
		{
			XmlWriterSettings settings = new XmlWriterSettings();
			settings.CloseOutput = true;
			using XmlWriter writer = XmlWriter.Create(stream, settings);
			writer.WriteStartDocument();
			serializer.Serialize(writer, this);
			writer.WriteEndDocument();
			writer.Flush();
			return stream.Length;
		}

		public void loadPreferences(bool async, bool applyLanguage)
		{
			lock (this)
			{
				_pendingApplyLanguage = applyLanguage;
				Console.WriteLine("loadPreferences(); begin - languageCode={0}", languageCode);
				Init();
				try
				{
					_loadPreferences();
				}
				catch (Exception)
				{
					Exception baseException = _task.Exception.GetBaseException();
					Console.WriteLine("StartupPreferences._task failed with an exception");
					Console.WriteLine(baseException.GetType());
					Console.WriteLine(baseException.Message);
					Console.WriteLine(baseException.StackTrace);
					throw baseException;
				}
				if (applyLanguage)
				{
					_SetLanguageFromCode(languageCode);
				}
			}
		}

		protected virtual void _SetLanguageFromCode(string language_code_string)
		{
			List<ModLanguage> mod_languages = Game1.content.Load<List<ModLanguage>>("Data\\AdditionalLanguages");
			bool found_language = false;
			if (mod_languages != null)
			{
				foreach (ModLanguage mod_language in mod_languages)
				{
					if (mod_language.ID == language_code_string)
					{
						LocalizedContentManager.SetModLanguage(mod_language);
						found_language = true;
						break;
					}
				}
			}
			if (!found_language)
			{
				LocalizedContentManager.LanguageCode language_code = LocalizedContentManager.LanguageCode.en;
				if (Enum.TryParse<LocalizedContentManager.LanguageCode>(language_code_string, out language_code) && language_code != LocalizedContentManager.LanguageCode.mod)
				{
					LocalizedContentManager.CurrentLanguageCode = language_code;
				}
				else
				{
					LocalizedContentManager.CurrentLanguageCode = LocalizedContentManager.GetDefaultLanguageCode();
				}
			}
		}

		private void _loadPreferences()
		{
			string fullFilePath = Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "StardewValley"), _filename);
			if (!File.Exists(fullFilePath))
			{
				Console.WriteLine("path '{0}' did not exist and will be created", fullFilePath);
				try
				{
					if (LocalizedContentManager.CurrentLanguageCode == LocalizedContentManager.LanguageCode.mod)
					{
						languageCode = LocalizedContentManager.CurrentModLanguage.ID;
					}
					else
					{
						languageCode = LocalizedContentManager.CurrentLanguageCode.ToString();
					}
					using FileStream stream2 = File.Create(fullFilePath);
					writeSettings(stream2);
				}
				catch (Exception e2)
				{
					Console.WriteLine("_loadPreferences; exception occured trying to create/write: {0}", e2);
					Game1.debugOutput = Game1.parseText(e2.Message);
					return;
				}
			}
			try
			{
				using (FileStream stream = File.Open(fullFilePath, FileMode.Open, FileAccess.Read))
				{
					readSettings(stream);
				}
				isLoaded = true;
			}
			catch (Exception e)
			{
				Console.WriteLine("_loadPreferences; exception occured trying open/read: {0}", e);
				Game1.debugOutput = Game1.parseText(e.Message);
			}
		}

		private void readSettings(Stream stream)
		{
			StartupPreferences p = (StartupPreferences)serializer.Deserialize(stream);
			startMuted = p.startMuted;
			timesPlayed = p.timesPlayed + 1;
			levelTenCombat = p.levelTenCombat;
			levelTenFishing = p.levelTenFishing;
			levelTenForaging = p.levelTenForaging;
			levelTenMining = p.levelTenMining;
			skipWindowPreparation = p.skipWindowPreparation;
			windowMode = p.windowMode;
			displayIndex = p.displayIndex;
			playerLimit = p.playerLimit;
			gamepadMode = p.gamepadMode;
			fullscreenResolutionX = p.fullscreenResolutionX;
			fullscreenResolutionY = p.fullscreenResolutionY;
			lastEnteredIP = p.lastEnteredIP;
			languageCode = p.languageCode;
			clientOptions = p.clientOptions;
		}
	}
}
