using UnityEngine;

public class PendulumPullController : MonoBehaviour
{
    private Rigidbody pendulumBody;
    private MousePullTest pullInput;
    private bool wasDragging;
    [SerializeField] private Transform pivotAnchor;
    private float pendulumLength;
    [SerializeField] private float velocity0 = 50f;
    [SerializeField] private float maxMoveDistance = 2.0f;
    // private Quaternion originalRotation;
    private Vector3 originalPosition;
    private Quaternion originalRotation;
    // [SerializeField][Min(0f)] private float tremblingWidth = 0.3f;
    // [SerializeField][Min(0.1f)] private float tremblingFrequency = 5f;
    private float nextTremblingTime;
    private Vector2 currentRandomPoint;

    private void Awake()
    {
        pendulumBody = GetComponent<Rigidbody>();
        if (pendulumBody == null)
        {
            Debug.LogError("Rigidbodyが見つかりません");
            enabled = false;
            return;
        }
        // 最初は物理挙動を制限
        pendulumBody.isKinematic = true;

        pullInput = GetComponentInParent<MousePullTest>();
        if (pullInput == null)
        {
            Debug.LogError("MousePullTestが見つかりません");
            enabled = false;
            return;
        }
        if (pivotAnchor == null)
        {
            Debug.LogError("PivotAnchorが設定されていません");
            enabled = false;
            return;
        }
        // 紐の長さ
        pendulumLength = pivotAnchor.position.y - pendulumBody.position.y;
        // originalPosition = pendulumBody.position;
        originalPosition = transform.localPosition;
    }

    private void LateUpdate()
    {
        // transform.rotation = originalRotation;
        bool isDragging = pullInput.IsDragging;
        if (isDragging && !wasDragging)
        {
            BeginPull();
        }
        if (isDragging)
        {
            UpdatePull();
        }
        if (wasDragging && !isDragging)
        {
            EndPull();
        }
        wasDragging = isDragging;
    }

    private void BeginPull()
    {
        Debug.Log("引っ張り開始");
        pendulumBody.isKinematic = true;
        // 現在いるレーンのZ座標を保持
        originalPosition.z = transform.localPosition.z;
    }

    private void EndPull()
    {
        Debug.Log($"引っ張り終了 Power: {pullInput.Power:P0}");
        float power = pullInput.Power;
        if (power <= 0f)
        {
            Debug.Log("ひっぱりキャンセル");
            // transform.position = originalPosition;
            transform.localPosition = originalPosition;
            return;
        }
        pendulumBody.isKinematic = false;
        pendulumBody.linearVelocity = new Vector3(-velocity0 * power, 0f, 0f);
        // pendulumBody.AddForce(new Vector3(-velocity0 * power, 0f, 0f), ForceMode.VelocityChange);
    }

    private void UpdatePull()
    {
        float pullRate = pullInput.VisualPullRate;
        float moveX = maxMoveDistance * pullRate;
        float moveY = pendulumLength - Mathf.Sqrt(pendulumLength * pendulumLength - moveX * moveX);
        Vector3 pullOffset = new Vector3(moveX, moveY, 0f);

        // 震えの追加
        // if (Time.time >= nextTremblingTime)
        // {
        //     currentRandomPoint = Random.insideUnitCircle;

        //     nextTremblingTime = Time.time + 1f / tremblingFrequency;
        // }

        // Vector3 tremblingOffset = new Vector3(0f, currentRandomPoint.y, 0f) * tremblingWidth * pullRate;
        // transform.position = originalPosition + pullOffset;
        transform.localPosition = originalPosition + pullOffset;
        // transform.position = originalPosition + pullOffset + tremblingOffset;
    }

    public void ResetPendulum()
    {
        // 速度を変更できる状態にする
        pendulumBody.isKinematic = false;
        // 振り子の移動と回転を停止
        pendulumBody.linearVelocity = Vector3.zero;
        pendulumBody.angularVelocity = Vector3.zero;
        // 初期位置と初期角度へ戻す
        transform.localPosition = originalPosition;
        transform.localRotation = originalRotation;
        // 次のドラッグまで物理演算を止める
        pendulumBody.isKinematic = true;
        wasDragging = false;
    }
}