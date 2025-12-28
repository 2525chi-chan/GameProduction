using UnityEngine;
using UnityEngine.UI;

public class ObjectHPDisplay : MonoBehaviour
{
    [Header("HP表示用のImage")]
    [SerializeField] Image hpBarImage;
    [Header("表示するキャンバス")]
    [SerializeField] Canvas canvas;
    [Header("HP表示の正面を向ける対象となるカメラ")]
    [SerializeField] Camera targetCamera;
    [Header("必要なコンポーネント")]
    [SerializeField] ObjectStatus objectStatus;

    void Start()
    {
        if (objectStatus == null) objectStatus = GetComponent<ObjectStatus>();
        
        if (canvas == null) return;
        canvas.worldCamera = targetCamera;
    }

    void Update()
    {
        canvas.transform.rotation = targetCamera.transform.rotation;

        hpBarImage.fillAmount = (float)objectStatus.Hp / objectStatus.MaxHp;
    }
}
