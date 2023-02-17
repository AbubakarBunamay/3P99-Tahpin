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


    private void Start()
    {
        //This is to set up the first question show, I dont know I need this but work
        question_counter++;
        if (question_counter != 1)
        {
            qNa.RemoveAt(currentQuestion);
        }
        genereateQuestion();
    }

    //Correct Function to proceed
    public void correct()
    {   
        qNa.RemoveAt(currentQuestion);
        genereateQuestion();
    }

    //Setting Up Answers
    void setAnswers()
    {
        for( int x = 0; x< choices.Length; x++)
        {
            choices[x].GetComponent<AnswerScript>().isCorrect = false;
            choices[x].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = qNa[currentQuestion].Answers[x];

            if(qNa[currentQuestion].CorrectAnswer == x + 1)
            {
                choices[x].GetComponent<AnswerScript>().isCorrect = true;
                

            }
        }
    }

    //Generate Questions
    void genereateQuestion()
    {
        if (qNa.Count > 0)
        {

            currentQuestion = Random.Range(0, qNa.Count);

            Questiontxt.text = qNa[currentQuestion].Question;

            setAnswers();

            //qNa.RemoveAt(currentQuestion);

        }
        else
        {
            //Possible UI for such
            Debug.Log("End of Questions");
            //or return to the main menu
        }

    }
}
