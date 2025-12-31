using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
//using UnityEngine.Rendering.PostProcessing;
using UnityEngine.Rendering.Universal;

public class BazuriShotEffector : MonoBehaviour
{

    [SerializeField]TMP_Text bazuriStartText;
    [SerializeField]TMP_Text bazuriGuideText;
    [SerializeField]float characterSpacingMax;  
    [SerializeField]float blinkInterval;
    [SerializeField]float blinkTime;
    [SerializeField] float blinkEndWaitTime=0.5f;
    [SerializeField] Volume volume;
    [SerializeField] float effectPower_Chromatic;//エフェクトの強さ
    [SerializeField] float effectPower_Vignette;
    [SerializeField] float effectTime;//遷移する速度

    private ChromaticAberration chromatic;
    private Vignette vignette;

    public void Start()
    {
        bazuriStartText.enabled = false;
        bazuriGuideText.enabled = false;
    }
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
    public  IEnumerator BlinkText()
    {
        var count = 0f;
        bazuriStartText.characterSpacing = 0;
        while (count< blinkTime)
        {
            count += Time.unscaledDeltaTime;

            var t = count / blinkTime;  
            yield return  new WaitForSecondsRealtime(blinkInterval); 
            bazuriStartText.enabled=!bazuriStartText.enabled;

            bazuriStartText.characterSpacing = Mathf.Lerp(0, characterSpacingMax, t);
        }
        bazuriStartText.enabled = true;
        yield return new WaitForSecondsRealtime(blinkEndWaitTime);

        bazuriStartText.characterSpacing = 0;
        bazuriStartText.enabled = false;
        bazuriGuideText.enabled = true;
    }

    public void ResetEffect()
    {
        chromatic.intensity.value = 0;
        vignette.intensity.value = 0;
    }
    public void ResetText()
    {
               bazuriStartText.characterSpacing = 0;
        bazuriStartText.enabled = false;
        bazuriGuideText.enabled = false;
    }
}
