using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollsionDetector : MonoBehaviour
{
    public float MovingForce;
    Vector3 StartPoint,startptz;
    Vector3 Origin;
    public int NoOfRays = 10;
    int i;
    RaycastHit HitInfo;
    float LengthOfRay=5, DistanceBetweenRays, DirectionFactor;
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
        StartPoint = new Vector3(transform.position.x , transform.position.y,transform.position.z);
        var z = (gameObject.GetComponent<BoxCollider>().size.z/2)-gameObject.GetComponent<BoxCollider>().center.z;
        // First ray origin point for this frame
        //if (transform.rotation.eulerAngles.y + 5 >= 90 || Mathf.Approximately(transform.rotation.eulerAngles.y, 90))
        //{
        //    StartPoint = new Vector3(transform.position.x+2, transform.position.y, GetComponent<Collider>().bounds.min.z + margin);
        //    DistanceBetweenRays = (GetComponent<Collider>().bounds.size.z - 2 * margin) / (NoOfRays - 1);
        //    isZ = true;
        //}
        //else
        //{
        //    StartPoint = new Vector3(GetComponent<Collider>().bounds.min.x + margin, transform.position.y, transform.position.z+2);
        //    isZ = false;
        //    DistanceBetweenRays = (GetComponent<Collider>().bounds.size.x - 2 * margin) / (NoOfRays - 1); 
        //}
        if (!IsCollidingVertically())
        {
            // transform.Translate(Vector3.up * MovingForce * Time.deltaTime * DirectionFactor);
            print("===========");
        }
        
    }
    
        bool IsCollidingVertically()
    {
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
                    gameObject.GetComponent<MovingCarBehavior>().speed = 10;
                }
                return true;
            }
            //else
            //{
            //    gameObject.GetComponent<MovingCarBehavior>().speed = 20;

            //}

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
