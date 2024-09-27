using BattleShip.Models;
using System.Numerics;

namespace BattleShip.App
{
    public class Game
    {
        public List<Player> players { get; set; }

        public Player? winner { get; set; }
        public bool isWinner { get; set; }
        public GameHistory history { get; private set; }
        private static readonly char[] shipLetter = { 'A', 'B', 'C', 'D', 'E', 'F' };
        private static readonly int[] shipSize = { 2, 3, 3, 4, 5 };
        private int currentPlayerIndex;


        public Game(List<Player> player, int currentPlayer)
        {
            this.players = player;
            this.history = new GameHistory();
            currentPlayerIndex = currentPlayer;
        }

        public void attack(Player attacker, Player defender, int x, int y)
        {
            char cell = defender.placeShipGrid.Grid[x, y];
            if (cell != '\0' && cell != 'O' && cell != 'X')
            {
                changeCell(defender, attacker, x, y, 'X', cell);
                if (checkWinner())
                {
                    displayWinner();
                    return;
                }
            }
            else if (cell != 'O' && cell != 'X')
            {
                changeCell(defender, attacker, x, y, 'O', cell);
            }

            if (defender.name == "ia")
            {
                playIA(defender, attacker);
            }

        }
        public void playIA(Player ia, Player player)
        {
            int[][] availableMoves = ia.placeShipGrid.GetAvailableMoves();
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
                history.AddMove(new Move(attacker, x, y, true));
            }
            else
            {
                history.AddMove(new Move(attacker, x, y, false));
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
                    winner = players.FirstOrDefault(p => p != winner);
                    isWinner = true;
                    winner = p;
                    return true;
                }
            }
            //ajouter la variable Grille dans PlaceShipGrid
            //ajouter une fonction qui vérifie si tous les bateaux sont coulés

            return false;
        }

        public void placeShip(Player player, Ship ship, int x, int y, string orientation)
        {
            char[,] grid = player.placeShipGrid.Grid;
            bool isHorizontal = orientation.ToLower() == "horizontal";
            player.placeShipGrid.PlaceShip(grid, ship, x, y, isHorizontal);

        }


        public char[,] displayGrid(bool isCurrent)
        {
            if (isCurrent)
            {
                Player currentPlayer = players[currentPlayerIndex];
                return currentPlayer.placeShipGrid.Grid;
            }
            else
            {
                int opponentIndex = currentPlayerIndex ^ 1;
                Player opponent = players[opponentIndex];
                return displayOpponentGrid(opponent.placeShipGrid);

            }


        }


        private char[,] displayOpponentGrid(PlaceShipGrid grid)
        {
            char[,] newGrid = new char[10, 10];

            for (int x = 0; x < grid.Grid.GetLength(0); x++)
            {
                for (int y = 0; y < grid.Grid.GetLength(1); y++)
                {
                    char cell = grid.Grid[x, y];

                    if (cell == 'X')
                    {
                        newGrid[x, y] = 'X';
                    }
                    else if (cell == 'O')
                    {
                        newGrid[x, y] = 'O';
                    }
                    else if (cell == '\0')
                    {
                        newGrid[x, y] = '.';
                    }
                    else
                    {
                        newGrid[x, y] = '.';
                    }
                }
            }
            return newGrid;
        }
    }
    


}