using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/* Controls the Enemy AI */

public class EnemyMovement : MonoBehaviour
{

	public float lookRadius = 10f;  // Detection range for player

	public Transform spawnPoint;
	Transform target;   // Reference to the player
	NavMeshAgent agent; // Reference to the NavMeshAgent

    public float maxIntervalBetweenBeeps = 1.0f;
    public float minIntervalBetweenBeeps = 0.05f;
    private float timeBetweenBeeps;
    private float beepPitch = -3;
    private AudioSource beepSound;



    // Use this for initialization
    void Start()
	{
        //target = GameManager.Instance.playerGameObj.transform;
        beepSound = GetComponent<AudioSource>();
		agent = GetComponent<NavMeshAgent>();

        StartCoroutine(BeepCoroutine());
	}

	// Update is called once per frame
	void Update()
	{
        if(target == null)
        {
            PlayerController player = GameManager.Instance.player;

            if (player == null)
            {
            } else
            {
                target = GameManager.Instance.player.transform;
            }
        }

        //NavMeshAgent.Warp(target.position);

        // Distance to the target
        float distance = Vector3.Distance(target.position, transform.position);

        // If inside the lookRadius
        if (distance <= lookRadius && distance >= 1)
        {
            
            //intensity of the beep in report to the enemy's distance from the player
            float beepIntensity = distance / lookRadius;

            Debug.Log(beepIntensity);

            beepPitch = Mathf.Lerp(3, -1, beepIntensity);
            timeBetweenBeeps = Mathf.Lerp(minIntervalBetweenBeeps, maxIntervalBetweenBeeps, beepIntensity);

            // Move towards the target
            agent.SetDestination(target.position);

            // If within attacking distance
            if (distance <= agent.stoppingDistance)
            {
                FaceTarget();   // Make sure to face towards the target
            }
        }
        else
        {
            beepPitch = -1;
            timeBetweenBeeps = 1;

            if (spawnPoint != null)
                agent.SetDestination(spawnPoint.transform.position);
        }
	}

    private IEnumerator BeepCoroutine()
    {
        yield return new WaitForSeconds(Random.RandomRange(0, 5));

        while(this.gameObject.active)
        {
            beepSound.pitch = beepPitch;
            beepSound.Play();
            yield return new WaitForSeconds(timeBetweenBeeps);
        }     
    }

    //test
    // Rotate to face the target
    void FaceTarget()
	{
		Vector3 direction = (target.position - transform.position).normalized;
		Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
		transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
	}


    // Show the lookRadius in editor
    void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, lookRadius);
	}
}