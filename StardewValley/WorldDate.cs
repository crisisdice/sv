using System;
using System.Xml.Serialization;
using Netcode;

namespace StardewValley
{
	public class WorldDate : INetObject<NetFields>
	{
		public const int MonthsPerYear = 4;

		public const int DaysPerMonth = 28;

		private readonly NetInt year = new NetInt(1);

		private readonly NetInt seasonIndex = new NetInt(0);

		private readonly NetInt dayOfMonth = new NetInt(1);

		public int Year
		{
			get
			{
				return year.Value;
			}
			set
			{
				year.Value = value;
			}
		}

		[XmlIgnore]
		public int SeasonIndex
		{
			get
			{
				return seasonIndex.Value;
			}
			internal set
			{
				seasonIndex.Value = value;
			}
		}

		public int DayOfMonth
		{
			get
			{
				return dayOfMonth.Value;
			}
			set
			{
				dayOfMonth.Value = value;
			}
		}

		public DayOfWeek DayOfWeek => (DayOfWeek)(DayOfMonth % 7);

		public string Season
		{
			get
			{
				return SeasonIndex switch
				{
					0 => "spring", 
					1 => "summer", 
					2 => "fall", 
					3 => "winter", 
					_ => throw new ArgumentException(Convert.ToString(SeasonIndex)), 
				};
			}
			set
			{
				switch (value)
				{
				case "spring":
					SeasonIndex = 0;
					break;
				case "summer":
					SeasonIndex = 1;
					break;
				case "fall":
					SeasonIndex = 2;
					break;
				case "winter":
					SeasonIndex = 3;
					break;
				default:
					throw new ArgumentException(value);
				}
			}
		}

		public int TotalDays
		{
			get
			{
				return ((Year - 1) * 4 + SeasonIndex) * 28 + (DayOfMonth - 1);
			}
			set
			{
				int totalMonths = value / 28;
				DayOfMonth = value % 28 + 1;
				SeasonIndex = totalMonths % 4;
				Year = totalMonths / 4 + 1;
			}
		}

		public int TotalWeeks => TotalDays / 7;

		public int TotalSundayWeeks => (TotalDays + 1) / 7;

		public NetFields NetFields { get; } = new NetFields();


		public WorldDate()
		{
			NetFields.AddFields(year, seasonIndex, dayOfMonth);
		}

		public WorldDate(WorldDate other)
			: this()
		{
			Year = other.Year;
			SeasonIndex = other.SeasonIndex;
			DayOfMonth = other.DayOfMonth;
		}

		public WorldDate(int year, string season, int dayOfMonth)
			: this()
		{
			Year = year;
			Season = season;
			DayOfMonth = dayOfMonth;
		}

		public string Localize()
		{
			return Utility.getDateStringFor(DayOfMonth, SeasonIndex, Year);
		}

		public override string ToString()
		{
			return "Year " + Year + ", " + Season + " " + DayOfMonth + ", " + DayOfWeek;
		}

		public static bool operator ==(WorldDate a, WorldDate b)
		{
			return a?.TotalDays == b?.TotalDays;
		}

		public static bool operator !=(WorldDate a, WorldDate b)
		{
			return a?.TotalDays != b?.TotalDays;
		}

		public static bool operator <(WorldDate a, WorldDate b)
		{
			return a?.TotalDays < b?.TotalDays;
		}

		public static bool operator >(WorldDate a, WorldDate b)
		{
			return a?.TotalDays > b?.TotalDays;
		}

		public static bool operator <=(WorldDate a, WorldDate b)
		{
			return a?.TotalDays <= b?.TotalDays;
		}

		public static bool operator >=(WorldDate a, WorldDate b)
		{
			return a?.TotalDays >= b?.TotalDays;
		}
	}
}
