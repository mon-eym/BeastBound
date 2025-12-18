using Beastbound.Models;
using System.Collections.Generic;
using Beastbound.Battle;
using BattleType = Beastbound.Battle.Type;

public static class Factory
{
    public static (Creature player, Creature enemy) CreateDemoDuel(string selectedName)
    {
        var player = selectedName switch
        {
            "Charizard" => PokemonFactory.CreateCharizard(),
            "Blastoise" => PokemonFactory.CreateBlastoise(),
            "Venusaur" => PokemonFactory.CreateVenusaur(),
            _ => PokemonFactory.CreateCharizard()
        };

        var enemy = new Creature
        {
            Name = "Tempestral",
            Level = 50,
            MaxHP = 160,
            CurrentHP = 160,
            Attack = 100,
            Defense = 85,
            Speed = 90,
            PrimaryType = BattleType.Gale,
            SecondaryType = BattleType.None,
            IsPlayer = false,
            Moves = new List<Move>
            {
                new Move("Gale Cutter", BattleType.Gale, 85, 100),
                new Move("Aerial Spike", BattleType.Gale, 90, 90),
                new Move("Storm Bolt", BattleType.Volt, 95, 95),
                new Move("Shade Veil", BattleType.Shade, 60, 100)
            }
        };

        Creature.PlayerPrimaryType = player.PrimaryType;
        return (player, enemy);
    }

    // ✅ Boss selector by stage
    public static Creature CreateBoss(int stageNumber)
    {
        return stageNumber switch
        {
            1 => GhostBoss(),   // weak to Charizard
            2 => SecondBoss(),    // weak to Blastoise
            3 => FinalBoss(),   // weak to Venusaur
            _ => GhostBoss()
        };
    }

    // ✅ Stage 1: Grass-type boss
    public static Creature GhostBoss()
    {
        return new Creature
        {
            Name = "Ghost",
            Level = 50,
            MaxHP = 180,
            CurrentHP = 180,
            Attack = 95,
            Defense = 100,
            Speed = 80,
            PrimaryType = BattleType.Leaf,
            SecondaryType = BattleType.None,
            Moves = new List<Move>
            {
                new Move("Vine Lash", BattleType.Leaf, 85, 100),
                new Move("Spore Burst", BattleType.Leaf, 70, 95),
                new Move("Root Snare", BattleType.Leaf, 90, 90),
                new Move("Nature's Grasp", BattleType.Leaf, 60, 100)
            }
        };
    }

    // ✅ Stage 2: Fire-type boss
    public static Creature SecondBoss()
    {
        return new Creature
        {
            Name = "Pyronox",
            Level = 50,
            MaxHP = 190,
            CurrentHP = 190,
            Attack = 105,
            Defense = 90,
            Speed = 85,
            PrimaryType = BattleType.Flame,
            SecondaryType = BattleType.None,
            Moves = new List<Move>
            {
                new Move("Inferno Burst", BattleType.Flame, 95, 90),
                new Move("Blazing Charge", BattleType.Flame, 80, 100),
                new Move("Cinder Storm", BattleType.Flame, 70, 95),
                new Move("Flame Barrage", BattleType.Flame, 100, 85)
            }
        };
    }

    // ✅ Stage 3: Water-type boss
    public static Creature FinalBoss()
    {
        return new Creature
        {
            Name = "Aquarion",
            Level = 50,
            MaxHP = 200,
            CurrentHP = 200,
            Attack = 100,
            Defense = 105,
            Speed = 75,
            PrimaryType = BattleType.Water,
            SecondaryType = BattleType.None,
            Moves = new List<Move>
            {
                new Move("Tidal Crush", BattleType.Water, 90, 95),
                new Move("Aqua Lance", BattleType.Water, 85, 100),
                new Move("Bubble Barrage", BattleType.Water, 70, 100),
                new Move("Hydro Slam", BattleType.Water, 100, 90)
            }
        };
    }
}