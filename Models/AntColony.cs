using System.Linq;

namespace RiskScore.Models
{
    internal class AntColonyPheromone
    {
        // pheromone increase factor
        private static double Q0 = 20.0;
        private static double Q1 = 11.0;
        private static double Q2 = 6.0;

        //user make 3 choise for one item
        private int numCities;
        //user have 10 variant per choise
        private const int numVar = 10;

        public int[] Calculation(int[][] ants1)
        {
            numCities = ants1.Length;
            int[][] ants = new int[numCities][];

            ants[0] = new int[] { 6, 5, 7 };
            ants[1] = new int[] { 6, 7, 5 };
            ants[2] = new int[] { 5, 7, 7 };
            double[][] pheromones = InitPheromones(numCities, numVar);

            for (int i = 0; i < ants1.Length; i++)
            {
                UpdatePheromones(pheromones, getColumn(ants1, i));
            }
            return BestTrail(pheromones);
        }

        public static T[] getColumn<T>(T[][] array, int index)
        {
            T[] column = new T[array[0].Length]; // Here I assume a rectangular 2D array! 
            for (int i = 0; i < column.Length; i++)
            {
                column[i] = array[index][i];
            }
            return column;
        }

        private static void UpdatePheromones(double[][] pheromones, int[] ants)
        {
            for (int k = 0; k < ants.Length; k++)
            {
                int val = ants[k] - 1;
                pheromones[k][val] += Q0;

                if (ants[k] - 1 > 0)
                    pheromones[k][val - 1] += Q1;
                if (ants[k] + 1 < 10)
                    pheromones[k][val + 1] += Q1;

                if (ants[k] - 2 > 0)
                    pheromones[k][val - 2] += Q2;
                if (ants[k] + 2 < 10)
                    pheromones[k][val + 2] += Q2;
            }
        }

        private int[] BestTrail(double[][] pher)
        {
            int[] bestTrail = new int[numCities];

            for (int i = 0; i < numCities; i++)
            {
                var col = getColumn(pher, i);
                double maxValue = col.Max();
                int maxIndex = col.ToList().IndexOf(maxValue);
                bestTrail[i] = maxIndex + 1;
            }

            return bestTrail;
        }

        private static double[][] InitPheromones(int numCities, int numVar)
        {
            double[][] pheromones = new double[numCities][];
            for (int i = 0; i <= numCities - 1; i++)
            {
                pheromones[i] = new double[numVar];
            }
            for (int i = 0; i <= pheromones.Length - 1; i++)
            {
                for (int j = 0; j <= pheromones[i].Length - 1; j++)
                {
                    pheromones[i][j] = 0.01;
                }
            }
            return pheromones;
        }
    }
}