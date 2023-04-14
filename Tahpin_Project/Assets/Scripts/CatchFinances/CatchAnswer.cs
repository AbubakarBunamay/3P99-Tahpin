using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatchAnswer : MonoBehaviour
{

    public int slotNumIdentity;
    public string answerText;
    private bool onlyOneHit = false;
    private LevelManagerCatch levelManager;

    public void SetLevelManager(LevelManagerCatch levelManager)
    {
        this.levelManager = levelManager;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (slotNumIdentity != 0 && onlyOneHit == false)
        {
            if (collision.gameObject.CompareTag("Bottom"))
            {
                onlyOneHit = true;
                levelManager.AnAnswerHitGround();
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
