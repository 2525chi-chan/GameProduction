using Live2D.Cubism.Core;
using UnityEngine;

[RequireComponent(typeof(CubismModel))]
public class LipSync : MonoBehaviour
{
    [SerializeField] AudioSource voiceSource; // ボイス用AudioSource
    [SerializeField] string mouthParamId = "ParamMouthOpenY";
    [SerializeField] string mouthFormParamId="ParamMouthForm";
    [SerializeField] float gain = 20f;       // 反応の強さ
    [SerializeField] int sampleSize = 256;   // 解析サンプル数

    CubismParameter mouthParam;
    CubismParameter mouthForm;
    float[] samples;


    void Start()
    {
        var model = GetComponent<CubismModel>();
        mouthParam = model.Parameters.FindById(mouthParamId);

        mouthForm=model.Parameters.FindById(mouthFormParamId);
        if (mouthParam == null)
        {
            Debug.LogError($"Mouth parameter '{mouthParamId}' が見つかりません。");
        }

        samples = new float[sampleSize];
    }

    void LateUpdate() // モーションより後で上書き
    {
        if (mouthParam == null || voiceSource == null)
            return;

        if (!voiceSource.isPlaying)
        {
           // mouthParam.Value = 0f;
           // return;
        }

        voiceSource.GetOutputData(samples, 0);

        float sum = 0f;
        for (int i = 0; i < samples.Length; i++)
            sum += Mathf.Abs(samples[i]);

        float volume = sum / sampleSize;              // 0〜小さい値
        float v = Mathf.Clamp01(volume * gain);       // ゲインで持ち上げて 0〜1 に

        mouthParam.Value = v;
        mouthForm.Value = 1;
    }
}
