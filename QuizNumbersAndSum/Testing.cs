using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace QuizNumbersAndSum
{
    class Testing
    {
        private static void Handler(Exception ex)
        {
            Console.WriteLine($"Exception happened: {ex.Message}");

        }

        private static void SafeExecute(Action test, Action<Exception> handler)
        {
            try
            {
                test.Invoke();
            }
            catch (Exception ex)
            {
                Handler(ex);
            }
        }


        internal static void TestAlgorithms(int from, int to, int size, StartConditions.SeriesKind seriesKind, LogWriter logWriter, int currentLoops, List<TimeSpan> timeSpanAlg1, List<TimeSpan> timeSpanAlg2, List<string> resultsAlg1, List<string> resultsAlg2, CancellationToken ct)
        {
            int[] numbers = null;


            switch (seriesKind)
            {
                case StartConditions.SeriesKind.naturalSeries:
                    numbers = StartConditions.GenerateNaturalSeries(from, to, size);
                    break;
                case StartConditions.SeriesKind.randomSeries:
                    numbers = StartConditions.GenerateRandomSeries(from, to, size);
                    break;
                case StartConditions.SeriesKind.shuffledNaturalSeries:
                    numbers = StartConditions.GenerateShuffledNaturalSeries(from, to, size);
                    break;
                default:
                    break;
            }
            // Create array and sum
            int sum = StartConditions.GenerateSum(from, to, size);

            // assembly and class name are supposed to be the same
            const string alg1Name = "DmitryAlgorithm";
            const string alg2Name = "AleksAlgorithm";
            const string mainMethodName = "GetIndexes";

            Thread thread1 = new Thread(() =>
                SafeExecute(() =>
                    RunAlgorithm.Run(numbers, sum, alg1Name, mainMethodName, timeSpanAlg1, resultsAlg1, logWriter),
                    Handler));
            thread1.IsBackground = true;

            Thread thread2 = new Thread(() =>
                SafeExecute(() =>
                    RunAlgorithm.Run(numbers, sum, alg2Name, mainMethodName, timeSpanAlg2, resultsAlg2, logWriter),
                    Handler));
            thread2.IsBackground = true;

            void Handler(Exception ex)
            {
                Console.WriteLine($"Exception happened at TestAlgorithm: {ex.Message}");
            }

            // In case of exception thread is closed, but test method continues, just prints the message.
            thread1.Start();
            thread2.Start();
            thread1.Join();
            thread2.Join();

            string report = "";
            report += $"Timespans for algoritm {alg1Name} & {alg2Name}";
            for (int i = 0; i < currentLoops; i++)
            {
                report += $"\n{timeSpanAlg1[i].TotalMilliseconds} ms, Result: {resultsAlg1[i]}";
                report += $"\n{timeSpanAlg2[i].TotalMilliseconds} ms, Result: {resultsAlg2[i]}";
            }
            logWriter.Write(report);

        }
    }
}
