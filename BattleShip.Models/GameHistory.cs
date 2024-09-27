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
            lastMove.player.placeShipGrid.Grid[lastMove.x, lastMove.y] = lastMove.previousValue;


            moves.Remove(lastMove);
            return true;
        }

        public void DisplayHistory()
        {
            foreach (Move move in moves)
            {
                Console.WriteLine($"Player {move.player.name} at position {move.x}, {move.y} {(move.isHit ? "hit" : "missed")}");
            }
        }

    }
}