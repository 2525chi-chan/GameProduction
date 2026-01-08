using UnityEngine;

public class BazuriShotHolder : MonoBehaviour
{
   public static BazuriShotHolder Instance{  get; private set; }


    public RenderTexture BestShotRT;
    public int BestScore;


    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }


    public void TryBestBazuriShot(RenderTexture rt, int score)
    {
        if (score < BestScore) return;

        BestScore = score;

     
      
        if (BestShotRT == null ||
         BestShotRT.width != rt.width ||
         BestShotRT.height != rt.height)
        {
            if (BestShotRT != null) BestShotRT.Release();
            BestShotRT = new RenderTexture(rt.width, rt.height, 0, rt.format);
        }

        Graphics.Blit(rt, BestShotRT); // 中身だけコピー

    }

  
}
