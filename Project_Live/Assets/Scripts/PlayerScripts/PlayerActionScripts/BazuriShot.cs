using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.SceneView;
//�쐬��:����


public class BazuriShot : MonoBehaviour// �o�Y���V���b�g���[�h�̐؂�ւ��̊Ǘ�
{
    [Header("���C���̃J����")]
    [SerializeField]CinemachineBrain cinemachineBrain;
    [SerializeField] PlayerInput playerInput;
    [Header("�v���C���[�����삷��J����")]
    [SerializeField] CinemachineFreeLook mainCamera;
    [Header("�o�Y���V���b�g�̍ۂɑ��삷��J����")]
    [SerializeField] CinemachineVirtualCamera bazuriCamera;

    const int highPriority = 10; //�\���������
   const int lowPriority = 0; //�\�������Ȃ���  
    public CinemachineVirtualCamera BazuriCamera
    {
        get
        {
            return bazuriCamera;
        }
        set
        {
            bazuriCamera = value;
        }
    }

    [Header("�J�����̑��쎞��")]
    [SerializeField] float cameraTime;
    [Header("�X���[���̃Q�[�����x(1��������Ȃ��ƃX���[�ɂȂ�Ȃ�)")]
    [SerializeField] float slowSpeed;
    [Header("�o�Y���V���b�g�̃X�g�b�N")]
    [SerializeField] int shotStock;
    [Header("�f�t�H���g�̃��C���[(�J��������ɗp���郌�C���[)")]
    [SerializeField] LayerMask layer;
    [Header("�{�^���������烂�[�h�ڍs�̃C���^�[�o��")]
    [SerializeField]float shotIntarval;
    [Header("�K�v�ȃR���|�[�l���g")]
    [SerializeField] BazuriCameraMove cameraMove;
    [SerializeField] BazuriShotAnalyzer analyzer;
    [SerializeField] GoodSystem goodSystem;
    [SerializeField]CameraFlash cameraFlash;
    [SerializeField] Transform player;
    [SerializeField] GameObject effect;
    private Camera analyzerCamera;
    private Coroutine bazuriCoroutine;
    private bool isBazuriMode = false;
    public bool IsBazuriMode
    {
        get { return isBazuriMode; }
        set {isBazuriMode=value;}
    }
    private void Start()
    {
        if (bazuriCamera != null&&mainCamera!=null)
        {
            bazuriCamera.Priority =lowPriority ;
            mainCamera.Priority = highPriority;
            analyzerCamera =bazuriCamera.GetComponent<Camera>();
            cinemachineBrain.m_CameraActivatedEvent.AddListener(OnCameraActivated);
        }

    }

    public void TryBazuriShot()
    {

        if (bazuriCoroutine != null)
        {
            StopCoroutine(bazuriCoroutine);
        }
        if (shotStock > 0)
        {
            bazuriCoroutine = StartCoroutine(BazuriModeRoutine());
        }
        

    }

    private IEnumerator BazuriModeRoutine()//�o�Y���V���b�g���[�h�ɐ؂�ւ�
    {
        cameraFlash.ResetAlpha();
      
        isBazuriMode = true;
         BazuriEffect();
//yield return new WaitForSeconds(shotIntarval);

        mainCamera.Priority=lowPriority;
        bazuriCamera.Priority = highPriority;

        float elapsed = 0f;
        playerInput.SwitchCurrentActionMap("Bazuri");
         ResetCamera();

      

        Time.timeScale = slowSpeed;

        while (elapsed < cameraTime)//���쎞�Ԓ��ɃV���b�g�{�^�����������΃o�Y���V���b�g���f
        {
            if (playerInput.actions["Shot"].WasPressedThisFrame())
            {
                cameraFlash.StartFlash();
               goodSystem.AddGood(analyzer.Analyzer(analyzerCamera, layer));
                break;
            }

            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }
       

        EndBazuriMode();
    }

    private void EndBazuriMode()//�v���C���[���[�h�ɐ؂�ւ�
    {
        isBazuriMode = false;
        Time.timeScale = 1f;

        if (mainCamera != null && bazuriCamera != null)
        {
            mainCamera.Priority = highPriority;
            bazuriCamera.Priority = lowPriority;
        }
        shotStock--;
        
        playerInput.SwitchCurrentActionMap("Player");
       // ResetCamera();
    }
    private void ResetCamera()//�J�����ʒu�A��]�̏�����
    {
        if (bazuriCamera != null)
        {

            bazuriCamera.transform.localPosition = Vector3.zero;
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
    public void RecoveryShotStock(int count)
    {
        shotStock+=count;
    }
     void OnCameraActivated(ICinemachineCamera newcam,ICinemachineCamera oldcam)
    {
    
        ResetCamera();
    }
    public void BazuriEffect()
    {
        if (effect != null && player != null)
        {
            Instantiate(effect, player.position, Quaternion.identity,player);
        }

    }
}
