using Beastbound.Models;
using System.Collections.Generic;
using Beastbound.Battle;
using BattleType = Beastbound.Battle.Type;
public static class Factory
{
    public static (Creature player, Creature enemy) CreateDemoDuel()
    {
        // Choose your starter here
        var player = PokemonFactory.CreateCharizard();
        // var player = PokemonFactory.CreateBlastoise();
        // var player = PokemonFactory.CreateVenusaur();

        var enemy = new Creature
        {
            Name = "Tempestral",
            Level = 50,
            MaxHP = 160,
            CurrentHP = 160,
            Attack = 100,
            Defense = 85,
            Speed = 100,
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
}