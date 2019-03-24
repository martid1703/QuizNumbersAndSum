using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DmitryAlgorithm
{
    class IndexValueRemainder : IComparable
    {
        public int index { get; private set; }// index of number in array
        public int value { get; private set; }// value of number
        public int remainder { get; private set; }// remainder between given sum and this number value

        public IndexValueRemainder(int index, int value, int remainder)
        {
            this.index = index;
            this.value = value;
            this.remainder = remainder;
        }

        public int CompareTo(object obj)
        {
            IndexValueRemainder ivr = obj as IndexValueRemainder;
            if (ivr == null)
            {
                throw new ArgumentException($"Cannot compare with object which is not of type {this.GetType()}");
            }
            return value.CompareTo(ivr.value);
        }
    }
}
