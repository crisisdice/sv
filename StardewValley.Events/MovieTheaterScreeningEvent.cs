using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using StardewValley.GameData.Movies;
using StardewValley.Locations;

namespace StardewValley.Events
{
	public class MovieTheaterScreeningEvent
	{
		public int currentResponse;

		public List<List<Character>> playerAndGuestAudienceGroups;

		public Dictionary<int, Character> _responseOrder = new Dictionary<int, Character>();

		protected Dictionary<Character, Character> _whiteListDependencyLookup;

		protected Dictionary<Character, string> _characterResponses;

		public MovieData movieData;

		protected List<Farmer> _farmers;

		protected Dictionary<Character, MovieConcession> _concessionsData;

		public Event getMovieEvent(string movieID, List<List<Character>> player_and_guest_audience_groups, List<List<Character>> npcOnlyAudienceGroups, Dictionary<Character, MovieConcession> concessions_data = null)
		{
			_concessionsData = concessions_data;
			_responseOrder = new Dictionary<int, Character>();
			_whiteListDependencyLookup = new Dictionary<Character, Character>();
			_characterResponses = new Dictionary<Character, string>();
			movieData = MovieTheater.GetMovieData()[movieID];
			playerAndGuestAudienceGroups = player_and_guest_audience_groups;
			currentResponse = 0;
			StringBuilder sb = new StringBuilder();
			Random theaterRandom = new Random((int)(Game1.stats.DaysPlayed + Game1.uniqueIDForThisGame / 2uL));
			sb.Append("movieScreenAmbience/-2000 -2000/");
			string playerCharacterEventName = "farmer" + Utility.getFarmerNumberFromFarmer(Game1.player);
			string playerCharacterGuestName = "";
			foreach (List<Character> list in playerAndGuestAudienceGroups)
			{
				if (!list.Contains(Game1.player))
				{
					continue;
				}
				for (int i7 = 0; i7 < list.Count; i7++)
				{
					if (!(list[i7] is Farmer))
					{
						playerCharacterGuestName = list[i7].name;
					}
				}
			}
			_farmers = new List<Farmer>();
			foreach (List<Character> playerAndGuestAudienceGroup in playerAndGuestAudienceGroups)
			{
				foreach (Character character3 in playerAndGuestAudienceGroup)
				{
					if (character3 is Farmer && !_farmers.Contains(character3))
					{
						_farmers.Add(character3 as Farmer);
					}
				}
			}
			List<Character> allAudience = playerAndGuestAudienceGroups.SelectMany((List<Character> x) => x).ToList();
			allAudience.AddRange(npcOnlyAudienceGroups.SelectMany((List<Character> x) => x).ToList());
			bool first = true;
			foreach (Character c2 in allAudience)
			{
				if (c2 != null)
				{
					if (!first)
					{
						sb.Append(" ");
					}
					if (c2 is Farmer)
					{
						Farmer f = c2 as Farmer;
						sb.Append("farmer" + Utility.getFarmerNumberFromFarmer(f));
					}
					else if (c2.name == "Krobus")
					{
						sb.Append("Krobus_Trenchcoat");
					}
					else
					{
						sb.Append(c2.name);
					}
					sb.Append(" -1000 -1000 0");
					first = false;
				}
			}
			sb.Append("/changeToTemporaryMap MovieTheaterScreen false/specificTemporarySprite movieTheater_setup/ambientLight 0 0 0/");
			string[] backRow = new string[8];
			playerAndGuestAudienceGroups = playerAndGuestAudienceGroups.OrderBy((List<Character> x) => theaterRandom.Next()).ToList();
			int startingSeat = theaterRandom.Next(8 - playerAndGuestAudienceGroups.SelectMany((List<Character> x) => x).Count() + 1);
			int whichGroup = 0;
			for (int i6 = 0; i6 < 8; i6++)
			{
				int seat7 = (i6 + startingSeat) % 8;
				if (playerAndGuestAudienceGroups[whichGroup].Count == 2 && (seat7 == 3 || seat7 == 7))
				{
					i6++;
					seat7++;
					seat7 %= 8;
				}
				for (int j5 = 0; j5 < playerAndGuestAudienceGroups[whichGroup].Count && seat7 + j5 < backRow.Length; j5++)
				{
					backRow[seat7 + j5] = ((playerAndGuestAudienceGroups[whichGroup][j5] is Farmer) ? ("farmer" + Utility.getFarmerNumberFromFarmer(playerAndGuestAudienceGroups[whichGroup][j5] as Farmer)) : ((string)playerAndGuestAudienceGroups[whichGroup][j5].name));
					if (j5 > 0)
					{
						i6++;
					}
				}
				whichGroup++;
				if (whichGroup >= playerAndGuestAudienceGroups.Count)
				{
					break;
				}
			}
			string[] midRow = new string[6];
			for (int j4 = 0; j4 < npcOnlyAudienceGroups.Count; j4++)
			{
				int seat = theaterRandom.Next(3 - npcOnlyAudienceGroups[j4].Count + 1) + j4 * 3;
				for (int i = 0; i < npcOnlyAudienceGroups[j4].Count; i++)
				{
					midRow[seat + i] = npcOnlyAudienceGroups[j4][i].name;
				}
			}
			int soFar = 0;
			int sittingTogetherCount = 0;
			for (int i5 = 0; i5 < backRow.Length; i5++)
			{
				if (backRow[i5] == null || !(backRow[i5] != "") || !(backRow[i5] != playerCharacterEventName) || !(backRow[i5] != playerCharacterGuestName))
				{
					continue;
				}
				soFar++;
				if (soFar < 2)
				{
					continue;
				}
				sittingTogetherCount++;
				Point seat2 = getBackRowSeatTileFromIndex(i5);
				sb.Append("warp ").Append(backRow[i5]).Append(" ")
					.Append(seat2.X)
					.Append(" ")
					.Append(seat2.Y)
					.Append("/positionOffset ")
					.Append(backRow[i5])
					.Append(" 0 -10/");
				if (sittingTogetherCount == 2)
				{
					sittingTogetherCount = 0;
					if (theaterRandom.NextDouble() < 0.5 && backRow[i5] != playerCharacterGuestName && backRow[i5 - 1] != playerCharacterGuestName)
					{
						sb.Append("faceDirection " + backRow[i5] + " 3 true/");
						sb.Append("faceDirection " + backRow[i5 - 1] + " 1 true/");
					}
				}
			}
			soFar = 0;
			sittingTogetherCount = 0;
			for (int i4 = 0; i4 < midRow.Length; i4++)
			{
				if (midRow[i4] == null || !(midRow[i4] != ""))
				{
					continue;
				}
				soFar++;
				if (soFar < 2)
				{
					continue;
				}
				sittingTogetherCount++;
				Point seat3 = getMidRowSeatTileFromIndex(i4);
				sb.Append("warp ").Append(midRow[i4]).Append(" ")
					.Append(seat3.X)
					.Append(" ")
					.Append(seat3.Y)
					.Append("/positionOffset ")
					.Append(midRow[i4])
					.Append(" 0 -10/");
				if (sittingTogetherCount == 2)
				{
					sittingTogetherCount = 0;
					if (i4 != 3 && theaterRandom.NextDouble() < 0.5)
					{
						sb.Append("faceDirection " + midRow[i4] + " 3 true/");
						sb.Append("faceDirection " + midRow[i4 - 1] + " 1 true/");
					}
				}
			}
			Point warpPoint = new Point(1, 15);
			soFar = 0;
			for (int i3 = 0; i3 < backRow.Length; i3++)
			{
				if (backRow[i3] != null && backRow[i3] != "" && backRow[i3] != playerCharacterEventName && backRow[i3] != playerCharacterGuestName)
				{
					Point seat6 = getBackRowSeatTileFromIndex(i3);
					if (soFar == 1)
					{
						sb.Append("warp ").Append(backRow[i3]).Append(" ")
							.Append(seat6.X - 1)
							.Append(" 10")
							.Append("/advancedMove ")
							.Append(backRow[i3])
							.Append(" false 1 " + 200 + " 1 0 4 1000/")
							.Append("positionOffset ")
							.Append(backRow[i3])
							.Append(" 0 -10/");
					}
					else
					{
						sb.Append("warp ").Append(backRow[i3]).Append(" 1 12")
							.Append("/advancedMove ")
							.Append(backRow[i3])
							.Append(" false 1 200 ")
							.Append("0 -2 ")
							.Append(seat6.X - 1)
							.Append(" 0 4 1000/")
							.Append("positionOffset ")
							.Append(backRow[i3])
							.Append(" 0 -10/");
					}
					soFar++;
				}
				if (soFar >= 2)
				{
					break;
				}
			}
			soFar = 0;
			for (int i2 = 0; i2 < midRow.Length; i2++)
			{
				if (midRow[i2] != null && midRow[i2] != "")
				{
					Point seat5 = getMidRowSeatTileFromIndex(i2);
					if (soFar == 1)
					{
						sb.Append("warp ").Append(midRow[i2]).Append(" ")
							.Append(seat5.X - 1)
							.Append(" 8")
							.Append("/advancedMove ")
							.Append(midRow[i2])
							.Append(" false 1 " + 400 + " 1 0 4 1000/");
					}
					else
					{
						sb.Append("warp ").Append(midRow[i2]).Append(" 2 9")
							.Append("/advancedMove ")
							.Append(midRow[i2])
							.Append(" false 1 300 ")
							.Append("0 -1 ")
							.Append(seat5.X - 2)
							.Append(" 0 4 1000/");
					}
					soFar++;
				}
				if (soFar >= 2)
				{
					break;
				}
			}
			sb.Append("viewport 6 8 true/pause 500/");
			for (int n = 0; n < backRow.Length; n++)
			{
				if (backRow[n] != null && backRow[n] != "")
				{
					Point seat4 = getBackRowSeatTileFromIndex(n);
					if (backRow[n] == playerCharacterEventName || backRow[n] == playerCharacterGuestName)
					{
						sb.Append("warp ").Append(backRow[n]).Append(" ")
							.Append(warpPoint.X)
							.Append(" ")
							.Append(warpPoint.Y)
							.Append("/advancedMove ")
							.Append(backRow[n])
							.Append(" false 0 -5 ")
							.Append(seat4.X - warpPoint.X)
							.Append(" 0 4 1000/")
							.Append("pause ")
							.Append(1000)
							.Append("/");
					}
				}
			}
			sb.Append("pause 3000/proceedPosition ").Append(playerCharacterGuestName).Append("/pause 1000");
			if (playerCharacterGuestName.Equals(""))
			{
				sb.Append("/proceedPosition farmer");
			}
			sb.Append("/waitForAllStationary/pause 100");
			foreach (Character c in allAudience)
			{
				if (getEventName(c) != playerCharacterEventName && getEventName(c) != playerCharacterGuestName)
				{
					if (c is Farmer)
					{
						sb.Append("/faceDirection ").Append(getEventName(c)).Append(" 0 true/positionOffset ")
							.Append(getEventName(c))
							.Append(" 0 42 true");
					}
					else
					{
						sb.Append("/faceDirection ").Append(getEventName(c)).Append(" 0 true/positionOffset ")
							.Append(getEventName(c))
							.Append(" 0 12 true");
					}
					if (theaterRandom.NextDouble() < 0.2)
					{
						sb.Append("/pause 100");
					}
				}
			}
			sb.Append("/positionOffset ").Append(playerCharacterEventName).Append(" 0 32/positionOffset ")
				.Append(playerCharacterGuestName)
				.Append(" 0 8/ambientLight 210 210 120 true/pause 500/viewport move 0 -1 4000/pause 5000");
			List<Character> responding_characters = new List<Character>();
			foreach (List<Character> playerAndGuestAudienceGroup2 in playerAndGuestAudienceGroups)
			{
				foreach (Character character2 in playerAndGuestAudienceGroup2)
				{
					if (!(character2 is Farmer) && !responding_characters.Contains(character2))
					{
						responding_characters.Add(character2);
					}
				}
			}
			for (int m = 0; m < responding_characters.Count; m++)
			{
				int index = theaterRandom.Next(responding_characters.Count);
				Character character = responding_characters[m];
				responding_characters[m] = responding_characters[index];
				responding_characters[index] = character;
			}
			int current_response_index = 0;
			foreach (MovieScene scene2 in movieData.Scenes)
			{
				if (scene2.ResponsePoint == null)
				{
					continue;
				}
				bool found_reaction = false;
				for (int l = 0; l < responding_characters.Count; l++)
				{
					MovieCharacterReaction reaction2 = MovieTheater.GetReactionsForCharacter(responding_characters[l] as NPC);
					if (reaction2 == null)
					{
						continue;
					}
					foreach (MovieReaction movie_reaction2 in reaction2.Reactions)
					{
						if (!movie_reaction2.ShouldApplyToMovie(movieData, MovieTheater.GetPatronNames(), MovieTheater.GetResponseForMovie(responding_characters[l] as NPC)) || movie_reaction2.SpecialResponses == null || movie_reaction2.SpecialResponses.DuringMovie == null || (!(movie_reaction2.SpecialResponses.DuringMovie.ResponsePoint == scene2.ResponsePoint) && movie_reaction2.Whitelist.Count <= 0))
						{
							continue;
						}
						if (!_whiteListDependencyLookup.ContainsKey(responding_characters[l]))
						{
							_responseOrder[current_response_index] = responding_characters[l];
							if (movie_reaction2.Whitelist != null)
							{
								for (int j3 = 0; j3 < movie_reaction2.Whitelist.Count; j3++)
								{
									Character white_list_character2 = Game1.getCharacterFromName(movie_reaction2.Whitelist[j3]);
									if (white_list_character2 == null)
									{
										continue;
									}
									_whiteListDependencyLookup[white_list_character2] = responding_characters[l];
									foreach (int key2 in _responseOrder.Keys)
									{
										if (_responseOrder[key2] == white_list_character2)
										{
											_responseOrder.Remove(key2);
										}
									}
								}
							}
						}
						responding_characters.RemoveAt(l);
						l--;
						found_reaction = true;
						break;
					}
					if (found_reaction)
					{
						break;
					}
				}
				if (!found_reaction)
				{
					for (int k = 0; k < responding_characters.Count; k++)
					{
						MovieCharacterReaction reaction = MovieTheater.GetReactionsForCharacter(responding_characters[k] as NPC);
						if (reaction == null)
						{
							continue;
						}
						foreach (MovieReaction movie_reaction in reaction.Reactions)
						{
							if (!movie_reaction.ShouldApplyToMovie(movieData, MovieTheater.GetPatronNames(), MovieTheater.GetResponseForMovie(responding_characters[k] as NPC)) || movie_reaction.SpecialResponses == null || movie_reaction.SpecialResponses.DuringMovie == null || !(movie_reaction.SpecialResponses.DuringMovie.ResponsePoint == current_response_index.ToString()))
							{
								continue;
							}
							if (!_whiteListDependencyLookup.ContainsKey(responding_characters[k]))
							{
								_responseOrder[current_response_index] = responding_characters[k];
								if (movie_reaction.Whitelist != null)
								{
									for (int j2 = 0; j2 < movie_reaction.Whitelist.Count; j2++)
									{
										Character white_list_character = Game1.getCharacterFromName(movie_reaction.Whitelist[j2]);
										if (white_list_character == null)
										{
											continue;
										}
										_whiteListDependencyLookup[white_list_character] = responding_characters[k];
										foreach (int key in _responseOrder.Keys)
										{
											if (_responseOrder[key] == white_list_character)
											{
												_responseOrder.Remove(key);
											}
										}
									}
								}
							}
							responding_characters.RemoveAt(k);
							k--;
							found_reaction = true;
							break;
						}
						if (found_reaction)
						{
							break;
						}
					}
				}
				current_response_index++;
			}
			current_response_index = 0;
			for (int j = 0; j < responding_characters.Count; j++)
			{
				if (!_whiteListDependencyLookup.ContainsKey(responding_characters[j]))
				{
					for (; _responseOrder.ContainsKey(current_response_index); current_response_index++)
					{
					}
					_responseOrder[current_response_index] = responding_characters[j];
					current_response_index++;
				}
			}
			responding_characters = null;
			foreach (MovieScene scene in movieData.Scenes)
			{
				_ParseScene(sb, scene);
			}
			while (currentResponse < _responseOrder.Count)
			{
				_ParseResponse(sb);
			}
			sb.Append("/stopMusic");
			sb.Append("/fade/viewport -1000 -1000");
			sb.Append("/pause 500/message \"" + Game1.content.LoadString("Strings\\Locations:Theater_MovieEnd") + "\"/pause 500");
			sb.Append("/requestMovieEnd");
			Console.WriteLine(sb.ToString());
			return new Event(sb.ToString());
		}

		protected void _ParseScene(StringBuilder sb, MovieScene scene)
		{
			if (scene.Sound != "")
			{
				sb.Append("/playSound " + scene.Sound);
			}
			if (scene.Music != "")
			{
				sb.Append("/playMusic " + scene.Music);
			}
			if (scene.MessageDelay > 0)
			{
				sb.Append("/pause " + scene.MessageDelay);
			}
			if (scene.Image >= 0)
			{
				sb.Append("/specificTemporarySprite movieTheater_screen " + movieData.SheetIndex + " " + scene.Image + " " + scene.Shake);
			}
			if (scene.Script != "")
			{
				sb.Append(scene.Script);
			}
			if (scene.Text != "")
			{
				sb.Append("/message \"" + scene.Text + "\"");
			}
			if (scene.ResponsePoint != null)
			{
				_ParseResponse(sb, scene);
			}
		}

		protected void _ParseResponse(StringBuilder sb, MovieScene scene = null)
		{
			if (_responseOrder.ContainsKey(currentResponse))
			{
				sb.Append("/pause 500");
				Character responding_character = _responseOrder[currentResponse];
				bool hadUniqueScript = false;
				if (!_whiteListDependencyLookup.ContainsKey(responding_character))
				{
					MovieCharacterReaction reaction = MovieTheater.GetReactionsForCharacter(responding_character as NPC);
					if (reaction != null)
					{
						foreach (MovieReaction movie_reaction in reaction.Reactions)
						{
							if (movie_reaction.ShouldApplyToMovie(movieData, MovieTheater.GetPatronNames(), MovieTheater.GetResponseForMovie(responding_character as NPC)) && movie_reaction.SpecialResponses != null && movie_reaction.SpecialResponses.DuringMovie != null && (movie_reaction.SpecialResponses.DuringMovie.ResponsePoint == null || movie_reaction.SpecialResponses.DuringMovie.ResponsePoint == "" || (scene != null && movie_reaction.SpecialResponses.DuringMovie.ResponsePoint == scene.ResponsePoint) || movie_reaction.SpecialResponses.DuringMovie.ResponsePoint == currentResponse.ToString() || movie_reaction.Whitelist.Count > 0))
							{
								if (movie_reaction.SpecialResponses.DuringMovie.Script != "")
								{
									sb.Append(movie_reaction.SpecialResponses.DuringMovie.Script);
									hadUniqueScript = true;
								}
								if (movie_reaction.SpecialResponses.DuringMovie.Text != "")
								{
									sb.Append(string.Concat("/speak ", responding_character.name, " \"", movie_reaction.SpecialResponses.DuringMovie.Text, "\""));
								}
								break;
							}
						}
					}
				}
				_ParseCharacterResponse(sb, responding_character, hadUniqueScript);
				foreach (Character key in _whiteListDependencyLookup.Keys)
				{
					if (_whiteListDependencyLookup[key] == responding_character)
					{
						_ParseCharacterResponse(sb, key);
					}
				}
			}
			currentResponse++;
		}

		protected void _ParseCharacterResponse(StringBuilder sb, Character responding_character, bool ignoreScript = false)
		{
			string response = MovieTheater.GetResponseForMovie(responding_character as NPC);
			if (_whiteListDependencyLookup.ContainsKey(responding_character))
			{
				response = MovieTheater.GetResponseForMovie(_whiteListDependencyLookup[responding_character] as NPC);
			}
			switch (response)
			{
			case "love":
				sb.Append("/friendship " + responding_character.Name + " " + 200);
				if (!ignoreScript)
				{
					sb.Append(string.Concat("/playSound reward/emote ", responding_character.name, " ", 20.ToString(), "/message \"", Game1.content.LoadString("Strings\\Characters:MovieTheater_LoveMovie", responding_character.displayName), "\""));
				}
				break;
			case "like":
				sb.Append("/friendship " + responding_character.Name + " " + 100);
				if (!ignoreScript)
				{
					sb.Append(string.Concat("/playSound give_gift/emote ", responding_character.name, " ", 56.ToString(), "/message \"", Game1.content.LoadString("Strings\\Characters:MovieTheater_LikeMovie", responding_character.displayName), "\""));
				}
				break;
			case "dislike":
				sb.Append("/friendship " + responding_character.Name + " " + 0);
				if (!ignoreScript)
				{
					sb.Append(string.Concat("/playSound newArtifact/emote ", responding_character.name, " ", 24.ToString(), "/message \"", Game1.content.LoadString("Strings\\Characters:MovieTheater_DislikeMovie", responding_character.displayName), "\""));
				}
				break;
			}
			if (_concessionsData != null && _concessionsData.ContainsKey(responding_character))
			{
				MovieConcession concession = _concessionsData[responding_character];
				string concession_response = MovieTheater.GetConcessionTasteForCharacter(responding_character, concession);
				string gender_tag = "";
				Dictionary<string, string> NPCDispositions = Game1.content.Load<Dictionary<string, string>>("Data\\NPCDispositions");
				if (NPCDispositions.ContainsKey(responding_character.name))
				{
					string[] disposition = NPCDispositions[responding_character.name].Split('/');
					if (disposition[4] == "female")
					{
						gender_tag = "_Female";
					}
					else if (disposition[4] == "male")
					{
						gender_tag = "_Male";
					}
				}
				string sound = "eat";
				if (concession.tags != null && concession.tags.Contains("Drink"))
				{
					sound = "gulp";
				}
				switch (concession_response)
				{
				case "love":
					sb.Append("/friendship " + responding_character.Name + " " + 50);
					sb.Append("/tossConcession " + responding_character.Name + " " + concession.id + "/pause 1000");
					sb.Append("/playSound " + sound + "/shake " + responding_character.Name + " 500/pause 1000");
					sb.Append(string.Concat("/playSound reward/emote ", responding_character.name, " ", 20.ToString(), "/message \"", Game1.content.LoadString("Strings\\Characters:MovieTheater_LoveConcession" + gender_tag, responding_character.displayName, concession.DisplayName), "\""));
					break;
				case "like":
					sb.Append("/friendship " + responding_character.Name + " " + 25);
					sb.Append("/tossConcession " + responding_character.Name + " " + concession.id + "/pause 1000");
					sb.Append("/playSound " + sound + "/shake " + responding_character.Name + " 500/pause 1000");
					sb.Append(string.Concat("/playSound give_gift/emote ", responding_character.name, " ", 56.ToString(), "/message \"", Game1.content.LoadString("Strings\\Characters:MovieTheater_LikeConcession" + gender_tag, responding_character.displayName, concession.DisplayName), "\""));
					break;
				case "dislike":
					sb.Append("/friendship " + responding_character.Name + " " + 0);
					sb.Append("/playSound croak/pause 1000");
					sb.Append(string.Concat("/playSound newArtifact/emote ", responding_character.name, " ", 40.ToString(), "/message \"", Game1.content.LoadString("Strings\\Characters:MovieTheater_DislikeConcession" + gender_tag, responding_character.displayName, concession.DisplayName), "\""));
					break;
				}
			}
			_characterResponses[responding_character] = response;
		}

		public Dictionary<Character, string> GetCharacterResponses()
		{
			return _characterResponses;
		}

		private static string getEventName(Character c)
		{
			if (c is Farmer)
			{
				return "farmer" + Utility.getFarmerNumberFromFarmer(c as Farmer);
			}
			return c.name;
		}

		private Point getBackRowSeatTileFromIndex(int index)
		{
			return index switch
			{
				0 => new Point(2, 10), 
				1 => new Point(3, 10), 
				2 => new Point(4, 10), 
				3 => new Point(5, 10), 
				4 => new Point(8, 10), 
				5 => new Point(9, 10), 
				6 => new Point(10, 10), 
				7 => new Point(11, 10), 
				_ => new Point(4, 12), 
			};
		}

		private Point getMidRowSeatTileFromIndex(int index)
		{
			return index switch
			{
				0 => new Point(3, 8), 
				1 => new Point(4, 8), 
				2 => new Point(5, 8), 
				3 => new Point(8, 8), 
				4 => new Point(9, 8), 
				5 => new Point(10, 8), 
				_ => new Point(4, 12), 
			};
		}
	}
}
