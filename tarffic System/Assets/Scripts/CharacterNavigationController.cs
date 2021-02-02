using UnityEngine;


public class CharacterNavigationController : MonoBehaviour
{
    public float movementSpeed = 1;
    public float movementRotation = 1;

    private Animator animator;

    bool finish;
    Vector3 destination;
    public int index = 0;
    public CreatePath path;
    Vector3 StartPoint;
    Vector3 Origin;
    int NoOfRays = 2;
    RaycastHit HitInfo;
    float LengthOfRay = 3, DistanceBetweenRays, DirectionFactor;
    int raySpacing = 4;
    public bool stop;
    private void Start()
    {
        StartPoint = new Vector3(transform.position.x, transform.position.y + 0.3f, transform.position.z);

    }

    private void Update()
    {
        StartPoint = new Vector3(transform.position.x, transform.position.y + 0.3f, transform.position.z);
        if(!stop)
        {
            if (index < path.pathPoints.Count)
            {

                destination = path.pathPoints[index].position;
                float distance = Vector3.Distance(path.pathPoints[index].position, transform.position);
                if (distance < 0.5f)
                    index++;

            }
            else
            {
                index = 0;
                return;
            }
            Vector3 direction = destination - transform.position;

            //animator.SetFloat("Speed", movementSpeed);
            transform.position = Vector3.MoveTowards(transform.position, destination, Time.deltaTime * movementSpeed);

            if (direction != Vector3.zero)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * 4);
            }
        }
  
        isPersonNear();
    }
    CharacterNavigationController lastHit;
    bool isPersonNear()
    {
        CharacterNavigationController person = gameObject.GetComponent<CharacterNavigationController>();
        Origin = StartPoint;
        float initRay = (NoOfRays / 2f) * raySpacing;
        for (float a = -initRay; a <= initRay; a += raySpacing)
        {

            Debug.DrawRay(Origin, Quaternion.Euler(0, a, 0) * this.transform.forward * LengthOfRay, Color.green);

            if (Physics.Raycast(Origin, Quaternion.Euler(0, a, 0) * this.transform.forward, out HitInfo, LengthOfRay))
            {

                if (HitInfo.collider.gameObject.tag == "person")
                {
                    if (HitInfo.collider.gameObject.GetComponent<CharacterNavigationController>().stop )
                    {
                        lastHit = HitInfo.collider.gameObject.GetComponent<CharacterNavigationController>();
                        person.stop = true;
                    }

                }

            }
            else
            {
                if (lastHit != null)
                {
                    if (!lastHit.stop)
                    {
                        person.stop = false;
                        lastHit = null;
                    }

                }
            }
          


        }

        return false;
    }
}
