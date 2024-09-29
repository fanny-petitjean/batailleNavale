using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleShip.Models
{
    public class Move
    {
        public int x { get; set; }
        public int y { get; set; }
        public bool isHit
        {
            get; set;
        }
        public Player attacker { get; set; }
        public Move(int x, int y, bool isHit)
        {
            this.x = x;
            this.y = y;
            this.isHit = isHit;
        }

        public char previousValue { get; set; }
        public Move(Player attacker, int x, int y, bool isHit)
        {
            this.x = x;
            this.y = y;
            this.attacker = attacker;
            this.isHit = isHit;
            this.previousValue = attacker.placeShipGrid.Grid[x,y];
        }
    }
}
