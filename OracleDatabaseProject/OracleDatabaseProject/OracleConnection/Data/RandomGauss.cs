using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OracleDatabaseProject
{
    class RandomGauss : Random
    {
        private double cache;
        private bool isCacheFilled = false;

        const double mean = 0.5;
        const double standardDeviation = 0.5 / Math.PI;

        private double Trunc(double value)
        {
            if (value < 0.0 || value >= 1.0) return this.Sample();
            return value;
        }

        public override double NextDouble()
        {
            return this.Sample();
        }

        public override int Next()
        {
            int gaussNumber = (int)(this.NextDouble() * int.MaxValue);
            return gaussNumber;
        }

        public override int Next(int maxValue)
        {
            return this.Next() % maxValue;
        }

        public override int Next(int minValue, int maxValue)
        {
            return this.Next(maxValue - minValue) + minValue;
        }

        protected override double Sample()
        {
            if (isCacheFilled)
            {
                isCacheFilled = false;
                return cache;
            }

            double r = 0.0;
            double x = 0.0;
            double y = 0.0;

            do
            {
                x = 2.0 * base.Sample() - 1.0;
                y = 2.0 * base.Sample() - 1.0;
                r = x * x + y * y;
            }
            while (r >= 1.0 || r == 0.0);

            double z = Math.Sqrt(-2.0 * Math.Log(r) / r);

            cache = Trunc(mean + standardDeviation * x * z);
            isCacheFilled = true;

            return Trunc(mean + standardDeviation * y * z);
        }
    }
}
