using System.Collections.Generic;

namespace T2FSv1.Controller
{
    public interface IOperator
    {
        public double getFiringIntervalU();
        public double getFiringIntervalL();
        public void calculateFiringInterval(double mulx, double muly, double muux, double muuy);
        public void setFiringInterval(double x, double y);
        void calculateFiringInterval(List<double> muu, List<double> mul);
    }
}