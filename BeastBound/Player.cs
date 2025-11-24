namespace PokelikeConsole
{
    internal sealed class Player
    {
        public int X { get; private set; }
        public int Y { get; private set; }

        // Simple rate limiting for movement (used on tall grass)
        public int StepDelayTicks { get; set; } = 0;
        private int _cooldown = 0;

        public Player(int x, int y)
        {
            X = x;
            Y = y;
        }

        public bool CanStep()
        {
            if (_cooldown > 0)
            {
                _cooldown--;
                return false;
            }
            return true;
        }

        public void MoveTo(int x, int y)
        {
            X = x;
            Y = y;
            _cooldown = StepDelayTicks;
        }
    }
}