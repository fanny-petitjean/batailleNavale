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
                // Si le joueur existe, incrémenter son nombre de victoires
                playerVictories[playerName]++;
            }
            else
            {
                // Sinon, ajouter le joueur au dictionnaire avec 1 victoire
                playerVictories[playerName] = 1;
            }
        }
    }
}

