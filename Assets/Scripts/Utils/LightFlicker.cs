using UnityEngine;

[RequireComponent(typeof(Light))]
public class LightFlicker : MonoBehaviour
{
    public float minIntensity = 1f;
    public float maxIntensity = 3f;
    public float flickerSpeed = 0.1f;

    private Light lightSource;

    private void Start()
    {
        lightSource = GetComponent<Light>();
    }

    private void Update()
    {
        // Smooth flicker using Perlin noise (natural feel)
        var noise = Mathf.PerlinNoise(Time.time * flickerSpeed, 0.0f);
        lightSource.intensity = Mathf.Lerp(minIntensity, maxIntensity, noise);
    }
}