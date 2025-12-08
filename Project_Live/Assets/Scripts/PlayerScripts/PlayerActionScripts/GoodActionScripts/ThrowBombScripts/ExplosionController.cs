using UnityEngine;

public class ExplosionController : MonoBehaviour
{
    [Header("最大スケール（直径）")]
    [SerializeField] float maxScale = 5f;
    [Header("広がる速度")]
    [SerializeField] float expandSpeed = 10f;
    [Header("広がりきった後、消滅するか")]
    [SerializeField] bool destroyOnMaxScale = true;

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

        else
        {
            if (!destroyOnMaxScale) return;
            else
            {
                Debug.Log("消滅");
                Destroy(gameObject);
            }
        }
    }
}
