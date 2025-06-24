using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.SceneView;
//�쐬��:����


public class BazuriShot : MonoBehaviour// �o�Y���V���b�g���[�h�̐؂�ւ��̊Ǘ�
{
    [SerializeField] PlayerInput playerInput;
    [Header("�v���C���[")]
    [SerializeField] Transform player;
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
    
   [SerializeField] BazuriCameraMove cameraMove;
    private Coroutine bazuriCoroutine;
    private bool isBazuriMode = false;
    public bool IsBazuriMode
    {
        get { return isBazuriMode; }
    }
    private float  count;
    
    private void Start()
    {
        bazuriCamera.SetActive(false);
      
       
    }

  

   
    public void TryBazuriShot()
    {

        if (bazuriCoroutine != null)
        {
            StopCoroutine(bazuriCoroutine);
        }
        bazuriCoroutine=StartCoroutine(BazuriModeRoutine());
       
    }

    private IEnumerator BazuriModeRoutine()
    {
        isBazuriMode = true;
        mainCamera.SetActive(false);
        bazuriCamera.SetActive(true);
       
        playerInput.SwitchCurrentActionMap("Bazuri");
        ResetCamera();

        Time.timeScale= slowSpeed;

        yield return new WaitForSecondsRealtime(cameraTime);

        EndBazuriMode();
    }

    private void EndBazuriMode()
    {
        isBazuriMode = false;
        Time.timeScale = 1f;

        if(mainCamera!=null) mainCamera.SetActive(true);
        if (bazuriCamera != null) bazuriCamera.SetActive(false);

        playerInput.SwitchCurrentActionMap("Player");
        ResetCamera();
    }
    private void ResetCamera()
    {
        if (bazuriCamera != null)
        {
            
            bazuriCamera.transform.localPosition=Vector3.zero;
          
            bazuriCamera.transform.localRotation = Quaternion.identity;
            cameraMove.ResetCameraRotation();
        }
    }
    private void OnDestroy()
    {
        if (isBazuriMode)
        {
            EndBazuriMode();
        }
    }
}
