using System.Collections.Generic;
using Beastbound.Battle;
using Beastbound;

// Alias to avoid conflict with System.Type
using BattleType = Beastbound.Battle.Type;

namespace Beastbound.Models
{
    public class Creature
    {
        public string Name { get; set; }
        public string Type { get; set; } // "Flame", "Dragon", "Gale", etc.
        public int Power { get; set; }   // e.g., 90
        public int Accuracy { get; set; } // 0–100
        public int Level { get; set; } = 50;
        public int MaxHP { get; set; }
        public int CurrentHP { get; set; }
        public static BattleType PlayerPrimaryType { get; set; }
        public static string SelectedPokemon { get; private set; } = "";
        public bool Player { get; set; }
        public int Attack { get; set; }
        public int Defense { get; set; }
        public int Speed { get; set; }
        public BattleType PrimaryType { get; set; } = BattleType.None;
        public BattleType SecondaryType { get; set; } = BattleType.None;
        public bool IsPlayer { get; set; }
        public List<Move> Moves { get; set; } = new();
        public bool IsFainted => CurrentHP <= 0;
    }
}