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

        public string attack(Player attacker, Player defender, int x, int y)
        {
            char cell = defender.placeShipGrid.Grid[x, y];

            string response = "null";

            if (cell != '\0' && cell != 'O' && cell != 'X')
            {
                changeCell(defender, attacker, x, y, 'X', cell);

                Ship ship = defender.placeShipGrid.ships.FirstOrDefault(s => s.letter == cell);
                if (ship != null && ship.isDead)
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

            // Si le défenseur est un IA, elle joue ensuite
            if (defender.name == "ia")
            {
                playIA(defender, attacker);
            }

            return response;
        }



        public void playIA(Player ia, Player player)
        {
            Move lastMove = history.LastHitMoveByPlayer(ia.name);

            if (lastMove != null)
            {
                int lastAttackX = lastMove.x;
                int lastAttackY = lastMove.y;

                int perimeter = 1;
                switch (ia.iaDifficulty)
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

                int[][] possibleMoves = player.placeShipGrid.GetPossibleMovesAround(lastAttackX, lastAttackY, perimeter);

                if (possibleMoves.Length > 0)
                {
                    var randomMove = possibleMoves[Random.Shared.Next(possibleMoves.Length)];
                    attack(ia, player, randomMove[0], randomMove[1]);
                }
            }
            else
            {
                int[][] availableMoves = player.placeShipGrid.GetAvailableMoves();
                var randomMove = availableMoves[Random.Shared.Next(availableMoves.Length)];
                attack(ia, player, randomMove[0], randomMove[1]);
            }
        }




        public void changeCell(Player defender, Player attacker, int x, int y, char touch, char letter)
        {
            defender.placeShipGrid.Grid[x, y] = touch; // Mise à jour du coup sur la grille

            if (touch == 'X') // Si c'est un coup touché
            {
                Ship ship = defender.placeShipGrid.ships.FirstOrDefault(s => s.letter == letter);
                if (ship != null)
                {
                    ship.RegisterHit(); // Enregistre le coup sur le bateau

                    if (ship.isDead) // Si le bateau est coulé
                    {
                        history.AddMove(new Move(attacker, defender, x, y, true, letter, "coulé"));
                    }
                    else // Si le bateau est juste touché
                    {
                        history.AddMove(new Move(attacker, defender, x, y, true, letter, "touché"));
                    }
                }
            }
            else
            {
                history.AddMove(new Move(attacker, defender, x, y, false, '\0', "loupé"));
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