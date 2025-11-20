using UnityEngine;

public class tempMove : MonoBehaviour
{
    float speed = 5;

    void Update()
    {
        //Handle player input
        HandlePosition();
    }

    //Player movement control
    void HandlePosition()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(horizontalInput, 0f, verticalInput) * speed * Time.deltaTime;
        transform.Translate(movement);
    }
}
