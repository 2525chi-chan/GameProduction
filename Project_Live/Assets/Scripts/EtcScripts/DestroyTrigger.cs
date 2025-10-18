using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class DestroyTrigger : MonoBehaviour
{
    [Header("触れると消滅するエリア")]
    [SerializeField] MeshCollider area;
    [Header("削除対象とするタグ")]
    [SerializeField] List<string> targetTags = new List<string>();

    void OnTriggerEnter(Collider other)
    {
        if (!targetTags.Contains(other.tag)) return;

        Destroy(other.gameObject);
        //Debug.Log(other.name + "は消滅した");
    }
}
