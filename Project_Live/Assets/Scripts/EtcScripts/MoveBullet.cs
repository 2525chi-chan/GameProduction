using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBullet : MonoBehaviour
{
    [Header("’e‚Ì‘¬“x")]
    [SerializeField] float bulletSpeed;

    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        rb.velocity = transform.forward * bulletSpeed * Time.deltaTime;
    }
}
