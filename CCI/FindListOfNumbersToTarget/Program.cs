using System;
using System.Collections.Generic;
using System.Linq;

class Program
{
  static void Main()
  {
    List<decimal> numbers = new List<decimal>
        {
            //69.98M, 
      //16.80M, 7.16M, 
      //-24.40M,
      6.96M, 5.80M, 4.04M, 3.95M,
            1.80M, 0.80M, 0.20M, 0.14M,
      //105.98M,
      23.98M, 
      //-40.00M,
      6.96M,
            1.52M, 1.48M
        };

    decimal target = 50.45M;
    long factorial = getFactorial(numbers.Count());
    Console.WriteLine("Factorial of " + numbers.Count().ToString() + " is " + factorial.ToString());
    IEnumerable<IEnumerable<decimal>> list = permutations(numbers);
    Console.WriteLine(list.Count());
    Console.WriteLine("Press return to continue...");
    Console.ReadLine();
  }
  public static long getFactorial(int nbr)
  {
    long factorial = 1;
    for (int i = 1; i <= nbr; i++)
      factorial *= i;
    return factorial;
  }
  public static IEnumerable<IEnumerable<decimal>> permutations(List<decimal> numbers)
  {
    if (numbers.Count == 1)
    {
      yield return numbers;
    }
    else
    {
      for (int i = 0; i < numbers.Count; i++)
      {
        // Remove the current number from the list.
        var remainingNumbers = new List<decimal>(numbers);
        remainingNumbers.RemoveAt(i);

        // Get all permutations of the remaining numbers.
        foreach (var permutation in permutations(remainingNumbers))
        {
          // Yield the current number followed by the permutation of the remaining numbers.
          yield return permutation.Prepend(numbers[i]);
        }
      }
    }
  }

}

