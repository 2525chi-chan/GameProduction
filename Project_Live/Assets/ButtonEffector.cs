using UnityEngine;
using UnityEngine.UI;
public class ButtonEffector : MonoBehaviour
{

    Animator animator;
    Button button;


    private void Start()
    {
        animator = GetComponent<Animator>();
        button = GetComponent<Button>();
    }
    // Update is called once per frame
    void Update()
    {
       
            animator.SetBool("Select", button.gameObject==UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject);
        
    }
  
}
