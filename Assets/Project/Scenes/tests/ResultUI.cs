using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ResultUI : MonoBehaviour
{
    private CanvasGroup resultCanvasGroup;
    [SerializeField] private RectTransform[] attackResultRows = new RectTransform[5];
    [SerializeField] private RectTransform separator;
    [SerializeField] private RectTransform totalResultText;
    private Vector2[] rowEndPositions;

    [SerializeField][Min(0.01f)] private float backgroundFadeDuration = 0.4f;

    [SerializeField][Min(0.01f)] private float rowSlideDuration = 0.3f;

    [SerializeField][Min(0f)] private float rowInterval = 0.15f;

    [SerializeField] private float slideStartOffsetX = 900f;
    private Vector2 totalResultEndPosition;
    private AttackResultController attackResultController;
    [SerializeField] private GameObject retryButton;

    private PlaySEPlayer SEPlayer;
    [SerializeField] private AudioClip finalResultSE;

    private void Awake()
    {
        resultCanvasGroup = GetComponent<CanvasGroup>();

        if (resultCanvasGroup == null)
        {
            Debug.LogError(
                "CanvasGroupが見つかりません"
            );

            enabled = false;
            return;
        }

        rowEndPositions = new Vector2[attackResultRows.Length];
        for (int i = 0; i < attackResultRows.Length; i++)
        {
            if (attackResultRows[i] == null)
            {
                Debug.LogError(
                    $"AttackResultRowsの{i}番目が設定されていません"
                );
                enabled = false;
                return;
            }
            rowEndPositions[i] = attackResultRows[i].anchoredPosition;
        }
        if (separator == null || totalResultText == null)
        {
            Debug.LogError(
                "SeparatorまたはTotalResultTextが設定されていません"
            );
            enabled = false;
            return;
        }
        totalResultEndPosition = totalResultText.anchoredPosition;

        attackResultController = GetComponent<AttackResultController>();
        if (attackResultController == null)
        {
            Debug.LogError("AttackResultControllerが見つかりません");
            enabled = false;
            return;
        }
        // 効果音系の読み込み確認
        SEPlayer = GetComponent<PlaySEPlayer>();
        if (SEPlayer == null)
        {
            Debug.LogError("PlaySEPlayerが見つかりません");
            enabled = false;
            return;
        }
        if (finalResultSE == null)
        {
            Debug.LogError("FinalResultSEが設定されていません");
            enabled = false;
            return;
        }
        if (retryButton == null)
        {
            Debug.LogError("RetryButtonが設定されていません");
            enabled = false;
            return;
        }

        HideResults();
    }

    public void ShowResults()
    {
        UpdateResultTexts();
        StopAllCoroutines();
        StartCoroutine(ShowResultsAnimation());
    }

    public void HideResults()
    {
        resultCanvasGroup.alpha = 0f;
        resultCanvasGroup.blocksRaycasts = false;
    }

    [ContextMenu("Debug Show Results")]
    private void DebugShowResults()
    {
        ShowResults();
    }

    private IEnumerator ShowResultsAnimation()
    {
        resultCanvasGroup.alpha = 0f;
        resultCanvasGroup.blocksRaycasts = true;

        // 各行を画面右側へ移動
        for (int i = 0; i < attackResultRows.Length; i++)
        {
            attackResultRows[i].anchoredPosition =
                rowEndPositions[i] +
                Vector2.right * slideStartOffsetX;
        }

        separator.gameObject.SetActive(false);

        totalResultText.anchoredPosition =
            totalResultEndPosition +
            Vector2.right * slideStartOffsetX;

        totalResultText.gameObject.SetActive(false);
        retryButton.SetActive(false);

        // 黒背景とタイトルをフェードイン
        float elapsedTime = 0f;

        while (elapsedTime < backgroundFadeDuration)
        {
            elapsedTime += Time.unscaledDeltaTime;

            float progress =
                elapsedTime / backgroundFadeDuration;

            resultCanvasGroup.alpha =
                Mathf.Clamp01(progress);

            yield return null;
        }

        resultCanvasGroup.alpha = 1f;

        // 1回目から順番に右から入れる
        for (int i = 0; i < attackResultRows.Length; i++)
        {
            // 効果音の再生
            SEPlayer.PlaySE();
            yield return SlideFromRight(
                attackResultRows[i],
                rowEndPositions[i]
            );

            yield return new WaitForSecondsRealtime(
                rowInterval
            );
        }

        separator.gameObject.SetActive(true);
        totalResultText.gameObject.SetActive(true);

        // 最後にTOTALを右から入れる
        // 効果音の再生
        SEPlayer.PlaySE(finalResultSE);
        yield return SlideFromRight(
            totalResultText,
            totalResultEndPosition
        );

        retryButton.SetActive(true);
    }

    private IEnumerator SlideFromRight(
        RectTransform target,
        Vector2 endPosition
    )
    {
        Vector2 startPosition =
            endPosition +
            Vector2.right * slideStartOffsetX;

        target.anchoredPosition = startPosition;

        float elapsedTime = 0f;

        while (elapsedTime < rowSlideDuration)
        {
            elapsedTime += Time.unscaledDeltaTime;

            float progress =
                elapsedTime / rowSlideDuration;

            progress = Mathf.Clamp01(progress);

            target.anchoredPosition = Vector2.Lerp(
                startPosition,
                endPosition,
                progress
            );

            yield return null;
        }

        target.anchoredPosition = endPosition;
    }

    private void UpdateResultTexts()
    {
        for (int i = 0; i < attackResultRows.Length; i++)
        {
            TMP_Text resultText = attackResultRows[i].GetComponent<TMP_Text>();
            if (resultText == null)
            {
                Debug.LogError($"{i + 1}回目のTMP_Textが見つかりません");
                continue;
            }
            if (i >= attackResultController.RecordedAttackCount)
            {
                resultText.text =
                    $"{i + 1}回目" +
                    $"<pos=30%>未使用";
                continue;
            }
            int damage = attackResultController.GetAttackDamage(i);
            // resultText.text = $"{i + 1}回目{damage,6}万円";
            string paddedDamage = damage.ToString().PadLeft(5);
            resultText.text =
                $"{i + 1}回目" +
                $"<pos=30%>" +
                $"<mspace=0.6em>{paddedDamage}</mspace>" +
                $"万円";
        }
        TMP_Text totalText = totalResultText.GetComponent<TMP_Text>();
        if (totalText != null)
        {

            if (attackResultController.TotalScore >= 10000)
            {
                totalText.text =
                    $"被害総額：{attackResultController.TotalScore / 10000}億"
                    + $"{attackResultController.TotalScore % 10000}万円";
            } else
            {
                totalText.text = $"被害総額：{attackResultController.TotalScore,5}万円";
            }
            
        }
    }
    
    public void RetryGame()
    {
        // ヒットストップ中でも通常速度へ戻す
        Time.timeScale = 1f;

        SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex
        );
    }
}