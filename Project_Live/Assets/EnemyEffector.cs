using UnityEngine;
using System.Collections.Generic;
using System.Linq;
public class EnemyEffector : MonoBehaviour//おねだりのターゲットになったときの見た目の変更をするスクリプト
{
    [SerializeField] GameObject rootObject;
    [SerializeField] Material defaultMaterial;
    [SerializeField]Material targetMaterial;

    [SerializeField] float outlineWidth = 10f;
    [SerializeField] float outlineMoveSpeed;
    [SerializeField] float outlineMoveMultiply;  

    EnemyStatus enemyStatus;
    List<Renderer> changeRenderer;
    bool prevIsTarget;
  //  float count= 0f;
    Material setMaterial;
    private void Start()
    {
        enemyStatus=GetComponent<EnemyStatus>();    
        changeRenderer = new List<Renderer>();
        setMaterial =new Material( targetMaterial);

        changeRenderer = rootObject.GetComponentsInChildren<Renderer>().ToList();

        if (enemyStatus.isTarget) ChangeMaterial();
        prevIsTarget = true;
    }

    void Update()
    {
        if (prevIsTarget != enemyStatus.isTarget)
        {
            ChangeMaterial();
        }

        if (enemyStatus.isTarget)
        {
            SinMoveOutlineWidth();
        }
       

        prevIsTarget=enemyStatus.isTarget;
    }

    public void ChangeMaterial()
    {
  
        foreach (Renderer renderer in changeRenderer)
        {

            renderer.material = enemyStatus.isTarget ? setMaterial : defaultMaterial;

        } 
      
       
    }
    public void SinMoveOutlineWidth()
    {
        var sin = Mathf.Sin(Time.time * outlineMoveSpeed);

        var width = outlineWidth + sin * outlineMoveMultiply;

        setMaterial.SetFloat("_Outline_Width", width);
    }
}
