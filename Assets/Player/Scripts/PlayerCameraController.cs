using System.Collections;
using UnityEngine;

public class PlayerCameraController : MonoBehaviour
{
    public Transform playerTransform_;

    [SerializeField] private SpriteRenderer blackBox_;
    [SerializeField] private float xMax_ = 0;
    [SerializeField] private float xMin_ = 0;
    [SerializeField] private float yMax_ = 0;
    [SerializeField] private float yMin_ = 0;

    void Start()
    {
        FadeOut();
    }
    void FixedUpdate()
    {
        calculateNewPosition();
    }

    private void calculateNewPosition()
    {
        var newPos = Vector3.Lerp(transform.position, playerTransform_.position, 0.1f);
        newPos = new Vector3(Mathf.Clamp(newPos.x, xMin_, xMax_), Mathf.Clamp(newPos.y, yMin_, yMax_), -10);

        transform.position = newPos;
    }

    public void FadeIn()
    {
        var fadeIn = boxFadeIn();
        StartCoroutine(fadeIn);
    }

    public void FadeOut()
    {
        var fadeOut = boxFadeOut();
        StartCoroutine(fadeOut);
    }
    private IEnumerator boxFadeOut()
    {
        while (blackBox_.color.a > 0)
        {
            blackBox_.color = new Color(0, 0, 0, Mathf.Clamp01(blackBox_.color.a - Time.deltaTime * 1f));
            yield return null;
        }
    }

    private IEnumerator boxFadeIn()
    {
        while (blackBox_.color.a < 1)
        {
            blackBox_.color = new Color(0, 0, 0, Mathf.Clamp01(blackBox_.color.a + Time.deltaTime * 1f));
            yield return null;
        }
    }

}
