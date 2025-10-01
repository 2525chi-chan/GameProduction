using Cinemachine;
using System.Collections;
using System.Collections.Generic;
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
    [Header("バズリショットのストック")]
    [SerializeField] int shotStock;
    [Header("デフォルトのレイヤー(カメラ判定に用いるレイヤー)")]
    [SerializeField] LayerMask layer;
    [Header("ボタン押下からモード移行のインターバル")]
    [SerializeField]float shotIntarval;
    [Header("必要なコンポーネント")]
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

    private IEnumerator BazuriModeRoutine()//バズリショットモードに切り替え
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
        shotStock--;
        
        playerInput.SwitchCurrentActionMap("Player");
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
