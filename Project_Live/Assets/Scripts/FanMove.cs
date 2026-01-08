using Unity.VisualScripting;
using UnityEngine;

public class FanMove : MonoBehaviour
{
    [SerializeField] GameObject hontai;
    [SerializeField] Renderer quadRenderer;

   
    [SerializeField] GameObject rightShoulder;
    [SerializeField] GameObject leftShoulder;
    [SerializeField] float swingLimit=50f;
    [SerializeField]float swingSpeed=0.5f;


    [SerializeField] float initializeVerocity=2f;
    [SerializeField] float gravity=-9.8f;
    float start_Y;
    float verticalVerocity = 0f;
    bool isSwingHand = false;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    Quaternion defaultRote;
  
    void Start()
    {
        defaultRote = rightShoulder.transform.localRotation;   
        start_Y= hontai.transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        verticalVerocity +=gravity* Time.deltaTime;
        Vector3 pos = hontai.transform.position;
        pos.y += verticalVerocity*Time.deltaTime;

        if (isSwingHand)
        {
            var angle = Mathf.Sin(swingSpeed * Time.time) * swingLimit;
            angle += defaultRote.eulerAngles.z;

            rightShoulder.transform.localRotation = Quaternion.Euler(0, 0, angle);
            leftShoulder.transform.localRotation = Quaternion.Euler(0, 0, -angle);
        }

        if (pos.y < start_Y)
        {

            pos.y = start_Y;
            verticalVerocity = initializeVerocity;

        }

        hontai.transform.position = pos;
    }
    public void SwingStart()
    {
        isSwingHand = true;
       
        quadRenderer.enabled = true;
    }
   
    public void SetSpeed(float speed)
    {
        swingLimit = speed;
    }
}
