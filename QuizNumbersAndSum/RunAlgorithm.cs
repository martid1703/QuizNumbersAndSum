using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace QuizNumbersAndSum
{
    class RunAlgorithm
    {

        // Runs inside new thread for this algorithm.
        internal static void Run(int[] numbers, int sum, string algName, string mainMethodName, List<TimeSpan> algTimeSpan, List<string> results, LogWriter logWriter)
        {
            //string numbersString = PrintArray(numbers);
            string resultStr = "";//=$"\nArray of numbers: {numbersString}";
            //resultStr += $"\nSum to match = {sum}";
            DateTime start = DateTime.Now;
            DateTime finish = DateTime.Now;
            object algInstance = null;
            object answerObj = null;

            try
            {
                //todo: find assembly by interface, not by name
                Assembly assembly = null;

                string assemblyLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                string assemblyLoadFrom = Path.Combine(assemblyLocation, algName + ".dll");
                assembly = Assembly.LoadFrom(assemblyLoadFrom);

                Type classType = assembly.GetType(algName + "." + algName);
                algInstance = Activator.CreateInstance(classType);
                MethodInfo mi = classType.GetMethod(mainMethodName);

                start = DateTime.Now;
                // start algorithm
                answerObj = mi.Invoke(algInstance, new object[] { numbers, sum });

            }
            catch (Exception e)
            {
                string errorMsg = $"Cannot run algoritm \"{algName}\", \nException message: {e.InnerException.Message}";
                //Console.WriteLine(errorMsg);
                Exception algRunException = new Exception(errorMsg, e.InnerException);
                throw algRunException;
            }
            finally
            {
                finish = DateTime.Now;
                TimeSpan algRunTime = finish - start;
                algTimeSpan.Add(algRunTime);

                // check if answer provided by alogrithm is correct
                resultStr = CheckAnswer.Check(numbers, sum, algName, resultStr, answerObj, algRunTime, results);

                logWriter.Write(resultStr);
                Console.Write(resultStr);
            }
        }
    }
}
