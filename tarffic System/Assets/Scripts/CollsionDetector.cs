using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CollsionDetector : MonoBehaviour
{
    public float MovingForce;
    Vector3 StartPoint,startptz;
    Vector3 Origin;
    public int NoOfRays = 2;
    int i;
    RaycastHit HitInfo;
    float LengthOfRay=3, DistanceBetweenRays, DirectionFactor;
    float margin = 0.015f;
    Ray ray;
    bool isZ = false;
    int raySpacing = 2;
    void Start()
    {
        //Length of the Ray is distance from center to edge
       // LengthOfRay = GetComponent<Collider>().bounds.extents.z;

        //Initialize DirectionFactor for upward direction
        DirectionFactor = GetComponent<Collider>().bounds.extents.z;
    }
    void Update()
    {
        StartPoint = new Vector3(transform.position.x , transform.position.y +0.3f,transform.position.z);
        isCarNear();
        
    }
    GameObject lastHit;
        bool isCarNear()
    {
        MovingCarBehavior car = gameObject.GetComponent<MovingCarBehavior>();
        Origin = StartPoint;
        float initRay = (NoOfRays / 2f) * raySpacing;
        for (float a = -initRay; a <= initRay; a += raySpacing)
        {
           
            Debug.DrawRay(Origin, Quaternion.Euler(0, a, 0) * this.transform.forward * LengthOfRay, Color.green);

            if (Physics.Raycast(Origin, Quaternion.Euler(0, a, 0) * this.transform.forward, out HitInfo, LengthOfRay))
            {
               
                print("Collided With " + HitInfo.collider.gameObject.name);
                // Negate the Directionfactor to reverse the moving direction of colliding cube(here cube2)
                //if(Mathf.Approximately( transform.rotation.y,90))
                //    {
                //    DirectionFactor = -DirectionFactor;

                //}
                if (HitInfo.collider.gameObject.tag == "vehicle")
                {
                    if(!HitInfo.collider.gameObject.GetComponent<MovingCarBehavior>().move)
                    {
                        
                        car.StopCar(true, HitInfo.collider.gameObject.transform.position,true);
                       
                        car.move= false;
                        car.nearCar = HitInfo.collider.gameObject;
                    }
                    
                    else if (car.speed==car.originalSpeed)
                    {
                        lastHit = HitInfo.collider.gameObject;
                        print("news: " + (HitInfo.collider.gameObject.GetComponent<MovingCarBehavior>().speed - 5));
                        car.ControlSpeed(HitInfo.collider.gameObject.GetComponent<MovingCarBehavior>().speed - 5);
                    }
                    
                   
                }
                else if(HitInfo.collider.gameObject.tag == "StopPoint")
                {
                    if(HitInfo.collider.transform.name==car.stopPointName)
                    {
                        if (HitInfo.collider.gameObject.GetComponent<Intersection>().status == IntersectionStatus.move)
                        {
                            car.ControlSpeed(car.originalSpeed);
                            car.move = true;
                            car.stopPointName = null;
                        }
                    }
                 
                }
                return true;
            }
            else
            {
                if (lastHit != null)
                {
                    //if (lastHit.GetComponent<MovingCarBehavior>().move)
                    //{
                    //    car.ControlSpeed(car.originalSpeed);
                    //    car.move = true;
                    //}
                    var dist = Vector3.Distance(lastHit.transform.position, car.transform.position);
                    print("dist:" + dist);
                    if(dist>15)
                    {
                        car.ControlSpeed(car.originalSpeed);
                        lastHit = null;
                    }
                   
                }


            }

        }
        //for (i = 0; i < NoOfRays; i++)
        //{
        //    // Ray to be casted.
        //    ray = new Ray(Origin, transform.forward * DirectionFactor);

        //    //Draw ray on screen to see visually. Remember visual length is not actual length.
        //    Debug.DrawRay(Origin, transform.forward * DirectionFactor, Color.yellow);
        //    if (Physics.Raycast(ray, out HitInfo, LengthOfRay))
        //    {
        //        print("Collided With " + HitInfo.collider.gameObject.name);
        //        // Negate the Directionfactor to reverse the moving direction of colliding cube(here cube2)
        //        //if(Mathf.Approximately( transform.rotation.y,90))
        //        //    {
        //        //    DirectionFactor = -DirectionFactor;

        //        //}
        //        if (HitInfo.collider.gameObject.tag == "vehicle")
        //        {
        //            gameObject.GetComponent<MovingCarBehavior>().speed = 10;
        //        }
        //        return true;
        //    }
        //    else
        //    {
        //        gameObject.GetComponent<MovingCarBehavior>().speed = 20;

        //    }
        //    //if (Mathf.Approximately(transform.rotation.y, 90))
        //    //{
        //    //    var DistanceBetweenRaysz = (GetComponent<Collider>().bounds.size.z - 2 * margin) / (NoOfRays - 1);
        //    //    Origin += new Vector3(0, 0, DistanceBetweenRaysz);

        //    //}
        //    //else
        //    //{

        //    //    Origin += new Vector3(DistanceBetweenRays, 0, 0);
        //    //}
        //    if (isZ)
        //        Origin += new Vector3(0, 0, DistanceBetweenRays);
        //    else
        //        Origin += new Vector3(DistanceBetweenRays, 0, 0);
        //}
        return false;
    }
}
