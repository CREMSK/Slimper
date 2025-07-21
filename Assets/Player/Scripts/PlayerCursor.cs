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

        transform.position = new Vector3(transform.position.x, transform.position.y, -2);
    }
    private Vector2 getMouseWorldPos()
    {
        Vector3 mousePosition = Mouse.current.position.ReadValue();
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mousePosition);

        return mouseWorldPosition;
    }
}
