using UnityEngine;
using UnityEngine.UI;

public class CastleDamageBar : MonoBehaviour
{
    [SerializeField][Min(0f)] private float damageBarDelay = 0.5f;
    [SerializeField][Min(0.01f)] private float healthDecreaseDuration = 1.5f;
    private Slider damageSlider;
    private float currentBarRate;
    private float animationStartValue;
    private float targetHealthRate;
    private float animationElapsedTime;
    private bool isHealthAnimating;

    private void Awake()
    {
        damageSlider = GetComponent<Slider>();
        if (damageSlider == null)
        {
            Debug.LogError("Sliderコンポーネントが見つかりません");
            enabled = false;
            return;
        }
        currentBarRate = 1f;
    }

    public void PlayDamageBarAnimation(float targetHealthRate)
    {
        animationStartValue = damageSlider.value;
        this.targetHealthRate = targetHealthRate;
        animationElapsedTime = 0f;
        isHealthAnimating = true;
    }

    private void Update()
    {
        if (!isHealthAnimating) { return; }
        animationElapsedTime += Time.deltaTime;
        if (animationElapsedTime < damageBarDelay) { return; }
        float animationTime = animationElapsedTime - damageBarDelay;
        float progress = animationTime / healthDecreaseDuration;
        progress = Mathf.Clamp01(progress);
        damageSlider.value = Mathf.Lerp(
            animationStartValue,
            targetHealthRate,
            progress
        );
        if (progress >= 1f)
        {
            isHealthAnimating = false;
        }
    }
}