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
    [Header("必要なコンポーネント")]
    [SerializeField] BazuriShot bazuri;

    float cameraYaw;
    float cameraXaw;
    Vector2 moveInput;
    Vector2 lookInput;
    float verticalInput;



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
        verticalInput = context.ReadValue<float>();
    }
    public void OnShot()
    {

    }

    void Update()
    {
        if (bazuri.IsBazuriMode)
        {
            HandleCameraMovement();
            HandleCameraRotation();

        }
    }
    public void ResetCameraRotation()
    {
        if (bazuri.BazuriCamera != null)
        {
            cameraYaw = 0f;
            cameraXaw = 0f;
        }
    }
    public void HandleCameraRotation()//回転制御
    {
        cameraYaw += lookInput.x * rotateSpeed;
        cameraXaw += -lookInput.y * rotateSpeed;
        cameraXaw = Mathf.Clamp(cameraXaw, -90, 90);


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

        Vector3 horizontalmove = (foward * moveInput.y + right * moveInput.x) * moveSpeed * Time.unscaledDeltaTime;//前後左右移動

        bazuri.BazuriCamera.transform.Translate(horizontalmove, Space.World);

        if (Mathf.Abs(verticalInput) > 0.01f)//上下移動
        {
            Vector3 verticalmove = verticalInput * moveSpeed * Time.unscaledDeltaTime * Vector3.up;

            bazuri.BazuriCamera.transform.Translate(verticalmove, Space.World);

        }
    }
}
