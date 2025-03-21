using UnityEngine;
using System.Collections;

public class CampFireScript : MonoBehaviour
{
    public GameObject basePlate;
    public float speed = 5f;
    public float oscillationAmount = 4f;
    public float lightIncreaseSpeed = 2f;
    public float maxLightIntensity = 38f;
    public float intensityVariation = 30f;
    public Color minColor = new(1f, 0.4f, 0f); // Deep orange
    public Color maxColor = new(1f, 1f, 0.6f); // Yellowish

    Vector3 initialPosition;
    Vector3 basePlateInitialPosition;

    bool isReturning = false;
    bool isOscillating = false;

    Light campfireLight;
    Light campfireLight2;

    void Start()
    {
        initialPosition = transform.position;

        campfireLight = GameObject.Find("/POI/Feu de camp/Point Light").GetComponent<Light>();
        campfireLight2 = GameObject.Find("/POI/Feu de camp/Point Light 2").GetComponent<Light>();

        basePlateInitialPosition = basePlate.transform.position;

        campfireLight.intensity = 0f;
        campfireLight2.intensity = 0f;
        campfireLight.enabled = false;
        campfireLight2.enabled = false;

        transform.position = new Vector3(initialPosition.x, 0.5f, initialPosition.z);

        basePlate.transform.position = basePlateInitialPosition;
    }

    void Update()
    {
        if (isOscillating)
        {
            // Flickering effect using Perlin Noise
            float flicker = Mathf.PerlinNoise(Time.time * speed, 0f) * intensityVariation;
            float intensity = Mathf.Clamp(maxLightIntensity - intensityVariation + flicker, 0f, maxLightIntensity);

            campfireLight.intensity = intensity;
            campfireLight2.intensity = intensity;

            // Smooth color transition between minColor and maxColor
            float colorT = Mathf.PerlinNoise(Time.time * speed * 0.5f, 1f);
            Color newColor = Color.Lerp(minColor, maxColor, colorT);

            campfireLight.color = newColor;
            campfireLight2.color = newColor;
        }
    }

    public void OnBasePlateClicked()
    {
        if (!isReturning && !isOscillating)
        {
            StartCoroutine(ReturnToInitialPosition());
        }
    }

    IEnumerator ReturnToInitialPosition()
    {
        isReturning = true;
        float t = 0f;
        Vector3 startPos = transform.position;

        while (t < 1f)
        {
            t += Time.deltaTime * 0.5f;
            transform.position = Vector3.Lerp(startPos, initialPosition, t);
            yield return null;
        }

        // Activate fire light and start intensity increase
        basePlate.SetActive(false);
        campfireLight.enabled = true;
        campfireLight2.enabled = true;
        isOscillating = true;
        isReturning = false;

        StartCoroutine(IncreaseLightIntensity());
    }

    IEnumerator IncreaseLightIntensity()
    {
        float intensity = 0f;
        while (intensity < maxLightIntensity)
        {
            intensity += Time.deltaTime * lightIncreaseSpeed;
            campfireLight.intensity = Mathf.Clamp(intensity, 0f, maxLightIntensity);
            campfireLight2.intensity = Mathf.Clamp(intensity, 0f, maxLightIntensity);
            yield return null;
        }
    }
}
