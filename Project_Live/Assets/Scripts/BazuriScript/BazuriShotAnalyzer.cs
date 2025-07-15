using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  class BazuriShotAnalyzer :MonoBehaviour
{
    [Header("�X�R�A��������(����ȏ㋗���������ƃX�R�A����������)")]
    [SerializeField] float scoredecrementDistance;
    [Header("�����X�R�A������")]
    [SerializeField] float scoredecrementRate;
    [Header("���ʃX�R�A�{��")]
    [SerializeField] float facingRate;
    [Header("����臒l(�ǂꂭ�炢�̐��ʂ����e���邩�B1�͐^���ʁA0�͐^��)")]
    [SerializeField] float facingThreshold;
  public  void Analyzer(Camera camera,LayerMask layer)
    {
        List<GameObject> gameObjects=DetectVisibleObjects(camera,layer);
        float sumScore = 0f;
        foreach (GameObject obj in gameObjects)
        {
         
            float score=obj.GetComponent<BazuriShotData>().score;

            score *= CameraDistance(obj.transform,camera);
            score *= IsFacingCamera(obj.transform, camera, facingThreshold) ? 1f : facingRate;
          
            sumScore += score;
        }  
            Debug.Log(sumScore);
    }
    public bool IsFacingCamera(Transform obj,Camera camera, float threshold)
    {

        Vector3 toCamera=(camera.transform.position - obj.transform.position).normalized;
        float dot = Vector3.Dot(obj.transform.forward, toCamera);

        return dot >= threshold;
    }
    public float CameraDistance(Transform obj, Camera camera)
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
    public List<GameObject> DetectVisibleObjects(Camera camera, LayerMask layer)
    {

        List<GameObject> viewObjects = new List<GameObject>();
        var allObjects=GameObject.FindObjectsOfType<Transform>();

        foreach (var obj in allObjects)
        {
            if((layer.value&(1<<obj.gameObject.layer))==0) {//UI���C���[�͒e��
                continue;
            }

            Vector3 cameraView=camera.WorldToViewportPoint(obj.position);
            if (cameraView.x >= 0 && cameraView.x <= 1//�J�������Ɏ��܂��Ă��邩�o�Y���V���b�g�̑Ώۂł���Βǉ�����
                && cameraView.y >= 0 && cameraView.y <= 1
                && cameraView.z >= 0 && obj.gameObject.GetComponent<BazuriShotData>())
            {
                viewObjects.Add(obj.gameObject);
            }
          

        }
        return viewObjects;
    }
    
}
