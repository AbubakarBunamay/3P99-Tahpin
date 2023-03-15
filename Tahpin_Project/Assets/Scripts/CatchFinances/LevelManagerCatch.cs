using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelManagerCatch : MonoBehaviour
{

    [SerializeField] private GameObject correctAnswer;
    [SerializeField] private GameObject wrongAnswer;

    [SerializeField] private Transform startSpawn;
    [SerializeField] private Transform endSpawn;
    [SerializeField] private Transform bottomCollider;

    [SerializeField] private QuestionAnswers[] questionAnswers;

    [SerializeField] private TextMeshPro questionText;
    [SerializeField] private TextMeshPro highScoreCounterText;

    [SerializeField] private GameObject catchFinancesMain;
    [SerializeField] private GameObject catchFinancesResults;

    [SerializeField] private TextMeshPro finalHighScoreText;
    [SerializeField] private TextMeshPro expEarned;

    [SerializeField] private GameObject homeButton;

    [Header("Basket")]
    [SerializeField] private BascketMouse basket;

    [Header("WrongAnswerLayer")]
    [SerializeField] private GameObject WrongLayerPrefab;
    [SerializeField] private Transform WrongLayerPosition;

    private List<GameObject> fallingObjectsList = new List<GameObject>();
    private int currentQuestionint = 0;

    private int highScore = 0;
    private int expTracker = 100;

    private void Start()
    {
        DropAnswers();
        questionText.text = questionAnswers[currentQuestionint].Question;
    }

    public void StartGame()
    {
        DropAnswers();
        questionText.text = questionAnswers[currentQuestionint].Question;
    }

    public void DropAnswers()
    {
        StartCoroutine("CycleThroughAnswersThenDrop");
    }

    public void BasketHit(bool answerWasCorrect)
    {
        if (answerWasCorrect)
        {
            highScore++;
            highScoreCounterText.text = $"{highScore}";
            foreach (GameObject i in fallingObjectsList)
            {
                Destroy(i);
            }
            fallingObjectsList.Clear();
        }
        else
        {
            foreach (GameObject i in fallingObjectsList)
            {
                /*float spawnPositionX = Random.Range(startSpawn.position.x, endSpawn.position.x);
                if (!i.GetComponent<CatchAnswer>().isCorrect)
                {
                    i.transform.position = new Vector3(spawnPositionX, bottomCollider.transform.position.y + 1.220249f, i.transform.position.z);
                    i.tag = "Bottom";
                }
                else
                {
                    Destroy(i);
                }*/
                Destroy(i);
            }
            fallingObjectsList.Clear();

            Instantiate(WrongLayerPrefab,WrongLayerPosition.position,Quaternion.identity);
            SpriteRenderer answerSpriteRenderer = correctAnswer.GetComponent<SpriteRenderer>();

            WrongLayerPosition.position = new Vector3(
                WrongLayerPosition.position.x, 
                WrongLayerPosition.position.y + answerSpriteRenderer.bounds.size.y,
                WrongLayerPosition.position.z
                );

            float newYPos = basket.transform.position.y + answerSpriteRenderer.bounds.size.y;
            basket.UpdateYPosition(newYPos);
        }

        StopAllCoroutines();

        new WaitForSeconds(1);
        NewQuestion();
    }

    private void NewQuestion()
    {
        currentQuestionint++;

        if (currentQuestionint >= questionAnswers.Length)
        {
            ShowResults();
        }
        else
        {
            questionText.text = questionAnswers[currentQuestionint].Question;
            DropAnswers();
        }

    }

    public void ShowResults()
    {
        Debug.Log("Results!");
        catchFinancesMain.gameObject.SetActive(false);
        catchFinancesResults.gameObject.SetActive(true);

        finalHighScoreText.text = $"Right Answers: {highScore}";
        expEarned.text = $"Total Exp earned: {expTracker * highScore}";
        homeButton.SetActive(true);

    }

    private IEnumerator CycleThroughAnswersThenDrop()
    {
        
        //int randomIntAnswer = Random.Range(0, questionAnswers[currentQuestionint].Answers.Length);

        for (int i = 0; i < questionAnswers[currentQuestionint].Answers.Length; i++)
        {
            float spawnPosition = Random.Range(startSpawn.position.x, endSpawn.position.x);
            Vector3 spawnPositionVector = new Vector3(spawnPosition, startSpawn.position.y, 0);
            if (i != questionAnswers[currentQuestionint].CorrectAnswer)
            {
                GameObject spawnedFallingAnswer = Instantiate(wrongAnswer, spawnPositionVector, Quaternion.identity);
                spawnedFallingAnswer.transform.GetChild(0).GetComponent<TextMeshPro>().text = questionAnswers[currentQuestionint].Answers[i];
                fallingObjectsList.Add(spawnedFallingAnswer);
            }
            else
            {
                GameObject spawnedFallingAnswer = Instantiate(correctAnswer, spawnPositionVector, Quaternion.identity);
                spawnedFallingAnswer.transform.GetChild(0).GetComponent<TextMeshPro>().text = questionAnswers[currentQuestionint].Answers[i];
                fallingObjectsList.Add(spawnedFallingAnswer);
            }

            yield return new WaitForSeconds(2);
        }
    }

}
