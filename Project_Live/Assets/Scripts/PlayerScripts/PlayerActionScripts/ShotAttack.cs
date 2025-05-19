using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�쐬�ҁF�K��

public class ShotAttack : MonoBehaviour
{
    [Header("���˂���e")]
    [SerializeField] GameObject bulletPrefab;
    [Header("���ˈʒu")]
    [SerializeField] Transform shotPos;
    [Header("�e�̑��x")]
    [SerializeField] float bulletSpeed;
    [Header("�Ĕ��˂܂ł̊Ԋu")]
    [SerializeField] float shotInterval = 0.5f;

    float timeSinceLastShot = 0f;

    void Update()
    {
        timeSinceLastShot += Time.deltaTime;
    }

    public void ShotBullet()
    {
        if (timeSinceLastShot < shotInterval) return;

        GameObject bullet = Instantiate(bulletPrefab, shotPos.transform.position, shotPos.transform.rotation);

        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.velocity = shotPos.forward * bulletSpeed;

        timeSinceLastShot = 0f;
    }
}
