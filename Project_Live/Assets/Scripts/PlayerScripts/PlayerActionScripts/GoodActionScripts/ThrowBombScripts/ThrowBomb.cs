using UnityEngine;

public class ThrowBomb : MonoBehaviour
{
    [Header("��������I�u�W�F�N�g")]
    [SerializeField] GameObject bombObject;
    [Header("������N�_")]
    [SerializeField] Transform throwPoint;
    [Header("������p�x�i0 = �O�������A90 = �^��")]
    [Range(0f, 90f)]
    [SerializeField] float throwAngle = 45f;
    [Header("�����鋭���i�����̑傫���j")]
    [SerializeField] float throwForce = 10f;

    public void Throw() //���e�̓�������
    {
        GameObject bomb = Instantiate(bombObject, throwPoint.position, Quaternion.identity);
        Rigidbody rb = bomb.GetComponent<Rigidbody>();

        Vector3 dir = Quaternion.AngleAxis(-throwAngle, transform.right) * transform.forward;

        rb.linearVelocity = dir.normalized * throwForce;
    }
}
