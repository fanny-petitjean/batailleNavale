using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
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
        public Player defender { get; set; }
        public Move(int x, int y, bool isHit)
        {
            this.x = x;
            this.y = y;
            this.isHit = isHit;
        }

        public char previousValue { get; set; }
        [JsonConstructor]

        public Move(Player attacker, Player defender, int x, int y, bool isHit, char previousValue)
        {
            this.x = x;
            this.y = y;
            this.attacker = attacker;
            this.defender = defender;
            this.isHit = isHit;
            this.previousValue = previousValue;
            Console.WriteLine("Move created");
            Console.WriteLine("previous", previousValue);
        }
     
    }
}
