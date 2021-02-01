using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingCarBehavior : MonoBehaviour
{
    public CreatePath roadPath;
    public int currentwayPointIndex = 0;
    public float speed;
    public float maxSpeed;
   
    public float reachDistance = 0.5f;
    public float rotaionSpeed = 5.0f;
    public string PathName;
    public bool move = true;
    Vector3 lastPos, currentPos;
    public float originalSpeed;
    float timeElapsed;
    float lerpDuration = 5;
    Vector3 endPos;
    public string stopPointName;
    public GameObject nearCar;
    public int x, z;
    public float x1, z1;
    float tempSpeed;
    public bool slowDown = false;
    void Start()
    {
        originalSpeed = speed;
        // roadPath = GameObject.Find(PathName).GetComponent<CreatePath>();
        lastPos = transform.position;
    }

    void FixedUpdate()
    {

      
        if (nearCar)
        {
            if (nearCar.GetComponent<MovingCarBehavior>().move)
            {
                transform.position = Vector3.Lerp(transform.position, futurePos, Time.deltaTime);
                 StartCoroutine("wait");
                // futurePos = Vector3.zero;
            }
            //
        }
        else
        {
            if (lerp)
            {
                transform.position = Vector3.Lerp(transform.position, endPos, Time.deltaTime/2);
            }

            else
            {
             
                

                float distance = Vector3.Distance(roadPath.pathPoints[currentwayPointIndex].position, transform.position);
                transform.position = Vector3.MoveTowards(transform.position, roadPath.pathPoints[currentwayPointIndex].position, Time.deltaTime * speed);
                var rotation = Quaternion.LookRotation(roadPath.pathPoints[currentwayPointIndex].position - transform.position);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * rotaionSpeed);
                if (distance < reachDistance)
                    currentwayPointIndex++;
                if (currentwayPointIndex >= roadPath.pathPoints.Count)
                    currentwayPointIndex = 0;
            }
        }


    }
    void Update()
    {
        if (!slowDown)
        {
            if (currentwayPointIndex > 0 && currentwayPointIndex < roadPath.pathPoints.Count - 1)
            {
                if (roadPath.pathPoints[currentwayPointIndex].position.x == roadPath.pathPoints[currentwayPointIndex - 1].position.x)
                {
                    if (roadPath.pathPoints[currentwayPointIndex].position.x != roadPath.pathPoints[currentwayPointIndex + 1].position.x)
                    {
                        if (tempSpeed == 0)
                        {
                            tempSpeed = speed;
                            speed = 10;
                        }

                    }
                    else
                    {
                        if (!slowDown)
                        {
                            if (tempSpeed > 0)
                            {
                                StartCoroutine("IncreaseSpeedGradually", tempSpeed);
                               
                                tempSpeed = 0;
                            }
                        }

                    }
                }
                else if (roadPath.pathPoints[currentwayPointIndex].position.z == roadPath.pathPoints[currentwayPointIndex - 1].position.z)
                {
                    if (roadPath.pathPoints[currentwayPointIndex].position.z != roadPath.pathPoints[currentwayPointIndex + 1].position.z)
                    {
                        if (tempSpeed == 0)
                        {
                            tempSpeed = speed;
                            speed = 10;
                        }

                    }
                    else
                    {
                        if (!slowDown)
                        {
                            if (tempSpeed > 0)
                            {
                                if (speed < originalSpeed)
                                    StartCoroutine("IncreaseSpeedGradually", tempSpeed);
                                
                                tempSpeed = 0;
                            }
                        }

                    }
                }
            }
        }
    }
  
    IEnumerator Lerp(float endValue)
    {
        float timeElapsed = 0;
        float initialValue = speed;
        if (!move)
        {
            initialValue = 0;
        }
        while (timeElapsed < lerpDuration)
        {
            speed = Mathf.Lerp(initialValue, endValue, timeElapsed / lerpDuration);
            timeElapsed += Time.deltaTime;

            yield return null;
        }

        speed = endValue;
        lerp = false;
        move = true;
        
    }
    IEnumerator IncreaseSpeedGradually(float endValue)
    {
        float timeElapsed = 0;
        float initialValue = speed;
        float duration = 5;
        while (timeElapsed < lerpDuration)
        {
            speed = Mathf.Lerp(initialValue, endValue, timeElapsed / duration);
            timeElapsed += Time.deltaTime;

            yield return null;
        }

        speed = endValue;
       
    }
    bool lerp = false;
    Vector3 futurePos = Vector3.zero;
    public void StopCar(bool doStop, Vector3 endPo, bool nearCar = false)
    {
        float dist = 2f;
        if (nearCar)
        {
            dist = 4;
            futurePos = endPo;
        }

        if (Mathf.CeilToInt(this.transform.position.x) == Mathf.CeilToInt(endPo.x))
        {

            endPo = endPo + new Vector3(0, 0, dist) * -this.transform.forward.z;
        }
       if (Mathf.CeilToInt(this.transform.position.z) == Mathf.CeilToInt(endPo.z))
        {

            endPo = endPo + new Vector3(dist, 0, 0) * -this.transform.forward.x;
        }
        if (doStop)
        {
            endPos = new Vector3(endPo.x, 0, endPo.z);
            lerp = true;
            move = false;
        }
        else
        {
            //StartCoroutine("Lerp",originalSpeed);
            speed = originalSpeed;
            lerp = false;
            move = true;
            //this.nearCar = null;
        }

        //StartCoroutine("Lerp",speed);
    }
    public void ControlSpeed(float speed)
    {
        StopAllCoroutines();
        print("slowDown");
        this.speed = speed;
        //StartCoroutine("Lerp", speed);
    }
    
    IEnumerator wait()
    {
        yield return new WaitForSecondsRealtime(2);
        speed = originalSpeed;
        lerp = false;
        move = true;
        this.nearCar = null;
    }
}
