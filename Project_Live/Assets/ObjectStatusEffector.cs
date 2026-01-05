using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using Unity.VisualScripting;
[System.Serializable]
public class BreakEffect
{
    public Image breakUI;
    public float damageRatio;
    public float maxDamageratio;
}
public class ObjectStatusEffector : MonoBehaviour
{
    [SerializeField]List<Image> effectedUI=new List<Image>();
    [SerializeField]List<BreakEffect> breakEffects = new List<BreakEffect>();
    [SerializeField] Volume volume;
    [SerializeField]float noiseThreshold=0.5f;//ÉmÉCÉYÇî≠ê∂Ç≥ÇπÇÈËáíl
    public  Material effectMaterial;
    ObjectStatusManager objectStatusManager;




    private int currentHpSum;

    private int maxHpSum;
    public int MaxHpSum
    {
        get { return maxHpSum; }
        set { maxHpSum = value; }
    }

    private FilmGrain filmGrain;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        volume.profile.TryGet<FilmGrain>(out filmGrain);
        

        objectStatusManager = GetComponent<ObjectStatusManager>();
        foreach(var ui in effectedUI)
        {
            ui.material = effectMaterial;
        }
    }

    // Update is called once per frame
    void Update()
    {
        currentHpSum = 0;
        foreach(var obj in objectStatusManager.ObjectStatuses)
        {
            currentHpSum += (int)obj.Hp;
        }
        var ratio = (float)currentHpSum / maxHpSum;


        if(ratio<noiseThreshold)
        {
            filmGrain.intensity.value = (1 - ratio /noiseThreshold);
        }
        else
        {
            filmGrain.intensity.value = 0;
        }

        foreach(var obj in breakEffects)
        {
            Color color=obj.breakUI.color;
            color.a = obj.damageRatio - ratio;
            color.a = Mathf.Clamp(color.a, 0, obj.maxDamageratio);
            obj.breakUI.color = color;
        }


            foreach (var ui in effectedUI)
            {

                ui.material.SetFloat("_GrayscaleAmount", 1 - ratio);
            }
    }
}
