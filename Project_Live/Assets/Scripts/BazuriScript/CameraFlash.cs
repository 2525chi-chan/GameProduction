
using UnityEngine;
using UnityEngine.UI;
public class CameraFlash : MonoBehaviour//�o�Y���V���b�g�������ۂ̃J�����t���b�V���B
{
    [Header("�t���b�V���f��(��ʂ𕢂����p�l��)")]
    [SerializeField] Image flashPanel;
    [Header("�t���b�V���̑��x")]
    [SerializeField]float flashSpeed = 5f;
    [Header("�p�l���̐F")]
    [SerializeField] Color flashColor;

    private bool isFlashing = false;
    // Update is called once per frame
    void Update()
    {
        if (isFlashing)
        {
            flashPanel.color=  Color.Lerp(flashPanel.color, Color.clear, flashSpeed * Time.deltaTime);
            if (flashPanel.color.a<=0.01f) {

                ResetAlpha();
            }
        }
    }
    public void StartFlash()
    {
        if (!isFlashing)
        {
            isFlashing = true;
           
            flashPanel.color = new Color(flashColor.r, flashColor.g, flashColor.b, 1f);
           
        }
    }
    public void ResetAlpha()//�A�����Ƀt���b�V�����N����Ȃ����ۂ�h�����߂ɁA�t���b�V���̐F�����Z�b�g����B
    {
               flashPanel.color = Color.clear;
        isFlashing = false;
    }
}
