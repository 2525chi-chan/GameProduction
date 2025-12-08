using Unity.VisualScripting;
using UnityEngine;

public class AttackWarningController : MonoBehaviour
{
    [Header("攻撃範囲予告の表示時間")]
    [SerializeField] float warningDuration = 1.0f;
    [Header("予告用プレハブ")]
    [SerializeField] GameObject warningPrefab;
    [Header("予告を表示する位置")]
    [Tooltip("設定されていない場合は、攻撃判定の生成位置をそのまま適用する")]
    [SerializeField] Transform showPosition;

    bool isWarningActive = false;
    bool isWarningFinished = false;
    GameObject currentWarningArea;
    float warningTimer = 0f;

    public bool IsWarningActive { get { return isWarningActive; } }
    public bool IsWarningFinished { get { return isWarningFinished; } set { isWarningFinished = value; } }
    void Update()
    {
        if (isWarningActive)
        {
            warningTimer += Time.deltaTime;
            if (warningTimer >= warningDuration)
                EndWarning();
        }
    }
    public void ShowAttackWarning(Transform attackPos)
    {
        if (attackPos == null || isWarningActive) return;

        if (showPosition != null) attackPos = showPosition;
        StartWarning(attackPos);
    }

    void StartWarning(Transform attackPos)
    {
        currentWarningArea = Instantiate(warningPrefab, attackPos.position, attackPos.rotation);

        isWarningActive = true;
        isWarningFinished = false;
        warningTimer = 0f;
    }

    void EndWarning()
    {
        if (currentWarningArea != null)
        {
            Destroy(currentWarningArea);
            currentWarningArea = null;
        }

        isWarningActive = false;
        isWarningFinished = true;
        warningTimer = 0f;
    }
}
