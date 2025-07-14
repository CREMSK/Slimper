using System.Collections;
using UnityEngine;

public class PlayerAnimationSystem : MonoBehaviour
{
    [SerializeField] private Animator animator_;
    [SerializeField] private SpriteRenderer spriteRenderer_;
    [SerializeField] private Rigidbody2D rigidbody_;

    private bool isGrounded_ = false;

    public void Land(Vector2 velocity, Vector2 normal)
    {
        isGrounded_ = true;
        animator_.SetBool("isGrounded", true);
        animator_.SetFloat("velocity", velocity.magnitude);
        var angle = Mathf.Atan2(normal.y, normal.x) * Mathf.Rad2Deg;

        spriteRenderer_.transform.rotation = Quaternion.Euler(0, 0, angle - 90);
    }

    public void Hop(Vector2 dir)
    {
        isGrounded_ = true;
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        spriteRenderer_.transform.rotation = Quaternion.Euler(0, 0, angle - 90);

        animator_.SetBool("isCharging", false);
        animator_.SetBool("isGrounded", false);
    }

    public void Charge()
    {
        animator_.SetBool("isCharging", true);
    }

    public void Jump(Vector2 dir, Vector2 vel)
    {
        isGrounded_ = false;
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        spriteRenderer_.transform.rotation = Quaternion.Euler(0, 0, angle - 90);

        animator_.SetBool("isCharging", false);
        animator_.SetBool("isGrounded", false);
    }

    public void Ground(Vector2 normal)
    {
        var angle = Mathf.Atan2(normal.y, normal.x) * Mathf.Rad2Deg;
        spriteRenderer_.transform.rotation = Quaternion.Euler(0, 0, angle - 90);
    }
}
