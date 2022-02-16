//=====================
//Some settings
//=====================
string inputFilePath = "input.txt"; // name of the input file
string outputFilePath = "output.txt"; // name of the output file
int amountToIgnore = 100; // we should ignore all words that are more than {countToIgnore}
int linesPerPage = 45;

//=====================
//Variables initialization
//=====================

string[] wordsArray = new string[1]; // array of words
int[] wordsCount = new int[1]; // array of count of words with corresponding index
char currentChar; // current char
int lastWordPointer = 0, currentWordIndex; // {lastWordPointer} index of the last filled word in array, {currentWordIndex} used for adding new word and copy array
int i = 0, j = 0; // indexes used everywhere
string currentWord = ""; // current word
int linesCount = 0; // count lines on the current page
string currentString = ""; // current line
int[][] pagesContent = new int[1][];
int currentPage = 0; // number of the current page

//=====================
//Reading the input file
//=====================
StreamReader streamReader = new StreamReader(inputFilePath);
readingFile:
{  
    if (!streamReader.EndOfStream)
    {
        currentString = streamReader.ReadLine();
        linesCount = linesCount + 1;
    }
    else
    {
        goto stopReading;
    }
    
    if (linesCount == linesPerPage)
    {
        linesCount = 0;
        currentPage = currentPage + 1;
    }

    i = 0; // iterator for the current line
    iteratingInString:
    {
        if (i == currentString.Length)
        {
            goto nextChar;
        }
            
        currentChar = currentString[i];
        
        if (97 <= currentChar && currentChar <= 122) // lowercase symbols codes in dec
        {
            currentWord = currentWord + currentChar;
            if (i + 1 < currentString.Length)
                goto nextChar;
        }
        else if (65 <= currentChar && currentChar <= 90) // uppercase symbols codes in dec
        {
            currentWord = currentWord + (char)(currentChar + 32); // converting to lowercase
            if (i + 1 < currentString.Length)
                goto nextChar;
        }

        if (currentWord != "")
        {
            currentWordIndex = 0;
            checkWords:
            {
                if (currentWordIndex == lastWordPointer) // we've checked all words and didn't find any matches
                {
                    goto addNewWord;
                }
                if (currentWord == wordsArray[currentWordIndex]) // check if current work is same with current word in array
                {
                    currentWord = "";
                    wordsCount[currentWordIndex] = wordsCount[currentWordIndex] + 1;
                    if (pagesContent[currentWordIndex].Length < wordsCount[currentWordIndex]) // check if array length is enough 
                    {
                        j = 0;
                        int[] newPages = new int[wordsCount[currentWordIndex] * 2]; // optimized resizing
                        nextCopy:
                        {
                            newPages[j] = pagesContent[currentWordIndex][j];
                            j = j + 1;
                            if (j < wordsCount[currentWordIndex] - 1)
                                goto nextCopy;
                        }
                        pagesContent[currentWordIndex] = newPages;
                        pagesContent[currentWordIndex][wordsCount[currentWordIndex] - 1] = currentPage;
                    }
                    pagesContent[currentWordIndex][wordsCount[currentWordIndex] - 1] = currentPage;
                    goto nextChar;
                }
                currentWordIndex++;
                goto checkWords;
            }

            addNewWord:
            if (lastWordPointer == wordsArray.Length) // if the last word is in the last position we have to expand the array
            {
                string[] newWordsArray = new string[wordsArray.Length * 2]; // optimized resizing
                int[] newWordsCountArray = new int[wordsArray.Length * 2];
                int[][] newPagesArray = new int[wordsArray.Length * 2][];

                currentWordIndex = 0;
                copyTheNextElement:
                {
                    if (currentWordIndex == lastWordPointer)
                    {
                        wordsArray = newWordsArray;
                        wordsCount = newWordsCountArray;
                        pagesContent = newPagesArray;
                        goto addTheLastWord;
                    }
                    newWordsArray[currentWordIndex] = wordsArray[currentWordIndex];
                    newWordsCountArray[currentWordIndex] = wordsCount[currentWordIndex];
                    newPagesArray[currentWordIndex] = pagesContent[currentWordIndex];
                    currentWordIndex++;
                    goto copyTheNextElement;
                }
            }
            addTheLastWord:
            {
                wordsArray[lastWordPointer] = currentWord;
                wordsCount[lastWordPointer] = 1;
                pagesContent[lastWordPointer] = new int[1];
                pagesContent[lastWordPointer][0] = currentPage;
                lastWordPointer++;
                currentWord = "";
            }
        }

        nextChar:
        {
            i = i + 1;
            if (i >= currentString.Length)
            {
                goto readNextLine;
            }
            goto iteratingInString;
        }
    }
    
    readNextLine:
    if (!streamReader.EndOfStream)
    {
        goto readingFile;
    }
    stopReading:
    {
        streamReader.Close();
    }
}

//=====================
//Bubble sort
//=====================
i = lastWordPointer - 1;
outerLoop:
{
    j = 0;
    if (i >= 1)
    {
        innerLoop:
        {
            if (j < i)
            {
                int currentCharIndex = 0;
                checkTheNextChar:
                {
                    if (wordsArray[j][currentCharIndex] == wordsArray[j+1][currentCharIndex]
                        && currentCharIndex+1 != wordsArray[j].Length && currentCharIndex+1 != wordsArray[j+1].Length)
                    {
                        currentCharIndex += 1;
                        goto checkTheNextChar;
                    }
                }

                if (currentCharIndex+1 == wordsArray[j].Length && currentCharIndex+1 != wordsArray[j+1].Length
                    && wordsArray[j][currentCharIndex] == wordsArray[j+1][currentCharIndex])
                {
                    goto compareNextWord;
                } else if (currentCharIndex+1 == wordsArray[j+1].Length && currentCharIndex+1 != wordsArray[j].Length
                            && wordsArray[j][currentCharIndex] == wordsArray[j+1][currentCharIndex])
                {
                    goto changeWords;
                }
                
                if (wordsArray[j][currentCharIndex] > wordsArray[j+1][currentCharIndex])
                {
                    goto changeWords;
                }
                else
                {
                    goto compareNextWord;
                }
                changeWords:
                {
                    int tempCount = wordsCount[j];
                    string tempWord = wordsArray[j];
                    var tempPages = pagesContent[j];
                    wordsCount[j] = wordsCount[j + 1];
                    wordsArray[j] = wordsArray[j + 1];
                    pagesContent[j] = pagesContent[j + 1];
                    wordsCount[j + 1] = tempCount;
                    wordsArray[j + 1] = tempWord;
                    pagesContent[j + 1] = tempPages;
                }
                compareNextWord:
                j = j + 1;
                goto innerLoop;
            }
        }
        i = i - 1;
        goto outerLoop;
    }
}

//=====================
//Writing to the file
//=====================
StreamWriter streamWriter = new StreamWriter(outputFilePath);
currentWordIndex = 0;
writeTheNextWord:
{
    if (wordsCount[currentWordIndex] < amountToIgnore) { // we should write this word to the file if its amount does not exceed {amountToIgnore}
        currentPage = 0;
        outPages:
        {
            if (currentPage == 0)
            {
                streamWriter.Write($"{wordsArray[currentWordIndex]} - {pagesContent[currentWordIndex][currentPage]}"); // writing the word and first page
            }
            currentPage = currentPage + 1;
            if (wordsCount[currentWordIndex] == currentPage)
            {
                streamWriter.WriteLine();
                goto endOutPages;
            }
            if (pagesContent[currentWordIndex][currentPage] != pagesContent[currentWordIndex][currentPage - 1]) // do not duplicate the same page
            {
                streamWriter.Write($", {pagesContent[currentWordIndex][currentPage]}");
            }
            goto outPages;
        }
    }
    endOutPages:
    {
        currentWordIndex = currentWordIndex + 1;
        if (currentWordIndex < lastWordPointer)
        {
            goto writeTheNextWord;
        }
    } 
}

streamWriter.Close();
