using UnityEngine;
using System.Collections.Generic;

public class PlayerLightCuller : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform playerCamera; // The camera direction reference

    [Header("Radius Settings")]
    [SerializeField] private float backRadius = 8f;      // Circular radius around the player
    [SerializeField] private float frontConeRadius = 15f; // Distance for front cone
    [SerializeField, Range(1f, 180f)] private float frontConeAngle = 60f; // Width of cone (in degrees)

    [Header("Performance")]
    [SerializeField] private float updateRate = 0.25f; // How often to update lights (seconds)
    
    private Light[] allLights;
    private float timer;

    void Start()
    {
        // Grab all lights tagged "LightSource" once at start
        GameObject[] lightObjs = GameObject.FindGameObjectsWithTag("LightSource");
        List<Light> foundLights = new List<Light>();

        foreach (var obj in lightObjs)
        {
            Light l = obj.GetComponent<Light>();
            if (l != null)
                foundLights.Add(l);
        }

        allLights = foundLights.ToArray();
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= updateRate)
        {
            timer = 0f;
            UpdateLights();
        }
    }

    private void UpdateLights()
    {
        Vector3 playerPos = transform.position;
        Vector3 forward = playerCamera.forward;

        foreach (Light light in allLights)
        {
            if (light == null) continue;

            Vector3 toLight = light.transform.position - playerPos;
            float dist = toLight.magnitude;

            bool inBackRadius = dist <= backRadius;
            bool inFrontCone = false;

            if (dist <= frontConeRadius)
            {
                float angle = Vector3.Angle(forward, toLight);
                if (angle <= frontConeAngle * 0.5f)
                    inFrontCone = true;
            }

            bool shouldBeOn = inBackRadius || inFrontCone;

            // Only toggle if necessary (saves performance)
            if (light.enabled != shouldBeOn)
                light.enabled = shouldBeOn;
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Visualize the radii and cone in Scene view
        Gizmos.color = new Color(1f, 1f, 0f, 0.2f);
        Gizmos.DrawWireSphere(transform.position, backRadius);

        if (playerCamera != null)
        {
            Vector3 pos = transform.position;
            Vector3 forward = playerCamera.forward;
            Quaternion leftRay = Quaternion.AngleAxis(-frontConeAngle * 0.5f, Vector3.up);
            Quaternion rightRay = Quaternion.AngleAxis(frontConeAngle * 0.5f, Vector3.up);
            Vector3 leftDir = leftRay * forward;
            Vector3 rightDir = rightRay * forward;

            Gizmos.color = new Color(0f, 1f, 0f, 0.25f);
            Gizmos.DrawLine(pos, pos + leftDir * frontConeRadius);
            Gizmos.DrawLine(pos, pos + rightDir * frontConeRadius);
            Gizmos.DrawWireSphere(pos, frontConeRadius);
        }
    }
}
