//�쐬�ҁF����
//�����ړ��ɂ��Ă�{https://www.gocca.work/unity-parabolic-movement/}������p

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommentLaunch : MonoBehaviour
{
    [Header("�X�e�[�W")]
    [SerializeField] GameObject stage;
    [Header("�؋󎞊�")]
    [SerializeField] float flightTime = 2f;
    [Header("�ړ����x�{��")]
    [SerializeField] float speedRate = 1f;
    private const float gravity = -9.8f;    //�d��
    BoxCollider commentCollider;    //���V���͎��Ȃ��悤�ɂ��邽�߂ɃR���C�_�[���擾

    // Start is called before the first frame update
    void Start()
    {
        Vector3 center = stage.transform.position + Vector3.up * stage.transform.localScale.y * 0.5f; // �X�e�[�W�̏�ʒ��S
        float radius = 0.5f * stage.transform.localScale.x; // �~���̃X�P�[�����甼�a���擾

        Vector3 randomTarget = GetRandomPointOnCylinderTop(center, radius);

        // �I�u�W�F�N�g�̍���������Y���W���グ��
        float objectHeight = GetObjectHeight(this.gameObject);
        randomTarget.y += objectHeight * 0.5f; // �����������S�����ʂ܂Ŕ����Ȃ̂�0.5�{

        commentCollider=GetComponent<BoxCollider>();

        commentCollider.enabled = false;    //�������ꂽ�u�Ԃ̓R���C�_�[������

        StartCoroutine(Launch(randomTarget, flightTime, speedRate, gravity));
    }

    float GetObjectHeight(GameObject obj)
    {
        var collider = obj.GetComponent<Collider>();
        if (collider != null)
        {
            return collider.bounds.size.y;
        }
        else
        {
            // Collider�Ȃ��ꍇ�͓K���ȃT�C�Y���f�t�H���g
            return 1f; // ����1m����
        }
    }
    Vector3 GetRandomPointOnCylinderTop(Vector3 center, float radius)
    {
        // ���S���烉���_���ȋ����E�p�x�œ_�����
        float angle = Random.Range(0f, Mathf.PI * 2f); // 0�`360�x
        float distance = Mathf.Sqrt(Random.Range(0f, 1f)) * radius; // �ʐϋϓ��ɂȂ�悤�Ɂ�����

        float x = Mathf.Cos(angle) * distance;
        float z = Mathf.Sin(angle) * distance;

        return new Vector3(center.x + x, center.y, center.z + z);
    }

    private IEnumerator Launch(Vector3 targetPos, float flightTime, float speedRate, float gravity) //�����ړ��̊֐�
    {
        var startPos = this.transform.position; //�X�|�[���ʒu
        var diffY = (targetPos - startPos).y;   //���n�_�ƃX�|�[���n�_��y���W�̍�
        var v = (diffY - 0.5f * gravity * flightTime * flightTime) / flightTime;    //y�����̏����x�̌v�Z

        for (var t = 0f; t < flightTime; t += (Time.deltaTime * speedRate))     
        {
            var p = Vector3.Lerp(startPos, targetPos, t / flightTime);  //���������̈ړ�����
            p.y = startPos.y + v * t + 0.5f * gravity * t * t;  //���������̈ړ�����
            this.transform.position = p;    //���߂����������Ɖ��������ֈړ�
            yield return null;  //����Ńt���[���ԂŌv�Z���Ă����炵��
        }

        this.transform.position = targetPos;     //���n�_�֔�����
        commentCollider.enabled = true; //���n������R���C�_�[��L���ɂ���
    }
}
