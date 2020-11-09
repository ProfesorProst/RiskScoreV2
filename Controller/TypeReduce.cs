using System;
using System.Collections.Generic;
using T2FSv1.Entity;

namespace T2FSv1.Controller
{
    public class TypeReduce
    {
        private Dictionary<Double, Rule> ruleMapy;
        private Dictionary<Double, Rule> ruleMapx;
        private List<Double> yrk;
        private List<Double> ylk;

        public TypeReduce(List<Rule> ruleList)
        {
            ruleMapx = new Dictionary<Double, Rule>();
            ruleMapy = new Dictionary<Double, Rule>();

            yrk = new List<Double>();
            ylk = new List<Double>();

            double addValue = 0.0000000001;
            int count = 0;
            foreach (Rule rule in ruleList)
            {
                ruleMapx[rule.getClAvgx() + addValue * count] = rule;
                ruleMapy[rule.getClAvgy() + addValue * count] = rule;

                Console.WriteLine((rule.getClAvgx() + (addValue * count)) + " " + (rule.getClAvgy() + (addValue * count)) + ", is " 
                    + rule.firingInterval.getFiringIntervalU() + "," + rule.firingInterval.getFiringIntervalL());
                count++;
            }

            yrk.AddRange(ruleMapy.Keys);
            yrk.Sort();

            ylk.AddRange(ruleMapx.Keys);
            ylk.Sort();
        }

        private TypeReduce(Dictionary<Double, Rule> ruleMapy, Dictionary<Double, Rule> ruleMapx, List<Double> yrk, List<Double> ylk)
        {
            this.ruleMapy = ruleMapy;
            this.ruleMapx = ruleMapx;
            this.yrk = yrk;
            this.ylk = ylk;
        }

        public double yl()
        {
            double ylkQuote = step1(ylk, ruleMapx);
            int roul = 0;
            int count = 0;

            bool test = true;
            while (test)
            {
                int R = findR(ylk, ylkQuote);

                double ylkDQuote = step3l(ylk, ruleMapx, R, roul);

                if (Math.Abs(ylkQuote - ylkDQuote) < 0.001 || count==1000)
                {
                    test = false;
                    break;
                }
                else
                {
                    ylkQuote = ylkDQuote;
                    count++;
                }
            }

            return ylkQuote;
        }

        public double yr()
        {
            double yrkQuote = step1(yrk, ruleMapy);

            bool test = true;
            while (test)
            {
                int R = findR(yrk, yrkQuote);

                double yrkDQuote = step3(yrk, ruleMapy, R);

                if (Math.Abs(yrkQuote - yrkDQuote) < 0.000000001)
                {
                    test = false;
                    break;
                }
                else
                {
                    yrkQuote = yrkDQuote;
                }
            }

            return yrkQuote;
        }

        public double step3(List<Double> yrk, Dictionary<Double, Rule> ruleMap, int R)
        {
            double num = 0.0;
            double den = 0.0;

            int count = 1;
            foreach (double yi in yrk)
            {
                if (yi == 0)
                {
                    continue;
                }
                Rule ruleRow = ruleMap[yi];

                double fri = 0.0;
                if (count <= R)
                    fri = ruleRow.firingInterval.getFiringIntervalU();
                else
                    fri = ruleRow.firingInterval.getFiringIntervalL();

                num += fri * yi;
                den += fri;

                count++;
            }

            return num / den;
        }

        public double step3l(List<Double> ylk, Dictionary<Double, Rule> ruleMap, int R, int ruleout)
        {
            double num = 0.0;
            double den = 0.0;

            int count = 1;
            foreach (double yi in ylk)
            {
                if (yi == 0)
                    continue;
                Rule ruleRow = ruleMap[yi];

                double fli = 0.0;
                if (count <= R)
                    fli = ruleRow.firingInterval.getFiringIntervalL();
                else
                    fli = ruleRow.firingInterval.getFiringIntervalU();

                if (fli != 0)
                    ruleout = count;
                num += fli * yi;
                den += fli;

                count++;
            }

            return num / den;
        }


        public double step1(List<Double> yrk, Dictionary<Double, Rule> ruleMap)
        {
            double num = 0.0;
            double den = 0.0;

            foreach (double yi in yrk)
            {
                if (yi == 0)
                {
                    continue;
                }
                Rule ruleRow = ruleMap[yi];
                double fri = (ruleRow.firingInterval.getFiringIntervalU() + ruleRow.firingInterval.getFiringIntervalL()) / 2;
                double yrki = yi;

                num += fri * yrki;
                den += fri;
            }
            return num / den;
        }

        public int findR(List<Double> yk, double yrkQuote)
        {
            if (yk.Count == 1)
                return 1;

            int R = 1;
            foreach (double yi in yk)
            {
                if (yi < yrkQuote && yk[R] > yrkQuote)
                    break;
                R++;
            }

            return R;
        }
    }
}