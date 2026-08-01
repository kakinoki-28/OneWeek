using UnityEngine;

public class BatteringRamResetController : MonoBehaviour
{
    private MousePullTest pullInput;
    private PendulumPullController pendulumController;
    private WeaponLaneController laneController;

    private void Awake()
    {
        pullInput = GetComponent<MousePullTest>();
        pendulumController = GetComponentInChildren<PendulumPullController>();
        laneController = GetComponentInChildren<WeaponLaneController>();

        if (pullInput == null)
        {
            Debug.LogError("MousePullTestが見つかりません");
            enabled = false;
            return;
        }

        if (pendulumController == null)
        {
            Debug.LogError(
                "PendulumPullControllerが見つかりません"
            );
            enabled = false;
            return;
        }

        if (laneController == null)
        {
            Debug.LogError(
                "WeaponLaneControllerが見つかりません"
            );
            enabled = false;
            return;
        }
    }

    [ContextMenu("Reset Weapon")]
    public void ResetWeapon()
    {
        pullInput.ResetPullState();
        pendulumController.ResetPendulum();
        laneController.ShowArrows();

        Debug.Log("破城槌をリセットしました");
    }
}