using BattleShip.Models;
using Microsoft.AspNetCore.Authorization;
namespace BattleShip.API
{
    [Authorize]  
    public class GameService
    {
        public Dictionary<Guid, Game> Games = new Dictionary<Guid, Game>();
        private Dictionary<string, int> PlayerVictories = new Dictionary<string, int>();


        public Guid AddGame(Game game)
        {
            Guid guid = Guid.NewGuid();
            Games.Add(guid, game);
            return guid;
        }

        public Game GetGame(Guid gameId)
        {
            if (Games.TryGetValue(gameId, out var game))
            {
                return game;
            }
            throw new KeyNotFoundException("Game not found.");
        }
        public Dictionary<string, int> GetPlayerVictories()
        {
            return PlayerVictories;
        }
        public void IncrementPlayerVictory(string playerName)
        {
            if (PlayerVictories.ContainsKey(playerName))
            {
                PlayerVictories[playerName]++;
            }
            else
            {
                PlayerVictories[playerName] = 1;
            }
        }
    }
}

