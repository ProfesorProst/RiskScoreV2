using System;
using System.Collections.Generic;
using T2FSv1.Controller;

namespace T2FSv1.Entity
{
    internal class TrapezeMF : AbstractTrapezeMF
    {
        private double startUMF;
        private double mean1UMF;
        private double mean2UMF;
        private double endUMF;

        private double startLMF;
        private double mean1LMF;
        private double mean2LMF;
        private double endLMF;

        private List<Double> elements;

        private double cl = 0.0;
        private double cr = 0.0;

        public TrapezeMF()
        {
        }

        public TrapezeMF(double startUMF, double mean1UMF, double mean2UMF, double endUMF, double startLMF, double mean1LMF, double mean2LMF, double endLMF)
        {
            this.startUMF = startUMF;
            this.mean1UMF = mean1UMF;
            this.mean2UMF = mean2UMF;
            this.endUMF = endUMF;
            this.startLMF = startLMF;
            this.mean1LMF = mean1LMF;
            this.mean2LMF = mean2LMF;
            this.endLMF = endLMF;
        }

        public override double getCl()
        {
            if (cl == 0.0)
                cl = CentriodIterMethod.getInstance().getCrorCl(this);
            
            return cl;
        }

        public override double getCr()
        {
            if (cr == 0.0)
                cr = CentriodIterMethod.getInstance().getCrorCl(this,true);

            return cr;
        }

        public override List<double> getElements(int n)
        {
            if (elements == null)
            {
                elements = new List<Double>();

                double start = Math.Min(startLMF, startUMF);
                double end = Math.Max(endLMF, endUMF);

                double inc = (start + end) / (n - 1);

                for (double i = start; i <= end; i = i + inc)
                    elements.Add(i);
            }

            return elements;
        }

        public override double lowerMemFunction(double x)
        {
            if (x < startUMF || x > endUMF)
                return 0;

            if (x >= mean1UMF && x <= mean2UMF)
                return 1;
            else if (x < mean1UMF)
                return (x - startUMF) / (mean1UMF - startUMF);
            else
                return (endUMF - x) / (endUMF - mean2UMF);
        }

        public override double upperMemFunction(double x)
        {
            if (x < startLMF || x > endLMF)
                return 0;

            if (x >= mean1LMF && x <= mean2LMF)
                return 1;
            else if (x < mean1LMF)
                return (x - startLMF) / (mean1LMF - startLMF);
            else
                return (endLMF - x) / (endLMF - mean2LMF);
        }
    }
}