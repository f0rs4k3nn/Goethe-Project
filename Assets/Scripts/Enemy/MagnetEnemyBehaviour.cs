using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetEnemyBehaviour : MonoBehaviour
{
    public GameObject platform;
    public Transform[] wayPoints;

    private int currentWaypointIndex;
    private Vector3 currentDestination;
   
    private CharacterController controller;

    private int speed;
    public int walkSpeed;
    public int followSpeed;

    public float waitingTime;
    public float spinInterval;

    private bool isWaiting = false;
    private bool arrivedAtDest = false;

    private bool isFollowingPlayer = false;

    public Transform player;

    // Start is called before the first frame update
    void Start()
    {
        speed = walkSpeed;
        currentWaypointIndex = 0;
        currentDestination = wayPoints[currentWaypointIndex].position;
        controller = GetComponent<CharacterController>();

        player = GameObject.Find("Player").transform;

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 offset = Random.insideUnitSphere;
       
        Vector3 position = transform.position;

        controller.Move( (-1) * (position - (currentDestination + offset)).normalized * speed * Time.deltaTime);

        if(!isFollowingPlayer)
        {
            Vector2 diff = new Vector2(Mathf.Abs(position.x - currentDestination.x), Mathf.Abs(position.z - currentDestination.z));

            if (diff.x < 0.1f && diff.y < 0.1f && !isWaiting)
            {

                isWaiting = true;
                StartCoroutine(SpinWait());

            }
        } else
        {
            currentDestination = player.position;
        }

    }

    private IEnumerator SpinWait()
    {
        transform.rotation = Quaternion.Euler(new Vector3(0.0f, 0, 0.0f));

        int numOfSpins = 0;
        while (numOfSpins <= 4)
        {
            yield return new WaitForSeconds(spinInterval);
            numOfSpins++;
            transform.Rotate(new Vector3(0.0f, 90f + transform.rotation.eulerAngles.y, 0.0f));
        }

        currentWaypointIndex++;

        if(currentWaypointIndex >= wayPoints.Length)
        {
            currentWaypointIndex = 0;
        }

        currentDestination = wayPoints[currentWaypointIndex].position;
        isWaiting = false;
    } 

    public void FollowPlayer()
    {
        isFollowingPlayer = true;
        speed = followSpeed;
        
    }

    public void PlayerEscaped()
    {





        speed = walkSpeed;
        isFollowingPlayer = false;
        currentDestination = wayPoints[currentWaypointIndex].position;
    }


}
