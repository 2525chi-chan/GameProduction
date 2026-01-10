using TMPro;
using UnityEngine;
using UnityEngine.UI;


public  enum Rank
{
    Normal,
    Puchi,
    Buzz,
    Buzz_Great,
    Buzz_Ultla
}
public class BazuriShotResult : MonoBehaviour
{

    [SerializeField] RawImage bestImage;
    [SerializeField] TMP_Text bestScoreText;
    [SerializeField] TMP_Text[] rankText;
    [SerializeField] TMP_Text goodText;
    void Start()
    {
        if (rankText != null)
        {
            foreach (var rank in rankText)
            {

                rank.enabled = false;
            }

        }
      
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

        if (rankText != null&&rankText.Length!=0) {  rankText[holder.rank].enabled = true; }

      
        goodText.text=holder.good.ToString()+"\n‚¢‚¢‚ËŠl“¾!!!";
      
    }
}
