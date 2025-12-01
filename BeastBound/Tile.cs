using System;

namespace PokelikeConsole
{
    internal enum TileType
    {
        Floor,
        Wall,
        DoorClosed,
        DoorOpen,
        DoorLocked,
        Sign,
        GrassLight,
        GrassMedium,
        GrassHeavy,
        Water,
        Player,
        NPC
    }

    internal readonly struct Tile
    {
        public TileType Type { get; }
        public bool IsWalkable { get; }
        public char GlyphAscii { get; }
        public string GlyphUnicode { get; }
        public ConsoleColor Foreground { get; }
        public ConsoleColor Background { get; }

        public Tile(
            TileType type, bool isWalkable,
            char glyphAscii, string glyphUnicode,
            ConsoleColor fg, ConsoleColor bg)
        {
            Type = type;
            IsWalkable = isWalkable;
            GlyphAscii = glyphAscii;
            GlyphUnicode = glyphUnicode;
            Foreground = fg;
            Background = bg;
        }

        public static Tile Make(TileType type)
        {
            switch (type)
            {
                case TileType.Floor:
                    return new Tile(type, true, '.', "·", ConsoleColor.Gray, ConsoleColor.Black);

                case TileType.Wall:
                    return new Tile(type, false, '#', "█", ConsoleColor.White, ConsoleColor.Black);

                case TileType.DoorClosed:
                    return new Tile(type, false, '+', "╬", ConsoleColor.Yellow, ConsoleColor.Black);

                case TileType.DoorOpen:
                    return new Tile(type, true, '/', "╫", ConsoleColor.Green, ConsoleColor.Black);

                case TileType.DoorLocked:
                    return new Tile(type, false, 'X', "╪", ConsoleColor.Red, ConsoleColor.Black);

                case TileType.Sign:
                    return new Tile(type, false, '?', "?", ConsoleColor.Cyan, ConsoleColor.Black);

                case TileType.GrassLight:
                    return new Tile(type, true, ',', "░", ConsoleColor.Green, ConsoleColor.Black);

                case TileType.GrassMedium:
                    return new Tile(type, true, ';', "▒", ConsoleColor.DarkGreen, ConsoleColor.Black);

                case TileType.GrassHeavy:
                    return new Tile(type, false, '#', "▓", ConsoleColor.DarkGray, ConsoleColor.Black);

                case TileType.Water:
                    return new Tile(type, false, '~', "≈", ConsoleColor.Blue, ConsoleColor.Black);

                case TileType.Player:
                    return new Tile(type, true, '@', "@", ConsoleColor.White, ConsoleColor.Black);

                case TileType.NPC:
                    return new Tile(type, true, 'N', "☺", ConsoleColor.Magenta, ConsoleColor.Black);

                default:
                    return new Tile(TileType.Floor, true, '.', "·", ConsoleColor.Gray, ConsoleColor.Black);
            }
        }
    }
}