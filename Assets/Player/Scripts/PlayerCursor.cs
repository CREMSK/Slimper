using System.Diagnostics;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerCursor : MonoBehaviour
{
    void Update()
    {
        Vector3 mousePos = Mouse.current.position.ReadValue();
        mousePos.z = Camera.main.nearClipPlane;
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(mousePos);

        transform.position = mouseWorldPos;
    }
}
