using System.Threading;
using UnityEngine;

public class BarrierEffector : MonoBehaviour
{

    [SerializeField] Material effectMaterial;
    [SerializeField] Gradient damageGradient_Base;
    [GradientUsage(true)]
    [SerializeField] Gradient damageGradient_Edge;
    [SerializeField] ObjectStatus status;
    [SerializeField] Renderer barrier;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
         barrier.material = new Material(effectMaterial);
    }

    // Update is called once per frame
    void Update()
    {
        var ratio = 1.0f - (float)status.Hp / status.MaxHp;
        var hp = (float)status.Hp / status.MaxHp;

        barrier.material.SetColor("_BaseColor", damageGradient_Base.Evaluate(ratio));
        barrier.material.SetColor("_FresnelColor", damageGradient_Edge.Evaluate(ratio));
        barrier.material.SetFloat("_BreakablePower", Mathf.Lerp(-1, 0, hp));
    }
}
