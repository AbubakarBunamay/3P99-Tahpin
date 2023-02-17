using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatchAnswer : MonoBehaviour
{

    public bool isCorrect;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isCorrect)
        {
            if (collision.gameObject.CompareTag("Bottom"))
            {
                LevelManagerCatch levelManager = FindObjectOfType<LevelManagerCatch>();
                levelManager.BasketHit(false);
            }
        }
        else
        {
            if (collision.gameObject.CompareTag("Bottom"))
            {
                gameObject.tag = "Bottom";
            }
        }
    }

}
