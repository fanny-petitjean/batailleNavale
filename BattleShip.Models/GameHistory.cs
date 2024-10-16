namespace BattleShip.Models
{
    public class GameHistory
    {
        private List<Move> moves;

        public GameHistory() {
            moves = new List<Move>();
        }

        public void AddMove(Move move)
        {
            moves.Add(move);
        }

        public bool RemoveMove()
        {
            if (moves.Count == 0) return false;
            Move lastMove = moves.Last();
            if (lastMove.defender.placeShipGrid.Grid[lastMove.x, lastMove.y] == 'X')
            {
                Ship shipToUpdate = lastMove.defender.placeShipGrid.ships.FirstOrDefault(ship => ship.letter == lastMove.previousValue);
                shipToUpdate.UnregisterHit();
                shipToUpdate.isDead = false;
            }
            lastMove.defender.placeShipGrid.Grid[lastMove.x, lastMove.y] = lastMove.previousValue;



            moves.Remove(lastMove);
            return true;
        }

        public bool RemoveMoveAll()
        {
            if (moves.Count == 0) return false;
            foreach (Move move in moves)
            {
                if (move.defender.placeShipGrid.Grid[move.x, move.y] == 'X')
                {
                    Ship shipToUpdate = move.defender.placeShipGrid.ships.FirstOrDefault(ship => ship.letter == move.previousValue);
                    shipToUpdate.UnregisterHit();
                    shipToUpdate.isDead = false;
                }
                move.defender.placeShipGrid.Grid[move.x, move.y] = move.previousValue;
            }
            
            moves = new List<Move>();
            return true;
        }

        public void DisplayHistory()
        {
            foreach (Move move in moves)
            {
                Console.WriteLine($"Le joueur {move.attacker.name} à la positiion {move.x}, {move.y} à {move.touch}");
            }
        }

        public string LastMoveName()
        {
            if (moves.Count == 0) return null;
            Move lastMove = moves.Last();
            return lastMove.attacker.name;
        }

        public Move LastMove()
        {
            if (moves.Count == 0) return null;
            return moves.Last();
        }
        public Move SecondLastMove()
        {
            if (moves.Count < 2) return null;
            return moves[moves.Count - 2]; 
        }

        public List<Move> GetMoves()
        {
            return moves;
        }
        public Move LastHitMoveByPlayer(string playerName)
        {
            for (int i = moves.Count - 1; i >= 0; i--)
            {
                if (moves[i].attacker.name.Equals(playerName, StringComparison.OrdinalIgnoreCase) &&
                    (moves[i].touch.Equals("touché", StringComparison.OrdinalIgnoreCase)))
                {
                    return moves[i];
                }
            }
            return null;
        }


    }
}