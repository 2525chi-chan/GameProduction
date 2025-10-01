using UnityEngine;

public class RushAttack : MonoBehaviour
{
    [Header("�A���U�����Ɏg�p����U������v���n�u")]
    [SerializeField] GameObject rushPrefab;
    [Header("�Ō�̍U���Ɏg�p����U������v���n�u")]
    [SerializeField] GameObject finishPrefab;
    [Header("�A���U���̌p������")]
    [SerializeField] float rushActiveTime = 2f;
    [Header("�A���U���̔����Ԋu")]
    [SerializeField] float rushInterval = 0.1f;
    [Header("�A���U���I����A�Ō�̈ꌂ���J��o���܂ł̎���")]
    [SerializeField] float timeToFinish = 0.5f;
    [Header("�Ō�̍U�����肪�����ɂȂ�܂ł̎���")]
    [SerializeField] float finishDisableTime = 0.5f;

    float currentActiveTime = 0f;
    float intervalTime = 0f;

    float currentTimeToFinish = 0f;

    float currentTimeToDisableFinish = 0f;

    bool isActivated = false;

    void Start()
    {
        rushPrefab.SetActive(false);
        finishPrefab.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isActivated) return;

        currentActiveTime += Time.deltaTime;
        intervalTime += Time.deltaTime;

        if (intervalTime > rushInterval)
        {
            Debug.Log("���b�V���J�n");
            if (!rushPrefab.activeSelf)rushPrefab.SetActive(true);
            else rushPrefab.SetActive(false);

            intervalTime = 0f;
        }

        if (rushActiveTime <= currentActiveTime)
        {
            if (!finishPrefab.activeSelf) currentTimeToFinish += Time.deltaTime;

            if (currentTimeToFinish > timeToFinish)
            {
                Debug.Log("�t�B�j�b�V���J�n");
                finishPrefab.SetActive(true);
                currentTimeToFinish = 0f;
            }

            if (finishPrefab.activeSelf) currentTimeToDisableFinish += Time.deltaTime;

            if (currentTimeToDisableFinish > finishDisableTime)
            {
                finishPrefab.SetActive(false);
                currentTimeToDisableFinish = 0f;
                SetIsDeactivate();
            }
        }
    }

    public void SetIsActivate() //���b�V���U�������̊J�n�t���O�𗧂Ă�
    {
        isActivated = true;
    }

    public void SetIsDeactivate()
    {
        rushPrefab.SetActive(false);
        finishPrefab.SetActive(false);
        currentActiveTime = 0f;
        intervalTime = 0f;
        currentTimeToFinish = 0f;
        currentTimeToDisableFinish = 0f;
        isActivated = false;
    }
}
