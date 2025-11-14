using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class SRPDisable : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        var pipeLine = GraphicsSettings.currentRenderPipeline as UniversalRenderPipelineAsset;

        if(pipeLine != null)
        {
            pipeLine.useSRPBatcher = false;
        }
    }

    
}
