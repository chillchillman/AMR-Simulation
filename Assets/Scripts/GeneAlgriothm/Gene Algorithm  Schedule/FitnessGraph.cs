using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FitnessGraph : MonoBehaviour
{
    [Header("Graph settings")]
    public RectTransform graphContainer;
    public GameObject pointfbx;
    public float graphHeight = 200.0f;
    public float graphWidth = 400.0f;

    public void DisplayGraph (List<float> fitnessHistory)
    {
        foreach(Transform child in graphContainer)
        {
            Destroy(child.gameObject);
        }

        float maxFitness = Mathf.Max(fitnessHistory.ToArray());
        float xStep = graphWidth / (fitnessHistory.Count - 1);

        for (int i = 0 ; i < fitnessHistory.Count ; i++)
        {
            float xPosition = xStep * i;
            float yPosition = fitnessHistory[i] / maxFitness * graphHeight;

            GameObject point = Instantiate(pointfbx, graphContainer);
            point.GetComponent<RectTransform>().anchoredPosition = new Vector2(xPosition, yPosition);
        }
    }
}
