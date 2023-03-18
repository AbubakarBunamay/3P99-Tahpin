using System.Collections;
using System.Collections.Generic;
using System.IO;
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


    [SerializeField] private GameObject triviaMain;
    [SerializeField] private GameObject triviaResults;

    [SerializeField] private TextMeshPro rightAnswerCounterResults;
    [SerializeField] private TextMeshPro expCounterResults;
    [SerializeField] private GameObject homeButton;

    [SerializeField] private TextMeshProUGUI rightAnswerCounter;
    private int highScore = 0;
    private int exp = 100;

    private Coroutine questionCoroutine;
    private void Start()
    {
        //Questions CSV File
        LoadQuestionsFromCSV("Assets/Scripts/Trivia/questions.csv");

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
    }

    private void LoadQuestionsFromCSV(string csvFilePath)
    {
        // Clear the existing questions list
        qNa.Clear();

        // Read the CSV file and split the lines into an array
        string[] lines = File.ReadAllLines(csvFilePath);

        // Loop through each line of the CSV file and add the question and answers to the qNa list
        for (int i = 0; i < lines.Length; i++)
        {
            string[] row = lines[i].Split(',');
            string question = row[0];
            string[] answers = new string[4];
            for (int j = 1; j < 5; j++)
            {
                answers[j - 1] = row[j];
            }
            int correctAnswer = int.Parse(row[5]);

            // Add the question and answers to the qNa list
            qNa.Add(new QuestionAnswers { Question = question, Answers = answers, CorrectAnswer = correctAnswer });

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
        }
        else
        {
            // Display feedback message
            string feedback = $"Sorry, the correct answer was {qNa[currentQuestion].Answers[qNa[currentQuestion].CorrectAnswer - 1]}";
            Debug.Log(feedback);
        }

        qNa.RemoveAt(currentQuestion);
        GenerateQuestion();
    }

    //Setting Up Answers
    void SetAnswers()
    {
        for (int x = 0; x < choices.Length; x++)
        {
            choices[x].GetComponent<AnswerScript>().isCorrect = false;
            choices[x].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = qNa[currentQuestion].Answers[x];

            if (qNa[currentQuestion].CorrectAnswer == x + 1)
            {
                choices[x].GetComponent<AnswerScript>().isCorrect = true;
            }
        }
    }

    //Generate Questions
    void GenerateQuestion()
    {
        // Stop the previously running coroutine/timer
        if (questionCoroutine != null)
        {
            StopCoroutine(questionCoroutine);
        }


        if (qNa.Count > 0)
        {
            //Randomize the questions
            currentQuestion = Random.Range(0, qNa.Count);

            //Add Questions
            Questiontxt.text = qNa[currentQuestion].Question;

            //Set the answers
            SetAnswers();

            // Start a timer to automatically generate the next question after 10 seconds
            questionCoroutine = StartCoroutine(QuestionTimer(5f));
        }
        else
        {
            //Possible UI for such
            Debug.Log("End of Questions");
            triviaMain.SetActive(false);
            triviaResults.SetActive(true);
            rightAnswerCounterResults.text = $"Right Answers: {highScore}";
            expCounterResults.text = $"Total Exp earned: {exp * highScore}";
            //or return to the main menu
            homeButton.SetActive(true);
            questionCoroutine = null; //Null the coroutine
        }
    }

    // Timer function to generate next question after a set amount of time
    IEnumerator QuestionTimer(float time)
    {
        float elapsedTime = 0f;

        while (elapsedTime < time)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Remove the current question and generate the next one
        qNa.RemoveAt(currentQuestion);
        GenerateQuestion();
    }

    

}