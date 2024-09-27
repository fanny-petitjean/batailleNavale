using BattleShip.App;

namespace BattleShip.App
{
    public class Player
    {
        public Guid id { get; }
        public string Name { get; set; }
        public bool isIa { get; }
        public PlaceShipGrid placeShipGrid { get; set; }


        public Player(string Name, List<Ship> ships, bool isIa)
        {
            this.id = Guid.NewGuid();
            this.Name = Name;
<<<<<<< HEAD
            this.placeShipGrid = new PlaceShipGrid();
            this.isIa = isIa;
=======
            this.ships = ships;
            //this.placeShipGrid = new PlaceShipGrid();
>>>>>>> 20720c9d0895205282ffeb1244decceb481da15d
        }
    }
}