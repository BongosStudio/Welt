using Welt.API;

namespace Welt
{
    public static class Constants
    {
        public static string[] Tips = new[]
        {
            "Each world is generated at random. That means you'll never see what's coming!",
            "A typical game instance contains 3 explorable galaxies at minimum. Each galaxy has thousands of solar systems, and each solar system has up to 15 planets! Phew that's a lot of blocks!",
            "Welt's Dynamic Entity Engine, or DEE, creates an evolution chain for each planet, designed for each ecosystem.",
            "Watch out! Some creatures have wings, some have shells, and some are just huge!",
            "404",
            "You can check out the game's soundtrack at https://soundcloud.com/mylittleconji. Just a fun fact.",
            "Torches can have 7 different colors. Experiment to see which ones makes your home look best!"
        };

        public enum DeathMessages
        {
            [StringEnum("%u fought Newton. They lost.")]
            Height,
            [StringEnum("%u learned water and oxygen are not the same thing.")]
            Drown,
            [StringEnum("%u was too cold. Now they're too hot.")]
            Fire,
            [StringEnum("%u died a happy person.")]
            Milk,
            [StringEnum("Death, meet %u. %u, meet Death.")]
            Generic
        }

        public const string VarUsername = "%u";
        public const string VarPosition = "%p";
        public const string VarBlock = "%b";
        public const string VarItem = "%i";
    }
}
