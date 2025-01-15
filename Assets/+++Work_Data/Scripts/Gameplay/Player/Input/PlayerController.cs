using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerController : MonoBehaviour
{
    private static readonly int Hash_MovementSpeed = Animator.StringToHash("MovementSpeed");
    private static readonly int Hash_Grounded = Animator.StringToHash("Grounded");
    private static readonly int Hash_Crouch = Animator.StringToHash("Crouch");

    #region Inspector


    [SerializeField] private float walkSpeed;
    [SerializeField] private float dashPower;
    
    [SerializeField] private float speedChangeRate = 10f;
    [SerializeField] private float rotationSpeed = 10f;

    [SerializeField] public float gravity = -19.62f;
    private Vector3 velocity;
    [Header("Camera")] 
    //needs a massive rework, doesnt fit for our type of game
    [SerializeField] private Transform cameraTarget;
    //rework
    [SerializeField] private float verticalCameraRotationMin = -30f;
    [SerializeField] private float verticalCameraRotationMax = 70f;
    [SerializeField] private float cameraHorizontalSpeed = 200f;
    [SerializeField] private float cameraVerticalSpeed = 130f;

    [Header("Animations")] 
    [SerializeField] private Animator animator;

    [SerializeField] private float coyoteTime;
    
    [Header("Mouse Settings")] 
    
    [SerializeField] private float mouseCameraSensitivity = 1f;
    
    [Header("Controller Settings")] 
    [SerializeField] private float controllerCameraSensitivity = 1f;
    [SerializeField] private bool invertY = true;

    
  

    #endregion
    
    #region Private Variables
    private Rigidbody _rigidbody;
 
    
    private GameInput _inputActions;
    private InputAction _moveAction;
    private InputAction _lookAction;
    private InputAction _dashAction;
    private Interactable _selectedInteractable;
    
    private Vector2 _moveInput;
    private Vector2 _lookInput;

    private Quaternion _characterTargetRotation = Quaternion.identity;
    
    private Vector2 _cameraRotation;
    private Vector3 _lastMovement;

    private bool _isGrounded;
    private bool _isRunning;
    private bool _isCrouching;
    private float _airTime;
    private float _currentSpeed = 3f;
    

    public Transform groundCheck;
    public LayerMask groundLayer;
    #endregion
    
    #region Event Functions
    private void Awake()
    {
    
        _inputActions = new GameInput();
        _moveAction = _inputActions.Player.Move;
        _lookAction = _inputActions.Player.Look;
        _dashAction = _inputActions.Player.Dash;

        _characterTargetRotation = transform.rotation;
        _cameraRotation = cameraTarget.rotation.eulerAngles;

        _currentSpeed = walkSpeed;
    }

public void OnEnable()
    {
        _inputActions.Enable();
        _dashAction.performed += DashAction;
       
    }

    private void Update()
    {
        ReadInput();

        Rotate(_moveInput);
        Move(_moveInput);

            //GroundCheck();
        UpdateAnimator();
    }

    private void LateUpdate()
    {
      RotateCamera(_lookInput);
      //_isGrounded = IsGrounded();
    }

  public void OnDisable()
    {
        _inputActions.Disable();
        _dashAction.performed -= DashAction;
    }

    private void OnDestroy()
    {
        
    }
    #endregion

    #region Input

    private void ReadInput()
    {
        _moveInput = _moveAction.ReadValue<Vector2>();
        _lookInput = _lookAction.ReadValue<Vector2>();
    }


    #endregion
    
    #region Movement
// #TODO rewrite cause this clips with walls.
    private void Rotate(Vector2 moveInput)
    {
        if (moveInput != Vector2.zero)
        {
            Vector3 inputDirection = new Vector3(moveInput.x, 0, moveInput.y).normalized;

            Vector3 worldInputDirection = cameraTarget.TransformDirection(inputDirection);
            worldInputDirection.y = 0;
            
            _characterTargetRotation = Quaternion.LookRotation(worldInputDirection);
        }

        if (Quaternion.Angle(transform.rotation, _characterTargetRotation) > 0.1f)
        {
            transform.rotation =
                Quaternion.Slerp(transform.rotation, _characterTargetRotation, rotationSpeed * Time.deltaTime);
        }
        else
        {
            transform.rotation = _characterTargetRotation;
        }
    }
    // #TODO rewrite to fit rb because NO way I'll write custom physics
    private void Move(Vector2 moveInput)
    {
        
        // _rigidbody.velocity = moveInput * (walkSpeed * Time.fixedDeltaTime); 
    }

    #endregion

    #region GroundCheck

  

 

    public void DashAction(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
           _rigidbody.AddForce(Vector3.forward * (dashPower * walkSpeed * Time.deltaTime));
        }
    }
    #endregion    
    
    #region Animator

    private void UpdateAnimator()
    {
        Vector3 velocity = _lastMovement;
        velocity.y = 0;
        float speed = velocity.magnitude;
        
        animator.SetFloat(Hash_MovementSpeed, speed);
        animator.SetBool(Hash_Grounded, _isGrounded);
        animator.SetBool(Hash_Crouch, _isCrouching);
    }

    #endregion
    
    #region Camera
    //fix clipping into walls while were at it...
   

    private void OnTriggerEnter(Collider other)
    {
        TrySelectInteractable(other);
    }
    

    private void OnTriggerExit(Collider other)
    {
        TryDeselectInteractable(other);
    }
    
 

    private void Interact(InputAction.CallbackContext ctx)
    {
        if (_selectedInteractable != null)
        {
            _selectedInteractable.Interact();
        }
    }

    private void TrySelectInteractable(Collider other)
    {
        Interactable interactable = other.GetComponent<Interactable>();

        if (interactable == null){ return; }

        if (_selectedInteractable != null)
        {
            _selectedInteractable.Deselect();
        }
        
        _selectedInteractable = interactable;
        _selectedInteractable.Select();
    }
    
    private void TryDeselectInteractable(Collider other)
    {
        Interactable interactable = other.GetComponent<Interactable>();

        if (interactable == null){ return; }

        if (interactable == _selectedInteractable)
        {
            _selectedInteractable.Deselect();
            _selectedInteractable = null;
        }
    }
    private void RotateCamera(Vector2 lookInput)
    {
        if (lookInput != Vector2.zero)
        {
            bool isMouseLook = IsMouseLook(); 
            //   ist mein bool true ? true : false
            float deltaTimeMultiplier = isMouseLook ? 1 : Time.deltaTime;

            float sensitivity = isMouseLook ? mouseCameraSensitivity : controllerCameraSensitivity;
  
            lookInput *= deltaTimeMultiplier * sensitivity;
            
            _cameraRotation.x += lookInput.y * cameraVerticalSpeed * (!isMouseLook && invertY ? -1 : 1);
            _cameraRotation.y += lookInput.x * cameraHorizontalSpeed;

            _cameraRotation.x = NormalizeAngle(_cameraRotation.x);
            _cameraRotation.y = NormalizeAngle(_cameraRotation.y);

            _cameraRotation.x = Mathf.Clamp(_cameraRotation.x, verticalCameraRotationMin, verticalCameraRotationMax);
        }
        
        cameraTarget.rotation = Quaternion.Euler(_cameraRotation.x, _cameraRotation.y, 0);
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
        if (_lookAction.activeControl == null)
        {
            return true;
        }

        return _lookAction.activeControl.device.name == "Mouse";
    }
    
    #endregion
}



