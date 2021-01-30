using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intersection : MonoBehaviour
{
    public IntersectionType type;
    public IntersectionStatus status = IntersectionStatus.move;
    public List<Transform> pathsAffectingStopPoint = new List<Transform>();
    public List<Transform> pathsAffectedByStopPoint = new List<Transform>();
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
        MovingCarBehavior car = otherObject.gameObject.GetComponent<MovingCarBehavior>();
        if (otherObject.gameObject.tag == "vehicle")
        {
            if (type == IntersectionType.intersectionPoint)
            {

                for (int i = 0; i < pathsAffectingStopPoint.Count; i++)
                {

                    if (car.roadPath.gameObject.name == pathsAffectingStopPoint[i].name)
                    {
                        status = IntersectionStatus.stop;
                    }
                }
                for (int i = 0; i < pathsAffectedByStopPoint.Count; i++)
                {
                    if (status == IntersectionStatus.stop && car.roadPath.gameObject.name == pathsAffectedByStopPoint[i].name)
                    {
                        //car.speed = Mathf.Lerp(car.speed, 0, 15);
                        car.StopCar(true,this.transform.position);
                        //car.move = false;
                        //StartCoroutine( "wait",otherObject.gameObject);
                    }
                }




            }
            else if (type == IntersectionType.light)
            {

            }
        }
      
    }
    void OnTriggerExit(Collider otherObject)
    {
        if(otherObject.gameObject.tag=="vehicle")
        {
            if (type == IntersectionType.intersectionPoint)
            {
                for (int i = 0; i < pathsAffectingStopPoint.Count; i++)
                {

                    if (otherObject.gameObject.GetComponent<MovingCarBehavior>().roadPath.gameObject.name == pathsAffectingStopPoint[i].name)
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
       
    }
    void OnTriggerStay(Collider other)
    {
        MovingCarBehavior car = other.gameObject.GetComponent<MovingCarBehavior>();
        if (other.gameObject.tag == "vehicle")
        {
            if (status == IntersectionStatus.move)
            {
                for (int i = 0; i < pathsAffectedByStopPoint.Count; i++)
                {
                    if (car.roadPath.gameObject.name == pathsAffectedByStopPoint[i].name)
                    {
                        car.move = true;
                        //car.speed = Mathf.Lerp(car.speed, car.originalSpeed, 15);
                        car.StopCar(false,Vector3.zero);
                        //StartCoroutine( "wait",otherObject.gameObject);
                    }
                }
                
            }
        }
        
    }
    IEnumerator wait(GameObject vehicle)
    {
        yield return new WaitForSecondsRealtime(10);
        vehicle.GetComponent<MovingCarBehavior>().move = true;
    }
}
