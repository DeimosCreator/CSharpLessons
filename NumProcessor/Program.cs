using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NumProcessor
{
    public class NumberProcessor
    {
        private double _result;

        public double Result
        {
            get => _result; set => _result = value;
        }

        public double CalculateSquare(in double number) => number * number;

        public void DivideNumbers(double dividend, double divisor, out double quotient)
        {
            quotient = divisor != 0 ? dividend / divisor : double.NaN;
        }


        public void AccumulateResult(ref double value)
        {
            _result += value;
            value = _result;
        }

        public NumberProcessor(double initialResult)
        {
            Result = initialResult;
        }
    }
    public class Program
    {
        static void Main()
        {
        }
    }
}
