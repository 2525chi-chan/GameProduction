using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BazuriShotResult : MonoBehaviour
{

    [SerializeField] RawImage bestImage;
    [SerializeField] TMP_Text bestScoreText;

    void Start()
    {
        var holder = BazuriShotHolder.Instance;

        if (holder != null && holder.BestShotRT != null)
        {
            bestImage.texture = holder.BestShotRT;
            bestImage.enabled = true;

            if (bestScoreText != null)
                bestScoreText.text = holder.BestScore.ToString();
        }
        else
        {
            bestImage.enabled = false;
            if (bestScoreText != null)
                bestScoreText.text = "-";
        }
    }

}
