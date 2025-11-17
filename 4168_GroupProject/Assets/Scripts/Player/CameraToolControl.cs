using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraToolControl : MonoBehaviour
{

    public Camera fpsCamera;

    public GameObject uiPanel;
    bool isOpen;

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
    }

    public void TakePicture(){
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
                    if (item.tag == "Enemy"){
                        //Enemy spotted
                        Debug.Log("Enemy seen!");
                    }else if (item.tag == "Objective"){
                        //Objective spotted
                        Debug.Log("Objective seen!");
                    }
                }
            }
        }
    }
}
