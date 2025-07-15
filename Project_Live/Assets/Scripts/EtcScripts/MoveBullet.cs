using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBullet : MonoBehaviour
{
    [Header("�e�̑��x")]
    [SerializeField] float bulletSpeed;

    void Update()
    {
        transform.localPosition += transform.forward * bulletSpeed * Time.deltaTime;
    }
}
