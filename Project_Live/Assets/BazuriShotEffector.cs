using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
//using UnityEngine.Rendering.PostProcessing;
using UnityEngine.Rendering.Universal;

public class BazuriShotEffector : MonoBehaviour
{

    [SerializeField] Volume volume;
    [SerializeField] float effectPower_Chromatic;//エフェクトの強さ
    [SerializeField] float effectPower_Vignette;
    [SerializeField] float effectTime;//遷移する速度

    private ChromaticAberration chromatic;
    private Vignette vignette;
    public IEnumerator EffectCoroutine()
    {
        var profile = volume.profile;


        if (profile.TryGet<ChromaticAberration>(out chromatic)&&profile.TryGet<Vignette>(out vignette))
        {
            var count = 0f;

            while (count < effectTime)
            {
                count += Time.unscaledDeltaTime;


                var t = count / effectTime;

                chromatic.intensity.value=Mathf.Lerp(0,effectPower_Chromatic, t);
                vignette.intensity.value=Mathf.Lerp(0, effectPower_Vignette, t);


                yield return null;

            }



        }


        
    }


    public void ResetEffect()
    {
        chromatic.intensity.value = 0;
        vignette.intensity.value = 0;
    }
}
