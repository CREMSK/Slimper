using System;
using System.Collections;
using Unity.Burst;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementSystem : MonoBehaviour
{
    // player components
    private Rigidbody2D playerRigidBody_;
    public Rigidbody2D PlayerRigidBody
    {
        private get{ return playerRigidBody_; }
        set
        {
            playerRigidBody_ = value;
        }
    }

    // variables
    private float rawCharge_;
    private bool _isGrounded;
    private bool _isCharging;
    private Vector2 contactPoint_;
    private float maxJumpAngle_ = 70f;
    private int minCharge_ = 4;
    private int maxCharge_ = 12;

    // properties
    private float charge_
    {
        get
        {
            return rawCharge_;
        }
        set
        {
            rawCharge_ = value;
            onChargeUpdate?.Invoke((int)math.round(value * (maxCharge_ - minCharge_)) + minCharge_);
        }
    }
    private bool isGrounded_
    {
        get { return _isGrounded; }
        set
        {
            _isGrounded = value;

            if (value)
            {
                playerRigidBody_.linearVelocity = new Vector2(0, 0);
                playerRigidBody_.gravityScale = 0;
            }
            else
            {
                playerRigidBody_.gravityScale = 2;
            }

            onGroundedUpdate?.Invoke(value);
        }
    }
    private bool isCharging_
    {
        get { return _isCharging; }
        set
        {
            _isCharging = value;
            onChargingUpdate?.Invoke(value);
        }
    }

    // events
    public event Action<int> onChargeUpdate;
    public event Action<bool> onGroundedUpdate;
    public event Action<bool> onChargingUpdate;
    public event Action<Vector2> onContactPointUpdate;
    public event Action<Vector2> onPlayerJump;

    // collision functions
    void OnCollisionEnter2D(Collision2D collision)
    {
        isGrounded_ = true;
    }
    void OnCollisionStay2D(Collision2D collision)
    {
        if (!isGrounded_)
        {
            return;
        }

        contactPoint_ = collision.GetContact(0).point;  
        onContactPointUpdate?.Invoke(getNormal());
    }
    void OnCollisionExit2D(Collision2D collision)
    {
        isGrounded_ = false;
        isCharging_ = false;
    }


    // JUMP FUNCTIONS
    public void StartJump()
    {
        isCharging_ = true;
        StartCoroutine("increaseCharge");
    }
    public void Jump(Vector2 mouseWorldPosition)
    {
        var charge = (int)math.round(charge_ * (maxCharge_- minCharge_)) + minCharge_;
        isCharging_ = false;

        StopCoroutine("increaseCharge");
        charge_ = 0;

        if (!isGrounded_)
        {
            return;
        }

        isGrounded_ = false;
        var jumpDirection = GetJumpDirection(mouseWorldPosition);
        playerRigidBody_.AddForce(jumpDirection * charge, ForceMode2D.Impulse);

        onPlayerJump?.Invoke(jumpDirection);
    }
    private IEnumerator increaseCharge()
    {
        while (isCharging_)
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

    private Vector2 GetDirectionToMouse(Vector2 mouseWorldPosition)
    {
        var directionToMouse = mouseWorldPosition - (Vector2)transform.position;

        return directionToMouse.normalized;
    }
    private Vector2 getNormal()
    {
        return ((Vector2)transform.position - contactPoint_).normalized;
    }
    private Vector2 GetJumpDirection(Vector2 mouseWorldPosition)
    {
        var directionToMouse = GetDirectionToMouse(mouseWorldPosition);

        var normal = getNormal();
        var tangent = new Vector2(-normal.y, normal.x);

        float up = Vector2.Dot(directionToMouse, normal);
        float along = Vector2.Dot(directionToMouse, tangent);

        float angleCos = up / Mathf.Sqrt(up * up + along * along);
        float limitCos = Mathf.Cos(maxJumpAngle_ * Mathf.Deg2Rad);

        if (angleCos >= limitCos)
        {
            return directionToMouse;
        }

        var clampedAlong = Mathf.Sin(maxJumpAngle_ * Mathf.Deg2Rad) * Mathf.Sign(along);

        var clampedDir = limitCos * normal + clampedAlong * tangent;
        return clampedDir.normalized;
    }
}
