using UnityEngine;

public class ThrowBomb : MonoBehaviour
{
    [Header("投擲するオブジェクト")]
    [SerializeField] GameObject bombObject;
    [Header("投げる起点")]
    [SerializeField] Transform throwPoint;
    [Header("投げる角度（0 = 前方水平、90 = 真上")]
    [Range(0f, 90f)]
    [SerializeField] float throwAngle = 45f;
    [Header("投げる強さ（初速の大きさ）")]
    [SerializeField] float throwForce = 10f;

    public void Throw() //爆弾の投擲処理
    {
        GameObject bomb = Instantiate(bombObject, throwPoint.position, Quaternion.identity);
        Rigidbody rb = bomb.GetComponent<Rigidbody>();

        Vector3 dir = Quaternion.AngleAxis(-throwAngle, transform.right) * transform.forward;

        rb.linearVelocity = dir.normalized * throwForce;
    }
}
