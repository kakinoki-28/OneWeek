using UnityEngine;
using UnityEngine.InputSystem;

public class MouseInputTest : MonoBehaviour
{
    public void Update()
    {
        Mouse mouse = Mouse.current;
        if (mouse == null)
        {
            Debug.LogError("Mouse not found");
            return;
        }
        if (mouse.leftButton.wasPressedThisFrame)
        {
            Vector2 position = mouse.position.ReadValue();

            Debug.Log($"マウスを押した位置: {position}");
        }
        if (mouse.leftButton.wasReleasedThisFrame)
        {
            Vector2 position = mouse.position.ReadValue();

            Debug.Log($"マウスを離した位置: {position}");
        }
    }
}