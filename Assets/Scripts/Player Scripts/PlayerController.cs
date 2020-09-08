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
    public Transform parentTransform;
    private Vector3 parentOffset;

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
    public float progressiveJumpPower;
    public bool doubleJumpUnlocked; // boolean to see if the player can double jump yet
    public bool canDoubleJump;

    public int maxVerticalVelocity;
    private bool canMove = false;

  //  bool canChangeGravity = true;

    private Transform camera;

    //public GameObject cameraLookAt;
    //public Transform playerModel;


    // Start is called before the first frame update
    void Start()
    {
        camera = Camera.main.transform;
        player = GetComponent<CharacterController>();
        canMove = true;
    }

    void Update()
    {
        if (!canMove)
            return;

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

       /* if (Input.GetKeyDown(KeyCode.G) && !isGrounded && canChangeGravity)
        {
            playerModel.rotation = Quaternion.Euler(0, 0, 180f + playerModel.rotation.eulerAngles.z);
            StartCoroutine(GChangeCooldown());
        }*/

        if (isGrounded)
        {
            if(parentTransform == null)
            {
                canDoubleJump = true;

                RaycastHit hit;
                if (Physics.Raycast(transform.position, Vector3.down, out hit, groundMask))
                {
                    parentTransform = hit.collider.gameObject.transform;
                    parentOffset = transform.position - parentTransform.position;
                }
            }
                   
            if (velocity.y < 0)
            {
               velocity.y = -2f;
            }
            
        } else //!isGrounded
        {
            velocity.y -= gravityIntensity * Time.deltaTime;
            parentTransform = null;
            
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
                velocity.y = 2*jumpForce;
            }


        }
        else if (Input.GetAxis("Jump") == 0)
        {
            jumpKeyReleased = true;
        }

        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        bool running = Input.GetKey(KeyCode.LeftShift);
        float targetSpeed = (running ? runSpeed : walkSpeed) * input.normalized.magnitude;
        float targetspeedSmooth = (isGrounded ? speedSmoothTime : speedSmoothTimeAir);
        float targetTurnSpeedSmooth = (isGrounded ? turnSmoothTime : turnSmoothTimeAir);

        if (input != Vector2.zero)
        {
            float targetRotation = Mathf.Atan2(input.x, input.y) * Mathf.Rad2Deg + camera.eulerAngles.y;
            transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref turnSmoothVelocity, targetTurnSpeedSmooth);

            movementDir = DegreeToVector3(targetRotation);
        } else
        {
            if(isGrounded)
            {
                targetspeedSmooth /= 3.5f;
            }
            
        }

        Vector3 initialPosition = transform.position;
        currentSpeed = Vector3.SmoothDamp(currentSpeed, movementDir * targetSpeed, ref speedSmoothVelocity, targetspeedSmooth);
        player.Move((currentSpeed * 0.01f) + velocity * Time.deltaTime);

        parentOffset += transform.position - initialPosition;
    }

    public void LateUpdate()
    {
       if(parentTransform == null)
        {
            return;
        }

        transform.position = parentOffset + parentTransform.position;
    }

    public static Vector3 RadianToVector3(float radian)
    {
        return new Vector3(Mathf.Sin(radian), 0,Mathf.Cos(radian));
    }

    public static Vector3 DegreeToVector3(float degree)
    {
        return RadianToVector3(degree * Mathf.Deg2Rad);
    }


    public void SetActive(bool isActive)
    {
        canMove = isActive;
    }

}
