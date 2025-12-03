using System;
using System.Runtime.InteropServices;
using System.Threading;
using WindowsInput;
using WindowsInput.Native;

namespace Beastbound.Utils
{
    public static class FullscreenHelper
    {
        [DllImport("kernel32.dll")]
        private static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        private const int SW_SHOWMAXIMIZED = 3;
        private const int SW_RESTORE = 9;

        public static void EnsureFullscreen()
        {
            var hWnd = GetConsoleWindow();
            if (hWnd == IntPtr.Zero) return;

            // Bring to foreground
            SetForegroundWindow(hWnd);
            Thread.Sleep(100);

            // Try Alt+Enter twice (some shells need a retry)
            var sim = new InputSimulator();
            TryAltEnter(sim);
            Thread.Sleep(250);
            TryAltEnter(sim);

            // Wait for resize and re-center later
            Thread.Sleep(300);

            // Fallback: maximize if Alt+Enter not supported (e.g., Windows Terminal)
            ShowWindow(hWnd, SW_SHOWMAXIMIZED);
            Thread.Sleep(150);
        }

        private static void TryAltEnter(InputSimulator sim)
        {
            sim.Keyboard.ModifiedKeyStroke(VirtualKeyCode.MENU, VirtualKeyCode.RETURN);
        }
    }
}