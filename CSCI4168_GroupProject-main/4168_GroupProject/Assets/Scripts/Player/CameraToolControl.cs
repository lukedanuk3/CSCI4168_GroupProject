using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraToolControl : MonoBehaviour
{
    public Camera fpsCamera;
    public AudioSource openSound;
    public AudioSource closeSound;
    public AudioSource photoTakingSound;

    public GameObject uiPanel;
    public UIManager uiManager;
    public bool isOpen;

    void Update(){
        if (isOpen){
            uiPanel.SetActive(true);
            if (Input.GetKeyUp(KeyCode.Return)){
                TakePicture();
            }
        }else{
            uiPanel.SetActive(false);
        }
    }

    public void OpenClose(){
        isOpen = !isOpen;
        if(isOpen){
            if(closeSound.isPlaying){
                closeSound.Stop();
            }
            openSound.time = 0.5f;
            openSound.Play();
        }
        else{
            if(openSound.isPlaying){
                openSound.Stop();
            }
            closeSound.Play();
        }
    }

    public void TakePicture(){
        if(!photoTakingSound.isPlaying){
            photoTakingSound.Play();
        }
        Plane[] frustumPlanes = GeometryUtility.CalculateFrustumPlanes(fpsCamera);
        List<GameObject> visibleObjects = new List<GameObject>();
        foreach (GameObject item in FindObjectsOfType<GameObject>())
        {
            Renderer renderer = item.GetComponent<Renderer>();
            if (renderer != null)
            {
                if (GeometryUtility.TestPlanesAABB(frustumPlanes, renderer.bounds))
                {
                    //Object is in frame, so check tag
                    if (item.tag == "FollowMonster"){
                        //Enemy spotted
                        Debug.Log("Enemy seen!");
                    }
                    else if (item.tag == "CameraMonster"){

                    }
                    else if (item.tag == "Objective"){
                        //Objective spotted
                        Debug.Log("Objective seen!");
                        uiManager.increaseCounter();
                    }
                }
            }
        }
    }
}