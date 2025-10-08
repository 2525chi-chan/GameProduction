using UnityEngine;

public class DestroyComment : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void OnClick()
    {
        Destroy(this.gameObject);
    }
}
