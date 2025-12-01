using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[RequireComponent(typeof(CharacterController))]
public class AlriksFPSController : MonoBehaviour
{
    [Header("Pickup Holding")]

    public GameObject pickupObject;
    public UnityEngine.UI.Image scopeUI;

    public Camera playerCamera;
    private Transform playerCameraTransform;
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
    //private Camera cam;
    float currentTargetFOV;


    [Header("Tilt Looking settings")]
    public float defaultLookSpeed = 2f;
    private float lookSpeed;
    public float scopelookSpeed = 0.2f;
    public float lookYLimit = 45f;


    Vector3 moveDirection = Vector3.zero;
    float rotationY = 0;

    public bool canMove = true;
    public bool isRunning, isScoped,isCinemachine, isHoldingPickup;


    CharacterController characterController;
   // public CinemachineVirtualCamera vcam;  // gammal
    public CinemachineCamera ccam;
    private const int DISTANCE = 5;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        scopeUI = GameObject.Find("Scope").GetComponent<Image>();
        scopeUI.gameObject.SetActive(false);
        lookSpeed = defaultLookSpeed;

        #region init Camera
        isCinemachine = GameObject.FindAnyObjectByType<CinemachineCamera>();
        print(isCinemachine + " it EXISTS , we have cinemachine");
        if (isCinemachine)
        {
            ccam = GameObject.FindAnyObjectByType<CinemachineCamera>().GetComponent<CinemachineCamera>();
            playerCameraTransform = ccam.transform;
            currentTargetFOV = normalFOV;
            ccam.Lens.FieldOfView = normalFOV;
        }
        else
        {
            playerCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
            playerCameraTransform = playerCamera.transform;
            currentTargetFOV = normalFOV;
            playerCamera.fieldOfView = normalFOV;
        }
        #endregion
    }

    void Update()
    {

        #region  Movment
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);


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
            lookSpeed = isScoped ? scopelookSpeed : defaultLookSpeed;
            currentTargetFOV = isScoped ? scopeFOV : normalFOV;
            scopeUI.gameObject.SetActive(isScoped);
            if (isCinemachine)
                ccam.Lens.FieldOfView = currentTargetFOV;
            else
                playerCamera.fieldOfView = currentTargetFOV;
           
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
            rotationY += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotationY = Mathf.Clamp(rotationY, -lookYLimit, lookYLimit);
            if (!isCinemachine)
                playerCamera.transform.localRotation = Quaternion.Euler(rotationY, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
            if (isHoldingPickup) pickupObject.transform.localPosition = new Vector3(pickupObject.transform.localPosition.x,-rotationY*0.1f, pickupObject.transform.localPosition.z);

        }
        #endregion

        #region Lerp FOV

        //if(cam.fieldOfView >= currentTargetFOV - 0.01f && cam.fieldOfView <= currentTargetFOV + 0.01f)
        if (isCinemachine)
            ccam.Lens.FieldOfView = Mathf.Lerp(ccam.Lens.FieldOfView, currentTargetFOV, Time.deltaTime * fovLerpSpeed);
        else
            playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, currentTargetFOV, Time.deltaTime * fovLerpSpeed);

        #endregion

        #region Pickup
        if (Input.GetMouseButtonDown(0))
        {
            print("GRABB");
            RaycastHit hit;
            // Does the ray intersect any objects excluding the player layer
            if (Physics.Raycast(playerCameraTransform.position, playerCameraTransform.TransformDirection(Vector3.forward), out hit, DISTANCE, LayerMask.GetMask("Default")))
            {
                Debug.DrawRay(playerCameraTransform.position, playerCameraTransform.TransformDirection(Vector3.forward) * hit.distance, Color.red);
                Debug.Log("Did Hit");
                //shoot(hit.collider.gameObject);
                Grabb(hit.collider.gameObject);
            }
            else
            {
                Debug.DrawRay(playerCameraTransform.position, playerCameraTransform.TransformDirection(Vector3.forward) * DISTANCE, Color.white);
                Debug.Log("Did not Hit");
            }
           
        }


        if (Input.GetMouseButtonUp(0))
        {
            print("Release");
                Release();
      
        }
        #endregion
    }

    private void Release()
    {
        if (pickupObject) { 
            pickupObject.transform.SetParent(null);
            pickupObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        }
        isHoldingPickup = false;
    }

    private void shoot( GameObject go)
    {
        //Grabb
        
    }

    private void Grabb(GameObject go)
    {
        if (!isHoldingPickup && go.tag.Equals("Pickup"))
        {
            Debug.LogWarning("PICKUP!!!!!!");
            pickupObject = go;
            pickupObject.transform.SetParent(transform);
            pickupObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition;
            isHoldingPickup = true;
            // Destroy(go);
        }
    }
}