using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatchAnswer : MonoBehaviour
{

    [SerializeField] float maxSpeed;

    public int slotNumIdentity;
    public string answerText;
    private bool onlyOneHit = false;
    private Rigidbody2D rb;
    private LevelManagerCatch levelManager;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Debug.Log(rb.velocity.magnitude);
        if(rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = Vector2.ClampMagnitude(rb.velocity, maxSpeed);
        }
    }

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
