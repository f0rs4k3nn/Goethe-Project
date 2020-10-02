using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bouncy : MonoBehaviour
{
    public float moveSmoothTime = 0.3f;
    public float bobbingIntensity = 0.3f;

    private float moveSmoothVelocity;
    private float currentPosYOffset;
    private float offset;
    private float previousOffset;

    void Start()
    {
        offset = Random.Range(0, 359);
        previousOffset = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        currentPosYOffset = Mathf.SmoothDamp(currentPosYOffset, Mathf.Sin(Time.timeSinceLevelLoad + offset) * bobbingIntensity, ref moveSmoothVelocity, moveSmoothTime);

        transform.localPosition += new Vector3(0, currentPosYOffset, 0);
        previousOffset = currentPosYOffset;
    }
}
