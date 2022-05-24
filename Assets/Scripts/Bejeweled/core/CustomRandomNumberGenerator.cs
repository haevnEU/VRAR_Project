using System;

namespace Bejeweled
{
    public class CustomRandomNumberGenerator
    {
        private static CustomRandomNumberGenerator instance = null;
        public static CustomRandomNumberGenerator get()
        {
            if (null == instance)
            {
                instance = new CustomRandomNumberGenerator();
            }
            return instance;
        }

        private Random rng;

        private CustomRandomNumberGenerator()
        {
            rng = new Random();
        }

        public int getNext()
        {
            return rng.Next();
        }

        public int getNext(int lower, int upper)
        {
            return rng.Next(lower, upper);
        }

        public int getNext(int lower, int upper, int prev)
        {
            return rng.Next(lower, upper);
        }

    }
}
