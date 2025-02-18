using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;
public class CameraBehaviour : MonoBehaviour

{
    public CinemachineFreeLook freeLookCamera;
    public float mouseRotationSpeed = 200.0f;
    public float controllerRotationSpeed = 100.0f;
    private PlayerBaseController _playerBaseController;
    private InputAction lookAction;   // InputAction for looking

    private bool isUsingController = false; // Track whether controller is being used

    private void Awake()
    {


        _playerBaseController = GetComponent<PlayerBaseController>();
        

      
        lookAction =_playerBaseController.lookAction;
    }

    private void Update()
    {
        HandleInput();
    }

    private void HandleInput()
    {
        DetectInputSource();
        Vector2 lookInput = lookAction.ReadValue<Vector2>();
      
        if (isUsingController)
        {
          print("controller");
            float controllerX = lookInput.x * controllerRotationSpeed * Time.deltaTime;
            float controllerY = -lookInput.y * controllerRotationSpeed * Time.deltaTime;

            freeLookCamera.m_XAxis.Value += controllerX;
            freeLookCamera.m_YAxis.Value += controllerY;
        }
        else if (!isUsingController)
        {
            print("mouse");
            float mouseX = lookInput.x * mouseRotationSpeed * Time.deltaTime;
            float mouseY = -lookInput.y * mouseRotationSpeed * Time.deltaTime;

            freeLookCamera.m_XAxis.Value += mouseX;
            freeLookCamera.m_YAxis.Value += mouseY;
        }
    }

    private void DetectInputSource()
    {
       
        var  isGamepad = _playerBaseController.lookAction.activeControl!= null && _playerBaseController.lookAction.activeControl.device.name == "Controller";
        var  isMouse = _playerBaseController.lookAction.activeControl!= null &&_playerBaseController.lookAction.activeControl.device.name == "Mouse";
        
        if (isGamepad)
        {
            isUsingController = true;
            return;
        }
        
        if (isMouse)
        {
            isUsingController = false;
            return;
        }
        
    }
    
    
}