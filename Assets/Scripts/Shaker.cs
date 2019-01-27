using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shaker : MonoBehaviour
{
    [SerializeField] Transform targetTransform;

    public float ShakeDuration { get { return shakeDuration;  } }

    private float shakeDuration = 0f;
    // Amplitude of the shake. A larger value shakes harder.
    private float shakeAmount = 0.7f;
    private float decreaseFactor = 1.0f;

    Vector3 originalPos;

    void OnEnable()
    {
        originalPos = targetTransform.localPosition;
    }

    void Update()
    {
        if (shakeDuration > 0)
        {
            Vector2 randomVector2 = Random.insideUnitCircle;
            Vector3 randomVector3 = new Vector3(randomVector2.x, randomVector2.y, 0);
            targetTransform.localPosition = originalPos + randomVector3 * shakeAmount;

            shakeDuration -= Time.deltaTime * decreaseFactor;
        }
        else
        {
            shakeDuration = 0f;
            targetTransform.localPosition = originalPos;
        }
    }

    public void Shake(float duration, float amount)
    {
        shakeDuration = duration;
        shakeAmount = amount;
    }
}
