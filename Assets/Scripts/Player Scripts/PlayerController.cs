using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;

public class PlayerController : MonoBehaviour
{
    public float walkSpeed = 2;
    public float runSpeed = 6;
    public int maxSpeed = 1;

    public float turnsmoothTime = 0.2f;
    float turnSmoothVelocity;
    CharacterController player;

    public float speedSmoothTime = 0.1f;
    float speedSmoothVelocity;
    float currentSpeed;
    Vector3 velocity;

    public Transform groundCheck;
    public float groundDistance;
    public LayerMask groundMask;
    public bool isGrounded;
    public float jumpForce;
    public float gravityIntensity;
    private bool jumpKeyReleased = true;
    public float airControl = 0.5f;
    public bool gravityIsReversed = false;

    public int maxVerticalVelocity;
    private bool canMove = false;

    bool canChangeGravity = true;

    Transform camera;

    public GameObject groundCheckObj;
    public GameObject cameraLookAt;
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
            if (gravityIsReversed)
            {
                if (velocity.y > 0)
                {
                    velocity.y = 2f;
                }
            }
            else if (velocity.y < 0)
            {
                velocity.y = -2f;
            }
        }

        if (Input.GetAxis("Jump") > 0 && isGrounded && jumpKeyReleased)
        {
            velocity.y = jumpForce * (gravityIsReversed ? -1 : 1);
            jumpKeyReleased = false;
        }
        else if (Input.GetAxis("Jump") == 0)
        {
            jumpKeyReleased = true;
        }

        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        bool running = Input.GetKey(KeyCode.LeftShift);
        float targetSpeed = (running ? runSpeed : walkSpeed) * input.magnitude;
        currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedSmoothVelocity, speedSmoothTime);

        if (input != Vector2.zero)
        {
            float targetRotation = Mathf.Atan2(input.x, input.y) * Mathf.Rad2Deg + camera.eulerAngles.y;
            transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref turnSmoothVelocity, turnsmoothTime);
        }

        player.Move(transform.forward * currentSpeed * Time.deltaTime);

        velocity.y -= gravityIntensity * Time.deltaTime * (gravityIsReversed ? -1 : 1);


       /* if (Mathf.Abs(velocity.y) > maxVerticalVelocity)
        {
            velocity.y = maxVerticalVelocity * (gravityIsReversed ? -1 : 1);
        }*/

        player.Move(velocity * Time.deltaTime);

        //transform.Translate(transform.forward * currentSpeed * Time.deltaTime, Space.World); 
    }

    /**
    void Update()
    {
        if (!canMove)
            return;

       if (Input.GetKeyDown(KeyCode.G) && !isGrounded && canChangeGravity)
        {
            playerModel.rotation = Quaternion.Euler(0, 0, 180f + playerModel.rotation.eulerAngles.z);
            StartCoroutine(GChangeCooldown());
        }

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded)
        {
            if(gravityIsReversed)
            {
                if(velocity.y > 0)
                {
                    velocity.y = 2f;
                } 
            } else if (velocity.y < 0)
            {
                velocity.y = -2f;
            }
        }

        if (Input.GetAxis("Jump") > 0 && isGrounded && jumpKeyReleased)
        {
            velocity.y = jumpForce * (gravityIsReversed ? -1 : 1);
            jumpKeyReleased = false;

        } else if (Input.GetAxis("Jump") == 0)
        {
            jumpKeyReleased = true;
        }

        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        bool running = Input.GetKey(KeyCode.LeftShift);
        float targetSpeed = (running ? runSpeed : walkSpeed) * input.magnitude;
        currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedSmoothVelocity, speedSmoothTime);

        if (input != Vector2.zero)
        {
            float targetRotation = Mathf.Atan2(input.x, input.y) * Mathf.Rad2Deg + camera.eulerAngles.y;
            transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref turnSmoothVelocity, turnsmoothTime);
        }

        player.Move(transform.forward * currentSpeed * Time.deltaTime);

        velocity.y -= gravityIntensity * Time.deltaTime;

        player.Move(velocity * Time.deltaTime);

        //transform.Translate(transform.forward * currentSpeed * Time.deltaTime, Space.World); 
    }
    **/
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
