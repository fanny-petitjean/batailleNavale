
namespace BattleShip.Models
{
    public class Player
    {
        public Guid Id { get; }
        public string Name { get; set; }
        public bool IsIa { get; }
        public int? IaDifficulty { get; }
        public PlaceShipGrid PlaceShipGrid { get; set; }


        public Player(string name, bool isIa, int iaDifficulty)
        {
            this.Id = Guid.NewGuid();
            this.Name = name;

            this.PlaceShipGrid = new PlaceShipGrid();
            this.IsIa = isIa;
            this.IaDifficulty = iaDifficulty;
        }
        public Player(string name, bool isIa, int gridSize, int iaDifficulty)
        {
            this.Id = Guid.NewGuid();
            this.Name = name;

            this.PlaceShipGrid = new PlaceShipGrid(gridSize);
            this.IsIa = isIa;
            this.IaDifficulty = iaDifficulty;

        }

        public Player(string name, bool isIa, PlaceShipGrid placeShipGrid)
        {
            this.Id = Guid.NewGuid();
            this.Name = name;

            this.PlaceShipGrid = placeShipGrid;
            this.IsIa = isIa;

        }
    }
        
}