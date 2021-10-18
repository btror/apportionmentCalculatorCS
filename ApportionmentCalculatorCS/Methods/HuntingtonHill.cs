using System;
using System.Linq;

namespace ApportionmentCalculatorNET
{
    public class HuntingtonHill
    {

        public static Tuple<decimal[], int[]> Calculate(int seats, int[] populations)
        {
            int states = populations.Length;
            int[] fairShares = new int[states];
            decimal[] priorityValues = new decimal[states];

     
            for (int i = 0; i < states; i++)
            {
                // Each state gets one seats by default.
                fairShares[i] = 1;

                // Calculate priority values.
                priorityValues[i] = populations[i] / (decimal)Math.Sqrt(fairShares[i] * (fairShares[i] + 1));
            }

            while (fairShares.Sum() != seats)
            {
                // Locate the highest priority and increment the seats.
                decimal highestDecimal = priorityValues.Max();
                int index = Array.IndexOf(priorityValues, highestDecimal);
                fairShares[index]++;

                // Update the priority values.
                for (int i = 0; i < states; i++)
                {
                    // Calculate priority values.
                    priorityValues[i] = populations[i] / (decimal)Math.Sqrt(fairShares[i] * (fairShares[i] + 1));
                }
            }

            for (int i = 0; i < states; i++)
            {
                priorityValues[i] = Math.Round(priorityValues[i], 5);
            }
            return Tuple.Create(priorityValues, fairShares);

        }
    }
}