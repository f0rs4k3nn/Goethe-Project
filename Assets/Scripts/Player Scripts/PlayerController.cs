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


    private GameObject temporaryParent;
    public Transform groundCheck;
    public float groundDistance;
    public LayerMask groundMask;
    public bool isGrounded;
    public float jumpForce;
    public float gravityIntensity;
    private bool jumpKeyReleased = true;
    public bool gravityIsReversed = false;

    //variables used for the progressive jump when the jump button is held
    private float accumulatedJumpPower; //force added since last jump
    public float accumulativeJumpLimit; //how much force can be added
    public float progressiveJumpPower;

    public int maxVerticalVelocity;
    private bool canMove = false;

    bool canChangeGravity = true;

    Transform camera;

    public GameObject groundCheckObj;
    //public GameObject cameraLookAt;
    public Transform playerModel;


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

        if (Input.GetKeyDown(KeyCode.G) && !isGrounded && canChangeGravity)
        {
            playerModel.rotation = Quaternion.Euler(0, 0, 180f + playerModel.rotation.eulerAngles.z);
            StartCoroutine(GChangeCooldown());
        }

        if (isGrounded)
        {
            if(transform.parent == null)
            {
                if(temporaryParent != null)
                {
                    Destroy(temporaryParent);
                }

                RaycastHit hit;
                if (Physics.Raycast(transform.position, Vector3.down, out hit, groundMask))
                {
                    Transform newParent = hit.collider.gameObject.transform;
                    Transform auxChild = (new GameObject()).transform;
                    auxChild.SetParent(newParent);
                    temporaryParent = auxChild.gameObject;
                    auxChild.localRotation = Quaternion.Euler(Vector3.zero);

                    Vector3 parentScale = newParent.localScale;
                    auxChild.localScale = new Vector3(1.0f / parentScale.x , 1.0f / parentScale.y, 1.0f / parentScale.z);

                    transform.parent = auxChild;
                }
            }
            

            if (gravityIsReversed)
            {
                if (velocity.y < 0)
                {
                    velocity.y = -2f;
                }
            }
            else if (velocity.y > 0)
            {
                velocity.y = 2f;
            }
        } else
        {
            transform.parent = null;
            Destroy(temporaryParent);
            temporaryParent = null;
        }

        if (Input.GetAxis("Jump") > 0)
        {
            if(isGrounded && jumpKeyReleased)
            {
                velocity.y = jumpForce * (gravityIsReversed ? -1 : 1);
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

       // currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedSmoothVelocity, targetspeedSmooth);

        if (input != Vector2.zero)
        {
            float targetRotation = Mathf.Atan2(input.x, input.y) * Mathf.Rad2Deg + camera.eulerAngles.y;
            transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref turnSmoothVelocity, targetTurnSpeedSmooth);

            movementDir = DegreeToVector3(targetRotation);
        } else
        {
            if(isGrounded)
            {
                targetspeedSmooth /= 3.0f;
            }
            
        }

        currentSpeed = Vector3.SmoothDamp(currentSpeed, movementDir * targetSpeed, ref speedSmoothVelocity, targetspeedSmooth);

        player.Move(currentSpeed * 0.01f);

        velocity.y -= gravityIntensity * Time.deltaTime * (gravityIsReversed ? -1 : 1) * (isGrounded ? 0 : 1);
        player.Move(velocity * Time.deltaTime);
        
        Debug.Log(velocity.y);
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

    IEnumerator GChangeCooldown()
    {
        canChangeGravity = false;
        gravityIsReversed = !gravityIsReversed;
        yield return new WaitForSeconds(1);
        canChangeGravity = true;
    }
}
