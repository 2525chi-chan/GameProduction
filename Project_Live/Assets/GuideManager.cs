using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
public class GuideManager : MonoBehaviour
{

    [SerializeField] GameObject rootGuide;
    [SerializeField] List<Image> guideImages = new();
    [SerializeField] InputAction action;
    int currentIndex = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SetGuideIndex(0);
    }



    public void GuideStart()
    {

        rootGuide.SetActive(true);
        SetGuideIndex(0);
    }
    public void GuideReset()
    {

        if (rootGuide.activeSelf)
            rootGuide.SetActive(false);
    }

    public void NextGuide(InputAction.CallbackContext context)
    {
        if (guideImages.Count == 0) return;
        if (!context.performed) return;
        currentIndex++;
        if (currentIndex >= guideImages.Count)
            currentIndex = 0; // 末尾で止める（ループさせたいなら 0）

        SetGuideIndex(currentIndex);
    }

    public void PrevGuide(InputAction.CallbackContext context)
    {
        if (guideImages.Count == 0) return;
        if (!context.performed) return;
        currentIndex--;
        if (currentIndex < 0)
            currentIndex = guideImages.Count - 1; // 先頭で止める（ループさせたいなら guideImages.Count-1）

        SetGuideIndex(currentIndex);
    }

    void SetGuideIndex(int index)
    {
        currentIndex = Mathf.Clamp(index, 0, guideImages.Count - 1);

        for (int i = 0; i < guideImages.Count; i++)
        {
            bool active = (i == currentIndex);
            guideImages[i].gameObject.SetActive(active);
        }
    }
    }
