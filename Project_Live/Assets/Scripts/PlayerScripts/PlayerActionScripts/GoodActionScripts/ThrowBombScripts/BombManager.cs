using UnityEngine;

public class BombManager : MonoBehaviour
{
    [Header("爆発の判定")]
    [SerializeField] GameObject explosionPrefab;
    [Header("投擲されてから爆発するまでの時間")]
    [SerializeField] float explosionDelay = 2f;
    [Header("爆発のエフェクト")]
    [SerializeField] GameObject effect;

    float explosionTimer = 0f;

    void Start()
    {
        explosionTimer = 0f;
    }

    void Update()
    {
        explosionTimer += Time.deltaTime;

        if (explosionTimer > explosionDelay) Explode();
    }

    void Explode() //爆発処理
    {
        if (explosionPrefab != null) Instantiate(explosionPrefab, transform.position, Quaternion.identity);

        if (effect != null) Instantiate(this.effect, transform.position, Quaternion.identity);

        Destroy(gameObject); //爆弾本体の削除
    }
}
