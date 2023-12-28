using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonMovement : PlayerBase
{
    public CharacterController controller;
    public Transform cam;

    public float speed = 6f;

    public float turnSmoothTime = 0.1f;
    public bool canMove = true;
    [HideInInspector]
    public float horizontal = 0;

    [HideInInspector]
    public float vertical = 0;
    float turnSmoothVelocity;

    [Header("Gravity")]
    public float gravity;
    public float currentGravity;
    public float constantGravity;
    public float maxGravity;

    private Vector3 direction;
    private Vector3 gravityDirection;
    private Vector3 gravityMovement;

    private PlayerAnimation playerAnimation;
    private bool isRunning = false;
    

    private void Start()
    {
        playerAnimation = GetComponent<PlayerAnimation>();
    }

    private bool IsGrounded()
    {
        return controller.isGrounded;
    }

    private void CalculateGravity()
    {
        if(IsGrounded())
        {
            currentGravity = constantGravity;
        }
        else
        {
            if(currentGravity > maxGravity)
            {
                currentGravity -= gravity * Time.deltaTime;
            }
        }
        gravityMovement = gravityDirection * -currentGravity * Time.deltaTime;
    }

    void Awake()
    {
        gravityDirection = Vector3.down;
    }

    void Update()
    {
        CalculateGravity();
        Movement();
        Dash();
        
    }

    private void Dash()
    {
        if ((Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) && direction.magnitude > 0.1f)
        {
            isRunning = true;
            speed = 12f;
            playerAnimation.SetWalking(false);
            playerAnimation.SetRunning(true);
        } else
        {
            isRunning = false;
            speed = 6f;
            playerAnimation.SetRunning(false);
        }
    }

    private void Movement()
    {
        if (canMove)
        {
            float _h = Input.GetAxisRaw("Horizontal");
            float _v = Input.GetAxisRaw("Vertical");
            horizontal = _h;
            vertical = _v;
        }

        direction = new Vector3(horizontal, 0f, vertical).normalized;
        if (!IsGrounded())
            controller.Move(gravityMovement.normalized);

        if (direction.magnitude >= 0.1f && !onBattleMode)
        {
            if (!isRunning)
                playerAnimation.SetWalking(true);
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, angle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * speed * Time.deltaTime);
        } else
        {
            playerAnimation.SetWalking(false);
            playerAnimation.SetIdle(true);
        }
    }
}
