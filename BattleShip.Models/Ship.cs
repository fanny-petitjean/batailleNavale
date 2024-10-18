
namespace BattleShip.Models
{
    public class Ship
    {
        public Guid Id { get; }
        public char Letter { get; set; }
        public int Size { get; set; }
        public int Hits { get; set; }
        public bool IsDead { get; set; }
        public bool IsHorizontal { get; set; }
        public int PositionX { get; set; }
        public int PositionY { get; set; }

        public Ship(char letter, int size, bool isHorizontal, int positionX, int positionY)
        {
            this.Id = Guid.NewGuid();
            this.Letter = letter;
            this.Size = size;
            this.Hits = 0;
            this.IsHorizontal = isHorizontal;
            this.PositionX = positionX;
            this.PositionY = positionY;

        }
        public Ship(char letter, int size)
        {
            this.Id = Guid.NewGuid();
            this.Letter = letter;
            this.Size = size;
            this.Hits = 0;

        }
        public void RegisterHit()
        {
            Hits++;

            if (Hits >= Size)
            {
                IsDead = true;
            }
        }
        public void UnregisterHit() { Hits--; }

    }
}