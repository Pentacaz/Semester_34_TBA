using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerBaseController : MonoBehaviour
{

  
  
  
    [SerializeField] private float speedChangeRate = 10f;
    [SerializeField] private float rotationSpeed = 10f;
    #region Camera

    private Quaternion _playerRotation;
    private Quaternion _rotation;
    public Vector3 _playerDirection;
    private Vector3 _relativeDirection;
    private float _turnSpeed = 8;
    private float _speed = 6;
    public Camera _Vcam;
    public GameObject trail;
    [Header("Camera")]
    //needs a massive rework, doesn't fit for our type of game
    [SerializeField]
    private Transform cameraTarget;

    [SerializeField] private float verticalCameraRotationMin = -30f;
    [SerializeField] private float verticalCameraRotationMax = 70f;
    [SerializeField] private float cameraHorizontalSpeed = 200f;
    [SerializeField] private float cameraVerticalSpeed = 130f;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float cameraDistance = 5.0f;
    [SerializeField] private float cameraRadius = 0.5f;

    [Header("Mouse Settings")] [SerializeField]
    private float mouseCameraSensitivity = 1f;

    [Header("Controller Settings")] [SerializeField]
    private float controllerCameraSensitivity = 1f;

    [SerializeField] private bool invertY = true;

    #endregion
    
    #region Input

    private PlayerInputActions _inputActions;
    public Interactable selectedInteractable;
    [FormerlySerializedAs("_moveAction")] public InputAction moveAction;
    private InputAction _runAction;
    [FormerlySerializedAs("_lookAction")] public InputAction lookAction;
    private InputAction _dodgeAction;
    private InputAction _engageAction;
    private InputAction _attackAction;


    #endregion

    #region Movement
    private Rigidbody _rigidbody;

    private Vector3 _currentVelocity;
    [FormerlySerializedAs("_moveInput")] public  Vector2 moveInput;
    [FormerlySerializedAs("_lookInput")] public Vector2 lookInput;

    public float moveSpeed;
    public float maxForce;

    #region  Dash

    public float dashForce;
    public float dashSpeed;
    public float dashUpForce;
    public float dashDuration;
    
    public float dashCooldown;
    public float dashCooldownTimer;

    public bool isDashing;
    public float pushBackForce = 1.0f;
    
    [FormerlySerializedAs("isDashing")] public bool canDash;

    #endregion
   
    #endregion

    private Animator _animator;
    private int _movementSpeedHash;

    private Cooking _cooking;
    private PlayerCombatController _playerCombatController;

    public int engageId;
    
    private void Awake()
    {
      
       

        _inputActions = new PlayerInputActions();

        moveAction = _inputActions.Player.Move;
        lookAction = _inputActions.Player.Look;
        _dodgeAction = _inputActions.Player.Dash;
        _engageAction = _inputActions.Player.Engage;
        _attackAction = _inputActions.Player.Attack;
      

    }

    void Start()
    {
        
        _rigidbody = GetComponent<Rigidbody>();
        _animator = GetComponentInChildren<Animator>();
        _movementSpeedHash = Animator.StringToHash("MovementSpeed");
        _playerCombatController = GetComponent<PlayerCombatController>();
        _cooking = GameObject.FindObjectOfType<Cooking>();
      


    }


    public void OnEnable()
    {
        EnableInput();

       _dodgeAction.performed += OnDash;
       //_dodgeAction.canceled += OnDash;
       
       _attackAction.performed += OnBaseAttack;
       //_attackAction.canceled += OnBaseAttack;
        
        
        _engageAction.performed += Interact;

    }

    // Update is called once per frame
    void Update()
    {
     
        ReadInput();
        AnimationSetUp(_currentVelocity);

        if (isDashing) 
        { dashCooldownTimer -= Time.deltaTime;
            if (dashCooldownTimer <= 0)
            {
                //Physics.IgnoreLayerCollision(, false);
                isDashing = false;
                trail.SetActive(false);
            } 
        }
     
    }

    private void FixedUpdate()
    {
        OnMove(moveInput);
    }

    private void LateUpdate()
    {
        OnLook(lookInput);
    }

 
    public void OnDisable()
    {
        DisableInput();
        _engageAction.performed-= Interact;
        _dodgeAction.performed -= OnDash;
        _dodgeAction.canceled -= OnDash;
        
        _attackAction.performed -= OnBaseAttack;
        _attackAction.canceled -= OnBaseAttack;
    }

    private void OnDash(InputAction.CallbackContext ctx)
    {
                if (ctx.performed)
                {  trail.SetActive(true);
                    isDashing = true;
                    dashCooldownTimer = dashCooldown;
                  

                }
                else if (ctx.canceled)
                { 
                    //isDashing = false;
                    
                }
    }

    private void OnBaseAttack(InputAction.CallbackContext ctx)
    {
        if (_playerCombatController.canAttack)
        {
            if(ctx.performed)
            {
                _playerCombatController.attack = !_playerCombatController.attack;
                _playerCombatController.attackId = 1;
                _playerCombatController.AttackHandler(); 
           
            }else if (ctx.canceled)
            {
                
                _playerCombatController.attack = false;
            }
        }
      
    }

    private void OnHeavyAttack(InputAction.CallbackContext ctx)
    {
        if (_playerCombatController.canHeavyAttack)
        {
            if(ctx.performed)
            {
                _playerCombatController.attack = !_playerCombatController.attack;
                _playerCombatController.attackId = 2;
           
            }else if (ctx.canceled)
            {
                
                _playerCombatController.attack = false;
            }
        }
    }
    

    public void EnableInput()
    {
        _inputActions.Enable();
    }

    public void DisableInput()
    {
        _inputActions.Disable();
    }


    public void OnMove(Vector2 moveInput)
    {
        _currentVelocity = _rigidbody.velocity;
        Vector3 targetVelocity = new Vector3(moveInput.x, 0, moveInput.y);
        targetVelocity *= isDashing ? dashSpeed : moveSpeed;

        targetVelocity = transform.TransformDirection(targetVelocity);

        Vector3 velocityChange = (targetVelocity - new Vector3(_currentVelocity.x, 0, _currentVelocity.z));
        velocityChange = Vector3.ClampMagnitude(velocityChange, maxForce);
        _rigidbody.AddForce(velocityChange, ForceMode.VelocityChange);

        RotateTarget();
    }



    public void RotateTarget()
    {
        
        /*

comment comment comment

*/ 
        _relativeDirection = _Vcam.transform.TransformDirection(_playerDirection);
        _relativeDirection.y = 0f;
        _relativeDirection.Normalize();

        _rotation = _Vcam.transform.rotation;
        _rotation.x = 0;
        _rotation.z = 0;
        transform.rotation = _rotation;


        _rigidbody.MovePosition(_rigidbody.position + _relativeDirection * (_speed * Time.deltaTime));
        
    }

   
    #region Engage
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
   
        if (selectedInteractable != null)
        {
            selectedInteractable.Interact();
            Debug.Log("interacted");
            //_navMeshPatrol.StopPatrolForDialog();
        }
    }



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
    
    private void TryDeselectInteractable(Collider other)
    {
        Interactable interactable = other.GetComponent<Interactable>();

        if (interactable == null){ return; }

        if (interactable == selectedInteractable)
        {
            selectedInteractable.Deselect();
            selectedInteractable = null;
        }
    }

    #endregion
    
    public void OnLook(Vector2 lookInput)
    {
     
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
        return lookAction.activeControl != null && lookAction.activeControl.device.name == "Mouse";
    }

    private bool IsControllerLook()
    {
        return lookAction.activeControl != null && lookAction.activeControl.device.name == "Gamepad";
    }
    
    void ReadInput()
    {
        moveInput = moveAction.ReadValue<Vector2>();
        lookInput = lookAction.ReadValue<Vector2>();
    }

    public void AnimationSetUp(Vector3 lastMovement)
    {
        //Update amimation
        Vector3 velocity = lastMovement;
        velocity.y = 0;
        float speed = velocity.magnitude;

        _animator.SetFloat(_movementSpeedHash, speed);
    }
    
    

    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            Debug.Log("wall collides");
            Vector3 collisionNormal = collision.contacts[0].normal;
            Vector3 pushBackDirection = -collisionNormal;

            if (_rigidbody != null)
            {
                _rigidbody.AddForce(pushBackDirection * pushBackForce, ForceMode.Impulse);
            }
        }
    }
    
    
}


