using UnityEngine;

public class Spikes : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        var mortal = collision.GetComponent<IMortal>();

        if (mortal == null)
        {
            return;
        }

        mortal.Die();
    }
}
