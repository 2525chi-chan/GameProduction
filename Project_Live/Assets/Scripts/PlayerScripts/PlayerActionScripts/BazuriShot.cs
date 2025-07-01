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
    [Header("�f�t�H���g�̃��C���[(�J��������ɗp���郌�C���[)")]
    [SerializeField] LayerMask layer;
    [Header("�K�v�ȃR���|�[�l���g")]
    [SerializeField] BazuriCameraMove cameraMove;
    [SerializeField] BazuriShotAnalyzer analyzer;

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
        bazuriCoroutine = StartCoroutine(BazuriModeRoutine());

    }

    private IEnumerator BazuriModeRoutine()//�o�Y���V���b�g���[�h�ɐ؂�ւ�
    {
        isBazuriMode = true;
        mainCamera.SetActive(false);
        bazuriCamera.SetActive(true);

        float elapsed = 0f;

        playerInput.SwitchCurrentActionMap("Bazuri");
        ResetCamera();

        Time.timeScale = slowSpeed;

        while (elapsed < cameraTime)//���쎞�Ԓ��ɃV���b�g�{�^�����������΃o�Y���V���b�g���f
        {
            if (playerInput.actions["Shot"].WasPressedThisFrame())
            {
                analyzer.Analyzer(analyzerCamera, layer);
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

        if (mainCamera != null) mainCamera.SetActive(true);
        if (bazuriCamera != null) bazuriCamera.SetActive(false);

        playerInput.SwitchCurrentActionMap("Player");
        ResetCamera();
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
}
