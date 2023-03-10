using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class swipery : MonoBehaviour, IDragHandler, IEndDragHandler
{

    // Restrict the drag ablilty between two points
    [SerializeField] private float maxYPosition;    
    private float minYPosition;
    private RectTransform rectTransform;

    // Store the initial position of the panel
    private Vector3 panelLocation;

    // The threshold for the swipe percentage
    public float percentThreshold = 0.2f;

    // The easing factor for the panel movement
    public float easing = 0.5f;

    // The total number of panels
    public int totalPanels = 1;

    // The current panel
    private int currentPanel = 1;

    // Called before the first frame update
    void Start()
    {
        // Store the initial position of the panel
        panelLocation = transform.position;
        minYPosition = panelLocation.y;
    }

    public void OnDrag(PointerEventData data)
    {
        
        // Calculate the difference in y between the press and drag position
        float difference = data.pressPosition.y - data.position.y;

        // Move the panel up or down based on the difference
        transform.position = panelLocation - new Vector3(0, difference, 0);

        if (transform.position.y >= maxYPosition)
        {
            transform.position = new Vector3(panelLocation.x, maxYPosition, panelLocation.z);
        }
        if (transform.position.y <= minYPosition)
        {
            transform.position = new Vector3(panelLocation.x, minYPosition, panelLocation.z);
        }

        Debug.Log(difference);

        

    }


    // Called when the panel is released after being dragged
    //public void OnEndDrag(PointerEventData data)
    //{
    //    // Calculate the swipe percentage based on the press and drag positions
    //    float percentage = (data.pressPosition.x - data.position.x) / Screen.width;

    //    // If the swipe percentage is greater than the threshold, move to the next or previous panel
    //    if (Mathf.Abs(percentage) >= percentThreshold)
    //    {
    //        // Calculate the new location for the panel
    //        Vector3 newLocation = panelLocation;
    //        if (percentage > 0 && currentPanel < totalPanels)
    //        {
    //            // Move to the next panel
    //            currentPanel++;
    //            newLocation += new Vector3(-Screen.width, 0, 0);
    //        }
    //        else if (percentage < 0 && currentPanel > 1)
    //        {
    //            // Move to the previous panel
    //            currentPanel--;
    //            newLocation += new Vector3(Screen.width, 0, 0);
    //        }

    //        // Start the coroutine to smoothly move the panel to the new location
    //        StartCoroutine(SmoothMove(transform.position, newLocation, easing));

    //        // Update the panel location to the new location
    //        panelLocation = newLocation;
    //    }
    //    else
    //    {
    //        // If the swipe percentage is less than the threshold, return the panel to its original position
    //        StartCoroutine(SmoothMove(transform.position, panelLocation, easing));
    //    }
    //}


    public void OnEndDrag(PointerEventData data)
    {
        // Calculate the distance between the current panel location and the locations of all the panels
        float minDistance = float.MaxValue;
        Vector3 nearestLocation = panelLocation;
        foreach (Transform child in transform.parent.transform)
        {
            float distance = Vector3.Distance(transform.position, child.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearestLocation = child.position;
            }
        }

        // Move the panel to the nearest location
        StartCoroutine(Smooth(transform.position, nearestLocation, easing));
        panelLocation = nearestLocation;
    }


    // Coroutine to smoothly move the panel from the start position to the end position over time - For smoothanimation
    IEnumerator Smooth(Vector3 startpos, Vector3 endpos, float seconds)
    {
        float t = 0f;
        while (t <= 1.0)
        {
            t += Time.deltaTime / seconds;
            transform.position = Vector3.Lerp(startpos, endpos, Mathf.SmoothStep(0f, 1f, t));
            yield return null;
        }
    }
}
