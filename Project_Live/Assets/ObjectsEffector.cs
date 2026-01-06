using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
public class ObjectEffector : MonoBehaviour
{
   
    [SerializeField]List<Material> matchMaterial=new List<Material>();
   
    [SerializeField] GameObject startObject;
   // [GradientUsage(true)]
 
    [GradientUsage(true)]
    [SerializeField] Gradient damageGradient_Model;

   [SerializeField] ObjectStatus status;

    List<Material> defaultMaterial=new List<Material>();
    List<Renderer> effecterRenderer=new List<Renderer>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       List<Renderer>allRenderer=startObject.GetComponentsInChildren<Renderer>(true).ToList<Renderer>();
      

        for (int i=0;i<matchMaterial.Count;i++)
        {
            defaultMaterial.Add(matchMaterial[i]);
        }

        foreach (var rend in allRenderer)
        {

            for (int i = 0; i < matchMaterial.Count; i++)
            {
                if (matchMaterial[i] == rend.sharedMaterial)
                {
                    effecterRenderer.Add(rend);

                    rend.material = new Material(matchMaterial[i]);
                    rend.material.CopyPropertiesFromMaterial(defaultMaterial[i]);
                    break;
                }

            }
                
                //var inst = new Material(effectMaterial);
                //rend.material = inst;
                //effecterRenderer.Add(rend);
            
        }


     
        //status = GetComponent<ObjectStatus>();

    }

    // Update is called once per frame
    void Update()
    {
        var ratio = 1.0f - (float)status.Hp / status.MaxHp;
       

        //barrier.material.SetColor("_BaseColor", damageGradient_Base.Evaluate(ratio));
        //barrier.material.SetColor("_FresnelColor", damageGradient_Edge.Evaluate(ratio));

        foreach (var rend in effecterRenderer)
        {
            rend.material.SetColor("_EmissionColor", damageGradient_Model.Evaluate(ratio));

        }
    }
}
