using System;
using System.Collections.Generic;
using System.Threading;
using System.Linq;


namespace Lab3
{
    public class HelperFunctions
    {
        /**
         * Counts number of words, separated by spaces, in a line.
         * @param line string in which to count words
         * @param start_idx starting index to search for words
         * @return number of words in the line
         */
        public static int WordCount(ref string line, int start_idx)
        {
            // If item at starting index is not a white space, wordcount starts at 1 to factor in starting word
            int count = 1;
            // Else if it starts on a white space, wordcount will start at 0
            if (Char.IsWhiteSpace(line[0])) 
                count = 0;
            for (int i = start_idx; i < line.Length; i++)
            {
                // If a white space is encountered at index i, only increment wordcount if the element (i + 1) is a character/start of a word 
                if (Char.IsWhiteSpace(line[i]))
                {
                    // To prevent "index out of bound errors", ensure that (i + 1) is within the bounds of the string length
                    if ((i + 1) < line.Length)
                    {
                        if (!Char.IsWhiteSpace(line[i + 1]))
                            count++;
                    }
                }
            }
            return count;
        }
        /**
    * Reads a file to count the number of words each actor speaks.
    *
    * @param filename file to open
    * @param mutex mutex for protected access to the shared wcounts map
    * @param wcounts a shared map from character -> word count
    */
        public static void CountCharacterWords(string filename, Mutex mutex, Dictionary<string, int> wcounts)
        {

            //===============================================
            //  IMPLEMENT THIS METHOD INCLUDING THREAD SAFETY
            //===============================================

            string line;  // for storing each line read from the file
            string character = "";  // empty character to start
            System.IO.StreamReader file = new System.IO.StreamReader(filename);

            while ((line = file.ReadLine()) != null)
            {
                //=================================================
                // YOUR JOB TO ADD WORD COUNT INFORMATION TO MAP
                //=================================================
                // Is the line a dialogueLine?
                //    If yes, get the index and the character name.
                //    if index > 0 and character not empty
                //       get the word counts
                //          if the key exists, update the word counts
                //          else add a new key-value to the dictionary
                //    reset the character

                int isDialogueIndex = IsDialogueLine(line, ref character);
                int charwcount;

                if (isDialogueIndex != -1)
                {
                    if (isDialogueIndex > 0 && character != " ")
                    {
                        mutex.WaitOne();
                        charwcount = WordCount(ref line, isDialogueIndex);
                        if (wcounts.ContainsKey(character))
                        {
                            wcounts[character] += charwcount;
                        }
                        else
                        {
                            // If the key doesn't exist, add the character to the dictionary and its value
                            wcounts.Add(character, charwcount);
                        }
                        mutex.ReleaseMutex();
                    }
                }
                // Reset character
                character = " ";
            }
            // Close file
            file.Close();
        }



        /**
         * Checks if the line specifies a character's dialogue, returning
         * the index of the start of the dialogue.  If the
         * line specifies a new character is speaking, then extracts the
         * character's name.
         *
         * Assumptions: (doesn't have to be perfect)
         *     Line that starts with exactly two spaces has
         *       CHARACTER. <dialogue>
         *     Line that starts with exactly four spaces
         *       continues the dialogue of previous character
         *
         * @param line line to check
         * @param character extracted character name if new character,
         *        otherwise leaves character unmodified
         * @return index of start of dialogue if a dialogue line,
         *      -1 if not a dialogue line
         */
        static int IsDialogueLine(string line, ref string character)
        {

            // new character
            if (line.Length >= 3 && line[0] == ' '
                && line[1] == ' ' && line[2] != ' ')
            {
                // extract character name

                int start_idx = 2;
                int end_idx = 3;
                while (end_idx <= line.Length && line[end_idx - 1] != '.')
                {
                    ++end_idx;
                }

                // no name found
                if (end_idx >= line.Length)
                {
                    return 0;
                }

                // extract character's name
                character = line.Substring(start_idx, end_idx - start_idx - 1);
                return end_idx;
            }

            // previous character
            if (line.Length >= 5 && line[0] == ' '
                && line[1] == ' ' && line[2] == ' '
                && line[3] == ' ' && line[4] != ' ')
            {
                // continuation
                return 4;
            }

            return 0;
        }

        /**
         * Sorts characters in descending order by word count
         *
         * @param wcounts a map of character -> word count
         * @return sorted vector of {character, word count} pairs
         */
        public static List<Tuple<int, string>> SortCharactersByWordcount(Dictionary<string, int> wordcount)
        {
            List<Tuple<int, string>> sortedByValueList = new List<Tuple<int, string>>();

            // Implement sorting by word count here
            foreach (KeyValuePair<string, int> pairItem in wordcount.OrderByDescending(key => key.Value)) {
                sortedByValueList.Add(Tuple.Create(pairItem.Value, pairItem.Key));
            }

            return sortedByValueList;
        }


        /**
         * Prints the List of Tuple<int, string>
         *
         * @param sortedList
         * @return Nothing
         */
        public static void PrintListofTuples(List<Tuple<int, string>> sortedList)
        {
            // Implement printing here
            sortedList.ForEach(Console.WriteLine);
        }
    }
}


    



    