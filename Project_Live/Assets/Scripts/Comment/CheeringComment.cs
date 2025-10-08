using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheeringComment : MonoBehaviour
{
    [Header("������HP")]
    [SerializeField] float addHPnum;
    [Header("HP����%�ȉ��ɂȂ�����񕜂ɂ��邩")]
    [SerializeField,Range(1,100)] float hpPercent;
    [Header("�o�t����U���{��")]
    [SerializeField] float attackMagnification;
    [Header("�o�t���鑬�x�{��")]
    [SerializeField] float agilityMagnification;
    [Header("�o�t�̎�������")]
    [SerializeField] float buffTime;
    [Header("�����R�����g�̓��e")]
    public  List<string> cheeringCommentContent = new List<string>();

    PlayerStatus playerStatus;
    PlayerBuffManager playerBuffManager;
    CommentSpawn commentSpawn;
    Button thisComment;

    private void Start()
    {
        playerBuffManager=GameObject.FindGameObjectWithTag("PlayerBuffManager").GetComponent<PlayerBuffManager>();
        playerStatus=GameObject.FindGameObjectWithTag("PlayerStatus").GetComponent<PlayerStatus>();
        commentSpawn = GameObject.FindGameObjectWithTag("CommentSpawn").GetComponent<CommentSpawn>();
        thisComment=this.gameObject.GetComponent<Button>();

        commentSpawn.cheeringCommentIsExist=true;

        SetCommentAction();
    }

    private void SetCommentAction()
    {
        thisComment.onClick.RemoveAllListeners(); // �O�̂��߃��X�i�[���N���A


        if (playerStatus.Hp / playerStatus.MaxHp <= hpPercent/100)  // HP���w��%�ȉ��Ȃ��
        {
            thisComment.onClick.AddListener(AddHP);
        }
        else
        {
            //  HP���w��%�ȏ�Ȃ�o�t����
            bool attackActive = playerBuffManager.attackBuff != null && playerBuffManager.attackBuff.isActive;
            bool speedActive = playerBuffManager.speedBuff != null && playerBuffManager.speedBuff.isActive;

            if (attackActive && !speedActive)
            {
                // �U���o�t���Ȃ�X�s�[�h�o�t
                thisComment.onClick.AddListener(BuffAgility);
            }
            else if (!attackActive && speedActive)
            {
                // �X�s�[�h�o�t���Ȃ�U���o�t
                thisComment.onClick.AddListener(BuffAtack);
            }
            else
            {
                // �����������Ă��� or �����������Ă��Ȃ��ꍇ�̓����_��
                if (Random.value < 0.5f)
                    thisComment.onClick.AddListener(BuffAtack);
                else
                    thisComment.onClick.AddListener(BuffAgility);
            }
        }
    }


    public void AddHP()
    {
        playerBuffManager.AddHP(addHPnum);
        DestroyComment();
    }

    public void BuffAtack()
    {
        playerBuffManager.BuffAttack(attackMagnification, buffTime);
        DestroyComment();
    }

    public void BuffAgility()
    {
        playerBuffManager.BuffMoveSpeed(agilityMagnification, buffTime);
        DestroyComment();
    }

    void DestroyComment()
    {
        Destroy(this.gameObject);
        commentSpawn.cheeringCommentIsExist = false;
    }

    private void OnDestroy()
    {
        commentSpawn.cheeringCommentIsExist = false;
    }
}
