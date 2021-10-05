using System;
using System.Linq;

namespace ApportionmentCalculatorNET
{
    public class HuntingtonHill
    {

        public static Tuple<int[], int[], decimal[], decimal[], decimal, decimal> Calculate(int seats, int[] populations)
        {
            int states = populations.Length;
            decimal initialDivisor = populations.Sum() / (decimal)seats;
            decimal[] initialQuotas = new decimal[states];

            // Calculate the initial quotas.
            for (int i = 0; i < states; i++)
            {
                initialQuotas[i] = populations[i] / initialDivisor;
            }

            int[] initialFairShares = new int[states];
            decimal[] initialGeometricMeans = new decimal[states];
            decimal[] finalGeometricMeans = new decimal[states];

            // Calculate the initial fair shares.
            for (int i = 0; i < states; i++)
            {
                decimal mean = (decimal)Math.Sqrt((double)Math.Floor(initialQuotas[i] * (Math.Floor(initialQuotas[i]) + 1)));
                initialGeometricMeans[i] = mean;
                finalGeometricMeans[i] = mean;

                if (mean < initialQuotas[i])
                {
                    initialFairShares[i] = (int)Math.Ceiling(initialQuotas[i]);
                }
                else if (mean > initialQuotas[i])
                {
                    initialFairShares[i] = (int)Math.Floor(initialQuotas[i]);
                }
            }

            decimal[] finalQuotas = new decimal[states];
            decimal modifiedDivisor = populations.Sum() / (decimal)seats;

            // Calculate the final quotas.
            for (int i = 0; i < states; i++)
            {
                finalQuotas[i] = populations[i] / modifiedDivisor;
            }

            int[] finalFairShares = new int[states];
            decimal estimator = populations.Sum() / seats;

            // Initialize final fair shares.
            for (int i = 0; i < states; i++)
            {
                finalFairShares[i] = (int)Math.Floor(initialQuotas[i]);
            }

            int timeKeeper = 0;

            // Begin apportionment.
            while (finalFairShares.Sum() != seats)
            {
                if (timeKeeper == 5000)
                {
                    break;
                }

                for (int i = 0; i < states; i++)
                {
                    decimal mean = (decimal)Math.Sqrt((double)(Math.Floor(finalQuotas[i]) * (Math.Floor(finalQuotas[i]) + 1)));
                    finalGeometricMeans[i] = mean;

                    if (mean < finalQuotas[i])
                    {
                        finalFairShares[i] = (int)Math.Ceiling(finalQuotas[i]);
                    }
                    else if (mean > finalQuotas[i])
                    {
                        finalFairShares[i] = (int)Math.Floor(finalQuotas[i]);
                    }
                }

                if (finalFairShares.Sum() > seats)
                {
                    modifiedDivisor += estimator;
                }
                else
                {
                    modifiedDivisor -= estimator;
                }

                estimator /= 2;

                if (modifiedDivisor == 0)
                {
                    modifiedDivisor = 1;
                }

                for (int i = 0; i < states; i++)
                {
                    finalQuotas[i] = populations[i] / modifiedDivisor;
                }

                for (int i = 0; i < states; i++)
                {
                    decimal mean = (decimal)Math.Sqrt((double)(Math.Floor(finalQuotas[i]) * (Math.Floor(finalQuotas[i]) + 1)));
                    finalGeometricMeans[i] = mean;

                    if (mean < finalQuotas[i])
                    {
                        finalFairShares[i] = (int)Math.Ceiling(finalQuotas[i]);
                    }
                    else if (mean > finalQuotas[i])
                    {
                        finalFairShares[i] = (int)Math.Floor(finalQuotas[i]);
                    }
                }

                timeKeeper++;
            }

            if (timeKeeper == 5000)
            {
                return null;
            }
            else
            {
                for (int i = 0; i < states; i++)
                {
                    initialQuotas[i] = Math.Round(initialQuotas[i], 5);
                    finalQuotas[i] = Math.Round(finalQuotas[i], 5);
                }
                initialDivisor = Math.Round(initialDivisor, 5);
                modifiedDivisor = Math.Round(modifiedDivisor, 5);
                return Tuple.Create(initialFairShares, finalFairShares, initialQuotas, finalQuotas, initialDivisor, modifiedDivisor);
            }
        }
    }
}