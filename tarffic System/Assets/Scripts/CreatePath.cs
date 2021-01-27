using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatePath : MonoBehaviour
{
    public Color pathColor =Color.red;
    public List<Transform> pathPoints = new List<Transform>();
    Transform[] pathPointsArray;
   
    void OnDrawGizmos()
    {
        Gizmos.color = pathColor;
        pathPointsArray = GetComponentsInChildren<Transform>();
        pathPoints.Clear();
        foreach (var item in pathPointsArray)
        {
            if(item!= this.transform)
            {
                pathPoints.Add(item);
            }
        }
        for (int i = 0; i < pathPoints.Count; i++)
        {
            Vector3 position = pathPoints[i].position;
            if (i > 0)
            {
                Vector3 previous = pathPoints[i - 1].position;
                Gizmos.DrawLine(previous,position);
                Gizmos.DrawWireSphere(position, 0.3f);
            }

        }
        
    }
}
