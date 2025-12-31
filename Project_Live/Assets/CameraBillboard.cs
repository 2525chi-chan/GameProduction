using UnityEngine;

public class CameraBillboard : MonoBehaviour
{
    [SerializeField] Transform mainCamera;
    public bool isEnabled = true;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!isEnabled) return;
        Vector3 direction = mainCamera.position - transform.position;

        // Y²¬•ª‚ğœ‹iã‰º‰ñ“]‚È‚µj
        direction.y = 0;

        // Y²‚Ì‚İƒJƒƒ‰•ûŒü‚Ö‰ñ“]
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(-direction);

            // Œ»İ‚ÌY²‰ñ“]‚Ì‚İ•Û
            transform.rotation = Quaternion.Euler(0, targetRotation.eulerAngles.y, 0);
        }
    }
}
