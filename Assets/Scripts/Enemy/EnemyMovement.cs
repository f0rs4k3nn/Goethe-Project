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

	// Use this for initialization
	void Start()
	{
		//target = GameManager.Instance.playerGameObj.transform;

		agent = GetComponent<NavMeshAgent>();
	}

	// Update is called once per frame
	void Update()
	{
        if(target == null)
        {
            PlayerController player = GameManager.Instance.player;

            if (player == null)
            {
                return;
            } else
            {
                target = GameManager.Instance.player.transform;

            }
        }

		// Distance to the target
		float distance = Vector3.Distance(target.position, transform.position);

		// If inside the lookRadius
		if (distance <= lookRadius)
		{
			// Move towards the target
			agent.SetDestination(target.position);

			// If within attacking distance
			if (distance <= agent.stoppingDistance)
			{
				FaceTarget();   // Make sure to face towards the target
			}
		}
		else if( spawnPoint != null)
			agent.SetDestination(spawnPoint.transform.position);
	}

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