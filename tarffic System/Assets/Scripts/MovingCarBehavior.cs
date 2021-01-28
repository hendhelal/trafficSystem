using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingCarBehavior : MonoBehaviour
{
    public CreatePath roadPath;
    public int currentwayPointIndex = 0;
    public float speed;
    public float reachDistance = 1.0f;
    public float rotaionSpeed = 5.0f;
    public string PathName;
    public bool move = true;
    Vector3 lastPos,currentPos;
    
    
    void Start()
    {
       // roadPath = GameObject.Find(PathName).GetComponent<CreatePath>();
        lastPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(move)
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
    //protected void LateUpdate()
    //{
    //    transform.localRotation = new Vector3(0, transform.localEulerAngles.z,0 );
    //}
    void OnCollisionEnter(Collision other)
    {
        //if(other.gameObject.tag=="StopPoint")
        //{
        //    move = false;
        //   StartCoroutine( wait());
        //}

        //else 
        if (other.gameObject.tag == "vehcile")
        {
            if (!other.gameObject.GetComponent<MovingCarBehavior>().move)
                move = false;
            else
                speed -= 5;
        }


        Debug.Log("coliidee");


    }
    void OnCollisionExit(Collision other)
    {
        if (other.gameObject.tag == "vehcile")
        {
            if (!move)
                move = true;
            else
                speed += 5;
        }


        Debug.Log("no coliidee");


    }
    IEnumerator wait()
    {
        yield return new WaitForSecondsRealtime(1);
        move = true;
    }
}
