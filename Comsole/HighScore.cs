using System;

namespace Comsole
{
	public class HighScore
	{
		public string playername;
		public long score;
		
		public HighScore(long score, string playername)
		{
			this.score = score;
			this.playername = playername;
		}
	}
}
