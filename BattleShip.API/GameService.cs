using BattleShip.Models;
using Microsoft.AspNetCore.Authorization;
namespace BattleShip.API
{
    [Authorize]  
    public class GameService
    {
        public Dictionary<Guid, Game> games = new Dictionary<Guid, Game>();

        
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
    }
}

