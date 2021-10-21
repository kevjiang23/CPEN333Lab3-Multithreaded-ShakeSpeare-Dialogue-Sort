using System;
using System.Collections.Generic;
using System.Threading;
using System.Diagnostics;

namespace Lab3
{
    class Program
    {
        static void Main(string[] args)
        {
            // map and mutex for thread safety
            Mutex mutex = new Mutex();
            Dictionary<string, int> wcountsSingleThread = new Dictionary<string, int>();
            Dictionary<string, int> wcountsMultiThread = new Dictionary<string, int>();
            List<Tuple<int, string>> singleThreadTupleList = new List<Tuple<int, string>>();
            List<Tuple<int, string>> multiThreadTupleList = new List<Tuple<int, string>>();

            var filenames = new List<string> {
                "../../data/shakespeare_antony_cleopatra.txt",
                "../../data/shakespeare_hamlet.txt",
                "../../data/shakespeare_julius_caesar.txt",
                "../../data/shakespeare_king_lear.txt",
                "../../data/shakespeare_macbeth.txt",
                "../../data/shakespeare_merchant_of_venice.txt",
                "../../data/shakespeare_midsummer_nights_dream.txt",
                "../../data/shakespeare_much_ado.txt",
                "../../data/shakespeare_othello.txt",
                "../../data/shakespeare_romeo_and_juliet.txt",
           };

            //=============================================================
            // YOUR IMPLEMENTATION HERE TO COUNT WORDS IN SINGLE THREAD
            //=============================================================
            for (int i = 0; i < 10; i++) {
                HelperFunctions.CountCharacterWords(filenames[i], mutex, wcountsSingleThread);
            }
            singleThreadTupleList = HelperFunctions.SortCharactersByWordcount(wcountsSingleThread);
            HelperFunctions.PrintListofTuples(singleThreadTupleList);
            Console.WriteLine("SingleThread is Done!");
            //=============================================================
            // YOUR IMPLEMENTATION HERE TO COUNT WORDS IN MULTIPLE THREADS
            //=============================================================

            // Creating 10 threads, one for each play
            int numThreads = 10;
            Thread[] mainThread = new Thread[numThreads];

            for (int j = 0; j < numThreads; j++)
            {
                mainThread[j] = new Thread(() => HelperFunctions.CountCharacterWords(filenames[j], mutex, wcountsMultiThread));
                mainThread[j].Start();
            }
            for (int j = 0; j < numThreads; j++)
            {
                mainThread[j].Join();
            }

            multiThreadTupleList = HelperFunctions.SortCharactersByWordcount(wcountsMultiThread);
            HelperFunctions.PrintListofTuples(multiThreadTupleList);
            Console.WriteLine("MultiThread is Done!");
            return;
        }
    }
} 