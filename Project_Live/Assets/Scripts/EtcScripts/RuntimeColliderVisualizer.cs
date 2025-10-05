using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class RuntimeColliderVisualizer : MonoBehaviour
{
    public Color color = new Color(1, 0, 0, 0.25f); // ”¼“§–¾
    private GameObject visObj;

    void Start()
    {
        SphereCollider col = GetComponent<SphereCollider>();
        visObj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        visObj.transform.SetParent(transform);
        visObj.transform.localPosition = col.center;
        visObj.transform.localScale = Vector3.one * col.radius * 2f;
        Destroy(visObj.GetComponent<Collider>()); // “–‚½‚è”»’è‚Í•s—v

        var mat = new Material(Shader.Find("Standard"));
        mat.color = color;
        visObj.GetComponent<Renderer>().material = mat;
    }
}
