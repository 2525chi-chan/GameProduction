using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ButtonActiveManager : MonoBehaviour
{
    public List<Image> buttonImages = new List<Image>();

    Button activeButton;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        activeButton= GetComponent<Button>();
    }

    // Update is called once per frame
    void Update()
    {
        bool isActive = activeButton.gameObject == UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        foreach (var image in buttonImages) {

            image.enabled = isActive;
            

        }
        activeButton.interactable = isActive;
    }
}
