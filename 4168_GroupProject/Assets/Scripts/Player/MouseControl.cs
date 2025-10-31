using UnityEngine;
using UnityEngine.InputSystem;


public class MouseControl : MonoBehaviour
{

    private Vector2 playerMouseInput;
    private Vector3 originalCamPosition;
    private float xRot;
    private float yRot;
    [SerializeField] private Transform playerCamera;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float sensitivity;

    //Gets the rotation variables
    void Start()
    {
        yRot = transform.eulerAngles.y;
        xRot = transform.eulerAngles.x;
    }

    //Updates the rotation, depending on the mouse input made by the player
    void LateUpdate()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        yRot += mouseX * sensitivity;
        xRot += mouseY * sensitivity;
        xRot = Mathf.Clamp(xRot, 0f, 80f);

        Quaternion targetRotation = Quaternion.Euler(xRot, yRot, 0f);
        playerMouseInput = new Vector2(mouseX, mouseY);

        MovePlayerCamera(); //This will move the camera attached to the player within the game
    }


    private void MovePlayerCamera(){ 
        //This method will use mouse input, particularly movement on the mouse to rotate the camera angle
        transform.localRotation = Quaternion.Euler(xRot, 0f, 0f);
        
    }
}
