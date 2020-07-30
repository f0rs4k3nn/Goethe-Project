using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FiringScript : MonoBehaviour
{
    public GameObject bullet;
    private bool canFire = true;
    public Transform firingPoint;
    public float fireRate;

    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetAxis("Fire1") > 0.2)
        {
            if(canFire)
            {
                Transform objTransform = Instantiate(bullet).GetComponent<Transform>();

                //obj.position = firingPoint.position;
                objTransform.position = firingPoint.position;
                objTransform.forward = GetComponent<Transform>().forward;
                Debug.Log(firingPoint.position);

                canFire = false;

                StartCoroutine(CoolDownStart());
            }
        }
    }

    private IEnumerator CoolDownStart()
    {
        yield return new WaitForSeconds(fireRate);

        canFire = true;
    }
}
