using System.Collections.Generic;
using System.Numerics;

namespace BattleShip.Models
{
    public class Game
    {
        public List<Player> Players { get; set; }

        public Player? Winner { get; set; }
        public bool IsWinner { get; set; }
        public GameHistory History { get; private set; }
        private static readonly char[] ShipLetter = { 'A', 'B', 'C', 'D', 'E', 'F' };
        private static readonly int[] ShipSize = { 2, 3, 3, 4, 5 };
        public int CurrentPlayerIndex { get; set; } 
        public Game()
        {
            this.Players = new List<Player>();
            this.History = new GameHistory();
        }

        public Game(List<Player> player, int currentPlayer)
        {
            this.Players = player;
            this.History = new GameHistory();
            CurrentPlayerIndex = currentPlayer;
        }

        public string attack(Player attacker, Player defender, int x, int y)
        {
            char cell = defender.PlaceShipGrid.Grid[x, y];

            string response = "null";

            if (cell != '\0' && cell != 'O' && cell != 'X')
            {
                changeCell(defender, attacker, x, y, 'X', cell);

                Ship ship = defender.PlaceShipGrid.Ships.FirstOrDefault(s => s.Letter == cell);
                if (ship != null && ship.IsDead)
                {
                    response = "coulé"; 
                }
                else
                {
                    response = "touché"; 
                }

                if (checkWinner())
                {
                    displayWinner();
                    return "gagnant";
                }
            }
            else if (cell != 'O' && cell != 'X')
            {
                changeCell(defender, attacker, x, y, 'O', cell);
                response = "loupé"; 
            }
            if (defender.Name == "ia")
            {
                playIA(defender, attacker);
            }

            return response;
        }
        
        public void playIA(Player ia, Player player)
        {
            Move lastMove = History.LastHitMoveByPlayer(ia.Name);

            if (lastMove != null)
            {
                int lastAttackX = lastMove.X;
                int lastAttackY = lastMove.Y;

                int perimeter = 1;
                switch (ia.IaDifficulty)
                {
                    case 1:
                        perimeter = 5;
                        break;
                    case 2:
                        perimeter = 3;
                        break;
                    case 3:
                        perimeter = 1;
                        break;
                }

                int[][] possibleMoves = player.PlaceShipGrid.GetPossibleMovesAround(lastAttackX, lastAttackY, perimeter);

                if (possibleMoves.Length > 0)
                {
                    var randomMove = possibleMoves[Random.Shared.Next(possibleMoves.Length)];
                    attack(ia, player, randomMove[0], randomMove[1]);
                }
                else
                {
                    int[][] availableMoves = player.PlaceShipGrid.GetAvailableMoves();
                    var randomMove = availableMoves[Random.Shared.Next(availableMoves.Length)];
                    attack(ia, player, randomMove[0], randomMove[1]);
                }
            }
            else
            {
                int[][] availableMoves = player.PlaceShipGrid.GetAvailableMoves();
                var randomMove = availableMoves[Random.Shared.Next(availableMoves.Length)];
                attack(ia, player, randomMove[0], randomMove[1]);
            }
        }




        public void changeCell(Player defender, Player attacker, int x, int y, char touch, char letter)
        {
            defender.PlaceShipGrid.Grid[x, y] = touch; 

            if (touch == 'X') 
            {
                Ship ship = defender.PlaceShipGrid.Ships.FirstOrDefault(s => s.Letter == letter);
                if (ship != null)
                {
                    ship.RegisterHit(); 

                    if (ship.IsDead) 
                    {
                        History.AddMove(new Move(attacker, defender, x, y, true, letter, "coulé"));
                    }
                    else 
                    {
                        History.AddMove(new Move(attacker, defender, x, y, true, letter, "touché"));
                    }
                }
            }
            else
            {
                History.AddMove(new Move(attacker, defender, x, y, false, '\0', "loupé"));
            }
        }

        public void displayWinner()
        {
            //afficher le gagnant
        }

        public bool checkWinner()
        {
            foreach (Player p in Players)
            {
                bool allShipDead = p.PlaceShipGrid.Ships.All(ship => ship.IsDead);
                if (allShipDead)
                {
                    Winner = Players.FirstOrDefault(player => player != p);
                    IsWinner = true;
                    return true;
                }
            }
            return false;
        }

        public void placeShip(Player player, Ship ship, int x, int y, string orientation)
        {
            char[,] grid = player.PlaceShipGrid.Grid;
            bool isHorizontal = orientation.ToLower() == "horizontal";
            player.PlaceShipGrid.PlaceShip(grid, ship, x, y, isHorizontal);

        }

    public List<PlaceShipGrid> displayGrid()
        {
            List<PlaceShipGrid> grids = new List<PlaceShipGrid>();
            foreach (Player p in Players)
            {
                grids.Add(p.PlaceShipGrid);
            }
            return grids;


        }

        public bool?[,] displayOpponentGrid(PlaceShipGrid grid)
        {
            var gridSize = grid.Grid.GetLength(0);
            var newGrid = new bool?[gridSize,gridSize];

            for (int x = 0; x < grid.Grid.GetLength(0); x++)
            {
                for (int y = 0; y < grid.Grid.GetLength(1); y++)
                {
                    char cell = grid.Grid[x, y];
                        if (cell == 'X')
                        {
                            newGrid[x,y]=true;
                        }
                        else if (cell == 'O')
                        {
                            newGrid[x, y] = false;
                        }
                        else
                        {
                            newGrid[x,y]= null;
                        }                    
                }

            }
            return newGrid;
        }
    }
    


}