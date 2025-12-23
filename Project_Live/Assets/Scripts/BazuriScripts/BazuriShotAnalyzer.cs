using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public  class BazuriShotAnalyzer :MonoBehaviour//バズリショットの評価を行うスクリプト
{
    [Header("スコア減衰距離(これ以上距離が離れるとスコアが減衰する)")]
    [SerializeField] float scoredecrementDistance;
    [Header("距離スコア減衰率")]
    [SerializeField] float scoredecrementRate;
    [Header("正面スコア倍率")]
    [SerializeField] float facingRate;
    [Header("正面閾値(どれくらいの正面を許容するか。1は真正面、0は真横)")]
    [SerializeField] float facingThreshold;
    [Header("ラグドール状態の敵にかける倍率")]
    [SerializeField] float ragdollRate=1f;


    
  public  int Analyzer(Camera camera,List<LayerMask> layers)//被写体の状態に応じてスコアを乗算する
    {
        List<GameObject> gameObjects=DetectVisibleObjects(camera,layers);
        float sumScore = 0f;
        foreach (GameObject obj in gameObjects)
        {
         
            float score=obj.GetComponent<BazuriShotData>().score;

            if (obj.CompareTag("Enemy") || obj.CompareTag("Player"))
            {
                Animator animator = obj.GetComponent<Animator>();

                if(animator == null)   animator= obj.GetComponentInParent<Animator>();

                
                if (animator != null)
                {
               //  Debug.Log(obj.name);
                    score *= MultiMotionScore(animator);
                }
               
            
            }
           
            score *= CameraDistance(obj.transform,camera);
            score *= IsFacingCamera(obj.transform, camera, facingThreshold) ? 1f : facingRate;
          
            sumScore += score;
        }  
            Debug.Log(sumScore);
        return (int)sumScore;
    }

    public float MultiMotionScore(Animator animator)
    {
        float scoreRate = 1f;
        if (!animator.enabled)//ラグドール状態だった場合の乗算
        {

            scoreRate *= ragdollRate;

            return scoreRate;
        }

        animator.TryGetComponent<BazuriMotionRate>(out var motionRate);

        if(motionRate != null)
        { 
            scoreRate *= motionRate.GetCurrentMotionRate(animator);
        }
        else
        {
            Debug.Log(animator.gameObject.name+"にBazuriMotionRateがアタッチされていません");
        }


            return scoreRate;
    }
    public bool IsFacingCamera(Transform obj,Camera camera, float threshold)//正面を向いていれば倍率をかける
    {

        Vector3 toCamera=(camera.transform.position - obj.transform.position).normalized;
        float dot = Vector3.Dot(obj.transform.forward, toCamera);

        return dot >= threshold;
    }
    public float CameraDistance(Transform obj, Camera camera)//カメラとの距離が遠い程スコアを減衰させる
    {
   float distance=Vector3.Distance(camera.transform.position,obj.transform.position);
            float scoreRate;
            if (distance <= scoredecrementDistance){
                scoreRate = 1f;
            }
            else
            {
                float step = Mathf.Floor((distance - scoredecrementDistance) / scoredecrementDistance);
                 scoreRate = Mathf.Clamp01(1f - (step + 1) * scoredecrementRate);
            }

        return scoreRate;
    }
    public List<GameObject> DetectVisibleObjects(Camera camera,List< LayerMask> layers)//カメラの中に映ったオブジェクトを検出する
    {

        List<GameObject> viewObjects = new List<GameObject>();
       
        var planes=GeometryUtility.CalculateFrustumPlanes(camera);

        var allObjects = FindObjectsByType<Transform>(FindObjectsSortMode.None);


        foreach (var obj in allObjects)
        {
            bool isInLayer = layers.Any(layer => (layer.value & (1 << obj.gameObject.layer)) != 0);
            if (!isInLayer)
                continue;

            bool isBazuriTarget = obj.gameObject.GetComponent<BazuriShotData>() != null && obj.gameObject.activeSelf;
            if (!isBazuriTarget)
                continue;



            var renderers = obj.gameObject.GetComponentsInChildren<Renderer>();
            if (renderers.Length == 0)
            {
                renderers = GetAllRenderer(obj);

                if (renderers.Length == 0)
                {
                    Debug.Log(obj.gameObject.name);
                    continue;
                }

            }
            

            var bounds = renderers[0].bounds;
            for (int i = 1; i < renderers.Length; i++)
            {
                bounds.Encapsulate(renderers[i].bounds);
            }

            if (GeometryUtility.TestPlanesAABB(planes, bounds))
            {
                viewObjects.Add(obj.gameObject);
            }


        }
        return viewObjects;
    }
    Renderer[] GetAllRenderer(Transform center)
    {
        Transform root = center.parent;

        while(root.parent != null)
        {
            root = root.parent;
        }

        var childrenRenderers = root.GetComponentsInChildren<Renderer>();

        return childrenRenderers;

    } 
}
