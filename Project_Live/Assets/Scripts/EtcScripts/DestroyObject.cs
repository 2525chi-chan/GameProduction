using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�쐬�ҁF�K��

public class DestroyObject : MonoBehaviour
{
    [Header("���ł���܂ł̎���")]
    [SerializeField] float destroyDelay = 3f;

    void Start()
    {
        Destroy(this.gameObject, destroyDelay);
    }
}
