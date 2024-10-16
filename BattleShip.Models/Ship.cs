
namespace BattleShip.Models
{
    public class Ship
    {
        public Guid id { get; }
        public char letter { get; set; }
        public int size { get; set; }
        public int hits { get; set; }
        public bool isDead { get; set; }
        public bool isHorizontal { get; set; }
        public int positionX { get; set; }
        public int positionY { get; set; }

        public Ship(char letter, int size, bool isHorizontal, int positionX, int positionY)
        {
            this.id = Guid.NewGuid();
            this.letter = letter;
            this.size = size;
            this.hits = 0;
            this.isHorizontal = isHorizontal;
            this.positionX = positionX;
            this.positionY = positionY;

        }
        public Ship(char letter, int size)
        {
            this.id = Guid.NewGuid();
            this.letter = letter;
            this.size = size;
            this.hits = 0;
            this.isHorizontal = isHorizontal;
            this.positionX = positionX;
            this.positionY = positionY;

        }
        public void RegisterHit()
        {
            hits++;

            if (hits >= size)
            {
                isDead = true;
            }
        }
        public void UnregisterHit() { hits--; }

    }
}