using System.Collections.Generic;
using UnityEngine;

public class CameraToolControl : MonoBehaviour
{
    bool isOpen;

    void Update(){
        if (isOpen){
            if (Input.GetKey(KeyCode.Return)){
                TakePicture();
            }
        }
    }

    public void OpenClose(){
        isOpen = !isOpen;
    }

    public void TakePicture(){
        Plane[] frustumPlanes = GeometryUtility.CalculateFrustumPlanes(Camera.main);
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