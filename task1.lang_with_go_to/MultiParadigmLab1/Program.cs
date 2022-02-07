using System;

namespace MultiParadigmLab1
{
    class Program
    {
        static void Main(string[] args)
        {
            // FIRST TASK
            string[] stopWords = { "for", "the", "in", "is", "a", "from" };
            int stopWordsCount = 6;
            int numberOfStopWordsFound = 0;

            int numberOfWordsToEstimate = 18; // !!! must be less than or equal to the number of words in a text 
            // string inputText = "africa White 1, tigers, live for live the, mostly in India, - white, Wild! - lions live mostly in africa, Africa!";
            string filepath = "lab1part1.txt";
            string inputText = System.IO.File.ReadAllText(filepath);

            string[] words = new string[numberOfWordsToEstimate];
            int numOfWordsCounter = 0;

            // variables for tracking words in input
            int wordBeginsIndex = 0;
            int wordEndsIndex = 0;

            bool foundABeginningOfAWord = false;
            bool isAStopWord = false;

            int numberOfFoundWords = 0;
            string wordBuilder = "";

            // THE ALGORITHM ITSELF
            int i = 0;
        wordsHunterLoopBody:
            inputText += " ";

            // trying to find a beginning of a new word
            if (!foundABeginningOfAWord && wordEndsIndex != 0)
            {
                int j = wordEndsIndex;
            searchingLoopBody:

                if ((inputText[j] > 64 && inputText[j] < 123) &&
                    ((inputText[j - 1] > 0 && inputText[j - 1] < 65) ||
                    (inputText[j - 1] > 90 && inputText[j - 1] < 97) || (inputText[j - 1] == '‘') || inputText[j - 1] == '—'))
                {
                    wordBeginsIndex = j;
                    foundABeginningOfAWord = true;
                    goto wordsHunterLoopBody;
                }

                j++;
                goto searchingLoopBody;
            }

            // found word case
            if ((inputText[i] > 64 && inputText[i] < 123) &&
                ((inputText[i + 1] > 0 && inputText[i + 1] < 65) || (inputText[i + 1] > 90 && inputText[i + 1] < 97)
                || (inputText[i + 1] == '‘') || inputText[i + 1] == '—'))
            {
                wordEndsIndex = i; 
                isAStopWord = false;

                // extracting the word out of array
                int extractCounter = wordBeginsIndex;
            extractLoopBody:

                // making it lowercased
                int unicodeValue = inputText[extractCounter];
                if ((unicodeValue >= 65) && (unicodeValue <= 90))
                {
                    unicodeValue += 32;
                }

                wordBuilder += (char)unicodeValue;

                if (extractCounter > wordEndsIndex)
                {
                    throw new ArgumentException("incorrect");
                }

                if (extractCounter != wordEndsIndex)
                {
                    extractCounter++;
                    goto extractLoopBody;
                }
                else
                {
                    // if we got to the end of the word
                    // checking if it is not a stop-word
                    int j = 0;
                checkingLoopBody:

                    if (wordBuilder == stopWords[j])
                    {
                        isAStopWord = true;
                        numberOfWordsToEstimate -= 1;
                        numberOfStopWordsFound += 1;
                    }

                    j++;
                    if (j < stopWordsCount) goto checkingLoopBody;

                    if (!isAStopWord)
                    {
                        words[numberOfFoundWords] = wordBuilder;
                        numOfWordsCounter++;
                        numberOfFoundWords++;
                    }

                    wordBuilder = "";
                    foundABeginningOfAWord = false;
                }
            }
            i++;
            if (numberOfFoundWords != numberOfWordsToEstimate)
                goto wordsHunterLoopBody;


            // analyzing our words array
            string[] uniqueWords = new string[numberOfWordsToEstimate];
            int[] wordsFrequency = new int[numberOfWordsToEstimate];

            int numberOfUniqueWords = 0;
            bool isUnique = true;

            int analyzeLoopCounter = 0;
        analyzeLoopLabel:
            // if we find the word not for the first time
            int internalAnalyzeLoopCounter = 0;
        internalAnalyzeLoop:
            if (words[analyzeLoopCounter] == uniqueWords[internalAnalyzeLoopCounter])
            {
                wordsFrequency[internalAnalyzeLoopCounter] += 1;
                isUnique = false;
            }
            internalAnalyzeLoopCounter++;
            if (internalAnalyzeLoopCounter < numberOfUniqueWords) { goto internalAnalyzeLoop; }

            // if we find the word for the first time
            if (isUnique && words[analyzeLoopCounter] != null)
            {
                uniqueWords[numberOfUniqueWords] = words[analyzeLoopCounter];
                wordsFrequency[numberOfUniqueWords] += 1;
                numberOfUniqueWords++;
            }

            isUnique = true;
            analyzeLoopCounter++;
            if (analyzeLoopCounter < numOfWordsCounter) { goto analyzeLoopLabel; }

            // initializing our array with indices
            int[] sortedIndices = new int[numberOfUniqueWords];
            int initArrayCounter = 0;
        initLoopLabel:
            sortedIndices[initArrayCounter] = initArrayCounter;
            initArrayCounter++;
            if (initArrayCounter < numberOfUniqueWords) { goto initLoopLabel; }
 
            // sorting indices with bubble sort
            int temp = 0;
            int temp2 = 0;
            int outerSortCounter = 1;
            int internalSortCounter = 0;
        outerSortLabel:
        internalSortLabel:    
              if (wordsFrequency[internalSortCounter] > wordsFrequency[internalSortCounter + 1])
              {
                  temp = wordsFrequency[internalSortCounter + 1];
                  wordsFrequency[internalSortCounter + 1] = wordsFrequency[internalSortCounter];
                  wordsFrequency[internalSortCounter] = temp;

                  temp2 = sortedIndices[internalSortCounter + 1];
                  sortedIndices[internalSortCounter + 1] = sortedIndices[internalSortCounter];
                  sortedIndices[internalSortCounter] = temp2;
              }
              internalSortCounter++;
              if (internalSortCounter < numberOfUniqueWords - outerSortCounter) { goto internalSortLabel; }
            internalSortCounter = 0;
            outerSortCounter++;
            if (outerSortCounter < numberOfUniqueWords - 1) { goto outerSortLabel; }

            // outputting the result
            int outputCounter = 1;
            outputLoopLabel:
            Console.WriteLine($"{uniqueWords[sortedIndices[numberOfUniqueWords - outputCounter]]}" +
                    $" - {wordsFrequency[numberOfUniqueWords - outputCounter]}");
            outputCounter++;
            if (outputCounter < numberOfUniqueWords + 1) { goto outputLoopLabel; }
        }
    }
}
