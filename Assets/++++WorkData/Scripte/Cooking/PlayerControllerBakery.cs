using UnityEngine;
using UnityEngine.InputSystem;


[RequireComponent(typeof(CharacterController))]
public class PlayerControllerBakery : MonoBehaviour
{
    private static readonly int Hash_MovementSpeed = Animator.StringToHash("MovementSpeed");
    private static readonly int Hash_Grounded = Animator.StringToHash("Grounded");
    private static readonly int Hash_Crouched = Animator.StringToHash("Crouched");
    //private static readonly int Hash_Jumped = Animator.StringToHash("Jumped");
    private static readonly int Hash_ActionTrigger = Animator.StringToHash("ActionTrigger");
    private static readonly int Hash_ActionId = Animator.StringToHash("ActionId");
    private static readonly int Hash_WeaponEquipTrigger = Animator.StringToHash("WeaponEquipTrigger");
    private static readonly int Hash_WeaponId = Animator.StringToHash("WeaponId");
    private static readonly int Hash_WeaponUnEquipTrigger = Animator.StringToHash("WeaponUnEquipTrigger");
    
    #region Inspector
    
   
    [Header("Movement")]
    
    [Min(0)]
    [Tooltip("The speed values of the player in uu/s")]
    [SerializeField] private float walkSpeed = 3f;
    
    [Min(0)]
    [Tooltip("How fast the movement speed is in-/decreasing.")]
    [SerializeField] private float speedChangeRate = 10f;
    [SerializeField] private float rotationSpeed = 10f;

    [Header("Slope Movement")] 
    
   // [SerializeField] private float pullDownForce = 5f;

    //[SerializeField] private LayerMask raycastMask = 1;

    //[SerializeField] private float raycastLength = 0.5f;
    
    [Header("Camera")]
    [SerializeField] private Transform cameraTarget;
   
    [SerializeField] private float verticalCameraRotationMin = -30f;
    [SerializeField] private float verticalCameraRotationMax = 70f;
    [SerializeField] private float cameraHorizontalSpeed = 200f;
    [SerializeField] private float cameraVerticalSpeed = 130f;
    
    [Header("Animator")]
     public Animator animator;

    [SerializeField] private float coyoteTime = .2f;
    
    [Header("Mouse Settings")]
    [Range(0f,2f)]
    [SerializeField] private float mouseCameraSensitivity = 1f;

    [Header("Controller Settings")]
    [Range(0f,2f)]
    [SerializeField] private float controllerCameraSensitivity = 1f;
    [SerializeField] private bool invertY = true;
    #endregion
    
    #region Private Variables
    private CharacterController characterController;
    
    public GameInput inputActions;
    private InputAction moveAction;
    private InputAction lookAction;
    private InputAction jumpAction;
    private Interactable selectedInteractable;
    private InputAction interactAction;

    private InputAction openMenu;
   // private InputAction attackAction;
    
    private Vector2 moveInput;
    private Vector2 lookInput;
    
  private Quaternion characterTargetRotation = Quaternion.identity;

    private Vector3 lastMovement;
    private Vector2 cameraRotation;

    private bool isGrounded = true;
    
    private float airTime;
    private bool isRunning;
    private bool isCrouched;
    private float currentSpeed;
    private int upperBody_AnimLayer;

    private GameController gameController;
    
    public GameObject exitMenu;

    
    [SerializeField] private float gravity = -19.62f;
    private Vector3 velocity;
    
    #endregion
    
    #region Unity Event Functios

    void CreateInputActionsIfNoneExist()
    {
        if (inputActions != null)
        {
            return;
        }
        inputActions = new GameInput();
    }
    
    private void Awake()
    {
        CreateInputActionsIfNoneExist();
        characterController = GetComponent<CharacterController>();
        //animator = GetComponentInChildren<>()
        gameController = FindObjectOfType<GameController>();
        upperBody_AnimLayer = animator.GetLayerIndex("UpperBody");
        

        moveAction = inputActions.Player.Move;
        lookAction = inputActions.Player.Look;
        
        interactAction = inputActions.Player.interact;
        
        characterTargetRotation = transform.rotation;
        cameraRotation = cameraTarget.rotation.eulerAngles;

        openMenu = inputActions.Player.Menu;

        currentSpeed = walkSpeed;
        exitMenu = GameObject.FindGameObjectWithTag("Menu");
        exitMenu.SetActive(false);

    }

    public void OnEnable()
    {
        CreateInputActionsIfNoneExist();
            
        EnableInput();
        
        interactAction.performed += Interact;

        openMenu.performed += OpenMenu;
        
    }

    private void Update()
    {
        ReadInput();
        
        Rotate(moveInput);
        Move(moveInput);

        CheckGround();
        
        UpdateAnimator();
    }

    private void LateUpdate()
    {
        RotateCamera(lookInput);
    }

    public void OnDisable()
    {
      DisableInput();

        openMenu.performed -= OpenMenu;

        interactAction.performed -= Interact;

        // attackAction.performed -= AttackInput;
    }

    private void OnDestroy()
    {
        
    }
    #endregion
    
    #region Input

    void ReadInput()
    {
        moveInput = moveAction.ReadValue<Vector2>();
        lookInput = lookAction.ReadValue<Vector2>();
    }
    
    public void UpperBody_Layer(float weight)
    {
        animator.SetLayerWeight(upperBody_AnimLayer,weight);
    }
    
    public void AnimationAction(int id)
    {
        animator.SetTrigger(Hash_ActionTrigger);
        animator.SetInteger(Hash_ActionId,id);
        
    }
    public void EndAction()
    {
        UpperBody_Layer(0);
        animator.SetInteger(Hash_ActionId,0);
    }
    
    public void EnableInput()
    {
       inputActions.Enable();
    }

    public void DisableInput()
    {
       inputActions.Disable();
    }
    
    
    #endregion
    
    #region Movement
    
    // rotates the player in the right direction
    private void Rotate(Vector2 moveInput)
    {
        if (moveInput != Vector2.zero)
        {
            Vector3 inputDirection = new Vector3(moveInput.x, 0, moveInput.y).normalized;

            Vector3 worldInputDirection = cameraTarget.TransformDirection(inputDirection);
            worldInputDirection.y = 0;
            
            characterTargetRotation = Quaternion.LookRotation(worldInputDirection);
        }

        if (Quaternion.Angle(transform.rotation, characterTargetRotation) > 0.1f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, characterTargetRotation, rotationSpeed * Time.deltaTime);
        }
        else
        {
            transform.rotation = characterTargetRotation;
        }
    }
    
    // moves the Player on button press
    
    private void Move(Vector2 moveInput)
    {
        
        float targetSpeed = moveInput == Vector2.zero ? 0 : this.currentSpeed * moveInput.magnitude;

        Vector3 currentVelocity = lastMovement;
        currentVelocity.y = 0;

        float currentSpeed = currentVelocity.magnitude;

        if (Mathf.Abs(currentSpeed - targetSpeed) > 0.01f)
        {
            currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, speedChangeRate * Time.deltaTime);
            animator.SetInteger(Hash_ActionId,1);
            animator.SetFloat(Hash_MovementSpeed,1);

        }
        else
        {
            currentSpeed = targetSpeed;
            animator.SetInteger(Hash_ActionId,0);
            animator.SetFloat(Hash_MovementSpeed,0);


        }

        Vector3 targetDirection = characterTargetRotation * Vector3.forward;

        Vector3 movement = targetDirection * currentSpeed;
        if (!isGrounded)
        {
            velocity.y += gravity * Time.deltaTime;
        }

        movement.y = velocity.y;

        characterController.Move(movement * Time.deltaTime);
        
        lastMovement = movement;
    
    }
    
    #endregion
    
    #region Physics

    private void OnTriggerEnter(Collider other)
    {
        TrySelectInteractable(other);
    }

    private void OnTriggerExit(Collider other)
    {
        TryDeselectInteractable(other);
    }

    #endregion
    
    #region Interaction

    // interact with things while pressing [E]
    private void Interact(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            print("interact");
            if (selectedInteractable != null)
            {
                selectedInteractable.Interact();
            }
        }
            
       
    }
    
    // if you are in a collider with the script Interactable it checks for any interactions
    private void TrySelectInteractable(Collider other)
    {
        Interactable interactable = other.GetComponent<Interactable>();

        if (interactable == null){ return; }

        if (selectedInteractable != null)
        {
            selectedInteractable.Deselect();
        }
        
        selectedInteractable = interactable;
        selectedInteractable.Select();
    }
    
    // deselects said interactable 
    private void TryDeselectInteractable(Collider other)
    {
        Interactable interactable = other.GetComponent<Interactable>();

        if (interactable == null)
        {
            return;
        }

        if (interactable == selectedInteractable)
        {
            selectedInteractable.Deselect();
            selectedInteractable = null;
        }
    }
    
    
    #endregion
    
    // opens the mainmenu

    private void OpenMenu(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            
            exitMenu.SetActive(!exitMenu.activeInHierarchy);
            if (exitMenu.activeInHierarchy)
            {
                
                moveAction.Disable();
                gameController.SetLastSelectable();
            
            }
            else
            {
                moveAction.Enable();
                gameController.SetLastSelectable();
            }
        }

       
        
    }
    
    #region Ground Check

    private void CheckGround()
    {
        if (characterController.isGrounded)
        {
            airTime = 0;
        }
        else
        {
            airTime += Time.deltaTime;
        }

        isGrounded = airTime < coyoteTime;
    }
    
    #endregion
    
    #region Animator

    private void UpdateAnimator()
    {
        Vector3 velocity = lastMovement;
        velocity.y = 0;
        float speed = velocity.magnitude;
        
         animator.SetFloat(Hash_MovementSpeed, speed);

    }

  
    public void AnimationsWeaponEquip(int id)
    {
        animator.SetTrigger(Hash_WeaponEquipTrigger);
        animator.SetInteger(Hash_WeaponId, id);
    }
    public void AnimationsWeaponUnequip(int id)
    {
        animator.SetTrigger(Hash_WeaponUnEquipTrigger);
        animator.SetInteger(Hash_WeaponId, id);
    }
    
    
    #endregion
    
    #region Camera

    // rotates the camera according to the mouse directions
    private void RotateCamera(Vector2 lookInput)
    {
        if (lookInput != Vector2.zero)
        {
            bool isMouseLook = IsMouseLook();

            float deltaTimeMultiplier = isMouseLook ? 1 : Time.deltaTime;
            //                     Bedingung     true                       false
            float sensitivity = isMouseLook ? mouseCameraSensitivity : controllerCameraSensitivity;

            lookInput *= deltaTimeMultiplier * sensitivity;
            
            cameraRotation.x += lookInput.y * cameraVerticalSpeed * (!isMouseLook && invertY ? -1 : 1);

            cameraRotation.y += lookInput.x * cameraHorizontalSpeed;

            cameraRotation.x = NormalizeAngle(cameraRotation.x);
            cameraRotation.y = NormalizeAngle(cameraRotation.y);

            cameraRotation.x = Mathf.Clamp(cameraRotation.x, verticalCameraRotationMin, verticalCameraRotationMax);
        }
        
        cameraTarget.rotation = Quaternion.Euler(cameraRotation.x, cameraRotation.y, 0);
    }

    private float NormalizeAngle(float angle)
    {
        angle %= 360;

        if (angle < 0)
        {
            angle += 360;
        }

        if (angle > 180)
        {
            angle -= 360;
        }

        return angle;
    }

    private bool IsMouseLook()
    {
        if (lookAction.activeControl == null)
        {
            return true;
        }

        return lookAction.activeControl.device.name == "Mouse";
    }
    
    #endregion
    
}
