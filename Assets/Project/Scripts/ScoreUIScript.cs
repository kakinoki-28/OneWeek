using UnityEngine;
using TMPro;

public class ScoreUIScript : MonoBehaviour
{
    private TMP_Text scoreText;
    [SerializeField] private Transform castleRoot;
    private float previousScore = 0f;

    private PrefabSwitcherScript[] castleParts;

    private void Awake()
    {
        scoreText = GetComponent<TMP_Text>();

        if (scoreText == null)
        {
            Debug.LogError("TMP_Textが見つかりません");
            enabled = false;
            return;
        }
        if (castleRoot == null)
        {
            Debug.LogError("CastleRootが設定されていません");
            enabled = false;
            return;
        }
        else
        {
            castleParts = castleRoot.GetComponentsInChildren<PrefabSwitcherScript>(true);
        }
        scoreText.text =
            $"<mark=#0000004F padding=\"20,20,0,0\">" +
            $"被害総額 0 万円" +
            $"</mark>";
    }

    // Update is called once per frame
    void Update()
    {
        int score = 0;
        foreach (PrefabSwitcherScript castlePart in castleParts)
        {
            score += Mathf.CeilToInt(castlePart.CurrentAmountDamage);
        }
        // スコアが変化していなければ更新しない
        if (score == previousScore)
        { return; }

        scoreText.text =
            $"<mark=#0000004F padding=\"20,20,0,0\">" + 
            $"被害総額 {score} 万円" +
            $"</mark>";
        previousScore = score;
    }
}
