using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StopPointsManager : MonoBehaviour
{
    List<Intersection> stopPoints;
    // Start is called before the first frame update
    void Start()
    {
        stopPoints = GetComponentsInChildren<Intersection>().ToList();
    }

    // Update is called once per frame
    void Update()
    {
        var isAllStop = false;
        for (int i = 0; i < stopPoints.Count; i++)
        {
            isAllStop = stopPoints[0].status == IntersectionStatus.stop ? true : false;
        }
      if(isAllStop)
        {
            stopPoints[0].status = IntersectionStatus.move;
        }
    }
}
