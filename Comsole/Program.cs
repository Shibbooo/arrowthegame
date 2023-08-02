using System;
using System.Threading;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;

namespace Comsole
{
	class Program
	{
		const string _GAME_NAME_ = "ARROW";
		const string _VERSION_ = "1.1";
		
		const int normalSpeed = 10;
		
		static string nickName;
		
		static List<HighScore> HighScores;
		
		static IniFile ini;		
		
		static Language language;
		
		public static void Main(string[] args)
		{
			Console.OutputEncoding = System.Text.Encoding.UTF8;
			Console.CursorVisible = false;
			CultureInfo ci = CultureInfo.InstalledUICulture;
			string lang = ci.TwoLetterISOLanguageName;
				
			language = new Language(lang);
			SetWindowTitle();
			nickName = null;
			
			ini = new IniFile("arrow.ini");
			if(!File.Exists("arrow.ini"))
			{
				ini.Write("PlayerName", "", "BOOT");
			}
			else
			{
				nickName = ini.Read("PlayerName", "BOOT");
				if(string.IsNullOrEmpty(nickName))
					nickName = null;
			}

			while(true)
			{
				HighScores = new List<HighScore>();
				GetHighScores();
				Console.Clear();
				Console.ForegroundColor = ConsoleColor.Green;
				Console.WriteLine("_________________________________       __");
				Console.WriteLine("___    |__  __ \\__  __ \\_  __ \\_ |     / /");
				Console.WriteLine("__  /| |_  /_/ /_  /_/ /  / / /_ | /| / / ");
				Console.WriteLine("_  ___ |  _, _/_  _, _// /_/ /__ |/ |/ /  ");
				Console.WriteLine("/_/  |_/_/ |_| /_/ |_| \\____/ ____/|__/ ");
				
				Console.WriteLine("");
				if(nickName == null)
				{
					Console.WriteLine(GetString("welcomemessage"));
					nickName = Console.ReadLine();
					ini.Write("PlayerName", nickName, "BOOT");
					Console.WriteLine(GetString("welcomenewplayer", nickName));
				}
				else
				{
					Console.WriteLine(GetString("welcomebackplayer", nickName));
				}
				
				Console.WriteLine("");
				Console.WriteLine(GetString("keybar"));
				
				ConsoleKeyInfo reply = Console.ReadKey();
				while(reply.Key != ConsoleKey.D1 
				      && reply.Key != ConsoleKey.D2 
				      && reply.Key != ConsoleKey.D3
				      && reply.Key != ConsoleKey.D4
				      && reply.Key != ConsoleKey.Escape)
				{
					reply = Console.ReadKey();
				}
				
				if(reply.Key == ConsoleKey.D1)
				{
					StartGame();
				}
				else if(reply.Key == ConsoleKey.D2)
				{
					ShowHighScores();
				}
				else if(reply.Key == ConsoleKey.D3)
				{
					ChangeNickName();
				}
				else if(reply.Key == ConsoleKey.D4)
				{
					OpenHelp();
				}
				else if(reply.Key == ConsoleKey.Escape)
				{
					break;
				}

			}
		}
		
		private static void SetWindowTitle()
		{
			Console.Title = _GAME_NAME_ + " Version " + _VERSION_;
		}
		
		private static void ChangeNickName()
		{
			Console.Clear();
			Console.WriteLine(GetString("entername"));
			nickName = Console.ReadLine();
			ini.Write("PlayerName", nickName, "BOOT");
		}
		
		private static void OpenHelp()
		{
			Console.Clear();
			Console.WriteLine(GetString("helpheadline"));
			Console.WriteLine("");
			
			Console.ForegroundColor = ConsoleColor.Cyan;
			Console.WriteLine(GetString("neutralobjects"));
			
			Console.ForegroundColor = staticobjects.defaultGridColor;
			Console.Write(staticobjects.empty);
			Console.ForegroundColor = ConsoleColor.White;
			Console.Write(" - " + GetString("desc_grid"));
			Console.WriteLine("");
			
			Console.WriteLine("");
			Console.ForegroundColor = ConsoleColor.Cyan;
			Console.WriteLine(GetString("goodobjects"));
			
			Console.ForegroundColor = staticobjects.defaultPlayerColor;
			Console.Write(Pacman.UP + ", " + Pacman.RIGHT + ", " + Pacman.DOWN + ", " + Pacman.LEFT);
			Console.ForegroundColor = ConsoleColor.White;
			Console.Write(" - " + GetString("desc_player"));
			Console.WriteLine("");
			
			Console.ForegroundColor = staticobjects.defaultGoodGuyColor;
			Console.Write(staticobjects.goodguy);
			Console.ForegroundColor = ConsoleColor.White;
			Console.Write(" - " + GetString("desc_goodguy"));
			Console.WriteLine("");
			
			Console.WriteLine("");
			Console.ForegroundColor = ConsoleColor.Cyan;
			Console.WriteLine(GetString("badobjects"));
			
			Console.ForegroundColor = staticobjects.defaultMobColor;
			Console.Write(staticobjects.mob);
			Console.ForegroundColor = ConsoleColor.White;
			Console.Write(" - " + GetString("desc_mob"));
			Console.WriteLine("");
			
			Console.ForegroundColor = staticobjects.defaultBadGuyColor;
			Console.Write(staticobjects.badguy);
			Console.ForegroundColor = ConsoleColor.White;
			Console.Write(" - " + GetString("desc_badguy"));
			Console.WriteLine("");
			
			Console.ForegroundColor = staticobjects.moreBadGuyColor;
			Console.Write(staticobjects.badguy);
			Console.ForegroundColor = ConsoleColor.White;
			Console.Write(" - " + GetString("desc_morebadguy"));
			Console.WriteLine("");
			
			Console.ForegroundColor = staticobjects.veryBadGuyColor;
			Console.Write(staticobjects.badguy);
			Console.ForegroundColor = ConsoleColor.White;
			Console.Write(" - " + GetString("desc_verybadguy"));
			Console.WriteLine("");
			

			Console.WriteLine("");
			Console.WriteLine("");
			WaitForBackToMenuCommand();
		}
		
		private static void StartGame()
		{
			Console.WriteLine(GetString("generatelevel"));
			Thread.Sleep(600);
			Console.WriteLine(GetString("generategrid"));
			Thread.Sleep(1000);
			Console.WriteLine(GetString("generateenemies"));
			Thread.Sleep(600);
			Console.WriteLine(GetString("generatespawn"));
			Thread.Sleep(300);
			
			// SPIEL STARTEN
			Console.ForegroundColor = ConsoleColor.DarkGray;
			while(true)
			{
				Play();
				Console.WriteLine(GetString("playagain"));
				ConsoleKeyInfo reply = Console.ReadKey();
				while(reply.Key != ConsoleKey.J && reply.Key != ConsoleKey.N)
				{
					reply = Console.ReadKey();
				}
				if(reply.Key == ConsoleKey.N)
				{
					break;
				}
				else
				{
					continue;
				}
			}
		}
		
		private static void GetHighScores()
		{
			HighScores.Clear();
			for(int i = 1; i <= 30; i++)
			{
				string score = ini.Read("HighScore_" + i, "SCORES");
				string scoreHolder = ini.Read("HighScoreHolder_" + i, "SCORES");
				
				if(string.IsNullOrEmpty(score) || string.IsNullOrEmpty(scoreHolder))
					break;
				
				var hs = new HighScore(long.Parse(score), scoreHolder);
				HighScores.Add(hs);
			}
			OrderHighScores();
		}
		
		private static void SetHighScores()
		{
			OrderHighScores();
			int counter = 0;
			foreach(HighScore hs in HighScores)
			{
				ini.Write("HighScore_" + (counter+1), hs.score.ToString() ,"SCORES");
				ini.Write("HighScoreHolder_" + (counter+1), hs.playername ,"SCORES");
				counter++;
				if(counter == 30)
					break;
			}
		}
		
		private static void AddHighScore(long score, string playername)
		{
			HighScores.Add(new HighScore(score, playername));
			SetHighScores();
		}
		
		private static void OrderHighScores()
		{
			HighScores = HighScores.OrderByDescending(hs=>hs.score).ToList();
		}

		
		private static void ShowHighScores()
		{
			GetHighScores();
			
			Console.Clear();
			Console.WriteLine("#########################################################");
			Console.WriteLine("# " + GetString("highscorerank") + " #            " + GetString("highscoreplayer") + "           #      " + GetString("highscorescore") + "    #");
			Console.WriteLine("#########################################################");
			int counter = 0;
			foreach (HighScore hs in HighScores)
			{
				int i = counter+1;
				string score = hs.score.ToString();
				string scoreHolder = hs.playername;
				
				if(string.IsNullOrEmpty(score) || string.IsNullOrEmpty(scoreHolder))
					break;
				string rank = i.ToString();
				if(i < 10)
					rank = "0" + i;
				
				string name = "";
				int namelength = scoreHolder.Length;
				int spaces = 30-namelength;
				if(namelength % 2 != 0)
					spaces = 30-(namelength+1);
				string spacestring = "";
				for(int a=0; a<(spaces/2); a++)
				{
					spacestring += " ";
				}
				if(namelength % 2 == 0)
					name = spacestring + scoreHolder + spacestring;
				else
					name = spacestring + " " + scoreHolder + spacestring;
				
				
				string highscore = "";
				int scorelength = score.Length;
				spaces = 15-scorelength;
				if(scorelength % 2 != 0)
					spaces = 15-(scorelength+1);
				spacestring = "";
				for(int a=0; a<(spaces/2); a++)
				{
					spacestring += " ";
				}
				if(scorelength % 2 == 0)
					highscore = spacestring + score + spacestring;
				else
					highscore = spacestring + " " + score + spacestring;
			
				Console.WriteLine("#   " + rank + "  #" + name + "# "+ highscore + " #");
				Console.WriteLine("#-------#------------------------------#----------------#");
				counter++;
				if (counter == 30)
					break;
			}
			Console.WriteLine("#########################################################");
			WaitForBackToMenuCommand();
		}
		
		private static void Play()
		{
			int speed = normalSpeed;
			int step = 0;
			
			Game game = new Game(language);
			game.Create();		
			game.PrintInfoBar(GetString("firstround"), true);
			Thread.Sleep(1000);
			game.UnPause();			
			
			staticobjects.directions dir = staticobjects.directions.NONE;
			ConsoleKeyInfo input = Console.ReadKey();
			dir = GetDirection(input);

			while(true)
			{			
				if(Console.KeyAvailable || game.ExitCode == "DIE")
				{
					input = Console.ReadKey();
					
					if(!game.pause)
						dir = GetDirection(input);
				}
				
				if(dir != staticobjects.directions.NONE)
				{
					game.PlayerDirection = dir;
				}
				
				game.RunGame();
				
				if(!game.bonusLevel && game.MobCount == 0)
				{
					game.round++;
					game.Create();
					
					if(!game.bonusLevel)
						game.PrintInfoBar(GetString("nextround", game.round, game.bonusTime), true);
					
					if(game.round % 10 == 0 || (game.maxMobCount >= game.maxMaxMobCount && game.round % 4 == 0))
					{
						speed = normalSpeed*2;
						game.PrintInfoBar(GetString("speedround", game.bonusTime), true);
					}
					else if (game.bonusLevel)
					{
						speed = normalSpeed*4;
					}
					else
					{
						speed = normalSpeed;
					}
					
					Thread.Sleep(1000);
					game.UnPause();
					
					input = Console.ReadKey();
				}
				else if(game.bonusLevel && game.remainTime <= 0)
				{
					speed = normalSpeed;
					game.round++;
					game.remainTime = game.bonusLevelRemainTime;
					game.Create();
					game.PrintInfoBar(GetString("bonuslevelend", game.round, game.bonusLifesCollected, game.bonusTime), true);	

					Thread.Sleep(1000);
					game.UnPause();
					
					input = Console.ReadKey();
				}
				
				if(input.Key == ConsoleKey.P && !game.pause)
				{
					game.Pause();
					input = new ConsoleKeyInfo();
				}
				else if(input.Key == ConsoleKey.P && game.pause)
				{
					game.UnPause();
					input = new ConsoleKeyInfo();
				}
				
				if(input.Key == ConsoleKey.Escape || game.ExitCode == "LOSE" || game.ExitCode == "TIMEOUT")
					break;
				
				Thread.Sleep(1000/speed);
				
				if(!game.pause)
				{
					step++;
					if(step == speed)
					{
						step = 0;
						game.currentTime++;
					}
				}
			}
			
			if(game.ExitCode == "LOSE")
			{
				game.GameOver();
				game.PrintInfoBar(GetString("youdied", game.score));
				AddHighScore(game.score, nickName);
			}
			else if(game.ExitCode == "TIMEOUT")
			{
				game.GameOver();
				game.PrintInfoBar(GetString("timeout", game.score));
				AddHighScore(game.score, nickName);
			}
			else if(game.MobCount == 0)
			{
				game.PrintInfoBar(GetString("youwin", game.score), true);
				AddHighScore(game.score, nickName);
			}
		}
		
		private static staticobjects.directions GetDirection(ConsoleKeyInfo input)
		{
			staticobjects.directions dir = staticobjects.directions.NONE;
			switch (input.Key)
			{
				case ConsoleKey.W:
				case ConsoleKey.UpArrow:
					dir = staticobjects.directions.UP;
					break;
				case ConsoleKey.A:
				case ConsoleKey.LeftArrow:
					dir = staticobjects.directions.LEFT;
					break;
				case ConsoleKey.S:
				case ConsoleKey.DownArrow:
					dir = staticobjects.directions.DOWN;
					break;
				case ConsoleKey.D:
				case ConsoleKey.RightArrow:
					dir = staticobjects.directions.RIGHT;
					break;
				default:
					break;
			}
			return dir;
		}
		
		private static void WaitForBackToMenuCommand()
		{
			Console.WriteLine("");
			Console.WriteLine(GetString("backtomenu"));
			Console.SetCursorPosition(0, 0);
			ConsoleKeyInfo reply = Console.ReadKey();
			while(reply.Key != ConsoleKey.Escape)
			{
				reply = Console.ReadKey();
			}
		}
		
		private static string GetString(string str, params Object[] args)
		{
			return language.GetString(str, args);
		}
	}
}