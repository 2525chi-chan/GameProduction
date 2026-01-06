using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Assertions.Must;
public class StageGradientManager : MonoBehaviour
{
    [SerializeField] GameObject rootObject;
    [GradientUsage(true)]
    [SerializeField] List<Gradient> stageGradient;

    [SerializeField]BuzuriRank buzuriRank;

    [SerializeField]float gradientSpeed=1f;
    List<Renderer> targetRenderers = new List<Renderer>();
    float time=0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        targetRenderers=rootObject.GetComponentsInChildren<Renderer>(true).ToList<Renderer>();


    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        float t01 = Mathf.Repeat(time / gradientSpeed, 1f);        // 0〜1をループ
        float tri = 1f - Mathf.Abs(1f - 2f * t01);            // 0→1→0→…をループ

        var grad = stageGradient[buzuriRank.CurrentIndex];
        Color c = grad.Evaluate(tri);

        foreach (var rend in targetRenderers)
        {
            rend.material.SetColor("_EmissionColor", c);
        }
    }
}
