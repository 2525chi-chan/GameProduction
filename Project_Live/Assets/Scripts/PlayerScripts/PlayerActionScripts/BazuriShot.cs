using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static UnityEditor.SceneView;



public class BazuriShot : MonoBehaviour// バズリショットの制御
{
    [Header("必要なコンポーネント")]
    [SerializeField]CinemachineBrain cinemachineBrain;
    [SerializeField] PlayerInput playerInput;
    [Header("メインカメラ")]
    [SerializeField] CinemachineFreeLook mainCamera;
    [Header("バズリショットに活用するカメラ")]
    [SerializeField] CinemachineVirtualCamera bazuriCamera;
    [Header("ズームアウト用カメラ(演出用)")]
    [SerializeField]CinemachineVirtualCamera zoomOutCamera;

    const int highPriority = 10; //使用中カメラ
   const int lowPriority = 0; //非使用のカメラ  
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

    [Header("バズリショット時間")]
    [SerializeField] float cameraTime;
    [Header("時間中のタイムスケール(1未満にしてください)")]
    [SerializeField] float slowSpeed;
    [Header("バズリショットの回数")]
    [SerializeField] int shotStock;
    [Header("使用後のクールタイム")]
    [SerializeField] float coolTime;
    public float CoolTime
    {
        get { return coolTime; }
    }
    [Header("判定に用いるオブジェクトのレイヤー")]
    [SerializeField] List<LayerMask> layers;
    [Header("スローになる時間")]
    [SerializeField]float timeScaleDownSpeed;
    [Header("シャッター音")]
    [SerializeField] AudioClip shutter;
    [Header("シャッター後音声")]
    [SerializeField] AudioClip shutterAfter;
    [Header("残り時間UI")]
    [SerializeField] Image countTimeUI;
    [Header("画像グラデーション")]
    [SerializeField] Gradient UIGradient;
    [Header("結果出力用のコンポーネント")]
    [SerializeField] RenderTexture bazuriTexture;
    [SerializeField] RawImage rawImage;
    [SerializeField]TMP_Text bazuriText;
    [Header("撮影後のフリーズ時間")]
    [SerializeField] float fleezeTime = 0.2f;
    [Header("得点カウント速度")]
    [SerializeField] float countSpeed=5;
    [Header("得点カウント後の待機時間")]
    [SerializeField]float countAfterTime = 0.5f; //�J�E���g��̑ҋ@����   

    [Header("ズームカメラの有効時間")]
    [SerializeField] float zoomStartDuration = 0.7f;
    [SerializeField]float zoomEndDuration = 0.5f;



    [Header("必要なコンポーネント")]
    [SerializeField] BazuriCameraMove cameraMove;
    [SerializeField]CameraBillboard cameraBillboard;
    [SerializeField] BazuriShotAnalyzer analyzer;
    [SerializeField]RequestManager requestManager;
    [SerializeField]Live2DTalkPlayer talkPlayer;
    [SerializeField] AudioSource SE;
    [SerializeField] GoodSystem goodSystem;
    [SerializeField]CameraFlash cameraFlash;
    [SerializeField] ZoomCamera zoomCamera;
    [SerializeField] BazuriShotEffector effector;
    [SerializeField] Transform player;
    [SerializeField] GameObject effect;

    private float countCoolTime;
    public float CountCoolTime
    {
        get { return countCoolTime; }
       // set { countCoolTime = value; }
    }
    private Camera analyzerCamera;
    private Coroutine bazuriCoroutine;  
    private Coroutine slowTimeCoroutine = null;
    private Coroutine fleezeCoroutine=null;
    private Coroutine countCoroutine = null;
    private int currentStock;
    bool shotTaken = false; //正常に撮影が行われたかどうか
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
      cameraBillboard.isEnabled = false;
        isBazuriMode = true;
         BazuriEffect();
        StartCoroutine(effector.BlinkText());
        StartCoroutine(effector.EffectCoroutine());
        StartSlowTimeScaleDown(Time.timeScale, 0, timeScaleDownSpeed);
        
        mainCamera.Priority=lowPriority;
        zoomOutCamera.Priority = highPriority;

        yield return new WaitForSecondsRealtime(zoomStartDuration);

        zoomOutCamera.Priority = lowPriority;
        bazuriCamera.Priority = highPriority;
        ResetCamera();
        
    
        StartCoroutine(zoomCamera.SetZoom(true));
        yield return new WaitForSecondsRealtime(zoomEndDuration);
        float elapsed = 0f;
        playerInput.SwitchCurrentActionMap("Bazuri");
      

         shotTaken = false; //�V���b�g���B��ꂽ���ǂ����̃t���O

        countTimeUI.fillAmount = 1;
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
                effector.ResetText();
              

                break;
            }

           
            countTimeUI.fillAmount =1-( elapsed / cameraTime);
            countTimeUI.color=UIGradient.Evaluate(elapsed/cameraTime);
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
        cameraBillboard.isEnabled = true;
        effector.ResetText();
        effector.ResetEffect();
        isBazuriMode = false;
        Time.timeScale = 1f;
       
        if (mainCamera != null && bazuriCamera != null)
        {
            mainCamera.Priority = highPriority;
            bazuriCamera.Priority = lowPriority;
        }
        currentStock--;
        countTimeUI.fillAmount = 1;
        countTimeUI.color = Color.white;
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
