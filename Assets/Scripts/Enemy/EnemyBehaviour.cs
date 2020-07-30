using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    private Transform target;
    public float speed;
    private CharacterController controller;
    private int healthPoints;
    public int maxHealth;

    // Start is called before the first frame update
    void Start()
    {
        healthPoints = maxHealth;
        target = GameObject.Find("Player").GetComponent<Transform>();
        controller = GetComponent<CharacterController>();
    }

    public void Damage(int damage)
    {
        healthPoints = healthPoints - damage;

        if(healthPoints <= 0)
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit ray;
        Vector3 movingDirection = (target.position - transform.position);

        if (Physics.Raycast(transform.position, movingDirection, out ray))
        {
            string name = ray.collider.gameObject.name;
            Debug.Log(name);

            if(name.Equals("Player"))
            {
                controller.Move( movingDirection * speed * Time.deltaTime);
            }
        }

         
    }
}
