namespace Beastbound.Battle
{
    public enum Type
    {
        None,
        Flame,
        Aqua,
        Terra,
        Gale,
        Volt,
        Shade,
        Light
    }

    public static class TypeChart
    {
        // Simplified effectiveness: 2x, 0.5x, 1x, 0x
        public static double Effectiveness(Type attack, Type defend)
        {
            if (defend == Type.None) return 1.0;
            if (attack == defend) return 1.0;

            return (attack, defend) switch
            {
                (Type.Flame, Type.Aqua) => 0.5,
                (Type.Flame, Type.Terra) => 0.5,
                (Type.Aqua, Type.Flame) => 2.0,
                (Type.Aqua, Type.Terra) => 2.0,
                (Type.Aqua, Type.Volt) => 0.5,
                (Type.Terra, Type.Volt) => 2.0,
                (Type.Terra, Type.Gale) => 0.5,
                (Type.Terra, Type.Aqua) => 0.5,
                (Type.Gale, Type.Terra) => 2.0,
                (Type.Gale, Type.Volt) => 0.5,
                (Type.Volt, Type.Aqua) => 2.0,
                (Type.Volt, Type.Terra) => 0.0, // Ground immunity style
                (Type.Volt, Type.Gale) => 2.0,
                (Type.Shade, Type.Light) => 0.5,
                (Type.Light, Type.Shade) => 2.0,
                _ => 1.0
            };
        }
    }
}