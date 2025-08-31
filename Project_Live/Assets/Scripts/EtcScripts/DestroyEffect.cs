using UnityEngine;

public class DestroyEffect : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField]float destroyTime=1.0f;
    [SerializeField]bool isDestroy=true;
  
    // Update is called once per frame
    void Update()
    {
        if(isDestroy)
        {
            Destroy(this.gameObject,destroyTime);
            isDestroy = false;
        }
    }
}
