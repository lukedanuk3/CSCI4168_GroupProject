using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SimplePlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;

    Rigidbody rb;
    Vector3 input;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal"); // A/D or left/right
        float v = Input.GetAxisRaw("Vertical");   // W/S or up/down

        input = new Vector3(h, 0f, v).normalized;
    }

    void FixedUpdate()
    {
        if (input.sqrMagnitude > 0.01f)
        {
            Vector3 move = input * moveSpeed * Time.fixedDeltaTime;
            rb.MovePosition(rb.position + move);

            // Face direction of movement
            transform.forward = input;
        }
    }
}
