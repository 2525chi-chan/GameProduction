using UnityEngine;

public class MatchCameraRect : MonoBehaviour//矩形の制御を管理するスクリプト
{
    [SerializeField] Camera baseCamera;
    [SerializeField] Camera bazuriCamera;

    // Update is called once per frame
    void Update()
    {
        if (baseCamera != null && bazuriCamera != null)
        {
 bazuriCamera.rect=baseCamera.rect;//bazuriCameraの矩形をbaseCameraの矩形に合わせる
        }
           
        
    }
}
