using System;

using Ex = System.Linq.Expressions.Expression;

namespace Boomers.Utilities.Linq.Extensions
{
    /// <summary>
    /// Based on the works of Keith Farmer and Jafar Husain:
    /// http://www.codeproject.com/KB/cs/genericoperators.aspx
    /// http://themechanicalbride.blogspot.com/2008/04/using-operators-with-generics.html
    /// </summary>
    internal struct NumberWrapper<T> where T : struct
    {
        private static Func<T, T, T> addition, subtraction, multiplication, division;

        static NumberWrapper()
        {
            Type type = typeof(T);
            var left = Ex.Parameter(type, "left");
            var right = Ex.Parameter(type, "right");

            addition = Ex.Lambda<Func<T, T, T>>(Ex.Add(left, right), left, right).Compile();
            subtraction = Ex.Lambda<Func<T, T, T>>(Ex.Subtract(left, right), left, right).Compile();
            multiplication = Ex.Lambda<Func<T, T, T>>(Ex.Multiply(left, right), left, right).Compile();
            division = Ex.Lambda<Func<T, T, T>>(Ex.Divide(left, right), left, right).Compile();
        }

        private T value;

        public NumberWrapper(T value)
        {
            this.value = value;
        }

        public static T operator +(NumberWrapper<T> left, NumberWrapper<T> right)
        {
            return addition(left.value, right.value);
        }

        public static T operator -(NumberWrapper<T> left, NumberWrapper<T> right)
        {
            return subtraction(left.value, right.value);
        }

        public static T operator *(NumberWrapper<T> left, NumberWrapper<T> right)
        {
            return multiplication(left.value, right.value);
        }

        public static T operator /(NumberWrapper<T> left, NumberWrapper<T> right)
        {
            return division(left.value, right.value);
        }
    }
}
