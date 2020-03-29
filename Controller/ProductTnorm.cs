using System.Collections.Generic;
using System.Linq;

namespace T2FSv1.Controller
{
    public class ProductTnorm : IOperator
    {
        public double firingIntervalU { get; private set; }
        public double firingIntervalL { get; private set; }

        public void setFiringInterval(double firingIntervalx, double firingIntervaly)
        {
            this.firingIntervalU = firingIntervalx;
            this.firingIntervalL = firingIntervaly;
        }

        public void calculateFiringInterval(double mulx, double muly, double muux, double muuy)
        {
            this.firingIntervalU = mulx * muly;
            this.firingIntervalL = muux * muuy;
        }

        public double getFiringIntervalU()
        {
            return firingIntervalU;
        }

        public double getFiringIntervalL()
        {
            return firingIntervalL;
        }

        public void calculateFiringInterval(List<double> muu, List<double> mul)
        {
            this.firingIntervalU = muu.Aggregate((x, y) => x * y);
            this.firingIntervalL = mul.Aggregate((x, y) => x * y);
        }
    }
}
