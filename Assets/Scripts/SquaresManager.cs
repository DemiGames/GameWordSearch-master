using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.Linq;


public struct RowAndColumn
{
    public int Row;
    public int Column;
}

public class SquaresManager : MonoBehaviour
{
    private bool isMouseDragging = false;
    public bool isClickLettersMode = false;

    public float startPosX;
    public float startPosY;
    public int countX;
    public int countY;
    public float outX;
    public float outY;
    public string objName = "Block_";
    private int id;
    private GameObject[,] grid;
    string[,] lettersGrid;

    int nextRow, nextColumn;
    RowAndColumn firstResult, nextResult;
    List<GameObject> neighbors;

    [SerializeField] Color originalLetterColor;
    [SerializeField] Color succedLetterColor;

    [SerializeField] Color attemptColor;
    [SerializeField] Color originalColor;
    [SerializeField] Color hintColor;


    [SerializeField] List<Color> randomSuccesColors;
    List<Color> copiedRandomSuccesColors;
    Color succesColor;
    [SerializeField] Color wrongColor;

    [SerializeField] SoundManager soundManager;
    [SerializeField] GameObject wordAlertObject;
    public GameObject sqPrefab;
    private List<GameObject> selectedSquares;
    private List<GameObject> copySelectedSquares = new List<GameObject>();
    private List<GameObject> hintedSquares = new List<GameObject>();
    private Transform objText;
    [SerializeField] private string NeededWord;
    private string copyNeededWord;
    bool isWordGuessed;


    private Dictionary<string, List<GameObject>> wordsAndPlacesDictionary;      //������� (����� - ������ � �������)


    private string russianAlphabet = "�����Ũ��������������������������";

    public List<string> levelWordList = new List<string>();

    public int amountWordsOnLevel = 6;
    public LevelManager levelManager;


    private void Start()
    {
        countX = 8; countY = 8;

        selectedSquares = new List<GameObject>();
        neighbors = new List<GameObject>();
        NeededWord = "";
        wordsAndPlacesDictionary = new Dictionary<string, List<GameObject>>();
        isWordGuessed = true;
        GenerateSquares();
        PlaceSquares();
    }



    private IEnumerator AnimateWordSquares()
    {
        ChooseRandomSuccedColor();      //����� ���������� �����
        foreach (var square in copySelectedSquares)
        {
            square.GetComponent<Controller>().FinalSquare();
            MakeColorLetterInSquare(square, succedLetterColor);
            yield return new WaitForSeconds(0.075f);
        }
        levelManager.WordLogic();
        selectedSquares.Clear();
    }
    //���������� ����� ����� ������ ��������
    void MakeColorLetterInSquare(GameObject square, Color letterColor)
    {
        TextMeshProUGUI letter = square.GetComponentInChildren<TextMeshProUGUI>();
        letter.color = letterColor;
    }
    //����� ����� ���������� ����� �� 4 ������ 
    void ChooseRandomSuccedColor()
    {
        int colorNumber = randomNum(0, copiedRandomSuccesColors.Count - 1);
        succesColor = copiedRandomSuccesColors[colorNumber];
        copiedRandomSuccesColors.Remove(succesColor);
    }
    //��������� ���� � �����
    public void GenerateAllLetters()
    {
        NeededWord = "";
        isWordGuessed = true;
        hintedSquares.Clear();
        copiedRandomSuccesColors = new List<Color>(randomSuccesColors);
        isClickLettersMode = levelManager.isClickLettersGameMode();

        //����� ���� �� ������ ������
        levelWordList = levelManager.GetLevelWords();
        //���������� �� �������� � ��������
        levelWordList = levelWordList.OrderByDescending(x => x.Length).ToList();

        AddWordsInGrid();
        AddLettersInGrid();
    }

    //�������� ���� �� ����
    public IEnumerator ShowAllLettersOnField()
    {
        for (int y = 0; y < countY; y++)
        {

            for (int x = 0; x < countX; x++)
            {
                objText = grid[x, y].GetComponentInChildren<Transform>().GetChild(0);
                objText.GetComponentInChildren<TMP_Text>().text = lettersGrid[x, y];
                yield return new WaitForSeconds(1f / (countX * countY));
            }
        }
        LockAllSquares(false);
    }

    //��������� ������ ����
    public void GetMode(bool mode)
    {
        isClickLettersMode = mode;

    }

    //�������� ���� �� ����� � ������
    private bool isWordInList(string searchWord)
    {
        //���� ����� �� ������, �� ��������� ���������� �����
        if (!isClickLettersMode)
        {
            if (!wordsAndPlacesDictionary.ContainsKey(searchWord))
                return false;

            List<GameObject> tempList = new List<GameObject>(wordsAndPlacesDictionary[searchWord]);
            if (Enumerable.SequenceEqual(tempList, selectedSquares))
            {
                return true;
            }
        }
        //���� ����� �� ������, �� ���������� ������ �����
        else
        {
            foreach (var word in levelWordList)
            {
                if (NeededWord == word)
                    return true;
            }
        }

        return false;
    }


    //������ ����� �����
    public void StartWord(GameObject obj)
    {

        //����� � ��������� ����
        if (!isClickLettersMode)
        {
            selectedSquares.Clear();
            isMouseDragging = true;
            firstResult = SearchElement(obj);

            //�������� �� �� ��� ������ ����� ���� ������ �������� ��������
            GetNeighbors(grid, obj);
        }

    }
    //����� ����� �����
    public void EndWord(GameObject obj)
    {
        //���� ����� �� ���������� ����
        if (!isClickLettersMode)
        {
            isMouseDragging = false;
            //���� ����� ���� � ������
            if (isWordInList(NeededWord))
            {
                copyNeededWord = NeededWord;
                isWordGuessed = true;

                copySelectedSquares = new List<GameObject>(selectedSquares);

                levelManager.RecieveGuessedWord(copyNeededWord);
                ClearHint(NeededWord);
                levelWordList.Remove(NeededWord);
                StartCoroutine(AnimateWordSquares());
            }
            //���� ����� ������������
            else
            {
                foreach (var square in selectedSquares)
                {

                    square.GetComponent<Controller>().StartWrongSquareAnim();
                    //����
                    soundManager.MakeWrongWordSound();

                }
                //�������������� � ������������ ������ ����
                if (levelWordList.Contains(NeededWord))
                    wordAlertObject.SetActive(true);

            }

            NeededWord = "";

        }
        //���� ����� �� ������ ����
        else
        {

            //���� ����� ���� � ������
            if (isWordInList(NeededWord))
            {
                copyNeededWord = NeededWord;
                isWordGuessed = true;
                copySelectedSquares = selectedSquares;

                levelManager.RecieveGuessedWord(copyNeededWord);
                ClearHint(NeededWord);
                levelWordList.Remove(NeededWord);
                StartCoroutine(AnimateWordSquares());
                NeededWord = "";
            }

            levelManager.ShowWord(NeededWord);
        }

    }


    //�������� ������� �� ������
    public void DeleteLetterFromWord(GameObject obj)
    {
        objText = obj.GetComponentInChildren<Transform>().GetChild(0);
        string deletingLetter = objText.GetComponent<TMP_Text>().text;

        selectedSquares.Remove(obj);
        NeededWord = NeededWord.Remove(0, NeededWord.Length);
        foreach (var squar in selectedSquares)
        {
            objText = squar.GetComponentInChildren<Transform>().GetChild(0);
            deletingLetter = objText.GetComponent<TMP_Text>().text;
            NeededWord += deletingLetter;

        }

    }
    //����� �������
    private List<GameObject> GetNeighbors(GameObject[,] array, GameObject obj)
    {
        neighbors.Clear();

        nextResult = SearchElement(obj);
        nextRow = nextResult.Row;
        nextColumn = nextResult.Column;

        int numRows = array.GetLength(0);
        int numColumns = array.GetLength(1);

        // ��������� �������� ������ ������ ���������� ��������
        for (int i = nextRow - 1; i <= nextRow + 1; i++)
        {
            for (int j = nextColumn - 1; j <= nextColumn + 1; j++)
            {
                // ���������� ������� ������� � ������������ ��������
                if ((i == nextRow && j == nextColumn) || (i != nextRow && j != nextColumn))
                    continue;
                // ���������, ��� ���������� ��������� � �������� �������
                if (i >= 0 && i < numRows && j >= 0 && j < numColumns)
                {
                    GameObject neighbor = array[i, j];

                    neighbors.Add(neighbor);
                }
            }
        }

        return neighbors;
    }

    public bool CanCheckSquare(GameObject obj)
    {
        if (neighbors.Contains(obj))
        {
            GetNeighbors(grid, obj);
            return true;
        }
        return false;
    }
    //�������� ������������ �������
    public void ClearLastSquare(GameObject obj)
    {
        if (selectedSquares.Count > 1 && obj == selectedSquares.ElementAt(selectedSquares.Count - 2))
        {
            int lastIndex = selectedSquares.Count - 1;

            selectedSquares.ElementAt(lastIndex).GetComponent<Controller>().UnChooseSquare();
            GetNeighbors(grid, obj);
            selectedSquares.RemoveAt(lastIndex);
            NeededWord = NeededWord.Substring(0, NeededWord.Length - 1);

        }
    }

    public void ClearLastLetter()
    {
        if (NeededWord.Length > 0)
        {
            int lastIndex = selectedSquares.Count - 1;
            selectedSquares.ElementAt(lastIndex).GetComponent<Controller>().UnChooseSquare();
            selectedSquares.RemoveAt(lastIndex);
            NeededWord = NeededWord.Substring(0, NeededWord.Length - 1);
        }
    }
    //����� ������ ������ � ���� � �������
    public RowAndColumn SearchElement(GameObject el)
    {
        RowAndColumn result;
        result.Row = 0;
        result.Column = 0;

        int rows = grid.GetLength(0);
        int columns = grid.GetLength(1);
        bool found = false;

        for (int row = 0; row < rows; row++)        //����� �������������� ������� ��������
        {
            for (int col = 0; col < columns; col++)
            {
                if (grid[row, col] == el)
                {
                    found = true;
                    result.Row = row;
                    result.Column = col;
                    break;
                }
            }
            if (found)
                break;
        }
        return result;
    }

    public bool GetMouseDragging()
    {
        return isMouseDragging;
    }

    public Color MakeAttemptColor()
    {
        return attemptColor;
    }

    public Color MakeOriginalColor()
    {
        return originalColor;
    }

    public Color MakeSuccesColor()
    {
        return succesColor;
    }
    public Color MakeWrongColor()
    {
        return wrongColor;
    }
    public bool isWordGuessedCheck()
    {
        return isWordGuessed;
    }
    //���������
    public void HintWord(string word)
    {
        isWordGuessed = false;
        List<GameObject> wordSquareLObjects = wordsAndPlacesDictionary[word];

        if (!isClickLettersMode)
        {
            foreach (GameObject obj in wordSquareLObjects)
            {
                if (!obj.GetComponentInChildren<Controller>().isHintSquare())
                {
                    hintedSquares.Add(obj);

                }
            }
        }
        else
        {
            //���������� ������� �� ������ ������
            foreach (var wordInList in levelWordList)
            {
                if (NeededWord != "" && NeededWord != null && wordInList.StartsWith(NeededWord))
                {

                    wordSquareLObjects = new List<GameObject>(wordsAndPlacesDictionary[wordInList]);

                    foreach (var sq in selectedSquares)
                    {
                        hintedSquares.Add(sq);
                        wordSquareLObjects.RemoveAt(0);
                    }
                    break;
                }
            }

            foreach (GameObject obj in wordSquareLObjects)
            {
                if (!obj.GetComponentInChildren<Controller>().isHintSquare())
                {
                    hintedSquares.Add(obj);
                }

                else
                {
                    objText = obj.GetComponentInChildren<Transform>().GetChild(0);
                    var letter = objText.GetComponentInChildren<TMP_Text>().text;

                    foreach (var cell in grid)
                    {
                        objText = cell.GetComponentInChildren<Transform>().GetChild(0);
                        if (!cell.GetComponentInChildren<Controller>().isHintSquare() && letter.ToString() == objText.GetComponentInChildren<TMP_Text>().text)
                        {
                            hintedSquares.Add(cell);
                            break;
                        }
                    }
                }
            }

        }

        foreach (var square in hintedSquares)
        {
            square.GetComponentInChildren<Controller>().HintSquare();
        }
    }

    private void ClearHint(string word)
    {
        List<GameObject> wordSquareLObjects = wordsAndPlacesDictionary[word];

        foreach (var sq in hintedSquares)
        {
            sq.GetComponentInChildren<Controller>().ClearHintSquare();
        }
        hintedSquares.Clear();
    }
    //���������� ��������� � ������ ��� ��������
    public void AddSquareToList(GameObject square)
    {
        selectedSquares.Add(square);
        if (square.transform.childCount > 0)
        {
            objText = square.GetComponentInChildren<Transform>().GetChild(0);
            NeededWord += objText.GetComponentInChildren<TMP_Text>().text;
        }
    }
    //�������� ��������� � ���������� � ��� ����
    private void GenerateSquares()
    {
        id = 0;
        float posXreset = startPosX;
        grid = new GameObject[countX, countY];
        lettersGrid = new string[countX, countY];
        for (int y = 0; y < countY; y++)
        {

            for (int x = 0; x < countX; x++)
            {
                id++;

                grid[x, y] = Instantiate(sqPrefab, new Vector2(startPosX, startPosY), Quaternion.identity) as GameObject;
                grid[x, y].name = objName + id;

                objText = grid[x, y].GetComponentInChildren<Transform>().GetChild(0);

                grid[x, y].transform.SetParent(transform);

                startPosX += outX;
            }
            startPosY -= outY;
            startPosX = posXreset;
        }
    }
    //��������� ���� � ��������� �������
    private void AddLettersInGrid()
    {
        for (int y = 0; y < countY; y++)
        {
            for (int x = 0; x < countX; x++)
            {
                if (IsSquareClear(x, y))
                {
                    char randomLetter = GetRandomRussianLetter();
                    lettersGrid[x, y] = char.ToString(randomLetter);
                }
            }
        }
    }
    //������� ����� ����
    public void ClearField()
    {
        for (int y = 0; y < countY; y++)
        {

            for (int x = 0; x < countX; x++)
            {
                lettersGrid[x, y] = "";
                objText = grid[x, y].GetComponentInChildren<Transform>().GetChild(0);
                objText.GetComponentInChildren<TMP_Text>().text = "";

            }
        }
        ClearColors();
        selectedSquares.Clear();
    }
    public void ClearColors()
    {

        for (int y = 0; y < countY; y++)
        {

            for (int x = 0; x < countX; x++)
            {
                //�������� ���������
                grid[x, y].gameObject.GetComponent<Controller>().ClearSquare();
                //�������� ����
                MakeColorLetterInSquare(grid[x, y].gameObject, originalLetterColor);
            }
        }
    }
    public void LockAllSquares(bool lockMode)
    {
        for (int y = 0; y < countY; y++)
        {
            for (int x = 0; x < countX; x++)
                grid[x, y].gameObject.GetComponent<Controller>().LockSquare(lockMode);
        }
    }
    private int randomNum(int min, int max)
    {
        return UnityEngine.Random.Range(min, max + 1);
    }
    //�������� �� ������ ����
    private bool IsSquareClear(int x, int y)
    {
        if (lettersGrid[x, y] == "" || lettersGrid[x, y] == null)
            return true;

        return false;
    }
    //���������� ���� �� ����
    private void AddWordsInGrid()
    {
        wordsAndPlacesDictionary.Clear();

        //���������� ����� ���� �� ����
        if (!isClickLettersMode)
        {
            foreach (string word in levelWordList)
            {
                string addingWord = word;

                int lentgthAddingWord = addingWord.Length;
                bool insertedWord = true;
                int firstRandX = 0;
                int firstRandY = 0;

                int attempts = 0;
                int insertedLetters = 0;
                bool isHorizontal = randomNum(0, 1) == 0;
                int patternChoise;
                int maxX, maxY;
                int maxChoise = 1, minChoise = 1;
                List<GameObject> placesList = new List<GameObject>();

                //����������� ���������� ��������� ���������
                switch (lentgthAddingWord)
                {
                    case 2:
                        maxChoise = 1;      //������ � ���� �����
                        break;
                    case 3:
                        maxChoise = 2;      //����� � ������
                        break;
                    default:
                        maxChoise = 3;      //�����, ������, ������
                        break;
                }

                if (lentgthAddingWord > countX)      //����������� ��� ���� ������� 8 ����
                    minChoise = 2;

                patternChoise = randomNum(minChoise, maxChoise);

                do
                {
                    placesList.Clear();
                    attempts++;
                    if (attempts == 250)
                    {
                        patternChoise = 1;
                        attempts = 0;
                    }
                    insertedLetters = 0;
                    isHorizontal = randomNum(0, 1) == 0;

                    //����� ��������
                    switch (patternChoise)
                    {

                        case 1:                 //������� - ������
                            // ��������� ����� �������������
                            if (isHorizontal)
                            {
                                //������� ������ �����
                                maxX = countX - lentgthAddingWord;
                                do
                                {
                                    firstRandX = randomNum(0, maxX);
                                    firstRandY = randomNum(0, countY - 1);
                                } while (!IsSquareClear(firstRandX, firstRandY));


                                for (int i = 0; i < lentgthAddingWord; i++)
                                {
                                    if (IsSquareClear(firstRandX + i, firstRandY))
                                    {
                                        insertedLetters++;
                                    }
                                }
                                if (insertedLetters != lentgthAddingWord)
                                {
                                    insertedWord = false;
                                }
                                else
                                {

                                    for (int i = 0; i < lentgthAddingWord; i++)
                                    {
                                        char letter = addingWord[i];

                                        lettersGrid[firstRandX + i, firstRandY] = char.ToString(letter);
                                        placesList.Add(grid[firstRandX + i, firstRandY]);

                                    }
                                    wordsAndPlacesDictionary.Add(addingWord, placesList);
                                    insertedWord = true;
                                }
                            }

                            // ��������� ����� �����������
                            else
                            {
                                maxY = countY - addingWord.Length;

                                firstRandX = randomNum(0, countX - 1);
                                firstRandY = randomNum(0, maxY);


                                for (int i = 0; i < addingWord.Length; i++)
                                {

                                    if (IsSquareClear(firstRandX, firstRandY + i))
                                    {
                                        insertedLetters++;
                                    }

                                }
                                //���� �� ��� ������ ������ ����� �� ����������
                                if (insertedLetters != addingWord.Length)
                                {
                                    insertedWord = false;

                                }
                                //���� ����� ���� �� �������� �����
                                else
                                {

                                    for (int i = 0; i < addingWord.Length; i++)
                                    {
                                        char letter = addingWord[i];

                                        lettersGrid[firstRandX, firstRandY + i] = char.ToString(letter);
                                        placesList.Add(grid[firstRandX, firstRandY + i]);
                                    }
                                    wordsAndPlacesDictionary.Add(addingWord, placesList);
                                    insertedWord = true;
                                }
                            }
                            break;
                        case 2:         //������� ������

                            int firstEdge = randomNum(2, (int)Math.Ceiling(lentgthAddingWord / 2.0));
                            int secondEdge = lentgthAddingWord - firstEdge;


                            if (isHorizontal)
                            {
                                //������� ������ �����
                                maxX = countX - firstEdge;
                                maxY = countY - secondEdge - 1;
                                do
                                {
                                    firstRandX = randomNum(0, maxX);
                                    firstRandY = randomNum(0, maxY);
                                } while (!IsSquareClear(firstRandX, firstRandY));

                                //������� ������� �����
                                for (int i = 0; i < firstEdge; i++)
                                {

                                    if (IsSquareClear(firstRandX + i, firstRandY))
                                    {
                                        insertedLetters++;
                                    }

                                }
                                //������� ������� �����
                                int fisrtEdgeEnd = firstRandX + (firstEdge - 1);
                                for (int i = 1; i <= secondEdge; i++)
                                {

                                    if (IsSquareClear(fisrtEdgeEnd, firstRandY + i))
                                    {
                                        insertedLetters++;
                                    }
                                }
                                //���� �� ��� ���� ���� ���� �����, �� ����������
                                if (insertedLetters != lentgthAddingWord)
                                {
                                    insertedWord = false;

                                }
                                else
                                {
                                    for (int i = 0; i < firstEdge; i++)
                                    {
                                        char letter = addingWord[i];
                                        lettersGrid[firstRandX + i, firstRandY] = char.ToString(letter);
                                        placesList.Add(grid[firstRandX + i, firstRandY]);
                                    }
                                    //������� ������� �����                                   
                                    for (int i = 0; i < secondEdge; i++)
                                    {
                                        char letter = addingWord[firstEdge + i];
                                        lettersGrid[fisrtEdgeEnd, firstRandY + i + 1] = char.ToString(letter);
                                        placesList.Add(grid[fisrtEdgeEnd, firstRandY + i + 1]);
                                    }
                                    wordsAndPlacesDictionary.Add(addingWord, placesList);
                                    insertedWord = true;
                                }
                            }

                            // ��������� ����� ����������� ���� -> ������������ ������
                            else
                            {
                                maxY = countX - firstEdge;
                                maxX = countY - secondEdge - 1;

                                firstRandX = randomNum(0, maxX);
                                firstRandY = randomNum(0, maxY);

                                //�������� ������� �������
                                do
                                {
                                    firstRandX = randomNum(0, maxX);
                                    firstRandY = randomNum(0, maxY);
                                } while (!IsSquareClear(firstRandX, firstRandY));

                                //�������� ������� �����
                                for (int i = 0; i < firstEdge; i++)
                                {

                                    if (IsSquareClear(firstRandX, firstRandY + i))
                                    {
                                        insertedLetters++;
                                    }

                                }
                                //�������� ������� �����
                                int fisrtEdgeEnd = firstRandY + (firstEdge - 1);
                                for (int i = 1; i <= secondEdge; i++)
                                {

                                    if (IsSquareClear(firstRandX + i, fisrtEdgeEnd))
                                    {
                                        insertedLetters++;
                                    }
                                }
                                //���� �� ��� ���� ���� ���� �����, �� ����������
                                if (insertedLetters != lentgthAddingWord)
                                {
                                    insertedWord = false;

                                }
                                else
                                {
                                    //������� ������� �����
                                    for (int i = 0; i < firstEdge; i++)
                                    {
                                        char letter = addingWord[i];
                                        lettersGrid[firstRandX, firstRandY + i] = char.ToString(letter);
                                        placesList.Add(grid[firstRandX, firstRandY + i]);
                                    }
                                    //������� ������� �����                                   
                                    for (int i = 0; i < secondEdge; i++)
                                    {
                                        char letter = addingWord[firstEdge + i];
                                        lettersGrid[firstRandX + 1 + i, fisrtEdgeEnd] = char.ToString(letter);
                                        placesList.Add(grid[firstRandX + 1 + i, fisrtEdgeEnd]);

                                    }

                                    wordsAndPlacesDictionary.Add(addingWord, placesList);
                                    insertedWord = true;
                                }
                            }
                            break;
                        case 3:             //������� ������

                            firstEdge = randomNum(2, (int)Math.Ceiling(lentgthAddingWord / 2.0));
                            secondEdge = randomNum(1, (int)Math.Floor(lentgthAddingWord / 2.0) - 1);
                            int thirdEdge = lentgthAddingWord - firstEdge - secondEdge;
                            int minX = 0, minY = 0;
                            int randomThirdEdgeDirection = randomNum(0, 1);

                            if (isHorizontal)
                            {
                                //������� ������ �����
                                maxX = countX - (firstEdge + thirdEdge);
                                maxY = countY - secondEdge - 1;

                                if (randomThirdEdgeDirection == 0)
                                {
                                    minX = thirdEdge - 1;
                                }
                                do
                                {
                                    firstRandX = randomNum(minX, maxX);
                                    firstRandY = randomNum(minY, maxY);
                                } while (!IsSquareClear(firstRandX, firstRandY));

                                //�������� ������� �����
                                for (int i = 0; i < firstEdge; i++)
                                {

                                    if (IsSquareClear(firstRandX + i, firstRandY))
                                    {
                                        insertedLetters++;
                                    }

                                }
                                //�������� ������� �����
                                int firstEdgeEnd = firstRandX + (firstEdge - 1);
                                for (int i = 1; i <= secondEdge; i++)
                                {

                                    if (IsSquareClear(firstEdgeEnd, firstRandY + i))
                                    {
                                        insertedLetters++;
                                    }
                                }
                                int secondEdgeEnd = firstRandY + secondEdge;
                                int thirdEdgeStart;
                                thirdEdgeStart = firstEdgeEnd;
                                //�������� �������� �����
                                for (int i = 1; i <= thirdEdge; i++)
                                {
                                    int j = i;
                                    if (randomThirdEdgeDirection == 0)
                                        j = -i;

                                    if (IsSquareClear(thirdEdgeStart + j, secondEdgeEnd))
                                    {
                                        insertedLetters++;
                                    }
                                }
                                //���� �� ��� ���� ���� ���� �����, �� ����������
                                if (insertedLetters != lentgthAddingWord)
                                {
                                    insertedWord = false;
                                }
                                else
                                {
                                    //������� ������� ����� 
                                    for (int i = 0; i < firstEdge; i++)
                                    {
                                        char letter = addingWord[i];
                                        lettersGrid[firstRandX + i, firstRandY] = char.ToString(letter);
                                        placesList.Add(grid[firstRandX + i, firstRandY]);
                                    }
                                    //������� ������� �����                                   
                                    for (int i = 0; i < secondEdge; i++)
                                    {
                                        char letter = addingWord[firstEdge + i];
                                        lettersGrid[firstEdgeEnd, firstRandY + i + 1] = char.ToString(letter);
                                        placesList.Add(grid[firstEdgeEnd, firstRandY + i + 1]);

                                    }
                                    //������� �������� �����                                   
                                    for (int i = 0; i < thirdEdge; i++)
                                    {
                                        int temp, j = i;
                                        temp = firstEdgeEnd + 1;
                                        if (randomThirdEdgeDirection == 0)
                                        {
                                            j = -i;
                                            temp = firstEdgeEnd - 1;
                                        }

                                        char letter = addingWord[firstEdge + secondEdge + i];

                                        lettersGrid[temp + j, secondEdgeEnd] = char.ToString(letter);
                                        placesList.Add(grid[temp + j, secondEdgeEnd]);

                                    }
                                    wordsAndPlacesDictionary.Add(addingWord, placesList);
                                    insertedWord = true;
                                }
                            }

                            // ��������� ����� ����������� ���� -> ������������ ������ - �����
                            else
                            {
                                maxX = countX - secondEdge - 1;
                                maxY = countY - (firstEdge + thirdEdge);

                                if (randomThirdEdgeDirection == 0)
                                {
                                    minY = thirdEdge - 1;
                                }
                                //�������� ������� �������
                                do
                                {
                                    firstRandX = randomNum(minX, maxX);
                                    firstRandY = randomNum(minY, maxY);
                                } while (!IsSquareClear(firstRandX, firstRandY));

                                //�������� ������� �����
                                for (int i = 0; i < firstEdge; i++)
                                {

                                    if (IsSquareClear(firstRandX, firstRandY + i))
                                    {
                                        insertedLetters++;
                                    }

                                }
                                //�������� ������� �����
                                int firstEdgeEnd = firstRandY + (firstEdge - 1);
                                for (int i = 1; i <= secondEdge; i++)
                                {

                                    if (IsSquareClear(firstRandX + i, firstEdgeEnd))
                                    {
                                        insertedLetters++;
                                    }
                                }
                                int secondEdgeEnd = firstRandX + secondEdge;
                                int thirdEdgeStart;
                                thirdEdgeStart = firstEdgeEnd;
                                //�������� �������� �����
                                for (int i = 1; i <= thirdEdge; i++)
                                {
                                    int j = i;
                                    if (randomThirdEdgeDirection == 0)
                                        j = -i;


                                    if (IsSquareClear(secondEdgeEnd, thirdEdgeStart + j))
                                    {
                                        insertedLetters++;
                                    }
                                }
                                //���� �� ��� ���� ���� ���� �����, �� ����������
                                if (insertedLetters != lentgthAddingWord)
                                {
                                    insertedWord = false;

                                }
                                else
                                {
                                    //������� ������� �����
                                    for (int i = 0; i < firstEdge; i++)
                                    {
                                        char letter = addingWord[i];
                                        lettersGrid[firstRandX, firstRandY + i] = char.ToString(letter);
                                        placesList.Add(grid[firstRandX, firstRandY + i]);
                                    }
                                    //������� ������� �����                                   
                                    for (int i = 0; i < secondEdge; i++)
                                    {
                                        char letter = addingWord[firstEdge + i];
                                        lettersGrid[firstRandX + i + 1, firstEdgeEnd] = char.ToString(letter);
                                        placesList.Add(grid[firstRandX + i + 1, firstEdgeEnd]);
                                    }
                                    //������� �������� �����                                   
                                    for (int i = 0; i < thirdEdge; i++)
                                    {
                                        int temp, j = i;
                                        temp = firstEdgeEnd + 1;
                                        if (randomThirdEdgeDirection == 0)
                                        {
                                            j = -i;
                                            temp = firstEdgeEnd - 1;
                                        }

                                        char letter = addingWord[firstEdge + secondEdge + i];

                                        lettersGrid[secondEdgeEnd, temp + j] = char.ToString(letter);
                                        placesList.Add(grid[secondEdgeEnd, temp + j]);
                                    }
                                    wordsAndPlacesDictionary.Add(addingWord, placesList);
                                    insertedWord = true;
                                }
                            }
                            break;
                    }
                } while (!insertedWord);
            }
        }




        //���������� ��������� ���� �� ���� �� ����
        else
        {
            foreach (string word in levelWordList)
            {
                string addingWord = word;


                int randX = 0;
                int randY = 0;

                List<GameObject> placesList = new List<GameObject>();
                for (int i = 0; i < addingWord.Length; i++)
                {
                    do
                    {
                        randX = randomNum(0, countX - 1);
                        randY = randomNum(0, countY - 1);
                    } while (!IsSquareClear(randX, randY));

                    char letter = addingWord[i];

                    objText = grid[randX, randY].GetComponentInChildren<Transform>().GetChild(0);

                    //objText.GetComponentInChildren<TMP_Text>().text = char.ToString(letter);
                    lettersGrid[randX, randY] = char.ToString(letter);
                    placesList.Add(grid[randX, randY]);
                }
                wordsAndPlacesDictionary.Add(addingWord, placesList);
            }
        }

    }


    public string SelectRandomWord(List<string> words)
    {
        int randomIndex = randomNum(0, words.Count - 1);
        string randomWord = words[randomIndex];
        return randomWord;
    }

    //����� ��������� �����
    private char GetRandomRussianLetter()
    {
        // ���������� ��������� ������ �� 0 �� ����� �������� ����� 1
        int randomIndex = randomNum(0, russianAlphabet.Length - 1);

        // �������� ��������� ����� �� ���������������� �������
        char randomLetter = russianAlphabet[randomIndex];

        return randomLetter;
    }
    //���������� ����� � ������
    private void PlaceSquares()
    {
        RectTransform parentRectTransform = GetComponent<RectTransform>();

        outX = 1f;               // ���������� ����� �������� �� ��� X
        outY = 1f;               // ���������� ����� �������� �� ��� Y
        float cellWidth = (parentRectTransform.rect.width - (outX * (countX - 1))) / countX;
        float cellHeight = (parentRectTransform.rect.height - (outY * (countY - 1))) / countY;

        for (int i = 0; i < transform.childCount; i++)
        {
            RectTransform childRectTransform = transform.GetChild(i).GetComponent<RectTransform>();

            int row = i / countX;
            int column = i % countY;

            // ������������ ������� �� ��� X
            float posX = column * outX + column * cellWidth - parentRectTransform.rect.width / 2f + cellWidth / 2f;

            // ������������ ������� �� ��� Y
            float posY = parentRectTransform.rect.height / 2f - (row * outY + row * cellHeight) - cellHeight / 2f;

            // ������������� ������� � ������� ��������� �������
            childRectTransform.sizeDelta = new Vector2(cellWidth, cellHeight);
            childRectTransform.anchoredPosition = new Vector2(posX, posY);
        }
    }


    public class SortDescendingComparer : IComparer<string>
    {
        public int Compare(string x, string y)
        {
            // ��������� ����� � ������� �� �������� � ��������
            return y.CompareTo(x);
        }
    }
}
