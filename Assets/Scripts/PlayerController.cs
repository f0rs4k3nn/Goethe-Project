using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;

public class PlayerController : MonoBehaviour
{
    public float walkSpeed = 12;
    public float runSpeed = 45;
   // public int maxSpeed = 1;

    public float turnSmoothTime = 0.18f;
    public float turnSmoothTimeAir = 0.5f;

    float turnSmoothVelocity;
    CharacterController player;

    public float speedSmoothTime = 0.25f;
    public float speedSmoothTimeAir = 0.5f;
    Vector3 speedSmoothVelocity;

    Vector3 currentSpeed; //used for movement given by input
    Vector3 velocity; //only used for calculating vertical velocity
    Vector3 movementDir = Vector3.zero;

    //Used to set the speed of the animation depanding on how fast the player 
    //*actually* is. E.g. We don't want the player to have a running animation
    //when running against a wall.
    Vector3 movementSinceLastFrame;
    Vector3 lastFramePositon;


   // private GameObject temporaryParent;
    public Transform parentTransform;
    private Vector3 previousParentPosition;
    private Vector3 previousParentRotation;
    private Vector3 parentMovement;

    public Transform groundCheck;
    public Transform ceilingCheck;
    public float groundDistance = 0.28f;
    public LayerMask groundMask;
    private bool isGrounded;
    public float jumpForce = 45;
    public float gravityIntensity;
    private bool m_jumpKeyReleased = true;
   // public bool gravityIsReversed = false;

    //variables used for the progressive jump when the jump button is held
    private float accumulatedJumpPower; //force added since last jump
    public float accumulativeJumpLimit; //how much force can be added
    public float progressiveJumpPower; //how much force to add while jump is still pressed
    public bool doubleJumpUnlocked; // boolean to see if the player can double jump yet
    private bool canDoubleJump;
    private int m_staticFallVelocity = -100; //the speed at which the player is pushed to the ground when grounded
    private bool m_beganFalling = false;


    public int maxFallSpeed = -250;
    private bool canMove = false;


    /**
     *Animation variables 
     **/
    public Animator m_Animator;
    [SerializeField] float m_RunCycleLegOffset = 0.2f; //specific to the character in sample assets, will need to be modified to work with others
    [SerializeField] float m_MoveSpeedMultiplier = 1f;
    [SerializeField] float m_AnimSpeedMultiplier = 1f;
    const float k_Half = 0.5f;
    float m_TurnAmount;
    float m_ForwardAmount;
    public float m_RunningAnimationMultiplier = 10;

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
       // m_Animator = GetComponent<Animator>();
        camera = Camera.main.transform;
        player = GetComponent<CharacterController>();
        canMove = true;
        movementSinceLastFrame = Vector3.zero;
    }

    void FixedUpdate()
    {
        
        if (!canMove)
            return;

        parentMovement = Vector3.zero;

        Collider[] groundColliders = Physics.OverlapSphere(groundCheck.position, groundDistance, groundMask);

       // Debug.Log(velocity.y + " " + groundColliders.Length);

        isGrounded = groundColliders.Length != 0; //if there is nothing beneath us, there is no parent

        if (isGrounded)
        {
            m_beganFalling = false;
            canDoubleJump = true;

            Transform currentGroundTransform = groundColliders[0].transform;

            if (parentTransform != currentGroundTransform)
            {
                parentTransform = currentGroundTransform;
                previousParentPosition = parentTransform.position;
                previousParentRotation = parentTransform.rotation.eulerAngles;

            } else if(parentTransform != null)
            {
                CalculateParentOffset();
            }

            if (velocity.y < 0)
            {
               velocity.y = m_staticFallVelocity;
            }
            
        } else //!isGrounded
        {
            /*
             * Because we have a static falling velocity of -45, we don't want our character to have a high velocity when they just started falling
             * this is why we first check so we can lower their starting falling velocity
             * */
            if(!m_beganFalling)
            {
                if(velocity.y < 0)
                {
                    m_beganFalling = true;
                    velocity.y = -5;
                }
            }
            
            
            velocity.y -= gravityIntensity * Time.deltaTime; //add gravity
            parentTransform = null; //because we have nothing beneath us, there is no parent

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
            if(isGrounded && m_jumpKeyReleased) //default jump
            {
                velocity.y = jumpForce;
                m_jumpKeyReleased = false;
                accumulatedJumpPower = 0;
            }
            else if(!m_jumpKeyReleased && accumulatedJumpPower < accumulativeJumpLimit && canDoubleJump) //accumulative jump, also makes sure it isn't a double jump
            {
                float jumpStep = progressiveJumpPower * Time.deltaTime;
                velocity.y += jumpStep;
                accumulatedJumpPower += jumpStep;
            } else if (!isGrounded && m_jumpKeyReleased && doubleJumpUnlocked && canDoubleJump) //doublejump
            {
                m_jumpKeyReleased = false;
                canDoubleJump = false;
                velocity.y = jumpForce;
            }
        } else if (Input.GetAxis("Jump") == 0)
        {
            
            m_jumpKeyReleased = true;
        }

        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        bool walking = !Input.GetKey(KeyCode.LeftShift);
        float targetSpeed = (walking ? runSpeed : walkSpeed) * input.normalized.magnitude;
        float targetspeedSmooth = (isGrounded ? speedSmoothTime : speedSmoothTimeAir);
        float targetTurnSpeedSmooth = (isGrounded ? turnSmoothTime : turnSmoothTimeAir);

        if (input != Vector2.zero)
        {
            float m_TurnAmount = Mathf.Atan2(input.x, input.y) * Mathf.Rad2Deg + camera.eulerAngles.y; //desired direction of movement
            transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, m_TurnAmount, ref turnSmoothVelocity, targetTurnSpeedSmooth);

            movementDir = DegreeToVector3(m_TurnAmount); //converts the degrees into a vector to use for the movement
        } else
        {
            if(isGrounded)
            {
                targetspeedSmooth /= 3.5f; //if we stop moving and we are grounded, stop faster
            }           
        }

        currentSpeed = Vector3.SmoothDamp(currentSpeed, movementDir * targetSpeed, ref speedSmoothVelocity, targetspeedSmooth);

        Vector3 movement = currentSpeed * 0.01f;

        Vector3 lastFramePositon = transform.position;
        player.Move(movement + velocity / 100.0f);
        currentSpeed = (transform.position - lastFramePositon) * 100f;
        currentSpeed.y = 0;

        m_TurnAmount *= 200;
        m_ForwardAmount = currentSpeed.magnitude;

        UpdateAnimator();

        transform.position += parentMovement;
    }

    
    /*
     * We use this because we want to make sure other objects push the player when they interact with them
     */
    private void OnTriggerStay(Collider other)
    {
        //Debug.Log("I'VE BEEN HIT");
      
        Vector3 objPos = other.transform.position;
        Vector3 currentPos = transform.position;


        Vector3 lastPosition = currentPos;
        player.Move((currentPos - objPos).normalized * Time.deltaTime * 0.7f);
        Vector3 newPos = transform.position;

        if(lastPosition == newPos)
        {
            Debug.LogError("I AM DEAD LMAP");
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

    void UpdateAnimator()
    {

        // update the animator parameters
        m_Animator.SetFloat("GroundSpeed", m_ForwardAmount, 0.1f, Time.deltaTime);
       // Debug.Log(m_ForwardAmount);
       // m_Animator.SetFloat("Turn", m_TurnAmount, 0.1f, Time.deltaTime);
        m_Animator.SetBool("IsGrounded", isGrounded);

        if (!isGrounded)
        {
            m_Animator.SetFloat("VerticalSpeed", velocity.y);
        }

        // calculate which leg is behind, so as to leave that leg trailing in the jump animation
        // (This code is reliant on the specific run cycle offset in our animations,
        // and assumes one leg passes the other at the normalized clip times of 0.0 and 0.5)
        float runCycle =
            Mathf.Repeat(
                m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime, 1);
        float jumpLeg = (runCycle < k_Half ? 1 : -1) * m_ForwardAmount;

      /*  if (isGrounded)
        {
            m_Animator.SetFloat("JumpLeg", jumpLeg);
        }*/

        // the anim speed multiplier allows the overall speed of walking/running to be tweaked in the inspector,
        // which affects the movement speed because of the root motion.
        if (isGrounded && m_ForwardAmount > 7)
        {
            m_Animator.speed = m_AnimSpeedMultiplier * m_ForwardAmount / 14.0f;
        }
        else
        {
            // don't use that while airborne
            m_Animator.speed = m_AnimSpeedMultiplier;
        }
    }

    void UpdateAnimatorOLD(Vector3 move)
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
            m_Animator.speed = m_AnimSpeedMultiplier * m_RunningAnimationMultiplier;
        }
        else
        {
            // don't use that while airborne
            m_Animator.speed = m_AnimSpeedMultiplier;
        }
    }

    public void SetActive(bool isActive)
    {
        canMove = isActive;
    }

}
