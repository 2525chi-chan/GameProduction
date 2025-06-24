using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BazuriCameraMove : MonoBehaviour
{
    [Header("�J�����̈ړ����x")]
    [SerializeField] float moveSpeed;
    [Header("�J�����̉�]���x")]
    [SerializeField] float rotateSpeed;
    [Header("�K�v�ȃR���|�[�l���g")]
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
    public void HandleCameraRotation()//��]����
    {
        cameraYaw += lookInput.x * rotateSpeed;
        cameraXaw += -lookInput.y * rotateSpeed;
        cameraXaw = Mathf.Clamp(cameraXaw, -90, 90);


        bazuri.BazuriCamera.transform.localRotation = Quaternion.Euler(cameraXaw, cameraYaw, 0);

    }
    public void HandleCameraMovement()//�ړ�����
    {
        Vector3 foward = bazuri.BazuriCamera.transform.forward;
        Vector3 right = bazuri.BazuriCamera.transform.right;

        foward.y = 0;
        right.y = 0;
        foward.Normalize();
        right.Normalize();

        Vector3 horizontalmove = (foward * moveInput.y + right * moveInput.x) * moveSpeed * Time.unscaledDeltaTime;//�O�㍶�E�ړ�

        bazuri.BazuriCamera.transform.Translate(horizontalmove, Space.World);

        if (Mathf.Abs(verticalInput) > 0.01f)//�㉺�ړ�
        {
            Vector3 verticalmove = verticalInput * moveSpeed * Time.unscaledDeltaTime * Vector3.up;

            bazuri.BazuriCamera.transform.Translate(verticalmove, Space.World);

        }
    }
}
