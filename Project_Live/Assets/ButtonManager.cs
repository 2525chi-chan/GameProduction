using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
public class ButtonManager : MonoBehaviour
{
    [SerializeField]List<Button>buttons = new List<Button>();



    bool isSelected;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        foreach (var button in buttons) { 
        
        if(button.gameObject== UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject)
            {
                break;
            }

      
        }
        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(buttons[0].gameObject);
    }
}
