using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.SceneView;
//作成者:福島


public class BazuriShot : MonoBehaviour// バズリショットモードの切り替えの管理
{
    [SerializeField] PlayerInput playerInput;
    [Header("プレイヤー")]
    [SerializeField] Transform player;
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

    [Header("必要なコンポーネント")]
    [SerializeField] BazuriCameraMove cameraMove;

    private Coroutine bazuriCoroutine;
    private bool isBazuriMode = false;
    public bool IsBazuriMode
    {
        get { return isBazuriMode; }
    }
    private void Start()
    {
        if (bazuriCamera != null)
        {
            bazuriCamera.SetActive(false);
        }

    }

    public void TryBazuriShot()
    {

        if (bazuriCoroutine != null)
        {
            StopCoroutine(bazuriCoroutine);
        }
        bazuriCoroutine = StartCoroutine(BazuriModeRoutine());

    }

    private IEnumerator BazuriModeRoutine()//バズリショットモードに切り替え
    {
        isBazuriMode = true;
        mainCamera.SetActive(false);
        bazuriCamera.SetActive(true);

        playerInput.SwitchCurrentActionMap("Bazuri");
        ResetCamera();

        Time.timeScale = slowSpeed;

        yield return new WaitForSecondsRealtime(cameraTime);

        EndBazuriMode();
    }

    private void EndBazuriMode()//プレイヤーモードに切り替え
    {
        isBazuriMode = false;
        Time.timeScale = 1f;

        if (mainCamera != null) mainCamera.SetActive(true);
        if (bazuriCamera != null) bazuriCamera.SetActive(false);

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
}
