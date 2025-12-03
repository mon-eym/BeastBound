using Beastbound.Battle;

// Alias to avoid conflict with System.Type
using BattleType = Beastbound.Battle.Type;

namespace Beastbound.Models
{
    public class Move
    {
        public string Name { get; set; }
        public BattleType Type { get; set; }
        public int Power { get; set; }
        public int Accuracy { get; set; } // 0-100

        public Move(string name, BattleType type, int power, int accuracy)
        {
            Name = name;
            Type = type;
            Power = power;
            Accuracy = accuracy;
        }
    }
}