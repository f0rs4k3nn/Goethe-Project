using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMeConsoles : MonoBehaviour
{
    public Transform BossConsole;
    public static List<Transform> Waypoints = new List<Transform>();
    Transform WaypointToFollow;
    void Start()
    {
        StartCoroutine(InitializeArrow());          
        StartCoroutine(LoopCheck());
    }
   
    void Update()
    {
        transform.LookAt(WaypointToFollow);
    }
    IEnumerator InitializeArrow()
    {
        while(transform.parent == null)
        {
            Debug.Log("checking for player...");
            transform.SetParent(GameManager.Instance.playerTransform);
            transform.localPosition = new Vector3(0, 2, 0.5f);
            transform.localRotation = new Quaternion(0, 0, 0, 0);
            yield return null;            
        }
        GameObject[] tempObj = GameObject.FindGameObjectsWithTag("SecurityConsole");

        foreach (GameObject obiect in tempObj)
            Waypoints.Add(obiect.transform);

        Debug.Log("Found " + Waypoints.Count + " Consoles");
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
        Transform target;
        if (Waypoints.Count > 0)
        {
            target = Waypoints[0].transform;
            float distance = Vector3.Distance(transform.position, target.position);
            for (int i = 1; i < Waypoints.Count - 1; i++)
            {
                if (Vector3.Distance(transform.position, Waypoints[i].position) < distance)
                {
                    distance = Vector3.Distance(transform.position, Waypoints[i].position);
                    target = Waypoints[i].transform;
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
