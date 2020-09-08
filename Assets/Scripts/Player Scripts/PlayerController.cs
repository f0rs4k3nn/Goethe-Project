using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;

public class PlayerController : MonoBehaviour
{
    public float walkSpeed = 2;
    public float runSpeed = 6;
    public int maxSpeed = 1;

    public float turnSmoothTime = 0.2f;
    public float turnSmoothTimeAir = 0.5f;

    float turnSmoothVelocity;
    CharacterController player;

    public float speedSmoothTime = 0.1f;
    public float speedSmoothTimeAir = 0.2f;
    Vector3 speedSmoothVelocity;

    Vector3 currentSpeed;
    Vector3 velocity;
    Vector3 movementDir = Vector3.zero;

   // private GameObject temporaryParent;
    private Transform parentTransform;
    private Vector3 previousParentPosition;
    private Vector3 previousParentRotation;
    private Vector3 parentMovement;

    public Transform groundCheck;
    public Transform ceilingCheck;
    public float groundDistance;
    public LayerMask groundMask;
    private bool isGrounded;
    public float jumpForce;
    public float gravityIntensity;
    private bool jumpKeyReleased = true;
   // public bool gravityIsReversed = false;

    //variables used for the progressive jump when the jump button is held
    private float accumulatedJumpPower; //force added since last jump
    public float accumulativeJumpLimit; //how much force can be added
    public float progressiveJumpPower; //how much force to add while jump is still pressed
    public bool doubleJumpUnlocked; // boolean to see if the player can double jump yet
    public bool canDoubleJump;


    public int maxFallSpeed;
    private bool canMove = false;


    /**
     *Animation variables 
     **/
    private Animator m_Animator;
    [SerializeField] float m_RunCycleLegOffset = 0.2f; //specific to the character in sample assets, will need to be modified to work with others
    [SerializeField] float m_MoveSpeedMultiplier = 1f;
    [SerializeField] float m_AnimSpeedMultiplier = 1f;
    const float k_Half = 0.5f;
    float m_TurnAmount;
    float m_ForwardAmount;
    public float m_RunningAnimationMultiplier = 10;


    //  bool canChangeGravity = true;

    private Transform camera;

    //public GameObject cameraLookAt;
    //public Transform playerModel;

    void Awake()
    {
        GameManager.Instance.Player = this; // Assign itself to the GameManager
    }

    // Start is called before the first frame update
    void Start()
    {
        //Time.timeScale = 0.03f;
        m_Animator = GetComponent<Animator>();
        camera = Camera.main.transform;
        player = GetComponent<CharacterController>();
        canMove = true;
    }

    void Update()
    {
        if (!canMove)
            return;

        parentMovement = Vector3.zero;     

        Collider[] groundColliders = Physics.OverlapSphere(groundCheck.position, groundDistance, groundMask);
        isGrounded = groundColliders.Length != 0;

        if (isGrounded)
        {
            canDoubleJump = true;

            bool parentChanged = false;
            Transform currentGroundTransform = groundColliders[0].transform;

            if (parentTransform != currentGroundTransform)
            {
                parentTransform = currentGroundTransform;
                previousParentPosition = parentTransform.position;
                previousParentRotation = parentTransform.rotation.eulerAngles;
                parentChanged = true;
            } 

            if(!parentChanged && parentTransform != null)
            {
               CalculateParentOffset(); 
            }

            if (velocity.y < 0)
            {
               velocity.y = -2f;
            }
            
        } else //!isGrounded
        {
            velocity.y -= gravityIntensity * Time.deltaTime;
            parentTransform = null;

            if(velocity.y < maxFallSpeed)
            {
                velocity.y = maxFallSpeed;
            }
            
            //if it hits something when jumping, stop him from adding up force
            if(Physics.CheckSphere(ceilingCheck.position, groundDistance, groundMask)) {
                accumulatedJumpPower = accumulativeJumpLimit;
                velocity.y = -5f;
            }
        }

        if (Input.GetAxis("Jump") > 0)
        {
            if(isGrounded && jumpKeyReleased)
            {
                velocity.y = jumpForce;
                jumpKeyReleased = false;
                accumulatedJumpPower = 0;
            }
            else
            {
                if(!jumpKeyReleased && accumulatedJumpPower < accumulativeJumpLimit)
                {
                    float jumpStep = progressiveJumpPower * Time.deltaTime;
                    velocity.y += jumpStep;
                    accumulatedJumpPower += jumpStep;
                }
            }

            if (!isGrounded && jumpKeyReleased && doubleJumpUnlocked && canDoubleJump)
            {
                canDoubleJump = false;
                velocity.y = jumpForce;
            }
        }
        else if (Input.GetAxis("Jump") == 0)
        {
            jumpKeyReleased = true;
        }

        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        bool walking = !Input.GetKey(KeyCode.LeftShift);
        float targetSpeed = (walking ? runSpeed : walkSpeed) * input.normalized.magnitude;
        float targetspeedSmooth = (isGrounded ? speedSmoothTime : speedSmoothTimeAir);
        float targetTurnSpeedSmooth = (isGrounded ? turnSmoothTime : turnSmoothTimeAir);

        if (input != Vector2.zero)
        {
            float m_TurnAmount = Mathf.Atan2(input.x, input.y) * Mathf.Rad2Deg + camera.eulerAngles.y;
            transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, m_TurnAmount, ref turnSmoothVelocity, targetTurnSpeedSmooth);

            movementDir = DegreeToVector3(m_TurnAmount);
        } else
        {
            if(isGrounded)
            {
                targetspeedSmooth /= 3.5f;
            }
            
        }

        currentSpeed = Vector3.SmoothDamp(currentSpeed, movementDir * targetSpeed, ref speedSmoothVelocity, targetspeedSmooth);

        Vector3 movement = currentSpeed * 0.01f ;

        m_TurnAmount *= 200;
        m_ForwardAmount = movement.magnitude * m_RunningAnimationMultiplier;
        UpdateAnimator(movement);
        player.Move(movement + parentMovement + velocity / 100.0f);
    }

    

    private void OnTriggerStay(Collider other)
    {
        if (!other.name.Equals("Player"))
        {
            Vector3 objPos = other.transform.position;
            Vector3 currentPos = transform.position;

            player.Move((currentPos - objPos).normalized * Time.deltaTime * 0.01f);
        }
    }

    public static Vector3 RadianToVector3(float radian)
    {
        return new Vector3(Mathf.Sin(radian), 0,Mathf.Cos(radian));
    }

    public static Vector3 DegreeToVector3(float degree)
    {
        return RadianToVector3(degree * Mathf.Deg2Rad);
    }

    private void CalculateParentOffset()
    {
        Vector3 currentParentPosition = parentTransform.position;
        parentMovement = currentParentPosition - previousParentPosition;
        previousParentPosition = currentParentPosition;

        Vector3 currentParentRotation = parentTransform.rotation.eulerAngles;
        Vector3 parentRotation = currentParentRotation - previousParentRotation;
        previousParentRotation = currentParentRotation;

        transform.RotateAround(currentParentPosition, Vector3.up, parentRotation.y);
    }

    void UpdateAnimator(Vector3 move)
    {
        
        // update the animator parameters
        m_Animator.SetFloat("Forward", m_ForwardAmount, 0.1f, Time.deltaTime);
        m_Animator.SetFloat("Turn", m_TurnAmount, 0.1f, Time.deltaTime);
        m_Animator.SetBool("OnGround", isGrounded);
        if (!isGrounded)
        {
            m_Animator.SetFloat("Jump", velocity.y);
        }

        // calculate which leg is behind, so as to leave that leg trailing in the jump animation
        // (This code is reliant on the specific run cycle offset in our animations,
        // and assumes one leg passes the other at the normalized clip times of 0.0 and 0.5)
        float runCycle =
            Mathf.Repeat(
                m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime, 1);
        float jumpLeg = (runCycle < k_Half ? 1 : -1) * m_ForwardAmount;
        if (isGrounded)
        {
            m_Animator.SetFloat("JumpLeg", jumpLeg);
        }

        // the anim speed multiplier allows the overall speed of walking/running to be tweaked in the inspector,
        // which affects the movement speed because of the root motion.
        if (isGrounded && move.magnitude > 0)
        {
            m_Animator.speed = Mathf.Max(m_AnimSpeedMultiplier * m_ForwardAmount, m_AnimSpeedMultiplier);
        }
        else
        {
            // don't use that while airborne
            m_Animator.speed = 1;
        }
    }

    public void SetActive(bool isActive)
    {
        canMove = isActive;
    }

}
