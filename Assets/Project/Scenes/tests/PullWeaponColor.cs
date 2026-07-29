using UnityEngine;


// 半透明な少し浮かせた膜みたいなのを作る
// 緑から赤へのグラデーション？
public class PullWeaponColor : MonoBehaviour
{
    [SerializeField] private Color startColor = Color.green;
    [SerializeField] private Color midColor = Color.yellow;
    [SerializeField] private Color endColor = Color.red;
    [SerializeField] [Range(0f, 1f)] private float transparency = 0.3f;
    private MousePullTest pullInput;
    private Renderer pullGaugeRenderer;
    private Material pullGaugeMaterial;
    [SerializeField] private Gradient pullColorGradient = new Gradient();

    private void Awake()
    {
        pullInput = GetComponentInParent<MousePullTest>();
        if (pullInput == null)
        {
            Debug.LogError("MousePullTestが見つかりません");
            enabled = false;
        }
        pullGaugeRenderer = GetComponent<Renderer>();
        if (pullGaugeRenderer == null)
        {
            Debug.LogError("Rendererが見つかりません");
            enabled = false;
        }
        pullGaugeMaterial = pullGaugeRenderer.material;
        // SetGaugeColor(0f);
        pullGaugeRenderer.enabled = false;

        ConfigureGradient();
    }

    private void LateUpdate()
    {
        float pullRate = pullInput.VisualPullRate;
        bool isWrongDirection = pullInput.Power < 0f;
        bool shouldShow =
            pullInput.IsDragging
            && !isWrongDirection
            && pullRate > 0f;
        pullGaugeRenderer.enabled = shouldShow;
        if (!shouldShow)
        {
            return;
        }

        SetGaugeColor(pullRate);
    }

    private void SetGaugeColor(float pullRate)
    {
        // Color currentColor = Color.Lerp(startColor, endColor, pullRate*pullRate);
        Color currentColor = pullColorGradient.Evaluate(pullRate);


        // currentColor.a = maxAlpha * pullRate;
        // currentColor.a = transparency;
        pullGaugeMaterial.color = currentColor;
    }

    private void ConfigureGradient()
    {
        GradientColorKey[] colorKeys =
        {
            new GradientColorKey(
                startColor,
                0f
            ),

            new GradientColorKey(
                midColor,
                0.5f
            ),

            new GradientColorKey(
                endColor,
                1f
            )
        };
        GradientAlphaKey[] alphaKeys =
        {
            new GradientAlphaKey(
                transparency,
                0f
            ),

            // new GradientAlphaKey(
            //     transparency,
            //     0.47f
            // ),

            new GradientAlphaKey(
                transparency+0.3f,
                0.5f
            ),

            // new GradientAlphaKey(
            //     transparency,
            //     0.53f
            // ),

            new GradientAlphaKey(
                transparency,
                1f
            )
        };

        pullColorGradient.SetKeys(
            colorKeys,
            alphaKeys
        );
        
    }
}