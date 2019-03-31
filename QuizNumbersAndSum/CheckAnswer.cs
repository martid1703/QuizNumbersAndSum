using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizNumbersAndSum
{
    class CheckAnswer
    {
        internal static string Check(int[] numbers, int sum, string algName, string resultStr, object answerObj, TimeSpan timeSpan, List<string> results)
        {
            try
            {
                Tuple<int, int> answer = answerObj as Tuple<int, int>;
                if (answer == null)
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
    }
}
