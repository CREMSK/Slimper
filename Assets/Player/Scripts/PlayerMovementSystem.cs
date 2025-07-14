using System;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementSystem : MonoBehaviour
{
    // player components
    private Rigidbody2D playerRigidBody_;
    private Transform playerTransform;

    // used properties
    private float charge_
    {
        get{ return charge_; }
        set
        {
            charge_ = value;
            onChargeUpdate?.Invoke((int)math.round(value * 10) + 2);
        } 
    }
    private bool isGrounded_{
        get{ return isGrounded_; }
        set
        {
            isGrounded_ = value;
            onGroundedUpdate?.Invoke(value);
        } 
    }
    private Vector2 contactPoint_;

    // setting properties
    private float maxJumpAngle_ = 70f;

    // events
    public event Action<int> onChargeUpdate;
    public event Action<bool> onGroundedUpdate;



    // collision functions
    void OnCollisionEnter2D(Collision2D collision)
    {   
        isGrounded_ = true;

        playerRigidBody_.linearVelocity = new Vector2(0, 0);
        playerRigidBody_.gravityScale = 0;
    }
    void OnCollisionStay2D(Collision2D collision)
    {
        contactPoint_ = collision.GetContact(0).point;
    }
    void OnCollisionExit2D(Collision2D collision)
    {
        isGrounded_ = false;
        playerRigidBody_.gravityScale = 2;
    }

    // JUMP FUNCTIONS
    public void StartCharge(InputAction.CallbackContext context)
    {
        StartCoroutine("increaseCharge");
    }
    public void Jump(InputAction.CallbackContext context)
    {   
        StopCoroutine("increaseCharge");
        var charge = math.round(charge_ * 10) + 2;
        charge_ = 0;

        if (!isGrounded_)
        {
            return;
        }
        var jumpVec = GetJumpAngle();
        playerRigidBody_.AddForce(jumpVec * charge, ForceMode2D.Impulse);
    }
    private IEnumerator increaseCharge()
    {
        while (true)
        {
            if (!isGrounded_)
            {
                yield return null;
            }

            charge_ = math.clamp(charge_ + Time.deltaTime * 2, 0, 1);
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
