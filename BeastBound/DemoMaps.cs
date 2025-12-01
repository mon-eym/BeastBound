using System;

namespace PokelikeConsole
{
    internal static class DemoMaps
    {
        // OVERWORLD
        public static Map BuildOverworld()
        {
            int w = 160, h = 80;
            var map = new Map(w, h, spawnX: 80, spawnY: 40);

            for (int x = 0; x < w; x++)
                for (int y = 0; y < h; y++)
                    map.Set(x, y, Tile.Make(TileType.Floor));

            // Hub cross
            for (int x = 0; x < w; x++)
                map.Set(x, 40, Tile.Make(TileType.Floor));
            for (int y = 0; y < h; y++)
                map.Set(80, y, Tile.Make(TileType.Floor));

            // Top room - Town
            Rect(map, 60, 10, 40, 20, TileType.Floor);
            Box(map, 60, 10, 40, 20, TileType.Wall);

            Box(map, 66, 14, 10, 6, TileType.Wall);
            map.Set(71, 19, Tile.Make(TileType.DoorClosed));

            Box(map, 86, 14, 10, 6, TileType.Wall);
            map.Set(91, 19, Tile.Make(TileType.DoorClosed));

            map.Set(80, 31, Tile.Make(TileType.Sign));

            for (int y = 31; y <= 39; y++)
                map.Set(80, y, Tile.Make(TileType.Floor));

            // Left room - Forest
            Rect(map, 10, 30, 40, 20, TileType.GrassLight);
            Box(map, 10, 30, 40, 20, TileType.Wall);

            for (int x = 14; x < 46; x += 4)
                for (int y = 32; y < 48; y += 3)
                    map.Set(x, y, Tile.Make(TileType.Wall));

            for (int x = 40; x <= 79; x++)
                map.Set(x, 40, Tile.Make(TileType.Floor));

            map.Set(39, 40, Tile.Make(TileType.Sign));

            // Bottom room - Beach
            Rect(map, 50, 55, 60, 15, TileType.GrassMedium);
            Box(map, 50, 55, 60, 15, TileType.Wall);

            Rect(map, 50, 70, 60, 10, TileType.Water);

            for (int y = 55; y <= 69; y++)
                map.Set(80, y, Tile.Make(TileType.Floor));

            map.Set(80, 54, Tile.Make(TileType.Sign));

            // Right room - Cave entrance
            Rect(map, 110, 30, 30, 20, TileType.GrassMedium);
            Box(map, 110, 30, 30, 20, TileType.Wall);

            Box(map, 120, 35, 10, 6, TileType.Wall);
            map.Set(125, 38, Tile.Make(TileType.DoorClosed));
            map.Set(124, 37, Tile.Make(TileType.Sign));

            for (int x = 81; x <= 119; x++)
                map.Set(x, 40, Tile.Make(TileType.Floor));

            // Battle House
            Box(map, 72, 44, 16, 10, TileType.Wall);
            map.Set(80, 53, Tile.Make(TileType.DoorClosed));
            map.Set(80, 54, Tile.Make(TileType.Sign));

            return map;
        }

        public static Map BuildBattleHouse()
        {
            int w = 30, h = 18;
            var map = new Map(w, h, spawnX: 15, spawnY: 14);

            for (int x = 0; x < w; x++)
                for (int y = 0; y < h; y++)
                    map.Set(x, y, Tile.Make(TileType.Floor));

            Box(map, 0, 0, w, h, TileType.Wall);

            map.Set(w / 2, h - 1, Tile.Make(TileType.DoorOpen));

            Box(map, 2, 2, 8, 6, TileType.Wall);
            map.Set(6, 5, Tile.Make(TileType.NPC));
            map.Set(6, 9, Tile.Make(TileType.Sign));

            Box(map, 11, 2, 8, 6, TileType.Wall);
            map.Set(15, 5, Tile.Make(TileType.NPC));
            map.Set(15, 9, Tile.Make(TileType.Sign));

            Box(map, 20, 2, 8, 6, TileType.Wall);
            map.Set(24, 5, Tile.Make(TileType.NPC));
            map.Set(24, 9, Tile.Make(TileType.Sign));

            return map;
        }

        public static Map BuildCave()
        {
            int w = 40, h = 20;
            var map = new Map(w, h, spawnX: 20, spawnY: 10);

            for (int x = 0; x < w; x++)
                for (int y = 0; y < h; y++)
                    map.Set(x, y, Tile.Make(TileType.Wall));

            Rect(map, 3, 3, 34, 14, TileType.Floor);

            map.Set(20, 17, Tile.Make(TileType.DoorOpen));

            return map;
        }

        public static Map BuildHouseInterior()
        {
            int w = 20, h = 12;
            var map = new Map(w, h, spawnX: 10, spawnY: 6);

            for (int x = 0; x < w; x++)
                for (int y = 0; y < h; y++)
                    map.Set(x, y, Tile.Make(TileType.Floor));

            Box(map, 0, 0, w, h, TileType.Wall);

            map.Set(w / 2, h - 1, Tile.Make(TileType.DoorOpen));

            return map;
        }

        private static void Rect(Map map, int x, int y, int w, int h, TileType type)
        {
            for (int ix = x; ix < x + w; ix++)
                for (int iy = y; iy < y + h; iy++)
                    if (map.InBounds(ix, iy))
                        map.Set(ix, iy, Tile.Make(type));
        }

        private static void Box(Map map, int x, int y, int w, int h, TileType type)
        {
            for (int ix = x; ix < x + w; ix++)
            {
                if (map.InBounds(ix, y))
                    map.Set(ix, y, Tile.Make(type));
                if (map.InBounds(ix, y + h - 1))
                    map.Set(ix, y + h - 1, Tile.Make(type));
            }

            for (int iy = y; iy < y + h; iy++)
            {
                if (map.InBounds(x, iy))
                    map.Set(x, iy, Tile.Make(type));
                if (map.InBounds(x + w - 1, iy))
                    map.Set(x + w - 1, iy, Tile.Make(type));
            }
        }
    }
}