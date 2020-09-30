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
		target = GameManager.Instance.playerGameObj.transform;

		agent = GetComponent<NavMeshAgent>();
	}

	// Update is called once per frame
	void Update()
	{
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

<<<<<<< HEAD
    private void OnTriggerEnter(Collider other)
    {
		if (other.tag == "Player")
		{
			other.GetComponent<PlayerDeath>().CheckPoint();
		}
    }
=======
<<<<<<< HEAD
=======
>>>>>>> 15243ef152d07798785876ad54a68a3b7d9d81b6

>>>>>>> a479fadd80434c9355e68466a7cb4e5563c01d02
    // Show the lookRadius in editor
    void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, lookRadius);
	}
}