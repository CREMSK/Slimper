using System.Collections;
using System.Xml;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class JumpController : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb2d;
    private float minForce_, charge_ = 2f;
    private float maxForce_ = 15f;
    private IEnumerator chargeRoutine_;

    void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    public void ChargeJump(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            StartCoroutine("increaseCharge");
            return;
        }
        if (context.canceled)
        {
            StopCoroutine("increaseCharge");
            rb2d.AddForce(Vector2.up * charge_, ForceMode2D.Impulse);
            charge_ = minForce_;
            return;
        }
    }
    private IEnumerator increaseCharge()
    {
        while (true)
        {
            charge_ = math.clamp(charge_ + Time.deltaTime * 5, minForce_, maxForce_);    
            print(charge_);
            yield return null;
        }
    }
}
