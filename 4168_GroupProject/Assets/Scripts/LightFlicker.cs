using UnityEngine;

public class LightFlicker : MonoBehaviour
{
    public Light lamp;
    public float minIntensity = 1f;
    public float maxIntensity = 2f;
    public float flickerSpeed = 10f;

    void Update()
    {
        float noise = Mathf.PerlinNoise(Time.time * flickerSpeed, 0f);
        lamp.intensity = Mathf.Lerp(minIntensity, maxIntensity, noise);
    }
}
