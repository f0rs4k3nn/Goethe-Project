using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lerp2Points : MonoBehaviour
{
    public Transform startPosition;
    public Transform endPosition;
    public float speed = 1;

    // Start is called before the first frame update
    void Start()
    {
        //startPosition = this.transform;
        //endPosition = GetComponentInChildren<Transform>().transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.localPosition = Vector3.Lerp(startPosition.localPosition, endPosition.localPosition, (Mathf.Sin(speed * Time.time) + 1.0f) / 2.0f);
    }
}
