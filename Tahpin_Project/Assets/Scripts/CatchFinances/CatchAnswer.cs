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
    private ConstantForce2D cf;
    private LevelManagerCatch levelManager;
    public float dir = 0;
    private bool basketContact;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        cf = GetComponent<ConstantForce2D>();
        if(transform.position.x < 0)
        {
            dir = -1;
        }
        else
        {
            dir = 1;
        }
    }

    private void Update()
    {
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
        if(collision.gameObject.layer == 7 || collision.gameObject.CompareTag("FallingAnswer"))
        {
            cf.enabled = false;
        }
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

    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            if((transform.position.x - collision.transform.position.x) < 0)
            {
                dir = 1;
            }
            else
            {
                dir = -1;
            }
            basketContact = true;
            rb.AddRelativeForce(new Vector2(3 * dir, 0));
        }
        else if (collision.gameObject.CompareTag("FallingAnswer") && !basketContact)
        {
            CatchAnswer otherAnswer = collision.gameObject.GetComponent<CatchAnswer>();
            if(dir == otherAnswer.dir)
            {
                dir = dir * -1;
            }
            rb.AddRelativeForce(new Vector2(3 * dir, 0));
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        cf.enabled = true;
        basketContact = false;
    }
}
