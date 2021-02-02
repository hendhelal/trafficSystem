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

    public bool move = true;

    public float originalSpeed;
    float lerpDuration = 5;
    Vector3 endPos;
    public string stopPointName;
    public GameObject nearCar;
    float tempSpeed;
    public bool slowDown = false;
    float slowSpeed;
    void Start()
    {
        originalSpeed = speed;

    }

    void FixedUpdate()
    {
        if (nearCar)
        {//if the near car start moving==> start moving
            if (nearCar.GetComponent<MovingCarBehavior>().move)
            {
                transform.position = Vector3.Lerp(transform.position, futurePos, Time.deltaTime);
                StartCoroutine("wait");

            }
        }
        else
        {
            if (slowDown)
            {
                speed = slowSpeed;
            }

            if (lerp)
            {
                transform.position = Vector3.Lerp(transform.position, endPos, Time.deltaTime / 2);
            }

            else if (move)
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
        // decrease speed at turns if not slowing down
        if (speed > 10)
        {
            if (currentwayPointIndex > 0 && currentwayPointIndex < roadPath.pathPoints.Count - 1)
            {
                if (roadPath.pathPoints[currentwayPointIndex].position.x == roadPath.pathPoints[currentwayPointIndex - 1].position.x && roadPath.pathPoints[currentwayPointIndex].position.x != roadPath.pathPoints[currentwayPointIndex + 1].position.x)
                {
                    if (tempSpeed == 0)
                    {
                        tempSpeed = speed;
                        speed = 10;
                    }

                }
                else
                {
                    if (tempSpeed > 0 && !slowDown)
                    {
                        StartCoroutine("IncreaseSpeedGradually", tempSpeed);

                        tempSpeed = 0;
                    }

                }
                if (roadPath.pathPoints[currentwayPointIndex].position.z == roadPath.pathPoints[currentwayPointIndex - 1].position.z && roadPath.pathPoints[currentwayPointIndex].position.z != roadPath.pathPoints[currentwayPointIndex + 1].position.z)
                {

                    if (tempSpeed == 0)
                    {
                        tempSpeed = speed;
                        speed = 10;
                    }
                   
                }
                else
                {
                    if (tempSpeed > 0 && !slowDown)
                    {
                        if (speed < originalSpeed)
                            StartCoroutine("IncreaseSpeedGradually", tempSpeed);

                        tempSpeed = 0;
                    }


                }
            }
        }
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
            print("stopppppppppppppppp");
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
        this.slowSpeed = speed;
        //StartCoroutine("Lerp", speed);
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
    IEnumerator wait()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        StartCoroutine("IncreaseSpeedGradually", originalSpeed);
        lerp = false;
        move = true;
        this.nearCar = null;
    }
}
