using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class DestroyTrigger : MonoBehaviour
{
    [Header("�G���Ə��ł���G���A")]
    [SerializeField] MeshCollider area;
    [Header("�폜�ΏۂƂ���^�O")]
    [SerializeField] List<string> targetTags = new List<string>();

    void OnTriggerEnter(Collider other)
    {
        if (!targetTags.Contains(other.tag)) return;

        Destroy(other.gameObject);
        //Debug.Log(other.name + "�͏��ł���");
    }
}
