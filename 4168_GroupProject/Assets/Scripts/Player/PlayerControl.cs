using UnityEngine;
using UnityEngine.InputSystem;

/*
 This script will be a component that handles user input for
the character. Use the Update callbacks for your movement logic. 
*/
public class PlayerControl : MonoBehaviour
{
    // These 4 vectors will be used to help our player/camera when moving

    private Vector3 movementDirection;
    [SerializeField] private AudioSource jumpSound; //The sound that will play when the player jumps
    [SerializeField] private Rigidbody body;
    [SerializeField] private Transform playerCamera; //The camera used to follow the player
    [Space]
    [SerializeField] private float speed; //The player's movement speed
    [SerializeField] private float rotationSpeed; //The player's rotation speed
    [SerializeField] private float sensitivity; //The sensitivity of the camera rotation
    [SerializeField] private float jumpForce; //The force at which the player is lifted off the ground

    private bool isGrounded = true; //This will be used to check if the player is on the ground
    private bool jumpRequested = false; //This will be used to check if a jump has been requested

    private float z; //Will be used to check for vertical input (forward or backward)
    private float x; //Will be used to check for horizontal input (left or right)
    private float mouseX; //Used to check for horizontal mouse input
    private float mouseY; //Used to check for vertical mouse input



    // This is what will be used to listen for the user's input

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Start()
    {
        body = GetComponent<Rigidbody>();
        //This will ensure the player isn't flipping endlessly when jumping or in the air
        body.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;


    }

    // Update is called once per frame
    void Update(){
        //If the user has hit the space button and they're currently on the ground, then 
        //the user wants to & is able to jump


        if(Input.GetKeyDown(KeyCode.Space) && isGrounded){
            jumpRequested = true;
        }


        //Gets input from the user's keyboard
        x = Input.GetAxis("Horizontal");
        z = Input.GetAxis("Vertical");

        //Gets input from the user's mouse
        mouseX = Input.GetAxis("Mouse X");
        mouseY = Input.GetAxis("Mouse Y");



    }

    void FixedUpdate()
    {
        //This code will help us determine our movement in relation to our camera
        Vector3 camForward = playerCamera.forward;
        Vector3 camRight = playerCamera.right;
        camForward.y = 0f;
        camForward.Normalize();
        camRight.y = 0f;
        camRight.Normalize();
        Vector3 forwardRelative = z * camForward;
        Vector3 rightRelative = x * camRight;
        Vector3 cameraRelatedMovement = forwardRelative + rightRelative;
        movementDirection = cameraRelatedMovement.normalized * speed;


        // playerMovementInput = new Vector3(x, 0f, z);
        Debug.DrawRay(transform.position, movementDirection * 2f, Color.green);
        Debug.DrawRay(transform.position, movementDirection * 2f, isGrounded ? Color.green : Color.red);

        MovePlayer(); //This will move the character within the game
        
        //This will only happen if the player is falling downward
        if(body.linearVelocity.y < 0){
            body.AddForce(Vector3.down * 2f, ForceMode.Acceleration);
        }
        body.constraints = RigidbodyConstraints.FreezeRotation;


    }

    private void MovePlayer(){
        

        //The velocity at which our character's rigid body will be moving, using input from the player
        Vector3 horizontalVelocity = new Vector3(movementDirection.x, body.linearVelocity.y, movementDirection.z);        
        body.linearVelocity = horizontalVelocity;

        
        
        
        //If a jump was requested, then the user will be lifted off the ground, using the jumpForce variable to
        //determine how far off it'll go up
        if(jumpRequested && isGrounded){
            isGrounded = false;
            jumpRequested = false;
            jumpSound.Play();
            body.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    
        //This will rotate the character, to match the direction of its movement, not the camera
        if(movementDirection.magnitude > 0.1f){
            Quaternion toRotation = Quaternion.LookRotation(new Vector3(movementDirection.x, 0f, movementDirection.z));
            body.MoveRotation(toRotation);
        }

    //This method will run if our player object collides with another object
   
} 
    private void OnCollisionEnter(Collision collision){
        //This will only run if our player collides with anything given the tag "Ground"
        if(collision.gameObject.tag == ("Ground") || collision.gameObject.tag == ("MovingPlatform")){
            if(!isGrounded){
            }
            isGrounded = true;
        }
    }
}

 