using Beastbound.Battle;
using BattleType = Beastbound.Battle.Type;

namespace Beastbound.Models
{
    public static class Factory
    {
        // Used by simple AI scoring
        public static BattleType PlayerPrimaryType { get; private set; } = BattleType.Flame;

        public static (Creature player, Creature enemy) CreateDemoDuel()
        {
            var player = new Creature
            {
                Name = "Pyrodon",
                Level = 50,
                MaxHP = 180,
                CurrentHP = 180,
                Attack = 120,
                Defense = 100,
                Speed = 95,
                PrimaryType = BattleType.Flame,
                SecondaryType = BattleType.None,
                IsPlayer = true
            };
            player.Moves.AddRange(new[]
            {
                new Move("Blaze Fang", BattleType.Flame, 80, 100),
                new Move("Stone Crash", BattleType.Terra, 75, 95),
                new Move("Wild Volt", BattleType.Volt, 90, 85),
                new Move("Aqua Slash", BattleType.Aqua, 70, 100)
            });

            var enemy = new Creature
            {
                Name = "Tempestral",
                Level = 50,
                MaxHP = 170,
                CurrentHP = 170,
                Attack = 110,
                Defense = 90,
                Speed = 110,
                PrimaryType = BattleType.Gale,
                SecondaryType = BattleType.None,
                IsPlayer = false
            };
            enemy.Moves.AddRange(new[]
            {
                new Move("Gale Cutter", BattleType.Gale, 85, 100),
                new Move("Aerial Spike", BattleType.Gale, 90, 90),
                new Move("Storm Bolt", BattleType.Volt, 95, 85),
                new Move("Shade Veil",  BattleType.Shade, 60, 100)
            });

            PlayerPrimaryType = player.PrimaryType;
            return (player, enemy);
        }
    }
}