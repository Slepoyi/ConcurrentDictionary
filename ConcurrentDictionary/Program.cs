using ConcurrentDictionary.Concurrent;
using ConcurrentDictionary.Validation;
using System;
using System.Threading.Tasks;

namespace ConcurrentDictionary
{
    class Program
    {
        static void Main(string[] args)
        {
            int odd = 1;
            int even = 0;
            int divisionFactor = 5;
            int sleepDuration = 100;

            Console.WriteLine("Enter the number n:");

            if (!int.TryParse(Console.ReadLine(), out int n))
            {
                Console.WriteLine("Something is wrong with your n.");
                return;
            }

            DictionaryWrapper concurrentDictionaryWrapper = new DictionaryWrapper();

            DictionaryWriter concurrentWriter1 = new DictionaryWriter(n, odd, "WS1", sleepDuration, concurrentDictionaryWrapper);
            DictionaryWriter concurrentWriter2 = new DictionaryWriter(n, even, "WS2", sleepDuration, concurrentDictionaryWrapper);

            DictionaryReader reader = new DictionaryReader(n, divisionFactor, sleepDuration, concurrentDictionaryWrapper);

            Task[] tasks = new Task[3];

            tasks[0] = Task.Factory.StartNew(() => concurrentWriter1.ConcurrentWriteToDict());
            tasks[1] = Task.Factory.StartNew(() => concurrentWriter2.ConcurrentWriteToDict());
            tasks[2] = Task.Factory.StartNew(() => reader.Read());

            Task.WaitAll(tasks);

            DictionaryValidator validator = new DictionaryValidator();

            if (validator.ValidateDictionary(concurrentDictionaryWrapper, n, divisionFactor))
                Console.WriteLine("Seems ok.");
            else
                Console.WriteLine("Something is wrong.");

            Console.ReadKey();
        }

        public static async void AddPairToDictionary(int firstPairKey, int secondPairKey, int count, DictionaryWrapper concurrentDictionaryWrapper)
        {
            await Task.Factory.StartNew(() => concurrentDictionaryWrapper.TryAdd(count + firstPairKey + secondPairKey, 
                string.Format(@"$MS - {0} + {1}", firstPairKey, secondPairKey))
            );
        }
    }
}
