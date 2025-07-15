using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D playerRigidBody_;
    [SerializeField] private Animator playerAnimator_;
    [SerializeField] private PlayerMovementSystem playerMovementSystem_;
    [SerializeField] private PlayerAnimationSystem playerAnimationSystem_;
    private PlayerInputSystem playerInputSystem_;

    void Awake()
    {
        playerInputSystem_ = new PlayerInputSystem();

        if (playerAnimationSystem_ != null)
        {
            playerAnimationSystem_.PlayerAnimator = playerAnimator_;
            playerAnimationSystem_.PlayerTransform = transform;
        }
        
        if (playerMovementSystem_ != null)
        {
            playerMovementSystem_.PlayerRigidBody = playerRigidBody_;
        }
    }

    void OnEnable()
    {
        Subscribe();
    }

    void OnDisable()
    {
        Unsubscribe();        
    }

    private void Subscribe()
    {
        if (playerAnimationSystem_ != null)
        {
            playerMovementSystem_.onGroundedUpdate += playerAnimationSystem_.SetGrounded;
            playerMovementSystem_.onChargingUpdate += playerAnimationSystem_.SetCharging;
            playerMovementSystem_.onContactPointUpdate += playerAnimationSystem_.UpdateGroundedAngle;
            playerMovementSystem_.onPlayerJump += playerAnimationSystem_.Jump;
        }

        if (playerMovementSystem_ != null)
        {
            playerInputSystem_.onJumpStart += playerMovementSystem_.StartJump;
            playerInputSystem_.onJumpEnd += playerMovementSystem_.Jump;
        }
    }

    private void Unsubscribe()
    {
        if (playerAnimationSystem_ != null)
        {
            playerMovementSystem_.onGroundedUpdate -= playerAnimationSystem_.SetGrounded;
            playerMovementSystem_.onChargingUpdate -= playerAnimationSystem_.SetCharging;
            playerMovementSystem_.onContactPointUpdate -= playerAnimationSystem_.UpdateGroundedAngle;
            playerMovementSystem_.onPlayerJump -= playerAnimationSystem_.Jump;
        }

        if (playerMovementSystem_ != null)
        {
            playerInputSystem_.onJumpStart -= playerMovementSystem_.StartJump;
            playerInputSystem_.onJumpEnd -= playerMovementSystem_.Jump;
        }
    }
}
