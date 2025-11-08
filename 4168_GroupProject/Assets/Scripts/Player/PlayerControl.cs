using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerControl : MonoBehaviour
{

    //Animation constants
    const string IDLE = "PLACEHOLDER";
    const string WALK = "PLACEHOLDER";

    //Movement
    public float lookSensitivity = 3f;
    public float speed = 5f;
    private float rotationY = 0f;

    //Physics
    private Rigidbody rigidbody;

    // Animation
    private Animator playerAnimator;
    public bool isAnimated = true;
    private string currentState = "Idle";
    
    void Start()
    {
        playerAnimator = GetComponent<Animator>();
        Cursor.lockState = CursorLockMode.Locked;
        rigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        //Handle player input
        HandleMouseMovement();
        HandlePosition();
    }

    void HandleMouseMovement(){
        float mouseX = Input.GetAxis("Mouse X") * lookSensitivity;
        rotationY += mouseX;
        transform.rotation = Quaternion.Euler(0f, rotationY, 0f);
    }

    //Player movement control
    void HandlePosition(){

        //Movement variables
        float xTranslation = 0f;
        float yTranslation = 0f;
        float zTranslation = 0f;

        //Input handling
        if (Input.GetKey(KeyCode.W)){
            zTranslation += 1f;
            SetAnimationState(WALK);
        }
        if (Input.GetKey(KeyCode.S)){ 
            zTranslation -= 1f;
            SetAnimationState(WALK);
        };
        if (Input.GetKey(KeyCode.D)){
            xTranslation += 1f;
            SetAnimationState(WALK);
        }
        if (Input.GetKey(KeyCode.A)){
            xTranslation -= 1f;
            SetAnimationState(WALK);
        }

        if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.A) || 
        Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.D)){
            SetAnimationState(IDLE);
        }

        //Moving in look direction
        Vector3 move = transform.forward * zTranslation + transform.right * xTranslation;
        move.y = 0f;
        if (move.magnitude > 1f)
            move.Normalize();

        //Compute velocity
        Vector3 moveVelocity = move * speed;
        rigidbody.linearVelocity = new Vector3(moveVelocity.x, rigidbody.linearVelocity.y, moveVelocity.z);
    }
    
    //Update current animation state
    void SetAnimationState(string newState){
        if (currentState == newState) return;

        currentState = newState;
        if (isAnimated)
            playerAnimator.Play(currentState);
    }
}
