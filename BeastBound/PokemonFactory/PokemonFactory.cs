using PokelikeConsole.Battle;

public static class PokemonFactory
{
    public static Pokemon CreateStarter()
    {
        return new Pokemon
        {
            Name = "TrucVert",
            Level = 50,
            HP = 61,
            MaxHP = 61,
            Moves = new[]
            {
                new Move { Name = "Headbutt", Power = 20 },
                new Move { Name = "Flash", Power = 0 },
                new Move { Name = "Mud-Slap", Power = 15 },
                new Move { Name = "Tackle", Power = 10 }
            }
        };
    }

    public static Pokemon CreateEasyEnemy()
    {
        return new Pokemon
        {
            Name = "Pidgey",
            Level = 10,
            HP = 30,
            MaxHP = 30,
            Moves = new[]
            {
                new Move { Name = "Gust", Power = 10 }
            }
        };
    }

    public static Pokemon CreateMediumEnemy()
    {
        return new Pokemon
        {
            Name = "Growlithe",
            Level = 25,
            HP = 50,
            MaxHP = 50,
            Moves = new[]
            {
                new Move { Name = "Ember", Power = 15 }
            }
        };
    }

    public static Pokemon CreateHardEnemy()
    {
        return new Pokemon
        {
            Name = "Mr-Mime",
            Level = 50,
            HP = 56,
            MaxHP = 56,
            Moves = new[]
            {
                new Move { Name = "Psybeam", Power = 20 }
            }
        };
    }
}