using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
public class GuideManager : MonoBehaviour
{

    [SerializeField] GameObject rootGuide;
    [SerializeField] List<Image> guideImages = new();
    public List<Image>GuideImages { get { return guideImages; } }
    [SerializeField] InputAction action;
    [SerializeField]bool isLooped = true;
    [SerializeField] Image rightArrow;
    [SerializeField] Image leftArrow;
    [SerializeField] AudioSource buttonAudio;
    [SerializeField] AudioClip clip;
    [SerializeField] AudioClip cancelClip;
    int currentIndex = 0;

    public int CurrentIndex
    {
        get
        {
            return currentIndex;
        }
        set
        {
            currentIndex = value;
        }
    }
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
        buttonAudio.PlayOneShot(cancelClip);
    }

    public void NextGuide(InputAction.CallbackContext context)
    {
        if (guideImages.Count == 0) return;
        if (!context.performed) return;
        currentIndex++;


        if (currentIndex >= guideImages.Count)
        {
            if (isLooped)
            {
                currentIndex = 0; // 末尾で止める（ループさせたいなら 0）
            }
            else
            { currentIndex=guideImages.Count-1;
               
            }

        }

        rightArrow.enabled=isLooped||currentIndex!=guideImages.Count-1;
        leftArrow.enabled = true;

        SetGuideIndex(currentIndex);
    }

    public void PrevGuide(InputAction.CallbackContext context)
    {
        if (guideImages.Count == 0) return;
        if (!context.performed) return;
        currentIndex--;
        if (currentIndex < 0)
        {
            if (isLooped)
            {

                currentIndex = guideImages.Count - 1; // 先頭で止める（ループさせたいなら guideImages.Count-1）

            }
            else
            {
                currentIndex = 0;

            }

        }

        leftArrow.enabled = isLooped || currentIndex!=0;
        rightArrow.enabled = true;
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

        buttonAudio.PlayOneShot(clip);
    }
    }
