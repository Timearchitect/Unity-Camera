using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class AlriksFPSController : MonoBehaviour
{
    public Camera playerCamera;
    [Header("Movement settings")]
    public float walkSpeed = 6f;
    public float runSpeed = 12f;
    public float jumpPower = 7f;
    public float gravity = 10f;

    [Header("FOV settings")]
    public float normalFOV = 50f;
    public float sprintFOV = 150f;
    public float scopeFOV = 10f;
    public float fovLerpSpeed = 3f;
    private Camera cam;
    float currentTargetFOV;


    [Header("Tilt Looking settings")]
    public float lookSpeed = 2f;
    public float lookXLimit = 45f;


    Vector3 moveDirection = Vector3.zero;
    float rotationX = 0;

    public bool canMove = true;
    public bool isRunning, isScoped,isCinemachine;


    CharacterController characterController;
   // public CinemachineVirtualCamera vcam;  // gammal
    public CinemachineCamera ccam;

    /*void Start()
    {
        if (vCam == null)
            vCam = GetComponent<CinemachineCamera>();

        var lens = vCam.m_Lens;
        lens.FieldOfView = normalFOV;
        vCam.m_Lens = lens;
    }

    void Update()
    {
        bool isSprinting = Keyboard.current.leftShiftKey.isPressed; // byt till din egen flagga vid behov

        float targetFov = isSprinting ? sprintFOV : normalFOV;

        var lens = vCam.m_Lens;
        lens.FieldOfView = Mathf.Lerp(
            lens.FieldOfView,
            targetFov,
            Time.deltaTime * fovLerpSpeed
        );
        vCam.m_Lens = lens;
    }*/
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

    
        if (cam == null)
            cam = GameObject.FindAnyObjectByType<Camera>();

  
        isCinemachine = GameObject.FindAnyObjectByType<CinemachineCamera>();
        print(isCinemachine + " it EXISTS , we have cinemachine");
        if (isCinemachine)
        {
            ccam = GameObject.FindAnyObjectByType<CinemachineCamera>().GetComponent<CinemachineCamera>();
            currentTargetFOV = normalFOV;
            // var lens = ccam.Lens;
            // lens.FieldOfView = normalFOV;
            // ccam.Lens = lens;

            ccam.Lens.FieldOfView = normalFOV;


        }
        else
        {
            playerCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
            currentTargetFOV = normalFOV;
            cam.fieldOfView = normalFOV;
        }
    }

    void Update()
    {

        #region  Movment
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        Input.GetKey(KeyCode.Escape);

        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.LeftShift))
        {
            // Om den onpress ner eller inte onpress ner
            isRunning = Input.GetKeyDown(KeyCode.LeftShift) || !Input.GetKeyUp(KeyCode.LeftShift);
            currentTargetFOV = isRunning ? sprintFOV : normalFOV;
        }

        //scope
        if (Input.GetMouseButtonDown(1) || Input.GetMouseButtonUp(1))
        {
            // Om den onpress ner eller inte onpress ner
            isScoped = Input.GetMouseButtonDown(1) | !Input.GetMouseButtonUp(1);
            lookSpeed = isScoped ? 0.1f : 2;
            currentTargetFOV = isScoped ? scopeFOV : normalFOV;

            if (isCinemachine)
                cam.fieldOfView = currentTargetFOV;
           else
                cam.fieldOfView = currentTargetFOV;
           
        }


        float curSpeedX = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Vertical") : 0;
        float curSpeedY = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Horizontal") : 0;
        float movementDirectionY = moveDirection.y;
        moveDirection = (forward * curSpeedX) + (right * curSpeedY);

        #endregion

        #region Jumping
        if (Input.GetButton("Jump") && canMove && characterController.isGrounded)
            moveDirection.y = jumpPower;
        else
            moveDirection.y = movementDirectionY;
        

        if (!characterController.isGrounded)
            moveDirection.y -= gravity * Time.deltaTime;


        #endregion

        #region Rotation
        characterController.Move(moveDirection * Time.deltaTime);

        if (canMove)
        {
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            if (!isCinemachine)
                playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
          

        }
        #endregion

   
  
  
       //if(cam.fieldOfView >= currentTargetFOV - 0.01f && cam.fieldOfView <= currentTargetFOV + 0.01f)
       if(isCinemachine)
            ccam.Lens.FieldOfView = Mathf.Lerp(ccam.Lens.FieldOfView, currentTargetFOV, Time.deltaTime * fovLerpSpeed);
        else
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, currentTargetFOV, Time.deltaTime * fovLerpSpeed);
      


    }
}