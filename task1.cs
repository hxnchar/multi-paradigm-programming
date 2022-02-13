//=====================
//Some settings
//=====================
int stopWordMinLenght = 3; // https://t.me/multiparadigm_labs/425
int countCommonWords = 25; // count of the most common words (25 by default)
string inputFilePath = "input.txt"; // name of the input file
string outputFilePath = "output.txt"; // name of the output file

//=====================
//Variables initialization
//=====================
string[] wordsArray = new string[1], resultWordsArray = new string[countCommonWords]; // arrays of words
int[] wordsCount = new int[1], resultWordsCount = new int[countCommonWords]; // arrays of count of words with corresponding index
int currentChar; // ascii code of the current char
int lastWordPointer = 0, currentWordIndex; // {lastWordPointer} index of the last filled word in array, {currentWordIndex} used for adding new word and copy array
int i = 0, j = 0; // indexes used everywhere
string currentWord = ""; // current word

//=====================
//Reading the input file
//=====================
StreamReader streamReader = new StreamReader(inputFilePath);
readingFile:
{
    // keep reading, if it isn't end of the file
    if (!streamReader.EndOfStream)
    {
        currentChar = streamReader.Read();
    }
    else
    {
        goto stopReading;
    }
        
    if (97 <= currentChar && currentChar <= 122) // lowercase symbols codes in dec
    {
        currentWord = currentWord + (char)currentChar;
        goto nextChar;
    }
    else if (65 <= currentChar && currentChar <= 90) // uppercase symbols codes in dec
    {
        currentWord = currentWord + (char)(currentChar + 32); // converting to lowercase
        goto nextChar;
    }
    if (currentWord != "")
    {
        if (currentWord.Length < stopWordMinLenght) // length of the current word couldn't be lower than {stopWordMinLenght}
        {
            currentWord = "";
        }
        else
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
                    wordsCount[currentWordIndex]++;
                    goto nextChar;
                }
                currentWordIndex++;
                goto checkWords;
            }

            addNewWord:
            {
                if (lastWordPointer == wordsArray.Length) // if the last word is in the last position we have to expand the array
                {
                    string[] newWordsArray = new string[wordsArray.Length * 2]; // optimized resizing
                    int[] newWordsCountArray = new int[wordsArray.Length * 2];
                    currentWordIndex = 0;
                    copyTheNextElement:
                    {
                        if (currentWordIndex == lastWordPointer) // we've copied all the elements
                        {
                            wordsArray = newWordsArray;
                            wordsCount = newWordsCountArray;
                            goto addTheLastWord;
                        }
                        newWordsArray[currentWordIndex] = wordsArray[currentWordIndex];
                        newWordsCountArray[currentWordIndex] = wordsCount[currentWordIndex];
                        currentWordIndex++;
                        goto copyTheNextElement;
                    }
                }
            }
            addTheLastWord:
            {
                wordsArray[lastWordPointer] = currentWord; // if we find a new word, it is single so far :(
                wordsCount[lastWordPointer] = 1;
                lastWordPointer++;
                currentWord = "";
            }
        }
    }

    nextChar:
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
                if (wordsCount[j] < wordsCount[j+1])
                {
                    int tempCount = wordsCount[j];
                    string tempWord = wordsArray[j];
                    wordsCount[j] = wordsCount[j + 1];
                    wordsArray[j] = wordsArray[j + 1];
                    wordsCount[j + 1] = tempCount;
                    wordsArray[j + 1] = tempWord;
                }
                j = j + 1;
                goto innerLoop;
            }
        }
        i = i - 1;
        goto outerLoop;
    }
}

//=====================
//Setting limit of {countCommonWords}
//=====================
j = lastWordPointer - 1;
i = 0;
settingLimit:
{
    if (i < countCommonWords && i <= j && wordsCount[i] != 0)
    {
        resultWordsArray[i] = wordsArray[i];
        resultWordsCount[i] = wordsCount[i];
        i = i + 1;
        goto settingLimit;
    }
}

//=====================
//Writing to the file
//=====================
j = lastWordPointer - 1;
i = 0;
StreamWriter streamWriter = new StreamWriter(outputFilePath);
writeNextElement:
{
    if (i <= j)
    {
        streamWriter.WriteLine($"{resultWordsArray[i]} - {resultWordsCount[i]}");
        i = i + 1;
        goto writeNextElement;
    }
}
streamWriter.Close();
