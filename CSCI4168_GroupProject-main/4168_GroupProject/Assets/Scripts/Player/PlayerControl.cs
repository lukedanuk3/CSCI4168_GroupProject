using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerControl : MonoBehaviour
{
    //FPS camera transform
    public Transform cameraTransform;

    //Animation constants
    const string IDLE = "PLACEHOLDER";
    const string WALK = "PLACEHOLDER";

    //Movement
    public float lookSensitivity = 3f;
    public float speed = 5f;
    private float rotationY = 0f;

    //Physics
    private Rigidbody rigidbody;

    //Animation
    private Animator playerAnimator;
    public bool isAnimated = true;
    private string currentState = "Idle";

    //Game world and character
    private GameObject[] interactables;
    public float interactionRange;

    //Inventory
    public List<GameObject> inventory;
    private int currentSlot;
    GameObject currentTool;
    public Transform toolHolder;
    public Vector3 toolRelativePosition;
    public Vector3 toolRelativeRotation;

    //Player's camera tool
    public GameObject camera;
    CameraToolControl cameraControl;
    
    void Start()
    {
        //Load all interactable objects in the level into the Interactables array
        interactables = GameObject.FindGameObjectsWithTag("Interactable");

        cameraControl = camera.GetComponent<CameraToolControl>();

        playerAnimator = GetComponent<Animator>();
        Cursor.lockState = CursorLockMode.Locked;
        rigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        //Handle player input
        HandlePosition();
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

        //Input for interacton with objects
        if (Input.GetKeyUp(KeyCode.E)){
            AttemptToInteract();
        }

        //Opening camera
        if (Input.GetKeyUp(KeyCode.C)){
            cameraControl.OpenClose();
        }

        //Switching tools in inventory
        if (Input.GetKeyUp(KeyCode.Q)){
            //Didn't use a bool here because of the variable naming difficulties that would come with that
            if (currentSlot == 0) currentSlot = 1;
            else currentSlot = 0;
            SelectTool(currentSlot);
        }

        Vector3 camForward = cameraTransform.forward;
        Vector3 camRight = cameraTransform.right;

        camForward.y = 0f;
        camRight.y = 0f;
        camForward.Normalize();
        camRight.Normalize();

        Vector3 move = camForward * zTranslation + camRight * xTranslation;
        if (move.magnitude > 1f)
            move.Normalize();

        Vector3 moveVelocity = move * speed;
        rigidbody.linearVelocity = new Vector3(moveVelocity.x, rigidbody.linearVelocity.y, moveVelocity.z);
    }

    //Try to interact with nearby object
    void AttemptToInteract(){
        //Iterate through interactable objects to find one within range
        foreach(GameObject interactable in interactables){
            float distanceToInteractable = Vector3.Distance(transform.position, interactable.transform.position);

            if (distanceToInteractable <= interactionRange)
            {
                HandleInteraction(interactable);
            }
        }
    }

    //Handle interaction
    void HandleInteraction(GameObject targetObject){
        targetObject.GetComponent<InteractionHandler>().Interact(gameObject);
    }

    //Tool functons
    public void EquipTool(GameObject tool){
        if (inventory.Count < 2) {
            Debug.Log("Equipped tool " + tool.name);
            inventory.Add(tool);
            SelectTool(inventory.Count - 1);
        }else{
            Debug.Log("Inventory full");
        }
    }

    public void SelectTool(int inventoryIndex){
        if (currentTool != null) currentTool.SetActive(false);

        if (inventoryIndex < inventory.Count){
            GameObject selected = inventory[inventoryIndex];
            selected.SetActive(true);

            selected.transform.SetParent(toolHolder, false);

            selected.transform.localPosition = selected.GetComponent<ToolData>().relativePosition;
            selected.transform.localEulerAngles = selected.GetComponent<ToolData>().relativeRotation;

            currentTool = selected;
        }
    }

    //Update current animation state
    void SetAnimationState(string newState){
        if (currentState == newState) return;

        currentState = newState;
        if (isAnimated)
            playerAnimator.Play(currentState);
    }
}