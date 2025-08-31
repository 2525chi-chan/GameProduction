using UnityEngine;
using UnityEngine.Experimental.VFX;
using UnityEngine.VFX;
public class BazuriEffect : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField] float moveSpeed;

    GameObject target;
    VisualEffect vfx;
    float offset;
    void Start()
    {
        target=GetComponent<GameObject>();
        vfx=GetComponent<VisualEffect>();
    }

    // Update is called once per frame
    void Update()
    {
         offset+= moveSpeed * Time.unscaledDeltaTime;

        vfx.SetVector3("End", new Vector3(offset, 1, 0));
        vfx.SetVector3("Start", new Vector3(-offset, 1, 0));

    }
}
