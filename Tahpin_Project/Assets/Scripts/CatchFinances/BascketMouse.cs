using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BascketMouse : MonoBehaviour
{

    [SerializeField] private Camera mainCamera;
    [SerializeField] private float minXPosition;
    [SerializeField] private float maxXPosition;
    [SerializeField] private float currentYPosition;

    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Vector3 mouseWorldPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPosition.z = 0;
        mouseWorldPosition.y = currentYPosition;
        //transform.position = mouseWorldPosition;

        //rb.position = mouseWorldPosition;
        rb.MovePosition(mouseWorldPosition);

        // Side Restrictions
        // Fix later
        /*if(transform.position.x <= maxXPosition)
        {
            transform.position = new Vector3(maxXPosition, 0, 0);
        }
        if (transform.position.x >= minXPosition)
        {
            transform.position = new Vector3(minXPosition, 0, 0);
        }*/
    }

    public void UpdateYPosition(float yPos)
    {
        currentYPosition= yPos;
        transform.position = new Vector3(transform.position.x, yPos, transform.position.z);
    }
}
