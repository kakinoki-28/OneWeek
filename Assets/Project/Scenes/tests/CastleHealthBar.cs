using UnityEngine;
using UnityEngine.UI;

public class CastleHealthBar : MonoBehaviour
{
    [SerializeField] private Transform castleRoot;
    private Slider healthSlider;
    private PrefabSwitcherScript[] castleParts;
    private float totalMaxHealth;

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
    }

    private void Update()
    {
        float totalCurrentHealth = 0f;
        foreach (PrefabSwitcherScript castlePart in castleParts)
        {
            totalCurrentHealth += castlePart.currentHealth;
        }
        healthSlider.value = totalCurrentHealth / totalMaxHealth;
    }
}