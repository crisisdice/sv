using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading;
using Microsoft.Xna.Framework.Content;
using StardewValley.GameData;

namespace StardewValley
{
	public class LocalizedContentManager : ContentManager
	{
		public delegate void LanguageChangedHandler(LanguageCode code);

		public enum LanguageCode
		{
			en,
			ja,
			ru,
			zh,
			pt,
			es,
			de,
			th,
			fr,
			ko,
			it,
			tr,
			hu,
			mod
		}

		public const string genderDialogueSplitCharacter = "¦";

		private static LanguageCode _currentLangCode = GetDefaultLanguageCode();

		private static ModLanguage _currentModLanguage = null;

		public CultureInfo CurrentCulture;

		protected static StringBuilder _timeFormatStringBuilder = new StringBuilder();

		public static readonly Dictionary<string, string> localizedAssetNames = new Dictionary<string, string>();

		public static LanguageCode CurrentLanguageCode
		{
			get
			{
				return _currentLangCode;
			}
			set
			{
				if (_currentLangCode != value)
				{
					LanguageCode prev = _currentLangCode;
					_currentLangCode = value;
					if (_currentLangCode != LanguageCode.mod)
					{
						_currentModLanguage = null;
					}
					Console.WriteLine("LocalizedContentManager.CurrentLanguageCode CHANGING from '{0}' to '{1}'", prev, _currentLangCode);
					LocalizedContentManager.OnLanguageChange?.Invoke(_currentLangCode);
					Console.WriteLine("LocalizedContentManager.CurrentLanguageCode CHANGED from '{0}' to '{1}'", prev, _currentLangCode);
				}
			}
		}

		public static bool CurrentLanguageLatin
		{
			get
			{
				if (CurrentLanguageCode != 0 && CurrentLanguageCode != LanguageCode.es && CurrentLanguageCode != LanguageCode.de && CurrentLanguageCode != LanguageCode.pt && CurrentLanguageCode != LanguageCode.fr && CurrentLanguageCode != LanguageCode.it && CurrentLanguageCode != LanguageCode.tr && CurrentLanguageCode != LanguageCode.hu)
				{
					if (CurrentLanguageCode == LanguageCode.mod)
					{
						return _currentModLanguage.UseLatinFont;
					}
					return false;
				}
				return true;
			}
		}

		public static ModLanguage CurrentModLanguage => _currentModLanguage;

		public static event LanguageChangedHandler OnLanguageChange;

		public static LanguageCode GetDefaultLanguageCode()
		{
			return LanguageCode.en;
		}

		public LocalizedContentManager(IServiceProvider serviceProvider, string rootDirectory, CultureInfo currentCulture)
			: base(serviceProvider, rootDirectory)
		{
			CurrentCulture = currentCulture;
		}

		public LocalizedContentManager(IServiceProvider serviceProvider, string rootDirectory)
			: this(serviceProvider, rootDirectory, Thread.CurrentThread.CurrentUICulture)
		{
		}

		protected static bool _IsStringAt(string source, string string_to_find, int index)
		{
			for (int i = 0; i < string_to_find.Length; i++)
			{
				int source_index = index + i;
				if (source_index >= source.Length)
				{
					return false;
				}
				if (source[source_index] != string_to_find[i])
				{
					return false;
				}
			}
			return true;
		}

		public static StringBuilder FormatTimeString(int time, string format)
		{
			_timeFormatStringBuilder.Clear();
			int brace_start_index = -1;
			for (int i = 0; i < format.Length; i++)
			{
				char character = format[i];
				switch (character)
				{
				case '[':
				{
					if (brace_start_index < 0)
					{
						brace_start_index = i;
						continue;
					}
					for (int k = brace_start_index; k <= i; k++)
					{
						_timeFormatStringBuilder.Append(format[k]);
					}
					brace_start_index = i;
					continue;
				}
				case ']':
					if (brace_start_index < 0)
					{
						break;
					}
					if (_IsStringAt(format, "[HOURS_12]", brace_start_index))
					{
						_timeFormatStringBuilder.Append((time / 100 % 12 == 0) ? "12" : (time / 100 % 12).ToString());
					}
					else if (_IsStringAt(format, "[HOURS_12_0]", brace_start_index))
					{
						_timeFormatStringBuilder.Append((time / 100 % 12 == 0) ? "0" : (time / 100 % 12).ToString());
					}
					else if (_IsStringAt(format, "[HOURS_24]", brace_start_index))
					{
						_timeFormatStringBuilder.Append(time / 100 % 24);
					}
					else if (_IsStringAt(format, "[HOURS_24_00]", brace_start_index))
					{
						_timeFormatStringBuilder.Append((time / 100 % 24).ToString("00"));
					}
					else if (_IsStringAt(format, "[MINUTES]", brace_start_index))
					{
						_timeFormatStringBuilder.Append((time % 100).ToString("00"));
					}
					else if (_IsStringAt(format, "[AM_PM]", brace_start_index))
					{
						if (time < 1200 || time >= 2400)
						{
							_timeFormatStringBuilder.Append(Game1.content.LoadString("Strings\\StringsFromCSFiles:DayTimeMoneyBox.cs.10370"));
						}
						else
						{
							_timeFormatStringBuilder.Append(Game1.content.LoadString("Strings\\StringsFromCSFiles:DayTimeMoneyBox.cs.10371"));
						}
					}
					else
					{
						for (int j = brace_start_index; j <= i; j++)
						{
							_timeFormatStringBuilder.Append(format[j]);
						}
					}
					brace_start_index = -1;
					continue;
				}
				if (brace_start_index < 0)
				{
					_timeFormatStringBuilder.Append(character);
				}
			}
			return _timeFormatStringBuilder;
		}

		public static void SetModLanguage(ModLanguage new_mod_language)
		{
			if (new_mod_language != _currentModLanguage)
			{
				_currentModLanguage = new_mod_language;
				CurrentLanguageCode = LanguageCode.mod;
			}
		}

		public virtual T LoadBase<T>(string assetName)
		{
			return base.Load<T>(assetName);
		}

		public override T Load<T>(string assetName)
		{
			return Load<T>(assetName, CurrentLanguageCode);
		}

		public virtual T Load<T>(string assetName, LanguageCode language)
		{
			if (language != 0)
			{
				if (!localizedAssetNames.TryGetValue(assetName, out var _))
				{
					string localizedAssetName = assetName + "." + LanguageCodeString(language);
					try
					{
						base.Load<T>(localizedAssetName);
						localizedAssetNames[assetName] = localizedAssetName;
					}
					catch (ContentLoadException)
					{
						localizedAssetName = assetName + "_international";
						try
						{
							base.Load<T>(localizedAssetName);
							localizedAssetNames[assetName] = localizedAssetName;
						}
						catch (ContentLoadException)
						{
							localizedAssetNames[assetName] = assetName;
						}
					}
				}
				return base.Load<T>(localizedAssetNames[assetName]);
			}
			return base.Load<T>(assetName);
		}

		public string LanguageCodeString(LanguageCode code)
		{
			string retVal = "";
			switch (code)
			{
			case LanguageCode.ja:
				retVal = "ja-JP";
				break;
			case LanguageCode.ru:
				retVal = "ru-RU";
				break;
			case LanguageCode.zh:
				retVal = "zh-CN";
				break;
			case LanguageCode.pt:
				retVal = "pt-BR";
				break;
			case LanguageCode.es:
				retVal = "es-ES";
				break;
			case LanguageCode.de:
				retVal = "de-DE";
				break;
			case LanguageCode.th:
				retVal = "th-TH";
				break;
			case LanguageCode.fr:
				retVal = "fr-FR";
				break;
			case LanguageCode.ko:
				retVal = "ko-KR";
				break;
			case LanguageCode.it:
				retVal = "it-IT";
				break;
			case LanguageCode.tr:
				retVal = "tr-TR";
				break;
			case LanguageCode.hu:
				retVal = "hu-HU";
				break;
			case LanguageCode.mod:
				retVal = _currentModLanguage.LanguageCode;
				break;
			}
			return retVal;
		}

		public LanguageCode GetCurrentLanguage()
		{
			return CurrentLanguageCode;
		}

		private string GetString(Dictionary<string, string> strings, string key)
		{
			if (strings.TryGetValue(key + ".desktop", out var result))
			{
				return result;
			}
			return strings[key];
		}

		public virtual string LoadStringReturnNullIfNotFound(string path)
		{
			string result = LoadString(path);
			if (!result.Equals(path))
			{
				return result;
			}
			return null;
		}

		public virtual string LoadString(string path)
		{
			parseStringPath(path, out var assetName, out var key);
			Dictionary<string, string> strings = Load<Dictionary<string, string>>(assetName);
			if (strings != null && strings.ContainsKey(key))
			{
				string sentence = GetString(strings, key);
				if (sentence.Contains("¦"))
				{
					sentence = ((!Game1.player.IsMale) ? sentence.Substring(sentence.IndexOf("¦") + 1) : sentence.Substring(0, sentence.IndexOf("¦")));
				}
				return sentence;
			}
			return LoadBaseString(path);
		}

		public virtual bool ShouldUseGenderedCharacterTranslations()
		{
			if (CurrentLanguageCode == LanguageCode.pt)
			{
				return true;
			}
			if (CurrentLanguageCode == LanguageCode.mod && CurrentModLanguage != null)
			{
				return CurrentModLanguage.UseGenderedCharacterTranslations;
			}
			return false;
		}

		public virtual string LoadString(string path, object sub1)
		{
			string sentence = LoadString(path);
			try
			{
				return string.Format(sentence, sub1);
			}
			catch (Exception)
			{
				return sentence;
			}
		}

		public virtual string LoadString(string path, object sub1, object sub2)
		{
			string sentence = LoadString(path);
			try
			{
				return string.Format(sentence, sub1, sub2);
			}
			catch (Exception)
			{
				return sentence;
			}
		}

		public virtual string LoadString(string path, object sub1, object sub2, object sub3)
		{
			string sentence = LoadString(path);
			try
			{
				return string.Format(sentence, sub1, sub2, sub3);
			}
			catch (Exception)
			{
				return sentence;
			}
		}

		public virtual string LoadString(string path, params object[] substitutions)
		{
			string sentence = LoadString(path);
			if (substitutions.Length != 0)
			{
				try
				{
					return string.Format(sentence, substitutions);
				}
				catch (Exception)
				{
					return sentence;
				}
			}
			return sentence;
		}

		public virtual string LoadBaseString(string path)
		{
			parseStringPath(path, out var assetName, out var key);
			Dictionary<string, string> strings = base.Load<Dictionary<string, string>>(assetName);
			if (strings != null && strings.ContainsKey(key))
			{
				return GetString(strings, key);
			}
			return path;
		}

		private void parseStringPath(string path, out string assetName, out string key)
		{
			int i = path.IndexOf(':');
			if (i == -1)
			{
				throw new ContentLoadException("Unable to parse string path: " + path);
			}
			assetName = path.Substring(0, i);
			key = path.Substring(i + 1, path.Length - i - 1);
		}

		public virtual LocalizedContentManager CreateTemporary()
		{
			return new LocalizedContentManager(base.ServiceProvider, base.RootDirectory, CurrentCulture);
		}
	}
}
