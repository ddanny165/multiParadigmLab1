using System;

namespace MultiParadigmLab1._2
{
    class Program
    {
        static void Main(string[] args)
        {
            string bookText = System.IO.File.ReadAllText($"prideAndPrejudice.txt");
            int characterCount = 706573; //num of symbols in a given text

            string[] stopWords = { "for", "that", "the", "into", "of", "with", "an", "at",
                "in", "is", "so", "if", "as", "on", "a", "from", "and", "to", "by" };
            int stopWordsCount = 19;
            int numberOfStopWordsFound = 0;

            string[] words = new string[95000];

            // for tracking words
            int wordBeginsIndex = 0;
            int wordEndsIndex = 0;
            string wordBuilder = "";
            int numberOfFoundWords = 0;

            bool foundABeginningOfAWord = false;
            bool isAStopWord = false;

            int i = 0;
        wordsHunterLoopBody:
            // trying to find a beginning of a new word
            if (!foundABeginningOfAWord && wordEndsIndex != 0)
            {
                int j = wordEndsIndex;
            searchingLoopBody:

                if ((bookText[j] > 64 && bookText[j] < 123) &&
                ((bookText[j - 1] > 0 && bookText[j - 1] < 65) ||
                (bookText[j - 1] > 90 && bookText[j - 1] < 97) || (bookText[j - 1] == '‘') || bookText[j - 1] == '—'))
                {
                    wordBeginsIndex = j;
                    foundABeginningOfAWord = true;

                    goto wordsHunterLoopBody;
                }

                j++;
                goto searchingLoopBody;
            }

            // found word case
            if ((bookText[i] > 64 && bookText[i] < 123) &&
            ((bookText[i + 1] > 0 && bookText[i + 1] < 65) || (bookText[i + 1] > 90 && bookText[i + 1] < 97)
            || (bookText[i + 1] == '‘') || bookText[i + 1] == '—'))
            {
                wordEndsIndex = i;
                isAStopWord = false;

                // extracting the word out of array
                int extractCounter = wordBeginsIndex;
            extractLoopBody:
                if (bookText[extractCounter] == ' '
                || bookText[extractCounter] == '\n')
                {
                    extractCounter++;
                    wordBuilder = "";
                }
                // making it lowercased          
                int asciiValue = bookText[extractCounter];
                if ((asciiValue > 64) && (asciiValue < 91))
                {
                    asciiValue += 32;
                }

                wordBuilder += (char)asciiValue;

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
                        numberOfStopWordsFound += 1;
                    }
                    j++;
                    if (j < stopWordsCount) goto checkingLoopBody;

                    if (!isAStopWord)
                    {
                        words[numberOfFoundWords] = wordBuilder;
                        numberOfFoundWords++;
                    }

                    wordBuilder = "";
                    foundABeginningOfAWord = false;
                }
            }
            i++;
            if (i < characterCount)  
                goto wordsHunterLoopBody;

            // analyzing our words array
            int possibleNumberOfUniqueWords = 6500;
            string[] uniqueWords = new string[possibleNumberOfUniqueWords];
            int numberOfUniqueWords = 0;
            bool isUnique = true;

            int currentPage = 1;
            int wordsPerPage = 245;

            // allocating memory for subarrays to track pages
            // index in this arr corresponds to index in the uniqueWords arr
            int[][] pagesForWords = new int[possibleNumberOfUniqueWords][];
            int allocationCounter = 0;
        allocationLoopLabel:
            pagesForWords[allocationCounter] = new int[500];
            allocationCounter++;
            if (allocationCounter < possibleNumberOfUniqueWords) { goto allocationLoopLabel; }

            // tracking pages (1 page == 245 words)
            int analyzeLoopCounter = 0;
        analyzeLoopLabel:
            int internalAnalyzeLoopCounter = 0;
        internalAnalyzeLoop:
            // if we find the word not for the first time
            if (words[analyzeLoopCounter] == uniqueWords[internalAnalyzeLoopCounter])
            {
                // here goes the code if we have already found a word
                currentPage = analyzeLoopCounter / wordsPerPage + 1;

                // looking for an empty space in an array
                int emptySpaceCounter = 0;
            emptySpaceSearchLoop:
                if (pagesForWords[internalAnalyzeLoopCounter][emptySpaceCounter] == 0)
                {
                    pagesForWords[internalAnalyzeLoopCounter][emptySpaceCounter] = currentPage;
                    goto endArray;
                }
                emptySpaceCounter++;
                if (emptySpaceCounter < 500) { goto emptySpaceSearchLoop; }
                
                endArray:
                isUnique = false;
            }
            internalAnalyzeLoopCounter++;
            if (internalAnalyzeLoopCounter < numberOfUniqueWords) { goto internalAnalyzeLoop; }

            // if we find the word for the first time
            if (isUnique && words[analyzeLoopCounter] != null)
            {
                // checking word's correctness
                int correctnessCount = 0;
                correctnessCheckLabel:
                
                    if (words[analyzeLoopCounter][correctnessCount] == '’'
                        || words[analyzeLoopCounter][correctnessCount] == '-'
                        || words[analyzeLoopCounter][correctnessCount] == '‘')
                    {
                        goto notCorrectWordCase;
                    }
                
                correctnessCount++;
                if (correctnessCount < words[analyzeLoopCounter].Length) { goto correctnessCheckLabel; }
                // here goes the code if we've not met the word before and the word is correct
                uniqueWords[numberOfUniqueWords] = words[analyzeLoopCounter];
            
                currentPage = analyzeLoopCounter / wordsPerPage + 1;

                // looking for an empty space in an array
                int searchEmptyCounter = 0;
            emptySearchLabel:
                if (pagesForWords[numberOfUniqueWords][searchEmptyCounter] == 0)
                {
                    pagesForWords[numberOfUniqueWords][searchEmptyCounter] = currentPage;
                    goto endArray2;
                }
                searchEmptyCounter++;
                if (searchEmptyCounter < 500) { goto emptySearchLabel; }
            endArray2:
                numberOfUniqueWords++;
            }
        notCorrectWordCase:
            isUnique = true;
            analyzeLoopCounter++;
            if (analyzeLoopCounter < numberOfFoundWords) { goto analyzeLoopLabel; }

            // sorting it alphabetically
            string tempString;
            int[] tempIntArray;
            int sortLoopLimit = 1;
            bool isCurrentBigger = false;

            int outerSortLoopCounter = 0;
            outerSortLoopLabel:
                int internalSortLoopCounter = 0;
            internalSortLoopLabel:
                    int compCount = 0;
                compCountLabel:
                    sortLoopLimit = uniqueWords[internalSortLoopCounter].Length > uniqueWords[internalSortLoopCounter + 1].Length ?
                        uniqueWords[internalSortLoopCounter + 1].Length : uniqueWords[internalSortLoopCounter].Length;

                    if (uniqueWords[internalSortLoopCounter][compCount] > uniqueWords[internalSortLoopCounter + 1][compCount])
                    {
                        isCurrentBigger = true;
                        goto swapLabel;
                    }
                    else if (uniqueWords[internalSortLoopCounter][compCount] < uniqueWords[internalSortLoopCounter + 1][compCount]) { goto swapLabel; }
                    compCount++;
                    if (compCount < sortLoopLimit) { goto compCountLabel; }

                swapLabel:
                    if (isCurrentBigger)
                    {
                        tempString = uniqueWords[internalSortLoopCounter + 1];
                        uniqueWords[internalSortLoopCounter + 1] = uniqueWords[internalSortLoopCounter];
                        uniqueWords[internalSortLoopCounter] = tempString;

                        tempIntArray = pagesForWords[internalSortLoopCounter + 1];
                        pagesForWords[internalSortLoopCounter + 1] = pagesForWords[internalSortLoopCounter];
                        pagesForWords[internalSortLoopCounter] = tempIntArray;

                        isCurrentBigger = false;
                    }
                internalSortLoopCounter++;
                if (internalSortLoopCounter < numberOfUniqueWords - 1) { goto internalSortLoopLabel;  }
            outerSortLoopCounter++;
            if (outerSortLoopCounter < numberOfUniqueWords - 1) { goto outerSortLoopLabel; }
            
            // outputting the result
            string outputPagesBuilder = "";
            int outputCounter = 0;
        outputLoopLabel:

            int outputPagesCounter = 0;
        outputPagesLoop:
            // ignoring repeating page numbers (if word occurs >2 times on the same page)
            if ( pagesForWords[outputCounter][outputPagesCounter]
                != pagesForWords[outputCounter][outputPagesCounter + 1] )
            {
                outputPagesBuilder += $"{pagesForWords[outputCounter][outputPagesCounter]}, ";
            }

            outputPagesCounter++;
            if ((pagesForWords[outputCounter][outputPagesCounter + 1] != 0) && (outputPagesCounter < 498)) { goto outputPagesLoop; }
            
            // ignoring words that occurs over 100 times
            if (outputPagesCounter < 100)
            {
                Console.WriteLine($"{uniqueWords[outputCounter]} - {outputPagesBuilder}");
            }

            outputCounter++;
            outputPagesBuilder = "";
            if (outputCounter < numberOfUniqueWords) { goto outputLoopLabel; }
        }
    }
}
