using UnityEngine;

public class RandomObjectivePicker : MonoBehaviour
{
    private GameObject[] randomObjectives;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        randomObjectives = GameObject.FindGameObjectsWithTag("Objective");
        foreach (GameObject objective in randomObjectives){
            objective.SetActive(false);
        }

        for(int i = 0; i < 5; i++){
            int randomNumber = Random.Range(0,randomObjectives.Length);
            //Checks if the randomly selected object is already active
            if(randomObjectives[randomNumber].activeInHierarchy){
                //If so, it'll run a loop until it finds one that isn't active
                while(randomObjectives[randomNumber].activeInHierarchy){
                    randomNumber = Random.Range(0,randomObjectives.Length);
                    if(!randomObjectives[randomNumber].activeInHierarchy){
                        randomObjectives[randomNumber].SetActive(true);
                        break;
                    }
                }
            }
            //If not, then it'll activate that object
            else{
                randomObjectives[randomNumber].SetActive(true);
            }
        }
    }
}
