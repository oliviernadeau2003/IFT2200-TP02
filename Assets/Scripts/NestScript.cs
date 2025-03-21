using UnityEngine;
using System.Collections;

public class NestScript : MonoBehaviour
{
    public float minShakeAmount = 0.05f;   // Minimum shaking intensity
    public float maxShakeAmount = 0.2f;    // Maximum shaking intensity
    public float shakeSpeed = 1.5f;        // How fast the nest shakes
    public float lerpSpeed = 1f;           // Speed of intensity changes
    public float shakePauseTime = 4f;      // How long to pause before shaking again

     Vector3 initialPosition;
    float currentShakeAmount;
    float targetShakeAmount;

    void Start()
    {
        initialPosition = transform.position;
        currentShakeAmount = minShakeAmount;
        targetShakeAmount = maxShakeAmount;

        StartCoroutine(ShakeWithPause());
    }

    void Update()
    {
        currentShakeAmount = Mathf.Lerp(currentShakeAmount, targetShakeAmount, Time.deltaTime * lerpSpeed);

        float shakeX = (Mathf.PerlinNoise(Time.time * shakeSpeed, 0f) - 0.5f) * currentShakeAmount;
        float shakeY = (Mathf.PerlinNoise(0f, Time.time * shakeSpeed) - 0.5f) * currentShakeAmount;

        transform.position = initialPosition + new Vector3(shakeX, shakeY, 0);
    }

    IEnumerator ShakeWithPause()
    {
        while (true)
        {
            targetShakeAmount = (targetShakeAmount == maxShakeAmount) ? minShakeAmount : maxShakeAmount;

            yield return new WaitForSeconds(shakePauseTime);

            shakePauseTime = Random.Range(0.5f, 1.5f); 
        }
    }
}
