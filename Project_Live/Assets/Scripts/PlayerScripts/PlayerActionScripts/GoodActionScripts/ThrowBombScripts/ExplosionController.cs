using UnityEngine;

public class ExplosionController : MonoBehaviour
{
    [Header("�ő�X�P�[���i���a�j")]
    [SerializeField] float maxScale = 5f;
    [Header("�L���鑬�x")]
    [SerializeField] float expandSpeed = 10f;

    Vector3 initialScale;

    void Start()
    {
        initialScale = transform.localScale;
        transform.localScale = Vector3.zero;
    }

    void Update()
    {
        float current = transform.localScale.x;

        if (current < maxScale)
        {
            float next = current + expandSpeed * Time.deltaTime;

            next = Mathf.Min(next, maxScale);

            transform.localScale = new Vector3(next, next, next);
        }

        else Destroy(gameObject);
    }
}
