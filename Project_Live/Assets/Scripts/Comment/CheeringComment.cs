using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CheeringComment  :AnimationCheeringComment
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
    [Header("���莞�̃G�t�F�N�g")]
    [SerializeField] ParticleSystem pressEffect;
    [Header("�O�Ղ�`���G�t�F�N�g")]
    [SerializeField] GameObject trailEffect;
    [Header("�O�ՃG�t�F�N�g�̗L��")]
    [SerializeField] bool trailFlag;

    PlayerStatus playerStatus;
    PlayerBuffManager playerBuffManager;
    CommentSpawn commentSpawn;
    CommentMove commentMove;
    
    Button thisComment;

    GameObject PowerBuffImage;
    GameObject AgilityBuffImage;

    private void Start()
    {
        playerBuffManager=GameObject.FindGameObjectWithTag("PlayerBuffManager").GetComponent<PlayerBuffManager>();
        playerStatus=GameObject.FindGameObjectWithTag("PlayerStatus").GetComponent<PlayerStatus>();
        commentSpawn = GameObject.FindGameObjectWithTag("CommentSpawn").GetComponent<CommentSpawn>();
        thisComment=this.gameObject.GetComponent<Button>();
        commentMove=this.gameObject.GetComponent<CommentMove>();

        animator = this.gameObject.GetComponent<Animator>();

        rectTransform=this.GetComponent<RectTransform>();
        PowerBuffImage = GameObject.FindGameObjectWithTag("PowerBuffImage");
        AgilityBuffImage = GameObject.FindGameObjectWithTag("AgilityBuffImage");

        trailEffect.SetActive(false);
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
        AnimationBeforeMethod();

        // �R���[�`���������BuffAttack�����s����R�[���o�b�N��n��
        OnButtonClicked(PowerBuffImage, () =>
        {
            // �A�j���[�V����������Ɏ��s�����
            playerBuffManager.BuffAttack(attackMagnification, buffTime);
            DestroyComment();
        });
    }

    public void BuffAgility()
    {
        AnimationBeforeMethod();

        OnButtonClicked(AgilityBuffImage, () =>
        {
            playerBuffManager.BuffMoveSpeed(agilityMagnification, buffTime);
            DestroyComment();
        });
    }

    void AnimationBeforeMethod()
    {
        pressEffect.Play();
        EventSystem.current.SetSelectedGameObject(null);
        commentMove.enabled = false;
        if (trailFlag)
        {
            trailEffect.SetActive(true);
        }
    }

    void DestroyComment()
    {
        Destroy(this.gameObject);
    }

    private void OnDestroy()
    {
        commentSpawn.cheeringCommentIsExist = false;
    }
}
