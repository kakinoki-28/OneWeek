using UnityEngine;
using UnityEngine.InputSystem;



// 親のオブジェクトの位置を動かす
// 空のプロジェクトで物体のプロジェクトと同じ配下に置いて使用する
public class PullWeaponMotion : MonoBehaviour
{
    [SerializeField]
    [Min(0f)]
    private float tremblingWidth = 0.3f;
    // [SerializeField]
    // [Min(0f)]
    // private float maxMoveDistance = 1.0f;
    [SerializeField]
    [Min(0.1f)]
    private float tremblingFrequency = 5f;
    // private Vector2 currentRandomPoint;
    private float nextTremblingTime;
    private bool isTremblingUp = false;
    private MousePullTest pullInput;
    private Vector3 originalPosition;

    private void Awake()
    {
        originalPosition = transform.localPosition;
        pullInput = GetComponentInParent<MousePullTest>();
        if (pullInput == null)
        {
            Debug.LogError("MousePullTestが見つかりません");
            enabled = false;
        }
    }

    // マウス処理の後に位置変更を行いたいのでlate
    private void LateUpdate()
    {
        if (!pullInput.IsDragging)
        {
            transform.localPosition = originalPosition;
            return;
        }
        float pullRate = pullInput.VisualPullRate;
        // Vector3 pullOffset = Vector3.right * maxMoveDistance * pullRate;
        // 震えの追加
        if (Time.time >= nextTremblingTime)
        {
            isTremblingUp = !isTremblingUp;
            nextTremblingTime = Time.time + 1f / tremblingFrequency;
        }

        float tremblingY;
        if (isTremblingUp) { tremblingY = 1f; }
        else { tremblingY = -1f; }

        Vector3 tremblingOffset = new Vector3(0f, tremblingY, 0f) * tremblingWidth * pullRate;

        // transform.localPosition = originalPosition + pullOffset + tremblingOffset;
        transform.localPosition = originalPosition + tremblingOffset;
    }
}