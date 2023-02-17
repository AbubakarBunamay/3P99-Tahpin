using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasketKillVolume : MonoBehaviour
{

    [SerializeField] private LevelManagerCatch levelManager;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        CatchAnswer answerScript = collision.gameObject.GetComponent<CatchAnswer>();

        if(answerScript != null)
        {
            if (answerScript.isCorrect)
            {
                Debug.Log("Correct Answer, Good Job!");
                levelManager.BasketHit(true);
            }
            else
            {
                Debug.Log("Wrong Answer Kiddo");
                levelManager.BasketHit(false);
            }
        }
    }
}
