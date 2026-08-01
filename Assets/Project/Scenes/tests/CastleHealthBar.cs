using UnityEngine;
using UnityEngine.UI;

public class CastleHealthBar : MonoBehaviour
{
    [SerializeField] private Transform castleRoot;
    [SerializeField] private CastleDamageBar damageBar;
    private Slider healthSlider;
    private PrefabSwitcherScript[] castleParts;
    private float totalMaxHealth;
    private float previousHealthRate;

    private void Awake()
    {
        healthSlider = GetComponent<Slider>();
        if (healthSlider == null)
        {
            Debug.LogError("Sliderコンポーネントが見つかりません");
            enabled = false;
            return;
        }

        if (castleRoot == null)
        {
            Debug.LogError("CastleRootが設定されていません");
            enabled = false;
            return;
        }

        SetCastle(castleRoot);

        if (damageBar == null)
        {
            Debug.LogError("CastleDamageBarが設定されていません");
            enabled = false;
            return;
        }
    }

    public void SetCastle(Transform newCastleRoot)
    {
        castleRoot = newCastleRoot;
        castleParts = castleRoot.GetComponentsInChildren<PrefabSwitcherScript>(true);
        totalMaxHealth = 0f;
        foreach (PrefabSwitcherScript castlePart in castleParts)
        {
            totalMaxHealth += castlePart.maxHealth;
        }
        previousHealthRate = totalMaxHealth;
    }

    private void Update()
    {
        float totalCurrentHealth = 0f;
        foreach (PrefabSwitcherScript castlePart in castleParts)
        {
            totalCurrentHealth += castlePart.currentHealth;
        }
        healthSlider.value = totalCurrentHealth / totalMaxHealth;

        // 体力が変化した場合にダメージバーの動きを開始
        if (previousHealthRate != healthSlider.value)
        {
            damageBar.PlayDamageBarAnimation(healthSlider.value);
            previousHealthRate = healthSlider.value;
        }
    }
}