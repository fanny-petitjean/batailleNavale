namespace BattleShip.Models
{
    public class GameHistory
    {
        private List<Move> Moves;

        public GameHistory() {
            Moves = new List<Move>();
        }

        public void AddMove(Move move)
        {
            Moves.Add(move);
        }

        public bool RemoveMove()
        {
            if (Moves.Count == 0) return false;
            Move lastMove = Moves.Last();
            if (lastMove.Defender.PlaceShipGrid.Grid[lastMove.X, lastMove.Y] == 'X')
            {
                Ship shipToUpdate = lastMove.Defender.PlaceShipGrid.Ships.FirstOrDefault(ship => ship.Letter == lastMove.PreviousValue);
                shipToUpdate.UnregisterHit();
                shipToUpdate.IsDead = false;
            }
            lastMove.Defender.PlaceShipGrid.Grid[lastMove.X, lastMove.Y] = lastMove.PreviousValue;



            Moves.Remove(lastMove);
            return true;
        }

        public bool RemoveMoveAll()
        {
            if (Moves.Count == 0) return false;
            foreach (Move move in Moves)
            {
                if (move.Defender.PlaceShipGrid.Grid[move.X, move.Y] == 'X')
                {
                    Ship shipToUpdate = move.Defender.PlaceShipGrid.Ships.FirstOrDefault(ship => ship.Letter == move.PreviousValue);
                    shipToUpdate.UnregisterHit();
                    shipToUpdate.IsDead = false;
                }
                move.Defender.PlaceShipGrid.Grid[move.X, move.Y] = move.PreviousValue;
            }

            Moves = new List<Move>();
            return true;
        }

        public void DisplayHistory()
        {
            foreach (Move move in Moves)
            {
                Console.WriteLine($"Le joueur {move.Attacker.Name} à la positiion {move.X}, {move.Y} à {move.Touch}");
            }
        }

        public string LastMoveName()
        {
            if (Moves.Count == 0) return null;
            Move lastMove = Moves.Last();
            return lastMove.Attacker.Name;
        }

        public Move LastMove()
        {
            if (Moves.Count == 0) return null;
            return Moves.Last();
        }
        public Move SecondLastMove()
        {
            if (Moves.Count < 2) return null;
            return Moves[Moves.Count - 2]; 
        }

        public List<Move> GetMoves()
        {
            return Moves;
        }
        public Move LastHitMoveByPlayer(string playerName)
        {
            for (int i = Moves.Count - 1; i >= 0; i--)
            {
                if (Moves[i].Attacker.Name.Equals(playerName, StringComparison.OrdinalIgnoreCase) &&
                    (Moves[i].Touch.Equals("touché", StringComparison.OrdinalIgnoreCase)))
                {
                    return Moves[i];
                }
            }
            return null;
        }


    }
}