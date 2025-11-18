using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.SceneView;
//作成者:福島


public class BazuriShot : MonoBehaviour// バズリショットモードの切り替えの管理
{
    [Header("メインのカメラ")]
    [SerializeField]CinemachineBrain cinemachineBrain;
    [SerializeField] PlayerInput playerInput;
    [Header("プレイヤーが操作するカメラ")]
    [SerializeField] CinemachineFreeLook mainCamera;
    [Header("バズリショットの際に操作するカメラ")]
    [SerializeField] CinemachineVirtualCamera bazuriCamera;

    const int highPriority = 10; //表示させる方
   const int lowPriority = 0; //表示させない方  
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

    [Header("カメラの操作時間")]
    [SerializeField] float cameraTime;
    [Header("スロー時のゲーム速度(1未満じゃないとスローにならない)")]
    [SerializeField] float slowSpeed;
    [Header("バズリショットの最大ストック")]
    [SerializeField] int shotStock;
    [Header("デフォルトのレイヤー(カメラ判定に用いるレイヤー)")]
    [SerializeField] LayerMask layer;
    [Header("時間が遅くなる速度")]
    [SerializeField]float timeScaleDownSpeed;
    [Header("必要なコンポーネント")]
    [SerializeField] BazuriCameraMove cameraMove;
    [SerializeField] BazuriShotAnalyzer analyzer;
    [SerializeField] GoodSystem goodSystem;
    [SerializeField]CameraFlash cameraFlash;
    [SerializeField] ZoomCamera zoomCamera;
    [SerializeField] Transform player;
    [SerializeField] GameObject effect;
    private Camera analyzerCamera;
    private Coroutine bazuriCoroutine;  
    private Coroutine slowTimeCoroutine = null;
    private int currentStock;
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
     
    }

    public void TryBazuriShot()
    {

        if (bazuriCoroutine != null)
        {
            StopCoroutine(bazuriCoroutine);
        }
        if (currentStock > 0)
        {
            bazuriCoroutine = StartCoroutine(BazuriModeRoutine());
        }
        

    }
    private IEnumerator SlowTimeScaleDown(float start,float target,float speed)
    {
        float t = 0;
        float duration = 1.0f / speed;
        while (t < duration)
        {
            if (isBazuriMode)//途中でバズリショットモードが解除されたら強制終了
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
    public void StartSlowTimeScaleDown(float start,float target,float speed)//時間を遅くする処理。単一のコルーチンでしか動かないようにする
    {

        if(slowTimeCoroutine != null)
        {
            StopCoroutine(slowTimeCoroutine);
            slowTimeCoroutine = null;
        }
        slowTimeCoroutine = StartCoroutine(SlowTimeScaleDown(start,target, speed));

    }
    private IEnumerator BazuriModeRoutine()//バズリショットモードに切り替え
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




        while (elapsed < cameraTime)//操作時間中にショットボタンが押されればバズリショット中断
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
       
        StartCoroutine(zoomCamera.SetZoom(false));
        EndBazuriMode();
    }

    private void EndBazuriMode()//プレイヤーモードに切り替え
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
    private void ResetCamera()//カメラ位置、回転の初期化
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
