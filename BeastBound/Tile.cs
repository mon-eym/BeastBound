using System;

namespace PokelikeConsole
{
    internal enum TileType
    {
        GrassShort,
        GrassTall,
        Path,
        Fence,
        Water,
        HouseWall,
        HouseRoof,
        Door,
        Window,
        Sign,
        Mountain,
        Flower,
        Tree,
        Sand,
        Bridge
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
                case TileType.GrassShort:
                    return new Tile(type, true, '.', "·", ConsoleColor.Green, ConsoleColor.DarkGreen);
                case TileType.GrassTall:
                    return new Tile(type, true, '"', "❈", ConsoleColor.Green, ConsoleColor.DarkGreen);
                case TileType.Path:
                    return new Tile(type, true, '#', "▓", ConsoleColor.Yellow, ConsoleColor.DarkYellow);
                case TileType.Fence:
                    return new Tile(type, false, '+', "┼", ConsoleColor.White, ConsoleColor.DarkGray);
                case TileType.Water:
                    return new Tile(type, false, '~', "≈", ConsoleColor.Cyan, ConsoleColor.DarkBlue);
                case TileType.HouseWall:
                    return new Tile(type, false, 'H', "█", ConsoleColor.Gray, ConsoleColor.DarkGray);
                case TileType.HouseRoof:
                    return new Tile(type, false, '^', "▀", ConsoleColor.Red, ConsoleColor.DarkRed);
                case TileType.Door:
                    return new Tile(type, false, 'D', "▛", ConsoleColor.DarkYellow, ConsoleColor.Black);
                case TileType.Window:
                    return new Tile(type, false, 'O', "□", ConsoleColor.White, ConsoleColor.DarkGray);
                case TileType.Sign:
                    return new Tile(type, false, 'S', "☗", ConsoleColor.White, ConsoleColor.DarkGray);
                default:
                    return new Tile(TileType.GrassShort, true, '.', "·", ConsoleColor.Green, ConsoleColor.DarkGreen);
                case TileType.Flower:
                    return new Tile(type, false, '*', "✿", ConsoleColor.Magenta, ConsoleColor.Green);
                case TileType.Tree:
                    return new Tile(type, false, 'T', "♣", ConsoleColor.DarkGreen, ConsoleColor.Green);
            }
        }
    }
}