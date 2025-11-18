using UnityEngine;
using UnityEngine.VFX;
public class DestroyEffect : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField]float destroyTime=1.0f;
    [SerializeField]bool isDestroy=true;


  BazuriShot BazuriShot;
    public VisualEffect vfx;
    
    void Start()
    {
        vfx = GetComponent<VisualEffect>();
      //  BazuriShot = GameObject.FindWithTag("BazuriShot").GetComponent<BazuriShot>();   
        if (isDestroy)
        {
            Destroy(this.gameObject, destroyTime);
           
        }

    }
    // Update is called once per frame
   
}
