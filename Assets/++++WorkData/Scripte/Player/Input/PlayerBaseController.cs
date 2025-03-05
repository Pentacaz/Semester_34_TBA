using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerBaseController : MonoBehaviour
{
    #region Camera
    //only relevant for bakery ---
    [SerializeField] private Transform cameraTarget;
    private Vector2 cameraRotation;
   
    [Header("Mouse Settings")]
    [Range(0f,2f)]
    [SerializeField] private float mouseCameraSensitivity = 1f;

    [Header("Controller Settings")]
    [Range(0f,2f)]
    [SerializeField] private float controllerCameraSensitivity = 1f;
    [SerializeField] private bool invertY = true;
   

    private Quaternion _playerRotation;
    private Quaternion _rotation;
    public Vector3 _playerDirection;
    private Vector3 _relativeDirection;
    private float _speed = 6;
    public Camera _Vcam;
    public GameObject trail;
    
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
    private InputAction _openMenu;


    #endregion

    #region Movement

    private Rigidbody _rigidbody;

    private Vector3 _currentVelocity;
    [FormerlySerializedAs("_moveInput")] public Vector2 moveInput;
    [FormerlySerializedAs("_lookInput")] public Vector2 lookInput;

    public float moveSpeed;
    public float maxForce;

    #region Dash

    public float dashSpeed;
    public float dashCooldown;
    public float dashCooldownTimer;
    public bool isDashing;
    public float pushBackForce = 1.0f;

    #endregion
 
    #endregion

    public Animator _animator;
    public GameObject exitMenu;
    private int _movementSpeedHash;
    private Collider _playerCollider;

   
    private PlayerCombatController _playerCombatController;
    private GameController gameController;
    private GameObject _dungeonIndicator;
    private GameObject _bakeryIndicator;
    public int engageId;

    
    void CreateInputActionsIfNoneExist()
    {
        if (_inputActions != null)
        {
            return;
        }
        _inputActions = new PlayerInputActions();
    }
    private void Awake()
    {   CreateInputActionsIfNoneExist();
        _inputActions = new PlayerInputActions();

        moveAction = _inputActions.Player.Move;
        lookAction = _inputActions.Player.Look;
        _dodgeAction = _inputActions.Player.Dash;
        _engageAction = _inputActions.Player.Engage;
        _attackAction = _inputActions.Player.Attack;
        _openMenu = _inputActions.Player.Menu;
        


    }

    void Start()
    {

        _rigidbody = GetComponent<Rigidbody>();
        _playerCollider = GetComponent<Collider>();
        _animator = GetComponentInChildren<Animator>();
        _movementSpeedHash = Animator.StringToHash("MovementSpeed");
        _playerCombatController = GetComponent<PlayerCombatController>();
     


    }


    public void OnEnable()
    {
        CreateInputActionsIfNoneExist();
        EnableInput();

        _dodgeAction.performed += OnDash;
        _attackAction.performed += OnBaseAttack;
        _openMenu.performed += OpenMenu;

        _engageAction.performed += Interact;

    }

    // Update is called once per frame
    void Update()
    {
        _bakeryIndicator = GameObject.Find("BakeryIndicator");
        _dungeonIndicator = GameObject.Find("DungeonIndicator");
        ReadInput();
        AnimationSetUp(_currentVelocity);

        DashActionCheck();

    }

    private void FixedUpdate()
    {
        OnMove(moveInput);
    }

   


    public void OnDisable()
    {
        DisableInput();
        _engageAction.performed -= Interact;
        _dodgeAction.performed -= OnDash;
        _dodgeAction.canceled -= OnDash;

        _attackAction.performed -= OnBaseAttack;
        _attackAction.canceled -= OnBaseAttack;
        
        _openMenu.performed -= OpenMenu;
       
    }

    private void OnDash(InputAction.CallbackContext ctx)
    {
        if (ctx.performed && _dungeonIndicator!= null)
        {
            trail.SetActive(true);
            isDashing = true;
            dashCooldownTimer = dashCooldown;


        }

    }

    public void DashActionCheck()
    {

        if (isDashing)
        {
            DeactivateCollider();
            dashCooldownTimer -= Time.deltaTime;
            if (dashCooldownTimer <= 0)
            {
                ActivateCollider();
                isDashing = false;
                trail.SetActive(false);
            }
        }


    }

    public void ActivateCollider()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            Collider enemyCollider = enemy.GetComponent<Collider>();

            if (enemyCollider != null)
            {
                Physics.IgnoreCollision(_playerCollider, enemyCollider, false);
            }
        }
    }

    public void DeactivateCollider()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            Collider enemyCollider = enemy.GetComponent<Collider>();

            if (enemyCollider != null)
            {
                Physics.IgnoreCollision(_playerCollider, enemyCollider, true);
            }
        }
    }


    private void OnBaseAttack(InputAction.CallbackContext ctx)
    {
        if (_dungeonIndicator!= null && _playerCombatController.canAttack)
        {
            if (ctx.performed)
            {
                _playerCombatController.attack = !_playerCombatController.attack;
                _playerCombatController.attackId = 1;
                _playerCombatController.AttackHandler();

            }
            else if (ctx.canceled)
            {

                _playerCombatController.attack = false;
            }
        }

    }

    private void OnHeavyAttack(InputAction.CallbackContext ctx)
    {
        if (_dungeonIndicator!= null && _playerCombatController.canHeavyAttack)
        {
            if (ctx.performed)
            {
                _playerCombatController.attack = !_playerCombatController.attack;
                _playerCombatController.attackId = 2;

            }
            else if (ctx.canceled)
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

        if (interactable == null)
        {
            return;
        }

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


