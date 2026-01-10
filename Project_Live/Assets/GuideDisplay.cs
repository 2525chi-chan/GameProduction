using UnityEngine;

public class GuideDisplay : MonoBehaviour
{

   GuideManager guideManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        guideManager = GetComponent<GuideManager>();
    }

   

    public void GuideEnd()
    {
        if (guideManager != null && guideManager.CurrentIndex == guideManager.GuideImages.Count - 1)
        {
            SelectScene.LoadScene(SelectScene.SceneName.Main);
        }
    }
}
