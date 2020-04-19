using Hearthstone_Deck_Tracker;
using Hearthstone_Deck_Tracker.Hearthstone;
using Hearthstone_Deck_Tracker.Utility.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattlegroundsMatchData
{
    class GameLogManager
    {
        public async static void SaveLog()
        {
            GameV2 _game = Core.Game;

            await LogIsComplete();

            var powerLog = _game.PowerLog;

            var createGameCount = 0;
            powerLog = powerLog.TakeWhile(x => !(x.Contains("CREATE_GAME") && createGameCount++ == 1)).ToList();

            string LogLocation = "BgPowerLog.txt";

            using (StreamWriter tw = new StreamWriter(LogLocation))
            {
                foreach (string s in powerLog)
                    tw.WriteLine(s);
            }

        }

        private static async Task LogIsComplete()
        {
            await Task.Delay(500);
            if (LogContainsStateComplete || Core.Game.IsInMenu)
                return;
            Log.Info("STATE COMPLETE not found");
            for (var i = 0; i < 5; i++)
            {
                await Task.Delay(1000);
                if (LogContainsStateComplete || Core.Game.IsInMenu)
                    break;
                Log.Info($"Waiting for STATE COMPLETE... ({i})");
            }
        }

        private static bool LogContainsStateComplete
            => Core.Game?.PowerLog?.Any(x => x.Contains("tag=STATE value=COMPLETE")) ?? false;

    }
}
