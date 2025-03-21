using UnityEngine;

public class TorchScript : MonoBehaviour
{
    public float speed = 2.5f;
    public float intensityVariation = 3.2f;
    public float maxLightIntensity = 10f;

    public Color minColor = new(1f, 0.4f, 0f); 
    public Color maxColor = new(1f, 1f, 0.6f); 

    private Light torchLight;

    void Start()
    {
        torchLight = GetComponentInChildren<Light>();

        torchLight.intensity = maxLightIntensity;
    }

    void Update()
    {
        if (torchLight != null)
        {
            // Flickering effect using Perlin Noise
            float flicker = Mathf.PerlinNoise(Time.time * speed, 0f) * intensityVariation;
            torchLight.intensity = Mathf.Clamp(maxLightIntensity - intensityVariation + flicker, 0f, maxLightIntensity);

            // Smooth color transition between minColor and maxColor
            float colorT = Mathf.PerlinNoise(Time.time * speed * 0.5f, 1f);
            torchLight.color = Color.Lerp(minColor, maxColor, colorT);
        }
    }
}
