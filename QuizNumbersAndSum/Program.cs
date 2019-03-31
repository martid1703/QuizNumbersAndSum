using System;
using System.Reflection;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Threading;

namespace QuizNumbersAndSum
{
    class Program
    {
        static void Main(string[] args)
        {
            int from = 1;
            int to = 200;
            int size = 1000;

            // setup initial array parameters
            if (args.Length == 3)
            {
                if (!Int32.TryParse(args[0], out from))
                {
                    throw new ArgumentException("Cannot parse int value from arguments!");
                }

                if (!Int32.TryParse(args[1], out to))
                {
                    throw new ArgumentException("Cannot parse int value from arguments!");
                }

                if (!Int32.TryParse(args[2], out size))
                {
                    throw new ArgumentException("Cannot parse int value from arguments!");
                }

            }

            CancellationTokenSource cts = new CancellationTokenSource();
            Thread tesingThread = new Thread(() => StartTesting(from, to, size, cts.Token));
            tesingThread.IsBackground = true;
            tesingThread.Start();

            Console.WriteLine("If you want to terminate program immediately type 'q' and hit Enter:");
            // await for the testing thread, also allows to finish program gracefully if tired of waiting


            // update progress bar here or smth similar

            string reply = Console.ReadLine();
            if (reply.Length > 0)
            {
                if (String.Compare(reply, "q") == 0)
                {
                    Console.WriteLine("Program has been terminated by the user.");
                    cts.Cancel();
                }
            }

            Console.WriteLine("Program has ended. Good bye.");
        }

        private static void StartTesting(int from, int to, int size, CancellationToken ct)
        {
            ct.Register(() =>
            {
                return;
            });

            string logPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string logName = "log.txt";

            // clear log from previous run info
            File.WriteAllText(Path.Combine(logPath, logName), string.Empty);

            LogWriter logWriter = new LogWriter(logPath, logName);

            int totalLoops = 3;
            int currentLoops = 0;
            List<TimeSpan> timeSpanAlg1 = new List<TimeSpan>();
            List<TimeSpan> timeSpanAlg2 = new List<TimeSpan>();
            List<string> resultsAlg1 = new List<string>();
            List<string> resultsAlg2 = new List<string>();

            Random rnd = new Random();
            for (int i = 0; i < totalLoops; i++)
            {
                // call algorithms with different types of incoming arrays: natural series or random series
                logWriter.Write($"Start generating numbers: {DateTime.Now.ToLongTimeString()}");
                
                //StartConditions.SeriesKind seriesKind = 
                    //(StartConditions.SeriesKind)rnd.Next(0, Enum.GetNames(typeof(StartConditions.SeriesKind)).Length);
                StartConditions.SeriesKind seriesKind = (StartConditions.SeriesKind)i;

                Console.WriteLine($"\n===========================================");
                Console.WriteLine($"\nRun number {i+1}. Series kind: {seriesKind}\n");

                currentLoops++;

                // testing algorithms
                logWriter.Write($"Start testing algorithms: {DateTime.Now.ToLongTimeString()}");
                    Testing.TestAlgorithms(from, to, size, seriesKind, logWriter, currentLoops, timeSpanAlg1, timeSpanAlg2, resultsAlg1, resultsAlg2, ct);
            }

            Console.WriteLine($"\n================================");
            Console.WriteLine($"\nAlgorithm testing has completed. Press any key to exit");
            Console.WriteLine($"================================");
        }

        private static string PrintArray(int[] numbers)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < numbers.Length; i++)
            {
                sb.Append($"{numbers[i]},");
            }
            return sb.ToString();
        }
    }
}
