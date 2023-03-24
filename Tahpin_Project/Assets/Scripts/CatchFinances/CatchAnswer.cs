using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatchAnswer : MonoBehaviour
{

    public bool isCorrect;
    private bool onlyOneHit = false;
    private LevelManagerCatch levelManager;

    public void SetLevelManager(LevelManagerCatch levelManager)
    {
        this.levelManager = levelManager;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isCorrect && onlyOneHit == false)
        {
            if (collision.gameObject.CompareTag("Bottom"))
            {
                onlyOneHit = true;
                levelManager.BasketHit(false);
            }
        }
        else
        {
            if (collision.gameObject.CompareTag("Bottom"))
            {
                //gameObject.tag = "Bottom";
                Destroy(gameObject);
            }
        }
    }

}
