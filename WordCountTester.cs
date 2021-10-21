using System;
using System.Collections.Generic;
namespace Lab3
{
    public class WordCountTester
    {
        static void Main()
        {
            // Generate a list class of strings, then run each through the WCTester function
            List<string> testList = new List<string>();

            // #1: Strings and words of different lengths (basic test)
            testList.Add("Nothing but snake eyes when I roll the dice"); // Expected = 9
            testList.Add("    "); // Expected = 0
            testList.Add("a b c d e f"); // Expected = 6
            testList.Add("aaaaaaaaaaa bbbbbbbbbbbbb ccccccccccccccc"); // Expected = 3

            // #2: Spaces at front of and in between words 
            testList.Add("    I like pizza"); // Expected = 3
            testList.Add("I         like       pizza        "); // Expected = 3


            // #3: Starting index occurring on a white space
            testList.Add("I like pizza"); // startIdx will be 1 (on the white space), expected = 3

            // #4: Special symbols
            testList.Add("@#*& !@()# #!^@! #@#&%"); // Expected = 4

            try
            {

                // For loop assigning the expected and start_idx values for each test case in the list
                for (int i = 0; i < 8; i++)
                {
                    int expected = 3;
                    int startIndex = 0;
                    if (i == 0)
                        expected = 9;
                    if (i == 1)
                        expected = 0;
                    if (i == 2)
                        expected = 6;
                    if (3 <= i && i <= 6)
                    {
                        if (i == 6)
                            startIndex = 1;
                        expected = 3;
                    }
                    if (i == 7)
                        expected = 4;
                    WCTester(testList[i], startIndex, expected);
                }

            }
            catch (UnitTestException e)
            {
                Console.WriteLine(e);
            }

        }


        /**
         * Tests word_count for the given line and starting index
         * @param line line in which to search for words
         * @param start_idx starting index in line to search for words
         * @param expected expected answer
         * @throws UnitTestException if the test fails
         */
        static void WCTester(string line, int start_idx, int expected)
        {
            int result = HelperFunctions.WordCount(ref line, start_idx);

            if (result != expected)
            {
                throw new Lab3.UnitTestException(ref line, start_idx, result, expected, String.Format("UnitTestFailed: result:{0} expected:{1}, line: {2} starting from index {3}", result, expected, line, start_idx));
            }
            else
                Console.WriteLine("{0} has passed the test! Result: {1}, Expected: {2}", line, result, expected);

        }
    }
} 