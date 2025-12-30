using UnityEngine;

public class Spikes : MonoBehaviour
{
    [SerializeField] private Collider2D spikesCollider_;

    void OnTriggerEnter2D(Collider2D collision)
    {
        var mortal = collision.GetComponent<IMortal>();

        if (mortal == null)
        {
            return;
        }

        mortal.Die(spikesCollider_.ClosestPoint(collision.transform.position));
    }
}
