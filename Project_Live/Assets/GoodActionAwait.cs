using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;


[System.Serializable]
  public class ActionUISet
{
  public TMP_Text text;
    public Image image;
} 
public class GoodActionAwait : MonoBehaviour
{

    
    
    [SerializeField]List<ActionUISet>actionUISets;
  //  [SerializeField] List<TMP_Text> actionTexts=new();
  //  [SerializeField]List<Image> images=new();
    [SerializeField] float pressedScale;
    [SerializeField] float defaultScale=1.05f;
    public void OnRelease(InputAction.CallbackContext context)
    {

        if (!context.performed) return;

        foreach(var ui in actionUISets)
        {
            ui.text.enabled=false;
            ui.image.transform.localScale = new Vector3(defaultScale,defaultScale,defaultScale);

        }
       // Debug.Log("aaasssd");
    }

    public void OnPress(InputAction.CallbackContext context)
    {
       if(!context.performed) return;

        foreach(var ui in actionUISets)
        {
            ui.text.enabled=true;
            ui.image.transform.localScale= new(pressedScale,pressedScale,pressedScale);
        }
       // Debug.Log("pppppp");
    }
}
