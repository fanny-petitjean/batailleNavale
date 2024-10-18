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
        public int X { get; set; }
        public int Y { get; set; }
        public char PreviousValue { get; set; }

        public bool IsHit
        {
            get; set;
        }
        public string Touch;
        public Player Attacker { get; set; }
        public Player Defender { get; set; }
        public Move(int x, int y, bool isHit, string touch)
        {
            this.X = x;
            this.Y = y;
            this.IsHit = isHit;
            this.Touch = touch;
        }

        [JsonConstructor]

        public Move(Player attacker, Player defender, int x, int y, bool isHit, char previousValue, string touch)
        {
            this.X = x;
            this.Y = y;
            this.Attacker = attacker;
            this.Defender = defender;
            this.IsHit = isHit;
            this.PreviousValue = previousValue;
            this.Touch = touch;
        }
     
    }
}
