using UnityEngine;

[CreateAssetMenu(fileName = "New RequestEnemy", menuName = "Request/Enemy")]
public class RequestEnemySO : RequestBaseSO
{
    public enum EnemyType
    {
        All, Selected
    }

    [Header("“|‚·“G‚Í‰½‚Å‚à‚¢‚¢‚Ì‚©Aw’è‚³‚ê‚½“G‚©")]
    public EnemyType enemyType;
    [Header("“|‚µ‚Ä‚Ù‚µ‚¢“G‚Ì”")]
    public int defeatEnemyNum;
    [Header("“|‚µ‚Ä‚Ù‚µ‚¢“G‚Ìí—Ş(*All‚Ìê‡‚ÍNone")]
    public GameObject targetEnemy;
    [Header("“|‚µ‚Ä‚Ù‚µ‚¢“G‚Ì–¼‘O")]
    public string enemyName;

    [System.NonSerialized]
    public int enemyCounter = 0;

    protected override void OnEnable()
    {
        base.OnEnable();
        //Initialize();
        requestType = RequestType.Enemy;
        switch (enemyType)
        {
            case EnemyType.All:
                displayText = "‚ ‚Æ" + defeatEnemyNum + "‘Ì“G‚ğ“|‚»‚¤!!!";
                break;
            case EnemyType.Selected:
                displayText = enemyName + "‚ğ‚ ‚Æ" + defeatEnemyNum + "‘Ì“|‚»‚¤III";
                break;
        }
    }
}
