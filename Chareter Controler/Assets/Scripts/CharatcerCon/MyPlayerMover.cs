using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyPlayerMover : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 0;
    [SerializeField] private float jumpForece = 1f;
    [SerializeField] private float doubleJump = 1.5f;
    [SerializeField] private int maxDoubleJump = 2;
    [SerializeField] private float sprintSpeed = 6f;
    [SerializeField] private float walkSpeed = 3.5f;
    [SerializeField] private float turnSmoothtime = .1f;
    [SerializeField] private string wallTagString = "Runnablewall";
    [SerializeField] private float wallJumpHight = 2f;


    private MyInputs inputs;
    private CharacterController controller;
    private Vector3 YPos;
    private float griavity = -9.81f;
    private bool isGrounder = false;
    private int isDoubleJump;    
    private float turnSmoothVel;
    private Transform cam;
    private bool isCanWallJump;
    private Vector3 wallJumpNormal;
    private Vector3 Velocity;


    private void Awake()
    {
        cam = Camera.main.transform;
        controller = GetComponent<CharacterController>();
        inputs = GetComponent<MyInputs>();

        Cursor.lockState = CursorLockMode.Locked;
    }


    private void Update()
    {
        isGrounder = controller.isGrounded;
        Movement();
        Gravitiy();
        Jump();
    }

    private void Movement()
    {
        Vector3 move = inputs.Move();

        Debug.Log(move);

        Velocity = move;

        
        if (inputs.Sprint())
        {
            moveSpeed = sprintSpeed;
        }
        else
        {
            moveSpeed = walkSpeed;
        }

        if (move.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(move.x, move.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVel, turnSmoothtime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;


            controller.Move(moveDir.normalized * moveSpeed * Time.deltaTime);
        }
        
    }
    
    private void Gravitiy()
    {
        

        if (isGrounder && YPos.y < 0)
        {
            YPos.y = 0f;
            
        }

        Debug.Log(isGrounder + " is grounded");

        YPos.y += griavity * Time.deltaTime;
        controller.Move(YPos * Time.deltaTime);
    }

    private void Jump()
    {
        if (isGrounder)
        {
            isCanWallJump = false;
            isDoubleJump = 0;
        }

        if (inputs.Jump())
        {
            if (isGrounder)
            {
                YPos.y += Mathf.Sqrt(jumpForece * -3f * griavity);                
            }
            else 
            {
                if (isDoubleJump < maxDoubleJump)
                {
                    YPos.y += Mathf.Sqrt(doubleJump * -3f * griavity);
                    isDoubleJump++;
                    Debug.Log("doubleJump " + isDoubleJump);
                }
                
            }
            if (isCanWallJump)
            {
                YPos.y += Mathf.Sqrt(wallJumpHight * -3f * griavity);
                Velocity = wallJumpNormal * moveSpeed;
            }

        }
        
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (!isGrounder && !hit.transform.CompareTag(wallTagString))
        {
            return;
        }

        Debug.DrawRay(hit.point, hit.normal, Color.cyan);
        if (hit.transform.CompareTag(wallTagString))
        {
            isCanWallJump = true;
            wallJumpNormal = hit.normal;
            Debug.Log(isCanWallJump + " Wall");
        }

        
    }
}
