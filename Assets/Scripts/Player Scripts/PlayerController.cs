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
    public bool normalG = true;

    private bool canMove = false;

    bool coroutineOver = true;

    Transform camera;

    public GameObject groundCheckObj;
    public GameObject cameraLookAt;


    // Start is called before the first frame update
    void Start()
    {
        camera = Camera.main.transform;
        player = GetComponent<CharacterController>();
        canMove = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G) && !isGrounded && coroutineOver)
        {
            StartCoroutine(YeahNigga());
        }
        
        if (normalG)
        {
            velocity.y -= gravityIntensity * Time.deltaTime;
            groundCheckObj.transform.position = new Vector3(0, transform.position.y - 0.6f, 0);
        }
        else if (!normalG)
        {
            velocity.y += gravityIntensity * Time.deltaTime;
            groundCheckObj.transform.position = new Vector3(0, transform.position.y + 0.6f, 0);
        }

        if (!canMove)
            return;

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

      // if (isGrounded && velocity.y < 0 && normalG)
      // {
      //     velocity.y = -2f;
      // }
      // else if(isGrounded && velocity.y < 0 && !normalG)
      // {
      //     velocity.y = 4f;
      // }

        if (Input.GetAxis("Jump") > 0 && isGrounded && jumpKeyReleased && normalG)
        {
            velocity.y = jumpForce;
            jumpKeyReleased = false;
        } else if(Input.GetAxis("Jump") == 0)
        {
            jumpKeyReleased = true;
        }

        if (Input.GetAxis("Jump") > 0 && isGrounded && jumpKeyReleased && !normalG)
        {
            velocity.y = -jumpForce;
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

        float targetRotation = Mathf.Atan2(input.x, input.y) * Mathf.Rad2Deg + camera.eulerAngles.y;

        if (input != Vector2.zero && normalG)
        {
                transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref turnSmoothVelocity, turnsmoothTime);
        } else if(input != Vector2.zero && !normalG)
        {
                transform.eulerAngles = - Vector3.down * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref turnSmoothVelocity, turnsmoothTime);
        }

        player.Move(transform.forward * currentSpeed * Time.deltaTime) ;

        player.Move(velocity * Time.deltaTime);

        //transform.Translate(transform.forward * currentSpeed * Time.deltaTime, Space.World); 
    }

    public void SetActive(bool isActive)
    {
        canMove = isActive;
    }

    IEnumerator YeahNigga()
    {
        coroutineOver = false;
        normalG = !normalG;
        yield return new WaitForSeconds(1);
        coroutineOver = true;
    }
}
