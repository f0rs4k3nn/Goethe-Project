using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleScript : MonoBehaviour
{
    public int rotationSpeed;
    private float moveSmoothVelocity;
    public float moveSmoothTime = 0.3f;
    public Transform parentT;
    private bool taken = false;
    public int pointsWorth = 1;

    private Vector3 currenPosition;
    private float currentPosYOffset;
    private float offset;
    public float bobbingIntensity = 0.3f;

    void Start()
    {
        offset = Random.Range(0, 359);
        transform.rotation = Quaternion.Euler(0, offset, 0);
        currenPosition = transform.position;
        parentT = transform.parent.transform;

    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0, rotationSpeed) * Time.deltaTime);

        currentPosYOffset = Mathf.SmoothDamp(currentPosYOffset, Mathf.Sin(Time.timeSinceLevelLoad + offset) * bobbingIntensity, ref moveSmoothVelocity, moveSmoothTime);
        currenPosition = new Vector3(parentT.position.x, parentT.position.y + currentPosYOffset, parentT.position.z);

        transform.position = currenPosition;
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if(!taken && other.tag =="Player")
        {
            taken = true;
            other.GetComponent<PlayerMisc>().IncrementPoints(pointsWorth);
            ScoringSystem.theScore += 1;
            Destroy(gameObject);
        } 
    }
}
