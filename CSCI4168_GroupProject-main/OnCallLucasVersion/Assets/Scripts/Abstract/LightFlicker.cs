using UnityEngine;

[RequireComponent(typeof(Light))]
public class LightFlicker : MonoBehaviour
{
    [Header("Light Settings")]
    [Tooltip("Assign a light if not using the one on this GameObject.")]
    [SerializeField] private Light lamp;
    
    [Tooltip("Percentage (0–1) of how much intensity will drop during flicker.")]
    [Range(0f, 1f)]
    [SerializeField] private float flickerDropPercent = 0.3f;

    [Tooltip("Speed of flicker variation (higher = faster).")]
    [SerializeField] private float flickerSpeed = 10f;

    [Tooltip("Chance (0–1) each frame to trigger a flicker when a tagged object is nearby.")]
    [Range(0f, 1f)]
    [SerializeField] private float flickerChance = 0.05f;

    [Header("Proximity Trigger")]
    [Tooltip("Radius around this object in which flickering may occur.")]
    [SerializeField] private float triggerRadius = 5f;

    [Tooltip("Tags that can trigger flickering when within radius.")]
    [SerializeField] private string[] triggerTags;

    private float baseIntensity;
    private bool isFlickering = false;

    void Start()
    {
        if (lamp == null)
            lamp = GetComponent<Light>();

        baseIntensity = lamp.intensity;
    }

    void Update()
    {
        // Check if any tagged object is nearby
        bool shouldFlicker = IsTriggerNearby();

        if (shouldFlicker && Random.value < flickerChance * Time.deltaTime * 60f)
        {
            if (!isFlickering)
                StartCoroutine(FlickerRoutine());
        }
    }

    private System.Collections.IEnumerator FlickerRoutine()
    {
        isFlickering = true;
        float elapsed = 0f;
        float flickerDuration = Random.Range(0.2f, 0.6f);

        while (elapsed < flickerDuration)
        {
            float noise = Mathf.PerlinNoise(Time.time * flickerSpeed, 0f);
            float drop = baseIntensity * flickerDropPercent;
            lamp.intensity = Mathf.Lerp(baseIntensity - drop, baseIntensity, noise);

            elapsed += Time.deltaTime;
            yield return null;
        }

        lamp.intensity = baseIntensity;
        isFlickering = false;
    }

    private bool IsTriggerNearby()
    {
        foreach (string tag in triggerTags)
        {
            GameObject[] objs = GameObject.FindGameObjectsWithTag(tag);
            foreach (var obj in objs)
            {
                if (Vector3.Distance(transform.position, obj.transform.position) <= triggerRadius)
                    return true;
            }
        }
        return false;
    }

    // Draw the trigger radius in Scene view
    void OnDrawGizmosSelected()
    {
        //Gizmos.color = new Color(1f, 0.8f, 0f, 0.3f);
        //Gizmos.DrawSphere(transform.position, triggerRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, triggerRadius);
    }
}
