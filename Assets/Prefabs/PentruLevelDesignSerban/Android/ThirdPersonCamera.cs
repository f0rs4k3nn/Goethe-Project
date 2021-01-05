using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.SceneManagement;

public class ThirdPersonCamera : MonoBehaviour
{
    public bool shouldRotate = true;

    // The target we are following
    public Transform target;
    // The distance in the x-z plane to the target
    public float distance = 10.0f;
    // the height we want the camera to be above the target
    public float height = 5.0f;
    // How much we
    public float heightDamping = 2.0f;
    public float rotationDamping = 3.0f;
    float wantedRotationAngle;
    float wantedHeight;
    float currentRotationAngle;
    float currentHeight;
    Quaternion currentRotation;

    public static float shakeMagnitude = 0f;
    private bool canMove = false;

    private UnityStandardAssets.Cameras.ProtectCameraFromWallClip clipControl;

    public float clipMoveTime = 0.05f;              // time taken to move when avoiding cliping (low value = fast, which it should be)
    public float returnTime = 0.4f;                 // time taken to move back towards desired position, when not clipping (typically should be a higher value than clipMoveTime)
    public float sphereCastRadius = 0.1f;           // the radius of the sphere used to test for object between camera and target
    public bool visualiseInEditor;                  // toggle for visualising the algorithm through lines for the raycast in the editor
    public float closestDistance = 0.5f;            // the closest distance the camera can be from the target
    public bool protecting { get; private set; }    // used for determining if there is an object between the target and the camera
    public string dontClipTag = "Player";           // don't clip against objects with this tag (useful for not clipping against the targeted object)

    private Transform m_Cam;                  // the transform of the camera
    private Transform m_Pivot;                // the point at which the camera pivots around
    public float maxDistance;             // the original distance to the camera before any modification are made
    private float m_MoveVelocity;             // the velocity at which the camera moved
    private float m_CurrentDist;              // the current distance from the camera to the target
    private Ray m_Ray = new Ray();                        // the ray used in the lateupdate for casting between the camera and the target
    private RaycastHit[] m_Hits;              // the hits between the camera and the target
    private RayHitComparer m_RayHitComparer;  // variable to compare raycast hit distances


    private void Awake()
    {
        GameManager.Instance.camera = this;
        shakeMagnitude = 0;
    }

    private void Start()
    {
        Application.targetFrameRate = 144;

        if (SceneManager.GetActiveScene().name == "TodayIsAGreatDayToDie")
        {
            heightDamping = 0;
        }
        else
        {
            heightDamping = 3;
        }


        canMove = true;

        // find the camera in the object hierarchy
        m_Cam = transform;
        m_Pivot = transform;

        //  m_Pivot = m_Cam.parent;
        //maxDistance = m_Cam.localPosition.magnitude;
        m_CurrentDist = maxDistance;

        // create a new RayHitComparer
        m_RayHitComparer = new RayHitComparer();
    }

    public void SetActive(bool isActive)
    {
        canMove = isActive;
    }

    private void LateUpdate()
    {
        // Debug.Log(m_CurrentDist);
        // initially set the target distance
        float targetDist = maxDistance;

        m_Ray.origin =
            m_Pivot.position
            + m_Pivot.forward * sphereCastRadius;

        m_Ray.direction = -m_Pivot.forward;

        // initial check to see if start of spherecast intersects anything
        var cols = Physics.OverlapSphere(m_Ray.origin, sphereCastRadius);

        bool initialIntersect = false;
        bool hitSomething = false;

        // loop through all the collisions to check if something we care about
        for (int i = 0; i < cols.Length; i++)
        {
            if ((!cols[i].isTrigger) &&
                !(cols[i].attachedRigidbody != null && cols[i].attachedRigidbody.CompareTag(dontClipTag)))
            {
                initialIntersect = true;
                break;
            }
        }

        // if there is a collision
        if (initialIntersect)
        {
            m_Ray.origin += m_Pivot.forward * sphereCastRadius;

            // do a raycast and gather all the intersections
            m_Hits = Physics.RaycastAll(m_Ray, maxDistance - sphereCastRadius);
        }
        else
        {
            // if there was no collision do a sphere cast to see if there were any other collisions
            m_Hits = Physics.SphereCastAll(m_Ray, sphereCastRadius, maxDistance + sphereCastRadius);
        }

        // sort the collisions by distance
        Array.Sort(m_Hits, m_RayHitComparer);

        // set the variable used for storing the closest to be as far as possible
        float nearest = Mathf.Infinity;

        // loop through all the collisions
        for (int i = 0; i < m_Hits.Length; i++)
        {
            // only deal with the collision if it was closer than the previous one, not a trigger, and not attached to a rigidbody tagged with the dontClipTag
            if (m_Hits[i].distance < nearest && (!m_Hits[i].collider.isTrigger) &&
                !(m_Hits[i].collider.attachedRigidbody != null &&
                  m_Hits[i].collider.attachedRigidbody.CompareTag(dontClipTag)))
            {
                // change the nearest collision to latest
                nearest = m_Hits[i].distance;
                targetDist = -m_Pivot.InverseTransformPoint(m_Hits[i].point).z;
                hitSomething = true;
            }
        }

        // visualise the cam clip effect in the editor
        if (hitSomething)
        {
            Debug.DrawRay(m_Ray.origin, -m_Pivot.forward * (targetDist + sphereCastRadius), Color.red);
        }


        RaycastHit hit;
        if (Physics.Linecast(target.position, transform.position, out hit))
        {
            //Debug.Log("WOW I  HIT " + hit.distance);
            if (!hit.collider.isTrigger && hit.collider.tag != "CheckPoint")
                targetDist = hit.distance - 5f;
        }

        // hit something so move the camera to a better position
        protecting = hitSomething;
        m_CurrentDist = Mathf.SmoothDamp(m_CurrentDist, targetDist, ref m_MoveVelocity,
                                       m_CurrentDist > targetDist ? clipMoveTime : returnTime);



        m_CurrentDist = Mathf.Clamp(m_CurrentDist, closestDistance, maxDistance);
        //m_Cam.localPosition = -Vector3.forward*m_CurrentDist;

        if (target)
        {
            // Calculate the current rotation angles
            wantedRotationAngle = target.eulerAngles.y;
            wantedHeight = target.position.y + height;
            currentRotationAngle = transform.eulerAngles.y;
            currentHeight = transform.position.y;
            // Damp the rotation around the y-axis
            currentRotationAngle = Mathf.LerpAngle(currentRotationAngle, wantedRotationAngle, rotationDamping * Time.deltaTime);
            // Damp the height
            currentHeight = Mathf.Lerp(currentHeight, wantedHeight, heightDamping * Time.deltaTime);
            // Convert the angle into a rotation
            currentRotation = Quaternion.Euler(0, currentRotationAngle, 0);
            // Set the position of the camera on the x-z plane to:
            // distance meters behind the target
            transform.position = target.position;
            transform.position -= currentRotation * Vector3.forward * m_CurrentDist;
            // Set the height of the camera
            transform.position = new Vector3(transform.position.x, currentHeight, transform.position.z);
            // Always look at the target
            if (shouldRotate)
                transform.LookAt(target);
        }
    }

    public float GetCurrentDistance()
    {
        return m_CurrentDist;
    }


    // comparer for check distances in ray cast hits
    public class RayHitComparer : IComparer
    {
        public int Compare(object x, object y)
        {
            return ((RaycastHit)x).distance.CompareTo(((RaycastHit)y).distance);
        }
    }

}
