using BattleShip.App;

namespace BattleShip.App
{
    public class Player
    {
        public Guid id { get; }
        public string name { get; set; }
        public bool isIa { get; }
        public PlaceShipGrid placeShipGrid { get; set; }


        public Player(string name, bool isIa)
        {
            char[,] grid = new char[10, 10];
            this.id = Guid.NewGuid();
            this.name = name;
    
            this.placeShipGrid = new PlaceShipGrid(grid);
            this.isIa = isIa;
            
            //this.placeShipGrid = new PlaceShipGrid();
            
            this.placeShipGrid = new PlaceShipGrid(grid);
            this.isIa = isIa;
            //this.placeShipGrid = new PlaceShipGrid();

        }
    }
}