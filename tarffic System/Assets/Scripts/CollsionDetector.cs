using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
/// <summary>
///  to check for near cars to control speed
/// </summary>
public class CollsionDetector : MonoBehaviour
{
    Vector3 StartPoint;
    Vector3 Origin;
    public int NoOfRays = 1;
    RaycastHit HitInfo;
    float LengthOfRay = 3;
    int raySpacing = 4;

    void Update()
    {
        //update start point if raycastRay
        StartPoint = new Vector3(transform.position.x, transform.position.y + 0.3f, transform.position.z);
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

                if (HitInfo.collider.gameObject.tag == "vehicle")
                {
                    // to check if cars moving in same dir==>prevent detecting cars in opposite dir
                    var ceilx = car.transform.forward.x;
                    var ceilz = car.transform.forward.z;
                    var ceilx1 = Mathf.Abs(HitInfo.collider.gameObject.transform.forward.x) > 0.7 ? 1 : 0;
                    var ceilz1 = Mathf.Abs(HitInfo.collider.gameObject.transform.forward.z) > 0.7 ? 1 : 0;
                    bool close = false;
                    if (ceilx1 == 1)
                    {
                        if ((car.transform.forward.x >= 0 && HitInfo.collider.gameObject.transform.forward.x >= 0) || (car.transform.forward.x <= 0 && HitInfo.collider.gameObject.transform.forward.x <= 0))
                        {
                            close = Mathf.Abs(Mathf.Abs(ceilx) - ceilx1) < 0.1f;
                        }

                    }
                    else if (ceilz1 == 1)
                    {
                        if ((car.transform.forward.z >= 0 && HitInfo.collider.gameObject.transform.forward.z >= 0) || (car.transform.forward.z <= 0 && HitInfo.collider.gameObject.transform.forward.z <= 0))
                        {
                            close = Mathf.Abs(Mathf.Abs(ceilz) - ceilz1) < 0.1f;
                        }
                    }
                    if (close)
                    {
                        print(gameObject.name + " Collided With " + HitInfo.collider.gameObject.name);
                        //if a car is near and not moving==>stop
                        if (!HitInfo.collider.gameObject.GetComponent<MovingCarBehavior>().move)
                        {
                            car.StopCar(true, HitInfo.collider.gameObject.transform.position, true);
                            car.move = false;
                            car.nearCar = HitInfo.collider.gameObject;
                        }
                        //if car is near ==>control speed
                        else  
                        {
                            if (HitInfo.collider.gameObject.GetComponent<MovingCarBehavior>().speed > 5)
                            {
                                lastHit = HitInfo.collider.gameObject;
                                car.ControlSpeed(HitInfo.collider.gameObject.GetComponent<MovingCarBehavior>().speed - 5);
                                car.slowDown = true;

                            }
                            else
                            {
                                lastHit = HitInfo.collider.gameObject;
                                car.ControlSpeed(HitInfo.collider.gameObject.GetComponent<MovingCarBehavior>().speed);
                                car.slowDown = true;

                            }
                        }

                        continue;
                    }


                }
                //else if(HitInfo.collider.gameObject.tag == "StopPoint")
                //{
                //    if(HitInfo.collider.transform.name==car.stopPointName)
                //    {
                //        if (HitInfo.collider.gameObject.GetComponent<Intersection>().status == IntersectionStatus.move)
                //        {
                //            car.ControlSpeed(car.originalSpeed);
                //            car.move = true;
                //            car.stopPointName = null;
                //        }
                //    }

                //}
                return true;
            }
            else
            {
                if (lastHit != null)
                {
                    var dist = Vector3.Distance(lastHit.transform.position, car.transform.position);

                    if (dist > 35)
                    {
                        car.ControlSpeed(car.originalSpeed);
                        car.slowDown = false;
                        lastHit = null;
                    }

                }


            }

        }

        return false;
    }

}
