using System.Collections.Generic;
using System;
using System.Threading.Tasks;

namespace SieveToolkit
{
    public static class Algorithms
    {
        public static bool IsPrime(int num)
        {
            if (num == 1)
            {
                return false;
            }
            if (num < 4)
            {
                return true;
            }
            foreach (int n in PrimeInRange(0, Convert.ToInt32(Math.Floor(Convert.ToDouble(Math.Sqrt(num))))))
            {
                if (num % n == 0)
                {
                    return false;
                }
            }
            return true;
        }
        public static List<int> PrimeInRange(int start, int end)
        {
            if (end - start > 1000 && start > 3)
            {
                List<Task<List<int>>> Tasks = new List<Task<List<int>>>();
                List<int> Segments = new List<int>();
                for (int i = start; i <= end; i += 1000)
                {
                    Segments.Add(i);
                }
                for (int i = 0; i < Segments.Count - 1; i++)
                {
                    int first = Segments[i];
                    int second = Segments[i + 1];
                    Tasks.Add(Task.Run<List<int>>(() => PrimeInRange(first, second)));
                }
                var results = Task.WhenAll<List<int>>(Tasks).Result;
                List<int> PrimeNums = new List<int>();
                foreach (var list in results)
                {
                    foreach (var num in list)
                    {
                        PrimeNums.Add(num);
                    }
                }
                return PrimeNums;
            }
            int n = start > 2 ? Convert.ToInt32(Math.Sqrt(end)) : end;
            bool[] sieve = new bool[n + 1];
            for (int i = 0; i < n + 1; i++)
            {
                sieve[i] = i % 2 != 0;
            }
            sieve[2] = true;
            for (int i = 3; i < Math.Sqrt(sieve.Length); i += 2)
            {
                if (sieve[i])
                {
                    for (int j = i * i; j < sieve.Length; j += i * 2)
                    {
                        sieve[j] = false;
                    }
                }
            }
            List<int> primes = new List<int>();
            for (int i = 2; i < sieve.Length; i++)
            {
                if (sieve[i])
                {
                    primes.Add(i);
                }
            }
            if (start > 2)
            {
                List<int> All = new List<int>();
                for (int i = start % 2 == 0 ? start + 1 : start; i < end; i += 2)
                {
                    All.Add(i);
                }
                foreach (var prime in primes)
                {
                    int x = Convert.ToInt32(start / prime);
                    int y = x % 2 == 0 ? x + 1 : x;
                    for (int i = Math.Max(y, prime); i <= end / prime; i += 2)
                    {
                        All.Remove(i * prime);
                    }
                }
                primes = All;
            }
            return primes;
        }
        public static List<int> AllDivisors(int n)
        {
            List<int> Divisors = new List<int>();
            for (int i = 1; i <= Math.Sqrt(n); i++)
            {
                if (n % i == 0)
                {
                    if (n / i == i)
                        Divisors.Add(i);
                    else
                    {
                        Divisors.Add(i);
                        Divisors.Add(n / i);
                    }
                }
            }
            Divisors.Sort();
            return Divisors;
        }
        public static int FirstRemove(int start, int end)
        {
            if (start <= 4)
            {
                return 4;
            }
            for (int i = start; i <= end; i++)
            {
                if (!IsPrime(i))
                {
                    return i;
                }
            }
            return -1;
        }
        public static int RemovedBy(int num)
        {
            if (IsPrime(num))
            {
                return -1;
            }
            return AllDivisors(num)[1];
        }
        public static int NthNumberRemovedBy(int prime, int n, int start)
        {
            int result = 0;
            int count = 0;
            List<int> PrimesBefore = PrimeInRange(0, prime);
            PrimesBefore.Remove(prime);
            for (int i = Math.Max(prime, (int)Math.Ceiling((double)start / (double)prime)); true; i++)
            {
                foreach (var Prime in PrimesBefore)
                {
                    if ((i * prime) % Prime == 0)
                    {
                        goto SKIP;
                    }
                }
                result = i * prime;
                count++;
                if (count == n)
                {
                    return result;
                }
            SKIP:
                ;
            }
        }
        public static int NumIsNthNumberRemoved(int num, int start, int end)
        {
            if (num % 2 == 0)
            {
                if (start <= 4)
                {
                    return (num / 2) - 1;
                }
                return (num / 2) - ((int)Math.Ceiling((double)start / 2) - 1);
            }
            int count = 0;
            int n = start > 2 ? Convert.ToInt32(Math.Sqrt(end)) : end;
            bool[] sieve = new bool[n + 1];
            for (int i = 0; i < n + 1; i++)
            {
                sieve[i] = i % 2 != 0;
            }
            sieve[2] = true;
            for (int i = 3; i < Math.Sqrt(sieve.Length); i += 2)
            {
                if (sieve[i])
                {
                    for (int j = i * i; j < sieve.Length; j += i * 2)
                    {
                        sieve[j] = false;
                    }
                }
            }
            List<int> primes = new List<int>();
            for (int i = 3; i < sieve.Length; i++)
            {
                if (sieve[i])
                {
                    primes.Add(i);
                }
            }
            List<int> All = new List<int>();
            for (int i = start % 2 == 0 ? start + 1 : start; i < end; i += 2)
            {
                All.Add(i);
            }
            foreach (var prime in primes)
            {
                int x = Convert.ToInt32(start / prime);
                int y = x % 2 == 0 ? x + 1 : x;
                for (int i = Math.Max(y, prime); i <= end / prime; i += 2)
                {
                    All.Remove(i * prime);
                    count++;
                    if (i * prime == num)
                    {
                        return count + (int)Math.Floor((double)(end - start) / 2);
                    }
                }
            }
            return -1;
        }
    }
}
