using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSSetting : MonoBehaviour
{
    [Header("ÉQÅ[ÉÄÇÃç≈ëÂFPS")]
    [SerializeField] int maxFPS = 60;

    // Start is called before the first frame update
    void Awake()
    {
        Application.targetFrameRate = maxFPS;
    }
}
