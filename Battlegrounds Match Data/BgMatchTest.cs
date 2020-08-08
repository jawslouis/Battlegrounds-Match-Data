using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BattlegroundsMatchData
{
    class BgMatchTest
    {
        public static void Test()
        {

            GameRecord record = CreateTestRecord();
            //GraphqlConnector.WriteBoard(record);
            GraphqlConnector.WriteGameRecord(record);

        }

        public static GameRecord CreateTestRecord()
        {
            TurnSnapshot snap = new TurnSnapshot();
            snap.Minions = "Cobalt Guardian (18/10 Divine Shield, Deathrattle), Cobalt Guardian (19/13 Divine Shield), Iron Sensei (6/2), Mecharoo (11/11 Taunt, Divine Shield, Deathrattle), Zapp Slywick (7/10 Windfury), Mechano-Egg (10/21 Taunt, Divine Shield, Deathrattle), Zapp Slywick (7/10 Windfury)";
            snap.Hero = "Testing (Snap1)";
            snap.Turn = 15;
            snap.isSelf = "Yes";
            snap.dateTime = DateTimeOffset.Now;
            snap.GameID = "test123";
            snap.player = "Me";

            TurnSnapshot snap2 = new TurnSnapshot();
            snap2.Minions = "Cobalt Guardian (18/10 Divine Shield, Deathrattle), Cobalt Guardian (19/13 Divine Shield), Iron Sensei (6/2), Mecharoo (11/11 Taunt, Divine Shield, Deathrattle), Zapp Slywick (7/10 Windfury), Mechano-Egg (10/21 Taunt, Divine Shield, Deathrattle), Zapp Slywick (7/10 Windfury)";
            snap2.Hero = "Testing (Snap2)";
            snap2.Turn = 15;
            snap2.dateTime = DateTimeOffset.Now;
            snap2.isSelf = "";
            snap2.result = "Lose";
            snap2.GameID = "test123";
            snap2.player = "Opponent";

            GameRecord record = new GameRecord();
            record.Histories.Add(snap);
            record.Histories.Add(snap2);
            record.Rating = 2100;
            record.MmrChange = -30;

            record.TavernTierTimings = new List<int> { 2, 5, 7, 9 };
            record.Snapshot = snap;

            return record;
        }
    }
}
