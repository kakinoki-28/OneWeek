using UnityEngine;
using TMPro;

public class AttackCountUI : MonoBehaviour
{
    [SerializeField] private AttackCountController attackCountController;
    private TMP_Text attackCountText;
    private int previousAttackCount = -1;
    private int previousMaxAttackCount = -1;

    private void Awake()
    {
        attackCountText = GetComponent<TMP_Text>();

        if (attackCountText == null)
        {
            Debug.LogError(
                "TMP_Textが見つかりません"
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
    }

    private void Update()
    {
        int remainingAttackCount = attackCountController.RemainingAttackCount;
        // 攻撃回数が変化していなければ更新しない
        if (
            remainingAttackCount == previousAttackCount
            && attackCountController.MaxAttackCount == previousMaxAttackCount
        )
        { return; }

        attackCountText.text =
            $"ATTACK {remainingAttackCount} / " +
            $"{attackCountController.MaxAttackCount}";

        previousAttackCount = remainingAttackCount;
        previousMaxAttackCount = attackCountController.MaxAttackCount;
    }
}