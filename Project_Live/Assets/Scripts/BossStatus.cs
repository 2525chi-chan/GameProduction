using UnityEngine;

public class BossStatus : MonoBehaviour
{
    [SerializeField] EnemyDeathHandler deathHandler;
    [SerializeField]GameClearManager gameClearManager;
    private void Start()
    {
        if(deathHandler == null)
        {
            deathHandler = GetComponent<EnemyDeathHandler>();
        }
        if(gameClearManager == null)
        {
            gameClearManager = GameObject.FindWithTag("GameManager").GetComponent<GameClearManager>();
        }
    }
    public void Update()
    {
        if(deathHandler.IsDead)
        {
            // ƒ{ƒX‚ª€–S‚µ‚½‚Æ‚«‚Ìˆ—‚ğ‚±‚±‚É’Ç‰Á
            Debug.Log("Boss has been defeated!");
            gameClearManager.StartClearSequence();
        }
    }

}
