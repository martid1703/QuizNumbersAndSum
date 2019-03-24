using BST_MSDN;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DmitryAlgorithm
{
    class DmitryAlgorithm : IQuizAlgorithm
    {
        IndexValueRemainder a;
        IndexValueRemainder b;
        int sum; // the sum that a.Value+b.Value should be equal to

        public DmitryAlgorithm()
        {
                
        }
        public override string ToString()
        {
            string answer;
            if (a==null || b==null)
            {
                answer = "Numbers not found.";
            }
            else
            {
                answer = $"Index A={a.index}, Index B={b.index}, {a.value}+{b.value}={sum}";
            }
            return answer;
        }

        public Tuple<int,int> GetIndexes(int[] numbers, int sum)
        {
            Tuple<int, int> answer=null;

            // Create new BST
            BinaryTree<IndexValueRemainder> bst = new BinaryTree<IndexValueRemainder>();

            for (int i = 0; i < numbers.Length; i++)
            {
                IndexValueRemainder ivr = new IndexValueRemainder(i, numbers[i], sum - numbers[i]);
                bst.Add(ivr);
            }

            while (bst.Count > 1)
            {
                if (CheckRootAndAnyNumber(bst, out a, out b))
                {
                    answer = new Tuple<int, int>(a.index, b.index);
                    return answer;
                }

                // if root number is not part of the solution, remove it from the tree and restart
                bst.Remove(bst.Root.Value);
            }

            return answer;
        }

        private bool CheckRootAndAnyNumber(BinaryTree<IndexValueRemainder> bst, out IndexValueRemainder a, out IndexValueRemainder b)
        {
            bool found;
            a = null;
            b = null;

            IndexValueRemainder nodeToCheck;

            // find a node in the bst, whose value equals to root.remainder value, as per conditions of the quiz
            IndexValueRemainder remainder = new IndexValueRemainder(0, bst.Root.Value.remainder, 0);

            found = bst.Contains(remainder,out nodeToCheck);

            // need to check so that indexes do not match - condition of the quiz
            if (found && bst.Root.Value.index!=nodeToCheck.index)
            {
                a = bst.Root.Value;
                b = nodeToCheck;
            }

            return found;
        }
    }
}
