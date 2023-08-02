using System;
using System.Collections.Generic;

namespace Comsole
{
	public class Game
	{
		public  int maxMaxMobCount = 64;
		
		private const int width = 44;
		private const int height = 22;
		private const int startLifes = 3;
		private const int mobStartCount = 2;
		private const int bossStartRound = 3;
		private const int maxRoundTime = 10;
		private const int maxMobKillTime = 1;
		private const int maxMaxBadGuySteps = 10;
		private const int maxMaxGoodGuySteps = 50;
		
		private const string mob = staticobjects.mob;
		private const string goodguy = staticobjects.goodguy;
		private const string badguy = staticobjects.badguy;
		private const string empty = staticobjects.empty;
		
		private const ConsoleColor defaultGridColor = staticobjects.defaultGridColor;
		private const ConsoleColor defaultMobColor = staticobjects.defaultMobColor;
		private const ConsoleColor defaultPlayerColor = staticobjects.defaultPlayerColor;
		private const ConsoleColor defaultBadGuyColor = staticobjects.defaultBadGuyColor;
		private const ConsoleColor moreBadGuyColor = staticobjects.moreBadGuyColor;
		private const ConsoleColor veryBadGuyColor = staticobjects.veryBadGuyColor;
		private const ConsoleColor defaultGoodGuyColor = staticobjects.defaultGoodGuyColor;
		
		private const ConsoleColor goodMessageColor = ConsoleColor.Green;
		private const ConsoleColor badMessageColor = ConsoleColor.Red;

		private Language language;
		private static readonly Random _RAND_ = new Random();

		private int playerID;
		private int mobKillTime;
		private int maxBadGuySteps;
		private int badGuySteps;
		private int maxGoodGuySteps;
		private int goodGuySteps;
		private int badGuys;
		private int goodGuys;
		
		private string playerSymbol;
		
		private Entity[] fields;
		private List<int> bosstable;
		
		public staticobjects.directions PlayerDirection;

		public long score;

		public int lifes;
		public int round;
		public int MobCount;
		public int maxMobCount;
		public int roundTime;
		public int currentTime;
		public int remainTime;
		public int bonusTime;
		public int lastBonusLevel;
		public int bonusLifesCollected;
		public int bonusLevelRemainTime;
		
		public bool bonusLevel;
		public bool pause;

		public string ExitCode;

		public Game(Language language)
		{
			round = 1;
			lifes = startLifes;
			remainTime = 0;
			this.language = language;
		}
		
		public void Create()
		{
			fields          = new Entity[width*height];
			bosstable		= new List<int>();
			
			pause = false;
			PlayerDirection = staticobjects.directions.NONE;
			playerSymbol    = Pacman.RIGHT;
			
			bonusTime = remainTime;
			roundTime = maxRoundTime + bonusTime;
			if(round > 10)
				roundTime += 5;
			currentTime = 0;
			
			MobCount = 0;
			
			maxMobCount = mobStartCount * round;
			
			if (maxMobCount > maxMaxMobCount)
				maxMobCount = maxMaxMobCount;
			
			maxBadGuySteps = maxMaxBadGuySteps;
			maxGoodGuySteps = maxMaxGoodGuySteps;
			
			mobKillTime = maxMobKillTime;
			
			if(round > 6)
			{
				mobKillTime = maxMobKillTime+1;
				maxBadGuySteps = maxBadGuySteps > 5 ? maxBadGuySteps-1 : 5;
			}
			
			if(score >= 200 && round - lastBonusLevel >= 10)
			{
				bonusLevel = true;
				lastBonusLevel = round;
				bonusLifesCollected = 0;
				maxGoodGuySteps = 10;
				roundTime = 10;
				bonusLevelRemainTime = remainTime;
			}
			else
			{
				bonusLevel = false;
			}
			
			//+++++++++++ Create GRID +++++++++++++++//
			int id = 0;
			for(int h = 0; h < height; h++) // Zeile für Zeile
			{
				for(int w = 0; w < width; w++) // Spalte für Spalte
				{
					var ent = new Entity(empty, id+1, w, h, staticobjects.types.GRID);
					fields[id] = ent;
					id++;
				}
			}
			//+++++++++++++++++++++++++++++++++++++++//
			
			
			if(!bonusLevel)
				CreateMobs();
				
			CreatePlayer();
			PrintGrid();
			
			if(bonusLevel)
				PrintInfoBar(GetString("startbonuslevel"), true);
		}
		
		private void CreatePlayer()
		{
			int id = width*(height/2)-(width/2);
			fields[id].name = playerSymbol;
			fields[id].setType(staticobjects.types.PACMAN);
			SetPlayerID(id);
		}
		
		private void CreateMobs()
		{
			for(int i = 0; i < maxMobCount; i++)
			{
				int a = RandomNumber(0, height*width-1);
				
				while(fields[a].getType() != staticobjects.types.GRID)
				{
					a = RandomNumber(0, height*width-1);
				}
				
				Entity _mob = fields[a];
				staticobjects.directions dir = staticobjects.directions.LEFT;
				int b = RandomNumber(0, 100);
				if(b < 25)
					dir = staticobjects.directions.RIGHT;
				else if(b >= 25 && b < 50)
					dir = staticobjects.directions.UP;		
				else if(b >= 50 && b < 75)
					dir = staticobjects.directions.DOWN;

				_mob.name = mob;
				_mob.setType(staticobjects.types.GHOST);				
				_mob.setDirection(dir);
				
				fields[a] = _mob;
				MobCount++;
			}
		}
		
		private void CreateBadGuy()
		{
			if(badGuySteps >= maxBadGuySteps)
			{
				badGuySteps = 0;
				int a = RandomNumber(0, height*width-1);
					
				while(fields[a].getType() != staticobjects.types.GRID)
				{
					a = RandomNumber(0, height*width-1);
				}
				
				Entity _badguy = fields[a];
				staticobjects.directions dir = staticobjects.directions.DOWN;
				int b = RandomNumber(0, 100);
				if(b < 25)
					dir = staticobjects.directions.RIGHT;
				else if(b >= 25 && b < 50)
					dir = staticobjects.directions.UP;		
				else if(b >= 50 && b < 75)
					dir = staticobjects.directions.LEFT;
				_badguy.name = badguy;
				_badguy.setType(staticobjects.types.MINIBOSS);				
				_badguy.setDirection(dir);
				_badguy.lifesOnDestroy = 1;
				
				ConsoleColor col = defaultBadGuyColor;
				if(badGuys > 0 && badGuys % 5 == 0)
				{
					_badguy.lifesOnDestroy = 2;
					col = moreBadGuyColor;
				}
				if(badGuys > 0 && badGuys % 10 == 0)
				{
					_badguy.lifesOnDestroy = 3;
					col = veryBadGuyColor;
				}
				
				fields[a] = _badguy;
				badGuys++;
				ChangeCharAtPositionSingle(fields[a].getPosition(), fields[a].name, col);
			}
			badGuySteps++;
		}
		
		private void CreateGoodGuy()
		{
			if(goodGuySteps >= maxGoodGuySteps)
			{
				goodGuySteps = 0;
				int a = RandomNumber(0, height*width-1);
					
				while(fields[a].getType() != staticobjects.types.GRID)
				{
					a = RandomNumber(0, height*width-1);
				}
				
				Entity _goodguy = fields[a];
				staticobjects.directions dir = staticobjects.directions.UP;
				int b = RandomNumber(0, 100);
				if(b < 25)
					dir = staticobjects.directions.RIGHT;
				else if(b >= 25 && b < 50)
					dir = staticobjects.directions.LEFT;		
				else if(b >= 50 && b < 75)
					dir = staticobjects.directions.DOWN;
				_goodguy.name = goodguy;
				_goodguy.setType(staticobjects.types.BOSS);				
				_goodguy.setDirection(dir);
				_goodguy.lifesOnDestroy = 1;
				_goodguy.createTime = remainTime;
				_goodguy.lifeTime = 5; // 5 seconds
				
				if(goodGuys > 0 && goodGuys % 5 == 0)
					_goodguy.lifesOnDestroy = 2;
				
				fields[a] = _goodguy;
				bosstable.Add(a);
				goodGuys++;
				ChangeCharAtPositionSingle(fields[a].getPosition(), fields[a].name, defaultGoodGuyColor);
			}
			goodGuySteps++;
		}

		private void SetPlayerID(int id)
		{
			playerID = id;
		}
		
		private void PrintGrid()
		{
			Console.Clear();
			
			int id = 0;
			string line = "";
			for(int h = 0; h < width*height; h++)
			{
				line += fields[id].name;
				id++;
				if(id % width == 0)
				{
					Console.WriteLine(line);
					line = "";
				}
			}
			Console.WriteLine(" ");
			Console.WriteLine(" ");
			Console.WriteLine(" ");
			Console.WriteLine(" ");
			Console.WriteLine(" ");
			PrintStatusBar();
		}
		
		
		public void RunGame()
		{
			if(!pause)
			{
				ExitCode = null;
				MovePlayer();
				MoveMobs();
				CheckBossLifeTime();
				GetRemainTime();
				PrintStatusBar();
				
				if(!bonusLevel && round >= bossStartRound)
				{
					if(HeadOrNumber() == 1)
						CreateBadGuy();
					else
						CreateGoodGuy();
				}
				else if (bonusLevel)
				{
					CreateGoodGuy();
				}
				
				if(lifes <= 0)
				{
					ExitCode = "LOSE";
				}
				
				if(!bonusLevel && remainTime <= 0)
				{
					ExitCode = "TIMEOUT";
				}
			}
			else
				PrintStatusBar();
		}
		
		public void Pause()
		{
			pause = true;
			Disable();
		}
		
		public void UnPause()
		{
			pause = false;
			Enable();
		}
		
		public void Enable()
		{
			foreach (Entity field in fields)
			{
				ConsoleColor color = defaultGridColor;
				if(field.getType() == staticobjects.types.PACMAN)
					color = defaultPlayerColor;
				else if(field.getType() == staticobjects.types.GHOST)
					color = defaultMobColor;
				else if(field.getType() == staticobjects.types.BOSS)
					color = defaultGoodGuyColor;
				else if(field.getType() == staticobjects.types.MINIBOSS)
					color = defaultBadGuyColor;
				
				ChangeCharAtPositionSingle(field.getPosition(), field.name, color);
			}
			
		}
		
		public void Disable()
		{
			foreach (Entity field in fields)
			{
				if(field.getType() != staticobjects.types.PACMAN)
					ChangeCharAtPositionSingle(field.getPosition(), field.name, defaultGridColor);
			}
		}
		
		private void MovePlayer()
		{
			staticobjects.directions dir = PlayerDirection;
			int oldID = playerID;
			switch(dir)
			{
				case staticobjects.directions.UP:
					playerID = MoveEntityUp(playerID, true);
					break;
				case staticobjects.directions.RIGHT:
					playerID = MoveEntityRight(playerID, true);
					break;
				case staticobjects.directions.DOWN:
					playerID = MoveEntityDown(playerID, true);
					break;
				case staticobjects.directions.LEFT:
					playerID = MoveEntityLeft(playerID, true);
					break;
				default:
					break;
			}
						
			ChangeCharAtPosition(fields[oldID].getPosition(), fields[playerID].getPosition(), fields[oldID].name, fields[playerID].name, defaultPlayerColor);
		}
		
		private void MoveMobs()
		{
			int[] mobs = new int[MobCount];
			int counter = 0;
			for (int i = 0; i < width*height; i++)
			{
				if(fields[i].getType() == staticobjects.types.GHOST)
				{
					mobs[counter] = i;
					counter++;
				}
			}
			
			for(int m = 0; m < counter; m++)
			{
				MoveMob(mobs[m]);
			}
		}
		
		private void MoveMob(int id)
		{
			staticobjects.directions dir = fields[id].getDirection();
			int oldID = id;
			int newId = 0;
			switch(dir)
			{
				case staticobjects.directions.UP:
					newId = MoveEntityUp(oldID);
					break;
				case staticobjects.directions.RIGHT:
					newId = MoveEntityRight(oldID);
					break;
				case staticobjects.directions.DOWN:
					newId = MoveEntityDown(oldID);
					break;
				case staticobjects.directions.LEFT:
					newId = MoveEntityLeft(oldID);
					break;
				default:
					break;
			}
			ChangeCharAtPosition(fields[oldID].getPosition(), fields[newId].getPosition(), fields[oldID].name, fields[newId].name, defaultMobColor);
		}
		

		private bool CheckCollision(int current, int next)
		{
			string currentName = fields[current].name;
			staticobjects.directions currentDirection = fields[current].getDirection();
			staticobjects.types CurrentType = fields[current].getType();
			staticobjects.types NewType = fields[next].getType();
			
			if(CurrentType == staticobjects.types.PACMAN && NewType == staticobjects.types.GHOST)
			{
				CollidePlayerWithMob(current, next);
			}
			else if(CurrentType == staticobjects.types.PACMAN && NewType == staticobjects.types.PACMAN)
			{
				//lastError = "PLAYER AND PLAYER?!?!?!";
			}
			else if(CurrentType == staticobjects.types.GHOST && NewType == staticobjects.types.PACMAN)
			{
				CollideMobWithPlayer(current, next);
			}
			else if(CurrentType == staticobjects.types.GHOST && NewType == staticobjects.types.GHOST)
			{
				CollideMobWithMob(current, next);
			}
			else if(CurrentType == staticobjects.types.PACMAN && NewType == staticobjects.types.BOSS)
			{
				CollidePlayerWithBoss(current, next);
			}
			else if(CurrentType == staticobjects.types.PACMAN && NewType == staticobjects.types.MINIBOSS)
			{
				CollidePlayerWithMiniBoss(current, next);
			}
			else if(CurrentType == staticobjects.types.GHOST && NewType == staticobjects.types.MINIBOSS)
			{
				CollideMobWithBoss(current, next);
			}
			else
			{
				fields[next].name = currentName;
				fields[next].setDirection(currentDirection);
				fields[next].setType(CurrentType);
				
				RemoveEntity(current);
				return false;
			}
			return true;
			
		}
		
		private void CollidePlayerWithMob(int current, int next)
		{
			score++;
			
			if(MobCount > 0)
				MobCount--;
			else
				MobCount = 0;
			
			RemoveEntity(current);	
			MakeFieldPlayer(next);

			roundTime += mobKillTime;	
			PrintInfoBar(GetString("mobkilled", mobKillTime), true);
		}
		
		private void CollideMobWithPlayer(int current, int next)
		{
			RevertMobDirection(next);
		}
		
		private void CollideMobWithMob(int current, int next)
		{
			RevertMobDirection(current);
			RevertMobDirection(next);
		}
		
		private void CollideMobWithBoss(int current, int next)
		{
			RevertMobDirection(current);
		}
		
		private void CollidePlayerWithBoss(int current, int next)
		{	
			int extra = fields[next].lifesOnDestroy;
			int extratime = mobKillTime*(2*extra);
			score = score+(10*extra);

			RemoveEntity(current);	
			MakeFieldPlayer(next);	
			
			if(!bonusLevel)
				roundTime += extratime;
			else
				bonusLifesCollected += extra;
			
			lifes += extra;
			
			PrintInfoBar(GetString("lifecollected", extra, extratime), true);
		}
		
		private void CollidePlayerWithMiniBoss(int current, int next)
		{
			int extra = fields[next].lifesOnDestroy;
			
			fields[next].name = "~";
			fields[next].setType(staticobjects.types.PACMAN);	
			
			RemoveEntity(current);
			
			lifes -= extra;
			PrintInfoBar(GetString("lifelost", extra));
			ExitCode = "DIE";
		}
		
		private void RevertMobDirection(int id)
		{
			staticobjects.directions dir = fields[id].getDirection();
			staticobjects.directions newDir = staticobjects.directions.LEFT;
			switch (dir)
			{
				case staticobjects.directions.LEFT:
					newDir = staticobjects.directions.RIGHT;
					break;
				case staticobjects.directions.RIGHT:
					newDir = staticobjects.directions.LEFT;
					break;
				case staticobjects.directions.UP:
					newDir = staticobjects.directions.DOWN;
					break;
				case staticobjects.directions.DOWN:
					newDir = staticobjects.directions.UP;
					break;
				default:
					newDir = staticobjects.directions.LEFT;
					break;
			}
			fields[id].setDirection(newDir);
		}
				
		private int MoveEntityUp(int current, bool player = false)
		{
			string currentName = fields[current].name;
			int nextField = GetNewId(current, "up");

			if(player)
			{
				playerSymbol = Pacman.UP;
				fields[current].name = playerSymbol;
				playerID = nextField;
			}
				
			CheckCollision(current, nextField);

			return nextField;
		}
		
		private int MoveEntityRight(int current, bool player = false)
		{
			string currentName = fields[current].name;
			int nextField = GetNewId(current, "right");

			if(player)
			{
				playerSymbol = Pacman.RIGHT;
				fields[current].name = playerSymbol;
				playerID = nextField;
			}
				
			CheckCollision(current, nextField);

			return nextField;
		}
				
		private int MoveEntityDown(int current, bool player = false)
		{
			string currentName = fields[current].name;
			int nextField = GetNewId(current, "down");

			if(player)
			{
				playerSymbol = Pacman.DOWN;
				fields[current].name = playerSymbol;
				playerID = nextField;
			}
				
			CheckCollision(current, nextField);

			return nextField;
		}
						
		private int MoveEntityLeft(int current, bool player = false)
		{
			string currentName = fields[current].name;
			int nextField = GetNewId(current, "left");

			if(player)
			{
				playerSymbol = Pacman.LEFT;
				fields[current].name = playerSymbol;
				playerID = nextField;
			}
				
			CheckCollision(current, nextField);

			return nextField;
		}
		
		private void CheckBossLifeTime()
		{
			foreach (int boss in bosstable)
			{
				Entity b = fields[boss];
				if (b.getType() == staticobjects.types.BOSS)
				{
					if(b.createTime - remainTime >= b.lifeTime)
					{
						RemoveEntity(boss, true);
					}
				}
			}
		}
		
		private void RemoveEntity(int id, bool change = false)
		{
			fields[id].setType(staticobjects.types.GRID);
			fields[id].name = empty;
			fields[id].setDirection(staticobjects.directions.NONE);
			fields[id].lifeTime = 0;
			fields[id].createTime = 0;
			fields[id].lifesOnDestroy = 0;
			if(change)
				ChangeCharAtPositionSingle(fields[id].getPosition(), fields[id].name, defaultGridColor);
		}
		
		private void MakeFieldPlayer(int id)
		{
			fields[id].lifeTime = 0;
			fields[id].createTime = 0;
			fields[id].lifesOnDestroy = 0;
			fields[id].name = playerSymbol;
			fields[id].setType(staticobjects.types.PACMAN);
		}
		
		private int GetNewId(int id, string dir)
		{
			int newID = 0;
			int row = fields[id].getRow();
			int col = fields[id].getCol();
			
			if(dir == "up")
			{
				newID = id-width;
				if(newID < 0)
					newID = (height-1)*width+col;
			}
			else if(dir == "right")
			{
				newID = id+1;	
				if(newID >= row*width+width)
					newID = row*width;
			}
			else if(dir == "down")
			{
				newID = id+width;
				if(newID >= height*width)
					newID = col;
			}
			else if(dir == "left")
			{
				newID = id-1;
				if(newID < row*width)
					newID = row*width+width-1;
			}
			return newID;
		}
		
		
		private void ChangeCharAtPositionSingle(Position newPos, string newChar, ConsoleColor color = defaultGridColor)
		{
			int oldX = Console.CursorLeft;
        	int oldY = Console.CursorTop;
			Console.SetCursorPosition(newPos.x, newPos.y);
			
			if(color != defaultGridColor)
				Console.ForegroundColor = color;
			Console.Write(newChar);
			if(color != defaultGridColor)
				Console.ForegroundColor = defaultGridColor;
			
			Console.SetCursorPosition(oldX, oldY);
		}
		
		private void ChangeCharAtPosition(Position oldPos, Position newPos,  string oldChar, string newChar, ConsoleColor color = defaultGridColor)
		{
			int oldX = Console.CursorLeft;
        	int oldY = Console.CursorTop;
        	Console.SetCursorPosition(oldPos.x, oldPos.y);

        	ResetConsoleColor();
			Console.Write(oldChar);
			
			Console.SetCursorPosition(newPos.x, newPos.y);
			if(color != defaultGridColor)
				Console.ForegroundColor = color;
			Console.Write(newChar);
			if(color != defaultGridColor)
				ResetConsoleColor();
			
			Console.SetCursorPosition(oldX, oldY);
		}
		
	
		private void PrintStatusBar()
		{
			int top = Console.CursorTop;
			Console.SetCursorPosition(0, top-4);
			Console.WriteLine(new string(' ', Console.WindowWidth-1));
			Console.WriteLine(new string(' ', Console.WindowWidth-1));
			Console.WriteLine(new string(' ', Console.WindowWidth-1));
			Console.SetCursorPosition(0, top-4);
			PrintLifesBar();
			PrintStats();
			PrintHelpBar();
			Console.SetCursorPosition(0, top);
		}
		
		private void PrintStats()
		{
			Console.BackgroundColor = ConsoleColor.Gray;
			Console.ForegroundColor = ConsoleColor.Black;
			string stats = GetString("stats", round, score, MobCount);
			Console.WriteLine(stats);
			ResetConsoleColor();
		}
		
		private void PrintLifesBar()
		{
			Console.BackgroundColor = ConsoleColor.Gray;
			Console.ForegroundColor = ConsoleColor.Black;
			string liveStar = "";
			string time = GetTimeString();
			
			for (int i = 0; i < lifes; i++)
			{
				liveStar += "+";
			}
			string live = GetString("timelife", time, liveStar);
			Console.WriteLine(live);
			ResetConsoleColor();
		}
		
		public void PrintInfoBar(string info = null, bool good = false)
		{
			Console.ForegroundColor = good ? goodMessageColor : badMessageColor;
			if (info != null)
			{
				int top = Console.CursorTop;
				Console.SetCursorPosition(0, top-1);
				Console.WriteLine(new string(' ', Console.WindowWidth-1));
				Console.SetCursorPosition(0, top-1);
				
				Console.WriteLine(info);
				Console.SetCursorPosition(0, top);
			}
			else
			{
				Console.WriteLine(new string(' ', Console.WindowWidth-1));
			}
			ResetConsoleColor();
		}
		
		private void PrintHelpBar()
		{			
			//Console.BackgroundColor = ConsoleColor.Gray;
			Console.ForegroundColor = ConsoleColor.White;
			
			if(pause)
			{
				Console.WriteLine(GetString("paused"));
			}
			else
			{
				Console.WriteLine(GetString("unpaused"));
			}
			ResetConsoleColor();
		}
		
		private void ResetConsoleColor()
		{
			Console.ForegroundColor = defaultGridColor;
			Console.BackgroundColor = ConsoleColor.Black;
		}
		
		private int RandomNumber(int min, int max)  
		{  
		    return _RAND_.Next(min, max);  
		} 
		
		private void GetRemainTime()
		{
			remainTime = roundTime - currentTime;
		}
		
		private int HeadOrNumber()
		{
			int randomNum = RandomNumber(0, 100);
			if(randomNum > 50)
				return 1;
			return 0;
		}
		
		private string GetTimeString()
		{
			string str = "";
			int part1 = 0;
			int part2 = 0;
			
			GetRemainTime();
			
			if (remainTime > 60)
			{
				part1 = remainTime / 60;
				part2 = remainTime % 60;
			}
			else
			{
				part1 = 0;
				part2 = remainTime;
			}
			
			string p1 = part1.ToString();
			string p2 = part2.ToString();
			
			if(part1 < 10)
			{
				p1 = "0" + p1;
			}
			if(part2 < 10)
			{
				p2 = "0" + p2;
			}	
			str = p1 + ":" + p2;
			return str;
		}
		
		public void GameOver()
		{
			Disable();
			const string gameover = "GAME OVER!";
			int len = gameover.Length;
			int top = Console.CursorTop;
			
			string spacestring = "";
			for(int i = 0; i < (width-len)/2; i++)
			{
				spacestring += "_";
			}
			
			Console.SetCursorPosition(0, height/2);
			Console.ForegroundColor = defaultGridColor;
			Console.Write(spacestring);
			
			Console.ForegroundColor = ConsoleColor.Red;
			Console.Write(gameover);
			
			Console.ForegroundColor = defaultGridColor;
			Console.Write(spacestring);
			
			Console.SetCursorPosition(0, top);
		}
		
		private string GetString(string str, params Object[] args)
		{		
			return language.GetString(str, args);
		}
	}
}
