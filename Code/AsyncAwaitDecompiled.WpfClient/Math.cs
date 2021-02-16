using System;
using System.Linq;
using System.Threading.Tasks;

namespace AsyncAwaitDecompiled.WpfClient
{
    public static class Math
    {
        public static long CalculateLowestCommonMultiple(int upperLimit)
        {
            if (upperLimit < 3)
                throw new ArgumentOutOfRangeException(nameof(upperLimit));

            var denominators = Enumerable.Range(2, upperLimit - 1).ToList();
            for (var i = (long) upperLimit; i < long.MaxValue; i++)
            {
                if (denominators.All(denominator => i % denominator == 0))
                    return i;
            }

            return -1L;
        }

        public static Task<long> CalculateLowestCommonMultipleAsync(int upperLimit)
        {
            return Task.Run(() => CalculateLowestCommonMultiple(upperLimit));
        }
    }
}