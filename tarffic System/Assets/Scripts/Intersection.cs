using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intersection : MonoBehaviour
{
    public IntersectionType type;
    public IntersectionStatus status = IntersectionStatus.move;
    public List<Transform> pathsAffectingStopPoint = new List<Transform>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter(Collider otherObject)
    {
        if (type == IntersectionType.intersectionPoint)
        {
            for (int i = 0; i < pathsAffectingStopPoint.Count; i++)
            {
                if (otherObject.gameObject.GetComponent<MovingCarBehavior>().PathName==pathsAffectingStopPoint[i].name)
                {
                    status = IntersectionStatus.stop;
                }
            }
          
            Debug.Log("intpoint eee");
            if (status == IntersectionStatus.stop)
            {
                otherObject.gameObject.GetComponent<MovingCarBehavior>().move = false;
                //StartCoroutine( "wait",otherObject.gameObject);
            }
            
           

        }
        else if (type == IntersectionType.light)
        {

        }
    }
    void OnTriggerExit(Collider otherObject)
    {
        if (type == IntersectionType.intersectionPoint)
        {
            for (int i = 0; i < pathsAffectingStopPoint.Count; i++)
            {
                if (otherObject.gameObject.GetComponent<MovingCarBehavior>().PathName == pathsAffectingStopPoint[i].name)
                {
                    status = IntersectionStatus.move;
                }
            }
           
            //if (status == IntersectionStatus.stop)
            //{
            //    status = IntersectionStatus.move;
            //    StartCoroutine( "wait",otherObject.gameObject);
            //}


        }
        else if (type == IntersectionType.light)
        {
           
        }
    }
    void OnTriggerStay(Collider other)
    {
        if (status == IntersectionStatus.move)
        {
            other.gameObject.GetComponent<MovingCarBehavior>().move = true;
            //StartCoroutine( "wait",otherObject.gameObject);
        }
    }
    IEnumerator wait(GameObject vehicle)
    {
        yield return new WaitForSecondsRealtime(10);
        vehicle.GetComponent<MovingCarBehavior>().move = true;
    }
}
