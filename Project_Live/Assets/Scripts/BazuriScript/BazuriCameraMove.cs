using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BazuriCameraMove : MonoBehaviour
{
    [Header("カメラの移動速度")]
    [SerializeField] float moveSpeed;
    [Header("カメラの回転速度")]
    [SerializeField] float rotateSpeed;
    [SerializeField] float rotateSmoothingSpeed;
    [SerializeField] BazuriShot bazuri;

    float cameraYaw;
    float cameraXaw;
    Vector2 moveInput;
    Vector2 lookInput;
    float verticalInput;
    Quaternion targetRotation;
    private void Start()
    {
       // ResetCameraRotation();
       
       
    }
   
   public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }
    public void OnLook(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();
    }
    public void OnCameraUpDown(InputAction.CallbackContext context)
    {
        verticalInput=context.ReadValue<float>();
    }
    public void OnShot()
    {

    }

    void Update()
    {
        if(bazuri.IsBazuriMode) {
            HandleCameraMovement();
            HandleCameraRotation();
         
        }
    }
    public void ResetCameraRotation()
    {
        if (bazuri.BazuriCamera != null)
        {

            cameraYaw = 0f;
            cameraXaw =0f;
          
        }
    }
    public void HandleCameraRotation()//回転制御
    {
        cameraYaw += lookInput.x * rotateSpeed;
        cameraXaw += -lookInput.y * rotateSpeed;
        cameraXaw = Mathf.Clamp(cameraXaw, -90, 90);
        targetRotation = Quaternion.Euler(cameraXaw, cameraYaw, 0);

        bazuri.BazuriCamera.transform.localRotation = Quaternion.Euler(cameraXaw, cameraYaw, 0);
           
    }
    public void HandleCameraMovement()//移動制御
    {
        Vector3 foward = bazuri.BazuriCamera.transform.forward;
        Vector3 right = bazuri.BazuriCamera.transform.right;

        foward.y = 0;
        right.y = 0;
        foward.Normalize();
        right.Normalize();

        Vector3 move = foward * moveInput.y + right * moveInput.x;

        bazuri.BazuriCamera.transform.position += move * moveSpeed * Time.unscaledDeltaTime;

        if (Mathf.Abs(verticalInput) > 0.01f)
        {
            Vector3 verticalmove = verticalInput * moveSpeed * Time.unscaledDeltaTime * Vector3.up;
            bazuri.BazuriCamera.transform.Translate(verticalmove, Space.World);

        }
    }
}
