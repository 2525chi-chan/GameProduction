using UnityEngine;
using System.Collections;

public class Meteor : MonoBehaviour
{
    [Header("地面着弾時に出力する音")]
    [SerializeField] AudioClip impactSound;
    GameObject linkedWarning;
    AudioSource SE;

    void Start()
    {
        SE = GameObject.FindWithTag("SE").GetComponent<AudioSource>();
    }
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

        if (SE != null && impactSound != null) SE.PlayOneShot(impactSound);

        // 効果演出後自己削除
        StartCoroutine(SelfDestruct());
    }

    IEnumerator SelfDestruct()
    {
        yield return new WaitForSeconds(2f);  // 着弾エフェクト時間調整
        Destroy(gameObject);
    }
}
