using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBaseController : MonoBehaviour
{
    private PlayerInputActions _inputActions;
    
    private InputAction _moveAction;
    //private InputAction lookAction;
    private InputAction _dodgeAction;
    private InputAction _engageAction;
    private InputAction _attackAction;


    private Cooking _cooking;

    public int engageId;

    // Start is called before the first frame update

    private void Awake()
    {
    
        _inputActions = new PlayerInputActions();
        
        _moveAction = _inputActions.Player.Move;
        //_lookAction = _inputActions.Player.Look;
        _dodgeAction = _inputActions.Player.Dodge;
        _engageAction = _inputActions.Player.Engage;
        _attackAction = _inputActions.Player.Attack;


        _cooking = GameObject.FindObjectOfType<Cooking>();
    
    }

    void Start()
    {

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

    }

   

    private void OnDisable()
    {

        DisableInput();
        //_dodgeAction.performed -= OnDodge;
        //_dodgeAction.canceled -= OnDodge;
        
        //_engageAction.performed -= OnEngage;
        _engageAction.canceled -= OnEngage;
       
       // _attackAction.performed -= OnAttack;
        
        
    }
    
    public void EnableInput()
    {
        _inputActions.Enable();
    }

    public void DisableInput()
    {
        _inputActions.Disable();
    }
    //TODO MOVEMENT -> TALKING -> COOKING -> BUY/SELL 

    public void OnDodge(InputAction.CallbackContext ctx)
    {
        
    }
    
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

    public void OnAttack(InputAction.CallbackContext ctx)
    {
        
    }
}
