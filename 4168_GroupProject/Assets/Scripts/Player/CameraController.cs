using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{

    [SerializeField] private float sensitivity;
    private Vector2 mouseInput;
    private float pitch;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up, mouseInput.x * sensitivity * Time.deltaTime);
        pitch -= mouseInput.y * sensitivity * Time.deltaTime;
        pitch = Mathf.Clamp(pitch, -90f, 90f);
        transform.localEulerAngles = new Vector3(pitch, transform.localEulerAngles.y, 0f);
    }

    public void OnMouseMove(InputAction.CallbackContext context){
        mouseInput = context.ReadValue<Vector2>();
    }
}
