using System.Collections;
using UnityEngine;

public class PlayerDeathSystem : MonoBehaviour
{
    public void Die()
    {
        var deathCoroutine = deathTimer(1.0f);
        StartCoroutine(deathCoroutine);
    }

    private IEnumerator deathTimer(float time)
    {
        while (time > 0)
        {
            time -= Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);
    }
}
