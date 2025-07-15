using System.Collections;
using UnityEngine;

public class PlayerAnimationSystem : MonoBehaviour
{
    private Animator playerAnimator_;
    private Transform playerTransform_;
    
    public Animator PlayerAnimator
    {
        private get { return playerAnimator_; }
        set { playerAnimator_ = value; } 
    }
    public Transform PlayerTransform
    {
        private get { return playerTransform_; }
        set { playerTransform_ = value; } 
    }

    public void SetCharging(bool isCharging)
    {
        playerAnimator_.SetBool("isCharging", isCharging);
    }

    public void SetGrounded(bool isGrounded)
    {
        playerAnimator_.SetBool("isGrounded", isGrounded);
        StopCoroutine("keepJumpAngle");
    }

    public void UpdateGroundedAngle(Vector2 normal)
    {
        var angle = Mathf.Atan2(normal.y, normal.x) * Mathf.Rad2Deg;
        playerTransform_.rotation = Quaternion.Euler(0, 0, angle - 90);
    }

    public void Jump(Vector2 jumpDirection)
    {
        var angle = Mathf.Atan2(jumpDirection.y, jumpDirection.x) * Mathf.Rad2Deg;
        StartCoroutine("keepJumpAngle", angle);
    }

    private IEnumerator keepJumpAngle(float angle)
    {
        while (true)
        {   
            playerTransform_.rotation = Quaternion.Euler(0, 0, angle - 90);
            yield return null;
        }
    }
}
