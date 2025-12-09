using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
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
    [Header("�o�Y���V���b�g�̍ő�X�g�b�N")]
    [SerializeField] int shotStock;
    [Header("�o�Y���V���b�g�̃N�[���^�C��")]
    [SerializeField] float coolTime;
    [Header("�f�t�H���g�̃��C���[(�J��������ɗp���郌�C���[)")]
    [SerializeField] List<LayerMask> layers;
    [Header("���Ԃ��x���Ȃ鑬�x")]
    [SerializeField]float timeScaleDownSpeed;
    [Header("シャッター音")]
    [SerializeField] AudioClip shutter;
    [Header("シャッター後音声")]
    [SerializeField] AudioClip shutterAfter;

    [Header("�o�Y���V���b�g�`��p")]
    [SerializeField] RenderTexture bazuriTexture;
    [SerializeField] RawImage rawImage;
    [SerializeField]TMP_Text bazuriText;
    [SerializeField] float fleezeTime = 0.2f;
    [SerializeField] float countSpeed=5;
    [SerializeField]float countAfterTime = 0.5f; //�J�E���g��̑ҋ@����   
    [Header("�K�v�ȃR���|�[�l���g")]
    [SerializeField] BazuriCameraMove cameraMove;
    [SerializeField] BazuriShotAnalyzer analyzer;
    [SerializeField]RequestManager requestManager;

    [SerializeField]Live2DTalkPlayer talkPlayer;

    [SerializeField] AudioSource SE;
    [SerializeField] GoodSystem goodSystem;
    [SerializeField]CameraFlash cameraFlash;
    [SerializeField] ZoomCamera zoomCamera;
    [SerializeField] Transform player;
    [SerializeField] GameObject effect;
    private float countCoolTime;
    private Camera analyzerCamera;
    private Coroutine bazuriCoroutine;  
    private Coroutine slowTimeCoroutine = null;
    private Coroutine fleezeCoroutine=null;
    private Coroutine countCoroutine = null;
    private int currentStock;
    bool shotTaken = false; //�o�Y���V���b�g�̃g�[�N��������Ă��邩�ǂ���
    public int CurrentStock
    {
        get { return currentStock; }
        set { currentStock = value; }
    }
    public int ShotStock
    {
        get { return shotStock; }
        set { shotStock = value; }
    }
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

        rawImage.enabled = false;

        countCoolTime = coolTime;
     
    }
    public void Update()
    {
        if (!isBazuriMode)
        {
            countCoolTime += Time.deltaTime;
        }
    }

    public void TryBazuriShot()
    {

        if (bazuriCoroutine != null)
        {
            StopCoroutine(bazuriCoroutine);
        }
        if (currentStock > 0&&countCoolTime>coolTime)
        {
           talkPlayer.PlayTalk("BazuriShot_Before");
            bazuriCoroutine = StartCoroutine(BazuriModeRoutine());
            countCoolTime = 0f;
        }
        

    }
    private IEnumerator SlowTimeScaleDown(float start,float target,float speed)
    {
        float t = 0;
        float duration = 1.0f / speed;
        while (t < duration)
        {
            if (isBazuriMode)//�r���Ńo�Y���V���b�g���[�h��������ꂽ�狭���I��
            {
                t += Time.unscaledDeltaTime;
                Time.timeScale = Mathf.Lerp(start, target, t / duration);

                yield return null;

            }
            else
            {

                Time.timeScale = 1f;
                slowTimeCoroutine = null;
                yield break;

            }
           
        } 
        Time.timeScale = target;
    }
    public IEnumerator FleezeScreen()
    {
        if (bazuriCamera == null) yield return null;


        var cam = analyzerCamera;
        var originalTarget = cam.targetTexture;

        cam.targetTexture = bazuriTexture;
        cam.Render();
        cam.targetTexture = originalTarget;


        rawImage.texture = bazuriTexture;
        rawImage.enabled = true;
        float t = 0f;
        while (t < fleezeTime)
        {
            t += Time.unscaledDeltaTime;


            yield return null;
        }
        rawImage.enabled = false;
        fleezeCoroutine = null;

    }
    public void StartSlowTimeScaleDown(float start,float target,float speed)//���Ԃ�x�����鏈���B�P��̃R���[�`���ł��������Ȃ��悤�ɂ���
    {

        if(slowTimeCoroutine != null)
        {
            StopCoroutine(slowTimeCoroutine);
            slowTimeCoroutine = null;
        }
        slowTimeCoroutine = StartCoroutine(SlowTimeScaleDown(start,target, speed));

    }
    private IEnumerator BazuriModeRoutine()//�o�Y���V���b�g���[�h�ɐ؂�ւ�
    {
        cameraFlash.ResetAlpha();
      
        isBazuriMode = true;
         BazuriEffect();


        mainCamera.Priority=lowPriority;
        bazuriCamera.Priority = highPriority;

        float elapsed = 0f;
        playerInput.SwitchCurrentActionMap("Bazuri");
         ResetCamera();
        StartSlowTimeScaleDown(Time.timeScale, 0, timeScaleDownSpeed);
     //   StartCoroutine(SlowTimeScaleDown(Time.timeScale, 0, timeScaleDownSpeed));
        StartCoroutine(zoomCamera.SetZoom(true));

         shotTaken = false; //�V���b�g���B��ꂽ���ǂ����̃t���O


        while (elapsed < cameraTime)//���쎞�Ԓ��ɃV���b�g�{�^�����������΃o�Y���V���b�g���f
        {
            if (playerInput.actions["Shot"].WasPressedThisFrame())
            {
                SE.PlayOneShot(shutter);
                cameraFlash.StartFlash();

                if(requestManager.requestBazuriShotIsReceipt&&!requestManager.isIntercepting)
                {
                    requestManager.CheckBazuriShot(analyzer.Analyzer(analyzerCamera, layers));
                }

                talkPlayer.PlayTalk("BazuriShot_After");
                shotTaken = true;
                if (fleezeCoroutine != null)
                {
                    StopCoroutine(fleezeCoroutine);
                }
                fleezeCoroutine = StartCoroutine(FleezeScreen());

                
              
               int score=(analyzer.Analyzer(analyzerCamera, layers));
                if(countCoroutine != null)
                {
                    StopCoroutine(countCoroutine);
                }
                countCoroutine =StartCoroutine(CountGood(score));
                
                goodSystem.AddGood(analyzer.Analyzer(analyzerCamera, layers));

              

                break;
            }

            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }

        if (shotTaken)
        {
            if (fleezeCoroutine != null)
                yield return fleezeCoroutine;   // �����ŏI���܂ő҂�

            if (countCoroutine != null)
                yield return countCoroutine;

        }
    
        StartCoroutine(zoomCamera.SetZoom(false));
        EndBazuriMode();
    }
    public IEnumerator CountGood(int targetScore)
    {
        // ���łɓ��� or �����Ă��瑦�I��
        if (targetScore <= 0)
        {
            bazuriText.text = targetScore.ToString();
            countCoroutine = null;
            yield break;
        }

        bazuriText.text = "0";

        float duration = countSpeed;          // �����́u���b�����ăJ�E���g���邩�v�̕b���ɂ���z��
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;

            float t = Mathf.Clamp01(elapsed / duration);
            int count = Mathf.RoundToInt(Mathf.Lerp(0, targetScore, t));

            bazuriText.text = count.ToString();
            yield return null;
        }
        
        SE.PlayOneShot(shutterAfter);

        bazuriText.text = targetScore.ToString();
        //  yield return new WaitForSeconds(countAfterTime);
        // �ŏI�I�ɕK���҂����� targetScore �ŏI��点��
        yield return new WaitForSecondsRealtime(countAfterTime); //�J�E���g��̑ҋ@����

        bazuriText.text = "";
        //  bazuriText.text = ("");
        countCoroutine = null;
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
        currentStock--;
        
        playerInput.SwitchCurrentActionMap("Player");

        if(slowTimeCoroutine != null)
        {
            StopCoroutine(slowTimeCoroutine);
            slowTimeCoroutine = null;
        }
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
    public void SetBazuriShotStock(int rank)
    {
        shotStock = rank;
        currentStock = shotStock;
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
