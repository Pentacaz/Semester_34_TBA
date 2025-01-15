using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBaseController : MonoBehaviour
{
    #region Input

    private PlayerInputActions _inputActions;
    
    private InputAction _moveAction;
    private InputAction _runAction;
    private InputAction _lookAction;
    private InputAction _dodgeAction;
    private InputAction _engageAction;
    private InputAction _attackAction;


    #endregion

    private Animator _animator;
    private int _movementSpeedHash;


    private Rigidbody _rigidbody;
    
    private Vector3 _currentVelocity;
    private Vector2 _moveInput;
    private Vector2 _lookInput;


    public float moveSpeed;
    public float sensitivity;
    public float maxForce;
    

    
    
    
    private Cooking _cooking;

    public int engageId;

    private void Awake()
    {
    
        _inputActions = new PlayerInputActions();
        
        _moveAction = _inputActions.Player.Move;
        _runAction = _inputActions.Player.Run;
        _lookAction = _inputActions.Player.Look;
        _dodgeAction = _inputActions.Player.Dodge;
        _engageAction = _inputActions.Player.Engage;
        _attackAction = _inputActions.Player.Attack;

    }

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _animator = GetComponentInChildren<Animator>();
        _movementSpeedHash = Animator.StringToHash("MovementSpeed");
        
        _cooking = GameObject.FindObjectOfType<Cooking>();
    }
    
    
    private void OnEnable()
    {
     EnableInput();

        //_dodgeAction.performed += OnDodge;
        //_dodgeAction.canceled += OnDodge;
        
        _engageAction.performed += OnEngage;
        //_engageAction.canceled += OnEngage;
       
       // _attackAction.performed += OnAttack;


    }
    // Update is called once per frame
    void Update()
    {
        ReadInput();
        AnimationSetUp(_currentVelocity);
    }

    private void FixedUpdate()
    {
        OnMove(_moveInput);
    }
    void ReadInput()
    {
        _moveInput = _moveAction.ReadValue<Vector2>();
        _lookInput = _lookAction.ReadValue<Vector2>();
    }
    public void AnimationSetUp(Vector3 lastMovement)
    {
        //Update amimation
        Vector3 velocity = lastMovement;
        velocity.y = 0;
        float speed = velocity.magnitude;
        
        _animator.SetFloat(_movementSpeedHash, speed);
    }
    
    
 
    private void OnDisable()
    {
        DisableInput();
        _engageAction.canceled -= OnEngage;
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
        Vector3 targetVelocity = new Vector3(moveInput.x,0, moveInput.y);
        targetVelocity *= moveSpeed;


        targetVelocity = transform.TransformDirection(targetVelocity);

        Vector3 velocityChange = (targetVelocity - _currentVelocity);
        Vector3.ClampMagnitude(velocityChange, maxForce);
        _rigidbody.AddForce(velocityChange,ForceMode.VelocityChange);
    }

    public void OnLook(Vector2 lookInput)
    {
        
    }
    //TODO MOVEMENT -> TALKING -> COOKING -> BUY/SELL 
    #region Engage

    public void OnEngage(InputAction.CallbackContext ctx)
    {
        //#TODO Add SWITCH CASE for different engages.
        if (ctx.performed && _cooking.inRange)
        {
            StartCoroutine(_cooking.ResultTextDisplay("Purrfection!"));
            Debug.Log("SUCCESS");
        } 
        else
        {
            StartCoroutine(_cooking.ResultTextDisplay("Cat-astrophe..."));

            Debug.Log("FAIL");
        }
    }

    #endregion
    
}
