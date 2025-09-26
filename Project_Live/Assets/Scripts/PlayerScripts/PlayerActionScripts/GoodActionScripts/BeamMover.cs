using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
//�쐬�ҁF�K��

public class BeamMover : MonoBehaviour
{
    [Header("�r�[�����L�т鑬��")]
    [SerializeField] float speed = 10f;
    [Header("�r�[�����L�т�ő�̒���")]
    [SerializeField] float maxLength = 10f;
    [Header("�`���ς���I�u�W�F�N�g")]
    [SerializeField] Transform target;
    [Header("�r�[���̃G�t�F�N�g")]
    [SerializeField]GameObject beamEffect;
    float currentLength = 0f;
    float previewLength = 0f;
    Vector3 initialScale;
     VisualEffect vfx;
    void Start()
    {
        if (target == null) return;
     GameObject effect= Instantiate(beamEffect, target.position, Quaternion.identity);
         vfx= effect.GetComponent<VisualEffect>();
        initialScale = target.localScale;
        previewLength = initialScale.y;
       
        vfx.SetVector3("StartPos", target.position); 
        
        Quaternion forwardDir = target.rotation;
        vfx.SetFloat("BeamDir", forwardDir.eulerAngles.y);
    }


    void Update()
    {
        if (currentLength > maxLength) return;

        previewLength = initialScale.y;

        currentLength += speed * Time.deltaTime;
        //currentLength = Mathf.Min(currentLength, maxLength);

        target.localScale = new Vector3(initialScale.x, currentLength, initialScale.z);

        float delta = currentLength - previewLength;

        target.localPosition = new Vector3(0, 0, (currentLength + delta) / 2);


        Vector3 beamEnd = target.position + target.up * currentLength;

        float beamScale = Vector3.Distance(vfx.GetVector3("StartPos"), beamEnd);

        vfx.SetVector3("EndPos", target.position);
        vfx.SetFloat("BeamScale", beamScale);


       
    }
}
