using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEngine.UI.Image;

public class LevelManagerCatch : MonoBehaviour
{

    [SerializeField] private GameObject correctAnswer;
    [SerializeField] private GameObject wrongAnswer;

    [SerializeField] private Transform startSpawn;
    [SerializeField] private Transform endSpawn;
    [SerializeField] private Transform bottomCollider;

    [SerializeField] private List<CatchQuestionAnswers> catchQuestionAnswers;
    [SerializeField] private TextAsset csvFile;

    [SerializeField] private TextMeshPro questionText;
    [SerializeField] private TextMeshPro highScoreCounterText;

    [SerializeField] private GameObject catchFinancesMain;
    [SerializeField] private GameObject catchFinancesResults;

    //[SerializeField] private TextMeshPro finalHighScoreText;
    //[SerializeField] private TextMeshPro expEarned;

    //[SerializeField] private GameObject homeButton;

    [Header("Basket")]
    [SerializeField] private BascketMouse basket;

    [Header("WrongAnswerLayer")]
    [SerializeField] private GameObject WrongLayerPrefab;
    [SerializeField] private Transform WrongLayerPosition;
    private Vector3 originalWrongLayerPosition;
    private Vector3 originalBasketPosition;

    private List<GameObject> fallingObjectsList = new List<GameObject>();
    private List<GameObject> wrongLayersInSceneList = new List<GameObject>();
    private int currentQuestionint = 0;
    private int currentAnswerint = 0;

    private int countToFiveQuestions = 0;

    private int highScore = 0;
    private int expTracker = 100;

    private void Start()
    {
        //Grab the original positions to reset later
        originalBasketPosition = basket.transform.position;
        originalWrongLayerPosition = WrongLayerPosition.position;
    }

    public void StartGame()
    {
        LoadQuestionsFromCSV(csvFile);
        DropAnswers();
        questionText.text = catchQuestionAnswers[currentQuestionint].CombineQuestionText(); ;
    }

    public void RestartGame()
    {
        // Reset Variables
        highScore = 0;
        expTracker = 100;
        countToFiveQuestions = 0;
        // reset highScore text
        highScoreCounterText.text = $"{highScore}";
        // Reset Basket * WrongLayer positions
        basket.UpdateYPosition(originalBasketPosition.y);
        WrongLayerPosition.position = originalWrongLayerPosition;

        // Reset the game scene
        catchFinancesMain.gameObject.SetActive(true);
        catchFinancesResults.gameObject.SetActive(false);
        //homeButton.SetActive(false);
        // Destroy all answers and wronglayers
        foreach (GameObject i in fallingObjectsList)
        {
            Destroy(i);
        }
        foreach (GameObject layer in wrongLayersInSceneList)
        {
            Destroy(layer);
        }
        // Clear the lists
        fallingObjectsList.Clear();
        wrongLayersInSceneList.Clear();
    }

    private void LoadQuestionsFromCSV(TextAsset csvFile)
    {
        // Clear the existing questions list
        catchQuestionAnswers.Clear();

        // Split the CSV data into lines
        string[] lines = csvFile.text.Split('\n');

        // Loop through each line of the CSV file and add the question and answers to the qNa list
        for (int i = 0; i < lines.Length; i++)
        {
            string[] row = lines[i].Trim().Split(',');

            if (row.Length != 10) // Error Handling
            {
                Debug.LogErrorFormat("Invalid CSV format on line {0}: expected 10 columns, found {1}", i + 1, row.Length);
                continue;
            }

            string[] questionSpereated = row[0].Split('[', ']');

            List<CatchQuestionFormat> question = new List<CatchQuestionFormat>();

            for (int x = 0; x < questionSpereated.Length; x++)
            {
                CatchQuestionFormat temp = new CatchQuestionFormat();

                temp.Text = questionSpereated[x];

                if (questionSpereated[x] == "1")
                {
                    temp.SlotIdentification = 1;
                    temp.Text = "___";
                }
                else if (questionSpereated[x] == "2")
                {
                    temp.SlotIdentification = 2;
                    temp.Text = "___";
                }
                else if (questionSpereated[x] == "3")
                {
                    temp.SlotIdentification = 3;
                    temp.Text = "___";
                }
                else
                {
                    temp.SlotIdentification = 0;
                }

                question.Add(temp);
            }

                /*CatchQuestionFormat[] question = new CatchQuestionFormat[questionSpereated.Length];
                for(int y = 1; y < question.Length; y++)
                {
                    for(int x = 0;x < questionSpereated.Length;x++)
                    {

                        Debug.Log(question[y]);
                        question[y].Text = questionSpereated[x];

                        if (questionSpereated[0] == "1")
                        {
                            question[y].SlotIdentification = 1;
                            question[y].Text = "___";
                        }
                        else if (questionSpereated[0] == "2")
                        {
                            question[y].SlotIdentification = 2;
                            question[y].Text = "___";
                        }
                        else if (questionSpereated[0] == "3")
                        {
                            question[y].SlotIdentification = 3;
                            question[y].Text = "___";
                        }
                        else
                        {
                            question[y].SlotIdentification = 0;
                        }
                    }
                }*/

            string[] answers = new string[6];
            for (int j = 1; j < 7; j++)
            {
                answers[j - 1] = row[j];
            }
            int correctAnswer1;
            int correctAnswer2;
            int correctAnswer3;

            if (!int.TryParse(row[7], out correctAnswer1)) //Error Handling 
            {
                Debug.LogErrorFormat("Invalid CSV format on line {0}: invalid integer value for correct answer", i + 1);
                continue;
            }
            if (!int.TryParse(row[8], out correctAnswer2)) //Error Handling 
            {
                Debug.LogErrorFormat("Invalid CSV format on line {0}: invalid integer value for correct answer", i + 1);
                continue;
            }
            if (!int.TryParse(row[9], out correctAnswer3)) //Error Handling 
            {
                Debug.LogErrorFormat("Invalid CSV format on line {0}: invalid integer value for correct answer", i + 1);
                continue;
            }

            // Add the question, answers, and reason to the qNa list
            catchQuestionAnswers.Add(new CatchQuestionAnswers { Question = question, Answers = answers, CorrectSlotOne = correctAnswer1, CorrectSlotTwo = correctAnswer2, CorrectSlotThree = correctAnswer3 });
        }

        if (catchQuestionAnswers.Count == 0)
        {
            Debug.LogError("Failed to load any questions from CSV file");
            return;
        }
    }

    public void DropAnswers()
    {
        countToFiveQuestions++;
        StartCoroutine("CycleThroughAnswersThenDrop");
    }

    public void BasketHit(int slotIdentity , string answerText)
    {
        if (slotIdentity != 0)
        {
            // Increase correct answer tracker
            highScore++;
            highScoreCounterText.text = $"{highScore}";
            
            foreach(CatchQuestionFormat word in catchQuestionAnswers[currentQuestionint].Question)
            {
                if(word.SlotIdentification == slotIdentity)
                {
                    word.Text = answerText;
                    break;
                }
            }
            questionText.text = catchQuestionAnswers[currentQuestionint].CombineQuestionText();
        }

        if (currentAnswerint >= 2)
        {
            // Destroy all the falling objects
            foreach (GameObject i in fallingObjectsList)
            {
                Destroy(i);
            }
            // Clear the list
            fallingObjectsList.Clear();
            StopAllCoroutines();

            new WaitForSeconds(1);
            NewQuestion();
        }

        currentAnswerint++;
    }

    public void AnAnswerHitGround()
    {
        foreach (GameObject i in fallingObjectsList)
        {
            // Destroy the falling objects
            Destroy(i);
        }
        // Clear the list
        fallingObjectsList.Clear();

        // Instantiate a layer of blocks to reduce the play area
        GameObject newWrongLayer = Instantiate(WrongLayerPrefab, WrongLayerPosition.position, Quaternion.identity);

        wrongLayersInSceneList.Add(newWrongLayer);
        //
        SpriteRenderer wrongLayerSpriteRenderer = WrongLayerPrefab.GetComponentInChildren<SpriteRenderer>();
        WrongLayerPosition.position = new Vector3(
            WrongLayerPosition.position.x,
            WrongLayerPosition.position.y + wrongLayerSpriteRenderer.bounds.size.y,
            WrongLayerPosition.position.z
            );

        float newYPos = basket.transform.position.y + wrongLayerSpriteRenderer.bounds.size.y;
        basket.UpdateYPosition(newYPos);


        StopAllCoroutines();

        new WaitForSeconds(1);
        NewQuestion();
    }

    private void NewQuestion()
    {
        currentQuestionint++;

        if (countToFiveQuestions >= 5)
        {
            Debug.Log("Five Questions Reached");
            countToFiveQuestions = 0;
            ShowResults();
        }
        else if(currentQuestionint >= catchQuestionAnswers.Count)
        {
            Debug.Log("Reached End of All Questions");
            currentQuestionint = 0;
            questionText.text = catchQuestionAnswers[currentQuestionint].CombineQuestionText();
            DropAnswers();
        }
        else
        {
            questionText.text = catchQuestionAnswers[currentQuestionint].CombineQuestionText();
            DropAnswers();
        }

        currentAnswerint = 0;
    }

    public void ShowResults()
    {
        Debug.Log("Results!");
        catchFinancesMain.gameObject.SetActive(false);
        catchFinancesResults.gameObject.SetActive(true);

        //finalHighScoreText.text = $"Right Answers: {highScore}";
        //expEarned.text = $"Total Exp earned: {expTracker * highScore}";
        //homeButton.SetActive(true);

        foreach(GameObject layer in wrongLayersInSceneList)
        {
            Destroy(layer);
        }
        wrongLayersInSceneList.Clear();

    }

    private IEnumerator CycleThroughAnswersThenDrop()
    {
        
        //int randomIntAnswer = Random.Range(0, questionAnswers[currentQuestionint].Answers.Length);

        for (int i = 0; i < catchQuestionAnswers[currentQuestionint].Answers.Length; i++)
        {
            float spawnPosition = Random.Range(startSpawn.position.x, endSpawn.position.x);
            Vector3 spawnPositionVector = new Vector3(spawnPosition, startSpawn.position.y, 0);

            GameObject spawnedFallingAnswer = Instantiate(wrongAnswer, spawnPositionVector, Quaternion.identity);
            spawnedFallingAnswer.transform.GetChild(0).GetComponent<TextMeshPro>().text = catchQuestionAnswers[currentQuestionint].Answers[i];
            CatchAnswer fallingObjectScript = spawnedFallingAnswer.GetComponent<CatchAnswer>();
            fallingObjectScript.SetLevelManager(this);
            fallingObjectScript.answerText = catchQuestionAnswers[currentQuestionint].Answers[i];

            if (i == catchQuestionAnswers[currentQuestionint].CorrectSlotOne - 1)
            {
                fallingObjectScript.slotNumIdentity = 1;
            }
            else if (i == catchQuestionAnswers[currentQuestionint].CorrectSlotTwo - 1)
            {
                fallingObjectScript.slotNumIdentity = 2;
            }
            else if(i == catchQuestionAnswers[currentQuestionint].CorrectSlotThree - 1)
            {
                fallingObjectScript.slotNumIdentity = 3;
            }
            else
            {
                fallingObjectScript.slotNumIdentity = 0;
            }
            fallingObjectsList.Add(spawnedFallingAnswer);

            /*if (i != catchQuestionAnswers[currentQuestionint].CorrectSlotOne)
            {
                GameObject spawnedFallingAnswer = Instantiate(wrongAnswer, spawnPositionVector, Quaternion.identity);
                spawnedFallingAnswer.transform.GetChild(0).GetComponent<TextMeshPro>().text = catchQuestionAnswers[currentQuestionint].Answers[i];
                CatchAnswer fallingObjectScript = spawnedFallingAnswer.GetComponent<CatchAnswer>();
                fallingObjectScript.SetLevelManager(this);
                fallingObjectsList.Add(spawnedFallingAnswer);
            }
            else
            {
                GameObject spawnedFallingAnswer = Instantiate(correctAnswer, spawnPositionVector, Quaternion.identity);
                spawnedFallingAnswer.transform.GetChild(0).GetComponent<TextMeshPro>().text = catchQuestionAnswers[currentQuestionint].Answers[i];
                CatchAnswer fallingObjectScript = spawnedFallingAnswer.GetComponent<CatchAnswer>();
                fallingObjectScript.SetLevelManager(this);
                fallingObjectsList.Add(spawnedFallingAnswer);
            }*/

            yield return new WaitForSeconds(2);
        }
    }

}


