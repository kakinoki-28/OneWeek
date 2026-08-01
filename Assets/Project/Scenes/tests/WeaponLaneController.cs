using UnityEngine;

public class WeaponLaneController : MonoBehaviour
{
    [SerializeField][Min(0.1f)] private float laneSpacing = 3f;
    [SerializeField] private Rigidbody pendulumBody;

    private int currentLane = 0;
    private float centerZ;
    [SerializeField] private Rigidbody pivotAnchor;
    [SerializeField] private GameObject leftArrow;
    [SerializeField] private GameObject rightArrow;
    private bool arrowsEnabled = true;
    private MousePullTest pullInput;

    public int CurrentLane => currentLane;

    private PlaySEPlayer SEPlayer;

    private void Awake()
    {
        centerZ = transform.position.z;
        if (pendulumBody == null)
        {
            Debug.LogError("PendulumBodyが設定されていません");
            enabled = false;
        }
        pullInput = GetComponentInParent<MousePullTest>();
        if (pullInput == null)
        {
            Debug.LogError("MousePullTestが見つかりません");
            enabled = false;
            return;
        }
        if(SEPlayer == null)
        {
            SEPlayer = GetComponent<PlaySEPlayer>();
            if(SEPlayer == null)
            {
                Debug.LogError("PlaySEPlayerが見つかりません");
                enabled = false;
                return;
            }
        }
        UpdateArrowVisibility();
    }
    public void MoveLeft()
    {
        MoveLane(-1);
    }

    public void MoveRight()
    {
        MoveLane(1);
    }

    private void MoveLane(int direction)
    {
        int nextLane = Mathf.Clamp(currentLane + direction, -1, 1);
        if (nextLane == currentLane) { return; }

        // SEの再生
        SEPlayer.PlaySE();

        float nextZ = centerZ + nextLane * laneSpacing;
        float moveZ = nextZ - transform.position.z;
        Vector3 moveOffset = new Vector3(0f, 0f, moveZ);
        // 親を動かす前の物理オブジェクトの座標を保存
        Vector3 pendulumPosition = pendulumBody.position;
        Vector3 pivotPosition = pivotAnchor.position;
        currentLane = nextLane;
        Vector3 nextPosition = transform.position;
        nextPosition.z = nextZ;
        transform.position = nextPosition;
        // 支点と質点を同じ距離だけ移動
        pendulumBody.position = pendulumPosition + moveOffset;
        pivotAnchor.position = pivotPosition + moveOffset;
        Debug.Log(
            $"レーン移動: {currentLane}, Z: {nextPosition.z}"
        );
        UpdateArrowVisibility();
    }
    private void UpdateArrowVisibility()
    {
        leftArrow.SetActive(arrowsEnabled && currentLane > -1);
        rightArrow.SetActive(arrowsEnabled && currentLane < 1);
    }
    public void HideArrows()
    {
        arrowsEnabled = false;
        UpdateArrowVisibility();
    }
    public void ShowArrows()
    {
        arrowsEnabled = true;
        UpdateArrowVisibility();
    }
    private void LateUpdate()
    {
        if (arrowsEnabled && pullInput.HasReleasedWeapon) { HideArrows(); }
    }
}