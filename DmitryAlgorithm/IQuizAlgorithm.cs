using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DmitryAlgorithm
{
    interface IQuizAlgorithm
    {
        /// <summary>
        /// method to be called by the testing program using reflection.
        /// </summary>
        /// <param name="numbers"></param>
        /// <param name="sum"></param>
        Tuple<int,int> GetIndexes(int[] numbers, int sum);


        /// <summary>
        /// answer consisting of two indexes in numbers array {i1,i2} and result check value1+value2=sum
        /// </summary>
        /// <returns></returns>
        string ToString();
    }
}
