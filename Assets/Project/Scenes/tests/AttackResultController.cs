using UnityEngine;

public class AttackResultController : MonoBehaviour
{
    [SerializeField] private ScoreUIScript scoreUI;

    [SerializeField] private AttackCountController attackCountController;

    private int[] attackDamageAmounts;
    private int recordedAttackCount;
    private int previousTotalScore;
    private ResultUI resultUI;

    public int RecordedAttackCount => recordedAttackCount;

    public int TotalScore =>  previousTotalScore;

    private void Awake()
    {
        if (scoreUI == null)
        {
            Debug.LogError(
                "ScoreUIScriptが設定されていません"
            );
            enabled = false;
            return;
        }
        if (attackCountController == null)
        {
            Debug.LogError(
                "AttackCountControllerが設定されていません"
            );
            enabled = false;
            return;
        }
        resultUI = GetComponent<ResultUI>();
        if (resultUI == null)
        {
            Debug.LogError(
                "ResultUIが見つかりません"
            );
            enabled = false;
            return;
        }
        attackDamageAmounts = new int[attackCountController.MaxAttackCount];
        recordedAttackCount = 0;
        previousTotalScore = 0;
    }

    public void RecordAttackResult()
    {
        if (recordedAttackCount >= attackDamageAmounts.Length) { return; }
        int currentTotalScore = scoreUI.CurrentScore;
        int currentAttackDamage = currentTotalScore - previousTotalScore;
        currentAttackDamage = Mathf.Max(0, currentAttackDamage);
        attackDamageAmounts[recordedAttackCount] = currentAttackDamage;
        recordedAttackCount++;
        previousTotalScore = currentTotalScore;
        Debug.Log(
            $"{recordedAttackCount}回目の被害額: " +
            $"{currentAttackDamage}万円"
        );

        if (recordedAttackCount >= attackDamageAmounts.Length)
        {
            resultUI.ShowResults();
        }
    }

    public int GetAttackDamage(int attackIndex)
    {
        if (
            attackIndex < 0 ||
            attackIndex >= recordedAttackCount
        )
        { return 0; }
        return attackDamageAmounts[attackIndex];
    }
}