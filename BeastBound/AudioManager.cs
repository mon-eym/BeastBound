using System;
using System.IO;
using NAudio.Wave;

namespace Beastbound.Audio
{
    public static class AudioManager
    {
        private static IWavePlayer? _output;
        private static WaveStream? _reader;

        public static void PlayLoop(string path)
        {
            PlayInternal(path, loop: true);
        }

        public static void PlayOnce(string path)
        {
            PlayInternal(path, loop: false);
        }

        public static void Stop()
        {
            try
            {
                _output?.Stop();
                _output?.Dispose();
                _output = null;

                _reader?.Dispose();
                _reader = null;
            }
            catch { /* ignore */ }
        }

        private static void PlayInternal(string path, bool loop)
        {
            Stop();

            string fullPath = Path.Combine(AppContext.BaseDirectory, path);
            Console.WriteLine("Trying to load: " + fullPath);

            // 🔍 Diagnostic checks
            Console.WriteLine("Directory exists: " + Directory.Exists(Path.GetDirectoryName(fullPath)));
            Console.WriteLine("File exists: " + File.Exists(fullPath));


            if (!File.Exists(fullPath))
            {
                Console.WriteLine("✘ File not found at runtime!");
                throw new FileNotFoundException("Audio file not found.", fullPath);

            }

            Console.WriteLine("✔ File found!");

            try
            {
                var reader = CreateReader(fullPath);
                if (loop) reader = new LoopStream(reader);

                _output = new WaveOutEvent();
                _output.Init(reader);
                _output.Play();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"⚠ Failed to play audio: {ex.Message}");
                // Optional: play a fallback sound or skip audio gracefully
            }
        }

        private static WaveStream CreateReader(string path)
        {
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return ext switch
            {
                ".wav" => new WaveFileReader(path),
                _ => throw new NotSupportedException($"Unsupported audio type: {ext}")
            };
        }

        private class LoopStream : WaveStream
        {
            private readonly WaveStream _source;

            public LoopStream(WaveStream source) => _source = source;

            public override WaveFormat WaveFormat => _source.WaveFormat;
            public override long Length => _source.Length;
            public override long Position
            {
                get => _source.Position;
                set => _source.Position = value;
            }

            public override int Read(byte[] buffer, int offset, int count)
            {
                int read = _source.Read(buffer, offset, count);
                if (read == 0)
                {
                    _source.Position = 0;
                    read = _source.Read(buffer, offset, count);
                }
                return read;
            }
        }
    }
}