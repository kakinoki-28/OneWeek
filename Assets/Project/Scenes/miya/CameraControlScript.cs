using UnityEngine;

public class CameraControlScript : MonoBehaviour
{
    [SerializeField] private Vector3 goalPosition = new Vector3(-22, 20, 10);
    [SerializeField] private Quaternion goalRotation = Quaternion.Euler(25, 115, 0);
    [SerializeField] private float RotateTime = 1.0f;

    private Vector3 currentVelocity;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        this.transform.position = Vector3.SmoothDamp(
            this.transform.position,
            goalPosition,
            ref currentVelocity,
            RotateTime
        );

        this.transform.rotation = Quaternion.Slerp(
            this.transform.rotation,
            goalRotation,
            1f-Mathf.Exp(-Time.deltaTime / RotateTime)
        );

    }
}
