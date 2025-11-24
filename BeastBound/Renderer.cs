using System;

namespace PokelikeConsole
{
    internal sealed class Renderer : IDisposable
    {
        private readonly int _viewW;
        private readonly int _viewH;
        private bool _asciiOnly;

        // Simple double-buffer to avoid flicker
        private char[,] _charBuffer;
        private ConsoleColor[,] _fgBuffer;
        private ConsoleColor[,] _bgBuffer;

        public Renderer(int viewWidth, int viewHeight, bool useAsciiOnly)
        {
            _viewW = viewWidth;
            _viewH = viewHeight;
            _asciiOnly = useAsciiOnly;

            _charBuffer = new char[_viewW, _viewH];
            _fgBuffer = new ConsoleColor[_viewW, _viewH];
            _bgBuffer = new ConsoleColor[_viewW, _viewH];

            Console.Clear();
        }

        public void ToggleAsciiMode() => _asciiOnly = !_asciiOnly;

        public void Draw(Map map, Player player, Npc[] npcs)
        {
            // Camera centered on player
            int camX = Clamp(player.X - _viewW / 2, 0, Math.Max(0, map.Width - _viewW));
            int camY = Clamp(player.Y - _viewH / 2, 0, Math.Max(0, map.Height - _viewH));

            // Prepare buffers
            for (int vy = 0; vy < _viewH; vy++)
                for (int vx = 0; vx < _viewW; vx++)
                {
                    int mx = camX + vx;
                    int my = camY + vy;
                    if (!map.InBounds(mx, my))
                    {
                        _charBuffer[vx, vy] = ' ';
                        _fgBuffer[vx, vy] = ConsoleColor.White;
                        _bgBuffer[vx, vy] = ConsoleColor.Black;
                        continue;
                    }

                    var tile = map.Get(mx, my);
                    _charBuffer[vx, vy] = _asciiOnly ? tile.GlyphAscii : tile.GlyphUnicode[0];
                    _fgBuffer[vx, vy] = tile.Foreground;
                    _bgBuffer[vx, vy] = tile.Background;
                }

            // Draw NPCs
            foreach (var npc in npcs)
            {
                if (npc.X >= camX && npc.Y >= camY && npc.X < camX + _viewW && npc.Y < camY + _viewH)
                {
                    int vx = npc.X - camX;
                    int vy = npc.Y - camY;
                    _charBuffer[vx, vy] = _asciiOnly ? 'N' : '☺';
                    _fgBuffer[vx, vy] = ConsoleColor.White;
                    _bgBuffer[vx, vy] = ConsoleColor.Black;
                }
            }

            // Draw Player
            if (player.X >= camX && player.Y >= camY && player.X < camX + _viewW && player.Y < camY + _viewH)
            {
                int vx = player.X - camX;
                int vy = player.Y - camY;
                _charBuffer[vx, vy] = _asciiOnly ? '@' : '⛹';
                _fgBuffer[vx, vy] = ConsoleColor.White;
                _bgBuffer[vx, vy] = ConsoleColor.Black;
            }

            // Compose HUD line
            string hud = $" Pos:({player.X},{player.Y})  ASCII:{_asciiOnly}  A:Interact  Tab:ASCII  Q:Quit ";
            WriteBuffer(hudLine: hud);
        }

        private void WriteBuffer(string hudLine)
        {
            Console.SetCursorPosition(0, 0);
            for (int vy = 0; vy < _viewH; vy++)
            {
                for (int vx = 0; vx < _viewW; vx++)
                {
                    Console.ForegroundColor = _fgBuffer[vx, vy];
                    Console.BackgroundColor = _bgBuffer[vx, vy];
                    Console.Write(_charBuffer[vx, vy]);
                }
                Console.ResetColor();
                Console.WriteLine();
            }

            Console.ResetColor();
            Console.WriteLine(hudLine.PadRight(_viewW));
        }

        public void ShowDialog(string text)
        {
            Console.ResetColor();
            Console.SetCursorPosition(0, _viewH + 1);
            Console.WriteLine(new string('-', _viewW));
            // Replace range operator with Substring for C# 7.3 compatibility
            Console.WriteLine(text.Length > _viewW ? text.Substring(0, _viewW) : text);
            Console.WriteLine(new string('-', _viewW));
        }

        public void Dispose()
        {
            Console.ResetColor();
        }

        // Replace Math.Clamp with a custom Clamp function since Math.Clamp is not available in older .NET versions.
        private static int Clamp(int value, int min, int max)
        {
            if (value < min) return min;
            if (value > max) return max;
            return value;
        }
    }
}