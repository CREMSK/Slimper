using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Transform contactIndicator;

    private Rigidbody2D rigidBody_;
    private float charge_ = 0f;
    private bool isGrounded_ = false;
    private Vector2 contactPoint_;
    private float maxJumpAngle_ = 70f;

    // UNITY FUNCTIONS
    void Awake()
    {
        rigidBody_ = GetComponent<Rigidbody2D>();
    }
        void OnCollisionEnter2D(Collision2D collision)
    {
        isGrounded_ = true;

        rigidBody_.linearVelocity = new Vector2(0, 0);
        rigidBody_.gravityScale = 0;
    }
    void OnCollisionStay2D(Collision2D collision)
    {
        contactPoint_ = collision.GetContact(0).point;

        if (contactIndicator != null)
        {
            contactIndicator.transform.position = contactPoint_;
        }
    }
    void OnCollisionExit2D(Collision2D collision)
    {
        isGrounded_ = false;
        rigidBody_.gravityScale = 2;
    }

    // JUMP FUNCTIONS
    public void StartJump(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            StartCoroutine("increaseCharge");
            return;
        }
        if (context.canceled)
        {
            StopCoroutine("increaseCharge");

            rigidBody_.simulated = true;
            Jump(charge_);

            charge_ = 0;
            return;
        }
    }
        private void Jump(float charge)
    {
        var jumpVec = GetJumpAngle();
        charge = math.round(charge * 10) + 2;

        rigidBody_.AddForce(jumpVec * charge, ForceMode2D.Impulse);
    }
    private IEnumerator increaseCharge()
    {
        while (true)
        {
            if (isGrounded_)
            {
                charge_ = math.clamp(charge_ + Time.deltaTime * 2, 0, 1);
            }
            yield return null;
        }
    }

    // HELPER FUNCTIONS
    private Vector2 GetMouseVec()
    {
        Vector3 mousePos = Mouse.current.position.ReadValue();
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(mousePos);
        Vector2 worldmpos = new Vector2(mouseWorldPos.x, mouseWorldPos.y);

        Vector2 mvec = worldmpos - (Vector2)transform.position;
        return mvec.normalized;
    }
    private Vector2 GetJumpAngle()
    {
        Vector2 normal = ((Vector2)transform.position - contactPoint_).normalized;
        Vector2 tangent = new Vector2(-normal.y, normal.x);
        Vector2 mvec = GetMouseVec();

        float up = Vector2.Dot(mvec, normal);
        float along = Vector2.Dot(mvec, tangent);

        float angleCos = up / Mathf.Sqrt(up * up + along * along);
        float limitCos = Mathf.Cos(maxJumpAngle_ * Mathf.Deg2Rad);

        if (angleCos >= limitCos)
        {
            return mvec;
        }

        float sign = Mathf.Sign(along);
        float clampedUp = limitCos;
        float clampedAlong = Mathf.Sin(maxJumpAngle_ * Mathf.Deg2Rad) * sign;

        Vector2 clampedDir = clampedUp * normal + clampedAlong * tangent;
        return clampedDir.normalized;
    }




}
