using UnityEngine;

public class AttackCountController : MonoBehaviour
{
    [SerializeField][Min(1)] private int maxAttackCount = 3;
    [SerializeField][Min(0)] private int remainingAttackCount;
    public int MaxAttackCount => maxAttackCount;
    public int RemainingAttackCount => remainingAttackCount;
    public bool CanAttack => remainingAttackCount > 0;

    private void Awake()
    {
        remainingAttackCount = maxAttackCount;
    }

    public bool Attack()
    {
        if (!CanAttack)
        {
            Debug.Log("攻撃回数が残っていません");
            return false;
        }
        remainingAttackCount--;
        return true;
    }

    [ContextMenu("Reset Attack Count")]
    public void ResetAttackCount()
    {
        remainingAttackCount = maxAttackCount;
    }
}