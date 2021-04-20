using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scrap_Scramble_Final_Version.GameRelated
{
    [Serializable]
    public enum FightResult
    {
        WIN,
        LOSS,
        BYE
    }

    [Serializable]
    public class PairsHandler
    {
        public Dictionary<ulong, ulong> opponents { get; private set; }
        public List<Dictionary<ulong, FightResult>> playerResults;

        public PairsHandler()
        {
            this.opponents = new Dictionary<ulong, ulong>();
            this.playerResults = new List<Dictionary<ulong, FightResult>>();
            this.playerResults.Add(new Dictionary<ulong, FightResult>());
        }

        //for new games
        public PairsHandler(GameHandler gameHandler)
        {
            this.opponents = new Dictionary<ulong, ulong>();
            this.playerResults = new List<Dictionary<ulong, FightResult>>();

            foreach (var player in gameHandler.players)
            {
                this.opponents.Add(player.Key, player.Key);
            }
        }

        public void SetPair(ulong a, ulong b)
        {
            if (!this.opponents.ContainsKey(a) || !this.opponents.ContainsKey(b)) return;

            this.opponents[this.opponents[a]] = this.opponents[a];
            this.opponents[this.opponents[b]] = this.opponents[b];

            this.opponents[a] = b;
            this.opponents[b] = a;
        }

        public void NextRoundPairs(GameHandler gameHandler, int times = 0)
        {
            this.playerResults.Add(new Dictionary<ulong, FightResult>());
            foreach (var id in gameHandler.players)
            {                
                this.playerResults.Last().Add(id.Key, FightResult.BYE);
            }

            //List<int> players = new List<int>();
            //List<int> newOpponents = new List<int>();

            List<ulong> players = new List<ulong>();
            Dictionary<ulong, ulong> newOpponents = new Dictionary<ulong, ulong>();

            //for (int i = 0; i < gameHandler.players.Count(); i++)
            //{
            //    newOpponents.Add(i);
            //    if (gameHandler.players[i].lives > 0) players.Add(i);
            //}

            foreach (var player in gameHandler.players)
            {
                newOpponents.Add(player.Key, player.Key);
                if (player.Value.lives > 0) players.Add(player.Key);
            }

            if (players.Count() == 1)
            {
                //newOpponents[players[0]] = players[0];
                newOpponents[players[0]] = players[0];
            }
            else if (players.Count() == 2)
            {
                newOpponents[players[0]] = players[1];
                newOpponents[players[1]] = players[0];
            }
            else
            {
                if (players.Count() % 2 == 1)
                {
                    players.Sort((x, y) => gameHandler.players[x].lives.CompareTo(gameHandler.players[y].lives));

                    for (int i = 0; i < players.Count(); i++)
                    {
                        if (opponents[players[i]] == players[i]) continue;
                        newOpponents[players[i]] = players[i];
                        players.RemoveAt(i);
                        break;
                    }
                }

                players = players.OrderBy(x => GameHandler.randomGenerator.Next()).ToList();

                if (players.Count() % 2 == 1)
                {
                    players.RemoveAt(0);
                }

                for (int i = 0; i < players.Count(); i += 2)
                {
                    if (this.opponents[players[i]] == players[i + 1] && times < 8)
                    {
                        this.playerResults.RemoveAt(this.playerResults.Count() - 1);
                        NextRoundPairs(gameHandler, times + 1);
                        return;
                    }
                    newOpponents[players[i]] = players[i + 1];
                    newOpponents[players[i + 1]] = players[i];
                }                
            }

            this.opponents = newOpponents;
            //for (int i = 0; i < newOpponents.Count(); i++) this.opponents.Add(newOpponents[i]);
            //BotInfoHandler.pairsReady = true;
        }
    }
}
