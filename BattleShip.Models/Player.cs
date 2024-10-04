
namespace BattleShip.Models
{
    public class Player
    {
        public Guid id { get; }
        public string name { get; set; }
        public bool isIa { get; }
        public PlaceShipGrid placeShipGrid { get; set; }


        public Player(string name, bool isIa)
        {
            this.id = Guid.NewGuid();
            this.name = name;

            this.placeShipGrid = new PlaceShipGrid();
            this.isIa = isIa;

        }
        public Player(string name, bool isIa, int gridSize)
        {
            this.id = Guid.NewGuid();
            this.name = name;

            this.placeShipGrid = new PlaceShipGrid(gridSize);
            this.isIa = isIa;

        }

        public Player(string name, bool isIa, PlaceShipGrid placeShipGrid)
        {
            this.id = Guid.NewGuid();
            this.name = name;

            this.placeShipGrid = placeShipGrid;
            this.isIa = isIa;

        }
    }
        
}