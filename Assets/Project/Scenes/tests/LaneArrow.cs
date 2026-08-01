using UnityEngine;

public class LaneArrow : MonoBehaviour
{
    private WeaponLaneController laneController;
    private enum Direction { Left, Right }
    [SerializeField] private Direction direction;


    private void Awake()
    {
        laneController = GetComponentInParent<WeaponLaneController>();
        if (laneController == null)
        {
            Debug.LogError(
                "WeaponLaneControllerが見つかりません"
            );
            enabled = false;
        }
    }

    public void Activate()
    {
        if (direction == Direction.Left)
        {
            laneController.MoveLeft();
        }
        else
        {
            laneController.MoveRight();
        }
    }
}