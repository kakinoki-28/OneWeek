using UnityEngine;

public class KeepHorizontal : MonoBehaviour
{
    [SerializeField]
    private Transform pendulumBody;

    private Quaternion originalWorldRotation;

    private void Awake()
    {
        originalWorldRotation = transform.rotation;
    }

    private void LateUpdate()
    {
        transform.localRotation =
            Quaternion.Inverse(pendulumBody.rotation)
            * originalWorldRotation;
    }
}