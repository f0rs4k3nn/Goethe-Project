using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMeConsoles : MonoBehaviour
{
    public Transform BossConsole;
    public static List<Transform> Waypoints = new List<Transform>();
    public Transform WaypointToFollow;
    private void Awake()
    {
        StartCoroutine(InitializeArrow());        
    }
    void Start()
    {        
        StartCoroutine(LoopCheck());
    }
   
    void Update()
    {
        transform.LookAt(WaypointToFollow);
    }
    IEnumerator InitializeArrow()
    {
        GameObject[] tempObj = GameObject.FindGameObjectsWithTag("SecurityConsole");

        foreach (GameObject obiect in tempObj)
            Waypoints.Add(obiect.transform);

        Debug.Log("Found " + Waypoints.Count + " Consoles");

        while (transform.parent == null)
        {
            Debug.Log("checking for player...");
            transform.SetParent(GameManager.Instance.playerTransform);
            transform.localPosition = new Vector3(0, 2, 0.5f);
            transform.localRotation = new Quaternion(0, 0, 0, 0);
            yield return null;            
        }        
    }
    IEnumerator LoopCheck()
    {
        while (enabled)
        {
            WaypointToFollow = CheckWaypoints();
            yield return new WaitForSeconds(0.5f);
        }       
    }
    private Transform CheckWaypoints()
    {
        Transform target = null;
        if (Waypoints.Count > 0)
        {            
            float distance = 0f;
            foreach(Transform obiect in Waypoints)
            {               
                float dist = Vector3.Distance(transform.position, obiect.position);
                Debug.LogWarning("Checking : " + obiect + ", dist : " + dist);
                if (distance == 0 || dist < distance)
                {
                    distance = dist;
                    target = obiect;
                }
            }            
        }
        else
        {
            return BossConsole;
        }
        
        return target;
    }
}
