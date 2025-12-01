using UnityEngine;
using TMPro;

public class TMP_ExitColor : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TextMeshPro textMesh;
    [SerializeField] private Light pointLight; 

    [Header("Toggle Settings")]
    [SerializeField] private Color colorOn = Color.red;
    [SerializeField] private Color colorOff = Color.gray;
    [SerializeField] private bool startOn = true;

    private bool isOn;

    void Start()
    {
        if (textMesh == null)
            textMesh = GetComponent<TextMeshPro>();

        isOn = startOn;
        textMesh.color = isOn ? colorOn : colorOff;
        
        // Set initial light state
        if (pointLight != null)
            pointLight.enabled = isOn;
    }

    // Toggles both text color and light, call this from other scripts/triggers
    public void ToggleColor()
    {
        isOn = !isOn;
        UpdateVisuals();
    }

    // Explicitly sets state, use this call this from other scripts/triggers
    public void SetColorState(bool state)
    {
        isOn = state;
        UpdateVisuals();
    }

    // Updates both text and light at once
    private void UpdateVisuals()
    {
        // Update text color immediately
        if (textMesh != null)
            textMesh.color = isOn ? colorOn : colorOff;
        
        // Update light immediately
        if (pointLight != null)
            pointLight.enabled = isOn;
    }

    // Get current state if needed
    public bool GetCurrentState()
    {
        return isOn;
    }
}