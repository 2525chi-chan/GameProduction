using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MultiAttackWarningController : MonoBehaviour
{
    [Header("複数攻撃予告プレハブ")]
    [SerializeField] GameObject multiWarningPrefab;
    [Header("予告表示時間")]
    [SerializeField] float multiWarningDuration = 1.0f;
    [Header("予告オフセット（y軸）")]
    [SerializeField] float warningOffsetY = 0.1f;

    List<GameObject> activeWarnings = new List<GameObject>();

    public bool IsWarningActive => activeWarnings.Count > 0;

    public bool IsWarningFinished { get; set; }


    public List<GameObject> CreateMultiWarnings(List<Vector3> positions)
    {
        // DestroyWarningAreas()削除で既存残存
        List<GameObject> newWarnings = new List<GameObject>();

        foreach (Vector3 pos in positions)
        {
            Vector3 warningPos = new Vector3(pos.x, warningOffsetY, pos.z);
            GameObject warning = Instantiate(multiWarningPrefab, warningPos, Quaternion.identity);
            newWarnings.Add(warning);
            activeWarnings.Add(warning);  // 管理追加（任意）
        }

        IsWarningFinished = false;
        StartCoroutine(WarningTimer(newWarnings));  // 独立タイマー開始
        return newWarnings;
    }

    IEnumerator WarningTimer(List<GameObject> warnings)
    {
        yield return new WaitForSeconds(multiWarningDuration);
        IsWarningFinished = true;  // 状態更新のみ、破壊せず
    }

    public void DestroyWarning(GameObject warning)  // Meteor用追加
    {
        if (warning != null) Destroy(warning);
        activeWarnings.Remove(warning);
    }
}
