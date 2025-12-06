using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Unity.AI.Navigation;

public class PlayerControl : MonoBehaviour
{

    //Used when checking interaction range
    InteractionHandler interactionHandler;

    //Used to display UI for specific interactions
    public GameObject doorOpenInstructions;
    public GameObject toolPickUpInstructions;
    public GameObject trapBreakInstructions;
    public GameObject levelSelectInstructions;
    public GameObject toolSelectInstructions;
    public GameObject needToChooseLevel;
    public GameObject nextLevelInstructions;
    [Space]
    public GameObject toolSelectUI;
    public GameObject levelSelectUI;
    [Space]

    //Used to close select UIs
    private bool toolSelectIsActive = false;
    private bool levelSelectIsActive = false;

    //Used to store the user's selected level
    public string levelName;
    public bool levelIsSelected = false;

    //Audio for player
    public AudioSource walkSound;
    
    //FPS camera transform
    public Transform cameraTransform;
    [Space]

    //NavMesh surface
    public NavMeshSurface navMeshSurface;

    //Used to turn off doors for NavMesh baking
    private GameObject[] doors;
    private GameObject[] exits;
    [Space]

    //Animation constants
    const string IDLE = "IDLE";
    const string WALK = "WALK";

    //Movement
    public float lookSensitivity = 3f;
    public float speed = 5f;
    private float rotationY = 0f;
    [Space]

    //Physics
    private Rigidbody rigidbody;

    //Animation
    private Animator playerAnimator;
    public bool isAnimated = true;
    private string currentState = "Idle";

    //Game world and character
    private GameObject[] interactables;
    public float interactionRange;
    private GameObject[] cameraEnemies;

    //Inventory
    public List<GameObject> inventory;
    private int currentSlot;
    GameObject currentTool;
    [Space]
    public AudioSource boardBreakSound;
    public AudioSource wireCutSound;
    public AudioSource fenceBreakSound;
    [Space]
    public Transform toolHolder;
    public Vector3 toolRelativePosition;
    public Vector3 toolRelativeRotation;

    //Tool Inventory
    private string[] items = new string[2];

    //Player's health
    public int health = 3;
    public float healTime = 10;
    private float timer = 0;
    public AudioClip damageTakingSound;

    //UI Manager
    public UIManager uiManager;
    //Tool use
    private int toolCoolDown = 100;
    private bool toolInUse = false;

    //Player's camera tool
    public GameObject camera;
    CameraToolControl cameraControl;
    public GameObject enemyInCamera;
    public GameObject objectiveInCamera;
    public GameObject objectiveAlreadyTaken;

    //Used to keep track on if goal's been achieved
    private bool goalReached = false;

    //Bunch of tool prefabs out there in space
    public GameObject levelTools;

    public bool shouldLoadInventory;
    public bool inventoryLoading;
    
    void Awake(){
        Debug.Log("Inventory on Awake: " + PlayerPrefs.GetString("Inventory"));
    }

    void Start()
    {
        inventory = new List<GameObject>() { null, null };

        //Load all interactable objects in the level into the Interactables array
        interactables = GameObject.FindGameObjectsWithTag("Interactable");
        cameraEnemies = GameObject.FindGameObjectsWithTag("CameraMonster");
        foreach (GameObject cameraEnemy in cameraEnemies){
            foreach(Renderer render in cameraEnemy.GetComponentsInChildren<Renderer>()){
                if(render != null){
                    render.enabled = false;
                }
            }
        }

        doors = GameObject.FindGameObjectsWithTag("Door");
        exits = GameObject.FindGameObjectsWithTag("Exit");
        cameraControl = camera.GetComponent<CameraToolControl>();

        playerAnimator = GetComponent<Animator>();
        Cursor.lockState = CursorLockMode.Locked;
        rigidbody = GetComponent<Rigidbody>();
        uiManager.setGameplayUIActive();

        if (shouldLoadInventory) LoadGlobalInventory();
    }

    void Update()
    {
        //Sprint speed multiplier
        if (Input.GetKeyDown(KeyCode.LeftShift)) speed *= 1.5f;
        if (Input.GetKeyUp(KeyCode.LeftShift)) speed /= 1.5f;

        //Handle player input
        HandlePosition();
        HandleOtherInput();
        if(currentState == WALK){
            if(!walkSound.isPlaying){
                walkSound.Play();
            }
            if(Input.GetKey(KeyCode.LeftShift)){
                Debug.Log("player is running");
                walkSound.pitch = 2.0f;
            }
            else{
                walkSound.pitch = 1.0f;
            }
        }
        else{
            walkSound.Stop();
        }

        CheckInteraction();
        
        //Check Interaction Range
        // CheckInteraction();

        if (toolInUse){
            toolCoolDown--;
            if (toolCoolDown == 0) {
                SelectTool(currentSlot);
                toolInUse = false;
                toolCoolDown = 100;
            }
        }

        if(cameraControl.uiPanel.activeInHierarchy){
            foreach (GameObject cameraEnemy in cameraEnemies){
                foreach(Renderer render in cameraEnemy.GetComponentsInChildren<Renderer>()){
                    if(render != null){
                        render.enabled = true;
                    }
                }
            }
        }
        else{
            foreach (GameObject cameraEnemy in cameraEnemies){
                foreach(Renderer render in cameraEnemy.GetComponentsInChildren<Renderer>()){
                    if(render != null){
                        render.enabled = false;
                    }
                }
            }   
        }

        if (health < 3){
            Debug.Log(health);
            timer = timer + Time.deltaTime;
            if (timer > healTime){
                heal();
                //Debug.Log("Healed! Current health: " + health);
                timer = 0;
            }
        }

        CheckGoalCounter();
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

    void HandleOtherInput(){
        //Input for interacton with objects
        if (Input.GetKeyUp(KeyCode.E)){
            AttemptToInteract();
        }

        //Opening camera
        if (Input.GetKeyUp(KeyCode.C)){
            cameraControl.OpenClose();
            Debug.Log(cameraControl.isOpen);
            if(cameraControl.isOpen){
                uiManager.setGameplayUIInactive();
            }
            else{
                Debug.Log("Camera is closed");
                uiManager.setGameplayUIActive();
            }
        }

        //Switching tools in inventory
        if (Input.GetKeyUp(KeyCode.Q)){
            ChangeCurrentSlot();

            Debug.Log("You are on tool: " + currentSlot);
        }

        //Get click
        if (Input.GetMouseButtonDown(0)){
            if (currentTool != null){
                UseCurrentTool();
            }
        }

        if (Input.GetKeyUp(KeyCode.Return)){
            if(SceneManager.GetActiveScene().name == "HUB"){
                Debug.Log("Right scene");
            if(RadioVoiceOver.instance != null && RadioVoiceOver.instance.skipInstructions.activeInHierarchy){
                Debug.Log("Stopping radio");
                RadioVoiceOver.instance.StopRadioVoiceOver();
            }
            }
        }
    }

    void UseCurrentTool(){
        string toolType = currentTool.GetComponent<ToolData>().toolType;
        if (toolType == "CROWBAR"){
            if (!toolInUse) UseCrowbar();
        }else if (toolType == "BOLTCUTTERS"){
            if (!toolInUse) UseBoltCutters();
        }else if (toolType == "WIRECUTTERS"){
            if (!toolInUse) UseWireCutters();
        }else if (toolType == "FLASHLIGHT"){
            currentTool.GetComponent<FlashlightBehaviour>().Toggle();
        }
    }

    //Check if player is in range of an interactable
    void CheckInteraction(){
        RaycastHit hit;
        Ray ray = new Ray(cameraTransform.position, cameraTransform.forward);
        if(Physics.Raycast(ray, out hit, interactionRange)){
            if(hit.collider.tag == "Exit"){
                if(hit.collider.GetComponentInParent<ExitHandler>().isDoorLocked())
                {
                    Debug.Log("Door is locked");
                }
                else
                {
                    Debug.Log("Door is unlocked");
                }
            }
            if(hit.collider.tag == "Door"){
                doorOpenInstructions.SetActive(true);
            }
            else if (hit.collider.tag == "Tool"){
                toolPickUpInstructions.SetActive(true);
            }
            else if(hit.collider.tag == "LevelSelector"){
                if(!levelSelectIsActive){
                levelSelectInstructions.SetActive(true);
                }
                else{
                levelSelectInstructions.SetActive(false);
                }
            }
            else if(hit.collider.tag == "ToolSelector"){
                if(!toolSelectIsActive){
                    Debug.Log("Tool select is not active");
                    toolSelectInstructions.SetActive(true);
                }
                else{
                    toolSelectInstructions.SetActive(false);
                }
            }
            else if(hit.collider.tag == "NextLevel"){
                if(levelIsSelected){
                    nextLevelInstructions.SetActive(true);
                }
                else{
                    needToChooseLevel.SetActive(true);
                }
            }
            }
            else{
                doorOpenInstructions.SetActive(false);
                toolPickUpInstructions.SetActive(false);
                toolSelectInstructions.SetActive(false);
                levelSelectInstructions.SetActive(false);
                needToChooseLevel.SetActive(false);
                nextLevelInstructions.SetActive(false);
            }
            if(cameraControl.uiPanel.activeInHierarchy){
                CheckCameraRange(ray, hit);
            }
    }
    //Show Interaction Text
    
    
    //Try to interact with nearby object
    void AttemptToInteract(){
        //Iterate through interactable objects to find one within range
        foreach(GameObject interactable in interactables){
            float distanceToInteractable = Vector3.Distance(transform.position, interactable.transform.position);

            if (distanceToInteractable <= interactionRange && !inventory.Contains(interactable))
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

    public void PickUpTool(GameObject tool){
        inventory[currentSlot] = tool;
        SelectTool(currentSlot);
        UpdateGlobalInventory();

    }

    public void SelectTool(int inventoryIndex)
    {
        if (currentTool != null)
            currentTool.SetActive(false);

        GameObject selected = inventory[inventoryIndex];

        if (selected == null)
        {
            currentTool = null;
            Debug.Log("Selected slot is empty");
            return;
        }
        if(inventoryIndex == 0)
        {
            uiManager.tool1Active();
        }
        else{
            uiManager.tool2Active();
        }
        Debug.Log("Selected Tool: " + selected.name);

        selected.SetActive(true);
        selected.transform.SetParent(toolHolder, false);

        selected.transform.localPosition = selected.GetComponent<ToolData>().relativePosition;
        selected.transform.localEulerAngles = selected.GetComponent<ToolData>().relativeRotation;

        currentTool = selected;
    }


    //Tool use...

    //Crowbar
    void UseCrowbar(){
        List<GameObject> boards = ObjectsInViewOfType("Board");
        if (boards != null){
            foreach (GameObject board in boards){
                float distanceToBoard = Vector3.Distance(transform.position, board.transform.position);
                if (distanceToBoard <= interactionRange){
                    boardBreakSound.Play();
                    board.GetComponent<BoardBehaviour>().Break();
                    
                }
            }
        }

        float usageX = 90;
        float usageY = 90;
        float usageZ = 90;

        currentTool.transform.localEulerAngles = currentTool.GetComponent<ToolData>().relativeRotation + new Vector3(usageX, usageY, usageZ);
        toolInUse = true;
    }


    //Bolt cutters
    void UseBoltCutters(){
        List<GameObject> steels = ObjectsInViewOfType("Steel");
        Debug.Log("Detected steels: " + (steels == null ? "null" : steels.Count.ToString()));
        
        if (steels != null){
            foreach (GameObject steel in steels){
                float distanceToSteel = Vector3.Distance(transform.position, steel.transform.position);
                if (distanceToSteel <= interactionRange){
                    fenceBreakSound.Play();
                    steel.GetComponent<SteelBehaviour>().Break();
                    
                }
            }
        }

        float usageX = 90;
        float usageY = 90;
        float usageZ = 90;

        currentTool.transform.localEulerAngles = currentTool.GetComponent<ToolData>().relativeRotation + new Vector3(usageX, usageY, usageZ);
        toolInUse = true;
    }

    //Wire Cutters
    void UseWireCutters(){
        List<GameObject> wires = ObjectsInViewOfType("Wire");
        if (wires != null){
            foreach (GameObject wire in wires){
                float distanceToWire = Vector3.Distance(transform.position, wire.transform.position);
                if (distanceToWire <= interactionRange){
                    wireCutSound.Play();
                    wire.GetComponent<WireBehaviour>().Snip();
                    
                }
            }
        }

        float usageX = 90;
        float usageY = 90;
        float usageZ = 90;

        currentTool.transform.localEulerAngles = currentTool.GetComponent<ToolData>().relativeRotation + new Vector3(usageX, usageY, usageZ);
        toolInUse = true;
    }

    //Return GameObjects in frame matching type
    List<GameObject> ObjectsInViewOfType(string objectType){
        Plane[] frustumPlanes = GeometryUtility.CalculateFrustumPlanes(cameraControl.fpsCamera);
        List<GameObject> visibleObjects = new List<GameObject>();
        foreach (GameObject item in FindObjectsOfType<GameObject>())
        {
            Renderer renderer = item.GetComponent<Renderer>();
            if (renderer != null)
            {
                if (GeometryUtility.TestPlanesAABB(frustumPlanes, renderer.bounds))
                {
                    if (item.tag == objectType) visibleObjects.Add(item);
                }
            }
        }
        return visibleObjects;
    }

    //Update current animation state
    void SetAnimationState(string newState){
        if (currentState == newState) return;

        currentState = newState;
        if (isAnimated)
            playerAnimator.Play(currentState);
    }

    //Remove health from the player
    public void takeDamage()
    {
        health--;
        if(health < 0){
            uiManager.activateGameOver();
        }
        //damageTakingSound.Play();
        uiManager.UpdateHealth(health);
    }

    //Add health to the player 
    public void heal()
    {
        health++;
        uiManager.UpdateHealth(health);
    }

    //Display the health the player has
    public int returnHealth()
    {
        return health;
    }

    //Update the tools the player has equipped
    public bool updateItems(string s, int index)
    {
        if (index < 0 || index > 1)
        {
            return false;
        }
        items[index] = s;
        return true;
    }

    //Return the tools the player currently has equipped
    public string[] getItems()
    {
        return items;
    }    

    //Updates the player's chosen level
    public void updateLevelChoice(string level){
        levelName = level;
        levelIsSelected = true;
        finishSelectingLevel();
    }
    
    //Removes the level select UI
    public void selectLevel(){
        Cursor.lockState = CursorLockMode.None;
        levelSelectIsActive = true;
        levelSelectUI.SetActive(true);
    }

    //Removes the flag on the level select UI, making it visible again to the player
    public void finishSelectingLevel(){
        levelSelectIsActive = false;
        Cursor.lockState = CursorLockMode.Locked;
        levelSelectUI.SetActive(false);
    }

    //Removes the flag on the tool select UI, making it visible again to the player
    public void finishSelectingTools(){
        Cursor.lockState = CursorLockMode.Locked;
        toolSelectIsActive = false;
        toolSelectUI.SetActive(false);
    }

    //Removes the tool select UI
    public void selectTools(){
        Cursor.lockState = CursorLockMode.None;
        toolSelectIsActive = true;
        Debug.Log("Tool UI is active");
        Debug.Log(toolSelectUI);
        toolSelectUI.SetActive(true);
    }

    //Loads the chosen level
    public void LoadChosenLevel(){
        Debug.Log("Inventory on leaving Hub: " + PlayerPrefs.GetString("Inventory"));
        if(levelIsSelected){
            SceneManager.LoadScene(levelName);
        }
    }

    // //Rebuilds the navmesh surface after a trap is broken
    // private void RebuildNavMeshSurface(){
    //     foreach (GameObject door in doors){
    //         door.SetActive(false);
    //     }
    //     navMeshSurface.BuildNavMesh();
    //     Debug.Log("Navmesh rebuilt");
    //     foreach (GameObject door in doors){
    //         door.SetActive(true);
    //     }
    // }

    //Checks what's in the camera's view
    private void CheckCameraRange(Ray ray, RaycastHit hit){
        if(cameraControl.isOpen){
        if(Physics.Raycast(ray, out hit, 30f)){
            if(hit.collider.tag == "CameraMonster" || hit.collider.tag == "FollowMonster"){
                Debug.Log("Monster in camera");
                enemyInCamera.SetActive(true);
            }
            else if(hit.collider.tag == "Objective"){
                if(hit.collider.GetComponent<ItemPhotoHandler>().CheckIfPhotoAlreadyTaken()){
                    objectiveAlreadyTaken.SetActive(true);
                    objectiveInCamera.SetActive(false);
                }
                else{
                    objectiveInCamera.SetActive(true);
                }
            }
            else{
                enemyInCamera.SetActive(false);
                objectiveInCamera.SetActive(false);
            }
        }
        else{
                enemyInCamera.SetActive(false);
                objectiveInCamera.SetActive(false);
        }
        }
        else{
            objectiveAlreadyTaken.SetActive(false);
            enemyInCamera.SetActive(false);
            objectiveInCamera.SetActive(false);
        }
    }

    //Unlocks one of three doors in the event of goal counter being reached
    private void CheckGoalCounter(){
        int currentCounter = int.Parse(uiManager.lifeText.text);
        if(currentCounter >= 5 && goalReached == false){
            goalReached = true;
            int index = Random.Range(0, exits.Length);
            GameObject exit = exits[index];
            exit.GetComponentInParent<ExitHandler>().UnlockDoor();
            Debug.Log(exit);
        }
    }

    //Update the player's saved inventory
    public void UpdateGlobalInventory(){
        string firstSlotName = "Empty";
        Debug.Log("INVENTORY: " +inventory[0].name);
        if (inventory[0] != null) firstSlotName = inventory[0].name;
        string secondSlotName = "Empty";
        if (inventory[1] != null) secondSlotName = inventory[1].name;
        PlayerPrefs.SetString("Inventory", firstSlotName + "/" + secondSlotName);


        Debug.Log("Inventory Here " + firstSlotName + "/" + secondSlotName);
    }

    public void LoadGlobalInventory(){
        inventoryLoading = true;
        string textInventory = PlayerPrefs.GetString("Inventory");
        string[] splitInventory = textInventory.Split("/");
        Dictionary<string, GameObject> levelToolObjects = levelTools.GetComponent<ToolList>().toolDictionary;

        if (splitInventory[0] != "Empty"){
            GameObject first = Instantiate(levelToolObjects[splitInventory[0]]);
            first.name = first.name.Replace("(Clone)", "");
            PickUpTool(first);
            ChangeCurrentSlot();
        }
        if (splitInventory[1] != "Empty"){
            GameObject second = Instantiate(levelToolObjects[splitInventory[1]]);
            second.name = second.name.Replace("(Clone)", "");
            PickUpTool(second);
            ChangeCurrentSlot();
        } 

        inventoryLoading = false;

        Debug.Log("Inventory on Inventory Load: " + PlayerPrefs.GetString("Inventory"));
    }

    //PUBLIC SCOPE NECESSARY FOR UI TO WORK (or at least I think so)
    public void ChangeCurrentSlot(){
        if (currentSlot == 0) currentSlot = 1;
        else currentSlot = 0;
        SelectTool(currentSlot);
    }
}