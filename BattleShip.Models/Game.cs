using System.Collections.Generic;
using System.Numerics;

namespace BattleShip.Models
{
    public class Game
    {
        public List<Player> players { get; set; }

        public Player? winner { get; set; }
        public bool isWinner { get; set; }
        public GameHistory history { get; private set; }
        private static readonly char[] shipLetter = { 'A', 'B', 'C', 'D', 'E', 'F' };
        private static readonly int[] shipSize = { 2, 3, 3, 4, 5 };
        public int currentPlayerIndex { get; set; } 
        public Game()
        {
            this.players = new List<Player>();
            this.history = new GameHistory();
        }

        public Game(List<Player> player, int currentPlayer)
        {
            this.players = player;
            this.history = new GameHistory();
            currentPlayerIndex = currentPlayer;
        }

        public char attack(Player attacker, Player defender, int x, int y)
        {
            char cell = defender.placeShipGrid.Grid[x, y];
            char response = 'n';
            if (cell != '\0' && cell != 'O' && cell != 'X')
            {
                changeCell(defender, attacker, x, y, 'X', cell);
                if (checkWinner())
                {
                    displayWinner();
                    return 'W';
                }
                response = 'X';

            }
            else if (cell != 'O' && cell != 'X')
            {
                changeCell(defender, attacker, x, y, 'O', cell);
                response = 'O';
            }

            if (defender.name == "ia")
            {
                playIA(defender, attacker);
            }
            return response;

        }
        public void playIA(Player ia, Player player)
        {
            int[][] availableMoves = player.placeShipGrid.GetAvailableMoves();
            var randomMove = availableMoves[Random.Shared.Next(availableMoves.Length)];
            attack(ia, player, randomMove[0], randomMove[1]);
        }

        public void changeCell(Player defender, Player attacker, int x, int y, char touch, char letter)
        {
            defender.placeShipGrid.Grid[x, y] = touch;
            if (touch == 'X')
            {
                Ship ship = defender.placeShipGrid.ships.FirstOrDefault(s => s.letter == letter);
                ship.RegisterHit();
                history.AddMove(new Move(attacker,defender, x, y, true, letter));
            }
            else
            {
                history.AddMove(new Move(attacker,defender, x, y, false, '\0'));
            }
        }

        public void displayWinner()
        {
            //display

            //afficher le gagnant
        }

        public bool checkWinner()
        {
            foreach (Player p in players)
            {
                bool allShipDead = p.placeShipGrid.ships.All(ship => ship.isDead);
                if (allShipDead)
                {
                    winner = players.FirstOrDefault(player => player != p);
                    isWinner = true;
                    return true;
                }
            }
            return false;
        }

        public void placeShip(Player player, Ship ship, int x, int y, string orientation)
        {
            char[,] grid = player.placeShipGrid.Grid;
            bool isHorizontal = orientation.ToLower() == "horizontal";
            player.placeShipGrid.PlaceShip(grid, ship, x, y, isHorizontal);

        }

 public List<PlaceShipGrid> displayGrid()
        {
            List<PlaceShipGrid> grids = new List<PlaceShipGrid>();
            foreach (Player p in players)
            {
                grids.Add(p.placeShipGrid);
            }
            return grids;


        }

        public bool?[,] displayOpponentGrid(PlaceShipGrid grid)
        {
            var newGrid = new bool?[10,10];

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