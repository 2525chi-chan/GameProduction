using UnityEngine;

public class BombManager : MonoBehaviour
{
    [Header("�����̔���")]
    [SerializeField] GameObject explosionPrefab;
    [Header("��������Ă��甚������܂ł̎���")]
    [SerializeField] float explosionDelay = 2f;
    [Header("�����̃G�t�F�N�g")]
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

    void Explode() //��������
    {
        if (explosionPrefab != null) Instantiate(explosionPrefab, transform.position, Quaternion.identity);

        if (effect != null) Instantiate(this.effect, transform.position, Quaternion.identity);

        Destroy(gameObject); //���e�{�̂̍폜
    }
}
