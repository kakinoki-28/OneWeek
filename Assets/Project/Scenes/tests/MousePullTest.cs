using UnityEngine;
using UnityEngine.InputSystem;

public class MousePullTest : MonoBehaviour
{
    [SerializeField]
    [Range(0.1f, 1f)]
    private float maxPullScreenRatio = 0.5f;
    private bool isDragging = false;
    private Vector2 pressPosition;
    private Vector2 currentPosition;
    private float pullDistance;
    private float power;
    private bool isOverMaxPull;
    // 引っ張りの割合を上限を超えた場合でも扱うための変数
    private float visualPullRate;
    private bool hasReleasedWeapon = false;
    public bool HasReleasedWeapon => hasReleasedWeapon;

    public float Power => power;
    public bool IsDragging => isDragging;
    public float VisualPullRate => visualPullRate;

    [SerializeField] private LayerMask laneArrowLayer;

    private void Update()
    {
        // 発射後はリセットされるまで入力を受け付けない
        if (hasReleasedWeapon) { return; }
        Mouse mouse = Mouse.current;
        if (mouse == null) { return; }
        if (mouse.leftButton.wasPressedThisFrame)
        {
            Vector2 mousePosition = mouse.position.ReadValue();
            if (TryClickLaneArrow(mousePosition)) { return; }
            BeginDrag(mouse.position.ReadValue());
        }
        if (isDragging && mouse.leftButton.isPressed)
        {
            UpdateDrag(mouse.position.ReadValue());
        }
        if (isDragging && mouse.leftButton.wasReleasedThisFrame)
        {
            EndDrag(mouse.position.ReadValue());
        }
    }

    private bool TryClickLaneArrow(Vector2 mousePosition)
    {
        Camera mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Debug.LogError("Main Cameraが見つかりません");
            return false;
        }
        Ray ray = mainCamera.ScreenPointToRay(mousePosition);
        bool didHit = Physics.Raycast(
            ray,
            out RaycastHit hit,
            Mathf.Infinity,
            laneArrowLayer,
            QueryTriggerInteraction.Collide
        );
        if (!didHit) { return false; }
        LaneArrow laneArrow = hit.collider.GetComponentInParent<LaneArrow>();
        if (laneArrow == null) { return false; }
        laneArrow.Activate();
        return true;
}

    private void BeginDrag(Vector2 position)
    {
        isDragging = true;
        pressPosition = position;
        Debug.Log($"ドラッグ開始位置：{pressPosition}");
    }

    private void UpdateDrag(Vector2 position)
    // 最大幅よりも大きいとpowerは0になりisOverMaxPullがtrueに
    // 逆方向に引くとpowerは負の値になる
    {
        currentPosition = position;
        // pullDistance = currentPosition.x - pressPosition.x;
        pullDistance = pressPosition.x - currentPosition.x;
        float maxPullDistance = Screen.width * maxPullScreenRatio;
        isOverMaxPull = pullDistance > maxPullDistance;
        float rawPullRate = pullDistance / maxPullDistance;
        visualPullRate = Mathf.Clamp01(rawPullRate);
        if (isOverMaxPull)
        {
            power = 0f;
        } 
        else
        {
            power = rawPullRate;
        }
        Debug.Log(
            $"引っ張り量: {pullDistance:F0}px, " +
            $"割合: {power:P0}, " +
            $"上限超過: {isOverMaxPull}, "
        );
    }

    private void EndDrag(Vector2 position)
    {
        // 離した瞬間の位置でもう一度計算
        UpdateDrag(position);
        if (isOverMaxPull)
        {
            Debug.Log("上限を超えたためパワーは0");
        }
        else if (power < 0)
        {
            Debug.Log($"逆方向に引いたためパワーが負の値, パワー: {power:P0}");
        }
        else
        {
            Debug.Log($"ドラッグ終了位置：{position}, パワー: {power:P0}");
        }
        if (!isOverMaxPull && power > 0f) { hasReleasedWeapon = true; }
        isDragging = false;
    }
    
    public void ResetPullState()
    {
        isDragging = false;
        hasReleasedWeapon = false;
        pressPosition = Vector2.zero;
        currentPosition = Vector2.zero;
        pullDistance = 0f;
        power = 0f;
        isOverMaxPull = false;
        visualPullRate = 0f;
    }
}