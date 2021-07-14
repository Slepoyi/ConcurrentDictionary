﻿using ConcurrentDictionary.ConcurrentDict;
using ConcurrentDictionary.ConcurrentWrite.Classes;
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
            int sleepDuration = 100;

            while (true)
            {
                Console.WriteLine("Enter the number n:");

                if (!int.TryParse(Console.ReadLine(), out int n))
                {
                    Console.WriteLine("Something is wrong with your n. Please repeat.");
                    continue;
                }

                ConcurrentDictionaryWrapper concurrentDictionaryWrapper = new ConcurrentDictionaryWrapper();

                ConcurrentWriter concurrentWriter1 = new ConcurrentWriter(n, odd, "WS1", sleepDuration);
                ConcurrentWriter concurrentWriter2 = new ConcurrentWriter(n, even, "WS2", sleepDuration);

                Reader reader = new Reader(n, 5, sleepDuration);

                reader.Notify += AddPairToDictionary;

                Task[] tasks = new Task[3];

                tasks[0] = Task.Factory.StartNew(() => concurrentWriter1.ConcurrentWriteToDict(concurrentDictionaryWrapper));
                tasks[1] = Task.Factory.StartNew(() => concurrentWriter2.ConcurrentWriteToDict(concurrentDictionaryWrapper));
                tasks[2] = Task.Factory.StartNew(() => reader.Read(concurrentDictionaryWrapper));

                Task.WaitAll(tasks);

                Console.WriteLine("Ok!");
                Console.ReadKey();
            }
        }

        private static void AddPairToDictionary(int signalKey, ConcurrentDictionaryWrapper concurrentDictionaryWrapper)
        {
            concurrentDictionaryWrapper.TryAdd(concurrentDictionaryWrapper.Count(), string.Format(@"MS - {0} + {1}", signalKey - 1, signalKey - 2));
        }
    }
}