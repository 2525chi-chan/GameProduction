//�쐬��:����

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CommentSpawn : MonoBehaviour
{
    [Header("��������I�u�W�F�N�g")]
    [SerializeField] GameObject commentPrefab;
    [Header("�X�e�[�W")]
    [SerializeField] GameObject stage;
    [Header("��������Ԋu")]
    [SerializeField] float spawnInterval = 2f;

    float time; //�o�ߎ��ԗp�ϐ�
    List<Vector3> sideVertices = new List<Vector3>();   //�X�e�[�W�̑��ʍ��W���i�[���Ă���List
    
    // Start is called before the first frame update
    void Start()
    {
        const float baseRadius = 0.5f;  //�~���̕W���X�P�[���̔��a
        float radius = baseRadius * stage.transform.localScale.x;   //�W���X�P�[���Ƀ��[�J���X�P�[���������邱�ƂŔ��a�����{�ɂȂ��Ă��邩���߂�

        var mesh=stage.GetComponent<MeshFilter>().mesh;
        var vertices = mesh.vertices;                       //unity�Ń��b�V���̒��_���W���擾���鎞�̂��܂��Ȃ�

        foreach (var v in vertices) //�S�Ă̒��_���W���瑤�ʒ��_�̍��W������List�Ɋi�[����J��Ԃ���
        {
            Vector3 worldV = stage.transform.TransformPoint(v);
            float r = Mathf.Sqrt(worldV.x * worldV.x + worldV.z * worldV.z);

            if (Mathf.Approximately(r, radius))
            {
                sideVertices.Add(worldV);
            }
        }
    }

    void Update()
    {
        time += Time.deltaTime;

        if (time >= spawnInterval)
        {
            DesideSpawnPos(sideVertices);
            time = 0f;
        }
    }

    void DesideSpawnPos(List<Vector3> sideVertices)
    {
        
        int random = Random.Range(0, sideVertices.Count);
        Debug.Log(random);

        Instantiate(commentPrefab, sideVertices[random], Quaternion.identity);
    }
}
