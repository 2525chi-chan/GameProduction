using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  class BazuriShotAnalyzer :MonoBehaviour
{
    [SerializeField] LayerMask detectableLayers;
  public  void Analyzer(Camera camera,LayerMask layer)
    {
        List<GameObject> gameObjects=DetectVisibleObjects(camera);
        foreach (GameObject obj in gameObjects)
        {

            Debug.Log(obj);
        }
            
    }

    public List<GameObject> DetectVisibleObjects(Camera camera)
    {

        List<GameObject> viewObjects = new List<GameObject>();
        var allObjects=GameObject.FindObjectsOfType<Renderer>();

        foreach (var obj in allObjects)
        {
            if(obj.gameObject.layer!=0) {//UIƒŒƒCƒ„[‚Í’e‚­
                continue;
            }

            Vector3 cameraView=camera.WorldToViewportPoint(obj.bounds.center);
            if (cameraView.z >= 0 &&
                cameraView.x>=0&&cameraView.x<=1
                &&cameraView.y>=0&&cameraView.y<=1)
            {
                viewObjects.Add(obj.gameObject);
            }

        }
        return viewObjects;
    }
    
}
