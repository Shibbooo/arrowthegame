using System;

namespace Comsole
{
	public static class staticobjects
	{
		public enum types {GRID, PACMAN, GHOST, MINIBOSS, BOSS};
		public enum directions {NONE, UP, RIGHT, DOWN, LEFT};
		
		public const string mob = "x";
		public const string goodguy = "+";
		public const string badguy = "#";
		public const string empty = "_";
		
		public const ConsoleColor defaultGridColor = ConsoleColor.DarkGray;
		public const ConsoleColor defaultMobColor = ConsoleColor.Cyan;
		public const ConsoleColor defaultPlayerColor = ConsoleColor.Green;
		public const ConsoleColor defaultBadGuyColor = ConsoleColor.Yellow;
		public const ConsoleColor moreBadGuyColor = ConsoleColor.DarkYellow;
		public const ConsoleColor veryBadGuyColor = ConsoleColor.Red;
		public const ConsoleColor defaultGoodGuyColor = ConsoleColor.White;
		
	}
}
