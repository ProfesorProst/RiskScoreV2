using System;
using T2FSv1.Entity;

namespace T2FSv1.Controller
{
    public class CentriodIterMethod
    {
        private int n;

        private static CentriodIterMethod instance = null;

        protected CentriodIterMethod(int n)
        {
            this.n = n;
        }

        public static CentriodIterMethod getInstance()
        {
            if (instance == null)
            {
                instance = new CentriodIterMethod(10000);
            }

            return instance;
        }

        public double getCrorCl(IMemFunc memFunc, bool ifCr = false)
        {
            double yQuote = step1ToAvr(memFunc);//Step 1

            bool test = true;
            while (test)
            {
                int e = step2FindE(memFunc, yQuote);//Step 2
                double yQQ = step3ForClOrCr(memFunc, e, ifCr);//Step 3

                if (yQQ == yQuote)
                    test = false; //Step 4
                else
                    yQuote = yQQ;//Step 5 and Repeat
            }

            return yQuote;
        }

        private double step1ToAvr(IMemFunc memFunc)
        {
            double num = 0.0;
            double den = 0.0;

            foreach (double step in memFunc.getElements(n))
            {
                double avrBetvenLandU = (memFunc.lowerMemFunction(step) + memFunc.upperMemFunction(step)) / 2;
                num = num + step * avrBetvenLandU;
                den += avrBetvenLandU;
            }

            return num / den;
        }
        
        private int step2FindE(IMemFunc memFunc, double yQuote)
        {
            int yeCount = 1;
            foreach (double step in memFunc.getElements(n))
            {
                double currentNumber = memFunc.getElements(n)[yeCount];
                if (yQuote < step && yQuote < currentNumber)
                    break;
                yeCount++;
            }
            return yeCount;
        }

        private double step3ForClOrCr(IMemFunc memFunc, int e, bool ifCr = false)
        {
            double num = 0.0;
            double den = 0.0;

            int z = 1;
            foreach (double step in memFunc.getElements(n))
            {
                double avrBetvenLandU = (memFunc.lowerMemFunction(step) + memFunc.upperMemFunction(step)) / 2;
                double deltaBetvenLandU = (memFunc.lowerMemFunction(step) - memFunc.upperMemFunction(step)) / 2;

                if (ifCr)
                {
                    int local = e;
                    e = z;
                    z = local;
                }

                double numberDipendOnClOrCr = 0;
                if (z <= e)
                    numberDipendOnClOrCr = avrBetvenLandU + deltaBetvenLandU;
                else
                    numberDipendOnClOrCr = avrBetvenLandU - deltaBetvenLandU;

                num += step * avrBetvenLandU;
                den += avrBetvenLandU;
                z++;
            }

            return num / den;
        }
        
    }
}