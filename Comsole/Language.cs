using System;
using System.Collections.Generic;

namespace Comsole
{
	public class Language
	{
		private static Dictionary<string, string> DICT;

		public Language(string lang)
		{
			DICT = new Dictionary<string, string>();
			Load(lang);
		}
		
		private void Load(string lang)
		{
			// DEUTSCH
			if(lang == "de")
			{
				DICT["startgame"] = "Spiel starten";
				DICT["welcomemessage"] = "Willkommen bei ARROW! Bitte gib deinen Spitznamen ein!";
				DICT["welcomenewplayer"] = "Wunderbar {0}! Dann wollen wir mal loslegen.";
				DICT["welcomebackplayer"] = "Willkommen zurück {0}! Dann wollen wir mal loslegen.";
				DICT["keybar"] = "[1]Spiel starten  [2]Highscores ansehen  [3]Name ändern  [4]Hilfe  [Esc]Spiel beenden ";
				DICT["entername"] = "Bitte gib deinen neuen Spitznamen ein!";
				DICT["generatelevel"] = "Super! ich generiere jetzt den Level!";
				DICT["generategrid"] = "Erstelle Gitter...";
				DICT["generateenemies"] = "Erschaffe gegnerische Entitäten...";
				DICT["generatespawn"] = "Setze Spielerspawn...";
				DICT["playagain"] = "Nochmal spielen? (J/N)";
				DICT["highscorerank"] = "PLATZ";
				DICT["highscoreplayer"] = "SPIELER";
				DICT["highscorescore"] = "PUNKTE";
				DICT["backtomenu"] = "[Esc] Zurück zum Hauptmenü";
				DICT["firstround"] = "Runde 1! Fange alle Mobs!";
				DICT["nextround"] = "Runde {0} - Zeitbonus: {1} Sekunden";
				DICT["speedround"] = "Turborunde! +100% Speed - Zeitbonus: {0} Sekunden";
				DICT["bonuslevelend"] = "Runde {0} - Bonusleben gesammelt: {1} - Zeitbonus: {2} Sekunden";
				DICT["youdied"] = "Gestorben! Du hast {0} Punkte erreicht!";
				DICT["timeout"] = "Zeit abgelaufen! Du hast {0} Punkte erreicht!";
				DICT["youwin"] = "Gewonnen! Du hast {0} Punkte erreicht!";		
				DICT["startbonuslevel"] = "Bonuslevel! Sammle soviele Leben, wie du kannst!";
				DICT["mobkilled"] = " +{0} Zeitbonus";
				DICT["lifecollected"] = "{0} Leben eingesammelt, +{1} Zeitbonus";
				DICT["lifelost"] = "Nicht so hastig... -{0} Leben.";
				DICT["stats"] = "Runde {0} | Punktzahl {1} - Mobs verbleibend: {2}";
				DICT["timelife"] = "Zeit: {0} - Leben: {1}";
				DICT["paused"] = "-- PAUSIERT -- [P] Weiter";
				DICT["unpaused"] = "Mit Richtungstasten oder WASD bewegen - [P] Pause";
				DICT["helpheadline"] = "Hier erkläre ich dir kurz und knapp, womit du es bei deinen Abenteuern zu tun hast!";
				DICT["neutralobjects"] = "Da gibt es einmal das neutrale Objekt:";
				DICT["desc_grid"] = "Das \"Grid\" ist das Ziel, ähm oder so ähnlich. Halt dich gut dran fest, sonst landest du im Nichts!";
				DICT["goodobjects"] = "Es gibt gute Wesen...";
				DICT["desc_player"] = "Das bist du. In allen möglichen Formen. Toll, was?";
				DICT["desc_goodguy"] = "Das ist dein Helfer in der Not. Sammle es ein und erhalte Leben! Aber beeil dich, es bleibt nicht lange.";
				DICT["badobjects"] = "und dann sind da noch die fiesen Bösewichte.";
				DICT["desc_mob"] = "Ein einfacher Mob, der beim Zusammenstoß mit anderen Bösewichten die Richtung wechselt. Fange ihn ein und erhalte Zeitboni!";
				DICT["desc_badguy"] = "Ein böser Fiesling, der nur blöd rumsteht. Aber vorsicht, berührst du ihn, verlierst du 1 Leben.";
				DICT["desc_morebadguy"] = "Noch so ein Taugenichts, der zieht dir allerdings 2 Leben ab!";
				DICT["desc_verybadguy"] = "Und der hier ist ganz besonders fies, er raubt 3 Leben!";
			}
			else
			{
				DICT["startgame"] = "Start Game";
				DICT["welcomemessage"] = "Welcome to ARROW! Please enter your nickname!";
				DICT["welcomenewplayer"] = "Awesome {0}! Let's start.";
				DICT["welcomebackplayer"] = "Welcome back {0}! Let's start.";
				DICT["keybar"] = "[1]Start game  [2]View highscores  [3]Change nickname  [4]Help  [Esc]Leave game";
				DICT["entername"] = "Please enter your new nickname!";
				DICT["generatelevel"] = "Awesome! I'm generating the level now for you";
				DICT["generategrid"] = "Creating grid...";
				DICT["generateenemies"] = "Creating enemies...";
				DICT["generatespawn"] = "Set spawn...";
				DICT["playagain"] = "Play again? (J/N)";
				DICT["highscorerank"] = " RANK";
				DICT["highscoreplayer"] = "PLAYER ";
				DICT["highscorescore"] = "SCORE ";
				DICT["backtomenu"] = "[Esc] Back zo main menu";
				DICT["firstround"] = "Round 1! Collect all mobs!";
				DICT["nextround"] = "Round {0} - Timebonus: {1} seconds";
				DICT["speedround"] = "Speed-Round! +100% Speed - Timebonus: {0} seconds";
				DICT["bonuslevelend"] = "Round {0} - Collected bonuslives: {1} - Timebonus: {2} seconds";
				DICT["youdied"] = "You died! You've gained {0} points!";
				DICT["timeout"] = "Time's over! You've gained {0} points!";
				DICT["youwin"] = "You win! You've gained {0} points!";		
				DICT["startbonuslevel"] = "Bonus-Level! Collect as many lives as you can!";
				DICT["mobkilled"] = " +{0} Timebonus";
				DICT["lifecollected"] = "{0} lives collected, +{1} Timebonus";
				DICT["lifelost"] = "Calm down... -{0} lives.";
				DICT["stats"] = "Round {0} | Score {1} - Remaining mobs: {2}";
				DICT["timelife"] = "Time: {0} - Lives: {1}";
				DICT["paused"] = "-- PAUSED -- [P] Continue";
				DICT["unpaused"] = "Move with the arrow keys or WASD - [P] Pause";
				DICT["helpheadline"] = "I'm going to explain you the most important things to know on your adventures!";
				DICT["neutralobjects"] = "The neutral object:";
				DICT["desc_grid"] = "The \"Grid\", it's your road through all rounds. Stick to it!";
				DICT["goodobjects"] = "Then we have the good guys...";
				DICT["desc_player"] = "That's you. Amazing, hu?";
				DICT["desc_goodguy"] = "This is your lifekeeper, collect it to gain lifepoints!";
				DICT["badobjects"] = "and the bad guys.";
				DICT["desc_mob"] = "A normal mob, changes it's direction when colliding with other bad guys. Hunt him to gain bonus seconds!";
				DICT["desc_badguy"] = "This is a very unpleasant guy. He may not move around but if you touch him, he will take 1 lifepoint of yours.";
				DICT["desc_morebadguy"] = "He is even more bad, takes 2 lifepoints!";
				DICT["desc_verybadguy"] = "The worst of all takes 3 lifepoints! Avoid meeting him.";
			}

		}
		
		public string GetString(string str, params object[] args)
		{
			string full = "";
			try
			{
				full = DICT[str];
				if(string.IsNullOrEmpty(full))
					return "NullObj_" + str;
			}
			catch
			{
				return "NullObj_" + str;
			}
			
			return string.Format(full, args);
		}
	}
}
