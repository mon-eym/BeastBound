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

            // Fill with grass
            for (int x = 0; x < w; x++)
                for (int y = 0; y < h; y++)
                    map.Set(x, y, Tile.Make(TileType.GrassShort));

            // Ocean at bottom
            Rect(map, 0, 60, w, 20, TileType.Water);
            for (int x = 0; x < w; x++)
                map.Set(x, 59, Tile.Make(TileType.Path)); // shoreline

            // Town (top-left)
            Rect(map, 10, 10, 30, 20, TileType.Path);
            House(map, 12, 12);
            House(map, 22, 12);
            House(map, 18, 20);
            map.Set(20, 10, Tile.Make(TileType.Sign));

            // Forest (top-right)
            Rect(map, 100, 5, 40, 25, TileType.GrassTall);
            for (int i = 0; i < 40; i++)
                map.Set(100 + i, 5 + i % 7, Tile.Make(TileType.Fence));

            // Route path connecting zones
            for (int x = 0; x < w; x++)
                map.Set(x, 40, Tile.Make(TileType.Path));
            for (int y = 20; y <= 60; y++)
                map.Set(80, y, Tile.Make(TileType.Path));

            // Cave entrance (bottom-right)
            Rect(map, 120, 50, 15, 10, TileType.HouseWall);
            map.Set(127, 55, Tile.Make(TileType.Door));
            map.Set(126, 54, Tile.Make(TileType.Sign));

            // Decorative tall grass patches
            Rect(map, 50, 25, 10, 10, TileType.GrassTall);
            Rect(map, 70, 45, 8, 8, TileType.GrassTall);

            // Route sign
            map.Set(80, 39, Tile.Make(TileType.Sign));

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