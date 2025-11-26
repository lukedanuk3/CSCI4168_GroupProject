using UnityEngine;
using UnityEngine.AI;
using System.Collections;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyBehavior : MonoBehaviour
{
    [Range(0,50)] [SerializeField] float sightRange = 20;
    private NavMeshAgent agent;
    private Transform playerPosition;
    [SerializeField] AudioSource audioSource;
    Animator animator;

    [SerializeField] Transform[] points;
    private int currentPoint = 0;
    private float pointReach = 0.5f;

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
        playerPosition = GameObject.FindWithTag("Player").transform;
        audioSource = GetComponent<AudioSource>();
        agent.autoBraking = false;
        }
    // Update is called once per frame
    void Update()
    {
        if(playerPosition == null || !agent.isActiveAndEnabled){
            return;
        }
        //Consistently updates the enemy's distance from the player
        float distanceFromPlayer = Vector3.Distance(playerPosition.position, this.transform.position);

        //If the player is within the enemy's sight, and the enemy isn't dead, ChasePlayer() will be invoked
        if(distanceFromPlayer <= sightRange){
            ChasePlayer();
        }

        //If the enemy is not within the enemy's sight, then they will invoke Patrol(), to patrol a list of pre-determined points
        else{
            Patrol();
        }
    }


    void Patrol(){

        if(!agent.isActiveAndEnabled){
            return;
        }
        //If the enemy has no points, then nothing will happen in this method
        if(points.Length == 0){
            return;
        }

        //sets the enemy's animation state to walking, to signify he's moving
        animator.SetBool("isWalking", true);

        //sets the enemy's destination to be the first point in its list of waypoints
        agent.SetDestination(points[currentPoint].position);

        //if the agent is within reach of his distance, then the enemy will make its way to the next waypoint
        if(!agent.pathPending && agent.remainingDistance < pointReach){

            //adds 1 to currentPoint and finds the remainder from points.Length to ensure it never looks for a point that doesn't exist
            //and thus goes in an endless cycle
            currentPoint = (currentPoint + 1) % points.Length;
            agent.SetDestination(points[currentPoint].position);
        }
    }
    //This method will chase the player, so long as the conditions to call it are met
    void ChasePlayer(){
        if(!agent.isActiveAndEnabled){
            return;
        }
        animator.SetBool("isWalking", true);
        agent.SetDestination(playerPosition.position);
    }
    //This method will only run if the enemy collides with other objects
    private void OnCollisionEnter(Collision collision){

        //This following block of code will only run if the enemy has run into an object whose tag is "Player"
        //In other words, this will only run if the enemy's come in contact with our enemy
        if(collision.gameObject.tag == ("Player")){

            //Get collider object for the player, and our enemy
            Collider player = collision.collider;
            Collider enemyAgent = GetComponent<Collider>();



        }

        if(collision.gameObject.tag == "Interactable"){

        }
    }
} 
