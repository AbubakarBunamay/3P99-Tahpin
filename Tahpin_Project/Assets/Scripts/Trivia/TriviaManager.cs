using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class TriviaManager : MonoBehaviour
{
    //Creating The Question
    public List<QuestionAnswers> qNa;

    //Button for choices
    public GameObject[] choices;

    //Variable for questions
    public int currentQuestion;

    public TextMeshProUGUI Questiontxt;

    //Seting up first question to show
    public int question_counter = 0;
    private int countToFiveQuestions = 0;


    [SerializeField] private GameObject triviaMain;
    [SerializeField] private GameObject triviaResults;

    //[SerializeField] private TextMeshPro rightAnswerCounterResults;
    //[SerializeField] private TextMeshPro expCounterResults;
    [SerializeField] private GameObject homeButton;

    [SerializeField] private TextMeshProUGUI rightAnswerCounter;
    private int highScore = 0;
    private int exp = 100;

    private Coroutine questionCoroutine;

    //TextAsset
    public TextAsset csvFile;

    //Text Hint/Reason
    public TextMeshProUGUI hint;
    public TextMeshProUGUI reason;

    //The panel 
    public GameObject feedbackPanel;

    // Timer + Clock
    [SerializeField] private float questionTime = 10f;
    [SerializeField] private Image timerImageFill;
    [SerializeField] private float factReadTime = 2f;


    /*private void Start()
    {
        //Questions CSV File
        LoadQuestionsFromCSV(csvFile);

        // This is to set up the first question to show
        question_counter++;
        rightAnswerCounter.text = $"{highScore}";

        if (question_counter != 1)
        {
            qNa.RemoveAt(currentQuestion);
        }

        //Generating Questions
        GenerateQuestion();

        // Print out the number of questions loaded
        Debug.Log("Number of questions: " + qNa.Count);
    }*/

    public void GameStart()
    {
        //Questions CSV File
        LoadQuestionsFromCSV(csvFile);

        // This is to set up the first question to show
        question_counter++;
        rightAnswerCounter.text = $"{highScore}";

        if (question_counter != 1)
        {
            qNa.RemoveAt(currentQuestion);
        }

        // Print out the number of questions loaded
        Debug.Log("Number of questions: " + qNa.Count);
    }

    public void ResetGame()
    {
        highScore = 0;
        exp = 100;
        timerImageFill.fillAmount = 0;
        rightAnswerCounter.text = $"{highScore}";
        triviaMain.SetActive(true);
        triviaResults.SetActive(false);
        feedbackPanel.SetActive(false);
    }

    private void LoadQuestionsFromCSV(TextAsset csvText)
    {
        // Clear the existing questions list
        qNa.Clear();

        // Split the CSV data into lines
        string[] lines = csvText.text.Split('\n');

        // Loop through each line of the CSV file and add the question and answers to the qNa list
        for (int i = 0; i < lines.Length; i++)
        {
            string[] row = lines[i].Trim().Split(',');

            if (row.Length != 7) // Error Handling
            {
                Debug.LogErrorFormat("Invalid CSV format on line {0}: expected 7 columns, found {1}", i + 1, row.Length);
                continue;
            }
            string question = row[0];
            string[] answers = new string[4];
            for (int j = 1; j < 5; j++)
            {
                answers[j - 1] = row[j];
            }
            int correctAnswer;

            if (!int.TryParse(row[5], out correctAnswer)) //Error Handling 
            {
                Debug.LogErrorFormat("Invalid CSV format on line {0}: invalid integer value for correct answer", i + 1);
                continue;
            }

            // Extract the reason column
            string reason = row[6];

            // Add the question, answers, and reason to the qNa list
            qNa.Add(new QuestionAnswers { Question = question, Answers = answers, CorrectAnswer = correctAnswer, Reason = reason });
        }

        if (qNa.Count == 0)
        {
            Debug.LogError("Failed to load any questions from CSV file");
            return;
        }

        // Set up the first question
        GenerateQuestion();
    }


    //Correct Function to proceed
    public void Correct(bool isCorrect)
    {
        if (isCorrect)
        {
            highScore++;
            rightAnswerCounter.text = $"{highScore}";
            hint.text = "Correct!";

        }
        else
        {
            // Display feedback message
            //string feedback = $"Sorry, the correct answer was {qNa[currentQuestion].Answers[qNa[currentQuestion].CorrectAnswer - 1]}";
            //Debug.Log(feedback);
            // Display feedback message
            string feedback = $"Sorry, the correct answer was {qNa[currentQuestion].Answers[qNa[currentQuestion].CorrectAnswer - 1]}";
            hint.text = "Wrong!";
            reason.text = feedback;
            
        }

        //Show Reason 
        //Debug.Log("Reason for correct answer: " + qNa[currentQuestion].Reason);

        //Show Reason 
        reason.text = "Reason: " + qNa[currentQuestion].Reason;

        //Show Panel
        feedbackPanel.SetActive(true);

        qNa.RemoveAt(currentQuestion);
        StopAllCoroutines();
        for (int x = 0; x < choices.Length; x++)
        {
            choices[x].SetActive(false);
        }
        //GenerateQuestion();
        StartCoroutine(InfoWaitTime(factReadTime));
    }

    //Setting Up Answers
    void SetAnswers()
    {
        for (int x = 0; x < choices.Length; x++)
        {
            choices[x].SetActive(true);
            choices[x].GetComponent<AnswerScript>().isCorrect = false;
            choices[x].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = qNa[currentQuestion].Answers[x];

            if (qNa[currentQuestion].CorrectAnswer == x + 1)
            {
                choices[x].GetComponent<AnswerScript>().isCorrect = true;
            }
        }
    }



    //Generate Questions
    public void GenerateQuestion()
    {
        // Stop the previously running coroutine/timer
        if (questionCoroutine != null)
        {
            StopCoroutine(questionCoroutine);
        }


        if(countToFiveQuestions >= 5)
        {
            countToFiveQuestions= 0;
            GameEnd();
        }
        else if (qNa.Count > 0)
        {
            //Randomize the questions
            currentQuestion = Random.Range(0, qNa.Count);

            // Increment Quesiton tracker
            countToFiveQuestions++;

            //Add Questions
            Questiontxt.text = qNa[currentQuestion].Question;

            //Set the answers
            SetAnswers();

            // Hide feedback panel
            //feedbackPanel.SetActive(false);

            // Start a timer to automatically generate the next question after 10 seconds
            questionCoroutine = StartCoroutine(QuestionTimer(questionTime));
        }
        else
        {
            GameEnd();
        }
    }

    private void GameEnd()
    {
        //Possible UI for such
        Debug.Log("End of Questions");
        triviaMain.SetActive(false);
        triviaResults.SetActive(true);
        //rightAnswerCounterResults.text = $"Right Answers: {highScore}";
        //expCounterResults.text = $"Total Exp earned: {exp * highScore}";
        //or return to the main menu
        homeButton.SetActive(true);
        questionCoroutine = null; //Null the coroutine
    }

    // Timer function to generate next question after a set amount of time
    IEnumerator QuestionTimer(float time)
    {
        float elapsedTime = 0f;

        while (elapsedTime < time)
        {
            elapsedTime += Time.deltaTime;
            timerImageFill.fillAmount = elapsedTime / time;
            yield return null;
        }

        //Show Reason 
        reason.text = "Reason: " + qNa[currentQuestion].Reason;

        //Show Panel
        feedbackPanel.SetActive(true);

        // Remove the current question and generate the next one
        qNa.RemoveAt(currentQuestion);

        for (int x = 0; x < choices.Length; x++)
        {
            choices[x].SetActive(false);
        }

        StartCoroutine(InfoWaitTime(factReadTime));
        //GenerateQuestion();
    }

    private IEnumerator InfoWaitTime(float duration)
    {
        float counter = 0f;

        while(counter < duration)
        {
            counter += Time.deltaTime;
            timerImageFill.fillAmount = counter / duration;
            yield return null;
        }

        GenerateQuestion();
        feedbackPanel.SetActive(false);
    }

    

}