using BattleShip.Models;
using Microsoft.AspNetCore.Authorization;
namespace BattleShip.API
{
    [Authorize]  
    public class GameService
    {
        public Dictionary<Guid, Game> games = new Dictionary<Guid, Game>();
        private Dictionary<string, int> playerVictories = new Dictionary<string, int>();


        public Guid AddGame(Game game)
        {
            Guid guid = Guid.NewGuid();
            games.Add(guid, game);
            return guid;
        }

        public Game GetGame(Guid gameId)
        {
            if (games.TryGetValue(gameId, out var game))
            {
                return game;
            }
            throw new KeyNotFoundException("Game not found.");
        }
        public Dictionary<string, int> GetPlayerVictories()
        {
            return playerVictories;
        }
        public void IncrementPlayerVictory(string playerName)
        {
            if (playerVictories.ContainsKey(playerName))
            {
                playerVictories[playerName]++;
            }
            else
            {
                playerVictories[playerName] = 1;
            }
        }
    }
}

