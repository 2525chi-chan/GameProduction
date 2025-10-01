using UnityEngine;

public class RushAttack : MonoBehaviour
{
    [Header("連続攻撃時に使用する攻撃判定プレハブ")]
    [SerializeField] GameObject rushPrefab;
    [Header("最後の攻撃に使用する攻撃判定プレハブ")]
    [SerializeField] GameObject finishPrefab;
    [Header("連続攻撃の継続時間")]
    [SerializeField] float rushActiveTime = 2f;
    [Header("連続攻撃の発生間隔")]
    [SerializeField] float rushInterval = 0.1f;
    [Header("連続攻撃終了後、最後の一撃を繰り出すまでの時間")]
    [SerializeField] float timeToFinish = 0.5f;
    [Header("最後の攻撃判定が無効になるまでの時間")]
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
            Debug.Log("ラッシュ開始");
            if (!rushPrefab.activeSelf)rushPrefab.SetActive(true);
            else rushPrefab.SetActive(false);

            intervalTime = 0f;
        }

        if (rushActiveTime <= currentActiveTime)
        {
            if (!finishPrefab.activeSelf) currentTimeToFinish += Time.deltaTime;

            if (currentTimeToFinish > timeToFinish)
            {
                Debug.Log("フィニッシュ開始");
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

    public void SetIsActivate() //ラッシュ攻撃処理の開始フラグを立てる
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
