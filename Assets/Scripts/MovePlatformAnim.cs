using UnityEngine;

public class MovePlatformAnim : MonoBehaviour
{
    public Transform distanceMin;
    public Transform distanceMax;
    public float maxAnimTime;
    private float _t = 0;      

    void Update()
    {
        _t += Time.fixedDeltaTime;  
        transform.position = Vector3.Lerp(distanceMin.position, distanceMax.position, (Mathf.Sin(_t/maxAnimTime) + 1.0f) / 2.0f); 
    }
}
