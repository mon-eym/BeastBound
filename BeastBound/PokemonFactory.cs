using Beastbound.Models;
using System.Collections.Generic;
using Beastbound.Battle;
using BattleType = Beastbound.Battle.Type;

public static class PokemonFactory
{
    public static Creature CreateCharizard()
    {
        return new Creature
        {
            Name = "Charizard",
            Level = 50,
            MaxHP = 180,
            CurrentHP = 180,
            Attack = 120,
            Defense = 100,
            Speed = 95,
            PrimaryType = BattleType.Flame,
            SecondaryType = BattleType.None,
            Player = true,
            Moves = new List<Move>
            {
                new Move("Flamethrower", BattleType.Flame, 90, 100),
                new Move("Fire Spin", BattleType.Flame, 35, 100),
                new Move("Dragon Claw", BattleType.Dragon, 80, 100),
                new Move("Air Slash", BattleType.Gale, 75, 95)
            }
        };
    }

    public static Creature CreateBlastoise()
    {
        return new Creature
        {

            Name = "Blastoise",
            Level = 50,
            MaxHP = 185,
            CurrentHP = 185,
            Attack = 115,
            Defense = 120,
            Speed = 85,
            PrimaryType = BattleType.Aqua,
            SecondaryType = BattleType.None,
            Player = true,
            Moves = new List<Move>
            {
                new Move("Hydro Pump", BattleType.Aqua, 90, 95),
                new Move("Water Gun", BattleType.Aqua, 35, 100),
                new Move("Ice Beam", BattleType.Frost, 85, 95),
                new Move("Bite", BattleType.Shade, 60, 100)
            }
        };
    }

    public static Creature CreateVenusaur()
    {
        return new Creature
        {
            Name = "Venusaur",
            Level = 50,
            MaxHP = 190,
            CurrentHP = 190,
            Attack = 110,
            Defense = 115,
            Speed = 80,
            PrimaryType = BattleType.Nature,
            SecondaryType = BattleType.None,
            Player = true,
            Moves = new List<Move>
            {
                new Move("Solar Beam", BattleType.Nature, 100, 100),
                new Move("Vine Whip", BattleType.Nature, 45, 100),
                new Move("Earthquake", BattleType.Terra, 95, 100),
                new Move("Body Slam", BattleType.Normal, 70, 100)
            }
        };
    }
}