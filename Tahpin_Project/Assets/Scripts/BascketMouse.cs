using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BascketMouse : MonoBehaviour
{

    [SerializeField] private Camera mainCamera;

    private void Update()
    {
        Vector3 mouseWorldPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPosition.z = 0;
        transform.position = mouseWorldPosition;
    }
}
