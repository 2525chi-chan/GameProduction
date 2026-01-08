using UnityEngine;
using System.Collections;

public class Meteor : MonoBehaviour
{
    GameObject linkedWarning;

    public void SetLinkedWarning(GameObject warning)
    {
        linkedWarning = warning;
    }

    public void DestroyLinkedWarning()
    {
        // 予告即時削除
        if (linkedWarning != null)
        {
            Destroy(linkedWarning);
            linkedWarning = null;
        }

        // 効果演出後自己削除
        StartCoroutine(SelfDestruct());
    }

    IEnumerator SelfDestruct()
    {
        yield return new WaitForSeconds(2f);  // 着弾エフェクト時間調整
        Destroy(gameObject);
    }

    // OnDestroy削除（Kill()で統一制御）
}
