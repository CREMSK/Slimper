using System;
using UnityEngine;

public class LevelExitTrigger : MonoBehaviour
{
    public event Action onExitLevel;
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag != "Player")
        {
            return;
        }

        collision.GetComponent<Rigidbody2D>().gravityScale = 0;

        onExitLevel?.Invoke();
    }
}
