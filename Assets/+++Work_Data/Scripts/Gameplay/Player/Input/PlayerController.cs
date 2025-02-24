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
    private static readonly int Hash_Crouch = Animator.StringToHash("Crouch");

    #region Inspector


    [SerializeField] private float walkSpeed;
    [SerializeField] private float dashPower;
    [SerializeField] private Transform playerOrientation;
    [SerializeField] private float speedChangeRate = 10f;
    [SerializeField] private float rotationSpeed = 10f;
  
    [Header("Camera")] 
    //needs a massive rework, doesn't fit for our type of game
    [SerializeField] private Transform cameraTarget;
    [SerializeField] private float verticalCameraRotationMin = -30f;
    [SerializeField] private float verticalCameraRotationMax = 70f;
    [SerializeField] private float cameraHorizontalSpeed = 200f;
    [SerializeField] private float cameraVerticalSpeed = 130f;
    [SerializeField] private Transform cameraTransform; 
    [SerializeField] private float cameraDistance = 5.0f;
    [SerializeField] private float cameraRadius = 0.5f;
    [Header("Animations")] 
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
    private Vector3 _velocity;

    private Quaternion _characterTargetRotation = Quaternion.identity;
    
    private Vector2 _cameraRotation;
    private Vector3 _lastMovement;


    private bool _isRunning;
    private bool _isCrouching;

    private float _currentSpeed = 3f;

    [SerializeField] private bool isInDungeon;
  

    #endregion
    
    #region Event Functions
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
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
        //if (isInDungeon)
        _dashAction.performed += DashAction;
        
        
       
    }

    private void Update()
    {
        ReadInput();
        
    }

    private void LateUpdate()
    {
      RotateCamera(_lookInput);
  
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

   

    #endregion
    
    public void DashAction(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
           _rigidbody.AddForce(Vector3.forward * (dashPower * walkSpeed * Time.deltaTime));
        }
    }
  
    
    #region Animator



    #endregion
    
 
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
    
    #region Camera
    private void RotateCamera(Vector2 lookInput)
    {
        if (lookInput != Vector2.zero)
        {
            bool isMouseLook = IsMouseLook();

            float deltaTimeMultiplier = isMouseLook ? 1 : Time.deltaTime;
            float sensitivity = isMouseLook ? mouseCameraSensitivity : controllerCameraSensitivity;

            lookInput *= deltaTimeMultiplier * sensitivity;

            _cameraRotation.x += lookInput.y * cameraVerticalSpeed * (!isMouseLook && invertY ? -1 : 1);
            _cameraRotation.y += lookInput.x * cameraHorizontalSpeed;

            _cameraRotation.x = Mathf.Clamp(NormalizeAngle(_cameraRotation.x), verticalCameraRotationMin, verticalCameraRotationMax);
            _cameraRotation.y = NormalizeAngle(_cameraRotation.y);

            cameraTarget.rotation = Quaternion.Euler(_cameraRotation.x, _cameraRotation.y, 0);
            
        }
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
        return _lookAction.activeControl == null || _lookAction.activeControl.device.name == "Mouse";
    }

    #endregion
}



