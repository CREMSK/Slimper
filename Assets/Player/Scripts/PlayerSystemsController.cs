using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D playerRigidBody_;
    [SerializeField] private Animator playerAnimator_;

    private PlayerMovementSystem playerMovementSystem_;
    private PlayerAnimationSystem playerAnimationSystem_;

    void Awake()
    {
        playerMovementSystem_ = GetComponent<PlayerMovementSystem>();
        playerAnimationSystem_ = GetComponent<PlayerAnimationSystem>();
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

    }

    private void Unsubscribe()
    {

    }
}
