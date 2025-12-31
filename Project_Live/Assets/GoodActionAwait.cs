using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using TMPro;
public class GoodActionAwait : MonoBehaviour
{
    [SerializeField] List<TMP_Text> actionTexts=new();
   
    public void OnRelease(InputAction.CallbackContext context)
    {

        if (!context.performed) return;

        foreach(var text in actionTexts)
        {
            text.enabled=false;

        }
        Debug.Log("aaasssd");
    }

    public void OnPress(InputAction.CallbackContext context)
    {
       if(!context.performed) return;

        foreach(var text in actionTexts)
        {
            text.enabled=true;
        }
        Debug.Log("pppppp");
    }
}
