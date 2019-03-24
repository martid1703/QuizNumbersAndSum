using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizNumbersAndSum
{

    class StartConditions
    {
        public enum SeriesKind
        {
            naturalSeries=0,
            randomSeries=1
        }

        static Random rnd = new Random();

        // Generate array of random numbers
        public static int[] GenerateRandomSeries(int from, int to, int size)
        {
            int[] numbers = new int[size];
            for (int i = 0; i < size; i++)
            {
                numbers[i] = rnd.Next(from, to+1);
            }

            return numbers;
        }

        // Generate array of natural series like 1,2,3...10
        public static int[] GenerateNaturalSeries(int from, int to, int size)
        {
            int[] numbers = new int[size];
            for (int i = 0; i < size; i++)
            {
                numbers[i] = i+1;
            }

            return numbers;
        }

        // Generate sum we will be looking for as a sum of any 2 items in numbers array
        public static int GenerateSum(int from, int to, int size)
        {
            int sum = 0;
            sum = rnd.Next(from, to * 2);
            return sum;
        }
    }
}
