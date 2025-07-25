using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.SceneView;
//作成者:福島


public class BazuriShot : MonoBehaviour// バズリショットモードの切り替えの管理
{
    [SerializeField] PlayerInput playerInput;
    [Header("メインのカメラ")]
    [SerializeField] GameObject mainCamera;
    [Header("バズリショットの際に操作するカメラ")]
    [SerializeField] GameObject bazuriCamera;
    public GameObject BazuriCamera
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
    [Header("必要なコンポーネント")]
    [SerializeField] BazuriCameraMove cameraMove;
    [SerializeField] BazuriShotAnalyzer analyzer;
    [SerializeField] GoodSystem goodSystem;
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
        if (bazuriCamera != null)
        {
            bazuriCamera.SetActive(false);
            analyzerCamera=bazuriCamera.GetComponent<Camera>();
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
        isBazuriMode = true;
        mainCamera.SetActive(false);
        bazuriCamera.SetActive(true);

        float elapsed = 0f;

        playerInput.SwitchCurrentActionMap("Bazuri");
        ResetCamera();

        Time.timeScale = slowSpeed;

        while (elapsed < cameraTime)//操作時間中にショットボタンが押されればバズリショット中断
        {
            if (playerInput.actions["Shot"].WasPressedThisFrame())
            {
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

        if (mainCamera != null) mainCamera.SetActive(true);
        if (bazuriCamera != null) bazuriCamera.SetActive(false);

        shotStock--;
        
        playerInput.SwitchCurrentActionMap("Player");
        ResetCamera();
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
}
