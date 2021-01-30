using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingCarBehavior : MonoBehaviour
{
    public CreatePath roadPath;
    public int currentwayPointIndex = 0;
    public float speed;
    public float maxSpeed;
    public float maxTorque = 80f;
    public float reachDistance = 0.5f;
    public float rotaionSpeed = 5.0f;
    public string PathName;
    public bool move = true;
    Vector3 lastPos, currentPos;

    public float maxSteerAngle = 45f;
    public WheelCollider FLWheel, FRWheel;
    public float originalSpeed;
    float timeElapsed;
    float lerpDuration = 5;
    Vector3 endPos;
    public string stopPointName;
    public GameObject nearCar;
    void Start()
    {
        originalSpeed = speed;
        // roadPath = GameObject.Find(PathName).GetComponent<CreatePath>();
        lastPos = transform.position;
    }

    void FixedUpdate()
    {
        //ChangeSteerAngle();
        //Move();
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
                transform.position = Vector3.Lerp(transform.position, endPos, Time.deltaTime);
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
    void ChangeSteerAngle()
    {
        Vector3 releativeVector = transform.InverseTransformPoint(roadPath.pathPoints[currentwayPointIndex].position);
        float steerAngle = (releativeVector.x / releativeVector.magnitude) * maxSteerAngle;
        FLWheel.steerAngle = steerAngle;
        FRWheel.steerAngle = steerAngle;
        float distance = Vector3.Distance(roadPath.pathPoints[currentwayPointIndex].position, transform.position);
        print(distance);
        if (distance < reachDistance)
            currentwayPointIndex++;
        if (currentwayPointIndex >= roadPath.pathPoints.Count)
            currentwayPointIndex = 0;

    }
    void Move()
    {
        speed = 2 * Mathf.PI * FLWheel.radius * FLWheel.rpm * 60 / 1000;
        if (speed < maxSpeed)
        {
            FLWheel.motorTorque = maxTorque;
            FRWheel.motorTorque = maxTorque;
        }
        else
        {
            FLWheel.motorTorque = 0;
            FRWheel.motorTorque = 0;
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
        this.speed = speed;
        //StartCoroutine("Lerp", speed);
    }
    // Update is called once per frame
    //void Update()
    //{
    //    if(move)
    //    {
    //        float distance = Vector3.Distance(roadPath.pathPoints[currentwayPointIndex].position, transform.position);
    //        transform.position = Vector3.MoveTowards(transform.position, roadPath.pathPoints[currentwayPointIndex].position, Time.deltaTime * speed);
    //        var rotation = Quaternion.LookRotation(roadPath.pathPoints[currentwayPointIndex].position - transform.position);
    //        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * rotaionSpeed);
    //        if (distance < reachDistance)
    //            currentwayPointIndex++;
    //        if (currentwayPointIndex >= roadPath.pathPoints.Count)
    //            currentwayPointIndex = 0;
    //    }

    //}
    //protected void LateUpdate()
    //{
    //    transform.localRotation = new Vector3(0, transform.localEulerAngles.z,0 );
    //}
    //void OnCollisionEnter(Collision other)
    //{
    //    //if(other.gameObject.tag=="StopPoint")
    //    //{
    //    //    move = false;
    //    //   StartCoroutine( wait());
    //    //}

    //    //else 
    //    if (other.gameObject.tag == "vehicle")
    //    {
    //        if (!other.gameObject.GetComponent<MovingCarBehavior>().move)
    //            speed = Mathf.Lerp(speed, 0, 10);
    //        else
    //            speed = Mathf.Lerp(speed, other.gameObject.GetComponent<MovingCarBehavior>().speed - 5, 5);
    //    }


    //    Debug.Log("coliidee");


    //}
    //void OnCollisionExit(Collision other)
    //{
    //    if (other.gameObject.tag == "vehicle")
    //    {
    //        if (!move)
    //            Mathf.Lerp(0, originalSpeed, 10);
    //        else
    //            speed = Mathf.Lerp(speed, originalSpeed, 5);
    //    }


    //    Debug.Log("no coliidee");


    //}
    IEnumerator wait()
    {
        yield return new WaitForSecondsRealtime(5);
        speed = originalSpeed;
        lerp = false;
        move = true;
        this.nearCar = null;
    }
}
