using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public int damage;
    public float speed;
    public int activeTime;

    private void Start()
    {
        Destroy(gameObject, activeTime);
    }
    private void Update()
    {
        transform.Translate(transform.forward * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name);
        if(other.gameObject.tag.Equals("Enemy"))
        {
            other.gameObject.GetComponent<EnemyBehaviour>().Damage(damage);
        }

        Destroy(gameObject);
    }
}
