using System.Diagnostics;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerCursor : MonoBehaviour
{
    void Start()
    {
        Cursor.visible = false;
    }
    void Update()
    {
        transform.position = getMouseWorldPos();
    }
    private Vector2 getMouseWorldPos()
    {
        Vector3 mousePosition = Mouse.current.position.ReadValue();
        Vector2 mouseWorldPosition = (Vector2)Camera.main.ScreenToWorldPoint(mousePosition);

        return mouseWorldPosition;
    }
}
