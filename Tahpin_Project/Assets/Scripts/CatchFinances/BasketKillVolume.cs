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
            if (answerScript.slotNumIdentity != 0)
            {
                Debug.Log("Correct Answer, Good Job!");
                levelManager.BasketHit(answerScript.slotNumIdentity, answerScript.answerText);
                answerScript.gameObject.SetActive(false);
            }
            else
            {
                Debug.Log("Wrong Answer Kiddo");
                levelManager.AnAnswerHitGround();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        CatchAnswer answerScript = collision.gameObject.GetComponent<CatchAnswer>();

        if (answerScript != null)
        {
            if (answerScript.slotNumIdentity != 0)
            {
                Debug.Log("Correct Answer, Good Job!");
                levelManager.BasketHit(answerScript.slotNumIdentity, answerScript.answerText);
                answerScript.gameObject.SetActive(false);
            }
            else
            {
                Debug.Log("Wrong Answer Kiddo");
                levelManager.AnAnswerHitGround();
            }
        }
    }
}
