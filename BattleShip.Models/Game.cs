using BattleShip.Models;
using System.Numerics;

namespace BattleShip.App
{
    public class Game
    {
        public List<Player> players { get; set; }

        public Player? winner { get; set; }
        public bool isWinner { get; set; }
        public GameHistory history { get; private set; } = new GameHistory();
        private static readonly char[] shipLetter = { 'A', 'B', 'C', 'D', 'E', 'F' };
        private static readonly int[] shipSize = { 2, 3, 3, 4, 5 };  


        public Game(List<Player> player)
        {
            this.players = player;
        }

        public void attack(Player attacker, Player defender, int x, int y)
        {
            //faire une fonction Vérifier si la case est vide ou pleine
            // Si elle est pleine alors récupérer id du ship + lui enlever une vie
            // retourne True si il est touché ou false sinon
            if(defender.placeShipGrid.Grid[x,y] != '\0')
            {
                char hit = defender.placeShipGrid.Grid[x, y];
                defender.placeShipGrid.Grid[x, y] = 'X';
                Ship ship = defender.placeShipGrid.ships.FirstOrDefault(s => s.letter == hit);
                ship.RegisterHit();
                checkWinner();


            }
            else
            {
                defender.placeShipGrid.Grid[x, y] = 'O';

            }

        }

        public bool checkWinner(){
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
            player.placeShipGrid.PlaceShip(ship, x, y, isHorizontal);

        }

        public void displayGrid()
        {

        }

    }
}