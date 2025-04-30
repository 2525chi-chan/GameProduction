//�쐬��:����

using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
class SpawnInterval //�X�|�[�����Ԃ̃v���p�e�B�p�N���X
{
    [Header("�X�|�[���܂ł̎���")]
    [SerializeField] public float time;
    [Header("���̃X�|�[�����Ԃ܂łɕK�v�Ȃ����ː�")]
    [SerializeField] public float needGoodNum;
}


public class CommentSpawn : MonoBehaviour
{
    [Header("��������I�u�W�F�N�g")]
    [SerializeField] GameObject commentPrefab;
    [Header("�X�e�[�W")]
    [SerializeField] GameObject stage;
    [Header("�X�|�[�����Ԑݒ�")]
    [SerializeField]List<SpawnInterval> spawnInterval=new List<SpawnInterval>();
    [Header("�K�v�ȃR���|�[�l���g")]
    [SerializeField]GoodSystem goodSystem;

    int spawnIntervalCurrentIndex = 0;  //���݂�spawnInterval�̗v�f��
    float spawnTimer; //�X�|�[�����Ԍv���p�ϐ�
    List<Vector3> sideVertices = new List<Vector3>();   //�X�e�[�W�̑��ʍ��W���i�[���Ă���List
    SpawnInterval currentInterval = new SpawnInterval();    //���݂̃X�|�[�����ԂƕK�v�Ȃ����ː���ݒ肵�Ă����C���X�^���X


    // Start is called before the first frame update
    void Start()
    {
        GetSideVertices(stage, sideVertices);   //�J�n���ɃX�e�[�W�̑��ʍ��W���擾
        currentInterval = spawnInterval[spawnIntervalCurrentIndex];     //�����̃X�|�[�����Ԃ�ݒ�
    }

    void Update()
    {
        spawnTimer += Time.deltaTime;   //�X�|�[���܂ł̎��Ԃ��v��

        if(spawnIntervalCurrentIndex+1<spawnInterval.Count)
        {
                if (goodSystem.GoodNum >= spawnInterval[spawnIntervalCurrentIndex+1].needGoodNum)   //�����ː������̒i�K�ɒB�����珈��
                {
                    spawnIntervalCurrentIndex++;
                    currentInterval = spawnInterval[spawnIntervalCurrentIndex];
                    Debug.Log("�����ː���"+currentInterval.needGoodNum+"�𒴂����̂ŃX�|�[�����Ԃ�"+currentInterval.time+"�b�ɕύX���܂���");
                }
        }

        if (spawnTimer >= currentInterval.time)
        {
            DesideSpawnPos(sideVertices);
            spawnTimer = 0f;
        }
    }

    void DesideSpawnPos(List<Vector3> sideVertices) //���ʍ��W���烉���_���ɐ������W��I�Ԋ֐�
    {
        int random = Random.Range(0, sideVertices.Count);
        //Debug.Log(random);

        Instantiate(commentPrefab, sideVertices[random], Quaternion.identity);
    }

    void GetSideVertices(GameObject stage,List<Vector3>sideVertices)    //���ʂ̒��_���W���擾����֐�
    {
        const float baseRadius = 0.5f;  //�~���̕W���X�P�[���̔��a
        float radius = baseRadius * stage.transform.localScale.x;   //�W���X�P�[���Ƀ��[�J���X�P�[���������邱�ƂŔ��a�����{�ɂȂ��Ă��邩���߂�

        var mesh = stage.GetComponent<MeshFilter>().mesh;
        var vertices = mesh.vertices;                       //unity�Ń��b�V���̒��_���W���擾���鎞�̂��܂��Ȃ�

        foreach (var v in vertices) //�S�Ă̒��_���W���瑤�ʒ��_�̍��W������List�Ɋi�[����J��Ԃ���
        {
            Vector3 worldV = stage.transform.TransformPoint(v); //�X�e�[�W��transform�����[�J�����W���烏�[���h���W�ɕύX
            float r = Mathf.Sqrt(worldV.x * worldV.x + worldV.z * worldV.z);    //���ׂĂ��钸�_�̒��S����̋��������߂�

            if (Mathf.Approximately(r, radius))     //���S����̋������X�e�[�W�̔��a�Ƌߎ��Ȃ瑤�ʂ̍��W�Ƃ���list�ɒǉ�
            {
                sideVertices.Add(worldV);
            }
        }
    }
}
