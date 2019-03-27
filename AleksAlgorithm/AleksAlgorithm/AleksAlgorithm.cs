using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AleksAlgorithm
{
    class AleksAlgorithm : IQuizAlgorithm
    {
        public List<int> numbers { get; private set; }
        public Dictionary<int, int> valuesAndIndices { get; private set; }
        private void CreateDictionary(List<int> Numbers)
        {
            this.numbers = Numbers;

            // preprocessing O(n)
            valuesAndIndices = Numbers
                .Select((v, i) => new { value = v, index = i })
                .ToDictionary(k => k.value, v => v.index);
        }

        public Tuple<int, int> GetIndexes(int[] numbers, int sum)
        {
            CreateDictionary(numbers.ToList<int>());
            {
                // search, also O(n)
                for (var i = 0; i < this.numbers.Count; ++i)
                {
                    var number1 = this.numbers[i];
                    var toFind = sum - number1;
                    if (valuesAndIndices.TryGetValue(toFind, out var j))
                        return new Tuple<int, int>(i,j);
                }

                // no solution found
                return new Tuple<int, int>(-1,-1); // or throw exception
            }
        }
    }
}
