using System;
using System.Linq;

namespace LambaTest
{
    class Program
    {
        private delegate int squareFunc(int i);
        private delegate int sumFunc(int i, int j);

        static void Main(string[] args)
        {
            squareFunc squared = x => x*x;
            int y = 5;
            int z = squared(5);
            Console.WriteLine($"{y} squred is equal to {z}");

            sumFunc sum = (a,b) => a + b;
            int x1 = 1;
            int x2 = 3;
            int x3 = sum(x1, x2);
            Console.WriteLine($"The sum of {x1} and {x2} is {x3}");

            int[] numbers = {1,2,3,4,5,6,7,8,9,10};
            int odds = numbers.Count(n => n % 2  == 0);
            var oddsSet = numbers.Where(n => n > 5).ToArray();
            Console.WriteLine($"Number of odds in set is {odds}");

        }
    }
}
