using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeLogger : MonoBehaviour
{

    public float currentTime;
    public bool logTime = false;

    private void Update()
    {
        if (logTime)
        {
            currentTime += Time.deltaTime;
        }
    }

    public void Reset()
    {

    }

}
