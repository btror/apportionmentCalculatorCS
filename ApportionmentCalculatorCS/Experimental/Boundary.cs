using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace ApportionmentCalculatorNET
{
    public class Boundary
    {

        public static decimal CalculateLowestBoundary(string method, decimal divisor, int[] populations, int seats)
        {
            decimal lowestDivisor = 0;
            decimal previousDivisor = 0;

            int states = populations.Length;
            int estimator = 1000000000;

            decimal[] quotas = new decimal[states];
            decimal[] fairShares = new decimal[states];

            int counter = 0;
            while (counter < 5000)
            {
                for (int i = 0; i < states; i++)
                {
                    quotas[i] = populations[i] / divisor;
                    if (method.Equals("System.Windows.Controls.ComboBoxItem: adam"))
                    {
                        fairShares[i] = Math.Ceiling(quotas[i]);
                    } else if (method.Equals("System.Windows.Controls.ComboBoxItem: webster"))
                    {
                        fairShares[i] = Math.Round(quotas[i]);
                    } else if (method.Equals("System.Windows.Controls.ComboBoxItem: jefferson"))
                    {
                        fairShares[i] = Math.Floor(quotas[i]);
                    }
                }
                if (fairShares.Sum() != seats) {
                    estimator = estimator / 10;
                    previousDivisor = divisor;
                    divisor = lowestDivisor - estimator;
                } else
                {
                    lowestDivisor = divisor;
                    divisor = previousDivisor - estimator;
                    if (lowestDivisor == divisor)
                    {
                        break;
                    }
                }
                counter++;
            }
            return lowestDivisor;
        }


        public static decimal CalculateHighestBoundary(string method, decimal divisor, int[] populations, int seats)
        {
            decimal highestDivisor = 0;
            decimal previousDivisor = 0;

            int states = populations.Length;
            int estimator = 1000000000;

            decimal[] quotas = new decimal[states];
            decimal[] fairShares = new decimal[states];

            int counter = 0;
            while (counter < 5000)
            {
                for (int i = 0; i < states; i++)
                {
                    quotas[i] = populations[i] / divisor;
                    if (method.Equals("System.Windows.Controls.ComboBoxItem: adam"))
                    {
                        fairShares[i] = Math.Ceiling(quotas[i]);
                    }
                    else if (method.Equals("System.Windows.Controls.ComboBoxItem: webster"))
                    {
                        fairShares[i] = Math.Round(quotas[i]);
                    }
                    else if (method.Equals("System.Windows.Controls.ComboBoxItem: jefferson"))
                    {
                        fairShares[i] = Math.Floor(quotas[i]);
                    }
                }
                if (fairShares.Sum() != seats)
                {
                    estimator = estimator / 10;
                    previousDivisor = divisor;
                    divisor = highestDivisor + estimator;
                }
                else
                {
                    highestDivisor = divisor;
                    divisor = previousDivisor + estimator;
                    if (highestDivisor == divisor)
                    {
                        break;
                    }
                }
                counter++;
            }
            return highestDivisor;
        }
    }
}
