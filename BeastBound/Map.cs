using System;

namespace PokelikeConsole
{
    internal sealed class Map
    {
        private readonly Tile[,] _tiles;

        public int Width { get; }
        public int Height { get; }
        public int SpawnX { get; }
        public int SpawnY { get; }

        public Map(int width, int height, int spawnX, int spawnY)
        {
            Width = width;
            Height = height;
            SpawnX = spawnX;
            SpawnY = spawnY;
            _tiles = new Tile[width, height];
        }

        public void Set(int x, int y, Tile tile) => _tiles[x, y] = tile;

        public Tile Get(int x, int y) => _tiles[x, y];

        public bool InBounds(int x, int y) => x >= 0 && y >= 0 && x < Width && y < Height;
    }

    internal static class DemoMaps
    {
        // Simple Pallet Town-like layout
        public static Map BuildOverworld()
        {
            int w = 160, h = 80;
            var map = new Map(w, h, spawnX: 80, spawnY: 40);

            // Base: short grass everywhere
            for (int x = 0; x < w; x++)
                for (int y = 0; y < h; y++)
                    map.Set(x, y, Tile.Make(TileType.GrassShort));

            // Central cross paths (hub like your screenshot)
            for (int x = 0; x < w; x++)
                map.Set(x, 40, Tile.Make(TileType.Path)); // horizontal
            for (int y = 0; y < h; y++)
                map.Set(80, y, Tile.Make(TileType.Path)); // vertical

            // --- Top "Town" room (above the cross) ---
            // Town plaza area
            Rect(map, 60, 10, 40, 20, TileType.Path);

            // Two houses
            House(map, 66, 14); // left house
            House(map, 86, 14); // right house

            // Fence border for town
            HorizontalFence(map, 60, 10, 40);
            HorizontalFence(map, 60, 30, 40);
            VerticalFence(map, 60, 10, 20);
            VerticalFence(map, 100, 10, 20);

            // Town sign at bottom of town area
            map.Set(80, 31, Tile.Make(TileType.Sign));

            // Path connecting town to hub
            for (int y = 31; y <= 39; y++)
                map.Set(80, y, Tile.Make(TileType.Path));

            // --- Left "Forest" room ---
            Rect(map, 10, 30, 40, 20, TileType.GrassTall);
            // A simple maze-ish fence inside forest
            for (int x = 14; x < 46; x += 4)
                for (int y = 32; y < 48; y += 2)
                    map.Set(x, y, Tile.Make(TileType.Fence));

            // Path from hub to forest
            for (int x = 40; x <= 79; x++)
                map.Set(x, 40, Tile.Make(TileType.Path));

            map.Set(39, 40, Tile.Make(TileType.Sign)); // "Forest" sign

            // --- Bottom "Beach" room ---
            Rect(map, 50, 55, 60, 15, TileType.Sand); // you can define Sand as a tile, or use Path/Grass
            Rect(map, 50, 70, 60, 10, TileType.Water);

            // Pier/path to water
            for (int y = 55; y <= 69; y++)
                map.Set(80, y, Tile.Make(TileType.Path));

            map.Set(80, 54, Tile.Make(TileType.Sign)); // "Beach" sign

            // --- Right "Cave" area ---
            Rect(map, 110, 30, 30, 20, TileType.GrassShort);
            // Rocky border using HouseWall as rock
            HorizontalWall(map, 110, 30, 30);
            HorizontalWall(map, 110, 49, 30);
            VerticalWall(map, 110, 30, 20);
            VerticalWall(map, 139, 30, 20);

            // Cave entrance (door) on the inner side
            Rect(map, 120, 35, 10, 6, TileType.HouseWall);
            map.Set(125, 38, Tile.Make(TileType.Door));
            map.Set(124, 37, Tile.Make(TileType.Sign)); // "Cave" sign

            // Path from hub to cave
            for (int x = 81; x <= 119; x++)
                map.Set(x, 40, Tile.Make(TileType.Path));

            // --- Battle House near center (interactive) ---
            // Place a special house south of the hub center
            House(map, 76, 44); // centered around x ~78
                                // Replace its door tile with a special BattleHouse door
                                // Assuming the default House places a door at (houseX+2, houseY+3)
            map.Set(78, 47, Tile.Make(TileType.Door));
            map.Set(78, 48, Tile.Make(TileType.Sign)); // "Battle House" sign

            return map;
        }
        public static Map BuildBattleHouse()
        {
            int w = 30, h = 18;
            var map = new Map(w, h, spawnX: 15, spawnY: 14);

            // Floor
            for (int x = 0; x < w; x++)
                for (int y = 0; y < h; y++)
                    map.Set(x, y, Tile.Make(TileType.Path));

            // Walls around
            for (int x = 0; x < w; x++)
            {
                map.Set(x, 0, Tile.Make(TileType.HouseWall));
                map.Set(x, h - 1, Tile.Make(TileType.HouseWall));
            }
            for (int y = 0; y < h; y++)
            {
                map.Set(0, y, Tile.Make(TileType.HouseWall));
                map.Set(w - 1, y, Tile.Make(TileType.HouseWall));
            }

            // Exit door (back to overworld) at bottom center
            map.Set(w / 2, h - 1, Tile.Make(TileType.Door));

            // Decorations / arena feel
            // Left “easy” side
            Rect(map, 4, 4, 7, 5, TileType.GrassShort);
            // Center “medium” side
            Rect(map, 12, 4, 7, 5, TileType.GrassTall);
            // Right “hard” side
            Rect(map, 20, 4, 7, 5, TileType.HouseWall); // like obstacles

            return map;
        }


        public static Map BuildCave()
        {
            int w = 40, h = 20;
            var map = new Map(w, h, spawnX: 20, spawnY: 10);

            // Fill cave walls
            for (int x = 0; x < w; x++)
                for (int y = 0; y < h; y++)
                    map.Set(x, y, Tile.Make(TileType.HouseWall));

            // Path inside cave
            Rect(map, 5, 5, 30, 10, TileType.Path);

            // Exit back to overworld
            map.Set(20, 18, Tile.Make(TileType.Door));

            return map;
        }

        public static Map BuildHouseInterior()
        {
            int w = 20, h = 12;
            var map = new Map(w, h, spawnX: 10, spawnY: 6);

            // Floor
            for (int x = 0; x < w; x++)
                for (int y = 0; y < h; y++)
                    map.Set(x, y, Tile.Make(TileType.Path));

            // Walls
            for (int x = 0; x < w; x++)
            {
                map.Set(x, 0, Tile.Make(TileType.HouseWall));
                map.Set(x, h - 1, Tile.Make(TileType.HouseWall));
            }
            for (int y = 0; y < h; y++)
            {
                map.Set(0, y, Tile.Make(TileType.HouseWall));
                map.Set(w - 1, y, Tile.Make(TileType.HouseWall));
            }

            // Exit door
            map.Set(10, h - 1, Tile.Make(TileType.Door));

            return map;
        }


        private static void Rect(Map map, int x, int y, int w, int h, TileType type)
        {
            for (int ix = x; ix < x + w; ix++)
                for (int iy = y; iy < y + h; iy++)
                    if (map.InBounds(ix, iy))
                        map.Set(ix, iy, Tile.Make(type));
        }

        private static void HorizontalFence(Map map, int x, int y, int length)
        {
            for (int i = 0; i < length; i++)
                map.Set(x + i, y, Tile.Make(TileType.Fence));
        }

        private static void VerticalFence(Map map, int x, int y, int length)
        {
            for (int i = 0; i < length; i++)
                map.Set(x, y + i, Tile.Make(TileType.Fence));
        }

        private static void House(Map map, int x, int y)
        {
            // Outer walls
            Rect(map, x, y, 10, 6, TileType.HouseWall);

            // Roof line
            for (int i = 0; i < 10; i++)
                map.Set(x + i, y, Tile.Make(TileType.HouseRoof));

            // Door
            map.Set(x + 4, y + 5, Tile.Make(TileType.Door));

            // Windows
            map.Set(x + 2, y + 2, Tile.Make(TileType.Window));
            map.Set(x + 7, y + 2, Tile.Make(TileType.Window));
        }
    }
}