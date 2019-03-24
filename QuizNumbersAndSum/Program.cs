using System;
using System.Reflection;
using System.IO;
using System.Text;
using System.Collections.Generic;

namespace QuizNumbersAndSum
{
    class Program
    {
        static void Main(string[] args)
        {
            int from = 1;
            int to = 200;
            int size = 10000;

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

            string logPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string logName = "log.txt";

            // clear log from previous run info
            File.WriteAllText(Path.Combine(logPath, logName), string.Empty);

            LogWriter logWriter = new LogWriter(logPath, logName);

            Random rnd = new Random();
            for (int i = 0; i < 3; i++)
            {
                // call algorithms with different types of incoming arrays: natural series or random series
                logWriter.Write($"Start generating numbers: {DateTime.Now.ToLongTimeString()}");
                StartConditions.SeriesKind seriesKind = (StartConditions.SeriesKind)rnd.Next(1, 2);

                // testing algorithms
                logWriter.Write($"Start testing algorithms: {DateTime.Now.ToLongTimeString()}");
                TestAlgorithms(from, to, size,seriesKind, logWriter);
            }

        }

        private static void TestAlgorithms(int from, int to, int size, StartConditions.SeriesKind seriesKind, LogWriter logWriter)
        {
            int[] numbers=null;
            List<TimeSpan> timeSpanAlg1=new List<TimeSpan>();
            List<TimeSpan> timeSpanAlg2=new List<TimeSpan>();
            List<string> resultsAlg1 = new List<string>();
            List<string> resultsAlg2 = new List<string>();

            switch (seriesKind)
            {
                case StartConditions.SeriesKind.naturalSeries:
                    numbers = StartConditions.GenerateNaturalSeries(from, to, size);
                    break;
                case StartConditions.SeriesKind.randomSeries:
                    numbers = StartConditions.GenerateRandomSeries(from, to, size);
                    break;
                default:
                    break;
            }
            // Create array and sum
            int sum = StartConditions.GenerateSum(from, to, size);

            // assembly and class name are supposed to be the same
            const string alg1Name = "DmitryAlgorithm";
            const string alg2Name = "to be defined";
            const string mainMethodName = "GetIndexes";

            RunAlgorithm(numbers, sum, alg1Name, mainMethodName, timeSpanAlg1, resultsAlg1, logWriter);
            //RunAlgorithm(numbers, sum, alg2Name, mainMethodName, timeSpanAlg2, resultsAlg2, logWriter);

            string report = "";

            report+=$"Timespans for algoritm {alg1Name} & {alg2Name}";
            for (int i = 0; i < timeSpanAlg1.Count; i++)
            {
                report+= $"\n{timeSpanAlg1[i].TotalMilliseconds} ms, Result: {resultsAlg1[i]}";
                //report += $"\n{timeSpanAlg2[i].TotalMilliseconds} ms, Result: {resultsAlg2[i]}";
            }
                logWriter.Write(report);

        }

        private static void RunAlgorithm(int[] numbers, int sum, string algName, string mainMethodName, List<TimeSpan> algTimeSpan, List<string> results, LogWriter logWriter)
        {
            string numbersString = PrintArray(numbers);
            string resultStr=$"\nArray of numbers: {numbersString}";
            resultStr += $"\nSum to match = {sum}";

            try
            {
                //todo: find assembly by interface, not by name
                Assembly assembly = null;

                string assemblyLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                string assemblyLoadFrom = Path.Combine(assemblyLocation, algName + ".dll");
                assembly = Assembly.LoadFrom(assemblyLoadFrom);

                Type classType = assembly.GetType(algName + "." + algName);
                object alg = Activator.CreateInstance(classType);
                MethodInfo mi = classType.GetMethod(mainMethodName);

                DateTime start = DateTime.Now;
                // start algorithm
                object answerObj = mi.Invoke(alg, new object[] { numbers, sum });
                DateTime finish = DateTime.Now;
                TimeSpan algRunTime = finish - start;
                algTimeSpan.Add(algRunTime);

                // check if answer provided by alogrithm is correct
                resultStr = CheckAnswer(numbers, sum, algName, resultStr, answerObj, algRunTime, results);

                logWriter.Write(resultStr);
                Console.Write(resultStr);
            }
            catch (Exception e)
            {
                string errorMsg = $"Cannot run algoritm! Algorithm \"{algName}\", \nException message: {e.Message}";
                logWriter.Write(errorMsg);
                Console.WriteLine(errorMsg);
            }
        }

        private static string CheckAnswer(int[] numbers, int sum, string algName, string resultStr, object answerObj, TimeSpan timeSpan, List<string> results)
        {
            try
            {
                Tuple<int, int> answer = answerObj as Tuple<int,int>;
                if (answer==null)
                {
                    resultStr += $"\nAlgorithm \"{algName}\" run time is: {timeSpan.TotalMilliseconds} ms, answer is: No answer found!";
                    results.Add("No answer found");
                }
                else
                {
                    if (numbers[answer.Item1] + numbers[answer.Item2] != sum)
                    {
                        resultStr += $"\nAlgorithm \"{algName}\" run time is: {timeSpan.TotalMilliseconds} ms, answer is: Incorrect! Provided indexes are: {answer.Item1}, {answer.Item2} and values: {numbers[answer.Item1]}+{numbers[answer.Item2]}!={sum}";
                        results.Add("Incorrect");

                    }
                    else
                    {
                        resultStr += $"\nAlgorithm \"{algName}\" run time is: {timeSpan.TotalMilliseconds} ms, answer is: Correct! Provided indexes are: {answer.Item1}, {answer.Item2} and values: {numbers[answer.Item1]}+{numbers[answer.Item2]}=={sum}";
                        results.Add("Correct");

                    }
                }
                
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return resultStr;
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
