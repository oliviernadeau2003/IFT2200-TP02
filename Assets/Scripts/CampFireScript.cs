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

    private Vector3 initialPosition;
    private Vector3 basePlateInitialPosition;

    private bool isReturning = false;
    private bool isOscillating = false;

    private Light campfireLight;
    private Light campfireLight2;

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
            float flicker = Mathf.PerlinNoise(Time.time * speed, 0f) * intensityVariation;
            campfireLight.intensity = Mathf.Clamp(maxLightIntensity - intensityVariation + flicker, 0f, maxLightIntensity);
            campfireLight2.intensity = Mathf.Clamp(maxLightIntensity - intensityVariation + flicker, 0f, maxLightIntensity);
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
            t += Time.deltaTime * 0.5f; // Speed of returning
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
