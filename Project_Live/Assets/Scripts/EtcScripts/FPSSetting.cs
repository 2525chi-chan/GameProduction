using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSSetting : MonoBehaviour
{
    [Header("ゲームの最大FPS")]
    [SerializeField] int maxFPS = 60;

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = maxFPS;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
