using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Script to test if correct/wrong
public class AnswerScript : MonoBehaviour
{
    public bool isCorrect = false; //All Answers are false at first

    public TriviaManager TriviaM; //For Calling the trivia function

    //Answer function if right/wrong
    public void Answer()
    {
        
        if (isCorrect)
        {
            Debug.Log("Correct");
            TriviaM.Correct(true);//proceed to the next question

        }
        else
        {
            Debug.Log("Wrong");
            TriviaM.Correct(false);//proceed to the next question
        }
    }
}
