using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
/// <summary>
/// responsible for dectecting persons and vehicles and organizing their movment
/// </summary>
public class Intersection : MonoBehaviour
{
    public IntersectionType type;
    public IntersectionStatus status = IntersectionStatus.move;
    public List<Transform> pathsAffectingStopPoint = new List<Transform>();
    public List<Transform> pathsAffectedByStopPoint = new List<Transform>();
    Light light;
    public int collisionCount = 0;
    bool done = true;
    // Start is called before the first frame update
    void Start()
    {
        if (type == IntersectionType.light)
        {
            status = IntersectionStatus.green;
            light = this.GetComponentInChildren<Light>();
            light.color = Color.green;
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (type == IntersectionType.light)
        {
            if (status == IntersectionStatus.green)
            {
                light.color = Color.green;
            }
            else if (status == IntersectionStatus.red)
            {
                light.color = Color.red;
            }

            if (done)
            {//switch lights if no car is in collison zone, or if red light
                if (collisionCount == 0 || status == IntersectionStatus.red)
                {
                    done = false;
                     StartCoroutine(SwitchLights());
                }

            }
        }


    }

    void OnTriggerEnter(Collider otherObject)
    {
        if (type == IntersectionType.intersectionPoint)
        {
            if (otherObject.gameObject.tag == "vehicle")
            {
                MovingCarBehavior car = otherObject.gameObject.GetComponent<MovingCarBehavior>();

                    for (int i = 0; i < pathsAffectingStopPoint.Count; i++)
                    {

                        if (car.roadPath.gameObject.name == pathsAffectingStopPoint[i].name)
                        {
                            status = IntersectionStatus.stop;
                            collisionCount++;
                        }
                    }
                    for (int i = 0; i < pathsAffectedByStopPoint.Count; i++)
                    {
                        if (status == IntersectionStatus.stop && car.roadPath.gameObject.name == pathsAffectedByStopPoint[i].name)
                        {
                            car.StopCar(true, this.transform.position);
                        }
                    }
            }
        }
   
        else if (type == IntersectionType.light)
        {

            if (otherObject.gameObject.tag == "vehicle")
            {
                collisionCount++;
                MovingCarBehavior car1 = otherObject.gameObject.GetComponent<MovingCarBehavior>();
                if (status == IntersectionStatus.red)
                {
                      var stopPoint = car1.transform.position+car1.transform.forward*2f;
                      car1.StopCar(true, stopPoint);
                }

            }
            else if (otherObject.gameObject.tag == "person")
            {
                CharacterNavigationController person = otherObject.gameObject.GetComponent<CharacterNavigationController>();
                if (status == IntersectionStatus.green)
                {
                    person.stop = true;

                }

            }



        }


    }
    void OnTriggerExit(Collider otherObject)
    {
        if (otherObject.gameObject.tag == "vehicle")
        {
            if (type == IntersectionType.intersectionPoint)
            {
                for (int i = 0; i < pathsAffectingStopPoint.Count; i++)
                {

                    if (otherObject.gameObject.GetComponent<MovingCarBehavior>().roadPath.gameObject.name == pathsAffectingStopPoint[i].name)
                    {

                        collisionCount--;
                    }
                }
                if (collisionCount == 0)
                {
                    status = IntersectionStatus.move;
                }

            }
            else if (type == IntersectionType.light)
            {
                collisionCount--;
            }
        }

    }
    void OnTriggerStay(Collider other)
    {
        if (type == IntersectionType.intersectionPoint)
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
                            car.StopCar(false, Vector3.zero);
                        }
                    }

                }
              
            }
        }

        else if (type == IntersectionType.light)
        {
            if (other.gameObject.tag == "vehicle")
            {
                MovingCarBehavior car1 = other.gameObject.GetComponent<MovingCarBehavior>();
                if (status == IntersectionStatus.green)
                {
                    car1.move = true;
                    car1.StopCar(false, Vector3.zero);
                }

            }
            else if (other.gameObject.tag == "person")
            {
                CharacterNavigationController person = other.gameObject.GetComponent<CharacterNavigationController>();
                if (status == IntersectionStatus.red)
                {
                    if (status == IntersectionStatus.red)
                    {
                        person.stop = false;
                    }

                }

            }


        }

    }
    IEnumerator SwitchLights()
    {
        yield return new WaitForSecondsRealtime(10);
        if (this.status == IntersectionStatus.red)
        {
            this.status = IntersectionStatus.green;
        }
        else if (this.status == IntersectionStatus.green)
        {
            this.status = IntersectionStatus.red;
        }
        done = true;
    }
}
