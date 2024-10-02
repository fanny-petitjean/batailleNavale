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

        public bool RemoveMove() {
            if( moves.Count == 0 ) return false ;
            Move lastMove = moves.Last();
            lastMove.attacker.placeShipGrid.Grid[lastMove.x, lastMove.y] = lastMove.previousValue;


            moves.Remove(lastMove);
            return true;
        }

        public void DisplayHistory()
        {
            foreach (Move move in moves)
            {
                Console.WriteLine($"Player {move.attacker.name} at position {move.x}, {move.y} {(move.isHit ? "hit" : "missed")}");
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
            if (moves.Count < 2) return null; // Vérifie qu'il y a au moins deux coups
            return moves[moves.Count - 2]; // Retourne l'avant-dernier coup
        }

        public List<Move> GetMoves()
        {
            return moves;
        }

    }
}