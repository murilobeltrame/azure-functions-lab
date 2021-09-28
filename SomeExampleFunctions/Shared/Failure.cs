using System;

namespace SomeExampleFunctions.Shared
{
    public static class Failure
    {
        public static void AtRateOf(short rate)
        {
            var rnd = new Random().Next(0,100);
            if (rnd <= rate) throw new Exception("I just failed.");
        }
    }
}
