using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Timeline.TimelinePlaybackControls;

public class AnimationAndMovementController : MonoBehaviour
{

    PlayerStats player;



    //Declere reference variables
    CharacterController characterController;
    Animator animator;
    PlayerInput playerInput; //Note: PlayerInput class must be generated from New Input System in Inspector

    // variables to store optimized setter/getter parameter IDs
    int isWalkingHash;

    // variables to store player input value
    Vector2 currentMovementInput;
    public Vector3 currentMovement;

    [HideInInspector]
    public Vector3 lastMovement;

    Vector3 appliedMovement;
    bool isMovementPressed;

    // constants
    float rotationFactorPerFrame = 15.0f;
    int zero = 0;

    // gravity variables
    float gravity = -9.8f;
    float groundedGravity = -.05f;

    // jumping variables
    bool isJumpPressed = false;
    float initialJumpVelocity;
    float maxJumpHeight = 2.0f;
    float maxJumpTime = .75f;
    bool isJumping = false;
    int jumpCountHash;
    int isJumpingHash;
    bool isJumpAnimating = false;
    int jumpCount = 0;
    Dictionary<int, float> initialJumpVelocities = new Dictionary<int, float>();
    Dictionary<int, float> jumpGravities = new Dictionary<int, float>();
    Coroutine currentJumpResetRoutine = null;

    // Awake is called erlier than Start in Unity's event life cycle
    void Awake()
    {
   
        /*if (GameManager.instance.isGameOver)
        {   
            return;
        }*/

        player = GetComponent<PlayerStats>();

        // initially set reference variables
        playerInput = new PlayerInput();
        characterController = GetComponent<CharacterController>();  
        animator = GetComponent<Animator>();

        // set the parameter hash references
        isWalkingHash = Animator.StringToHash("isWalking");
        isJumpingHash = Animator.StringToHash("isJumping");
        jumpCountHash = Animator.StringToHash("jumpCount");

        // set the player input callback
        playerInput.CharacterControls.Move.started += OnMovementInput;
        playerInput.CharacterControls.Move.canceled += OnMovementInput;
        playerInput.CharacterControls.Move.performed += OnMovementInput;
        playerInput.CharacterControls.Jump.started += OnJump;
        playerInput.CharacterControls.Jump.canceled += OnJump;

        setupJumpVariables();

        lastMovement = new Vector3(1, 0, 0f);
    }

    // callback handler function for jump buttons
    void OnJump(InputAction.CallbackContext context)
    {
        isJumpPressed = context.ReadValueAsButton();
    }

    // handler function to set the player input values
    void OnMovementInput(InputAction.CallbackContext context)
    {
        /*if (GameManager.instance.isGameOver)
        {
            return;
        }*/

        currentMovementInput = context.ReadValue<Vector2>();

        if (currentMovementInput.x != 0)
        {
            lastMovement = new Vector3(currentMovementInput.x, 0, 0f);
        }
        if(currentMovementInput.y != 0)
        {
            lastMovement = new Vector3(0f, 0, currentMovementInput.y);
        }
        if(currentMovementInput.x != 0 && currentMovementInput.y != 0)
        {
            lastMovement = new Vector3(currentMovementInput.x, 0, currentMovementInput.y);
        }
        var matrix = Matrix4x4.Rotate(Quaternion.Euler(0, 45, 0));
        var skewedMovement = matrix.MultiplyPoint3x4(lastMovement);
        lastMovement = skewedMovement;

        currentMovement.x = currentMovementInput.x * player.CurrentMoveSpeed;
        currentMovement.z = currentMovementInput.y * player.CurrentMoveSpeed;
        isMovementPressed = currentMovementInput.x != zero || currentMovementInput.y != zero;
    }

    void handleAnimation()
    {
        // get parameter values from animator
        bool isWalking = animator.GetBool(isWalkingHash);

        // start walking if movement pressed is true and not already walking
        if (isMovementPressed && !isWalking)
        {
            animator.SetBool(isWalkingHash, true);
        }
        // stop walking if isMovementPressed is false and not already walking
        else if (!isMovementPressed && isWalking)
        {
            animator.SetBool(isWalkingHash, false);
        }
    }

    void handleRotation()
    {

        var matrix = Matrix4x4.Rotate(Quaternion.Euler(0, 45, 0));
        var skewedMovement = matrix.MultiplyPoint3x4(currentMovement);

        Vector3 positionToLookAt;
        // the change in position our character should point to
        positionToLookAt.x = skewedMovement.x;
        positionToLookAt.y = zero;
        positionToLookAt.z = skewedMovement.z;
        // the current rotation of our character
        Quaternion currentRotation = transform.rotation;

        if (isMovementPressed)
        {
            // crates a new rotation based on where the player is currently pressing
            Quaternion targetRotation = Quaternion.LookRotation(positionToLookAt);
            // rotate the character to face the positionToLookAt
            transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, rotationFactorPerFrame * Time.deltaTime);
        }
    }

    // Update is called once per frame
    void Update()
    {
        handleRotation();
        handleAnimation();
        var matrix = Matrix4x4.Rotate(Quaternion.Euler(0, 45, 0));
        var skewedMovement = matrix.MultiplyPoint3x4(currentMovement);
        appliedMovement.x = skewedMovement.x;
        appliedMovement.z = skewedMovement.z;
        characterController.Move(appliedMovement * Time.deltaTime);
        handleGravity();
        handleJump();
    }


    // set the initial velocity and gravity using jump heights and duration
    void setupJumpVariables()
    {
        float timeToApex = maxJumpTime / 2;
        gravity = (-2 * maxJumpHeight) / Mathf.Pow(timeToApex, 2);
        initialJumpVelocity = (2 * maxJumpHeight) / timeToApex;
        float secondJumpGravity = (-2 * (maxJumpHeight + 2)) / Mathf.Pow((timeToApex * 1.25f), 2);
        float secondJumpInitialVelocity = (2 * (maxJumpHeight + 2)) / (timeToApex * 1.25f);
        float thirdJumpGravity = (-2 * (maxJumpHeight + 4)) / Mathf.Pow((timeToApex * 1.5f), 2);
        float thirdJumpInitialVelocity = (2 * (maxJumpHeight + 4)) / (timeToApex * 1.5f);

        initialJumpVelocities.Add(1, initialJumpVelocity);
        initialJumpVelocities.Add(2, secondJumpInitialVelocity);
        initialJumpVelocities.Add(3, thirdJumpInitialVelocity);

        jumpGravities.Add(0, gravity);
        jumpGravities.Add(1, gravity);
        jumpGravities.Add(2, secondJumpGravity);
        jumpGravities.Add(3, thirdJumpGravity);
    }

    // launch character into the air with initial vertical velocity if conditions met
    void handleJump()
    {
        if (!isJumping && characterController.isGrounded && isJumpPressed)
        {
            if (jumpCount < 3 && currentJumpResetRoutine != null)
            {
                StopCoroutine(currentJumpResetRoutine);
            }
            animator.SetBool(isJumpingHash, true);
            isJumpAnimating = true;
            isJumping = true;
            jumpCount = 1;
            animator.SetInteger(jumpCountHash, jumpCount);
            currentMovement.y = initialJumpVelocities[jumpCount];
            appliedMovement.y = initialJumpVelocities[jumpCount];
        }
        else if(!isJumpPressed && isJumping && characterController.isGrounded) 
        {
            isJumping = false;
        }
    }


    // apply proper gravity if the player is grounded or not
    void handleGravity()
    {
        bool isFalling = currentMovement.y <= 0.0f || !isJumpPressed;
        float fallMultiplier = 2.0f;
        if (characterController.isGrounded)
        {
            if (isJumpAnimating)
            {
                animator.SetBool(isJumpingHash, false);
                isJumpAnimating = false;
                currentJumpResetRoutine = StartCoroutine(jumpResetRoutine());
                if (jumpCount == 3)
                {
                    jumpCount = 0;
                    animator.SetInteger(jumpCountHash, jumpCount);
                }
            }
            currentMovement.y = groundedGravity;
            appliedMovement.y = groundedGravity;
        }
        else if (isFalling)
        {
            float previousYVelocity = currentMovement.y;
            currentMovement.y = currentMovement.y + (jumpGravities[jumpCount] * fallMultiplier * Time.deltaTime);
            appliedMovement.y = Mathf.Max((previousYVelocity + currentMovement.y) * .5f, -20.0f);
        }
        else
        {
            float previousYVelocity = currentMovement.y;
            currentMovement.y = currentMovement.y + (jumpGravities[jumpCount] * Time.deltaTime);
            appliedMovement.y = (previousYVelocity + currentMovement.y) * .5f;
        }
    }

    IEnumerator jumpResetRoutine()
    {
        yield return new WaitForSeconds(.5f);
        jumpCount = 0;
    }


    // enable the character controls action map
    void OnEnable()
    {
        playerInput.CharacterControls.Enable();
    }
    // disable the character controls action map
    void OnDisable()
    {
        playerInput.CharacterControls.Disable();
    }

}
