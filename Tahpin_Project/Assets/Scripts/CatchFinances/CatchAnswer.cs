using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatchAnswer : MonoBehaviour
{

    public bool isCorrect;
    private bool onlyOneHit = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isCorrect && onlyOneHit == false)
        {
            if (collision.gameObject.CompareTag("Bottom"))
            {
                onlyOneHit = true;
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
