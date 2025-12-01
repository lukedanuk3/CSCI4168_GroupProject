using UnityEngine;

public class EmissionSync : MonoBehaviour
{
    public Light lamp;
    public Renderer lampRenderer;
    public Color baseEmissionColor = Color.white;

    void Update()
    {
        float strength = lamp.intensity;
        lampRenderer.material.SetColor("_EmissionColor", baseEmissionColor * strength);
    }
}
