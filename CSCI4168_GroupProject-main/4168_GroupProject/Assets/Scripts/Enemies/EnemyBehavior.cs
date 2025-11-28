using UnityEngine;
using UnityEngine.AI;
using System.Collections;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyBehavior : MonoBehaviour
{
    [Range(0,50)] [SerializeField] float sightRange = 20;
    private NavMeshAgent agent;
    private GameObject player;
    [SerializeField] AudioSource audioSource;
    Animator animator;

    [SerializeField] Transform[] points;
    private int currentPoint;
    private float pointReach = 0.5f;
    private float rotationSpeed;

    /*
    When the scene starts, we'll do the following
        - Assign a NavMeshAgent component to our enemy
        - Obtain our player's beginning coordinates
        - Assign the enemy's death sound (guess who it is)
    */
    void Start(){
        animator = GetComponent<Animator>();
        animator.SetBool("isWalking", false);
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindWithTag("Player");
        audioSource = GetComponent<AudioSource>();
        agent.autoBraking = false;
        currentPoint = Random.Range(0, points.Length);
        agent.updateRotation = false;
        rotationSpeed = 2;
        }
    // Update is called once per frame
    void Update()
    {
        // // if(player.transform.position == null || !agent.isActiveAndEnabled){
        //     return;
        // }

        //Consistently updates the enemy's distance from the player
        float distanceFromPlayer = Vector3.Distance(player.transform.position, this.transform.position);

        //Update the enemy's direction based on their direction of movement
        // Vector3 direction = agent.velocity.normalized;
        // Quaternion lookRotation = Quaternion.LookRotation(direction);
        // transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);


        if(gameObject.tag == "CameraMonster"){
            //If the player is within the enemy's sight, and the enemy isn't dead, ChasePlayer() will be invoked
            if(EnemySeesPlayer()){
                ChasePlayer();
            }

            //If the enemy is not within the enemy's sight, then they will invoke Patrol(), to patrol a list of pre-determined points
            else{
                if(points.Length > 0){
                    Patrol();
                }
            }
        }

        if(gameObject.tag == "FollowMonster"){
            FollowPlayer();
        }

        Vector3 direction = agent.desiredVelocity;
        if(direction.sqrMagnitude > 0.01f){
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
        }

    }


    //Method for when enemy is patrolling around the map, not knowing where the enemy currently is
    void Patrol(){

        if(!agent.isActiveAndEnabled){
            return;
        }

        //sets the enemy's animation state to walking, to signify he's moving
        animator.SetBool("isWalking", true);
        // Debug.Log("Point Chosen: " + currentPoint);
        // Debug.Log("Original Destination: " + points[currentPoint].position);
        // Debug.Log("Original local Destination: " + points[currentPoint].position);


        //sets the enemy's destination to be the first point in its list of waypoints
        // agent.SetDestination(points[currentPoint].position);

        //if the agent is within reach of his distance, then the enemy will make its way to the next waypoint
        if(!agent.pathPending && agent.remainingDistance < pointReach){
            Debug.Log("Path Complete! Getting New Path");
            animator.SetBool("isWalking", false);
            GetNewDestination();
            Debug.Log("New Path: " + points[currentPoint].position);
        }
    }

    //This method will get a new destination for our enemy
    void GetNewDestination(){
        Debug.Log("Getting new waypoint");
        //Temporarily stores our current position's value in our list
            int tempNumber = currentPoint;

            //Gets the next random position from our list
            currentPoint = Random.Range(0, points.Length);

            //If our new position is the exact same as our previous position, then we'll endlessly assign a random number to
            //currentPoint, until it's different from our previous value, ensuring a new position is always assigned
            if(currentPoint == tempNumber){
                while(currentPoint == tempNumber){
                    currentPoint = Random.Range(0, points.Length);
                }
            }
            Debug.Log("New point found");
            Debug.Log(points[currentPoint].position);
            agent.SetDestination(points[currentPoint].position);
    }

    //This method will chase the player, so long as the conditions to call it are met
    void ChasePlayer(){
        Debug.Log("Destination: " + agent.destination);
        if(!agent.isActiveAndEnabled){
            return;
        }
        animator.SetBool("isWalking", true);
        agent.SetDestination(player.transform.position);
    }

    //This method will follow the player, unless they're looked at (using PlayerSeesEnemy), in which case they'll stop
    void FollowPlayer(){
        animator.SetBool("isMoving", true);
        if(PlayerSeesEnemy()){
            Debug.Log("Player is in view of enemy");
            agent.isStopped = true;
            animator.SetBool("isMoving", false);
        }
        else{
            animator.SetBool("isMoving", true);
            agent.isStopped = false;
            agent.SetDestination(player.transform.position);
        }
        agent.SetDestination(player.transform.position);
    }


    public bool EnemySeesPlayer(){
        Transform camera = player.GetComponent<PlayerControl>().cameraTransform;
        Vector3 directionToPlayer = (camera.position - transform.position).normalized;
        float angle = Vector3.Angle(transform.forward, directionToPlayer);

        if(angle < 60f){
            RaycastHit hit;
            if(Physics.Raycast(transform.position, directionToPlayer, out hit, 60f)){
                if(hit.transform == player.transform){
                    Debug.Log("Spooky Squid sees player");
                    return true;
                }
            }
        }
        return false;
    }
    //This method will check if the player actively sees the enemy.
    //If they do, the enemy will stop moving completely, imitating a statue
    public bool PlayerSeesEnemy(){
        Transform camera = player.GetComponent<PlayerControl>().cameraTransform;
        Vector3 directionToEnemy = (transform.position - camera.position).normalized;
        float angle = Vector3.Angle(camera.forward, directionToEnemy);

        // Checks if something hits within 30 meters
        if(angle < 60f){
            RaycastHit hit;
            if(Physics.Raycast(camera.position, directionToEnemy, out hit, 30f)){
                //If the player's camera sees the follow monster, it will stop moving altogether
                if(hit.transform == transform){
                    Debug.Log("In view of monster");
                    return true;
                }
            }
        }
        return false;
    }
    //This method will only run if the enemy collides with other objects
    private void OnTriggerEnter(Collider collision){
        //This following block of code will only run if the enemy has run into an object whose tag is "Player"
        //In other words, this will only run if the enemy's come in contact with our enemy
        if(collision.gameObject.tag == "Interactable"){
            Debug.Log("Enemy Interacted with door");
            collision.gameObject.GetComponent<InteractionHandler>().Interact(gameObject);
        }
    }
} 
