using UnityEngine;
using System.Collections;

public class NestScript : MonoBehaviour
{
    public float minShakeAmount = 0.05f;   // Minimum shaking intensity
    public float maxShakeAmount = 0.2f;    // Maximum shaking intensity
    public float shakeSpeed = 1.5f;        // How fast the nest shakes
    public float lerpSpeed = 1f;           // Speed of intensity changes
    public float shakePauseTime = 4f;      // How long to pause before shaking again

    public float liftSpeed = 0.2f;        // Slower speed at which the nest rises (reduced for slower lift)
    public float rotationSpeed = 50f;      // Slower rotation speed (reduced for slower rotation)
    public float disappearanceTime = 1.2f; // Time it takes for the nest to disappear

    Vector3 initialPosition;
    float currentShakeAmount;
    float targetShakeAmount;
    bool isLifted = false;

    void Start()
    {
        initialPosition = transform.position;
        currentShakeAmount = minShakeAmount;
        targetShakeAmount = maxShakeAmount;

        StartCoroutine(ShakeWithPause());
    }

    void Update()
    {
        if (!isLifted) 
        {
            currentShakeAmount = Mathf.Lerp(currentShakeAmount, targetShakeAmount, Time.deltaTime * lerpSpeed);

            float shakeX = (Mathf.PerlinNoise(Time.time * shakeSpeed, 0f) - 0.5f) * currentShakeAmount;
            float shakeY = (Mathf.PerlinNoise(0f, Time.time * shakeSpeed) - 0.5f) * currentShakeAmount;

            transform.position = initialPosition + new Vector3(shakeX, shakeY, 0);
        }
    }

    IEnumerator ShakeWithPause()
    {
        while (!isLifted)
        {
            targetShakeAmount = (targetShakeAmount == maxShakeAmount) ? minShakeAmount : maxShakeAmount;

            yield return new WaitForSeconds(shakePauseTime);

            shakePauseTime = Random.Range(0.5f, 1.5f);
        }
    }

    void OnMouseDown()
    {
        if (!isLifted)
        {
            isLifted = true;
            StartCoroutine(LiftAndRotate());
        }
    }

    IEnumerator LiftAndRotate()
    {
        float t = 0f;
        Vector3 startPosition = transform.position;
        Quaternion startRotation = transform.rotation;

        StopCoroutine(ShakeWithPause());

        while (t < 1f)
        {
            t += Time.deltaTime * liftSpeed;

            transform.position = Vector3.Lerp(startPosition, startPosition + new Vector3(0, 2f, 0), t);

            transform.rotation = Quaternion.Lerp(startRotation, startRotation * Quaternion.Euler(0, 0, rotationSpeed * Time.deltaTime), t);

            yield return null;
        }

        yield return new WaitUntil(() => transform.position.y >= startPosition.y + 2f);

        yield return new WaitForSeconds(disappearanceTime);

        gameObject.SetActive(false);
    }
}
