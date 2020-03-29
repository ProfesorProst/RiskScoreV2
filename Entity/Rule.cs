using System;
using System.Collections.Generic;
using T2FSv1.Controller;

namespace T2FSv1.Entity
{
    public class Rule
    {
        private List<IMemFunc> antc = new List<IMemFunc>();
        private List<IMemFunc> cons = new List<IMemFunc>();

        public IOperator firingInterval { get; private set; }

        public Rule(List<IMemFunc> antc, List<IMemFunc> cons, IOperator firingInterval)
        {
            this.antc.AddRange(antc);
            this.cons.AddRange(cons);
            this.firingInterval = firingInterval;
        }

        internal void calculateFiringInterval(double[] inputs)
        {
            int i = 0;
            List<double> muu = new List<double>();
            List<double> mul = new List<double>();
            foreach (double number in inputs)
            {
                IMemFunc memFunc = antc[i];
                muu.Add(memFunc.upperMemFunction(number));
                mul.Add(memFunc.lowerMemFunction(number));
                i++;
            }

            firingInterval.calculateFiringInterval(muu,mul);
        }

        public double getClAvgy()
        {
            double avg = 0.0;
            int n = 0;
            foreach (IMemFunc memFunc in cons)
            {
                avg = avg + memFunc.getCr();
                n++;
            }

            return avg / n;
        }

        public double getClAvgx()
        {
            double avg = 0.0;
            int n = 0;
            foreach (IMemFunc memFunc in cons)
            {
                avg = avg + memFunc.getCl();
                n++;
            }

            return avg / n;
        }
    }
}