using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBullet : MonoBehaviour
{
    [Header("’e‚Ì‘¬“x")]
    [SerializeField] float bulletSpeed;

    void Update()
    {
        transform.localPosition += transform.forward * bulletSpeed * Time.deltaTime;
    }
}
