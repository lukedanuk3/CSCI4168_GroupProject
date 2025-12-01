using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(AudioLowPassFilter))]
public class ProximityAudioEmitter : MonoBehaviour
{
    [Header("Detection Settings")]
    public string detectionTag = "Player"; // Tag to detect (e.g., Player)
    public float detectionRadius = 6f; // Radius within which sound may trigger

    [Header("Directional Audio Cone")]
    [Range(0f, 180f)] public float innerConeAngle = 30f; // Full volume zone
    [Range(0f, 180f)] public float outerConeAngle = 90f; // Beyond this angle sound fades
    [Range(0f, 1f)] public float outsideConeVolumeMultiplier = 0.25f; // Volume outside cone

    [Header("Audio Settings")]
    public AudioClip audioClip; // The sound clip to play
    public bool playOncePerEntry = false; // Whether to only play once when entering range
    public float playbackCooldown = 1f; // Cooldown between playbacks

    [Header("Distance Low-Pass Filter")]
    public bool enableLowPassFilter = true; // Toggle low-pass filter based on distance
    [Range(500f, 22000f)] public float minCutoffFrequency = 1000f; // Max distance muffled sound
    [Range(500f, 22000f)] public float maxCutoffFrequency = 22000f; // Near sound clarity

    [Header("2D Fallback")]
    public bool enable2DFallback = true; // Toggle for near sound 2D blend
    public float twoDThreshold = 0.8f; // Distance ratio to start 2D blending

    private AudioSource audioSource; // Main audio source reference
    private AudioLowPassFilter lowPass; // Reference to the low-pass filter
    private bool hasPlayed = false; // Track if sound has been played already
    private float cooldownTimer = 0f; // Timer between allowed playbacks

    void Awake()
    {
        // Initialize required components
        audioSource = GetComponent<AudioSource>();
        lowPass = GetComponent<AudioLowPassFilter>();

        // Configure default 3D sound properties
        audioSource.spatialBlend = 1f; // 1 = full 3D sound
        audioSource.rolloffMode = AudioRolloffMode.Logarithmic; // Natural fade curve
        audioSource.playOnAwake = false;
        audioSource.loop = false;
        audioSource.minDistance = 1f; // Start of max volume
        audioSource.maxDistance = detectionRadius; // End of audible range

        // Apply initial filter toggle
        lowPass.enabled = enableLowPassFilter;
    }

    void Update()
    {
        // Locate the target by tag (e.g., Player)
        GameObject target = GameObject.FindGameObjectWithTag(detectionTag);
        if (!target) return;

        // Measure distance between emitter and target
        float dist = Vector3.Distance(transform.position, target.transform.position);

        // ---------------------- 3D/2D BLEND ----------------------
        if (enable2DFallback)
        {
            // Calculate fade threshold (near sound becomes 2D)
            float fadeThreshold = audioSource.minDistance * twoDThreshold;
            // Lerp between 3D and 2D blend depending on distance
            audioSource.spatialBlend = Mathf.Clamp01(Mathf.InverseLerp(fadeThreshold, audioSource.minDistance, dist));
        }

        // ---------------------- LOW-PASS FILTER ----------------------
        if (enableLowPassFilter)
        {
            // Normalize distance between 0 (close) and 1 (far)
            float t = Mathf.InverseLerp(audioSource.minDistance, detectionRadius, dist);
            // Interpolate cutoff frequency — closer = clearer sound, farther = muffled
            float cutoff = Mathf.Lerp(maxCutoffFrequency, minCutoffFrequency, t);
            lowPass.cutoffFrequency = cutoff;
        }

        // ---------------------- SOUND PLAYBACK ----------------------
        if (dist <= detectionRadius)
        {
            tryPlayDirectionalSound(target.transform); // Try play if within range
        }
        else
        {
            hasPlayed = false; // Reset state when leaving range
        }

        // Decrease cooldown over time
        if (cooldownTimer > 0f)
            cooldownTimer -= Time.deltaTime;
    }

    // Attempts to play a sound if conditions are met
    private void tryPlayDirectionalSound(Transform target)
    {
        if (audioClip == null) return; // Skip if no clip
        if (playOncePerEntry && hasPlayed) return; // Skip if already played once
        if (cooldownTimer > 0f) return; // Skip if still cooling down

        // Compute direction and angle between light forward vector and listener
        Vector3 dirToListener = (target.position - transform.position).normalized;
        float angle = Vector3.Angle(transform.forward, dirToListener);

        float volumeMultiplier = 1f; // Default full volume

        // Reduce volume if outside the inner cone
        if (angle > innerConeAngle)
        {
            if (angle > outerConeAngle)
                volumeMultiplier = outsideConeVolumeMultiplier; // Fully outside cone
            else
            {
                // Smooth fade between inner and outer cone edges
                float t = Mathf.InverseLerp(innerConeAngle, outerConeAngle, angle);
                volumeMultiplier = Mathf.Lerp(1f, outsideConeVolumeMultiplier, t);
            }
        }

        // Play sound once with calculated directional volume
        audioSource.PlayOneShot(audioClip, volumeMultiplier);

        // Register playback and apply cooldown
        hasPlayed = true;
        cooldownTimer = playbackCooldown;
    }

    // -----------------------------------------------------------------------
    //                  GIZMOS for Scene View Debug Visualization
    // -----------------------------------------------------------------------
    private void OnValidate()
    {
        // Ensure audio source reference always exists (editor safety)
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
    }

    private void OnDrawGizmosSelected()
    {
        // Prevent null errors if audioSource isn't initialized yet
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
        if (audioSource == null) return; // Still null? Exit.

        // Draw main detection radius
        Gizmos.color = new Color(0.3f, 0.8f, 1f, 0.5f);
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        // Draw outer cone (low volume region)
        Gizmos.color = new Color(1f, 0.5f, 0f, 0.4f);
        drawCone(outerConeAngle);

        // Draw inner cone (max volume zone)
        Gizmos.color = new Color(1f, 1f, 0f, 0.8f);
        drawCone(innerConeAngle);

        // Visual cue for min distance / low-pass zone
        Gizmos.color = new Color(0.5f, 0.3f, 1f, 0.15f);
        Gizmos.DrawWireSphere(transform.position, audioSource.minDistance);
    }

    // Draws a cone visualization from the forward direction based on angle
    private void drawCone(float angle)
    {
        float radius = detectionRadius * 0.8f; // Visual length of cone

        // Compute rotation boundaries for cone edges
        Quaternion leftRot = Quaternion.Euler(0, -angle, 0);
        Quaternion rightRot = Quaternion.Euler(0, angle, 0);

        // Convert to world directions
        Vector3 leftDir = leftRot * transform.forward;
        Vector3 rightDir = rightRot * transform.forward;

        // Draw cone edge lines
        Gizmos.DrawRay(transform.position, leftDir * radius);
        Gizmos.DrawRay(transform.position, rightDir * radius);

        // Draw arc connecting edges for visualization
        int segments = 24; // Smoothness of arc
        Vector3 prevPoint = transform.position + rightDir * radius;
        for (int i = 1; i <= segments; i++)
        {
            float t = Mathf.Lerp(-angle, angle, (float)i / segments);
            Vector3 dir = Quaternion.Euler(0, t, 0) * transform.forward;
            Vector3 nextPoint = transform.position + dir * radius;
            Gizmos.DrawLine(prevPoint, nextPoint);
            prevPoint = nextPoint;
        }
    }
}
