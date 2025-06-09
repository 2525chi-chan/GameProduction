using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
//�쐬��:����
public class BazuriShot : MonoBehaviour// �o�Y���V���b�g���[�h�̐؂�ւ��̊Ǘ�
{
    [SerializeField] PlayerInput playerInput;
    [Header("���C���̃J����")]
    [SerializeField] GameObject mainCamera;
    [Header("�o�Y���V���b�g�̍ۂɑ��삷��J����")]
    [SerializeField] GameObject bazuriCamera;
    public GameObject BazuriCamera
    {
        get
        {   return bazuriCamera;
        }
        set
        {    bazuriCamera = value;
        }
    }
    [Header("�J�����̑��쎞��")]
    [SerializeField] float cameraTime;
    [Header("�X���[���̃Q�[�����x(1��������Ȃ��ƃX���[�ɂȂ�Ȃ�)")]
    [SerializeField] float slowSpeed;

    [SerializeField] Transform defaultPos;
    private bool isBazuri = false;
    public bool IsBazuri
    {
        get { return isBazuri; }
    }
    private float  count;
    
    private void Start()
    {
        bazuriCamera.SetActive(false);
        defaultPos=bazuriCamera.transform;
    }

    private void Update()
    {
        if (isBazuri)
        {
            count += Time.unscaledDeltaTime;
            if (count >= cameraTime)
            {
                isBazuri = false;
                mainCamera.SetActive(true);
                bazuriCamera.SetActive(false);
                Time.timeScale = 1;
                playerInput.SwitchCurrentActionMap("Player");
                bazuriCamera.transform.position = defaultPos.position;
                count = 0;

            }
        }
    }
    public void TryBazuriShot()
    {
        if (isBazuri) return;
        Debug.Log("�o�Y��V���b�g�����I");
        isBazuri = true;
        mainCamera.SetActive(false);
        bazuriCamera.SetActive(true);
        Time.timeScale = slowSpeed;
        playerInput.SwitchCurrentActionMap("Bazuri");

    }
}
